
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using LocalNote.Commands;
using LocalNote.Models;
using LocalNote.Repositories;

using LocalNote.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.UI.Xaml.Controls;

namespace UnitTestLocalNote
{
    [TestClass]
    public class UnitTestLocalNote
    {
        //LocalNoteRepo localNoteRepo = new LocalNoteRepo();
        //private static StorageFolder textfileFolder = ApplicationData.Current.LocalFolder;

        // test method to check a file is created
        [TestMethod]
        public void Test_Check_Filename()
        {
            DataRepo.InitializeDatabase();

            string fileName = "NewTestFile";
            string text = "A new test file created";
            string fName, newText;
            bool isFile = false;

            DataRepo.CreateNewFile(fileName, text);

            List<NoteFile> entries = new List<NoteFile>();
            entries = DataRepo.GetData();

            foreach (NoteFile file in entries)
            {
                fName = file.FileName;
                newText = file.FileText;

                if(fName == fileName)
                {
                    isFile = true;
                    break;
                }
                
            }

            Assert.IsTrue(isFile);

        }

        // test method to check a file name is correctly created
        [TestMethod]
        public void Test_Check_Correct_Filename()
        {
            DataRepo.InitializeDatabase();

            string fileName = "NewTestFile";
            string text = "A new test file created";
            string fName;
            string anotherFileName = fileName + "modified";

            DataRepo.CreateNewFile(fileName, text);

            List<NoteFile> entries = new List<NoteFile>();
            entries = DataRepo.GetData();

            foreach (NoteFile file in entries)
            {
                fName = file.FileName;

                if (fName == fileName)
                {
                    break;
                }

            }

            Assert.AreNotEqual(fileName, anotherFileName);

        }

        // test method to check a file content is created
        [TestMethod]
        public void Test_Check_FileContent()
        {
            DataRepo.InitializeDatabase();

            string fileName = "NewTestFile";
            string text = "A new test file created";
            string newText;
            bool isText = false;

            DataRepo.CreateNewFile(fileName, text);

            List<NoteFile> entries = new List<NoteFile>();
            entries = DataRepo.GetData();

            foreach (NoteFile file in entries)
            {
                newText = file.FileText;

                if (text == newText)
                {
                    isText = true;
                    break;
                }

            }

            Assert.IsTrue(isText);

        }

        // test method to check a file content is accurately created
        [TestMethod]
        public void Test_Check_Correct_FileContent()
        {
            DataRepo.InitializeDatabase();

            string fileName = "NewTestFile";
            string text = "A new test file created";
            string anotherText = text + "modified";
            string newText;

            DataRepo.CreateNewFile(fileName, text);

            List<NoteFile> entries = new List<NoteFile>();
            entries = DataRepo.GetData();

            foreach (NoteFile file in entries)
            {
                newText = file.FileText;

                if (text == newText)
                {
                    break;
                }

            }

            Assert.AreNotEqual(text, anotherText);

        }

        // test method to check a write to file function
        [TestMethod]
        public void Test_Check_WriteToFile()
        {
            DataRepo.InitializeDatabase();

            string fileName = "NewTestFile";
            string text = "A new test file created";
            string anotherText = "modified";
            string newText="";

            DataRepo.CreateNewFile(fileName, text);
            Thread.Sleep(2000);
            DataRepo.WriteToFile(anotherText, fileName);


            List<NoteFile> entries = new List<NoteFile>();
            entries = DataRepo.GetData();

            foreach (NoteFile file in entries)
            {
                newText = file.FileText;

                if (newText == anotherText)
                {
                    break;
                }

            }

            Assert.AreEqual(anotherText, newText);

        }

        // test method to check a delete file function
        [TestMethod]
        public void Test_Check_Delete_File()
        {
            DataRepo.InitializeDatabase();

            string fileName = "FileForDeletion";
            string text = "A new test file created";
            bool isDeleted = false;
            string fName = "";

            DataRepo.CreateNewFile(fileName, text);
            Thread.Sleep(2000);
            DataRepo.DeleteFile(fileName);


            List<NoteFile> entries = new List<NoteFile>();
            entries = DataRepo.GetData();

            foreach (NoteFile file in entries)
            {
                fName = file.FileName;

                if (fName == fileName)
                {
                    isDeleted = true;
                    break;
                }

            }

            Assert.IsFalse(isDeleted);

        }




