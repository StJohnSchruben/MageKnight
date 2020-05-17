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
    /// Interaction logic for BattleGround.xaml
    /// </summary>
    public partial class BattleGround : UserControl
    {
        public BattleGround()
        {
            InitializeComponent();
            this.IsVisibleChanged += BattleGround_IsVisibleChanged;
        }

        private void BattleGround_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var user = this.DataContext as IUser;
           // int count = this.models.Items.Count;
        }
    }
}
