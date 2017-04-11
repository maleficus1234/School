package TME1;

import java.sql.Connection;

public class Main {

	public static void main(String[] args) 
	{
		
		
		Connection con = Database.getConnection();
		
		//Database.generateData(con);
		
		double time = 0;
		/*Database.deindexCourses(con);
		Database.readAllCourses(con);
		for(int i = 0; i < 1000; i++)
		{
			long start = System.nanoTime();
			Database.readAllCourses(con);
			time += (System.nanoTime() - start);
		}
		System.out.println("Average time to retrieve all courses without an index: " + time/1000.0/1000000.0);

		time = 0;
		Database.treeIndexCoursesKey(con);
		Database.readAllCourses(con);
		for(int i = 0; i < 1000; i++)
		{
			long start = System.nanoTime();
			Database.readAllCourses(con);
			time += (System.nanoTime() - start);
		}
		System.out.println("Average time to retrieve all courses with a B+ tree index on the primary key: " + time/1000.0/1000000.0);
		
		time = 0;
		Database.deindexCourses(con);
		Database.hashIndexCoursesKey(con);
		Database.readAllCourses(con);
		for(int i = 0; i < 1000; i++)
		{
			long start = System.nanoTime();
			Database.readAllCourses(con);
			time += (System.nanoTime() - start);
		}
		System.out.println("Average time to retrieve all courses with a hash index on the primary key: " + time/1000.0/1000000.0);
		
		time = 0;
		Database.deindexCourses(con);
		Database.treeIndexCoursesAll(con);
		Database.readAllCourses(con);
		for(int i = 0; i < 1000; i++)
		{
			long start = System.nanoTime();
			Database.readAllCourses(con);
			time += (System.nanoTime() - start);
		}
		System.out.println("Average time to retrieve all courses with a B+ tree index on all columns: " + time/1000.0/1000000.0);
		
		time = 0;
		Database.deindexCourses(con);
		Database.hashIndexCoursesAll(con);
		Database.readAllCourses(con);
		for(int i = 0; i < 1000; i++)
		{
			long start = System.nanoTime();
			Database.readAllCourses(con);
			time += (System.nanoTime() - start);
		}
		System.out.println("Average time to retrieve all courses with a hash index on all columns: " + time/1000.0/1000000.0);
		
		
		
		
		time = 0;
		Database.deindexRegistration(con);
		Database.readAllRegistrations(con);
		for(int i = 0; i < 1000; i++)
		{
			long start = System.nanoTime();
			Database.readAllRegistrations(con);
			time += (System.nanoTime() - start);
		}
		System.out.println("Average time to retrieve all registrations without an index: " + time/1000.0/1000000.0);
		
		time = 0;
		Database.treeIndexRegistrationAll(con);
		Database.readAllRegistrations(con);
		for(int i = 0; i < 1000; i++)
		{
			long start = System.nanoTime();
			Database.readAllRegistrations(con);
			time += (System.nanoTime() - start);
		}
		System.out.println("Average time to retrieve all registrations with a B+ tree index on all columns: " + time/1000.0/1000000.0);
		
		
		
		time = 0;
		Database.deindexRegistration(con);
		Database.hashIndexRegistrationAll(con);
		Database.readAllRegistrations(con);
		for(int i = 0; i < 1000; i++)
		{
			long start = System.nanoTime();
			Database.readAllRegistrations(con);
			time += (System.nanoTime() - start);
		}
		System.out.println("Average time to retrieve all registrations with a hash index on all columns: " + time/1000.0/1000000.0);*/
		
		/*time = 0;
		//Database.deindexRegistration(con);
		//Database.hashIndexRegistrationAll(con);
		Database.hashIndexCourseFees(con);
		Database.increaseCourseFees(con);
		for(int i = 0; i < 1000; i++)
		{
			long start = System.nanoTime();
			Database.increaseCourseFees(con);
			time += (System.nanoTime() - start);
		}
		System.out.println("Average time to increase course fees without an index: " + time/1000.0/1000000.0);*/
		
		/*time = 0;
		//Database.deindexRegistration(con);
		//Database.hashIndexRegistrationAll(con);
		//Database.hashIndexCourseFees(con);
		Database.readGradeRange(con);
		for(int i = 0; i < 1000; i++)
		{
			long start = System.nanoTime();
			Database.readGradeRange(con);
			time += (System.nanoTime() - start);
		}
		System.out.println("Average time to read course fee range without an index: " + time/1000.0/1000000.0);*/
		
		time = 0;
		//Database.deindexRegistration(con);
		//Database.hashIndexRegistrationAll(con);
		//Database.hashIndexCourseFees(con);
		Database.read5(con);
		for(int i = 0; i < 1000; i++)
		{
			long start = System.nanoTime();
			Database.read5(con);
			time += (System.nanoTime() - start);
		}
		System.out.println("Average time to read course fee range without an index: " + time/1000.0/1000000.0);
		
		try
		{
			con.close();
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
	}

	
}
