using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for TagEditor.xaml
    /// </summary>
    public partial class TagEditor : UserControl
    {
        private TagLib.File file;

        public TagEditor()
        {
            InitializeComponent();

        }

        //set the values of the tag editor
        public void SetValues(string title, string artist, string album, string year, TagLib.File tagFile)
        {
            try
            {
                titleTb.Text = title;
                artistTb.Text = artist;
                albumTb.Text = album;
                yearTb.Text = year;
                file = tagFile;
            }
            catch (FormatException ex)
            {
                MessageBox.Show("An exception Occured" + ex.Message);
            }
        }

        //Edit the current np3 tag info
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            string[] artists = artistTb.Text.Split(' ');

            file.Tag.Title = titleTb.Text;
            file.Tag.Performers = artists;
            file.Tag.Album = albumTb.Text;

            try
            {
                if (!(Convert.ToUInt32(yearTb.Text) > 2021))  //check if the year is not greater than 2021
                {
                    file.Tag.Year = Convert.ToUInt32(yearTb.Text);
                }
                else
                {
                    file.Tag.Year = 1999;  //if year is greater than 2021, wrong year, then set the year 1999
                }

                file.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An exception Occured" + ex.Message);
            }
        }
    }
}
