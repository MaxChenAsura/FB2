<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2ha/WFB2HA0400_Qry.aspx.cs" Inherits="WebContent_fb2ha_WFB2HA0400_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');
            $(".number").mask('9999/99/99');
            $(".PROFESSION_ALLOWANCE").blur(function () {
                $(this).parseNumber({ format: "#,###", locale: "tw" });
                $(this).formatNumber({ format: "#,###", locale: "tw" });
            });
            $(".MANAGEMENT_ALLOWANCE").blur(function () {
                $(this).parseNumber({ format: "#,###", locale: "tw" });
                $(this).formatNumber({ format: "#,###", locale: "tw" });
            });
            //$('#txt_START_DT_S,#txt_START_DT_E').change(function () {
            //    if ($('#txt_START_DT_S').val() != '' && $('#txt_START_DT_E').val() != '') {
            //        var b = new Date($('#txt_START_DT_S').val());
            //        var a = new Date($('#txt_START_DT_E').val());
            //        if (b > a) {
            //            $(this).val("");
            //            alert("起日不可大於迄日");
            //        }
            //    }
            //});
            $(".number2").mask("999");
            gridviewScroll();
            $.unblockUI();
        }
        function getPjob() {
            var now = new Date();
            var date = now.format("yyyy/MM/dd");
            var PJOB_CD = $("#txt_PJOB_CD").val();
            alert(date);
            OpenSearch('Pjob_Search.aspx', 'txt_PJOB_CD', 'txt_PJOB_DESC', 'PJOB_CD=' + PJOB_CD + '&START_DT=' + date);

        }
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());

        }
        function gridviewScroll() {
            if ($("#HID_Freeze").val() == "Y") {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "400",
                    barcolor: "#7F7F7F",
                    freezesize: 4

                });
                CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
            }
            else {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "400",
                    barcolor: "#7F7F7F"

                });
            }
        }

        //清空畫面
        function ClearAll() {
            $("#ddl_LEVEL_CD").val("-1");
            $("#txt_START_DT_S").val("");
            $("#txt_START_DT_E").val("");
            $("#txt_END_DT_S").val("");
            $("#txt_END_DT_E").val("");
            $('#rbl_IS_VALID').find("input[value='Y']").prop("checked", "checked");

        }

        function CheckSearch() {
            //if ($("#txt_START_DT_S").val() == "" && $("#txt_START_DT_E").val() == "") {
            //    if ($("#rbl_IS_VALID input:radio:checked").val() != "ALL") {
            //        alert("未輸入生效期間，不可以點選「有效」、「無效」");
            //        return false;
            //    }
            //}
            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
        }


        function setHidPjobCD(obj) {
            var value = $("#" + obj.id).val();
            $("#HID_PJOB_CD").val(value.substring(0, 1));
        }
        function CheckDtlAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidwfb299_Dtl_NotChoiceMessage').val());
                return false;
            }
        }
        function LookUpCheckboxs() {
            var ItemCheckBoxs = $("[type$=checkbox]");
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="10%" />
                    <col width="90%" />
                </colgroup>
                <tbody>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_LEVEL_CD" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_LEVEL_CD%>"></asp:Label>:

                        </th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_LEVEL_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                    </tr>

                </tbody>
            </table>

            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="10%" />
                    <col width="25%" />
                    <col width="10%" />
                    <col width="25%" />
                    <col width="30%" />
                </colgroup>
                <tbody>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_START_DT" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_start_dt%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_START_DT_S" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date number"></asp:TextBox>
                            ~
                                <asp:TextBox ID="txt_START_DT_E" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date number"></asp:TextBox>
                            <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sh_format_award_stime%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_START_DT_S" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sh_format_award_etime%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_START_DT_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:CompareValidator ControlToValidate="txt_START_DT_E" ControlToCompare="txt_START_DT_S" ErrorMessage="<%$Resources:Resource,wfb2ha_error_start_dt_s_e%>"
                                Operator="GreaterThanEqual" Type="Date" runat="server" Display="None" ForeColor="Red" ValidationGroup="GroupA"></asp:CompareValidator>
                        </td>
                        <%--結束日期--%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_end_dt%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_END_DT_S" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="ymd date"></asp:TextBox>
                            ~
                                <asp:TextBox ID="txt_END_DT_E" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="ymd date"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_END_SDT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_END_DT_S" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator3" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_END_EDT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_END_DT_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToCompare="txt_END_DT_S"
                                ControlToValidate="txt_END_DT_E" ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_EFFECT_END_DT_RANGE3%>" Type="Date" Operator="GreaterThanEqual"
                                Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                        </td>
                        <td align="left" class="Body_label">
                            <asp:RadioButtonList ID="rbl_IS_VALID" runat="server" RepeatDirection="Horizontal" ClientIDMode="Static">
                                <asp:ListItem Text="<%$Resources:Resource,wf2da_rbVALID_Y%>" Value="Y" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:Resource,wf2da_rbVALID_N%>" Value="N"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:Resource,wf2da_rbVALID_ALL%>" Value="ALL"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="Body_label" colspan="5">
                            <div id="init">
                                <aces:Btn ID="WFB2HA0400Search" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0500Search%>" OnClick="WFB2HA0400Search_Click" OnClientClick="return CheckSearch();" />
                                <%--<asp:Button ID="WFB2HA0400Search" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0500Search%>" OnClick="WFB2HA0400Search_Click" OnClientClick="return CheckSearch();" />--%>

                                <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2ha_btn_clear%>" onclick="ClearAll();" />
                            </div>
                        </td>
                    </tr>

                    <tr>
                        <td align="center" height="1" colspan="5">
                            <hr>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="5">
                            <aces:Btn ID="WFB2HA0400Detail" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0400Detail%>" Visible="false" OnClientClick="return CheckDtlAction();" OnClick="WFB2HA0400Detail_Click" />
                            <%--<asp:Button ID="WFB2HA0400Detail" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0400Detail%>" Visible="false" OnClientClick="return CheckDtlAction();" OnClick="WFB2HA0400Detail_Click" />--%>
                        </td>
                    </tr>

                </tbody>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2HA0400DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="ddl_LEVEL_CD" DefaultValue=""
                        Name="level_cd" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="txt_START_DT_S" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="start_dt_s" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="txt_START_DT_E" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="start_dt_e" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="txt_END_DT_S"
                        Name="end_dt_s" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_END_DT_E"
                        Name="end_dt_e" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="rbl_IS_VALID" DefaultValue=""
                        Name="is_valid" PropertyName="SelectedValue" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1360px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="30px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectNonDisabledCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_RowNumber1%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_LEVEL_CD%>" SortExpression="LEVEL_CD" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEVEL_CD" runat="server" Text='<%#Bind("LEVEL_CD")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_EDIT_LEVEL_CD" runat="server" CssClass="MandatoryField"></asp:DropDownList>
                            <asp:HiddenField ID="hid_EDIT_LEVEL_CD" runat="server" Value='<%#Bind("LEVEL_CD")%>' />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_Required_LEVEL_CD%>"
                                ControlToValidate="ddl_EDIT_LEVEL_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_NEW_LEVEL_CD" runat="server" CssClass="MandatoryField"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_Required_LEVEL_CD%>"
                                ControlToValidate="ddl_NEW_LEVEL_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                   <%-- 職務區分 
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_PJOB_TYPE%>" SortExpression="PJOB_TYPE" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_PJOB_TYPE" runat="server" Text='<%#Bind("PJOB_NAME")%>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>
                    --%>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_START_DT%>" SortExpression="START_DT" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_START_DT" runat="server" Text='<%#Bind("START_DT")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_START_DT" runat="server" MaxLength="10" Width="81px" CssClass="MandatoryField date" Text='<%#Bind("START_DT")%>'></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_START_DT" runat="server" MaxLength="10" Width="81px" CssClass="MandatoryField date"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_END_DT%>" SortExpression="END_DT" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_END_DT" runat="server" Text='<%#Bind("END_DT")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_END_DT" runat="server" MaxLength="10" Width="81px" CssClass="MandatoryField date" Text='<%#Bind("END_DT")%>'></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_END_DT" runat="server" MaxLength="10" Width="81px" CssClass="MandatoryField date"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_LEVEL_PAY%>" SortExpression="LEVEL_PAY" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="right">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEVEL_PAY" runat="server" Text='<%#Bind("LEVEL_PAY","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_TOP_LEVEL_PAY%>" SortExpression="TOP_LEVEL_PAY" HeaderStyle-Width="130px" ItemStyle-HorizontalAlign="right">
                        <ItemTemplate>
                            <asp:Label ID="lb_TOP_LEVEL_PAY" runat="server" Text='<%#Bind("TOP_LEVEL_PAY","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_ABILITY_PAY_LOW%>" SortExpression="ABILITY_PAY_LOW" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="right">
                        <ItemTemplate>
                            <asp:Label ID="lb_ABILITY_PAY_LOW" runat="server" Text='<%#Bind("ABILITY_PAY_LOW","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_ABILITY_PAY_MID%>" SortExpression="ABILITY_PAY_MID" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="right">
                        <ItemTemplate>
                            <asp:Label ID="lb_ABILITY_PAY_MID" runat="server" Text='<%#Bind("ABILITY_PAY_MID","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_ABILITY_PAY_HIGH%>" SortExpression="ABILITY_PAY_HIGH" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="right">
                        <ItemTemplate>
                            <asp:Label ID="lb_ABILITY_PAY_HIGH" runat="server" Text='<%#Bind("ABILITY_PAY_HIGH","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_IS_UNION_MEMBER%>" SortExpression="IS_UNION_MEMBER" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_IS_UNION_MEMBER" runat="server" Text='<%#Bind("IS_UNION_MEMBER")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_REMARK%>" SortExpression="REMARK" HeaderStyle-Width="400" ItemStyle-HorizontalAlign="left">
                        <ItemTemplate>
                            <asp:Label ID="lb_REMARK" runat="server" Text='<%#Bind("REMARK")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_REMARK" runat="server" MaxLength="210" Width="300px" Text='<%#Bind("REMARK")%>'></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_REMARK" runat="server" MaxLength="210" Width="300px"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                </EmptyDataTemplate>
                <HeaderStyle CssClass="GridviewScrollHeader" />
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle HorizontalAlign="Center" />
                <EditRowStyle HorizontalAlign="Center" />
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
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
            <asp:HiddenField ID="HID_PJOB_CD" runat="server" ClientIDMode="Static" Value="" />
            <asp:HiddenField ID="HID_MANAGEMENT_ALLOWANCE" runat="server" ClientIDMode="Static" Value="" />
            <asp:HiddenField ID="HID_PROFESSION_ALLOWANCE" runat="server" ClientIDMode="Static" Value="" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Dtl_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Dtl_NotChoiceMessage%>" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
