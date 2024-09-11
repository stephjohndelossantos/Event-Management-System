using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Final_Library_System
{
    /// <summary>
    /// Interaction logic for STUDENT_HOME.xaml
    /// </summary>
    public partial class STUDENT_HOME : Window
    {
        private SolidColorBrush color1 = new SolidColorBrush(Color.FromRgb(243, 243, 243)); //Color for hovering the menuButton
        private SolidColorBrush color2 = new SolidColorBrush(Color.FromArgb(0, 1, 1, 1)); //Color for exiting the menuButton
        private SolidColorBrush white = new SolidColorBrush(Colors.White); //white color
        private SolidColorBrush gray = new SolidColorBrush(Color.FromRgb(125, 125, 125)); //gray color

        private bool canProcDash = true;
        private bool canProcBorrow = false;
        private bool canProcReturn = false;
        private bool canProcHist = false;
        private bool canProcLog = false;

        private bool logout = false;
        private bool close = false;

        private string StudentId;
        private string selectedID;
        private int BookQuantity;
        private string BB_Id;

        //connection
        private static SqlConnection conn = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Library_System;Integrated Security=True");
        public STUDENT_HOME()
        {
            InitializeComponent();
            STUDENT_HOME_LOADED();
            
        }
        private void STUDENT_HOME_LOADED() 
        {
            Display_Details();
            Display_Grid_Book();
            Display_Grid_Return();
            Display_Grid_History();
            logout = false;
            close = false;
        }
        //for display and stuff,
        private void informationDisplay(string warn, bool choice) 
        {
            informationGrid.Visibility = Visibility.Visible;
            txtInformation.Text = warn;
            if (choice)
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
        private void okButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            informationGrid.Visibility = Visibility.Hidden;
            STUDENT_HOME_LOADED();
            DeleteBlurEffectToGrid(allGrid);
        }

        private void cancelButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            informationGrid.Visibility = Visibility.Hidden;
            STUDENT_HOME_LOADED();
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
            allGrid.BeginAnimation(BlurBitmapEffect.RadiusProperty, new DoubleAnimation(0, TimeSpan.FromSeconds(.1)));
        }
        private void menuColors(int i) 
        {
            //proceeding by clicking
            canProcDash = false;
            canProcBorrow = false;
            canProcReturn = false;
            canProcHist = false;
            canProcLog = false;

            //changes the color of menu buttons
            pathDashboard.Fill = gray;
            pathBorrow.Fill = gray;
            pathReturn.Fill = gray;
            pathHistory.Fill = gray;
            pathLogout.Fill = gray;
            txtDashboardGridButton.Foreground = gray;
            txtBorrowGridButton.Foreground = gray;
            txtReturnGridButton.Foreground = gray;
            txtHistoryGridButton.Foreground = gray;
            txtLogoutGridButton.Foreground = gray;

            //visibility of every panel
            homeView.Visibility = Visibility.Hidden;
            borrowView.Visibility = Visibility.Hidden;
            returnView.Visibility = Visibility.Hidden;
            histView.Visibility = Visibility.Hidden;


            if (i == 1)
            {
                canProcDash = true;
                pathDashboard.Fill = white;
                txtDashboardGridButton.Foreground = white;
                homeView.Visibility = Visibility.Visible;
                dashboardGridButton.Background = color2;
            }
            else if (i == 2)
            {
                canProcBorrow = true;
                pathBorrow.Fill = white;
                txtBorrowGridButton.Foreground = white;
                borrowView.Visibility = Visibility.Visible;
                borrowGridButton.Background = color2;
            }
            else if (i == 3)
            {
                canProcReturn = true;
                pathReturn.Fill = white;
                txtReturnGridButton.Foreground = white;
                returnView.Visibility = Visibility.Visible;
                returnGridButton.Background = color2;
            }
            else if (i == 4)
            {
                canProcHist = true;
                pathHistory.Fill = white;
                txtHistoryGridButton.Foreground = white;
                histView.Visibility = Visibility.Visible;
                historyGridButton.Background = color2;
            }
            else if (i == 5) 
            {
                canProcLog = true;
                pathLogout.Fill = white;
                txtLogoutGridButton.Foreground = white;
                logoutGridButton.Background = color2;
            }
        }
        //----------------This part is for Dashboard----------------
        public class dashboardGrid1
        {
            public string recentABBook { get; set; }
        }
        public class dashboardGrid2 
        {
            public string topBook { get; set; }
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
            STUDENT_HOME_LOADED();

        }

        private void Display_Details()
        {
            StudentId = DB_Connect.getStudentId;

            string FName = "SELECT Student_FirstName FROM Students WHERE Student_Id = '" + StudentId + "'";
            string tempFName = DB_Connect.getValue(FName);
            string LName = "SELECT Student_LastName FROM Students WHERE Student_Id = '" + StudentId + "'";
            string tempLName = DB_Connect.getValue(LName);
            txtFNameTitle.Text = tempFName + "!";

            string Name = tempFName + " " + tempLName;
            txtstudentName.Text =Name;
            string Id = "SELECT Student_Number FROM Students WHERE Student_Id = '" + StudentId + "'";
            txtstudentID.Text = DB_Connect.getValue(Id);
            string Course = "SELECT Course FROM Students WHERE Student_Id = '" + StudentId + "'";
            txtstudentCourse.Text = DB_Connect.getValue(Course);
            string Year = "SELECT Year FROM Students WHERE Student_Id = '" + StudentId + "'";
            txtstudentYear.Text = DB_Connect.getValue(Year);
            string Email = "SELECT Student_Email FROM Students WHERE Student_Id = '" + StudentId + "'";
            txtstudentEmail.Text = DB_Connect.getValue(Email);

            string bbBooks = "SELECT COUNT(*) FROM BorrowedBooks WHERE Student_Id = '" + StudentId + "'";
            borrowCount.Text = DB_Connect.getValue(bbBooks);
            string rbBooks = "SELECT COUNT(*) FROM ReturnedBooks WHERE Student_Id = '" + StudentId + "'";
            returnCount.Text = DB_Connect.getValue(rbBooks);

            string rbbBooks = "SELECT TOP 1 b.Book_Title FROM BorrowedBooks bb JOIN Books b ON bb.Book_Id = b.Book_Id WHERE bb.Student_Id = '" + StudentId + "' ORDER BY bb.DateTime DESC;";
            recBorrow.Text = DB_Connect.getValue(rbbBooks);

            string RBooks = "SELECT TOP 1 b.Book_Title FROM ReturnedBooks rb JOIN Books b ON rb.Book_Id = b.Book_Id WHERE rb.Student_Id = '" + StudentId + "' ORDER BY rb.Returned_date DESC;";
            recReturn.Text = DB_Connect.getValue(RBooks);

            if (recBorrow.Text == "")
            {
                recBorrow.Text = "None";
            }
            if (recReturn.Text == "")
            {
                recReturn.Text = "None";
            }

            //RECENTLY ADDED BOOKS ALSO FOR YOUUR TOP BOOKS
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
            query = "SELECT TOP 3 b.Book_Title, COUNT(rb.Book_Id) AS Borrow_Count FROM ReturnedBooks rb JOIN Books b ON rb.Book_Id = b.Book_Id JOIN Authors a ON b.Author_Id = a.Author_Id WHERE rb.Student_Id = "+StudentId+" GROUP BY b.Book_Title ORDER BY Borrow_Count DESC";
            command = new SqlCommand(query, conn);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    dashboardGrid2 dash = new dashboardGrid2
                    {
                        topBook = reader["Book_Title"].ToString(),
                    };

                    dataList2.Add(dash);
                }
            }
            topBookDataGrid.ItemsSource = dataList2;
        }
        //----------------This part is for Borrow--------------

        // TO CONVERT INTO UI
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
        private void borrowGridButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!canProcBorrow) 
            {
                borrowGridButton.Background = color1;
            }
        }

        private void borrowGridButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!canProcBorrow) 
            {
                borrowGridButton.Background = color2;
            }
            
        }

        private void borrowGridButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            menuColors(2);
            STUDENT_HOME_LOADED();

        }
        private void txtBookSearch_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
        //BOOK 
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
        private void borrowButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(selectedID))
                {
                    if (BookQuantity > 0)
                    {
                        string query = "INSERT INTO BorrowedBooks (Student_Id, Book_Id, DateTime) " +
                        "VALUES ('" + StudentId + "','" + selectedID + "', GETDATE()) " +
                        "UPDATE Books SET Book_Quantity = Book_Quantity - 1 WHERE Book_Id = '" + selectedID + "'";
                        DB_Connect.QueryCommands(query);
                        informationDisplay("You have successfully borrowed a book!", true);
                        Display_Grid_Book();
                    }
                    else
                    {
                        informationDisplay("The book is out of stock!", true);

                    }
                }
                else
                {
                    informationDisplay("Select book first!", true);
                }
            }
            catch (Exception ex) 
            {
                informationDisplay("Please recheck the details if it is in correct format or empty!", true);
            }
            
            STUDENT_HOME_LOADED();
        }

        //btm we pressed/click the grid to show the data
        private void bookDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;


            if (gd.SelectedItem != null)
            {
                Book selectedBook = (Book)gd.SelectedItem;
                selectedID = selectedBook.BookId.ToString();
                BookQuantity = selectedBook.Book_Quantity;
                bookTitleInfo.Text = selectedBook.Book_Title;
                bookISBNInfo.Text = selectedBook.Book_ISBN;
                bookAuthorInfo.Text = selectedBook.Book_Author;

            }
        }
        //code to put value in the database or table in sql
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
        //----------------This part is for Return--------------
        public class RBook
        {
            public int BorrowedId { get; set; }
            public int ReturnBookID { get; set; }
            public string ReturnDate { get; set; }
            public string ReturnBook_Title { get; set; }
            public string ReturnBook_ISBN { get; set; }
            public string ReturnBook_Author { get; set; }
        }
        private void returnGridButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!canProcReturn) 
            {
                returnGridButton.Background = color1;
            }
        }

        private void returnGridButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!canProcReturn) 
            {
                returnGridButton.Background = color2;
            }
        }

        private void returnGridButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            menuColors(3);
            STUDENT_HOME_LOADED();
        }

        // FOR BOOKSEARCH CODE FOR THE DATABASE /TABLE
        private void txtRBookSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRBookSearch.Text))
            {
                Display_Grid_Return();
            }
            else
            {
                List<RBook> filteredList = new List<RBook>();
                conn.Close();
                conn.Open();

                string query = "SELECT BorrowedBooks.Borrowed_Book_Id, BorrowedBooks.Book_Id, BorrowedBooks.DateTime, " +
               "Books.Book_Title, Books.Book_ISBN, " +
               "CONCAT(Authors.Author_FirstName, ' ', Authors.Author_LastName) AS Author " +
               "FROM BorrowedBooks " +
               "JOIN Books ON BorrowedBooks.Book_Id = Books.Book_Id " +
               "JOIN Authors ON Books.Author_Id = Authors.Author_Id " +
               "WHERE (Books.Book_Title LIKE '%" + txtRBookSearch.Text + "%' OR " +
               "Books.Book_ISBN LIKE '" + txtRBookSearch.Text + "%') AND " +
               "BorrowedBooks.Student_Id = @StudentId";


                SqlCommand command = new SqlCommand(query, conn);

                command.Parameters.AddWithValue("@StudentId", StudentId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        RBook rbook = new RBook
                        {
                            BorrowedId = Convert.ToInt32(reader["Borrowed_Book_Id"]),
                            ReturnBookID = Convert.ToInt32(reader["Book_Id"]),
                            ReturnDate = reader["DateTime"].ToString(),
                            ReturnBook_Title = reader["Book_Title"].ToString(),
                            ReturnBook_ISBN = reader["Book_ISBN"].ToString(),
                            ReturnBook_Author = reader["Author"].ToString(),

                        };

                        filteredList.Add(rbook);
                    }
                }

                returnDataGrid.ItemsSource = filteredList;
            }
        }
        private void returnButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(selectedID))
                {
                    string query = "BEGIN TRANSACTION; " +
                    "INSERT INTO ReturnedBooks (Borrowed_Book_Id, Book_Id, Student_Id, Borrowed_date, Returned_date) " +
                    "SELECT Borrowed_Book_Id, Book_Id, Student_Id, DateTime, GETDATE() FROM BorrowedBooks WHERE Borrowed_Book_Id = " + BB_Id + "; " +
                    "UPDATE Books SET Book_Quantity = Book_Quantity + 1 " +
                    "WHERE Books.Book_Id = (SELECT Book_Id FROM BorrowedBooks WHERE Borrowed_Book_Id = " + BB_Id + "); " +
                    "DELETE FROM BorrowedBooks WHERE Borrowed_Book_Id = " + BB_Id + "; " +
                    "COMMIT TRANSACTION;";
                    DB_Connect.QueryCommands(query);
                    informationDisplay("You have successfully returned a book!", true);
                    Display_Grid_Return();
                    Display_Grid_Book();
                }
                else
                {
                    informationDisplay("Select a book first!", true);
                }
            }
            catch (Exception ex) 
            {
                informationDisplay("Please recheck the details if it is in correct format or empty!", true);
            }            
            STUDENT_HOME_LOADED();
        }

        private void returnDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;


            if (gd.SelectedItem != null)
            {
                RBook selectedRBook = (RBook)gd.SelectedItem;
                selectedID = selectedRBook.ReturnBookID.ToString();
                BB_Id = selectedRBook.BorrowedId.ToString();
                returnTitleInfo.Text = selectedRBook.ReturnBook_Title;
                returnISBNInfo.Text = selectedRBook.ReturnBook_ISBN;
                returnAuthorInfo.Text = selectedRBook.ReturnBook_Author;
            }
        }
        private void Display_Grid_Return()
        {

            List<RBook> dataList = new List<RBook>();
            conn.Close();
            conn.Open();
            //MO CONNECT SA RETURNED OR DISPLAY THE TABLE FROM THE SQL DATABASE
            SqlCommand command = new SqlCommand(@"
    SELECT 
        BorrowedBooks.Borrowed_Book_Id, 
        BorrowedBooks.Book_Id, 
        BorrowedBooks.DateTime, 
        Books.Book_Title, 
        Books.Book_ISBN, 
        CONCAT(Authors.Author_FirstName, ' ', Authors.Author_LastName) AS Author 
    FROM 
        BorrowedBooks
    JOIN 
        Books ON BorrowedBooks.Book_Id = Books.Book_Id
    JOIN 
        Authors ON Books.Author_Id = Authors.Author_Id
    WHERE 
        BorrowedBooks.Student_Id = @StudentId", conn);
            command.Parameters.AddWithValue("@StudentId", StudentId);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    RBook rbook = new RBook
                    {
                        BorrowedId = Convert.ToInt32(reader["Borrowed_Book_Id"]),
                        ReturnBookID = Convert.ToInt32(reader["Book_Id"]),
                        ReturnDate = reader["DateTime"].ToString(),
                        ReturnBook_Title = reader["Book_Title"].ToString(),
                        ReturnBook_ISBN = reader["Book_ISBN"].ToString(),
                        ReturnBook_Author = reader["Author"].ToString(),

                    };

                    dataList.Add(rbook);
                }
            }
            returnDataGrid.ItemsSource = dataList;
        }
        //----------------This part is for History--------------
        public class History
        {
            public int HistoryBorrowedId { get; set; }
            public int HistoryBookID { get; set; }
            public string HistoryBook_Title { get; set; }
            public string HistoryBook_ISBN { get; set; }
            public string HistoryBook_Author { get; set; }
            public string HistoryBook_Borrow_Date { get; set; }
            public string HistoryBook_Return_Date { get; set; }
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
            menuColors(4);
            STUDENT_HOME_LOADED();
        }
        private void historyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;


            if (gd.SelectedItem != null)
            {
                History selectedHistory = (History)gd.SelectedItem;
                historyTitleInfo.Text = selectedHistory.HistoryBook_Title;
                historyISBNInfo.Text = selectedHistory.HistoryBook_ISBN;
                historyAuthorInfo.Text = selectedHistory.HistoryBook_Author;
                historyDataBInfo.Text = selectedHistory.HistoryBook_Borrow_Date;
                historyDateRInfo.Text = selectedHistory.HistoryBook_Return_Date;
            }
        }
        private void txtHistSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHistSearch.Text))
            {
                Display_Grid_History();
            }
            else
            {
                List<History> filteredList = new List<History>();
                conn.Close();
                conn.Open();

                string query = "SELECT rb.Borrowed_Book_Id, rb.Book_Id, b.Book_Title, b.Book_ISBN, " +
               "CONCAT(a.Author_FirstName, ' ', a.Author_LastName) AS Author, " +
               "rb.Borrowed_date, rb.Returned_date " +
               "FROM ReturnedBooks rb " +
               "JOIN Books b ON rb.Book_Id = b.Book_Id " +
               "JOIN Authors a ON b.Author_Id = a.Author_Id " +
               "WHERE b.Book_Title LIKE '%" + txtHistSearch.Text + "%' OR " +
               "b.Book_ISBN LIKE '" + txtHistSearch.Text + "%'OR " +
               "CONCAT(a.Author_FirstName, ' ', a.Author_LastName) LIKE '" + txtHistSearch.Text + "%'";

                SqlCommand command = new SqlCommand(query, conn);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        History hist = new History
                        {
                            HistoryBorrowedId = Convert.ToInt32(reader["Borrowed_Book_Id"]),
                            HistoryBookID = Convert.ToInt32(reader["Book_Id"]),
                            HistoryBook_Title = reader["Book_Title"].ToString(),
                            HistoryBook_ISBN = reader["Book_ISBN"].ToString(),
                            HistoryBook_Author = reader["Author"].ToString(),
                            HistoryBook_Borrow_Date = reader["Borrowed_date"].ToString(),
                            HistoryBook_Return_Date = reader["Returned_date"].ToString(),
                        };

                        filteredList.Add(hist);
                    }
                }

                historyDataGrid.ItemsSource = filteredList;
            }
        }
        private void Display_Grid_History()
        {
            List<History> dataList = new List<History>();
            conn.Close();
            conn.Open();
            SqlCommand command = new SqlCommand(@"
    SELECT 
        rb.Borrowed_Book_Id, 
        rb.Book_Id, 
        b.Book_Title, 
        b.Book_ISBN, 
        CONCAT(a.Author_FirstName, ' ', a.Author_LastName) AS Author, 
        rb.Borrowed_date, 
        rb.Returned_date 
    FROM 
        ReturnedBooks rb 
    JOIN 
        Books b ON rb.Book_Id = b.Book_Id 
    JOIN 
        Authors a ON b.Author_Id = a.Author_Id 
    WHERE 
        rb.Student_Id = @StudentId", conn);
            command.Parameters.AddWithValue("@StudentId", StudentId);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    History hist = new History
                    {
                        HistoryBorrowedId = Convert.ToInt32(reader["Borrowed_Book_Id"]),
                        HistoryBookID = Convert.ToInt32(reader["Book_Id"]),
                        HistoryBook_Title = reader["Book_Title"].ToString(),
                        HistoryBook_ISBN = reader["Book_ISBN"].ToString(),
                        HistoryBook_Author = reader["Author"].ToString(),
                        HistoryBook_Borrow_Date = reader["Borrowed_date"].ToString(),
                        HistoryBook_Return_Date = reader["Returned_date"].ToString(),

                    };

                    dataList.Add(hist);
                }
            }
            historyDataGrid.ItemsSource = dataList;
        }

        //-------------This part is for Logout---------------
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
            menuColors(5);
            STUDENT_HOME_LOADED();
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
        }

    }
}
