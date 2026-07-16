<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dc/action/WFB2DC0600_Add_batch.aspx.cs" Inherits="WebContent_fb2dc_WFB2DC0600_Add_batch" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">

        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.ym').mask('9999/99/99');
            $("#txt_DEPT_NAME").attr("readonly", true);
            $("#txt_WORK_SHIFT_DESC").attr("readonly", true);
            $("#txt_EMP_NAME").attr("readonly", true);
            gridviewScroll();
            $.unblockUI();
        }

        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());

        }
        function gridviewScroll() {

            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "999",
                height: "300",
                barcolor: "#7F7F7F"

            });

        }

        //清空畫面
        function ClearAll() {
            $("#txt_JOIN_DT").val("");
            $("#txt_DEPT_NO").val("");
            $("#txt_DEPT_NAME").val("");
            $("#ddl_PLANT_CD").val("-1");
            $("#ddl_STATION_CD").val("-1");
            $("#txt_WORK_SHIFT_CD").val("");
            $("#txt_WORK_SHIFT_DESC").val("");
            $("#ddl_LINE_CD").val("-1");

        }

        function CheckSearch() {

        }
        //儲存前檢查
        function saveCheck() {
            var processed = true;

            if (Page_ClientValidate("GroupB")) {


                BlockUI();
            }
            else
                processed = false;
            if (!processed)
                $.unblockUI();

            return processed;
        }
        //儲存前檢查
        function saveCheck2() {
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
        function getEmp() {
            OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N','Y');

            //var json = OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N');
            //if (json != undefined) {
            //    $("#lb_NEW_PLANT_CD").text(json.PLANT_NAME);
            //    $("#lb_NEW_WORK_SHIFT_CD").text(json.WORK_SHIFT_DESC);
            //}

        }

        function returnEMPValueToPage(eid, ename, value) {
            var returnValue = value;
            if (value == undefined) {
                returnValue = 'undefined';
            }
            //alert(returnValue);
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                $('#' + eid).val(obj.EMP_ID.trim());
                $('#' + ename).val(obj.EMP_NAME.trim());
                $("#lb_NEW_DEPT_NAME").text(obj.DEPT_FULL_NAME);
                $("#lb_NEW_PLANT_CD").text(obj.PLANT_NAME);
                $("#lb_NEW_WORK_SHIFT_CD").text(obj.WORK_SHIFT_DESC);
                
            }
        }

        function checkConfirm() {
            var result = confirm("是否確定取消?");
            if (result)
                document.location = "WFB2DC0600_Qry.aspx";
        }
        function CheckDelAction() {
            if (LookUpCheckboxs() > 0)
                return confirm('<%=Resources.Resource.wfb2sk_Del_ConfirmMessage%>');
            else {
                alert('<%=Resources.Resource.wfb2sk_Del_NotChoiceMessage%>');
                return false;
            }
        }
        function CheckModeifyAction() {
            if (LookUpCheckboxs() == 1) {
                return true;
            }
            else {
                alert('<%=Resources.Resource.wfb2hc_CheckBox_NotChoiceOneMessage%>');
                return false;
            }
        }
        //檢查勾了幾個checkbox
        function LookUpCheckboxs() {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }

        function openQry() {
            window.location.href("WFB2DC0600_Qry.aspx");
            return false;
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020px" border="0" class="Body_Label">
                <colgroup>
                    <col width="12%" />
                    <col width="18%" />
                    <col width="15%" />
                    <col width="28%" />
                    <col width="22%" />
                    <col width="5%" />
                </colgroup>
                <tbody>
                    <!-- START: 1st Line in Search Criteria area -->

                    <tr id="tr1">
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_ABNORMAL_TYPE" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_ABNORMAL_TYPE%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_ABNORMAL_TYPE" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ABNORMAL_TYPE%>"
                                ControlToValidate="ddl_ABNORMAL_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_ABNORMAL_REASON_CD" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_ABNORMAL_REASON_CD%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" colspan="3">
                            <asp:DropDownList ID="ddl_ABNORMAL_REASON_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ABNORMAL_REASON_CD%>"
                                ControlToValidate="ddl_ABNORMAL_REASON_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="tr2">
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_CALENDAR_DT" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CALENDAR_DT%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_CALENDAR_DT" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="MandatoryField date ym"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_CALENDAR_DT%>"
                                ControlToValidate="txt_CALENDAR_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cv_CALENDAR_DT" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CALENDAR_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_CALENDAR_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </td>

                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_ABNORMAL_DT" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_ABNORMAL_DT2%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_ABNORMAL_DT" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="MandatoryField date ym"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ABNORMAL_DT%>"
                                ControlToValidate="txt_ABNORMAL_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cv_ABNORMAL_DT" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_ABNORMAL_DT_format%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_ABNORMAL_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </td>

                        <td align="left" class="Body_label" colspan="2">
                            <asp:DropDownList ID="ddl_HOUR" runat="server" CssClass="MandatoryField">
                                <asp:ListItem Text="" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="00" Value="00"></asp:ListItem>
                                <asp:ListItem Text="01" Value="01"></asp:ListItem>
                                <asp:ListItem Text="02" Value="02"></asp:ListItem>
                                <asp:ListItem Text="03" Value="03"></asp:ListItem>
                                <asp:ListItem Text="04" Value="04"></asp:ListItem>
                                <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                <asp:ListItem Text="06" Value="06"></asp:ListItem>
                                <asp:ListItem Text="07" Value="07"></asp:ListItem>
                                <asp:ListItem Text="08" Value="08"></asp:ListItem>
                                <asp:ListItem Text="09" Value="09"></asp:ListItem>
                                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                <asp:ListItem Text="23" Value="23"></asp:ListItem>
                            </asp:DropDownList>
                            時
						                        <asp:DropDownList ID="ddl_MINUTE" runat="server" CssClass="MandatoryField">
                                                </asp:DropDownList>
                            分
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ABNORMAL_DT_HOUR%>"
                                                ControlToValidate="ddl_HOUR" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ABNORMAL_DT_MINUTE%>"
                                ControlToValidate="ddl_MINUTE" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="tr3">
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_IS_CONFIRM" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_IS_CONFIRM%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_IS_CONFIRM" runat="server" CssClass="MandatoryField">
                                <asp:ListItem Text="" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                                <asp:ListItem Text="N" Value="N"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_IS_CONFIRM%>"
                                ControlToValidate="ddl_IS_CONFIRM" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="IS_RE_MAKE" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_IS_RE_MAKE%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" colspan="3">
                            <asp:RadioButtonList ID="rbl_IS_RE_MAKE" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="是" Value="Y"></asp:ListItem>
                                <asp:ListItem Text="否" Value="N" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>

                    </tr>
                    <tr id="tr4">
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_REMARK%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" colspan="5">
                            <asp:TextBox ID="txt_REMARK" runat="server" MaxLength="210" Width="500px" ClientIDMode="Static"></asp:TextBox>
                        </td>
                    </tr>

                </tbody>
            </table>
            <fieldset style="padding: 5px; padding-top:10px; border-color: red; width: 1000px;">
                <legend class="Body_label"><font color="Red" size="+1">人員選取</font></legend>

                <table width="1000px" border="0">
                    <colgroup>
                        <col width="12%" />
                        <col width="18%" />
                        <col width="15%" />
                        <col width="28%" />
                        <col width="12%" />
                        <col width="15%" />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_JOIN_DT" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_JOIN_DT%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_JOIN_DT" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date ym"></asp:TextBox>
                                <asp:CustomValidator ID="cv_JOIN_DT" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_JOIN_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_JOIN_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            </td>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_DEPT_NO%>"></asp:Label>:</th>
                            <td align="left" class="Body_label" colspan="3"><span style="text-align: left">
                                <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" Width="64px" ClientIDMode="Static" OnTextChanged="txt_DEPT_NO_TextChanged" AutoPostBack="true"></asp:TextBox>
                                <input id="bt_DEPT_SEARCH" type="button" value="..." onclick="OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', 'N');" />
                                <asp:TextBox ID="txt_DEPT_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_PLANT_CD" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_PLANT_CD%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:DropDownList ID="ddl_PLANT_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                            </td>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_WORK_SHIFT_CD" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_WORK_SHIFT_CD%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_WORK_SHIFT_CD" runat="server" MaxLength="2" Width="64px" ClientIDMode="Static" OnTextChanged="txt_WORK_SHIFT_CD_TextChanged" AutoPostBack="true"></asp:TextBox>
                                <input id="btn_WORK_SHIFT_CD" type="button" value="..." onclick="OpenSearch('WorkShift_Search.aspx', 'txt_WORK_SHIFT_CD', 'txt_WORK_SHIFT_DESC', '');" />
                                <asp:TextBox ID="txt_WORK_SHIFT_DESC" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                            </td>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_LINE_CD" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_LINE_CD%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:DropDownList ID="ddl_LINE_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="Body_label" colspan="6">
                                <div id="init">
                                    <aces:Btn ID="WFB2DC0600Search" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0600Search%>" OnClick="WFB2DC0600Search_Click" />
                                    <%--<asp:Button ID="WFB2DC0600Search" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0600Search%>" OnClick="WFB2DC0600Search_Click" />--%>
                                    <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2dc_btn_clear%>" onclick="ClearAll();" />
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td align="right" colspan="6">
                                    
                                                <aces:Btn ID="WFB2DC0600Add" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0600Add%>" OnClick="WFB2DC0600Add_Click" />
                                                    <aces:Btn ID="WFB2DC0600Delete" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0600Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2DC0600Delete_Click" Visible="False" />
                                                    <aces:Btn ID="WFB2DC0600Save" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0600Save%>" Visible="false" OnClick="WFB2DC0600Save_Click" OnClientClick="CheckValid();" ValidationGroup="GroupB" />

                               <%-- <asp:Button ID="WFB2DC0600Add" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0600Add%>" OnClick="WFB2DC0600Add_Click" />
                                <asp:Button ID="WFB2DC0600Delete" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0600Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2DC0600Delete_Click" Visible="False" />
                                <asp:Button ID="WFB2DC0600Save" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0600Save%>" Visible="false" OnClick="WFB2DC0600Save_Click" OnClientClick="CheckValid();" ValidationGroup="GroupB" />--%>

                                <asp:Button ID="WFB2DC0600Cancel" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0600Cancel%>" Visible="false" OnClick="WFB2DC0600Cancel_Click" OnClientClick="return confirm('是否確定取消?');" />
                            </td>
                        </tr>
                    </tbody>
                </table>
                <div id="editPage1" style="font-weight: normal; width: 980px; border-width: 1px;">
                    <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getBatchData"
                        SelectCountMethod="getBatchCount" TypeName="CFB2DC0600DAO" EnablePaging="True"
                        SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                        OnSelected="ods1_Selected">
                        <SelectParameters>
                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                            <asp:Parameter Name="maximumRows" Type="Int32" />
                            <asp:ControlParameter ControlID="txt_JOIN_DT"
                                Name="join_dt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                            <asp:ControlParameter ControlID="txt_DEPT_NO" DefaultValue=""
                                Name="dept_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                            <asp:ControlParameter ControlID="ddl_PLANT_CD" DefaultValue=""
                                Name="plant_cd" PropertyName="SelectedValue" Type="String" />
                            <asp:ControlParameter ControlID="ddl_LINE_CD" DefaultValue=""
                                Name="line_cd" PropertyName="SelectedValue" Type="String" />
                            <asp:ControlParameter ControlID="txt_WORK_SHIFT_CD"
                                Name="work_shift_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                            <asp:ControlParameter ControlID="hid_AddEMP"
                                Name="AddEmp" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                            <asp:ControlParameter ControlID="hid_DeleteEMP"
                                Name="DeleteEmp" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                            <asp:ControlParameter ControlID="hid_SearchFlag"
                                Name="SearchFlag" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                        AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                        OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated"
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
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_RowNumber%>" HeaderStyle-Width="60px">
                                <ItemTemplate>
                                    <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Label ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" size="3" ClientIDMode="Static" class="MandatoryField" OnTextChanged="txt_EMP_ID_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    <input id="bt_EMP_ID" type="button" value="..." onclick="getEmp();" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_EMP_ID%>"
                                        ControlToValidate="txt_EMP_ID" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                </FooterTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <FooterStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>' ClientIDMode="Static"></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" Width="98px"></asp:TextBox>
                                </FooterTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <FooterStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_PLANT_CD%>" SortExpression="PLANT_CD" HeaderStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Label ID="lb_PLANT_CD" runat="server" Text='<%#Bind("PLANT_NAME")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lb_PLANT_CD" runat="server" Text='<%#Bind("PLANT_NAME")%>'></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lb_NEW_PLANT_CD" runat="server" ClientIDMode="Static"></asp:Label>
                                </FooterTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <FooterStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_DEPT_NAME%>" SortExpression="DEPT_FULL_NAME" HeaderStyle-Width="310px">
                                <ItemTemplate>
                                    <asp:Label ID="lb_DEPT_NAME" runat="server" Text='<%#Bind("DEPT_FULL_NAME")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lb_DEPT_NAME" runat="server" Text='<%#Bind("DEPT_FULL_NAME")%>'></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lb_NEW_DEPT_NAME" runat="server" ClientIDMode="Static"></asp:Label>
                                </FooterTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <FooterStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_WORK_SHIFT_CD%>" SortExpression="WORK_SHIFT_CD" HeaderStyle-Width="300px">
                                <ItemTemplate>
                                    <asp:Label ID="lb_WORK_SHIFT_CD" runat="server" Text='<%#Bind("WORK_SHIFT_DESC")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lb_WORK_SHIFT_CD" runat="server" Text='<%#Bind("WORK_SHIFT_DESC")%>'></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lb_NEW_WORK_SHIFT_CD" runat="server" ClientIDMode="Static"></asp:Label>
                                </FooterTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <FooterStyle HorizontalAlign="Left" />
                            </asp:TemplateField>

                        </Columns>
                        <EmptyDataTemplate>

                            <table class="grid-view" width="1000px" border="1" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF">
                                <colgroup>
                                    <col width="30px" />
                                    <col width="60px" />
                                    <col width="100px" />
                                    <col width="100px" />
                                    <col width="100px" />
                                    <col width="310px" />
                                    <col width="300px" />
                                </colgroup>
                                <tr class="header">
                                    <td></td>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2ha_RowNumber%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_EMP_ID%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_EMP_NAME%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_PLANT_CD%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label17" runat="server" Text="<%$Resources:Resource,wfb2dc_DEPT_NAME%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2dc_WORK_SHIFT_CD%>"></asp:Label>
                                    </td>

                                </tr>
                                <tr class="normal">
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" size="3" ClientIDMode="Static" class="MandatoryField" OnTextChanged="txt_EMP_ID_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        <input id="bt_EMP_ID" type="button" value="..." onclick="getEmp();" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_EMP_ID%>"
                                            ControlToValidate="txt_EMP_ID" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" Width="98px"></asp:TextBox>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lb_NEW_PLANT_CD" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lb_NEW_DEPT_NAME" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lb_NEW_WORK_SHIFT_CD" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>

                                </tr>
                            </table>

                        </EmptyDataTemplate>
                        <HeaderStyle CssClass="GridviewScrollHeader" />
                        <PagerStyle CssClass="GridviewScrollPager" />
                        <EditRowStyle CssClass="normal" />
                        <FooterStyle HorizontalAlign="Center" />
                        <EditRowStyle HorizontalAlign="Center" />
                    </asp:GridView>
                    <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                        <tr valign="top">

                            <td class="GridviewScrollPager TD">
                                <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static" AutoPostBack="true">
                                    <asp:ListItem Text="每頁10000筆" Value="10000"></asp:ListItem>
                                    <%--<asp:ListItem Text="每頁20筆" Value="20"></asp:ListItem>
                                                    <asp:ListItem Text="每頁30筆" Value="30"></asp:ListItem>
                                                    <asp:ListItem Text="每頁40筆" Value="40"></asp:ListItem>
                                                    <asp:ListItem Text="每頁50筆" Value="50"></asp:ListItem>--%>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 5px"></td>
                            <td style="font-size: 14px;">
                                <asp:Label ID="lb_TotalCount" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hid_AddEMP" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hid_DeleteEMP" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hid_SearchFlag" runat="server" ClientIDMode="Static" />
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
                </div>
            </fieldset>

            <table width="1020px" border="0" cellspacing="0" cellpadding="0" style="padding-top:20px;" >
                <tr>
                    <td align="right" class="Body_label">
                        <%-- 產生 --%>
                        <div id="saveFunction" style="text-align: right;">
                            <aces:Btn ID="WFB2DC0600Add2" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0600Add2%>" OnClick="WFB2DC0600Add2_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
                            <%--<asp:Button ID="WFB2DC0600Add2" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0600Add2%>" OnClick="WFB2DC0600Add2_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />--%>
                            <asp:Button ID="btn_back" runat="server" Text="<%$Resources:Resource,wfb2dc_btn_back%>" OnClick="btn_back_Click" OnClientClick="return confirm('是否確定取消?');" />
                            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
