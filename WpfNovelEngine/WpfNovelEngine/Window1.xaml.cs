using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
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
using WpfVisualNovel;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.IO.Packaging;

namespace WpfNovelEngine
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        DataBase db = new DataBase();
        
        public Window1()
        {
            InitializeComponent();
            RefreshData();
        }


        private void AddChoiсeButton(in ChoiceButton[] choices)
        {
            myBGStackPanel.Children.Clear();
            myStackPanel.Children.Clear();

            int quantity = choices.Length;

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
                        Content = choices[i].Text
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
                }
            }
        }
        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            new GamePlay().Show();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnOut_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }

        void RefreshData()
        {
            var SelectedItemStoryLine = comboBoxStoryline.SelectedItem;
            var SelectedItemPages = comboBoxPage.SelectedItem;            

            if (SelectedItemStoryLine != null)
            {
                if (SelectedItemPages != null)
                {
                    myStackPanel.Children.Clear();
                    myBGStackPanel.Children.Clear();
                    if (db.pageIsQuestion(Convert.ToInt32(SelectedItemPages), SelectedItemStoryLine.ToString()))
                    {
                        ChoiceButton[] choices;
                        db.GetChoices(Convert.ToInt32(SelectedItemPages), SelectedItemStoryLine.ToString(), out choices);
                        if (choices == null)
                            db.delQuestion(Convert.ToInt32(SelectedItemPages), SelectedItemStoryLine.ToString());
                        else
                        {
                            AddChoiсeButton(in choices);
                        }
                    }

                    Page page = new Page();
                    db.SendPage(Convert.ToInt32(SelectedItemPages), SelectedItemStoryLine.ToString(), out page);
                    if (page != null)
                    {
                        Image background = new Image
                        {
                            Width = (double)page.BGWidth,
                            Height = (double)page.BGHeight,
                            Source = new BitmapImage(new Uri(page.BGImagePath))
                        };
                        Canvas.SetTop(background, (double)page.BGPositionY);
                        Canvas.SetLeft(background, (double)page.BGPositionX);
                        CanvasGame.Children.Add(background);

                        narativePanel.Text = page.Text;
                        labelCharacter.Content = page.Character;
                    }
                }
            }

            string[] storylines;
            db.SendStorylines(out storylines);            
            comboBoxStoryline.Items.Clear();            
            for (int i = 0; i < storylines?.Length; i++)
            {
                comboBoxStoryline.Items.Add(storylines[i]);
            }

            int[] pages;
            db.SendAllPagesNums(SelectedItemStoryLine?.ToString(), out pages);
            comboBoxPage.Items.Clear();
            for (int i = 0; i < pages?.Length; i++)
            {
                comboBoxPage.Items.Add(pages[i]);
            }

            comboBoxStoryline.SelectedItem = SelectedItemStoryLine;
            comboBoxPage.SelectedItem = SelectedItemPages;
        }

        private void btnAddStoryline_Click(object sender, RoutedEventArgs e)
        {
            new AddStoryline().ShowDialog();
            RefreshData();
        }

        private void btnDelStoryline_Click(object sender, RoutedEventArgs e)
        {
            new DelStoryline().ShowDialog();
            RefreshData();
        }

        private void btnAddPage_Click(object sender, RoutedEventArgs e)
        {
            string SelectedStoryline;
            try
            {
            SelectedStoryline = comboBoxStoryline.SelectedItem.ToString();
            }
            catch
            {
                MessageBox.Show("You didn't selected StoryLine!");
                return;
            }
            int newPageNumber = db.Addpage(SelectedStoryline);
            RefreshData();
            comboBoxPage.SelectedItem = newPageNumber;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            narativePanel.Visibility = Visibility.Hidden;
        }

        private void narativePanelInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Page page = new Page();
                page.Text = textBoxnarativePanel.Text;
                db.SetPage(Convert.ToInt32(comboBoxPage.SelectedItem), comboBoxStoryline.SelectedItem.ToString(), in page);

                textBoxnarativePanel.Text = "";
                narativePanel.Visibility = Visibility.Visible;
                Keyboard.ClearFocus();
            }
        }

        private void textBoxCharacter_TextChanged(object sender, TextChangedEventArgs e)
        {
            labelCharacter.Visibility = Visibility.Hidden;
        }

        private void textBoxCharacter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Page page = new Page();
                page.Character = textBoxCharacter.Text;
                db.SetPage(Convert.ToInt32(comboBoxPage.SelectedItem), comboBoxStoryline.SelectedItem.ToString(), in page);

                textBoxCharacter.Text = "";
                labelCharacter.Visibility = Visibility.Visible;
                Keyboard.ClearFocus();
                RefreshData();
            }
        }

        private void btnDelPage_Click(object sender, RoutedEventArgs e)
        {
            db.DelPage(Convert.ToInt32(comboBoxPage?.SelectedItem), comboBoxStoryline?.SelectedItem.ToString());
            RefreshData();
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            string SelectedStoryline;
            try
            {
                SelectedStoryline = comboBoxStoryline.SelectedItem.ToString();
            }
            catch
            {
                MessageBox.Show("You didn't selected StoryLine!");
                return;
            }
            int SelectedPageNumber = Convert.ToInt32(comboBoxPage.SelectedItem);
            db.Addpage(SelectedStoryline, SelectedPageNumber);
            RefreshData();
            comboBoxPage.SelectedItem = SelectedPageNumber;
        }

        private void btnAddChoiсe_Click(object sender, RoutedEventArgs e)
        {
            new AddChoice(textBoxChoise.Text, comboBoxStoryline.SelectedItem.ToString(), Convert.ToInt32(comboBoxPage.SelectedItem)).ShowDialog();
            RefreshData();
        }

        private void btnDelChoiсe_Click(object sender, RoutedEventArgs e)
        {
            ChoiceButton[] choices;
            db.GetChoices(Convert.ToInt32(comboBoxPage.SelectedItem), comboBoxStoryline.SelectedItem.ToString(), out choices);
            new DelChoice(choices, Convert.ToInt32(comboBoxPage.SelectedItem), comboBoxStoryline.SelectedItem.ToString()).ShowDialog();
            RefreshData();
        }

        private void comboBoxStoryline_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            comboBoxStoryline.SelectionChanged -= comboBoxStoryline_SelectionChanged;

            RefreshData();

            comboBoxStoryline.SelectionChanged += comboBoxStoryline_SelectionChanged;
        }

        private void comboBoxPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            comboBoxPage.SelectionChanged -= comboBoxPage_SelectionChanged;

            RefreshData();

            comboBoxPage.SelectionChanged += comboBoxPage_SelectionChanged;
        }
    }
}