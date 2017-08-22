<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html style="height:100%;width:100%;">
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=100" />

    <link rel="stylesheet" type="text/css" href="/Content/themes/default/easyui.css"/>
   	<link rel="stylesheet" type="text/css" href="/Content/demo.css"/>

    <script src="/Content/js/jquery-1.7.2.js" type="text/javascript"></script>	
	<script type="text/javascript" src="/Content/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="/Content/easyui-helper.js"></script>

    <script runat="server">
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var report = Session["report"] as MvcApp.Models.Reports.ReportModel;
                if (report != null)
                {
                    ReportDataSource reportDataSource = new ReportDataSource("DataSet1", report.dataSource);
                    ReportViewer1.LocalReport.ReportPath = System.AppDomain.CurrentDomain.BaseDirectory + "/Report/" + report.reportName;
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(reportDataSource);
                    ReportViewer1.LocalReport.DisplayName = report.displayName;
                    ReportViewer1.LocalReport.SetParameters(report.Parameters);

                    /*
                    string userAgent = Request.ServerVariables.Get("HTTP_USER_AGENT");
                    if (userAgent.Contains("MSIE 7.0") || userAgent.Contains("MSIE 8.0"))
                    {
                        ReportViewer1.Attributes.Add("style", "margin-bottom: 30px;");
                    }
                    */
                    //ReportViewer1.Attributes.Add("data-options", "fit:true");
                    //ReportViewer1.CssClass = "easyui-panel";
                    ReportViewer1.KeepSessionAlive = false;
                    
                    
                    
                    
                }
            
            }
        }
    
    </script>
</head>
<body style="padding:0;margin:0;border:0px solid red;height:100%;"
     onresize="bodyresize()">


            <form id="form1" runat="server" >
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>

                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" AsyncRendering="true"
                    Font-Size="8pt" InteractiveDeviceInfos="(集合)" WaitMessageFont-Names="Verdana" Height="100%"
                    WaitMessageFont-Size="14pt" PageCountMode="Actual" SizeToReportContent="false"
                    Style="width:100%;" 
                    >

                </rsweb:ReportViewer>

            </form>

    <script language="javascript">
        
        function bodyresize() {
            var h;
            if ($.browser.msie) { //IE浏览器
                h = (document.body.clientHeight - 30).toString() + "px";
                //$('#div_r').css("height", h);
                $('#ReportViewer1').css("height", h);
            }
            else {
                h = (document.body.clientHeight - 0).toString() + "px";
                //$('#div_r').css("height", h);
                $('#ReportViewer1').css("height", h);
            }
        }
        
       $(document).ready(function(){
           bodyresize();
           
           //alert($.browser.version);

       })
       

    </script>

</body>
</html>