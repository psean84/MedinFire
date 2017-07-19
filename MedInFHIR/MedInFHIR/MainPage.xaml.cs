using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Model;


namespace MedInFHIR
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent(); 
        }

        async void Button_Clicked(object sender, EventArgs e)
        {
            PatientInfo pi = new PatientInfo(new FhirClient(new Uri("http://fhirtest.uhn.ca/baseDstu2")));

            pi.InitialisePatient(edPatientID.Text);

            if (!pi.isExceptionEncountered)
                await Navigation.PushAsync(new TabParent(pi));
            else
                lblError.Text = pi.ExceptionIssues.First().Diagnostics;
        }
 
    }
}
