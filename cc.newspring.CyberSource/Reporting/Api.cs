// <copyright>
// Copyright 2013 by the Spark Development Network
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using cc.newspring.CyberSource.ITransactionProcessor;
using Rock.VersionInfo;

namespace cc.newspring.CyberSource.Reporting
{
    /// <summary>
    /// Provides interaction with CyberSource XML reporting API
    /// </summary>
    public class Api
    {
        public string merchantId { get; set; }

        public string transactionKey { get; set; }

        public string reportUser { get; set; }

        public string reportPassword { get; set; }

        public bool isLive { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Api"/> class.
        /// </summary>
        /// <param name="merchant">The merchant.</param>
        /// <param name="key">The key.</param>
        /// <param name="isTest">if set to <c>true</c> [is test].</param>
        public Api( string merchant, string key, string user, string userPassword, bool live = false )
        {
            merchantId = merchant;
            transactionKey = key;
            reportUser = user;
            reportPassword = userPassword;
            isLive = live;
        }

        /// <summary>
        /// Gets the API URL.
        /// </summary>
        /// <returns></returns>
        private string ReportingApiUrl()
        {
            if ( isLive )
            {
                return "https://ebc.cybersource.com/ebc";
            }
            else
            {
                return "https://ebctest.cybersource.com/ebctest";
            }
        }

        /// <summary>
        /// Gets the report.
        /// </summary>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="reportParameters">The report parameters.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        public DataTable GetReport( string reportName, Dictionary<string, string> reportParameters, out string errorMessage )
        {
            // Request a report
            errorMessage = string.Empty;
            string formattedDate = reportParameters["date"];
            string requestUrl = string.Format( "{0}/DownloadReport/{1}/{2}/{3}.xml", ReportingApiUrl(), formattedDate, merchantId, reportName );

            XDocument xmlResponse = SendRequest( requestUrl, out errorMessage );
            if ( xmlResponse != null )
            {
                XNamespace xmlNS = string.Format( "{0}/{1}", ReportingApiUrl(), "reports/dtd/sdr.dtd" );
                var payments = xmlResponse.Root.Descendants( xmlNS + "SubscriptionPayment" ).ToList();

                DataTable dt = new DataTable();
                dt.Columns.Add( "Amount", typeof( string ) );
                dt.Columns.Add( "Code", typeof( string ) );
                dt.Columns.Add( "Time", typeof( string ) );
                dt.Columns.Add( "Schedule", typeof( string ) );
                dt.Columns.Add( "Status", typeof( string ) );

                foreach ( XElement payment in payments )
                {
                    var dataRow = dt.NewRow();
                    var paymentInfo = payment.Element( xmlNS + "PaymentData" );
                    var statusDetails = payment.Element( xmlNS + "SubscriptionDetails" );

                    dataRow["Code"] = (string)payment.Attribute( "subscription_id" );
                    dataRow["Time"] = (string)payment.Attribute( "transaction_date" );
                    dataRow["Amount"] = (string)paymentInfo.Element( xmlNS + "recurring_payment_event_amount" );
                    dataRow["Schedule"] = (string)statusDetails.Element( xmlNS + "recurring_frequency" );
                    dataRow["Status"] = (string)statusDetails.Element( xmlNS + "subscription_status" );

                    dt.Rows.Add( dataRow );
                }

                return dt;
            }

            return null;
        }

        /// <summary>
        /// Sends the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        private XDocument SendRequest( string requestUrl, out string errorMessage )
        {
            errorMessage = string.Empty;
            XDocument response = null;

            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create( requestUrl );
            webRequest.UserAgent = VersionInfo.GetRockProductVersionFullName();
            webRequest.Credentials = new NetworkCredential( reportUser, reportPassword );
            webRequest.ContentType = "text/xml; encoding='utf-8'";
            webRequest.Method = "GET";

            try
            {
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                var stream = webResponse.GetResponseStream();
                using ( XmlReader reader = new XmlTextReader( stream ) )
                {
                    response = XDocument.Load( reader );
                }
            }
            catch ( WebException we )
            {
                using ( HttpWebResponse webResponse = (HttpWebResponse)we.Response )
                {
                    errorMessage = string.Format( "The requested report could not be found. Status code: {0}", webResponse.StatusCode );
                    return null;
                }
            }

            return response;
        }

        /* ========= EXAMPLE XML RESPONSE ==========
        XDocument example = XDocument.Parse(@"
        <!DOCTYPE Report SYSTEM ""https://ebctest.cybersource.com/ebctest/reports/dtd/sdr.dtd"">
        <Report Name=""Subscription Detail""
            Version=""1.0""
            xmlns=""https://ebctest.cybersource.com/ebctest/reports/dtd/sdr.dtd""
            MerchantID=""infodev""
            ReportStartDate=""2010-02-11T15:00:00+09:00""
            ReportEndDate=""2010-02-12T15:00:00+09:00"">
            <SubscriptionPayments>
                <SubscriptionPayment payment_request_id=""11111111111111111111""
                        subscription_id=""111111111111111111111""
                        transaction_date=""2010-02-11T18:43:28+09:00""
                        merchant_ref_number=""1111111111111""
                        transaction_ref_number=""111111RYZPS6548PSX""
                        e_commerce_indicator=""M"">
                    <ShipTo>
                    <ship_to_firstname>JOHN</ship_to_firstname>
                    <ship_to_lastname>SMITH</ship_to_lastname>
                    <ship_to_address1>8310 Capitol of Texas Hwy
                        </ship_to_address1>
                    <ship_to_address2>Suite 100</ship_to_address2>
                    <ship_to_city>Austin</ship_to_city>
                    <ship_to_state>TX</ship_to_state>
                    <ship_to_zip>78731</ship_to_zip>
                    <ship_to_country>US</ship_to_country>
                    <ship_to_company_name>Your Company</ship_to_company_name>
                    </ShipTo>
                    <PaymentMethod>
                        <Card>
                            <card_type>Visa</card_type>
                            <customer_cc_expmo>01</customer_cc_expmo>
                            <customer_cc_expyr>2011</customer_cc_expyr>
                            <account_suffix>1111</account_suffix>
                        </Card>
                    </PaymentMethod>
                    <PaymentData>
                        <ics_applications>ics_auth,ics_bill</ics_applications>
                        <recurring_payment_event_amount>99.99</recurring_payment_event_amount>
                        <payment_processor>hsbc</payment_processor>
                        <currency_code>USD</currency_code>
                        <reason_code>200</reason_code>
                        <auth_rcode>0</auth_rcode>
                        <auth_code>JS1111</auth_code>
                        <auth_type>O</auth_type>
                        <auth_auth_avs>N</auth_auth_avs>
                        <auth_auth_response>00</auth_auth_response>
                        <auth_cavv_response>1111</auth_cavv_response>
                        <ics_rcode>1</ics_rcode>
                        <ics_rflag>111111111</ics_rflag>
                        <ics_rmsg>1111111111</ics_rmsg>
                        <request_token>5r9uxlPGppxMFEWusMJsKaWtdb444</request_token>
                    </PaymentData>
                    <MerchantDefinedData>
                        <merchant_defined_data1>gift</merchant_defined_data1>
                        <merchant_defined_data2>rush shipping</merchant_defined_data2>
                        <merchant_defined_data3>document #1</merchant_defined_data3>
                        <merchant_defined_data4>document #2</merchant_defined_data4>
                    </MerchantDefinedData>
                    <SubscriptionDetails>
                        <recurring_payment_amount>0.00</recurring_payment_amount>
                        <subscription_type>on-demand</subscription_type>
                        <subscription_title>My Subscription</subscription_title>
                        <last_subscription_status>CURRENT</last_subscription_status>
                        <subscription_status>CURRENT</subscription_status>
                        <subscription_payment_method>SW</subscription_payment_method>
                        <recurring_start_date>2010-02-01 07:00:00.0</recurring_start_date>
                        <next_scheduled_date>2010-03-01 07:00:00.0</next_scheduled_date>
                        <event_retry_count>0</event_retry_count>
                        <payments_success>0</payments_success>
                        <payment_success_amount>0.00</payment_success_amount>
                        <recurring_number_of_payments>0</recurring_number_of_payments>
                        <installment_sequence>0.00</installment_sequence>
                        <installment_total_count>0.00</installment_total_count>
                        <recurring_frequency>on-demand</recurring_frequency>
                        <recurring_approval_required>N</recurring_approval_required>
                        <recurring_payment_event_approved_by>hsbc</recurring_payment_event_approved_by>
                        <recurring_automatic_renew>N</recurring_automatic_renew>
                        <comments>0</comments>
                        <setup_fee>0.00</setup_fee>
                        <setup_fee_currency>USD</setup_fee_currency>
                        <tax_amount>0.000000000000000</tax_amount>
                        <merchant_secure_data1>0</merchant_secure_data1>
                        <merchant_secure_data2>0</merchant_secure_data2>
                        <merchant_secure_data3>0</merchant_secure_data3>
                        <merchant_secure_data4>0</merchant_secure_data4>
                    </SubscriptionDetails>
                </SubscriptionPayment>
            </SubscriptionPayments>
        </Report>");
        ================================================================= */
    }
}