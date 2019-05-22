using RealPolyclinic.HelpMethods;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RealPolyclinic.Models
{
    public class Patient : INotifyPropertyChanged, IDataErrorInfo
    {
        private int id;
        private string firstname;
        private string surname;
        private string patronymic;
        private string snils;
        private string telephone;
        private int id_card;

        public string this[string columnName]
        {
            get
            {
                Error = String.Empty;
                switch (columnName)
                {
                    case "FirstName":
                        if (FirstName.Length < 2 || FirstName.Length > 20)
                        {
                            Error += " Имя должно быть больше 1 и меньше 20 символов ";
                        }
                        if (HelpfulMethods.IsNumberContains(FirstName))
                        {
                            Error += " Имя содержит цифры ";
                        }
                        if (!HelpfulMethods.IsOnlyLetters(FirstName))
                        {
                            Error +=" Имя содержит недопустимы символы ";
                        }
                        break;
                    case "SurName":
                        if (HelpfulMethods.IsNumberContains(SurName))
                        {
                            Error += " Фамилия содержит цифры ";
                        }
                        if (SurName.Length < 2 || SurName.Length > 20)
                        {
                            Error += " Фамилия должно быть больше 1 и меньше 20 символов ";
                        }
                        if (!HelpfulMethods.IsOnlyLetters(SurName))
                        {
                            Error += " Фамилия содержит недопустимы символы ";
                        }
                        break;
                    case "Patronymic":
                        if (HelpfulMethods.IsNumberContains(Patronymic))
                        {
                            Error += " Отчество содержит цифры ";
                        }
                        if (Patronymic.Length < 2 || Patronymic.Length > 20)
                        {
                            Error += " Отчество должно быть больше 1 и меньше 20 символов ";
                        }
                        if (!HelpfulMethods.IsOnlyLetters(Patronymic))
                        {
                            Error += " Отчество содержит недопустимы символы ";
                        }
                        break;
                    case "Snils":
                        if (HelpfulMethods.CheckDigitsinStr(Snils,11))
                        {
                            Error += " Введите корректный номер снилс, состоящий из 11 цифр";
                        }
                        break;
                    case "Telephone":
                        if (HelpfulMethods.CheckDigitsinStr(Telephone,11))
                        {
                            Error += " Введите корректный номер телефона, состоящий из 11 цифр";
                        }
                        break;
                }
                return Error;
            }
        }
        public string Error { get; private set; }

        public string FirstName
        {
            get { return firstname; }
            set
            {
                firstname = value;
                OnPropertyChanged("FirstName");
            }
        }
        public string SurName
        {
            get { return surname; }
            set
            {
                surname = value;
                OnPropertyChanged("SurName");
            }
        }
        public string Patronymic
        {
            get { return patronymic; }
            set
            {
                patronymic = value;
                OnPropertyChanged("Patronymic");
            }
        }
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        public int Id_Card
        {
            get { return id_card; }
            set
            {
                id_card = value;
                OnPropertyChanged("Id_Card");
            }
        }
        public string Snils
        {
            get { return snils; }
            set
            {
                snils = value;
                OnPropertyChanged("Snils");
            }
        }
        public string Telephone
        {
            get { return telephone; }
            set
            {
                telephone = value;
                OnPropertyChanged("Telephone");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = " ")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
