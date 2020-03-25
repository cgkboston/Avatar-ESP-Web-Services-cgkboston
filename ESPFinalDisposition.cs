// PROJECT: ESPFormsWS
// FILENAME: ESPFormsWS.asmx.cs
//    BUILD: 191213
//
// Copyright 2019 A Pretty Cool Program
//
// SSMHAWS is licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance
// with the License. You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on
// an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
// License for the specific language governing permissions and limitations under the License.
//
// For more information about SSMHAWS, please visit http://aprettycoolprogram.com/SSMHAWS.
//
using NTST.ScriptLinkService.Objects;
using System;
using System.Web.Services;

namespace SSMHAWS
{
    /* ESPForms.asmx.cs
     * A Web Service for Netsmart's Avatar EHR that does various things with Child assessment pif date fields.
     * Build 1912131655 - December 13, 2019 4:55 pm
     *
     * Developed by Christopher Kennedy
     *              System Engineer
     *              Aspire Health Alliance
     *              Quincy, Massachusetts 02171
     *
     *              ckennedy@aspirehealthalliance.org
     *
     * This code is:
     *      - very much beta
     *      - will require modification to work in your environment.
     *      - over-commented, so it's abundently clear as to what it does, and how it works.
     *
     *  Please see the Readme.txt file for additional information about how to use this Web Service, and how to develop
     *  your own Web Services for Avatar.
     *
     *  Version history and changes can be found in changelog.txt.
     */


