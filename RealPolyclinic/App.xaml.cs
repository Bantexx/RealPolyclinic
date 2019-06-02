using RealPolyclinic.Models;
using RealPolyclinic.ViewModels;
using RealPolyclinic.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RealPolyclinic
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {       
        AuthorizationVM Avm = new AuthorizationVM();
        public Authorization AuthWin;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //AuthWin = new Authorization() { DataContext = Avm };
            //AuthWin.Show();
            MainWindow mw = new MainWindow() { DataContext = new MainVM() };
            mw.Show();
        }
    }
}
