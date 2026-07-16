<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2ib/WFB2IB0200_Qry.aspx.cs" Inherits="WebContent_fb2ib_WFB2IB0200_Qry" Culture="auto" UICulture="auto"  %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <style type="text/css">
        #txt_LICENSE_ID_Add {
            text-transform: uppercase;
        }

        #txt_LICENSE_ID {
            text-transform: uppercase;
        }

        #txt_LICENSE_ID_Add {
            background-color: white;
        }

        #txt_BIRTH_DT_Add {
            background-color: white;
        }

        #txt_EMP_NAME_Add {
            background-color: white;
        }
    </style>


    <script type="text/javascript">



        jQuery(document).ready(function () {

            iniForm();

        });


        function iniForm() {
            $("#txt_START_YM_Add").datepicker({ dateFormat: 'yy/mm' });
            $("#txt_END_YM_Add").datepicker({ dateFormat: 'yy/mm' });
            $("#txt_LICENSE_ID_Add,#txt_BIRTH_DT_Add,#txt_EMP_NAME_Add").css("color", "black").css("background-color", "white").attr("disabled", true);
            $(".number").mask('9999/99');
            $("#txt_EMP_ID").mask('99999');
            $("#txt_EMP_ID_Add").mask('99999');


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
                                $('#txt_EMP_NAME').keydown(false)
                                $('#txt_LICENSE_ID').val(JData.LICENSE_ID);
                                $('#txt_LICENSE_ID').keydown(false)
                                $('#txt_BIRTH_DT').val(JData.BIRTH_DT);
                                $('#txt_BIRTH_DT').keydown(false)
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                }
            });
            $('#txt_EMP_ID_Add').change(function () {
                if ($('#txt_EMP_ID_Add').val().length == 5) {
                    //ajax 取得員工基本資料
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_EMP_ID_Add').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "")
                                alert(JData.errMsg);
                            else {

                                $('#txt_EMP_NAME_Add').val(JData.EMP_NAME);
                                $('#txt_EMP_NAME_Add').keydown(false)
                                $('#txt_LICENSE_ID_Add').val(JData.LICENSE_ID);
                                $('#txt_LICENSE_ID_Add').keydown(false)
                                $('#txt_BIRTH_DT_Add').val(JData.BIRTH_DT);
                                $('#txt_BIRTH_DT_Add').keydown(false)
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                }
                if ($('#txt_EMP_ID_Add').val().length == 0) {
                    //ajax 取得員工基本資料
                    $('#txt_EMP_NAME_Add').val("");
                    $('#txt_EMP_NAME_Add').keydown(false)
                    $('#txt_LICENSE_ID_Add').val("");
                    $('#txt_LICENSE_ID_Add').keydown(false)
                    $('#txt_BIRTH_DT_Add').val("");
                    $('#txt_BIRTH_DT_Add').keydown(false)
                }

            });
            $('#txt_START_YM_Add,#txt_END_YM_Add').change(function () {
                if ($('#txt_START_YM_Add').val() != '' && $('#txt_END_YM_Add').val() != '') {
                    var b = new Date($('#txt_START_YM_Add').val() + '/01');
                    var a = new Date($('#txt_END_YM_Add').val() + '/01');
                    if (b > a) {
                        $(this).val("");
                        alert("起日不可大於迄日");
                    }
                }
            });

        }
        function checkDate(obj) {
            var strDate = obj.value;
            re = /\d{4}\/\d{2}\/\d{2}/g
            if (re.test(strDate)) {
                var DateArray = strDate.split("/");
                var dateElement = new Date(DateArray[0], parseInt(DateArray[1]) - 1, DateArray[2]);
                if (!((dateElement.getFullYear() == parseInt(DateArray[0])) && ((dateElement.getMonth() + 1) == parseInt(DateArray[1])) && (dateElement.getDate() == parseInt(DateArray[2])))) {
                    alert($('#checkDateFailMessage').val())
                    obj.value = '';
                }

            }
            else {
                alert($('#checkDateFailMessage').val())
                obj.value = '';
            }
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
            var date = new Date();
            $('#txt_EMP_ID').val("");
            $('#txt_EMP_NAME').val("");
            $('#txt_START_YM').val(date.getFullYear());
            $('#txt_LICENSE_ID').val("");
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
                alert($('#hidWFB2IB_Mod_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckDtlAction() {
            if (LookUpCheckboxs() == 1) {
                $.ajax({
                    url: "../commgeo/WFB2GetINS2_DETAIL_TMPData.ashx",
                    data: {
                        EMP_ID: $('#txt_EMP_ID').val()
                    },
                    type: "GET",
                    dataType: 'json',
                    success: function (JData) {
                        if (JData.errMsg == "1")
                            alert(JData.errMsg);
                        else {
                            alert(JData.errMsg);
                        }
                    },

                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });

                return true;
            }
            else {
                alert($('#hidWFB2IB_Dtl_NotChoiceMessage').val());
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
            if (!processed) {
                $.unblockUI();
                return;
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
        function CheckDelAction() {
            if (LookUpCheckboxs() > 0)
                //return confirm($('#hidwfb299_Del_ConfirmMessage').val());
                return true;
            else {
                alert($('#hidwfb299_Del_NotChoiceMessage').val());
                return false;
            }
        }

        function IsIntText() {
            var charkeycode = window.event.keyCode;
            if (charkeycode > 47 && charkeycode < 58) {
                return true;
            }
            return false;
        }
        function IsFloatText() {

            var charkc = window.event.keyCode
            if (charkc == 46 || (charkc >= 48 && charkc <= 57)) {
            }
            return false;
        }

        function CheckSaveAction() {
            if ($('#txtCALENDAR_CD_Edit').val() != undefined) {
                if ($('#txtCALENDAR_CD_Edit').val().trim() == "") {
                    alert($('#hidWFB2IB_txtCALENDAR_CD_NotNull').val());
                    return false;
                }
                else
                    return confirm($('#hidWFB2IB_Save_ConfirmMessage').val());
            }
            else
                return confirm($('#hidWFB2IB_Save_ConfirmMessage').val());
        }


        function checkconfirm(checkconfirmmessage) {
            var ret = confirm(checkconfirmmessage);
            if (ret) {
                __doPostBack('execute', '');

            }
        }
        //function OpenEmpSearchSB2300() {
        //    var returnValue = window.showModalDialog("../comm/Dept_Search.aspx?mode=all&super=N&parentFuncId=" + parentFuncID, self, 'dialogWidth=1000px;dialogHeight=400px;scroll=no;addressbar:No;');
        //    if (returnValue == undefined) {
        //        returnValue = window.returnValue;
        //    }
        //    if (!(typeof returnValue === 'undefined')) {

        //        var obj = jQuery.parseJSON(returnValue);
        //        $("#txt_EMP_ID_Add").val(obj.EMP_ID);
        //        //$("#" + emp_name).val(obj.EMP_NAME);

        //        doEmpAjax();
        //        return obj;

        //    }
        //}
        function returnEMPValueToPage(emp_id, emp_name, value) {
            var returnValue = value;
            if (value == undefined) {
                returnValue = 'undefined';
            }
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                $("#txt_EMP_ID_Add").val(obj.EMP_ID);
                doEmpAjax();
                return obj;
            }
        }
        function doEmpAjax() {
            //ajax 取得員工基本資料
            $.ajax({
                url: "../commgeo/WFB2GetEmpData.ashx",
                data: {
                    EMP_ID: $('#txt_EMP_ID_Add').val()
                },
                type: "GET",
                dataType: 'json',
                success: function (JData) {
                    if (JData.errMsg != "") {
                        $('#txt_EMP_NAME').val("");
                        $('#txt_EMP_NAME_Add').val("");
                        $('#txt_LICENSE_ID_Add').val("");
                        $('#txt_BIRTH_DT_Add').val("");
                        alert(JData.errMsg);
                    } else {
                        $('#txt_EMP_NAME_Add').val(JData.EMP_NAME);
                        $('#txt_EMP_NAME_Add').keydown(false)
                        $('#txt_LICENSE_ID_Add').val(JData.LICENSE_ID);
                        $('#txt_LICENSE_ID_Add').keydown(false)
                        $('#txt_BIRTH_DT_Add').val(JData.BIRTH_DT);
                        $('#txt_BIRTH_DT_Add').keydown(false)
                    }
                },

                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });
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
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2ib_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" CssClass="Clear" onkeypress="return IsIntText();"></asp:TextBox>
                                        <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID','txt_EMP_NAME','','');" />
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" CssClass="Clear" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_START_YM" runat="server" Text="<%$Resources:Resource,wfb2ib_lb_START_YM%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_START_YM" runat="server" MaxLength="4" Width="64px" ClientIDMode="Static" CssClass="Clear" onkeyup="value=value.replace(/[^\d]/g,'') "></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="年度輸入錯誤"
                                            ControlToValidate="txt_START_YM" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="\d\d\d\d" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_LICENSE_ID" runat="server" Text="<%$Resources:Resource,wfb2ib_LICENSE_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:TextBox ID="txt_LICENSE_ID" runat="server" MaxLength="10" Width="128px" ClientIDMode="Static" CssClass="Clear"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <%--<aces:Btn ID="WFB2IB0200Search" runat="server" Text="<%$Resources:Resource,btn_Search%>" OnClick="WFB2IB0200Search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA"  />--%>

                                            <asp:Button ID="WFB2IB0200Search" runat="server" Text="<%$Resources:Resource,btn_Search%>" OnClick="WFB2IB0200Search_Click" OnClientClick="CheckValid();"  ValidationGroup="GroupA" />

                                            <input id="WFB2IB0200Clear" type="button" value="<%$Resources:Resource,btn_clear%>" runat="server" onclick="ClearAll();" />
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
                            <%--<aces:Btn ID="WFB2IB0200Add" runat="server" Text="<%$Resources:Resource,btn_Add%>" OnClick="WFB2IB0200Add_Click" />
                            <aces:Btn ID="WFB2IB0200Delete" runat="server" Text="<%$Resources:Resource,btn_Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2IB0200Delete_Click" Visible="False" />
                            <aces:Btn ID="WFB2IB0200Edit" runat="server" Text="<%$Resources:Resource,btn_Edit%>" OnClick="WFB2IB0200Edit_Click" Visible="False" />
                            <aces:Btn ID="WFB2IB0200Save" runat="server" Text="<%$Resources:Resource,btn_Save%>" Visible="false" OnClick="WFB2IB0200Save_Click" OnClientClick="return saveCheck();" ValidationGroup="GroupA" />--%>

                             <asp:Button ID="WFB2IB0200Add" runat="server" Text="<%$Resources:Resource,btn_Add%>" OnClick="WFB2IB0200Add_Click" />
                            <asp:Button ID="WFB2IB0200Delete" runat="server" Text="<%$Resources:Resource,btn_Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2IB0200Delete_Click" Visible="False" />
                            <asp:Button ID="WFB2IB0200Edit" runat="server" Text="<%$Resources:Resource,btn_Edit%>"  OnClick="WFB2IB0200Edit_Click" Visible="False" />
                            <asp:Button ID="WFB2IB0200Save" runat="server" Text="<%$Resources:Resource,btn_Save%>" Visible="false" OnClick="WFB2IB0200Save_Click" OnClientClick="return saveCheck();" ValidationGroup="GroupA" />

                            <asp:Button ID="WFB2IB0200Cancel" runat="server" Text="<%$Resources:Resource,btn_Cancel%>" Visible="false" OnClick="WFB2IB0200Cancel_Click" OnClientClick="return confirm($('#HidCheckCanMessage').val());" />
                            <asp:HiddenField ID="HidCancel" runat="server" Value="<%$Resources:Resource,btn_Cancel%>" />
                        </div>

                    </td>
                </tr>
            </table>
            <!--2990400_QRY結束-->
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="Cfb2ib0200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="txt_EMP_ID" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_START_YM"
                        Name="txt_START_YM" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_LICENSE_ID"
                        Name="txt_LICENSE_ID" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />

                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField>
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
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2_RowNumber%>">
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

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ib_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="120px">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="txt_EMP_ID_Add" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>

                        <FooterTemplate>
                            <div style="text-align: center; width: 120px">
                                <asp:TextBox ID="txt_EMP_ID_Add" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" CssClass="MandatoryField" onkeypress="return IsIntText();"></asp:TextBox>
                                <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID_Add', '', 'N', 'Y');" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ib_EMP_ID_Required%>"
                                    ControlToValidate="txt_EMP_ID_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="">
                                </asp:RequiredFieldValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ib_LICENSE_ID%>" SortExpression="LICENSE_ID">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_LICENSE_ID" runat="server" Text='<%#Bind("LICENSE_ID")%>' BorderWidth="0"></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="lbl_LICENSE_ID" runat="server" Text='<%#Bind("LICENSE_ID")%>' BorderWidth="0"></asp:Label>
                            </div>
                        </EditItemTemplate>

                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:TextBox ID="txt_LICENSE_ID_Add" runat="server" MaxLength="10" Width="94px" ClientIDMode="Static" Enabled="false" BorderWidth="0"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ib_LICENSE_ID_Required%>"
                                    ControlToValidate="txt_LICENSE_ID_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="">
                                </asp:RequiredFieldValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ib_BIRTH_DT%>" SortExpression="BIRTH_DT">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:Label ID="lbl_BIRTH_DT" runat="server" Text='<%#Bind("BIRTH_DT2")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="txt_BIRTH_DT_Add" CssClass="number" MaxLength="10" ClientIDMode="Static" runat="server" Text='<%#Bind("BIRTH_DT2")%>' BorderWidth="0"></asp:Label>
                            </div>
                        </EditItemTemplate>

                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_BIRTH_DT_Add" runat="server" MaxLength="10" Width="84px" ClientIDMode="Static" CssClass="number" Enabled="false" BorderWidth="0"></asp:TextBox>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>




                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ib_EMP_NAME%>" SortExpression="EMP_NAME">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="lbl_EMP_NAME" runat="server" Width="64" Text='<%# Convert.ToString(Eval("EMP_NAME"))%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="txt_EMP_NAME_Add" MaxLength="30" Width="64" ClientIDMode="Static" runat="server" Text='<%#Bind("EMP_NAME")%>' BorderWidth="0"></asp:Label>

                            </div>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:TextBox ID="txt_EMP_NAME_Add" MaxLength="30" Width="64" ClientIDMode="Static" runat="server" Enabled="false" BorderWidth="0"></asp:TextBox>

                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ib_NONPAY_CAT%>" SortExpression="NONPAY">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_NONPAY" runat="server" Text='<%#Bind("NONPAY")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:DropDownList ID="ddl_NONPAY_Add" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ib_NONPAY_Required%>"
                                    ControlToValidate="ddl_NONPAY_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                </asp:RequiredFieldValidator>

                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:DropDownList ID="ddl_NONPAY_Add" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ib_NONPAY_Required%>"
                                    ControlToValidate="ddl_NONPAY_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                </asp:RequiredFieldValidator>

                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ib_REMARK%>" SortExpression="REMARK" HeaderStyle-Width="150">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_REMARK" runat="server" Text='<%#Bind("REMARK")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:TextBox ID="txt_REMARK_Add" Height="50px" Width="150px" MaxLength="50" ClientIDMode="Static" TextMode="MultiLine" runat="server" Text='<%#Bind("REMARK")%>'></asp:TextBox>


                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_REMARK_Add" runat="server" Width="150px" Height="50px" MaxLength="50" TextMode="MultiLine" ClientIDMode="Static"></asp:TextBox>


                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ib_START_YM%>" SortExpression="START_YM">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="lbl_START_YM" runat="server" Text='<%#Bind("START_YM")%>' CssClass="number"></asp:Label>

                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:TextBox ID="txt_START_YM_Add" runat="server" MaxLength="7" Width="64px" CssClass="MandatoryField number" ClientIDMode="Static" Text='<%#Bind("START_YM")%>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ib_START_YM_Required%>"
                                    ControlToValidate="txt_START_YM_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="Regular_txt_SALARY_DT" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2ib_ERR_START_YM_Add_DT%>" ControlToValidate="txt_START_YM_Add" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression="(19|20|99)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:TextBox ID="txt_START_YM_Add" runat="server" MaxLength="7" Width="64px" CssClass="MandatoryField number" ClientIDMode="Static"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ib_START_YM_Required%>"
                                    ControlToValidate="txt_START_YM_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="Regular_txt_SALARY_DT" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2ib_ERR_START_YM_Add_DT%>" ControlToValidate="txt_START_YM_Add" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression="(19|20|99)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ib_END_YM%>" SortExpression="END_YM">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="lbl_END_YM" runat="server" Text='<%#Bind("END_YM")%>' CssClass="number"></asp:Label>

                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:TextBox ID="txt_END_YM_Add" runat="server" MaxLength="7" Width="64px" ClientIDMode="Static" Text='<%#Bind("END_YM")%>' CssClass="number"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="Regular_txt_END_YM_Add" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2ib_ERR_END_YM_Add_DT%>" ControlToValidate="txt_END_YM_Add" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression="(19|20|99)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>

                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:TextBox ID="txt_END_YM_Add" runat="server" MaxLength="7" Width="64px" ClientIDMode="Static" CssClass="number"></asp:TextBox>

                                <asp:RegularExpressionValidator ID="Regular_txt_END_YM_Add" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2ib_ERR_END_YM_Add_DT%>" ControlToValidate="txt_END_YM_Add" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression="(19|20|99)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>

                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>

                <EmptyDataTemplate>
                    <table class="grid-view" width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                        <tr class="header">
                            <td width="20px"></td>
                            <td width="40px">
                                <asp:Label ID="lblHeaderRowNumber" runat="server" Text="<%$Resources:Resource,wfb299_RowNumber%>"></asp:Label>
                            </td>
                            <td width="120px">
                                <asp:Label ID="lblHeaderEMP_ID" runat="server" Text="<%$Resources:Resource,wfb2ib_EMP_ID%>"></asp:Label>
                            </td>
                            <td width="100px">
                                <asp:Label ID="lblHeaderLICENSE_ID" runat="server" Text="<%$Resources:Resource,wfb2ib_LICENSE_ID%>"></asp:Label>
                            </td>

                            <td width="60px">
                                <asp:Label ID="lblHeaderBIRTH_DT" runat="server" Text="<%$Resources:Resource,wfb2ib_BIRTH_DT%>"></asp:Label>
                            </td>
                            <td width="60px">
                                <asp:Label ID="lblHeaderEMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2ib_EMP_NAME%>"></asp:Label>
                            </td>
                            <td width="60px">
                                <asp:Label ID="lblHeaderNONPAY" runat="server" Text="<%$Resources:Resource,wfb2ib_NONPAY_CAT%>"></asp:Label>
                            </td>
                            <td width="60px">
                                <asp:Label ID="lblHeaderREMARK" runat="server" Text="<%$Resources:Resource,wfb2ib_REMARK%>"></asp:Label>
                            </td>
                            <td width="60px">
                                <asp:Label ID="lblHeaderSTART_YM" runat="server" Text="<%$Resources:Resource,wfb2ib_START_YM%>"></asp:Label>
                            </td>
                            <td width="60px">
                                <asp:Label ID="lblHeaderEND_YM" runat="server" Text="<%$Resources:Resource,wfb2ib_END_YM%>"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td></td>
                            <td></td>
                            <td>
                                <div style="text-align: center; width: 100%;">
                                    <asp:TextBox ID="txt_EMP_ID_Add" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" CssClass="MandatoryField" onkeypress="return IsIntText();"></asp:TextBox>
                                    <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID_Add', '', 'N', 'Y');" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ib_EMP_ID_Required%>"
                                        ControlToValidate="txt_EMP_ID_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%;">
                                    <asp:TextBox ID="txt_LICENSE_ID_Add" runat="server" MaxLength="10" Width="128px" ClientIDMode="Static" CssClass="MandatoryField" onkeyup="value=value.replace(/[\W]/g,'') "></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ib_LICENSE_ID_Required%>"
                                        ControlToValidate="txt_LICENSE_ID_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:TextBox ID="txt_BIRTH_DT_Add" runat="server" MaxLength="10" Width="84px" ClientIDMode="Static" onchange="javascript:checkDate(this);"></asp:TextBox>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:TextBox ID="txt_EMP_NAME_Add" MaxLength="150" ClientIDMode="Static" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:TextBox>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:DropDownList ID="ddl_NONPAY_Add" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ib_NONPAY_Required%>"
                                        ControlToValidate="ddl_NONPAY_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:TextBox ID="txt_REMARK_Add" Height="50px" Width="150px" MaxLength="50" TextMode="MultiLine" ClientIDMode="Static" runat="server" Text='<%#Bind("REMARK")%>'></asp:TextBox>

                                </div>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:TextBox ID="txt_START_YM_Add" runat="server" MaxLength="7" CssClass="MandatoryField number" Width="64px" ClientIDMode="Static" Text='<%#Bind("START_YM")%>' onkeypress="return IsIntText();"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ib_START_YM_Required%>"
                                        ControlToValidate="txt_START_YM_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:TextBox ID="txt_END_YM_Add" runat="server" MaxLength="7" ClientIDMode="Static" Width="64px" CssClass="number" onkeypress="return IsIntText();"></asp:TextBox>

                                </div>

                                </div>
                            </td>
                            </div>
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
                <HeaderStyle CssClass="GridviewScrollHeader" />
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle HorizontalAlign="Center" />
                <EditRowStyle HorizontalAlign="Center" />
            </asp:GridView>
            <td style="font-size: 14px;">
                <asp:Label ID="lb_TotalCount2" runat="server" Text="" Visible="false" Font-Size="10"></asp:Label>
            </td>
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

            <asp:HiddenField runat="server" ClientIDMode="Static" ID="checkDateFailMessage" Value="<%$Resources:Resource,wfb2ib_check_Error%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="DateFailMessage" Value="<%$Resources:Resource,wfb2ib_checkdata_Error%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="HidCheckDelMessage" Value="<%$Resources:Resource,wfb2ib_CheckDel_Required%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="HidCheckCanMessage" Value="<%$Resources:Resource,wfb2ib_CheckCan_Required%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="HidCheckEditMessage" Value="<%$Resources:Resource,wfb2ib_CheckEdit_Required%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="HidCheckDeleteMessage" Value="<%$Resources:Resource,wfb2ib_CheckDelete_Required%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Del_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Mod_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Mod_NotChoiceMessage%> " />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Save_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Save_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Del_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Dtl_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Dtl_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Cancel_ConfirmMessage%>" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