    /// <summary>Entry point.</summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    public class ESPFormsWS : WebService
    {
        /// <summary>Main logic for the ESPFormsWS Web Service - REQUIRED BY AVATAR!.</summary>
        /// <param name="sentOptionObject">The OptionObject2 sent from Avatar.</param>
        /// <param name="scriptAction">The passed script action (ChildPif).</param>
        /// <returns>A completed OptionObject2.</returns>
        [WebMethod]
        public OptionObject2 RunScript(OptionObject2 sentOptionObject, string scriptAction)
        {
            int counter = 0;
            String FieldValueStr = String.Empty;
            String NewFieldValueStr = String.Empty;
            Boolean blankForm;
            String FormName = String.Empty;
            String FormID = String.Empty;
            //Int32 fieldValue = 0;
            switch (scriptAction)
            {
                case "childPifSA":
                    return ChildPifScriptAction(sentOptionObject);
                case "adultPifSA":
                    return AdultPifScriptAction(sentOptionObject);
                case "HelloWorld":
                    sentOptionObject.ErrorCode = 3; // 3: Returns an Error Message with OK button.
                    sentOptionObject.ErrorMesg = "[Hello World]";
                    return CompleteOptionObject(sentOptionObject, sentOptionObject, true, false);
                case "Display":
                    sentOptionObject.ErrorCode = 3; // 3: Returns an Error Message with OK button.
                    foreach (var individualForm in sentOptionObject.Forms)
                    {
                        blankForm = true;
                        counter = 0;
                        foreach (var individualField in individualForm.CurrentRow.Fields)
                        {
                            if (!String.IsNullOrEmpty(individualField.FieldValue))
                            {
                                blankForm = false;
                                sentOptionObject.ErrorMesg += individualField.FieldNumber + " = [" + individualField.FieldValue + "]; ";
                                counter += 1;
                            }
                        }
                        if (!blankForm)
                        {
                            FormID = individualForm.FormId;
                            // MCI/Child Pif follows:
                            if (String.Compare(FormID, "264") == 0) FormName = String.Copy("ESP/MCI Child Adolescent PERSONAL INFORMATION/ DEMOGRAPHICS");
                            else if (String.Compare(FormID, "265") == 0)
                            {
                                FormName = String.Copy("ESP/MCI Child Adolescent ASSESSMENT");
                            }
                            else if (String.Compare(FormID, "266") == 0)
                            {
                                FormName = String.Copy("ESP/MCI Child Adolescent LEGAL");
                            }
                            else if (String.Compare(FormID, "268") == 0)
                            {
                                FormName = String.Copy("ESP/MCI Child Adolescent MEDICAL/ PHYSICAL");
                            }
                            else if (String.Compare(FormID, "269") == 0)
                            {
                                FormName = String.Copy("ESP/MCI Child Adolescent ALERGIES REPORTED");
                            }
                            else if (String.Compare(FormID, "270") == 0)
                            {
                                FormName = String.Copy("ESP/MCI Child Adolescent MEDICATIONS");
                            }
                            else if (String.Compare(FormID, "271") == 0)
                            {
                                FormName = String.Copy("ESP/MCI Child Adolescent RELEVENT HISTORY");
                            }
                            else if (String.Compare(FormID, "272") == 0)
                            {
                                FormName = String.Copy("ESP/MCI Child Adolescent ADDICTION");
                            }
                            else if (String.Compare(FormID, "274") == 0)
                            {
                                FormName = String.Copy("ESP/MCI Child Adolescent MOST RECENT ACUTE ADMISSION(S) AND TREATMENT HISTORY"); // new
                            }
                            else if (String.Compare(FormID, "275") == 0)
                            {
                                FormName = String.Copy("ESP/MCI Child Adolescent MENTAL STATUS EXAM/ RISK ASSESSMENT");
                            }
                            else if (String.Compare(FormID, "276") == 0)
                            {
                                FormName = String.Copy("ESP/MCI Child Adolescent STRENGTHS AND SERVICE PREFERENCES");
                            }
                            else if (String.Compare(FormID, "277") == 0)
                            {
                                FormName = String.Copy("ESP/MCI Child Adolescent DIAGNOSIS TYPE"); // new
                            }
                            else if (String.Compare(FormID, "278") == 0)
                            {
                                FormName = String.Copy("ESP/MCI Child Adolescent IDENTIFIED NEEDS AND GOALS FOR TREATMENT");
                            }
                            else if (String.Compare(FormID, "279") == 0)
                            {
                                FormName = String.Copy("ESP/MCI Child Adolescent Additional Recommendations"); // new
                            }
                            else if (String.Compare(FormID, "224") == 0)
                            {
                                FormName = String.Copy("ESP/MCI Child Adolescent  RESOLUTION/ DISPOSITION/ TREATMENT RECOMMENDATIONS"); // old
                            }
                            else if (String.Compare(FormID, "280") == 0)
                            {
                                FormName = String.Copy("ESP/MCI Child Adolescent DISPOSITION DETAILS");
                            }
                            // Adult Pif follows:
                            else if (String.Compare(FormID, "247") == 0)
                            {
                                FormName = String.Copy("ESP Adult PERSONAL INFORMATION/ DEMOGRAPHICS");
                            }
                            else if (String.Compare(FormID, "248") == 0)
                            {
                                FormName = String.Copy("ESP Adult ASSESSMENT");
                            }
                            else if (String.Compare(FormID, "249") == 0)
                            {
                                FormName = String.Copy("ESP Adult LEGAL");
                            }
                            else if (String.Compare(FormID, "250") == 0)
                            {
                                FormName = String.Copy("ESP Adult COLLATERALS");
                            }
                            else if (String.Compare(FormID, "251") == 0)
                            {
                                FormName = String.Copy("ESP Adult MEDICAL/ PHYSICAL");
                            }
                            else if (String.Compare(FormID, "252") == 0)
                            {
                                FormName = String.Copy("ESP Adult ALERGIES REPORTED");
                            }
                            else if (String.Compare(FormID, "253") == 0)
                            {
                                FormName = String.Copy("ESP Adult MEDICATIONS");
                            }
                            else if (String.Compare(FormID, "254") == 0)
                            {
                                FormName = String.Copy("ESP Adult RELEVENT HISTORY");
                            }
                            else if (String.Compare(FormID, "255") == 0)
                            {
                                FormName = String.Copy("ESP Adult ADDICTION");
                            }
                            else if (String.Compare(FormID, "256") == 0)
                            {
                                FormName = String.Copy("ESP Adult ADDICTION/SUBSTANCE HISTORY");
                            }
                            else if (String.Compare(FormID, "257") == 0)
                            {
                                FormName = String.Copy("ESP Adult MOST RECENT ACUTE ADMISSION(S) AND TREATMENT HISTORY"); //new
                            }
                            else if (String.Compare(FormID, "258") == 0)
                            {
                                FormName = String.Copy("ESP Adult MENTAL STATUS EXAM/ RISK ASSESSMENT");
                            }
                            else if (String.Compare(FormID, "259") == 0)
                            {
                                FormName = String.Copy("ESP Adult STRENGTHS AND SERVICE PREFERENCES");
                            }
                            else if (String.Compare(FormID, "260") == 0)
                            {
                                FormName = String.Copy("ESP Adult DIAGNOSIS TYPE"); // new
                            }
                            else if (String.Compare(FormID, "261") == 0)
                            {
                                FormName = String.Copy("ESP Adult IDENTIFIED NEEDS AND GOALS FOR TREATMENT");
                            }
                            else if (String.Compare(FormID, "262") == 0)
                            {
                                FormName = String.Copy("ESP Adult Additional Recommendations"); // new
                            }
                            else if (String.Compare(FormID, "224") == 0)
                            {
                                FormName = String.Copy("ESP/MCI Adult RESOLUTION/ DISPOSITION/ TREATMENT RECOMMENDATIONS"); // old
                            }
                            else if (String.Compare(FormID, "263") == 0)
                            {
                                FormName = String.Copy("ESP/MCI Child Adolescent DISPOSITION DETAILS");
                            }
                            if (String.IsNullOrEmpty(FormName))
                            {
                                sentOptionObject.ErrorMesg += "\tFORM ID: ";
                                sentOptionObject.ErrorMesg += individualForm.FormId;
                            }
                            else
                            {
                                sentOptionObject.ErrorMesg += "\tSECTION NAME: ";
                                sentOptionObject.ErrorMesg += FormName;
                            }
                            sentOptionObject.ErrorMesg += "\n";
                            FormName = String.Empty;
                            FormID = String.Empty;
                        }
                    }
                    return CompleteOptionObject(sentOptionObject, sentOptionObject, true, false);
                default:
                    return sentOptionObject;
            }
        }

        /// <summary>Provides the version number of this script - REQUIRED BY AVATAR.</summary>
        /// <returns>The script version number.</returns>
        [WebMethod]
        public string GetVersion()
        {
            return "Build 191213";
        }

        /// <summary>Attempts to compare dates in the "ESP MCI/Child Adolescent PIF/Assessment" form and report any errors.</summary>
        /// <param name="sentOptionObject">The Optionbject2 object.</param>
        /// <returns>A completed OptionObject2.</returns>
        public static OptionObject2 ChildPifScriptAction(OptionObject2 sentOptionObject)
        {
            // This method is used with the Child Assessment Pif form (ESP MCI/Child Adolescent PIF/Assessment).
            // You will need to modify these to match the ID values in your environment.

            // Personal Information / Demographics Section: 
            const string EvaluationDateDateOfServiceField = "263.02";
            const string DateOfBirthField = "263.05";
            const string RequestDateField = "263.09";
            const string RequestTimeField = "263.1";
            const string PrimaryRIDNumberField = "263.36"; // added 8/20/2019 This field must be 12 characters.
            // Assessment Section
            const string LocationOfServiceField = "263.57";
            const string EDArrivalDayField = "263.61";
            const string EDArrivalTimeField = "263.62";
            // Disposition Details Section
            const string PsychPhoneConsultRequestedDateField = "264.51";
            const string PsychPhoneConsultRequestedTimeField = "264.52";
            const string PsychConsultBeganDateField = "264.53";
            const string PsychConsultBeganTimeField = "264.54";
            const string InterventionBeganTimeField = "264.55";
            const string PsychInterventionRequestedDateField = "264.61";
            const string PsychInterventionRequestedTimeField = "264.62";
            const string PsychInterventionBeganDateField = "264.63";
            const string PsychInterventionBeganTimeField = "264.64";
            // more PIF fields
            const string ReadyDateField = "264.65";
            const string ReadyTimeField = "264.66";
            const string AdditionalRIDNumberField = "264.67"; // added 8/20/2019 This field must be 12 characters.
            const string ServiceDurationFaceToFaceMastersField = "264.57";
            const string ServiceDurationCollateralMastersField = "264.58";
            const string ServiceDurationFaceToFaceBachelorsField = "264.59";
            const string ServiceDurationCollateralBachelorsField = "264.6";
            const string draftFinalField = "264.93"; // added 11/04/2019 to validate prior to finalizing.

            // miscelaneous booleaneous.
            Boolean EDServiceLocation = false;
            Boolean PrimaryRIDNumberP = false;
            Boolean AdditionalRIDNumberP = false;
            Boolean DraftFinalP = false;

            // Initialize placeholders for the various fields on the pif.
            String DateOfBirthStr = String.Empty;
            String EvaluationDateDateOfServiceStr = String.Empty;
            String LocationOfServiceStr = String.Empty;
            String EDArrivalDayStr = String.Empty;
            String EDArrivalTimeStr = String.Empty;
            String PsychPhoneConsultRequestedDateStr = String.Empty;
            String PsychPhoneConsultRequestedTimeStr = String.Empty;
            String PsychConsultBeganDateStr = String.Empty;
            String PsychConsultBeganTimeStr = String.Empty;
            String PsychInterventionRequestedDateStr = String.Empty;
            String PsychInterventionRequestedTimeStr = String.Empty;
            String PsychInterventionBeganDateStr = String.Empty;
            String PsychInterventionBeganTimeStr = String.Empty;
            String InterventionBeganTimeStr = String.Empty;
            String RequestDateStr = String.Empty;
            String RequestTimeStr = String.Empty;
            String ReadyDateStr = String.Empty;
            String ReadyTimeStr = String.Empty;
            String ServiceDurationStr = String.Empty;
            String PrimaryRIDNumberStr = String.Empty;
            String AdditionalRIDNumberStr = String.Empty;
			int ServiceDurationTotal = 0;

            foreach (var individualForm in sentOptionObject.Forms)
            {
                foreach (var individualField in individualForm.CurrentRow.Fields)
                {
                    switch (individualField.FieldNumber)
                    {
                        // Assessment Section
                        case LocationOfServiceField:
                            if (individualField.FieldValue.Equals("17") == true) // ED is value 17
                            {
                                EDServiceLocation = true;
                            }
                            break;
                        case EDArrivalDayField:
                            EDArrivalDayStr = Convert.ToString(individualField.FieldValue);
                            break;
                        case EDArrivalTimeField:
                            EDArrivalTimeStr = Convert.ToString(individualField.FieldValue);
                            break;
                        // Disposition Details Section
                        case PsychPhoneConsultRequestedDateField:
                            PsychPhoneConsultRequestedDateStr = String.Copy(individualField.FieldValue);
                            break;
                        case PsychPhoneConsultRequestedTimeField:
                            PsychPhoneConsultRequestedTimeStr = String.Copy(individualField.FieldValue);
                            break;
                        case PsychConsultBeganDateField:
                            PsychConsultBeganDateStr = String.Copy(individualField.FieldValue);
                            break;
                        case PsychConsultBeganTimeField:
                            PsychConsultBeganTimeStr = String.Copy(individualField.FieldValue);
                            break;
                        case PsychInterventionRequestedDateField:
                            PsychInterventionRequestedDateStr = String.Copy(individualField.FieldValue);
                            break;
                        case PsychInterventionRequestedTimeField:
                            PsychInterventionRequestedTimeStr = String.Copy(individualField.FieldValue);
                            break;
                        case PsychInterventionBeganDateField:
                            PsychInterventionBeganDateStr = String.Copy(individualField.FieldValue);
                            break;
                        case PsychInterventionBeganTimeField:
                            PsychInterventionBeganTimeStr = String.Copy(individualField.FieldValue);
                            break;
                        case ReadyDateField:
                            ReadyDateStr = String.Copy(individualField.FieldValue);
                            break;
                        case ReadyTimeField:
                            ReadyTimeStr = String.Copy(individualField.FieldValue);
                            break;
                        case RequestDateField:
                            RequestDateStr = String.Copy(individualField.FieldValue);
                            break;
                        case RequestTimeField:
                            RequestTimeStr = String.Copy(individualField.FieldValue);
                            break;
                        case InterventionBeganTimeField:
                            InterventionBeganTimeStr = String.Copy(individualField.FieldValue);
                            break;

                        /* This rule was removed on 8/20/2019, this rule is handled by myAvatar's form designer capabilities.
                        case ServiceProgramField:
                            ServiceProgramStr = String.Copy(individualField.FieldValue);
                            if ((ServiceProgramStr.Equals("304") != true) && (ServiceProgramStr.Equals("306") != true)) // If the Selected Service Program is not 304,306: Adult And Child Crisis Intervention
                            {
                                ServiceProgramP = true;
                            }
                            break;
                        */

                        /* For ServiceDuration we calculate the field from the componenent fields */
                        case ServiceDurationFaceToFaceMastersField:
                        case ServiceDurationCollateralMastersField:
                        case ServiceDurationFaceToFaceBachelorsField:
                        case ServiceDurationCollateralBachelorsField:
                            if ((String.Compare(individualField.FieldValue, "") != 0) && individualField.FieldValue.Equals("0") != true)
                            {
                                ServiceDurationTotal += int.Parse(individualField.FieldValue);
                                ServiceDurationStr = ServiceDurationTotal.ToString();
                            }
                            break;
                        // Personal Information / Demographics Section: 
                        case EvaluationDateDateOfServiceField:
                            EvaluationDateDateOfServiceStr = String.Copy(individualField.FieldValue);
                            break;
                        // Additional fields added 8/20/2019: 
                        case PrimaryRIDNumberField:
                            PrimaryRIDNumberStr = String.Copy(individualField.FieldValue);  // If entered then the length must be 12 charactrers.
                            if ((PrimaryRIDNumberStr.Length > 0) && (PrimaryRIDNumberStr.Length != 12))
                            {
                                PrimaryRIDNumberP = true;
                            }
                            break;
                        case AdditionalRIDNumberField:
                            AdditionalRIDNumberStr = String.Copy(individualField.FieldValue);  // If entered then the length must be 12 characters.
                            if ((AdditionalRIDNumberStr.Length > 0) && (AdditionalRIDNumberStr.Length != 12))
                            {
                                AdditionalRIDNumberP = true;
                            }
                            break;
                        // Additional Field added 9/18/2019:
                        case DateOfBirthField:
                            DateOfBirthStr = String.Copy(individualField.FieldValue); // Client must be at least one year old.
                            break;
                        // Additional Field added 11/4/2019
                        case draftFinalField:
                            if (individualField.FieldValue.Equals("F") == true) // if draft final is set to final
                            {
                                DraftFinalP = true;
                            }
                            break;
                    }
                }
            }
            // Compare the various datetime values generated above.
            return (ComparePifDates(sentOptionObject, EDServiceLocation, EDArrivalDayStr, EDArrivalTimeStr, PsychPhoneConsultRequestedDateStr, PsychPhoneConsultRequestedTimeStr,
                PsychConsultBeganDateStr, PsychConsultBeganTimeStr, PsychInterventionRequestedDateStr, PsychInterventionRequestedTimeStr,
                PsychInterventionBeganDateStr, PsychInterventionBeganTimeStr, ReadyDateStr, ReadyTimeStr, RequestDateStr, RequestTimeStr,
                InterventionBeganTimeStr, ServiceDurationStr, EvaluationDateDateOfServiceStr, PrimaryRIDNumberP, AdditionalRIDNumberP, DraftFinalP, DateOfBirthStr));
        }

        public static OptionObject2 AdultPifScriptAction(OptionObject2 sentOptionObject)
        {
            // This method is used with the  Adult Assessment Pif form (ESP Adult PIF/Comprehensive Assessment).
            // You will need to modify these to match the ID values in your environment.

            // Personal Information / Demographics Section: 
            const string EvaluationDateDateOfServiceField = "260.5";
            const string DateOfBirthField = "260.53";
            const string RequestDateField = "260.57";
            const string RequestTimeField = "260.58";
            const string PrimaryRIDNumberField = "260.84";
            // Assessment Section
            const string LocationOfServiceField = "261.05";
            const string EDArrivalDayField = "261.09";
            const string EDArrivalTimeField = "261.1";
            // Disposition Details Section
            const string PsychPhoneConsultRequestedDateField = "261.99";
            const string PsychPhoneConsultRequestedTimeField = "262";
            const string PsychConsultBeganDateField = "262.01";
            const string PsychConsultBeganTimeField = "262.02";
            const string InterventionBeganTimeField = "262.03";
            const string PsychInterventionRequestedDateField = "262.09";
            const string PsychInterventionRequestedTimeField = "262.1";
            const string PsychInterventionBeganDateField = "262.11";
            const string PsychInterventionBeganTimeField = "262.12";
            // more PIF fields
            const string ReadyDateField = "262.13";
            const string ReadyTimeField = "262.14";
            const string AdditionalRIDNumberField = "262.15";
            const string ServiceDurationField = "262.22";
            const string draftFinalField = "262.42"; // added 11/04/2019 to validate prior to finalizing.


            // miscelaneous booleaneous.
            Boolean EDServiceLocation = false;
            Boolean PrimaryRIDNumberP = false;
            Boolean AdditionalRIDNumberP = false;
            Boolean DraftFinalP = false;

            // Initialize placeholders for the various fields on the pif.
            String DateOfBirthStr = String.Empty;
            String EvaluationDateDateOfServiceStr = String.Empty;
            String LocationOfServiceStr = String.Empty;
            String EDArrivalDayStr = String.Empty;
            String EDArrivalTimeStr = String.Empty;
            String PsychPhoneConsultRequestedDateStr = String.Empty;
            String PsychPhoneConsultRequestedTimeStr = String.Empty;
            String PsychConsultBeganDateStr = String.Empty;
            String PsychConsultBeganTimeStr = String.Empty;
            String PsychInterventionRequestedDateStr = String.Empty;
            String PsychInterventionRequestedTimeStr = String.Empty;
            String PsychInterventionBeganDateStr = String.Empty;
            String PsychInterventionBeganTimeStr = String.Empty;
            String InterventionBeganTimeStr = String.Empty;
            String RequestDateStr = String.Empty;
            String RequestTimeStr = String.Empty;
            String ReadyDateStr = String.Empty;
            String ReadyTimeStr = String.Empty;
            String ServiceDurationStr = String.Empty;
            String PrimaryRIDNumberStr = String.Empty;
            String AdditionalRIDNumberStr = String.Empty;

            foreach (var individualForm in sentOptionObject.Forms)
            {
                foreach (var individualField in individualForm.CurrentRow.Fields)
                {
                    switch (individualField.FieldNumber)
                    {
                        // Assessment Section
                        case LocationOfServiceField:
                            if (individualField.FieldValue.Equals("17") == true) // ED is value 17
                            {
                                EDServiceLocation = true;
                            }
                            break;
                        case EDArrivalDayField:
                            EDArrivalDayStr = Convert.ToString(individualField.FieldValue);
                            break;
                        case EDArrivalTimeField:
                            EDArrivalTimeStr = Convert.ToString(individualField.FieldValue);
                            break;
                        // Disposition Details Section
                        case PsychPhoneConsultRequestedDateField:
                            PsychPhoneConsultRequestedDateStr = String.Copy(individualField.FieldValue);
                            break;
                        case PsychPhoneConsultRequestedTimeField:
                            PsychPhoneConsultRequestedTimeStr = String.Copy(individualField.FieldValue);
                            break;
                        case PsychConsultBeganDateField:
                            PsychConsultBeganDateStr = String.Copy(individualField.FieldValue);
                            break;
                        case PsychConsultBeganTimeField:
                            PsychConsultBeganTimeStr = String.Copy(individualField.FieldValue);
                            break;
                        case PsychInterventionRequestedDateField:
                            PsychInterventionRequestedDateStr = String.Copy(individualField.FieldValue);
                            break;
                        case PsychInterventionRequestedTimeField:
                            PsychInterventionRequestedTimeStr = String.Copy(individualField.FieldValue);
                            break;
                        case PsychInterventionBeganDateField:
                            PsychInterventionBeganDateStr = String.Copy(individualField.FieldValue);
                            break;
                        case PsychInterventionBeganTimeField:
                            PsychInterventionBeganTimeStr = String.Copy(individualField.FieldValue);
                            break;
                        case ReadyDateField:
                            ReadyDateStr = String.Copy(individualField.FieldValue);
                            break;
                        case ReadyTimeField:
                            ReadyTimeStr = String.Copy(individualField.FieldValue);
                            break;
                        case RequestDateField:
                            RequestDateStr = String.Copy(individualField.FieldValue);
                            break;
                        case RequestTimeField:
                            RequestTimeStr = String.Copy(individualField.FieldValue);
                            break;
                        case InterventionBeganTimeField:
                            InterventionBeganTimeStr = String.Copy(individualField.FieldValue);
                            break;
                        /* This rule was removed on 8/20/2019, this rule is handled by myAvatar's form designer capabilities.
                        case ServiceProgramField:
                            ServiceProgramStr = String.Copy(individualField.FieldValue);
                            if ((ServiceProgramStr.Equals("304") != true) && (ServiceProgramStr.Equals("306") != true)) // If the Selected Service Program is not 304,306: Adult And Child Crisis Intervention
                            {
                                ServiceProgramP = true;
                            }
                            break;
                        */
                        case ServiceDurationField:
                            ServiceDurationStr = String.Copy(individualField.FieldValue);
                            break;
                        // Personal Information / Demographics Section: 
                        case EvaluationDateDateOfServiceField:
                            EvaluationDateDateOfServiceStr = String.Copy(individualField.FieldValue);
                            break;
                        // Additional fields added 8/20/2019: 
                        case PrimaryRIDNumberField:
                            PrimaryRIDNumberStr = String.Copy(individualField.FieldValue);  // If entered then the length must be 12 charactrers.
                            if ((PrimaryRIDNumberStr.Length > 0) && (PrimaryRIDNumberStr.Length != 12))
                            {
                                PrimaryRIDNumberP = true;
                            }
                            break;
                        case AdditionalRIDNumberField:
                            AdditionalRIDNumberStr = String.Copy(individualField.FieldValue);  // If entered then the length must be 12 characters.
                            if ((AdditionalRIDNumberStr.Length > 0) && (AdditionalRIDNumberStr.Length != 12))
                            {
                                AdditionalRIDNumberP = true;
                            }
                            break;
                        // Additional Field added 9/18/2019:
                        case DateOfBirthField:
                            DateOfBirthStr = String.Copy(individualField.FieldValue); // Client must be at least one year old.
                            break;
                        // Additional Field added 11/4/2019
                        case draftFinalField:
                            if (individualField.FieldValue.Equals("F") == true) // if draft final is set to final
                            {
                                DraftFinalP = true;
                            }
                            break;
                    }
                }
            }
            // Compare the various datetime values generated above.
            return (ComparePifDates(sentOptionObject, EDServiceLocation, EDArrivalDayStr, EDArrivalTimeStr, PsychPhoneConsultRequestedDateStr, PsychPhoneConsultRequestedTimeStr,
                PsychConsultBeganDateStr, PsychConsultBeganTimeStr, PsychInterventionRequestedDateStr, PsychInterventionRequestedTimeStr,
                PsychInterventionBeganDateStr, PsychInterventionBeganTimeStr, ReadyDateStr, ReadyTimeStr, RequestDateStr, RequestTimeStr,
                InterventionBeganTimeStr, ServiceDurationStr, EvaluationDateDateOfServiceStr, PrimaryRIDNumberP, AdditionalRIDNumberP, DraftFinalP, DateOfBirthStr));
        }

        public static OptionObject2 ComparePifDates(OptionObject2 sentOptionObject, Boolean EDServiceLocation, String EDArrivalDayStr, String EDArrivalTimeStr,
                String PsychPhoneConsultRequestedDateStr, String PsychPhoneConsultRequestedTimeStr, String PsychConsultBeganDateStr, String PsychConsultBeganTimeStr,
                String PsychInterventionRequestedDateStr, String PsychInterventionRequestedTimeStr, String PsychInterventionBeganDateStr, String PsychInterventionBeganTimeStr,
                String ReadyDateStr, String ReadyTimeStr, String RequestDateStr, String RequestTimeStr, String InterventionBeganTimeStr, String ServiceDurationStr, String EvaluationDateDateOfServiceStr,
                Boolean PrimaryRIDNumberP, Boolean AdditionalRIDNumberP, Boolean DraftFinalP, String DateOfBirthStr)
        {
            // Initialize rule violation Booleans
            Boolean ruleViolation1a = false;
            Boolean ruleViolation2a1 = false;
            Boolean ruleViolation2a2 = false;
            Boolean ruleViolation2a3 = false;
            Boolean ruleViolation2a4 = false;
            Boolean ruleViolation2a5 = false;
            Boolean ruleViolation2a6 = false;
            Boolean ruleViolation2a7 = false;
            Boolean ruleViolation2a8 = false;
            Boolean ruleViolation2b = false;
            Boolean ruleViolation3a = false;
            Boolean ruleViolation4a = false;
            // added ruleViolation4b 10/4/2019 to account for rule violations from Carol's initial portal test.
            Boolean ruleViolation4b = false;
            Boolean ruleViolation5a = false;
            Boolean ruleViolation6a1 = false;
            Boolean ruleViolation6a2 = false;
            Boolean ruleViolation7b = false;
            Boolean ruleViolation7c = false;
            Boolean ruleViolation7d = false;
            Boolean ruleViolation8a = false;
            Boolean ruleViolation8b = false;
            Boolean ruleViolation8d = false; // Note this is 8d and not 8c because Stephanie's MBHP Date Rules Document already has 8c 
                                             // as: c.	Can be later than Bed Search Began, Placement Secured, Intervention Ended.
                                             // Rule 8d added 9/26/2019
            Boolean ruleViolation8e = false;
            Boolean ruleViolation9a = false;
            Boolean ruleViolation9b = false;
            Boolean ruleViolation9c = false;
            Boolean ruleViolation10a = false;
            Boolean ruleViolation10b = false;
            Boolean ruleViolation10c = false; // Rule 10c added 9/26/2019
            // Here we note rules 11, 12, and 13 are implemented in the ESP Final Disposition Web Service, FinalDispositionWS.asmx.cs as described in the Updated MBHP Date Rules doc.
            // added ruleViolation14a, 14b to account for rid number length violations
            Boolean ruleViolation14a = PrimaryRIDNumberP;
            Boolean ruleViolation14b = AdditionalRIDNumberP;
            // added date of birth check 9/18/2019, client must be at least one year old.
            Boolean ruleViolation15a = false;
            Boolean ruleViolation15b = false;
            Boolean ruleViolation17a = false;
            Boolean ruleViolation17b = false;
            Boolean ruleViolation17c = false;
            Boolean ruleViolation17d = false;
            Boolean ruleViolation17e = false;
            Boolean ruleViolation17f = false;
            Boolean ruleViolation17g = false;
            Boolean ruleViolation17h = false;
            // Boolean BirthDateP = false;
            int errorCounter = 0;
            String draftOrFinalStr = String.Empty;
            String draftOrFinalPrefixStr = String.Empty;

            if (DraftFinalP == true)
            {
                draftOrFinalStr = String.Copy("Error ");
                draftOrFinalPrefixStr = String.Copy("");
            }
            else
            {
                draftOrFinalStr = String.Copy("Info ");
                draftOrFinalPrefixStr = String.Copy("");

            }

            // Rule 1a states: the Date of Service cannot be in the future.  Therefore the date of service must be either the current day or earlier.
            ruleViolation1a = MBHPDateRules(EvaluationDateDateOfServiceStr, null, null, null, "rule1a");

            // Start by confirming rule #2 from the MBHP Date Rules Document which states ED ARrival Date/Time Must be the earliest date, if the location is ED.
            if (EDServiceLocation == true)
            {
                /* Rule 1 confirms the Assessment ED Arrival prior to Psych Consultation Request.  Rule 2a. MBHP Date Rules. */
                ruleViolation2a1 = MBHPDateRules(EDArrivalDayStr, EDArrivalTimeStr, PsychPhoneConsultRequestedDateStr, PsychPhoneConsultRequestedTimeStr, "rule2a");
                /* Rule 2 confirms the Assessment ED Arrival prior to Psych Consultation Began.  Rule 2a. MBHP Date Rules. */
                ruleViolation2a2 = MBHPDateRules(EDArrivalDayStr, EDArrivalTimeStr, PsychConsultBeganDateStr, PsychConsultBeganTimeStr, "rule2a");
                /* Rule 3 confirms the Assessment ED Arrival prior to Psych Intervention Request.  Rule 2a. MBHP Date Rules. */
                ruleViolation2a3 = MBHPDateRules(EDArrivalDayStr, EDArrivalTimeStr, PsychInterventionRequestedDateStr, PsychInterventionRequestedTimeStr, "rule2a");
                /* Rule 4 confirms the Assessment ED Arrival prior to Psych Intervention Began.  Rule 2a. MBHP Date Rules. */
                ruleViolation2a4 = MBHPDateRules(EDArrivalDayStr, EDArrivalTimeStr, PsychInterventionBeganDateStr, PsychInterventionBeganTimeStr, "rule2a");
                /* Rule 5 confirms the Assessment ED Arrvial prior to PIF Ready time.  Rule 2a. MBHP Date Rules. */
                ruleViolation2a5 = MBHPDateRules(EDArrivalDayStr, EDArrivalTimeStr, ReadyDateStr, ReadyTimeStr, "rule2a");
                /* Rule 6 confirms the Assessment ED Arrival prior to Service Date. Rule 2a.  MBHP Date Rules. */
                /* THIS IS IMPORTANT: SINCE THERE IS NO TimeOfService FIELD, I SET THE SERVICE TIME to 12:01 AM.*/
                ruleViolation2a6 = MBHPDateRules(EDArrivalDayStr, "12:00 AM", EvaluationDateDateOfServiceStr, "12:00 AM", "rule2a");
                /* Rule 7 confirms the Assessment ED Arrival prior to Request Date/Time.  Rule 2a. MBHP Date Rules. */
                ruleViolation2a7 = MBHPDateRules(EDArrivalDayStr, EDArrivalTimeStr, RequestDateStr, RequestTimeStr, "rule2a");
                /* Rule 8 confirms the Assessment ED Arrival Date/TIme is prior to EvaluationDateDateOfService Date/InterventionBeganTimeStr */
                /* Important the reason this rule is viable is that if the client arrives at the ED at 11pm and service begins at 1am, then the EvaluationDateDateOfServiceStr is one greater than the EDArrivalDayStr */
                /* Also we know the intervention Began Date associated with the Intervention Began Time on the disposition detail is the Date of Service date from the . */
                ruleViolation2a8 = MBHPDateRules(EDArrivalDayStr, EDArrivalTimeStr, EvaluationDateDateOfServiceStr, InterventionBeganTimeStr, "rule2a");
                /* Note for ruleviolation2b we send 12 am to the date rules routine because if the ED Arrival Day is more than 5 months back, it doesn't matter what time the client arrived.  Reason being
                 * by the time the file is extracted and sent off to MBHP, the following day or later, you're already well beyond the 6 months.  In fact this is also why the system prevents ED Arrival more than 5
                 * months back, to give the system a month to send the file, in the event we are in the early days of a month timeframe for data sent to MBHP. */
                ruleViolation2b = MBHPDateRules(EDArrivalDayStr, "12:00 AM", null, null, "rule2b");
            }

            /* Rule 3a is similar to rule 2b.  This rule fires only when the Service location is not the ED, hence the else.  Rule 3a states that the earliest date which
                is the Request Date/Time, for locations other than the ED, must be less than 5 months back, again to allow 14 days to get the forms off to MBHP.  
                The salient point for this rule is that the Ready date is always after the Request Date and the Evaluation Date is always after the Ready Date.  
                Hence if the Request Date is less than 5 months back then are the other two dates. */
            else
            {
                ruleViolation3a = MBHPDateRules(RequestDateStr, "12:00 AM", null, null, "rule3a");
            }

            /* Rule 4a confirms Request Date/Time is before the Ready Date/Time: Rule 4a. MBHP Date Rules*/
            ruleViolation4a = MBHPDateRules(RequestDateStr, RequestTimeStr, ReadyDateStr, ReadyTimeStr, "rule4a");

            // 10/4/2019: Another after round 1 portal testing
            /* Rule 4b: DateTimeReadiness can not be later than DateTimeInterventionBegan.  
            * Note rule 4b is similar to rules 2ax and 4a above, so they process the same as well, i.e. via the default case at the end of the MBHPDateRules method. */
            ruleViolation4b = MBHPDateRules(ReadyDateStr, ReadyTimeStr, EvaluationDateDateOfServiceStr, InterventionBeganTimeStr, "rule4b");

            // This rule confirms intervention began time cannot be more than 24 hours after ready time.
            // Note that this rule compares the PIF ReadyDate/Time to the PIF Date of Service AND the Disposition Details Intervention Start Time 
            // This observation is key and very important.
            ruleViolation5a = MBHPDateRules(ReadyDateStr, ReadyTimeStr, EvaluationDateDateOfServiceStr, InterventionBeganTimeStr, "rule5a");

            // ruleViolation6a1 is a dumb rule, because of Avatar or kronos.  All it does is compare the minutes of the service duration to 336 hours (or 14 days) This is rule 6a in Stephanie's MBHP document.
            // just an opinion here: the duration should be a caclculated field based on the service start date/time and service end date/time, regardless of what kronos or the billing module needs.  The fact is
            // if a service can be up to 14 days in duration then it doesn't fit in the confines of kronos and the billing module needs to bill for services rendered, period.  
            // Can this be handled with Add-on or rollup service(s)?
            if (DraftFinalP == true)
            {
                ruleViolation6a1 = MBHPDateRules(ServiceDurationStr, null, null, null, "rule6a1");
                // Rule6a2 is a new rule to confirm the user is entering minutes and NOT units/hours.
                ruleViolation6a2 = MBHPDateRules(ServiceDurationStr, null, null, null, "rule6a2");
            }
            ruleViolation7b = MBHPDateRules(PsychPhoneConsultRequestedDateStr, "12:00 AM", null, null, "rule7b");

            ruleViolation7c = MBHPDateRules(EvaluationDateDateOfServiceStr, InterventionBeganTimeStr, PsychPhoneConsultRequestedDateStr, PsychPhoneConsultRequestedTimeStr, "rule7c");

            ruleViolation7d = MBHPDateRules(EvaluationDateDateOfServiceStr, "12:00 AM", PsychPhoneConsultRequestedDateStr, "12:00 AM", "rule7d");

            // Rule 8a and 8b are for the Psychiatric Consult Request  / Began

            /* Rule 8a will confirm Psychiatric Consult Began Date/Time Cannot be before Psych Phone Consult Requested (rule 8a) */
            ruleViolation8a = MBHPDateRules(PsychPhoneConsultRequestedDateStr, PsychPhoneConsultRequestedTimeStr, PsychConsultBeganDateStr, PsychConsultBeganTimeStr, "rule8a");
            
            /* Rule 8b will confirm Psychiatric Consult Began Date/Time Cannot be more than 12 hours after Psych Phone Consult Requested. */
            ruleViolation8b = MBHPDateRules(PsychPhoneConsultRequestedDateStr, PsychPhoneConsultRequestedTimeStr, PsychConsultBeganDateStr, PsychConsultBeganTimeStr, "rule8b");

            /* Rule 8d will confirm Psych Phone Consult Began Date Cannot be more than 14 days after Evaluation Date/Date of Service.  This rule was added later in the development cycle, 9/26/2019 (cgk). 
               This rule is required to satisfy a rule inconsistency which could prevent the user from saving a final disposition after finalizing the either the Child/Adolescent or Adult PIF, so it important. 
               It is necessary to set some upper limits on the Pif to guarantee the final disposition dates are within the 30 day range range after the Pif is saved. 
               I choose 14 days to provide maximum flexibility while still ensuring the data saved will satisfy the final disposition requirements of 30 days after the date of service  */
            ruleViolation8d = MBHPDateRules(EvaluationDateDateOfServiceStr, "12:00 AM", PsychConsultBeganDateStr, "12:00 AM", "rule8d");

            ruleViolation8e = MBHPDateRules(PsychPhoneConsultRequestedDateStr, "12:00 AM", PsychConsultBeganDateStr, "12:00 AM", "rule8e");

            ruleViolation9a = MBHPDateRules(PsychInterventionRequestedDateStr, "12:00 AM", null, null, "rule9a");

            ruleViolation9b = MBHPDateRules(EvaluationDateDateOfServiceStr, InterventionBeganTimeStr, PsychInterventionRequestedDateStr, PsychInterventionRequestedTimeStr, "rule9b");

            ruleViolation9c = MBHPDateRules(EvaluationDateDateOfServiceStr, "12:00 AM", PsychInterventionRequestedDateStr, "12:00 AM", "rule9c");

            // Rule 10a and 10b are for the Psych Intervention Request / Began

            /* Rule 10a confirms Psychiatric Intervention Began Date/Time cannot be before Psychiatric Intervention Requested Date/Time. */
            ruleViolation10a = MBHPDateRules(PsychInterventionRequestedDateStr, PsychInterventionRequestedTimeStr, PsychInterventionBeganDateStr, PsychInterventionBeganTimeStr, "rule10a");

            /* Rule 10b confirms Pyschiatric Intervention Began Date/Time cannot be more than 12 hours after Psychiatric Intervention Requested. */
            ruleViolation10b = MBHPDateRules(PsychInterventionRequestedDateStr, PsychInterventionRequestedTimeStr, PsychInterventionBeganDateStr, PsychInterventionBeganTimeStr, "rule10b");

            /* Rule 10c will confirm Psych Intervention Began Date Cannot be more than 14 days after Evaluation Date/Date of Service */
            ruleViolation10c = MBHPDateRules(EvaluationDateDateOfServiceStr, "12:00 AM", PsychInterventionBeganDateStr, "12:00 AM", "rule10c");

            /* Rule 15a confirms the client is at least one year old.  This showed up during early October testing. */
            ruleViolation15a = MBHPDateRules(DateOfBirthStr, "", "", "", "rule15a");
            
            /* Rule 15b confirms the client is younger than 150 years old (or 149 years and change).  This also showed up during early October testing. */
            ruleViolation15b = MBHPDateRules(DateOfBirthStr, "", "", "", "rule15b");

            /* Next we confirm the date fields have a corresponding time field for the final disposition where the fields are note required. */
            if (DraftFinalP == true)
            {
                if (!string.IsNullOrEmpty(PsychPhoneConsultRequestedDateStr) && string.IsNullOrEmpty(PsychPhoneConsultRequestedTimeStr))
                    ruleViolation17a = true;
                if (!string.IsNullOrEmpty(PsychConsultBeganDateStr) && string.IsNullOrEmpty(PsychConsultBeganTimeStr))
                    ruleViolation17b = true;
                if (!string.IsNullOrEmpty(PsychInterventionRequestedDateStr) && string.IsNullOrEmpty(PsychInterventionRequestedTimeStr))
                    ruleViolation17c = true;
                if (!string.IsNullOrEmpty(PsychInterventionBeganDateStr) && string.IsNullOrEmpty(PsychInterventionBeganTimeStr))
                    ruleViolation17d = true;
                if (string.IsNullOrEmpty(PsychPhoneConsultRequestedDateStr) && !string.IsNullOrEmpty(PsychPhoneConsultRequestedTimeStr))
                    ruleViolation17e = true;
                if (string.IsNullOrEmpty(PsychConsultBeganDateStr) && !string.IsNullOrEmpty(PsychConsultBeganTimeStr))
                    ruleViolation17f = true;
                if (string.IsNullOrEmpty(PsychInterventionRequestedDateStr) && !string.IsNullOrEmpty(PsychInterventionRequestedTimeStr))
                    ruleViolation17g = true;
                if (string.IsNullOrEmpty(PsychInterventionBeganDateStr) && !string.IsNullOrEmpty(PsychInterventionBeganTimeStr))
                    ruleViolation17h = true;
            }

            /* Return an information message box to the user and reset any and all invalid date fields. */
            if (ruleViolation1a  == true || ruleViolation2a1 == true || ruleViolation2a2 == true || ruleViolation2a3 == true || ruleViolation2a4 == true || ruleViolation2a5 == true || 
                ruleViolation2a6 == true || ruleViolation2a7 == true || ruleViolation2a8 == true || ruleViolation2b == true  || ruleViolation3a == true  || ruleViolation4a == true  || 
                ruleViolation4b  == true || ruleViolation5a == true  || ruleViolation6a1 == true || ruleViolation6a2 == true || ruleViolation7b == true  || ruleViolation7c == true  || 
                ruleViolation7d  == true || ruleViolation8a == true  || ruleViolation8b == true  || ruleViolation8d == true  || ruleViolation8e == true  || ruleViolation9a == true  || 
                ruleViolation9b == true  || ruleViolation10a == true || ruleViolation10b == true || ruleViolation10c == true || ruleViolation14a == true || ruleViolation14b == true || 
                ruleViolation15a == true || ruleViolation15b == true || ruleViolation17a == true || ruleViolation17b == true || ruleViolation17c == true || ruleViolation17d == true || 
                ruleViolation17e == true || ruleViolation17f == true || ruleViolation17g == true || ruleViolation17h == true)
            {
                if (ruleViolation1a == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true || errorCounter == 1)
                    {
                        if (DraftFinalP == true)
                        {
                            sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  PIF Evaluation Date/Date of service {" + EvaluationDateDateOfServiceStr 
                                + "} cannot be in the future [after today].\n\r";
                        }
                        else
                        {
                            sentOptionObject.ErrorMesg += "Evaluation date/Date of service {" + EvaluationDateDateOfServiceStr 
                                + "} cannot be in the future.\n\r";
                        }
                    }
                }
                // The first 8 rules correspond to MBHP Date Rules Document Rule Number: 2a.
                if (ruleViolation2a1 == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true || errorCounter == 1)
                    {
                        if (DraftFinalP == true)
                        {
                            sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  Assessment.ED Arrival {" + EDArrivalDayStr + ", " + EDArrivalTimeStr 
                                + "} cannot be later than Disposition Details.Psych Phone Consult Requested {" + PsychPhoneConsultRequestedDateStr + ", " + PsychPhoneConsultRequestedTimeStr + "}.\n\r";
                        }
                        else
                        {
                            sentOptionObject.ErrorMesg += "ED Arrival {" + EDArrivalDayStr + ", " + EDArrivalTimeStr 
                                + "} cannot be later than Psych Phone Consult Requested {" + PsychPhoneConsultRequestedDateStr + ", " + PsychPhoneConsultRequestedTimeStr + "}.\n\r";
                        }
                    }
                }
                if (ruleViolation2a2 == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true || errorCounter == 1)
                    {
                        if (DraftFinalP == true)
                        {
                            sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  Assessment.ED Arrival: {" + EDArrivalDayStr + ", " + EDArrivalTimeStr 
                                + "} cannot be later than Disposition Details.Psych Consult Began {" + PsychConsultBeganDateStr + ", " + PsychConsultBeganTimeStr + "}.\n\r";
                        }
                        else
                        {
                            sentOptionObject.ErrorMesg += "ED Arrival {" + EDArrivalDayStr + ", " + EDArrivalTimeStr 
                                + "} cannot be later than Psych Consult Began {" + PsychConsultBeganDateStr + ", " + PsychConsultBeganTimeStr + "}.\n\r";
                        }
                    }
                }
                if (ruleViolation2a3 == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true || errorCounter == 1)
                    {
                        if (DraftFinalP == true)
                        {
                            sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  Assessment.ED Arrival: {" + EDArrivalDayStr + ", " + EDArrivalTimeStr 
                                + "} cannot be later than Disposition Details.Psych Intervention Requested: {" + PsychInterventionRequestedDateStr + ", " + PsychInterventionRequestedTimeStr + "}.\n\r";
                        }
                        else
                        {
                            sentOptionObject.ErrorMesg += "ED Arrival {" + EDArrivalDayStr + ", " + EDArrivalTimeStr 
                                + "} cannot be later than Psych Intervention Requested {" + PsychInterventionRequestedDateStr + ", " + PsychInterventionRequestedTimeStr + "}.\n\r";
                        }
                    }
                }
                if (ruleViolation2a4 == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true || errorCounter == 1)
                    {
                        if (DraftFinalP == true)
                        {
                            sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  Assessment.ED Arrival {" + EDArrivalDayStr + ", " + EDArrivalTimeStr 
                                + "} cannot be later than Disposition Details.Psych Intervention Began {" + PsychInterventionBeganDateStr + ", " + PsychInterventionBeganTimeStr + "}.\n\r";
                        }
                        else
                        {
                            sentOptionObject.ErrorMesg += "ED Arrival {" + EDArrivalDayStr + ", " + EDArrivalTimeStr 
                                + "} cannot be later than Psych Intervention {" + PsychInterventionBeganDateStr + ", " + PsychInterventionBeganTimeStr + "}.\n\r";
                        }
                    }
                }
                if (ruleViolation2a5 == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true || errorCounter == 1)
                    {
                        if (DraftFinalP == true)
                        {
                            sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  Assessment.ED Arrival {" + EDArrivalDayStr + ", " + EDArrivalTimeStr 
                                + "} cannot be later than PIF Ready Date {" + ReadyDateStr + ", " + ReadyTimeStr + "}.\n\r";
                        }
                        else
                        {
                            sentOptionObject.ErrorMesg += "ED Arrival: {" + EDArrivalDayStr + ", " + EDArrivalTimeStr 
                                + "} cannot be later than Ready Date {" + ReadyDateStr + ", " + ReadyTimeStr + "}.\n\r";
                        }
                    }
                }
                if (ruleViolation2a6 == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true || errorCounter == 1)
                    {
                        if (DraftFinalP == true)
                        {
                            sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  Assessment.ED Arrival {" + EDArrivalDayStr 
                                + "} cannot be later than PIF Evaluation Date/Date of Service {" + EvaluationDateDateOfServiceStr + "}.\n\r";
                        }
                        else
                        {
                            sentOptionObject.ErrorMesg += "ED Arrival {" + EDArrivalDayStr 
                                + "} cannot be later than PIF Evaluation Date/Date of Service {" + EvaluationDateDateOfServiceStr + "}.\n\r";
                        }
                    }
                }
                if (ruleViolation2a7 == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true || errorCounter == 1)
                    {
                        if (DraftFinalP == true)
                        {
                            sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  Assessment.ED Arrival {" + EDArrivalDayStr + ", " + EDArrivalTimeStr 
                                + "} cannot be later than PIF Request {" + RequestDateStr + ", " + RequestTimeStr + "}.\n\r";
                        }
                        else
                        {
                            sentOptionObject.ErrorMesg += "ED Arrival {" + EDArrivalDayStr + ", " + EDArrivalTimeStr 
                                + "} cannot be later than Request {" + RequestDateStr + ", " + RequestTimeStr + "}.\n\r";
                        }
                    }
                }
                if (ruleViolation2a8 == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true || errorCounter == 1)
                    {
                        if (DraftFinalP == true)
                        {
                            sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  Assessment.ED Arrival {" + EDArrivalDayStr + ", " + EDArrivalTimeStr 
                                + "} cannot be later than PIF Evaluation Date/Date of Service, Details.Intervention Began Time {" + EvaluationDateDateOfServiceStr + ", " + InterventionBeganTimeStr + "}.\n\r";
                        }
                        else
                        {
                            sentOptionObject.ErrorMesg += "ED Arrival {" + EDArrivalDayStr + ", " + EDArrivalTimeStr 
                                + "} cannot be later than Intervention {" + EvaluationDateDateOfServiceStr + ", " + InterventionBeganTimeStr + "}.\n\r";
                        }
                    }
                }
                if (ruleViolation2b == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true || errorCounter == 1)
                    {
                        if (DraftFinalP == true)
                        {
                            sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  Assessment.ED Arrival Date {" + EDArrivalDayStr 
                                + "} cannot be more than 5 months (150 days) ago to provide one month to get the data to MBHP within the 180 day window.\n\r";
                        }
                        else
                        {
                            sentOptionObject.ErrorMesg += "ED Arrival Day: {" + EDArrivalDayStr 
                                + "} cannot be more than 5 months (150 days) ago to provide one month to get the data to MBHP within the 180 day window.\n\r";
                        }
                    }
                }
                if (ruleViolation3a == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true || errorCounter == 1)
                    {
                        if (DraftFinalP == true)
                        {
                            sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  PIF Request Date {" + RequestDateStr 
                                + "} cannot be more than 5 months (150 days) ago to provide one month to get the data to MBHP within the 180 day window.\n\r";
                        }
                        else
                        {
                            sentOptionObject.ErrorMesg += "Request Date {" + RequestDateStr 
                                + "} cannot be more than 5 months (150 days) ago to provide one month to get the data to MBHP within the 180 day window.\n\r";
                        }
                    }
                }
                if (ruleViolation4a == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true || errorCounter == 1)
                    {
                        if (DraftFinalP == true)
                        {
                            sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  PIF Ready {" + ReadyDateStr + ", " + ReadyTimeStr 
                                + "} cannot be before PIF Request {" + RequestDateStr + ", " + RequestTimeStr + "}.\n\r";
                        }
                        else
                        {
                            sentOptionObject.ErrorMesg += "Ready {" + ReadyDateStr + ", " + ReadyTimeStr 
                                + "} cannot be before Request {" + RequestDateStr + ", " + RequestTimeStr + "}.\n\r";
                        }
                    }
                }
                if (ruleViolation4b == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true || errorCounter == 1)
                    {
                        if (DraftFinalP == true)
                        {
                            sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  PIF Ready DateTime {" + ReadyDateStr + ", " + ReadyTimeStr 
                                + "} cannot be later than PIF Evaluation Date/Date of Service, DISPOSITION DETAILS Intervention Began Time {" + EvaluationDateDateOfServiceStr + ", " + InterventionBeganTimeStr + "}.\n\r";
                        }
                        else
                        {
                            sentOptionObject.ErrorMesg += "Ready {" + ReadyDateStr + ", " + ReadyTimeStr 
                                + "} cannot be later than PIF Evaluation Date/Date of Service, Intervention Began {" + EvaluationDateDateOfServiceStr + ", " + InterventionBeganTimeStr + "}.\n\r";
                        }
                    }
                }
                if (ruleViolation5a == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true || errorCounter == 1)
                    {
                        if (DraftFinalP == true)
                        {
                            sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  PIF Evaluation Date/Date of Service, Disposition Details.Intervention Began Time {" + EvaluationDateDateOfServiceStr + ", " + InterventionBeganTimeStr 
                                + "} cannot be more than 24 hours after the PIF Ready {" + ReadyDateStr + ", " + ReadyTimeStr + "}.\n\r";
                        }
                        else
                        {
                            sentOptionObject.ErrorMesg += "Intervention {" + EvaluationDateDateOfServiceStr + ", " + InterventionBeganTimeStr 
                                + "} cannot start more than 24 hours after PIF Ready {" + ReadyDateStr + ", " + ReadyTimeStr + "}.\n\r";
                        }
                    }
                }
                if (ruleViolation6a1 == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true || errorCounter == 1)
                    {
                        if (DraftFinalP == true)
                        {
                            sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  Disposition Details.Service Duration {" + ServiceDurationStr 
                                + "} (minutes), cannot be more than 360 minutes.\n\r";
                        }
                        else
                        {
                            sentOptionObject.ErrorMesg += "Service Duration {" + ServiceDurationStr 
                                + "} (minutes) cannot be more than 360 minutes.\n\r";
                        }
                    }
                }
                if (ruleViolation6a2 == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true || errorCounter == 1)
                    {
                        if (DraftFinalP == true)
                        {
                            sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  Disposition Details.Service Duration {" + ServiceDurationStr 
                                + "} (minutes) cannot be less than 10 minutes.\n\r";
                        }
                        else
                        {
                            // Note this error message never fires, because rule6a2 only fires after all component service durations have been provided, i.e. when DraftFinalP = true.
                            sentOptionObject.ErrorMesg += "Service Duration {" + ServiceDurationStr 
                                + "} (minutes) cannot be less than 10 minutes.\n\r";
                        }
                    }
                }
                if (ruleViolation8a == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true || errorCounter == 1)
                    {
                        if (DraftFinalP == true)
                        {
                            sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  Disposition Details.Psych Consult Began {" + PsychConsultBeganDateStr + ", " + PsychConsultBeganTimeStr 
                                + "} cannot be before Psych Phone Consult Requested {" + PsychPhoneConsultRequestedDateStr + ", " + PsychPhoneConsultRequestedTimeStr + "}.\n\r";
                        }
                        else
                        {
                            sentOptionObject.ErrorMesg += "Psych Consult Began {" + PsychConsultBeganDateStr + ", " + PsychConsultBeganTimeStr 
                                + "} cannot be before Psych Phone Consult Requested {" + PsychPhoneConsultRequestedDateStr + ", " + PsychPhoneConsultRequestedTimeStr + "}.\n\r";
                        }
                    }
                }
                if (ruleViolation7b == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true || errorCounter == 1)
                    {
                        if (DraftFinalP == true)
                        {
                            sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  Disposition Details.Psych Phone Consult Requested {" + PsychPhoneConsultRequestedDateStr 
                                + "} cannot be more than 5 months (150 days) ago to provide one month to get the data to MBHP within the 180 day window.\n\r";
                        }
                        else
                        {
                            sentOptionObject.ErrorMesg += "Psych Phone Consult cannot be more than 5 months (150 days) ago.\n\r";
                        }
                    }
                }
                if (ruleViolation7c == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true || errorCounter == 1)
                    {
                        if (DraftFinalP == true)
                        {
                            sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  Disposition Details.Psych Phone Consult Requested {" + PsychPhoneConsultRequestedDateStr + ", " + PsychPhoneConsultRequestedTimeStr 
                                + "} cannot be before PIF Evaluation Date/Date of Service, Intervention Began {" + EvaluationDateDateOfServiceStr + ", " + InterventionBeganTimeStr + "}.\n\r";
                        }
                        else
                        {
                            sentOptionObject.ErrorMesg += "Psych Phone Consult Requested {" + PsychPhoneConsultRequestedDateStr + ", " + PsychPhoneConsultRequestedTimeStr 
                                + "} cannot be before PIF Evaluation Date/Date of Service, Intervention Began {" + EvaluationDateDateOfServiceStr + ", " + InterventionBeganTimeStr + "}.\n\r";
                        }
                    }
                }
                if (ruleViolation7d == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true || errorCounter == 1)
                    {
                        if (DraftFinalP == true)
                        {
                            sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  Disposition Details.Psych Phone Consult Requested Date {" + PsychPhoneConsultRequestedDateStr 
                                + "} cannot be before PIF Evaluation Date/Date of Service {" + EvaluationDateDateOfServiceStr + "}.\n\r";
                        }
                        else
                        {
                            sentOptionObject.ErrorMesg += "Psych Phone Consult Request {" + PsychPhoneConsultRequestedDateStr 
                                + "} cannot be before PIF Evaluation Date/Date of Service {" + EvaluationDateDateOfServiceStr + "}.\n\r";
                        }
                    }
                }
                if (ruleViolation8b == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true || errorCounter == 1)
                    {
                        if (DraftFinalP == true)
                        {
                            sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  Disposition Details.Psych Consult Began {" + PsychConsultBeganDateStr + ", " + PsychConsultBeganTimeStr 
                                + "} cannot be more than 12 hours after Psych Phone Consult Requested: {" + PsychPhoneConsultRequestedDateStr + ", " + PsychPhoneConsultRequestedTimeStr + "}.\n\r";
                        }
                        else
                        {
                            sentOptionObject.ErrorMesg += "Psych Consult Began {" + PsychConsultBeganDateStr + ", " + PsychConsultBeganTimeStr 
                                + "} cannot be more than 12 hours after Psych Phone Consult Requested {" + PsychPhoneConsultRequestedDateStr + ", " + PsychPhoneConsultRequestedTimeStr + "}.\n\r";
                        }
                    }
                }
                if (ruleViolation8d == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true || errorCounter == 1)
                    {
                        if (DraftFinalP == true)
                        {
                            sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  Disposition Details.Psych Phone Consult Began Date: {" + PsychConsultBeganDateStr 
                                + "} cannot be more than 14 days after PIF Evaluation Date/Date of Service {" + EvaluationDateDateOfServiceStr + "}.\n\r";
                        }
                        else
                        {
                            sentOptionObject.ErrorMesg += "Psych Phone Consult: {" + PsychConsultBeganDateStr 
                                + "} cannot be more than 14 days after PIF Evaluation date/Date of Service {" + EvaluationDateDateOfServiceStr + "}.\n\r";
                        }
                    }
                }
                if (ruleViolation8e == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true || errorCounter == 1)
                    {
                        if (DraftFinalP == true)
                        {
                            sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  Psych Consult Began Date {" + PsychConsultBeganDateStr 
                                + "} cannot be before Psych Phone Consult Requested Date {" + PsychPhoneConsultRequestedDateStr + "}.\n\r";
                        }
                        else
                        {
                            sentOptionObject.ErrorMesg += "Psych Consult Began {" + PsychConsultBeganDateStr 
                                + "} cannot be before Psych Phone Consult Requested {" + PsychPhoneConsultRequestedDateStr + "}.\n\r";
                        }
                    }
                }
                if (ruleViolation9a == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true || errorCounter == 1)
                    {
                        if (DraftFinalP == true)
                        {
                            sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  Details.Psych Intervention Requested Date {" + PsychInterventionRequestedDateStr 
                                + "} cannot be more than 5 months (150 days) ago to provide one month to get the data to MBHP within the 180 day window.\n\r";
                        }
                        else
                        {
                            sentOptionObject.ErrorMesg += "Psych Intervention {" + PsychInterventionRequestedDateStr 
                                + "} cannot be more than 5 months (150 days) ago.\n\r";
                        }
                    }
                }
                if (ruleViolation9b == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true || errorCounter == 1)
                    {
                        if (DraftFinalP == true)
                        {
                            sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  Disposition Details.Psych Intervention Requested {" + PsychInterventionRequestedDateStr + ", " + PsychInterventionRequestedTimeStr 
                                + "} cannot be before PIF Evaluation Date/Date of Service, Intervention Began Time: {" + EvaluationDateDateOfServiceStr + ", " + InterventionBeganTimeStr + "}.\n\r";
                        }
                        else
                        {
                            sentOptionObject.ErrorMesg += "Psych Intervention Requested {" + PsychInterventionRequestedDateStr + ", " + PsychInterventionRequestedTimeStr 
                                + "} cannot be before Intervention Began {" + EvaluationDateDateOfServiceStr + ", " + InterventionBeganTimeStr + "}.\n\r";
                        }
                    }
                }
                if (ruleViolation9c == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true || errorCounter == 1)
                    {
                        if (DraftFinalP == true)
                        {
                            sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  Disposition Details.Psych Intervention Requested Date {" + PsychInterventionRequestedDateStr 
                                + "} cannot be before PIF Evaluation Date/Date of Service {" + EvaluationDateDateOfServiceStr + "}.\n\r";
                        }
                        else
                        {
                            sentOptionObject.ErrorMesg += "Psych Intervention {" + PsychInterventionRequestedDateStr 
                                + "} cannot be before PIF Evaluation Date/Date of Service {" + EvaluationDateDateOfServiceStr + "}.\n\r";
                        }
                    }
                }
                if (ruleViolation10a == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true || errorCounter == 1)
                    {
                        if (DraftFinalP == true)
                        {
                            sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  Disposition Details.Psych Intervention Began: {" + PsychInterventionBeganDateStr + ", " + PsychInterventionBeganTimeStr 
                                + "} cannot be before Psych Intervention Requested: {" + PsychInterventionRequestedDateStr + ", " + PsychInterventionRequestedTimeStr + "}.\n\r";
                        }
                        else
                        {
                            sentOptionObject.ErrorMesg += "Psych Intervention Began {" + PsychInterventionBeganDateStr + ", " + PsychInterventionBeganTimeStr 
                                + "} cannot be before Psych Intervention Requested {" + PsychInterventionRequestedDateStr + ", " + PsychInterventionRequestedTimeStr + "}.\n\r";
                        }
                    }
                }
                if (ruleViolation10b == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true || errorCounter == 1)
                    {
                        if (DraftFinalP == true)
                        {
                            sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  Disposition Details.Psych Intervention Began: {" + PsychInterventionBeganDateStr + ", " + PsychInterventionBeganTimeStr 
                                + "} cannot be more than 12 hours after Psych Intervention Requested: {" + PsychInterventionRequestedDateStr + ", " + PsychInterventionRequestedTimeStr + "}.\n\r";
                        }
                        else
                        {
                            sentOptionObject.ErrorMesg += "Psych Intervention Began {" + PsychInterventionBeganDateStr + ", " + PsychInterventionBeganTimeStr 
                                + "} cannot be more than 12 hours after Psych Intervention Requested {" + PsychInterventionRequestedDateStr + ", " + PsychInterventionRequestedTimeStr + "}.\n\r";
                        }
                    }
                }
                if (ruleViolation10c == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true || errorCounter == 1)
                    {
                        if (DraftFinalP == true)
                        {
                            sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  Disposition Details.Psych Intervention Began Date: {" + PsychInterventionBeganDateStr 
                                + "} cannot be more than 14 days after PIF Evaluation Date/Date of Service {" + EvaluationDateDateOfServiceStr + "}.\n\r";
                        }
                        else
                        {
                            sentOptionObject.ErrorMesg += "Psych Intervention {" + PsychInterventionBeganDateStr 
                                + "} cannot be more than 14 days after PIF Evaluation Date/Date of Service {" + EvaluationDateDateOfServiceStr + "}.\n\r";
                        }
                    }
                }
                if (ruleViolation14a == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true || errorCounter == 1)
                    {
                        if (DraftFinalP == true)
                        {
                            sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  Primary Payer/Insurance RID Number must be 12 characters in length.\n\r";
                        }
                        else
                        {
                            sentOptionObject.ErrorMesg += "RID must be 12 characters.\n\r";
                        }
                    }
                }
                if (ruleViolation14b == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true || errorCounter == 1)
                    {
                        if (DraftFinalP == true)
                        {
                            sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  Additional Insurance RID Number must be 12 characters in length.\n\r";
                        }
                        else
                        {
                            sentOptionObject.ErrorMesg += "Secondary RID must be 12 characters.\n\r";
                        }
                    }
                }
                if (ruleViolation15a == true)
                {
                    // BirthDateP = true;
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true || errorCounter == 1)
                    {
                        if (DraftFinalP == true)
                        {
                            sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  Client must be at least one year old, PIF Date of Birth is currently set to {" + DateOfBirthStr + "}.\n\r";
                        }
                        else
                        {
                            sentOptionObject.ErrorMesg += "Client must be at least one year old, Date of Birth currently set to {" + DateOfBirthStr + "}.\n\r";
                        }
                    }
                }
                if (ruleViolation15b == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true || errorCounter == 1)
                    {
                        if (DraftFinalP == true)
                        {
                            sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) 
                                + ".  Client cannot be in their 150th year or older.  PIF Date of Birth is currently set to {" + DateOfBirthStr + "}.\n\r";
                        }
                        else
                        {
                            sentOptionObject.ErrorMesg += "Client cannot be over 150, {" + DateOfBirthStr + "}.\n\r";
                        }
                    }
                }
                if (ruleViolation17a == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  Disposition Details.Psych Phone Consult Requested Date must have associated Psych Phone Consult Requested Time.\n\r";
                    }
                }
                if (ruleViolation17b == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  Disposition Details.Psych Consult Began Date must have associated Psych Consult Began Time.\n\r";
                    }
                }
                if (ruleViolation17c == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  Disposition Details.Psych Intervention Requested Date must have associated Psych Intervention Requested Time.\n\r";
                    }
                }
                if (ruleViolation17d == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  Disposition Details.Psych Intervention Began Date must have associated Psych Intervention Began Time.\n\r";
                    }
                }
                if (ruleViolation17e == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  Disposition Details.Psych Phone Consult Requested Time must have associated Psych Phone Consult Requested Date.\n\r";
                    }
                }
                if (ruleViolation17f == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  Disposition Details.Psych Consult Began Time must have associated Psych Consult Began Date.\n\r";
                    }
                }
                if (ruleViolation17g == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  Disposition Details.Psych Intervention Requested Time must have associated Psych Intervention Requested Date.\n\r";
                    }
                }
                if (ruleViolation17h == true)
                {
                    if (errorCounter == 0)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalPrefixStr;
                    }
                    ++errorCounter;
                    if (DraftFinalP == true)
                    {
                        sentOptionObject.ErrorMesg += draftOrFinalStr + Convert.ToString(errorCounter) + ".  Disposition Details.Psych Intervention Began Time must have associated Psych Intervention Began Date.\n\r";
                    }
                }
                if (DraftFinalP == true) // || BirthDateP == true)
                {
                    sentOptionObject.ErrorCode = 1;
                }
                else
                {
                    sentOptionObject.ErrorCode = 3;
                }
                sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("01:00 AM", "1 am"); sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("02:00 AM", "2 am"); sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("03:00 AM", "3 am");
                sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("04:00 AM", "4 am"); sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("05:00 AM", "5 am"); sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("06:00 AM", "6 am");
                sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("07:00 AM", "7 am"); sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("08:00 AM", "8 am"); sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("09:00 AM", "9 am");
                sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("10:00 AM", "10 am"); sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("11:00 AM", "11 am"); sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("12:00 AM", "12 am");
                sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("01:00 PM", "1 pm"); sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("02:00 PM", "2 pm"); sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("03:00 PM", "3 pm");
                sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("04:00 PM", "4 pm"); sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("05:00 PM", "5 pm"); sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("06:00 PM", "6 pm");
                sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("07:00 PM", "7 pm"); sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("08:00 PM", "8 pm"); sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("09:00 PM", "9 pm");
                sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("10:00 PM", "10 pm"); sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("11:00 PM", "11 pm"); sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("12:00 PM", "12 pm");
                sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("01:", "1:"); sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("02:", "2:"); sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("03:", "3:");
                sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("04:", "4:"); sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("05:", "5:"); sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("06:", "6:");
                sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("07:", "7:"); sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("08:", "8:"); sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("09:", "9:");
                sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("01/", "1/"); sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("02/", "2/"); sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("03/", "3/");
                sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("04/", "4/"); sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("05/", "5/"); sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("06/", "6/");
                sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("07/", "7/"); sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("08/", "8/"); sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg.Replace("09/", "9/");

                return CompleteOptionObject(sentOptionObject, sentOptionObject, true, false);
             }
            return CompleteOptionObject(sentOptionObject, sentOptionObject, true, false);
        }


        public static Boolean MBHPDateRules(String date1, String time1, String date2, String time2, String ruleNumber)
        {
            Boolean retVal = false;
            String ampm1, ampm2;
            Int32 year1, year2, month1, month2, day1, day2, hour1, hour2, minute1, minute2, ymd1, ymd2, t1, t2;

            // Rule 15a, 15b confirms the client is at least one year old and not quite 150 years old.
            if ((ruleNumber.Equals("rule1a") == true) || (ruleNumber.Equals("rule15a") == true) || (ruleNumber.Equals("rule15b") == true))
            {
                if ((string.IsNullOrEmpty(date1) == true) || (date1.Equals("01/01/1900") == true))
                {
                    return (retVal);
                }

                /* ymd1 is for the date of birth, or Evaluation Date/Date of service */
                year1 = Convert.ToInt32(date1.Substring(6, 4));
                month1 = Convert.ToInt32(date1.Substring(0, 2));
                day1 = Convert.ToInt32(date1.Substring(3, 2));
                ymd1 = year1 * 10000 + month1 * 100 + day1;

                /* ymd2 is for the current date */
                DateTime today = System.DateTime.Today;
                year2 = today.Year;
                month2 = today.Month;
                day2 = today.Day;
                ymd2 = year2 * 10000 + month2 * 100 + day2;

                /* we compare the current date to the date of birth to confirm the client is one year old or more, if not then there is an error. */
                if ((ruleNumber.Equals("rule15a") == true) && ((ymd2 - ymd1) < 10000)) // here 10000 represents one year because in ymd format above, we multiply the year by 10000.
                                                                                       // God, please let this be right because its kind of cheesy.
                {
                    return (true);
                }
                else if ((ruleNumber.Equals("rule15b") == true) && ((ymd2 - ymd1) > 1490000)) // here 1,490,000 represents 149 years because in ymd format above, we multiply the year by 10000.
                                                                                              // again God, let this be right or at least ok because this rule is cheesier than the one above.
                                                                                              // what I think this says is if the date of birth is more than 149 years ago, then current date 20191008 (ymd2)
                                                                                              // minus birth date (ymd1) of say oh I don't know maybe 18700101 or prior is greater than 1,490,000, then this 
                                                                                              // geyser is older than 149 years old and, hence, is not eligible for MBHP funded hospital emergency room based 
                                                                                              // psychiatric intervention, consultation, bed search, placement or disposition.  This one needs to be disposed
                                                                                              // to an old folks home.
                {
                    return (true);
                }
                else if (ruleNumber.Equals("rule1a") == true && (ymd1 > ymd2)) // Rule 1a states: The Date of service cannot be in the future.
                {
                    return (true);
                }
                return (retVal);
            }
            /* ruleViolation6a = MBHPDateRules(ServiceDurationStr, null, null, null, "rule6a");*/

            // Rule 6a uses service duration to calculate minutes.
            if (ruleNumber.Equals("rule6a1") == true || ruleNumber.Equals("rule6a2") == true)
            {
                if (string.IsNullOrEmpty(date1))
                    return (retVal);
                else
                {
                    minute1 = Convert.ToInt32(date1);
                    // Minutes is 14 days (times 24 hours per day times 60 minutes per hour):
					// Updated rule6a1 to be 6 hours in one day = 360 minutes, not 14 days as before
					// Also rule6a2 prevents user from trying to enter hours for duration.
                    minute2 = 360;
                    if ((ruleNumber.Equals("rule6a1") && minute1 > minute2) || (ruleNumber.Equals("rule6a2") && minute1 < 10))
                    {
                        return (true);
                    }
                    else
                    {
                        return (retVal);
                    }
                }
            }

            if (ruleNumber.Equals("rule2b") == true || ruleNumber.Equals("rule3a") == true || ruleNumber.Equals("rule7b") == true || ruleNumber.Equals("rule9a") == true)
            {
                if (string.IsNullOrEmpty(date1) || string.IsNullOrEmpty(time1))
                {
                    return (retVal);
                }
                else
                {
                    System.DateTime dtm1 = new System.DateTime(Convert.ToInt32(date1.Substring(6, 4)), Convert.ToInt32(date1.Substring(0, 2)), Convert.ToInt32(date1.Substring(3, 2)), Convert.ToInt32(time1.Substring(0, 2)), Convert.ToInt32(time1.Substring(3, 2)), 0);
                    if (DateTime.Now.Subtract(dtm1).Days <= (180 - 30))
                    {
                        return (retVal);
                    }
                    else
                    {
                        return (true);
                    }
                }
            }

            // here we return if both fields are not available.
            if (time1.Equals("00:00 AM") == true || date1.Equals("01/01/1900") == true || time2.Equals("00:00 AM") == true || date2.Equals("01/01/1900") == true)
            {
                return retVal;
            }
            else
            {
                if (string.IsNullOrEmpty(date1) || string.IsNullOrEmpty(time1) || string.IsNullOrEmpty(date2) || string.IsNullOrEmpty(time2))
                {
                    return retVal;
                }
            }

            /* Set the local date/time strings from the input date/time strings for date1/time1 */
            year1 = Convert.ToInt32(date1.Substring(6, 4));
            month1 = Convert.ToInt32(date1.Substring(0, 2));
            day1 = Convert.ToInt32(date1.Substring(3, 2));
            hour1 = Convert.ToInt32(time1.Substring(0, 2));
            minute1 = Convert.ToInt32(time1.Substring(3, 2));
            ampm1 = time1.Substring(6, 2);

            /* Set the local date from the input date string for date2.  If date2 is today, then set the local date to today */
            if (date2.Equals("today") == true)
            {
                /* ymd2 is for the current date */
                DateTime today = System.DateTime.Today;
                year2 = today.Year;
                month2 = today.Month;
                day2 = today.Day;
            }
            else
            {
                year2 = Convert.ToInt32(date2.Substring(6, 4));
                month2 = Convert.ToInt32(date2.Substring(0, 2));
                day2 = Convert.ToInt32(date2.Substring(3, 2));
            }

            /* Set the local time strings from the input time string for time2 */
            hour2 = Convert.ToInt32(time2.Substring(0, 2));
            minute2 = Convert.ToInt32(time2.Substring(3, 2));
            ampm2 = time2.Substring(6, 2);

            // Note the importance of accounting for PM hours (excluding 12 PM).
            if (ampm1.Equals("PM") == true && hour1 < 12)
            {
                hour1 += 12;
            }
            if (ampm2.Equals("PM") == true && hour2 < 12)
            {
                hour2 += 12;
            }

            // Note the importance of accounting for 12 AM as well.
            if (ampm1.Equals("AM") == true && hour1 == 12)
            {
                hour1 = 0;
            }
            if (ampm2.Equals("AM") == true && hour2 == 12)
            {
                hour2 = 0;
            }
            System.DateTime dt1 = new System.DateTime(year1, month1, day1, hour1, minute1, 0);
            System.DateTime dt2 = new System.DateTime(year2, month2, day2, hour2, minute2, 0);

            System.TimeSpan diff = dt2.Subtract(dt1);

            t1 = hour1 * 60 + minute1;
            t2 = hour2 * 60 + minute2;
 
            // Rule5a compares the PIF ReadyDate/Time to the PIF Evaluation Date/Date of Service AND the Disposition Details Intervention Began Time.  This observation is important.
            // ruleViolation5a = MBHPDateRules(ReadyDateStr, ReadyTimeStr, EvaluationDateDateOfServiceStr, InterventionBeganTimeStr, "rule5a");
            // If the difference is more than 24 hours then diff.Days will be 1 or greater, return true.
            if (ruleNumber.Equals("rule5a") == true)
            {
                if (diff.Days > 0)
                {
                    return (true);
                }
                else
                {
                    return (retVal);
                }
            }

            // rule 8b and 10b keep the begin dates within 12 hours of the request dates:
            else if (ruleNumber.Equals("rule8b") == true || ruleNumber.Equals("rule10b") == true)
            {
                if (diff.Days > 0 || (diff.Days == 0 && diff.Hours > 12) || (diff.Days == 0 && diff.Hours == 12 && diff.Minutes > 0))
                {
                    return (true);
                }
                else
                {
                    return (retVal);
                }
            }

            // rule 8d and 10c keep the details begin dates within reasonable range because once the Pif is saved there's no turning back.  
            // Here we are saying these activities have to begin within the max service duration (336 hours = 14 days).
            else if (ruleNumber.Equals("rule8d") == true || ruleNumber.Equals("rule10c") == true)
            {
                if (diff.Days > 14)
                {
                    return (true);
                }
                else
                {
                    return (retVal);
                }
            }

            // The default rule is applied to rules: 2a1, 2a2, 2a3, 2a4, 2a5, 2a6, 2a7, 2a8, 4a, 4b, 8a, 10a.
            // Essentially the default is applied to all rules except: 5a, 8b,d and 10b,c 
            // The default rule compares the dates and if days, hours or minutes are greater than 0, then return true.  else return false because the dates are chronologically alligned.
            else
            {
                if (diff.Days > 0 || (diff.Days == 0 && diff.Hours > 0) || (diff.Days == 0 && diff.Hours == 0 && diff.Minutes >= 0))
                {
                    return (retVal);
                }
                else
                {
                    return (true);
                }
            }
        }

      
  

        public static OptionObject2 CompleteOptionObject(OptionObject2 sentOptionObject, OptionObject2 returnOptionObject, bool setRecommended, bool setNotRecommended)
        {

            // The REQUIRED properties.
            var completedOptionObject = new OptionObject2
            {
                EntityID = sentOptionObject.EntityID,
                Facility = sentOptionObject.Facility,
                NamespaceName = sentOptionObject.NamespaceName,
                OptionId = sentOptionObject.OptionId,
                ParentNamespace = sentOptionObject.ParentNamespace,
                ServerName = sentOptionObject.ServerName,
                SystemCode = sentOptionObject.SystemCode
            };

            // The RECOMMENDED properties.
            if (setRecommended == true)
            {
                completedOptionObject.EpisodeNumber = sentOptionObject.EpisodeNumber;
                completedOptionObject.OptionStaffId = sentOptionObject.OptionStaffId;
                completedOptionObject.OptionUserId = sentOptionObject.OptionUserId;

                // If the returnOptionObject has data, use that to complete the completedOptionObject. Otherwise, use
                // the data that exists in the sentOptionObject.
                if (returnOptionObject.ErrorCode >= 1)
                {
                    completedOptionObject.ErrorCode = returnOptionObject.ErrorCode;
                    completedOptionObject.ErrorMesg = returnOptionObject.ErrorMesg;
                }
                else
                {
                    completedOptionObject.ErrorCode = sentOptionObject.ErrorCode;
                    completedOptionObject.ErrorMesg = sentOptionObject.ErrorMesg;
                }
            }

            // The NOT RECOMMENDED properties.
            if (setNotRecommended == true)
            {
                completedOptionObject.Forms = sentOptionObject.Forms;
            }

            return completedOptionObject;
        }
    }
}
