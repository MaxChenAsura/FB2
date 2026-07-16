<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2ia/action/WFB2IA3100_Qry.aspx.cs" Inherits="WebContent_fb2ia_WFB2IA3100_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm' });
            $(".ym").mask('9999/99');
            gridviewScroll();
            $.unblockUI();
            $('#txt_COMPANY_NAME').attr("readonly", true);
        }


        function ShowRecord(obj) {
            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        //凍結視窗
        function gridviewScroll() {
            if ($("#HID_Freeze").val() == "Y") {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "500",
                    barcolor: "#7F7F7F",
                    headerrowcount: 2,
                    freezesize: 5

                });
                CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
            }

        }
        //function BILLS_KIND_Choose() {
        //    $("#HID_BILLS_KIND").val($("#ddl_BILLS_KIND").val());
        //}
        function ddlCheck(source, arguments) {
            if ($("#ddl_BILLS_KIND").val() == "-1")
                arguments.IsValid = false;
            else
                arguments.IsValid = true;

        }
        function Company_Search(id, name, value) {
            var returnValue = value;
            if (value == undefined) {
                returnValue = 'undefined';
            }
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                $("#" + id).val(obj.CD);
                $("#" + name).val(obj.DESC);
                __doPostBack('question', 'true');
            }
        }

        //清空
        function doClear() {
            $("#txt_COMPANY_CD").val("");
            $("#txt_COMPANY_NAME").val("");
            $("#txt_FEES_YM").val("");
            $("#txt_LICENCE_ID").val("");
            $("#txt_INS_NAME").val("");
            $("#txt_FAMILY_NAME").val("");
            $("#ddl_BILLS_KIND").val("-1");
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
                                <col width="30%" />
                                <col width="15%" />
                                <col width="10%" />
                                <col width="25%" />
                                <col width="5%" />


                            </colgroup>
                            <tbody>

                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_COMPANY_CD" runat="server" Text="<%$Resources:Resource,wfb2ia_COMPANY_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_COMPANY_CD" runat="server" MaxLength="1" Width="64px" ClientIDMode="Static" CssClass="MandatoryField" onKeyUp="companyCheck(this.value);"></asp:TextBox>
                                        <input id="bt_COMPANY_SEARCH" type="button" value="..." onclick="OpenSearch('Company_Search.aspx', 'txt_COMPANY_CD', 'txt_COMPANY_NAME', '', 'Y');" />
                                        <asp:TextBox ID="txt_COMPANY_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_COMPANY_CD%>"
                                            ControlToValidate="txt_COMPANY_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BILLS_KIND" runat="server" Text="<%$Resources:Resource,wfb2ia_BILLS_KIND%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_BILLS_KIND" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_required_BILLS_KIND%>" ClientValidationFunction="ddlCheck" ForeColor="Red"
                                            ControlToValidate="ddl_BILLS_KIND" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                </tr>

                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_FEES_YM" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_FEES_YM%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_FEES_YM" runat="server" MaxLength="6" Width="64px" CssClass="MandatoryField ym date" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_required_FEES_YM%>"
                                            ControlToValidate="txt_FEES_YM" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_error_FEES_YM%>" ControlToValidate="txt_FEES_YM" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LICENCE_ID" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_LICENCE_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_LICENCE_ID" runat="server" MaxLength="20" Width="80px" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_INS_NAME" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_INS_NAME%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_INS_NAME" runat="server" MaxLength="18" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_FAMILY_NAME" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_FAMILY_NAME%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_FAMILY_NAME" runat="server" MaxLength="18" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>

                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2IA3100Search" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0100Search%>" OnClick="WFB2IA3100Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />
                                            <aces:Btn ID="WFB2IA3100Process" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA3100Process%>" OnClick="WFB2IA3100Process_Click" OnClientClick="BlockUI();" />

                                            <%--<asp:Button ID="WFB2IA3100Search" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0100Search%>" OnClick="WFB2IA3100Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />--%>
                                            <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2ia_btn_clear%>" OnClientClick="return doClear();" />
                                            <%--<asp:Button ID="WFB2IA3100Process" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA3100Process%>" OnClick="WFB2IA3100Process_Click" OnClientClick="BlockUI();" />--%>
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
                    <td colspan="4">
                        <br />
                    </td>
                </tr>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="GetData"
                SelectCountMethod="GetCount" TypeName="CFB2IA3100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">

                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_COMPANY_CD" DefaultValue=""
                        Name="company_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_BILLS_KIND" DefaultValue=""
                        Name="bills_kind" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_FEES_YM" DefaultValue=""
                        Name="fees_ym" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_LICENCE_ID" DefaultValue=""
                        Name="licence_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_INS_NAME" DefaultValue=""
                        Name="ins_name" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_FAMILY_NAME" DefaultValue=""
                        Name="family_name" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>


            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">

                <Columns>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2si_RowNum%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_COMPANY_CD%>" SortExpression="COMPANY_CD">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_COMPANY_SNAME" runat="server" Text='<%#Bind("COMPANY_SNAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_LICENCE_ID%>" SortExpression="LICENSE_ID">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_LICENCE_ID" runat="server" Text='<%#Bind("LICENSE_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_INS_NAME%>" SortExpression="INS_NAME">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_INS_NAME" runat="server" Text='<%#Bind("INS_NAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_FAMILY_NAME%>" SortExpression="FAMILY_NAME">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_FAMILY_NAME" runat="server" Text='<%#Bind("FAMILY_NAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_INS_AMT%>" SortExpression="INS_AMT">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_INS_AMT" runat="server" Text='<%#Bind("INS_AMT","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_CHANG_TYPE%>" SortExpression="CHANG_TYPE">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_CHANG_TYPE" runat="server" Text='<%#Bind("CHANG_TYPE")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_FEES_REMARK%>" SortExpression="FEES_REMARK">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_FEES_REMARK" runat="server" Text='<%#Bind("FEES_REMARK")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_FEES_SELF%>" SortExpression="FEES_SELF">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_FEES_SELF" runat="server" Text='<%#Bind("FEES_SELF","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_FEES_CMP%>" SortExpression="FEES_CMP">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_FEES_CMP" runat="server" Text='<%#Bind("FEES_CMP","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_FEES%>" SortExpression="FEES">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_FEES" runat="server" Text='<%#Bind("FEES","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_FEES_SELF%>" SortExpression="TRACED_FEES_SELF">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_TRACED_FEES_SELF" runat="server" Text='<%#Bind("TRACED_FEES_SELF","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_FEES_CMP%>" SortExpression="TRACED_FEES_CMP">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_TRACED_FEES_CMP" runat="server" Text='<%#Bind("TRACED_FEES_CMP","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_FEES%>" SortExpression="TRACED_FEES">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_TRACED_FEES" runat="server" Text='<%#Bind("TRACED_FEES","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_FEES_TOTAL%>" SortExpression="FEES_TOTAL">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_FEES_TOTAL" runat="server" Text='<%#Bind("FEES_TOTAL","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <EmptyDataTemplate>

                    <table class="grid-view" width="1020px">
                        <tr class="header">
                            <td>
                                <asp:Label ID="Label" runat="server" Text="<%$Resources:Resource,wfb2si_RowNum%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Labe2" runat="server" Text="<%$Resources:Resource,wfb2ia_COMPANY_CD%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Labe3" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_LICENCE_ID%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text="<%$Resources:Resource,wfb2ia_INS_NAME%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_FAMILY_NAME%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_INS_AMT%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2ia_CHANG_TYPE%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2ia_FEES_REMARK%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2ia_FEES_SELF%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2ia_FEES_CMP%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2ia_FEES%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2ia_FEES_SELF%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2ia_FEES_CMP%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label10" runat="server" Text="<%$Resources:Resource,wfb2ia_FEES%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="<%$Resources:Resource,wfb2ia_FEES_TOTAL%>"></asp:Label>
                            </td>

                        </tr>

                    </table>

                </EmptyDataTemplate>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <asp:GridView ID="gv_result2" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">

                <Columns>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2si_RowNum%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_COMPANY_CD%>" SortExpression="COMPANY_CD">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_COMPANY_SNAME" runat="server" Text='<%#Bind("COMPANY_SNAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_LICENCE_ID%>" SortExpression="LICENSE_ID">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_LICENCE_ID" runat="server" Text='<%#Bind("LICENSE_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_INS_NAME%>" SortExpression="INS_NAME">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_INS_NAME" runat="server" Text='<%#Bind("INS_NAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_CHANG_TYPE%>" SortExpression="CHANG_TYPE">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_CHANG_TYPE" runat="server" Text='<%#Bind("CHANG_TYPE")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_INS_AMT%>" SortExpression="INS_AMT">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_INS_AMT" runat="server" Text='<%#Bind("INS_AMT","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_FEES_SELF%>" SortExpression="FEES_SELF">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_FEES_SELF" runat="server" Text='<%#Bind("FEES_SELF","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_LAST_UPDATE_DT%>" SortExpression="LAST_UPDATE_DT">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_LAST_UPDATE_DT" runat="server" Text='<%#Bind("LAST_UPDATE_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <EmptyDataTemplate>

                    <table class="grid-view" width="1020px">
                        <tr class="header">
                            <td>
                                <asp:Label ID="Label" runat="server" Text="<%$Resources:Resource,wfb2si_RowNum%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Labe2" runat="server" Text="<%$Resources:Resource,wfb2ia_COMPANY_CD%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Labe3" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_LICENCE_ID%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text="<%$Resources:Resource,wfb2ia_INS_NAME%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2ia_CHANG_TYPE%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_INS_AMT%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2ia_FEES_SELF%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="<%$Resources:Resource,wfb2ia_LAST_UPDATE_DT%>"></asp:Label>
                            </td>

                        </tr>

                    </table>

                </EmptyDataTemplate>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <asp:GridView ID="gv_result3" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">

                <Columns>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2si_RowNum%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_COMPANY_CD%>" SortExpression="COMPANY_CD">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_COMPANY_SNAME" runat="server" Text='<%#Bind("COMPANY_SNAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_LICENCE_ID%>" SortExpression="LICENSE_ID">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_LICENCE_ID" runat="server" Text='<%#Bind("LICENSE_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_INS_NAME%>" SortExpression="INS_NAME">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_INS_NAME" runat="server" Text='<%#Bind("INS_NAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_INS_AMT%>" SortExpression="INS_AMT">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_INS_AMT" runat="server" Text='<%#Bind("INS_AMT","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_RATE%>" SortExpression="RATE">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_RATE" runat="server" Text='<%#Bind("RATE")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_FEES_SELF%>" SortExpression="FEES_SELF">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_FEES_SELF" runat="server" Text='<%#Bind("FEES_SELF","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_LAST_UPDATE_DT%>" SortExpression="LAST_UPDATE_DT">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_LAST_UPDATE_DT" runat="server" Text='<%#Bind("LAST_UPDATE_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <EmptyDataTemplate>

                    <table class="grid-view" width="1020px">
                        <tr class="header">
                            <td>
                                <asp:Label ID="Label" runat="server" Text="<%$Resources:Resource,wfb2si_RowNum%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Labe2" runat="server" Text="<%$Resources:Resource,wfb2ia_COMPANY_CD%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Labe3" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_LICENCE_ID%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text="<%$Resources:Resource,wfb2ia_INS_NAME%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_INS_AMT%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2ia_RATE%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2ia_FEES_SELF%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="<%$Resources:Resource,wfb2ia_LAST_UPDATE_DT%>"></asp:Label>
                            </td>

                        </tr>

                    </table>

                </EmptyDataTemplate>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <asp:GridView ID="gv_result4" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">

                <Columns>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2si_RowNum%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_COMPANY_CD%>" SortExpression="COMPANY_CD">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_COMPANY_SNAME" runat="server" Text='<%#Bind("COMPANY_SNAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_LICENCE_ID%>" SortExpression="LICENSE_ID">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_LICENCE_ID" runat="server" Text='<%#Bind("LICENSE_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_INS_NAME%>" SortExpression="INS_NAME">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_INS_NAME" runat="server" Text='<%#Bind("INS_NAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_INS_AMT%>" SortExpression="INS_AMT">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_INS_AMT" runat="server" Text='<%#Bind("INS_AMT","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_RATE2%>" SortExpression="RATE">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_RATE" runat="server" Text='<%#Bind("RATE")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_FEES_SELF%>" SortExpression="FEES_SELF">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_FEES_SELF" runat="server" Text='<%#Bind("FEES_SELF","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_LAST_UPDATE_DT%>" SortExpression="LAST_UPDATE_DT">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_LAST_UPDATE_DT" runat="server" Text='<%#Bind("LAST_UPDATE_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <EmptyDataTemplate>

                    <table class="grid-view" width="1020px">
                        <tr class="header">
                            <td>
                                <asp:Label ID="Label" runat="server" Text="<%$Resources:Resource,wfb2si_RowNum%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Labe2" runat="server" Text="<%$Resources:Resource,wfb2ia_COMPANY_CD%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Labe3" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_LICENCE_ID%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text="<%$Resources:Resource,wfb2ia_INS_NAME%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_INS_AMT%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2ia_RATE2%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2ia_FEES_SELF%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="<%$Resources:Resource,wfb2ia_LAST_UPDATE_DT%>"></asp:Label>
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
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />

            <%--<asp:HiddenField ID="HID_BILLS_KIND" runat="server" ClientIDMode="Static" />--%>

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
