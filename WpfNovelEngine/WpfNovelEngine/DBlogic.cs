using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WpfNovelEngine;

namespace WpfVisualNovel
{
    internal class DBlogic
    {
        private SQLiteConnection DBconnect;
        private SQLiteDataReader DBdata;
        private string strBuffer;
        public string StrBuffer
        {
            get { return strBuffer; }
        }


        public string DataBuffer
        {
            get { return DBdata.GetString(0); }
        }

        public DBlogic()
        {
            DBconnect = new SQLiteConnection("Data Source=dataset\\VisualNovelDataBase.db");
            DBconnect.Open();
        }

        public void SendDialogPage(int Entry, int Brange, out DialogPage dialogPage)
        {
            SQLiteCommand DBcommand = new SQLiteCommand($"SELECT character,content,background,foreground FROM DIALOG_{Brange} WHERE serial_num == {Entry};", DBconnect);
            DBdata = DBcommand.ExecuteReader();
            if (DBdata.Read())
            {
                dialogPage = new DialogPage();
                dialogPage.character =  DBdata.GetValue(0).ToString();
                dialogPage.content =    DBdata.GetValue(1).ToString();
                dialogPage.background = DBdata.GetValue(2).ToString();
                dialogPage.foreground = DBdata.GetValue(3).ToString();
            }
            dialogPage = null;
        }

        public QuestionPage SendQuestionsPage(int Brange, out QuestionPage questionPage)
        {
            questionPage = new QuestionPage();

            SQLiteCommand DBcommand = new SQLiteCommand($"SELECT MAX(serial_num) FROM DIALOG_{Brange};", DBconnect);
            DBdata = DBcommand.ExecuteReader();
            DBdata.Read();
            int Entry = Convert.ToInt32(DBdata.GetValue(0));
            DBdata.Close();

            DBcommand = new SQLiteCommand($"SELECT character,content,background,foreground FROM DIALOG_{Brange} WHERE serial_num == {Entry};", DBconnect);
            DBdata = DBcommand.ExecuteReader();
            if (DBdata.Read())
            {
                questionPage.character = DBdata.GetValue(0).ToString();
                questionPage.content = DBdata.GetValue(1).ToString();
                questionPage.background = DBdata.GetValue(2).ToString();
                questionPage.foreground = DBdata.GetValue(3).ToString();
            }
            DBdata.Close();


            DBcommand = new SQLiteCommand($"SELECT content FROM QUESTION_{Brange};", DBconnect);
            try { DBdata = DBcommand.ExecuteReader(); }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }

            int rowsCount = 0;
            string[] questions = new string[rowsCount];

            while (DBdata.Read())
            {
                push_up(ref questions, ref rowsCount);
                questions[rowsCount - 1] = DBdata.GetValue(0).ToString();
            }
            questionPage.questions = questions;
            DBdata.Close();


            DBcommand = new SQLiteCommand($"SELECT brange FROM QUESTION_{Brange};", DBconnect);

            try { DBdata = DBcommand.ExecuteReader(); }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }

            rowsCount = 0;
            string[] branges = new string[rowsCount];

            while (DBdata.Read())
            {
                push_up(ref branges, ref rowsCount);
                branges[rowsCount - 1] = DBdata.GetValue(0).ToString();
            }
            questionPage.branges = branges;

            return questionPage;
        }

        private void push_up(ref string[] arr, ref int size)
        {
            string[] newArr = new string[size + 1];
            for (int i = 0; i < size; i++)
            {
                newArr[i] = arr[i];
            }
            arr = newArr;
            size++;
            return;
        }
    }
}
