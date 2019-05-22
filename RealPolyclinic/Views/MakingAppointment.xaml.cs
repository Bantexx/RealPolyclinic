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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RealPolyclinic.Views
{
    /// <summary>
    /// Логика взаимодействия для MakingAppointment.xaml
    /// </summary>
    public partial class MakingAppointment : UserControl
    {
        public MakingAppointment()
        {
            InitializeComponent();
        }

        private void Sv2_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (sender == sv1)
            {
                sv2.ScrollToVerticalOffset(e.VerticalOffset);
                sv2.ScrollToHorizontalOffset(e.HorizontalOffset);
            }
            else
            {
                sv1.ScrollToVerticalOffset(e.VerticalOffset);
                sv1.ScrollToHorizontalOffset(e.HorizontalOffset);
            }
        }
    }
}
