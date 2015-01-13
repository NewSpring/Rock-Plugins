using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Financial;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Web.UI;
using Rock.Web.UI.Controls;

namespace RockWeb.Plugins.cc_newspring.GiveIn60Seconds
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

    // 5

    [AccountsField( "Accounts", "The accounts to display.  By default all active accounts with a Public Name will be displayed", false, "", "", 6 )]
    [BooleanField( "Additional Accounts", "Display option for selecting additional accounts", "Don't display option",
        "Should users be allowed to select additional accounts?  If so, any active account with a Public Name value will be available", true, "", 7 )]
    [TextField( "Add Account Text", "The button text to display for adding an additional account", false, "Add Another Account", "", 8 )]
    [BooleanField( "Require Phone", "Should the user be prompted for their phone number?", true, "", 11, "RequirePhone" )]
    [BooleanField( "Require Email", "Should the user be prompted for their email address?", true, "", 12, "RequireEmail" )]
    [CodeEditorField( "Page Header", "The text (HTML) to display at the top of the page.", CodeEditorMode.Html, CodeEditorTheme.Rock, 400, true, @"<p>
STEP {{PageNumber}} OF 4
</p><br>", "Text Options", 13 )]
    [CodeEditorField( "Page Footer", "The text (HTML) to display at the bottom of the page.", CodeEditorMode.Html, CodeEditorTheme.Rock, 400, true, @"<br><div class='alert alert-info'>
If you have questions about giving, or difficulty giving online to {{OrganizationName}}, please contact us at {{OrganizationEmail}} or {{ OrganizationPhone }}.
</div>", "Text Options", 14 )]
    [CodeEditorField( "Receipt Message", "The text (HTML) to display when the contribution is successful.", CodeEditorMode.Html, CodeEditorTheme.Rock, 400, true, @"
<p>
Thank you for your generous contribution.  Your support is helping {{ OrganizationName }} reach the world for Jesus.
</p>
", "Text Options", 15 )]
    [SystemEmailFieldAttribute( "Confirm Account", "Confirm Account Email Template", false, Rock.SystemGuid.SystemEmail.SECURITY_CONFIRM_ACCOUNT, "Email Templates", 17, "ConfirmAccountTemplate" )]

    #endregion Block Attributes

    public partial class Give : Rock.Web.UI.RockBlock
    {
        #region Fields

        private GatewayComponent _ccGateway;

        private GatewayComponent _achGateway;

        /// <summary>
        /// Gets or sets the accounts that are available for user to add to the list.
        /// </summary>
        protected List<AccountItem> Accounts
        {
            get
            {
                var accounts = ViewState["Accounts"] as List<AccountItem>;
                if ( accounts == null )
                {
                    accounts = new List<AccountItem>();
                }

                return accounts;
            }

            set
            {
                ViewState["Accounts"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the payment transaction code.
        /// </summary>
        protected string TransactionCode
        {
            get { return ViewState["TransactionCode"] as string ?? string.Empty; }
            set { ViewState["TransactionCode"] = value; }
        }

        /// <summary>
        /// Gets or sets the currency type value identifier.
        /// </summary>
        protected int? CreditCardTypeValueId
        {
            get { return ViewState["CreditCardTypeValueId"] as int?; }
            set { ViewState["CreditCardTypeValueId"] = value; }
        }

        #endregion Fields

        #region Base Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            RockPage page = Page as RockPage;
            if ( page != null )
            {
                page.PageNavigate += page_PageNavigate;
            }

            string ccGatewayGuid = GetAttributeValue( "CCGateway" );
            if ( !string.IsNullOrWhiteSpace( ccGatewayGuid ) )
            {
                _ccGateway = GatewayContainer.GetComponent( ccGatewayGuid );
                if ( _ccGateway != null )
                {
                    mypExpiration.MinimumYear = RockDateTime.Now.Year;
                }
            }

            string achGatewayGuid = GetAttributeValue( "ACHGateway" );
            if ( !string.IsNullOrWhiteSpace( achGatewayGuid ) )
            {
                _achGateway = GatewayContainer.GetComponent( achGatewayGuid );
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            // Hide errors on every postback
            nbMessage.Visible = false;
            pnlDupWarning.Visible = false;
            nbSaveAccount.Visible = false;

            if ( _ccGateway != null || _achGateway != null )
            {
                // Save amounts from controls to the viewstate list
                foreach ( RepeaterItem item in rptAccountList.Items )
                {
                    var accountAmount = item.FindControl( "txtAccountAmount" ) as RockTextBox;
                    if ( accountAmount != null )
                    {
                        if ( Accounts.Count > item.ItemIndex )
                        {
                            decimal amount = decimal.MinValue;
                            if ( decimal.TryParse( accountAmount.Text, out amount ) )
                            {
                                Accounts[item.ItemIndex].Amount = amount;
                            }
                        }
                    }
                }

                // Update the total amount
                lblTotalAmount.Text = Accounts.Sum( f => f.Amount ).ToString( "F2" );

                // ==============================================================
                if ( !Page.IsPostBack )
                {
                    // Get the list of accounts that can be used
                    GetAccounts();
                    BindCampuses();

                    // Display Options
                    btnAddAccount.Title = GetAttributeValue( "AddAccountText" );

                    bool ccEnabled = _ccGateway != null;
                    bool achEnabled = _achGateway != null;
                    if ( ccEnabled )
                    {
                        hfPaymentTab.Value = "CreditCard";
                    }
                    else
                    {
                        hfPaymentTab.Value = "ACH";
                    }

                    if ( ccEnabled && achEnabled )
                    {
                        phPills.Visible = true;
                        divCCPaymentInfo.AddCssClass( "tab-pane" );
                        divACHPaymentInfo.AddCssClass( "tab-pane" );
                    }

                    divCCPaymentInfo.Visible = ccEnabled;
                    divACHPaymentInfo.Visible = achEnabled;

                    if ( rblSavedCC.Items.Count > 0 )
                    {
                        rblSavedCC.Items[0].Selected = true;
                        rblSavedCC.Visible = true;
                        divNewCard.Style[HtmlTextWriterStyle.Display] = "none";
                    }
                    else
                    {
                        rblSavedCC.Visible = false;
                        divNewCard.Style[HtmlTextWriterStyle.Display] = "block";
                    }

                    if ( rblSavedAch.Items.Count > 0 )
                    {
                        rblSavedAch.Items[0].Selected = true;
                        rblSavedAch.Visible = true;
                        divNewBank.Style[HtmlTextWriterStyle.Display] = "none";
                    }
                    else
                    {
                        rblSavedAch.Visible = false;
                        divNewCard.Style[HtmlTextWriterStyle.Display] = "block";
                    }

                    RegisterScript();
                    SetPage( 1 );
                }

                // Set active tab on postback based on which tab was last selected
                if ( hfPaymentTab.Value == "ACH" )
                {
                    liCreditCard.RemoveCssClass( "active" );
                    divCCPaymentInfo.RemoveCssClass( "active" );
                    divACHPaymentInfo.AddCssClass( "active" );
                    liACH.AddCssClass( "active" );
                }
                else
                {
                    liCreditCard.AddCssClass( "active" );
                    divCCPaymentInfo.AddCssClass( "active" );
                    divACHPaymentInfo.RemoveCssClass( "active" );
                    liACH.RemoveCssClass( "active" );
                }

                ScriptManager.GetCurrent( this.Page ).EnableSecureHistoryState = false;
            }
            else
            {
                SetPage( 0 );
                ShowMessage( NotificationBoxType.Danger, "Configuration Error", "Please check the configuration of this block and make sure a valid Credit Card and/or ACH Financial Gateway has been selected." );
            }

            BindHeaders();
        }

        #endregion Base Control Methods

        #region Event methods

        /// <summary>
        /// Handles the SelectedIndexChanged event of the cpCampuses control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void cpCampuses_SelectedIndexChanged( object sender, EventArgs e )
        {
            var selectedCampusId = cpCampuses.SelectedCampusId;
            var matchingAccounts = new List<int>();
            if ( selectedCampusId != null )
            {
                matchingAccounts = Accounts.Where( a => a.CampusId == selectedCampusId || a.CampusId == null )
                    .Select( c => c.Id ).ToList();
            }

            Accounts.ForEach( a => a.Selected = matchingAccounts.Contains( a.Id ) );
            this.AddHistory( "step", "1", null );
            BindAccounts();
        }

        /// <summary>
        /// Handles the SelectionChanged event of the btnAddAccount control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnAddAccount_SelectionChanged( object sender, EventArgs e )
        {
            var selected = Accounts.FirstOrDefault( a => a.Id == ( btnAddAccount.SelectedValueAsId() ?? 0 ) );
            if ( selected != null )
            {
                selected.Selected = true;
            }

            BindAccounts();
        }

        /// <summary>
        /// Handles the ItemDataBound event of the rptPersonPicker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RepeaterItemEventArgs"/> instance containing the event data.</param>
        protected void rptPersonPicker_ItemDataBound( object sender, RepeaterItemEventArgs e )
        {
            var currentPerson = e.Item.DataItem as Person;
            if ( e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem )
            {
                var cbPerson = e.Item.FindControl( "cbPerson" ) as RockCheckBox;
                var hfPersonId = e.Item.FindControl( "hfPersonId" ) as HiddenField;
                var txtExistingEmail = e.Item.FindControl( "txtExistingEmail" ) as RockTextBox;
                cbPerson.Label = currentPerson.FullName;
                hfPersonId.Value = currentPerson.Id.ToString();
                txtExistingEmail.Text = currentPerson.Email;
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the pnbPhone control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void pnbPhone_TextChanged( object sender, EventArgs e )
        {
            var rockContext = new RockContext();
            var personService = new PersonService( rockContext );
            var personList = personService.GetByPhonePartial( pnbPhone.Text ).ToList();

            if ( personList.Any() )
            {
                rptPersonPicker.DataSource = personList;
                rptPersonPicker.DataBind();
                divNewPerson.Visible = true;
            }
            else
            {
                divNoResults.Visible = true;
                divNewPerson.Visible = true;
            }
        }

        /// <summary>
        /// Handles the Click event of the lbSaveAccount control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbSaveAccount_Click( object sender, EventArgs e )
        {
            var rockContext = new RockContext();

            if ( string.IsNullOrWhiteSpace( TransactionCode ) )
            {
                nbSaveAccount.Text = "Sorry, the account information cannot be saved as there's not a valid transaction code to reference";
                nbSaveAccount.Visible = true;
                return;
            }

            if ( phCreateLogin.Visible )
            {
                if ( string.IsNullOrWhiteSpace( txtUserName.Text ) || string.IsNullOrWhiteSpace( txtPassword.Text ) )
                {
                    nbSaveAccount.Title = "Missing Informaton";
                    nbSaveAccount.Text = "A username and password are required when saving an account";
                    nbSaveAccount.NotificationBoxType = NotificationBoxType.Danger;
                    nbSaveAccount.Visible = true;
                    return;
                }

                if ( new UserLoginService( rockContext ).GetByUserName( txtUserName.Text ) != null )
                {
                    nbSaveAccount.Title = "Invalid Username";
                    nbSaveAccount.Text = "The selected Username is already being used.  Please select a different Username";
                    nbSaveAccount.NotificationBoxType = NotificationBoxType.Danger;
                    nbSaveAccount.Visible = true;
                    return;
                }

                if ( txtPasswordConfirm.Text != txtPassword.Text )
                {
                    nbSaveAccount.Title = "Invalid Password";
                    nbSaveAccount.Text = "The password and password confirmation do not match";
                    nbSaveAccount.NotificationBoxType = NotificationBoxType.Danger;
                    nbSaveAccount.Visible = true;
                    return;
                }
            }

            if ( !string.IsNullOrWhiteSpace( txtSaveAccount.Text ) )
            {
                GatewayComponent gateway = hfPaymentTab.Value == "ACH" ? _achGateway : _ccGateway;
                var ccCurrencyType = DefinedValueCache.Read( new Guid( Rock.SystemGuid.DefinedValue.CURRENCY_TYPE_CREDIT_CARD ) );
                var achCurrencyType = DefinedValueCache.Read( new Guid( Rock.SystemGuid.DefinedValue.CURRENCY_TYPE_ACH ) );

                string errorMessage = string.Empty;

                Person authorizedPerson = null;
                string referenceNumber = string.Empty;
                int? currencyTypeValueId = hfPaymentTab.Value == "ACH" ? achCurrencyType.Id : ccCurrencyType.Id;

                var transaction = new FinancialTransactionService( rockContext ).GetByTransactionCode( TransactionCode );
                if ( transaction != null )
                {
                    authorizedPerson = transaction.AuthorizedPersonAlias.Person;
                    referenceNumber = gateway.GetReferenceNumber( transaction, out errorMessage );
                }

                if ( authorizedPerson != null )
                {
                    if ( phCreateLogin.Visible )
                    {
                        var user = UserLoginService.Create(
                            rockContext,
                            authorizedPerson,
                            Rock.Model.AuthenticationServiceType.Internal,
                            EntityTypeCache.Read( Rock.SystemGuid.EntityType.AUTHENTICATION_DATABASE.AsGuid() ).Id,
                            txtUserName.Text,
                            txtPassword.Text,
                            false );

                        var mergeObjects = new Dictionary<string, object>();
                        mergeObjects.Add( "ConfirmAccountUrl", RootPath + "ConfirmAccount" );

                        var personDictionary = authorizedPerson.ToDictionary();
                        mergeObjects.Add( "Person", personDictionary );

                        mergeObjects.Add( "User", user.ToDictionary() );

                        var recipients = new Dictionary<string, Dictionary<string, object>>();
                        recipients.Add( authorizedPerson.Email, mergeObjects );

                        // TODO FIGURE THIS OUT
                        //Rock.Communication.Email.Send( GetAttributeValue( "ConfirmAccountTemplate" ).AsGuid(), recipients, ResolveRockUrl( "~/" ), ResolveRockUrl( "~~/" ) );
                    }

                    var paymentInfo = GetPaymentInfo();

                    if ( errorMessage.Any() )
                    {
                        nbSaveAccount.Title = "Invalid Transaction";
                        nbSaveAccount.Text = string.Format( "Sorry, the account information cannot be saved. {0}", errorMessage );
                        nbSaveAccount.NotificationBoxType = NotificationBoxType.Danger;
                        nbSaveAccount.Visible = true;
                    }
                    else
                    {
                        var savedAccount = new FinancialPersonSavedAccount();
                        savedAccount.PersonAliasId = authorizedPerson.Id;
                        savedAccount.ReferenceNumber = referenceNumber;
                        savedAccount.Name = txtSaveAccount.Text;
                        savedAccount.MaskedAccountNumber = paymentInfo.MaskedNumber;
                        savedAccount.TransactionCode = TransactionCode;
                        savedAccount.GatewayEntityTypeId = gateway.TypeId;
                        savedAccount.CurrencyTypeValueId = currencyTypeValueId;
                        savedAccount.CreditCardTypeValueId = CreditCardTypeValueId;

                        var savedAccountService = new FinancialPersonSavedAccountService( rockContext );
                        savedAccountService.Add( savedAccount );
                        rockContext.SaveChanges();

                        cbSaveAccount.Visible = false;
                        txtSaveAccount.Visible = false;
                        phCreateLogin.Visible = false;
                        divSaveActions.Visible = false;

                        nbSaveAccount.Title = "Success";
                        nbSaveAccount.Text = "The account has been saved for future use";
                        nbSaveAccount.NotificationBoxType = NotificationBoxType.Success;
                        nbSaveAccount.Visible = true;
                    }
                }
                else
                {
                    nbSaveAccount.Title = "Invalid Transaction";
                    nbSaveAccount.Text = "Sorry, the account information cannot be saved as there's not a valid transaction code to reference";
                    nbSaveAccount.NotificationBoxType = NotificationBoxType.Danger;
                    nbSaveAccount.Visible = true;
                }
            }
            else
            {
                nbSaveAccount.Title = "Missing Account Name";
                nbSaveAccount.Text = "Please enter a name to use for this account";
                nbSaveAccount.NotificationBoxType = NotificationBoxType.Danger;
                nbSaveAccount.Visible = true;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnPrev control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnPrev_Click( object sender, EventArgs e )
        {
            // Previous should only be enabled on page two or three
            int pageId = hfCurrentPage.Value.AsIntegerOrNull() ?? 0;
            if ( pageId > 1 )
            {
                SetPage( pageId - 1 );
            }
        }

        /// <summary>
        /// Handles the Click event of the btnNext control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnNext_Click( object sender, EventArgs e )
        {
            string errorMessage = string.Empty;

            switch ( hfCurrentPage.Value.AsIntegerOrNull() ?? 0 )
            {
                case 1:
                    this.AddHistory( "step", "2", null );
                    PrefillPerson();
                    SetPage( 2 );
                    break;

                case 2:
                    this.AddHistory( "step", "3", null );
                    BindSelectedPerson();
                    SetPage( 3 );
                    break;

                case 3:
                    if ( VerifyPaymentInfo( out errorMessage ) )
                    {
                        if ( btnNext.Text != "Give Now" )
                        {
                            TransactionCode = string.Empty;
                        }

                        if ( ProcessConfirmation( out errorMessage ) )
                        {
                            this.AddHistory( "step", "4", null );
                            SetPage( 4 );
                        }
                        else
                        {
                            ShowMessage( NotificationBoxType.Danger, "Payment Error", errorMessage );
                        }
                    }
                    else
                    {
                        ShowMessage( NotificationBoxType.Danger, "Oops!", errorMessage );
                    }

                    break;
            }
        }

        #endregion Event methods

        #region Init methods

        /// <summary>
        /// Adds the headers.
        /// </summary>
        private void BindHeaders()
        {
            // Resolve the text field merge fields
            var configValues = new Dictionary<string, object>();
            Rock.Web.Cache.GlobalAttributesCache.Read().AttributeValues
                .Where( v => v.Key.StartsWith( "Organization", StringComparison.CurrentCultureIgnoreCase ) )
                .ToList()
                .ForEach( v => configValues.Add( v.Key, v.Value ) );
            configValues.Add( "PageNumber", hfCurrentPage.Value.AsType<int?>() ?? 0 );
            phPageHeader.Controls.Add( new LiteralControl( GetAttributeValue( "PageHeader" ).ResolveMergeFields( configValues ) ) );
            phPageFooter.Controls.Add( new LiteralControl( GetAttributeValue( "PageFooter" ).ResolveMergeFields( configValues ) ) );
            phReceipt.Controls.Add( new LiteralControl( GetAttributeValue( "ReceiptMessage" ).ResolveMergeFields( configValues ) ) );
        }

        /// <summary>
        /// Gets the accounts.
        /// </summary>
        private void GetAccounts()
        {
            var rockContext = new RockContext();
            var selectedGuids = GetAttributeValues( "Accounts" ).Select( Guid.Parse ).ToList();
            bool showAll = !selectedGuids.Any();

            bool additionalAccounts = true;
            if ( !bool.TryParse( GetAttributeValue( "AdditionalAccounts" ), out additionalAccounts ) )
            {
                additionalAccounts = true;
            }

            Accounts = new List<AccountItem>();

            // Enumerate through all active accounts that have a public name
            foreach ( var account in new FinancialAccountService( rockContext ).Queryable()
                .Where( f =>
                    f.IsActive &&
                    f.PublicName != null &&
                    f.PublicName.Trim() != string.Empty &&
                    ( f.StartDate == null || f.StartDate <= RockDateTime.Today ) &&
                    ( f.EndDate == null || f.EndDate >= RockDateTime.Today ) )
                .OrderBy( f => f.Order ) )
            {
                bool isPreSelected = showAll || selectedGuids.Contains( account.Guid );
                var accountItem = new AccountItem( account.Id, account.Order, account.Name, account.Description, account.CampusId, isPreSelected );
                if ( isPreSelected || additionalAccounts )
                {
                    Accounts.Add( accountItem );
                }
            }
        }

        /// <summary>
        /// Gets the campuses.
        /// </summary>
        /// <returns></returns>
        private void BindCampuses()
        {
            var rockContext = new RockContext();
            var campuses = new CampusService( rockContext ).Queryable()
                .OrderBy( a => a.Name ).ToList();

            //if ( campuses.Count > 1 )
            //{
            //    campuses.Insert( 0, new Campus() { Name = string.Empty } );
            //    cpCampuses.Campuses = campuses;
            //}
            //else
            //{
            //    cpCampuses.Visible = false;
            //    BindAccounts();
            //}
        }

        /// <summary>
        /// Binds the accounts.
        /// </summary>
        private void BindAccounts()
        {
            rptAccountList.DataSource = Accounts.Where( a => a.Selected ).OrderBy( a => a.Order ).ToList();
            rptAccountList.DataBind();

            //btnAddAccount.Visible = Accounts.Where( a => !a.Selected ).Any();
            btnAddAccount.DataSource = Accounts.Where( a => !a.Selected ).ToList();
            btnAddAccount.DataBind();
        }

        /// <summary>
        /// Binds the selected person's data.
        /// </summary>
        protected void BindSelectedPerson()
        {
            Person selectedPerson;
            foreach ( RepeaterItem personDisplay in rptPersonPicker.Items )
            {
                var cbPerson = (RockCheckBox)personDisplay.FindControl( "cbPerson" );
                if ( cbPerson.Checked )
                {
                    var hfPersonId = (HiddenField)personDisplay.FindControl( "hfPersonId" );
                    ViewState["PersonId"] = hfPersonId.ValueAsInt();
                }
            }

            selectedPerson = GetPerson();

            // Set personal information if there is a currently logged in person
            if ( selectedPerson != null )
            {
                var personService = new PersonService( new RockContext() );

                // Bind their default address if it exists
                Guid addressTypeGuid = Guid.Empty;
                if ( !Guid.TryParse( GetAttributeValue( "AddressType" ), out addressTypeGuid ) )
                {
                    addressTypeGuid = new Guid( Rock.SystemGuid.DefinedValue.GROUP_LOCATION_TYPE_HOME );
                }

                var address = personService.GetFirstLocation( selectedPerson.Id, DefinedValueCache.Read( addressTypeGuid ).Id );
                if ( address != null )
                {
                    txtStreet.Text = address.Location.Street1;
                    txtCity.Text = address.Location.City;
                    ddlState.SelectedValue = address.Location.State;
                    txtZip.Text = address.Location.PostalCode;
                }

                txtBillingFirstName.Text = selectedPerson.FirstName;
                txtBillingLastName.Text = selectedPerson.LastName;
                txtEmail.Text = selectedPerson.Email;
            }
            else
            {
                txtBillingFirstName.Text = txtFirstName.Text;
                txtBillingLastName.Text = txtLastName.Text;
                txtEmail.Text = txtNewEmail.Text;
            }

            tdTotal.Description = Accounts.Sum( a => a.Amount ).ToString( "C" );
        }

        /// <summary>
        /// Prefills the person if they are logged in.
        /// </summary>
        private void PrefillPerson()
        {
            if ( CurrentPerson != null )
            {
                var currentNumber = CurrentPerson.PhoneNumbers.FirstOrDefault();
                if ( currentNumber != null )
                {
                    pnbPhone.Number = currentNumber.Number;
                }

                rptPersonPicker.DataSource = new List<Person>() { CurrentPerson };
                rptPersonPicker.DataBind();
            }
        }

        #endregion Init methods

        #region Payment methods

        /// <summary>
        /// Processes the payment information.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        private bool VerifyPaymentInfo( out string errorMessage )
        {
            errorMessage = string.Empty;
            var errorMessages = new List<string>();

            // Validate that an amount was entered
            if ( Accounts.Sum( a => a.Amount ) <= 0 )
            {
                errorMessages.Add( "Make sure you've entered an amount for at least one account" );
            }

            // Validate that no negative amounts were entered
            if ( Accounts.Any( a => a.Amount < 0 ) )
            {
                errorMessages.Add( "Make sure the amount you've entered for each account is a positive amount" );
            }

            // New person, require first and last name
            if ( txtFirstName.Visible == true )
            {
                if ( string.IsNullOrWhiteSpace( txtFirstName.Text ) || string.IsNullOrWhiteSpace( txtLastName.Text ) )
                {
                    errorMessages.Add( "Make sure to enter both a first and last name" );
                }
            }

            if ( string.IsNullOrWhiteSpace( pnbPhone.Number ) )
            {
                errorMessages.Add( "Make sure to enter a valid phone number.  A phone number is required for us to attribute your contribution" );
            }

            if ( string.IsNullOrWhiteSpace( txtEmail.Text ) )
            {
                errorMessages.Add( "Make sure to enter a valid email address.  An email address is required for us to send you a payment confirmation" );
            }

            if ( hfPaymentTab.Value == "ACH" )
            {
                // Validate ach options
                if ( rblSavedAch.Items.Count > 0 && ( rblSavedAch.SelectedValueAsInt() ?? 0 ) > 0 )
                {
                    // TODO: Find saved account
                }
                else
                {
                    if ( string.IsNullOrWhiteSpace( txtBankName.Text ) )
                    {
                        errorMessages.Add( "Make sure to enter a bank name" );
                    }

                    if ( string.IsNullOrWhiteSpace( txtRoutingNumber.Text ) )
                    {
                        errorMessages.Add( "Make sure to enter a valid routing number" );
                    }

                    if ( string.IsNullOrWhiteSpace( txtAccountNumber.Text ) )
                    {
                        errorMessages.Add( "Make sure to enter a valid account number" );
                    }
                }
            }
            else
            {
                // validate cc options
                if ( rblSavedCC.Items.Count > 0 && ( rblSavedCC.SelectedValueAsInt() ?? 0 ) > 0 )
                {
                    // TODO: Find saved card
                }
                else
                {
                    if ( string.IsNullOrWhiteSpace( txtBillingFirstName.Text ) && string.IsNullOrWhiteSpace( txtBillingLastName.Text ) )
                    {
                        errorMessages.Add( "Make sure to enter the full name as it appears on your credit card or bank statement" );
                    }

                    if ( string.IsNullOrWhiteSpace( txtCreditCard.Text ) )
                    {
                        errorMessages.Add( "Make sure to enter a valid credit card number" );
                    }

                    if ( string.IsNullOrWhiteSpace( txtCVV.Text ) )
                    {
                        errorMessages.Add( "Make sure to enter a valid credit card security code" );
                    }

                    var currentMonth = RockDateTime.Today;
                    currentMonth = new DateTime( currentMonth.Year, currentMonth.Month, 1 );
                    if ( !mypExpiration.SelectedDate.HasValue || mypExpiration.SelectedDate.Value.CompareTo( currentMonth ) < 0 )
                    {
                        errorMessages.Add( "Make sure to enter a valid credit card expiration date" );
                    }
                }
            }

            if ( errorMessages.Any() )
            {
                errorMessages.Insert( 0, "We found a couple things to fix:" );
                errorMessage = errorMessages.AsDelimited( "<br/>" );
                return false;
            }

            return true;
        }

        /// <summary>
        /// Processes the confirmation.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        private bool ProcessConfirmation( out string errorMessage )
        {
            var rockContext = new RockContext();
            if ( string.IsNullOrWhiteSpace( TransactionCode ) )
            {
                GatewayComponent gateway = hfPaymentTab.Value == "ACH" ? _achGateway : _ccGateway;
                if ( gateway == null )
                {
                    errorMessage = "There was a problem creating the payment gateway information";
                    return false;
                }

                Person person = GetPerson( true );
                if ( person == null )
                {
                    errorMessage = "There was a problem creating the person information";
                    return false;
                }

                PaymentInfo paymentInfo = GetPaymentInfo();
                if ( paymentInfo == null )
                {
                    errorMessage = "There was a problem creating the payment information";
                    return false;
                }
                else
                {
                    paymentInfo.FirstName = person.FirstName;
                    paymentInfo.LastName = person.LastName;
                }

                if ( paymentInfo.CreditCardTypeValue != null )
                {
                    CreditCardTypeValueId = paymentInfo.CreditCardTypeValue.Id;
                }

                // Assume one-time gift
                var transaction = gateway.Charge( paymentInfo, out errorMessage );
                if ( transaction != null )
                {
                    transaction.TransactionDateTime = RockDateTime.Now;
                    transaction.AuthorizedPersonAlias.PersonId = person.Id;
                    transaction.GatewayEntityTypeId = gateway.TypeId;
                    transaction.TransactionTypeValueId = DefinedValueCache.Read( new Guid( Rock.SystemGuid.DefinedValue.TRANSACTION_TYPE_CONTRIBUTION ) ).Id;
                    transaction.CurrencyTypeValueId = paymentInfo.CurrencyTypeValue.Id;
                    transaction.CreditCardTypeValueId = CreditCardTypeValueId;

                    Guid sourceGuid = Guid.Empty;
                    if ( Guid.TryParse( GetAttributeValue( "Source" ), out sourceGuid ) )
                    {
                        transaction.SourceTypeValueId = DefinedValueCache.Read( sourceGuid ).Id;
                    }

                    foreach ( var account in Accounts.Where( a => a.Amount > 0 ) )
                    {
                        var transactionDetail = new FinancialTransactionDetail();
                        transactionDetail.Amount = account.Amount;
                        transactionDetail.AccountId = account.Id;
                        transaction.TransactionDetails.Add( transactionDetail );
                    }

                    // Get the batch name
                    string ccSuffix = string.Empty;
                    if ( paymentInfo.CreditCardTypeValue != null )
                    {
                        ccSuffix = paymentInfo.CreditCardTypeValue.GetAttributeValue( "BatchNameSuffix" );
                    }

                    if ( string.IsNullOrWhiteSpace( ccSuffix ) )
                    {
                        ccSuffix = paymentInfo.CurrencyTypeValue.Value;
                    }

                    string batchName = string.Format( "{0} {1}", GetAttributeValue( "BatchNamePrefix" ).Trim(), ccSuffix );

                    var batchService = new FinancialBatchService( rockContext );
                    var batch = batchService.Queryable()
                        .Where( b =>
                            b.Status == BatchStatus.Open &&
                            b.BatchStartDateTime <= transaction.TransactionDateTime &&
                            b.BatchEndDateTime > transaction.TransactionDateTime &&
                            b.Name == batchName )
                        .FirstOrDefault();
                    if ( batch == null )
                    {
                        batch = new FinancialBatch();
                        batch.Name = batchName;
                        batch.Status = BatchStatus.Open;
                        batch.BatchStartDateTime = transaction.TransactionDateTime.Value.Date.Add( gateway.BatchTimeOffset );
                        if ( batch.BatchStartDateTime > transaction.TransactionDateTime )
                        {
                            batch.BatchStartDateTime.Value.AddDays( -1 );
                        }

                        batch.BatchEndDateTime = batch.BatchStartDateTime.Value.AddDays( 1 ).AddMilliseconds( -1 );
                        batch.ControlAmount = 0;
                        batchService.Add( batch );
                        rockContext.SaveChanges();

                        batch = batchService.Get( batch.Id );
                    }

                    batch.ControlAmount += transaction.TotalAmount;

                    var transactionService = new FinancialTransactionService( rockContext );
                    transaction.BatchId = batch.Id;
                    transactionService.Add( transaction );
                    rockContext.SaveChanges();

                    TransactionCode = transaction.TransactionCode;
                }
                else
                {
                    return false;
                }

                tdTransactionCode.Description = TransactionCode;
                tdTransactionCode.Visible = !string.IsNullOrWhiteSpace( TransactionCode );

                // If there was a transaction code returned and this was not already created from a previous saved account,
                // show the option to save the account.
                //if ( !( paymentInfo is ReferencePaymentInfo ) && !string.IsNullOrWhiteSpace( TransactionCode ) )
                //{
                //    cbSaveAccount.Visible = true;
                //    pnlSaveAccount.Visible = true;
                //    txtSaveAccount.Visible = true;

                //    // If current person does not have a login, have them create a username and password
                //    phCreateLogin.Visible = !new UserLoginService( rockContext ).GetByPersonId( person.Id ).Any();
                //}
                //else
                //{
                //    pnlSaveAccount.Visible = false;
                //}

                return true;
            }
            else
            {
                pnlDupWarning.Visible = true;
                btnNext.Text = "Yes, Give Again";
                errorMessage = string.Empty;
                return false;
            }
        }

        #endregion Payment methods

        #region Navigation/Error Methods

        /// <summary>
        /// Handles the PageNavigate event of the page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="HistoryEventArgs"/> instance containing the event data.</param>
        protected void page_PageNavigate( object sender, HistoryEventArgs e )
        {
            int pageId = e.State["step"].AsIntegerOrNull() ?? 0;
            if ( pageId > 0 )
            {
                SetPage( pageId );
            }
        }

        /// <summary>
        /// Sets the page.
        /// </summary>
        /// <param name="page">The page.</param>
        private void SetPage( int page )
        {
            pnlStepOne.Visible = page == 1;
            pnlStepTwo.Visible = page == 2;
            pnlStepThree.Visible = page == 3;
            pnlStepFour.Visible = page == 4;
            divActions.Visible = page > 0;

            btnPrev.Visible = page > 1;
            btnNext.Visible = page < 4;
            btnNext.Text = page > 2 ? "Give Now" : "Next";

            hfCurrentPage.Value = page.ToString();

            pnlGiveIn60Seconds.Update();
        }

        /// <summary>
        /// Shows the message.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="title">The title.</param>
        /// <param name="text">The text.</param>
        private void ShowMessage( NotificationBoxType type, string title, string text )
        {
            if ( !string.IsNullOrWhiteSpace( text ) )
            {
                nbMessage.Text = text;
                nbMessage.Title = title;
                nbMessage.NotificationBoxType = type;
                nbMessage.Visible = true;
            }
        }

        #endregion Navigation/Error Methods

        #region Helper Methods

        /// <summary>
        /// Gets the person.
        /// </summary>
        /// <param name="create">if set to <c>true</c> [create].</param>
        /// <returns></returns>
        private Person GetPerson( bool create = false )
        {
            Person person = null;
            var rockContext = new RockContext();
            var personService = new PersonService( rockContext );

            int personId = ViewState["PersonId"] as int? ?? 0;
            if ( personId == 0 && CurrentPerson != null )
            {
                person = CurrentPerson;
            }
            else if ( personId != 0 )
            {
                person = personService.Get( personId );
            }

            if ( person == null && create )
            {
                // Create Person
                person = new Person();
                person.FirstName = txtFirstName.Text;
                person.LastName = txtLastName.Text;
                person.Email = txtNewEmail.Text;
                person.EmailPreference = EmailPreference.EmailAllowed;

                var phone = new PhoneNumber();
                phone.CountryCode = PhoneNumber.CleanNumber( pnbPhone.CountryCode );
                phone.Number = PhoneNumber.CleanNumber( pnbPhone.Number );
                phone.NumberTypeValueId = DefinedValueCache.Read( new Guid( Rock.SystemGuid.DefinedValue.PERSON_PHONE_TYPE_HOME ) ).Id;
                person.PhoneNumbers.Add( phone );

                // Create Family
                var familyGroup = GroupService.SaveNewFamily( rockContext, person, null, false );
                if ( familyGroup != null )
                {
                    GroupService.AddNewFamilyAddress(
                        rockContext,
                        familyGroup,
                        GetAttributeValue( "AddressType" ),
                        txtStreet.Text,
                        string.Empty,
                        txtCity.Text,
                        ddlState.SelectedValue,
                        txtZip.Text,
                        string.Empty );
                }

                ViewState["PersonId"] = person != null ? person.Id : 0;
            }

            return person;
        }

        /// <summary>
        /// Gets the payment information.
        /// </summary>
        /// <returns></returns>
        private PaymentInfo GetPaymentInfo()
        {
            PaymentInfo paymentInfo = null;
            if ( hfPaymentTab.Value == "ACH" )
            {
                if ( rblSavedAch.Items.Count > 0 && ( rblSavedAch.SelectedValueAsId() ?? 0 ) > 0 )
                {
                    paymentInfo = GetReferenceInfo( rblSavedAch.SelectedValueAsId().Value );
                }
                else
                {
                    paymentInfo = GetACHInfo();
                }
            }
            else
            {
                if ( rblSavedCC.Items.Count > 0 && ( rblSavedCC.SelectedValueAsId() ?? 0 ) > 0 )
                {
                    paymentInfo = GetReferenceInfo( rblSavedCC.SelectedValueAsId().Value );
                }
                else
                {
                    paymentInfo = GetCCInfo();
                }
            }

            paymentInfo.Amount = Accounts.Sum( a => a.Amount );
            paymentInfo.Email = txtEmail.Text;
            paymentInfo.Phone = PhoneNumber.FormattedNumber( pnbPhone.CountryCode, pnbPhone.Number, true );
            paymentInfo.Street1 = txtStreet.Text;
            paymentInfo.City = txtCity.Text;
            paymentInfo.State = ddlState.SelectedValue;
            paymentInfo.PostalCode = txtZip.Text;

            return paymentInfo;
        }

        /// <summary>
        /// Gets the credit card information.
        /// </summary>
        /// <returns></returns>
        private CreditCardPaymentInfo GetCCInfo()
        {
            var cc = new CreditCardPaymentInfo( txtCreditCard.Text, txtCVV.Text, mypExpiration.SelectedDate.Value );
            cc.NameOnCard = string.Format( "{0} {1}", txtBillingFirstName.Text, txtBillingLastName.Text );
            cc.FirstName = txtBillingFirstName.Text;
            cc.LastName = txtBillingLastName.Text;
            cc.LastNameOnCard = txtBillingLastName.Text;

            cc.BillingStreet1 = txtStreet.Text;
            cc.BillingCity = txtCity.Text;
            cc.BillingState = ddlState.SelectedValue;
            cc.BillingPostalCode = txtZip.Text;

            return cc;
        }

        /// <summary>
        /// Gets the ACH information.
        /// </summary>
        /// <returns></returns>
        private ACHPaymentInfo GetACHInfo()
        {
            var ach = new ACHPaymentInfo( txtAccountNumber.Text, txtRoutingNumber.Text, rblAccountType.SelectedValue == "Savings" ? BankAccountType.Savings : BankAccountType.Checking );
            ach.BankName = txtBankName.Text;
            return ach;
        }

        /// <summary>
        /// Gets the reference information.
        /// </summary>
        /// <param name="savedAccountId">The saved account unique identifier.</param>
        /// <returns></returns>
        private ReferencePaymentInfo GetReferenceInfo( int savedAccountId )
        {
            var savedAccount = new FinancialPersonSavedAccountService( new RockContext() ).Get( savedAccountId );
            if ( savedAccount != null )
            {
                var reference = new ReferencePaymentInfo();
                reference.TransactionCode = savedAccount.TransactionCode;
                reference.ReferenceNumber = savedAccount.ReferenceNumber;
                reference.MaskedAccountNumber = savedAccount.MaskedAccountNumber;
                reference.InitialCurrencyTypeValue = DefinedValueCache.Read( savedAccount.CurrencyTypeValue );
                if ( reference.InitialCurrencyTypeValue.Guid.Equals( new Guid( Rock.SystemGuid.DefinedValue.CURRENCY_TYPE_CREDIT_CARD ) ) )
                {
                    reference.InitialCreditCardTypeValue = DefinedValueCache.Read( savedAccount.CreditCardTypeValue );
                }

                return reference;
            }

            return null;
        }

        #endregion Helper Methods

        #region Helper Class

        /// <summary>
        /// Lightweight object for each contribution item
        /// </summary>
        [Serializable]
        protected class AccountItem
        {
            public int Id { get; set; }

            public int Order { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }

            public int? CampusId { get; set; }

            public bool Selected { get; set; }

            public decimal Amount { get; set; }

            public string AmountFormatted
            {
                get
                {
                    return Amount > 0 ? Amount.ToString( "F2" ) : string.Empty;
                }
            }

            public AccountItem( int id, int order, string name, string description, int? campusId, bool selected )
            {
                Id = id;
                Order = order;
                Name = name;
                Description = description;
                CampusId = campusId;
                Selected = selected;
            }
        }

        #endregion Helper Class

        #region Javascript

        /// <summary>
        /// Registers the startup script.
        /// </summary>
        private void RegisterScript()
        {
            RockPage.AddScriptLink( ResolveUrl( "~/Scripts/jquery.creditCardTypeDetector.js" ) );

            string scriptFormat = @"
            Sys.Application.add_load(function () {{
                // As amounts are entered, validate that they are numeric and recalc total
                $('.account-amount').on('change', function() {{
                    var totalAmt = Number(0);
                    $('.account-amount .form-control').each(function (index) {{
                        var itemValue = $(this).val();
                        if (itemValue != null && itemValue != '') {{
                            if (isNaN(itemValue)) {{
                                $(this).parents('div.input-group').addClass('has-error');
                            }}
                            else {{
                                $(this).parents('div.input-group').removeClass('has-error');
                                var num = Number(itemValue);
                                $(this).val(num.toFixed(2));
                                totalAmt = totalAmt + num;
                            }}
                        }}
                        else {{
                            $(this).parents('div.input-group').removeClass('has-error');
                        }}
                    }});
                    $('.total-amount').html('$ ' + totalAmt.toFixed(2));
                    return false;
                }});

                // Lookup person after keyup event with 10 chars
                $('div.number-lookup > input.form-control').on('keyup', function () {{
                    var number = $(this).val().replace(/\D/g, '');
                    if (number.length == 10) {{
                        __doPostBack($(this).attr('id'), '');
                        $('.person-picker').slideToggle();
                    }}
                }});

                // Save the state of the selected payment type pill to a hidden field so that state can
                // be preserved through postback
                $('a[data-toggle=""pill""]').on('shown.bs.tab', function (e) {{
                    var tabHref = $(e.target).attr(""href"");
                    if (tabHref == '#{0}') {{
                        $('#{1}').val('CreditCard');
                    }} else {{
                        $('#{1}').val('ACH');
                    }}
                }});

                // Detect credit card type
                $('.credit-card').creditCardTypeDetector({{ 'credit_card_logos': '.card-logos' }});

                // Hide or show a div based on selection of checkbox
                $('input:checkbox.toggle-input').unbind('click').on('click', function () {{
                    $('input:checkbox.toggle-input').not(this).prop('checked', false);
                    $('.toggle-content').not($(this).parents('.checkbox').next('.toggle-content')).slideUp();
                    $(this).parents('.checkbox').next('.toggle-content').slideToggle();
                }});

                // Disable the submit button as soon as it's clicked to prevent double-clicking
                $('a[id$=""btnNext""]').click(function() {{
			        $(this).addClass('disabled');
			        $(this).unbind('click');
			        $(this).click(function () {{
				        return false;
			        }});
                }});
            }});";

            string script = string.Format( scriptFormat, divCCPaymentInfo.ClientID, hfPaymentTab.ClientID );
            ScriptManager.RegisterStartupScript( pnlGiveIn60Seconds, this.GetType(), "giving-profile", script, true );
        }

        #endregion Javascript
    }
}