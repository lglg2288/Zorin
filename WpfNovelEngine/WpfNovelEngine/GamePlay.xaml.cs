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
    /// Логика взаимодействия для GamePlay.xaml
    /// </summary>
    public partial class GamePlay : Window
    {
        private DataBase db;
        private int countPage;
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
            countPage = 0;
        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            countPage++;

            Page page;
            db.SendPage(countPage, currentStoryline, out page);

            if (page == null)
            {
                MessageBox.Show("A non-existent page index was requested.");
                this.Close();
                return;
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
    }
}
