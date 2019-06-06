using RealPolyclinic.HelpMethods;
using RealPolyclinic.Models;
using RealPolyclinic.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RealPolyclinic.ViewModels
{
    public class MainVM : BaseViewModel
    {
        public User CurrentUser { get; set; }
        private BaseViewModel _currentView;
        public BaseViewModel CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        private BaseViewModel _prevView;
        public BaseViewModel previousView
        {
            get { return _prevView; }
            set
            {
                _prevView = value;
                OnPropertyChanged();
            }
        }

        public ICommand gotoAdp { get; set; }
        public ICommand gotoSrch { get; set; }
        public ICommand GoToPat { get; set; }
        public ICommand PreviousPage { get; set; }
        public ICommand gotoApp { get; set; }
        public ICommand exit { get; set; }

        public MainVM()
        {
            gotoAdp = new RelayCommand(x => CurrentView = new AddPatientVM());
            gotoSrch = new RelayCommand(x => CurrentView = new SearchPatientVM());
            GoToPat = new RelayCommand(x => { CurrentView = new PatientProfileVM(Convert.ToInt32(x)); previousView = new SearchPatientVM(); }, x => HelpfulMethods.Checkdata(x));
            PreviousPage = new RelayCommand(x => CurrentView = previousView);
            gotoApp = new RelayCommand(x => CurrentView = new AppointmentVM());
            exit = new RelayCommand(x => ExitWindow(x));
        }
        private void ExitWindow(object items)
        {
            object[] parameters = items as object[];
            var currentWind = parameters[0] as Window;
            Authorization auth = new Authorization() { DataContext = new AuthorizationVM() };
            auth.Show();
            currentWind.Close();
        }
    }
}
