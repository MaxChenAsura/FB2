<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sh/WFB2SH0100_Qry.aspx.cs" Inherits="WebContent_WFB2SH0100_Qry" Culture="auto" UICulture="auto" %>

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

            reComma("AWARD_BASE", 3);
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
            $("#txt_AWARD").val("");
            $("#txt_LEVEL_CD").val("");
            $("#ddl_WS_CD").val("-1");
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
                    <col width="20%" />
                    <col width="10%" />
                    <col width="20%" />
                    <col width="10%" />
                    <col width="20%" />
                    <col width="10%" />

                </colgroup>
                <tbody>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--考績--%>
                            <asp:Label ID="Label10" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_award%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_AWARD" runat="server" Width="100px" ClientIDMode="Static" MaxLength="2"> </asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <%--資格--%>
                            <asp:Label ID="Label11" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_level_cd%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_LEVEL_CD" runat="server" Width="100px" ClientIDMode="Static" MaxLength="3"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <%--職種--%>
                            <asp:Label ID="Label12" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_ws_cd%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_WS_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th></th>
                        <td align="right" colspan="5">
                            
                            <aces:Btn ID="WFB2SH0100Search" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_search%>" OnClick="WFB2SH0100Search_Click" OnClientClick="CheckSearch();" />
                            <%--
                            <asp:Button ID="WFB2SH0100Search" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_search%>" OnClick="WFB2SH0100Search_Click" OnClientClick="CheckSearch();" />
                             --%>
                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2sh_btn_clear%>" onclick="ClearAll();" />
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

                                <aces:Btn ID="WFB2SH0100Add" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_add%>" Visible="true" OnClick="WFB2SH0100Add_Click" />
                                <aces:Btn ID="WFB2SH0100Delete" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_delete%>" Visible="false" OnClick="WFB2SH0100Delete_Click" OnClientClick="return doDelete();" />
                                <aces:Btn ID="WFB2SH0100Edit" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_edit%>" Visible="false" OnClick="WFB2SH0100Edit_Click" OnClientClick="BlockUI();" />
                                <aces:Btn ID="WFB2SH0100OK" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_ok%>" Visible="false" OnClick="WFB2SH0100OK_Click" OnClientClick="return saveCheck()" />
                                <%--
                                <asp:Button ID="WFB2SH0100Add" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_add%>" Visible="true" OnClick="WFB2SH0100Add_Click"  />
                                <asp:Button ID="WFB2SH0100Delete" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_delete%>" Visible="false" OnClick="WFB2SH0100Delete_Click" OnClientClick="return doDelete();" />
                                <asp:Button ID="WFB2SH0100Edit" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_edit%>" Visible="false" OnClick="WFB2SH0100Edit_Click" OnClientClick="BlockUI();" />
                                <asp:Button ID="WFB2SH0100OK" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_ok%>" Visible="false"  OnClick="WFB2SH0100OK_Click" OnClientClick="return saveCheck()"/>
                                --%>

                                <asp:Button ID="btn_cancel" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_cancel%>" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="btn_cancel_Click" />
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SH0100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="hid_qry_AWARD"
                        Name="award" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_qry_LEVEL_CD"
                        Name="level_cd" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_qry_WS_CD"
                        Name="ws_cd" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
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
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_rownumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--資格--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_level_cd%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left" SortExpression="LEVEL_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEVEL_CD" runat="server" Text='<%#Bind("LEVEL_CD")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_LEVEL_CD" runat="server" Text='<%#Bind("LEVEL_CD")%>' Width="80px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_NEW_LEVEL_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--職種--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_ws_cd%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="WS_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_WS_CD" runat="server" Text='<%#Bind("WS_CD_DESC")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_WS_CD" runat="server" Text='<%#Bind("WS_CD_DESC")%>' Width="100px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_NEW_WS_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--考績--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_award%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left" SortExpression="AWARD">
                        <ItemTemplate>
                            <asp:Label ID="lb_AWARD" runat="server" Text='<%#Bind("AWARD")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_AWARD" runat="server" Text='<%#Bind("AWARD")%>' Width="80px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_NEW_AWARD" runat="server" ClientIDMode="Static" CssClass="MandatoryField" Width="80px"></asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--年終格差--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_award_base%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right" SortExpression="AWARD_BASE">
                        <ItemTemplate>
                            <asp:Label ID="lb_AWARD_BASE" runat="server" Text='<%#Bind("AWARD_BASE")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_AWARD_BASE" runat="server" Text='<%#Bind("AWARD_BASE")%>' ClientIDMode="Static" Width="100px" CssClass="MandatoryField numFormat  decimal"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="EDIT_AWARD_BASE" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sh_required_award_base%>"
                                ControlToValidate="txt_EDIT_AWARD_BASE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_AWARD_BASE" runat="server" ClientIDMode="Static" Width="100px" CssClass="MandatoryField numFormat  decimal"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="NEW_AWARD_BASE" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sh_required_award_base%>"
                                ControlToValidate="txt_NEW_AWARD_BASE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--年終格差說明--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_award_desc%>" HeaderStyle-Width="500px" ItemStyle-HorizontalAlign="Left" SortExpression="AWARD_DESC">
                        <ItemTemplate>
                            <asp:Label ID="lb_AWARD_DESC" runat="server" Text='<%#Bind("AWARD_DESC")%>' Width="500px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_AWARD_DESC" runat="server" Text='<%#Bind("AWARD_DESC")%>' MaxLength="50" ClientIDMode="Static" Width="500px"></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_AWARD_DESC" runat="server" MaxLength="50" ClientIDMode="Static" Width="500px"> </asp:TextBox>
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
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_rownumber%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_level_cd%>" Width="80px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_ws_cd%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_award%>" Width="80px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_award_base%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_award_desc%>" Width="500px"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td></td>
                            <td></td>
                            <td>
                                <asp:DropDownList ID="ddl_NEW_LEVEL_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField" Width="80px"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_NEW_WS_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField" Width="100px"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_NEW_AWARD" runat="server" ClientIDMode="Static" CssClass="MandatoryField" Width="80px"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_AWARD_BASE" runat="server" ClientIDMode="Static" Width="100px" CssClass="MandatoryField numFormat  decimal"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="NEW_AWARD_BASE2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sh_required_award_base%>"
                                    ControlToValidate="txt_NEW_AWARD_BASE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_AWARD_DESC" runat="server" MaxLength="50" ClientIDMode="Static" Width="500px"> </asp:TextBox>
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
            <!-- 查詢條件 -->
            <asp:HiddenField ID="hid_qry_AWARD" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_qry_LEVEL_CD" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_qry_WS_CD" runat="server" ClientIDMode="Static" />

            <!-- 每頁的筆數 -->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <!-- 是否要凍結視窗 -->
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="N" />
            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
