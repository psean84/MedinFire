using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

namespace MedInFHIR
{
    public class PDInformation
    {
        public List<DisplayName> Names { get; private set; }

        public string Gender { get; private set; }

        public DateTime BirthDate { get; private set; }

        public bool Deceased { get; private set; }

        public DateTime DeceasedDate { get; private set; }
        
        public string MaritalStatus { get; private set; }

        public bool Active { get; private set; }
        
        public string Organization { get; private set; }

        public string GeneralPractitioner { get; private set; }

        public string PreferredLanguage { get; private set; }

        public List<ContactPoint> Telecom { get; private set; }

        public List<ContactInfo> Contact { get; private set; }

        private readonly  Dictionary<string, string> MaritalCodes = new Dictionary<string, string>() {{"M", "Married"}, { "A", "Annulled" }, { "D", "Divorced" }, { "I", "Interlocutory" }, { "L", "Legally Separated" }, { "P", "Polygamous" }, { "S", "Never Married" }, { "T", "Domestic partner" }, { "U", "Unmarried" }, { "W", "Widowed" }, { "UNK", "unknown" }, };
        
        public string Id { get; private set; }

        public List<Address> Addresses { get; private set; }

        /// <summary>
        /// Initialise everything to PDInformation class obect with reference Patient and other objects
        /// </summary>
        /// <param name="pt">Patient object which is used to initialise local properties</param>
        public PDInformation(Patient pt)
        {
            Id = pt.Id;

            Names = new List<DisplayName>();

            BirthDate = new DateTime();

            DeceasedDate = new DateTime();
            
            if(pt.Telecom != null)
                Telecom = pt.Telecom;

            Contact = new List<ContactInfo>();

            if(pt.Active.HasValue)
                Active = pt.Active.Value;

            if(pt.Language != null)
                PreferredLanguage = pt.Language;

            if(pt.Gender != null)
                Gender = pt.Gender.Value.ToString();
            
            string ms = "";

            if (pt.MaritalStatus != null)
            {
                MaritalCodes.TryGetValue(pt.MaritalStatus.Coding.First().Code.ToUpper(), out ms);

                if (ms == "")
                {
                    MaritalStatus = pt.MaritalStatus.Text;
                }
                else
                {
                    MaritalStatus = ms;
                }
            }

            if(pt.ManagingOrganization != null)
                Organization = pt.ManagingOrganization.Display;


            if(pt.Contact != null)
                setContactInformation(pt.Contact);

            if(pt.Name != null)
                setNames(pt.Name);


            if(pt.BirthDate != null)
                setBirthDateTime(pt.BirthDate);

            if (pt.CareProvider.Count > 0)
                GeneralPractitioner = pt.CareProvider.First().Display;

            if (pt.Address.Count() > 0)
                Addresses = pt.Address;
                        
        }
        
        /// <summary>
        /// Extracting readable names from List of HumanName objects
        /// </summary>
        /// <param name="ns">List of names in HumanName object</param>
        private void setNames(List<HumanName> ns)
        {
            //Extracting Name from single HumanName object
            foreach (HumanName n in ns)
            {
                DisplayName d = new DisplayName();

                string name = "";

                if (n.Prefix.Count() > 0)
                    name += n.Prefix.First();

                foreach (string sn in n.Given)
                    name += sn + " ";

                if (n.Family.Count() > 0)
                    name += n.Family.First() + " ";

                if (n.Suffix.Count() > 0)
                    name += n.Suffix.First() + " ";

                if(n.Use != null)
                    name += "(" + n.Use + ")";

                d.FullName = name;

                Names.Add(d);
            }
        }

        /// <summary>
        /// Settin the Birth date in dateTime object
        /// </summary>
        /// <param name="date"> Birthdate string from Patient object </param>
        private void setBirthDateTime(string date)
        {
            DateTime dt = new DateTime();

            DateTime.TryParse(date, out dt);

            BirthDate = dt;
        }
                

        private void setContactInformation(List<Patient.ContactComponent> lstcc)
        {
            foreach(Patient.ContactComponent cc in lstcc)
            {
                Contact.Add(new ContactInfo(cc));
            }
        }

    }

    public class DisplayName
    {
        public string FullName { get; set; }
    }

}
