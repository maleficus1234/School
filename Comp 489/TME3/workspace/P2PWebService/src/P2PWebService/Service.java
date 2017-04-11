/*
 * COMP 489 TME 3
 * Jason Bell 3078931
 * February 28, 2016
 */

package P2PWebService;

public class Service 
{
	public void share(String username, String filename)
	{
		Database.share(username, filename);
	}
  
	public boolean authorize(String username, String password)
	{
		return Database.authorize(username, password);
	}
  
	public void updateAddress(String username, String address, int port)
	{
		Database.updateAddress(username, address, port);
	}
  
	public String [] getSharedFiles(String username)
	{
		return Database.getShared(username);
	}
  
	public boolean isShared(String username, String filename)
	{
		return Database.isShared(username, filename);
	}
  
	public void stopShare(String username, String filename)
	{
		Database.stopShare(username, filename);
	}
  
	public String sayHello()
	{
		return "The server says hi!";
	}

	public String [] findFile(String username, String filename)
	{
		return Database.findFile(username, filename);
	}
 } 
