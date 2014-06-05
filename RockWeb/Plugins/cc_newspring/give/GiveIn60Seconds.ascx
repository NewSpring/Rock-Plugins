<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GiveIn60Seconds.ascx.cs" Inherits="RockWeb.Plugins.cc_newspring.give.GiveIn60Seconds" %>

<asp:UpdatePanel ID="pnlGiveIn60Seconds" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <!-- validation summary here? -->

        <asp:PlaceHolder ID="phPageHeader" runat="server" />

        <!-- Step One -->
        <asp:Panel ID="pnlStepOne" runat="server">

            <div class="panel panel-default contribution-info">
                <div class="panel-heading">
                    <h3 class="panel-title">Enter an Amount</h3>
                </div>
                <div class="panel-body">
                    <fieldset>

                        <Rock:CampusPicker ID="cpCampuses" runat="server" Label="Select your Campus" />

                        <asp:Repeater ID="rptAccountList" runat="server">
                            <ItemTemplate>
                                <div data-toggle="tooltip" data-placement="top" title="<%# Eval("Description") %>">
                                    <Rock:CurrencyBox ID="txtAccountAmount" runat="server" Label='<%# Eval("Name") %>' Text='<%# Eval("AmountFormatted") %>' Placeholder="0.00" CssClass="account-amount" />
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>

                        <Rock:ButtonDropDownList ID="btnAddAccount" runat="server" CssClass="btn btn-primary" Visible="false" Label=" "
                            DataTextField="Name" DataValueField="Id" OnSelectionChanged="btnAddAccount_SelectionChanged" />

                        <div class="form-group total-gift">

                            <label>Total</label>
                            <asp:Label ID="lblTotalAmount" runat="server" CssClass="form-control-static total-amount" />
                        </div>
                    </fieldset>
                </div>
            </div>
        </asp:Panel>

        <!-- Step Two -->
        <asp:Panel ID="pnlStepTwo" runat="server" Visible="false">

            <div class="panel panel-default contribution-personal">
                <div class="panel-heading">
                    <h3 class="panel-title">Personal Information</h3>
                </div>
                <div class="panel-body">
                    <fieldset>
                        <Rock:PhoneNumberBox ID="pnbPhone" runat="server" CssClass="number-lookup" Label="Phone" />

                        <asp:Repeater ID="rptPersonPicker" runat="server">
                            <ItemTemplate>
                                <div class="checkbox">
                                    <Rock:RockCheckBox ID="cbPerson" runat="server" Label='<%# Eval("FullName") %>' data-id='<%# Eval("ID") %>' CssClass="toggle-input" />
                                </div>
                                <div id="divPersonDetail" runat="server" class="toggle-content" style="display: none">
                                    <Rock:RockTextBox ID="txtExistingEmail" runat="server" Label="Email" Text='<%# Eval("Email") %>' />
                                </div>
                            </ItemTemplate>
                            <FooterTemplate>
                                <div class="checkbox">
                                    <Rock:RockCheckBox ID="cbNewPerson" runat="server" Label="... or enter a different name" CssClass="toggle-input" />
                                </div>
                                <div id="divPersonDetail" runat="server" class="toggle-content" style="display: none">
                                    <Rock:RockTextBox ID="txtFirst" runat="server" Label="First" />
                                    <Rock:RockTextBox ID="txtLast" runat="server" Label="Last" />
                                    <Rock:RockTextBox ID="txtEmail" runat="server" Label="Email" />
                                    <%--<Rock:CampusPicker ID="cpCampuses" runat="server" Label="Your Campus" />--%>
                                </div>
                            </FooterTemplate>
                        </asp:Repeater>
                    </fieldset>
                </div>
            </div>
        </asp:Panel>

        <!-- Step Three -->
        <asp:Panel ID="pnlStepThree" runat="server" Visible="false">

            <div class="panel panel-default contribution-payment">

                <asp:HiddenField ID="hfPaymentTab" runat="server" />

                <div class="panel-heading">
                    <h3 class="panel-title">Billing Address</h3>
                </div>

                <div class="panel-body">
                    <fieldset>
                        <Rock:RockTextBox ID="txtFirstName" runat="server" Label="First" />
                        <Rock:RockTextBox ID="txtLastName" runat="server" Label="Last" />
                        <Rock:RockTextBox ID="txtEmail" runat="server" Label="Email" />
                        <Rock:RockTextBox ID="txtStreet" runat="server" Label="Street" />
                        <Rock:RockTextBox ID="txtCity" runat="server" Label="City" />
                        <Rock:StateDropDownList ID="ddlState" runat="server" UseAbbreviation="true" Label="State" />
                        <Rock:RockTextBox ID="txtZip" runat="server" Label="Zip" />
                        <Rock:RockTextBox ID="txtCountry" runat="server" Label="Country" Enabled="false" Text="United States" />
                    </fieldset>
                </div>

                <div class="panel-heading">
                    <h3 class="panel-title">Payment Information</h3>
                </div>

                <asp:PlaceHolder ID="phPills" runat="server" Visible="false">
                    <ul class="nav nav-pills">
                        <li id="liCreditCard" runat="server"><a href='#<%=divCCPaymentInfo.ClientID%>' data-toggle="pill">Credit Card</a></li>
                        <li id="liACH" runat="server"><a href='#<%=divACHPaymentInfo.ClientID%>' data-toggle="pill">Bank Account</a></li>
                    </ul>
                </asp:PlaceHolder>

                <div class="tab-content">

                    <div id="divCCPaymentInfo" runat="server" visible="false">
                        <fieldset>
                            <Rock:RockRadioButtonList ID="rblSavedCC" runat="server" Label=" " CssClass="radio-list" RepeatDirection="Vertical" DataValueField="Id" DataTextField="Name" />
                            <div id="divNewCard" runat="server" class="radio-content">
                                <Rock:RockTextBox ID="txtCreditCard" runat="server" Label="Credit Card #" MaxLength="19" CssClass="credit-card" />
                                <ul class="card-logos">
                                    <li class="card-visa"></li>
                                    <li class="card-mastercard"></li>
                                    <li class="card-amex"></li>
                                    <li class="card-discover"></li>
                                </ul>
                                <Rock:NumberBox ID="txtCVV" Label="Card Security Code" runat="server" MaxLength="4" />
                                <Rock:MonthYearPicker ID="mypExpiration" runat="server" Label="Expiration Date" />
                            </div>
                        </fieldset>
                    </div>

                    <div id="divACHPaymentInfo" runat="server" visible="false">
                        <fieldset>
                            <Rock:RockRadioButtonList ID="rblSavedAch" runat="server" Label=" " CssClass="radio-list" RepeatDirection="Vertical" DataValueField="Id" DataTextField="Name" />
                            <div id="divNewBank" runat="server" class="radio-content">
                                <Rock:RockTextBox ID="txtBankName" runat="server" Label="Bank Name" />
                                <Rock:RockTextBox ID="txtRoutingNumber" runat="server" Label="Routing #" />
                                <Rock:RockTextBox ID="txtAccountNumber" runat="server" Label="Account #" />
                                <Rock:RockRadioButtonList ID="rblAccountType" runat="server" RepeatDirection="Horizontal" Label="Account Type">
                                    <asp:ListItem Text="Checking" Selected="true" />
                                    <asp:ListItem Text="Savings" />
                                </Rock:RockRadioButtonList>
                                <%--<asp:Image ID="imgCheck" runat="server" ImageUrl="<%$ Fingerprint:~/Assets/Images/check-image.png %>" />--%>
                            </div>
                        </fieldset>
                    </div>

                    <Rock:TermDescription ID="tdTotal" runat="server" Term="Total" />
                </div>
            </div>
            </div>

            <%--<asp:Panel ID="pnlDupWarning" runat="server" CssClass="alert alert-block">
                <h4>Warning!</h4>
                <p>
                    You have already submitted a transaction that has been processed.  Are you sure you want
                to submit another possible duplicate transaction?
                </p>
                <asp:LinkButton ID="btnConfirm" runat="server" Text="Yes, submit another transaction" CssClass="btn btn-primary" OnClick="btnConfirm_Click" />
            </asp:Panel>--%>
        </asp:Panel>

        <!-- Step Four -->
        <asp:Panel ID="pnlStepFour" runat="server" Visible="false">
            <div class="well">
                <legend>Gift Information</legend>

                <dl class="dl-horizontal gift-success">
                    <Rock:TermDescription ID="tdTransactionCode" runat="server" Term="Confirmation Code" />
                </dl>
            </div>

            <asp:Panel ID="pnlSaveAccount" runat="server" Visible="false">
                <div class="well">
                    <legend>Make Giving Even Easier</legend>
                    <fieldset>
                        <Rock:RockCheckBox ID="cbSaveAccount" runat="server" Text="Save account information for future gifts" CssClass="toggle-input" />
                        <div id="divSaveAccount" runat="server" class="toggle-content">
                            <Rock:RockTextBox ID="txtSaveAccount" runat="server" Label="Name for this account" CssClass="input-large" />

                            <asp:PlaceHolder ID="phCreateLogin" runat="server" Visible="false">

                                <div class="control-group">
                                    <div class="controls">
                                        <div class="alert alert-info">
                                            <b>Note:</b> For security purposes you will need to login to use your saved account information.  To create
	    			                    a login account please provide a user name and password below. You will be sent an email with the account
	    			                    information above as a reminder.
                                        </div>
                                    </div>
                                </div>

                                <Rock:RockTextBox ID="txtUserName" runat="server" Label="Username" CssClass="input-medium" />
                                <Rock:RockTextBox ID="txtPassword" runat="server" Label="Password" CssClass="input-medium" TextMode="Password" />
                                <Rock:RockTextBox ID="txtPasswordConfirm" runat="server" Label="Confirm Password" CssClass="input-medium" TextMode="Password" />
                            </asp:PlaceHolder>

                            <Rock:NotificationBox ID="nbSaveAccount" runat="server" Visible="false" NotificationBoxType="Danger" />

                            <div id="divSaveActions" runat="server" class="actions">
                                <asp:LinkButton ID="lbSaveAccount" runat="server" Text="Save Account" CssClass="btn btn-primary" OnClick="lbSaveAccount_Click" />
                            </div>
                        </div>
                    </fieldset>
                </div>
            </asp:Panel>
        </asp:Panel>

        <Rock:NotificationBox ID="nbMessage" runat="server" Visible="false" />

        <div id="divActions" runat="server" class="actions">
            <asp:LinkButton ID="btnPrev" runat="server" Text="Previous" CssClass="btn btn-link" OnClick="btnPrev_Click" Visible="false" />
            <asp:LinkButton ID="btnNext" runat="server" Text="Next" CssClass="btn btn-primary" OnClick="btnNext_Click" />
        </div>

        <asp:PlaceHolder ID="phPageFooter" runat="server" />

        <asp:HiddenField ID="hfCurrentPage" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>
