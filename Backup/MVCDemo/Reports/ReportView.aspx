<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<% string temp = "text" + Session["temp"]; 
   
   %>
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
                var report = Session["report"] as Farm.Controllers.Raisers.ReportModel;
                if (report != null)
                {
                    ReportDataSource reportDataSource = new ReportDataSource("DataSet1", report.dataSource);
                    ReportViewer1.LocalReport.ReportPath = System.AppDomain.CurrentDomain.BaseDirectory + "/Reports/" + report.reportName;
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(reportDataSource);
                    ReportViewer1.LocalReport.DisplayName = report.displayName;
                }
            
            }
        }
    
    </script>

    <div id="cc" style=" height:100%; text-align:center;">

            <form id="form1" runat="server" >
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>

                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" 
                    Font-Size="8pt" InteractiveDeviceInfos="(集合)" WaitMessageFont-Names="Verdana" 
                    WaitMessageFont-Size="14pt" PageCountMode="Actual" 
                    
                    BorderStyle="Solid" BorderWidth="1" BorderColor="ControlDark"
                    BackColor="Control"
                    Style=" width:100%; height:100%; overflow:auto;"
                    >

                </rsweb:ReportViewer>
    
            </form>

    </div>


