<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" Inherits="Rock.Web.UI.RockPage" %>

<asp:Content ID="ctMain" ContentPlaceHolderID="main" runat="server">

        <!-- Start Content Area -->

        <!-- Page Title -->
        
        
        <section> 

            <!-- Ajax Error -->
            <div class="alert alert-danger ajax-error" style="display:none">
                <p><strong>Error</strong></p>
                <span class="ajax-error-message"></span>
            </div>

            <div class="grid">
              <div class="shell">
                  <div class="grid__item one-whole logo">
                      <img src="https://s3.amazonaws.com/ns.assets/newspring/newspring.rock.forms.logo.png" alt="NewSpring Church Logo" />
                  </div>
                <div class="grid__item one-whole">
                  <Rock:Zone Name="Main" runat="server" />
                </div>
              </div>
            </div>

        </section>

        <!-- End Content Area -->

        <script>
            $(document).ready(function () {
                $('.block-configuration').css('display', 'none');
                $('.zone-configuration').css('display', 'none');
            });
        </script>

</asp:Content>

