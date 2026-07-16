<%@ Control Language="C#" AutoEventWireup="true" CodeFile="~/tw/co/toyota/UserControl/UCCommCodeDropDwonList.ascx.cs" Inherits="UserControl_UCCommCodeDropDwonList" %>
<asp:DropDownList ID="ddlCommCode" runat="server" />
<asp:RequiredFieldValidator ControlToValidate="ddlCommCode" Enabled="false" ID="Validator_ddlCommCode"  runat="server" Display="None"/>  