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
                // var kidspring = document.getElementById("kidspringChart").getContext("2d");
                // var fuse = document.getElementById("fuseChart").getContext("2d");
                // var vip = document.getElementById("vipChart").getContext("2d");

                window.attendanceChart = new Chart(overallAttendance).Line(overallAttendanceData, lineOptions);
                // window.kidspringChart = new Chart(kidspring).Doughnut(kidspringData, pieOptions);
                // window.fuseChart = new Chart(fuse).Doughnut(fuseData, pieOptions);
                // window.vipChart = new Chart(vip).Doughnut(vipData, pieOptions);
            }
        </script>
        
        <!-- Start Content Area -->
        
        <!-- Ajax Error -->
        <div class="alert alert-danger ajax-error" style="display:none">
            <p><strong>Error</strong></p>
            <span class="ajax-error-message"></span>
        </div>

        <div class="container" style="padding-top: 30px;">
            <div class="row">
                <div class="col-md-12">
                    <h1>Attendance Dashboard</h1>
                </div>
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
                <div class="col-md-3">
                    <div class="panel panel-block">
                        <div class="panel-heading clearfix">
                            <h1 class="panel-title pull-left">This Week</h1>
                        </div>
                        <div class="panel-body">
                            <h1 class="flush">30,211 <i class="fa fa-fw fa-caret-down brand-danger pull-right"></i></h1>
                        </div>   
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="panel panel-block">
                        <div class="panel-heading clearfix">
                            <h1 class="panel-title pull-left">This Week Last Year</h1>
                        </div>
                        <div class="panel-body">
                            <h1 class="flush">26,742 <i class="fa fa-fw fa-caret-up brand-success pull-right"></i></h1>
                        </div>   
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="panel panel-block">
                        <div class="panel-heading clearfix">
                            <h1 class="panel-title pull-left">Last Week</h1>
                        </div>
                        <div class="panel-body">
                            <h1 class="flush">31,528 <i class="fa fa-fw fa-caret-up brand-success pull-right"></i></h1>
                        </div>   
                    </div>
                </div>
                <!-- <div class="col-md-4">
                    <p><a class="btn btn-primary full-width" href="#" role="button">View Campus Dashboard</a></p>
                </div> -->
            </div>

            <div class="row">
                <div class="col-md-12">
                    <h1>Annual Stats</h1>
                </div>
                <div class="col-md-3">
                    <div class="panel panel-block">
                        <div class="panel-heading clearfix">
                            <h1 class="panel-title pull-left">Salvations (YTD)</h1>
                        </div>
                        <div class="panel-body">
                            <h2 class="flush">8,216</h2>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="panel panel-block">
                        <div class="panel-heading clearfix">
                            <h1 class="panel-title pull-left">Baptisms (YTD)</h1>
                        </div>
                        <div class="panel-body">
                            <h2 class="flush">18,681</h2>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="panel panel-block">
                        <div class="panel-heading clearfix">
                            <h1 class="panel-title pull-left">Fuse (YTD)</h1>
                        </div>
                        <div class="panel-body">
                            <h2 class="flush">121,396</h2>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="panel panel-block">
                        <div class="panel-heading clearfix">
                            <h1 class="panel-title pull-left">KidSpring (YTD)</h1>
                        </div>
                        <div class="panel-body">
                            <h2 class="flush">536,396</h2>
                        </div>
                    </div>
                </div>
            </div>
            <br>

            <div class="row">                
                <div class="col-md-12">
                    <h1>Staff Information</h1>
                </div>

                <div class="col-md-4">
                    <div class="panel panel-block panel-chart">
                        <div class="panel-heading clearfix">
                            <h1 class="panel-title pull-left">Staff News</h1>
                        </div>
                        <div class="panel-body">
                            <h3>Staff Contact Lookup on your Phone</h3>
                            <p>Lorem ipsum Amet do incididunt ut commodo in deserunt dolore cillum in sed esse et deserunt consequat ut dolore.</p>
                            <a href="#" class="link--arrow">Read More</a>
                            <br><br>
                            <h3>Switching HR/Payroll Software</h3>
                            <p>Lorem ipsum Consectetur Ut incididunt anim sed officia ad sed sed reprehenderit sint irure sit.</p>
                            <a href="#" class="link--arrow">Read More</a><br><br>
                            <p class="flush"><a class="btn btn-primary full-width" href="#" role="button">More News</a></p>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="panel panel-block panel-chart">
                        <div class="panel-heading clearfix">
                            <h1 class="panel-title pull-left">Staff Events</h1>
                        </div>
                        <div class="panel-body">
                            <h3>July 2nd, 2015</h3>
                            <p>Offices Closed - Fourth of July</p>
                            <br>
                            <h3>July 9th, 2015</h3>
                            <p>July All-Staff</p>
                            <br>
                            <h3>August 20th, 2015</h3>
                            <p>August All-Staff</p>
                            <br>
                            <p class="flush"><a class="btn btn-primary full-width" href="#" role="button">More Events</a></p>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="panel panel-block panel-chart">
                        <div class="panel-heading clearfix">
                            <h1 class="panel-title pull-left">Staff Updates</h1>
                        </div>
                        <div class="panel-body">
                            <h3>Joseph Ze McDerment</h3>
                            <p>Join me in celebrating the McDerment Family! Jon (CEN - Next Steps Director) and his wife, Ragan brought Joseph Ze McDerment home from China a few weeks ago. We are so excited for the McDerments and their growing family!</p>
                            <br>
                            <h3>Darious Smith</h3>
                            <p>No longer on staff</p>
                            <br>
                            <p class="flush"><a class="btn btn-primary full-width" href="#" role="button">More Staff Updates</a></p>
                        </div>
                    </div>
                </div>
            </div>

            <!-- <div class="row">
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
                <div class="col-md-4 centered">
                    <div class="panel panel-block panel-chart">
                        <div class="panel-heading clearfix">
                            <h1 class="panel-title pull-left">Fuse/Sunday</h1>
                        </div>
                        <div class="panel-body">
                            <canvas id="fuseChart"></canvas>
                        </div>   
                    </div>
                </div>
                <div class="col-md-4 centered">
                    <div class="panel panel-block panel-chart">
                        <div class="panel-heading clearfix">
                            <h1 class="panel-title pull-left">VIP/Sunday</h1>
                        </div>
                        <div class="panel-body">
                            <canvas id="vipChart"></canvas>
                        </div>   
                    </div>
                </div>
            </div> -->

            <div class="row">
                <div class="col-md-3">
                    
                </div>
                <div class="col-md-3">
                    
                </div>
                <div class="col-md-3">
                    
                </div>
                <div class="col-md-3">
                    
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

