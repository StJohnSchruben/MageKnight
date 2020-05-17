using MKModel;
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
    /// Interaction logic for Dial.xaml
    /// </summary>
    public partial class Dial : UserControl
    {
        public Dial()
        {
            InitializeComponent();
            this.IsVisibleChanged += Dial_IsVisibleChanged;
        }

        private void Dial_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var dial = this.DataContext as IDial;
        }
    }
}
