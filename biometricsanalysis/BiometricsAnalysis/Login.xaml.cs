using BiometricsAnalysis.Tables;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BiometricsAnalysis
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private Database_Utils db_util;
        private Log log;

        public Login()
        {
            this.InitializeComponent();
            db_util = new Database_Utils();
            log = null;
       //     user_name_txt.Text = "gabriel";
        //    password_txt.Text = "nicusor";
            //ButtonAutomationPeer peer = new ButtonAutomationPeer(connect_btn);
         //   IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
          //  invokeProv.Invoke();
            // Insert code required on object creation below this point.
        }

        private void connect_btn_Click(object sender, RoutedEventArgs e)
        {
            connect_util();
        }
        private void password_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                connect_util();
            }
        }

        private void user_name_txt_GotFocus(object sender, RoutedEventArgs e)
        {
            if (password_txt.Text == "")
            {
                user_name_txt.Foreground = Brushes.Black;
                password_txt.Foreground = Brushes.LightGray;
                password_txt.Text = "password";
            }
            if (user_name_txt.Text == "username")
            {
                user_name_txt.Foreground = Brushes.Black;
                user_name_txt.Text = "";
            }
        }

        private void password_txt_GotFocus(object sender, RoutedEventArgs e)
        {
            if (user_name_txt.Text == "")
            {
                password_txt.Foreground = Brushes.Black;
                user_name_txt.Foreground = Brushes.LightGray;
                user_name_txt.Text = "username";
            }
            if (password_txt.Text == "password")
            {
                password_txt.Foreground = Brushes.Black;
                password_txt.Text = "";

            }
        }

        private void user_name_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                connect_util();
            }
        }

        private void connect_util()
        {
            string user_name, password;
            int id_user = 0;
            user_name = user_name_txt.Text != "" ? user_name_txt.Text : "";
            password = password_txt.Text != "" ? password_txt.Text : "";

            id_user = db_util.query_user(user_name, password);


            if (id_user != 0)
            {
                log = new Log(id_user);
                log.id_log = db_util.insert_log(log);

                MainWindow main_window = new MainWindow();
                main_window.log = log;
                main_window.db_util = db_util;
                main_window.Show();
                this.Close();
            }
            else
            {
                info_lbl.Content = "Something went wrong!\nTry again";
            }
        }

        private void forgot_text_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void user_name_txt_LostFocus(object sender, RoutedEventArgs e)
        {
            if (user_name_txt.Text == "")
            {
                user_name_txt.Foreground = Brushes.LightGray;
                user_name_txt.Text = "username";
            }
        }

        private void password_txt_LostFocus(object sender, RoutedEventArgs e)
        {
            if (password_txt.Text == "")
            {
                password_txt.Foreground = Brushes.LightGray;
                password_txt.Text = "password";
            }
        }
    }
}