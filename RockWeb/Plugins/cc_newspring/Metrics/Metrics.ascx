<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Metrics.ascx.cs" Inherits="RockWeb.Plugins.cc_newspring.Metrics.Metrics" %>
<div class="col-md-3">
<asp:UpdatePanel ID="pnlContent" runat="server">
    <ContentTemplate>

        <asp:HiddenField ID="metricClass" runat="server" />
        <asp:HiddenField ID="metricDisplay" runat="server" />
        <asp:HiddenField ID="metricNumber" runat="server" />
        <asp:HiddenField ID="metricTitle" runat="server" />


        <% if ( metricDisplay.Value.Equals( "Text" ) )
            { %>
        
            <div class="panel panel-block">
                <div class="panel-heading clearfix">
                    <h1 class="panel-title pull-left"><%= metricTitle.Value %></h1>
                </div>
                <div class="panel-body">
                    <h1 class="flush"><%= metricNumber.Value %><% if ( metricClass.Value != "" )
            { %> <i class="fa fa-fw <%= metricClass.Value %> pull-right"></i><% } %></h1>
                </div>
            </div>

            <Rock:NotificationBox ID="churchMetricWarning" runat="server" NotificationBoxType="Warning"
                Text="Please select a metric in the block settings." />
        
        <% } %>
    </ContentTemplate>
</asp:UpdatePanel>
    </div>