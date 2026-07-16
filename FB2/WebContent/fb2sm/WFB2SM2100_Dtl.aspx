<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sm/WFB2SM2100_Dtl.aspx.cs" Inherits="WebContent_fb2sm_WFB2SM210_Dtl" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $("#txt_NEW_EMP_ID").mask('99999');
            $(".number2").mask("999");
            $('#txt_DEPT_NAME').attr("readonly", true);
            $('#txt_EMP_NAME').attr("readonly", true);
            $.unblockUI();
            gridviewScroll();
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
                } else {
                    $('#txt_EMP_NAME').val("");
                }
            });

            //部門代號取得部門名稱的ajax
            $("#txt_DEPT_NO").change(function () {
                if ($("#txt_DEPT_NO").val().length == 7) {
                    $.ajax({
                        url: "../commgeo/WFB2GetDeptData.ashx",
                        data: {
                            DEPT_NO: $('#txt_DEPT_NO').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_DEPT_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_DEPT_NAME').val(JData.DEPT_NAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_DEPT_NAME').val("");
                }
            });
        }

        function gridviewScroll() {
            if ($("#HID_Freeze").val() == "Y") {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "400",
                    barcolor: "#7F7F7F",
                    freezesize: 7
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
        function ShowRecord(obj) {
            $("#HID_PageRow").val($("#ddlPerPageRow").val());
        }
        function CheckSearch() {

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
        }
        function backQry() {
            location.href("WFB2SM2100_Qry.aspx");
        }
        function ClearAll() {
            $("#txt_DEPT_NO").val("");
            $("#txt_DEPT_NAME").val("");
            $("#txt_EMP_ID").val("");
            $("#ddl_LEVEL_CD").val("");
            $("#txt_EMP_NAME").val("");
            $("#ddl_LEVEL_CD_NEW").val("");
            $("#ddl_EMP_CHG_CD").val("");
            $("#ddl_WS_CD").val("");
            return false;
        }
        function CheckDelAction() {
            if (LookUpCheckboxs() > 0)
                return confirm($('#hidwfb2sc_Del_ConfirmMessage').val());
            else {
                alert($('#hidwfb2sc_Del_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckModeifyAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidwfb2sc_Mod_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckSaveAction() {
            if ($('#txtCALENDAR_CD_Edit').val() != undefined) {
                if ($('#txtCALENDAR_CD_Edit').val().trim() == "") {
                    alert($('#hidwfb2sc_txtCALENDAR_CD_NotNull').val());
                    return false;
                }
                else
                    return confirm($('#hidwfb2sc_Save_ConfirmMessage').val());
            }
            else
                return confirm($('#hidwfb2sc_Save_ConfirmMessage').val());
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
       
        function returnEMPValueToPage(emp_id, emp_name, value) {
            var returnValue = value;
            if (value == undefined) {
                returnValue = 'undefined';
            }
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                $("#txt_NEW_EMP_ID_freezeitem").val(obj.EMP_ID.trim());
                $("#txt_NEW_EMP_ID").val(obj.EMP_ID.trim());
                __doPostBack('emp_text_change', '');
                //return obj;
            }
        }

        function returnDEPTValueToPage(id, name, value) {
            var returnValue = value;
            if (value == undefined) {
                returnValue = 'undefined';
            }
            //alert(returnValue);
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                $('#' + id).val(obj.DEPT_NO);
                $('#' + name).val(obj.DEPT_NAME_20 + ' ' + obj.DEPT_NAME_40);

                return obj;

            }
        }
    </script>
    <%-- <style type="text/css">
        .auto-style1 {
            width: 268435136px;
        }
    </style>--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <%--查詢條件欄位寬度--%>
                <colgroup>
                    <col width="17%" />
                    <col width="25%" />
                    <col width="15%" />
                    <col width="43%" />
                </colgroup>

                <tbody>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_DATA_YEAR" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_DATA_YEAR%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_DATA_YEAR" runat="server" MaxLength="4" Width="81px" ClientIDMode="Static"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_DATA_SEQ" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_DATA_SEQ%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_DATA_SEQ" runat="server" MaxLength="1" Width="81px" ClientIDMode="Static"></asp:Label>
                        </td>

                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_NOTICE_DT" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_NOTICE_DT%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_NOTICE_DT" runat="server" ClientIDMode="Static"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_PROCESS_STATUS" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_PROCESS_STATUS%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_PROCESS_STATUS" runat="server" ClientIDMode="Static"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_REMARK%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" colspan="4">
                            <asp:TextBox ID="txt_REMARK" runat="server" MaxLength="2400" Width="580px" TextMode="MultiLine" Rows="3" ClientIDMode="Static" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EXECUTIVE_DATE" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_EXECUTIVE_DATE%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_EXECUTIVE_DATE" runat="server" ClientIDMode="Static"></asp:Label>
                        </td>
                        <th></th>
                        <td></td>
                        <tr>

                            <!-- end: Create MODULE ID -->
                            <!-- START: Create a line to separate Search field with body field -->
                            <tr>
                                <td align="center" height="1" colspan="4">
                                    <hr>
                                </td>
                            </tr>

                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_DEPT_NO%>"></asp:Label>:
                                </th>
                                <td align="left" class="Body_label" colspan="3">
                                    <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" size="12" ClientIDMode="Static"></asp:TextBox>
                                    <input id="Button14" type="button" value="..." onclick="OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', '','Y');" />
                                    <asp:TextBox ID="txt_DEPT_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_EMP_ID%>"></asp:Label>:
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" size="12" ClientIDMode="Static"></asp:TextBox>
                                    <input id="bt_HEAD_EMP_ID" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N','');" />
                                </td>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_LEVEL_CD" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_LEVEL_CD%>"></asp:Label>:
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:DropDownList ID="ddl_LEVEL_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                </td>

                            </tr>
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_EMP_NAME%>"></asp:Label>:
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="20" size="20" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_LEVEL_CD_NEW" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_LEVEL_CD_NEW%>"></asp:Label>:
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:DropDownList ID="ddl_LEVEL_CD_NEW" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                </td>

                            </tr>
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_EMP_CHG_CD" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_EMP_CHG_CD%>"></asp:Label>:
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:DropDownList ID="ddl_EMP_CHG_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                </td>

                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_WS_CD" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_WS_CD%>"></asp:Label>:
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:DropDownList ID="ddl_WS_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                </td>
                            </tr>



                            <tr>
                                <td colspan="4" align="right" class="Body_label">
                                    <div id="init">
                                        <aces:Btn ID="WFB2SM2101Search" runat="server" Text="<%$Resources:Resource,wfb2sm_WFB2SM210Search%>" OnClick="WFB2SM210Search_Click" />
                                        <%--<asp:Button ID="WFB2SM2101Search" runat="server" Text="<%$Resources:Resource,wfb2sm_WFB2SM210Search%>" OnClick="WFB2SM210Search_Click" />--%>
                                        <asp:Button ID="btn_clear" runat="server" type="button" Text="<%$Resources:Resource,wfb2sm_btn_clear%>" OnClientClick="return ClearAll();" />
                                         <asp:Button ID="btn_back" runat="server" Text="<%$Resources:Resource,btn_back%>" OnClick="btn_back_Click" />
                                    </div>
                                </td>
                                <tr>
                                    <td align="center" height="1" colspan="4">
                                        <hr>
                                    </td>
                                </tr>
                                <%--新增刪除修改確認取消Button--%>
                                <tr>
                                    <td align="right" colspan="10">
                                       <%-- <asp:Button ID="WFB2SM2101Add" runat="server" Text="<%$resources:resource,wfb2sm_WFB2SM210Add%>" OnClick="WFB2SM210Add_Click" />
                                        <asp:Button ID="WFB2SM2101Delete" runat="server" Text="<%$resources:resource,wfb2sm_WFB2SM210Delete%>" OnClick="WFB2SM210Delete_Click" OnClientClick="return CheckDelAction();" Visible="false" />
                                        <asp:Button ID="WFB2SM2101Edit" runat="server" Text="<%$resources:resource,wfb2sm_WFB2SM210Edit%>" OnClick="WFB2SM210Edit_Click" OnClientClick="return CheckModeifyAction();" Visible="false" />

                                        <asp:Button ID="WFB2SM2101OK" runat="server" Text="<%$resources:resource,wfb2sm_WFB2SM210OK%>" Visible="false" OnClick="WFB2SM210OK_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />--%>
                                       
                                        <aces:Btn ID="WFB2SM2101Add" runat="server" Text="<%$resources:resource,wfb2sm_WFB2SM210Add%>" OnClick="WFB2SM210Add_Click" />
                                        <aces:Btn ID="WFB2SM2101Delete" runat="server" Text="<%$resources:resource,wfb2sm_WFB2SM210Delete%>" OnClick="WFB2SM210Delete_Click" OnClientClick="return CheckDelAction();" Visible="false" />
                                        <aces:Btn ID="WFB2SM2101Edit" runat="server" Text="<%$resources:resource,wfb2sm_WFB2SM210Edit%>" OnClick="WFB2SM210Edit_Click" OnClientClick="return CheckModeifyAction();" Visible="false" />

                                        <aces:Btn ID="WFB2SM2101OK" runat="server" Text="<%$resources:resource,wfb2sm_WFB2SM210OK%>" Visible="false" OnClick="WFB2SM210OK_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
                                     <asp:Button ID="WFB2SM2100Cancel" runat="server" Text="<%$resources:resource,wfb2sm_WFB2SM210Cancel%>" OnClientClick="return confirm($('#hidwfb2sc_Cancel_ConfirmMessage').val());" Visible="false" OnClick="WFB2SM210Cancel_Click" />
                                    </td>
                                </tr>
                </tbody>

            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getDtlData"
                SelectCountMethod="getDtlCount" TypeName="CFB2SM2100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_DEPT_NO" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="dept_no" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="txt_EMP_ID" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="emp_id" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="ddl_LEVEL_CD" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="level_cd" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="txt_EMP_NAME" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="emp_name" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="ddl_LEVEL_CD_NEW" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="level_cd_new" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="ddl_EMP_CHG_CD" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="emp_chg_cd" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="ddl_WS_CD" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="ws_cd" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="txt_DATA_YEAR" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="data_year" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="txt_DATA_SEQ" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="data_seq" PropertyName="Text" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <%--cb_check--%>
                    <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-Width="30px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" meta:resourcekey="cb_checkallResource1" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" meta:resourcekey="cb_checkResource1" ClientIDMode="AutoID" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" Checked="true" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--NO--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_NO%>" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_NO" runat="server" Text='<%#Bind("NO")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_NO" runat="server" Text='<%#Bind("NO")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_NO" runat="server" Text='<%#Bind("NO")%>'></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--異常 EXCEPTION_STATUS 1--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_lb_EXCEPTION_STATUS%>" SortExpression="EXCEPTION_STATUS" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_EXCEPTION_STATUS" runat="server" Text='<%#Bind("EXCEPTION_STATUS")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--異動狀態--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_lb_CHG_STATUS%>" SortExpression="CHG_STATUS" HeaderStyle-Width="70px" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_CHG_STATUS" runat="server" Text='<%#Bind("CHG_STATUS_DESC")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--部門 DEPT_NO 7--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_lb_DEPT_NO%>" SortExpression="DEPT_NO" HeaderStyle-Width="250px" ItemStyle-Width="250px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_DEPT_NO" runat="server" Text='<%#Bind("DEPT_NO")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_NEW_DEPT_NO" runat="server" Width="100%" Style="text-align: left; word-wrap: break-word;" Text='<%#Bind("DEPT_NO")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_NEW_DEPT_NO" runat="server" Width="100%" Style="text-align: left;"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--工號 EMP_ID 5--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_EDIT_EMP_ID" runat="server" Width="100%" Style="text-align: left;" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" AutoPostBack="true" CssClass="MandatoryField" OnTextChanged="txt_NEW_EMP_ID_TextChanged"></asp:TextBox>
                            <input id="bt_NEW_EMP_ID" type="button" value="..." onclick="OpenEmpSearch('txt_NEW_EMP_ID', '', 'N', 'Y');" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sm_Required_EMP_ID%> "
                                ControlToValidate="txt_NEW_EMP_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--姓名 EMP_NAME 30--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_NEW_EMP_NAME" runat="server" Width="100%" Style="text-align: left;" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_NEW_EMP_NAME" runat="server" Width="100%" Style="text-align: left;"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--職種 WS_CD 1--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_lb_WS_CD%>" SortExpression="WS_CD" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_WS_CD" runat="server" Text='<%#Bind("WS_CD")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_NEW_WS_CD" runat="server" Width="100%" Style="text-align: center;" Text='<%#Bind("WS_CD")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_NEW_WS_CD" runat="server" Width="100%" Style="text-align: center;"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--年資 WORK_YEARS 5--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_lb_WORK_YEARS%>" SortExpression="WORK_YEARS" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_WORK_YEARS" runat="server" Text='<%#Bind("WORK_YEARS")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_NEW_WORK_YEARS" runat="server" Width="100%" Style="text-align: right;" Text='<%#Bind("WORK_YEARS")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_NEW_WORK_YEARS" runat="server" Width="100%" Style="text-align: right;"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--原資格 LEVEL_CD 3--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_lb_LEVEL_CD%>" SortExpression="LEVEL_CD" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEVEL_CD" runat="server" Text='<%#Bind("LEVEL_CD")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_NEW_LEVEL_CD" runat="server" Width="100%" Style="text-align: center;" Text='<%#Bind("LEVEL_CD")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_NEW_LEVEL_CD" runat="server" Width="100%" Style="text-align: center;"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--原級數 GRADE_CD 1--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_lb_GRADE_CD%>" SortExpression="GRADE_CD" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_GRADE_CD" runat="server" Text='<%#Bind("GRADE_CD")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_NEW_GRADE_CD" runat="server" Width="100%" Style="text-align: center;" Text='<%#Bind("GRADE_CD")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_NEW_GRADE_CD" runat="server" Width="100%" Style="text-align: center;"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--原職務  PJOB_CD--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_lb_PJOB_CD%>" SortExpression="PJOB_CD" HeaderStyle-Width="150px" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_PJOB_CD" runat="server" Text='<%#Bind("PJOB_CD")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_NEW_PJOB_CD" runat="server" Width="100%" Style="text-align: left;" Text='<%#Bind("PJOB_CD")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_NEW_PJOB_CD" runat="server" Width="100%" Style="text-align: left;"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--資格年資 LEVEL_WORK_YEARS--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_lb_LEVEL_WORK_YEARS%>" SortExpression="LEVEL_WORK_YEARS" HeaderStyle-Width="70px" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEVEL_WORK_YEARS" runat="server" Text='<%#Bind("LEVEL_WORK_YEARS")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_NEW_LEVEL_WORK_YEARS" runat="server" Width="100%" Style="text-align: right;" Text='<%#Bind("LEVEL_WORK_YEARS")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_NEW_LEVEL_WORK_YEARS" runat="server" Width="100%" Style="text-align: right;"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--晉昇資格 LEVEL_CD_NEW--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_lb_LEVEL_CD_NEW%>" SortExpression="LEVEL_CD_NEW" HeaderStyle-Width="70px" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEVEL_CD_NEW" runat="server" Text='<%#Bind("LEVEL_CD_NEW")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_NEW_LEVEL_CD_NEW" runat="server" Text='5A' Style="text-align: center;"></asp:Label>
                            <asp:HiddenField ID="hid_EDIT_LEVEL_CD_NEW" runat="server" Value='<%#Bind("LEVEL_CD_NEW")%>' />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_NEW_LEVEL_CD_NEW" runat="server" Text='5A' Style="text-align: center;"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--晉昇級數 GRADE_CD_NEW--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_lb_GRADE_CD_NEW%>" SortExpression="GRADE_CD_NEW" HeaderStyle-Width="70px" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_GRADE_CD_NEW" runat="server" Text='<%#Bind("GRADE_CD_NEW")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_NEW_GRADE_CD_NEW" runat="server" CssClass="MandatoryField">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sm_Required_GRADE_CD_NEW%>"
                                ControlToValidate="ddl_NEW_GRADE_CD_NEW" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_NEW_GRADE_CD_NEW" runat="server" CssClass="MandatoryField">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sm_Required_GRADE_CD_NEW%>"
                                ControlToValidate="ddl_NEW_GRADE_CD_NEW" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--晉昇職務  PJOB_CD_NEW--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_lb_PJOB_CD_NEW%>" SortExpression="PJOB_CD_NEW" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                        <ItemTemplate>
                            <asp:Label ID="lb_PJOB_CD_NEW" runat="server" Width="100%" Style="text-align: left;" Text='<%#Bind("PJOB_CD_NEW_DESC")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_NEW_PJOB_CD_NEW" runat="server" CssClass="MandatoryField"></asp:DropDownList>
                            <asp:HiddenField ID="hid_EDIT_PJOB_CD_NEW" runat="server" Value='<%#Bind("PJOB_CD_NEW")%>' />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sm_Required_PJOB_CD_NEW%>"
                                ControlToValidate="ddl_NEW_PJOB_CD_NEW" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_NEW_PJOB_CD_NEW" runat="server" CssClass="MandatoryField"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sm_Required_PJOB_CD_NEW%>"
                                ControlToValidate="ddl_NEW_PJOB_CD_NEW" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--在職區分 EMP_CHG_CD--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_lb_EMP_CHG_CD%>" SortExpression="EMP_CHG_CD" HeaderStyle-Width="70px" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_CHG_CD" runat="server" Text='<%#Bind("EMP_CHG_CD")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_NEW_EMP_CHG_CD" runat="server" Width="100%" Style="text-align: left;" Text='<%#Bind("EMP_CHG_CD")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_NEW_EMP_CHG_CD" runat="server" Width="100%" Style="text-align: left;"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--前一能力考核 ASSESS_SCORE_1--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_lb_ASSESS_SCORE_1%>" SortExpression="ASSESS_SCORE_1" HeaderStyle-Width="110px" ItemStyle-Width="110px">
                        <ItemTemplate>
                            <asp:Label ID="lb_ASSESS_SCORE_1" runat="server" Text='<%#Bind("ASSESS_SCORE_1")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_NEW_ASSESS_SCORE_1" runat="server" Width="100%" Style="text-align: center;" Text='<%#Bind("ASSESS_SCORE_1")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_NEW_ASSESS_SCORE_1" runat="server" Width="100%" Style="text-align: center;"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--前能力考核 ASSESS_SCORE_2--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_lb_ASSESS_SCORE_2%>" SortExpression="ASSESS_SCORE_2" HeaderStyle-Width="110px" ItemStyle-Width="110px">
                        <ItemTemplate>
                            <asp:Label ID="lb_ASSESS_SCORE_2" runat="server" Text='<%#Bind("ASSESS_SCORE_2")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_NEW_ASSESS_SCORE_2" runat="server" Width="100%" Style="text-align: center;" Text='<%#Bind("ASSESS_SCORE_2")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_NEW_ASSESS_SCORE_2" runat="server" Width="100%" Style="text-align: center;"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--生效狀態 EXECUTIVE_STATUS--%>
                    <asp:TemplateField HeaderText="hid" SortExpression="EXECUTIVE_STATUS" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lb_EXECUTIVE_STATUS" runat="server" Text='<%#Bind("EXECUTIVE_STATUS")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_EXECUTIVE_STATUS" runat="server" Text='<%#Bind("EXECUTIVE_STATUS")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--資料年度 DATA_YEAR--%>
                    <asp:TemplateField HeaderText="hid" SortExpression="DATA_YEAR" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lb_DATA_YEAR" runat="server" Text='<%#Bind("DATA_YEAR")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_EDIT_DATA_YEAR" runat="server" Text='<%#Bind("DATA_YEAR")%>'></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <div style="width: 1020px; overflow-x: auto; height:140px" >
                        <table class="grid-view" width="1640px" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                            <tr class="header">
                                <td width="30px"></td>
                                <td width="40px">
                                    <asp:Label ID="Label1" runat="server" Width="100%" Text="<%$Resources:Resource,wfb2sm_NO%>"></asp:Label>
                                </td>
                                <td width="60px">
                                    <asp:Label ID="Label2" runat="server" Width="100%" Text="<%$Resources:Resource,wfb2sm_lb_EXCEPTION_STATUS%>"></asp:Label>
                                </td>
                                <td width="70px">
                                    <asp:Label ID="Label18" runat="server" Width="100%" Text="<%$Resources:Resource,wfb2sm_lb_CHG_STATUS%>"></asp:Label>
                                </td>
                                <td width="250px">
                                    <asp:Label ID="Label3" runat="server" Width="100%" Text="<%$Resources:Resource,wfb2sm_lb_DEPT_NO%>"></asp:Label>
                                </td>
                                <td width="60px">
                                    <asp:Label ID="Label4" runat="server" Width="100%" Text="<%$Resources:Resource,wfb2sm_lb_EMP_ID%>"></asp:Label>
                                </td>
                                <td width="80px">
                                    <asp:Label ID="Label5" runat="server" Width="100%" Text="<%$Resources:Resource,wfb2sm_lb_EMP_NAME%>"></asp:Label>
                                </td>
                                <td width="50px">
                                    <asp:Label ID="Label6" runat="server" Width="100%" Text="<%$Resources:Resource,wfb2sm_lb_WS_CD%>"></asp:Label>
                                </td>
                                <td width="50px">
                                    <asp:Label ID="Label7" runat="server" Width="100%" Text="<%$Resources:Resource,wfb2sm_lb_WORK_YEARS%>"></asp:Label>
                                </td>
                                <td width="60px">
                                    <asp:Label ID="Label8" runat="server" Width="100%" Text="<%$Resources:Resource,wfb2sm_lb_LEVEL_CD%>"></asp:Label>
                                </td>
                                <td width="60px">
                                    <asp:Label ID="Label9" runat="server" Width="100%" Text="<%$Resources:Resource,wfb2sm_lb_GRADE_CD%>"></asp:Label>
                                </td>
                                <td width="100px">
                                    <asp:Label ID="Label10" runat="server" Width="100%" Text="<%$Resources:Resource,wfb2sm_lb_PJOB_CD%>"></asp:Label>
                                </td>
                                <td width="70px">
                                    <asp:Label ID="Label11" runat="server" Width="100%" Text="<%$Resources:Resource,wfb2sm_lb_LEVEL_WORK_YEARS%>"></asp:Label>
                                </td>
                                <td width="70px">
                                    <asp:Label ID="Label12" runat="server" Width="100%" Text="<%$Resources:Resource,wfb2sm_lb_LEVEL_CD_NEW%>"></asp:Label>
                                </td>
                                <td width="70px">
                                    <asp:Label ID="Label13" runat="server" Width="100%" Text="<%$Resources:Resource,wfb2sm_lb_GRADE_CD_NEW%>"></asp:Label>
                                </td>
                                <td width="150px">
                                    <asp:Label ID="Label14" runat="server" Width="100%" Text="<%$Resources:Resource,wfb2sm_lb_PJOB_CD_NEW%>"></asp:Label>
                                </td>
                                <td width="70px">
                                    <asp:Label ID="Label15" runat="server" Width="100%" Text="<%$Resources:Resource,wfb2sm_lb_EMP_CHG_CD%>"></asp:Label>
                                </td>
                                <td width="110px">
                                    <asp:Label ID="Label16" runat="server" Width="100%" Text="<%$Resources:Resource,wfb2sm_lb_ASSESS_SCORE_1%>"></asp:Label>
                                </td>
                                <td width="110px">
                                    <asp:Label ID="Label17" runat="server" Width="100%" Text="<%$Resources:Resource,wfb2sm_lb_ASSESS_SCORE_2%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td>
                                    <asp:Label ID="lb_NEW_DEPT_NO" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_NEW_EMP_ID" runat="server" Width="64px" ClientIDMode="Static" AutoPostBack="true" CssClass="MandatoryField" MaxLength="5" OnTextChanged="txt_NEW_EMP_ID_TextChanged"></asp:TextBox>
                                    <input id="bt_NEW_EMP_ID" type="button" value="..." onclick="OpenEmpSearch('txt_NEW_EMP_ID', '', 'N', 'Y');" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sm_Required_EMP_ID%> "
                                        ControlToValidate="txt_NEW_EMP_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:Label ID="lb_NEW_EMP_NAME" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lb_NEW_WS_CD" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lb_NEW_WORK_YEARS" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lb_NEW_LEVEL_CD" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lb_NEW_GRADE_CD" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lb_NEW_PJOB_CD" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lb_NEW_LEVEL_WORK_YEARS" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lb_NEW_LEVEL_CD_NEW" runat="server" Text=''></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_NEW_GRADE_CD_NEW" runat="server" CssClass="MandatoryField" Width="60px" >
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sm_Required_GRADE_CD_NEW%>"
                                        ControlToValidate="ddl_NEW_GRADE_CD_NEW" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_NEW_PJOB_CD_NEW" runat="server" CssClass="MandatoryField"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="MandatoryField" ErrorMessage="<%$Resources:Resource,wfb2sm_Required_PJOB_CD_NEW%>"
                                        ControlToValidate="ddl_NEW_PJOB_CD_NEW" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:Label ID="lb_NEW_EMP_CHG_CD" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lb_NEW_ASSESS_SCORE_1" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lb_NEW_ASSESS_SCORE_2" runat="server"></asp:Label>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                        </table>
                    </div>

                </EmptyDataTemplate>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr valign="top">

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
            <asp:HiddenField ID="HID_PROCESS_STATUS" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_DEPT_NO" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_DEPT_NAME" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_LEVEL_CD" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_PJOB_CD" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_PJOB_DESC" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_EMP_CHG_CD" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_EMP_CHG_DESC" runat="server" ClientIDMode="Static" />


            <%--<asp:HiddenField ID="HID_PJOB_DESC_NEW" runat="server" ClientIDMode="Static" />--%>
            <%--在職天數算到年底--%>
            <asp:HiddenField ID="HID_WORK_DAY_TOEndDay" runat="server" ClientIDMode="Static" />
            <%--任現資格天數算到年底--%>
            <asp:HiddenField ID="HID_LEVEL_WORK_DAY_TOEndDay" runat="server" ClientIDMode="Static" />
            <%--提出核可日有無值--%>
            <asp:HiddenField ID="HID_IsClose" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb2sc_Del_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Mod_NotChoiceMessage" Value="<%$Resources:Resource,wfb2sc_Mod_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Save_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Save_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Del_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Cancel_ConfirmMessage%>" />
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
