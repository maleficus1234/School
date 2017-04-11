/*
 * COMP 489 TME 3
 * Jason Bell 3078931
 * February 28, 2016
 */

package P2PClient;

import java.net.*;
import java.nio.file.Paths;
import java.io.*;
import java.util.*;
import java.nio.file.*;

import P2PWebService.*;

// The program container
public class Main 
{
	// The CORBA servant through which we communicate with the server.
	static ServiceProxy server;
	// A reader to read console input.
	static BufferedReader consoleReader;
	// The username for this peer.
	static String username;
	// The address of this peer
	static String address;
	// The port to listen on: defaults to 3000
	static int port = 3000;
	// Handles incoming peer requests and dispatches the files.
	static Dispatcher dispatcher = null;

	// Program entry point.
	public static void main(String args[])
	{
		System.out.println("Starting");
		try
		{
			// Create the reader to get console input.
			consoleReader = new BufferedReader(new InputStreamReader(System.in));

			// Start up the CORBA bindings.
            server = new ServiceProxy();
			
			
	        try
	        {
	        	System.out.println(server.sayHello());
	        }
	        catch(Exception e)
	        {
	        	System.out.println("Unable to connect to the server.");
	        	return;
	        }
	        
	        // Get user credentials
	        System.out.println("Enter username:");
	        username = consoleReader.readLine();
	        System.out.println("Enter password:");
	        String password = consoleReader.readLine();
	        
	        // Validate credentials
	        if(server.authorize(username, "asdf"))
	        {
	        	System.out.println("Authorized");

	            address = InetAddress.getLocalHost().getHostAddress();
	            // Change the listen port depending on the user, so two clients can be tested on the same machine.
	            if(username.equals("jasonb"))
	            	port = 3001;
	            if(username.equals("angelinaj"))
	            	port = 3002;
	        	System.out.println("Listening on " + address + ":" + port);
	        	
	        	// Inform the server of our IP so that other peers know where to find our sweet, sweet, files
	        	server.updateAddress(username, address, port);
	        	
	        	// Start up the program loop.
	        	programLoop();
	        }
	        else System.out.println("Not authorized");

		} 
		catch (Exception e) 
		{
			System.out.println("Error while connecting to CORBA services.") ;
		}
	}
	
	
	// The main program loop
	static void programLoop() throws Exception
	{
		// Start up the dispatcher to listen for incoming requests and send files.
		dispatcher = new Dispatcher(port);
		dispatcher.run();
		
		boolean keepRunning = true;
		while(keepRunning)
		{
			// Display user options
			System.out.println();
			System.out.println("Enter an option:");
			System.out.println("1 - View all files");
			System.out.println("2 - Share a file");
			System.out.println("3 - Unshare a file");
			System.out.println("4 - Search for a file");
			System.out.println("x - Exit");
			System.out.println();
			
			// Get the chosen option.
			String key = consoleReader.readLine();
			key = key.toLowerCase();
			
			// Delegate user option
			if(key.equals("1"))
			{
				viewAllFiles();
			}
			if(key.equals("2"))
			{
				shareFile();
			}
			if(key.equals("3"))
			{
				unshareFile();
			}
			if(key.equals("4"))
			{
				searchFile();
			}
			if(key.equals("x"))
			{
				keepRunning = false;
				dispatcher.close();
			}
		}
	}
	
