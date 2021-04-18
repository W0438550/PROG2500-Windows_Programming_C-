using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TC2_RegistrationWizard
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public string WindowTitle { get; set; }
        User user = (User)Application.Current.Resources["user"];


        public MainPage()
        {
            this.InitializeComponent();
            tbFirstName.Text = user.FirstName ?? "";
            tbLastName.Text = user.LastName ?? "";
            tbPassword.Password = user.Password ?? "";

        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {

        }
                


        private void Next_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Page2));
            user.FirstName = tbFirstName.Text ?? "";
            user.LastName = tbLastName.Text ?? "";
            user.Password = tbPassword.Password ?? "";
            
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
