using LocalNote.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalNote.Repositories
{
    public static class DataRepo
    {
        static SqliteConnection db =
                new SqliteConnection("Filename=dbLocalNote.db");

        // create table and columns 
        public static void InitializeDatabase()
        {
            try
            {
                using (db)
                {
                    db.Open();
                    String createTable = "CREATE TABLE IF NOT EXISTS " +
                        "NoteTable (NoteID integer, " +
                        "FileName nvarchar(100) NOT NULL, " + "FileText nvarchar(400));";

                    SqliteCommand create = new SqliteCommand(createTable, db);
                    create.ExecuteReader();
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Oh noes! An error occurred with table creating " + ex.Message);
            }
        }

        // create a file with a file name and text in the database
        public static void CreateNewFile(string fileName, string text)
        {
            try
            {
                using (db)
                {
                    db.Open();
                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    insertCommand.CommandText = "INSERT INTO NoteTable " +
                        "VALUES (NULL, @fName, @txt);";
                    insertCommand.Parameters.AddWithValue("@fName", fileName);
                    insertCommand.Parameters.AddWithValue("@txt", text);
                    insertCommand.ExecuteReader();
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Oh noes! An error occurred with file creating " + ex.Message);
            }
        }

        // update the text of a file in the table
        public static void WriteToFile(string text, string fileName)
        {
            try
            {
                using (db)
                {
                    db.Open();
                    SqliteCommand updateCommand = new SqliteCommand();
                    updateCommand.Connection = db;

                    updateCommand.CommandText = "UPDATE NoteTable SET FileText = @txt WHERE FileName = @fName;";
                    updateCommand.Parameters.AddWithValue("@fName", fileName);
                    updateCommand.Parameters.AddWithValue("@txt", text);
                    updateCommand.ExecuteReader();
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Oh noes! An error occurred with file writing " + ex.Message);
            }
        }

        // delete a file, delete a row from the table
        public static void DeleteFile(string fileName)
        {
            try
            {
                using (db)
                {
                    db.Open();
                    SqliteCommand selectCommand = new SqliteCommand();
                    selectCommand.Connection = db;

                    selectCommand.CommandText = "DELETE FROM NoteTable " +
                        "WHERE FileName = @FName;";

                    selectCommand.Parameters.AddWithValue("@FName", fileName);

                    selectCommand.ExecuteReader();

                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Oh noes! An error occurred with file deleting " + ex.Message);
            }
        }

        // this function returns a list of the file name and text in a NoteFile model
        public static List<NoteFile> GetData()
        {
            List<NoteFile> entries = new List<NoteFile>();
            try
            {
                using (db)
                {
                    db.Open();

                    SqliteCommand selectCommand =
                        new SqliteCommand("SELECT FileName, FileText FROM NoteTable;", db);

                    SqliteDataReader query = selectCommand.ExecuteReader();
                    while (query.Read())
                    {
                        entries.Add(GetNoteFile(query));
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Oh noes! An error occurred with file creating " + ex.Message);
            }

            return entries;
        }

        // this function return a NoteFile object by combining file name and text
        private static NoteFile GetNoteFile(IDataRecord record)
        {
            return new NoteFile(record.GetString(0), record.GetString(1));
        }


    }
}
