using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TodayFocus.ViewModel.Command;
using TodayFocus.Model;

namespace TodayFocus.ViewModel
{
    public class MainWindowViewModel : ObservableCollection<FocusTask>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        private bool _VisibilityListToday;
              

        public bool VisibilityListToday
        {
            get { return _VisibilityListToday; }
            set { 
                _VisibilityListToday = value;
                OnPropertyChanged("VisibilityListToday");
            }
        }

        private bool _VisibilityCreateNew;

        public bool VisibilityCreateNew
        {
            get { return _VisibilityCreateNew; }
            set { 
                _VisibilityCreateNew = value;
                OnPropertyChanged("VisibilityCreateNew");
            }
        }

        private DateTime _newTaskDate = DateTime.Today;
        private string _newTaskName = string.Empty;


        public DateTime NewTaskDate
        {
            get
            {
                return _newTaskDate;
            }
            set
            {
                _newTaskDate = value;
                OnPropertyChanged("NewTaskDate");
            }
        }

        public string NewTaskName
        {
            get { return _newTaskName; }
            set
            {
                _newTaskName = value;
                OnPropertyChanged("NewTaskName");
            }
        }

        public CreateNewGridCommand CreateNewGridCommand { get; set; }
        public TodayFocusCommand TodayFocusCommand { get; set; }

        public CreateNewTaskCommand CreateNewTaskCommand { get; set; }

        public DiscardNewTaskCommand DiscardNewTaskCommand { get; set; }

        public MainWindowViewModel()
        {
            MongoCRUD db = new MongoCRUD("SimpleTasks");
            this.TodayFocusCommand = new TodayFocusCommand(this);
            this.CreateNewGridCommand = new CreateNewGridCommand(this);
            this.CreateNewTaskCommand = new CreateNewTaskCommand(this);
            this.DiscardNewTaskCommand = new DiscardNewTaskCommand(this);

            var Todo = db.LoadRecords<FocusTask>("Tasks");
            foreach (var item in Todo)
            {
                Add(new FocusTask(item.Id, item.TaskName, item.TaskDate));
                Debug.WriteLine(item.TaskName);

            }

        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));


        }

        public void TodayFocusBtn()
        {
                   
            this.VisibilityCreateNew = false;
            this.VisibilityListToday = true;
           
        }

        public void CreateNewGridBtn()
        {
            
            this.VisibilityListToday = false;
            this.VisibilityCreateNew = true;

        }

        public void CreateNewTaskBtn()
        {
            MongoCRUD db = new MongoCRUD("SimpleTasks");
            FocusTask newFocusTask = new FocusTask
            {
                TaskName = NewTaskName,
                TaskDate = NewTaskDate,

            };
          
            db.InsertRecord("Tasks", newFocusTask);
            Add(newFocusTask);
            NewTaskName = "";
            NewTaskDate = DateTime.Today;
            TodayFocusBtn();
        }

        public void DiscardNewTaskBtn()
        {
            NewTaskName = "";
            NewTaskDate = DateTime.Today;
            TodayFocusBtn();
        }

    }

    
}
