<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sc/WFB2SC4100_Qry.aspx.cs" Inherits="WebContent_fb2sc_WFB2SC4100_Qry" %>

<%@ Register Src="~/UserControl/UCDateTimeRange.ascx" TagPrefix="uc1" TagName="UCDateTimeRange" %>
<%@ Register Src="~/UserControl/UCDateTextBoxRange.ascx.ascx" TagPrefix="uc1" TagName="UCDateTextBoxRangeascx" %>
<%@ Register Src="~/UserControl/UCCommCodeDropDwonList.ascx" TagPrefix="uc1" TagName="UCCommCodeDropDwonList" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            //$.mask.definitions['~'] =' [1 - 12]';
            $('#txtSALARY_YM').datepicker({ dateFormat: 'yy/mm' });
            $('#txtSALARY_YM').mask('9999/99');
            $('.ymd').mask('9999/99/99');
            iniForm();
        });
        function iniForm() {
            //工號取得姓名的ajax
            $("#txt_EMP_ID").blur(function () {
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
                                $('#txt_EMP_NAME').val(JData.EMP_NAME);
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
            $("#txt_DEPT_NO").blur(function () {
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
                                $('#txt_DEPT_DESC').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_DEPT_DESC').val(JData.DEPT_NAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_DEPT_DESC').val("");
                }
            });
            gridviewScroll();
            $.unblockUI();
        }

        function Clear() {
            $("#txt_DEPT_NO").val("");
            $("#txt_DEPT_DESC").val("");
            $("#txtSALARY_YM").val("");
            $("#txt_LEAVE_DT_S").val("");
            $("#txt_LEAVE_DT_E").val("");
            $("#ddlCommCode").val("");
            $("#txt_EMP_ID").val($("#hid_defalut_EMP_ID").val());
            $("#txt_EMP_NAME").val($("#hid_defalut_EMP_NAME").val());
        }



        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
        }
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"
            });
        }
        function CheckDelAction() {
            if (LookUpCheckboxs() != 1) {
                alert($('#hidwfb2sc_DelNotChoiceMessage').val());
                return false;
            }
            else
                BlockUI();
        }
        function CheckPrintAction() {
            if (LookUpCheckboxs() != 1) {
                alert($('#hidwfb2sc_DelNotChoiceMessage').val());
                return false;
            }
            else {
                //BlockUI();
                //var ItemCheckBoxs = $("[type=checkbox]");
                //var SALARY_TYPE = $("[id=hidSALARY_TYPE]")[CheckCheckIndex-5].value;
                ////var SALARY_TYPE = ItemCheckBoxs[CheckCheckIndex].parentNode.parentNode.querySelector('#hidSALARY_TYPE').value;
                //var IS_SUPER = $('#hidIsSuper').val();
                //var SALARY_DT = ItemCheckBoxs[CheckCheckIndex].parentNode.parentNode.querySelector('#lblSALARY_DT').innerText;
                //var EMP_ID = ItemCheckBoxs[CheckCheckIndex].parentNode.parentNode.querySelector('#lblEMP_ID').innerText;
                //var PAY_KIND = ItemCheckBoxs[CheckCheckIndex].parentNode.parentNode.querySelector('#hidPAY_KIND').value;
                //var SALARY_EMAIL = ItemCheckBoxs[CheckCheckIndex].parentNode.parentNode.querySelector('#hidSALARY_EMAIL').value;
                //window.open("WFB2SC4100_Detail1.aspx?IS_SUPER=" + encodeURI(IS_SUPER)
                //                                 + "&SALARY_TYPE=" + encodeURI(SALARY_TYPE)
                //                                 + "&SALARY_DT=" + encodeURI(SALARY_DT)
                //                                 + "&EMP_ID=" + encodeURI(EMP_ID)
                //                                 + "&PAY_KIND=" + encodeURI(PAY_KIND)
                //                                 + "&SALARY_EMAIL=" + encodeURI(SALARY_EMAIL)
                //                                 + "&PDF=Y");
                //return false;
            }
        }
        var CheckCheckIndex = -1;
        function LookUpCheckboxs() {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked && ItemCheckBoxs[i].id.indexOf("cb_all") == -1) {
                    HaveCheck++;
                    CheckCheckIndex = i;
                }
            }
            return HaveCheck;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <table width="1020px" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table width="100%">
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lb_SALARY_YM" Text="<%$Resources:Resource,wfb2sc_SALARY_YM%>" ClientIDMode="Static" />
                                </th>
                                <td>
                                    <asp:TextBox ID="txtSALARY_YM" runat="server" ClientIDMode="Static" Width="70" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                        ErrorMessage="薪資年月格式錯誤" ControlToValidate="txtSALARY_YM" ForeColor="Red" ValidationGroup="GroupA"
                                        ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                </td>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lb_SALARY_DT" Text="<%$Resources:Resource,wfb2sc_SALARY_DT_Range%>" />
                                </th>
                                <td>
                                    <uc1:UCDateTimeRange runat="server" ID="UC_SALARY_DT" StartDateMaxLength="10" ClientIDMode="Static" ControlClientIDMode="Static" EndDateMaxLength="10" EndDateCssClass="ymd" StartDateCssClass="ymd"
                                        CompareValidatorOperator="GreaterThanEqual" CompareValidatorErrMesg="發薪日期區間(迄)不能小於發薪日期區間(起)" ValidatorStartDateErrMesg="發薪日期區間(起)格式錯誤"
                                        ValidatorEndDateErrMesg="發薪日期區間(迄)格式錯誤" ClientValidationFunctionE="CheckDate" ClientValidationFunctionS="CheckDate" ValidationGroup="GroupA" />
                                    <%--<uc1:UCDateTimeRange
                                        runat="server"
                                        ID="UC_SALARY_DT"
                                        ClientIDMode="Static" />--%>
                                </td>
                            </tr>
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lb_EMP_NO" Text="<%$Resources:Resource,wfb2sc_EMP_NO%>" />
                                </th>
                                <td>
                                    <asp:TextBox ID="txt_EMP_ID" runat="server" ClientIDMode="Static" MaxLength="5" Width="50" />
                                    <%--OnTextChanged="txt_EMP_ID_TextChanged" AutoPostBack="true"--%>
                                    <asp:Button runat="server" ID="btn_EMP_ID" Text="..." ClientIDMode="Static" />
                                </td>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="Label1" Text="<%$Resources:Resource,wfb2sc_EMP_NAME%>" />
                                </th>
                                <td>
                                    <asp:TextBox ID="txt_EMP_NAME" runat="server" ClientIDMode="Static" />
                                </td>
                            </tr>
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lb_DEPT_NO" Text="<%$Resources:Resource,wfb2sc_DEPT_NO%>" />
                                </th>
                                <td>
                                    <asp:TextBox ID="txt_DEPT_NO" runat="server" ClientIDMode="Static" MaxLength="7" Width="70" />
                                    <%--OnTextChanged="txt_DEPT_NO_TextChanged" AutoPostBack="true"--%>
                                    <asp:Button runat="server" ID="btn_DEPT_NO" Text="..." ClientIDMode="Static" />
                                    <asp:TextBox ID="txt_DEPT_DESC" runat="server" BorderWidth="0" Enabled="false" ClientIDMode="Static" Style="background-color: white; color: black;"></asp:TextBox>
                                </td>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lb_EMP_CHG_CD" Text="<%$Resources:Resource,wfb2sc_EMP_CHG_CD%>" />
                                </th>
                                <td>
                                    <uc1:UCCommCodeDropDwonList runat="server"
                                        ID="uc_EMP_CHG_CD"
                                        DataTextField="SUB_CD,SUB_DESC"
                                        DataValueField="SUB_CD"
                                        SYS_CDs="HB"
                                        MAIN_CDs="EMP_CHG_CD"
                                        IS_VALID="True"
                                        DataTextFormatString="{0} - {1}"
                                        OrderSeq="ASC"
                                        FirstItem="<%$Resources:Resource,wfb2sc_dll_PlaceChoice%>"
                                        ClientIDMode="Static" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <aces:Btn runat="server" ID="WFB2SC4100Search" Text="<%$Resources:Resource,wfb2sc_WFB2SC4100Search%>" OnClick="WFB2SC4100Search_Click" ClientIDMode="Static" ValidationGroup="GroupA" OnClientClick="CheckValid();" />
                        <aces:Btn runat="server" ID="WFB2SC4100Detail1" Text="<%$Resources:Resource,wfb2sc_WFB2SC4100Detail1%>" OnClick="WFB2SC4100Detail1_Click" ClientIDMode="Static" OnClientClick="return CheckDelAction();" />
                        <aces:Btn runat="server" ID="WFB2SC4100Print" Text="<%$Resources:Resource,wfb2sc_WFB2SC4100Print%>" ClientIDMode="Static" OnClick="WFB2SC4100Print_Click" OnClientClick="CheckPrintAction();" />

                        <%--<asp:Button runat="server" ID="WFB2SC4100Search" Text="<%$Resources:Resource,wfb2sc_WFB2SC4100Search%>" OnClick="WFB2SC4100Search_Click" ClientIDMode="Static" ValidationGroup="GroupA" OnClientClick="CheckValid();" />
                        <asp:Button runat="server" ID="WFB2SC4100Detail1" Text="<%$Resources:Resource,wfb2sc_WFB2SC4100Detail1%>" OnClick="WFB2SC4100Detail1_Click" ClientIDMode="Static" OnClientClick="return CheckDelAction();" />
                        <asp:Button runat="server" ID="WFB2SC4100Print" Text="<%$Resources:Resource,wfb2sc_WFB2SC4100Print%>" ClientIDMode="Static" OnClick="WFB2SC4100Print_Click" OnClientClick="CheckPrintAction();" />--%>
                        <asp:Button runat="server" ID="btn_Grant_Confim_later" ClientIDMode="Static" Style="display: none" />
                        <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2sc_Clear%>" OnClientClick="Clear();return false;" />

                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="txt_EMP_ID" />
            <asp:PostBackTrigger ControlID="btn_EMP_ID" />
            <asp:PostBackTrigger ControlID="txt_DEPT_NO" />
            <asp:PostBackTrigger ControlID="btn_DEPT_NO" />
            <asp:PostBackTrigger ControlID="WFB2SC4100Search" />
            <asp:PostBackTrigger ControlID="WFB2SC4100Detail1" />
            <asp:PostBackTrigger ControlID="WFB2SC4100Print" />
            <asp:PostBackTrigger ControlID="btn_Grant_Confim_later" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="GetGridData"
                SelectCountMethod="GetGridDataCount" TypeName="WFB2SC4100BO" EnablePaging="True" SortParameterName="sortExpression"
                OnSelected="ods1_Selected" OnSelecting="obs1_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="hidIsSuper"
                        Name="strIsSuper" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txtSALARY_YM"
                        Name="SALARY_YM" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="UC_SALARY_DT"
                        Name="SALARY_DT_S" PropertyName="StartDateText" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="UC_SALARY_DT"
                        Name="SALARY_DT_E" PropertyName="EndDateText" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="QryEMP_ID" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_EMP_NAME"
                        Name="EMP_NAME" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_DEPT_NO"
                        Name="DEPT_NO" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="uc_EMP_CHG_CD"
                        Name="EMP_CHG_CD" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound" meta:resourcekey="gv_resultResource1">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="30px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" meta:resourcekey="cb_checkallResource1" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" meta:resourcekey="cb_checkResource1" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2da_RowNumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="RowNumber" Text='<%#Bind("RowNumber")%>' runat="server" Width="97%" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--發薪類別 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_SALARY_TYPE%>" SortExpression="SALARY_TYPE" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:HiddenField ID="hidSALARY_TYPE" Value='<%#Bind("SALARY_TYPE")%>' runat="server" ClientIDMode="static" />
                            <asp:HiddenField ID="hidPAY_KIND" Value='<%#Bind("PAY_KIND")%>' runat="server" ClientIDMode="static" />
                            <asp:HiddenField ID="hidSALARY_EMAIL" Value='<%#Bind("SALARY_EMAIL")%>' runat="server" ClientIDMode="static" />
                            <asp:Label ID="lblSALARY_TYPE" runat="server" Width="97%" ClientIDMode="static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--發放項目 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_PAY_KIND%>" SortExpression="PAY_KIND" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblPAY_KIND" Text='<%#Bind("PAY_KIND_DESC")%>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                     <%--薪資年月 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_SALARY_YM%>" SortExpression="SALARY_YM" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblSALARY_YM" Text='<%#Bind("SALARY_YM")%>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--發薪日 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_hd_SALARY_DT%>" SortExpression="SALARY_DT" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblSALARY_DT" Text='<%#Bind("SALARY_DT")%>' runat="server" ClientIDMode="static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblEMP_ID" Text='<%#Bind("EMP_ID")%>' runat="server" ClientIDMode="static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblEMP_NAME" Text='<%#Bind("EMP_NAME")%>' runat="server" Width="97%" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--實際匯款日期 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_REMIT_DT%>" SortExpression="REMIT_DT" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblREMIT_DT" Text='<%#Bind("REMIT_DT")%>' runat="server" Width="97%" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:TextBox ID="txtREMIT_DT_Edit" runat="server" Text='<%#Bind("REMIT_DT")%>' Width="97%" />
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:TextBox ID="txtREMIT_DT_Edit" Text="" runat="server" Width="97%" />
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
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
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SC4100Search"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="WFB2SC4100Detail1"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="WFB2SC4100Print"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="btn_Grant_Confim_later"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="btn_clear"></asp:PostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
    <!--預設的工號,姓名-->
    <asp:HiddenField ID="hid_defalut_EMP_ID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hid_defalut_EMP_NAME" runat="server" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidIsSuper" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_DelNotChoiceMessage" Value="<%$Resources:Resource,wfb2sc_DelNotChoiceMessage%>" />
</asp:Content>
