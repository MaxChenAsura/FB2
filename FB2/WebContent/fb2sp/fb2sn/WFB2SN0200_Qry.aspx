<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sn/WFB2SN0200_Qry.aspx.cs" Inherits="WebContent_WFB2SN0200_Qry" Culture="auto" UICulture="auto" %>
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
            $(".year").mask('9999');
            $(".decimal").css("text-align", "right").css("ime-mode", "disabled");

            //期間的預設值
            //var todayDate = getTodayDate();
            //$("#txt_APPLY_DT_S").val(todayDate);
            //$("#txt_APPLY_DT_E").val(todayDate);


            reComma("ALLOWANCE_VALUE", 0);
            //GridView必須
            gridviewScroll();
            $.unblockUI();
        };


        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());

        };
                
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "400",
                barcolor: "#7F7F7F"
            });
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
        };

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
        };

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


        };
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
        };

        //清空畫面
        function ClearAll() {
            $("#ddl_AFA_FOR").val("-1");           
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
                            <asp:Label ID="lb_YEAR" runat="server" Text="<%$Resources:Resource,wfb2sn_lb_YEAR%>"></asp:Label>：	
                        </th>
                        <td align="left">                                            
                            <asp:TextBox ID="txt_YEAR" CssClass="MandatoryField yy date" runat="server" MaxLength="4" Width="81px" ClientIDMode="Static" OnTextChanged="txt_YEAR_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sn_required_YEAR%>"
                             ControlToValidate="txt_YEAR" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server"
                             ErrorMessage="<%$Resources:Resource,wfb2sn_error_YEAR%>" ControlToValidate="txt_YEAR" ForeColor="Red" ValidationGroup="GroupA"
                             ValidationExpression="\d\d\d\d" Display="None"></asp:RegularExpressionValidator>
                        </td>                         
                    </tr>
                    <tr>
                         <th align="left" class="Body_TableHeader">
                             <asp:Label ID="lbl_AFA_FOR" runat="server" Text="<%$Resources:Resource,wfb2sn_lbl_AFA_FOR%>"></asp:Label>:
                         </th>                                    
                         <td align="left" class="Body_label">
                             <asp:DropDownList ID="ddl_AFA_FOR" runat="server" ClientIDMode="Static" ></asp:DropDownList>                             
                         </td>
                         <td></td> 
                         <td></td> 
                    </tr>  
                    <tr>
                        <th></th>
                        <td align="right" colspan="5">
                            <aces:Btn ID="WFB2SN0200Search" runat="server" Text="<%$Resources:Resource,wfb2sn_btn_search%>" OnClick="WFB2SN0200Search_Click" OnClientClick="CheckSearch();"/>
                            
                            <%--
                            
                            <asp:Button ID="WFB2SN0200Search" runat="server" Text="<%$Resources:Resource,wfb2sn_btn_search%>" OnClick="WFB2SN0200Search_Click" OnClientClick="CheckSearch();"/>
                                --%>
                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2sh_btn_clear%>" onclick="ClearAll();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                </tbody>
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SN0200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_YEAR"
                        Name="YEAR" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_AFA_FOR" DefaultValue=""
                        Name="AFA_FOR" PropertyName="SelectedValue" Type="String" /> 
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1019px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">
                <Columns>
                    <%--序號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sn_lb_rownumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>                            
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--阿法值對像--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sn_lbl_AFA_FOR%>" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_AFA_FOR" runat="server" Text='<%#Bind("AFA_FOR")%>' Width="200px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--核可狀態--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sn_lb_AFA_APPROVE_STATUS%>" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Left" >
                        <ItemTemplate>
                            <asp:Label ID="lb_AFA_APPROVE_STATUS" runat="server" Text='<%#Bind("AFA_APPROVE_STATUS")%>' Width="50px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--提出核可人員--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sn_lb_AFA_RELEASE_BY%>" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_AFA_RELEASE_BY" runat="server" Text='<%#Bind("AFA_RELEASE_BY")%>' Width="140px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--提出核可日--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sn_lb_AFA_RELEASE_DT%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right" >
                        <ItemTemplate>
                            <asp:Label ID="lb_AFA_RELEASE_DT" runat="server" Text='<%#Bind("AFA_RELEASE_DT","{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--核可人員--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sn_lb_AFA_APPROVE_BY%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" >
                        <ItemTemplate>
                            <asp:Label ID="lb_AFA_APPROVE_BY" runat="server" Text='<%#Bind("AFA_APPROVE_BY")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--核可日期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sn_lb_AFA_APPROVE_DT%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" >
                        <ItemTemplate>
                            <asp:Label ID="lb_AFA_APPROVE_DT" runat="server" Text='<%#Bind("AFA_APPROVE_DT","{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                            <asp:HiddenField ID="HID_KEY_VALUE" Value='<%#Bind("keyWord")%>'  runat="server" ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--薪資轉出日--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sn_lb_SALARY_TRANS_DT%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" >
                        <ItemTemplate>
                            <asp:Label ID="lb_SALARY_TRANS_DT" runat="server" Text='<%#Bind("SALARY_TRANS_DT","{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>                  
                    <%--功能--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sn_lb_function%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <aces:Btn ID="WFB2SN0200Detail" runat="server"
                                CommandName="ToDetail"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                Text="查詢明細" />
                           
                            <%--
                                <asp:Button ID="WFB2SN0200Detail" runat="server"
                                CommandName="ToDetail"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                Text="查詢明細" />

                                
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
           
            <!-- 是否要凍結視窗 -->
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="N" />
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
