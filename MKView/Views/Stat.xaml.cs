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
    /// Interaction logic for Stat.xaml
    /// </summary>
    public partial class Stat : UserControl
    {
        public Stat()
        {
            InitializeComponent();
            this.IsVisibleChanged += Stat_IsVisibleChanged;
        }

        private void Stat_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var click = this.DataContext as IStat;
        }
    }
}
