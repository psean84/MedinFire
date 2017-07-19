using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;


namespace MedInFHIR
{
    public class ContactInfo
    {
        public string Relationship { get; private set; }

        public string Name { get; private set; }

        public List<ContactPoint> Telecom { get; private set; }

        public DateTime StartPeriod { get; private set; }

        public DateTime EndPeriod { get; private set; }

        public Address Address{ get; private set; }

        public string Organisation { get; private set; }

        public string Gender { get; private set; }

        public ContactInfo(Patient.ContactComponent con)
        {

            if(con.Telecom != null)
                Telecom = con.Telecom;

            if (con.Period != null)
            {
                DateTime tmpDate = new DateTime();
                StartPeriod = new DateTime();
                EndPeriod = new DateTime();

                DateTime.TryParse(con.Period.Start, out tmpDate);

                if (StartPeriod != tmpDate)
                    StartPeriod = tmpDate;

                DateTime.TryParse(con.Period.End, out tmpDate);

                if(EndPeriod != tmpDate)
                    EndPeriod = tmpDate;
            }

            if(con.Relationship != null)
                Relationship = con.Relationship.First().Coding.First().Display;


            if(con.Gender != null)
                Gender = con.Gender.Value.ToString();

            if(con.Organization != null)
                Organisation = con.Organization.Display;


            if(con.Address != null)
                Address = con.Address;

            if (con.Name != null)
                setNames(con.Name);
        }

        /// <summary>
        /// Extracting readable names from List of HumanName objects
        /// </summary>
        /// <param name="ns">List of names in HumanName object</param>
        private void setNames(HumanName n)
        {

            string name = "";

            if (n.Prefix.Count() > 0)
                name += n.Prefix.First();

            foreach (string sn in n.Given)
                name += sn + " ";

            foreach (string sn in n.Family)
                name += sn + " ";

            if (n.Suffix.Count() > 0)
                name += n.Suffix.First() + " ";
            
            if (name == "")
                name = n.Text;

            if (n.Use != null)
                name += "(" + n.Use + ")";


            Name = name;

        }
    }
}
