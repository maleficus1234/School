/*
 * Comp 489 TME1
 * Jason Bell
 * 3078931
 * February 24, 2016
 */

package WebService;

import java.net.*;
import java.io.*;
import java.nio.file.*;

// Serves HTTP and FTP requests for content found in the /Content folder.
public class WebService 
{
	// The socket to listen with.
	ServerSocket serverSocket = null;
	
	public WebService(int port) 
			throws IOException,
			IllegalArgumentException
	{
		this.serverSocket = new ServerSocket(port);
	}
	
	// Listen for and process requests.
	public void listen()
		throws IOException
	{
		// Check that the server socket was created properly.
		if(serverSocket == null)
			throw new NullPointerException();
		
		while(true)
		{
			// Blocks until a client connects
			Socket client = serverSocket.accept();
			
			//  Read the request message from the socket.
			BufferedReader reader = new BufferedReader(new InputStreamReader(client.getInputStream()));
			String inputLine;
			String filename = "";
			while ((inputLine = reader.readLine()) != null)
			{
				if(inputLine.length() == 0) break;
				// Get the filename being requested.
				if(inputLine.startsWith("GET"))
				{
					String [] split = inputLine.split(" ");
					filename = split[1];
				}
				
			}
			
			// A writer to output our response.
			BufferedWriter writer = new BufferedWriter(new OutputStreamWriter(client.getOutputStream()));
			// Did we find a file name? If not, it's some request type that we don't handle here.
			if(filename.equals(""))
			{
				writer.write("HTTP/1.1 501 Not Implemented\r\n");
				writer.write("Server: COMP489TME1 (Win32)\r\n");
				writer.write("Connection: Closed\r\n");
				writer.write("\r\n");
				writer.write(get404Message(filename));
			}
			else
			{
				System.out.println("Received HTTP GET for:");
				
				// We found a file name, so see if the file exists.
				Path file = Paths.get("Content" + filename);
				System.out.println(filename);
				if(Files.exists(file))
				{
					// File exists, so load it as a batch of bytes, and send it to the client.
					writer.write("HTTP/1.1 200 Continue\r\n");
					writer.write("\r\n");
					writer.flush();
					client.getOutputStream().write(Files.readAllBytes(file));
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
			
			// Flush before closing or the byte stream may be interrupted!
			writer.flush();
			
			client.close();
		}
	}
	
	// Closes the listen socket.
	public void close()
		throws IOException
	{
		if(this.serverSocket != null) this.serverSocket.close();
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
