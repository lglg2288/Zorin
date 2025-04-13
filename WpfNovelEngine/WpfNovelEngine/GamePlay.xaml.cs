using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
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
        private int currentPage;
        private string currentStoryline;
        public GamePlay()
        {
            InitializeComponent();
            db = new DataBase();
            currentStoryline = db.GetFirstStorylineName();
            if (currentStoryline == null)
            {
                MessageBox.Show("There are no Storylines");
                this.Close();
            }
            currentPage = 0;
            MusicManager.StopAll();
        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            currentPage++;

            MusicManager.init(currentStoryline, currentPage, false);

            btnNextPage.Visibility = Visibility.Visible;
            Page page;
            db.SendPage(currentPage, currentStoryline, out page);

            if (page == null)
            {
                MessageBox.Show($"A non-existent page Id was requested. (number = {currentPage}, storyline = {currentStoryline})");
                this.Close();
                return;
            }



            if (db.pageIsQuestion(currentPage, currentStoryline))
            {
                ChoiceButton[] choices;
                db.GetChoices(currentPage, currentStoryline, out choices);
                if (choices == null)
                    db.delQuestion(currentPage, currentStoryline);
                else
                    AddChoiсeButton(in choices);
                btnNextPage.Visibility = Visibility.Hidden;
            }

            Image background;
            try
            {
                background = new Image
                {
                    Width = (double)page.BGWidth,
                    Height = (double)page.BGHeight,
                    Source = new BitmapImage(new Uri(page.BGImagePath))
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                background = new Image
                {
                    Width = (double)page.BGWidth,
                    Height = (double)page.BGHeight,
                    Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\dataset\images\error404.jpg"))
                };
            }
            Canvas.SetTop(background, (double)page.BGPositionY);
            Canvas.SetLeft(background, (double)page.BGPositionX);
            CanvasBG.Children.Clear();
            CanvasBG.Children.Add(background);

            LoadForegroundObjects(currentStoryline, currentPage);

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
            db.GetChoices(currentPage, currentStoryline, out choices);

            for (int i = 0; i < choices.Length; i++)
            {
                if (choices[i].Text == buttonName)
                {
                    if (choices[i].StorylineID == -1)
                    {
                        this.Close();
                        return;
                    }
                    currentStoryline = db.GetStoryline(choices[i].StorylineID);
                    currentPage = db.GetPageNumber(choices[i].StorylineID);

                    myStackPanel.Children.Clear();
                    myBGStackPanel.Children.Clear();

                    btnNextPage_Click(null, null);
                    return;
                }
            }
            return;
        }

        private void LoadForegroundObjects(string storyline, int pageNumber)
        {
            db.GetForegroungObjects(storyline, pageNumber, out FGObject[] fgObjects);

            CanvasFG.Children.Clear();

            if (fgObjects == null || fgObjects.Length == 0)
                return;

            foreach (var fgObject in fgObjects)
            {
                Image fgSprite = CreateImage(fgObject);
                Canvas.SetTop(fgSprite, (double)fgObject.FGPositionY);
                Canvas.SetLeft(fgSprite, (double)fgObject.FGPositionX);
                CanvasFG.Children.Add(fgSprite);
            }
        }

        private Image CreateImage(FGObject fgObject)
        {
            try
            {
                return new Image
                {
                    Width = (double)fgObject.FGWidth,
                    Height = (double)fgObject.FGHeight,
                    Source = new BitmapImage(new Uri(fgObject.FGImagePath))
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки изображения: {fgObject.FGImagePath}\n{ex.Message}");

                return new Image
                {
                    Width = (double)fgObject.FGWidth,
                    Height = (double)fgObject.FGHeight,
                    Source = new BitmapImage(new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"dataset\images\error404.jpg")))
                };
            }
        }

    }
}
