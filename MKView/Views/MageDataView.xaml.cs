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
        }

        private void MageDataView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            IMageKnightBattleViewModel viewModel = this.DataContext as IMageKnightBattleViewModel;
            this.clicksList.ItemsSource = viewModel.Dial.Clicks;
        }

        private void Button_Click(object sender, MouseButtonEventArgs e)
        {
            Button button = sender as Button;
            string commandName = button.Name;
            IMageKnightBattleViewModel viewModel = this.DataContext as IMageKnightBattleViewModel;
            if (viewModel != null)
            {
                switch (commandName)
                {
                    case "Command":
                        viewModel.Command.Execute(null);
                        break;
                    case "Charge":
                        viewModel.Charge.Execute(null);
                        break;
                    case "Healing":
                        viewModel.Healing.Execute(null);
                        break;
                    case "Magic Levitation":
                        viewModel.MagicLevitation.Execute(null);
                        break;
                    case "Magic Blast":
                        viewModel.MagicBlast.Execute(null);
                        break;
                    case "Regeneration":
                        viewModel.Regeneration.Execute(null);
                        break;
                    case "Stealth":
                        viewModel.Stealth.Execute(null);
                        break;
                    case "Necromancy":
                        viewModel.Necromancy.Execute(null);
                        break;
                    case "WheaponMaster":
                        viewModel.WheaponMaster.Execute(null);
                        break;
                    case "Quickness":
                        viewModel.Quickness.Execute(null);
                        break;
                    case "Shockwave":
                        viewModel.ShockWave.Execute(null);
                        break;
                    case "Magic Healing":
                        viewModel.MagicHealing.Execute(null);
                        break;
                    case "Bound":
                        viewModel.Bound.Execute(null);
                        break;
                    case "Flight":
                        viewModel.Flight.Execute(null);
                        break;
                    case "Flame Lightning":
                        viewModel.FlameLightining.Execute(null);
                        break;
                }
            }
        }
    }
}
