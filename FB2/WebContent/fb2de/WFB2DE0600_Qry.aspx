<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2de/WFB2DE0600_Qry.aspx.cs" Inherits="WebContent_fb2de_WFB2DE0600_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            gridviewScroll();
            $.unblockUI();           
        }

        //判斷是否為數字
        function IsIntText() {
            var charkeycode = window.event.keyCode;
            if (charkeycode > 47 && charkeycode < 58) {  //0的keycode為47 ,9的keycode為58
                return true;
            }
            return false;

        }
        function addzero(input) {
            if (input.value.length == 1) input.value = '0' + input.value;
            return input;
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
            var dd = $('#ddl_CLOCK_NO').val();
            alert("dd"+dd);
            $('#ddl_CLOCK_NO').val(-1);           
        }

        function CheckSearch() {

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                return false;
        }

        //儲存前檢查
        function saveCheck() {
            var processed = true;
            if (processed) {
                if (Page_ClientValidate("GroupA")) {
                    BlockUI();
                } else
                    processed = false;
            }


            if (!processed) {
                $.unblockUI();
                return;
            }
            return processed;
        }     
        

        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());            
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="10%" />
				    <col width="30%" />
					<col width="10%" />
					<col width="15%" />
					<col width="15%" />
					<col width="20%" />
                </colgroup>
                <tbody>
                    <!-- START: 1st Line in Search Criteria area -->
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_CLOCK_NO" runat="server" Text="<%$Resources:Resource,wfb2de_lb_CLOCK_NO%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" >
                            <asp:DropDownList ID="ddl_CLOCK_NO" runat="server" Width="200px" ClientIDMode="Static"></asp:DropDownList>                       
                        </td>                       
                    </tr>                    
                    <tr>
                        <td align="right" class="Body_label" colspan="10">
                            <div id="init">
                                <aces:Btn ID="WFB2DE0600Search" runat="server" Text="<%$Resources:Resource,wfb2de_WFB2DE0600Search%>" OnClick="WFB2DE0600Search_Click"/>

                                <%--
                                     <asp:Button ID="WFB2DE0600Search" runat="server" Text="<%$Resources:Resource,wfb2de_WFB2DE0600Search%>" OnClick="WFB2DE0600Search_Click"/>
                                     <aces:Btn ID="WFB2DE0600Search" runat="server" Text="<%$Resources:Resource,wfb2de_WFB2DE0600Search%>" OnClick="WFB2DE0600Search_Click"/>
                                    --%>
                                
                                <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2dd_btn_clear%>" onclick="ClearAll();"/>                               
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" height="1" colspan="10">
                            <hr>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="10">
                            <aces:Btn ID="WFB2DE0600Add" runat="server" Text="<%$Resources:Resource,wfb2de_WFB2DE0600Add%>" OnClick="WFB2DE0600Add_Click"  />
                            <aces:Btn ID="WFB2DE0600Edit" runat="server" Text="<%$Resources:Resource,wfb2de_WFB2DE0600Edit%>" Visible="false" OnClick="WFB2DE0600Edit_Click" />
                            <aces:Btn ID="WFB2DE0600Delete" runat="server" Text="<%$Resources:Resource,wfb2de_WFB2DE0600Delete%>"  Visible="false" OnClick="WFB2DE0600Delete_Click"/>
                            <aces:Btn ID="WFB2DE0600Save" runat="server" Text="<%$Resources:Resource,wfb2de_WFB2DE0600Save%>" OnClientClick="return saveCheck();" Visible="false" OnClick="WFB2DE0600Save_Click"/>
                            <%--
                            <asp:Button ID="WFB2DE0600Add" runat="server" Text="<%$Resources:Resource,wfb2de_WFB2DE0600Add%>" OnClick="WFB2DE0600Add_Click"  />
                            <asp:Button ID="WFB2DE0600Edit" runat="server" Text="<%$Resources:Resource,wfb2de_WFB2DE0600Edit%>" Visible="false" OnClick="WFB2DE0600Edit_Click" />
                            <asp:Button ID="WFB2DE0600Delete" runat="server" Text="<%$Resources:Resource,wfb2de_WFB2DE0600Delete%>"  Visible="false" OnClick="WFB2DE0600Delete_Click"/>
                            <asp:Button ID="WFB2DE0600Save" runat="server" Text="<%$Resources:Resource,wfb2de_WFB2DE0600Save%>" OnClientClick="return saveCheck();" Visible="false" OnClick="WFB2DE0600Save_Click"/>
                            
                            <aces:Btn ID="WFB2DE0600Add" runat="server" Text="<%$Resources:Resource,wfb2de_WFB2DE0600Add%>" OnClick="WFB2DE0600Add_Click"  />
                            <aces:Btn ID="WFB2DE0600Edit" runat="server" Text="<%$Resources:Resource,wfb2de_WFB2DE0600Edit%>" Visible="false" OnClick="WFB2DE0600Edit_Click" />
                            <aces:Btn ID="WFB2DE0600Delete" runat="server" Text="<%$Resources:Resource,wfb2de_WFB2DE0600Delete%>"  Visible="false" OnClick="WFB2DE0600Delete_Click"/>
                            <aces:Btn ID="WFB2DE0600Save" runat="server" Text="<%$Resources:Resource,wfb2de_WFB2DE0600Save%>" OnClientClick="return saveCheck();" Visible="false" OnClick="WFB2DE0600Save_Click"/>
   
                                --%>
                            <asp:Button ID="WFB2DE0600Cancel" runat="server" Text="<%$Resources:Resource,wfb2dd_WFB2DD0100Cancel%>" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="WFB2DE0600Cancel_Click" CausesValidation="false"/>                            
                        </td>
                    </tr>
                    <!-- END: Create a line -->
                </tbody>
            </table>
              <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DE0600DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="ddl_CLOCK_NO" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="CLOCK_NO" PropertyName="SelectedValue" Type="String" />                   
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
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_RowNumber%>" HeaderStyle-Width="20px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2de_lb_CLOCK_NO%>" SortExpression="CLOCK_NO" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_CLOCK_NO" runat="server" Text='<%#Bind("CLOCK_NO")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_NEW_CLOCK_NO" runat="server" Text='<%#Bind("CLOCK_NO")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_NEW_CLOCK_NO" runat="server" Width="200px" OnSelectedIndexChanged="ddl_CLOCK_NOSelectedIndexChanged" ClientIDMode="AutoID" AutoPostBack="true" CssClass="MandatoryField"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2de_Required_CLOCK_NO%>"
                                    ControlToValidate="ddl_NEW_CLOCK_NO" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2de_lb_MONEY%>" SortExpression="PRICE" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_MONEY" runat="server" Text='<%#Bind("PRICE")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_NEW_MONEY" runat="server" Text='<%#Bind("PRICE","{0:n0}")%>' MaxLength="3" CssClass="MandatoryField" Style="text-align: right;ime-mode:disabled" onblur="return addzero(this)" onkeypress="return IsIntText();" onpaste="return false"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2de_Required_MONEY%>"
                                    ControlToValidate="txt_NEW_MONEY" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_MONEY" runat="server"  CssClass="MandatoryField" MaxLength="3" Style="text-align: right;ime-mode:disabled" onblur="return addzero(this)" onkeypress="return IsIntText();" onpaste="return false"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2de_Required_MONEY%>"
                                    ControlToValidate="txt_NEW_MONEY" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2de_lb_CLOCK_DESC%>" SortExpression="CLOCK_DESC" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_CLOCK_DESC" runat="server" Text='<%#Bind("CLOCK_DESC")%>' Style="word-wrap: break-word;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_NEW_CLOCK_DESC" runat="server" Text='<%#Bind("CLOCK_DESC")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_NEW_CLOCK_DESC" runat="server" ClientIDMode="Static"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2de_lb_CLOCK_IP%>" SortExpression="CLOCK_IP" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_CLOCK_IP" runat="server" Text='<%#Bind("CLOCK_IP")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_NEW_CLOCK_IP" runat="server" Text='<%#Bind("CLOCK_IP")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_NEW_CLOCK_IP" runat="server" Text='<%#Bind("PLANT_CD")%>'></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>                                      
                </Columns>
                <EmptyDataTemplate>
                    <table class="grid-view" width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                        <tr class="header">
                            <td></td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2df_RowNumber%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2de_lb_CLOCK_NO%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2de_lb_MONEY%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2de_lb_CLOCK_DESC%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2de_lb_CLOCK_IP%>"></asp:Label>
                            </td>                                                   
                        </tr>
                        <tr class="normal">
                            <td></td>
                            <td></td>
                            <td>
                               <asp:DropDownList ID="ddl_NEW_CLOCK_NO" runat="server" Width="200px" OnSelectedIndexChanged="ddl_CLOCK_NOSelectedIndexChanged" ClientIDMode="AutoID" AutoPostBack="true" CssClass="MandatoryField"></asp:DropDownList>
                               <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2de_Required_CLOCK_NO%>"
                                    ControlToValidate="ddl_NEW_CLOCK_NO" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_MONEY" runat="server"  CssClass="MandatoryField" MaxLength="3" Style="text-align: right;ime-mode:disabled" onblur="return addzero(this)" onkeypress="return IsIntText();" onpaste="return false"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2de_Required_MONEY%>"
                                    ControlToValidate="txt_NEW_MONEY" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:Label ID="lb_NEW_CLOCK_DESC" runat="server"></asp:Label>
                            </td>
                            <td>
                                 <asp:Label ID="lb_NEW_CLOCK_IP" runat="server"></asp:Label>
                            </td>                           
                        </tr>
                    </table>

                </EmptyDataTemplate>
                <HeaderStyle CssClass="GridviewScrollHeader" />
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle HorizontalAlign="Center" />
                <EditRowStyle HorizontalAlign="Center" />
            </asp:GridView>
            <td style="font-size: 14px;">
                <asp:Label ID="lb_TotalCount2" runat="server" Text="" Visible="false" Font-Size="10"></asp:Label>
            </td>
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