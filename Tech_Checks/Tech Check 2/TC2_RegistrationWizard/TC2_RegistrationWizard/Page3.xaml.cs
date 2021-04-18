using System;
using System.Collections.Generic;
using System.Diagnostics;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace TC2_RegistrationWizard
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Page3 : Page
    {
        User user = (User)Application.Current.Resources["user"];

        public Page3()
        {
            this.InitializeComponent();
            tbEmail.Text = user.Email ?? "";
            tbHomePage.Text = user.HomePage ?? "";

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(user.FirstName + Environment.NewLine + user.LastName + Environment.NewLine + user.Password + Environment.NewLine + user.Address +
                Environment.NewLine + user.City + Environment.NewLine + user.Postal + Environment.NewLine + user.Email + Environment.NewLine + user.HomePage);
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Page2));



        }
    }
}
