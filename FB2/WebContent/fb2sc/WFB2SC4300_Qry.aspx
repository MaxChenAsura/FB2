<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sc/WFB2SC4300_Qry.aspx.cs" Inherits="WebContent_fb2sc_WFB2SC4300_Qry" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>



    <script type="text/javascript">



        jQuery(document).ready(function () {
            iniForm();

        });

        function iniForm() {
            $("#txt_REPAY_SDT").datepicker({ dateFormat: 'yy/mm/dd' });
            $("#txt_REPAY_EDT").datepicker({ dateFormat: 'yy/mm/dd' });
            $("#txt_REPAY_DT_Add_").datepicker({ dateFormat: 'yy/mm/dd' });
            $("#txt_EMP_NAME_Add,#txt_EMP_CD").css("color", "black").css("background-color", "white");
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');
            $(".number").mask('9999/99/99');
            $(".empid").mask('99999');
            $(".money").mask('99999.999999');

            gridviewScroll();
            $.unblockUI();

            $('#txt_EMP_ID').change(function () {
                if ($('#txt_EMP_ID').val().length == 5) {
                    //ajax 取得員工基本資料
                    doEmpAjax();
                }
                else {
                    $('#txt_EMP_NAME').val("");
                }
            });
            $('#txt_EMP_ID_Add').change(function () {
                if ($('#txt_EMP_ID_Add').val().length == 5) {
                    //ajax 取得員工基本資料
                    doEmpAjax2();
                }
            });
            //$('#txt_UNITS_Add').change(function () {
            //    var num = $('#txt_UNITS_Add').val();
            //    var userreg=/^[0-9]+([.]{1}[0-9]{1,2})?$/;
            //    if (userreg.test(num)) {
            //    } else {
            //        alert("請輸入數字");
            //        $(this).val("");

            //    }
            //});

            $('#txt_REPAY_SDT,#txt_REPAY_EDT').change(function () {
                if ($('#txt_REPAY_SDT').val() != '' && $('#txt_REPAY_EDT').val() != '') {
                    var b = new Date($('#txt_REPAY_SDT').val());
                    var a = new Date($('#txt_REPAY_EDT').val());
                    if (b > a) {
                        $(this).val("");
                        alert("起日不可大於迄日");
                    }
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
                success: function (JData) {
                    if (JData.errMsg != "") {
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
            if ($("#txt_EMP_ID_Add").val().length == 5) {
                $.ajax({
                    url: "../commgeo/WFB2GetEmpData.ashx",
                    data: {
                        EMP_ID: $('#txt_EMP_ID_Add').val()
                    },
                    type: "GET",
                    dataType: 'json',
                    success: function (JData) {
                        if (JData.errMsg != "") {
                            $('#txt_EMP_NAME_add').val("");
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
            } else {
                $('#txt_EMP_NAME_Add').val("");
                $('#txt_EMP_CD_Add').val("");
                $('#txt_DEPT_DESC_Add').val("");
                $('#txt_LEAVE_DT_Add').val("");
            }
        }
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        function gridviewScroll() {


            if ($("#HID_Freeze").val() == "N") {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "500",
                    barcolor: "#7F7F7F"
                });
                //$("#ContentPlaceHolder1_gv_resultWrapper").height(600);
            }
            else {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "500",
                    barcolor: "#7F7F7F",
                    freezesize: 4

                });

                CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
            }


        }
        function ClearAll() {
            $('#txt_REPAY_SDT').val("");
            $('#txt_REPAY_EDT').val("");
            $('#txt_SALARY_NAME').val("");
            $('#ddl_PROCESS_STATUS').val(-1);
            $('#ddl_REPAY_SUB_ID').find('option').remove().end();


            $('#ddl_REPAY_TYPE').val(-1);
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
            var ItemCheckBoxs = $("[type=checkbox]");
            alert(LookUpCheckboxs());
            if (ItemCheckBoxs.length != 5) {
                if (LookUpCheckboxs() == 1)
                    return true;
                else {
                    alert($('#hidwfb2_Mod_NotChoiceMessage').val());
                    return false;
                }
            }
        }

        function CheckDtlAction() {
            var processed = true;
            if (LookUpCheckboxs() > 0)
                processed = true;
            else {
                alert($('#hidwfb2_Dtl_NotChoiceMessage').val());
                return false;
            }

            if (confirm("確定要刪除?")) {
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


            if (Page_ClientValidate("GroupC")) {
                BlockUI();
            }
            else
                processed = false;
            if (!processed)
                $.unblockUI();

            return processed;
        }

        function CheckAdd() {
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
                if (ItemCheckBoxs[i].checked && ItemCheckBoxs[i].id.indexOf("cb_all") == -1) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
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
                alert("請選取資料!");

                return false;
            }
        }
        function check_UNITS_Add(field) {
            var number = $('#' + field).val();
            var re = /(^[-+]?\d{1,3})+(.\d{1})?$/;
            if (number != "") {
                if (!re.test(number)) {
                    $('#' + field).val("");
                    alert('只能輸入正負三位數與一位小數點,且不能為0');
                    return false;
                }
                else {
                    if (parseFloat(number, 10) == 0) {
                        $('#' + field).val("");
                        alert('只能輸入正負三位數與一位小數點,且不能為0');
                        return false;
                    }
                }
            }

        }
        function MyCheckValid() {
            setTimeout("MydelayedShow()", 1);
        }

        function MydelayedShow() {
            if (Page_IsValid != null && Page_IsValid == true) {
                $.unblockUI();
            }
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
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
                                        <asp:Label ID="REPAY_DT" runat="server" Text="">追溯日期區間</asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_REPAY_SDT" runat="server" Width="100px" ClientIDMode="Static" CssClass="number"></asp:TextBox>
                                        ~
                                        <asp:TextBox ID="txt_REPAY_EDT" runat="server" Width="100px" ClientIDMode="Static" CssClass="number"></asp:TextBox>
                                        <asp:CustomValidator ID="qrytest3" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_OP_DT_E%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_REPAY_SDT" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator4" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_OP_DT_E%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_REPAY_EDT" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_REPAY_SDT"
                                            ControlToValidate="txt_REPAY_EDT" ErrorMessage="追溯日期起日不可大於迄日" Type="Date" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupC"></asp:CompareValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="PROCESS_STATUS" runat="server" Text="">處理狀態</asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_PROCESS_STATUS" runat="server" Width="100px" ClientIDMode="Static">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="REPAY_TYPE" runat="server" Text="">追溯類別</asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_REPAY_TYPE" runat="server" Width="100px" ClientIDMode="Static" AutoPostBack="True" OnSelectedIndexChanged="ddl_REPAY_TYPE_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="REPAY_SUB_ID" runat="server" Text="">追溯項目</asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_REPAY_SUB_ID" runat="server" ClientIDMode="Static">
                                            <asp:ListItem Value="請先選擇追溯類別">請先選擇追溯類別</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="EMP_ID" runat="server" Text="">工號</asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" CssClass="empid"></asp:TextBox>
                                        <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', '', '');" />

                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="EMP_NAME" runat="server" Text="">姓名</asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="30" Width="64px" ClientIDMode="Static" CssClass="Clear"></asp:TextBox>
                                    </td>
                                </tr>

                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SC4300Search" runat="server" Text="<%$Resources:Resource,btn_Search%>" OnClientClick="saveCheck();" OnClick="WFB2SC4300Search_Click" />
                                            <%--<asp:Button ID="WFB2SC4300Search" runat="server" Text="<%$Resources:Resource,btn_Search%>" OnClientClick="saveCheck();" OnClick="WFB2SC4300Search_Click" />--%>
                                            <asp:Button ID="WFB2SC4300Clear" runat="server" Text="<%$Resources:Resource,btn_clear%>" OnClientClick="ClearAll();" CausesValidation="false" />
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

                    <td align="left" class="Body_label">
                        <table width="100%">
                            <tr>
                                <td align="left" class="Body_label" style="width: 60%;">
                                    <div id="init_excel" style="text-align: left;">
                                        Excel上傳檔案:<asp:FileUpload ID="FileUpload1" runat="server" Width="300" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sl_lb_Excel_Upload_isNull%>"
                                            ControlToValidate="FileUpload1" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                         <aces:Btn ID="WFB2SC4300Upload" runat="server" Text="EXCEL上傳" OnClick="WFB2SC4300Upload_Click" ValidationGroup="GroupB" OnClientClick="MyCheckValid();" />
                                       <aces:Btn ID="WFB2SC4300Download" runat="server" Text="範本下載" OnClick="WFB2SC4300Download_Click" />
                                        <%--<asp:Button ID="WFB2SC4300Upload" runat="server" Text="EXCEL上傳" OnClick="WFB2SC4300Upload_Click" ValidationGroup="GroupB" OnClientClick="MyCheckValid();" />
                                        <asp:Button ID="WFB2SC4300Download" runat="server" Text="範本下載" OnClick="WFB2SC4300Download_Click" />--%>
                                    </div>
                                </td>
                                <td align="right" class="Body_label" style="width: 40%;">
                                    <div id="init_grid" style="text-align: right;">
                                        <aces:Btn ID="WFB2SC4300Add" runat="server" Text="<%$Resources:Resource,btn_Add%>" OnClick="WFB2SC4300Add_Click" />
                                        <aces:Btn ID="WFB2SC4300Delete" runat="server" Text="<%$Resources:Resource,btn_Delete%>" OnClientClick="return  CheckDtlAction();" OnClick="WFB2SC4300Delete_Click" Visible="false" />
                                        <aces:Btn ID="WFB2SC4300Edit" runat="server" Text="<%$Resources:Resource,btn_Edit%>" OnClick="WFB2SC4300Edit_Click" Visible="false" />
                                        <aces:Btn ID="WFB2SC4300Save" runat="server" Text="<%$Resources:Resource,btn_Save%>" Visible="false" OnClick="WFB2SC4300Save_Click" OnClientClick="return CheckAdd();" />                               
                                        
                                        <%--
                                        <asp:Button ID="WFB2SC4300Add" runat="server" Text="<%$Resources:Resource,btn_Add%>" OnClick="WFB2SC4300Add_Click" />
                                        <asp:Button ID="WFB2SC4300Delete" runat="server" Text="<%$Resources:Resource,btn_Delete%>" OnClientClick="return  CheckDtlAction();" OnClick="WFB2SC4300Delete_Click" Visible="false" />
                                        <asp:Button ID="WFB2SC4300Edit" runat="server" Text="<%$Resources:Resource,btn_Edit%>" OnClick="WFB2SC4300Edit_Click" Visible="false" />
                                        <asp:Button ID="WFB2SC4300Save" runat="server" Text="<%$Resources:Resource,btn_Save%>" Visible="false" OnClick="WFB2SC4300Save_Click" OnClientClick="return CheckAdd();" />
                                          
                                        
                                            --%>
                                        <asp:Button ID="WFB2SC4300Cancel" runat="server" Text="<%$Resources:Resource,btn_Cancel%>" Visible="false" OnClick="WFB2SC4300Cancel_Click" OnClientClick="return confirm('您確定要放棄目前的編輯資料嗎?');" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <!--2990400_QRY結束-->
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="Cfb2SC430DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_REPAY_SDT"
                        Name="txt_REPAY_SDT" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_REPAY_EDT"
                        Name="txt_REPAY_EDT" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_PROCESS_STATUS"
                        Name="ddl_PROCESS_STATUS" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_REPAY_TYPE"
                        Name="ddl_REPAY_TYPE" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_REPAY_SUB_ID"
                        Name="ddl_REPAY_SUB_ID" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="txt_EMP_ID" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_EMP_NAME"
                        Name="txt_EMP_NAME" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />

                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1990px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="30px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" meta:resourcekey="cb_checkallResource1" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2sc_RowNumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lbl_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="追溯日期" SortExpression="REPAY_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lbl_REPAY_DT" runat="server" Text='<%# Convert.ToString(Eval("REPAY_DT","{0:yyyy/MM/dd}")) %>' CssClass="number"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lbl_REPAY_DT_Add" runat="server" MaxLength="10" ClientIDMode="Static" CssClass="number" Text='<%#Convert.ToString(Eval("REPAY_DT","{0:yyyy/MM/dd}"))%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center;">
                                <asp:TextBox ID="txt_REPAY_DT_Add" runat="server" MaxLength="10" Width="80px" ClientIDMode="Static" CssClass="MandatoryField date" AutoPostBack="true" OnTextChanged="txt_REPAY_DT_Add_TextChanged"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="請輸入追溯日期"
                                    ControlToValidate="txt_REPAY_DT_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="qrytest1" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="追溯日期須輸入日期格式:(例如:2001/01/01)" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_REPAY_DT_Add" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>

                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="130px" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lbl_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="txt_EMP_ID_Add" runat="server" Text='<%#Bind("EMP_ID")%>' MaxLength="5" Width="130px" ClientIDMode="Static"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_EMP_ID_Add" runat="server" CssClass="MandatoryField empid" MaxLength="5" Width="80px" ClientIDMode="Static" AutoPostBack="true" OnTextChanged="txt_EMP_ID_Add_TextChanged"></asp:TextBox>
                            <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID_Add', 'txt_EMP_NAME_Add', '', '');" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="請輸入工號"
                                ControlToValidate="txt_EMP_ID_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                            </asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lbl_EMP_NAME" runat="server" Text='<%# Convert.ToString(Eval("EMP_NAME"))%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="txt_EMP_NAME_Add" MaxLength="30" ClientIDMode="Static" runat="server" Text='<%#Bind("EMP_NAME")%>' BorderWidth="0" Width="60px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_EMP_NAME_Add" MaxLength="30" ClientIDMode="Static" runat="server" BorderWidth="0" Width="60px"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_HOURLY_WAGE%>" SortExpression="HOURLY_WAGE" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lbl_HOURLY_WAGE" runat="server" Text='<%#Bind("HOURLY_WAGE")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lbl_HOURLY_WAGE_Add" runat="server" Text='<%#Bind("HOURLY_WAGE")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lbl_HOURLY_WAGE_Add" runat="server" Style="text-align: right" Width="120px"></asp:Label>
                        </FooterTemplate>


                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_REPAY_TYPE%>" SortExpression="REPAY_TYPE" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lbl_REPAY_TYPE" runat="server" Text='<%#Bind("DESC1")%>'></asp:Label>
                            <asp:HiddenField ID="HidREPAY_TYPE" runat="server" Value='<%#Bind("REPAY_TYPE")%>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lbl_REPAY_TYPE_Add" runat="server" Text='<%#Bind("DESC1")%>'></asp:Label>
                            <asp:HiddenField ID="HidREPAY_TYPE_add" runat="server" Value='<%#Bind("REPAY_TYPE")%>' />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_REPAY_TYPE_Add" runat="server" ClientIDMode="Static" CssClass="MandatoryField" AutoPostBack="True" OnSelectedIndexChanged="ddl_REPAY_TYPE_Add_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="請選擇追溯類別"
                                ControlToValidate="ddl_REPAY_TYPE_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                            </asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_REPAY_SUB_ID%>" SortExpression="REPAY_SUB_ID" HeaderStyle-Width="190px" ItemStyle-Width="190px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lbl_REPAY_SUB_ID" runat="server" Text='<%#Bind("DESC2")%>'></asp:Label>
                            <asp:HiddenField ID="HidREPAY_SUB_ID" runat="server" Value='<%#Bind("REPAY_SUB_ID")%>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lbl_REPAY_SUB_ID_Add" runat="server" Text='<%#Bind("DESC2")%>'></asp:Label>
                            <asp:HiddenField ID="HidREPAY_SUB_ID_add" runat="server" Value='<%#Bind("REPAY_SUB_ID")%>' />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_REPAY_SUB_ID_Add" runat="server" ClientIDMode="Static" CssClass="MandatoryField" AutoPostBack="True" OnSelectedIndexChanged="ddl_REPAY_SUB_ID_SelectedIndexChanged">
                                <asp:ListItem Value="-1">請先選擇追朔類別</asp:ListItem>
                            </asp:DropDownList>

                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="請選擇追溯項目"
                                ControlToValidate="ddl_REPAY_SUB_ID_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                            </asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_BASE_VALUE%>" SortExpression="BASE_VALUE" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lbl_BASE_VALUE" runat="server" Text='<%#Bind("BASE_VALUE")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lbl_BASE_VALUE_Add" runat="server" ClientIDMode="Static" Text='<%#Bind("BASE_VALUE")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lbl_BASE_VALUE_Add" runat="server" ClientIDMode="Static"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_UNITS%>" SortExpression="UNITS" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lbl_UNITS" runat="server" Text='<%#Bind("UNITS")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_UNITS_Add" runat="server" MaxLength="5" Style="text-align: right;" CssClass="MandatoryField" onblur="return check_UNITS_Add(this.id);" ClientIDMode="Static" Text='<%#Bind("UNITS")%>' Width="60px" AutoPostBack="true" OnTextChanged="txt_UNITS_Add_TextChanged"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="請輸入時(日)數"
                                ControlToValidate="txt_UNITS_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                            </asp:RequiredFieldValidator>
                            <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server"
                                ErrorMessage="只能輸入正負三位數與一位小數點,且不能為0" ControlToValidate="txt_UNITS_Add"
                                ValidationExpression="(^[-+]?\d{1,3})+(.\d{1})+$" Display="None" ValidationGroup="GroupA">
                            </asp:RegularExpressionValidator>--%>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_UNITS_Add" runat="server" MaxLength="6" Style="text-align: right;" CssClass="MandatoryField" onblur="return check_UNITS_Add(this.id);" ClientIDMode="Static" Width="60px" AutoPostBack="true" OnTextChanged="txt_UNITS_Add_TextChanged"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="請輸入時(日)數"
                                ControlToValidate="txt_UNITS_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                            </asp:RequiredFieldValidator>
                            <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server"
                                ErrorMessage="只能輸入正負三位數與一位小數點,且不能為0" ControlToValidate="txt_UNITS_Add"
                                ValidationExpression="(^[-+]?\d{1,3})+(.\d{1})+$" Display="None" ValidationGroup="GroupA">
                            </asp:RegularExpressionValidator>--%>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_AMOUNT%>" SortExpression="AMOUNT" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lbl_AMOUNT" runat="server" Text='<%#Bind("AMOUNT")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lbl_AMOUNT_Add" runat="server" Text='<%#Bind("AMOUNT")%>' ClientIDMode="Static"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lbl_AMOUNT_Add" runat="server" ClientIDMode="Static"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_SALARY_ID%>" SortExpression="SALARY_ID" HeaderStyle-Width="150px" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lbl_SALARY_ID" runat="server" Text='<%#Bind("SALARY_NAME")%>'></asp:Label>
                            <asp:HiddenField ID="HidSALARY_ID" runat="server" Value='<%#Bind("SALARY_ID")%>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lbll_SALARY_ID" runat="server" ClientIDMode="Static" Text='<%#Bind("SALARY_NAME")%>'></asp:Label>
                            <asp:HiddenField ID="HidSALARY_ID" runat="server" Value='<%#Bind("SALARY_ID")%>' />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lbll_SALARY_ID" runat="server" ClientIDMode="Static"></asp:Label>
                            <asp:HiddenField ID="HidSALARY_ID" runat="server" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_PROCESS_STATUS%>" SortExpression="PROCESS_STATUS" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lbl_PROCESS_STATUS" runat="server" Text='<%#Bind("DESC1_1")%>'></asp:Label>
                            <asp:HiddenField ID="HidPROCESS_STATUS" runat="server" Value='<%#Bind("PROCESS_STATUS")%>' />
                            <asp:HiddenField ID="HidSQE_NO1" runat="server" Value='<%#Bind("SEQ_NO")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_APPROVE_BY%>" SortExpression="APPROVE_BY" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lbl_APPROVE_BY" runat="server" Text='<%#Bind("APPROVE_NAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_APPROVE_DT%>" SortExpression="APPROVE_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lbl_APPROVE_DT" runat="server" Text='<%# Convert.ToString(Eval("APPROVE_DT","{0:yyyy/MM/dd}"))%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_REMARK%>" SortExpression="REMARK" HeaderStyle-Width="300px" ItemStyle-Width="300px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lbl_REMARK" runat="server" Text='<%#Bind("REMARK")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_REMARK_Add" runat="server" MaxLength="150" ClientIDMode="Static" Text='<%#Bind("REMARK")%>' TextMode="MultiLine" Rows="3" Columns="32" CssClass="MandatoryField"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="請輸入備註說明"
                                ControlToValidate="txt_REMARK_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                            </asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_REMARK_Add" runat="server" MaxLength="150" ClientIDMode="Static" Text='<%#Bind("REMARK")%>' TextMode="MultiLine" Rows="3" Columns="32" CssClass="MandatoryField"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="請輸入備註說明"
                                ControlToValidate="txt_REMARK_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                            </asp:RequiredFieldValidator>
                        </FooterTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_APP_REMARK%>" SortExpression="APP_REMARK" HeaderStyle-Width="300px" ItemStyle-Width="300px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lbl_APP_REMARK" runat="server" Text='<%#Bind("APP_REMARK")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
                <EmptyDataTemplate>
                    <div style="overflow: auto; width: 1020px;">
                        <table class="grid-view" width="1990px" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                            <tr class="header">
                                <td width="30px"></td>
                                <td width="40px">
                                    <asp:Label ID="lblHeaderRowNumber" runat="server" Text="<%$Resources:Resource,wfb299_RowNumber%>"></asp:Label>
                                </td>
                                <td width="100px">
                                    <asp:Label ID="lblHeaderREPAY_DT" runat="server" Text="<%$Resources:Resource,wfb2sc_REPAY_DT%>"></asp:Label>
                                </td>
                                <td width="130px">
                                    <asp:Label ID="lblHeaderEMP_ID" runat="server" Text="<%$Resources:Resource,wfb2sc_EMP_ID%>"></asp:Label>
                                </td>
                                <td width="80px">
                                    <asp:Label ID="lblHeaderEMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2sc_EMP_NAME%>"></asp:Label>
                                </td>
                                <td width="120px">
                                    <asp:Label ID="lblHeaderHOURLY_WAGE" runat="server" Text="<%$Resources:Resource,wfb2sc_HOURLY_WAGE%>"></asp:Label>
                                </td>
                                <td width="100px">
                                    <asp:Label ID="lblHeaderBUDGET_REPAY_TYPE" runat="server" Text="<%$Resources:Resource,wfb2sc_REPAY_TYPE%>"></asp:Label>
                                </td>
                                <td width="190px">
                                    <asp:Label ID="lblHeaderREPAY_SUB_ID" runat="server" Text="<%$Resources:Resource,wfb2sc_REPAY_SUB_ID%>"></asp:Label>
                                </td>
                                <td width="100px">
                                    <asp:Label ID="lblHeaderBASE_VALUE" runat="server" Text="<%$Resources:Resource,wfb2sc_BASE_VALUE%>"></asp:Label>
                                </td>
                                <td width="100%">
                                    <asp:Label ID="lblHeaderUNITS" runat="server" Text="<%$Resources:Resource,wfb2sc_UNITS%>"></asp:Label>
                                </td>
                                <td width="80px">
                                    <asp:Label ID="lblHeaderAMOUNT" runat="server" Text="<%$Resources:Resource,wfb2sc_AMOUNT%>"></asp:Label>
                                </td>
                                <td width="80px">
                                    <asp:Label ID="lblHeaderSALARY_ID" runat="server" Text="<%$Resources:Resource,wfb2sc_SALARY_ID%>"></asp:Label>
                                </td>
                                <td width="150px">
                                    <asp:Label ID="lblHeaderPROCESS_STATUS" runat="server" Text="<%$Resources:Resource,wfb2sc_PROCESS_STATUS%>"></asp:Label>
                                </td>
                                <td width="100px">
                                    <asp:Label ID="lblHeaderAPPROVE_BY" runat="server" Text="<%$Resources:Resource,wfb2sc_APPROVE_BY%>"></asp:Label>
                                </td>
                                <td width="100px">
                                    <asp:Label ID="lblHeaderAPPROVE_DT" runat="server" Text="<%$Resources:Resource,wfb2sc_APPROVE_DT%>"></asp:Label>
                                </td>
                                <td width="300px">
                                    <asp:Label ID="lblHeaderREMARK" runat="server" Text="<%$Resources:Resource,wfb2sc_REMARK%>"></asp:Label>
                                </td>
                                <td width="300px">
                                    <asp:Label ID="lblHeaderAPP_REMARK" runat="server" Text="<%$Resources:Resource,wfb2sc_APP_REMARK%>"></asp:Label>
                                </td>
                            </tr>
                            <tr class="normal">
                                <td></td>
                                <td>
                                    <div style="text-align: center; width: 40px"></div>
                                </td>
                                <td>
                                    <div style="text-align: left; width: 100px">
                                        <asp:TextBox ID="txt_REPAY_DT_Add" runat="server" MaxLength="10" Width="80px" ClientIDMode="Static" CssClass="MandatoryField date" AutoPostBack="true" OnTextChanged="txt_REPAY_DT_Add_TextChanged"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="請輸入追溯日期"
                                            ControlToValidate="txt_REPAY_DT_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                        </asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="qrytest1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="追溯日期須輸入日期格式:(例如:2001/01/01)" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_REPAY_DT_Add" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: left; width: 130px">
                                        <asp:TextBox ID="txt_EMP_ID_Add" runat="server" CssClass="MandatoryField" MaxLength="5" Width="60px" ClientIDMode="Static" AutoPostBack="true" OnTextChanged="txt_EMP_ID_Add_TextChanged"></asp:TextBox>
                                        <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID_Add', 'txt_EMP_NAME_Add', '', '');" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="請輸入工號"
                                            ControlToValidate="txt_EMP_ID_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: left; width: 100px;">
                                        <asp:TextBox ID="txt_EMP_NAME_Add" MaxLength="30" ClientIDMode="Static" runat="server" BorderWidth="0" Width="60px"></asp:TextBox>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: right; width: 120px;">
                                        <asp:Label ID="lbl_HOURLY_WAGE_Add" runat="server" Width="120px" Style="text-align: right"></asp:Label>
                                        <%--<asp:RequiredFieldValidator ID="HOURLY_WAGE_Required" runat="server" ErrorMessage="此工號取得時薪=0,請確認該員工當月敘薪資料是否正確!"
                                            ControlToValidate="lbl_HOURLY_WAGE_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="0">
                                        </asp:RequiredFieldValidator>--%>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: left; width: 100px">
                                        <asp:DropDownList ID="ddl_REPAY_TYPE_Add" runat="server" Width="100px" ClientIDMode="Static" CssClass="MandatoryField" AutoPostBack="True" OnSelectedIndexChanged="ddl_REPAY_TYPE_Add_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="請選擇追溯類別"
                                            ControlToValidate="ddl_REPAY_TYPE_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: left; width: 150px">
                                        <asp:DropDownList ID="ddl_REPAY_SUB_ID_Add" runat="server" ClientIDMode="Static" CssClass="MandatoryField" AutoPostBack="True" OnSelectedIndexChanged="ddl_REPAY_SUB_ID_SelectedIndexChanged">
                                            <asp:ListItem Value="-1">請先選擇追朔類別</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="請選擇追溯項目"
                                            ControlToValidate="ddl_REPAY_SUB_ID_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </td>

                                <td>
                                    <div style="text-align: right; width: 140px">
                                        <asp:Label ID="lbl_BASE_VALUE_Add" runat="server" ClientIDMode="Static"></asp:Label>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: right; width: 80px">
                                        <asp:TextBox ID="txt_UNITS_Add" runat="server" MaxLength="5" CssClass="MandatoryField" onblur="return check_UNITS_Add(this.id);" ClientIDMode="Static" Width="50px" AutoPostBack="true" OnTextChanged="txt_UNITS_Add_TextChanged"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="請輸入時(日)數"
                                            ControlToValidate="txt_UNITS_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                        </asp:RequiredFieldValidator>
                                        <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server"
                                            ErrorMessage="只能輸入正負三位數與一位小數點,且不能為0" ControlToValidate="txt_UNITS_Add"
                                            ValidationExpression="(^[-+]?\d{1,3})+(.\d{1})+$" Display="None" ValidationGroup="GroupA">
                                        </asp:RegularExpressionValidator>--%>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: right; width: 100px">
                                        <asp:Label ID="lbl_AMOUNT_Add" runat="server" ClientIDMode="Static"></asp:Label>

                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: left; width: 150px">
                                        <asp:Label ID="lbll_SALARY_ID" runat="server" ClientIDMode="Static"></asp:Label>
                                        <asp:HiddenField ID="HidSALARY_ID" runat="server" />
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: left; width: 100px;">
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: left; width: 100px;">
                                    </div>
                                </td>

                                <td>
                                    <div style="text-align: left; width: 100px;">
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: left; width: 300px;">
                                        <asp:TextBox ID="txt_REMARK_Add" runat="server" MaxLength="150" ClientIDMode="Static" Text='<%#Bind("REMARK")%>' TextMode="MultiLine" Rows="3" Columns="30" CssClass="MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="請輸入備註說明"
                                            ControlToValidate="txt_REMARK_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: left; width: 300px;">
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
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
            <asp:TextBox ID="HiddFN_DT" runat="server" Width="0px" ClientIDMode="Static" BorderWidth="0" Text=" "></asp:TextBox>
            <asp:RequiredFieldValidator ID="HiddFN_DT_Required" runat="server" ErrorMessage="此筆資料日期尚未計薪,無法新增追溯資料"
                ControlToValidate="HiddFN_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue=" ">
            </asp:RequiredFieldValidator>
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HidREPAY_TYPE_V1" runat="server" />  <%-- 加項代碼--%>
            <asp:HiddenField ID="HidREPAY_TYPE_V2" runat="server" />  <%-- 減項代碼--%>
            <asp:HiddenField ID="Hidden_TAX" runat="server" />  <%-- 加班是否應免稅--%>
            <asp:HiddenField ID="HidCODE_VAL2" runat="server" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_NotChoiceMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2_Mod_NotChoiceMessage" Value="<%$Resources:Resource,wfb2_Mod_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Save_ConfirmMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_ConfirmMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2_Dtl_NotChoiceMessage" Value="<%$Resources:Resource,wfb2_Dtl_NotChoiceMessage%>" />

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary3" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupC" ShowSummary="false" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SC4300Upload"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="WFB2SC4300Download"></asp:PostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>


