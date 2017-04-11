/*
 * COMP 489 TME 3
 * Jason Bell 3078931
 * February 28, 2016
 */

package P2PClient;

import java.net.*;
import java.nio.file.Files;
import java.util.*;
import java.util.concurrent.locks.*;
import java.io.*;
import java.nio.file.*;

// Handles any requests for files and dispatches the file to the peer, if available.
public class Dispatcher 
{
	// The username for this peer
	String username;
	// The port that this peer is listening on
	int port;
	
	// The thread that listens for incoming connections.
	Thread listenThread;
	// The thread that dispatches files to other peers.
	Thread dispatchThread;
	// If true, the threads keep running.
	boolean keepRunning = true;
	
	// The listen socket.
	ServerSocket serverSocket;
	
	// Queue up any pending requests (sockets) here.
	Queue<Socket> pendingRequests = new ArrayDeque<Socket>();
	
	// Lock and condition to coordinate the listen and dispatch thread: listen thread
	// uses the condition to notify the dispatch thread that there are new pending requests.
	Lock lock = new ReentrantLock();
	Condition arePending = lock.newCondition();
	
	// Create a new dispatcher that listens on the given port.
	public Dispatcher(int port)
	{
		this.port = port;
	}
	
	// Run the threads.
	public void run()
	{
		try
		{
			// Create and start the dispatch thread: sends requested files to peers.
			dispatchThread = new Thread()
			{
				public void run()
				{
					try
					{
						while(keepRunning)
						{
							// Get a socket for a pending request, or wait if there are none.
							Socket socket = null;
							lock.lock();
							if(pendingRequests.size() == 0) arePending.await();
							if(keepRunning == false) return;
							socket = pendingRequests.poll();
							lock.unlock();
							
							// Create a reader so we can read the HTTP GET message (Since that what I chose to use to send files)
							BufferedReader reader = new BufferedReader(new InputStreamReader(socket.getInputStream()));
							
							// Store the filename to be retrieved here.
							String filename = "";
							// Stores the last line read from the stream.
							String inputLine = "";
							// Read from the stream line by line.
							while ((inputLine = reader.readLine()) != null)
							{
								// Have we reached the end of the stream?
								if(inputLine.length() == 0) break;
								// If the line starts with GET, the second word of this line will be the requested filename.
								if(inputLine.startsWith("GET"))
								{
									String [] split = inputLine.split(" ");
									filename = split[1];
								}
							}
							
							// Create a writer so we can output our response HTTP message.
							BufferedWriter writer = new BufferedWriter(new OutputStreamWriter(socket.getOutputStream()));
							if(filename.equals(""))
							{
								// We did not recognize a GET request, so output a 501 error.
								writer.write("HTTP/1.1 501 Not Implemented\r\n");
								writer.write("Server: COMP489TME1 (Win32)\r\n");
								writer.write("Connection: Closed\r\n");
								writer.write("\r\n");
								writer.write(get404Message(filename));
							}
							else
							{
								// Get a reference to the file to the file
								Path file = Paths.get("Files\\" + filename);
								if(Files.exists(file))
								{
									// THe file exists, so send a 200 message followed by the binary data of the file.
									writer.write("HTTP/1.1 200 Continue\r\n");
									writer.write("\r\n");
									writer.flush();
									socket.getOutputStream().write(Files.readAllBytes(file));
								}
								else
								{
									// File not found, so send a 404 error.
									writer.write("HTTP/1.1 404 Not Found\r\n");
									writer.write("Server: COMP489TME1 (Win32)\r\n");
									writer.write("Connection: Closed\r\n");
									writer.write("\r\n");
									writer.write(get404Message(filename));
								}
							}
							writer.flush();
							
							socket.close();
						}
					}
					catch(Exception e)
					{
						e.printStackTrace();
					}
				}
			};
			dispatchThread.start();
			
			// Start up the thread that listens for incoming connections.
			listenThread = new Thread()
			{
			    public void run() 
			    {
			    	try
			    	{
			    		// Create the listen socket.
			    		serverSocket = new ServerSocket(port);
						while(keepRunning)
						{
							// Get an incoming socket connection: blocks until one is received.
							Socket socket = serverSocket.accept();
							// Add the socket to the pendingRequests queue and notify the dispatch thread.
							lock.lock();
							pendingRequests.add(socket);
							arePending.signalAll();
							lock.unlock();
						}
						serverSocket.close();
			    	}
			    	catch(Exception e)
			    	{
			    		if(keepRunning == false) return;
			    		e.printStackTrace();
			    	}
				}

			};
			listenThread.start();
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
	}
	
	// Shut down the dispatcher
	public void close()
	{
		keepRunning = false;
		try
		{
			serverSocket.close();
			lock.lock();
			arePending.signalAll();
			lock.unlock();
		}
		catch(Exception e) {}
	}
	
	// Outputs a 404 message for the content name, in html.
	String get404Message(String content)
	{
		return "<html>"
				+ "<head>"
				+ "<title>404 Not Found</title>"
				+ "</head>"
				+ "<body>"
				+ "<h1>Not Found</h1>"
				+ "<p>The requested URL " + content + " was not found on this server.</p>"
				+ "</body>"
				+ "</html>";
	}
}
