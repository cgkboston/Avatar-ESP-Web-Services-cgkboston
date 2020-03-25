// PROJECT: FinalDispositionWS
// FILENAME: FinalDispositionWS.asmx.cs
//    BUILD: 200205
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
//using System.Data.Odbc;
//using System.Collections.Generic;
//using System.Text;

namespace SSMHAWS
{
    /* FinalDispositionWS.asmx.cs
     * A Web Service for Netsmart's Avatar EHR that does various things with Final Disposition date fields.
     * Note Build string format is YYMMDDHHMM, not YYYYMMDDHHMM for consistency with the original CBAN implementation.
     * Build 2002050840 - Monday February 5, 2020 8:40 am
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

    public class FinalDispositionWS : WebService
    {
        /// <summary>Main logic for the FinalDispositionWS Web Service - REQUIRED BY AVATAR!.</summary>
        /// <param name="sentOptionObject">The OptionObject2 sent from Avatar.</param>
        /// <param name="scriptAction">The passed script action (FinalDispositionSA).</param>
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
            String HelloWorldStr = String.Empty;
            //Int32 fieldValue = 0;
            switch (scriptAction)
            {
                case "FinalDispositionSA":
                case "FinalDispositionSubmit":
                    return FinalDispositionScriptAction(sentOptionObject, scriptAction);
                /* This was just a test of odbc, this is not recommended for security and performance reasons.*/
                /*case "HelloWorld":
                    sentOptionObject.ErrorCode = 3; // 3: Returns an Error Message with OK button.
                    // sentOptionObject.ErrorMesg = "[Hello World]";
                    OdbcConnection DbConnection = new OdbcConnection("DSN=AVPM1972SSMH-SANDBOX");
                    DbConnection.Open();
                    OdbcCommand DbCommand = DbConnection.CreateCommand();
                    DbCommand.CommandText = "SELECT getdate()";
                    OdbcDataReader DbReader = DbCommand.ExecuteReader();
                    int fCount = DbReader.FieldCount;
                    for (int i = 0; i < fCount; i++){String fName = DbReader.GetName(i);} 
                    while (DbReader.Read())
                    {
                        for (int i = 0; i < fCount; i++)
                        {
                            HelloWorldStr += DbReader.GetString(i);
                        }
                    }
                    DbReader.Close();
                    DbCommand.Dispose();
                    DbConnection.Close();
                    sentOptionObject.ErrorMesg = "Hello world? hopefully this shows the current database server time stamp: [" + HelloWorldStr + "]";
                    return CompleteOptionObject(sentOptionObject, sentOptionObject, true, false);
                */
                case "Display":
                    // To See what the web service processes change the script action from FinalDispositionSA to Display.
                    return FinalDispositionScriptAction(sentOptionObject, "Display");
                case "OldDisplay":
                    // Use oldDisplay script action during initial development to get a handle on the myAvatar form ids, section names and field numbers.
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

                            if (FormID.Equals("171") == true)
                            {
                                FormName = String.Copy("Final Disposition");
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
            return "Build 200205";
        }

