using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

namespace MedInFHIR
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TabDetectedIssues : ContentPage
	{
		public TabDetectedIssues ()
		{
			InitializeComponent ();
		}

        private void StackLayout_BindingContextChanged(object sender, EventArgs e)
        {
            StackLayout st = (StackLayout)sender;

            List<DetectedIssue> lstDi = (List<DetectedIssue>)st.BindingContext;
                        
            if(lstDi.Count() == 0)
            {
                Label lblTmp =  new Label() { Text = "No Information of Detected Issues found for this patient.", FontAttributes = FontAttributes.Bold, VerticalOptions = LayoutOptions.Start, HorizontalOptions = LayoutOptions.CenterAndExpand, FontSize=Xamarin.Forms.Device.GetNamedSize(NamedSize.Medium, typeof(Label)) };
                lblError = lblTmp;
            }else
            {
                lstDetectedIssues.ItemsSource = lstDi;
            }
        }


        private void Label_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Text")
            {
                Label tmpLabel = (Label)sender;

                string[] tmpStr;

                if (tmpLabel != null)
                {
                    tmpStr = tmpLabel.Text.Split(new char[] { ':' });

                    if (tmpStr[1] == " ")
                    {
                        tmpLabel.Text = tmpStr[0] + ": Not Specified";
                    }
                }

            }
        }

    }
}