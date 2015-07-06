<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Metrics.ascx.cs" Inherits="RockWeb.Plugins.cc_newspring.Metrics.Metrics" %>

<asp:UpdatePanel ID="pnlContent" runat="server">
    <ContentTemplate>

        <asp:HiddenField ID="metricType" runat="server" />

        <%= metricType.Value %>

        <asp:Panel ID="pnlPieChart" runat="server">

            <Rock:NotificationBox ID="nbMetricWarning" runat="server" NotificationBoxType="Warning"
                Text="Please select at least one metric in the block settings." />

            <asp:Literal ID="pieTitle" runat="server" Text="Sunday Attendance Breakdown" />

            <asp:Literal ID="pieSubTitle" runat="server" Text="" />

            <Rock:PieChart ID="churchMetricNumber" runat="server" />
            <Rock:PieChart ID="pieAttendance" runat="server" />
        </asp:Panel>

        <asp:Panel ID="pnlLineChart" runat="server">

            <Rock:NotificationBox ID="lineMetricWarning" runat="server" NotificationBoxType="Warning"
                Text="Please select a metric in the block settings." />

            <asp:Literal ID="lineChartTitle" runat="server" Text="Sunday Total Attendance" />

            <asp:Literal ID="lineChartSubtitle" runat="server" />

            <Rock:LineChart ID="lineAttendance" runat="server" />
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>