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
    /// Логика взаимодействия для AddStoryline.xaml
    /// </summary>
    public partial class AddStoryline : Window
    {
        private DataBase db;
        public AddStoryline()
        {
            InitializeComponent();
            db = new DataBase();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            db.AddStoryline(textBoxName.Text);
            this.Close();
        }
    }
}
