using System;
using System.Data.SqlClient;
using System.Windows;
using System.Data;

namespace Final_Library_System
{
    internal class DB_Connect
    {
        static SqlConnection conn = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Library_System;Integrated Security=True");
        static SqlCommand command;
        static SqlDataAdapter adapter;
        public static string getStudentId;

        public static void QueryCommands(string query)
        {
            conn.Close();
            conn.Open();
            command = new SqlCommand(query, conn);
            command.ExecuteNonQuery();
            conn.Close();

        }

        public static string getValue(string query)
        {
            conn.Close();
            conn.Open();
            command = new SqlCommand(query, conn);
            object result = command.ExecuteScalar();
            if (result != null)
            {
                conn.Close();
                return result.ToString();
            }
            else
            {
                conn.Close();
                return null;
            }

        }

        public static DataTable Display(string query)
        {
            conn.Close();
            conn.Open();
            command = new SqlCommand(query, conn);
            adapter = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            conn.Close();
            return dt;

        }
        public static string compareID(string ID, string fname, string lname, string course, string year, string email, string pass)
        {
            string query = "SELECT * FROM Students WHERE Student_Number = @User_ID";
            conn.Close();
            conn.Open();
            command = new SqlCommand(query, conn);

            command.Parameters.AddWithValue("@User_ID", ID);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                return "The ID exist! Please check your ID!";
            }
            conn.Close();
            conn.Open();
            string inputquery = "INSERT INTO Students (Student_Number, Student_Email, Student_Password, Student_FirstName, Student_LastName, Course, Year) VALUES (@StudentNumber, @Email, @Password, @FirstName, @LastName, @Course, @Year)";
            command = new SqlCommand(inputquery, conn);
            command.Parameters.AddWithValue("@StudentNumber", ID);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Password", pass);
            command.Parameters.AddWithValue("@FirstName", fname);
            command.Parameters.AddWithValue("@LastName", lname);
            command.Parameters.AddWithValue("@Course", course);
            command.Parameters.AddWithValue("@Year", year);

            // Execute the query
            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                conn.Close();
                return "The Account successfully created!";
            }
            else
            {
                conn.Close();
                return "There is an error with the code! Please recheck";
            }

        }
        public static bool logIn(string id, string pass)
        {
            string query = "SELECT * FROM Students WHERE Student_Number = @User_ID AND Student_Password = @Password";
            conn.Close();
            conn.Open();
            command = new SqlCommand(query, conn);

            command.Parameters.AddWithValue("@User_ID", id);
            command.Parameters.AddWithValue("@Password", pass);

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}