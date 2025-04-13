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
                    BGPositionX = DBdata.GetValue(5) == DBNull.Value ? -14        : Convert.ToInt32(DBdata.GetValue(5)),
                    BGPositionY = DBdata.GetValue(6) == DBNull.Value ? 0          : Convert.ToInt32(DBdata.GetValue(6))
                };
            }
            else
                page = null;

            DBdata.Close();
            return;
        }

        public void AddForegroundObject(string Storyline, int PageNumber, ref FGObject Fg)
        {
            if (PageNumber == 0)
                return;
            string strConnect = "";
            strConnect += $"INSERT INTO PageCharacters(PageID) "
                        + $"VALUES "
                        + $"( "
                        + $"    ( "
                        + $"        SELECT PageID FROM Pages WHERE StorylineID = (SELECT StorylineID FROM Storylines WHERE Name = '{Storyline}') "
                        + $"        AND "
                        + $"        Number = {PageNumber} "
                        + $"    ) "
                        + $")";

            SQLiteCommand DBcommand = new SQLiteCommand(strConnect, DBconnect);
            DBdata = DBcommand.ExecuteReader();
            DBdata.Close();

            DBcommand.CommandText = "SELECT MAX(PageCharacterID) FROM PageCharacters;";
            DBdata = DBcommand.ExecuteReader();
            DBdata.Read();
            Fg.ID = Convert.ToInt32(DBdata.GetValue(0));

            SetForegroundObject(in Fg);
            return;
        }

        public void SetForegroundObject(in FGObject Fg)
        {
            string commandStr = "UPDATE PageCharacters SET ";
            if (Fg.FGImagePath != null) commandStr += $"CharacterImagePath = '{Fg.FGImagePath}',"; 
            if (Fg.FGWidth     != null) commandStr += $"Width              = {Fg.FGWidth    },"; else commandStr += "Width     = NULL,";
            if (Fg.FGHeight    != null) commandStr += $"Height             = {Fg.FGHeight   },"; else commandStr += "Height    = NULL,";
            if (Fg.FGPositionX != null) commandStr += $"PositionX          = {Fg.FGPositionX},"; else commandStr += "PositionX = NULL,";
            if (Fg.FGPositionY != null) commandStr += $"PositionY          = {Fg.FGPositionY},"; else commandStr += "PositionY = NULL,";
            if (commandStr[commandStr.Length - 1] == ',')
                commandStr = commandStr.Remove(commandStr.Length - 1);
            commandStr += $" WHERE PageCharacterID = {Fg.ID}";

            SQLiteCommand DBcommand = new SQLiteCommand(commandStr, DBconnect);
            DBdata = DBcommand.ExecuteReader();
            DBdata.Close();
        }

        public void GetForegroungObjects(string Storyline, int PageNumber, out FGObject[] FGs)
        {
            SQLiteCommand DBCommand = new SQLiteCommand(
                $"SELECT CharacterImagePath, Width, Height, PositionX, PositionY, PageCharacterID FROM PageCharacters WHERE PageID = " +
                $"( SELECT PageID FROM Pages WHERE Number = {PageNumber} AND StorylineID = (SELECT StorylineID FROM Storylines WHERE Name = '{Storyline}') );",
                DBconnect
                );
            DBdata = DBCommand.ExecuteReader();

            FGs = new FGObject[0];

            while (DBdata.Read())
            {
                push_up(ref FGs);
                FGs[FGs.Length - 1] = new FGObject()
                {
                    FGImagePath = DBdata.GetValue(0) == DBNull.Value ? "C:\\Zorin\\WpfNovelEngine\\WpfNovelEngine\\bin\\Debug\\dataset\\images\\error404.jpg" : DBdata.GetValue(0).ToString(),
                    FGWidth     = DBdata.GetValue(1) == DBNull.Value ? 1308 : Convert.ToInt32(DBdata.GetValue(1)),
                    FGHeight    = DBdata.GetValue(2) == DBNull.Value ? 720  : Convert.ToInt32(DBdata.GetValue(2)),
                    FGPositionX = DBdata.GetValue(3) == DBNull.Value ? -14  : Convert.ToInt32(DBdata.GetValue(3)),
                    FGPositionY = DBdata.GetValue(4) == DBNull.Value ? 0    : Convert.ToInt32(DBdata.GetValue(4)),
                    ID          = Convert.ToInt32(DBdata.GetValue(5)),
                };
            }
            DBdata.Close();
            if (FGs.Length <= 0)
                FGs = null;
            return;
        }

        public void DelForegroundObject(in FGObject Fg)
        {
            string commandStr = $"DELETE FROM PageCharacters WHERE PageCharacterID = {Fg.ID}";

            SQLiteCommand DBcommand = new SQLiteCommand(commandStr, DBconnect);
            DBdata = DBcommand.ExecuteReader();
            DBdata.Close();
        }

        public void SetPage(int pageNumber, string Storyline, in Page page)
        {
            string commandStr = "UPDATE Pages SET ";
            if (page.Character   != null) commandStr += $"Character   = '{page.Character  }',";
            if (page.Text        != null) commandStr += $"Text        = '{page.Text       }',";
            if (page.BGImagePath != null) commandStr += $"BGImagePath = '{page.BGImagePath}',"; 
            if (page.BGWidth     != null) commandStr += $"BGWidth     = '{page.BGWidth    }',"; else commandStr += "BGWidth     = NULL,";
            if (page.BGHeight    != null) commandStr += $"BGHeight    = '{page.BGHeight   }',"; else commandStr += "BGHeight    = NULL,";
            if (page.BGPositionX != null) commandStr += $"BGPositionX = '{page.BGPositionX}',"; else commandStr += "BGPositionX = NULL,";
            if (page.BGPositionY != null) commandStr += $"BGPositionY = '{page.BGPositionY}',"; else commandStr += "BGPositionY = NULL,";
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
            if (pageNumber == 0)
            {
                MessageBox.Show("Page do not selected!");
                return;
            }

            string dbCommand = "";
            dbCommand += "DELETE FROM Pages ";
            dbCommand += $"WHERE Number = {pageNumber} ";
            dbCommand += "AND ";
            dbCommand += $"StorylineID = (SELECT StorylineID FROM Storylines WHERE Name = '{Storyline}'); ";
            dbCommand += "UPDATE Pages ";
            dbCommand += "SET Number = Number - 1 ";
            dbCommand += $"WHERE Number > {pageNumber};";

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

        public void AddChoice(string Storyline, int pageNumber, string choice, string NextStoryline, int NextPage)
        {
            SQLiteCommand DBCommand = new SQLiteCommand($"SELECT QuestionID FROM Questions WHERE PageID = ( SELECT PageID FROM Pages WHERE Number = {pageNumber} AND StorylineID = (SELECT StorylineID FROM Storylines WHERE Name = '{Storyline}')  );", DBconnect);
            DBdata = DBCommand.ExecuteReader();
            int QuestionID;

            if (DBdata.Read())
            {
                QuestionID = Convert.ToInt32(DBdata.GetValue(0));
                DBdata.Close();
                //   \/ \/ \/
                DBCommand.CommandText = $"INSERT INTO Answers(Text, NextPageID, QuestionID) VALUES('{choice}', (SELECT PageID FROM Pages WHERE Number = {NextPage} AND StorylineID = (SELECT StorylineID FROM Storylines WHERE Name = '{NextStoryline}')), {QuestionID});";
                DBdata = DBCommand.ExecuteReader();
                DBdata.Close();
            }
            else
            {
                DBdata.Close();
                DBCommand.CommandText = $"INSERT INTO Questions (PageID) VALUES ( ( SELECT PageID FROM Pages WHERE Number = {pageNumber} AND StorylineID = (SELECT StorylineID FROM Storylines WHERE Name = '{Storyline}')  ) );";
                DBdata = DBCommand.ExecuteReader();
                DBdata.Close();

                DBCommand.CommandText = $"SELECT QuestionID FROM Questions WHERE PageID = ( SELECT PageID FROM Pages WHERE Number = {pageNumber} AND StorylineID = (SELECT StorylineID FROM Storylines WHERE Name = '{Storyline}')  );";
                DBdata = DBCommand.ExecuteReader();
                DBdata.Read();
                QuestionID = Convert.ToInt32(DBdata.GetValue(0));
                DBdata.Close();

                DBCommand.CommandText = $"INSERT INTO Answers(Text, NextPageID, QuestionID) VALUES('{choice}', (SELECT PageID FROM Pages WHERE Number = {NextPage} AND StorylineID = (SELECT StorylineID FROM Storylines WHERE Name = '{NextStoryline}')), {QuestionID});";
                DBdata = DBCommand.ExecuteReader();
                DBdata.Close();
            }

            return;
        }
        public void GetChoices(int pageNumber, string Storyline, out ChoiceButton[] choices)
        {
            SQLiteCommand DBCommand = new SQLiteCommand($"SELECT Text, NextPageID FROM Answers WHERE QuestionID = (SELECT QuestionID FROM Questions WHERE PageID = (SELECT PageID FROM Pages WHERE Number = {pageNumber} AND StorylineID = (SELECT StorylineID FROM Storylines WHERE Name = '{Storyline}')));", DBconnect);
            DBdata = DBCommand.ExecuteReader();

            choices = new ChoiceButton[0];            

            while (DBdata.Read())
            {
                push_up(ref choices);
                choices[choices.Length - 1] = new ChoiceButton(DBdata.GetValue(0).ToString(), Convert.ToInt32(DBdata.GetValue(1) == DBNull.Value ? -1 : DBdata.GetValue(1)));
            }
            DBdata.Close();
            if (choices.Length <= 0)
                choices = null;
            return;
        }

        /// <summary>
        /// удаляет свойство страницы Question если все варианты ответа удалены
        /// </summary>
        /// <param name="Storyline"></param>
        /// <param name="pageNumber"></param>
        /// <param name="answer"></param>
        public void DelChoice(string Storyline, int pageNumber, string answer)
        {
            SQLiteCommand DBCommand = new SQLiteCommand($"SELECT QuestionID FROM Questions WHERE PageID = (SELECT PageID FROM Pages WHERE Number = {pageNumber} AND StorylineID = (SELECT StorylineID FROM Storylines WHERE Name = '{Storyline}'));", DBconnect);
            DBdata = DBCommand.ExecuteReader();
            int QuestionID;

            if (DBdata.Read())
            {
                QuestionID = Convert.ToInt32(DBdata.GetValue(0));
                DBdata.Close();

                DBCommand.CommandText = $"DELETE FROM Answers WHERE QuestionID = {QuestionID} AND Text = '{answer}'";
                DBdata = DBCommand.ExecuteReader();
                DBdata.Close();

                //удаляем свойство страницы Question если все варианты ответа удалены
                DBCommand.CommandText = $"SELECT QuestionID FROM Answers";
                DBdata = DBCommand.ExecuteReader();
                if (!DBdata.Read())
                    delQuestion(pageNumber, Storyline);
            }
            else
            {
                DBdata.Close();                
            }

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
            SQLiteCommand DBCommand = new SQLiteCommand($"SELECT QuestionID FROM Questions WHERE PageID = "
                + $"("
                + $"SELECT PageID FROM Pages WHERE Number = {pageNumber} "
                + $"AND StorylineID = (SELECT StorylineID FROM Storylines WHERE Name = '{Storyline}')"
                + $")", DBconnect);
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

        public void delQuestion(int pageNumber, string Storyline)
        {
            SQLiteCommand DBCommand = new SQLiteCommand($"DELETE FROM Questions WHERE PageID = "
                + $"("
                + $"SELECT PageID FROM Pages WHERE Number = {pageNumber} "
                + $"AND StorylineID = (SELECT StorylineID FROM Storylines WHERE Name = '{Storyline}')"
                + $")", DBconnect);
            DBdata = DBCommand.ExecuteReader();
            DBdata.Close();
            return;
        }

        public string GetStoryline(int PageID)
        {
            SQLiteCommand DBCommand = new SQLiteCommand($"SELECT Name FROM Storylines WHERE StorylineID = (SELECT StorylineID FROM Pages WHERE PageID = {PageID});", DBconnect);
            DBdata = DBCommand.ExecuteReader();
            if (DBdata.Read())
            {
                string strStoryline = DBdata.GetValue(0).ToString();
                DBdata.Close();
                return strStoryline;
            }
            else
            {
                DBdata.Close();
                return null;
            }
        }

        public int GetPageNumber(int PageID)
        {
            SQLiteCommand DBCommand = new SQLiteCommand($"SELECT Number FROM Pages WHERE PageID = {PageID};", DBconnect);
            DBdata = DBCommand.ExecuteReader();
            if (DBdata.Read())
            {
                int PageNumber = Convert.ToInt32(DBdata.GetValue(0));
                DBdata.Close();
                return PageNumber;
            }
            else
            {
                DBdata.Close();
                return 0;
            }
        }
        public void AddMusic(in MusicEntry music)
        {
            string strCommand = "INSERT INTO Musics (Path, ActionType, MusicType, Volume, PageID) VALUES (@Path, @ActionType, @MusicType, @Volume, @PageID);";
            using (var command = new SQLiteCommand(strCommand, DBconnect))
            {
                command.Parameters.AddWithValue("@Path", music.Path);
                command.Parameters.AddWithValue("@ActionType", music.ActionType);
                command.Parameters.AddWithValue("@MusicType", music.MusicType);
                command.Parameters.AddWithValue("@Volume", music.Volume);
                command.Parameters.AddWithValue("@PageID", music.PageID);

                command.ExecuteNonQuery();
            }
        }
        public List<MusicEntry> GetMusicsForPage(int pageId)
        {
            List<MusicEntry> musics = new List<MusicEntry>();

            string query = "SELECT ID, Path, ActionType, MusicType, Volume, PageID FROM Musics WHERE PageID = @PageID;";
            using (var command = new SQLiteCommand(query, DBconnect))
            {
                command.Parameters.AddWithValue("@PageID", pageId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        musics.Add(new MusicEntry
                        {
                            ID = reader.GetInt32(0),
                            Path = reader.GetString(1),
                            ActionType = reader.GetString(2),
                            MusicType = reader.GetString(3),
                            Volume = reader.GetInt32(4),
                            PageID = reader.GetInt32(5)
                        });
                    }
                }
            }

            return musics;
        }

        public void UpdateMusic(MusicEntry music)
        {
            string query = @"
                            UPDATE Musics 
                            SET Path = @Path, 
                                ActionType = @ActionType, 
                                MusicType = @MusicType, 
                                Volume = @Volume, 
                                PageID = @PageID
                            WHERE ID = @ID;";

            using (var command = new SQLiteCommand(query, DBconnect))
            {
                command.Parameters.AddWithValue("@Path", music.Path);
                command.Parameters.AddWithValue("@ActionType", music.ActionType);
                command.Parameters.AddWithValue("@MusicType", music.MusicType);
                command.Parameters.AddWithValue("@Volume", music.Volume);
                command.Parameters.AddWithValue("@PageID", music.PageID);
                command.Parameters.AddWithValue("@ID", music.ID);

                command.ExecuteNonQuery();
            }
        }

        public int? GetPageId(string Storyline, int PageNum)
        {
            string strCommand = "SELECT PageID FROM Pages WHERE Number = @PageNum AND StorylineID = (SELECT StorylineID FROM Storylines WHERE Name = @Storyline);";

            using (var command = new SQLiteCommand(strCommand, DBconnect))
            {
                command.Parameters.AddWithValue("@PageNum", PageNum);
                command.Parameters.AddWithValue("@Storyline", Storyline);

                var getedData = command.ExecuteScalar();

                if (getedData == null || getedData is DBNull)
                    return null;

                return Convert.ToInt32(getedData); ;
            }
        }
        public SQLiteDataReader custom(string SQLcommand)
        {
            SQLiteCommand DBCommand = new SQLiteCommand(SQLcommand, DBconnect);
            SQLiteDataReader dataReader = DBCommand.ExecuteReader();
            return dataReader;
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