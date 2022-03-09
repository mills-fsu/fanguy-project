using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;
using Lib.CIS4930.Standard.Models;
using static Lib.CIS4930.Standard.TaskManager;
using System;
using Lib.CIS4930.Standard;
using Windows.UI.Xaml.Media;
using Windows.UI;

namespace UWP.CIS4930.ViewModels
{
    public class TaskDialogViewModel : INotifyPropertyChanged
    {
        public enum DialogMode
        {
            New,
            Edit,
        };

        public ObservableCollection<TaskType> Types
        {
            get => new ObservableCollection<TaskType>() { TaskType.ToDo, TaskType.Appointment };
        }

        private TaskType selectedType;
        public TaskType SelectedType {
            get => selectedType;
            set
            {
                if (mode == DialogMode.Edit)
                    return;

                selectedType = value;

                var oldName = task.Name;
                var oldDescription = task.Description;

                if (selectedType == TaskType.ToDo)
                {
                    task = new ToDo()
                    {
                        Name = oldName,
                        Description = oldDescription
                    };
                }
                else
                {
                    task = new Appointment()
                    {
                        Name = oldName,
                        Description = oldDescription
                    };
                }

                NotifyPropertyChanged(nameof(SelectedType));
                NotifyPropertyChanged(nameof(ShowToDoFields));
                NotifyPropertyChanged(nameof(ShowAppointmentFields));
                NotifyPropertyChanged(nameof(Task));
            }
        }

        public bool ShowToDoFields { get => SelectedType == TaskType.ToDo; }
        public bool ShowAppointmentFields { get => SelectedType == TaskType.Appointment; }

        private ITask task;
        public ITask Task
        {
            get => task;
            set
            {
                task = value;
            }
        }

        public string Title { get; private set; }

        public bool EnableTypePicker => mode == DialogMode.New;

        private string apptAttendees;
        public string ApptAttendees 
        {
            get => apptAttendees;
            set
            {
                apptAttendees = value;

                var attendees = new List<string>(apptAttendees.Split(','));
                attendees.ForEach(a => a = a.Trim());
                (task as Appointment).Attendees = attendees;
            }
        }

        private DateTimeOffset boundToDoDueDate;
        public DateTimeOffset BoundToDoDueDate
        {
            get => boundToDoDueDate;
            set
            {
                boundToDoDueDate = value;

                (task as ToDo).Deadline = boundToDoDueDate.DateTime;
            }
        }

        private DateTimeOffset boundApptStartDate;
        public DateTimeOffset BoundApptStartDate
        {
            get => boundApptStartDate;
            set
            {
                boundApptStartDate = value;

                UpdateApptStart();
            }
        }

        private TimeSpan boundApptStartTime;
        public TimeSpan BoundApptStartTime
        {
            get => boundApptStartTime;
            set
            {
                boundApptStartTime = value;

                UpdateApptStart();
            }
        }

        private DateTimeOffset boundApptEndDate;
        public DateTimeOffset BoundApptEndDate
        {
            get => boundApptEndDate;
            set
            {
                boundApptEndDate = value;

                UpdateApptEnd();
            }
        }

        private TimeSpan boundApptEndTime;
        public TimeSpan BoundApptEndTime
        {
            get => boundApptEndTime;
            set
            {
                boundApptEndTime = value;

                UpdateApptEnd();
            }
        }

        private string errorText;
        public string ErrorText 
        {
            get => errorText;
            set
            {
                errorText = value;
                NotifyPropertyChanged(nameof(ErrorText));
            }
        }

        private TaskManager taskManager;
        private DialogMode mode;
        private int taskServiceIndex;

        public TaskDialogViewModel(DialogMode mode, TaskManager taskManager, ITask startTask)
        {
            this.taskManager = taskManager;
            this.mode = mode;

            ErrorText = "";

            if (mode == DialogMode.New)
            {
                Title = "Add Task";
                selectedType = TaskType.ToDo;
                task = new ToDo();

                boundToDoDueDate = DateTimeOffset.Now;
                boundApptStartDate = DateTimeOffset.Now;
                boundApptEndDate = DateTimeOffset.Now;
            }
            else
            {
                Title = "Edit Task";
                task = startTask;

                taskServiceIndex = taskManager.TaskService.Tasks.IndexOf(startTask);

                if (task is ToDo todo)
                {
                    selectedType = TaskType.ToDo;

                    boundToDoDueDate = todo.Deadline;

                    boundApptStartDate = DateTime.Now;
                    boundApptEndDate = DateTime.Now;
                }
                else if (task is Appointment appt)
                {
                    selectedType = TaskType.Appointment;

                    boundToDoDueDate = DateTime.Now;

                    boundApptStartDate = appt.Start;
                    boundApptStartTime = appt.Start.TimeOfDay;
                    boundApptEndDate = appt.End;
                    boundApptEndTime = appt.End.TimeOfDay;
                    apptAttendees = string.Join(",", appt.Attendees);
                }
            }
        }

        public void SaveClick(ContentDialog _sender, ContentDialogButtonClickEventArgs args)
        {
            ErrorText = "";

            if (!Validate())
            {
                args.Cancel = true;
                return;
            }

            if (mode == DialogMode.New)
            {
                taskManager.Create(Task);
            }
            else
            {
                taskManager.Update(taskServiceIndex, Task);
            }
        }

        private bool Validate()
        {
            // if the user doesn't update the priority slider, the Task's priority will
            // still be 0, which we don't want
            if (Task.Priority == 0)
            {
                Task.Priority = 1;
            }

            if (string.IsNullOrEmpty(task.Name))
            {
                ErrorText += "Name must not be empty. ";
                return false;

            }

            if (task is Appointment appt)
            {
                if (boundApptEndDate < boundApptStartDate)
                {
                    ErrorText += "End date cannot be before start date. ";
                    return false;
                }
                if (boundApptEndTime < boundApptStartTime)
                {
                    ErrorText += "End time cannot be before start time. ";
                    return false;
                }
            }

            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateApptStart()
        {
            var appt = task as Appointment;

            appt.Start = boundApptStartDate.Date;
            appt.Start = appt.Start.Add(boundApptStartTime);
        }

        private void UpdateApptEnd()
        {
            var appt = task as Appointment;

            appt.End = boundApptEndDate.Date;
            appt.End = appt.End.Add(boundApptEndTime);
        }
    }
}
