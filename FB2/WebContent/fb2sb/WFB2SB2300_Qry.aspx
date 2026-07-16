<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sb/WFB2SB2300_Qry.aspx.cs" Inherits="WebContent_fb2sb_WFB2SB2300_Qry" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });

        function iniForm() {
            $('#txt_SALARY_NAME').attr("readonly", true);
            $("#txt_DATA_YM,#txt_YEAR_MONTH_Add").datepicker({ dateFormat: 'yy/mm' });
            $(".number").mask('9999/99');
            $(".number2").mask('9999/99/99');
            $(".empid").mask('99999')
            gridviewScroll();
            $.unblockUI();
            $('#txt_EMP_ID').change(function () {
                if ($('#txt_EMP_ID').val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_EMP_ID').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg == "1")
                                alert(JData.errMsg);
                            else {
                                $('#txt_EMP_NAME').val(JData.EMP_NAME);
                                $('#txt_LICENSE_ID').val(JData.LICENSE_ID);
                                $('#txt_BIRTH_DT').val(JData.BIRTH_DT);
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

            $('#txt_SALARY_ID').change(function () {
                if ($('#txt_SALARY_ID').val().length <= 4) {
                    //ajax 取得薪資
                    $.ajax({
                        url: "../commgeo/WFB2GetSalaryData.ashx",
                        data: {
                            SALARY_ID: $('#txt_SALARY_ID').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_SALARY_ID').val("");
                                $('#txt_SALARY_NAME').val("");
                            }
                            else {
                                $('#txt_SALARY_ID').val(JData.SALARY_ID);
                                $('#txt_SALARY_NAME').val(JData.SALARY_NAME);

                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
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
                , freezesize: 4
            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
        }

        function ClearAll() {
            $('#txt_DATA_YM').val("");
            $('#txt_SALARY_ID').val("");
            $('#txt_SALARY_NAME').val("");
            $('#ddl_PROCESS_STATUS').val(-1);
            $('#ddl_SALARY_STATUS').val(-1);
            $('#ddl_EMP_CD').val(-1);
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
            if (LookUpCheckboxs() == 1) {
                BlockUI();
                return true;
            }
            else {
                alert($('#hidwfb299_Mod_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckDtlAction() {
            var processed = true;
            if (LookUpCheckboxs() == 1) {
                processed = true;
            } else {
                alert($('#hidwfb299_Del_NotChoiceMessage').val());
                processed = false;
                return processed;
            }

            if (confirm($('#hidwfb299_Del_ConfirmMessage').val())) {
                processed = true;
                $.unblockUI();
            } else {
                processed = false;
            }

            if (!processed) {
                $.unblockUI();
            }
            return processed;
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

        function openSalaryID() {
            var salaryId = $("#txt_SALARY_ID").val();
            OpenSearch('SalaryItem_Search.aspx', 'txt_SALARY_ID', 'txt_SALARY_NAME', "SALARY_ID=" + salaryId + "&isPermissions=Y", '');
        }

        function CheckdeleteAction() {

            if (LookUpCheckboxs() > 0) {
                for (var i = 0; i < ItemCheckBoxs.length; i++) {
                    if (choietr[i] != null) {
                        var ck = choietr[i] - 2;
                        if (ck >= 0 && $("[id=hid_PROCESS_STATUS]")[ck].value != "Y") {
                            return CheckDtlAction();
                        }
                    }
                }
            }
            else {
                alert($('#hidwfb299_Del_NotChoiceMessage').val());

                return false;
            }
        }

        var choietr = [];
        var ItemCheckBoxs = null;
        //檢查勾了幾個checkbox
        function LookUpCheckboxs() {
            //choietr = null;
            ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                choietr[i] = null;
                if (ItemCheckBoxs[i].checked && ItemCheckBoxs[i].id.indexOf("_freezeitem") != -1 && ItemCheckBoxs[i].id.indexOf("_freezeheader") == -1) {
                    if (choietr[i] == null)
                        choietr[i] = i;
                    HaveCheck++;
                }
            }
            return HaveCheck;
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
                                <col width="12%" />
                                <col width="30%" />
                                <col width="12%" />
                                <col width="46%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DATA_YM" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_DATA_YM%>"></asp:Label>:

                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DATA_YM" runat="server" MaxLength="7" Width="64px" ClientIDMode="Static" CssClass="MandatoryField number"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_SALARY_YM2_isNull%>"
                                            ControlToValidate="txt_DATA_YM" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="">
                                        </asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="Regular_txt_DATA_YM" runat="server"
                                            ErrorMessage="資料年月日期格式錯誤" ControlToValidate="txt_DATA_YM" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="(19|20|99)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_SALARY_ID" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_SALARY_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_ID" runat="server" MaxLength="4" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                        <input id="Button8" type="button" value="..." onclick="openSalaryID();" />
                                        <asp:TextBox ID="txt_SALARY_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_PROCESS_STATUS" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_PROCESS_STATUS%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_PROCESS_STATUS" runat="server" ClientIDMode="Static">
                                        </asp:DropDownList>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_SALARY_STATUS" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_SALARY_STATUS%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_SALARY_STATUS" runat="server" ClientIDMode="Static">
                                        </asp:DropDownList>
                                    </td>

                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_EMP_CD" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_EMP_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:DropDownList ID="ddl_EMP_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>


                                </tr>
                                <tr>

                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2sc_EMP_NO%>"></asp:Label>
                                        : </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" ClientIDMode="Static" MaxLength="5" Width="50px" CssClass="empid"></asp:TextBox>
                                        <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', '', '');" />

                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2sc_EMP_NAME%>"></asp:Label>
                                        : </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" ClientIDMode="Static" MaxLength="5" Width="100px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>

                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SB2300Search" runat="server" Text="<%$Resources:Resource,btn_Search%>" OnClick="WFB2SB2300Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />
                                            <%--<asp:Button ID="WFB2SB2300Search" runat="server" Text="<%$Resources:Resource,btn_Search%>" OnClick="WFB2SB2300Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />--%>
                                            <input id="WFB2SB2300Clear" type="button" value="<%$Resources:Resource,btn_clear%>" onclick="ClearAll();" runat="server" />
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
                    <td>
                        <table style="text-align: left; width: 100%;">
                            <tr>

                                <td align="left" class="Body_label" style="width: 60%;">
                                    <div id="init_excel" style="display: inline; text-align: left; width: 60%;">
                                        Excel上傳檔案:<asp:FileUpload ID="FileUpload1" runat="server" Width="300" />
                                        <aces:Btn ID="WFB2SB2300Upload" runat="server" Text="<%$Resources:Resource,wfb2sb_WFB2SB2300Upload%>" OnClick="WFB2SB2300Upload_Click" />
                                        <aces:Btn ID="WFB2SB2300Download" runat="server" Text="<%$Resources:Resource,wfb2sb_WFB2SB2300Download%>" OnClick="WFB2SB2300Download_Click" />
                                        <%-- <asp:Button ID="WFB2SB2300Upload" runat="server" Text="<%$Resources:Resource,wfb2sb_WFB2SB2300Upload%>" OnClick="WFB2SB2300Upload_Click" />
                                        <asp:Button ID="WFB2SB2300Download" runat="server" Text="<%$Resources:Resource,wfb2sb_WFB2SB2300Download%>" OnClick="WFB2SB2300Download_Click" />--%>
                                    </div>

                                </td>
                                <td align="right" class="Body_label" style="width: 40%;">
                                    <div id="init_grid" style="text-align: right; width: 40%;">
                                        <aces:Btn ID="WFB2SB2300Add" runat="server" Text="<%$Resources:Resource,btn_Add%>" OnClick="WFB2SB2300Add_Click" />
                                        <aces:Btn ID="WFB2SB2300Delete" runat="server" Text="<%$Resources:Resource,btn_Delete%>" OnClick="WFB2SB2300Delete_Click" OnClientClick="return CheckdeleteAction();" Visible="false" />
                                        <aces:Btn ID="WFB2SB2300Edit" runat="server" Text="<%$Resources:Resource,btn_Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2SB2300Edit_Click" Visible="false" />
                                        <%--<asp:Button ID="WFB2SB2300Add" runat="server" Text="<%$Resources:Resource,btn_Add%>" OnClick="WFB2SB2300Add_Click" />
                                        <asp:Button ID="WFB2SB2300Delete" runat="server" Text="<%$Resources:Resource,btn_Delete%>" OnClick="WFB2SB2300Delete_Click" OnClientClick="return CheckdeleteAction();" Visible="false" />
                                        <asp:Button ID="WFB2SB2300Edit" runat="server" Text="<%$Resources:Resource,btn_Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2SB2300Edit_Click" Visible="false" />--%>
                                    </div>

                                </td>


                            </tr>

                        </table>
                    </td>
                </tr>
            </table>
            <!--2990400_QRY結束-->
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="Cfb2sb2300DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_DATA_YM"
                        Name="txt_DATA_YM" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="txt_EMP_ID" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_SALARY_ID"
                        Name="txt_SALARY_ID" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_PROCESS_STATUS"
                        Name="ddl_PROCESS_STATUS" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_SALARY_STATUS"
                        Name="ddl_SALARY_STATUS" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_EMP_NAME"
                        Name="txt_EMP_NAME" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_EMP_CD"
                        Name="ddl_EMP_CD" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />



                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="2140px"
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
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2_RowNumber%>" HeaderStyle-Width="40px">
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

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_DATA_YM%>" SortExpression="DATA_YM" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_DATA_YM" runat="server" Text='<%#Bind("DATA_YM")%>' CssClass="number"></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_EMP_ID%>" SortExpression="EMP_ID" ItemStyle-Width="60px">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                            </div>
                        </ItemTemplate>

                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_EMP_CD%>" SortExpression="DESC1" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="lbl_DESC1" runat="server" Text='<%# Convert.ToString(Eval("DESC1"))%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_SALARY_ID%>" SortExpression="SALARY_ID" HeaderStyle-Width="180px">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_SALARY_ID" runat="server" Text='<%#Bind("SALARY_NAME")%>'></asp:Label>
                                <asp:HiddenField ID="hid_SALARY_ID" runat="server" Value='<%#Bind("SALARY_ID")%>' />
                            </div>
                        </ItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_CHG_AMT_A%>" SortExpression="CHG_AMT_A" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:Label ID="lbl_CHG_AMT_A" runat="server" Text='<%#Convert.ToString(Eval("CHG_AMT_A","{0:N0}"))%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_PROCESS_STATUS%>" SortExpression="PROCESS_STATUS" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_PROCESS_STATUS" runat="server" Text='<%#Bind("DESC2")%>'></asp:Label>
                                <asp:HiddenField ID="hid_PROCESS_STATUS" runat="server" Value='<%#Bind("PROCESS_STATUS")%>' ClientIDMode="Static" />
                            </div>
                        </ItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_CHG_STATUS%>" SortExpression="CHG_STATUS" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_CHG_STATUS" runat="server" Text='<%#Bind("DESC3")%>'></asp:Label>
                                <asp:HiddenField ID="hid_CHG_STATUS" runat="server" Value='<%#Bind("CHG_STATUS")%>' ClientIDMode="Static" />
                            </div>
                        </ItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_SALARY_STATUS%>" SortExpression="SALARY_STATUS" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_SALARY_STATUS" runat="server" Text='<%#Bind("DESC4")%>'></asp:Label>
                                <asp:HiddenField ID="hid_SALARY_STATUS" runat="server" Value='<%#Bind("SALARY_STATUS")%>' />
                            </div>
                        </ItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_SALARY_DT%>" SortExpression="SALARY_DT" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_SALARY_DT" runat="server" Text='<%#Convert.ToString(Eval("SALARY_DT", "{0:yyyy/MM/dd}")) %>' CssClass="number2"></asp:Label>
                            </div>
                        </ItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_CHG_AMT_B%>" SortExpression="CHG_AMT_B" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:Label ID="lbl_CHG_AMT_B" runat="server" Text='<%# Convert.ToString(Eval("CHG_AMT_B","{0:N0}"))%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_CREATED_BY%>" SortExpression="CREATED_BY" HeaderStyle-Width="140px">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_CREATED_BY" runat="server" Text='<%#Bind("CREATED_NAME")%>'></asp:Label>
                                <asp:HiddenField ID="hid_CREATED_BY" runat="server" Value='<%#Bind("CREATED_BY")%>' />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_APPROVE_BY%>" SortExpression="APPROVE_NAME" HeaderStyle-Width="140px">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_APPROVE_BY" runat="server" Text='<%#Bind("APPROVE_NAME")%>'></asp:Label>

                            </div>
                        </ItemTemplate>


                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_APPROVE_DT%>" SortExpression="APPROVE_DT" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_APPROVE_DT" runat="server" Text='<%#Bind("APPROVE_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>

                            </div>
                        </ItemTemplate>


                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_REMARK%>" SortExpression="REMARK" HeaderStyle-Width="300px">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_REMARK" runat="server" Text='<%#Bind("REMARK")%>'></asp:Label>

                            </div>
                        </ItemTemplate>


                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_APP_REMARK%>" SortExpression="APP_REMARK" ItemStyle-Width="300px">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_APP_REMARK" runat="server" Text='<%#Bind("APP_REMARK")%>'></asp:Label>
                                <asp:HiddenField ID="hid_SEQ_NO" runat="server" Value='<%#Bind("SEQ_NO")%>' />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
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

            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Del_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Mod_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Mod_NotChoiceMessage%> " />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Save_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Save_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Del_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Dtl_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Dtl_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Cancel_ConfirmMessage%>" />

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SB2300Upload"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="WFB2SB2300Download"></asp:PostBackTrigger>

        </Triggers>

    </asp:UpdatePanel>
</asp:Content>


