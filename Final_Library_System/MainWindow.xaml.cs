using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Final_Library_System
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string teststring;
        public MainWindow()
        {
            InitializeComponent();
            uclm.Source = new BitmapImage(new Uri(@"\IMAGE\uclm.jpg", UriKind.RelativeOrAbsolute));
        }

        //for register
        private void regButton_Click(object sender, RoutedEventArgs e)
        {

            col1.Width = new GridLength(1, GridUnitType.Star);
            col2.Width = new GridLength(1.3, GridUnitType.Star);
            registerPanel.Visibility = Visibility.Visible;
            loginPanel.Visibility = Visibility.Hidden;
            uclm.Source = new BitmapImage(new Uri(@"\IMAGE\uclm1.jpg", UriKind.RelativeOrAbsolute));

        }


        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            col1.Width = new GridLength(1.3, GridUnitType.Star);
            col2.Width = new GridLength(1, GridUnitType.Star);
            registerPanel.Visibility = Visibility.Hidden;
            loginPanel.Visibility = Visibility.Visible;
            uclm.Source = new BitmapImage(new Uri(@"\IMAGE\uclm.jpg", UriKind.RelativeOrAbsolute));
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            if (txtID.ToString() != "" && txtPassword.Password.ToString() != "")
            {
                if (DB_Connect.logIn(txtID.Text.ToString(), txtPassword.Password.ToString()))
                {
                    string query = "SELECT Student_Id FROM Students WHERE Student_Number = '" + txtID.Text + "'";
                    string result = DB_Connect.getValue(query);
                    DB_Connect.getStudentId = result;
                    if (txtID.Text == "123321")
                    {
                        ADMIN_HOME admn = new ADMIN_HOME();
                        admn.Show();
                        this.Close();
                    }
                    else
                    {
                        STUDENT_HOME std = new STUDENT_HOME();
                        std.Show();
                        this.Close();
                    }


                }
                else
                {
                    //LOGIN_SUC.Visibility = Visibility.Visible;
                    informationLoginText.Text = "Incorrect ID or Password";
                    informationLoginPanel.Visibility = Visibility.Visible;
                }
            }
            else
            {
                informationLoginText.Text = "Please enter the details!";
                informationLoginPanel.Visibility = Visibility.Visible;
            }

        }
        //register
        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtIDReg.Text.ToString() != "" && txtFNameReg.Text.ToString() != "" && txtLNameReg.Text.ToString() != "" && txtCourseReg.Text.ToString() != "" && txtYearReg.Text.ToString() != "" && txtEmailReg.Text.ToString() != "")
                {
                    if (txtPasswordReg.Password.ToString() == txtConPasswordReg.Password.ToString() && txtPasswordReg.Password.ToString() != "" && txtConPasswordReg.Password.ToString() != "")
                    {
                        teststring = DB_Connect.compareID(txtIDReg.Text.ToString(), txtFNameReg.Text.ToString(), txtLNameReg.Text.ToString(), txtCourseReg.Text.ToString(), txtYearReg.Text.ToString(), txtEmailReg.Text.ToString(), txtPasswordReg.Password.ToString());
                        informationRegisterText.Text = teststring;
                        informationRegisterPanel.Visibility = Visibility.Visible;
                        if (teststring == "The Account successfully created!")
                        {
                            col1.Width = new GridLength(1.3, GridUnitType.Star);
                            col2.Width = new GridLength(1, GridUnitType.Star);
                            registerPanel.Visibility = Visibility.Hidden;
                            loginPanel.Visibility = Visibility.Visible;

                        }
                        else
                        {
                            registerPanel.Visibility = Visibility.Visible;
                            loginPanel.Visibility = Visibility.Hidden;
                        }
                        uclm.Stretch = Stretch.Fill;
                    }
                    else
                    {
                        teststring = "Password did not matched! Please try again!";
                        informationRegisterText.Text = teststring;
                        informationRegisterPanel.Visibility = Visibility.Visible;
                    }

                }
                else
                {
                    teststring = "Details is not complete! Please recheck!";
                    informationRegisterText.Text = teststring;
                    informationRegisterPanel.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex) 
            {
                teststring = "Please recheck the details if it is in correct format!";
                informationRegisterText.Text = teststring;
                informationRegisterPanel.Visibility = Visibility.Visible;

            }

        }

        private void informationRegisterButton_Click(object sender, RoutedEventArgs e)
        {
            informationRegisterPanel.Visibility = Visibility.Hidden;
        }



        private void informationLoginButton_Click(object sender, RoutedEventArgs e)
        {
            informationLoginPanel.Visibility = Visibility.Hidden;
        }

        private void minimizeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void closeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
