using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Xml.Linq;
using static System.Data.Entity.Infrastructure.Design.Executor;
using static System.Net.Mime.MediaTypeNames;

namespace WpfNovelEngine
{
    internal class DataBase
    {
        private SQLiteConnection DBconnect;
        private SQLiteDataReader DBdata;

        public DataBase()
        {
            DBconnect = new SQLiteConnection("Data Source=dataset\\DataBaseVisualNovel.db");
            DBconnect.Open();
        }

        public void AddStoryline(string Name)
        {
            SQLiteCommand DBCommand = new SQLiteCommand($"SELECT Name FROM Storylines WHERE Name = '{Name}';", DBconnect);
            DBdata = DBCommand.ExecuteReader();
            if (DBdata.Read())
            {
                DBdata.Close();
                MessageBox.Show($"The \"{Name}\" Storyline already exists");
                return;
            }
            else
            {
                DBdata.Close();
                DBCommand.CommandText = $"INSERT INTO Storylines (Name) VALUES ('{Name}');";
                DBdata = DBCommand.ExecuteReader();
                DBdata.Close();
                return;
            }
        }

        /// <summary>
        /// Возвращает номер созданой страницы.
        /// </summary>
        /// <returns>Номер созданой страницы.</returns>
        public int Addpage(string StorylineName)
        {
            

            SQLiteCommand DBcommand = new SQLiteCommand($"SELECT MAX(Number) FROM Pages WHERE StorylineID = (SELECT StorylineID FROM Storylines WHERE Name = '{StorylineName}');", DBconnect);
            DBdata = DBcommand.ExecuteReader();
            if (!DBdata.Read())
                MessageBox.Show("ERROOOOOOOOR OBJECT SQLiteDataReader.READ()");
            string maxPageNumber = DBdata.GetValue(0).ToString();
            DBdata.Close();

            if (maxPageNumber == "")
            {
                DBcommand.CommandText = $"INSERT INTO Pages (Number, StorylineID) VALUES (1, (SELECT StorylineID FROM Storylines WHERE Name = '{StorylineName}'));";
                DBdata = DBcommand.ExecuteReader();
                DBdata.Close();
                return 1;
            }
            else
            {
                DBcommand.CommandText = $"INSERT INTO Pages (Number, StorylineID) VALUES ({Convert.ToInt32(maxPageNumber) + 1}, (SELECT StorylineID FROM Storylines WHERE Name = '{StorylineName}'));";
                DBdata = DBcommand.ExecuteReader();
                DBdata.Close();
                return Convert.ToInt32(maxPageNumber) + 1;
            }
        }
        public void Addpage(string StorylineName, int pageNumber)
        {
            if (pageNumber == 0)
                return;
            string strConnect = "";
            strConnect += "UPDATE Pages ";
            strConnect += $"SET Number = Number + 1 ";
            strConnect += $"WHERE Number >= {pageNumber}; ";
            strConnect += "INSERT INTO Pages (Number, StorylineID) ";
            strConnect += $"VALUES ({pageNumber}, (SELECT StorylineID FROM Storylines WHERE Name = '{StorylineName}'));";

            SQLiteCommand DBcommand = new SQLiteCommand(strConnect, DBconnect);
            DBdata = DBcommand.ExecuteReader();
            DBdata.Close();
            return;
        }


        public void DelStoryline(string Name)
        {
            SQLiteCommand DBCommand = new SQLiteCommand($"SELECT Name FROM Storylines WHERE Name = '{Name}';", DBconnect);
            DBdata = DBCommand.ExecuteReader();
            if (DBdata.Read())
            {
                DBdata.Close();
                DBCommand.CommandText = $"DELETE FROM Storylines WHERE Name == '{Name}';";
                DBdata = DBCommand.ExecuteReader();
                DBdata.Close();
                return;
            }
            else
            {
                DBdata.Close();
                MessageBox.Show($"The \"{Name}\" Storyline does not exists");
                return;
            }
        }