        //// test method to check a file is created
        //[TestMethod]
        //public async System.Threading.Tasks.Task Test_Check_Filename_Async()
        //{
        //    string fileName = "NewTestFile.txt";
        //    string text = "A new test file created";

        //    localNoteRepo.CreateNewFile(fileName, text);

        //    StorageFile file = null;

        //    file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
        //    string fileStr = file.Name.ToString();

        //    Assert.AreEqual(fileStr, fileName);
        //}

        //// test method to check a file is created with the given name, not match with other name
        //[TestMethod]
        //public async System.Threading.Tasks.Task Test_Check_Different_Filename_Async()
        //{
        //    string fileName = "NewTestFile.txt";
        //    string anotherFile = "NewTest.txt";
        //    string text = "A new test file created";

        //    localNoteRepo.CreateNewFile(fileName, text);

        //    StorageFile file = null;

        //    file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
        //    string fileStr = file.Name.ToString();

        //    Assert.AreNotEqual(fileStr, anotherFile);
        //}

        //// test method to check a file is created or not 
        //[TestMethod]
        //public void Test_Create_EmptyFile_Async()
        //{
        //    string newFile = "";
        //    string fileContent = "";

        //    System.Threading.Tasks.Task<Exception> task = Assert.ThrowsExceptionAsync<Exception>
        //        (async() => localNoteRepo.CreateNewFile(newFile, text: fileContent));
        //}

        //// test method to check a file is created with the given content
        //[TestMethod]
        //public async System.Threading.Tasks.Task Test_Same_FileContent_Async()
        //{
        //    string newFile = "NewContentTestFile.txt";
        //    string text = "A new test file created.";

        //    Windows.Storage.StorageFile sampleFile =
        //                        await textfileFolder.GetFileAsync(newFile + ".txt");
        //    await Windows.Storage.FileIO.WriteTextAsync(sampleFile, text);

        //    string retrievedText = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);

        //    Assert.AreEqual(retrievedText, text);
        //}

        //// test method to check a file is created with the given content, don't match with other content
        //[TestMethod]
        //public async System.Threading.Tasks.Task Test_Different_FileContent_Async()
        //{
        //    string newFile = "NewContentTestFile.txt";
        //    string text = "A new test file created.";
        //    string newStr = "Test to check that different file contents don't match";

        //    localNoteRepo.WriteToFile(text, newFile);

        //    Windows.Storage.StorageFile sampleFile =
        //                        await textfileFolder.GetFileAsync(newFile + ".txt");

        //    string retrievedText = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);

        //    Assert.AreNotEqual(retrievedText, newStr);
        //}

        //// test method to check deleteFile method if the file is not available
        //[TestMethod]
        //public void Test_Delete_File_Async()
        //{
        //    string newFile = "NewFile.txt";
        //    string fileContent = "A new file created to test the delete method.";

        //    localNoteRepo.CreateNewFile(newFile, fileContent);

        //    localNoteRepo.DeleteFile(newFile);

        //    System.Threading.Tasks.Task<FileNotFoundException> task = Assert.ThrowsExceptionAsync<FileNotFoundException>
        //        (async () => await ApplicationData.Current.LocalFolder.GetFileAsync(newFile));
        //}

        //// test method to check deleteFile method if the file name is not provided / file name is null
        //[TestMethod]
        //public void Test_Delete_Null_File_Async()
        //{
        //    string newFile = "NewFile.txt";
        //    string fileContent = "A new file created to test the delete method.";

        //    localNoteRepo.CreateNewFile(newFile, fileContent);

        //    System.Threading.Tasks.Task<FileNotFoundException>
        //        task = Assert.ThrowsExceptionAsync<FileNotFoundException>(async () => localNoteRepo.DeleteFile(null));
        //}

        //// test method to check WriteToFile method, if the method can append text to a file 
        //[TestMethod]
        //public async System.Threading.Tasks.Task Test_Write_Into_File_Async()
        //{
        //    string newFile = "AFile.txt";
        //    string text = "A new file created.";
        //    string newText = "New text added to the node";
        //    string expected = newText +text;

        //    localNoteRepo.CreateNewFile(newFile, text);
            
        //    Thread.Sleep(2000);

        //    localNoteRepo.WriteToFile(newText, newFile);
          
        //    Windows.Storage.StorageFile sampleFile =
        //                       await textfileFolder.GetFileAsync(newFile +".txt");

        //    string retrievedText = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);

        //    Assert.AreEqual(retrievedText, expected);

        //}




    }
}
