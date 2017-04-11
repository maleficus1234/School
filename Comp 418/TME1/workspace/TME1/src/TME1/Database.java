/*
 * COMP 418 TME 1
 * Jason Bell 3078931
 * March 21, 2016
 */

package TME1;

import java.sql.*;
import java.util.*;

public class Database 
{
	// Get a mysql connection: make sure to close it when you're done!
	public static Connection getConnection()
	{
		try
		{
			return DriverManager.getConnection("jdbc:mysql://localhost:3306/comp418tme1", "scrl", "scrl_tool");
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
		return null;
	}
	
	public static void generateData(Connection con)
	{
		try
		{
			//Connection con = getConnection();
			
			System.out.println("Generating test data. This make take a while...");
			
			System.out.println("Clearing current data from database...");
			
			// First, let's delete all current data in the database to avoid unwanted entries.
			// (Suppose that last time we generated 1000 students, but only want 100 this time)
			Statement statement = con.createStatement();
			statement.execute("TRUNCATE TABLE student");
			statement.execute("TRUNCATE TABLE course");
			statement.execute("TRUNCATE TABLE registration");
			
			Random rn = new Random();
			
			int courses = 100;
			int students = 1000;
			int studentsPerCourse = 50;
			
			
			System.out.println("Generating students...");
			
			for(int i = 0; i < students; i++)
			{
				int sid = i;
				String name = "student" + i;
				String address = name + " address";
				int telephone = i;
				int age = rn.nextInt((80 - 20) + 1) + 20;
				
				String sql = "INSERT INTO student (sid, name, address, telephone, age) VALUES (" + sid + ","
								+ "'" + name + "',"
								+ "'" + address + "',"
								+ telephone + ","
								+ age
								+ ")";
				//System.out.println(sql);
				statement.execute(sql);
				
			}
			
			// Generating courses and registering 50 random unique students into each.
			System.out.println("Generating courses and registering students...");
			for(int i = 0; i < courses; i++)
			{
				int courseno = i;
				String title = "COMP" + i;
				String dept = "COMP";
				int numberOfCredits = rn.nextInt((6 - 1) + 1) + 1;
				int courseFees = rn.nextInt((1000 - 10) + 1) + 10;
				
				String sql = "INSERT INTO course (courseno, title, department, numberofcredits, coursefees) VALUES (" + courseno + ","
						+ "'" + title + "',"
						+ "'" + dept + "',"
						+ numberOfCredits + ","
						+ courseFees
						+ ")";
				statement.execute(sql);
				// Register 50 students in the course
				List<Integer> studentNos = new ArrayList<Integer>();
				int jump = rn.nextInt((20 - 1) + 1) + 1;
				//int sid = jump;
				while(studentNos.size() <= studentsPerCourse)
				{
					Integer sid = rn.nextInt((999) + 1);
					if(!studentNos.contains(sid)) studentNos.add(sid);
				}
				
				for(Integer sid : studentNos)
				{
					// Get a random set of 50 unique student ids. We know the numbers fall between 0 and 999,
					// so there's no need to query the student table for the sake of this test data.
					int registeredCourse = courseno;
					String startDate = "Start date for student " + sid + " in courseno " + registeredCourse;
					String completeDate = "Completion date for student " + sid + " in courseno " + registeredCourse;
					int grade = rn.nextInt((100 - 0) + 1);
					sql = "INSERT INTO registration (sid, courseno, startdate, completedate, grade) VALUES (" + sid + ","
							+ registeredCourse + ","
							+ "'" + startDate + "',"
							+ "'" + completeDate + "',"
							+ grade
							+ ")";
					
					statement.execute(sql);
				}
			}
			statement.close();
			//con.close();
			System.out.println("Done generating test data.");
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
	}
	
	public static void treeIndexCoursesKey(Connection con)
	{
		try
		{
			//Connection con = getConnection();
			Statement statement = con.createStatement();
			statement.execute("CREATE INDEX `idx_course_courseno` USING BTREE ON `comp418tme1`.`course` (courseno) COMMENT '' ALGORITHM DEFAULT LOCK DEFAULT");
			statement.close();
			//con.close();
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
	}
	
	public static void treeIndexCoursesAll(Connection con)
	{
		try
		{
			//Connection con = getConnection();
			Statement statement = con.createStatement();
			statement.execute("CREATE INDEX `idx_course_courseno` USING BTREE ON `comp418tme1`.`course` (courseno, title, department, numberofcredits, coursefees) COMMENT '' ALGORITHM DEFAULT LOCK DEFAULT");
			statement.close();
			//con.close();
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
	}
	
	public static void hashIndexCoursesAll(Connection con)
	{
		try
		{
			//Connection con = getConnection();
			Statement statement = con.createStatement();
			statement.execute("CREATE INDEX `idx_course_courseno` USING HASH ON `comp418tme1`.`course` (courseno, title, department, numberofcredits, coursefees) COMMENT '' ALGORITHM DEFAULT LOCK DEFAULT");
			statement.close();
			//con.close();
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
	}
	
	public static void hashIndexCoursesKey(Connection con)
	{
		try
		{
			//Connection con = getConnection();
			Statement statement = con.createStatement();
			statement.execute("CREATE INDEX `idx_course_courseno` USING HASH ON `comp418tme1`.`course` (courseno) COMMENT '' ALGORITHM DEFAULT LOCK DEFAULT");
			statement.close();
			//con.close();
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
	}
	
	public static void deindexCourses(Connection con)
	{
		try
		{
			//Connection con = getConnection();
			Statement statement = con.createStatement();
			statement.execute("DROP INDEX `idx_course_courseno` ON `comp418tme1`.`course`");
			statement.close();
			//con.close();
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
	}
	
	public static void readAllCourses(Connection con)
	{
		try
		{
			//Connection con = getConnection();
			Statement statement = con.createStatement();
			ResultSet result = statement.executeQuery("SELECT * FROM course");
			while(result.next())
			{
				
			}
			statement.close();
			//con.close();
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
	}
	
	public static void deindexRegistration(Connection con)
	{
		try
		{
			//Connection con = getConnection();
			Statement statement = con.createStatement();
			statement.execute("DROP INDEX `idx_registration` ON `comp418tme1`.`registration`");
			statement.close();
			//con.close();
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
	}
	
	
	public static void treeIndexRegistrationAll(Connection con)
	{
		try
		{
			//Connection con = getConnection();
			Statement statement = con.createStatement();
			statement.execute("CREATE INDEX `idx_registration` USING BTREE ON `comp418tme1`.`registration` (grade) COMMENT '' ALGORITHM DEFAULT LOCK DEFAULT");
			statement.close();
			//con.close();
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
	}
	
	public static void hashIndexRegistrationAll(Connection con)
	{
		try
		{
			//Connection con = getConnection();
			Statement statement = con.createStatement();
			statement.execute("CREATE INDEX `idx_registration` USING HASH ON `comp418tme1`.`registration` (grade) COMMENT '' ALGORITHM DEFAULT LOCK DEFAULT");
			statement.close();
			//con.close();
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
	}

	public static void readAllRegistrations(Connection con)
	{
		try
		{
			//Connection con = getConnection();
			Statement statement = con.createStatement();
			ResultSet result = statement.executeQuery("SELECT grade FROM registration ORDER BY grade");
			
			while(result.next())
			{
				
			}
			statement.close();
			//con.close();
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
	}
	
	public static void increaseCourseFees(Connection con)
	{
		try
		{
			//Connection con = getConnection();
			Statement statement = con.createStatement();
			statement.execute("UPDATE course SET coursefees = coursefees + 6");
			statement.close();
			//con.close();
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
	}
	
	public static void hashIndexCourseFees(Connection con)
	{
		try
		{
			//Connection con = getConnection();
			Statement statement = con.createStatement();
			statement.execute("CREATE INDEX `idx_courseFees` USING HASH ON `comp418tme1`.`course` (coursefees) COMMENT '' ALGORITHM DEFAULT LOCK DEFAULT");
			statement.close();
			//con.close();
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
	}
	
	public static void readFeeRange(Connection con)
	{
		try
		{
			//Connection con = getConnection();
			Statement statement = con.createStatement();
			ResultSet result = statement.executeQuery("SELECT courseno, title, coursefees FROM course WHERE coursefees >= 400 AND coursefees <= 600");
			
			while(result.next())
			{
				
			}
			statement.close();
			//con.close();
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
	}
	
	public static void readGradeRange(Connection con)
	{
		try
		{
			//Connection con = getConnection();
			Statement statement = con.createStatement();
			ResultSet result = statement.executeQuery("SELECT startdate, completedate, grade FROM registration WHERE grade >= 30 AND grade <= 60");
			
			while(result.next())
			{
				
			}
			statement.close();
			//con.close();
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
	}
	
	public static void read5(Connection con)
	{
		try
		{
			//Connection con = getConnection();
			Statement statement = con.createStatement();
			ResultSet result = statement.executeQuery("SELECT student.sid, name "
+ " from student, registration, course "
+ " WHERE grade > 70 AND registration.sid = student.sid AND registration.courseno = course.courseno AND title = 'COMP0'"
+ " order by age asc");
			
			while(result.next())
			{
				
			}
			statement.close();
			//con.close();
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
	}
}
