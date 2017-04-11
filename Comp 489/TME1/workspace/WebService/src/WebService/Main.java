/*
 * Comp 489 TME1
 * Jason Bell
 * 3078931
 * February 24, 2016
 */

package WebService;

import java.io.*;

public class Main 
{
	// Program entry point
	public static void main(String[] args) 
	{
		WebService webService = null;
		try
		{
			// Create the web service, start 
			webService = new WebService(80);
			webService.listen();
		}
		catch(IOException e)
		{
			System.out.println("IOException:");
			e.printStackTrace();
		}
		catch(IllegalArgumentException e)
		{
			System.out.println("IllegalArgumentException:");
			e.printStackTrace();
		}
		finally
		{
			try
			{
				// Close on error.
				if(webService != null) 
					webService.close();
			}
			catch(IOException e)
			{
				System.out.println("IOException:");
				e.printStackTrace();
			}
		}
	}
}
