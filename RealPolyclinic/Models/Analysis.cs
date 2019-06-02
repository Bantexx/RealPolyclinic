using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RealPolyclinic.Models
{
    public class Analysis : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = " ")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        int id_analysis;
        string type;
        int id_patient;
        string date;
        string desc;

        public int Id_Analysis
        {
            get { return id_analysis; }
            set
            {
                id_analysis = value;
                OnPropertyChanged();
            }
        }
        public string Type
        {
            get { return type; }
            set
            {
                type = value;
                OnPropertyChanged();
            }
        }
        public string Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }
        public string Description
        {
            get { return desc; }
            set
            {
                desc = value;
                OnPropertyChanged();
            }
        }
        public int Id_Patient
        {
            get { return id_patient; }
            set
            {
                id_patient = value;
                OnPropertyChanged();
            }
        }
        public string this[string columnName]
        {
            get
            {
                Error = String.Empty;
                switch (columnName)
                {
                    case "Date":
                        DateTime temp;
                        if (!DateTime.TryParse(date, out temp))
                        {
                            Error += "Неправильная дата";
                        }
                        break;
                }
                return Error;
            }
        }
        public string Error { get; private set; }
    }
}
