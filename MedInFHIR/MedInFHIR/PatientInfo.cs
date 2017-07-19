using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Model;
using Hl7.Fhir.Support;

namespace MedInFHIR
{
    public class PatientInfo
    {
        public bool isExceptionEncountered;

        public List<OperationOutcome.IssueComponent> ExceptionIssues { get; private set; }

        public List<AllergyIntolerance> Allergies { get; private set; }

        public PDInformation Generalinformation { get; private set; }

        public List<Condition> Conditions { get; private set; }

        public List<DetectedIssue> DetectedIssues { get; private set; }
        
        FhirClient _fc;

        //Constructor which takes FHIR server Uri as argument to initialize new patient object
        public PatientInfo(FhirClient fhirclient)
        {
            try
            {
                isExceptionEncountered = false;

                _fc = fhirclient;

                Allergies = new List<AllergyIntolerance>();

                Conditions = new List<Condition>();

                ExceptionIssues = new List<OperationOutcome.IssueComponent>();

                DetectedIssues = new List<DetectedIssue>();

            }
            catch (FhirOperationException foe)
            {
                isExceptionEncountered = true;
                ExceptionIssues = foe.Outcome.Issue;
            }
        }


        /// <summary>
        /// Setting Global Patient object _patientInfo
        /// </summary>
        public void InitialisePatient(string id)
        {
            try
            {
                isExceptionEncountered = false;

                Patient pt = _fc.Read<Patient>(ResourceIdentity.Build("Patient", id));

                Generalinformation = new PDInformation(pt);

                setPatientDetectedIssues();

                getPatientAllergies();

                getPatientCurrentConditions();

            }
            catch(FhirOperationException foe)
            {
                isExceptionEncountered = true;
                ExceptionIssues = foe.Outcome.Issue;
            }
            catch (Exception ex)
            {
                isExceptionEncountered = true;
                OperationOutcome.IssueComponent ic = new OperationOutcome.IssueComponent();
                ic.Diagnostics = ex.Message;
                ExceptionIssues.Add(ic);
            }
        }

        /// <summary>
        /// To get patient Allergies with the help of global Patient object
        /// </summary>
        public void getPatientAllergies()
        {
            try
            {
                SearchParams searchParams = new SearchParams();

                searchParams.Add("patient", Generalinformation.Id);
                
                searchParams.Add("_count", "20");

                Bundle bd = _fc.Search<AllergyIntolerance>(searchParams);

                bool isCountOver = false;

                while(!isCountOver)
                {
                    if (bd.NextLink == null)
                        isCountOver = true;

                    foreach (Bundle.EntryComponent et in bd.Entry)
                    {
                        if (et.Resource.ResourceType == ResourceType.AllergyIntolerance)
                            Allergies.Add((AllergyIntolerance)et.Resource);
                    }

                    bd = _fc.Continue(bd);
                }                
            }
            catch (FhirOperationException foe)
            {
                isExceptionEncountered = true;
                ExceptionIssues = foe.Outcome.Issue;
            }
            catch (Exception ex)
            {
                isExceptionEncountered = true;
                OperationOutcome.IssueComponent ic = new OperationOutcome.IssueComponent();
                ic.Diagnostics = ex.Message;
                ExceptionIssues.Add(ic);
            }
        }

        /// <summary>
        /// To get patient history (procedures performed on or with patient ) with the help of global Patient object
        /// </summary>
        public void getPatientCurrentConditions()
        {
            try
            {
                SearchParams searchParams = new SearchParams();

                searchParams.Add("patient", Generalinformation.Id);

                searchParams.Add("_count", "20");

                Bundle bd = _fc.Search<Condition>(searchParams);

                bool isCountOver = false;

                while (!isCountOver)
                {
                    if (bd.NextLink == null)
                        isCountOver = true;

                    foreach (Bundle.EntryComponent et in bd.Entry)
                    {
                        if (et.Resource.ResourceType == ResourceType.Condition)
                            Conditions.Add((Condition)et.Resource);
                    }

                    bd = _fc.Continue(bd);
                }
            }
            catch (FhirOperationException foe)
            {
                isExceptionEncountered = true;
                ExceptionIssues = foe.Outcome.Issue;
            }
            catch (Exception ex)
            {
                isExceptionEncountered = true;
                OperationOutcome.IssueComponent ic = new OperationOutcome.IssueComponent();
                ic.Diagnostics = ex.Message;
                ExceptionIssues.Add(ic);
            }
        }

        /// <summary>
        /// To get patient Allergies with the help of global Patient object
        /// </summary>
        public void setPatientDetectedIssues()
        {
            try
            {
                SearchParams searchParams = new SearchParams();

                searchParams.Add("patient", Generalinformation.Id);

                searchParams.Add("_count", "20");

                Bundle bd = _fc.Search<DetectedIssue>(searchParams);

                bool isCountOver = false;

                List<DetectedIssue> lstDI = new List<DetectedIssue>();

                while (!isCountOver)
                {
                    if (bd.NextLink == null)
                        isCountOver = true;

                    foreach (Bundle.EntryComponent et in bd.Entry)
                    {
                        if (et.Resource.ResourceType == ResourceType.DetectedIssue)
                            lstDI.Add((DetectedIssue)et.Resource);
                    }

                    bd = _fc.Continue(bd);
                }

                DetectedIssues.AddRange(lstDI);
            }
            catch (FhirOperationException foe)
            {
                isExceptionEncountered = true;
                ExceptionIssues = foe.Outcome.Issue;
            }
            catch (Exception ex)
            {
                isExceptionEncountered = true;
                OperationOutcome.IssueComponent ic = new OperationOutcome.IssueComponent();
                ic.Diagnostics = ex.Message;
                ExceptionIssues.Add(ic);
            }
        }

    }
}
