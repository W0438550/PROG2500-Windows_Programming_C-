using LocalNote.Models;
using LocalNote.Repositories;
using LocalNote.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace LocalNote.Commands
{
    public class DeleteCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private NoteFileViewModel NFViewModel;

        LocalNoteRepo localNoteRepo = new LocalNoteRepo();

        public DeleteCommand(NoteFileViewModel nfViewModel)
        {
            NFViewModel = nfViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return NFViewModel.canDelete;

        }

        public async void Execute(object parameter)
        {
            // create a content dialog with the appropriate contents
            ContentDialog deleteFileDialog = new ContentDialog
            {
                Title = "Delete Note",
                Content = "Are you sure you want to delete this node?",
                PrimaryButtonText = "Delete Note",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await deleteFileDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {

                // delete the selected file from the windows storage
                DataRepo.DeleteFile(NFViewModel.FileName);

                // set the selected file empty and make unselectable the Edit and Save button
                NFViewModel.SelectedFile = new NoteFile("", "");
                NFViewModel.canEdit = false;
                NFViewModel.canSave = false;

                NFViewModel.ChangesMade();
                NFViewModel.PerformFiltering();
                NFViewModel.CreateAListOfFiles();
            }
            else
            {
                // do nothing
            }
        }

        public void FireCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
