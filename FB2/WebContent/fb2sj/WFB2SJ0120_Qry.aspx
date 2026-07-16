<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sj/WFB2SJ0120_Qry.aspx.cs" Inherits="WebContent_WFB2SJ0120_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
     <style type="text/css">
        #txt_DISTING_CD {
            text-transform: uppercase;
        }
    </style>
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




        //查詢前檢核
        function CheckSearch() {
            var processed = true;
            BlockUI();
            return processed;
        }

        //清空畫面
        function ClearAll() {
            $("#txt_DISTING_CD").val("");
            $("#txt_DISTING_DESC").val("");
        }


    </script>
    <style type="text/css">
        .auto-style1 {
            height: 23px;
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="15%" />
                    <col width="20%" />
                    <col width="10%" />
                    <col width="15%" />
                    <col width="10%" />
                    <col width="30%" />
                </colgroup>
                <tbody>
                     <tr>                        
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_DISTING_CD" runat="server" Text="區分代碼"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_DISTING_CD" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_DISTING_DESC" runat="server" Text="區分說明"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_DISTING_DESC" runat="server" MaxLength="20" ClientIDMode="Static"></asp:TextBox>
                        </td>
                    </tr>                   
                    <tr>
                        <th></th>
                        <th></th>
                        <th></th>
                        <td align="right" class="Body_label">
                            <div id="init">
                                <aces:Btn ID="WFB2SJ0120Search" runat="server" Text="查詢" OnClick="WFB2SJ0120Search_Click" OnClientClick="return CheckSearch();" />
                           
                                <%--<asp:Button ID="WFB2IA0100Search" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0100Search%>" OnClick="WFB2IA0100Search_Click" OnClientClick="BlockUI();" />--%>
                                            
                                <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2ia_btn_clear%>" OnClientClick="ClearAll();" />
                            </div>
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
                                <aces:Btn ID="WFB2SJ0120Add" runat="server" Text="新增" Visible="true" OnClick="WFB2SJ0120Add_Click" OnClientClick="BlockUI();" />
                                <aces:Btn ID="WFB2SJ0120Upd" runat="server" Text="修改" Visible="false" OnClick="WFB2SJ0120Upd_Click" OnClientClick="BlockUI();" />
                                <aces:Btn ID="WFB2SJ0120Del" runat="server" Text="刪除" Visible="false" OnClick="WFB2SJ0120Del_Click" OnClientClick="return doButtonConfirm(this.value);" />
                               
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SJ0120DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" /> 
                    <asp:ControlParameter ControlID="txt_DISTING_CD"
                        Name="disting_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" /> 
                    <asp:ControlParameter ControlID="txt_DISTING_DESC"
                        Name="disting_desc" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" /> 
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-Width="40px">
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
                    </asp:TemplateField>
                    <%--區分代碼--%>
                    <asp:TemplateField HeaderText="區分代碼" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" SortExpression="DISTING_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_DISTING_CD" runat="server" Text='<%#Bind("DISTING_CD")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--區分說明--%>
                    <asp:TemplateField HeaderText="區分說明" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="DISTING_DESC">
                        <ItemTemplate>
                            <asp:Label ID="lb_DISTING_DESC" runat="server" Text='<%#Bind("DISTING_DESC")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--外數區分--%>
                    <asp:TemplateField HeaderText="外數區分" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Left" SortExpression="IS_OUT">
                        <ItemTemplate>
                            <asp:Label ID="lb_IS_OUT" runat="server" Text='<%#Bind("IS_OUT")%>' Width="50px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>    
                    <%--備考對象--%>
                    <asp:TemplateField HeaderText="備考對象" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Left" SortExpression="IS_REMARK">
                        <ItemTemplate>
                            <asp:Label ID="lb_IS_REMARK" runat="server" Text='<%#Bind("IS_REMARK")%>' Width="50px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>      
                    <%--是否生效--%>
                    <asp:TemplateField HeaderText="是否生效" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Left" SortExpression="IS_VALID">
                        <ItemTemplate>
                            <asp:Label ID="lb_IS_VALID" runat="server" Text='<%#Bind("IS_VALID")%>' Width="50px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>  
                    <%--備考內容--%>
                    <asp:TemplateField HeaderText="備考內容" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left" SortExpression="CONTENT">
                        <ItemTemplate>
                            <asp:Label ID="lb_CONTENT" runat="server" Text='<%#Bind("CONTENT")%>' Width="150px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>  
                    <%--備註--%>
                    <asp:TemplateField HeaderText="備註" HeaderStyle-Width="180px" ItemStyle-HorizontalAlign="Left" >
                        <ItemTemplate>
                            <asp:Label ID="lb_REMARK" runat="server" Text='<%#Bind("REMARK")%>' Width="180px"></asp:Label>
                        </ItemTemplate>
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
            <asp:HiddenField ID="hid_today" runat="server" ClientIDMode="Static" />

            <!-- 每頁的筆數 -->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <!-- 是否要凍結視窗 -->
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="N" />
            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />

        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
