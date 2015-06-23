<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" Inherits="Rock.Web.UI.RockPage" %>

<asp:Content ID="ctFeature" ContentPlaceHolderID="feature" runat="server">

    <section class="main-feature">
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

            var kidspringData = [
                {
                    value: randomScalingFactor(),
                    color:"#6BAC43",
                    highlight: "#456F2B",
                    label: "Adults"
                },
                {
                    value: 6281,
                    color: "#369dd5",
                    highlight: "#267098",
                    label: "KidSpring"
                }
            ]

            window.onload = function(){
                var overallAttendance = document.getElementById("attendanceChart").getContext("2d");
                var kidspring = document.getElementById("kidspringChart").getContext("2d");

                window.attendanceChart = new Chart(overallAttendance).Line(overallAttendanceData, lineOptions);
                window.kidspringChart = new Chart(kidspring).Doughnut(kidspringData, pieOptions);
            }
        </script>
        
        <!-- Start Content Area -->
        
        <!-- Ajax Error -->
        <div class="alert alert-danger ajax-error" style="display:none">
            <p><strong>Error</strong></p>
            <span class="ajax-error-message"></span>
        </div>

        <div style="padding: 30px;">
            <div class="row">
                <div class="col-md-9">
                    <div class="panel panel-block panel-chart">
                        <div class="panel-heading clearfix">
                            <h1 class="panel-title pull-left">Last 6 Months</h1>
                        </div>
                        <div class="panel-body">
                            <canvas id="attendanceChart"></canvas>
                        </div>   
                    </div>
                </div>
                <div class="col-md-3 col-sm-4">
                    <div class="panel panel-block">
                        <div class="panel-heading clearfix">
                            <h1 class="panel-title pull-left">This Week</h1>
                        </div>
                        <div class="panel-body">
                            <h1>30,211 <i class="fa fa-fw fa-caret-down brand-danger pull-right"></i></h1>
                        </div>   
                    </div>
                </div>
                <div class="col-md-3 col-sm-4">
                    <div class="panel panel-block">
                        <div class="panel-heading clearfix">
                            <h1 class="panel-title pull-left">This Week Last Year</h1>
                        </div>
                        <div class="panel-body">
                            <h1>26,742 <i class="fa fa-fw fa-caret-up brand-success pull-right"></i></h1>
                        </div>   
                    </div>
                </div>
                <div class="col-md-3 col-sm-4">
                    <div class="panel panel-block">
                        <div class="panel-heading clearfix">
                            <h1 class="panel-title pull-left">Last Week</h1>
                        </div>
                        <div class="panel-body">
                            <h1>31,528 <i class="fa fa-fw fa-caret-up brand-success pull-right"></i></h1>
                        </div>   
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-4 centered">
                    <div class="panel panel-block panel-chart">
                        <div class="panel-heading clearfix">
                            <h1 class="panel-title pull-left">KidSpring/Adults</h1>
                        </div>
                        <div class="panel-body">
                            <canvas id="kidspringChart"></canvas>
                        </div>   
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="panel panel-block panel-chart">
                        <div class="panel-heading clearfix">
                            <h1 class="panel-title pull-left">Fuse/Sunday</h1>
                        </div>
                        <div class="panel-body">
                            
                        </div>   
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="panel panel-block panel-chart">
                        <div class="panel-heading clearfix">
                            <h1 class="panel-title pull-left">VIP/Sunday</h1>
                        </div>
                        <div class="panel-body">
                            
                        </div>   
                    </div>
                </div>
            </div>

        </div>
        
        <div class="hidden">
        <div class="row">
            <div class="col-md-12">
                <Rock:Zone Name="Section A" runat="server" />
            </div>
        </div>

        <div class="row">
            <div class="col-md-4">
                <Rock:Zone Name="Section B" runat="server" />
            </div>
            <div class="col-md-4">
                <Rock:Zone Name="Section C" runat="server" />
            </div>
            <div class="col-md-4">
                <Rock:Zone Name="Section D" runat="server" />
            </div>
            <div class="col-md-4">
                <Rock:Zone Name="Section E" runat="server" />
            </div>
            <div class="col-md-4">
                <Rock:Zone Name="Section F" runat="server" />
            </div>
            <div class="col-md-4">
                <Rock:Zone Name="Section G" runat="server" />
            </div>
        </div>
        </div>

        <!-- End Content Area -->

	</main>
        
</asp:Content>