        public void SendStorylines(out string[] storylines)
        {
            storylines = new string[0];
            SQLiteCommand DBCommand = new SQLiteCommand($"SELECT Name FROM Storylines;", DBconnect);
            DBdata = DBCommand.ExecuteReader();

            while (DBdata.Read())
            {
                push_up(ref storylines);
                storylines[storylines.Length - 1] = DBdata.GetValue(0).ToString();
            }
            if (storylines.Length == 0)
            {
                storylines = null;
                return;
            }
            return;
        }

        public void SendAllPagesNums(string Storyline, out int[] pages)
        {
            pages = new int[0];
            SQLiteCommand DBCommand = new SQLiteCommand($"SELECT Number FROM Pages WHERE StorylineID = (SELECT StorylineID FROM Storylines WHERE Name = '{Storyline}') ORDER BY Number ASC;", DBconnect);
            DBdata = DBCommand.ExecuteReader();

            while (DBdata.Read())
            {
                push_up(ref pages);
                pages[pages.Length - 1] = Convert.ToInt32(DBdata.GetValue(0));
            }
            if (pages.Length == 0)
            {
                pages = null;
                return;
            }
            return;
        }

        public void SendPage(int Number, string Storyline, out Page page)
        {
            SQLiteCommand DBcommand = new SQLiteCommand($"SELECT Character, Pages.Text , BGImagePath, BGWidth, BGHeight, BGPositionX, BGPositionY FROM Pages WHERE Number = {Number} AND StorylineID = (SELECT StorylineID FROM Storylines WHERE Name = '{Storyline}');", DBconnect);
            DBdata = DBcommand.ExecuteReader();
            if (DBdata.Read())
            {
                page = new Page()
                {
                    Character   = DBdata.GetValue(0) == DBNull.Value ? "error404" : DBdata.GetValue(0).ToString(),
                    Text        = DBdata.GetValue(1) == DBNull.Value ? "error404" : DBdata.GetValue(1).ToString(),
                    BGImagePath = DBdata.GetValue(2) == DBNull.Value ? "C:\\Zorin\\WpfNovelEngine\\WpfNovelEngine\\bin\\Debug\\dataset\\images\\error404.jpg" : DBdata.GetValue(2).ToString(),
                    BGWidth     = DBdata.GetValue(3) == DBNull.Value ? 1308       : Convert.ToInt32(DBdata.GetValue(3)),
                    BGHeight    = DBdata.GetValue(4) == DBNull.Value ? 720        : Convert.ToInt32(DBdata.GetValue(4)),
                    BGPositionX = DBdata.GetValue(5) == DBNull.Value ? 0          : Convert.ToInt32(DBdata.GetValue(5)),
                    BGPositionY = DBdata.GetValue(6) == DBNull.Value ? 0          : Convert.ToInt32(DBdata.GetValue(6))
                };
            }
            else
                page = null;

            DBdata.Close();
            return;
        }

        public void SetPage(int pageNumber, string Storyline, in Page page)
        {
            string commandStr = "UPDATE Pages SET ";
            if (page.Character   != null) commandStr += $"Character   = '{page.Character  }',";
            if (page.Text        != null) commandStr += $"Text        = '{page.Text       }',";
            if (page.BGImagePath != null) commandStr += $"BGImagePath = '{page.BGImagePath}',";
            if (page.BGWidth     != null) commandStr += $"BGWidth     = '{page.BGWidth    }',";
            if (page.BGHeight    != null) commandStr += $"BGHeight    = '{page.BGHeight   }',";
            if (page.BGPositionX != null) commandStr += $"BGPositionX = '{page.BGPositionX}',";
            if (page.BGPositionY != null) commandStr += $"BGPositionY = '{page.BGPositionY}',";
            if (commandStr[commandStr.Length - 1] == ',')
                commandStr = commandStr.Remove(commandStr.Length - 1);
            commandStr += $" WHERE Number = {pageNumber}";
            commandStr += $" AND StorylineID = (SELECT StorylineID FROM Storylines WHERE Name = '{Storyline}');";
            SQLiteCommand DBcommand = new SQLiteCommand(commandStr, DBconnect);
            DBdata = DBcommand.ExecuteReader();
            DBdata.Close();
        }

