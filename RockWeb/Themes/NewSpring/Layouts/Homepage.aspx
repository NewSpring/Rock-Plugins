<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" Inherits="Rock.Web.UI.RockPage" %>

<asp:Content ID="ctMain" ContentPlaceHolderID="main" runat="server">

    <!-- Start Content Area -->

    <!-- Page Title -->
    <section id="page-title">
        <h1 class="title">
            <Rock:PageIcon ID="PageIcon" runat="server" />
            <Rock:PageTitle ID="PageTitle" runat="server" />
        </h1>
        <Rock:PageBreadCrumbs ID="PageBreadCrumbs" runat="server" />
        <Rock:PageDescription ID="PageDescription" runat="server" />
    </section>

    <section id="page-content">
        <!-- Ajax Error -->
        <div class="alert alert-danger ajax-error" style="display: none">
            <p><strong>Error</strong></p>
            <span class="ajax-error-message"></span>
        </div>

        <div class="row">
            <div class="col-md-12">
                <Rock:Zone Name="Feature" runat="server" />
            </div>
        </div>

        <div class="row">
            <div class="col-md-9">
                <div class="row">
                    <div class="col-md-12">
                        <Rock:Zone Name="All Staff Live" runat="server" />
                    </div>
                    <div class="col-md-12">
                        <Rock:Zone Name="Main Content Header" runat="server" />
                    </div>
                    <div class="col-md-6">
                        <Rock:Zone Name="Left Main" runat="server" />
                    </div>
                    <div class="col-md-6">
                        <Rock:Zone Name="Right Main" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <Rock:Zone Name="Right Sidebar" runat="server" />
            </div>
        </div>
    </section>
    <!-- End Content Area -->
</asp:Content>