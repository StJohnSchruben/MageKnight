using MKViewModel;
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
    /// Interaction logic for MageDataView.xaml
    /// </summary>
    public partial class MageDataView : UserControl
    {
        public MageDataView()
        {
            InitializeComponent();
            this.DataContextChanged += MageDataView_DataContextChanged;
            this.Loaded += MageDataView_Loaded;
        }

        private void MageDataView_Loaded(object sender, RoutedEventArgs e)
        {
            var data = this.DataContext as IUserViewModel;
        }

        private void MageDataView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            
        }
    }
}
