using Lib.CIS4930.Standard.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using UWP.CIS4930.ViewModels;
using static UWP.CIS4930.ViewModels.TaskDialogViewModel;
using Lib.CIS4930.Standard;

namespace UWP.CIS4930.Dialogs
{
    public sealed partial class TaskDialog : ContentDialog
    {
        public TaskDialog(DialogMode mode, TaskManager taskManager, ITask startTask = null)
        {
            this.InitializeComponent();
            this.DataContext = new TaskDialogViewModel(mode, taskManager, startTask);
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            (DataContext as TaskDialogViewModel).SaveClick(sender, args);
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // pass
        }
    }
}
