using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using WpfNovelEngine;

namespace WpfVisualNovel
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DialogPage currentDialogPage;
        private QuestionPage questionPage;
        private DBlogic db = new DBlogic();
        private int currentBrange = 0;
        private int currentEntry = 0;

        public MainWindow()
        {
            InitializeComponent();

            db.SendDialogPage(currentEntry, currentBrange, out currentDialogPage);

            if (currentDialogPage != null)
            {
                Image backgroundImage = new Image
                {
                    Width = 1280,
                    Height = 1280,
                    Source = new BitmapImage(new Uri(currentDialogPage.background))
                };
                Canvas.SetBottom(backgroundImage, -280);
                CanvasGame.Children.Add(backgroundImage);

                Image foregroundImage = new Image
                {
                    Width = 1280,
                    Height = 1280,
                    Source = new BitmapImage(new Uri(currentDialogPage.foreground))
                };
                CanvasGame.Children.Add(foregroundImage);
            }
        }

        private void ChoiseButton_Click(object sender, EventArgs e)
        {
            string senderStr = sender.ToString();
            string buttonName = "";
            for (int i = 32; i < senderStr.Length; i++)
            {
                buttonName += senderStr[i];
            }

            for (int i = 0; i < questionPage.questions.Length; i++)
            {
                if (questionPage.questions[i] == buttonName)
                {
                    if (questionPage.branges[i] == "null")
                    {
                        this.Close();
                        return;
                    }                    
                    currentBrange =  Convert.ToInt32(questionPage.branges[i]);
                    myStackPanel.Children.Clear();
                    myBGStackPanel.Children.Clear();
                    currentEntry = 0;
                    db.SendDialogPage(currentEntry, currentBrange, out currentDialogPage);
                    narativePanel.Content = currentDialogPage.content;
                    CharacterNamePanel.Content = currentDialogPage.character;
                    currentEntry++;
                    HandlerNarativePanel.Visibility = Visibility.Visible;
                    return;
                }
            }
            return;
        }

        private void d(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandlerNarativePanel_Click(object sender, RoutedEventArgs e)
        {
            db.SendDialogPage(currentEntry, currentBrange, out currentDialogPage);
            if (currentDialogPage == null)
            {
                HandlerNarativePanel.Visibility = Visibility.Hidden;
                db.SendQuestionsPage(currentBrange, out questionPage);
                AddChoiseButton();
                return;
            }
            narativePanel.Content = currentDialogPage.content;
            CharacterNamePanel.Content = currentDialogPage.character;

            currentEntry++;
        }

        private void AddChoiseButton()
        {
            int quantity = questionPage.questions.Length;
            Button[] ChoiseButton = new Button[quantity];
            Rectangle[] BGChoiseButton = new Rectangle[quantity];

            {
                int GapInsertButton = Convert.ToInt32(200.0 / Convert.ToDouble(quantity) + 20.0 - Math.Pow(quantity, 1.47));
                int heigth = 40;
                int width = 500;
                for (int i = 0; i < quantity; i++)
                {
                    ChoiseButton[i] = new Button()
                    {
                        Margin = new Thickness { Top = GapInsertButton },
                        Background = new SolidColorBrush(Colors.Transparent),
                        Foreground = new SolidColorBrush(Colors.White),
                        BorderBrush = new SolidColorBrush(Colors.Transparent),
                        Height = heigth,
                        Width = width,
                        FontSize = 25,
                        Content = questionPage.questions[i]
                    };

                    myStackPanel.Children.Add(ChoiseButton[i]);

                    BGChoiseButton[i] = new Rectangle()
                    {
                        Margin = new Thickness { Top = GapInsertButton },
                        Height = heigth,
                        Width = width,
                        Opacity = 0.5,
                        Fill = new SolidColorBrush(Colors.Black)
                    };

                    myBGStackPanel.Children.Add(BGChoiseButton[i]);

                    ChoiseButton[i].Click += ChoiseButton_Click;
                }
            }
        }
        private void push_up(ref int[] arr, int size)
        {
            int[] newArr = new int[size + 1];
            for (int i = 0; i < size; i++)
            {
                newArr[i] = arr[i];
            }
            arr = newArr;
            return;
        }
    }
}