        /// <summary>Process the final disposition dates and return any misalligned dates per the MBHP portal date rules.</summary>
        /// <param name="sentOptionObject">The Optionbject2 object.</param>
        /// <returns>A completed OptionObject2 with error code set to sentOptionObject.ErrorCode = 1 ONLY when an error is detected.  The 
        /// error message is sentOptionObject.ErrorMesg</returns>
        public static OptionObject2 FinalDispositionScriptAction(OptionObject2 sentOptionObject, String displayOrProcess)
        {

            //  The Field ID numbers we use are:
            const string FDSelectAssessmentPIFField = "259.8";
            const string FDFinalDispositionSecuredDateField = "259.91";
            const string FDFinalDispositionSecuredTimeField = "259.9";
            const string FDBedSearchBeganDateField = "259.82";
            const string FDBedSearchBeganTimeField = "259.83";
            const string FDPlacementSecuredDateField = "259.95";
            const string FDPlacementSecuredTimeField = "259.96";

            // Initialize rule violation Booleans
            Boolean ruleViolation11b = false;
            Boolean ruleViolation11bDateOnly = false;
            Boolean ruleViolation12a = false;
            Boolean ruleViolation12d = false;
            Boolean ruleViolation12dDateOnly = false;
            Boolean ruleViolation13a = false;
            Boolean ruleViolation13c = false;
            Boolean ruleViolation13d = false;
            Boolean ruleViolation13dDateOnly = false;
            Boolean ruleViolation13e = false;
            Boolean ruleViolation13eDateOnly = false;
            Boolean ruleViolation13f = false;

            /* Important: The FDSelectAssessmentPIFField dropdown contains the values for: PIF Date of Service, PIF Intervention Began Time, PIF Ready Date/Time, 
             * PIF Psych Interv Began Date/Time, PIF Psych Consult Began Date/Time and PIF Service Duration.  These values are passed in FDSelectAssessmentPIFField
             * as ...,[DateOfService,InterventionBeganTime,ReadyDate,ReadyTime,PsychInterventionBeganDate, PsychIntervnetionBeganTime,PsychConsultBeganDate,PsychConsultBeganTime,
             * ServiceDurationMinutes],...  This web service will ignore the pre '[' data which is Child or Adult,Assessment Date,Practitioner,Entry Date and will also ignore the post ']' 
             * data which is __NMF65196.00001 (i.e. NMF Julian date.sequence number.00001). */

            String FDSelectAssessmentPIFStr = String.Empty;
            // The portion of the the Final Disposition Select Assessment PIF String which represents the required fields from the Prior Form (i.e. the ESP MCE/Child Adolescent PIF/Assessment or the ESP Adult PIF/Comprehensive Assessment).
            String PIFData = String.Empty;
            // ... Contains the following 9 fields:
            String PIFDateOfServiceStr = String.Empty;
            String PIFInterventionBeganTimeStr = String.Empty;
            String PIFReadyDateStr = String.Empty;
            String PIFReadyTimeStr = String.Empty;
            String PIFPsychiatricInterventionBeganDateStr = String.Empty;
            String PIFPsychiatricInterventionBeganTimeStr = String.Empty;
            String PIFPsychiatricConsultBeganDateStr = String.Empty;
            String PIFPsychiatricConsultBeganTimeStr = String.Empty;
            String PIFServiceDurationStr = String.Empty; // Note: Service Duration is always in minutes.
            
            // Strings to hold the remaining Final Disposition fields
            String FDFinalDispositionSecuredDateStr = String.Empty;
            String FDFinalDispositionSecuredTimeStr = String.Empty;
            String FDBedSearchBeganDateStr = String.Empty;
            String FDBedSearchBeganTimeStr = String.Empty;
            String FDPlacementSecuredDateStr = String.Empty;
            String FDPlacementSecuredTimeStr = String.Empty;

            // Variables for parsing the FDSelectAssessmentPIFStr
            Int32 StartIndex, EndIndex;
            Int32 FieldToCorrect = 0;
            Int32 year, month, day, hour, minute;
            Int32 DurationMinutes;
            Int32 CalculatedEndDateIndex = 0;
            String PIFMaxDateStr = String.Empty;
            String PIFMaxTimeStr = String.Empty;
            String ampm = String.Empty;
            String CalculatedEndDateIndexStr = String.Empty;
            String CalculatedEndDateStr = String.Empty;
            String PIFCalculatedInterventionEndedDateStr = String.Empty;
            String PIFCalculatedInterventionEndedTimeStr = String.Empty;

            foreach (var individualForm in sentOptionObject.Forms)
            {
                foreach (var individualField in individualForm.CurrentRow.Fields)
                {
                    switch (individualField.FieldNumber)
                    {
                        // FinalDisposition Section.
                        case FDSelectAssessmentPIFField:
                            FDSelectAssessmentPIFStr = Convert.ToString(individualField.FieldValue);
                            if (String.IsNullOrEmpty(FDSelectAssessmentPIFStr) == true)
                            {
                                // Here error code 3 is just an OK message box.
                                sentOptionObject.ErrorMesg = "Please select the Assessment/PIF correspoding to this final disposition prior to continuing.";
                                sentOptionObject.ErrorCode = 3;
                                return CompleteOptionObject(sentOptionObject, sentOptionObject, true, false);
                            }
                            // First Extract the PIF data from the full drop down string.
                            StartIndex = FDSelectAssessmentPIFStr.IndexOf('[') + 1;
                            EndIndex = FDSelectAssessmentPIFStr.IndexOf(']') - 1;
                            PIFData = FDSelectAssessmentPIFStr.Substring(StartIndex, EndIndex-StartIndex+1);

                            // Next extract the individual PIF field values from the PIFData

                            EndIndex = PIFData.IndexOf(',');
                            PIFDateOfServiceStr = PIFData.Substring(0, EndIndex);

                            StartIndex = PIFData.IndexOf(",") + 1; PIFData = PIFData.Substring(StartIndex); EndIndex = PIFData.IndexOf(',');
                            PIFInterventionBeganTimeStr = PIFData.Substring(0, EndIndex);

                            StartIndex = PIFData.IndexOf(",") + 1; PIFData = PIFData.Substring(StartIndex); EndIndex = PIFData.IndexOf(',');
                            PIFReadyDateStr = PIFData.Substring(0, EndIndex);

                            StartIndex = PIFData.IndexOf(",") + 1; PIFData = PIFData.Substring(StartIndex); EndIndex = PIFData.IndexOf(',');
                            PIFReadyTimeStr = PIFData.Substring(0, EndIndex);

                            StartIndex = PIFData.IndexOf(",") + 1; PIFData = PIFData.Substring(StartIndex); EndIndex = PIFData.IndexOf(',');
                            PIFPsychiatricInterventionBeganDateStr = PIFData.Substring(0, EndIndex);

                            StartIndex = PIFData.IndexOf(",") + 1; PIFData = PIFData.Substring(StartIndex); EndIndex = PIFData.IndexOf(',');
                            PIFPsychiatricInterventionBeganTimeStr = PIFData.Substring(0, EndIndex);

                            StartIndex = PIFData.IndexOf(",") + 1; PIFData = PIFData.Substring(StartIndex); EndIndex = PIFData.IndexOf(',');
                            PIFPsychiatricConsultBeganDateStr = PIFData.Substring(0, EndIndex);

                            StartIndex = PIFData.IndexOf(",") + 1;
                            PIFData = PIFData.Substring(StartIndex);
                            EndIndex = PIFData.IndexOf(',');
                            PIFPsychiatricConsultBeganTimeStr = PIFData.Substring(0, EndIndex);

                            StartIndex = PIFData.IndexOf(",") + 1;
                            PIFServiceDurationStr = PIFData.Substring(StartIndex);
                            if (string.IsNullOrWhiteSpace(PIFServiceDurationStr))
                            {
                                PIFServiceDurationStr = "0";
                            }
                            
                            // Some debug code to confirm the data values above are set correctly.
                            /* sentOptionObject.ErrorMesg = 
                                "     [PIF Date of Service: {" + PIFDateOfServiceStr + "}{" + PIFInterventionBeganTimeStr + 
                                "}] - [PIF Ready: {" + PIFReadyDateStr + "}{" + PIFReadyTimeStr +
                                "}] - [PIF Psych Intervention: {" + PIFPsychiatricInterventionBeganDateStr + "}{" + PIFPsychiatricInterventionBeganTimeStr + 
                                "}] - [PIF Psych Consult: {" + PIFPsychiatricConsultBeganDateStr + "}{" + PIFPsychiatricConsultBeganTimeStr + 
                                "}] - [PIF Service Duration: {" + PIFServiceDurationStr + 
                                "}] - [PIFData from myAvatar now is: [" + PIFData + "]";
                            sentOptionObject.ErrorCode = 3;
                            return CompleteOptionObject(sentOptionObject, sentOptionObject, true, false);
                            */
                            break;
                        case FDFinalDispositionSecuredDateField:
                            FDFinalDispositionSecuredDateStr = Convert.ToString(individualField.FieldValue);
                            break;
                        case FDFinalDispositionSecuredTimeField:
                            FDFinalDispositionSecuredTimeStr = Convert.ToString(individualField.FieldValue);
                            break;
                        case FDBedSearchBeganDateField:
                            FDBedSearchBeganDateStr = Convert.ToString(individualField.FieldValue);
                            break;
                        case FDBedSearchBeganTimeField:
                            FDBedSearchBeganTimeStr = Convert.ToString(individualField.FieldValue);
                            break;
                        case FDPlacementSecuredDateField:
                            FDPlacementSecuredDateStr = Convert.ToString(individualField.FieldValue);
                            break;
                        case FDPlacementSecuredTimeField:
                            FDPlacementSecuredTimeStr = Convert.ToString(individualField.FieldValue);
                            break;
                    }
                }
            }
            // Compare the various datetime values generated above.

            // First we take the information from the PIF and determine the max date to compare to the FD Disposition Secured Date rule 13a in the MBHP Date Rules Doc.
            // And yes in case you're curious, an array could handle much of this cleaner and more efficiently, but this is for a later date/time

            if (MBHPDateRulesFD(PIFDateOfServiceStr, PIFInterventionBeganTimeStr, PIFReadyDateStr, PIFReadyTimeStr, "rule13aPrep") == true)
            {
                PIFMaxDateStr = PIFDateOfServiceStr;
                PIFMaxTimeStr = PIFInterventionBeganTimeStr;
                FieldToCorrect = 1; // Date of Service Intervention Began
            }
            else
            {
                PIFMaxDateStr = PIFReadyDateStr;
                PIFMaxTimeStr = PIFReadyTimeStr;
                FieldToCorrect = 2; // PIF Ready
            }

            // Compare the max of Date to the psychiatric intervention began date
            if (MBHPDateRulesFD(PIFPsychiatricInterventionBeganDateStr,PIFPsychiatricInterventionBeganTimeStr, PIFMaxDateStr, PIFMaxTimeStr, "rule13aPrep") == true)
            {
                PIFMaxDateStr = PIFPsychiatricInterventionBeganDateStr;
                PIFMaxTimeStr = PIFPsychiatricInterventionBeganTimeStr;
                FieldToCorrect = 3; // Psych Intervention Began
            }

            // Compare the max of Date to the psychiatric consult began date
            if (MBHPDateRulesFD(PIFPsychiatricConsultBeganDateStr, PIFPsychiatricConsultBeganTimeStr, PIFMaxDateStr, PIFMaxTimeStr, "rule13aPrep") == true)
            {
                PIFMaxDateStr = PIFPsychiatricConsultBeganDateStr;
                PIFMaxTimeStr = PIFPsychiatricConsultBeganTimeStr;
                FieldToCorrect = 4; // Psych Consult Began
            }

            /* Currently we have PIFMaxDate/Time which is the last date/time of intervention began, ready, psych consult began and psych interv began.
             * We will compare this max value to the calculated intervention ended date time to get the final maximum date/time value to be compared to the
             * Disposition Secured Date/Time to ensure Disposition Secured Date/Time must be after all above dates (refer to MBHP Date Rules, rule13a).
             * Therefore, naturally, we first calculate the intervention ended date/time.  The following gobs of code, ending with FieldToCorrect = 5, do just that. */
            
            year = Convert.ToInt32(PIFDateOfServiceStr.Substring(6, 4));
            month = Convert.ToInt32(PIFDateOfServiceStr.Substring(0, 2));
            day = Convert.ToInt32(PIFDateOfServiceStr.Substring(3, 2));
            hour = Convert.ToInt32(PIFInterventionBeganTimeStr.Substring(0, 2));
            minute = Convert.ToInt32(PIFInterventionBeganTimeStr.Substring(3, 2));
            ampm = PIFInterventionBeganTimeStr.Substring(6, 2);

            // Note the importance of accounting for PM hours (excluding 12 PM).
            if (ampm.Equals("PM") == true && hour < 12)
            {
                hour += 12;
            }

            // Note the importance of accounting for 12 AM as well.
            if (ampm.Equals("AM") == true && hour == 12)
            {
                hour = 0;
            }
            System.DateTime CalculatedEndDate = new System.DateTime(year, month, day, hour, minute, 0);
            DurationMinutes = Int32.Parse(PIFServiceDurationStr);
            CalculatedEndDate = CalculatedEndDate.AddMinutes(DurationMinutes);

            CalculatedEndDateStr = CalculatedEndDate.ToString();
            CalculatedEndDateIndex = CalculatedEndDateStr.IndexOf(" ");
            PIFCalculatedInterventionEndedDateStr = CalculatedEndDateStr.Substring(0, CalculatedEndDateIndex);
            PIFCalculatedInterventionEndedTimeStr = CalculatedEndDateStr.Substring(CalculatedEndDateIndex + 1);

            if (PIFCalculatedInterventionEndedDateStr.Substring(1,1).Equals("/") == true)
            {
                PIFCalculatedInterventionEndedDateStr = "0" + PIFCalculatedInterventionEndedDateStr;
            }

            if (PIFCalculatedInterventionEndedDateStr.Substring(4, 1).Equals("/") == true)
            {
                PIFCalculatedInterventionEndedDateStr = PIFCalculatedInterventionEndedDateStr.Substring(0, 3) + "0" + PIFCalculatedInterventionEndedDateStr.Substring(3);
            }

            if (PIFCalculatedInterventionEndedTimeStr.Substring(1,1).Equals(":") == true)
            {
                PIFCalculatedInterventionEndedTimeStr = "0" + PIFCalculatedInterventionEndedTimeStr;
            }

            PIFCalculatedInterventionEndedTimeStr = PIFCalculatedInterventionEndedTimeStr.Substring(0, 5) + " " + PIFCalculatedInterventionEndedTimeStr.Substring(9, 2);

            /* Now that we have the calculated intervention ended date/time in the Default SQL Server Format, we compare to the max date to set the final max date from the PIF which will be the value we compare to the Disposition Secured Date/Time. */

            if (MBHPDateRulesFD(PIFCalculatedInterventionEndedDateStr,PIFCalculatedInterventionEndedTimeStr, PIFMaxDateStr, PIFMaxTimeStr, "rule13aPrep") == true)
            {
                PIFMaxDateStr = PIFCalculatedInterventionEndedDateStr;
                PIFMaxTimeStr = PIFCalculatedInterventionEndedTimeStr;
                FieldToCorrect = 5; // Calculated Intervention Ended
            }

            // Some debug code to display the information and return in the event the script action on a final disposition form field is set to "Display"
            // This may be useful for future debugging purposes or to see what the massively/overly complex code above is actually doing.
            if (displayOrProcess.Equals("Display") == true)
            {
                sentOptionObject.ErrorMesg =
                "     [PIF Date of Service {" + PIFDateOfServiceStr + "}{" + PIFInterventionBeganTimeStr +
                "}] - [PIF Ready {" + PIFReadyDateStr + "}{" + PIFReadyTimeStr +
                "}] - [PIF Psych Intervention {" + PIFPsychiatricInterventionBeganDateStr + "}{" + PIFPsychiatricInterventionBeganTimeStr +
                "}] - [PIF Psych Consult {" + PIFPsychiatricConsultBeganDateStr + "}{" + PIFPsychiatricConsultBeganTimeStr +
                "}] - [PIF Service Duration {" + PIFServiceDurationStr +
                "}]\r\n[DOS month/day/year is {" + month + "/" + day + "/" + year +
                  "}]\n[DOS hour:minute {" + hour + ":" + minute +
                "}]\r\n[PIF Calculated End Date {" + CalculatedEndDate +
                  "}]\n[PIF Calculated End Date String {" + CalculatedEndDateStr +
                  "}]\n[PIF Calculated Intervention End Date/Time {" + PIFCalculatedInterventionEndedDateStr + "}{" + PIFCalculatedInterventionEndedTimeStr +
                  "}]\n[PIF Intervention Began Date/Time {" + PIFDateOfServiceStr + "}{" + PIFInterventionBeganTimeStr +
                  "}]\n[PIFMaxDate {" + PIFMaxDateStr + "}{" + PIFMaxTimeStr +
                "}]\r\n[FD Bed Search Began {" + FDBedSearchBeganDateStr + "}{" + FDBedSearchBeganTimeStr +
                  "}]\n[FD Placement Secured {" + FDPlacementSecuredDateStr + "}{" + FDPlacementSecuredTimeStr +
                  "}]\n[FD Final Disposition Secured {" + FDFinalDispositionSecuredDateStr + "}{" + FDFinalDispositionSecuredTimeStr +
                "}]\n[displayOrProcess {" + displayOrProcess +
                "}]";
                sentOptionObject.ErrorCode = 3;
                return CompleteOptionObject(sentOptionObject, sentOptionObject, true, false);
            }

            /* Rule 11b Evaluation Date/Date of Service, Intervention Began Time cannot be later than Bed Search Began Date/Time. */
            ruleViolation11b = MBHPDateRulesFD(PIFDateOfServiceStr, PIFInterventionBeganTimeStr, FDBedSearchBeganDateStr, FDBedSearchBeganTimeStr, "rule11b");
            if (ruleViolation11b == false)
                ruleViolation11bDateOnly = MBHPDateRulesFD(PIFDateOfServiceStr, "12:00 AM", FDBedSearchBeganDateStr, "12:00 AM", "rule11bDateOnly");

            /* Rule 12a confirms the placement secured within 14 days of the when the bed search began. */
            ruleViolation12a = MBHPDateRulesFD(FDBedSearchBeganDateStr, FDBedSearchBeganTimeStr, FDPlacementSecuredDateStr, FDPlacementSecuredTimeStr, "rule12a");

            /* Rule 12d Bed Search Began Date/Time cannot be later than Placement Secured Date/Time */
            ruleViolation12d = MBHPDateRulesFD(FDBedSearchBeganDateStr, FDBedSearchBeganTimeStr, FDPlacementSecuredDateStr, FDPlacementSecuredTimeStr, "rule12d");
            if (ruleViolation12d == false)
                ruleViolation12dDateOnly = MBHPDateRulesFD(FDBedSearchBeganDateStr, "12:00 AM", FDPlacementSecuredDateStr, "12:00 AM", "rule12dDateOnly");

            /* Rule 13a confirms the disposition secured date/time must be after all above dates */

            ruleViolation13a = MBHPDateRulesFD(PIFMaxDateStr, PIFMaxTimeStr, FDFinalDispositionSecuredDateStr, FDFinalDispositionSecuredTimeStr, "rule13a");

            /* Rule 13c confirms the disposition secured date/time is withing 30 days of the intervention began date/time */
 
            ruleViolation13c = MBHPDateRulesFD(PIFDateOfServiceStr, PIFInterventionBeganTimeStr, FDFinalDispositionSecuredDateStr, FDFinalDispositionSecuredTimeStr, "rule13c");

            /* Rule 13d confirms the disposition secured date/time cannot be before placement secured date/time. */

            ruleViolation13d = MBHPDateRulesFD(FDPlacementSecuredDateStr, FDPlacementSecuredTimeStr, FDFinalDispositionSecuredDateStr, FDFinalDispositionSecuredTimeStr, "rule13d");
            /* Here since placement secured date / time are not required fields, we consider the case where date is provided and time is left blank. 
             * This confirms the disposition secured date cannot be before the placement secured date. */
            if (ruleViolation13d == false)
                ruleViolation13dDateOnly = MBHPDateRulesFD(FDPlacementSecuredDateStr, "12:00 AM", FDFinalDispositionSecuredDateStr, "12:00 AM", "rule13dDateOnly");

            /* Rule 13e confirms the disposition secured date/time must be later than the Bed Search Began Date/Time. */

            ruleViolation13e = MBHPDateRulesFD(FDBedSearchBeganDateStr, FDBedSearchBeganTimeStr, FDFinalDispositionSecuredDateStr, FDFinalDispositionSecuredTimeStr, "rule13e");
            /* Here since placement secured date / time are not required fields, we consider the case where date is provided and time is left blank. 
             * This confirms the disposition secured date cannot be before the bed saerch began date. */
            if (ruleViolation13e == false)
                ruleViolation13eDateOnly = MBHPDateRulesFD(FDBedSearchBeganDateStr, "12:00 AM", FDFinalDispositionSecuredDateStr, "12:00 AM", "rule13eDateOnly");

            /* Rule 13f confirms the Final Disposition Date/Time is not in the future.  This rule is Date/Time Only because both are required. */
            ruleViolation13f = MBHPDateRulesFD(FDFinalDispositionSecuredDateStr, FDFinalDispositionSecuredTimeStr, null, null, "rule13f");

            /* Return an information message box to the user and reset any and all invalid date fields. */
            if (ruleViolation11b == true || ruleViolation11bDateOnly == true || ruleViolation12a == true || ruleViolation12d == true || ruleViolation12dDateOnly == true || 
                ruleViolation13a == true || ruleViolation13c == true || ruleViolation13d == true || ruleViolation13dDateOnly == true || ruleViolation13e == true || 
                ruleViolation13eDateOnly == true || ruleViolation13f == true)
            {
                // If any of the date rules are violated, reset the corresponding date filed and return a corresponding message.

                // ** TODO #1 ** RESET THE OFFENDING FIELD ON THE FORM DO THIS OR ATTEMPT TO DO THIS NEXT WEEK **
                // The first few rules only account for the MBHP Date Rules Document Rule Number: 2a.
                if (ruleViolation11b == true)
                    sentOptionObject.ErrorMesg = "PIF Evaluation Date/Date of Service, Intervention Began Time {" + PIFDateOfServiceStr + ", " + PIFInterventionBeganTimeStr 
                        + "} cannot be later than the Bed Search Began Date/Time {" + FDBedSearchBeganDateStr + ", " + FDBedSearchBeganTimeStr + "}.";
                else if (ruleViolation11bDateOnly == true)
                    sentOptionObject.ErrorMesg = "PIF Evaluation Date/Date of Service {" + PIFDateOfServiceStr 
                        + "} cannot be later than Bed Search Began Date {" + FDBedSearchBeganDateStr + "}.";
                else if (ruleViolation12d == true)
                    sentOptionObject.ErrorMesg = "Bed Search Began Date/Time {" + FDBedSearchBeganDateStr + ", " + FDBedSearchBeganTimeStr 
                        + "} cannot be later than Placement Secured Date/Time {" + FDPlacementSecuredDateStr + ", " + FDPlacementSecuredTimeStr + "}.";
                else if (ruleViolation12dDateOnly == true)
                    sentOptionObject.ErrorMesg = "Bed Search Began Date {" + FDBedSearchBeganDateStr 
                        + "} cannot be later than Placement Secured Date {" + FDPlacementSecuredDateStr + "}.";
                else if (ruleViolation12a == true)
                    sentOptionObject.ErrorMesg = "Placement Secured cannot be more than 14 days after Bed Search Began.";
                else if (ruleViolation13a == true)
                {
                    sentOptionObject.ErrorMesg = "Disposition Secured must be later than the ";
                    PIFMaxTimeStr = PIFMaxTimeStr.Replace("01:00 AM", "1 am"); PIFMaxTimeStr = PIFMaxTimeStr.Replace("02:00 AM", "2 am"); PIFMaxTimeStr = PIFMaxTimeStr.Replace("03:00 AM", "3 am");
                    PIFMaxTimeStr = PIFMaxTimeStr.Replace("04:00 AM", "4 am"); PIFMaxTimeStr = PIFMaxTimeStr.Replace("05:00 AM", "5 am"); PIFMaxTimeStr = PIFMaxTimeStr.Replace("06:00 AM", "6 am");
                    PIFMaxTimeStr = PIFMaxTimeStr.Replace("07:00 AM", "7 am"); PIFMaxTimeStr = PIFMaxTimeStr.Replace("08:00 AM", "8 am"); PIFMaxTimeStr = PIFMaxTimeStr.Replace("09:00 AM", "9 am");
                    PIFMaxTimeStr = PIFMaxTimeStr.Replace("10:00 AM", "10 am"); PIFMaxTimeStr = PIFMaxTimeStr.Replace("11:00 AM", "11 am"); PIFMaxTimeStr = PIFMaxTimeStr.Replace("12:00 AM", "12 am");
                    PIFMaxTimeStr = PIFMaxTimeStr.Replace("01:00 PM", "1 pm"); PIFMaxTimeStr = PIFMaxTimeStr.Replace("02:00 PM", "2 pm"); PIFMaxTimeStr = PIFMaxTimeStr.Replace("03:00 PM", "3 pm");
                    PIFMaxTimeStr = PIFMaxTimeStr.Replace("04:00 PM", "4 pm"); PIFMaxTimeStr = PIFMaxTimeStr.Replace("05:00 PM", "5 pm"); PIFMaxTimeStr = PIFMaxTimeStr.Replace("06:00 PM", "6 pm");
                    PIFMaxTimeStr = PIFMaxTimeStr.Replace("07:00 PM", "7 pm"); PIFMaxTimeStr = PIFMaxTimeStr.Replace("08:00 PM", "8 pm"); PIFMaxTimeStr = PIFMaxTimeStr.Replace("09:00 PM", "9 pm");
                    PIFMaxTimeStr = PIFMaxTimeStr.Replace("10:00 PM", "10 pm"); PIFMaxTimeStr = PIFMaxTimeStr.Replace("11:00 PM", "11 pm"); PIFMaxTimeStr = PIFMaxTimeStr.Replace("12:00 PM", "12 pm");
                    PIFMaxTimeStr = PIFMaxTimeStr.Replace("01:", "1:"); PIFMaxTimeStr = PIFMaxTimeStr.Replace("02:", "2:"); PIFMaxTimeStr = PIFMaxTimeStr.Replace("03:", "3:");
                    PIFMaxTimeStr = PIFMaxTimeStr.Replace("04:", "4:"); PIFMaxTimeStr = PIFMaxTimeStr.Replace("05:", "5:"); PIFMaxTimeStr = PIFMaxTimeStr.Replace("06:", "6:");
                    PIFMaxTimeStr = PIFMaxTimeStr.Replace("07:", "7:"); PIFMaxTimeStr = PIFMaxTimeStr.Replace("08:", "8:"); PIFMaxTimeStr = PIFMaxTimeStr.Replace("09:", "9:");
                    PIFMaxDateStr = PIFMaxDateStr.Replace("01/", "1/"); PIFMaxDateStr = PIFMaxDateStr.Replace("02/", "2/"); PIFMaxDateStr = PIFMaxDateStr.Replace("03/", "3/");
                    PIFMaxDateStr = PIFMaxDateStr.Replace("04/", "4/"); PIFMaxDateStr = PIFMaxDateStr.Replace("05/", "5/"); PIFMaxDateStr = PIFMaxDateStr.Replace("06/", "6/");
                    PIFMaxDateStr = PIFMaxDateStr.Replace("07/", "7/"); PIFMaxDateStr = PIFMaxDateStr.Replace("08/", "8/"); PIFMaxDateStr = PIFMaxDateStr.Replace("09/", "9/");
                    switch (FieldToCorrect)
                    {
                        case 1:
                            sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg + "PIF Evaluation Date/Date of Service, Intervention Began Time " + PIFMaxDateStr + ", " + PIFMaxTimeStr + ".";
                            break;
                        case 2:
                            sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg + "PIF Demographics Ready " + PIFReadyDateStr + ", " + PIFReadyTimeStr + ".";
                            break;
                        case 3:
                            sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg + "Disposition Detail Psych Intervention Began " + PIFPsychiatricInterventionBeganDateStr + ", " + PIFPsychiatricInterventionBeganTimeStr + ".";
                            break;
                        case 4:
                            sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg + "Disposition Detail Psych Consult Began " + PIFPsychiatricConsultBeganDateStr + ", " + PIFPsychiatricConsultBeganTimeStr + ".";
                            break;
                        case 5:
                            sentOptionObject.ErrorMesg = sentOptionObject.ErrorMesg + "<b><i><u>Calculated</b></i></u> Intervention End " + PIFMaxDateStr + ", " + PIFMaxTimeStr + ".";
                            break;
                    }
                }
                else if (ruleViolation13c == true)
                {
                    sentOptionObject.ErrorMesg = "Disposition Secured Date/Time {" + FDFinalDispositionSecuredDateStr + ", " + FDFinalDispositionSecuredTimeStr + 
                        "} cannot be later than 30 days after PIF Evaluation Date/Date of Service, Intervention Began Time {" + PIFDateOfServiceStr + ", " + PIFInterventionBeganTimeStr + "}.";
                }
                else if (ruleViolation13d == true)
                {
                    sentOptionObject.ErrorMesg = "Placement Secured Date/Time {" + FDPlacementSecuredDateStr + ", " + FDPlacementSecuredTimeStr
                        + "} cannot be later than Final Disposition Secured Date/Time {" + FDFinalDispositionSecuredDateStr + ", " + FDFinalDispositionSecuredTimeStr + "}.";
                }
                else if (ruleViolation13dDateOnly == true)
                {
                    sentOptionObject.ErrorMesg = "Placement Secured Date {" + FDPlacementSecuredDateStr 
                        + "} cannot be later than Final Disposition Secured Date {" + FDFinalDispositionSecuredDateStr + "}.";
                }
                else if (ruleViolation13e == true)
                {
                    sentOptionObject.ErrorMesg = "Final Disposition Secured Date/Time {" + FDFinalDispositionSecuredDateStr + ", " + FDFinalDispositionSecuredTimeStr 
                        + "} must be later than the Bed Search Began Date/Time {" + FDBedSearchBeganDateStr + ", " + FDBedSearchBeganTimeStr + "}.";
                }
                else if (ruleViolation13eDateOnly == true)
                {
                    sentOptionObject.ErrorMesg = "Bed Search Began Date {" + FDBedSearchBeganDateStr 
                        + "} cannot be later than Final Disposition Secured Date {" + FDFinalDispositionSecuredDateStr + "}.";
                }
                else if (ruleViolation13f == true)
                {
                    sentOptionObject.ErrorMesg = "Final Disposition Date {" + FDFinalDispositionSecuredDateStr 
                        + "} cannot be later than current Date.";
                }
                if (displayOrProcess.Equals("FinalDispositionSA") == true)
                {
                    sentOptionObject.ErrorCode = 3;
                }
                else if (displayOrProcess.Equals("FinalDispositionSubmit") == true)
                {
                    sentOptionObject.ErrorCode = 1;
                }
                else
                {
                    sentOptionObject.ErrorCode = 1;
                    sentOptionObject.ErrorMesg += " ... <u><b>IMPORTANT: </b><i>Contact Your Avatar Sys Admin to properly configure the ScriptLink Web Services for your dev/test environment</i></u> ...";
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

        public static Boolean MBHPDateRulesFD(String date1, String time1, String date2, String time2, String ruleNumber)
        {
            Boolean retVal = false;
            String ampm1, ampm2;
            Int32 year1, year2, month1, month2, day1, day2, hour1, hour2, minute1, minute2, t1, t2, ymd1, ymd2;

            /* If rule number is 13c. then confirm both date1 v date2 and date1/time1 v date2/time2.*/
            if (ruleNumber.Equals("rule13c") == true)
            {
                /* Here we set the Intervention Began time to midnight (12:00 AM), if the user did not provide 
                 * the field on the PIF disposition details.  This is important because whereas the 
                 * PIF Date of Service is required, the disposition details Intervention Began time is not required.
                 * Hence it's possible that by the time we get to the final disposition, we only have an 
                 * Intervention began date (i.e. date of service) to compare to the required 
                 * disposition Secured date/time.  Note, I'm thinking it is not necessary to apply 
                 * this same logic to rule 13a/13aprep because it is accounted for by the 
                 * intervention ended date/time which is by definition after the 
                 * intervention began and oh by the way both date of service and service duration are required. */
                if (string.IsNullOrEmpty(time1) || time1.Equals("00:00 AM"))
                /* Note this is a requirement to check rule 13c which states: 13. Disposition Secured 
                 * Date /Time (55) section-name (Tab): Final Disposition c. Cannot be later than 30 
                 * days after Intervention Began with no qualifier on the Intervention Began time 
                 * because it is not required in the PIF disposition detail.  So hey, if the intervention 
                 * began time is not specified, then it processes as null meaning midnight unless you have a better idea.
                 * Obviously making intervention began time a required field resolves this issue, but it does not appear
                 * to be in the cards as the final Adult and Child PIF are already in UAT. */
               {
                   time1 = string.Copy("12:00 AM");
               }
            }
            else if (ruleNumber.Equals("rule13f") == true)
            {
                if (string.IsNullOrEmpty(date1) || date1.Equals("01/01/1900") == true)
                {
                    return (retVal);
                }

                /* ymd1 is for the Final Disposition Date */
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

                if (ruleNumber.Equals("rule13f") == true && (ymd1 > ymd2)) // Rule 13f states: The Final Disposition Date cannot be in the future.
                                                                           // IMPORTANT: For this rule, we ignore the time stamp, why you ask, the reason
                                                                           // is simple.  Only the date is compared to the future date because that's all 
                                                                           // the MBHP Portal should check.  The other side of this coin is by the time this gets 
                                                                           // sent off to MBHP, a user entering a future time on the current date for the
                                                                           // Time Final Disposition Secured will no longer be an issue because the date is 
                                                                           // in the past.  Confusing yes, but I'm ok with this one as least for now.  
                                                                           // We'll see what the MBHP portal says.
                {
                    return (true);
                }
                return (retVal);
            }

            // Rule 13a states: Disposition Secured Date/Time - Final Disposition Must be after all above dates.  Rule 13a is the max of all dates.
            // All dates include: 1. Ready date/time, 2. psych phone contact began date/time, 3. psych intervention began date/time and 4. the calculated intervention ended date/time. 

            if (ruleNumber.Equals("rule13a") == true)
            {
                if ((date1.Equals(date2) == true) && (time1.Equals(time2) == true))
                {
                    return (true);
                }
            }

            // The following if statement accounts for an anomoly within the Avatar Form Designer/Definition whereby 
            // a group by statement occurs prior to a SQL select statement and this prevents the interface from 
            // displaying a data row in a drop down list if any value within a select statement contains a null value.
            // Please refer to the Avatar Form Developers team for further clarification.
            if (time1.Equals("00:00 AM") == true || date1.Equals("01/01/1900") == true || time2.Equals("00:00 AM") == true || date2.Equals("01/01/1900") == true)
            {
                return retVal;
            }

            // Important removed the date1.Equals 01/01/1900 because for now this value is the 'null' (quotes intentional to specify that the value transmitted over the
            // web to the web service is not null (i.e. \0), but rather the Avatar implementation of null within the Avatar Form Developer.  The if below is strictly for 
            // the case when the Avatar Form Developer works properly accounting for null within grouped Form Drop downs.  This will allow us to send ,, for a null date or 
            // time within Avatar as opposed to sending ,00:00 AM, or ,01/01/1900, to represent the null value, \0.
            if (string.IsNullOrEmpty(date1) || string.IsNullOrEmpty(time1) || string.IsNullOrEmpty(date2) || string.IsNullOrEmpty(time2))
                return retVal;

            /* Set the local date/time strings from the input date/time strings for date1/time1 */
            year1 = Convert.ToInt32(date1.Substring(6, 4));
            month1 = Convert.ToInt32(date1.Substring(0, 2));
            day1 = Convert.ToInt32(date1.Substring(3, 2));
            hour1 = Convert.ToInt32(time1.Substring(0, 2));
            minute1 = Convert.ToInt32(time1.Substring(3, 2));
            ampm1 = time1.Substring(6, 2);

             /* Set the local date strings from the input date string for date2 */
            year2 = Convert.ToInt32(date2.Substring(6, 4));
            month2 = Convert.ToInt32(date2.Substring(0, 2));
            day2 = Convert.ToInt32(date2.Substring(3, 2));

            /* Set the local time strings from the input time string for time2 */
            hour2 = Convert.ToInt32(time2.Substring(0, 2));
            minute2 = Convert.ToInt32(time2.Substring(3, 2));
            ampm2 = time2.Substring(6, 2);

            // Note the importance of accounting for PM Hours (excluding 12pm).

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

            t1 = year1 * 100000000 + month1 * 1000000 + day1 * 10000 + hour1 * 100 + minute1;
            t2 = year2 * 100000000 + month2 * 1000000 + day2 * 10000 + hour2 * 100 + minute2;

            if (ruleNumber.Equals("rule12a") == true) 
            {
                // if (((diff.Days == 14) && ((hour2 * 60) + minute2) > ((hour1 * 60) + minute1)) || (diff.Days > 14))
                if (diff.Days >= 14)
                {
                    return (true);
                }
                else
                {
                    return (retVal);
                }
            }
            else if (ruleNumber.Equals("rule13c") == true)
            {
                if (diff.Days > 30 || diff.Days == 30 && diff.Hours > 0 || diff.Days == 30 && diff.Hours == 0 && diff.Minutes > 0)
                //if (diff.Days >= 30)
                {
                    return (true);
                }
                else
                {
                    return (retVal);
                }
            }
            /* rule13e is an interesting rule in that it is required that DateTimeFinalDispositionSecured: must be later than
               DateTimeBedsearchBegan (not just not before) whereas rule 13d does not require DateTimeFinalDispositionSecured 
               be later than placement secured, only, not before; AND more importantly, rule 12d does not required Bed Search 
               Began be before than Placement Secured.  The ER has a board of open beds, therefore they can begin a bed search and
               secure placement within the same minute.  The important point here is when the bed search begins and ends on the
               same minute, the final disposition secured MUST be at least a minute later.  If the placement is after the bed search 
               begin then the final disposition secured can be at the same minute in time as the placement secured.  The MBHP portal 
               date rules are illogical in this respect, imho. */
            else if (ruleNumber.Equals("rule13e") == true)
            {
                if (t1 >= t2)
                {
                    return (true);
                }
                else
                {
                    return (retVal);
                }
            }
            /* for rules besides 12a and 13{c,e} (i.e. rule 13a preparation and rule 13a) */
            else
            {
                if (t1 > t2)
                {
                    return (true);
                }
                else
                {
                    return (retVal);
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
