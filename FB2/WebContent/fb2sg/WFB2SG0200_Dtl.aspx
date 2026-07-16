<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sg/WFB2SG0200_Dtl.aspx.cs" Inherits="WebContent_WFB2SG0200_Dtl" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        $(function () {

            iniForm();
        });
        function iniForm() {
            //日期格式心須
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.date').mask('9999/99/99');
            $(".decimal").css("text-align", "right").css("ime-mode", "disabled");


            //reComma("ALLOWANCE_VALUE", 0);
            //GridView必須
            gridviewScroll();
            $.unblockUI();
        }


        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());

        }

        //凍結視窗用
        function gridviewScroll() {
            if ($("#HID_Freeze").val() == "Y") {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "400",
                    barcolor: "#7F7F7F",
                    freezesize: 6

                });
                CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
            }
            else {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "400",
                    barcolor: "#7F7F7F"

                });
            }
        }

        //刪除,檢查是否有勾選
        function doDelete() {
            if (checkboxsSelected() > 0) {
                return confirm("確定要刪除?");
            } else {
                alert("請選取資料!");
                return false;
            }
        }

        var choietr = null;
        //回傳目前Checkbox被勾選的數量
        function checkboxsSelected() {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                    if (choietr == null) {
                        choietr = i;
                    }
                }
            }
            return HaveCheck;
        }

        //查詢前檢核
        function CheckSearch() {
            //其它需要檢核的

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
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

        //清空畫面
        function ClearAll() {
            $("#ddl_ENV_ALLOWANCE_TYPE").val("-1");
        }

    </script>
    <style type="text/css">
        .auto-style1 {
            height: 23px;
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
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
                            <asp:Label ID="Label" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_festival_type%>"></asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="txt_FESTIVAL_TYPE_DESC" runat="server" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_prid_cd%>"></asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="txt_EMP_CD_DESC" runat="server" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_festival_dt%>"></asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="txt_FESTIVAL_DT" runat="server" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="Label10" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_festival_pay_dt%>"></asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="txt_FESTIVAL_PAY_DT" runat="server" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th></th>
                        <td align="right" colspan="5">
                            <asp:Button ID="WFB2SG0200Back" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_back%>" OnClientClick="BlockUI();" OnClick="WFB2SG0200Back_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="Body_label" colspan="6">
                            <div id="init_grid">
                                <aces:Btn ID="WFB2SG0201Add" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_add%>" Visible="true" OnClick="WFB2SG0201Add_Click" />
                                <aces:Btn ID="WFB2SG0201Delete" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_delete%>" Visible="false" OnClick="WFB2SG0201Delete_Click" OnClientClick="return doDelete();" />
                                <aces:Btn ID="WFB2SG0201OK" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_ok%>" Visible="false" ValidationGroup="GroupA" OnClick="WFB2SG0200OK_Click" />

                                <%--<asp:Button ID="WFB2SG0201Add" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_add%>" Visible="true" OnClick="WFB2SG0201Add_Click" />
                                <asp:Button ID="WFB2SG0201Delete" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_delete%>" Visible="false" OnClick="WFB2SG0201Delete_Click" OnClientClick="return doDelete();" />
                                <asp:Button ID="WFB2SG0201OK" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_ok%>" Visible="false" ValidationGroup="GroupA" OnClick="WFB2SG0200OK_Click" />--%>
                                <asp:Button ID="btn_cancel" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_cancel%>" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="btn_cancel_Click" />
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getDataDtl"
                SelectCountMethod="getCountDtl" TypeName="CFB2SG0200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="HiD_FESTIVAL_TYPE"
                        Name="festival_type" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="HID_EMP_CD"
                        Name="emp_cd" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_FESTIVAL_DT"
                        Name="festival_dt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_FESTIVAL_PAY_DT"
                        Name="festivalPayDT" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <%-- 
					//txt
					 <asp:ControlParameter ControlID="txt_APPLY_DT_S"
                        Name="appleDT_S" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
					//下拉式                   
					<asp:ControlParameter ControlID="ddl_ENV_ALLOWANCE_TYPE"
                        Name="env_type" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
					//radio
                    <asp:ControlParameter ControlID="rbl_USE_STATUS" DefaultValue=""
                        Name="is_valid" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
                    --%>
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" Width="20px" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" Width="20px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--序號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_rownumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--邏輯--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_festival_logic%>" HeaderStyle-Width="180px" ItemStyle-HorizontalAlign="Left" SortExpression="FESTIVAL_LOGIC">
                        <ItemTemplate>
                            <asp:Label ID="lb_FESTIVAL_LOGIC" runat="server" Text='<%#Bind("FESTIVAL_LOGIC")%>' Width="180px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_FESTIVAL_LOGIC" runat="server" Text='<%#Bind("FESTIVAL_LOGIC")%>' Width="180px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_NEW_FESTIVAL_LOGIC" runat="server" CssClass="MandatoryField"  Width="80px"></asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--欄位選項--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_calculate_item%>" HeaderStyle-Width="180px" ItemStyle-HorizontalAlign="Left" SortExpression="CALCULATE_ITEM">
                        <ItemTemplate>
                            <asp:Label ID="lb_CALCULATE_ITEM" runat="server" Text='<%#Bind("CALCULATE_ITEM_DESC")%>' Width="180px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_CALCULATE_ITEM" runat="server" Text='<%#Bind("CALCULATE_ITEM_DESC")%>' Width="180px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_NEW_CALCULATE_ITEM" runat="server" CssClass="MandatoryField" OnSelectedIndexChanged="ddl_NEW_CALCULATE_ITEM_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--條件--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_calculate_cond%>" HeaderStyle-Width="180px" ItemStyle-HorizontalAlign="Left" SortExpression="CALCULATE_COND">
                        <ItemTemplate>
                            <asp:Label ID="lb_CALCULATE_COND" runat="server" Text='<%#Bind("CALCULATE_COND")%>' Width="180px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_CALCULATE_COND" runat="server" Text='<%#Bind("CALCULATE_COND")%>' Width="180px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_NEW_CALCULATE_COND" runat="server" CssClass="MandatoryField" Width="80px" ></asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--內容1--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_calculate_content1%>" HeaderStyle-Width="180px" ItemStyle-HorizontalAlign="Left" SortExpression="CALCULATE_CONTENT1">
                        <ItemTemplate>
                            <asp:Label ID="lb_CALCULATE_CONTENT1" runat="server" Text='<%#Bind("CALCULATE_CONTENT1")%>' Width="180px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="lb_CALCULATE_CONTENT1" runat="server" Text='<%#Bind("FESTIVAL_DESC")%>' Width="180px"></asp:TextBox>
                            </ItemTemplate>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_NEW_CALCULATE_CONTENT1" runat="server"  Width="80px" ></asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--內容2--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_calculate_content2%>" HeaderStyle-Width="180px" ItemStyle-HorizontalAlign="center" SortExpression="CALCULATE_CONTENT2">
                        <ItemTemplate>
                            <asp:Label ID="lb_CALCULATE_CONTENT2" runat="server" Text='<%#Bind("CALCULATE_CONTENT2","{0:yyyy/MM/dd}")%>' Width="180px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_CALCULATE_CONTENT2" runat="server" Text='<%#Bind("CALCULATE_CONTENT2","{0:yyyy/MM/dd}")%>' Width="180px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_NEW_CALCULATE_CONTENT2" runat="server" MaxLength="10" Width="180px" ClientIDMode="Static" CssClass="date"></asp:TextBox>

                                <!--驗證日期格式-->
                                <asp:CustomValidator ID="NEW_CALCULATE_CONTENT2" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2sg_format_calculate_content2%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_NEW_CALCULATE_CONTENT2" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>

                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
                <%--當DB無資料時，就會使用此table --%>
                <EmptyDataTemplate>
                    <table class="grid-view" width="1020px">
                        <tr class="header">
                            <td>
                                <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" Width="20px" />
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_rownumber%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_festival_logic%>" Width="180px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_calculate_item%>" Width="180px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_calculate_cond%>" Width="180px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_calculate_content1%>" Width="180px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_calculate_content2%>" Width="180px"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td></td>
                            <td></td>
                            <td>
                                <asp:DropDownList ID="ddl_NEW_FESTIVAL_LOGIC" runat="server" CssClass="MandatoryField" Width="80px" ></asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_NEW_CALCULATE_ITEM" runat="server" CssClass="MandatoryField" OnSelectedIndexChanged="ddl_NEW_CALCULATE_ITEM_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_NEW_CALCULATE_COND" runat="server" CssClass="MandatoryField" Width="80px" ></asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_NEW_CALCULATE_CONTENT1" runat="server" Width="80px" ></asp:DropDownList>

                            </td>
                            <td>
                                <div style="text-align: left; width: 100%">
                                    <asp:TextBox ID="txt_NEW_CALCULATE_CONTENT2" runat="server" MaxLength="10" Width="100px" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>
                                    <!--驗證日期格式-->
                                    <asp:CustomValidator ID="NEW_CALCULATE_CONTENT2" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="<%$Resources:Resource,wfb2sg_format_calculate_content2%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                        ControlToValidate="txt_NEW_CALCULATE_CONTENT2" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                </div>
                            </td>
                    </table>
                </EmptyDataTemplate>
                <HeaderStyle CssClass="GridviewScrollHeader" />
                <PagerStyle CssClass="GridviewScrollPager" />

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

            <asp:HiddenField ID="HID_FESTIVAL_TYPE" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_EMP_CD" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_TARGET_GEN_DT" runat="server" ClientIDMode="Static" />
            <!-- 每頁的筆數 -->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <!-- 是否要凍結視窗 -->
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
