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
using System.Windows.Shapes;

namespace WpfNovelEngine
{
    /// <summary>
    /// Логика взаимодействия для AddChoice.xaml
    /// </summary>
    public partial class AddChoice : Window
    {
        private DataBase db;
        private string Answer;
        private string currentStoryline;
        private int currentPage;
        public AddChoice(string Answer, string currentStoryline, int currentPage)
        {
            InitializeComponent();
            db = new DataBase();
            RefreshData();
            this.Answer = Answer;
            this.currentStoryline = currentStoryline;
            this.currentPage = currentPage;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            db.AddChoice(currentStoryline, currentPage, Answer, comboBoxStoryline.SelectedItem.ToString(), Convert.ToInt32(comboBoxPage.SelectedItem));
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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

        void RefreshData()
        {
            var SelectedItemStoryLine = comboBoxStoryline.SelectedItem;
            var SelectedItemPages = comboBoxPage.SelectedItem;
            
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
    }
}
