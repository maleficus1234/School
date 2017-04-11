/*
 * Comp 489 TME1
 * Jason Bell
 * 3078931
 * February 24, 2016
 */

package WebClient;

import java.net.*;
import java.io.*;

// A client service that performs HTTP GET requests and displays the response.
public class ClientService 
{
	// Perform an HTTP GET request for the given address and display the result.
	public static void get(String address)
	{
		BufferedReader reader = null;
		try
		{
			// Create the proxy object
			Proxy proxy = new Proxy(Proxy.Type.HTTP, new InetSocketAddress("localhost", 4444));
			
			// Open connection to the address.
			URL url = new URL(address);
			URLConnection con = url.openConnection(proxy);
			
			// Display the HTTP server response code.
			
			if(con instanceof HttpURLConnection)
			{
				System.out.print("Server response: ");
				displayHTTPCode((HttpURLConnection)con);
				System.out.println(((HttpURLConnection) con).getResponseMessage());
			}
			
			// Read the content sent to the client from the address and dump to the console.
			reader = new BufferedReader(new InputStreamReader(url.openStream()));
			String inputLine;
	        while ((inputLine = reader.readLine()) != null)
	            System.out.println(inputLine);	        
		}
		catch(MalformedURLException e)
		{
			// Don't bother dumping the whole stacktrace: if this error happened it's because
			// the user entered an invalid URL.
			System.out.println("Malformed URL:");
			System.out.println(e.getMessage());
		}
		catch(Exception e)
		{
			System.out.println("Exception message: " + e.getMessage());
		}
		finally
		{
			try
			{
				if(reader != null)
					reader.close();
			}
			catch(Exception e)
			{
				e.printStackTrace();
			}
		}		
	}
	
	// Output the HTTP code and message to console.
	public static void displayHTTPCode(HttpURLConnection con)
	{
		try
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
		catch(Exception e) {}
	}
}
