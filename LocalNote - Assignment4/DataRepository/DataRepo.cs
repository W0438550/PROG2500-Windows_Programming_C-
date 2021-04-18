using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Data;

namespace DataRepository
{
    public static class DataRepo
    {

        public static void InitializeDatabase()
        {
            using (SqliteConnection db =
                new SqliteConnection("Filename=dbLocalNote.db"))
            {
                db.Open();
                String createTable = "CREATE TABLE IF NOT EXISTS " +
                    "NoteTable (NoteID integer, " +
                    "FileName nvarchar(100) NOT NULL, " + "FileText nvarchar(200));";

                SqliteCommand create = new SqliteCommand(createTable, db);
                create.ExecuteReader();
            }
        }

        public static void CreateNewFile(string fileName, string text)
        {
            using (SqliteConnection db =
                new SqliteConnection("Filename=dbLocalNote.db"))
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

        public static void WriteToFile(string text, string fileName)
        {
            using (SqliteConnection db =
                new SqliteConnection("Filename=dbLocalNote.db"))
            {
                db.Open();
                SqliteCommand updateCommand = new SqliteCommand();
                updateCommand.Connection = db;

                updateCommand.CommandText = "UPDATE NoteTable SET FileText = @txt WHERE FileName = @fName;";
                  //  "VALUES (NULL, @fName, @txt);";
                updateCommand.Parameters.AddWithValue("@fName", fileName);
                updateCommand.Parameters.AddWithValue("@txt", text);
                updateCommand.ExecuteReader();
            }
        }

        public static void DeleteFile(string fileName)
        {
            using (SqliteConnection db =
                new SqliteConnection("Filename=dbLocalNote.db"))
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

        //public static List<string> GetData()
        //{
        //    List<string> entries = new List<string>();

        //    using (SqliteConnection db =
        //       new SqliteConnection("Filename=dbLocalNote.db"))
        //    {
        //        db.Open();
        //        SqliteCommand selectCommand =
        //            new SqliteCommand("SELECT FileName FROM NoteTable;", db);

        //        SqliteDataReader query = selectCommand.ExecuteReader();
        //        while (query.Read())
        //        {
        //            entries.Add(query.GetString(0));
        //        }
        //        return entries;
        //    }
        //}

        //public static List<NoteFile> GetData()
        //{
        //    List<NoteFile> entries = new List<NoteFile>();

        //    //using (SqliteConnection db =
        //    //   new SqliteConnection(ConnectionName))

        //    using (SqliteConnection db =
        //     new SqliteConnection("Filename=dbLocalNote.db"))
        //    {

        //        db.Open();

        //        SqliteCommand selectCommand =
        //            new SqliteCommand("SELECT NoteName, NoteContent, NoteID FROM NoteTable;", db);

        //        SqliteDataReader query = selectCommand.ExecuteReader();
        //        while (query.Read())
        //        {
        //            entries.Add(GetNoteFile(query));
        //        }


        //        return entries;
        //    }
        //}

        public static string GetFileData(string fileName)
        {
            string result = "";

            using (SqliteConnection db =
               new SqliteConnection("Filename=dbLocalNote.db"))
            {

                try
                {
                    db.Open();
                    SqliteCommand selectCommand =
                        new SqliteCommand("SELECT FileText FROM NoteTable WHERE FileName = @FName;", db);

                        selectCommand.Parameters.AddWithValue("@FName", fileName);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    result = query.GetString(1);

                   
                }
                catch(Exception ex)
                {

                }

                return result;

                //while (query.Read())
                //{
                //    entries.Add(query.GetString(1));
                //}
                //return entries;
            }
        }

        //private static NoteFile GetNoteFile(IDataRecord record)
        //{
        //    return new NoteFile(record.GetInt32(2), record.GetString(0), record.GetString(1));
        //}


    }

}
