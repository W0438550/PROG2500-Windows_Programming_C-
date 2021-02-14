using System;
using System.Windows;
using System.Windows.Controls;

namespace MusicPlayer
{
    /// <summary>
    ///  logic for TagEditor.xaml
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
                titleDisplay.Text = title;
                artistDisplay.Text = artist;
                albumDisplay.Text = album;
                yearDisplay.Text = year;
                file = tagFile;
            }
            catch(FormatException ex)
            {
                MessageBox.Show("An exception Occured" + ex.Message);
            }
        }

        // Edit the current mp3 tag info
        private void SaveButton(object sender, RoutedEventArgs e)
        {
            string[] artists = artistDisplay.Text.Split(' ');

            file.Tag.Title = titleDisplay.Text;
            file.Tag.Performers = artists;
            file.Tag.Album = albumDisplay.Text;

            try
            {
                if(!(Convert.ToUInt32(yearDisplay.Text) > 2021))  //check whether the 
                {
                    file.Tag.Year = Convert.ToUInt32(yearDisplay.Text);
                }
                else
                {
                    file.Tag.Year = 1459;  //if year is greater than 2021, wrong year, then set the year 1459
                }

                file.Save();
            }
            catch(Exception ex)
            {
                MessageBox.Show("An exception Occured" + ex.Message);
            }
        }
    }
}
