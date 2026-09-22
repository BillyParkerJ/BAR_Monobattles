using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BAR_Monobattles
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = MainViewModel;
        }

        private MainViewModel _mainViewModel;
        public MainViewModel MainViewModel
        {
            get
            {
                if (_mainViewModel == null)
                {
                    _mainViewModel = new MainViewModel();
                }
                return _mainViewModel;
            }
            set
            {
                _mainViewModel = value;
            }
        }

        private void TypeCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TypeCB.SelectedIndex != 0)
            {
                MainViewModel.ChosenUType = (string)TypeCB.SelectedValue;
            }
        }

        private void FactionCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FactionCB.SelectedIndex != 0)
            {
                MainViewModel.ChosenFaction = (string)FactionCB.SelectedValue;
            }
        }

        private void GenBtn_Click(object sender, RoutedEventArgs e)
        {
            if (FactionCB.SelectedIndex == 0 && TypeCB.SelectedIndex == 0)
            {
                MessageBox.Show("Please select a Faction and Unittype!", "Not Allowed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (FactionCB.SelectedIndex == 0 && TypeCB.SelectedIndex != 0)
            {
                MessageBox.Show("Please select a Faction!", "No Faction selected", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (FactionCB.SelectedIndex != 0 && TypeCB.SelectedIndex == 0)
            {
                MessageBox.Show("Please select a Unittype!", "No Unit-Type Selected", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MainViewModel.RollUnit("Both");
            if(MainViewModel.unitList != null &&  MainViewModel.unitList.Count > 0)
            {
                MainViewModel.RollUnitBtnEnabled = false;
                MainViewModel.T1_RerollUnitBtnEnabled = true;
                MainViewModel.T2_RerollUnitBtnEnabled = true;
                MainViewModel.FactionCB_enabled = false;
                MainViewModel.UnitTypeCB_enabled = false;
            }
        }

        private void Reroll_T1_Btn_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.T1_rerolled = true;
            MainViewModel.RollUnit("Tier_1", true);
            MainViewModel.T1_RerollUnitBtnEnabled = false;
        }

        private void Reroll_T2_Btn_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.T2_rerolled = true;
            MainViewModel.RollUnit("Tier_2", true);
            MainViewModel.T2_RerollUnitBtnEnabled = false;
        }

        private void T1_Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            T1_Image.Source = (ImageSource)FindResource("UnitIMGNotFound.png");
        }

        private void T2_Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            T1_Image.Source = (ImageSource)FindResource("UnitIMGNotFound.png");
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.ResetAll();
            FactionCB.SelectedIndex = 0;
            TypeCB.SelectedIndex = 0;
        }
    }

}