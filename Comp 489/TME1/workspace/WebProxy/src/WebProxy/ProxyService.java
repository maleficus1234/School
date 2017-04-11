/*
 * Comp 489 TME1
 * Jason Bell
 * 3078931
 * February 24, 2016
 */

package WebProxy;

import java.net.*;
import java.io.*;
import java.util.*;
import java.util.concurrent.locks.*;

// Proxy service that listens on a port and forwards HTTP and FTP requests, and
// returns the response.
public class ProxyService 
{
	// Socket that listens for incoming connections.
	ServerSocket serverSocket;
	// If true, the running listen thread will keep running;
	boolean keepRunning = true;
	// Listens for incoming connections.
	Thread listenThread;
	// Processes requests.
	Thread workThread;
	// Storage to pass a socket from the listen thread to the work thread.
	Queue<Socket> pendingSockets = new ArrayDeque<Socket>();
	// For coordination between the listen and work threads.
	Lock lock = new ReentrantLock();
	// Used by the listen thread to notify the work thread that a new socket is in the queue.
	Condition arePending = lock.newCondition();
	
	// Create a new proxy service that listens on the given port.
	// Have the user of the service handle exceptions, since exceptions here indicate that something
	// is wrong from the start.
	public ProxyService(int port)
			throws IOException,
			IllegalArgumentException
	{
		this.serverSocket = new ServerSocket(port);
	}
	
	// Starts two threads that listen for and handle incoming requests.
	public void listen()
	{
		// Create the listen thread with an anonymous method.
		listenThread = new Thread()
		{
		    public void run() 
		    {
		    	// Keep running as long as this condition is true.
				while(keepRunning)
				{
					try
					{
						// Wait for an incoming socket connection (blocks)
						Socket client = serverSocket.accept();
						
						// Load the socket into the queue, and notify the work thread.
						lock.lock();
						pendingSockets.add(client);
						arePending.signal();
						lock.unlock();
						// Go back to listening!
					}
					catch(Exception e)
					{
						e.printStackTrace();
					}
				}
		    }
		};
		listenThread.start(); // Execute the thread.
		
		// Create the worker thread.
		workThread = new Thread()
		{
		    public void run() 
		    {
		    	// Keep running as long as this condition is true.
				while(keepRunning)
				{
					BufferedWriter writer = null;
					Socket client = null;
					// Store the address to which to route the request here.
					String address = "";
					try
					{
						// Check if there's a pending socket in the queue.
						lock.lock();
						if(pendingSockets.size() == 0) arePending.await();
						client = pendingSockets.poll();
						lock.unlock();
						// Create a reader to read the incoming FTP or HTTP request.
						BufferedReader reader = new BufferedReader(new InputStreamReader(client.getInputStream()));
						
						// Store the entirety of the message here, to be sent to the target address.
						String message = "";
						// Stores the last line read from the stream.
						String inputLine = "";
						// Read from the stream line by line.
						while ((inputLine = reader.readLine()) != null)
						{
							// Have we reached the end of the stream?
							if(inputLine.length() == 0) break;
							// If the line starts with GET, the second word of this line will be the target address.
							if(inputLine.startsWith("GET"))
							{
								String [] split = inputLine.split(" ");
								address = split[1];
							}
							// Add to the total message, making sure to add newlines.
							message += inputLine + "\r\n";
						}
						
						// Output the message, just to see it in action.
						System.out.println(message);
						// Create a writer so that we can buffer and flush a response.
						writer = new BufferedWriter(new OutputStreamWriter(client.getOutputStream()));
						
						// If we didn't find an address, we don't know what to do with this request.
						if(address.equals(""))
						{
							writer.write("HTTP/1.1 400 Bad Request\r\n");
							writer.write("Server: COMP489TME1 (Win32)\r\n");
							writer.write("Connection: Closed\r\n");
							writer.write("\r\n");
						}
						else
						{
							// Open a connection to the address that we found. Use the base class,
							// URLConnection so that we can handle both HTTP And FTP requests without doing
							// anything special for each.
							URL url = new URL(address);
							URLConnection con = url.openConnection();
	
							con.setConnectTimeout(5000);
					        con.setReadTimeout(5000);
							System.out.println("response: " + ((HttpURLConnection)con).getResponseCode());
							
							// A stream in which we can store the incoming bytes in response to the request.
							// We need to process them as bytes rather than text, so that non-text content is
							// transferred properly. In other words, any kind of special text encoding would be
							// bad if, for example, we're transferring image data.
							ByteArrayOutputStream byteStream = new ByteArrayOutputStream();
	
							// Fixed size buffer to hold a batch of incoming bytes.
							byte[] buffer = new byte[4096];
							InputStream incoming = con.getInputStream();
							// Keep reading bytes from the incoming stream into the byte stream, until no more arrive.
							while(true) {
								int n = incoming.read(buffer);
								if( n < 0 ) break;
								byteStream.write(buffer,0,n);
							}
							// Forward the received byte data back to the client.
							client.getOutputStream().write(byteStream.toByteArray());
							incoming.close();
	
						}
						// Flush any data still held by the writer and close the connection.
						writer.flush();
						client.close();
					}
					catch(UnknownHostException e)
					{
						write404(client, writer, address);
					}
					catch(ConnectException e)
					{
						write404(client, writer, address);
					}
					catch(FileNotFoundException e)
					{
						write404(client, writer, address);
					}
					catch(Exception e)
					{
						e.printStackTrace();
					}
				}
		    }
		};
		workThread.start();
	}
		
	// Close the proxy service.
	public void close()
	{
		try
		{
			if(this.serverSocket != null) 
				this.serverSocket.close();
		}
		catch(Exception e)
		{

		}
	}
	
	// Write a 404 error to the stream, and close the connection.
	void write404(Socket client, BufferedWriter writer, String address)
	{
		try
		{
			writer.write("HTTP/1.1 404 Not Found\r\n");
			writer.write("Server: COMP489TME1 (Win32)\r\n");
			writer.write("Connection: Closed\r\n");
			writer.write("\r\n");
			writer.write(get404Message(address));
			writer.flush();
			client.close();
		}
		catch(IOException e)
		{
			e.printStackTrace();
		}
	}
	
	// Returns a 404 error html error message for the given content name.
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
