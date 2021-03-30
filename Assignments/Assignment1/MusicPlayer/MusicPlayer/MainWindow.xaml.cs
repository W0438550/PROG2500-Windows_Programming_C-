using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;


//Ref: https://www.wpf-tutorial.com/audio-video/how-to-creating-a-complete-audio-video-player/


namespace MusicPlayer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		TagLib.File currentFile;
		private bool mediaPlayerIsPlaying = false;
		private bool userIsDraggingSlider = false;
		Thread tagThread;

		//initialize the TagEditor class
		private TagEditor editor { get; set; } = null;
		public MainWindow()
		{
			InitializeComponent();

			DispatcherTimer timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += Timer_Tick;
			timer.Start();
			menuCurTag.IsEnabled = false;
			menuEditTag.IsEnabled = false;
			menuOpen.IsEnabled = true;
		}

		//declare the method to display time of the media playing
		private void Timer_Tick(object sender, EventArgs e)
		{
			if ((mePlayer.Source != null) && (mePlayer.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider))
			{
				SliProgress.Minimum = 0;
				SliProgress.Maximum = mePlayer.NaturalDuration.TimeSpan.TotalSeconds;
				SliProgress.Value = mePlayer.Position.TotalSeconds;
			}
		}

		private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;  //checks validity to execute
		}

		private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			try
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "Media files (*.mp3;*.mpg;*.mpeg)|*.mp3;*.mpg;*.mpeg|All files (*.*)|*.*"; //filter by file ext

				if (openFileDialog.ShowDialog() == true)
				{
					mePlayer.Source = new Uri(openFileDialog.FileName);
					currentFile = TagLib.File.Create(openFileDialog.FileName);  //copy media tag info to a TagLib file 
				}
				menuCurTag.IsEnabled = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show("An exception Occured" + ex.Message);
			}
		}

		private void Play_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (mePlayer != null) && (mePlayer.Source != null); //check validity to play media
		}

		private void Play_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			mePlayer.Play();
			mediaPlayerIsPlaying = true;
			menuCurTag.IsEnabled = true;

		}

		private void Pause_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = mediaPlayerIsPlaying;   //check validity to pause media
		}

		private void Pause_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			mePlayer.Pause();
		}

		private void Stop_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = mediaPlayerIsPlaying;  //check validity to stop media
		}

		private void Stop_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			mePlayer.Stop();
			mediaPlayerIsPlaying = false;
		}

		private void SliProgress_DragStarted(object sender, DragStartedEventArgs e)
		{
			userIsDraggingSlider = true;  // pre-checking that can slide progress can be executed, checks whether the action can be performed
		}

		private void SliProgress_DragCompleted(object sender, DragCompletedEventArgs e)
		{
			userIsDraggingSlider = false;
			mePlayer.Position = TimeSpan.FromSeconds(SliProgress.Value);  //set media player position in seconds
		}

		private void SliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			lblProgressStatus.Text = TimeSpan.FromSeconds(SliProgress.Value).ToString(@"hh\:mm\:ss");
		}

		private void Exit_Program(object sender, RoutedEventArgs e)
		{
			try
			{
				Application.Current.Shutdown();
			}
			catch (Exception ex)
			{
				MessageBox.Show("An exception Occured" + ex.Message);
			}
		}

		private void SetupEditor()
		{
			try
			{
				//set the current mp3 tag info hidden
				titleTb.Visibility = Visibility.Hidden;
				artistTb.Visibility = Visibility.Hidden;
				albumTb.Visibility = Visibility.Hidden;
				yearTb.Visibility = Visibility.Hidden;

				editor = new TagEditor();

				//get the current mp3 tag info from text boxeses
				var title = titleTb.Text;
				var artist = artistTb.Text;
				var album = albumTb.Text;
				var year = yearTb.Text;

				//call the method to set the tag info 
				editor.SetValues(title, artist, album, year, currentFile);

				//add the user control into the main window
				MainGrid.Children.Add(editor);

				Grid.SetColumn(editor, 0);
				Grid.SetColumn(editor, 1);
				Grid.SetRow(editor, 0);
				Grid.SetRow(editor, 1);
			}
			catch (Exception ex)
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
				titleTb.Visibility = Visibility.Visible;
				artistTb.Visibility = Visibility.Visible;
				albumTb.Visibility = Visibility.Visible;
				yearTb.Visibility = Visibility.Visible;


				var title = currentFile.Tag.Title;
				var artist = currentFile.Tag.AlbumArtists;
				var album = currentFile.Tag.Album;
				var year = currentFile.Tag.Year;
				string artistStr = string.Concat(artist);

				//diaplay the current mp3 tag info into textbox
				titleTb.Text = title;
				artistTb.Text = artistStr;
				albumTb.Text = album;
				yearTb.Text = year.ToString();
			}
			catch (Exception ex)
			{
				MessageBox.Show("An exception Occured" + ex.Message);
			}
		}

		private void Tag_edit(object sender, RoutedEventArgs e)
		{
			//call SetupEditor method to current mp3 tag info
			tagThread = new Thread(SetupEditor);
			SetupEditor();
			menuEditTag.IsEnabled = false;
		}
	}


}

