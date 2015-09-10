<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="RockWeb.Themes.NewSpring.Layouts.Error" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=10" />
    <title>Rock - Error</title>

    <link rel="stylesheet" href="<%= Page.ResolveUrl("~/Themes/NewSpring/Styles/bootstrap.css") %>" />
    <link rel="stylesheet" href="<%= Page.ResolveUrl("~/Themes/NewSpring/Styles/theme.css") %>" />

    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    
</head>
<body id="error" class="error">
    <form id="form1" runat="server">

        <div id="content">
            
            <div id="content-box">
                <div class="row row--fullscreen text-center">
                    <div class="col-md-12 align--middle ">
                        <div class="error-wrap">
                            <h1 class="push--bottom">That Wasn't Supposed To Happen...</h1>

                            <p>An error has occurred while processing your request. Your organization's administrators have been notified of this problem.</p>

                            <p><a onclick="history.go(-1);" class="btn btn-lg btn-primary"><span class="fa fa-arrow-left"></span> Go Back One Page</a></p>

                            <p>
                                <a class="btn btn-sm btn-primary" role="button" data-collapse-toggle="rockError">
                                    View Error Message
                                </a>
                            </p>

                            
                            <div class="row text-center">
                                <div class="col-md-6 col-md-offset-3">
                                    <div class="collapse" data-collapse-target="rockError">
                                        <asp:Literal ID="lErrorInfo" runat="server"></asp:Literal>
                                        <div class="alert alert-danger">
                                            <h4>Test Heading</h4>
                                            <p><strong>Message</strong><br>This is a message</p>
                                            <p><strong>Stack Trace</strong><br><pre>This is a stack trace detail section</pre></p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <script>
            $(document).ready(function () {

                //toggle the componenet with class msg_body
                //$(".exception-type").click(function () {
                //    $(this).next(".stack-trace").slideToggle(500);
                //});

                var collapseTrigger = document.querySelector('[data-collapse-toggle="rockError"]');

                console.log(collapseTrigger);
                <%--$(".stack-trace").hide();--%>

                //toggle the componenet with class msg_body
                $(collapseTrigger).on("click", function () {
                    console.log('clicked');
                    var collapseTarget = document.querySelector('[data-collapse-target="rockError"]');
                    $(collapseTarget).toggleClass('in');
                });
            });
        </script>

    </form>
</body>
</html>
