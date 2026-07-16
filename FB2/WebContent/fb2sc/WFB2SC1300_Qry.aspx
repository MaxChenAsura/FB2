<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sc/WFB2SC1300_Qry.aspx.cs" Inherits="WebContent_fb2sc_WFB2SC1300_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            gridviewScroll();
            $.unblockUI();
        }

        function gridviewScroll() {

            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "400",
                barcolor: "#7F7F7F",
            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
        }

        function ClearAll() {
            $("#txt_OPERATION_ID").val("");
            $("#txt_OPERATION_NAME").val("");
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                    <colgroup>
                        <col width="15%" />
                        <col width="15%" />
                        <col width="15%" />
                        <col width="15%" />
                        <col width="40%" />
                    </colgroup>
                    <tbody>
                        <!-- START: 1st Line in Search Criteria area -->
                        <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_OPERATION_ID" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_OPERATION_ID%>"></asp:Label>:</th>
                            <td align="left" class="Body_label" colspan="2">
                                <asp:TextBox ID="txt_OPERATION_ID" runat="server" MaxLength="4" Width="70px" ClientIDMode="Static"></asp:TextBox>
                            </td>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_OPERATION_NAME" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_OPERATION_NAME%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_OPERATION_NAME" runat="server" MaxLength="60" Width="100px" ClientIDMode="Static"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="Body_label" colspan="10">
                                <div id="init">
                                    <aces:Btn ID="WFB2SC1300Search" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1300Search%>" Visible="true" OnClick="WFB2SC1300Search_Click" />

                                    <%--<asp:Button ID="WFB2SC1300Search" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1300Search%>" Visible="true" OnClick="WFB2SC1300Search_Click" />--%>
                                    <a href="#" tabindex="91" name="WFB2B1100Clear" id="WFB2B1100Clear">
                                        <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2sc_WFB2SC1300Clear%>" onclick="ClearAll();" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" height="1" colspan="10">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="10">
                                <div id="init_add" style="display: inline">
                                </div>
                                <div id="init_grid" style="display: inline">
                                    <aces:Btn ID="WFB2SC1300Update" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1300Update%>" Visible="false" OnClick="WFB2SC1300Update_Click" />

                                    <%--<asp:Button ID="WFB2SC1300Update" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1300Update%>" Visible="false" OnClick="WFB2SC1300Update_Click" />--%>
                                </div>
                                <div id="grid_add">
                                    <aces:Btn ID="WFB2SC1300Confirm" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1300Confirm%>" Visible="false" OnClick="WFB2SC1300Confirm_Click" />

                                    <%--<asp:Button ID="WFB2SC1300Confirm" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1300Confirm%>" Visible="false" OnClick="WFB2SC1300Confirm_Click" />--%>
                                    <asp:Button ID="WFB2SC1300Cancel" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1300Cancel%>" Visible="false" OnClientClick="return confirm('您確定要放棄目前的資料嗎?');" OnClick="WFB2SC1300Cancel_Click" />
                                </div>
                            </td>
                        </tr>
                        <!-- end: Create MODULE ID -->
                    </tbody>
                </table>

            </div>
            <div id="editPage1">

                <div id="gridList1">
                    <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                        SelectCountMethod="getCount" TypeName="CFB2SC1300DAO" EnablePaging="True"
                        SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                        OnSelected="ods1_Selected">
                        <SelectParameters>
                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                            <asp:Parameter Name="maximumRows" Type="Int32" />
                            <asp:ControlParameter ControlID="txt_OPERATION_ID" DefaultValue=""
                                Name="operation_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                            <asp:ControlParameter ControlID="txt_OPERATION_NAME" DefaultValue=""
                                Name="operation_name" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                        AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="false" OnSorting="gv_result_Sorting"
                        OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                        OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound" meta:resourcekey="gv_resultResource1">
                        <Columns>
                            <asp:TemplateField HeaderStyle-Width="20px">
                                <%--<HeaderTemplate>
                                    <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectNonDisabledCheckboxes(this);" ClientIDMode="Static" />
                                </HeaderTemplate>--%>
                                <ItemTemplate>
                                    <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_RowNumber%>" HeaderStyle-Width="20px">
                                <ItemTemplate>
                                    <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                                </EditItemTemplate>
                                <HeaderStyle Width="20px"></HeaderStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_SALARY_TYPE%>" SortExpression="SALARY_TYPE" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lb_SALARY_TYPE" runat="server" Text='<%#Bind("SALARY_NAME")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="txt_EDIT_SALARY_TYPE" runat="server" MaxLength="10" Width="100px" Text='<%#Bind("SALARY_NAME")%>'></asp:Label>
                                </EditItemTemplate>
                                <HeaderStyle Width="100px"></HeaderStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_OPERATION_ID%>" SortExpression="OPERATION_ID" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lb_OPERATION_ID" runat="server" Text='<%#Bind("OPERATION_ID")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="txt_EDIT_OPERATION_ID" runat="server" MaxLength="10" Width="100px" Text='<%#Bind("OPERATION_ID")%>'></asp:Label>
                                </EditItemTemplate>
                                <HeaderStyle Width="100px"></HeaderStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_OPERATION_NAME%>" SortExpression="OPERATION_NAME" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lb_OPERATION_NAME" runat="server" Text='<%#Bind("OPERATION_NAME")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_EDIT_OPERATION_NAME" runat="server" MaxLength="10" Width="100px" Text='<%#Bind("OPERATION_NAME")%>'></asp:TextBox>
                                </EditItemTemplate>
                                <HeaderStyle Width="100px"></HeaderStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_SALARY_REQ%>" SortExpression="SALARY_REQ" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lb_SALARY_REQ" runat="server" Text='<%#Bind("SALARY_REQ")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddl_yn" runat="server">
                                        <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                                        <asp:ListItem Text="N" Value="N"></asp:ListItem>
                                    </asp:DropDownList>
                                    <%--TODO--%>
                                    <%--<asp:TextBox ID="txt_EDIT_SALARY_REQ" runat="server" MaxLength="10" Width="100px" Text='<%#Bind("SALARY_REQ")%>'></asp:TextBox>--%>
                                </EditItemTemplate>
                                <HeaderStyle Width="100px"></HeaderStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_PROC_SOUCE%>" SortExpression="PROC_SOUCE" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lb_PROC_SOUCE" runat="server" Text='<%#Bind("PROC_SOUCE")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="txt_EDIT_PROC_SOUCE" runat="server" MaxLength="1" Width="100px" Text='<%#Bind("PROC_SOUCE")%>'></asp:Label>
                                </EditItemTemplate>
                                <HeaderStyle Width="100px"></HeaderStyle>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="SALARY_TYPE" HeaderStyle-Width="100px" Visible="false" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="hid_SALARY_TYPE" runat="server" Text='<%#Bind("SALARY_TYPE")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="hid_EDIT_SALARY_TYPE" runat="server" MaxLength="10" Width="100px" Text='<%#Bind("SALARY_TYPE")%>'></asp:Label>
                                </EditItemTemplate>
                                <HeaderStyle Width="100px"></HeaderStyle>
                            </asp:TemplateField>

                        </Columns>
                        <HeaderStyle CssClass="GridviewScrollHeader" />
                        <PagerStyle CssClass="GridviewScrollPager" />
                        <EditRowStyle HorizontalAlign="Center" />
                    </asp:GridView>
                    <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                        <tr valign="top">

                            <td class="GridviewScrollPager TD">
                               <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static" AutoPostBack="true">
                                    <asp:ListItem Text="每頁10000筆" Value="10000"></asp:ListItem>
                                                             <%--   <asp:ListItem Text="每頁20筆" Value="20"></asp:ListItem>
                          <asp:ListItem Text="每頁30筆" Value="30"></asp:ListItem>
                            <asp:ListItem Text="每頁40筆" Value="40"></asp:ListItem>
                            <asp:ListItem Text="每頁50筆" Value="50"></asp:ListItem>--%>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 5px"></td>
                            <td style="font-size: 14px;">
                                <asp:Label ID="lb_TotalCount" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>
