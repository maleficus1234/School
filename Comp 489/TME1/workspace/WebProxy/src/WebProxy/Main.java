/*
 * Comp 489 TME1
 * Jason Bell
 * 3078931
 * February 24, 2016
 */

package WebProxy;

public class Main 
{
	// Program entry point
	public static void main(String[] args) 
	{
		// Start up a proxy service
		ProxyService webService = null;
		try
		{
			webService = new ProxyService(4444);
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
		
		// If the proxy service started successfully, start the listen thread, and close on the enter key being hit.
		if(webService != null)
		{
			webService.listen();
			try
			{
				System.in.read();
			}
			catch(Exception ex)
			{}
			finally
			{
				webService.close();
			}
		}
	}
}
