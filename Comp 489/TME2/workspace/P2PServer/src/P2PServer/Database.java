/*
 * COMP 489 TME 2
 * Jason Bell 3078931
 * February 28, 2016
 */

package P2PServer;

import java.sql.*;
import java.util.*;

// Wraps up JDBC code
public class Database 
{
	// Get a mysql connection: make sure to close it when you're done!
	public Connection getConnection()
	{
		try
		{
			return DriverManager.getConnection("jdbc:mysql://localhost:3306/p2p", "scrl", "scrl_tool");
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
		return null;
	}
	
	// Authorize this username and password
	public boolean authorize(String username, String password)
	{
		try
		{
			// Query uses with this username and password
			Connection con = getConnection();
			Statement statement = con.createStatement();
			ResultSet result = statement.executeQuery("SELECT username FROM user WHERE username = '" + username + "' AND password = '" + password + "';");
			boolean foundUser = false;
			// If there's a result, there's a match for this username and password.
			if(result.next())
				foundUser = true;
			con.close();
			return foundUser;
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
		return false;
	}
	
	// Update the most recent address and port for this username.
	public void updateAddress(String username, String address, int port)
	{
		try
		{
			
			Connection con = getConnection();
			Statement statement = con.createStatement();
			// Update the user entry with the most recent address and port.
			statement.execute("UPDATE user SET lastaddress = '" + address + "', lastport = " + port + " WHERE username = '" + username + "';");
			con.close();
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
	}
	
	// Return true if this file is being shared by the user, false if not.
	public boolean isShared(String username, String filename)
	{
		try
		{
			Connection con = getConnection();
			Statement statement = con.createStatement();
			// Query shared files with this username and filename
			ResultSet result = statement.executeQuery("SELECT filename FROM sharedfile WHERE username = '" + username + "' AND filename = '" + filename + "';");
			boolean isShared = false;
			// If there's a result, we have a match.
			if(result.next())
				isShared = true;
			con.close();
			return isShared;
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
		return false;
	}
	
	// Add or set a file as shared
	public void share(String username, String filename)
	{
		try
		{
			// If user is already sharing this file, skip.
			if(isShared(username, filename)) return;
			
			Connection con = getConnection();
			Statement statement = con.createStatement();
			// Add shared file for this user.
			statement.execute("REPLACE INTO sharedfile (username, filename) VALUES('" + username + "','" + filename + "');");
			con.close();
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
	}
	
	// Get a list of filenames shared by this user.
	public String [] getShared(String username)
	{
		ArrayList<String> files = new ArrayList<String>();
		try
		{
			Connection con = getConnection();
			Statement statement = con.createStatement();
			// Get all entries from sharedfile with this username
			ResultSet result = statement.executeQuery("SELECT * FROM sharedfile WHERE username = '" + username + "';");
			while(result.next())
				files.add(result.getString("filename"));
			con.close();
			return files.toArray(new String[0]);
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
		return files.toArray(new String[0]);
	}
	
	// Stop sharing a file from this user.
	public void stopShare(String username, String filename)
	{
		try
		{
			// If it's already not being shared, ignore.
			if(!isShared(username, filename)) return;
			
			Connection con = getConnection();
			Statement statement = con.createStatement();
			// Delete the shared file entry with this username and filename.
			statement.execute("DELETE FROM sharedfile WHERE username = '" + username + "' AND filename = '" + filename + "';");
			con.close();
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
	}
	
	// Find the addresses of peers with this file.
	public String [] findFile(String username, String filename)
	{
		// Store found addresses here. Returns an empty list if none is found.
		ArrayList<String> addresses = new ArrayList<String>();
		try
		{
			// If user is already sharing this file, just return.
			if(isShared(username, filename)) return addresses.toArray(new String[0]);
			
			Connection con = getConnection();
			Statement statement = con.createStatement();
			// Find all shared file instances of this filename.
			ResultSet result = statement.executeQuery("SELECT * FROM sharedfile WHERE filename = '" + filename + "';");
			ArrayList<String> users = new ArrayList<String>();
			
			// Get the usernames of all peers with this file (we're NOT sending that info to the user though!)
			while(result.next())
			{
				users.add(result.getString("username"));
			}
			
			// Get the last known address for each user, and ONLY return the addresses to the user.
			for(String user : users)
			{
				result = statement.executeQuery("SELECT lastaddress, lastport FROM user WHERE username = '" + user + "';");
				while(result.next())
					addresses.add("HTTP://"+result.getString("lastaddress") + ":" + result.getString("lastport"));
			}
			con.close();
			
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
		return addresses.toArray(new String[0]);
	}
}
