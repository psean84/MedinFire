using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MedInFHIR
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabParent : TabbedPage
    {
        public TabParent()
        {
            InitializeComponent();                        
        }

        public TabParent(PatientInfo pi)
        {
            InitializeComponent();
            
            tpInfo.BindingContext = pi.Generalinformation;

            tpAllergy.BindingContext = pi.Allergies;

            tpDetectedIssues.BindingContext = pi.DetectedIssues;

        }
        
    }
}