using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfNovelEngine
{
    /// <summary>
    /// Логика взаимодействия для GamePlay.xaml
    /// </summary>
    public partial class GamePlay : Window
    {
        private DataBase db;
        private int countPage;
        private string Storyline;
        public GamePlay()
        {
            InitializeComponent();
            db = new DataBase();
            Storyline = db.GetFirstStorylineName();
            if (Storyline == null)
            {
                MessageBox.Show("There are no Storylines");
                this.Close();
            }
            countPage = 0;
        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            countPage++;

            Page page;
            db.SendPage(countPage, Storyline, out page);

            if (page == null)
            {
                MessageBox.Show("A non-existent page index was requested.");
                this.Close();
                return;
            }

            if (db.pageIsQuestion(countPage, Storyline))
            {
                ChoiceButton[] choices;
                db.GetChoices(countPage, Storyline, out choices);
                if (choices == null)
                    db.delQuestion(countPage, Storyline);
                else
                    AddChoiсeButton(in choices);
            }

            Image background = new Image
            {
                Width = (double)page.BGWidth,
                Height = (double)page.BGHeight,
                Source = new BitmapImage(new Uri(page.BGImagePath))
            };
            Canvas.SetTop(background, (double)page.BGPositionY);
            Canvas.SetLeft(background, (double)page.BGPositionX);
            CanvasGame.Children.Add(background);

            labelDialog.Content = page.Text;
            labelCharacterName.Content = page.Character;
        }

        private void AddChoiсeButton(in ChoiceButton[] choices)
        {
            myBGStackPanel.Children.Clear();
            myStackPanel.Children.Clear();

            int quantity = choices.Length;

            Button[] choiceButton = new Button[quantity];
            Rectangle[] BGChoiceButton = new Rectangle[quantity];

            {
                int GapInsertButton = Convert.ToInt32(200.0 / Convert.ToDouble(quantity) + 20.0 - Math.Pow(quantity, 1.47));
                int heigth = 40;
                int width = 500;
                for (int i = 0; i < quantity; i++)
                {
                    choiceButton[i] = new Button()
                    {
                        Margin = new Thickness { Top = GapInsertButton },
                        Background = new SolidColorBrush(Colors.Transparent),
                        Foreground = new SolidColorBrush(Colors.White),
                        BorderBrush = new SolidColorBrush(Colors.Transparent),
                        Height = heigth,
                        Width = width,
                        FontSize = 25,
                        Content = choices[i].Text
                    };

                    myStackPanel.Children.Add(choiceButton[i]);

                    BGChoiceButton[i] = new Rectangle()
                    {
                        Margin = new Thickness { Top = GapInsertButton },
                        Height = heigth,
                        Width = width,
                        Opacity = 0.5,
                        Fill = new SolidColorBrush(Colors.Black)
                    };

                    myBGStackPanel.Children.Add(BGChoiceButton[i]);

                    choiceButton[i].Click += choiceButton_Click;
                }
            }
        }

        private void choiceButton_Click(object sender, EventArgs e)
        {
            ChoiceButton[] choices;
            string senderStr = sender.ToString();
            string buttonName = "";
            
            for (int i = 32; i < senderStr.Length; i++)
            {
                buttonName += senderStr[i];
            }
            db.GetChoices(countPage, Storyline, out choices);

            for (int i = 0; i < choices.Length; i++)
            {
                if (choices[i].Text == buttonName)
                {
                    if (choices[i].StorylineID == -1)
                    {
                        this.Close();
                        return;
                    }                    
                    //SQLiteDataReader dataReader = db.custom($"SELECT Name TEXT FROM Storylines WHERE StorylineID = {choices[i].StorylineID}");
                    //if (dataReader.Read())
                    //    Storyline = dataReader.GetValue(0);
                    //myStackPanel.Children.Clear();
                    //myBGStackPanel.Children.Clear();
                    //currentEntry = 0;
                    //db.SendDialogPage(currentEntry, currentBrange, out currentDialogPage);
                    //narativePanel.Content = currentDialogPage.content;
                    //CharacterNamePanel.Content = currentDialogPage.character;
                    //currentEntry++;
                    //HandlerNarativePanel.Visibility = Visibility.Visible;
                    return;
                }
            }
            return;
        }
    }
}
