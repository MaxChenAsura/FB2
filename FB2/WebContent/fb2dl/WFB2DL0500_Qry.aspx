<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2dl/WFB2DL0500_Qry.aspx.cs" Inherits="WebContent_fb2hd_WFB2DL0500_Qry" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.date').mask('9999/99/99');
            gridviewScroll();
            $.unblockUI();
           

        }
        function ShowRecord(obj) {
            $("#HID_PageRow").val($("#ddlPerPageRow").val());
        }
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "400",
                barcolor: "#7F7F7F",
                //freezesize: 4

            });
            //CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
        }

        function ClearAll() {
            $('#txt_DL_GEN_DESC').val("");
        }

        function CheckMODE_ID(source, arguments) {
            var re = /^[\d|a-zA-Z]+$/;
            if (!re.test($("#txt_MODE_ID_Add").val()))
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }



        //儲存前檢查
        function saveCheck() {
            var processed = true;
            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                processed = false;
            if (!processed)
                $.unblockUI();

            return processed;
        }

        //刪除,檢查是否有勾選
        function doDelete() {
            var processed = true;
            BlockUI();

            if (checkboxsSelected() > 0) {
                processed = confirm($('#hidwfb299_Del_ConfirmMessage').val());
            } else {
                alert($('#hidwfb299_Del_NotChoiceMessage').val());
                processed = false;
            }

            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }

        //修改,檢查是否有勾選
        function doUpdate() {
            var processed = true;
            BlockUI();

            if (checkboxsSelected() ==1) {
                processed = confirm("確定要修改!");
            } else {
                alert("請選擇1筆資料!");
                processed = false;
            }

            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }

        var choietr = null;
        //回傳目前Checkbox被勾選的數量
        function checkboxsSelected() {
            choietr = null;
            var ItemCheckBoxs = $(":checkbox[id$=cb_check]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }
        //查詢前檢查
        function searchCheck() {
            var processed = true;
            BlockUI();
            
            return processed;
        }
        function CheckModeifyAction() {
            if (checkboxsSelected() == 1)
                return true;
            else {
                alert($('#hidwfb299_Mod_NotChoiceMessage').val());
                return false;
            }
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!--2DL0500_QRY開始-->
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="10%" />
                                <col width="50%" />
                                <col width="10%" />
                                <col width="30%" />

                            </colgroup>
                            <tbody> 
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DL_GEN_DESC" runat="server" Text="說明"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DL_GEN_DESC" runat="server" MaxLength="20" Columns="10" ClientIDMode="Static"></asp:TextBox>
                                    </td>                                   
                                </tr>
 
                                <tr>
                                    <th></th>
                                    <td align="right" class="Body_label" colspan="10">
                                        <div id="init">
                                            <aces:Btn ID="WFB2DL0500Search" runat="server" Text="<%$Resources:Resource,btn_search%>" OnClick="WFB2DL0500Search_Click" OnClientClick="return searchCheck();" />
                                            <input id="WFB2DL0500Clear" type="button" value="<%$Resources:Resource,btn_clear%>" runat="server" onclick="ClearAll();" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>

                    <td align="right" class="Body_label">
                        <div id="init_grid">                                                    
                            <aces:Btn ID="WFB2DL0500Add" runat="server" Text="新增" OnClick="WFB2DL0500Add_Click" />
                            <aces:Btn ID="WFB2DL0500Upd" runat="server" Text="修改" OnClientClick="return doUpdate();" OnClick="WFB2DL0500Upd_Click" Visible="False" />
                            <aces:Btn ID="WFB2DL0500Del" runat="server" Text="刪除" OnClientClick="return doDelete();" OnClick="WFB2DL0500Del_Click" Visible="False" />
                            
                            <%--
                            <asp:Button ID="WFB2DL0500Add" runat="server" Text="<%$Resources:Resource,btn_add%>" OnClick="WFB2DL0500Add_Click" />
                            <asp:Button ID="WFB2DL0500Delete" runat="server" Text="<%$Resources:Resource,btn_delete%>" OnClientClick="return doDelete();" OnClick="WFB2DL0500Delete_Click" Visible="False" />
                           --%>
                        </div>

                    </td>
                </tr>
            </table>
            <!--2DL0500_QRY結束-->
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DL0500DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_DL_GEN_DESC"
                        Name="dl_gen_desc" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="30px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2_RowNumber%>" SortExpression="RowNumber" HeaderStyle-Width="40px" />
                    <asp:BoundField DataField="DL_GEN_DESC" HeaderText="特休生成說明" SortExpression="DL_GEN_DESC" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="PROC_CD_DESC" HeaderText="作業碼" SortExpression="PROC_CD" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="SDT_CD_DESC" HeaderText="起始碼" SortExpression="SDT_CD" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="EDT_CD_DESC" HeaderText="結束碼" SortExpression="EDT_CD" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="LOGI_CD_DESC" HeaderText="邏輯碼" SortExpression="LOGI_CD" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="DL_GENDT_CD_DESC" HeaderText="特休生成日碼" SortExpression="DL_GENDT_CD" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="left" />
                    <asp:BoundField DataField="IS_D01_SAME" HeaderText="當年度復職" SortExpression="IS_D01_SAME" HeaderStyle-Width="100px"  ItemStyle-HorizontalAlign="Left" />
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">
                    <td class="GridviewScrollPager TD">
                        <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static" AutoPostBack="true">
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_10_Rows%>" Value="10"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_20_Rows%>" Value="20"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_30_Rows%>" Value="30"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_40_Rows%>" Value="40"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_50_Rows%>" Value="50"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 5px"></td>
                    <td style="font-size: 14px;">
                        <asp:Label ID="lb_TotalCount" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Del_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Del_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Mod_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Mod_NotChoiceMessage%> " />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


