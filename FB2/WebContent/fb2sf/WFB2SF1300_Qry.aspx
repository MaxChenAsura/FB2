<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sf/WFB2SF1300_Qry.aspx.cs" Inherits="WebContent_fb2sf_WFB2SF1300_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>



    <script type="text/javascript">



        jQuery(document).ready(function () {

            iniForm();
        });


        function iniForm() {
            $("#txt_LICENSE_ID_Add,#txt_BIRTH_DT_Add,#txt_EMP_NAME_Add,#txt_EMP_NAME").css("color", "black").css("background-color", "white").attr("disabled", true);

            $("#txt_SALARY_DT").datepicker({ dateFormat: 'yy/mm/dd' });
            //$("#txt_HOPE_PAT_DT_S").datepicker({ dateFormat: 'yy/mm/dd' });
            //$("#txt_HOPE_PAT_DT_E").datepicker({ dateFormat: 'yy/mm/dd' });
            //$("#txt_HOPE_PAT_DT").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".number2").mask('9999/99/99');
            $("#txt_EMP_ID").mask('99999');
            $(".number").mask('9999/99');
            gridviewScroll();
            $.unblockUI();
            $('#txt_EMP_ID').change(function () {
                if ($('#txt_EMP_ID').val().length == 5) {
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
                                alert(JData.errMsg);
                                $('#txt_EMP_ID').val("");
                                $('#txt_EMP_NAME').val("");
                                $('#txt_EMP_CD').val(JData.EMP_CD + "-" + JData.SUB_DESC);
                                $('#txt_PJOB_DESC').val(JData.PJOB_DESC);
                                $('#txt_DEPT_NO').val(JData.DEPT_NO);
                                $('#txt_DEPT_NAME').val(JData.DEPT_NAME);
                                $('#txt_WORK_SHIFT_CD').val(JData.WORK_SHIFT_CD);
                                $('#txt_AGE').val(JData.AGE);
                                $('#txt_JOIN_DT').val(JData.JOIN_DT);
                                $('#txt_REGISTER_ADDR').val(JData.REGISTER_ADDR);
                                $('#txt_CONTACT_ADDR').val(JData.CONTACT_ADDR);
                                $('#txt_MOBILE_TEL_1').val(JData.MOBILE_TEL_1);
                                $('#txt_CONTACT_TEL').val(JData.CONTACT_TEL);
                                $('#txt_DEPT_DESC').val(JData.DEPT_NO + '-' + JData.DEPT_NAME);
                                $('#txt_LEVEL_CD').val(JData.LEVEL_CD);
                            }
                            else {
                                $('#txt_EMP_NAME').val(JData.EMP_NAME);
                                $('#txt_EMP_CD').val(JData.EMP_CD + "-" + JData.SUB_DESC);
                                $('#txt_PJOB_DESC').val(JData.PJOB_DESC);
                                $('#txt_DEPT_NO').val(JData.DEPT_NO);
                                $('#txt_DEPT_NAME').val(JData.DEPT_NAME);
                                $('#txt_WORK_SHIFT_CD').val(JData.WORK_SHIFT_CD);
                                $('#txt_AGE').val(JData.AGE);
                                $('#txt_JOIN_DT').val(JData.JOIN_DT);
                                $('#txt_REGISTER_ADDR').val(JData.REGISTER_ADDR);
                                $('#txt_CONTACT_ADDR').val(JData.CONTACT_ADDR);
                                $('#txt_MOBILE_TEL_1').val(JData.MOBILE_TEL_1);
                                $('#txt_CONTACT_TEL').val(JData.CONTACT_TEL);
                                $('#txt_DEPT_DESC').val(JData.DEPT_NO + '-' + JData.DEPT_NAME);
                                $('#txt_LEVEL_CD').val(JData.LEVEL_CD);

                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                }
            });
            $('#txt_HOPE_PAT_DT_S,#txt_HOPE_PAT_DT_E').change(function () {
                if ($('#txt_HOPE_PAT_DT_S').val() != '' && $('#txt_HOPE_PAT_DT_E').val() != '') {
                    var b = new Date($('#txt_HOPE_PAT_DT_S').val());
                    var a = new Date($('#txt_HOPE_PAT_DT_E').val());
                    if (b > a) {
                        $(this).val("");
                        alert("起日不可大於迄日");
                    }
                }
            });

            $('#ddl_ACCT_ID').change(function () {
                if ($('#ddl_ACCT_ID').val() == 'Y') {
                    //$("#txt_HOPE_PAT_DT,#txt_S_DT,#txt_E_DT").datepicker("destory");
                    $("#txt_HOPE_PAT_DT,#txt_S_DT,#txt_E_DT").removeClass("hasDatepicker").removeAttr("id");
                }
            });

        }
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        function gridviewScroll() {
            if ($("#HID_Freeze").val() == "Y") {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "400",
                    barcolor: "#7F7F7F",
                    freezesize: 3

                });
                CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
            }
        }
        function CheckExecuteAction1() {
            //已轉傳票就直接alert錯誤
            if ($("#ddl_ACCT_ID").val() == 'Y') {
                alert("已轉過部門傳票,不允再執行轉傳票");
                return false;
            }
            if (LookUpCheckboxs() > 0) {
                if (Page_ClientValidate("GroupC")) {
                    BlockUI();
                } else {
                    return false;
                }
            }
            else {
                alert("請最少選擇一筆資料");
                return false;
            }
        }

        //function CheckExecuteAction2() {
        //    if ($('#txt_DEPT_ACCT_ID').val()!="") {


        //    }

        //    else {
        //        alert("請輸入部門傳票號碼");
        //        return false;
        //    }
        //}
        //function CheckExecuteAction3() {
        //    if ($('#txt_DEPT_ACCT_ID').val() != "") { }

        //    else {
        //        alert("請最少選擇一筆資料");
        //        return false;
        //    }
        //}
        function ClearAll() {
            $('#txt_SALARY_DT').val("");
            $('#txt_EMP_ID').val("");
            $('#txt_EMP_NAME').val("");
            $('#txt_HOPE_PAT_DT_S').val("");
            $('#txt_HOPE_PAT_DT_E').val("");
            $('#txt_VENDOR_ID').val("");
            $('#txt_DEPT_ACCT_ID').val("");

            $('#ddl_ACCT_ID').val(-1);
            $('#ddl_SALARY_TYPE').val(-1);
            $('#ddl_SYS_ID').val(-1);
            $('#ddl_SYS_ID').val(-1);
            $('#ddl_SYS_ID').val(-1);
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
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidWFB2IB_Dtl_NotChoiceMessage').val());
                return false;
            }
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

        function printCheck(msg) {
            var processed = true;

            if (Page_ClientValidate("GroupB")) {
                //BlockUI();
            }
            else
                processed = false;

            if (processed) {
                processed = confirm("確定要進行" + msg);
                //BlockUI();
            }

            if (!processed)
                $.unblockUI();

            return processed;
        }

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="15%" />
                                <col width="35%" />
                                <col width="15%" />
                                <col width="35%" />

                            </colgroup>
                            <tbody>
                                <tr>
                                    <%--發薪日 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_SALARY_DT" runat="server" Text="<%$Resources:Resource,wfb2sf_lbl_SALARY_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_DT" runat="server" Width="100px" ClientIDMode="Static" CssClass="number2 date"></asp:TextBox>
                                         <%--
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sf_ERR_SALARY_DT2%>"
                                            ControlToValidate="txt_SALARY_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                                --%>
                                        <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"                                          
                                            ErrorMessage="<%$Resources:Resource,wfb2sf_ERR_SALARY_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_SALARY_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>

                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_START_YM" runat="server" Text="<%$Resources:Resource,wfb2sf_lbl_START_YM%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_SALARY_TYPE" runat="server" ClientIDMode="Static" CssClass=""></asp:DropDownList>
                                          <%--
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sf_ERR_SALARY_TYPE%>"
                                            ControlToValidate="ddl_SALARY_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    --%>
                                    </td>
                                </tr>
                                <tr>
                                    <%--轉傳票否 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_ACCT_ID" runat="server" Text="<%$Resources:Resource,wfb2sf_lbl_ACCT_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_ACCT_ID" runat="server" Width="100px" ClientIDMode="Static" CssClass="MandatoryField">
                                            <asp:ListItem Value="N" Selected="True">N-否</asp:ListItem>
                                            <asp:ListItem Value="Y">Y-是</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2sf_lbl_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                        <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', '', '');" />
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <%--希望匯款日 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_HOPE_PAT_DT" runat="server" Text="<%$Resources:Resource,wfb2sf_lbl_HOPE_PAT_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_HOPE_PAT_DT_S" runat="server" Width="100px" ClientIDMode="Static" CssClass="number2 date"></asp:TextBox>
                                        ~
                                        <asp:TextBox ID="txt_HOPE_PAT_DT_E" runat="server" Width="100px" ClientIDMode="Static" CssClass="number2 date"></asp:TextBox>

                                        <asp:CustomValidator ID="CustomValidatorHOPE_PAT_DT_S" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sf_ERR_HOPE_PAT_DT_S%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_HOPE_PAT_DT_S" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidatorHOPE_PAT_DT_E" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sf_ERR_HOPE_PAT_DT_E%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_HOPE_PAT_DT_E" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_DEPT_ACCT_ID" runat="server" Text="<%$Resources:Resource,wfb2sf_lbl_DEPT_ACCT_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DEPT_ACCT_ID" runat="server" Width="100px" ClientIDMode="Static" MaxLength="10"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <%--廠商CODE --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_VENDOR_ID" runat="server" Text="<%$Resources:Resource,wfb2sf_lbl_VENDOR_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_VENDOR_ID" runat="server" Width="100px" ClientIDMode="Static" MaxLength="10"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="onlyEngNum" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2sf_VENDOR_ID_Error%>" ControlToValidate="txt_VENDOR_ID" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_ACCT_ID" runat="server" Text="<%$Resources:Resource,wfb2sf_lb_ACCT_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_ACCT_ID" runat="server" Width="100px" ClientIDMode="Static" MaxLength="16"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <%--批號 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_Lno" runat="server" Text="<%$Resources:Resource,wfb2sf_lb_Lno%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_Lno" runat="server" Width="100px" ClientIDMode="Static" MaxLength="10"></asp:TextBox>                                       
                                    </td>  
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_IaDat" runat="server" Text="入帳日期"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_IaDat" runat="server" Width="81px" CssClass="date MandatoryField"  ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="請輸入入帳日期"
                                             ControlToValidate="txt_IaDat" ForeColor="Red" ValidationGroup="GroupC" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator6" runat="server" ValidateEmptyText="true"
                                             ErrorMessage="入帳日期格式錯誤" ClientValidationFunction="CheckDate" ForeColor="Red"
                                             ControlToValidate="txt_IaDat" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
                                    </td>                                
                                </tr>
                                <tr>
                                    <th></th>
                                    <th align="right" colspan="3">
                                        <aces:Btn ID="WFB2SF1300Search" runat="server" Text="<%$Resources:Resource,wfb2sf_WFB2SF1300Search%>" OnClick="WFB2SF130Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />
                                        <%--<asp:Button ID="WFB2SF1300Search" runat="server" Text="<%$Resources:Resource,wfb2sf_WFB2SF1300Search%>" OnClick="WFB2SF130Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();"/>--%>
                                        <input id="WFB2SF1300Clear" type="button" value="<%$Resources:Resource,btn_clear%>" runat="server" onclick="ClearAll();" />
                                        <aces:Btn ID="WFB2SF1300Delete" runat="server" Text="<%$Resources:Resource,wfb2sf_WFB2SF1300Delete%>" OnClick="WFB2SF1300Delete_Click" ValidationGroup="GroupB" />
                                        <%--<asp:Button ID="WFB2SF1300Delete" runat="server" Text="<%$Resources:Resource,wfb2sf_WFB2SF1300Delete%>" OnClick="WFB2SF1300Delete_Click" ValidationGroup="GroupB" />--%>                                        
                                    </th>
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
                            <aces:Btn ID="WFB2SF1300Execute" runat="server" Text="<%$Resources:Resource,wfb2sf_WFB2SF1300Execute%>" OnClick="WFB2SF1300Execute_Click" OnClientClick="return CheckExecuteAction1();" Visible="false" />
                            <aces:Btn ID="WFB2SF1300Print" runat="server" Text="<%$Resources:Resource,wfb2sf_WFB2SF1300Print%>" ValidationGroup="GroupB" OnClick="WFB2SF1300Print_Click" Visible="false" OnClientClick="return printCheck(this.value);" />

                            <%--<asp:Button ID="WFB2SF1300Execute" runat="server" Text="<%$Resources:Resource,wfb2sf_WFB2SF1300Execute%>" OnClick="WFB2SF1300Execute_Click" OnClientClick="return CheckExecuteAction1();"  Visible="false"/>
                            <asp:Button ID="WFB2SF1300Print" runat="server" Text="<%$Resources:Resource,wfb2sf_WFB2SF1300Print%>" ValidationGroup="GroupB"  OnClick="WFB2SF1300Print_Click" Visible="false" OnClientClick="return printCheck(this.value);"/>--%>
                        </div>

                    </td>
                </tr>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SF1300DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="ods1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_SALARY_DT"
                        Name="txt_SALARY_DT" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_SALARY_TYPE"
                        Name="ddl_SALARY_TYPE" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_ACCT_ID"
                        Name="ddl_ACCT_ID" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="txt_EMP_ID" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_HOPE_PAT_DT_S"
                        Name="txt_HOPE_PAT_DT_S" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_HOPE_PAT_DT_E"
                        Name="txt_HOPE_PAT_DT_E" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_VENDOR_ID"
                        Name="txt_VENDOR_ID" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_DEPT_ACCT_ID"
                        Name="txt_DEPT_ACCT_ID" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_ACCT_ID"
                        Name="txt_ACCT_ID" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_Lno"
                        Name="txt_Lno" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />

                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="true" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
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
                                <asp:Label ID="lbl_RowNumber" runat="server"  Width="40px" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                                <asp:Label ID="lbl_RowNumber" runat="server" Width="40px" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                                <asp:Label ID="lbl_NewRowNumber" runat="server" Width="40px" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%-- 發文字號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_DOC_NO%>" HeaderStyle-Width="140px"  SortExpression="DOC_NO" ItemStyle-HorizontalAlign="left">
                        <ItemTemplate>
                                <asp:Label ID="lbl_DOC_NO"  Width="140px" runat="server" Text='<%#Bind("DOC_NO")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <%-- 發薪日期--%>
                     <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_SALARY_DT%>" SortExpression="SALARY_DT" HeaderStyle-Width="50px">
                        <ItemTemplate>
                            <asp:Label Width="100px" Style="text-align: center;" ID="lb_SALARY_DT" runat="server" Text='<%#Bind("SALARY_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 工號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_EMP_ID%>"  HeaderStyle-Width="60px"   SortExpression="EMP_ID" ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                                <asp:Label ID="lbl_EMP_ID"  Width="60px" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 姓名--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_EMP_NAME%>"  HeaderStyle-Width="80px"  SortExpression="EMP_NAME" ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                                <asp:Label ID="lbl_EMP_NAME"  runat="server" Width="80px" Text='<%# Convert.ToString(Eval("EMP_NAME"))%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 償還金額--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_AMOUNT%>"  HeaderStyle-Width="80px"  SortExpression="AMOUNT" ItemStyle-HorizontalAlign="right">
                        <ItemTemplate>
                                <asp:Label ID="lbl_AMOUNT" runat="server"  Width="80px"  Text='<%#Bind("AMOUNT","{0:n0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 扣款項目--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_SALARY_NAME%>" HeaderStyle-Width="100px"  SortExpression="SALARY_NAME">
                        <ItemTemplate>
                                <asp:Label ID="lbl_SALARY_NAME" runat="server"  Width="100px"  Text='<%#Bind("SALARY_NAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 債權人--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_CREDITOR%>" SortExpression="CREDITOR"  HeaderStyle-Width="100px"  >
                        <ItemTemplate>
                                <asp:Label ID="lbl_CREDITOR" runat="server" Width="100px" Text='<%#Bind("CREDITOR")%>'></asp:Label>
                                  <%--支付對象--%>
                                 <asp:HiddenField ID="hid_PAY_TARGET" runat="server" Value='<%#Bind("PAY_TARGET")%> ' ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 廠商CODE--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_VENDOR_ID%>" SortExpression="VENDOR_ID">
                        <ItemTemplate>
                                <asp:Label ID="lbl_VENDOR_ID" runat="server"  Width="100px" Text='<%#Bind("VENDOR_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 希望匯款日--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_HOPE_PAT_DT%>" SortExpression="HOPE_PAT_DT"  HeaderStyle-Width="100px" >
                        <ItemTemplate>
                                <asp:TextBox ID="txt_HOPE_PAT_DT" runat="server" Width="100px"  MaxLength="10"  ClientIDMode="AutoID" CssClass="MandatoryField number2 date" Text='<%#Bind("HOPE_PAT_DT","{0:yyyy/MM/dd}")%>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 發生期間起--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_S_DT%>" SortExpression="S_DT"  HeaderStyle-Width="100px" >
                        <ItemTemplate>
                                <asp:TextBox ID="txt_S_DT" runat="server" MaxLength="10" Width="100px" ClientIDMode="AutoID" CssClass="MandatoryField number2 date" Text='<%#Bind("S_DT","{0:yyyy/MM/dd}")%>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 發生期間迄--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_E_DT%>" SortExpression="E_DT"  HeaderStyle-Width="100px" >
                        <ItemTemplate>
                                <asp:TextBox ID="txt_E_DT" runat="server" MaxLength="10"  Width="100px"  ClientIDMode="AutoID" CssClass="MandatoryField number2 date" Text='<%#Bind("E_DT","{0:yyyy/MM/dd}")%>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- 匯款方式--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_PAYMONEY_TYPE%>" SortExpression="PAYMONEY_TYPE"  HeaderStyle-Width="160px" >
                        <ItemTemplate>
                            <div style="width:160px">
                                <asp:Label ID="lbl_PAYMONEY_TYPE" runat="server" Text='<%#Bind("PAYMONEY_TYPE2")%>'  Width="160px" ></asp:Label>
                                <asp:RadioButtonList ID="rdo_PAYMONEY_TYPE" runat="server" RepeatDirection="Horizontal"></asp:RadioButtonList>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                   <%-- 傳票號碼--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_ACCT_ID%>" SortExpression="ACCT_ID"  HeaderStyle-Width="120px" >
                        <ItemTemplate>
                                <asp:Label ID="lbl_ACCT_ID" runat="server" Text='<%#Bind("ACCT_ID")%>'  Width="120px"></asp:Label>
                                <asp:HiddenField runat="server" ClientIDMode="Static" ID="hid_SEQ" Value='<%#Bind("SEQ")%>' />
                                <asp:HiddenField runat="server" ClientIDMode="Static" ID="hid_SALARY_DT" Value='<%#Bind("SALARY_DT")%>' />
                                <asp:HiddenField runat="server" ClientIDMode="Static" ID="hid_SALARY_TYPE" Value='<%#Bind("SALARY_TYPE")%>' />
                                <asp:HiddenField runat="server" ClientIDMode="Static" ID="hid_PAY_KIND" Value='<%#Bind("PAY_KIND")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 批號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_lb_Lno%>" SortExpression="Lno"  HeaderStyle-Width="100px" >
                        <ItemTemplate>
                                <asp:Label ID="lb_Lno" runat="server" Text='<%#Bind("Lno")%>'  Width="100px" ></asp:Label>                                
                        </ItemTemplate>
                    </asp:TemplateField>
                     <%-- 部門傳票號碼--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_DEPT_ACCT_ID%>" SortExpression="DEPT_ACCT_ID"  HeaderStyle-Width="100px" >
                        <ItemTemplate>
                                <asp:Label ID="lbl_DEPT_ACCT_ID" runat="server" Text='<%#Bind("DEPT_ACCT_ID")%>'  Width="100px" ></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>                    
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
                      
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />

            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_NotChoiceMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Mod_NotChoiceMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Save_ConfirmMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_ConfirmMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Dtl_NotChoiceMessage" />
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary3" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupC" ShowSummary="false" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SF1300Print"></asp:PostBackTrigger>
        </Triggers>

    </asp:UpdatePanel>
</asp:Content>


