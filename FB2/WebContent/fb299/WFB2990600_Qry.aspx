<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb299/WFB2990600_Qry.aspx.cs" Inherits="WebContent_WFB2990600_Qry" Culture="auto" UICulture="auto" %>

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
            $(".numFormat").mask('9.999');
            $(".decimal").css("text-align", "right").css("ime-mode", "disabled");

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
                    freezesize: 0

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
            var processed = true;
            BlockUI();
            if (checkboxsSelected() > 0) {
                processed = confirm("確定要刪除?");
            } else {
                alert("請選取資料!");
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
            BlockUI();
            //其它需要檢核的
            /*
            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            */
        }

        ////將DIV的scollbar跑到最低,移到Basic.js
        //function gridViewScrollBottom(id) {
        //    $("table[id$="+id+"]").parent().scrollTop(99999);
        //}


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
            $("#ddl_TOVRCD").val("-1");
            $("#txt_T5AWC").val("");
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
                    <col width="15%" />
                    <col width="35%" />
                    <col width="15%" />
                    <col width="35%" />

                </colgroup>
                <tbody>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--新加班類型--%>
                            <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb299_lb_tovrcd%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_TOVRCD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                       <th align="left" class="Body_TableHeader">
                            <%--舊加班類型--%>
                            <asp:Label ID="Label12" runat="server" Text="<%$Resources:Resource,wfb299_lb_t5awc%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_T5AWC" runat="server" Width="70px" ClientIDMode="Static" MaxLength="1" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th></th>
                        <td align="right" colspan="4">
                            
                            <aces:Btn ID="WFB2990600Search" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_search%>" OnClick="WFB2990600Search_Click" OnClientClick="CheckSearch();" />
                            <%--
                            <asp:Button ID="WFB2990600Search" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_search%>" OnClick="WFB2990600Search_Click" OnClientClick="CheckSearch();" />
                             --%>
                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2sh_btn_clear%>" onclick="ClearAll();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="Body_label" colspan="4">
                            <div id="init_grid">
                                <aces:Btn ID="WFB2990600Add" runat="server" Text="新增" Visible="true" OnClick="WFB2990600Add_Click" />
                                <aces:Btn ID="WFB2990600Delete" runat="server" Text="刪除" Visible="false" OnClick="WFB2990600Delete_Click" OnClientClick="return doDelete();" />
                                <aces:Btn ID="WFB2990600Edit" runat="server" Text="修改" Visible="false" OnClick="WFB2990600Edit_Click" OnClientClick="BlockUI();" />
                                <aces:Btn ID="WFB2990600OK" runat="server" Text="確認" Visible="false" OnClick="WFB2990600OK_Click" OnClientClick="return saveCheck()" />
                                <asp:Button ID="btn_cancel" runat="server" Text="取消" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="btn_cancel_Click" />
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2990600DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="ddl_TOVRCD"
                        Name="tovrcd" PropertyName="selectedValue" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_T5AWC"
                        Name="t5awc" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" Width="40px" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" Width="40px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--序號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_rownumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--新加班類型--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_lb_tovrcd%>" HeaderStyle-Width="500px" ItemStyle-Width="500px" ItemStyle-HorizontalAlign="Left" SortExpression="TOVRCD">
                        <ItemTemplate>
                            <asp:Label ID="lb_TOVRCD" runat="server" Text='<%#Bind("TOVRCD_DESC")%>' Width="500px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_TOVRCD" runat="server" Text='<%#Bind("TOVRCD_DESC")%>' Width="500px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                             <asp:DropDownList ID="ddl_EWN_TOVRCD" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--舊加班類型--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_lb_t5awc%>" HeaderStyle-Width="400px" ItemStyle-Width="400px" ItemStyle-HorizontalAlign="Left" SortExpression="T5AWC">
                        <ItemTemplate>
                            <asp:Label ID="lb_T5AWC" runat="server" Text='<%#Bind("T5AWC")%>' Width="400px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                             <asp:TextBox  ID="txt_EDIT_T5AWC"  runat="server"  Text='<%#Bind("T5AWC")%>'  MaxLength="1" CssClass="MandatoryField" style="TEXT-TRANSFORM:uppercase"  />
                             <asp:RequiredFieldValidator ID="EDIT_T5AWC" runat="server" ErrorMessage="<%$Resources:Resource,wfb299_required_t5awc%>"
                                ControlToValidate="txt_EDIT_T5AWC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox  ID="txt_NEW_T5AWC"  runat="server" MaxLength="1" CssClass="MandatoryField"  style="TEXT-TRANSFORM:uppercase"  />
                             <asp:RequiredFieldValidator ID="NEW_T5AWC" runat="server" ErrorMessage="<%$Resources:Resource,wfb299_required_t5awc%>"
                                ControlToValidate="txt_NEW_T5AWC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    
                </Columns>
                <%--當DB無資料時，就會使用此table --%>
                <EmptyDataTemplate>
                    <table class="grid-view" width="1020px">
                        <tr class="header">
                            <td>
                                <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" Width="40px" />
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_rownumber%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb299_lb_tovrcd%>" Width="500px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb299_lb_t5awc%>" Width="400px"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td></td>
                            <td></td>
                            <td>
                                  <asp:DropDownList ID="ddl_EWN_TOVRCD" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                            </td>
                            <td>
                               <asp:TextBox  ID="txt_NEW_T5AWC"  runat="server" MaxLength="1" CssClass="MandatoryField"  style="TEXT-TRANSFORM:uppercase"  />
                                 <asp:RequiredFieldValidator ID="NEW_T5AWC" runat="server" ErrorMessage="<%$Resources:Resource,wfb299_required_t5awc%>"
                                    ControlToValidate="txt_NEW_T5AWC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
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
            <asp:HiddenField ID="hid_today" runat="server" ClientIDMode="Static" />
            
            <!-- 每頁的筆數 -->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <!-- 是否要凍結視窗 -->
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="N" />
            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />


        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