	// Lists all files owned by the user, and which are set to shared.
	static void viewAllFiles()
	{
		try
		{
			// Get the list of files that this user has shared.
			String [] sharedFiles = server.getSharedFiles(username);
			if(sharedFiles == null)
				sharedFiles = new String[0];
			// Get the list of all files that this user has in his/her Files folder
			File folder = new File("Files");
			File[] localFiles = folder.listFiles();
			
			// Display all files, and flag if they're shared.
			System.out.println();
			for(File file : localFiles)
			{
				System.out.print(file.getName());
				boolean isShared = false;
				for(String share : sharedFiles)
					
					if(share.equals(file.getName()))
						isShared = true;
				if(isShared)
					System.out.println(" - shared");
				else
					System.out.println();
			}
			System.out.println();
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
	}
	
	// Flag a file as shared.
	static void shareFile()
	{
		try
		{
			// Get the list of files in the Files folder.
			File folder = new File("Files");
			File[] listOfFiles = folder.listFiles();
			ArrayList<String> canShare = new ArrayList<String>();
			int i = 0;
			System.out.println();
			
			// Display the files in the Files folder that are not currently shared.
			for(File file : listOfFiles)
			{
				String localFile = file.getName();
				if(!server.isShared(username, localFile))
				{
					System.out.println(i + " - " + localFile);
					canShare.add(localFile);
					i++;
				}
			}
			
			// Get user choice of file to share, or go back.
			System.out.println();
			System.out.println("Select file to share, or x to go back.");
			System.out.println();
			String key = consoleReader.readLine();
			if(key.equals("x")) return;

			// Flag the chosen file as shared.
			int shareIndex = Integer.parseInt(key);
			server.share(username, canShare.get(shareIndex));
		}
		catch(Exception e)
		{
			System.out.println("Unrecognized input");
			return;
		}
	}
	
	// Unshare a file
	static void unshareFile()
	{
		try
		{
			// Get the list of shared files from the server
			String [] sharedFiles = server.getSharedFiles(username);
			if(sharedFiles == null)
				sharedFiles = new String[0];
			// Display shared files with numbers for user choice.
			System.out.println();
			for(int index = 0; index < sharedFiles.length; index++)
			{
				System.out.println(index + " - " + sharedFiles[index]);
			}
			
			// Get user selection
			System.out.println();
			System.out.println("Select a file to unshare, or x to go back.");
			System.out.println();
			String key = consoleReader.readLine();
			key = key.toLowerCase();
			if(key.equals("x")) return;

			// Unshare the file
			int shareIndex = Integer.parseInt(key);
			server.stopShare(username, sharedFiles[shareIndex]);
		}
		catch(Exception e)
		{
			System.out.println("Unrecognized input");
			return;
		}
	}
	
	// Search for a filename
	static void searchFile()
	{
		try
		{
			System.out.println();
			System.out.println("Enter a file name to search for:");
			System.out.println();
			
			// Get the filename to search fore
			String filename = consoleReader.readLine();
			
			// Get a list of addresses of peers with this file
			String[] addresses = server.findFile(username, filename);
			if(addresses == null)
				addresses = new String[0];
			
			// Check if we already have this file
			if(Files.exists(Paths.get("Files\\" + filename)))
			{
				System.out.println("You already have this file");
				return;
			}
			else if(addresses.length == 0) // Check if there are any matches at all
			{
				System.out.println("No matches found");
				return;
			}
			else
			{
				// Download the file if desired.
				System.out.println("File found. Download? Y/N");
				String key = consoleReader.readLine();
				key = key.toLowerCase();
				if(key.equals("n")) return;
				else 
					if(key.equals("y"))
					{
						downloadFile(addresses, filename);
					}
					else
					{
						System.out.println("Unrecognized input");
						return;
					}
			}
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
		System.out.println();
	}
	
	// Download a file to the Files folder.
	static void downloadFile(String [] addresses, String filename)
	{
		try
		{
			HttpURLConnection con = null;
			// Try each address until we can connect to one.
			for(String address: addresses)
			{
				try
				{
					// Connect with an HTTP connection, for simplicity
					URL url = new URL(address + "/" + filename);
					con = (HttpURLConnection)url.openConnection();
					displayHTTPCode((HttpURLConnection)con);
					// If we got this far, we're successfully connected and don't need to keep trying.
					break;
				}
				catch(ConnectException e)
				{
					con = null;
				}
			}

			// If con is null, we were unable to connect to a peer.
			if(con == null)
			{
				System.out.println("Unable to connect to any peers with this file");
				return;
			}
			
			
			
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
			incoming.close();
			
			// Create the file in the Files folder.
			Path path = Paths.get("Files\\" + filename);
			Files.write(path, byteStream.toByteArray());
			System.out.println("File downloaded");
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
	}

	// Output the HTTP code and message to console.
	public static void displayHTTPCode(HttpURLConnection con) throws Exception
	{

			switch(con.getResponseCode())
			{
				case 200:
					System.out.println("200 OK");
					break;
				case 400:
					System.out.println("400 Bad Request");
					break;
				case 401:
					System.out.println("401 Unauthorized");
					break;
				case 402:
					System.out.println("402 Payment Required");
					break;
				case 403:
					System.out.println("403 Forbidden");
					break;
				case 404:
					System.out.println("404 Not Found");
					break;
				case 405:
					System.out.println("405 Method Not Found");
					break;
				case 406:
					System.out.println("406 Not Acceptable");
					break;
				case 407:
					System.out.println("407 Proxy Authentication Required");
					break;
				case 408:
					System.out.println("408 Request Timeout");
					break;
				case 409:
					System.out.println("409 Conflict");
					break;
				case 410:
					System.out.println("410 Gone");
					break;
				case 411:
					System.out.println("411 Length Required");
					break;
				case 412:
					System.out.println("412 Precondition Failed");
					break;
				case 413:
					System.out.println("413 Request Entity Too Large");
					break;
				case 414:
					System.out.println("414 Request-URI Too Long");
					break;
				case 415:
					System.out.println("415 Unsupported Media Type");
					break;
				case 416:
					System.out.println("416 Requested Range Not Satisfiable");
					break;
				case 417:
					System.out.println("417 Expectation Failed");
					break;
				case 500:
					System.out.println("500 Internal Server Error");
					break;
				case 501:
					System.out.println("501 Not Implemented");
					break;
				case 502:
					System.out.println("502 Bad Gateway");
					break;
				case 503:
					System.out.println("503 Service Unavailable");
					break;
				case 504:
					System.out.println("504 Gateway Timeout");
					break;
				case 505:
					System.out.println("505 HTTP Version Not Supported");
					break;
				default:
					System.out.println("Unrecognized code: " + con.getResponseCode());
			}


	}
}
