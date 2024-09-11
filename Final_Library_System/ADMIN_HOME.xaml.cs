using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Final_Library_System
{
    /// <summary>
    /// Interaction logic for ADMIN_HOME.xaml
    /// </summary>
    public partial class ADMIN_HOME : Window
    {
        private SolidColorBrush color1 = new SolidColorBrush(Color.FromRgb(243, 243, 243)); //Color for hovering the menuButton
        private SolidColorBrush color2 = new SolidColorBrush(Color.FromArgb(0, 1, 1, 1)); //Color for exiting the menuButton
        private SolidColorBrush white = new SolidColorBrush(Colors.White); //white color
        private SolidColorBrush gray = new SolidColorBrush(Color.FromRgb(125, 125, 125)); //gray color

        private bool canProcDash = true;
        private bool canProcAuth = false;
        private bool canProcBook = false;
        private bool canProcStud = false;
        private bool canProcHist = false;
        private bool canProcLog = false;

        private bool authAdd = false;
        private bool authUpdate = false;
        private bool authDelete = false;
        private bool bookAdd = false;
        private bool bookUpdate = false; 
        private bool bookDelete = false;
        private bool studAdd = false;
        private bool studUpdate = false;
        private bool studDelete = false;
        private bool logout = false;
        private bool close = false;

        private string selectedID;
        private int selectedAuthorId;

        //connection for admin
        static SqlConnection conn = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Library_System;Integrated Security=True");
        public ADMIN_HOME()
        {
            InitializeComponent();
            ADMIN_HOME_LOADED();
        }
        private void ADMIN_HOME_LOADED() 
        {
            Display_Details();
            Display_Grid_Author();
            Display_Grid_Book();
            Display_CB_Authors();
            Display_Grid_Student();
            Display_Grid_RBook();
            authAdd = false;
            authUpdate = false;
            authDelete = false;
            bookAdd = false;
            bookUpdate = false;
            bookDelete = false;
            studAdd = false;
            studUpdate = false;
            studDelete = false;
            logout = false;
            close = false;
        }
        private void ApplyBlurEffectToGrid(Grid grid)
        {
            BlurEffect blurEffect = new BlurEffect
            {
                Radius = 4,
                RenderingBias = RenderingBias.Quality
            };

            grid.Effect = blurEffect;
        }
        private void DeleteBlurEffectToGrid(Grid grid)
        {

            BlurEffect blurEffect = new BlurEffect
            {
                Radius = 0,
                RenderingBias = RenderingBias.Quality
            };

            grid.Effect = blurEffect;
        }
        private void informationDisplay(string warn, bool choice) 
        {
            informationGrid.Visibility = Visibility.Visible;
            txtInformation.Text = warn;
            if(choice) 
            {
                okButton.Visibility = Visibility.Visible;
                canyesButton.Visibility = Visibility.Hidden;
            }
            else 
            {
                okButton.Visibility = Visibility.Hidden;
                canyesButton.Visibility = Visibility.Visible;
            }
            ApplyBlurEffectToGrid(allGrid);
        }
        private void okButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            informationGrid.Visibility = Visibility.Hidden;
            ADMIN_HOME_LOADED();
            DeleteBlurEffectToGrid(allGrid);
        }

        private void cancelButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            informationGrid.Visibility = Visibility.Hidden;
            ADMIN_HOME_LOADED();
            DeleteBlurEffectToGrid(allGrid);
        }

        private void yesButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            informationGrid.Visibility = Visibility.Hidden;

            if (logout) 
            {
                MainWindow mw = new MainWindow();
                mw.Show();
                this.Close();
            }
            else if (close) 
            {
                this.Close();
            }

        }
        private void Display_Details() 
        {
            string StudentCount = "SELECT COUNT(*) FROM Students;";
            regStudCount.Text = DB_Connect.getValue(StudentCount);

            string BookCounts = "SELECT COUNT(*) FROM Books;";
            bookCount.Text = DB_Connect.getValue(BookCounts);

            string BorrowCounts = "SELECT COUNT(*) FROM BorrowedBooks;";
            borrowCount.Text = DB_Connect.getValue(BorrowCounts);

            string ReturnedCounts = "SELECT COUNT(*) FROM ReturnedBooks;";
            returnCount.Text = DB_Connect.getValue(ReturnedCounts);

            string rbbBooks = "SELECT TOP 1 b.Book_Title FROM BorrowedBooks bb JOIN Books b ON bb.Book_Id = b.Book_Id ORDER BY bb.DateTime DESC;";
            recBorrow.Text = DB_Connect.getValue(rbbBooks);

            string RBooks = "SELECT TOP 1 b.Book_Title FROM ReturnedBooks rb JOIN Books b ON rb.Book_Id = b.Book_Id ORDER BY rb.Returned_date DESC;";
            recReturn.Text = DB_Connect.getValue(RBooks);

            List<dashboardGrid1> dataList1 = new List<dashboardGrid1>();
            conn.Close();
            conn.Open();
            string query = "SELECT TOP 5 Book_Title FROM Books WHERE Book_Id IS NOT NULL AND Book_Title IS NOT NULL ORDER BY Book_Id DESC";
            SqlCommand command = new SqlCommand(query, conn);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    dashboardGrid1 dash = new dashboardGrid1
                    {
                        recentABBook = reader["Book_Title"].ToString(),
                    };

                    dataList1.Add(dash);
                }
            }
            recentABDataGrid.ItemsSource = dataList1;

            List<dashboardGrid2> dataList2 = new List<dashboardGrid2>();
            conn.Close();
            conn.Open();
            query = "SELECT TOP 3 CONCAT(Student_FirstName, ' ', Student_LastName) as Student_Name FROM Students WHERE Student_Id IS NOT NULL ORDER BY Student_Id DESC";
            command = new SqlCommand(query, conn);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    dashboardGrid2 dash = new dashboardGrid2
                    {
                        recStudents = reader["Student_Name"].ToString(),
                    };

                    dataList2.Add(dash);
                }
            }
            recStudentsDataGrid.ItemsSource = dataList2;


        }
        private void menuColors(int i) 
        {
            canProcDash = false;
            canProcAuth = false;
            canProcBook = false;
            canProcStud = false;
            canProcHist = false;
            canProcLog = false;

            homeView.Visibility = Visibility.Hidden;
            authorView.Visibility = Visibility.Hidden;
            bookView.Visibility = Visibility.Hidden;
            studentView.Visibility = Visibility.Hidden;
            histView.Visibility = Visibility.Hidden;

            pathDashboard.Fill = gray;
            pathAuthor.Fill = gray; 
            pathBook.Fill = gray;
            pathStudent.Fill = gray;
            pathHistory.Fill = gray;
            pathLog.Fill = gray;
            txtDashboardGridButton.Foreground = gray;
            txtAuthorGridButton.Foreground = gray;
            txtBookGridButton.Foreground = gray;
            txtStudentGridButton.Foreground = gray;
            txtHistoryGridButton.Foreground = gray;
            txtLogoutGridButton.Foreground = gray;
            

            if (i == 1) 
            {
                canProcDash = true;
                homeView.Visibility = Visibility.Visible;
                pathDashboard.Fill = white;
                txtDashboardGridButton.Foreground = white;
                dashboardGridButton.Background = color2;
            }
            else if (i == 2)
            {
                canProcAuth = true;
                authorView.Visibility = Visibility.Visible;
                pathAuthor.Fill = white;
                txtAuthorGridButton.Foreground = white;
                authorGridButton.Background = color2;
            }
            else if (i == 3)
            {
                canProcBook = true;
                bookView.Visibility = Visibility.Visible;
                pathBook.Fill = white;
                txtBookGridButton.Foreground = white;
                bookGridButton.Background = color2;
            }
            else if (i == 4)
            {
                canProcStud = true;
                studentView.Visibility = Visibility.Visible;
                pathStudent.Fill = white;
                txtStudentGridButton.Foreground = white;
                studentGridButton.Background = color2;
            }
            else if (i == 5)
            {
                canProcHist = true;
                histView.Visibility = Visibility.Visible;
                pathHistory.Fill = white;
                txtHistoryGridButton.Foreground = white;
                historyGridButton.Background = color2;
            }
            else if (i == 6)
            {
                canProcLog = true;
                pathLog.Fill = white;
                txtLogoutGridButton.Foreground = white;
                logoutGridButton.Background = color2;
            }

        }
        //--------------This part is for Dashboard--------------------
        public class dashboardGrid1 
        {
            public string recentABBook { get; set; }            
        }
        public class dashboardGrid2 
        {
            public string recStudents { get; set; }
        }
        private void dashboardGridButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!canProcDash)
            {
                dashboardGridButton.Background = color1;
            }
        }

        private void dashboardGridButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!canProcDash)
            {
                dashboardGridButton.Background = color2;
            }
        }

        private void dashboardGridButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            menuColors(1);
            ADMIN_HOME_LOADED();
        }

        //-------------This part is for Author-----------
        public class Author
        {
            public int AuthId { get; set; }
            public string Author_FirstName { get; set; }
            public string Author_LastName { get; set; }
            public string Author_Country { get; set; }

            public string FullName { get; set; }
        }
        private void authorGridButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!canProcAuth)
            {
                authorGridButton.Background = color1;
            }
        }

        private void authorGridButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!canProcAuth)
            {
                authorGridButton.Background = color2;
            }
        }

        private void authorGridButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            menuColors(2);
            ADMIN_HOME_LOADED();
        }
        private void authAddButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (txtAuthorFName.Text != "" && txtAuthorLName.Text != "" && txtAuthorCountry.Text != "")
                {
                    string query = "insert into Authors values ('" + txtAuthorFName.Text + "','" + txtAuthorLName.Text + "','" + txtAuthorCountry.Text + "')";
                    DB_Connect.QueryCommands(query);
                    informationDisplay("The author has been added!", true);
                    authAdd = true;
                    Display_Grid_Author();
                }
                else 
                {
                    informationDisplay("Author details is not complete or empty!", true);
                }

            }
            catch (Exception ex) 
            {
                informationDisplay("Please recheck the details if it is in correct format or empty!", true);
            }
        }

        private void authUpdateButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string query = "UPDATE Authors " +
               "SET Author_FirstName = '" + txtAuthorFName.Text + "', " +
               "Author_LastName = '" + txtAuthorLName.Text + "', " +
               "Author_Country = '" + txtAuthorCountry.Text + "' " +
               "WHERE Author_Id = " + selectedID;

                DB_Connect.QueryCommands(query);
                informationDisplay("THe author has been updated!", true);
                authUpdate = true;
                Display_Grid_Author();
            }
            catch (Exception ex) 
            {
                informationDisplay("Please recheck the details if it is in correct format or empty!", true);

            }
           
        }

        private void authDeleteButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string query = "delete from Authors where  Author_Id = " + selectedID + "";
                DB_Connect.QueryCommands(query);
                informationDisplay("The author has been deleted!", true);
                authDelete = true;
                Display_Grid_Author();
            }
            catch (Exception ex) 
            {
                informationDisplay("Please recheck the details if it is in correct format or empty!", true);

            }            
        }

        private void authDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;

            if (gd.SelectedItem != null)
            {
                Author selectedAuthor = (Author)gd.SelectedItem;

                selectedID = selectedAuthor.AuthId.ToString();
                txtAuthorFName.Text = selectedAuthor.Author_FirstName;
                txtAuthorLName.Text = selectedAuthor.Author_LastName;
                txtAuthorCountry.Text = selectedAuthor.Author_Country;

            }
        }

        private void txtAuthorSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAuthorSearch.Text))
            {
                Display_Grid_Author();
            }
            else
            {
                List<Author> filteredList = new List<Author>();
                conn.Close();
                conn.Open();

                string query = "SELECT * FROM Authors WHERE " +
                    "Author_FirstName + ' ' + " +
                    "Author_Country + '' +" +
                    "Author_LastName LIKE '%" + txtAuthorSearch.Text + "%'";

                SqlCommand command = new SqlCommand(query, conn);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Author author = new Author
                        {
                            AuthId = Convert.ToInt32(reader["Author_Id"]),
                            Author_FirstName = reader["Author_FirstName"].ToString(),
                            Author_LastName = reader["Author_LastName"].ToString(),
                            Author_Country = reader["Author_Country"].ToString()
                        };

                        filteredList.Add(author);
                    }
                }

                authDataGrid.ItemsSource = filteredList;
            }
        }
        private void Display_Grid_Author()
        {
            List<Author> dataList = new List<Author>();
            conn.Close();
            conn.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM Authors", conn);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Author author = new Author
                    {
                        AuthId = Convert.ToInt32(reader["Author_Id"]),
                        Author_FirstName = reader["Author_FirstName"].ToString(),
                        Author_LastName = reader["Author_LastName"].ToString(),
                        Author_Country = reader["Author_Country"].ToString()
                    };

                    dataList.Add(author);
                }
            }
            authDataGrid.ItemsSource = dataList;

        }

        //--------------This part is for Book------------------
        public class Book
        {
            public int BookId { get; set; }
            public string Book_Title { get; set; }
            public string Book_ISBN { get; set; }
            public int Book_Quantity { get; set; }
            public int AuthID { get; set; }
            public string Book_Author { get; set; }
            public string Book_Status { get; set; }
        }
        private void bookGridButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!canProcBook)
            {
                bookGridButton.Background = color1;
            }
        }

        private void bookGridButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!canProcBook)
            {
                bookGridButton.Background = color2;
            }
        }

        private void bookGridButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            menuColors(3);
            ADMIN_HOME_LOADED();
        }

        private void bookDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;

            if (gd.SelectedItem != null)
            {
                Book selectedBook = (Book)gd.SelectedItem;

                selectedID = selectedBook.BookId.ToString();
                txtBookTitle.Text = selectedBook.Book_Title;
                txtBookISBN.Text = selectedBook.Book_ISBN;
                selectedAuthorId = selectedBook.AuthID;
                txtBookQuantity.Text = selectedBook.Book_Quantity.ToString();
                cboxBookAuthor.Text = selectedBook.Book_Author;
            }
        }

        private void bookAddButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (txtBookTitle.Text != "" && txtBookISBN.Text != "" && txtBookQuantity.Text != "")
                {
                    string query = "insert into Books values ('" + txtBookTitle.Text + "','" + txtBookISBN.Text +
                    "','" + txtBookQuantity.Text + "','" + selectedAuthorId + "')";
                    DB_Connect.QueryCommands(query);
                    informationDisplay("The book has been added!", true);
                    bookAdd = true;
                    Display_Grid_Book();
                }
                else 
                {
                    informationDisplay("Books details is not complete or empty!", true);
                }
                
            }
            catch (Exception ex) 
            {
                informationDisplay("Please recheck the details if it is in correct format or empty!", true);
            }            
        }

        private void bookUpdateButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string query = "UPDATE Books " +
              "SET Book_Title = '" + txtBookTitle.Text + "', " +
              "Book_ISBN = '" + txtBookISBN.Text + "', " +
              "Author_Id = '" + selectedAuthorId + "', " +
              "Book_Quantity = '" + txtBookQuantity.Text + "' " +
              "WHERE Book_Id = " + selectedID;

                DB_Connect.QueryCommands(query);
                informationDisplay("The book has been updated!", true);
                bookUpdate = true;
                Display_Grid_Book();
            }
            catch (Exception ex) 
            {
                informationDisplay("Please recheck the details if it is in correct format or empty!", true);
            }          
        }

        private void bookDeleteButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string query = "delete from Books where  Book_Id = " + selectedID + "";
                DB_Connect.QueryCommands(query);
                informationDisplay("The book has been deleted!", true);
                bookDelete = true;
                Display_Grid_Book();
            }
            catch (Exception ex) 
            {
                informationDisplay("Please recheck the details if it is in correct format or empty!", true);
            }            
        }
        private void txtBookSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBookSearch.Text))
            {
                Display_Grid_Book();
            }
            else
            {
                List<Book> filteredList = new List<Book>();
                conn.Close();
                conn.Open();

                string query = "SELECT *, " +
                               "CASE WHEN Books.Book_Quantity > 0 THEN 'AVAILABLE' ELSE 'NOT AVAILABLE' END AS Book_Status " +
                               "FROM Books " +
                               "JOIN Authors ON Books.Author_Id = Authors.Author_Id " +
                               "WHERE CONCAT(Book_Title, ' ', Book_ISBN, ' ', Authors.Author_FirstName, ' ', Authors.Author_LastName) LIKE '%" + txtBookSearch.Text + "%'";

                SqlCommand command = new SqlCommand(query, conn);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Book book = new Book
                        {
                            BookId = Convert.ToInt32(reader["Book_Id"]),
                            Book_Title = reader["Book_Title"].ToString(),
                            Book_ISBN = reader["Book_ISBN"].ToString(),
                            Book_Quantity = (int)reader["Book_Quantity"],
                            AuthID = Convert.ToInt32(reader["Author_Id"]),
                            Book_Author = reader["Author_FirstName"].ToString() + " " + reader["Author_LastName"].ToString(),
                            Book_Status = reader["Book_Status"].ToString()
                        };

                        filteredList.Add(book);
                    }
                }

                bookDataGrid.ItemsSource = filteredList;
            }

        }
        private void Display_CB_Authors()
        {
            List<Author> authorsList = new List<Author>();
            conn.Close();
            conn.Open();

            using (SqlCommand command = new SqlCommand("SELECT * FROM Authors", conn))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Author author = new Author
                        {
                            AuthId = Convert.ToInt32(reader["Author_Id"]),
                            Author_FirstName = reader["Author_FirstName"].ToString(),
                            Author_LastName = reader["Author_LastName"].ToString()
                        };

                        authorsList.Add(author);
                    }
                }
            }

            authorsList.ForEach(a => a.FullName = $"{a.Author_FirstName} {a.Author_LastName}");

            cboxBookAuthor.ItemsSource = authorsList;
            cboxBookAuthor.DisplayMemberPath = "FullName";
            cboxBookAuthor.SelectedValuePath = "Author_Id";
            cboxBookAuthor.SelectedValue = selectedAuthorId;
        }
        private void cboxBookAuthor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboxBookAuthor.SelectedItem != null)
            {
                selectedAuthorId = ((Author)cboxBookAuthor.SelectedItem).AuthId;
            }
        }
        private void Display_Grid_Book()
        {
            List<Book> dataList = new List<Book>();
            conn.Close();
            conn.Open();
            SqlCommand command = new SqlCommand(@"
        SELECT 
            Books.Book_Id,
            Books.Book_Title,
            Books.Book_ISBN,
            Books.Book_Quantity,
            Books.Author_Id AS AuthID,
            Authors.Author_FirstName + ' ' + Authors.Author_LastName AS Book_Author,
            CASE
                WHEN Books.Book_Quantity > 0 THEN 'AVAILABLE'
                ELSE 'NOT AVAILABLE'
            END AS Book_Status
        FROM 
            Books
        JOIN
            Authors ON Books.Author_Id = Authors.Author_Id", conn);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Book book = new Book
                    {
                        BookId = Convert.ToInt32(reader["Book_Id"]),
                        Book_Title = reader["Book_Title"].ToString(),
                        Book_ISBN = reader["Book_ISBN"].ToString(),
                        Book_Quantity = Convert.ToInt32(reader["Book_Quantity"]),
                        AuthID = Convert.ToInt32(reader["AuthID"]),
                        Book_Author = reader["Book_Author"].ToString(),
                        Book_Status = reader["Book_Status"].ToString()
                    };

                    dataList.Add(book);
                }
            }
            bookDataGrid.ItemsSource = dataList;
        }


        //--------------THis part is for Students--------------
        public class Student
        {
            public int StudID { get; set; }
            public int Student_Id { get; set; }
            public string Student_FirstName { get; set; }
            public string Student_LastName { get; set; }
            public string Student_Course { get; set; }
            public int Student_Year { get; set; }
            public string Student_Email { get; set; }
            public string Student_Password { get; set; }
        }
        private void studentGridButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!canProcStud)
            {
                studentGridButton.Background = color1;
            }
        }

        private void studentGridButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!canProcStud)
            {
                studentGridButton.Background = color2;
            }
        }

        private void studentGridButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            menuColors(4);
            ADMIN_HOME_LOADED();
        }
        private void studDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;

            if (gd.SelectedItem != null)
            {
                Student selectedStudent = (Student)gd.SelectedItem;

                selectedID = selectedStudent.StudID.ToString();
                txtStudentID.Text = selectedStudent.Student_Id.ToString();
                txtStudentFName.Text = selectedStudent.Student_FirstName;
                txtStudentLName.Text = selectedStudent.Student_LastName;
                txtStudentCourse.Text = selectedStudent.Student_Course;
                txtStudentYear.Text = selectedStudent.Student_Year.ToString();
                txtStudentEmail.Text = selectedStudent.Student_Email;
                txtStudentPassword.Text = selectedStudent.Student_Password;

            }
        }

        private void studAddButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (txtStudentID.Text != "" && txtStudentFName.Text != "" && txtStudentLName.Text != "" && txtStudentCourse.Text != "" && txtStudentYear.Text != "" && txtStudentEmail.Text != "" && txtStudentPassword.Text != "") 
                {
                    string query = "insert into Students values ('" + txtStudentID.Text + "','" + txtStudentEmail.Text + "','" + txtStudentPassword.Text +
                    "','" + txtStudentFName.Text + "','" + txtStudentLName.Text + "','" + txtStudentCourse.Text + "','" + txtStudentYear.Text + "')";
                    DB_Connect.QueryCommands(query);
                    informationDisplay("The student has been added!", true);
                    studAdd = true;
                    Display_Grid_Student();
                }
                else
                {
                    informationDisplay("Student details is not complete or empty!", true);
                }                
            }
            catch (Exception ex) 
            {
                informationDisplay("Please recheck the details if it is in correct format or empty!", true);
            }            
        }

        private void studUpdateButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string query = "UPDATE Students " +
               "SET Student_Number = '" + txtStudentID.Text + "', " +
               "Student_Email = '" + txtStudentEmail.Text + "', " +
               "Student_Password = '" + txtStudentPassword.Text + "', " +
               "Student_FirstName = '" + txtStudentFName.Text + "', " +
               "Student_LastName = '" + txtStudentLName.Text + "', " +
               "Course = '" + txtStudentCourse.Text + "', " +
               "Year= '" + txtStudentYear.Text + "' " +
               "WHERE Student_Id = " + selectedID;

                DB_Connect.QueryCommands(query);
                informationDisplay("The student has been updated!", true);
                studUpdate = true;
                Display_Grid_Student();
            }
            catch (Exception ex) 
            {
                informationDisplay("Please recheck the details if it is in correct format or empty!", true);
            }           
        }

        private void studDeleteButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string query = "delete from Students where  Student_Id = " + selectedID + "";
                DB_Connect.QueryCommands(query);
                informationDisplay("The student has been deleted!", true);
                studDelete = true;
                Display_Grid_Student();
            }
            catch (Exception ex) 
            {
                informationDisplay("Please recheck the details if it is in correct format or empty!", true);
            }            
        }
        private void txtStudSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStudSearch.Text))
            {
                Display_Grid_Student();
            }
            else
            {
                List<Student> filteredList = new List<Student>();
                conn.Close();
                conn.Open();

                string query = "SELECT * FROM Students WHERE " +
                               "CONVERT(NVARCHAR(MAX), Student_Number) + ' ' + " +
                                "Student_FirstName + ' ' + " +
                                "Student_LastName + ' ' + " +
                                "Course + ' ' + " +
                                "CONVERT(NVARCHAR(MAX), Year) + ' ' + " +
                                "Student_Password + ' ' + " +
                                "Student_Email LIKE '%" + txtStudSearch.Text + "%'";

                SqlCommand command = new SqlCommand(query, conn);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Student student = new Student
                        {
                            StudID = Convert.ToInt32(reader["Student_Id"]),
                            Student_Id = Convert.ToInt32(reader["Student_Number"]),
                            Student_FirstName = reader["Student_FirstName"].ToString(),
                            Student_LastName = reader["Student_LastName"].ToString(),
                            Student_Course = reader["Course"].ToString(),
                            Student_Year = Convert.ToInt32(reader["Year"]),
                            Student_Email = reader["Student_Email"].ToString(),
                            Student_Password = reader["Student_Password"].ToString(),
                        };

                        filteredList.Add(student);
                    }
                }
                studDataGrid.ItemsSource = filteredList;
            }

        }
        private void Display_Grid_Student()
        {
            List<Student> dataList = new List<Student>();
            conn.Close();
            conn.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM Students", conn);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Student student = new Student
                    {
                        StudID = Convert.ToInt32(reader["Student_Id"]),
                        Student_Id = Convert.ToInt32(reader["Student_Number"]),
                        Student_FirstName = reader["Student_FirstName"].ToString(),
                        Student_LastName = reader["Student_LastName"].ToString(),
                        Student_Course = reader["Course"].ToString(),
                        Student_Year = Convert.ToInt32(reader["Year"]),
                        Student_Email = reader["Student_Email"].ToString(),
                        Student_Password = reader["Student_Password"].ToString(),
                    };

                    dataList.Add(student);
                }
            }
            studDataGrid.ItemsSource = dataList;

        }
        //--------------This part is for History--------------
        public class RBook
        {
            public int Return_Borrow_ID { get; set; }
            public int Return_Borrow_Book_ID { get; set; }
            public int Return_Student_ID { get; set; }
            public string Return_Name { get; set; }
            public string Return_Title { get; set; }
            public string Return_ISBN { get; set; }
            public string Return_Author { get; set; }
            public string Return_Borrow_Date { get; set; }
            public string Return_Date { get; set; }
        }
        private void historyGridButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!canProcHist)
            {
                historyGridButton.Background = color1;
            }
        }

        private void historyGridButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!canProcHist)
            {
                historyGridButton.Background = color2;
            }
        }

        private void historyGridButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            menuColors(5);
            ADMIN_HOME_LOADED();
        }

        private void txtHistSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHistSearch.Text))
            {
                Display_Grid_RBook();
            }
            else
            {
                List<RBook> filteredList = new List<RBook>();
                conn.Close();
                conn.Open();

                string query = "SELECT rb.Returned_Books_Id AS Return_Borrow_ID, " +
                                "rb.Borrowed_Book_Id AS Return_Borrow_Book_ID, " +
                                "rb.Student_Id AS Return_Student_ID, " +
                                "CONCAT(s.Student_FirstName, ' ', s.Student_LastName) AS Return_Name, " +
                                "b.Book_Title AS Return_Title, " +
                                "b.Book_ISBN AS Return_ISBN, " +
                                "CONCAT(a.Author_FirstName, ' ', a.Author_LastName) AS Return_Author, " +
                                "rb.Borrowed_date AS Return_Borrow_Date, " +
                                "rb.Returned_date AS Return_Date " +
                                "FROM ReturnedBooks rb " +
                                "INNER JOIN Students s ON rb.Student_Id = s.Student_Id " +
                                "INNER JOIN Books b ON rb.Book_Id = b.Book_Id " +
                                "INNER JOIN Authors a ON b.Author_Id = a.Author_Id " +
                                "WHERE CONCAT(s.Student_FirstName, ' ', s.Student_LastName, b.Book_Title, b.Book_ISBN, a.Author_FirstName, ' ', a.Author_LastName) LIKE '%" + txtHistSearch.Text + "%'";
                SqlCommand command = new SqlCommand(query, conn);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        RBook hist = new RBook
                        {
                            Return_Borrow_ID = Convert.ToInt32(reader["Return_Borrow_ID"]),
                            Return_Borrow_Book_ID = Convert.ToInt32(reader["Return_Borrow_Book_ID"]),
                            Return_Student_ID = Convert.ToInt32(reader["Return_Student_ID"]),
                            Return_Name = reader["Return_Name"].ToString(),
                            Return_Title = reader["Return_Title"].ToString(),
                            Return_ISBN = reader["Return_ISBN"].ToString(),
                            Return_Author = reader["Return_Author"].ToString(),
                            Return_Borrow_Date = reader["Return_Borrow_Date"].ToString(),
                            Return_Date = reader["Return_Date"].ToString()
                        };

                        filteredList.Add(hist);
                    }
                }
                histDataGrid.ItemsSource = filteredList;
            }
        }
        private void Display_Grid_RBook()
        {
            List<RBook> dataList = new List<RBook>();
            conn.Close();
            conn.Open();

            SqlCommand command = new SqlCommand(@"
        SELECT 
            rb.Returned_Books_Id AS Return_Borrow_ID, 
            rb.Borrowed_Book_Id AS Return_Borrow_Book_ID, 
            rb.Student_Id AS Return_Student_ID,
            CONCAT(s.Student_FirstName, ' ', s.Student_LastName) AS Return_Name,
            b.Book_Title AS Return_Title,
            b.Book_ISBN AS Return_ISBN,
            CONCAT(a.Author_FirstName, ' ', a.Author_LastName) AS Return_Author,
            rb.Borrowed_date AS Return_Borrow_Date, 
            rb.Returned_date AS Return_Date
        FROM ReturnedBooks rb 
        JOIN Books b ON rb.Book_Id = b.Book_Id 
        JOIN Authors a ON b.Author_Id = a.Author_Id 
        JOIN Students s ON rb.Student_Id = s.Student_Id", conn);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    RBook rbook = new RBook
                    {
                        Return_Borrow_ID = Convert.ToInt32(reader["Return_Borrow_ID"]),
                        Return_Borrow_Book_ID = Convert.ToInt32(reader["Return_Borrow_Book_ID"]),
                        Return_Student_ID = Convert.ToInt32(reader["Return_Student_ID"]),
                        Return_Name = reader["Return_Name"].ToString(),
                        Return_Title = reader["Return_Title"].ToString(),
                        Return_ISBN = reader["Return_ISBN"].ToString(),
                        Return_Author = reader["Return_Author"].ToString(),
                        Return_Borrow_Date = reader["Return_Borrow_Date"].ToString(),
                        Return_Date = reader["Return_Date"].ToString()
                    };

                    dataList.Add(rbook);
                }
            }

            histDataGrid.ItemsSource = dataList;
        }


        //------------This part is for logout----------------
        private void logoutGridButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!canProcLog)
            {
                logoutGridButton.Background = color1;
            }
        }
        private void logoutGridButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!canProcLog)
            {
                logoutGridButton.Background = color2;
            }
        }

        private void logoutGridButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            menuColors(6);
            ADMIN_HOME_LOADED();
            informationDisplay("Are you sure you want to logout?", false);
            logout = true;
        }

        private void minimizeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void closeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            informationDisplay("Are you sure you want to exit? You will login again after closing.", false);
            close = true;
            //this.Close();
        }


    }
}
