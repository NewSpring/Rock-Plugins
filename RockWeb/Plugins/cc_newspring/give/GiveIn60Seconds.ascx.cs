using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;

using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Financial;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Web.UI;
using Rock.Web.UI.Controls;

namespace RockWeb.Plugins.cc_newspring.give
{
    #region Block Attributes

    /// <summary>
    /// Add a new one-time or scheduled transaction
    /// </summary>
    [DisplayName( "Give In 60 Seconds" )]
    [Category( "NewSpring > Give In 60 Seconds" )]
    [Description( "Provides a simple giving page to look up attendee data and process contributions." )]
    [ComponentField( "Rock.Financial.GatewayContainer, Rock", "Credit Card Gateway", "The payment gateway to use for Credit Card transactions", false, "", "", 0, "CCGateway" )]
    [ComponentField( "Rock.Financial.GatewayContainer, Rock", "ACH Card Gateway", "The payment gateway to use for ACH (bank account) transactions", false, "", "", 1, "ACHGateway" )]
    [TextField( "Batch Name Prefix", "The batch prefix name to use when creating a new batch", false, "Online Giving - ", "", 2 )]
    [DefinedValueField( Rock.SystemGuid.DefinedType.FINANCIAL_SOURCE_TYPE, "Source", "The Financial Source Type to use when creating transactions", false, false, "", "", 3 )]
    [GroupLocationTypeField( Rock.SystemGuid.GroupType.GROUPTYPE_FAMILY, "Address Type", "The location type to use for the person's address", false,
        Rock.SystemGuid.DefinedValue.GROUP_LOCATION_TYPE_HOME, "", 4 )]
    [BooleanField( "Scheduled Transactions", "Allow", "Don't Allow",
        "If the selected gateway(s) allow scheduled transactions, should that option be provided to user", true, "", 9, "AllowScheduled" )]
    [CodeEditorField( "Confirmation Header", "The text (HTML) to display at the top of the confirmation section.", CodeEditorMode.Html, CodeEditorTheme.Rock, 400, true, @"
<p>
Please confirm the information below. Once you have confirmed that the information is accurate click the 'Finish' button to complete your transaction.
</p>
", "Text Options", 13 )]
    [CodeEditorField( "Confirmation Footer", "The text (HTML) to display at the bottom of the confirmation section.", CodeEditorMode.Html, CodeEditorTheme.Rock, 400, true, @"
<div class='alert alert-info'>
By clicking the 'finish' button below I agree to allow {{ OrganizationName }} to debit the amount above from my account. I acknowledge that I may
update the transaction information at any time by returning to this website. Please call the Finance Office if you have any additional questions.
</div>
", "Text Options", 14 )]
    [CodeEditorField( "Success Header", "The text (HTML) to display at the top of the success section.", CodeEditorMode.Html, CodeEditorTheme.Rock, 400, true, @"
<p>
Thank you for your generous contribution.  Your support is helping {{ OrganizationName }} actively
achieve our mission.  We are so grateful for your commitment.
</p>
", "Text Options", 15 )]
    [CodeEditorField( "Success Footer", "The text (HTML) to display at the bottom of the success section.", CodeEditorMode.Html, CodeEditorTheme.Rock, 400, true, @"
", "Text Options", 16 )]
    [EmailTemplateField( "Confirm Account", "Confirm Account Email Template", false, Rock.SystemGuid.SystemEmail.SECURITY_CONFIRM_ACCOUNT, "Email Templates", 17, "ConfirmAccountTemplate" )]

    #endregion

    public partial class GiveIn60Seconds : Rock.Web.UI.RockBlock
    {
        #region Fields

        private GatewayComponent _ccGateway;

        private GatewayComponent _achGateway;

        /// <summary>
        /// Gets or sets the accounts that are available for user to add to the list.
        /// </summary>
        protected List<AccountItem> AvailableAccounts
        {
            get
            {
                var accounts = ViewState["AvailableAccounts"] as List<AccountItem>;
                if ( accounts == null )
                {
                    accounts = new List<AccountItem>();
                }

                return accounts;
            }

            set
            {
                ViewState["AvailableAccounts"] = value;
            }
        }

        #endregion

        #region Base Control Methods

        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            if ( !Page.IsPostBack )
            {
                var rockContext = new RockContext();

                foreach ( var account in new FinancialAccountService( rockContext ).Queryable()
                .Where( f =>
                    f.IsActive &&
                    f.PublicName != null &&
                    f.PublicName.Trim() != string.Empty &&
                    ( f.StartDate == null || f.StartDate <= RockDateTime.Today ) &&
                    ( f.EndDate == null || f.EndDate >= RockDateTime.Today ) )
                .OrderBy( f => f.Order ) )
                {
                    var accountItem = new AccountItem( account.Id, account.Order, account.Name, account.CampusId );
                    AvailableAccounts.Add( accountItem );
                }

                rptAccountList.DataSource = AvailableAccounts.OrderBy( a => a.Order ).ToList();
                rptAccountList.DataBind();
                //cpCampus.SetValue( gfSettings.GetUserPreference( "Campus" ) );

                //BindGrid();
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Handles the Click event of the btnNext control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnNext_Click( object sender, EventArgs e )
        {
            //// Page 1 = Payment Info
            //// Page 2 = Confirmation
            //// Page 3 = Success
            //// Page 0 = Only message box is displayed

            int pageId = hfCurrentPage.Value.AsInteger() ?? 0;
            if ( pageId > 0 )
            {
                pnlStepOne.Visible = pageId == 1;
                pnlStepTwo.Visible = pageId == 2;
                pnlStepThree.Visible = pageId == 3;
                //divActions.Visible = page > 0;

                btnBack.Visible = pageId == 2;
                btnNext.Visible = pageId < 3;
                btnNext.Text = pageId > 1 ? "Finish" : "Next";
            }

            hfCurrentPage.Value = pageId.ToString();
        }

        /// <summary>
        /// Handles the Click event of the btnBack control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnBack_Click( object sender, EventArgs e )
        {
        }

        #endregion

        #region Helper Classes

        /// <summary>
        /// Lightweight object for each contribution item
        /// </summary>
        [Serializable]
        protected class AccountItem
        {
            public int Id { get; set; }

            public int Order { get; set; }

            public string Name { get; set; }

            public int? CampusId { get; set; }

            public decimal Amount { get; set; }

            public string AmountFormatted
            {
                get
                {
                    return Amount > 0 ? Amount.ToString( "F2" ) : string.Empty;
                }
            }

            public AccountItem( int id, int order, string name, int? campusId )
            {
                Id = id;
                Order = order;
                Name = name;
                CampusId = campusId;
            }
        }

        #endregion
    }
}