        public void DelPage(int pageNumber, string Storyline)
        {
            string dbCommand = "";
            dbCommand += "DELETE FROM Pages ";
            dbCommand += $"WHERE Number = {pageNumber} ";
            dbCommand += "AND ";
            dbCommand += $"StorylineID = (SELECT StorylineID FROM Storylines WHERE Name = '{Storyline}'); ";
            dbCommand += "UPDATE Pages ";
            dbCommand += "SET Number = Number - 1 ";
            dbCommand += $"WHERE Number >= {pageNumber};";

            SQLiteCommand DBCommand = new SQLiteCommand(dbCommand, DBconnect);
            DBdata = DBCommand.ExecuteReader();
            DBdata.Close();
            return;
        }

        /// <summary>
        /// В случае отсутствия сюжетных линий в бд может вернуть null
        /// </summary>
        /// <returns>Имя сюжетной линии с самым ранним primary key</returns>
        public string GetFirstStorylineName()
        {
            SQLiteCommand DBCommand = new SQLiteCommand($"SELECT Name FROM Storylines WHERE StorylineID = (SELECT MIN(StorylineID) FROM Storylines);", DBconnect);
            DBdata = DBCommand.ExecuteReader();

            if (DBdata.Read())
            {
                return DBdata.GetValue(0).ToString();
            }
            else
                return null;
        }

        public void AddChoice(string Storyline, int pageNumber, string choice, string NextStoryline)
        {
            SQLiteCommand DBCommand = new SQLiteCommand($"SELECT QuestionID FROM Questions WHERE PageID = {pageNumber} AND StorylineID = (SELECT StorylineID FROM Storylines WHERE Name = '{Storyline}');", DBconnect);
            DBdata = DBCommand.ExecuteReader();
            int QuestionID;

            if (DBdata.Read())
            {
                QuestionID = Convert.ToInt32(DBdata.GetValue(0));
                DBdata.Close();

                DBCommand.CommandText = $"INSERT INTO Answers(Text, NextPageID, QuestionID) VALUES('{choice}', (SELECT StorylineID FROM Storylines WHERE Name = '{NextStoryline}'), {QuestionID});";
                DBdata = DBCommand.ExecuteReader();
                DBdata.Close();
            }
            else
            {
                DBdata.Close();
                DBCommand.CommandText = $"INSERT INTO Questions (StorylineID, PageID) VALUES ((SELECT StorylineID FROM Storylines WHERE Name = '{Storyline}'), {pageNumber});";
                DBdata = DBCommand.ExecuteReader();
                DBdata.Close();

                DBCommand.CommandText = $"SELECT QuestionID FROM Questions WHERE PageID = {pageNumber} AND StorylineID = (SELECT StorylineID FROM Storylines WHERE Name = '{Storyline}');";
                DBdata = DBCommand.ExecuteReader();
                DBdata.Read();
                QuestionID = Convert.ToInt32(DBdata.GetValue(0));
                DBdata.Close();

                DBCommand.CommandText = $"INSERT INTO Answers(Text, NextPageID, QuestionID) VALUES({choice}, (SELECT StorylineID FROM Storylines WHERE Name = '{NextStoryline}'), {QuestionID});";
                DBdata = DBCommand.ExecuteReader();
                DBdata.Close();
            }

            return;
        }

        public void SendChoices(int pageNumber, string Storyline, out Dictionary<string, int> choices)
        {
            choices = null;
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="Storyline"></param>
        /// <returns>Возвращает true если страница isQuestion, иначе false</returns>
        public bool pageIsQuestion(int pageNumber, string Storyline)
        {
            SQLiteCommand DBCommand = new SQLiteCommand($"SELECT QuestionID FROM Questions WHERE PageID = (SELECT PageID FROM Pages WHERE Number = {pageNumber} AND StorylineID = (SELECT StorylineID FROM Storylines WHERE Name = '{Storyline}'))", DBconnect);
            DBdata = DBCommand.ExecuteReader();
            if(DBdata.Read())
            {
                DBdata.Close();
                return true;
            }
            else
            {
                DBdata.Close();
                return false;
            }
        }

        private void push_up<T>(ref T[] arr)
        {
            T[] newArr = new T[arr.Length + 1];
            for (int i = 0; i < arr.Length; i++)
            {
                newArr[i] = arr[i];
            }
            arr = newArr;
            return;
        }
    }
}