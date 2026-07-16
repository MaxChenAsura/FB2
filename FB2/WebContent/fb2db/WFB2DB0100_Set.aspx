<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2db/WFB2DB0100_Set.aspx.cs" Inherits="WebContent_WFB2DB0100_Set" Culture="auto" UICulture="auto" %>

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
            $(".amt").mask('999');
            $(".decimal").css("text-align", "right");
            //$(".decimal").css("text-align", "right").css("ime-mode", "disabled");

            reComma("ALLOWANCE_VALUE", 0);
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
            var processed = true;
            BlockUI();
            if (checkboxsSelected() > 0) {
                processed = confirm("確定要刪除?");
            } else {
                alert("請選擇資料");
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
                return;  //不然按取消需按二次
            }
            return processed;
        }

        //清空畫面
        function ClearAll() {
            $("#txt_RULE_CD").val("");
            $("#txt_RULE_DESC").val("");
        }

        //班別選單回傳值
        function Shift_Search(eid, ename, value) {
            var returnValue = value;
            if (value == undefined) {
                returnValue = 'undefined';
            }
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                $("#txt_NEW_SHIFT_CD").val(obj.CD);
                $("#txt_NEW_SHIFT_DESC").val(obj.DESC);
            }
        }
        //檢查班別是否存在
        function SHIFT_CD_Check(value) {
            if ($("#txt_NEW_SHIFT_CD").val().length == 2)
                __doPostBack('question', 'shiftCD');
        }
        function getRULE_DESC(value) {
            if ($("#txt_NEW_RULE_CD").val().length == 3)
                __doPostBack('question', 'ruleCD');
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
                            <%--循環規則代碼--%>
                            <asp:Label ID="Label10" runat="server" Text="<%$Resources:Resource,wfb2db_lb_rule_cd%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_RULE_CD" runat="server" Width="100px" MaxLength="3" ClientIDMode="Static"> </asp:TextBox>

                        </td>
                        <th align="left" class="Body_TableHeader">
                            <%--循環規則說明--%>
                            <asp:Label ID="Label11" runat="server" Text="<%$Resources:Resource,wfb2db_lb_rule_desc%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_RULE_DESC" runat="server" Width="100px" ClientIDMode="Static"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th></th>
                        <td align="right" colspan="5">
                            <aces:Btn ID="WFB2DB0104Search" runat="server" Text="<%$Resources:Resource,wfb2db_btn_search%>" OnClick="WFB2DB0104Search_Click" />
                            <%-- 
                                <asp:Button ID="WFB2DB0104Search" runat="server" Text="查詢" OnClick="WFB2DB0104Search_Click" />
                            --%>
                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2db_btn_clear%>" onclick="ClearAll();" />
                            <asp:Button ID="btn_Back" runat="server" Text="<%$Resources:Resource,wfb2db_Back%>" ClientIDMode="Static" OnClick="btn_Back_Click" />
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
                                <aces:Btn ID="WFB2DB0104Add" runat="server" Text="<%$Resources:Resource,wfb2db_btn_add%>" Visible="true" OnClick="WFB2DB0104Add_Click"  />
                                <aces:Btn ID="WFB2DB0104Delete" runat="server" Text="<%$Resources:Resource,wfb2db_btn_delete%>" Visible="false" OnClick="WFB2DB0104Delete_Click" OnClientClick="return doDelete();" />
                                <aces:Btn ID="WFB2DB0104Edit" runat="server" Text="<%$Resources:Resource,wfb2db_btn_edit%>" Visible="false" OnClick="WFB2DB0104Edit_Click" OnClientClick="BlockUI();" />
                                <aces:Btn ID="WFB2DB0104Save" runat="server" Text="<%$Resources:Resource,wfb2db_btn_ok%>" Visible="false" OnClick="WFB2DB0104Save_Click" OnClientClick="saveCheck()" />
                                <%-- -
                                <asp:Button ID="WFB2DB0104Add" runat="server" Text="新增" Visible="true" OnClick="WFB2DB0104Add_Click" OnClientClick="return CheckSearch();" />
                                <asp:Button ID="WFB2DB0104Delete" runat="server" Text="刪除" Visible="false" OnClick="WFB2DB0104Delete_Click" OnClientClick="return doDelete();" />
                                <asp:Button ID="WFB2DB0104Edit" runat="server" Text="修改" Visible="false" OnClick="WFB2DB0104Edit_Click" OnClientClick="BlockUI();" />
                                <asp:Button ID="WFB2DB0104Save" runat="server" Text="確認" Visible="false" OnClick="WFB2DB0104save_Click" OnClientClick="saveCheck()" />    
                                 --%>

                                <asp:Button ID="btn_cancel" runat="server" Text="<%$Resources:Resource,wfb2db_btn_cancel%>" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="btn_cancel_Click" />
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getSetData"
                SelectCountMethod="getSetCount" TypeName="WFB2DB0100DL" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_RULE_CD"
                        Name="ruleCD" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_RULE_DESC"
                        Name="ruleDesc" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1018px"
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
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_lb_rownumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--循環規則代碼--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_lb_rule_cd%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="RULE_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_RULE_CD" runat="server" Text='<%#Bind("RULE_CD")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                           <asp:Label ID="lb_RULE_CD" runat="server" Text='<%#Bind("RULE_CD")%>' Width="100px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_RULE_CD" runat="server" ClientIDMode="Static" MaxLength="3" Width="100px" CssClass="MandatoryField" Style="TEXT-TRANSFORM: uppercase" onkeyup="value=value.replace(/[\W]/g,'');getRULE_DESC(this.value);"  onblur="getRULE_DESC(this.value);"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2db_required_rule_cd%>"
                                ControlToValidate="txt_NEW_RULE_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="onlyEngNum" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2db_format_rule_cd%>" ControlToValidate="txt_NEW_RULE_CD" ForeColor="Red" ValidationGroup="GroupA"
                                ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--循環規則說明--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_lb_rule_desc%>" HeaderStyle-Width="300px" ItemStyle-HorizontalAlign="Left" SortExpression="RULE_DESC">
                        <ItemTemplate>
                            <asp:Label ID="lb_RULE_DESC" runat="server" Text='<%#Bind("RULE_DESC")%>' Width="300px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_NEW_RULE_DESC" Text='<%#Bind("RULE_DESC")%>' runat="server" ClientIDMode="Static" MaxLength="60" Width="300px"></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_RULE_DESC" runat="server" ClientIDMode="Static" MaxLength="60" Width="300px"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--流水序號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_lb_rule_seq%>" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" SortExpression="RULE_SEQ">
                        <ItemTemplate>
                            <asp:Label ID="lb_RULE_SEQ" runat="server" Text='<%#Bind("RULE_SEQ")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_RULE_SEQ" runat="server" Text='<%#Bind("RULE_SEQ")%>' Width="60px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--班別--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_lb_shift_cd%>" HeaderStyle-Width="300px" ItemStyle-HorizontalAlign="Left" SortExpression="SHIFT_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_SHIFT_CD" runat="server" Text='<%#Bind("SHIFT_DESC")%>' Width="300px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_NEW_SHIFT_CD" Text='<%#Bind("Edit_SHIFT_CD")%>' runat="server" CssClass="MandatoryField" Width="40px" MaxLength="2" onblur="SHIFT_CD_Check(this.value);" onKeyUp="SHIFT_CD_Check(this.value);"></asp:TextBox>
                            <input id="btn_SHIFT_CD_Edit" type="button" value="..." onclick="OpenSearch('Shift_Search.aspx', 'txt_NEW_SHIFT_CD', 'txt_NEW_SHIFT_CD', '', 'Y'); return false;" />
                            <asp:TextBox ID="txt_NEW_SHIFT_DESC" Text='<%#Bind("Edit_SHIFT_DESC")%>' runat="server" Enabled="false" Width="200px"  />
                            <asp:RequiredFieldValidator ID="vaild_NEW_SHIFT_CD" runat="server" ErrorMessage="<%$Resources:Resource,wfb2db_required_shift_cd%>"
                                ControlToValidate="txt_NEW_SHIFT_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="vaild_NEW_SHIFT_DESC" runat="server" ErrorMessage="<%$Resources:Resource,wfb2db_error_shift_cd%>"
                                ControlToValidate="txt_NEW_SHIFT_DESC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_SHIFT_CD" runat="server" CssClass="MandatoryField" Width="40px" MaxLength="2" onblur="SHIFT_CD_Check(this.value);" onKeyUp="SHIFT_CD_Check(this.value);"></asp:TextBox>
                            <input id="btn_SHIFT_CD_Edit" type="button" value="..." onclick="OpenSearch('Shift_Search.aspx', 'txt_NEW_SHIFT_CD', 'txt_NEW_SHIFT_CD', '', 'Y'); return false;" />
                            <asp:TextBox ID="txt_NEW_SHIFT_DESC" runat="server" Enabled="false" Width="200px" />
                            <asp:RequiredFieldValidator ID="vaild_NEW_SHIFT_CD" runat="server" ErrorMessage="<%$Resources:Resource,wfb2db_required_shift_cd%>"
                                ControlToValidate="txt_NEW_SHIFT_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="vaild_NEW_SHIFT_DESC" runat="server" ErrorMessage="<%$Resources:Resource,wfb2db_error_shift_cd%>"
                                ControlToValidate="txt_NEW_SHIFT_DESC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--循環天數--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_lb_circle_days%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" SortExpression="CIRCLE_DAYS">
                        <ItemTemplate>
                            <asp:Label ID="lb_CIRCLE_DAYS" runat="server" Text='<%#Bind("CIRCLE_DAYS")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_NEW_CIRCLE_DAYS" Text='<%#Bind("CIRCLE_DAYS")%>' runat="server" ClientIDMode="Static" MaxLength="3" Width="80px" CssClass="MandatoryField  decimal"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="vaild_NEW_CIRCLE_DAYS" runat="server" ErrorMessage="<%$Resources:Resource,wfb2db_required_circle_days%>"
                                ControlToValidate="txt_NEW_CIRCLE_DAYS" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="Custom_NEW_CIRCLE_DAYS" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2db_format_circle_days%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_CIRCLE_DAYS" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_CIRCLE_DAYS" runat="server" ClientIDMode="Static" MaxLength="3" Width="80px" CssClass="MandatoryField  decimal"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="vaild_NEW_CIRCLE_DAYS" runat="server" ErrorMessage="<%$Resources:Resource,wfb2db_required_circle_days%>"
                                ControlToValidate="txt_NEW_CIRCLE_DAYS" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="Custom_NEW_CIRCLE_DAYS" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2db_format_circle_days%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_CIRCLE_DAYS" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--是否含假日--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_lb_is_include_holiday%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" SortExpression="IS_INCLUDE_HOLIDAY">
                        <ItemTemplate>
                            <asp:Label ID="lb_IS_INCLUDE_HOLIDAY" runat="server" Text='<%#Bind("IS_INCLUDE_HOLIDAY_DESC")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_IS_INCLUDE_HOLIDAY" runat="server" CssClass="MandatoryField" Width="60px"></asp:DropDownList>
                            <asp:HiddenField ID="hid_IS_INCLUDE_HOLIDAY" runat="server" Value='<%#Bind("IS_INCLUDE_HOLIDAY")%> ' ClientIDMode="Static" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_IS_INCLUDE_HOLIDAY" runat="server" CssClass="MandatoryField" Width="60px"></asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
                <%--當查詢條件,無資料時，就會使用此table --%>
                <EmptyDataTemplate>
                    <table class="grid-view" width="1020px">
                        <tr class="header">
                            <td>
                                <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" Width="20px" />
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2db_lb_rownumber%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lb_RULE_CD" runat="server" Text='<%$Resources:Resource,wfb2db_lb_rule_cd%>' Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lb_RULE_DESC" runat="server" Text='<%$Resources:Resource,wfb2db_lb_rule_desc%>' Width="300px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lb_RULE_SEQ" runat="server" Text='<%$Resources:Resource,wfb2db_lb_rule_seq%>' Width="60px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lb_SHIFT_CD" runat="server" Text='<%$Resources:Resource,wfb2db_lb_shift_cd%>' Width="300px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lb_CIRCLE_DAYS" runat="server" Text='<%$Resources:Resource,wfb2db_lb_circle_days%>' Width="80px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lb_IS_INCLUDE_HOLIDAY" runat="server" Text='<%$Resources:Resource,wfb2db_lb_is_include_holiday%>' Width="80px"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td></td>
                            <td></td>
                            <td>
                                <asp:TextBox ID="txt_NEW_RULE_CD" runat="server" ClientIDMode="Static" MaxLength="3" Width="100px" CssClass="MandatoryField" Style="TEXT-TRANSFORM: uppercase" onkeyup="value=value.replace(/[\W]/g,'')"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2db_required_rule_cd%>"
                                    ControlToValidate="txt_NEW_RULE_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="onlyEngNum" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2db_format_rule_cd%>" ControlToValidate="txt_NEW_RULE_CD" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_RULE_DESC" runat="server" ClientIDMode="Static" MaxLength="60" Width="300px" ></asp:TextBox>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_SHIFT_CD" runat="server" CssClass="MandatoryField" Width="40px" MaxLength="2" onblur="SHIFT_CD_Check(this.value);" onKeyUp="SHIFT_CD_Check(this.value);"></asp:TextBox>
                                <input id="btn_SHIFT_CD_Edit" type="button" value="..." onclick="OpenSearch('Shift_Search.aspx', 'txt_NEW_SHIFT_CD', 'txt_NEW_SHIFT_CD', '', 'Y'); return false;" />
                                <asp:TextBox ID="txt_NEW_SHIFT_DESC" runat="server" Enabled="false" />
                                <asp:RequiredFieldValidator ID="vaild_NEW_SHIFT_CD" runat="server" ErrorMessage="<%$Resources:Resource,wfb2db_required_shift_cd%>"
                                    ControlToValidate="txt_NEW_SHIFT_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="vaild_NEW_SHIFT_DESC" runat="server" ErrorMessage="<%$Resources:Resource,wfb2db_error_shift_cd%>"
                                    ControlToValidate="txt_NEW_SHIFT_DESC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_CIRCLE_DAYS" runat="server" ClientIDMode="Static" MaxLength="3" Width="80px" CssClass="MandatoryField  decimal"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="vaild_NEW_CIRCLE_DAYS" runat="server" ErrorMessage="<%$Resources:Resource,wfb2db_required_circle_days%>"
                                    ControlToValidate="txt_NEW_CIRCLE_DAYS" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="Custom_NEW_CIRCLE_DAYS" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2db_format_circle_days%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_CIRCLE_DAYS" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_IS_INCLUDE_HOLIDAY" runat="server" CssClass="MandatoryField" Width="60px"></asp:DropDownList>
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

            <!-- 每頁的筆數 -->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <!-- 是否要凍結視窗 -->
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
