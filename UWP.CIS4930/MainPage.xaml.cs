using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP.CIS4930.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWP.CIS4930
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            DataContext = new MainPageViewModel();
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            (DataContext as MainPageViewModel).Add();
        }

        private void EditClick(object sender, RoutedEventArgs e)
        {
            (DataContext as MainPageViewModel).Edit();
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            (DataContext as MainPageViewModel).Delete();
        }

        private void CompleteClick(object sender, RoutedEventArgs e)
        {
            (DataContext as MainPageViewModel).ToggleComplete();
        }

        private void OpenNewFileClick(object sender, RoutedEventArgs e)
        {
            (DataContext as MainPageViewModel).OpenNewFile();
        }
    }
}
