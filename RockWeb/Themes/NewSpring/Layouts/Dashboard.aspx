<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" Inherits="Rock.Web.UI.RockPage" %>

<asp:Content ID="ctFeature" ContentPlaceHolderID="feature" runat="server">

    <section class="main-feature hidden">
        <div class="row">
            <div class="col-md-12">
                <Rock:Zone Name="Feature" runat="server" />
            </div>
        </div>
    </section>

</asp:Content>

<asp:Content ID="ctMain" ContentPlaceHolderID="main" runat="server">
    
	<main>

        <script>
            var randomScalingFactor = function(){ return Math.round(Math.random()*(36000-45000)+36000)};

            var lineOptions = {
                responsive: true,
                scaleFontSize: 16,
                tooltipFontSize: 16,
                bezierCurve: false,
                datasetStrokeWidth : 3,
                pointDotRadius : 6,
                pointDotStrokeWidth : 3,
            }

            var pieOptions = {
                animation: false,
                responsive: true,
                scaleFontSize: 16,
                tooltipFontSize: 16,
                bezierCurve: false,
                datasetStrokeWidth : 3,
                pointDotRadius : 6,
                pointDotStrokeWidth : 3,
            }

            var overallAttendanceData = {
                labels : ["January","February","March","April","May","June","July"],
                datasets : [
                    {
                        label: "2015",
                        fillColor: "rgba(28,104,62,0)",
                        strokeColor: "rgba(28,104,62,1)",
                        pointColor: "rgba(28,104,62,1)",
                        pointStrokeColor: "#fff",
                        data : [randomScalingFactor(),randomScalingFactor(),randomScalingFactor(),randomScalingFactor(),randomScalingFactor(),randomScalingFactor(),randomScalingFactor()]
                    },
                    {
                        label: "2014",
                        fillColor: "rgba(89,161,46,0)",
                        strokeColor: "rgba(89,161,46,1)",
                        pointColor: "rgba(89,161,46,1)",
                        pointStrokeColor: "#fff",
                        data : [randomScalingFactor(),randomScalingFactor(),randomScalingFactor(),randomScalingFactor(),randomScalingFactor(),randomScalingFactor(),randomScalingFactor()]
                    }
                ]
            }

            var volAttendanceData = {
                labels : ["January","February","March","April","May","June","July"],
                datasets : [
                    {
                        label: "2015",
                        fillColor: "rgba(28,104,62,0)",
                        strokeColor: "rgba(28,104,62,1)",
                        pointColor: "rgba(28,104,62,1)",
                        pointStrokeColor: "#fff",
                        data : [randomScalingFactor(),randomScalingFactor(),randomScalingFactor(),randomScalingFactor(),randomScalingFactor(),randomScalingFactor(),randomScalingFactor()]
                    },
                    {
                        label: "2014",
                        fillColor: "rgba(89,161,46,0)",
                        strokeColor: "rgba(89,161,46,1)",
                        pointColor: "rgba(89,161,46,1)",
                        pointStrokeColor: "#fff",
                        data : [randomScalingFactor(),randomScalingFactor(),randomScalingFactor(),randomScalingFactor(),randomScalingFactor(),randomScalingFactor(),randomScalingFactor()]
                    }
                ]
            }

            var kidspringData = [
                {
                    value: 28526,
                    color:"#6bac43",
                    highlight: "#6bac43",
                    label: "Adults"
                },
                {
                    value: 7258,
                    color: "#1c683e",
                    highlight: "#1c683e",
                    label: "KidSpring"
                }
            ]

            var fuseData = [
                {
                    value: 28526,
                    color:"#6bac43",
                    highlight: "#6bac43",
                    label: "Adults"
                },
                {
                    value: 14295,
                    color: "#1c683e",
                    highlight: "#1c683e",
                    label: "Fuse"
                }
            ]

            var vipData = [
                {
                    value: 28526,
                    color:"#6bac43",
                    highlight: "#6bac43",
                    label: "Sunday Attendance"
                },
                {
                    value: 328,
                    color: "#1c683e",
                    highlight: "#1c683e",
                    label: "VIP Visits"
                }
            ]

            window.onload = function(){
                var overallAttendance = document.getElementById("attendanceChart").getContext("2d");
                var volAttendance = document.getElementById("volAttendanceChart").getContext("2d");
                var kidspring = document.getElementById("kidspringChart").getContext("2d");
                var fuse = document.getElementById("fuseChart").getContext("2d");
                var vip = document.getElementById("vipChart").getContext("2d");

                window.attendanceChart = new Chart(overallAttendance).Line(overallAttendanceData, lineOptions);
                window.volAttendanceChart = new Chart(volAttendance).Line(volAttendanceData, lineOptions);
                window.kidspringChart = new Chart(kidspring).Doughnut(kidspringData, pieOptions);
                window.fuseChart = new Chart(fuse).Doughnut(fuseData, pieOptions);
                window.vipChart = new Chart(vip).Doughnut(vipData, pieOptions);
            }
        </script>
        
        <!-- Start Content Area -->
        
        <!-- Ajax Error -->
        <div class="alert alert-danger ajax-error" style="display:none">
            <p><strong>Error</strong></p>
            <span class="ajax-error-message"></span>
        </div>

        <!-- Main Dashboard -->
        <section id="page-title">
            <h1 class="title"><Rock:PageIcon ID="PageIcon" runat="server" /> <Rock:PageTitle ID="PageTitle" runat="server" /></h1>
            <Rock:PageBreadCrumbs ID="PageBreadCrumbs" runat="server" />
            <Rock:PageDescription ID="PageDescription" runat="server" />
        </section>

        <div class="container">
            <div class="soft">

                <div class="row">

                    <div class="col-md-12">
                        <Rock:Zone Name="Section A" runat="server" />
                    </div>

                    <Rock:Zone Name="Section B" runat="server" />

                </div>

                <div class="row">

                    <div class="col-md-12">
                        <Rock:Zone Name="Section C" runat="server" />
                    </div>

                    <Rock:Zone Name="Section D" runat="server" />
                    
                </div>

                <div class="row">

                    <div class="col-md-12">
                        <Rock:Zone Name="Section E" runat="server" />
                    </div>

                    <Rock:Zone Name="Section F" runat="server" />

                </div>

                <div class="row">

                    <div class="col-md-6">

                        <Rock:Zone Name="Section G" runat="server" />

                    </div>
                    <div class="col-md-6">

                        <Rock:Zone Name="Section H" runat="server" />
                        
                    </div>
                </div>
            </div>
        </div>
        <!-- End Content Area -->

	</main>
        
</asp:Content>

