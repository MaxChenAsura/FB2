<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dc/action/WFB2DC0400_Qry.aspx.cs" Inherits="WebContent_fb2dc_WFB2DC0400_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');
            gridviewScroll();
            $.unblockUI();
        }

        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());

        }
        function gridviewScroll() {

            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "400",
                barcolor: "#7F7F7F"

            });

        }

        //清空畫面
        function ClearAll() {
            $("#ddl_CARD_TYPE").val("-1");
            $("#txt_CARD_NO").val("");
            $("#ddl_select_CARD_HANDLE").val("-1");
            $("#txt_CHANGE_DT").val("");
            $("#ddl_CHANGE_DT").val("-1");
            $('#cb_CARD_STATUS1').prop("checked", false);
            $('#cb_CARD_STATUS2').prop("checked", false);
            $("#txt_CHANGE_DT").val("");
            
            return false;
        }

        function CheckSearch() {

        }
        //儲存前檢查
        function saveCheck() {
            var processed = true;
            var msg = "";
            if (Page_ClientValidate("GroupA")) {
                if (Page_IsValid != null && Page_IsValid == true) {
                    if ($("#hid_mod").val() == "add") {
                        //新增模式

                        var re = /^[\d|a-zA-Z]{5}$/;
                        if (!re.test($("#txt_CARD_MID_NO").val()))
                            msg += "卡號限輸入英數字五碼，不可為中文字\r\n";

                        var START_DT = new Date($('#txt_START_DT').val());
                        var END_DT = new Date($('#txt_END_DT').val());
                        if (START_DT > END_DT)
                            msg += "生效日期不得大於結束日期\r\n";

                        var lb_CARD_USED_CD = $("#lb_add_CARD_USED_CD").text();
                        var CARD_USED_CD = lb_CARD_USED_CD.split("-");
                        if (CARD_USED_CD[0] == "A" || CARD_USED_CD[0] == "B") {
                            if ($("#txt_PERSON_ID").val() == "")
                                msg += "工號/廠商人員編號不可空白\r\n";
                        }
                        else {
                            if ($("#txt_CARD_NAME").val() == "")
                                msg += "使用對象為共用，姓名不可空白\r\n";
                            if ($("#ddl_TEMP_CARD_CD").val() == "-1")
                                msg += "使用對象為共用，臨時卡區分不可空白\r\n";
                        }
                    }
                    else {
                        //修改模式
                        var START_DT = new Date($('#txt_START_DT').val());
                        var END_DT = new Date($('#txt_END_DT').val());
                        if (START_DT > END_DT)
                            msg += "生效日期不得大於結束日期\r\n";

                        var lb_CARD_USED_CD = $("#lb_edit_CARD_USED_CD").text();
                        var CARD_USED_CD = lb_CARD_USED_CD.split("-");
                        if (CARD_USED_CD[0] != "A" && CARD_USED_CD[0] != "B") {
                            if ($("#txt_CARD_NAME").val() == "")
                                msg += "使用對象為共用，姓名不可空白\r\n";
                            if ($("#ddl_TEMP_CARD_CD").val() == "-1")
                                msg += "使用對象為共用，臨時卡區分不可空白\r\n";
                        }
                    }
                }
                if (msg.trim().length > 0) {
                    $.unblockUI();
                    alert(msg);
                    return false;
                }
                else {
                    BlockUI();
                    return true;
                }
            }
            else {
                $.unblockUI();
                return false;
            }
        }
        function openSearch() {
            if ($("#rbl_EMP_TYPE input:radio:checked").val() == "1") {
                var json = OpenEmpSearch('txt_PERSON_ID', 'txt_PERSON_NAME', 'N');
            }
            else {
                var json = OpenSearch('Vendor_d_Search.aspx', 'txt_PERSON_ID', 'txt_PERSON_NAME', 'VENDOR_NO=' + $("#txt_PERSON_ID").val());

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

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020px" border="0" class="Body_Label">
                <colgroup>
                    <col width="15%" />
                    <col width="30%" />
                    <col width="15%" />
                    <col width="15%" />
                    <col width="10%" />
                    <col width="15%" />
                </colgroup>
                <tbody>
                    <tr>
                        <%-- 卡片屬性 --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_CARD_TYPE" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CARD_TYPE%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_QRY_CARD_TYPE" runat="server" ClientIDMode="Static"></asp:DropDownList>

                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_CARD_NO" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CARD_NO%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_QRY_CARD_NO" runat="server" MaxLength="5" Width="90px" ClientIDMode="Static" Style="text-transform: uppercase" OnTextChanged="txt_CARD_NO_TextChanged"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2dc_Required_CARD_MID_NO%>" ControlToValidate="txt_QRY_CARD_NO" ForeColor="Red"
                                ValidationExpression="([0-9|a-zA-Z]{5})" Display="None" ValidationGroup="GroupA">
                            </asp:RegularExpressionValidator>
                        </td>
                        </td>
                         <%-- 姓名 --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_CARD_NAME" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CARD_NAME%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_QRY_CARD_NAME" runat="server" Width="90px" ClientIDMode="Static" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_CARD_STATUS" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CARD_STATUS%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:CheckBox ID="cb_CARD_STATUS1" runat="server" Text="註銷" ClientIDMode="Static" />
                            <asp:CheckBox ID="cb_CARD_STATUS2" runat="server" Text="有效" ClientIDMode="Static" />

                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_CARD_HANDLE" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CARD_HANDLE%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_select_CARD_HANDLE" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                        <th></th>
                        <td></td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_CHANGE_DT" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CHANGE_DT%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_CHANGE_DT" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                            <asp:CustomValidator ID="qrytest1" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CHANGE_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_CHANGE_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:DropDownList ID="ddl_CHANGE_DT" runat="server" ClientIDMode="Static">
                                <asp:ListItem Text="" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="1.入社日期" Value="1"></asp:ListItem>
                                <asp:ListItem Text="2.任現資格日" Value="2"></asp:ListItem>
                                <asp:ListItem Text="3.任現職務日" Value="3"></asp:ListItem>
                                <asp:ListItem Text="4.任現部級部門日" Value="4"></asp:ListItem>
                            </asp:DropDownList>

                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_PLANT_CD" runat="server" Text="<%$Resources:Resource,wfb2dc_PLANT_CD%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_PLANT_CD" runat="server" ClientIDMode="Static">
                            </asp:DropDownList>
                        </td>
                         <th></th>
                        <td></td>
                    </tr>
                    <tr>
                        <td align="right" class="Body_label" colspan="6">
                            <div id="init">
                                <aces:Btn ID="WFB2DC0400Search" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2DC0400Search%>" OnClick="WFB2DC0400Search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />

                                <%--<asp:Button ID="WFB2DC0400Search" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2DC0400Search%>" OnClick="WFB2DC0400Search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />--%>

                                <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2dc_btn_clear%>" OnClientClick="return ClearAll();" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" height="1" colspan="6">
                            <hr>
                        </td>
                    </tr>
                </tbody>
            </table>
            <table cellspacing="1" cellpadding="1" width="1020px" border="0" class="Body_Label">
                <colgroup>
                    <col width="10%" />
                    <col width="35%" />
                    <col width="15%" />
                    <col width="40%" />
                </colgroup>
                <tbody>
                    <tr>
                        <td align="left" colspan="2">


                            <asp:DropDownList ID="ddl_CARD_HANDLE" Width="100px" runat="server"></asp:DropDownList>
                            <%-- 執行處理 --%>
                            <aces:Btn ID="WFB2DC0400NoMakeCard" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2DC0400NoMakeCard%>" OnClick="WFB2DC0400NoMakeCard_Click" />
                            &nbsp;&nbsp;&nbsp;
                             <%-- 匯出製卡人員 --%>
                            <aces:Btn ID="WFB2DC0400ExportToMake" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2DC0400ExportToMake%>" OnClick="WFB2DC0400ExportToMake_Click" />
                            &nbsp;&nbsp;&nbsp;
                             <%-- 卡鐘有效卡即時匯出 --%>
                            <aces:Btn ID="WFB2DC0400Export" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2DC0400Export%>" OnClick="WFB2DC0400Export_Click" />

                            <%--<asp:Button ID="WFB2DC0400Export" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2DC0400Export%>" OnClick="WFB2DC0400Export_Click" />--%>
                            <%--<asp:Button ID="WFB2DC0400NoMakeCard" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2DC0400NoMakeCard%>" OnClick="WFB2DC0400NoMakeCard_Click" />--%>
                            <%--<asp:Button ID="WFB2DC0400ExportToMake" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2DC0400ExportToMake%>" OnClick="WFB2DC0400ExportToMake_Click" />--%>
                        </td>
                        <td align="right" colspan="2">
                            <aces:Btn ID="WFB2DC0400Add" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2DC0400Add%>" OnClick="WFB2DC0400Add_Click" />
                            <aces:Btn ID="WFB2DC0400Delete" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2DC0400Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2DC0400Delete_Click" Visible="False" />
                            <aces:Btn ID="WFB2DC0400Edit" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2DC0400Edit%>" OnClick="WFB2DC0400Edit_Click" Visible="False" />
                            <aces:Btn ID="WFB2DC0400Save" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2DC0400Save%>" Visible="false" OnClick="WFB2DC0400Save_Click" OnClientClick="return saveCheck();" ValidationGroup="GroupA" />

                            <%--                            <asp:Button ID="WFB2DC0400Add" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2DC0400Add%>" OnClick="WFB2DC0400Add_Click" />
                            <asp:Button ID="WFB2DC0400Delete" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2DC0400Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2DC0400Delete_Click" Visible="False" />
                            <asp:Button ID="WFB2DC0400Edit" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2DC0400Edit%>" OnClick="WFB2DC0400Edit_Click" Visible="False" />
                            <asp:Button ID="WFB2DC0400Save" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2DC0400Save%>" Visible="false" OnClick="WFB2DC0400Save_Click" OnClientClick="return saveCheck();" ValidationGroup="GroupA" />--%>

                            <asp:Button ID="WFB2DC0400Cancel" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2DC0400Cancel%>" Visible="false" OnClick="WFB2DC0400Cancel_Click" OnClientClick="return confirm('是否確定取消?');" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left"></td>
                        <td colspan="3"></td>
                    </tr>
                    <!-- END: Create a line -->
                </tbody>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DC0400DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />

                    <asp:ControlParameter ControlID="ddl_QRY_CARD_TYPE" DefaultValue=""
                        Name="card_type" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="txt_QRY_CARD_NO" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="card_no" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="cb_CARD_STATUS1" DefaultValue=""
                        Name="card_status1" PropertyName="Checked" Type="Boolean" />
                    <asp:ControlParameter ControlID="cb_CARD_STATUS2" DefaultValue=""
                        Name="card_status2" PropertyName="Checked" Type="Boolean" />
                    <asp:ControlParameter ControlID="ddl_select_CARD_HANDLE" DefaultValue=""
                        Name="card_handle" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="txt_CHANGE_DT" DefaultValue=""
                        Name="change_dt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_CHANGE_DT" DefaultValue=""
                        Name="ddl_change_dt" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="ddl_PLANT_CD" DefaultValue=""
                        Name="plant_cd" PropertyName="SelectedValue" Type="String" />
                     <asp:ControlParameter ControlID="txt_QRY_CARD_NAME" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="card_name" PropertyName="Text" Type="String" />

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
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%-- 卡片屬性--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_CARD_TYPE%>" SortExpression="CARD_TYPE" HeaderStyle-Width="140px" ItemStyle-Width="140px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_CARD_TYPE" runat="server" Text='<%#Bind("CARD_TYPE_DESC")%>' Width="140px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_CARD_TYPE" runat="server" Text='<%#Bind("CARD_TYPE_DESC")%>' Width="140px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_NEW_CARD_TYPE" runat="server" CssClass="MandatoryField" AutoPostBack="true" OnSelectedIndexChanged="ddl_CARD_TYPE_SelectedIndexChanged" Width="120px"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_CARD_TYPE%>"
                                ControlToValidate="ddl_NEW_CARD_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <%--工號/廠商人員編號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_PERSON_ID%>" SortExpression="PERSON_ID" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_PERSON_ID" runat="server" Text='<%#Bind("PERSON_ID")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_PERSON_ID" runat="server" Text='<%#Bind("PERSON_ID")%>' Width="80px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_PERSON_ID" runat="server" MaxLength="5" Width="70px" OnTextChanged="txt_PERSON_ID_TextChanged" AutoPostBack="true" ClientIDMode="Static"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--卡號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_CARD_MID_NO%>" SortExpression="CARD_MID_NO" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_CARD_MID_NO" runat="server" Text='<%#Bind("CARD_MID_NO")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_CARD_MID_NO" runat="server" Text='<%#Bind("CARD_MID_NO")%>' Width="80px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_CARD_MID_NO" runat="server" ClientIDMode="Static" MaxLength="5" Width="70px" CssClass="MandatoryField" OnTextChanged="txt_CARD_MID_NO_TextChanged" AutoPostBack="true"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--流水號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_CARD_SEQ%>" SortExpression="CARD_SEQ" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_CARD_SEQ" runat="server" Text='<%#Bind("CARD_SEQ")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_CARD_SEQ" runat="server" Text='<%#Bind("CARD_SEQ")%>' Width="60px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_CARD_SEQ" runat="server" Style="text-align: right;" MaxLength="1" Width="50px" CssClass="MandatoryField"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                      <%--姓名--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_CARD_NAME%>" SortExpression="CARD_NAME" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_CARD_NAME" runat="server" Text='<%#Bind("CARD_NAME")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_CARD_NAME" runat="server" MaxLength="30" Width="90px" ClientIDMode="Static" Text='<%#Bind("CARD_NAME")%>'></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_CARD_NAME" runat="server" MaxLength="30" Width="90px" ClientIDMode="Static"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%-- 員工區分 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_EMP_CD%>" SortExpression="EMP_CD" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_CD" runat="server" Text='<%#Bind("EMP_CD_DESC")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_EMP_CD" runat="server" Text='<%#Bind("EMP_CD_DESC")%>' Width="100px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_EMP_CD" runat="server" Width="90px" ClientIDMode="Static"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%-- 資格代號 --%>
                       <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_LEVEL_CD%>" SortExpression="LEVEL_CD" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEVEL_CD_DESC" runat="server" Text='<%#Bind("LEVEL_CD_DESC")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_LEVEL_CD_DESC" runat="server" Text='<%#Bind("LEVEL_CD_DESC")%>' Width="80px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_LEVEL_CD_DESC" runat="server" Width="70px" ClientIDMode="Static"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%-- 職務名稱 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_PJOB_CD%>" SortExpression="PJOB_CD" HeaderStyle-Width="140px" ItemStyle-Width="140px" ItemStyle-HorizontalAlign="left">
                        <ItemTemplate>
                            <asp:Label ID="lb_PJOB_DESC" runat="server" Text='<%#Bind("PJOB_DESC")%>' Width="140px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_PJOB_DESC" runat="server" Text='<%#Bind("PJOB_DESC")%>' Width="140px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_PJOB_DESC" runat="server" Width="130px" ClientIDMode="Static"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%-- 部門名稱--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2df_lb_DEPT_NAME%>" SortExpression="DEPT_NO" HeaderStyle-Width="200px" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_DEPT_NO" runat="server" Text='<%#Bind("DEPT_NO")%>' Width="200px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_DEPT_NO" runat="server" Text='<%#Bind("DEPT_NO")%>' Width="200px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_DEPT_NO" runat="server" Width="190px" ClientIDMode="Static"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--使用對象--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_CARD_USED_CD%>" SortExpression="CARD_USED_CD" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_CARD_USED_CD" runat="server" Text='<%#Bind("CARD_USED_DESC")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_edit_CARD_USED_CD" runat="server" Text='<%#Bind("CARD_USED_DESC")%>' ClientIDMode="Static" Width="80px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_add_CARD_USED_CD" runat="server" ClientIDMode="Static" Width="70px"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                  
                    <%--備註--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_NOTES%>" SortExpression="NOTES" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_NOTES" runat="server" Text='<%#Bind("NOTES")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_NOTES" runat="server" MaxLength="60" Width="70px" ClientIDMode="Static" Text='<%#Bind("NOTES")%>'></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NOTES" runat="server" MaxLength="60" Width="70px" ClientIDMode="Static" ></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--生效日期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_START_DT%>" SortExpression="START_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_START_DT" runat="server" Text='<%#Bind("START_DT")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_START_DT" runat="server" MaxLength="10" Width="90px" ClientIDMode="Static" CssClass="MandatoryField date" Text='<%#Bind("START_DT")%>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_START_DT%>"
                                ControlToValidate="txt_START_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="qrytest3" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_START_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_START_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_START_DT" runat="server" MaxLength="10" Width="90px" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_START_DT%>"
                                ControlToValidate="txt_START_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="qrytest4" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_START_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_START_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--結束日期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_END_DT%>" SortExpression="END_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_END_DT" runat="server" Text='<%#Bind("END_DT")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_END_DT" runat="server" MaxLength="10" Width="90px" ClientIDMode="Static" CssClass="date" Text='<%#Bind("END_DT")%>'></asp:TextBox>
                            <asp:CustomValidator ID="qrytest5" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_END_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_END_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_END_DT" runat="server" MaxLength="10" Width="90px" ClientIDMode="Static" CssClass="date" Text='<%#Bind("END_DT")%>'></asp:TextBox>
                            <asp:CustomValidator ID="qrytest6" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_END_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_END_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--卡片狀態--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_CARD_STATUS%>" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_CARD_STATUS" runat="server" Width="60px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_CARD_STATUS" runat="server" Width="60px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--臨時卡區分--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_TEMP_CARD_CD%>" SortExpression="TEMP_CARD_CD" HeaderStyle-Width="140px" ItemStyle-Width="140px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_TEMP_CARD_CD" runat="server" Text='<%#Bind("TEMP_CARD_DESC")%>' Width="140px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_TEMP_CARD_CD" runat="server" ClientIDMode="Static" Width="140px"></asp:DropDownList>
                            <asp:HiddenField ID="hid_TEMP_CARD_CD" runat="server" Value='<%#Bind("TEMP_CARD_DESC")%>' />

                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_TEMP_CARD_CD" runat="server" ClientIDMode="Static" Width="130px"></asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--卡片處理--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_CARD_HANDLE%>" SortExpression="CARD_HANDLE" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_CARD_HANDLE" runat="server" Text='<%#Bind("CARD_HANDLE_DESC")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_CARD_HANDLE" runat="server" Text='<%#Bind("CARD_HANDLE_DESC")%>' Width="100px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_CARD_HANDLE" runat="server" Width="90px"></asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>

                </Columns>
                <EmptyDataTemplate>
                    <div style="width: 1020px; overflow: auto; height: 100px;">
                        <table class="grid-view" width="1200" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF">
                            <tr class="header">
                                <td width="20px"></td>
                                 <%-- 序號--%>
                                <td width="40px">
                                    <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2dc_RowNumber%>" Width="40px"></asp:Label>
                                </td>
                                 <%-- 卡片屬性--%>
                                <td width="120px">
                                    <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CARD_TYPE%>"></asp:Label>
                                </td>
                                <%--工號/廠商人員編號--%>
                                <td width="70px">
                                    <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_PERSON_ID%>" Width="80px"></asp:Label>
                                </td>
                                 <%--卡號--%>
                                <td>
                                    <asp:Label ID="Label17" runat="server" Text="<%$Resources:Resource,wfb2dc_CARD_MID_NO%>"  width="60px"></asp:Label>
                                </td>
                                <%--流水號--%>
                                <td >
                                    <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2dc_CARD_SEQ%>" width="60px"></asp:Label>
                                </td>
                                <%--姓名--%>
                                <td >
                                    <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CARD_NAME%>" Width="100px"></asp:Label>
                                </td>
                                <%-- 員工區分 --%>
                                 <td >
                                    <asp:Label ID="Label14" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_EMP_CD%>" width="100px"></asp:Label>
                                </td>
                                <%-- 資格代號 --%>
                                 <td width="100px">
                                    <asp:Label ID="Label15" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_LEVEL_CD%>" Width="80px"></asp:Label>
                                </td>
                                <%-- 職務名稱 --%>
                                <td width="140px">
                                    <asp:Label ID="Label16" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_PJOB_CD%>"  Width="140px"></asp:Label>
                                </td>
                                <%-- 部門名稱--%>
                                <td width="200px">
                                    <asp:Label ID="Label13" runat="server" Text="<%$Resources:Resource,wfb2df_lb_DEPT_NAME%>" Width="200px"></asp:Label>
                                </td>
                                <%--使用對象--%>
                                <td width="60px">
                                    <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CARD_USED_CD%>" Width="80px"></asp:Label>
                                </td>
                                <%--備註--%>
                                <td >
                                    <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_NOTES%>" Width="80px"></asp:Label>
                                </td>
                                <%--生效日期--%>
                                <td >
                                    <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_START_DT%>" width="100px"></asp:Label>
                                </td>
                                 <%--結束日期--%>
                                <td >
                                    <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_END_DT%>" width="100px"></asp:Label>
                                </td>
                                <%--卡片狀態--%>
                                <td width="60px">
                                    <asp:Label ID="Label10" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CARD_STATUS%>" Width="60px"></asp:Label>
                                </td>
                                 <%--臨時卡區分--%>
                                <td width="140px">
                                    <asp:Label ID="Label11" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_TEMP_CARD_CD%>"  Width="140px"></asp:Label>
                                </td>
                                <%--卡片處理--%>
                                <td width="100px">
                                    <asp:Label ID="Label12" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CARD_HANDLE%>"  Width="100px"></asp:Label>
                                </td>
                            </tr>
                            <tr class="normal">
                                <td width="20px"></td>
                                <td width="40px"></td>
                                <%-- 卡片屬性--%>
                                <td>
                                    <asp:DropDownList ID="ddl_NEW_CARD_TYPE" runat="server" CssClass="MandatoryField" AutoPostBack="true" OnSelectedIndexChanged="ddl_CARD_TYPE_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_CARD_TYPE%>"
                                        ControlToValidate="ddl_NEW_CARD_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <%--工號/廠商人員編號--%>
                                    <asp:TextBox ID="txt_PERSON_ID" runat="server" MaxLength="5" Width="80px" OnTextChanged="txt_PERSON_ID_TextChanged" AutoPostBack="true" ClientIDMode="Static"></asp:TextBox>

                                </td>
                                <td>
                                    <%--卡號--%>
                                    <asp:TextBox ID="txt_CARD_MID_NO" runat="server" ClientIDMode="Static" MaxLength="5" Width="70px" CssClass="MandatoryField" OnTextChanged="txt_CARD_MID_NO_TextChanged" AutoPostBack="true"></asp:TextBox>
                                </td>
                                <td>
                                    <%--流水號--%>
                                    <asp:TextBox ID="txt_CARD_SEQ" runat="server" Style="text-align: right;" MaxLength="1" Width="60px" CssClass="MandatoryField"></asp:TextBox>
                                </td>
                                <td>
                                     <%--姓名--%>
                                    <asp:TextBox ID="txt_CARD_NAME" runat="server" MaxLength="30" Width="100px" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <td>
                                     <%-- 員工區分 --%>
                                    <asp:Label ID="lb_EMP_CD" runat="server" Width="100px" ClientIDMode="Static"></asp:Label>
                                </td>
                                <td>
                                     <%-- 資格代號 --%>
                                    <asp:Label ID="lb_LEVEL_CD_DESC" runat="server" Width="80px" ClientIDMode="Static"></asp:Label>
                                </td>
                                 <td>
                                     <%-- 職務名稱 --%>
                                    <asp:Label ID="lb_PJOB_DESC" runat="server" Width="140px" ClientIDMode="Static"></asp:Label>
                                </td>
                                <td>
                                     <%-- 部門名稱 --%>
                                    <asp:Label ID="lb_DEPT_NO" runat="server" Width="200px" ClientIDMode="Static"></asp:Label>
                                </td>
                                <td>
                                    <%--使用對象--%>
                                    <asp:Label ID="lb_add_CARD_USED_CD" runat="server" ClientIDMode="Static" Width="80px"></asp:Label>
                                </td>
                                
                                <td>
                                    <%--備註--%>
                                    <asp:TextBox ID="txt_NOTES" runat="server" MaxLength="60" Width="80px" ClientIDMode="Static" Text='<%#Bind("NOTES")%>'></asp:TextBox>
                                </td>
                                <td>
                                    <%--生效日期--%>
                                    <asp:TextBox ID="txt_START_DT" runat="server" MaxLength="10" Width="100px" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_START_DT%>"
                                        ControlToValidate="txt_START_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="qrytest4" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_START_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                        ControlToValidate="txt_START_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                </td>
                                <td>
                                    <%--結束日期--%>
                                    <asp:TextBox ID="txt_END_DT" runat="server" MaxLength="10" Width="100px" ClientIDMode="Static" CssClass="date" Text='<%#Bind("END_DT")%>'></asp:TextBox>
                                    <asp:CustomValidator ID="qrytest6" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_END_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                        ControlToValidate="txt_END_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                </td>
                                <td width="60px">
                                    <%--卡片狀態--%>
                                </td>
                                <td>
                                    <%--臨時卡區分--%>
                                    <asp:DropDownList ID="ddl_TEMP_CARD_CD" runat="server" ClientIDMode="Static" Width="140px"></asp:DropDownList>
                                </td>
                                <td>
                                    <%--卡片處理--%>
                                    <asp:DropDownList ID="ddl_CARD_HANDLE" runat="server" Width="100px"></asp:DropDownList>
                                </td>
                            </tr>
                    </div>
                    </table>


                </EmptyDataTemplate>
                <HeaderStyle CssClass="GridviewScrollHeader" />
                <PagerStyle CssClass="GridviewScrollPager" />
                <EditRowStyle CssClass="normal" />
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
            <asp:HiddenField ID="hid_mod" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
        <%--<Triggers >
            <asp:PostBackTrigger ControlID="WFB2DC0400ExportToMake" />
        </Triggers>--%>
    </asp:UpdatePanel>
</asp:Content>
