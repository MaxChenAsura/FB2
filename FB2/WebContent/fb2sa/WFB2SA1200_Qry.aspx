<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sa/action/WFB2SA1200_Qry.aspx.cs" Inherits="WebContent_fb2sa_WFB2SA1200_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<%@ Register Src="~/UserControl/UCCommCodeDropDwonList.ascx" TagPrefix="uc1" TagName="UCCommCodeDropDwonList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".year").mask('9999');
            //$("#txt_GRADE_CD").mask('9');
            gridviewScroll();
            $.unblockUI();

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
        //function EDUCATION_CD_Choose() {
        //    $("#HID_EDUCATION_CD").val($("#ddl_EDUCATION_CD").val());
        //}
        //function WS_CD_Choose() {
        //    $("#HID_WS_CD").val($("#ddl_WS_CD").val());
        //}


        //清空
        function doClear() {
            var now = new Date();
            var Current = now.getFullYear();
            $("#txt_DATA_YEAR").val(Current);
            $("#txt_LEVEL_CD").val("");
            $("#txt_GRADE_CD").val("");
            $("#txt_GRADE_YEAR").val("");
            $("#ddl_EDUCATION_CD").val("-1");
            $("#ddl_WS_CD").val("-1");


            return false;
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
                        <!--查詢條件table(HTML)-->
                        <!--TextBox,input button,DropDownList-->
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="15%" />
                                <col width="20%" />
                                <col width="15%" />
                                <col width="20%" />
                                <col width="15%" />
                                <col width="15%" />


                            </colgroup>
                            <tbody>

                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DATA_YEAR" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_DATA_YEAR1%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DATA_YEAR" runat="server" MaxLength="4" Width="64px" ClientIDMode="Static" CssClass="MandatoryField year"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sa_required_DATA_YEAR1%>"
                                            ControlToValidate="txt_DATA_YEAR" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2sa_error_DATA_YEAR1%>" ControlToValidate="txt_DATA_YEAR" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="\d\d\d\d" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EDUCATION_CD" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_EDUCATION_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_EDUCATION_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LEVEL_CD" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_LEVEL_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_LEVEL_CD" runat="server" MaxLength="3" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="onlyEngNum" runat="server"
                                            ErrorMessage="資格只能輸入英數字" ControlToValidate="txt_LEVEL_CD" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>

                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_GRADE_CD" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_GRADE_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_GRADE_CD" runat="server" MaxLength="1" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_WS_CD" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_WS_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_WS_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_GRADE_YEAR" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_GRADE_YEAR%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_GRADE_YEAR" runat="server" MaxLength="4" Width="64px" ClientIDMode="Static" CssClass="year"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2sa_error_GRADE_YEAR%>" ControlToValidate="txt_GRADE_YEAR" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="\d\d\d\d" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>

                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SA1200Search" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA1200Search%>" OnClick="WFB2SA1200Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />

                                            <%--<asp:Button ID="WFB2SA1200Search" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA1200Search%>" OnClick="WFB2SA1200Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />--%>
                                            <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2ia_btn_clear%>" OnClientClick="return doClear();" />
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

            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="GetData"
                SelectCountMethod="GetCount" TypeName="CFB2SA1200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">

                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_DATA_YEAR" DefaultValue=""
                        Name="data_year" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_EDUCATION_CD" DefaultValue=""
                        Name="education_cd" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_LEVEL_CD" DefaultValue=""
                        Name="level_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_GRADE_CD" DefaultValue=""
                        Name="grade_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_WS_CD" DefaultValue=""
                        Name="ws_cd" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_GRADE_YEAR" DefaultValue=""
                        Name="grade_year" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>


            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">

                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2si_RowNum%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_WS_CD%>" SortExpression="WS_CD">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_WS_CD" runat="server" Text='<%#Bind("WS_CD")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_LEVEL_CD1%>" SortExpression="LEVEL_CD">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_LEVEL_CD" runat="server" Text='<%#Bind("LEVEL_CD")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_GRADE_CD%>" SortExpression="GRADE_CD">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_GRADE_CD" runat="server" Text='<%#Bind("GRADE_CD")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_EDUCATION_CD%>" SortExpression="EDUCATION_CD">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_EDUCATION_CD" runat="server" Text='<%#Bind("EDUCATION_CD")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_GRADE_YEAR%>" SortExpression="GRADE_YEAR">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_GRADE_YEAR" runat="server" Text='<%#Bind("GRADE_YEAR","{0:yyyy}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_LEVEL_PAY1%>" SortExpression="LEVEL_PAY1">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_LEVEL_PAY1" runat="server" Text='<%#Bind("LEVEL_PAY1","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_LEVEL_PAY2%>" SortExpression="LEVEL_PAY2">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_LEVEL_PAY2" runat="server" Text='<%#Bind("LEVEL_PAY2","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_LEVEL_PAY3%>" SortExpression="LEVEL_PAY3">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_LEVEL_PAY3" runat="server" Text='<%#Bind("LEVEL_PAY3","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_EFFECT_SDT%>" SortExpression="EFFECT_SDT">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_EFFECT_SDT" runat="server" Text='<%#Bind("EFFECT_SDT","{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_EFFECT_EDT%>" SortExpression="EFFECT_EDT">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_EFFECT_EDT" runat="server" Text='<%#Bind("EFFECT_EDT","{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <EmptyDataTemplate>

                    <table class="grid-view" width="1020px">
                        <tr class="header">
                            <td>
                                <asp:CheckBox ID="cb_checkall" runat="server" onclick="javascript:SelectAllCheckboxes(this);" />
                            </td>
                            <td>
                                <asp:Label ID="Label" runat="server" Text="<%$Resources:Resource,wfb2si_RowNum%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Labe2" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_WS_CD%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Labe3" runat="server" Text="<%$Resources:Resource,wfb2sa_LEVEL_CD1%>"></asp:Label>
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
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_LEVEL_PAY2%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_LEVEL_PAY3%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_EFFECT_SDT%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_EFFECT_EDT%>"></asp:Label>
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

            <!--紀錄頁數(跳頁用)-->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <%--<asp:HiddenField ID="HID_EDUCATION_CD" runat="server" ClientIDMode="Static"/>
            <asp:HiddenField ID="HID_WS_CD" runat="server" ClientIDMode="Static"/>--%>

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <%--<asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />--%>
            <!--resource-->
            <%--<asp:HiddenField ID="hid_wfb2ia_Dtl_NotChoiceMessage" runat="server" ClientIDMode="Static" />--%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


