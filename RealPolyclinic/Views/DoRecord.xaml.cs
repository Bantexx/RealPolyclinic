using RealPolyclinic.Models;
using RealPolyclinic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RealPolyclinic.Views
{
    /// <summary>
    /// Логика взаимодействия для DoRecord.xaml
    /// </summary>
    public partial class DoRecord : Window
    {
        public DoRecord(InfoAppoint IA)
        {
            InitializeComponent();
            DataContext = new MakeAnAppointment(IA);
        }
    }
}
