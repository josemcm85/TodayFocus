using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel;


namespace TodayFocus.Model
{
    public class FocusTask : INotifyPropertyChanged
    {
        [BsonId] // _id
        public Guid Id { get; set; }

        private string _TaskName;

        public string TaskName
        {
            get { return _TaskName; }
            set { 
                _TaskName = value;
                OnPropertyChanged("TaskName");
            }
        }

        private DateTime _TaskDate;

        public DateTime TaskDate
        {
            get { return _TaskDate; }
            set { 
                _TaskDate = value;
                OnPropertyChanged("TaskDate");
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        public FocusTask(Guid Id, string TaskName, DateTime TaskDate)
        {
            this.Id = Id;
            this.TaskName = TaskName;
            this.TaskDate = TaskDate;
        }

        public FocusTask()
        {

        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));


        }

        public override string ToString()
        {
            return TaskName;
        }
    }
}
