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
    public partial class TabPatient : ContentPage
    {
        public TabPatient()
        {
            InitializeComponent();
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

                    if (tmpStr[1].Contains("0001") || tmpStr[1] == " ")
                    {
                        tmpLabel.Text = tmpStr[0] + ": Not Specified";
                    }
                }

            }
        }

        private void nameStack_BindingContextChanged(object sender, EventArgs e)
        {            
            if (nameStack.Children.Count() == 0)
            {
                List<DisplayName> Names = (List<DisplayName>)nameStack.BindingContext;

                foreach (DisplayName d in Names)
                {
                    Label lblName = new Label();

                    lblName.Text = d.FullName;

                    lblName.FontAttributes = FontAttributes.Bold;

                    nameStack.Children.Add(lblName);
                }
            }
        }

        private void telecomStack_BindingContextChanged(object sender, EventArgs e)
        {
            StackLayout st = (StackLayout)sender;

            List<ContactPoint> cp = (List<ContactPoint>)st.BindingContext;

            if(st.Children.Count() == 0)
            {
                foreach (ContactPoint c in cp)
                {
                    Label lbl = new Label();

                    lbl.Text = "";

                    if (c.System != null)
                        lbl.Text = c.System + ": ";

                    if (c.Value != null)
                        lbl.Text += c.Value;
                    
                    if(c.Use != null)
                        lbl.Text += " (" + c.Use.Value.ToString() + ")";

                    st.Children.Add(lbl);
                }
            }
        }

        private void stAddreses_BindingContextChanged(object sender, EventArgs e)
        {
            StackLayout st = (StackLayout)sender;

            List<Address> ads = (List<Address>)st.BindingContext;
            if (st.Children.Count() == 0 && ads != null)
            {
                foreach (Address ad in ads)
                {
                    st.Children.Add(stacklayoutAddress(ad));
                }
            }
        }

        private void lineStack_BindingContextChanged(object sender, EventArgs e)
        {
            Frame currentControl = (Frame)sender;

            Address ad = (Address)currentControl.BindingContext;

            if(ad != null)
                currentControl.Content = stacklayoutAddress(ad);
        }

        private StackLayout stacklayoutAddress(Address ad)
        {
            StackLayout st = new StackLayout();
            
            #region Set header for the address Use with the period if specified

            Label lbl = new Label();
             
            lbl.Text = ad.Use.Value.ToString();

            lbl.FontAttributes = FontAttributes.Bold;

            if (ad.Period != null)
            {
                DateTime dt = new DateTime();

                DateTime.TryParse(ad.Period.Start, out dt);

                if (dt.Year != 1)
                {
                    lbl.Text += "(" + string.Format("{0:dd-MM-yyyy}", dt) + " - ";

                    DateTime.TryParse(ad.Period.End, out dt);

                    if (dt.Year != 1)
                    {
                        lbl.Text += string.Format("{0:dd-MM-yyyy}", dt);
                    }

                    lbl.Text += ")";
                }
            }

            st.Children.Add(lbl);

            #endregion

            #region Set lines to address

            List<string> lines = ad.Line.ToList();

            foreach(string line in lines)
            {
                Label tmpLine = new Label();

                tmpLine.Margin = new Thickness(0);

                tmpLine.Text = line;

                st.Children.Add(tmpLine);
            }

            #endregion

            #region Set Remaining address
            
            if (ad.City != null)
            {
                st.Children.Add(new Label() { Text = ad.City, Margin=0 });
            }

            if (ad.District != null)
            {
                st.Children.Add(new Label() { Text = ad.District, Margin = 0 });
            }

            if (ad.State != null)
            {
                st.Children.Add(new Label() { Text = ad.State, Margin = 0 });
            }

            if (ad.PostalCode != null)
            {
                st.Children.Add(new Label() { Text = ad.PostalCode, Margin = 0 });
            }

            if (ad.Country != null)
            {
                st.Children.Add(new Label() { Text = ad.Country, Margin = 0 });
            }
            #endregion
            
            return st;
        }

        private void Label_SetHeight(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Label lbl = (Label)sender;

            if(e.PropertyName == "Height" && lbl != null)
            {
                if(lbl.Text == null)
                {
                    lbl.HeightRequest = 0;
                }
            }
        }
        
    }
}