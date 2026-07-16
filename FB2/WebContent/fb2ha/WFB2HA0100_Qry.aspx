<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2ha/WFB2HA0100_Qry.aspx.cs" Inherits="WebContent_fb2ha_WFB2HA0100_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {

            $(".number").mask('99');
            //$(".number").css("text-align", "right").css("ime-mode", "disabled");
            gridviewScroll();
            $.unblockUI();
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


        //清空畫面
        function ClearAll() {

            $("#ddl_DEPT_LEVEL").val("-1");

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

        //開啟部門基本資料維護
        function openHA020(id) {
            window.open("WFB2HA0200_Qry.aspx?dept_level=" + id + "&parentFuncId=" + parentFuncID, 'HA0200');
        }

        function cancelClientClick() {
            return confirm($("#hid_cancel_ConfirmMessage").val());
        }

        function CheckModeifyAction() {
            if (LookUpCheckboxs() == 1) {
                return true;
            }
            else {
                alert($('#hid_chooseOneMessage').val());
                return false;
            }
        }

        function CheckDelAction() {
            if (LookUpCheckboxs() > 0) {
                var rtnval = confirm($('#hid_delete_ConfirmMessage').val());
                if (rtnval) BlockUI();
                return rtnval;
            }
            else {
                alert($('#hid_notChooseMessage').val());
                return false;
            }
        }

        function LookUpCheckboxs() {
            var ItemCheckBoxs = $("#gv_result [type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked && ItemCheckBoxs[i].id.indexOf("cb_all") == -1) {
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
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

                <tr height="100%" valign="top">
                    <td>
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
                                        <asp:Label ID="lb_DEPT_LEVEL" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_DEPT_LEVEL%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_DEPT_LEVEL" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                        <asp:HiddenField ID="hid_DEPT_LEVEL" ClientIDMode="Static" runat="server" />
                                    </td>

                                </tr>

                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2HA0100Search" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA010Search%>" OnClick="WFB2HA010Search_Click" />

                                            <%--<asp:Button ID="WFB2HA0100Search" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA010Search%>" OnClick="WFB2HA010Search_Click" />--%>

                                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2ha_btn_clear%>" onclick="ClearAll();" />
                                            <asp:HiddenField ID="hid_cancel_ConfirmMessage" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_delete_ConfirmMessage" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_notChooseMessage" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_chooseOneMessage" ClientIDMode="Static" runat="server" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center" height="1" colspan="10">
                        <hr>
                    </td>
                </tr>
                <tr>

                    <td align="right" class="Body_label">
                        <div id="init_grid">
                            <aces:Btn ID="WFB2HA0100Add" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA010Add%>" OnClick="WFB2HA010Add_Click" />
                            <aces:Btn ID="WFB2HA0100Delete" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA010Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2HA010Delete_Click" Visible="False" />
                            <aces:Btn ID="WFB2HA0100Edit" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA010Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2HA010Edit_Click" Visible="False" />
                            <aces:Btn ID="WFB2HA0100Save" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA010Save%>" Visible="false" OnClick="WFB2HA010Save_Click" ValidationGroup="GroupA" />

                            <%--<asp:Button ID="WFB2HA0100Add" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA010Add%>" OnClick="WFB2HA010Add_Click" />
                            <asp:Button ID="WFB2HA0100Delete" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA010Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2HA010Delete_Click" Visible="False" />
                            <asp:Button ID="WFB2HA0100Edit" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA010Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2HA010Edit_Click" Visible="False" />
                            <asp:Button ID="WFB2HA0100Save" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA010Save%>" Visible="false" OnClick="WFB2HA010Save_Click" ValidationGroup="GroupA" />--%>
                            <asp:Button ID="WFB2HA0100Cancel" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA010Cancel%>" Visible="false" OnClick="WFB2HA010Cancel_Click" OnClientClick="return cancelClientClick()" />
                        </div>

                    </td>
                </tr>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2HA0100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="hid_DEPT_LEVEL" DefaultValue=""
                        Name="dept_level" PropertyName="Value" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_RowNumber%>" HeaderStyle-Width="30px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_DEPT_LEVEL%>" SortExpression="DEPT_LEVEL" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label ID="lb_DEPT_LEVEL" runat="server" Text='<%#Bind("DEPT_LEVEL")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_EDIT_DEPT_LEVEL" runat="server" Text='<%#Bind("DEPT_LEVEL")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_DEPT_LEVEL" runat="server" MaxLength="2" Width="40px" CssClass="MandatoryField number"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_DEPT_LEVEL%>"
                                ControlToValidate="txt_NEW_DEPT_LEVEL" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="checknum" runat="server" ValidateEmptyText="true"
                                ErrorMessage="部門層級格式錯誤" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_DEPT_LEVEL" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </FooterTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                        <FooterStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_DEPT_LEVEL_DESC%>" SortExpression="DEPT_LEVEL_DESC" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_DEPT_LEVEL_DESC" runat="server" Text='<%#Bind("DEPT_LEVEL_DESC")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_DEPT_LEVEL_DESC" runat="server" MaxLength="30" Width="100px" Text='<%#Bind("DEPT_LEVEL_DESC")%>' CssClass="MandatoryField"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_DEPT_LEVEL_DESC%>"
                                ControlToValidate="txt_EDIT_DEPT_LEVEL_DESC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_DEPT_LEVEL_DESC" runat="server" MaxLength="30" Width="100px" CssClass="MandatoryField"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_DEPT_LEVEL_DESC%>"
                                ControlToValidate="txt_NEW_DEPT_LEVEL_DESC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                        <FooterStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_LEVEL_TYPE_DESC%>" SortExpression="LEVEL_TYPE" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEVEL_TYPE_DESC" runat="server" Text='<%#Bind("LEVEL_TYPE_DESC")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_EDIT_LEVEL_TYPE" runat="server" CssClass="MandatoryField"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_LEVEL_TYPE%>"
                                ControlToValidate="ddl_EDIT_LEVEL_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                            <asp:HiddenField ID="hid_EDIT_LEVEL_TYPE" runat="server" Value='<%#Bind("LEVEL_TYPE")%>' />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_NEW_LEVEL_TYPE" runat="server" CssClass="MandatoryField"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_LEVEL_TYPE%>"
                                ControlToValidate="ddl_NEW_LEVEL_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                        <FooterStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_IS_VALID%>" SortExpression="IS_VALID" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_IS_VALID" runat="server" Text='<%#Bind("IS_VALID")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_EDIT_IS_VALID" runat="server" CssClass="MandatoryField">
                                <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                                <asp:ListItem Text="N" Value="N"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:HiddenField ID="hid_EDIT_IS_VALID" runat="server" Value='<%#Bind("IS_VALID")%>' />

                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_NEW_IS_VALID" runat="server" CssClass="MandatoryField">
                                <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                                <asp:ListItem Text="N" Value="N"></asp:ListItem>
                            </asp:DropDownList>
                        </FooterTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                        <FooterStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_REMARK%>" SortExpression="REMARK" HeaderStyle-Width="300px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_REMARK" runat="server" Text='<%#Bind("REMARK")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_REMARK" runat="server" MaxLength="210" Width="300px" Text='<%#Bind("REMARK")%>'></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_REMARK" runat="server" MaxLength="210" Width="300px"></asp:TextBox>
                        </FooterTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                        <FooterStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_function%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <input id="btn_Detail" type="button" value="<%$Resources:Resource,wfb2ha_btn_Detail%>" runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <input id="btn_Detail" type="button" value="<%$Resources:Resource,wfb2ha_btn_Detail%>" runat="server" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <input id="btn_Detail" type="button" value="<%$Resources:Resource,wfb2ha_btn_Detail%>" runat="server" />
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>

                    <table class="grid-view" width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                        <tr class="header">
                            <td></td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2ha_RowNumber%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_DEPT_LEVEL%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_DEPT_LEVEL_DESC%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_LEVEL_TYPE_DESC%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_IS_VALID%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_REMARK%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_function%>"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td></td>
                            <td></td>
                            <td style="text-align: Right;">
                                <asp:TextBox ID="txt_NEW_DEPT_LEVEL" runat="server" MaxLength="2" Width="50px" CssClass="MandatoryField number"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_DEPT_LEVEL%>"
                                    ControlToValidate="txt_NEW_DEPT_LEVEL" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="checknum" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="部門層級格式錯誤" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_DEPT_LEVEL" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            </td>
                            <td style="text-align: Left;">
                                <asp:TextBox ID="txt_NEW_DEPT_LEVEL_DESC" runat="server" MaxLength="30" Width="100px" CssClass="MandatoryField"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_DEPT_LEVEL_DESC%>"
                                    ControlToValidate="txt_NEW_DEPT_LEVEL_DESC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <td style="text-align: Left;">
                                <asp:DropDownList ID="ddl_NEW_LEVEL_TYPE" runat="server" CssClass="MandatoryField"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_LEVEL_TYPE%>"
                                    ControlToValidate="ddl_NEW_LEVEL_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                            </td>
                            <td style="text-align: Left;">
                                <asp:DropDownList ID="ddl_NEW_IS_VALID" runat="server" CssClass="MandatoryField">
                                    <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                                    <asp:ListItem Text="N" Value="N"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: Left;">
                                <asp:TextBox ID="txt_NEW_REMARK" runat="server" MaxLength="210" Width="400px"></asp:TextBox>
                            </td>
                            <td></td>

                        </tr>
                    </table>

                </EmptyDataTemplate>
                <HeaderStyle CssClass="GridviewScrollHeader" />
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle HorizontalAlign="Center" />
                <EditRowStyle HorizontalAlign="Center" />
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
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
