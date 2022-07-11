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
        MongoCRUD db = new MongoCRUD("SimpleTasks");

        private bool _VisibilityEditGrid;

        public bool VisibilityEditGrid
        {
            get { return _VisibilityEditGrid; }
            set { _VisibilityEditGrid = value;
                OnPropertyChanged("VisibilityEditGrid");
            }
        }


        private bool _VisibilityListToday;

        
        public bool VisibilityListToday
        {
            get { return _VisibilityListToday; }
            set { 
                _VisibilityListToday = value;
                OnPropertyChanged("VisibilityListToday");
            }
        }

        private bool _VisibilityAllList;

        public bool VisibilityAllList
        {
            get { return _VisibilityAllList; }
            set { _VisibilityAllList = value;
                OnPropertyChanged("VisibilityAllList");
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

        private FocusTask _taskEdit;

        public FocusTask TaskEdit
        {
            get { return _taskEdit; }
            set { _taskEdit = value;
                OnPropertyChanged("TaskEdit");
            }
        }


        public CreateNewGridCommand CreateNewGridCommand { get; set; }
        public TodayFocusCommand TodayFocusCommand { get; set; }

        public CreateNewTaskCommand CreateNewTaskCommand { get; set; }

        public DiscardNewTaskCommand DiscardNewTaskCommand { get; set; }
        public DeleteTaskCommand DeleteTaskCommand { get; set; }
        public EditTaskCommand EditTaskCommand { get; set; }
        public SubmitChangesCommand SubmitChangesCommand { get; set; }
        public DiscardChangesCommand DiscardChangesCommand { get; set; }
        public AllTasksCommand AllTasksCommand { get; set; }


        public MainWindowViewModel()
        {
            
            this.TodayFocusCommand = new TodayFocusCommand(this);
            this.CreateNewGridCommand = new CreateNewGridCommand(this);
            this.CreateNewTaskCommand = new CreateNewTaskCommand(this);
            this.DiscardNewTaskCommand = new DiscardNewTaskCommand(this);
            this.DeleteTaskCommand = new DeleteTaskCommand(this);
            this.EditTaskCommand = new EditTaskCommand(this);
            this.SubmitChangesCommand = new SubmitChangesCommand(this);
            this.DiscardChangesCommand = new DiscardChangesCommand(this);
            this.AllTasksCommand = new AllTasksCommand(this);
            TodayFocusBtn();
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));


        }

        public void TodayFocusBtn()
        {
            
            LoadTodayTasks();
            this.VisibilityCreateNew = false;
            this.VisibilityEditGrid = false;
            this.VisibilityAllList = false;
            this.VisibilityListToday = true;
           
        }

        public void AllTasksBtn()
        {
            LoadAllTasks();
            this.VisibilityCreateNew = false;
            this.VisibilityEditGrid = false;
            this.VisibilityListToday = false;
            this.VisibilityAllList = true;
                       

        }

        public void CreateNewGridBtn()
        {
            
            this.VisibilityListToday = false;
            this.VisibilityEditGrid = false;
            this.VisibilityAllList = false;
            this.VisibilityCreateNew = true;

        }

        public void CreateNewTaskBtn()
        {
            
            FocusTask newFocusTask = new FocusTask
            {
                TaskName = NewTaskName,
                TaskDate = NewTaskDate,

            };
          
            db.InsertRecord("Tasks", newFocusTask);
                        
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

        public void LoadTodayTasks()
        {
            
            Clear();
            var Todo = db.LoadRecords<FocusTask>("Tasks").OrderBy(i => i.TaskDate);
            foreach (var item in Todo)
            {
                if(item.TaskDate.Date == DateTime.Today.Date)
                    Add(new FocusTask(item.Id, item.TaskName, item.TaskDate));                 

            }

        }

        public void LoadAllTasks()
        {
            
            Clear();
            var Todo = db.LoadRecords<FocusTask>("Tasks").OrderBy(i => i.TaskDate);
            foreach (var item in Todo)
            {                
               Add(new FocusTask(item.Id, item.TaskName, item.TaskDate));
            }

            

        }

        public void DeleteTaskBtn(FocusTask FocusTask)
        {
            db.DeleteRecord<FocusTask>("Tasks", FocusTask.Id);
            Remove(FocusTask);

        }

        public void EditTaskBtn(FocusTask FocusTask)
        {
            this.TaskEdit = FocusTask;
            this.VisibilityListToday = false;
            this.VisibilityCreateNew = false;
            this.VisibilityAllList = false;
            this.VisibilityEditGrid = true;

        }

        public void SubmitChangesBtn(FocusTask FocusTask)
        {
            db.UpsertRecord<FocusTask>("Tasks", FocusTask.Id, FocusTask);

            TodayFocusBtn();
        }

        public void DiscardChangesBtn()
        {

            TodayFocusBtn();
        }




    }

    
}
