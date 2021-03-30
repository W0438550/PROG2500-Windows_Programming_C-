using LocalNote.Repositories;
using LocalNote.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace LocalNote.Commands
{
    public class SaveCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private NoteFileViewModel NFViewModel;
        private TextBox myTextBox;
        LocalNoteRepo localNoteRepo = new LocalNoteRepo();

        public SaveCommand(NoteFileViewModel nfViewModel, TextBox textBox)
        {
            NFViewModel = nfViewModel;
            myTextBox = textBox;
        }

        public bool CanExecute(object parameter)
        {
            return NFViewModel.canSave;
        }

        public async void Execute(object parameter)
        {
            string text = myTextBox.Text;

            //If the text doesn't match with the existing file names
            if (NFViewModel.isNew)
            {
                SaveDialog saveDialog = new SaveDialog(NFViewModel);
                await saveDialog.ShowAsync();
                // create a new file
                if (!saveDialog.IsExited)
                {
                    localNoteRepo.CreateNewFile(saveDialog.FName, text);
                }

            }
            else
            {
                // write to an existing file
                localNoteRepo.WriteToFile(text, NFViewModel.FileName);
            }


            // update the files collection and allow file searching
            NFViewModel.CreateAListOfFiles();
            NFViewModel.PerformFiltering();

            // make selectable/unselectable of command buttons
            NFViewModel.canEdit = false;
            NFViewModel.canDelete = false;
            NFViewModel.canSave = false;
            myTextBox.IsReadOnly = true;

            NFViewModel.ChangesMade();

            // set the text box to blank
            myTextBox.Text = "";

        }

        public void FireCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}
