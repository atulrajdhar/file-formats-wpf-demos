#region Copyright Syncfusion Inc. 2001-2021.
// Copyright Syncfusion Inc. 2001-2021. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace syncfusion.demoscommon.wpf
{
  
    public class LaunchShowcaseDemoAction : TriggerAction<ListView>
    {
        protected override void Invoke(object parameter)
        {
            if (this.AssociatedObject == null)
                return;

            var viewmodel = this.AssociatedObject.DataContext as DemoBrowserViewModel;
            if (viewmodel == null)
                return;
            if (AssociatedObject != null && AssociatedObject.SelectedItem is DemoInfo && viewmodel != null)
            {
                try
                {
                    DemoInfo demo = AssociatedObject.SelectedItem as DemoInfo;
                    if (demo.ShowBusyIndicator)
                    {
                        viewmodel.IsShowCaseDemoBusy = true;
                    }
                    var window = Activator.CreateInstance(demo.DemoViewType) as Window;
                    viewmodel.SelectedShowcaseSample = demo;
                    AssociatedObject.SelectedItem = null;

                    DemosNavigationService.MainWindow.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        viewmodel.IsShowCaseDemoBusy = false;
                    }),
                    System.Windows.Threading.DispatcherPriority.ApplicationIdle);

                    if (window != null)
                    {
                        window.Owner = DemosNavigationService.MainWindow;
                        window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        window.Closed += Window_Closed;
                        window.ShowDialog();
                    }
                }
                catch (Exception exception)
                {
                    viewmodel.IsShowCaseDemoBusy = false;
                    ErrorLogging.LogError(exception.Message + "\n" + exception.StackTrace);
                }
            }
        }

        /// <summary>
        /// Occurs when the window is about to close. 
        /// </summary>
        private void Window_Closed(object sender, EventArgs e)
        {
            (sender as Window).Closed -= Window_Closed;
            (sender as Window).Owner = null;
        }
    }
}
