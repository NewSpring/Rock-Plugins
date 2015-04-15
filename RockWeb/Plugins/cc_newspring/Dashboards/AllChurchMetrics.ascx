<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AllChurchMetrics.ascx.cs" Inherits="RockWeb.Plugins.cc_newspring.Dashboards.AllChurchMetrics" %>

<asp:UpdatePanel ID="pnlContent" runat="server">
    <ContentTemplate>

        <asp:Panel ID="pnlPieChart" runat="server">

            <Rock:NotificationBox ID="nbMetricWarning" runat="server" NotificationBoxType="Warning"
                Text="Please select at least one metric in the block settings." />

            <asp:Literal ID="pieTitle" runat="server" Text="Titlte" />

            <asp:Literal ID="pieSubTitle" runat="server" Text="Subtitle" />

            <Rock:PieChart ID="pieAttendance" runat="server" />
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>