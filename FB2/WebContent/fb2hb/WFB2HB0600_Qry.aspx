<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2hb/WFB2HB0600_Qry.aspx.cs" Inherits="WebContent_fb2hb_WFB2HB0600_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            //$("#txt_EMP_NAME").attr("readonly", true);
            $("#txt_DEPT_NAME").attr("readonly", true);
            $("#txt_PJOB_DESC").attr("readonly", true);

            gridviewScroll();
            $.unblockUI();
        }

        function getPjob() {
            var now = new Date();
            var date = now.format("yyyy/MM/dd");
            var PJOB_CD = $("#txt_PJOB_CD").val();
            OpenSearch('Pjob_Search.aspx', 'txt_PJOB_CD', 'txt_PJOB_DESC', 'PJOB_CD=' + PJOB_CD + '&START_DT=' + date);

        }

        function gridviewScroll() {

            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "400",
                barcolor: "#7F7F7F",
                freezesize: 4

            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");

        }
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        //清空畫面
        function ClearAll() {
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#txt_DEPT_NO").val("");
            $("#txt_DEPT_NAME").val("");
            $("#txt_PJOB_CD").val("");
            $("#txt_PJOB_DESC").val("");
            $("#ddl_LEVEL_CD_S").val("-1");
            $("#ddl_LEVEL_CD_E").val("-1");
            $("#ddl_EMP_CHG_CD").val("11");
        }

        function ExportExcel(val) {
            var keys = [];
            var values = [];
            keys[0] = "emp_id";
            values[0] = val;
            openWindowWithPost("WFB2HB0600_ExportExcel.aspx", "_blank", keys, values);
        }

        function doUnblock() {
            $.unblockUI();
        }
        function doBlock() {
            BlockUI();
        }
        //考核相關資料下載
        function checkDowning() {
            var processed = true;
            if (LookUpCheckboxs() > 0)
                processed = true;
            else {
                alert($('#hidwfb299_Dtl_NotChoiceMessage').val());
                processed = false;
            }

            if (processed) {
                processed = confirm($("#hidconfirm_Execute").val()+"?");
                BlockUI();
            }

            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }
        function LookUpCheckboxs() {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked && ItemCheckBoxs[i].id.indexOf("_freezeitem") != -1 && ItemCheckBoxs[i].id.indexOf("_freezeheader") == -1) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="8%" />
                    <col width="20%" />
                    <col width="8%" />
                    <col width="20%" />
                    <col width="8%" />
                    <col width="36%" />
                </colgroup>
                <tbody>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_EMP_ID%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="42px" ClientIDMode="Static" OnTextChanged="txt_EMP_ID_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <input id="bt_EMP_ID" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N');" />
                          
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <%--姓名--%>
                            <asp:Label ID="Label11" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_emp_name%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_NAME" runat="server" Width="80px" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_DEPT_NO%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" >
                            <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" Width="64px" ClientIDMode="Static" OnTextChanged="txt_DEPT_NO_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <input id="bt_DEPT_SEARCH" type="button" value="..." onclick="OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', 'N');" />
                            <asp:TextBox ID="txt_DEPT_NAME" runat="server" MaxLength="10" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <td align="left" class="Body_label"></td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_PJOB_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_PJOB_CD%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_PJOB_CD" runat="server" MaxLength="10" Width="64px" ClientIDMode="Static" OnTextChanged="txt_PJOB_CD_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <input id="btn_PJOB_CD" type="button" value="..." onclick="getPjob();" />
                            <asp:TextBox ID="txt_PJOB_DESC" runat="server" BorderWidth="0" ClientIDMode="Static" Width="80px"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_LEVEL_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_LEVEL_CD%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_LEVEL_CD_S" runat="server" ClientIDMode="Static"></asp:DropDownList>
                            ~ 
                    <asp:DropDownList ID="ddl_LEVEL_CD_E" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                         <%--在職區分--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="Label17" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_emp_chg_cd%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:DropDownList ID="ddl_EMP_CHG_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                            </td>
                    </tr>
                    <tr>
                        <td align="right" class="Body_label" colspan="10">
                            <div id="init">
                                  
                                    <asp:Button ID="WFB2HB0600Search" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0600Search%>" OnClick="WFB2HB0600Search_Click" OnClientClick="BlockUI();" />
                                  <%-- 
                                <aces:Btn ID="WFB2HB0600Search" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0600Search%>" OnClick="WFB2HB0600Search_Click" OnClientClick="BlockUI();" />
                                  --%>  
                                <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2hb_btn_clear%>" onclick="ClearAll();" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" height="1" colspan="10">
                            <hr>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="10">
                            <div id="init_grid" style="display: inline">
                                <aces:Btn ID="WFB2HB0600Detail" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0600Dtl%>" Visible="false" OnClick="WFB2HB0600Detail_Click" />
                                <aces:Btn ID="WFB2HB0600ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0600ExcelDown%>" OnClick="WFB2HB0600ExcelDown_Click" OnClientClick="return checkDowning();" Visible="False" />
                                <%--
                                    <asp:Button ID="WFB2HB0600Detail" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0600Dtl%>" Visible="false" OnClick="WFB2HB0600Detail_Click" />
                                <asp:Button ID="WFB2HB0600ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0600ExcelDown%>" OnClick="WFB2HB0600ExcelDown_Click" OnClientClick="return checkDowning();" Visible="False" />
                                  --%>  
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2HB0600DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_DEPT_NO" DefaultValue=""
                        Name="dept_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_PJOB_CD" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="pjob_cd" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="ddl_LEVEL_CD_S" DefaultValue=""
                        Name="level_cd_s" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="ddl_LEVEL_CD_E" DefaultValue=""
                        Name="level_cd_e" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="hid_is_super"
                        Name="is_super" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="hid_is_dept"
                        Name="is_dept" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="hid_departments"
                        Name="departments" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                     <asp:ControlParameter ControlID="txt_EMP_NAME" DefaultValue=""
                        Name="emp_name" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_EMP_CHG_CD" DefaultValue=""
                        Name="emp_chg_cd" PropertyName="SelectedValue" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
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
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2hb_RowNumber%>" SortExpression="RowNumber" ItemStyle-Width="40px" HeaderStyle-Width="40px" />
                    <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2hb_lb_EMP_ID%>" SortExpression="EMP_ID" ItemStyle-Width="60px" HeaderStyle-Width="60px" />
                    <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2hb_lb_EMP_NAME%>" SortExpression="EMP_NAME" ItemStyle-Width="80px" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="DEPT_NAME" HeaderText="<%$Resources:Resource,wfb2hb_lb_DEPT_NAME%>" SortExpression="DEPT_NO" ItemStyle-Width="450px" HeaderStyle-Width="450px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="WS_CD" HeaderText="<%$Resources:Resource,wfb2hb_lb_WS_CD%>" SortExpression="WS_CD" ItemStyle-Width="40px" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="LEVEL_CD" HeaderText="<%$Resources:Resource,wfb2hb_LEVEL_CD%>" SortExpression="LEVEL_CD" ItemStyle-Width="50px" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="PJOB_DESC" HeaderText="<%$Resources:Resource,wfb2hb_PJOB_CD%>" SortExpression="PJOB_CD" ItemStyle-Width="120px" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left"/>
                    <asp:BoundField DataField="AGE" HeaderText="<%$Resources:Resource,wfb2hb_lb_AGE%>" SortExpression="AGE" ItemStyle-Width="60px" HeaderStyle-Width="60px" HtmlEncode="false" ItemStyle-HorizontalAlign="Right"/>
                    <asp:BoundField DataField="WORK_YEARS" HeaderText="<%$Resources:Resource,wfb2hb_lb_WORK_YEARS%>" SortExpression="WORK_YEARS" ItemStyle-Width="80px" HeaderStyle-Width="80px" HtmlEncode="false" ItemStyle-HorizontalAlign="Right"/>
                    <asp:BoundField DataField="RECENT_LEVEL_WORK_DAYS" HeaderText="<%$Resources:Resource,wfb2hb_lb_RECENT_LEVEL_WORK_DAYS%>" SortExpression="RECENT_LEVEL_WORK_DAYS" ItemStyle-Width="80px" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HtmlEncode="false" />
                    <asp:BoundField DataField="EMP_CHG_DESC" HeaderText="<%$Resources:Resource,wfb2hb_lb_EMP_CHG_CD%>" SortExpression="EMP_CHG_CD" ItemStyle-Width="80px" HeaderStyle-Width="80px" HtmlEncode="false" ItemStyle-HorizontalAlign="Left"/>
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
            <asp:HiddenField ID="hid_is_super" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_is_dept" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_departments" runat="server" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Dtl_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Dtl_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidconfirm_Execute" Value="<%$Resources:Resource,confirm_Execute%>" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2HB0600ExcelDown" />
        </Triggers>
    </asp:UpdatePanel>
    <%--<iframe name="exportExcel" id="exportExcel" runat="server" src ="WFB2HB0600_ExportExcel.aspx"></iframe>--%>
</asp:Content>
