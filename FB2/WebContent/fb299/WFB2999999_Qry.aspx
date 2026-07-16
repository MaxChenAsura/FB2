<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb299/WFB2999999_Qry.aspx.cs" Inherits="WebContent_fb299_WFB2999999_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script type="text/javascript">
        function test() {
            //document.forms[0].submit();		
            var uid = '<%=emp_id%>';
            window.showModalDialog('http://clacesvm01p.kuozui.com.tw/hrinfo/chgHRADPWD.aspx?uid=' + uid, self, 'dialogWidth=1000px;dialogHeight=1000px;scroll=no;addressbar:No;');
            window.close();
        }

    </script>
</head >
<body onload="test()">
    <form id="form1" method="get" action="http://clacesvm01p.kuozui.com.tw/hrinfo/chgHRADPWD.aspx">
        <input type="hidden" id="uid" name="uid" value="<%=emp_id%>" />
    </form>
</body>
</html>

