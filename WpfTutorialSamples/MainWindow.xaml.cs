using Microsoft.Win32;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;


//Reference to building a media player in WPF: https://www.wpf-tutorial.com/audio-video/how-to-creating-a-complete-audio-video-player/


namespace MusicPlayer
{
    /// <summary>
    /// Logic for the buttons on the Windows.xaml form
    /// </summary>
    public partial class MainWindow : Window
    {
		TagLib.File currentFile;
        private bool musicPlayerIsPlaying = false;
        private bool isDragSlider = false;
		Thread tagThread;

		//A tag editor class is created with getters and setters
        private TagEditor editor { get; set; } = null;
        public MainWindow()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer(); // new timer object created to record time of media
            timer.Interval = TimeSpan.FromSeconds(1); // timer is set to 1 second of interval 
            timer.Tick += Timer_Tick;
            timer.Start();
			menuCurTag.IsEnabled = false;
			menuEditTag.IsEnabled = false;
			menuOpen.IsEnabled = true;
        }

		// method to show the time in seconds and minutes of the media currently playing
        private void Timer_Tick(object sender, EventArgs e)
        {
            if ((mePlayer.Source != null) && (mePlayer.NaturalDuration.HasTimeSpan) && (!isDragSlider))
            {
                SliProgress.Minimum = 0;
                SliProgress.Maximum = mePlayer.NaturalDuration.TimeSpan.TotalSeconds;
                SliProgress.Value = mePlayer.Position.TotalSeconds;
            }
        }

		private void Open_Execute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;  // execute event only if it is true
		}

		private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			try
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "Media files (*.mp3;*.mpg;*.mpeg)|*.mp3;*.mpg;*.mpeg|All files (*.*)|*.*"; // all the file types playable is displayed to user 

				if (openFileDialog.ShowDialog() == true)
				{
					mePlayer.Source = new Uri(openFileDialog.FileName);
					currentFile = TagLib.File.Create(openFileDialog.FileName);  //copy media tag info being isplayed to a TagLib file 
				}
				menuCurTag.IsEnabled = true;
			}
			catch(Exception ex)
            {
				MessageBox.Show("An exception Occured" + ex.Message);
			}
		}

		private void Play_Execute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (mePlayer != null) && (mePlayer.Source != null); // play the media only if the conditions are not null
		}

		private void Play_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			mePlayer.Play();
			musicPlayerIsPlaying = true;
			menuCurTag.IsEnabled = true;
			
		}

		private void Pause_Execute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = musicPlayerIsPlaying;   // pause media only if the media is already playing, otherwise it is set to invisible 
		}

		private void Pause_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			mePlayer.Pause();
		}

		private void Stop_Execute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = musicPlayerIsPlaying;  // stop the media only if the media is playing, otherwise it is set to invisible
		}

		private void Stop_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			mePlayer.Stop();
			musicPlayerIsPlaying = false; // check if the mode of the player is set to false
		}

		private void Slider_DragStarted(object sender, DragStartedEventArgs e)
		{
			isDragSlider = true;  // pre-checking that can slide progress can be executed, checks whether the action can be performed
		}

		private void Slider_DragCompleted(object sender, DragCompletedEventArgs e)
		{
			isDragSlider = false;
			mePlayer.Position = TimeSpan.FromSeconds(SliProgress.Value);  //set media player position in seconds
		}

		private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			lblProgressStatus.Text = TimeSpan.FromSeconds(SliProgress.Value).ToString(@"hh\:mm\:ss");
		}

        private void Exit_Program(object sender, RoutedEventArgs e)
        {
			try
			{
				Application.Current.Shutdown();
			}
			catch(Exception ex)
            {
				MessageBox.Show("An exception Occured" + ex.Message);
			}
        }

		private void Editor_Setup()
        {
			try
			{	
				//set the current mp3 tag info hidden
				titleDisplay.Visibility = Visibility.Hidden;
				artistDisplay.Visibility = Visibility.Hidden;
				albumDisplay.Visibility = Visibility.Hidden;
				yearDisplay.Visibility = Visibility.Hidden;

				editor = new TagEditor();

				//get the current mp3 tag info from text boxeses
				var title = titleDisplay.Text;
				var artist = artistDisplay.Text;
				var album = albumDisplay.Text;
				var year = yearDisplay.Text;

				//call the method to set the tag info 
				editor.SetValues(title, artist, album, year, currentFile);

				//add the user control into the main window
				MainGrid.Children.Add(editor);

				Grid.SetColumn(editor, 0);
				Grid.SetColumn(editor, 1);
				Grid.SetRow(editor, 0);
				Grid.SetRow(editor, 1);
			}
			catch(Exception ex)
            {
				MessageBox.Show("An exception Occured" + ex.Message);
			}

		}

        private void Current_tag(object sender, RoutedEventArgs e)
        {
			try
			{
				//set the current mp3 tag info visible
				menuEditTag.IsEnabled = true;
				titleDisplay.Visibility = Visibility.Visible;
				artistDisplay.Visibility = Visibility.Visible;
				albumDisplay.Visibility = Visibility.Visible;
				yearDisplay.Visibility = Visibility.Visible;


				var title = currentFile.Tag.Title;
				var artist = currentFile.Tag.AlbumArtists;
				var album = currentFile.Tag.Album;
				var year = currentFile.Tag.Year;
				string artistStr = string.Concat(artist);

				//display the current mp3 tag info into textbox
				titleDisplay.Text = title;
				artistDisplay.Text = artistStr;
				albumDisplay.Text = album;
				yearDisplay.Text = year.ToString();
			}
			catch(Exception ex)
            {
				MessageBox.Show("An exception Occured" + ex.Message);
			}
		}

        private void Tag_Editor(object sender, RoutedEventArgs e)
        {
			//call Editor_Setup method to current mp3 tag info
			tagThread = new Thread(Editor_Setup);
			Editor_Setup();
			menuEditTag.IsEnabled = false;
        }
    }

  
}

