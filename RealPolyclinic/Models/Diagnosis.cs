using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RealPolyclinic.Models
{
    public class Diagnosis : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = " ")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        int id_diagnosis;
        string title;
        int id_patient;
        int id_doctor;
        string date;
        string desc;
        public int Id_Diagnosis
        {
            get { return id_diagnosis; }
            set
            {
                id_diagnosis = value;
                OnPropertyChanged();
            }
        }
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
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
        public int Id_Doctor
        {
            get { return id_doctor; }
            set
            {
                id_doctor = value;
                OnPropertyChanged();
            }
        }
        public string Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged();
            }
        }
        public string NameDoc { get; set; }
        public string Description
        {
            get { return desc; }
            set
            {
                desc = value;
                OnPropertyChanged();
            }
        }
    }
}
