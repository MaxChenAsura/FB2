<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sb/WFB2SB1100_Qry.aspx.cs" Inherits="WebContent_fb2sb_WFB2SB1100_Qry" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });

        function iniForm() {
            $("#txt_YEAR_MONTH_Add").datepicker({ dateFormat: 'yymm' });

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

            //grid新增
            $('#txt_EMP_ID_Add').change(function () {
                if ($('#txt_EMP_ID_Add').val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_EMP_ID_Add').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_EMP_ID_Add').val("");
                                $('#txt_EMP_NAME_Add').text("");
                                $('#txt_EMP_CHG_CD_DESC_Add').text("");

                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_EMP_NAME_Add').text(JData.EMP_NAME);
                                $('#txt_EMP_CHG_CD_DESC_Add').text(JData.EMP_STATUS + '-' + JData.EMP_STATUS_DESC);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_EMP_ID_Add').val("");
                    $('#txt_EMP_NAME_Add').text("");
                    $('#txt_EMP_CHG_CD_DESC_Add').text("");
                }
            });
        }
        function ShowRecord(obj) {
            alert("ShowRecord");
            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"
            });
        }
        function ClearAll() {
            $('#ddl_SUB_CD').val(-1);
            $('#txt_EMP_ID').val("");
            $('#txt_EMP_NAME').val("");


        }

        function CheckMODE_ID(source, arguments) {
            var re = /^[\d|a-zA-Z]+$/;
            if (!re.test($("#txt_MODE_ID_Add").val()))
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }

        function CheckModeifyAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidWfb2sb_Mod_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckDtlAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidWfb2sb_Dtl_NotChoiceMessage').val());
                return false;
            }
        }
        //儲存前檢查
        function saveCheck() {
            var processed = true;


            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                return;
            if (!processed)
                $.unblockUI();

            return processed;
        }

        function CheckSaveAction() {
            if ($('#txtCALENDAR_CD_Edit').val() != undefined) {
                if ($('#txtCALENDAR_CD_Edit').val().trim() == "") {
                    alert($('#hidWfb2sb_txtCALENDAR_CD_NotNull').val());
                    return false;
                }
                else
                    return confirm($('#hidWfb2sb_Save_ConfirmMessage').val());
            }
            else
                return confirm($('#hidWfb2sb_Save_ConfirmMessage').val());
        }
        function OpenEmpSearchSB110(emp_id) {
            OpenEmpSearch('txt_EMP_ID_Add', 'txt_EMP_NAME_Add', 'N', 'Y');
            //if (json != undefined) {
            //    $('#txt_EMP_NAME_Add').text(json.EMP_NAME);
            //    $('#txt_EMP_CHG_CD_DESC_Add').text(json.EMP_STATUS + '-' + json.EMP_STATUS_DESC);
            //}
        }
        function returnEMPValueToPage(emp_id, emp_name, value) {
            var returnValue = value;
            if (value == undefined) {
                returnValue = 'undefined';
            }
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                $('#txt_EMP_ID_Add').val(obj.EMP_ID);
                $('#txt_EMP_NAME_Add').text(obj.EMP_NAME);
                $('#txt_EMP_CHG_CD_DESC_Add').text(obj.EMP_STATUS + '-' + obj.EMP_STATUS_DESC);
                return obj;
            }
        }
        function CheckDelAction() {
            if (LookUpCheckboxs() > 0)
                return confirm($("#hidwfb2_Del_ConfirmMessage").val());
            else {
                alert($("#hidwfb2_Del_NotChoiceMessage").val());
                return false;
            }
        }

        function LookUpCheckboxs() {
            var ItemCheckBoxs = $(":checkbox[id$=cb_check]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }
        function CancelConfirm() {
            return confirm($("#hidfb2CancelConfirm").val());
        }

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!--2990400_QRY開始-->
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="13%" />
                                <col width="30%" />
                                <col width="13%" />
                                <col width="44%" />
                            </colgroup>
                            <tbody>
                                <th></th>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,WFB2SB_SUB_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_SUB_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>

                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2dg_lb_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="50px" ClientIDMode="Static"></asp:TextBox>
                                        <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', '', '');" />

                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_EMP_NAME2%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="10" Width="100px" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SB1100Search" runat="server" Text="<%$Resources:Resource,btn_Search%>" OnClick="WFB2SB1100Search_Click" OnClientClick="BlockUI();" />
                                            <%--<asp:Button ID="WFB2SB1100Search" runat="server" Text="<%$Resources:Resource,btn_Search%>" OnClick="WFB2SB1100Search_Click" OnClientClick="BlockUI();" />--%>
                                            <asp:Button ID="WFB2SB1100Clear" runat="server" type="button" Text="<%$Resources:Resource,btn_Clear%>" OnClientClick="ClearAll();" />
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

                            <aces:Btn ID="WFB2SB1100Add" runat="server" Text="<%$Resources:Resource,btn_Add%>" OnClick="WFB2SB1100Add_Click" />
                            <aces:Btn ID="WFB2SB1100Delete" runat="server" Text="<%$Resources:Resource,btn_Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2SB1100Delete_Click" Visible="False" />
                            <aces:Btn ID="WFB2SB1100Detail" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0100Dtl%>" Visible="false" OnClick="WFB2SB1100Detail_Click" />
                            <aces:Btn ID="WFB2SB1100Save" runat="server" Text="<%$Resources:Resource,btn_confirm%>" Visible="false" OnClick="WFB2SB1100Save_Click" OnClientClick="return saveCheck();" ValidationGroup="GroupA" />

                            <%-- <asp:Button ID="WFB2SB1100Add" runat="server" Text="<%$Resources:Resource,btn_Add%>" OnClick="WFB2SB1100Add_Click" />
                            <asp:Button ID="WFB2SB1100Delete" runat="server" Text="<%$Resources:Resource,btn_Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2SB1100Delete_Click" Visible="False" />
                            <asp:Button ID="WFB2SB1100Detail" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0100Dtl%>" Visible="false" OnClick="WFB2SB1100Detail_Click" />
                             <asp:Button ID="WFB2SB1100Save" runat="server" Text="<%$Resources:Resource,btn_confirm%>" Visible="false" OnClick="WFB2SB1100Save_Click" OnClientClick="return saveCheck();" ValidationGroup="GroupA" />--%>
                            <asp:Button ID="WFB2SB1100Cancel" runat="server" Text="<%$Resources:Resource,btn_Cancel%>" Visible="false" OnClick="WFB2SB1100Cancel_Click" OnClientClick="return CancelConfirm();" />
                        </div>

                    </td>
                </tr>
            </table>
            <!--2990400_QRY結束-->
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SB1100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <%-- StartRowIndexParameterName="startRowIndex" --%>
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="ddl_SUB_CD"
                        Name="data_type" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_EMP_NAME"
                        Name="emp_name" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />

                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="30px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" meta:resourcekey="cb_checkallResource1" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" meta:resourcekey="cb_checkResource1" ClientIDMode="AutoID" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:CheckBox ID="cb_check" runat="server" Checked="true" />
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2ib_RowNumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,WFB2SB_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>' CssClass="number"></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="lbl_EMP_ID_Add" runat="server" Text='<%#Bind("EMP_ID")%>' CssClass="number"></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:TextBox ID="txt_EMP_ID_Add" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearchSB110('txt_EMP_ID_Add');" />
                                <%--onclick="OpenEmpSearch('txt_EMP_ID_Add', 'txt_EMP_NAME_Add', 'txt_EMP_CHG_CD_DESC_Add');" --%>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_EMP_ID_isNull%>"
                                    ControlToValidate="txt_EMP_ID_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,WFB2SB_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="txt_EMP_NAME_Add" MaxLength="150" ClientIDMode="Static" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                                <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="請輸入姓名"
                                    ControlToValidate="txt_EMP_NAME_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>--%>
                            </div>
                        </EditItemTemplate>

                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="txt_EMP_NAME_Add" runat="server" MaxLength="10" Width="64px" ClientIDMode="Static"></asp:Label>
                                <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="請輸入姓名"
                                    ControlToValidate="txt_EMP_NAME_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>--%>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,WFB2SB_EMP_CHG_CD_DESC%>" SortExpression="EMP_STATUS" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="EMP_CHG_CD_DESC" runat="server" Text='<%# Convert.ToString(Eval("EMP_STATUS"))%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="txt_EMP_CHG_CD_DESC_Add" MaxLength="150" ClientIDMode="Static" runat="server" Text='<%#Bind("EMP_STATUS")%>'></asp:Label>
                                <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="請輸入員工狀態"
                                    ControlToValidate="txt_EMP_CHG_CD_DESC_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>--%>
                            </div>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="txt_EMP_CHG_CD_DESC_Add" MaxLength="150" ClientIDMode="Static" runat="server" Text='<%#Bind("EMP_STATUS")%>'></asp:Label>
                                <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="請輸入員工狀態"
                                    ControlToValidate="txt_EMP_CHG_CD_DESC_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>--%>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,WFB2SB_SUB_CD%>" SortExpression="SUB_CD" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="lb_SUB_CD" runat="server" Text='<%# Convert.ToString(Eval("SUB_CD")) +"-"+ Convert.ToString(Eval("SUB_DESC"))%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:DropDownList ID="ddl_SUB_CD_Add" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sb_ddl_SUB_CD_isNull%>"
                                    ControlToValidate="ddl_SUB_CD_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                </asp:RequiredFieldValidator>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:DropDownList ID="ddl_SUB_CD_Add" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sb_ddl_SUB_CD_isNull%>"
                                    ControlToValidate="ddl_SUB_CD_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                </asp:RequiredFieldValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
                <EmptyDataTemplate>
                    <table class="grid-view" width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                        <tr class="header">
                            <td width="30px"></td>
                            <td width="40px">
                                <asp:Label ID="lblHeaderRowNumber" runat="server" Text="<%$Resources:Resource,wfb299_RowNumber%>"></asp:Label>
                            </td>
                            <td width="150px">
                                <asp:Label ID="lblHeaderlbl_EMP_ID" runat="server" Text="<%$Resources:Resource,WFB2SB_EMP_ID%>"></asp:Label>
                            </td>
                            <td width="200px">
                                <asp:Label ID="lblHeaderEMP_NAME" runat="server" Text="<%$Resources:Resource,WFB2SB_EMP_NAME%>"></asp:Label>
                            </td>
                            <td width="200px">
                                <asp:Label ID="lblHeaderEMP_CHG_CD_DESC" runat="server" Text="<%$Resources:Resource,WFB2SB_EMP_CHG_CD_DESC%>"></asp:Label>
                            </td>
                            <td width="200px">
                                <asp:Label ID="lblHeaderlb_SUB_CD" runat="server" Text="<%$Resources:Resource,WFB2SB_SUB_CD%>"></asp:Label>
                            </td>

                        </tr>
                        <tr class="normal">
                            <td></td>
                            <td></td>
                            <td>
                                <div style="text-align: center; width: 100%;">
                                    <asp:TextBox ID="txt_EMP_ID_Add" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                    <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearchSB110('txt_EMP_ID_Add');" />
                                    <%-- onclick="OpenEmpSearch('txt_EMP_ID_Add', 'txt_EMP_NAME_Add', 'txt_EMP_CHG_CD_DESC_Add');"--%>

                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_EMP_ID_isNull%>"
                                        ControlToValidate="txt_EMP_ID_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:Label ID="txt_EMP_NAME_Add" MaxLength="150" ClientIDMode="Static" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                                    <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="請輸入姓名"
                                        ControlToValidate="txt_EMP_NAME_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                    </asp:RequiredFieldValidator>--%>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:Label ID="txt_EMP_CHG_CD_DESC_Add" MaxLength="150" ClientIDMode="Static" runat="server" Text='<%#Bind("EMP_CHG_DESC")%>'></asp:Label>
                                    <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="請輸入員工狀態"
                                        ControlToValidate="txt_EMP_CHG_CD_DESC_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                    </asp:RequiredFieldValidator>--%>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:DropDownList ID="ddl_SUB_CD_Add" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sb_ddl_SUB_CD_isNull%>"
                                        ControlToValidate="ddl_SUB_CD_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </td>

                            </div>
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
            </asp:GridView>
            <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">
                    <td class="GridviewScrollPager TD">
                        <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static">
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
            <!-- 是否為supervisor  -->
            <asp:HiddenField ID="HID_IS_SUPERVISOR" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_IS_ADD" runat="server" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_NotChoiceMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Mod_NotChoiceMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Save_ConfirmMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb2_Dtl_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2_Dtl_NotChoiceMessage" Value="<%$Resources:Resource,wfb2_Dtl_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb2_Dtl_NotChoiceMessage1%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidfb2CancelConfirm" Value="<%$Resources:Resource,wfb2CancelConfirm%>" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


