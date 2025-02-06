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
    /// Логика взаимодействия для DelChoice.xaml
    /// </summary>
    public partial class DelChoice : Window
    {
        private DataBase db;
        private int pageNumber;
        private string Storyline;

        public DelChoice(ChoiceButton[] choices, int pageNumber, string Storyline)
        {
            InitializeComponent();
            db = new DataBase();
            for (int i = 0; i < choices.Length; i++)
            {
                comboBoxChoice.Items.Add(choices[i].Text);
            }
            this.pageNumber = pageNumber;
            this.Storyline = Storyline;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            db.DelChoice(Storyline, pageNumber, comboBoxChoice.SelectedItem.ToString());
            this.Close();
        }
    }
}
