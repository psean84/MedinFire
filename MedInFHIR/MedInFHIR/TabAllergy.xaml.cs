using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Hl7.Fhir.Model;

namespace MedInFHIR
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabAllergy : ContentPage
    {
        public TabAllergy()
        {
            InitializeComponent();
        }

        private void wvAllergies_BindingContextChanged(object sender, EventArgs e)
        {
            var allergies = (List<AllergyIntolerance>)this.BindingContext;

            string htmlContent = "<html><body>";

            if (allergies.Count > 0)
            {
                foreach(AllergyIntolerance at in allergies)
                {
                    foreach (Coding c in at.Substance.Coding)
                    {
                        htmlContent += "<h4>" + c.Display + "<span style=\"font-size:70%\">   (<b>" + at.Status + "</b>)</span>"+"</h4>";
                                                
                        if(at.Criticality != null)
                            htmlContent += "<p>Critical: " + at.Criticality + "</p>";

                        if(at.Type != null)
                            htmlContent += "<p>Tolerence Type: <b>" + at.Type + "</b></p>";

                        if(at.Category != null)
                            htmlContent += "<p>Tolerence Category: <b>" + at.Category + "</b></p>";

                        if(at.LastOccurence != null)
                            htmlContent += "<p>Last occured on: <b>" + at.LastOccurence + "</b></p>";

                        htmlContent += "<hr />";
                    }
                }
            }
            else
            {
                wvAllergies.VerticalOptions = LayoutOptions.FillAndExpand;
                htmlContent += "<h2 style='text-align: center;vertical-align: middle'>No information about Allergies found for this patient.</h2>";
            }

            htmlContent += "</body></html>";

            HtmlWebViewSource htmlSource = new HtmlWebViewSource();

            htmlSource.Html = htmlContent;

            wvAllergies.Source = htmlSource;
        }
    }
}