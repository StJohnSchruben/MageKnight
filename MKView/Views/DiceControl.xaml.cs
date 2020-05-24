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

namespace MKView.Views
{
    /// <summary>
    /// Interaction logic for DiceControl.xaml
    /// </summary>
    public partial class DiceControl : UserControl
    {
        private Random rand = new Random();
        public DiceControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.result.Content = rand.Next(1, 7);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int i = rand.Next(1, 7);
            int j = rand.Next(1, 7);
            this.result.Content = i + j;
        }
    }
}
