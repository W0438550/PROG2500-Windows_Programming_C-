using iNote.Commands;
using iNote.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace iNote.ViewModels
{
    public class NoteFileViewModel : INotifyPropertyChanged
    {
        public SaveCommand SaveCommand { get; }
        public AddCommand AddCommand { get; }
        public EditCommand EditCommand { get; }
        public DeleteCommand DeleteCommand { get; }
        public ExitCommand ExitCommand { get; }
        //public AboutCommand AboutCommand { get; }

        private TextBox textBox;

        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<NoteFile> NoteFiles { get; set; }

        private List<NoteFile> _allFiles = new List<NoteFile>();

        private NoteFile _selectedFile;

        public bool isNew = true;

        public string FileDescription { get; set; }
        public string FileName { get; set; }
        public string FileText { get; set; }

        public bool canEdit { get; set; } = true;
        public bool canSave { get; set; } = true;
        public bool canDelete { get; set; } = false;

        private string _filter;


        public NoteFileViewModel(TextBox tBox)
        {
            textBox = tBox;

            // create an object of NoteFile collection
            NoteFiles = new ObservableCollection<NoteFile>();

            // initialize the commands
            this.DeleteCommand = new DeleteCommand(this);
            this.AddCommand = new AddCommand(this, tBox);
            this.EditCommand = new EditCommand(this, tBox);
            this.SaveCommand = new SaveCommand(this, tBox);
            this.ExitCommand = new ExitCommand();
            //this.AboutCommand = new AboutCommand();


            // create a list of files
            CreateAListOfFiles();

            //Perform Filtering
            PerformFiltering();
        }

        // get the selected file and update the command buttons
        public NoteFile SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                _selectedFile = value;

                if (value != null)
                {
                    FileName = value.FileName;
                    FileText = value.FileText;
                    canDelete = true;
                    canEdit = true;
                    canSave = false;
                    textBox.IsReadOnly = true;
                    isNew = false;
                }

                // envoke the event handler with filename and filetext changes
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FileName"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FileText"));

                // update the command event handlers 
                ChangesMade();
            }
        }

        // set the command event handlers
        public void ChangesMade()
        {
            DeleteCommand.FireCanExecuteChanged();
            AddCommand.FireCanExecuteChanged();
            EditCommand.FireCanExecuteChanged();
            SaveCommand.FireCanExecuteChanged();
            ExitCommand.FireCanExecuteChanged();
            //AboutCommand.FireCanExecuteChanged();

        }

        // filter the file names
        public void PerformFiltering()
        {
            NoteFiles.Clear();

            //If filter is null set it to ""
            if (_filter == null)
            {
                _filter = "";
            }

            // filter the value with lower case and trim string
            var lowerCaseFilter = Filter.ToLowerInvariant().Trim();

            //Use LINQ query to get all file names
            var result =
                _allFiles.Where(d => d.FileName.ToLowerInvariant()
                .Contains(lowerCaseFilter))
                .ToList();

            //Get list of values in current filtered list that we want to remove
            //(ie. don't meet new filter criteria)
            var toRemove = NoteFiles.Except(result).ToList();

            //Loop to remove items that fail filter
            foreach (var x in toRemove)
            {
                NoteFiles.Remove(x);
            }

            var resultCount = result.Count;

            // Add back in correct order.
            for (int i = 0; i < resultCount; i++)
            {
                var resultItem = result[i];
                if (i + 1 > NoteFiles.Count || !NoteFiles[i].Equals(resultItem))
                {
                    NoteFiles.Insert(i, resultItem);
                }
            }
        }

        public string Filter
        {
            get { return _filter; }
            set
            {
                if (value == _filter) { return; }
                _filter = value;
                PerformFiltering();

                // invoke whenever the property is changed
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Filter)));
            }
        }

        // Display the existing file names
        public void CreateAListOfFiles()
        {
            _allFiles.Clear();
            NoteFiles.Clear();

            //StorageFolder textFolder = ApplicationData.Current.LocalFolder;

            //IReadOnlyList<StorageFile> storageFiles = await textFolder.GetFilesAsync();

            ////For each file input into all files list
            //foreach (StorageFile file in storageFiles)
            //{
            //    if (file != null)
            //    {
            //        try
            //        {
            //            // display file names from the list
            //            _allFiles.Add(new NoteFile(file.DisplayName, await FileIO.ReadTextAsync(file)));
            //        }
            //        catch (FileNotFoundException ex)
            //        {
            //            Debug.WriteLine("Oh noes! File not found " + ex.Message);
            //        }
            //    }
            //}

            // add notefiles to the list
            _allFiles = Repositories.DataRepo.GetData();

            // call the perform filtering
            PerformFiltering();
        }

        // check whether the filename is already exist or not
        public bool NameExists(string text)
        {
            foreach (NoteFile file in NoteFiles)
            {
                if (text == file.FileName)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
