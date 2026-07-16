<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dc/action/WFB2DC0300_Dtl.aspx.cs" Inherits="WebContent_WFB2DC0300_Dtl" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".number").mask('999');

            $("#txt_VENDOR_NO").attr("readonly", true);
            $("#txt_VENDOR_NAME").attr("readonly", true);
            $("#txt_VENDOR_MEMBER_NAME").attr("readonly", true);
            $("#txt_VENDOR_NO2").attr("readonly", true);

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

        //清空畫面
        function ClearAll() {
            $("#txt_VENDOR_MEMBER_NO").val("");
            $("#txt_VENDOR_MEMBER_NAME").val("");
        }

        function getVendorMember() {
            var vendor_no = $("#txt_VENDOR_NO").val();
            var json = OpenSearch('Vendor_d_Search.aspx', 'txt_VENDOR_MEMBER_NO', 'txt_VENDOR_MEMBER_NAME', 'VENDOR_NO=' + vendor_no);
            if (json != undefined) {
                $("#txt_VENDOR_MEMBER_NO").val(json.CD);    //廠商人員編號
                $("#txt_VENDOR_MEMBER_NAME").val(json.DESC);  //廠商人員姓名
            }
        }

        function LookUpCheckboxs() {
            var ItemCheckBoxs = $(":checkbox[id$=cb_check]");
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
                return confirm("確定要刪除?");
            else {
                alert("請選取資料!");
                return false;
            }
        }

        function getVENDOR_MEMBER_NAME(VENDOR_MEMBER_id) {
            var txt = document.getElementById(VENDOR_MEMBER_id);
            if (txt.value != "") {
                document.getElementById('hid_getVENDOR_MEMBER_NAME').click();
                return false;
            } else {
                $("#txt_VENDOR_MEMBER_NAME").val("");
            }
        }

        function Check_ID_LENGTH(source, arguments) {
            var vendor_no = $("#txt_VENDOR_NO").val();
            var value = $.trim(arguments.Value) + vendor_no;
            if (value.length != 5)
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }

        function openQry() {
            window.location.href("WFB2DC0300_Qry.aspx");
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
                                        <asp:Label ID="lb_VENDOR_NO" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_VENDOR_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_VENDOR_NO" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_VENDOR_NAME" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_VENDOR_NAME%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_VENDOR_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_VENDOR_MEMBER_NO" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_VENDOR_MEMBER_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_VENDOR_MEMBER_NO" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" onblur="javascript:getVENDOR_MEMBER_NAME(this.id)"></asp:TextBox>
                                        <input id="btn_VENDOR_MEMBER_NO" type="button" value="..." onclick="getVendorMember();" />
                                        <asp:TextBox ID="txt_VENDOR_MEMBER_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2DC0301Search" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0301Search%>" OnClientClick="BlockUI();" OnClick="WFB2DC0301Search_Click" />

                                            <%--<asp:Button ID="WFB2DC0301Search" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0301Search%>" OnClientClick="BlockUI();" OnClick="WFB2DC0301Search_Click" />--%>
                                            
                                            <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2dc_btn_clear%>" OnClientClick="ClearAll();" />
                                            <asp:Button ID="btn_back" runat="server" Text="<%$Resources:Resource,wfb2dc_btn_back%>" OnClick="btn_back_Click" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td align="right" class="Body_label">
                        <hr>
                        <div id="init_grid">
                        <aces:Btn ID="WFB2DC0301Add" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0301Add%>" OnClick="WFB2DC0301Add_Click" />
                            <aces:Btn ID="WFB2DC0301Delete" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0301Delete%>" OnClientClick="return CheckDelAction();" Visible="false" OnClick="WFB2DC0301Delete_Click" />
                            <aces:Btn ID="WFB2DC0301Edit" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0301Edit%>" Visible="false" OnClick="WFB2DC0301Edit_Click" />
                            <aces:Btn ID="WFB2DC0301Save" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0301Save%>" Visible="false" ValidationGroup="GroupA" OnClick="WFB2DC0301Save_Click" />

<%--                            <asp:Button ID="WFB2DC0301Add" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0301Add%>" OnClick="WFB2DC0301Add_Click" />
                            <asp:Button ID="WFB2DC0301Delete" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0301Delete%>" OnClientClick="return CheckDelAction();" Visible="false" OnClick="WFB2DC0301Delete_Click" />
                            <asp:Button ID="WFB2DC0301Edit" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0301Edit%>" Visible="false" OnClick="WFB2DC0301Edit_Click" />
                            <asp:Button ID="WFB2DC0301Save" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0301Save%>" Visible="false" ValidationGroup="GroupA" OnClick="WFB2DC0301Save_Click" />--%>
                            
                            <asp:Button ID="WFB2DC0301Cancel" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0301Cancel%>" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="WFB2DC0301Cancel_Click" />
                        </div>
                    </td>
                </tr>

            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData2"
                SelectCountMethod="getCount2" TypeName="CFB2DC0300DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_VENDOR_NO"
                        Name="vendor_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_VENDOR_MEMBER_NO"
                        Name="vendor_member_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" Width="1020px"
                OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound"
                OnRowCreated="gv_result_RowCreated" OnDataBound="gv_result_DataBound"
                OnPageIndexChanging="gv_result_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_RowNumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%-- 廠商人員編號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_VENDOR_MEMBER_NO%>" SortExpression="VENDOR_MEMBER_NO" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_VENDOR_MEMBER_NO" runat="server" Text='<%#Bind("VENDOR_MEMBER_NO")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_VENDOR_MEMBER_NO" runat="server" Text='<%#Bind("VENDOR_MEMBER_NO")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:TextBox ID="txt_VENDOR_NO2" runat="server" BorderWidth="0" ClientIDMode="Static" Width="30px"></asp:TextBox>
                                <asp:TextBox ID="txt_NEW_VENDOR_MEMBER_NO" runat="server" ClientIDMode="Static" Width="120px" CssClass="MandatoryField number"></asp:TextBox>
                                <!--驗證長度需為3 -->
                                <asp:CustomValidator ID="CustomValidatorVENDOR_MEMBER_NO_LENGTH3" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_VENDOR_MEMBER_NO_LENGTH5%>" ClientValidationFunction="Check_ID_LENGTH" ForeColor="Red"
                                    ControlToValidate="txt_NEW_VENDOR_MEMBER_NO" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
                                <asp:CustomValidator ID="CustomValidatorVENDOR_MEMBER_NO" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2di_ERR_VENDOR_MEMBER_NO_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_VENDOR_MEMBER_NO" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_VENDOR_MEMBER_NAME%>" SortExpression="VENDOR_MEMBER_NAME" HeaderStyle-Width="300px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_VENDOR_MEMBER_NAME" runat="server" Text='<%#Bind("VENDOR_MEMBER_NAME")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_VENDOR_MEMBER_NAME" runat="server" Text='<%#Bind("VENDOR_MEMBER_NAME")%>' Width="100%" MaxLength="30"></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_NEW_VENDOR_MEMBER_NAME" runat="server" ClientIDMode="Static" Width="100%" MaxLength="30"></asp:TextBox>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                </Columns>
                <EmptyDataTemplate>
                    <table class="grid-view" width="1020px">
                        <tr class="header">
                            <td>
                                <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" Width="20px" />
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2dc_RowNumber%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_VENDOR_MEMBER_NO%>" Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_VENDOR_MEMBER_NAME%>" Width="300px"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td>
                                <asp:CheckBox ID="cb_check" runat="server" Width="20px" />
                            </td>
                            <td>
                                <asp:Label ID="lb_NewRowNumber" runat="server" Text="1" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:TextBox ID="txt_VENDOR_NO2" runat="server" BorderWidth="0" ClientIDMode="Static" Width="30px"></asp:TextBox>
                                    <asp:TextBox ID="txt_NEW_VENDOR_MEMBER_NO" runat="server" ClientIDMode="Static" Width="120px" CssClass="number"></asp:TextBox>
                                     <!--驗證長度需為3 -->
                                <asp:CustomValidator ID="CustomValidatorVENDOR_MEMBER_NO_LENGTH4" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_VENDOR_MEMBER_NO_LENGTH5%>" ClientValidationFunction="Check_ID_LENGTH" ForeColor="Red"
                                    ControlToValidate="txt_NEW_VENDOR_MEMBER_NO" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
                                <asp:CustomValidator ID="CustomValidatorVENDOR_MEMBER_NO" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2di_ERR_VENDOR_MEMBER_NO_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_VENDOR_MEMBER_NO" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: left; width: 100%">
                                    <asp:TextBox ID="txt_NEW_VENDOR_MEMBER_NAME" runat="server" ClientIDMode="Static" Width="100%" MaxLength="30"></asp:TextBox>
                                </div>
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
            <asp:Button ID="hid_getVENDOR_MEMBER_NAME" runat="server" Style="display: none" ClientIDMode="Static" OnClick="hid_getVENDOR_MEMBER_NAME_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

