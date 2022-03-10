using Lib.CIS4930.Standard;
using Lib.CIS4930.Standard.Models;
using Lib.CIS4930.Standard.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UWP.CIS4930.Dialogs;
using static Lib.CIS4930.Standard.TaskManager;

namespace UWP.CIS4930.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public enum PrioritySortMode
        {
            HighToLow,
            LowToHigh,
            None,
        }

        private TaskManager taskManager;
        private ObservableCollection<ITask> tasks;

        public ObservableCollection<ITask> Tasks
        {
            get
            {
                if (tasks == null)
                    tasks = new ObservableCollection<ITask>();

                if (tasks.Count > 0)
                    tasks.Clear();

                if (!string.IsNullOrEmpty(searchString))
                {
                    // we filter by search value
                    taskManager.TaskService.Tasks
                        .Where(task =>
                            task.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                            task.Description.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                            (
                                task is Appointment appt && appt.Attendees.Any(a => a.Contains(searchString))
                            )
                        )
                        .ToList()
                        .ForEach(tasks.Add);
                }
                else
                {
                    taskManager.TaskService.Tasks.ForEach(tasks.Add);
                }

                // next, filter by todo filter mode
                if (TodoFilter == TodoFilterMode.NotComplete)
                {
                    tasks = new ObservableCollection<ITask>(
                        tasks
                            .Where(task => 
                                task is Appointment || 
                                task is ToDo && (task as ToDo).IsCompleted == false
                            )
                        );
                }

                // finally, sort by priority
                if (SortMode == PrioritySortMode.HighToLow)
                {
                    tasks = new ObservableCollection<ITask>(tasks.OrderByDescending(task => task.Priority).ToList());
                }
                else if (SortMode == PrioritySortMode.LowToHigh)
                {
                    tasks = new ObservableCollection<ITask>(tasks.OrderByDescending(task => task.Priority)
                        .Reverse()
                        .ToList());

                }

                return tasks;
            }
            set
            {
                tasks = value;
            }
        }

        private ITask selectedTask;
        public ITask SelectedTask
        {
            get { return selectedTask; }
            set
            {
                selectedTask = value;
            }
        }

        private string searchString;
        public string SearchString
        {
            get => searchString;
            set
            {
                searchString = value;
                NotifyPropertyChanged(nameof(Tasks));
            }
        }

        public IEnumerable<TodoFilterMode> TodoFilters
        {
            get => new List<TodoFilterMode>() { TodoFilterMode.All, TodoFilterMode.NotComplete };
        }

        public IEnumerable<PrioritySortMode> SortModes
        {
            get => new List<PrioritySortMode>() 
            {
                PrioritySortMode.None,
                PrioritySortMode.HighToLow, 
                PrioritySortMode.LowToHigh, 
            };
        }

        private TodoFilterMode todoFilter;
        public TodoFilterMode TodoFilter 
        {
            get => todoFilter;
            set
            {
                todoFilter = value;
                NotifyPropertyChanged(nameof(Tasks));
            }
        }

        private PrioritySortMode sortMode;
        public PrioritySortMode SortMode
        {
            get => sortMode;
            set
            {
                sortMode = value;
                NotifyPropertyChanged(nameof(Tasks));
            }
        }

        public MainPageViewModel()
        {
            taskManager = new TaskManager();
        }

        public async void Add()
        {
            var dialog = new TaskDialog(TaskDialogViewModel.DialogMode.New, taskManager);
            await dialog.ShowAsync();
            
            NotifyPropertyChanged(nameof(Tasks));
        }

        public void Delete()
        {
            var indexToDelete = tasks.IndexOf(SelectedTask);
            taskManager.Delete(indexToDelete);

            NotifyPropertyChanged(nameof(Tasks));
        }

        public async void Edit()
        {
            if (selectedTask == null)
                return;

            var dialog = new TaskDialog(TaskDialogViewModel.DialogMode.Edit, taskManager, selectedTask);
            await dialog.ShowAsync();

            NotifyPropertyChanged(nameof(Tasks));
        }

        public void ToggleComplete()
        {
            if (!(selectedTask is ToDo))
                return;

            var indexToDelete = tasks.IndexOf(SelectedTask);
            taskManager.EditIsCompleted(indexToDelete, !(selectedTask as ToDo).IsCompleted);

            NotifyPropertyChanged(nameof(Tasks));
        }

        public async void OpenNewFile()
        {
            var dialog = new OpenDialog(taskManager.TaskService);
            await dialog.ShowAsync();

            NotifyPropertyChanged(nameof(Tasks));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
