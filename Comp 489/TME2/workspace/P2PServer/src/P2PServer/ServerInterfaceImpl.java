package P2PServer;

import org.omg.CORBA.*;

import P2PCommon.*;

public class ServerInterfaceImpl extends ServerServantPOA
{
	ORB orb;
	Database database;
	
	public void setORB(ORB orb_val) 
	{
		orb = orb_val; 
		database = new Database();
	}

	public void share(String username, String filename)
	{
		database.share(username, filename);
	}
  
	public boolean authorize(String username, String password)
	{
		return database.authorize(username, password);
	}
  
	public void updateAddress(String username, String address, int port)
	{
		database.updateAddress(username, address, port);
	}
  
	public String [] getSharedFiles(String username)
	{
		return database.getShared(username);
	}
  
	public boolean isShared(String username, String filename)
	{
		return database.isShared(username, filename);
	}
  
	public void stopShare(String username, String filename)
	{
		database.stopShare(username, filename);
	}
  
	public String sayHello()
	{
		return "The server says hi!";
	}

	public String [] findFile(String username, String filename)
	{
		return database.findFile(username, filename);
	}
}
