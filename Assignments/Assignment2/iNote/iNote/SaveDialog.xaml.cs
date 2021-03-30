using iNote.ViewModels;
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

namespace iNote
{
    public sealed partial class SaveDialog : ContentDialog
    {
        NoteFileViewModel NFViewModel;
        public string FName = "";
        public bool IsExited = false;
        public bool IsValid = false;
        public SaveDialog(NoteFileViewModel nfvm)
        {
            NFViewModel = nfvm;
            this.InitializeComponent();

        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

            if (string.IsNullOrEmpty(fileName.Text))
            {
                args.Cancel = true;
                msgBox.Text = "Empty name";

            }
            // if name already exists
            else if (NFViewModel.NameExists(fileName.Text))
            {
                args.Cancel = true;
                msgBox.Text = "Name exists";
            }
            else
            {
                // set the file name
                FName = fileName.Text;
                args.Cancel = false;
                IsValid = true;
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.FName = "";
            this.IsExited = true;
        }
    }
}
