using RealPolyclinic.HelpMethods;
using RealPolyclinic.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RealPolyclinic.ViewModels
{
    public class AddNewToCardVM:BaseViewModel
    {
        public string[] elementsCard { get; set; }
        public Diagnosis diagnosis { get; set; }
        public Vaccination vaccination { get; set; }
        public Survey survey { get; set; }
        public Analysis analys { get; set; }
        string selectitem;
        public string selectedItem
        {
            get { return selectitem; }
            set
            {
                switch (value)
                {
                    case "Diagnosis":
                        showDiag = "Visible";
                        showVacc = "Hidden";
                        showSurv = "Hidden";
                        showAnalys = "Hidden";
                        break;
                    case "Vaccination":
                        showDiag = "Hidden";
                        showVacc = "Visible";
                        showSurv = "Hidden";
                        showAnalys = "Hidden";
                        break;
                    case "Survey":
                        showDiag = "Hidden";
                        showVacc = "Hidden";
                        showSurv = "Visible";
                        showAnalys = "Hidden";
                        break;
                    case "Analys":
                        showDiag = "Hidden";
                        showVacc = "Hidden";
                        showSurv = "Hidden";
                        showAnalys = "Visible";
                        break;
                }
                selectitem = value;
                OnPropertyChanged();
            }
        }
        string shdiag;
        public string showDiag
        {
            get { return shdiag; }
            set
            {
                shdiag = value;
                OnPropertyChanged();
            }
        }
        string shvacc;
        public string showVacc
        {
            get { return shvacc; }
            set
            {
                shvacc = value;
                OnPropertyChanged();
            }
        }
        string shsurv;
        public string showSurv
        {
            get { return shsurv; }
            set
            {
                shsurv = value;
                OnPropertyChanged();
            }
        }
        string shanalys;
        public string showAnalys
        {
            get { return shanalys; }
            set
            {
                shanalys = value;
                OnPropertyChanged();
            }
        }

        public ICommand Add_Diag { get; set; }
        public ICommand Add_Vacc { get; set; }
        public ICommand Add_Surv { get; set; }
        public ICommand Add_Analys { get; set; }

        public AddNewToCardVM(int id_pat,int id_doc)
        {
            elementsCard = new string[] { "Diagnosis", "Vaccination","Survey","Analys" };

            showDiag = "Hidden";
            showVacc = "Hidden";
            showSurv = "Hidden";
            showAnalys = "Hidden";

            diagnosis = new Diagnosis() {Id_Patient=id_pat,Id_Doctor=id_doc};
            vaccination = new Vaccination() { Id_Patient = id_pat,Date=""};
            survey = new Survey() { Id_Patient = id_pat, Date = "" };
            analys = new Analysis() { Id_Patient = id_pat, Date = "" };
         
            Add_Diag = new RelayCommand(x =>addDiagnosis(diagnosis));
            Add_Vacc = new RelayCommand(x => addVaccination(vaccination));
            Add_Surv = new RelayCommand(x => addSurvey(survey));
            Add_Analys = new RelayCommand(x => addAnalys(analys));
        }
        private void addDiagnosis(Diagnosis dng)
        {
            if (dng.Title != "" && dng.Description != "")
            {
                WorkWithDb wwd = new WorkWithDb();
                string sqlexp = "INSERT INTO Diagnoses(Title,Id_Patient,Id_Doctor,Data,Description) " +
                    "VALUES(@Title,@Id_p,@Id_d,@Date,@Desc)";
                Dictionary<string, object> values = new Dictionary<string, object>()
                {
                    { "@Title",dng.Title },
                    { "@Id_p",dng.Id_Patient },
                    { "@Id_d",dng.Id_Doctor },
                    { "@Date",DateTime.Now.Date },
                    { "@Desc",dng.Description },
                };
                Queue<SqlDbType> types = new Queue<SqlDbType>();
                types.Enqueue(SqlDbType.NVarChar);
                types.Enqueue(SqlDbType.Int);
                types.Enqueue(SqlDbType.Int);
                types.Enqueue(SqlDbType.Date);
                types.Enqueue(SqlDbType.NVarChar);

                wwd.InsertInDb(sqlexp, values, types);
            }
        }
        private void addVaccination(Vaccination vacc)
        {
            if (vacc.Type != "" && vacc.Description != ""&&vacc.Error==String.Empty)
            {
                WorkWithDb wwd = new WorkWithDb();
                string sqlexp = "INSERT INTO Vaccinations(Type,Description,Data_Vacc,Id_Patient) " +
                    "VALUES(@Type,@Desc,@Date,@Id_p)";
                Dictionary<string, object> values = new Dictionary<string, object>()
                {
                    { "@Type",vacc.Type },
                    { "@Id_p",vacc.Id_Patient },
                    { "@Date",Convert.ToDateTime(vacc.Date)},
                    { "@Desc",vacc.Description },
                };
                Queue<SqlDbType> types = new Queue<SqlDbType>();
                types.Enqueue(SqlDbType.NVarChar);
                types.Enqueue(SqlDbType.Int);
                types.Enqueue(SqlDbType.Date);
                types.Enqueue(SqlDbType.NVarChar);
                wwd.InsertInDb(sqlexp, values, types);
            }
        }
        private void addSurvey(Survey vacc)
        {
            if (vacc.Title != "" && vacc.Description != "" && vacc.Error == String.Empty)
            {
                WorkWithDb wwd = new WorkWithDb();
                string sqlexp = "INSERT INTO Surveys(Description,Title,Data,Id_Patient) " +
                    "VALUES(@Title,@Desc,@Date,@Id_p)";
                Dictionary<string, object> values = new Dictionary<string, object>()
                {
                    { "@Title",vacc.Title },
                    { "@Id_p",vacc.Id_Patient },
                    { "@Date",Convert.ToDateTime(vacc.Date)},
                    { "@Desc",vacc.Description },
                };
                Queue<SqlDbType> types = new Queue<SqlDbType>();
                types.Enqueue(SqlDbType.NVarChar);
                types.Enqueue(SqlDbType.Int);
                types.Enqueue(SqlDbType.Date);
                types.Enqueue(SqlDbType.NVarChar);
                wwd.InsertInDb(sqlexp, values, types);
            }
        }
        private void addAnalys(Analysis vacc)
        {
            if (vacc.Type != "" && vacc.Description != "" && vacc.Error == String.Empty)
            {
                WorkWithDb wwd = new WorkWithDb();
                string sqlexp = "INSERT INTO Analysis(Id_Patient,Type,DateofDelivery,Description) " +
                    "VALUES(@Id_p,@Type,@DoD,@Desc)";
                Dictionary<string, object> values = new Dictionary<string, object>()
                {
                    { "@Type",vacc.Type },
                    { "@Id_p",vacc.Id_Patient },
                    { "@DoD",Convert.ToDateTime(vacc.Date)},
                    { "@Desc",vacc.Description },
                };
                Queue<SqlDbType> types = new Queue<SqlDbType>();
                types.Enqueue(SqlDbType.NVarChar);
                types.Enqueue(SqlDbType.Int);
                types.Enqueue(SqlDbType.Date);
                types.Enqueue(SqlDbType.NVarChar);
                wwd.InsertInDb(sqlexp, values, types);
            }
        }

    }
}
