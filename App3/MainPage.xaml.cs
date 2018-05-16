using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace App3
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private event EventHandler test;
        public MainPage()
        {
            this.InitializeComponent();
            test += MainPage_test;
        }

        private async void MainPage_test(object sender, EventArgs e)
        {
            DoLongRunningTaskAsync();
        }

        Task task;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            test?.Invoke(this, EventArgs.Empty);
        }

        void DoLongRunningTaskAsync()
        {
            if ((task != null) && (task.IsCompleted == false
                || task.Status == TaskStatus.Running
                || task.Status == TaskStatus.WaitingToRun
                || task.Status == TaskStatus.WaitingForActivation))
            {
                Debug.WriteLine($"{DateTime.Now.ToString()} -> Task canceled");
            }
            else
            {
                task = Task.Factory.StartNew(async () =>
                {
                    Debug.WriteLine($"{DateTime.Now.ToString()} -> Task has been started");

                    await Task.Delay(TimeSpan.FromSeconds(5));
                })
                .Unwrap()
                .ContinueWith((a) =>
                {
                    if (a.IsCompleted)
                    {
                        Debug.WriteLine($"{DateTime.Now.ToString()} -> Task has been completed");
                    }
                });
            }
        }


        public static async Task RunOnUI(Action action)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                action();
            });
        }
    }
}
