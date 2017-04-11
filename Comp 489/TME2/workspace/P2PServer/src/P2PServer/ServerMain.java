package P2PServer;

import org.omg.CORBA.*;
import org.omg.PortableServer.*;
import org.omg.PortableServer.POA;
import org.omg.CosNaming.*;

import P2PCommon.*;

public class ServerMain 
{
	// Program entry point.
	public static void main(String args[])
	{
		// Start up the database binding
		Database db = new Database();
		db.getConnection();
		
		// Start up CORBA
		ORB orb = ORB.init(args, null);
		
		try
		{
			POA rootpoa = POAHelper.narrow(orb.resolve_initial_references("RootPOA"));
			rootpoa.the_POAManager().activate();
			
			ServerInterfaceImpl serverInterface = new ServerInterfaceImpl();
			serverInterface.setORB(orb);
			
			org.omg.CORBA.Object ref = rootpoa.servant_to_reference(serverInterface);
		    ServerServant href = ServerServantHelper.narrow(ref);
		    
		    org.omg.CORBA.Object obj = orb.resolve_initial_references("NameService");
	        NamingContextExt namingContext = NamingContextExtHelper.narrow(obj);
	        
	        String name = "P2PCORBA";
	        NameComponent path[] = namingContext.to_name( name );
	        namingContext.rebind(path, href);

	        System.out.println("P2P server ready and waiting ...");

	        orb.run();
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
	}
}
