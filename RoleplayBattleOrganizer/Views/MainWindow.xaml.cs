using RoleplayBattleOrganizer.ViewsModels;
using System.Windows;

namespace RoleplayBattleOrganizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private VMMain _vmMain;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = VMMain;
        }

        public VMMain VMMain
        {
            get
            {
                if (_vmMain == null)
                {
                    _vmMain = new VMMain();
                }
                return _vmMain;
            }
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            var s = VMMain.Save;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var s = VMMain.Save;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var l = VMMain.Load;
        }

        private void DataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            VMMain.CommandAddSavedFighterToFightersList.Execute(VMMain.SelectedSavedFighter);
        }

        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //if (btnSaveOrEditFighter.IsEnabled)
            //    btnSaveOrEditFighter.Command.Execute(null);
        }
    }
}
