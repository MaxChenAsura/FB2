<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="WebContent_Default" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <script type="text/javascript" src="../Scripts/Basic.js"></script>
    <title></title>

</head>
<body>
    <form id="form1" runat="server">
    <div>

       
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>

        <input id="bt_test" type="button" value="..." onclick="OpenMultiSelect('TextBox1', 'TB_H_M_PJOB', 'PJOB_DESC', 'PJOB_CD');" />
        <asp:Button ID="Button1" runat="server" Text="pdf產製" OnClick="Button1_Click" />
        <br />
        <br />
        <asp:Button ID="Button2" runat="server" Text="寄信測試" OnClick="Button2_Click" />
       
    </div>
    </form>
</body>
</html>
