<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2ha/WFB2HA0600_PRIV3.aspx.cs" Inherits="WebContent_fb2ha_WFB2HA0600_PRIV3" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>

    <style type="text/css">
        #txt_HR_CHG_CD {
            text-transform: uppercase;
        }

        #txt_HR_CHG_CD_Add {
            text-transform: uppercase;
        }
    </style>

    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });


        function iniForm() {
            $("#txt_HR_CHG_DESC,#txt_EMP_NAME,#txt_HR_CHG_DESC_Add,#txt_EMP_NAME_Add").css("color", "black").css("background-color", "white").attr("disabled", true);
            $("#txt_YEAR_MONTH_Add").datepicker({ dateFormat: 'yymm' });
            $(".number").mask('9999/99');
            $(".empid").mask('99999');
            gridviewScroll();
            $.unblockUI();
            $('#txt_EMP_ID').change(function () {
                if ($('#txt_EMP_ID').val().length == 5) {
                    //ajax 取得員工基本資料
                    doEmpAjax();
                }
            });
            $('#txt_EMP_ID_Add').change(function () {
                if ($('#txt_EMP_ID_Add').val().length == 5) {
                    //ajax 取得員工基本資料
                    doEmpAjax2();
                }
            });
            $('#txt_HR_CHG_CD').change(function () {
                if ($('#txt_HR_CHG_CD').val().length == 3) {

                    var regex = new RegExp("^[a-zA-Z0-9]+$");
                    var str = $('#txt_HR_CHG_CD').val();
                    if (regex.test(str)) {
                        //ajax 取得薪資
                        $.ajax({
                            url: "../commgeo/WFB2ChangeCode_Search.ashx",
                            data: {
                                HR_CHG_CD: $('#txt_HR_CHG_CD').val()
                            },
                            type: "GET",
                            cache: false,
                            dataType: 'json',
                            success: function (JData) {
                                if (JData.errMsg != "") {
                                    $('#txt_HR_CHG_DESC').val("");
                                }
                                else {
                                    $('#txt_HR_CHG_CD').val(JData.HR_CHG_CD_A);
                                    $('#txt_HR_CHG_DESC').val(JData.HR_CHG_DESC);

                                }
                            },

                            error: function (xhr, ajaxOptions, thrownError) {
                                alert(xhr.status);
                                alert(thrownError);
                            }
                        });

                    } else {
                        alert($("#hidwfb2_alphanumeric_onlyMessage").val());
                        $('#txt_HR_CHG_CD').val("");
                        return false;
                    }
                }e


            });
            $('#txt_HR_CHG_CD_Add').change(function () {
                if ($('#txt_HR_CHG_CD_Add').val().length == 3) {

                    var regex = new RegExp("^[a-zA-Z0-9]+$");
                    var str = $('#txt_HR_CHG_CD_Add').val();
                    if (regex.test(str)) {
                        //ajax 取得薪資
                        $.ajax({
                            url: "../commgeo/WFB2ChangeCode_Search.ashx",
                            data: {
                                HR_CHG_CD: $('#txt_HR_CHG_CD_Add').val()
                            },
                            type: "GET",
                            cache: false,
                            dataType: 'json',
                            success: function (JData) {
                                if (JData.errMsg != "") {
                                    $('#txt_HR_CHG_CD_Add').val("");
                                    $('#txt_HR_CHG_DESC_Add').val("");
                                }
                                else {
                                    $('#txt_HR_CHG_CD_Add').val(JData.HR_CHG_CD_A);
                                    $('#txt_HR_CHG_DESC_Add').val(JData.HR_CHG_DESC);

                                }
                            },

                            error: function (xhr, ajaxOptions, thrownError) {
                                alert(xhr.status);
                                alert(thrownError);
                            }
                        });

                    } else {
                        alert($("#hidwfb2_alphanumeric_onlyMessage").val());
                        $('#txt_HR_CHG_CD_Add').val("");
                        return false;
                    }
                } else {
                    $('#txt_HR_CHG_DESC_Add').val("");
                }

            });
        }
        function doEmpAjax() {
            //ajax 取得員工基本資料
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
                        $('#txt_EMP_ID').val("");
                        $('#txt_EMP_NAME').val("");
                        $('#txt_EMP_CD').val("");
                        $('#txt_DEPT_DESC').val("");
                        $('#txt_LEAVE_DT').val("");
                        alert(JData.errMsg);
                    } else {
                        $('#txt_EMP_NAME').val(JData.EMP_NAME);
                        $('#txt_EMP_CD').val(JData.EMP_CD_DESC);
                        $('#txt_DEPT_DESC').val(JData.DEPT_NO + '-' + JData.DEPT_NAME);
                        $('#txt_LEAVE_DT').val(JData.LEAVE_DT);
                    }
                },

                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });
        }
        function doEmpAjax2() {
            //ajax 取得員工基本資料
            $.ajax({
                url: "../commgeo/WFB2GetEmpData.ashx",
                data: {
                    EMP_ID: $('#txt_EMP_ID_Add').val()
                },
                type: "GET",
                cache: false,
                dataType: 'json',
                success: function (JData) {
                    if (JData.errMsg != "") {
                        $('#txt_EMP_ID_Add').val("");
                        $('#txt_EMP_NAME_Add').val("");
                        $('#txt_EMP_CD_Add').val("");
                        $('#txt_DEPT_DESC_Add').val("");
                        $('#txt_LEAVE_DT_Add').val("");
                        alert(JData.errMsg);
                    } else {
                        $('#txt_EMP_NAME_Add').val(JData.EMP_NAME);
                        $('#txt_EMP_CD_Add').val(JData.EMP_CD_DESC);
                        $('#txt_DEPT_DESC_Add').val(JData.DEPT_NO + '-' + JData.DEPT_NAME);
                        $('#txt_LEAVE_DT_Add').val(JData.LEAVE_DT);
                    }
                },

                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
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
            });
        }
        function ClearAll() {
            $('#txt_HR_CHG_CD').val("");
            $('#txt_HR_CHG_DESC').val("");
            $('#txt_EMP_ID').val("");
            $('#txt_EMP_NAME').val("");
            $('#ddl_IS_VALID').val(-1);
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
                alert($('#hidWfb2ha_Mod_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckDtlAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidWfb2ha_Dtl_NotChoiceMessage').val());
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
                processed = false;
            if (!processed)
                $.unblockUI();

            return processed;
        }
        function CheckDtlAction() {
            var processed = true;
            if (LookUpCheckboxs() > 0) {
                processed = true;
            } else {
                alert($('#hidwfb2_Dtl_NotChoiceMessage').val());
                processed = false;
                return processed;
            }
            if (confirm($('#hidwfb2_Del_ConfirmMessage').val())) {
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

        function Check_ID_LENGTH(source, arguments) {
            if ($("#txt_EMP_ID_Add").val().length != 5)
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }

    </script>

    <style type="text/css">
        .auto-style1 {
            width: 15%;
        }

        #init {
            width: 617px;
        }
    </style>

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
                                <col width="20%" />
                                <col width="35%" />
                                <col width="10%" />
                                <col width="30%" />

                            </colgroup>
                            <tbody>
                                <caption>
                                    <tr>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lbl_HR_CHG_CD" runat="server" Text="">人事異動代碼</asp:Label>:
                                        </th>
                                        <td align="left" valign="middle" class="Body_label">
                                            <asp:TextBox ID="txt_HR_CHG_CD" runat="server" ClientIDMode="Static" MaxLength="3" Width="64px"></asp:TextBox>
                                            <input id="btn_HR_CHG_CD" type="button" value="..." onclick="OpenSearch('HrChangeCode_Search.aspx', 'txt_HR_CHG_CD', 'txt_HR_CHG_DESC', '');" />
                                            <asp:TextBox ID="txt_HR_CHG_DESC" runat="server" BorderWidth="0" ClientIDMode="Static" Width="120px"></asp:TextBox>
                                        </td>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="EMP_ID" runat="server" Text="">工號</asp:Label>
                                            : 
                                        </th>
                                        <td align="left" valign="middle" class="Body_label">
                                            <asp:TextBox ID="txt_EMP_ID" runat="server" ClientIDMode="Static" MaxLength="5" Width="64px" CssClass="empid"></asp:TextBox>
                                            <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME');" />
                                            <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>

                                        </td>
                                    </tr>
                                    <tr>
                                        <th class="auto-style1"></th>
                                        <th class="auto-style1"></th>
                                        <th class="auto-style1"></th>
                                        <td align="right" class="Body_label">
                                            <div id="init">
                                                <aces:Btn ID="WFB2HA0600Search" runat="server" OnClick="WFB2HA0600Search_Click" OnClientClick="BlockUI();" Text="<%$Resources:Resource,btn_Search%>" />

                                                <%--<asp:Button ID="WFB2HA0600Search" runat="server" OnClick="WFB2HA0600Search_Click" OnClientClick="BlockUI();" Text="<%$Resources:Resource,btn_Search%>" />--%>
                                                                                                
                                                <input id="WFB2HA0600Clear" type="button" value="<%$Resources:Resource,btn_clear%>" runat="server" onclick="ClearAll();" />
                                                <asp:Button ID="WFB2HA0600_BACK" runat="server" OnClick="WFB2HA0600_BACK_Click" Text="<%$Resources:Resource,btn_back%>" />
                                            </div>
                                        </td>
                                    </tr>

                                </caption>
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
                        <aces:Btn ID="WFB2HA0600Add" runat="server" Text="<%$Resources:Resource,btn_Add%>" OnClick="WFB2HA0600Add_Click" />
                            <aces:Btn ID="WFB2HA0600Delete" runat="server" Text="<%$Resources:Resource,btn_Delete%>" OnClientClick="return CheckDtlAction();" OnClick="WFB2HA0600Delete_Click" Visible="False" />
                            <aces:Btn ID="WFB2HA0600Save" runat="server" Text="<%$Resources:Resource,btn_Save%>" Visible="false" OnClick="WFB2HA0600Save_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />

<%--                            <asp:Button ID="WFB2HA0600Add" runat="server" Text="<%$Resources:Resource,btn_Add%>" OnClick="WFB2HA0600Add_Click" />
                            <asp:Button ID="WFB2HA0600Delete" runat="server" Text="<%$Resources:Resource,btn_Delete%>" OnClientClick="return CheckDtlAction();" OnClick="WFB2HA0600Delete_Click" Visible="False" />
                            <asp:Button ID="WFB2HA0600Save" runat="server" Text="<%$Resources:Resource,btn_Save%>" Visible="false" OnClick="WFB2HA0600Save_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />--%>
                            
                            <asp:Button ID="WFB2HA0600Cancel" runat="server" Text="<%$Resources:Resource,btn_Cancel%>" Visible="false" OnClick="WFB2HA0600Cancel_Click" OnClientClick="return confirm('是否確定取消?');" />
                        </div>

                    </td>
                </tr>
            </table>
            <!--2990400_QRY結束-->
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="Cfb2ha0600_PRIV3DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_HR_CHG_CD"
                        Name="HR_CHG_CD" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="EMP_ID" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />

                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1024px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" meta:resourcekey="cb_checkallResource1" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" meta:resourcekey="cb_checkResource1" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2ib_RowNumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <div>
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
                    <asp:TemplateField HeaderText="<%$Resources:Resource,Wfb2ha_HR_CHG_CD%>" SortExpression="HR_CHG_CD">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_HR_CHG_CD" runat="server" Text='<%#Bind("HR_CHG_CD")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:TextBox ID="txt_HR_CHG_CD_Add" runat="server" Text='<%#Bind("HR_CHG_CD")%>' CssClass="MandatoryField"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="請填寫人事異動代碼"
                                    ControlToValidate="txt_HR_CHG_CD_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>
                            </div>
                        </EditItemTemplate>

                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:TextBox ID="txt_HR_CHG_CD_Add" runat="server" ClientIDMode="Static" CssClass="MandatoryField" MaxLength="3"></asp:TextBox>
                                <input id="btn_HR_CHG_CD_Add" type="button" value="..." onclick="OpenSearch('HrChangeCode_Search.aspx', 'txt_HR_CHG_CD_Add', 'txt_HR_CHG_DESC_Add', '');" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="請填寫人事異動代碼"
                                    ControlToValidate="txt_HR_CHG_CD_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,Wfb2ha_HR_CHG_DESC%>" SortExpression="HR_CHG_DESC">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:Label ID="lbl_HR_CHG_DESC" runat="server" Text='<%#Bind("HR_CHG_DESC")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:TextBox ID="txt_HR_CHG_DESC_Add" runat="server" Text='<%#Bind("HR_CHG_DESC")%>' BorderWidth="0"></asp:TextBox>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_HR_CHG_DESC_Add" runat="server" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,Wfb2ha_EMP_ID%>" SortExpression="EMP_ID">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="lbl_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:TextBox ID="txt_EMP_ID_Add" runat="server" Text='<%#Bind("EMP_ID")%>' CssClass="MandatoryField  empid" MaxLength="5"></asp:TextBox>
                                <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID_Add', 'txt_EMP_NAME_Add');" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="請填寫工號"
                                    ControlToValidate="txt_EMP_ID_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2di_ERR_EMP_ID_LENGTH5%>" ClientValidationFunction="Check_ID_LENGTH" ForeColor="Red"
                                    ControlToValidate="txt_EMP_ID_Add" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
                            </div>
                        </EditItemTemplate>

                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:TextBox ID="txt_EMP_ID_Add" runat="server" ClientIDMode="Static" CssClass="MandatoryField" TextMode="Number" MaxLength="5"></asp:TextBox>
                                <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID_Add', 'txt_EMP_NAME_Add');" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="請填寫工號"
                                    ControlToValidate="txt_EMP_ID_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2di_ERR_EMP_ID_LENGTH5%>" ClientValidationFunction="Check_ID_LENGTH" ForeColor="Red"
                                    ControlToValidate="txt_EMP_ID_Add" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_EMP_NAME%>" SortExpression="EMP_NAME">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="lbl_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME_C")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:TextBox ID="txt_EMP_NAME_Add" runat="server" Text='<%#Bind("EMP_NAME_C")%>' BorderWidth="0"></asp:TextBox>
                            </div>
                        </EditItemTemplate>

                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:TextBox ID="txt_EMP_NAME_Add" runat="server" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
                <EmptyDataTemplate>
                    <table class="grid-view" width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                        <tr class="header">
                            <td width="250px">
                                <asp:Label ID="lblHeaderHR_CHG_CD" runat="server" Text="<%$Resources:Resource,Wfb2ha_HR_CHG_CD%>"></asp:Label>
                            </td>
                            <td width="250px"
                                <asp:Label ID="lblHeaderHR_CHG_DESC" runat="server" Text="<%$Resources:Resource,Wfb2ha_HR_CHG_DESC%>"></asp:Label>
                            </td>
                            <td width="250px">
                                <asp:Label ID="lblHeaderHR_EMP_ID" runat="server" Text="<%$Resources:Resource,Wfb2ha_EMP_ID%>"></asp:Label>
                            </td>
                            <td width="250px">
                                <asp:Label ID="lblHeaderHR_EMP_NAME" runat="server" Text="<%$Resources:Resource,Wfb2ha_EMP_NAME%>"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td>
                                <div style="text-align: center; width: 100%;">
                                    <asp:TextBox ID="txt_HR_CHG_CD_Add" ClientIDMode="Static" runat="server" CssClass="MandatoryField" MaxLength="3"></asp:TextBox>
                                    <input id="btn_HR_CHG_CD" type="button" value="..." onclick="OpenSearch('HrChangeCode_Search.aspx', 'txt_HR_CHG_CD', 'txt_HR_CHG_DESC', 'HR_CHG_CD=B&UPD_RIGHT_CD=D&IS_FOR_BATCH=Y&IS_FOR_TRANSFER_IN=3&IS_VALID=Y&EMP_ID=10003');" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="請填寫人事異動代碼"
                                        ControlToValidate="txt_HR_CHG_CD_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:TextBox ID="txt_HR_CHG_DESC_Add" runat="server" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:TextBox ID="txt_EMP_ID_Add" ClientIDMode="Static" runat="server" CssClass="MandatoryField"></asp:TextBox>
                                    <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID_Add', 'txt_EMP_NAME_Add');" />

                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="請填寫工號"
                                        ControlToValidate="txt_EMP_ID_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                    </asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2di_ERR_EMP_ID_LENGTH5%>" ClientValidationFunction="Check_ID_LENGTH" ForeColor="Red"
                                    ControlToValidate="txt_EMP_ID_Add" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:TextBox ID="txt_EMP_NAME_Add" runat="server" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
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
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2_alphanumeric_onlyMessage" Value="<%$Resources:Resource,wfb2_alphanumeric_onlyMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb2_Dtl_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2_Dtl_NotChoiceMessage" Value="<%$Resources:Resource,wfb2_Dtl_NotChoiceMessage%>" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


