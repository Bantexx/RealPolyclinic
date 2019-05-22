using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RealPolyclinic.Models
{
    public class Doctor : INotifyPropertyChanged
    {
        private int id_Doc;
        private string _fullname;
        private int _spec;
        private int _exp;
        private int _cabnumb;
        private int _instcode;
        private int _schedule;

        public int Id_Doctor
        {
            get { return id_Doc; }
            set
            {
                id_Doc = value;
                OnPropertyChanged(
);
            }
        }
        public string FullName
        {
            get { return _fullname; }
            set
            {
                _fullname = value;
                OnPropertyChanged(
);
            }
        }
        public int Specialization
        {
            get { return _spec; }
            set
            {
                _spec = value;
                OnPropertyChanged(
);
            }
        }
        public int Experience
        {
            get { return _exp; }
            set
            {
                _exp = value;
                OnPropertyChanged(
);
            }
        }
        public int CabinetNumber
        {
            get { return _cabnumb; }
            set
            {
                _cabnumb = value;
                OnPropertyChanged(
);
            }
        }
        public int InsitutionCode
        {
            get { return _instcode; }
            set
            {
                _instcode = value;
                OnPropertyChanged(
);
            }
        }
        public int Schedule
        {
            get { return _schedule; }
            set
            {
                _schedule = value;
                OnPropertyChanged(
);
            }
        }
        public string Login { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = " ")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
