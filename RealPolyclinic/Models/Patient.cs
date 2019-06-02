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
        private string address;
        private string policy;
        private string birthday;
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
                    case "Policy":
                        if (HelpfulMethods.CheckDigitsinStr(Policy, 16))
                        {
                            Error += " Введите корректный номер полиса, состоящий из 16 цифр";
                        }
                        break;
                    case "Birthday":
                        DateTime temp;
                        if (!DateTime.TryParse(birthday, out temp))
                        {
                            Error += " Неправильная дата ";
                        }
                        else if(Convert.ToDateTime(birthday).Date>DateTime.Now.Date|| Convert.ToDateTime(birthday).Date.Year<DateTime.Now.Year-120)
                        { 
                            Error += " Неправильный диапозон даты ";
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
        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                OnPropertyChanged("Address");
            }
        }
        public string Policy
        {
            get { return policy; }
            set
            {
                policy = value;
                OnPropertyChanged("Policy");
            }
        }
        public string Birthday
        {
            get
            {
                return birthday;        
            }
            set
            {
                birthday = value;
                OnPropertyChanged("Birthday");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = " ")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
