using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TodayFocus.ViewModel.Command
{
    public class CreateNewGridCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public MainWindowViewModel MainWindowVM { get; set; }

        public CreateNewGridCommand(MainWindowViewModel MainWindowVM)
        {
            this.MainWindowVM = MainWindowVM;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.MainWindowVM.CreateNewGridBtn();
        }
    }
}
