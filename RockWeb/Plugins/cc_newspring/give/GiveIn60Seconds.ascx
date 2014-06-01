<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GiveIn60Seconds.ascx.cs" Inherits="RockWeb.Plugins.cc_newspring.give.GiveIn60Seconds" %>

<asp:UpdatePanel ID="pnlContent" runat="server" UpdateMode="Conditional">
<ContentTemplate>
    <asp:Panel ID="pnlHeader" runat="server">
        <Rock:BootstrapButton ID="btnBack" runat="server" Text="BACK" OnClick="btnBack_Click" />
    </asp:Panel>

    <asp:UpdatePanel ID="pnlBody" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        
        <asp:ValidationSummary ID="lblWarning" runat="server" />

        <asp:Panel ID="pnlStepOne" runat="server">
            <form class="form-horizontal">
                <asp:Repeater ID="rptAccountList" runat="server">
                    <ItemTemplate>
                        <Rock:CurrencyBox ID="txtAccountAmount" runat="server" Label='<%# Eval("Name") %>' Text='<%# Eval("AmountFormatted") %>' Placeholder="0.00" CssClass="account-amount" />
                    </ItemTemplate>
                </asp:Repeater>
            </form>

            <div class="form-group">
                <label>Total</label>
                <asp:Label ID="lblTotalAmount" runat="server" CssClass="form-control-static total-amount" />
            </div>

            <div id="divRepeatingPayments" runat="server" visible="false">
                <Rock:ButtonDropDownList ID="btnFrequency" runat="server" CssClass="btn btn-primary" Label="Frequency"
                    DataTextField="Name" DataValueField="Id" />
                <Rock:DatePicker ID="dtpStartDate" runat="server" Label="First Payment" />
            </div>
        
        </asp:Panel>

        <asp:Panel ID="pnlStepTwo" runat="server" Visible="false">         

            <Rock:PhoneNumberBox ID="txtNumber" runat="server" Label="Phone" />
            <Rock:RockCheckBox ID="chkPerson" runat="server" />
            <Rock:RockTextBox ID="txtEmail" runat="server" />            
            <Rock:CampusPicker ID="cpCampus" runat="server" Label="Your Campus"/>

        
        </asp:Panel>

        <asp:Panel ID="pnlStepThree" runat="server" Visible="false">
        
            <fieldset>

            <Rock:RockTextBox ID="txtFirstName" runat="server" Label="First Name" />
            <Rock:RockTextBox ID="txtLastName" runat="server" Label="Last Name" />
            <Rock:RockTextBox ID="txtStreet" runat="server" Label="Address" /> 
            <Rock:RockTextBox Label="City" ID="txtCity" runat="server" />
            <Rock:StateDropDownList Label="State" ID="ddlState" runat="server" UseAbbreviation="true" />
            <Rock:RockTextBox  ID="txtZip" runat="server" Label="Zip" />
            
            <div id="divCCPaymentInfo" runat="server">
                <fieldset>
                    <Rock:RockRadioButtonList ID="rblSavedCC" runat="server" Label=" " CssClass="radio-list" RepeatDirection="Vertical" DataValueField="Id" DataTextField="Name" />
                    <div id="divNewCard" runat="server" class="radio-content">
                        <Rock:RockTextBox ID="txtCardFirstName" runat="server" Label="First Name on Card" Visible="false"></Rock:RockTextBox>
                        <Rock:RockTextBox ID="txtCardLastName" runat="server" Label="Last Name on Card" Visible="false"></Rock:RockTextBox>
                        <Rock:RockTextBox ID="txtCardName" runat="server" Label="Name on Card" Visible="false"></Rock:RockTextBox>
                        <Rock:RockTextBox ID="txtCreditCard" runat="server" Label="Credit Card #" MaxLength="19" CssClass="credit-card" />
                        <ul class="card-logos">
                            <li class="card-visa"></li>
                            <li class="card-mastercard"></li>
                            <li class="card-amex"></li>
                            <li class="card-discover"></li>
                        </ul>
                        <Rock:MonthYearPicker ID="mypExpiration" runat="server" Label="Expiration Date" />
                        <Rock:NumberBox ID="txtCVV" Label="Card Security Code" runat="server" MaxLength="4" />                        
                    </div>
                </fieldset>
            </div>

            <div id="divACHPaymentInfo" runat="server">
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
                        <asp:Image ID="imgCheck" runat="server" ImageUrl="<%$ Fingerprint:~/Assets/Images/check-image.png %>" />                                    
                    </div>
                </fieldset>
            </div>

            
        
        </asp:Panel>

        <asp:Panel ID="pnlStepFour" runat="server" Visible="false">
        


        
        </asp:Panel>

        <Rock:BootstrapButton ID="btnNext" runat="server" Text="NEXT" OnClick="btnNext_Click" />

    </ContentTemplate>
    </asp:UpdatePanel>


    <asp:Panel ID="pnlFooter" runat="server">
    



    
    </asp:Panel>

    <asp:HiddenField ID="hfCurrentPage" runat="server" />
        
</ContentTemplate>
</asp:UpdatePanel>