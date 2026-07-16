<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2de/WFB2DE0300_Qry.aspx.cs" Inherits="WebContent_fb2de_WFB2DE0300_Qry" %>

<%@ Register Src="~/UserControl/UCCommCodeDropDwonList.ascx" TagPrefix="uc1" TagName="UCCommCodeDropDwonList" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $('.date').datepicker({ dateFormat: 'yy/mm' });
            $('#txt_MANAGER_YM').mask('9999/99');
            $.unblockUI();

        }

        //jQuery(document).ready(function () {
        //    $('.date').datepicker({ dateFormat: 'yy/mm' });
        //    $('#txt_MANAGER_YM').mask('9999/99');
        //});

        function checkvalue() {
            var processed = true;
            //alert($("#hid_Valid_Flag").val());
            
            //檢查是否做過月度結算
            $.ajax({
                url: "WFB2DE0300_CheckData.ashx",
                data: {
                    MANAGER_YM: $('#txt_MANAGER_YM').val(),
                    PLANT_CD: $('#ddlCommCode').val()
                },
                type: "GET",
                dataType: 'text',
                async: false,
                success: function (result) {
                    if (result == "Y")
                        processed = confirm("此年度已月度結算過, 是否重新結算??");
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });

            

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                processed = false;
            if (!processed)
                $.unblockUI();
           

            return processed;

            //var ret = confirm('此年度已月度結算過, 是否重新結算??');
            //if (ret) {
            //    __doPostBack('question', 'true');

            //} else {
            //    __doPostBack('question', 'false');
            //}

        }

        //清空畫面
        function ClearAll() {
            $('#txt_MANAGER_YM').val("");
            $('#ddlCommCode').val(" ");

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table class="Body_Label" width="1048px">
        <tr>
            <td class="Body_Label" style="TEXT-ALIGN: left; background-color: #808080; width: 100px; height: 30px">
                <asp:Label ID="lb_MANAGER_YM" runat="server" Text="<%$Resources:Resource,wfb2de_MANAGER_YM%>" ForeColor="White"></asp:Label>
            </td>
            <td class="Body_Label" style="TEXT-ALIGN: left; width: 70px">
                <asp:TextBox ID="txt_MANAGER_YM" runat="server" MaxLength="7" Style="text-align: left" CssClass="MandatoryField date" ClientIDMode="Static" Width="70px"></asp:TextBox>
            </td>
            <asp:RequiredFieldValidator ID="Validator_MANAGER_YM_NotNull" runat="server" ErrorMessage="<%$Resources:Resource,wfb2de_MANAGER_YM_NotNull%>" ControlToValidate="txt_MANAGER_YM"
                ForeColor="Red" Display="None" ValidationGroup="GroupA">
            </asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                ErrorMessage="<%$Resources:Resource,wfb2de_MANAGER_YM_EER%>" ControlToValidate="txt_MANAGER_YM" ForeColor="Red"
                ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None" ValidationGroup="GroupA">
            </asp:RegularExpressionValidator>
            <td>
            </td>
        </tr>
        <tr>
            <td class="Body_Label" style="TEXT-ALIGN: left; background-color: #808080; width: 100px; height: 30px">
                <asp:Label ID="lb_PLANT_CD" runat="server" Text="<%$Resources:Resource,wfb2de_PLANT_CD%>" ForeColor="White"></asp:Label>
            </td>
            <td class="Body_Label" style="TEXT-ALIGN: left; width: 70px; height: 30px">
                <uc1:UCCommCodeDropDwonList runat="server" ID="UCCommCodeDropDwonList" ValidationGroup="GroupB" ClientIDMode="Static" />
            </td>
        </tr>
        <tr>
            <th></th>
            <th></th>
            <td class="Body_Label" style="TEXT-ALIGN: right">
                <aces:Btn ID="WFB2DE0300Execute" runat="server" Text="<%$Resources:Resource,wfb2de_WFB2DE0200Execute%>" OnClientClick="return checkvalue();"  OnClick="WFB2DE0300Execute_Click"/>

                <%--<asp:Button ID="WFB2DE0300Execute" runat="server" Text="<%$Resources:Resource,wfb2de_WFB2DE0200Execute%>" OnClientClick="return checkvalue();"  OnClick="WFB2DE0300Execute_Click"/>--%>
                
                <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2de_btn_clear%>" onclick="ClearAll();"/>                
            </td>
        </tr>
    </table>


    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
    <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
</asp:Content>

