<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sa/action/WFB2SA1300_Dtl.aspx.cs" Inherits="WebContent_fb2sa_WFB2SA1300_Dtl" Culture="auto" UICulture="auto" %>

<%@ Register Src="~/UserControl/UCDateTimeRange.ascx" TagPrefix="uc1" TagName="UCDateTimeRange" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        var li_id;
        function iniForm() {
            $("#ddlPerPageRow").css("width","85px").css("Font-Size", "14px");
            //$("#ddlPerPageRow").css("font-family", "Times New Roman, Times, serif");
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.ym').mask('9999/99/99');
            //$(".number").formatNumber({ format: "#,###", locale: "tw" });
            //gridviewScroll();
            $.unblockUI();
            $("#tabs").tabs();
            $('#ul li').click(function () {
                li_id = $(this).attr('id');
            });
            ChangeTab(li_id);

        }

        function ChangeTab(li_id) {
            $("#tabs").tabs({ active: li_id });
        }


        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            $("#HID_PageRow2").val($("#ddlPerPageRow2").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"
            });
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

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!--頁面table-->
            <table width="1020px" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <!--查詢條件table(HTML)-->
                        <!--TextBox,input button,DropDownList-->
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="10%" />
                                <col width="30%" />
                                <col width="10%" />
                                <col width="30%" />
                                <col width="20%" />

                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DATA_YEAR" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_DATA_YEAR1%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DATA_YEAR" runat="server" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PROCESS_STATUS" runat="server" Text="<%$Resources:Resource,wfb2sa_PROCESS_STATUS%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_PROCESS_STATUS" runat="server" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_START_DT" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_STARTandEND_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <uc1:UCDateTimeRange runat="server" ID="UCDateTimeRange" StartDateMaxLength="10" ClientIDMode="Static" ControlClientIDMode="Static" EndDateMaxLength="10" EndDateCssClass="MandatoryField ym" StartDateCssClass="MandatoryField ym"
                                            CompareValidatorOperator="GreaterThanEqual" CompareValidatorErrMesg="<%$Resources:Resource,wfb2sa_greaterthan_DT%>" ValidatorStartDateErrMesg="<%$Resources:Resource,wfb2sa_error_DT_start%>"
                                             EndDateRequire="true" EndDateRequireMsg="生效日期迄不可空白" StartDateRequire="true" StartDateRequireMsg="生效日期起不可空白"
                                            ValidatorEndDateErrMesg="<%$Resources:Resource,wfb2sa_error_DT_end%>" ClientValidationFunctionE="CheckDate" ClientValidationFunctionS="CheckDate" ValidationGroup="GroupA" />
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_RELEASE_BY" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_RELEASE_BY%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_RELEASE_BY" runat="server" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_RELEASE_DT" runat="server" Text="<%$Resources:Resource,wfb2sa_RELEASE_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_RELEASE_DT" runat="server" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_APPROVE_STATUS" runat="server" Text="<%$Resources:Resource,wfb2sa_APPROVE_STATUS%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_APPROVE_STATUS" runat="server" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_APPROVE_BY" runat="server" Text="<%$Resources:Resource,wfb2sa_APPROVE_BY%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_APPROVE_BY" runat="server" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_APPROVE_DT" runat="server" Text="<%$Resources:Resource,wfb2sa_APPROVE_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_APPROVE_DT" runat="server" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2sa_REMARK%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:TextBox ID="txt_REMARK" runat="server" Width="900px" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <asp:Button ID="btn_cancel" runat="server" Text="<%$Resources:Resource,wfb2sa_cancel%>" OnClick="btn_cancel_Click" OnClientClick="BlockUI();" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>

                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>

                    <td align="right" class="Body_label">

                        <div id="init_grid">
                            <%--<asp:Btn ID="WFB2SA1300Release" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA1300Release%>" OnClick="WFB2SA1300Release_Click" Visible="true" ValidationGroup="GroupA" OnClientClick="CheckValid();" />--%>

                            <asp:Button ID="WFB2SA1300Release" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA1300Release%>" OnClick="WFB2SA1300Release_Click" Visible="true" ValidationGroup="GroupA" OnClientClick="CheckValid();" />
                        </div>

                    </td>
                </tr>
            </table>

            <!-- 頁籤 開始-------------------  -->
            <div id="tabs" style="width: 1020px">
                <ul id="ul">
                    <li id="0"><a href="#Result">【初任薪結果】</a></li>
                    <li id="1"><a href="#Set">【設定條件】</a></li>
                </ul>
                <br />
                <!--<div id="tabCon" align="left"></div>-->
                <!--初任薪結果頁籤資料的GIRD-->
                <div id="Result" align="left">
                    <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="GetDtlData"
                        SelectCountMethod="GetDtlCount" TypeName="CFB2SA1300DAO" EnablePaging="True"
                        SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                        StartRowIndexParameterName="startRowIndex"
                        OnSelected="ods1_Selected">

                        <SelectParameters>
                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                            <asp:Parameter Name="maximumRows" Type="Int32" />
                            <asp:ControlParameter ControlID="txt_DATA_YEAR" DefaultValue=""
                                Name="data_year" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                        </SelectParameters>
                    </asp:ObjectDataSource>


                    <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                        AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting"
                        OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                        OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">

                        <Columns>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_approve_mark%>" SortExpression="APPROVE_MARK">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                                    <asp:HiddenField ID="hid_APPROVE_MARK" runat="server" ClientIDMode="Static" Value='<%#Bind("APPROVE_MARK")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2sk_RowNum%>" HeaderStyle-Width="40px">
                                <ItemTemplate>
                                    <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_WS_CD%>" SortExpression="WS_CD">
                                <ItemTemplate>
                                    <asp:Label Width="100%" Style="text-align: left;" ID="lb_WS_CD" ClientIDMode="Static" runat="server" Text='<%#Bind("WS_CD")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_LEVEL_CD1%>" SortExpression="LEVEL_CD">
                                <ItemTemplate>
                                    <asp:Label Width="100%" Style="text-align: left;" ID="lb_LEVEL_CD" runat="server" Text='<%#Bind("LEVEL_CD")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_GRADE_CD%>" SortExpression="GRADE_CD" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:Label Width="100%" Style="text-align: left;" ID="lb_GRADE_CD" ClientIDMode="Static" runat="server" Text='<%#Bind("GRADE_CD")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_EDUCATION_CD%>" SortExpression="EDUCATION_CD">
                                <ItemTemplate>
                                    <asp:Label Width="100%" Style="text-align: left;" ID="lb_EDUCATION_CD" runat="server" ClientIDMode="Static" Text='<%#Bind("EDUCATION_CD")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_GRADE_YEAR%>">
                                <ItemTemplate>
                                    <asp:Label Width="100%" Style="text-align: left;" ID="lb_GRADE_YEAR" ClientIDMode="Static" runat="server" Text='<%#Bind("GRADE_YEAR")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_LEVEL_PAY1%>">
                                <ItemTemplate>
                                    <asp:Label Width="100%" Style="text-align: right;" ID="lb_LEVEL_PAY1" ClientIDMode="Static" runat="server" Text='<%#Bind("LEVEL_PAY1","{0:N0}")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_LEVEL_PAY2%>">
                                <ItemTemplate>
                                    <asp:Label Width="100%" Style="text-align: right;" ID="lb_LEVEL_PAY2" ClientIDMode="Static" runat="server" Text='<%#Bind("LEVEL_PAY2","{0:N0}")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_LEVEL_PAY3%>">
                                <ItemTemplate>
                                    <asp:Label Width="100%" Style="text-align: right;" ID="lb_LEVEL_PAY3" ClientIDMode="Static" runat="server" Text='<%#Bind("LEVEL_PAY3","{0:N0}")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>

                        <EmptyDataTemplate>

                            <table class="grid-view" width="1020px">
                                <tr class="header">
                                    <td>
                                        <asp:Label ID="Label10" runat="server" Text="<%$Resources:Resource,wfb2sa_APPROVE_MARK%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2sk_RowNum%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_WS_CD%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2sa_LEVEL_CD1%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label17" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_GRADE_CD%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_EDUCATION_CD%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_GRADE_YEAR%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_LEVEL_PAY1%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_LEVEL_PAY2%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_LEVEL_PAY3%>"></asp:Label>
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
                                <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static" AutoPostBack="true" Font-Size="14px">
                                    <asp:ListItem Text="每頁10筆" Value="10"></asp:ListItem>
                                    <asp:ListItem Text="每頁20筆" Value="20"></asp:ListItem>
                                    <asp:ListItem Text="每頁30筆" Value="30"></asp:ListItem>
                                    <asp:ListItem Text="每頁40筆" Value="40"></asp:ListItem>
                                    <asp:ListItem Text="每頁50筆" Value="50"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 5px"></td>
                            <td style="width: 100%; font-size: 14px;">
                                <asp:Label ID="lb_TotalCount" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <!--設定條件頁籤資料的GIRD-->
                <div id="Set" align="left">
                    <asp:ObjectDataSource ID="ods2" runat="server" SelectMethod="GetDtlData2"
                        SelectCountMethod="GetDtlCount2" TypeName="CFB2SA1300DAO" EnablePaging="True"
                        SortParameterName="sortExpression2" OnSelecting="obs1_Selecting2"
                        StartRowIndexParameterName="startRowIndex"
                        OnSelected="ods1_Selected2">

                        <SelectParameters>
                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                            <asp:Parameter Name="maximumRows" Type="Int32" />
                            <asp:ControlParameter ControlID="txt_DATA_YEAR" DefaultValue=""
                                Name="data_year" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                        </SelectParameters>
                    </asp:ObjectDataSource>


                    <asp:GridView ID="gv_result2" runat="server" AllowPaging="True" AllowSorting="true"
                        AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting2"
                        OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated2" Width="1020px"
                        OnPageIndexChanging="gv_result_PageIndexChanging2" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound2">

                        <Columns>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_APPROVE_MARK%>" SortExpression="APPROVE_MARK">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                                    <asp:HiddenField ID="hid_APPROVE_MARK" runat="server" ClientIDMode="Static" Value='<%#Bind("APPROVE_MARK")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2sk_RowNum%>" HeaderStyle-Width="40px">
                                <ItemTemplate>
                                    <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_WS_CD%>" SortExpression="WS_CD">
                                <ItemTemplate>
                                    <asp:Label Width="100%" Style="text-align: left;" ID="lb_WS_CD" ClientIDMode="Static" runat="server" Text='<%#Bind("WS_CD")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_LEVEL_CD1%>" SortExpression="LEVEL_CD">
                                <ItemTemplate>
                                    <asp:Label Width="100%" Style="text-align: left;" ID="lb_LEVEL_CD" runat="server" Text='<%#Bind("LEVEL_CD")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_GRADE_CD%>" SortExpression="GRADE_CD" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:Label Width="100%" Style="text-align: left;" ID="lb_GRADE_CD" ClientIDMode="Static" runat="server" Text='<%#Bind("GRADE_CD")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_EDUCATION_CD%>" SortExpression="EDUCATION_CD">
                                <ItemTemplate>
                                    <asp:Label Width="100%" Style="text-align: left;" ID="lb_EDUCATION_CD" runat="server" ClientIDMode="Static" Text='<%#Bind("EDUCATION_CD")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_START_SALARY%>" SortExpression="START_SALARY">
                                <ItemTemplate>
                                    <asp:Label Width="100%" Style="text-align: right;" ID="lb_START_SALARY" ClientIDMode="Static" runat="server" Text='<%#Bind("START_SALARY","{0:N0}")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_BASE_YEAR%>" SortExpression="BASE_YEAR">
                                <ItemTemplate>
                                    <asp:Label Width="100%" Style="text-align: left;" ID="lb_BASE_YEAR" ClientIDMode="Static" runat="server" Text='<%#Bind("BASE_YEAR")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_START_YEAR%>" SortExpression="START_YEAR">
                                <ItemTemplate>
                                    <asp:Label Width="100%" Style="text-align: left;" ID="lb_START_YEAR" ClientIDMode="Static" runat="server" Text='<%#Bind("START_YEAR")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_END_YEAR%>" SortExpression="END_YEAR">
                                <ItemTemplate>
                                    <asp:Label Width="100%" Style="text-align: left;" ID="lb_END_YEAR" ClientIDMode="Static" runat="server" Text='<%#Bind("END_YEAR")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_BASE_RANGE%>" SortExpression="BASE_RANGE">
                                <ItemTemplate>
                                    <asp:Label Width="100%" Style="text-align: right;" ID="lb_BASE_RANGE" ClientIDMode="Static" runat="server" Text='<%#Bind("BASE_RANGE","{0:N0}")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_FEMALE_RANGE%>" SortExpression="FEMALE_RANGE">
                                <ItemTemplate>
                                    <asp:Label Width="100%" Style="text-align: right;" ID="lb_FEMALE_RANGE" ClientIDMode="Static" runat="server" Text='<%#Bind("FEMALE_RANGE","{0:N0}")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_ARMY_RANGE%>" SortExpression="ARMY_RANGE">
                                <ItemTemplate>
                                    <asp:Label Width="100%" Style="text-align: right;" ID="lb_ARMY_RANGE" ClientIDMode="Static" runat="server" Text='<%#Bind("ARMY_RANGE","{0:N0}")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>

                        <EmptyDataTemplate>

                            <table class="grid-view" width="1020px">
                                <tr class="header">
                                    <td>
                                        <asp:Label ID="Label10" runat="server" Text="<%$Resources:Resource,wfb2sa_APPROVE_MARK%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2sk_RowNum%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_WS_CD%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2sa_LEVEL_CD1%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label17" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_GRADE_CD%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_EDUCATION_CD%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_START_SALARY%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_BASE_YEAR%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_START_YEAR%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_END_YEAR%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_BASE_RANGE%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label11" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_FEMALE_RANGE%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label12" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_ARMY_RANGE%>"></asp:Label>
                                    </td>
                                </tr>
                            </table>

                        </EmptyDataTemplate>
                        <PagerStyle CssClass="GridviewScrollPager" />
                        <FooterStyle CssClass="GridviewScrollPager" />
                    </asp:GridView>
                    <table id="OnePage2" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                        <tr height="100%" valign="top">
                            <td class="GridviewScrollPager TD">
                                <asp:DropDownList ID="ddlPerPageRow2" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static" AutoPostBack="true" Font-Size="14px">
                                    <asp:ListItem Text="每頁10筆" Value="10"></asp:ListItem>
                                    <asp:ListItem Text="每頁20筆" Value="20"></asp:ListItem>
                                    <asp:ListItem Text="每頁30筆" Value="30"></asp:ListItem>
                                    <asp:ListItem Text="每頁40筆" Value="40"></asp:ListItem>
                                    <asp:ListItem Text="每頁50筆" Value="50"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 5px"></td>
                            <td style="width: 100%; font-size: 14px;">
                                <asp:Label ID="lb_TotalCount2" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <!--紀錄頁數(跳頁用)-->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_PageRow2" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
            <!--resource)-->
            <%--<asp:HiddenField ID="hid_wfb2ia_Del_ConfirmMessage" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2ia_Del_NotChoiceMessage" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2ia_Mod_NotChoiceMessage" runat="server" ClientIDMode="Static" />--%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
