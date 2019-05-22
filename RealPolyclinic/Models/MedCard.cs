using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RealPolyclinic.Models
{
    public class MedCard : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = " ")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private int id_Card;
        private int id_Patient;
        private DateTime date_Created;
        private int institution_Code;
        private string diagnoses;
        private string vaccinations;
        private string surveys;
        private string analysis;

        public int Id_card
        {
            get { return id_Card; }
            set
            {
                id_Card = value;
                OnPropertyChanged();
            }
        }
        public int Id_Patient {
            get { return id_Patient; }
            set
            {
                id_Patient = value;
                OnPropertyChanged();
            }
        }
        public DateTime Date_Created {
            get { return date_Created; }
            set
            {
                date_Created = value;
                OnPropertyChanged();
            }
        }
        public int Institution_Code {
            get { return institution_Code; }
            set
            {
                institution_Code = value;
                OnPropertyChanged();
            }
        }
        public string Diagnoses {
            get { return diagnoses; }
            set
            {
                diagnoses = value;
                OnPropertyChanged();
            }
        }
        public string Vaccinations {
            get { return vaccinations; }
            set
            {
                vaccinations = value;
                OnPropertyChanged();
            }
        }
        public string Analysis {
            get { return analysis; }
            set
            {
                analysis = value;
                OnPropertyChanged();
            }
        }
        public string Surveys
        {
            get { return surveys; }
            set
            {
                surveys = value;
                OnPropertyChanged();
            }
        }

    }
}
