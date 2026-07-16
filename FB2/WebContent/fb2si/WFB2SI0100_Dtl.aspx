<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2si/WFB2SI0100_Dtl.aspx.cs" Inherits="WebContent_fb2si_WFB2SI0100_Dtl" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $(".ymd").mask('9999/99/99');
            //$(".money").formatNumber({ format: "#,###", locale: "tw" });
            gridviewScroll();
            $.unblockUI();
            //工號取得姓名的ajax
            $("#txt_EMP_ID").change(function () {
                if ($("#txt_EMP_ID").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_EMP_ID').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        cache: false,
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_EMP_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_EMP_NAME').val($.trim(JData.EMP_NAME));
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                }
                else {
                    $('#txt_EMP_NAME').val("");
                }
            });
        }
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"
                , freezesize: 9
            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
        }
        function CheckDelAction() {
            if (LookUpCheckboxs() > 0)
                return confirm($('#hid_wfb2si_Del_ConfirmMessage').val());
            else {
                alert($('#hid_wfb2si_Del_NotChoiceMessage').val());
                return false;
            }
        }
        //下載
        function checkDowning(msg) {
            BlockUI();
            if (confirm("確定要進行" + msg)) {
                return true;
            }
            else {
                $.unblockUI();
                return false;
            }
        }
        //檢核一括更新
        function CheckUpdateAction() {
            if (LookUpCheckboxs() > 0) {
                if ($("#ddl_PAY_STATUS_up").val() == "") {
                    alert($('#hid_wfb2si_PayStatus_NotChoiceMessage').val());
                    return false;
                }
                else {
                    return confirm($('#hid_wfb2si_Upd_ConfirmMessage').val());
                }
            }
            else {
                alert($('#hid_wfb2si_Upd_NotChoiceMessage').val());
                return false;
            }
        }

        function LookUpCheckboxs() {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked && ItemCheckBoxs[i].id.indexOf("cb_all") == -1) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }
        //上傳檢查
        function checkUpload(msg) {
            var processed = true;
            if (Page_ClientValidate("GroupB")) {
                //BlockUI();
            }
            else {
                processed = false;
                return false;
            }
            BlockUI();
            processed = confirm("確定要進行" + msg);
            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }
        //清空
        function doClear() {
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#ddl_EMP_CHG_CD").val("-1");
            $("#txt_LEVEL_CD").val("");
            $("#ddl_PAY_STATUS").val("-1");
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="20%" />
                                <col width="25%" />
                                <col width="20%" />
                                <col width="25%" />
                                <col width="20%" />

                            </colgroup>
                            <tbody>

                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BONUS_YEAR" runat="server" Text="<%$Resources:Resource,wfb2si_BONUS_YEAR%>"></asp:Label>
                                    </th>
                                    <td colspan="3">
                                        <asp:TextBox ID="txt_BONUS_YEAR" runat="server" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2si_BONUS_DAYS%>"></asp:Label>
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txt_BONUS_DAYS" runat="server" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BONUS_DT" runat="server" Text="<%$Resources:Resource,wfb2si_lb_BONUS_DT%>"></asp:Label>
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txt_BONUS_DT" runat="server" ReadOnly="true" BorderWidth="0" CssClass="ymd"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BONUS_TOTAL_AMOUNT" runat="server" Text="<%$Resources:Resource,wfb2si_BONUS_TOTAL_AMOUNT%>"></asp:Label>
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txt_BONUS_TOTAL_AMOUNT" runat="server" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BONUS_TOTAL_DECIMAL" runat="server" Text="<%$Resources:Resource,wfb2si_BONUS_TOTAL_DECIMAL%>"></asp:Label>
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txt_BONUS_TOTAL_DECIMAL" runat="server" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_TRANS_DT" runat="server" Text="<%$Resources:Resource,wfb2si_lb_SALARY_TRANS_DT%>"></asp:Label>
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txt_SALARY_TRANS_DT" runat="server" ReadOnly="true" BorderWidth="0" CssClass="ymd"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_APPROVE_STATUS" runat="server" Text="<%$Resources:Resource,wfb2si_APPROVE_STATUS%>"></asp:Label>
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txt_APPROVE_STATUS" runat="server" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2si_REMARK%>"></asp:Label>
                                    </th>
                                    <td colspan="5">
                                        <asp:TextBox TextMode="MultiLine" Rows="5" ID="txt_REMARK" runat="server" Width="100%" Enabled="false" BorderWidth="1" Style="overflow: auto"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_Excel_Download" runat="server" Text="<%$Resources:Resource,wfb2si_Excel_Download%>"></asp:Label>
                                    </th>
                                    <td colspan="3">

                                        <aces:Btn ID="WFB2SI0101download_maintain" runat="server" Text="<%$Resources:Resource,wfb2si_download_maintain%>" OnClick="download_maintain_Click" OnClientClick="return checkDowning(this.value);" />
                                        <aces:Btn ID="WFB2SI0101download_original" runat="server" Text="<%$Resources:Resource,wfb2si_download_original%>" OnClick="download_original_Click" OnClientClick="return checkDowning(this.value);" />
                                        <%--
                                        <asp:Button ID="WFB2SI0101download_maintain" runat="server" Text="<%$Resources:Resource,wfb2si_download_maintain%>" OnClick="download_maintain_Click" OnClientClick="return checkDowning(this.value);" />
                                        <asp:Button ID="WFB2SI0101download_original" runat="server" Text="<%$Resources:Resource,wfb2si_download_original%>" OnClick="download_original_Click" OnClientClick="return checkDowning(this.value);" />
                                        --%>  
                                    </td>
                                </tr>
                            </tbody>

                        </table>
                        <tr>
                            <td colspan="5">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <fieldset style="padding: 5px; border-color: red;">
                                    <legend class="Body_label">資料上傳</legend>
                                    <table cellspacing="7" cellpadding="1" width="100%" border="0" class="Body_Label">
                                        <tr>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_Bonus_Target" runat="server" Text="<%$Resources:Resource,wfb2si_Bonus_Target%>"></asp:Label>
                                            </th>
                                            <td style="border: hidden">
                                                <asp:Label ID="lb_Excel_Upload_all" runat="server" Text="<%$Resources:Resource,wfb2si_Excel_Upload_all%>"></asp:Label>
                                                <asp:FileUpload ID="FileUpload1" runat="server" CssClass="MandatoryField" Width="500px" />
                                                <%-- 檢查附檔名只能全大寫或全小寫的xls,xlsx --%>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="FileUpload1" ForeColor="Red" ValidationGroup="GroupB"
                                                    ErrorMessage="<%$Resources:Resource,wfb2_format_excel%>" ValidationExpression="^([a-zA-Z].*|[1-9].*)\.(xls|XLS|xlsx|XLSX)$" Display="None"></asp:RegularExpressionValidator>
                                                <%-- 路徑是否為空白 --%>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="FileUpload1" ForeColor="Red" ValidationGroup="GroupB"
                                                    ErrorMessage="<%$Resources:Resource,wfb2_required_excel%>" Display="None"></asp:RequiredFieldValidator>

                                            </td>
                                            <td style="border: hidden">

                                                <aces:Btn ID="WFB2SI0100Upload" runat="server" Text="<%$Resources:Resource,wfb2si_Import%>" OnClick="WFB2SI0100Upload_Click" OnClientClick="return checkUpload(this.value);" />
                                                <aces:Btn ID="WFB2SI0100download_example" runat="server" Text="<%$Resources:Resource,wfb2si_download_example%>" OnClick="download_example_Click" OnClientClick="BlockUI();" />
                                                <%--
                                                <asp:Button ID="WFB2SI0100Upload" runat="server" Text="<%$Resources:Resource,wfb2si_Import%>" OnClick="WFB2SI0100Upload_Click" OnClientClick="return checkUpload(this.value);" />
                                                <asp:Button ID="WFB2SI0100download_example" runat="server" Text="<%$Resources:Resource,wfb2si_download_example%>" OnClick="download_example_Click" OnClientClick="BlockUI();" />
                                                --%>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table cellspacing="1" cellpadding="1" width="1020px" border="0" class="Body_Label">
                                    <colgroup>
                                        <col width="10%" />
                                        <col width="26%" />
                                        <col width="10%" />
                                        <col width="26%" />
                                        <col width="10%" />
                                        <col width="28%" />
                                    </colgroup>
                                    <tbody>

                                        <tr>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2si_EMP_ID%>"></asp:Label>
                                            </th>
                                            <td>
                                                <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="50px" ClientIDMode="Static"></asp:TextBox>
                                                <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N', '');" />
                                            </td>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2si_EMP_NAME%>"></asp:Label>
                                            </th>
                                            <td>
                                                <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="50" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_EMP_CHG_CD" runat="server" Text="<%$Resources:Resource,wfb2si_EMP_CHG_CD%>"></asp:Label>
                                            </th>
                                            <td>
                                                <asp:DropDownList ID="ddl_EMP_CHG_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                            </td>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_LEVEL_CD" runat="server" Text="<%$Resources:Resource,wfb2si_LEVEL_CD%>"></asp:Label>
                                            </th>
                                            <td>
                                                <asp:TextBox ID="txt_LEVEL_CD" runat="server" MaxLength="3" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_PAY_STATUS" runat="server" Text="<%$Resources:Resource,wfb2si_PAY_STATUS%>"></asp:Label>:
                                            </th>
                                            <td colspan="3">
                                                <asp:DropDownList ID="ddl_PAY_STATUS" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <td align="right" class="Body_label" colspan="4">
                                                <div id="init">

                                                    <aces:Btn ID="WFB2SI0101Search" runat="server" Text="<%$Resources:Resource,wfb2si_search%>" OnClick="WFB2SI0101Search_Click" ValidationGroup="GroupA" OnClientClick="BlockUI();" />
                                                    <%--
                                                    <asp:Button ID="WFB2SI0101Search" runat="server" Text="<%$Resources:Resource,wfb2si_search%>" OnClick="WFB2SI0101Search_Click" ValidationGroup="GroupA" OnClientClick="BlockUI();" />
                                                    --%>
                                                    <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2sk_clear%>" OnClientClick="return doClear();" />
                                                    <asp:Button ID="btn_backpage" runat="server" Text="<%$Resources:Resource,wfb2si_backpage%>" OnClick="btn_backpage_Click" OnClientClick="BlockUI();" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="8">
                                                <hr />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="lb_PAY_STATUS2" runat="server" Text="<%$Resources:Resource,wfb2si_PAY_STATUS%>"></asp:Label>
                                                <asp:DropDownList ID="ddl_PAY_STATUS_up" runat="server" ClientIDMode="Static"></asp:DropDownList>

                                                <aces:Btn ID="WFB2SI0100Update" runat="server" Text="<%$Resources:Resource,wfb2si_Update%>" OnClientClick="return CheckUpdateAction();BlockUI();" OnClick="WFB2SI0100Update_Click" />
                                                <%--
                                                <asp:Button ID="WFB2SI0100Update" runat="server" Text="<%$Resources:Resource,wfb2si_Update%>" OnClientClick="return CheckUpdateAction();BlockUI();" OnClick="WFB2SI0100Update_Click" />
                                                --%>
                                            </td>
                                            <td align="right" class="Body_label" colspan="4">
                                                <div id="init_grid">

                                                    <aces:Btn ID="WFB2SI0101Delete" runat="server" Text="<%$Resources:Resource,wfb2si_del%>" OnClientClick="return CheckDelAction();" OnClick="WFB2SI0101Delete_Click" Visible="false" />
                                                    <%--
                                                    <asp:Button ID="WFB2SI0101Delete" runat="server" Text="<%$Resources:Resource,wfb2si_del%>" OnClientClick="return CheckDelAction();" OnClick="WFB2SI0101Delete_Click" Visible="false" />
                                                    --%>
                                                </div>

                                            </td>
                                        </tr>
                                    </tbody>

                                </table>
                            </td>
                        </tr>
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>


            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="GetDtlData2"
                SelectCountMethod="GetDtlCount" TypeName="CFB2SI0100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">

                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_BONUS_YEAR" DefaultValue=""
                        Name="bonus_year" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_EMP_ID" DefaultValue=""
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_EMP_NAME" DefaultValue=""
                        Name="emp_name" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_EMP_CHG_CD" DefaultValue=""
                        Name="emp_chg_cd" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="txt_LEVEL_CD" DefaultValue=""
                        Name="level_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_PAY_STATUS" DefaultValue=""
                        Name="pay_status" PropertyName="SelectedValue" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>


            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1200px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">

                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectNonDisabledCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2si_RowNum%>" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Width="60px" Style="text-align: center;" ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label Width="60px" Style="text-align: center;" ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </EditItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_APPROVE_MARK%>" SortExpression="APPROVE_MARK" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Width="60px" Style="text-align: center;" ID="lb_APPROVE_MARK" runat="server" Text='<%#Bind("APPROVE_MARK")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label Width="60px" Style="text-align: center;" ID="lb_APPROVE_MARK" runat="server" Text='<%#Bind("APPROVE_MARK")%>'></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_CHG_STATUS%>" SortExpression="CHG_STATUS" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: left;" ID="lb_CHG_STATUS" runat="server" Text='<%#Bind("CHG_STATUS")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label Width="80px" Style="text-align: left;" ID="lb_CHG_STATUS" runat="server" Text='<%#Bind("CHG_STATUS")%>'></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_APPROVE_FLAG%>" SortExpression="APPROVE_FLAG" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Width="60px" Style="text-align: left;" ID="lb_APPROVE_FLAG" runat="server" Text='<%#Bind("APPROVE_FLAG_DESC")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label Width="60px" Style="text-align: left;" ID="lb_APPROVE_FLAG" runat="server" Text='<%#Bind("APPROVE_FLAG_DESC")%>'></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_WS_CD%>" SortExpression="WS_CD" HeaderStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label Width="120px" Style="text-align: left;" ID="lb_WS_CD" runat="server" Text='<%#Bind("WS_CD_DESC")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label Width="120px" Style="text-align: left;" ID="lb_WS_CD" runat="server" Text='<%#Bind("WS_CD_DESC")%>'></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_lb_LEVEL_CD%>" SortExpression="LEVEL_CD" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label Width="40px" Style="text-align: left;" ID="lb_LEVEL_CD" runat="server" Text='<%#Bind("LEVEL_CD")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label Width="40px" Style="text-align: left;" ID="lb_LEVEL_CD" runat="server" Text='<%#Bind("LEVEL_CD")%>'></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_EMP_ID1%>" SortExpression="EMP_ID" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Width="60px" Style="text-align: left;" ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label Width="60px" Style="text-align: left;" ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_EMP_NAME1%>" SortExpression="EMP_NAME" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: left;" ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label Width="80px" Style="text-align: left;" ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_WORK_DAYS%>" SortExpression="WORK_DAYS" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Width="60px" Style="text-align: right;" ID="lb_WORK_DAYS" runat="server" Text='<%#Bind("WORK_DAYS")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_WORK_DAYS" runat="server" MaxLength="5" Width="80px" Text='<%#Bind("WORK_DAYS")%>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_ATTEND_DAYS%>" SortExpression="ATTEND_DAYS" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Width="60px" Style="text-align: right;" ID="lb_ATTEND_DAYS" runat="server" Text='<%#Bind("ATTEND_DAYS")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_ATTEND_DAYS" runat="server" MaxLength="7" Width="80px" Text='<%#Bind("ATTEND_DAYS")%>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_BONUS_WORK_DAYS%>" SortExpression="BONUS_WORK_DAYS" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Width="60px" Style="text-align: right;" ID="lb_BONUS_WORK_DAYS" runat="server" Text='<%#Bind("BONUS_WORK_DAYS")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_BONUS_WORK_DAYS" runat="server" MaxLength="7" Width="80px" Text='<%#Bind("BONUS_WORK_DAYS")%>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_REWARD_DAYS%>" SortExpression="REWARD_DAYS" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Width="60px" Style="text-align: right;" ID="lb_REWARD_DAYS" runat="server" Text='<%#Bind("REWARD_DAYS")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_REWARD_DAYS" runat="server" MaxLength="7" Width="80px" Text='<%#Bind("REWARD_DAYS")%>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_DISCIPLINE_DAYS%>" SortExpression="DISCIPLINE_DAYS" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Width="60px" Style="text-align: right;" ID="lb_DISCIPLINE_DAYS" runat="server" Text='<%#Bind("DISCIPLINE_DAYS")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_DISCIPLINE_DAYS" runat="server" MaxLength="7" Width="80px" Text='<%#Bind("DISCIPLINE_DAYS")%>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_BONUS_AMT%>" SortExpression="BONUS_AMT" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: right;" ID="lb_BONUS_AMT" runat="server" Text='<%#Bind("BONUS_AMT", "{0:N0}")%>' ClientIDMode="Static"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label Width="80px" Style="text-align: right;" ID="lb_BONUS_AMT" runat="server" MaxLength="7" Text='<%#Bind("BONUS_AMT", "{0:N0}")%>' ClientIDMode="Static"></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_PAY_STATUS1%>" SortExpression="PAY_TYPE" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Width="60px" Style="text-align: left;" ID="lb_PAY_STATUS" runat="server" Text='<%#Bind("PAY_TYPE")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_EDIT_PAY_STATUS" runat="server"></asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_EMP_CHG_CD1%>" SortExpression="EMP_CHG_CD" HeaderStyle-Width="140px">
                        <ItemTemplate>
                            <asp:Label Width="140px" Style="text-align: left;" ID="lb_EMP_CHG_CD" runat="server" Text='<%#Bind("EMP_CHG_CD")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_EMP_CHG_CD" runat="server" MaxLength="2" Text='<%#Bind("EMP_CHG_CD")%>'></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <EmptyDataTemplate>

                    <table class="grid-view" width="1020px">
                        <tr class="header">
                            <td>
                                <asp:CheckBox ID="cb_checkall" runat="server" onclick="javascript:SelectAllCheckboxes(this);" />
                            </td>
                            <td>
                                <asp:Label ID="Label" runat="server" Text="<%$Resources:Resource,wfb2si_RowNum%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Labe2" runat="server" Text="<%$Resources:Resource,wfb2si_APPROVE_MARK%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Labe3" runat="server" Text="<%$Resources:Resource,wfb2si_CHG_STATUS%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2si_APPROVE_FLAG%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Labe5" runat="server" Text="<%$Resources:Resource,wfb2si_WS_CD%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Labe6" runat="server" Text="<%$Resources:Resource,wfb2si_lb_LEVEL_CD%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Labe7" runat="server" Text="<%$Resources:Resource,wfb2si_EMP_ID1%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Labe8" runat="server" Text="<%$Resources:Resource,wfb2si_EMP_NAME1%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Labe9" runat="server" Text="<%$Resources:Resource,wfb2si_WORK_DAYS%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Labe10" runat="server" Text="<%$Resources:Resource,wfb2si_ATTEND_DAYS%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="<%$Resources:Resource,wfb2si_BONUS_WORK_DAYS%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2si_REWARD_DAYS%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2si_DISCIPLINE_DAYS%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label14" runat="server" Text="<%$Resources:Resource,wfb2si_BONUS_AMT%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label15" runat="server" Text="<%$Resources:Resource,wfb2si_PAY_STATUS1%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label16" runat="server" Text="<%$Resources:Resource,wfb2si_EMP_CHG_CD1%>"></asp:Label>
                            </td>

                        </tr>

                    </table>

                </EmptyDataTemplate>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">
                    <td class="GridviewScrollPager TD">
                        <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static" AutoPostBack="true">
                            <asp:ListItem Text="每頁10筆" Value="10"></asp:ListItem>
                            <asp:ListItem Text="每頁20筆" Value="20"></asp:ListItem>
                            <asp:ListItem Text="每頁30筆" Value="30"></asp:ListItem>
                            <asp:ListItem Text="每頁40筆" Value="40"></asp:ListItem>
                            <asp:ListItem Text="每頁50筆" Value="50"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 5px"></td>
                    <td style="font-size: 14px;">
                        <asp:Label ID="lb_TotalCount" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>

            <!--紀錄頁數(跳頁用)-->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <%--<asp:HiddenField ID="HID_PAY_STATUS_up" runat="server" ClientIDMode="Static" />--%>
            <%--<asp:HiddenField ID="HID_EMP_CHG_CD" runat="server" ClientIDMode="Static" />--%>
            <%--<asp:HiddenField ID="HID_PAY_STATUS" runat="server" ClientIDMode="Static" />--%>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
            <asp:HiddenField ID="hid_wfb2si_Del_NotChoiceMessage" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2si_Del_ConfirmMessage" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2si_PayStatus_NotChoiceMessage" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2si_Upd_ConfirmMessage" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2si_Upd_NotChoiceMessage" runat="server" ClientIDMode="Static" />
        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SI0101download_maintain" />
            <asp:PostBackTrigger ControlID="WFB2SI0101download_original" />
            <asp:PostBackTrigger ControlID="WFB2SI0100download_example" />
            <asp:PostBackTrigger ControlID="WFB2SI0100Upload" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

