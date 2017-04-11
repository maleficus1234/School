/*
 * Comp 489 TME1
 * Jason Bell
 * 3078931
 * February 24, 2016
 */

package WebClient;

import java.io.*;

public class Main 
{
	// The program entry point
	public static void main(String[] args) 
	{
		// Create a buffered reader so that we can read commands from the console.
		BufferedReader br = new BufferedReader(new InputStreamReader(System.in));
		// Program keeps running while this is true
		boolean keepRunning = true;
		while(keepRunning)
		{
			// Super duper helpful console message.
			System.out.println();
			System.out.println("Enter an address to send a GET request to, or q to exit.");
			String input = "";
			try
			{
				// Read the command. It'll either be 'q' to quit or an url.
				input = br.readLine();
			}
			catch(Exception e) {}
			// Process the console input: either ignore if it's nothing, quit, or send the get request.
			if(!input.equals(""))
			{
				if(input.equals("q"))
					keepRunning = false;
				else
					ClientService.get(input);
			}
		}
	}
}
