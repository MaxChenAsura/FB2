<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sj/WFB2SJ3000_Qry.aspx.cs" Inherits="WebContent_WFB2SJ3000_Qry" Culture="auto" UICulture="auto" %>
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
            $(".number").mask('999');
            $(".number").css("text-align", "right");
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
            $("#txt_REDUCE_CD").val("");
            $("#txt_REDUCE_DESC2").val("");
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

        function Check_DT(source, arguments) {

            if ($("#txt_UNEFFECT_DT").val() < $("#lb_EFFECT_DT").text())
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }
        function Check_DT_NEW(source, arguments) {

            if ($("#txt_NEW_UNEFFECT_DT").val() < $("#txt_NEW_EFFECT_DT").val())
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
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
                                    <%--考核類別--%>
                                    <asp:Label ID="lb_ASSESS_TYPE" runat="server" Text="<%$Resources:Resource,wfb2sj_lb_assess_type%>"></asp:Label>:</th>                                    
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_ASSESS_TYPE" runat="server" Width="100px" ClientIDMode="Static"> </asp:DropDownList>
                                    </td>
                                    <th></th>
                                    <th></th>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SJ3000Search" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_search%>" OnClick="WFB2SJ3000Search_Click" OnClientClick="BlockUI();" />

                                            <%--<asp:Button ID="WFB2SJ3000Search" runat="server" Text="<%$Resources:Resource,wfb2sj_WFB2SJ3000Search%>" OnClick="WFB2SJ3000Search_Click" OnClientClick="BlockUI();" />--%>
                                            
                                            <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_clear%>" OnClientClick="ClearAll();" />
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
                        <aces:Btn ID="WFB2SJ3000Add" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_add%>" OnClick="WFB2SJ3000Add_Click" />
                        <aces:Btn ID="WFB2SJ3000Delete" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2SJ3000Delete_Click" Visible="false" />
                        <aces:Btn ID="WFB2SJ3000UPDATE" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_edit%>" OnClick="WFB2SJ3000Edit_Click" Visible="false" />
                        <aces:Btn ID="WFB2SJ3000Save" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_save%>" Visible="false" OnClick="WFB2SJ3000Save_Click" ValidationGroup="GroupA" />

                            <%--<asp:Button ID="WFB2SJ3000Add" runat="server" Text="<%$Resources:Resource,wfb2sj_WFB2SJ3000Add%>" OnClick="WFB2SJ3000Add_Click" />
                            <asp:Button ID="WFB2SJ3000Delete" runat="server" Text="<%$Resources:Resource,wfb2sj_WFB2SJ3000Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2SJ3000Delete_Click" Visible="false" />
                            <asp:Button ID="WFB2SJ3000Edit" runat="server" Text="<%$Resources:Resource,wfb2sj_WFB2SJ3000Edit%>" OnClick="WFB2SJ3000Edit_Click" Visible="false" />
                            <asp:Button ID="WFB2SJ3000Save" runat="server" Text="<%$Resources:Resource,wfb2sj_WFB2SJ3000Save%>" Visible="false" OnClick="WFB2SJ3000Save_Click" ValidationGroup="GroupA" />--%>
                            
                            <asp:Button ID="WFB2SJ3000Cancel" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_cancel%>" Visible="false" OnClick="WFB2SJ3000Cancel_Click" OnClientClick="return confirm('是否確定取消?');" />
                        </div>
                    </td>
                </tr>

            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SJ3000DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                        <asp:ControlParameter ControlID="ddl_ASSESS_TYPE"
                        Name="assess_type" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound"  Width="1010px"
                OnRowCreated="gv_result_RowCreated" OnDataBound="gv_result_DataBound"
                OnPageIndexChanging="gv_result_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static"  Width="20px" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID"  Width="20px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="序號" HeaderStyle-Width="50px" >
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'  Width="50px" ></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                                <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="50px" ></asp:Label>                         
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="50px" ></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_ASSESS_TYPE%>" SortExpression="ASSESS_TYPE" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_ASSESS_TYPE" runat="server" Text='<%#Bind("ASSESS_TYPE_DESC")%>' ></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:HiddenField ID="hid_SN" runat="server" Value='<%#Bind("SN")%> ' ClientIDMode="Static" />
                            <asp:HiddenField ID="hid_ASSESS_TYPE" runat="server" Value='<%#Bind("ASSESS_TYPE")%> ' ClientIDMode="Static" />
                            <asp:Label ID="lb_ASSESS_TYPE" runat="server" Text='<%#Bind("ASSESS_TYPE_DESC")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_NEW_ASSESS_TYPE" runat="server"  ClientIDMode="Static" CssClass="MandatoryField"  ></asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="考核項目" SortExpression="ITEM_NAME" HeaderStyle-Width="250px" ItemStyle-Width="250px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_ITEM_NAME" runat="server" Text='<%#Bind("ITEM_NAME")%>'  Width="200px" ></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_NEW_ITEM_NAME" runat="server" Text='<%#Bind("ITEM_NAME")%>' ClientIDMode="Static" CssClass="MandatoryField"  Width="200px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="考核項目不可為空白"
                                ControlToValidate="txt_NEW_ITEM_NAME" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_ITEM_NAME" runat="server" ClientIDMode="Static" CssClass="MandatoryField " Width="200px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="考核項目不可為空白"
                                ControlToValidate="txt_NEW_ITEM_NAME" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="考核說明" SortExpression="ITEM_DESC" HeaderStyle-Width="350px" ItemStyle-Width="350px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_ITEM_DESC" runat="server" Text='<%#Bind("ITEM_DESC")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_ITEM_DESC" runat="server" Text='<%#Bind("ITEM_DESC")%>' ClientIDMode="Static" CssClass="MandatoryField" MaxLength="200" Width="350px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="考核說明不可為空白"
                                ControlToValidate="txt_ITEM_DESC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_ITEM_DESC" runat="server" ClientIDMode="Static" CssClass="MandatoryField" MaxLength="200" Width="350px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="考核說明不可為空白"
                                ControlToValidate="txt_NEW_ITEM_DESC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="最高分數" SortExpression="ASSESS_SCORE" HeaderStyle-Width="200px" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_ASSESS_SCORE" runat="server" Text='<%#Bind("ASSESS_SCORE")%>'  ></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_ASSESS_SCORE" runat="server" Text='<%#Bind("ASSESS_SCORE")%>' CssClass="MandatoryField number" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="最高分數不可為空白"
                                ControlToValidate="txt_ASSESS_SCORE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="最高分數僅能輸入0~99"
                                ControlToValidate="txt_ASSESS_SCORE" Type="Integer" MinimumValue="0" MaximumValue="99"
                                ValidationGroup="GroupA" Display="None"></asp:RangeValidator>
                            <asp:CustomValidator ID="CustomValidatorASSESS_SCORE" runat="server" ValidateEmptyText="true"
                                ErrorMessage="最高分數不可為空白" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_ASSESS_SCORE" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>

                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_ASSESS_SCORE" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Text="0"  Width="30px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="最高分數不可為空白"
                                ControlToValidate="txt_NEW_ASSESS_SCORE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="最高分數僅能輸入0~99"
                                ControlToValidate="txt_NEW_ASSESS_SCORE" Type="Integer" MinimumValue="0" MaximumValue="100"
                                ValidationGroup="GroupA" Display="None"></asp:RangeValidator>
                            <asp:CustomValidator ID="CustomValidatorASSESS_SCORE" runat="server" ValidateEmptyText="true"
                                ErrorMessage="最高分數不可為空白" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_ASSESS_SCORE" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    

                </Columns>
                <EmptyDataTemplate>
                    <table class="grid-view" width="1024px">
                        <tr class="header">
                            <td>
                                <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" Width="20px" />
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="序號" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="考核類別" Width="90px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="考核項目" Width="150px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="考核說明" Width="250px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="最高分數" Width="100px"></asp:Label>
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
                                    <asp:DropDownList ID="ddl_NEW_ASSESS_TYPE" runat="server" Width="90px" ClientIDMode="Static" CssClass="MandatoryField"  ></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorASSESS_TYPE" runat="server" ErrorMessage="考核類別不可為空白"
                                    ControlToValidate="ddl_NEW_ASSESS_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                               
                            </td>
                            <td>
                                  <asp:TextBox ID="txt_NEW_ITEM_NAME" runat="server" ClientIDMode="Static" CssClass="MandatoryField " Width="150px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="不可為空白"
                                ControlToValidate="txt_NEW_ITEM_NAME" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            
                            </td>
                            <td>
                                 <asp:TextBox ID="txt_NEW_ITEM_DESC" runat="server" ClientIDMode="Static" CssClass="MandatoryField" MaxLength="100" Width="250px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="考核說明不可為空白"
                                    ControlToValidate="txt_NEW_ITEM_DESC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                              <asp:TextBox ID="txt_NEW_ASSESS_SCORE" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Text="0" Width="100px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="最高分數不可為空白"
                                ControlToValidate="txt_NEW_ASSESS_SCORE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="最高分數僅能輸入0~99"
                                ControlToValidate="txt_NEW_ASSESS_SCORE" Type="Integer" MinimumValue="0" MaximumValue="100"
                                ValidationGroup="GroupA" Display="None"></asp:RangeValidator>
                            <asp:CustomValidator ID="CustomValidatorASSESS_SCORE" runat="server" ValidateEmptyText="true"
                                ErrorMessage="最高分數不可為空白" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_ASSESS_SCORE" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>

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
            <asp:Button ID="HID_cancel" runat="server" Style="display: none" ClientIDMode="Static" OnClick="HID_cancel_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

