<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sg/WFB2SG0300_Qry.aspx.cs" Inherits="WebContent_WFB2SG0300_Qry" Culture="auto" UICulture="auto" %>
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

            //期間的預設值
            //var todayDate = getTodayDate();
            //$("#txt_APPLY_DT_S").val(todayDate);
            //$("#txt_APPLY_DT_E").val(todayDate);


            reComma("ALLOWANCE_VALUE", 0);
            //GridView必須
            //gridviewScroll();
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

        //提出核可
        function CheckRelease() {
            var processed = true;
            BlockUI();
            if (checkboxsSelected() == 1) {
                processed = confirm("確定要提出核可？");
            }
            else {
                alert("請選取一筆資料!");
                choietr = null;
                processed = false;
            }
            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }

        //薪資轉出
        function CheckAnnounce() {
            var processed = true;
            BlockUI();
            if (checkboxsSelected() == 1) {
                /*
                choietr = choietr - 1;
                //對象生成
                if ($("[id=hid_TARGET_GEN_DT]")[choietr].value == "") {
                    alert("請先進行對象生成或上傳!");
                    processed = false;
                } else if ($("[id=hid_APPROVE_STATUS]")[choietr].value != "Y") {
                    alert("請先進行核可作業！");
                    processed = false;
                }else{
                    processed = confirm("確定要進行薪資轉出？ [ !!!注意：薪資發佈後資料無法進行修改]");
                }
                */
                processed = confirm("確定要進行薪資轉出？ [ !!!注意：薪資發佈後資料無法進行修改]");
            }
            else {
                alert("請選取一筆資料!");
                choietr = null;
                processed = false;
            }
            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }

        var choietr = null;
        //回傳目前Checkbox被勾選的數量
        function checkboxsSelected() {
            choietr = null;
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

        //清空畫面
        function ClearAll() {
            $("#ddl_FESTIVAL_TYPE").val("-1");
            $("#txt_FESTIVAL_DT_S").val("");
            $("#txt_FESTIVAL_DT_E").val("");
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
                            <%--節金類別--%>
                            <asp:Label ID="Label10" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_festival_type%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_FESTIVAL_TYPE" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--節日日期區間--%>
                            <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_festival_dt_s_e%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_FESTIVAL_DT_S" runat="server" Width="100px" MaxLength="10" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                            ~
                            <asp:TextBox ID="txt_FESTIVAL_DT_E" runat="server" Width="100px" MaxLength="10" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                            <!--驗證日期格式-->
                            <asp:CustomValidator ID="qry_txt_FESTIVAL_DT_S" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sg_format_festival_dt_s%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_FESTIVAL_DT_S" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>

                            <asp:CustomValidator ID="qry_txt_FESTIVAL_DT_E" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sg_format_festival_dt_e%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_FESTIVAL_DT_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <!--起日不可大於迄日 -->
                            <asp:CompareValidator ID="qry5" runat="server" ControlToCompare="txt_FESTIVAL_DT_S"
                                ControlToValidate="txt_FESTIVAL_DT_E" ErrorMessage="<%$Resources:Resource,wfb2sg_error_festival_dt_s_e%>" Type="Date" Operator="GreaterThanEqual"
                                Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                        </td>
                        <th></th>
                        <td></td>
                    </tr>
                    <tr>
                        <th></th>
                        <td align="right" colspan="5">
                             
                            <aces:Btn ID="WFB2SG0300Search"  runat="server" Text="<%$Resources:Resource,wfb2sg_btn_search%>" OnClick="WFB2SG0300Search_Click" OnClientClick="CheckSearch();" />
                             <%--    
                            <asp:Button ID="WFB2SG0300Search" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_search%>" OnClick="WFB2SG0300Search_Click" OnClientClick="CheckSearch();" />
                           --%>
                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2sg_btn_clear%>" onclick="ClearAll();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                        <tr>
                            <td align="right" class="Body_label" colspan="6">
                                <div id="init_grid">
                                    <aces:Btn ID="WFB2SG0300Release" runat="server" OnClick="WFB2SG0300Release_Click" OnClientClick="return CheckRelease();" Text="<%$Resources:Resource,wfb2sg_btn_release%>" Visible="false" />
                                    <aces:Btn ID="WFB2SG0300Announce" runat="server" OnClick="WFB2SG0300Announce_Click" OnClientClick="return CheckAnnounce();" Text="<%$Resources:Resource,wfb2sg_btn_announce%>" Visible="false" />
                                    <%--
                                    <asp:Button ID="WFB2SG0300Release" runat="server" OnClick="WFB2SG0300Release_Click" OnClientClick="return CheckRelease();" Text="<%$Resources:Resource,wfb2sg_btn_release%>" Visible="false" />
                                    <asp:Button ID="WFB2SG0300Announce" runat="server" OnClick="WFB2SG0300Announce_Click" OnClientClick="return CheckAnnounce();" Text="<%$Resources:Resource,wfb2sg_btn_announce%>" Visible="false" />
                                     --%>
                                </div>
                            </td>
                        </tr>
                </tbody>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SG0300DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="hid_qry_FESTIVAL_TYPE"
                        Name="festival_type" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_qry_FESTIVAL_DT_S"
                        Name="festival_dt_s" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_qry_FESTIVAL_DT_E"
                        Name="festival_dt_e" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectNonDisabledCheckboxes(this);" ClientIDMode="Static" Width="20px" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" Width="20px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--序號--%>
                    <asp:TemplateField HeaderText="序號" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="60px"></asp:Label>
                            <!-- 資料凍結註記-->
                            <asp:HiddenField ID="hid_FREEZE_FLAG" runat="server" Value='<%#Bind("FREEZE_FLAG")%> ' ClientIDMode="Static" />
                        </ItemTemplate>
                        <EditItemTemplate>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--節金類別--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_festival_type%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="FESTIVAL_TYPE">
                        <ItemTemplate>
                            <asp:Label ID="lb_FESTIVAL_TYPE" runat="server" Text='<%#Bind("FESTIVAL_TYPE_DESC")%>' Width="100px"></asp:Label>
                            <asp:HiddenField ID="hid_FESTIVAL_TYPE" runat="server" Value='<%#Bind("FESTIVAL_TYPE")%> ' ClientIDMode="Static" />

                        </ItemTemplate>
                        <EditItemTemplate>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--節日日期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_festival_dt%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="FESTIVAL_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_FESTIVAL_DT" runat="server" Text='<%#Bind("FESTIVAL_DT","{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                            <asp:HiddenField ID="hid_FESTIVAL_DT" runat="server" Value='<%#Bind("FESTIVAL_DT","{0:yyyy/MM/dd}")%> ' ClientIDMode="Static" />
                        </ItemTemplate>
                        <EditItemTemplate>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--節金說明
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_festival_desc%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="FESTIVAL_DESC">
                        <ItemTemplate>
                            <asp:Label ID="FESTIVAL_DESC" runat="server" Text='<%#Bind("FESTIVAL_DESC")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    --%>
                    <%--節金發放日期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_festival_pay_dt%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="center" SortExpression="FESTIVAL_PAY_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_FESTIVAL_PAY_DT" runat="server" Text='<%#Bind("FESTIVAL_PAY_DT","{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                            <asp:HiddenField ID="hid_FESTIVAL_PAY_DT" runat="server" Value='<%#Bind("FESTIVAL_PAY_DT","{0:yyyy/MM/dd}")%> ' ClientIDMode="Static" />
                        </ItemTemplate>
                        <EditItemTemplate>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--對象生成日--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_target_gen_dt%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="center" SortExpression="TARGET_GEN_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_TARGET_GEN_DT" runat="server" Text='<%#Bind("TARGET_GEN_DT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                            <asp:HiddenField ID="hid_TARGET_GEN_DT" runat="server" Value='<%#Bind("TARGET_GEN_DT", "{0:yyyy/MM/dd}")%> ' ClientIDMode="Static" />
                        </ItemTemplate>
                        <EditItemTemplate>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--提出核可日--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_release_dt%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="center" SortExpression="RELEASE_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_RELEASE_DT" runat="server" Text='<%#Bind("RELEASE_DT","{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                            <asp:HiddenField ID="hid_RELEASE_DT" runat="server" Value='<%#Bind("RELEASE_DT","{0:yyyy/MM/dd}")%> ' ClientIDMode="Static" />
                            <!-- 核可狀態 -->
                            <asp:HiddenField ID="hid_APPROVE_STATUS" runat="server" Value='<%#Bind("APPROVE_STATUS")%> ' ClientIDMode="Static" />
                        </ItemTemplate>
                        <EditItemTemplate>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                  <%--核可日期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_approve_dt%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="center" SortExpression="APPROVE_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_APPROVE_DT" runat="server" Text='<%#Bind("APPROVE_DT","{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--薪資轉出日--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_salary_trans_dt%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="center" SortExpression="SALARY_TRANS_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_SALARY_TRANS_DT" runat="server" Text='<%#Bind("SALARY_TRANS_DT","{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--功能--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_function%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                              
                            <aces:Btn ID="WFB2SG0300Detail" runat="server"
                                CommandName="ToDetail"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                Text="<%$Resources:Resource,wfb2sg_btn_detail%>" />
                          <%--
                            <asp:Button ID="WFB2SG0300Detail" runat="server"
                                CommandName="ToDetail"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                Text="<%$Resources:Resource,wfb2sg_btn_detail%>" />
                             --%>
                        </ItemTemplate>
                        <EditItemTemplate>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
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
            <!-- 查詢條件 -->
            <asp:HiddenField ID="hid_qry_FESTIVAL_TYPE" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_qry_FESTIVAL_DT_S" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_qry_FESTIVAL_DT_E" runat="server" ClientIDMode="Static" />

            <!-- 每頁的筆數 -->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <!-- 是否要凍結視窗 -->
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
