<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sa/action/WFB2SA1600_Qry.aspx.cs" Inherits="WebContent_WFB2SA1600_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
     <style type="text/css">
        #txt_PJOB_CD {
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
            $("#ddl_STATUS").val("-1");
            $("#ddl_SALARY_ID").val("-1");
            $("#ddl_HIRE_TYPE").val("-1");
            $("#txt_PJOB_CD").val("");
            $("#txt_PJOB_DESC").val("");
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
                        <%-- 狀態 --%>    
                        <th align="left" class="Body_TableHeader">
                        <asp:Label ID="lb_STATUS" runat="server" Text="<%$Resources:Resource,sa160_STATUS%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label"> 
                            <asp:DropDownList ID="ddl_STATUS" runat="server" ClientIDMode="Static"></asp:DropDownList>                           
                        </td>      
                          <%-- 薪資項目 --%>    
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_SALARY_ID" runat="server" Text="<%$Resources:Resource,sa160_SALARY_ID%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_SALARY_ID" runat="server" ClientIDMode="Static"></asp:DropDownList>                                       
                        </td>             
                            <%-- 類別 --%>             
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_HIRE_TYPE" runat="server" Text="<%$Resources:Resource,sa160_HIRE_TYPE%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">  
                            <asp:DropDownList ID="ddl_HIRE_TYPE" runat="server" ClientIDMode="Static"></asp:DropDownList>                                      
                        </td>
                       
                    </tr>
                    <tr>
                        <%-- 職務代號 --%>    
                        <th align="left" class="Body_TableHeader">
                        <asp:Label ID="lb_PJOB_CD" runat="server" Text="<%$Resources:Resource,sa160_PJOB_CD%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label"> 
                            <asp:TextBox ID="txt_PJOB_CD" runat="server" MaxLength="4" ClientIDMode="Static"></asp:TextBox>                                                                                          
                        </td>                  
                            <%-- 職務名稱 --%>              
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_PJOB_DESC" runat="server" Text="<%$Resources:Resource,sa160_PJOB_DESC%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">  
                                <asp:TextBox ID="txt_PJOB_DESC" runat="server" MaxLength="10"   ClientIDMode="Static"></asp:TextBox>                                    
                        </td>
                            <th  >
                        </th>
                        <td  >
                        </td>
                    </tr>                    
                    <tr>
                        <td align="right" colspan="6">
                            <aces:Btn ID="WFB2SA1600Search" runat="server" Text="查詢" OnClick="WFB2SA1600Search_Click" OnClientClick="return CheckSearch();" />
                            <%--
                            <asp:Button ID="WFB2SA1600Search" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_search%>" OnClick="WFB2SA1600Search_Click" OnClientClick="return CheckSearch();" />
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
                                <aces:Btn ID="WFB2SA1600Add" runat="server" Text="新增" Visible="true" OnClick="WFB2SA1600Add_Click" OnClientClick="BlockUI();" />
                                <aces:Btn ID="WFB2SA1600Upd" runat="server" Text="修改" Visible="false" OnClick="WFB2SA1600Upd_Click" OnClientClick="BlockUI();" />
                                <aces:Btn ID="WFB2SA1600Del" runat="server" Text="刪除" Visible="false" OnClick="WFB2SA1600Del_Click" OnClientClick="return doButtonConfirm(this.value);" />
                                <aces:Btn ID="WFB2SA1600All" runat="server" Text="一括更新" Visible="false" OnClick="WFB2SA1600All_Click" OnClientClick="BlockUI();" />
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SA1600DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="ddl_STATUS"
                        Name="status" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />         
                     <asp:ControlParameter ControlID="ddl_HIRE_TYPE"
                        Name="hire_type" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />    
                    <asp:ControlParameter ControlID="ddl_SALARY_ID"
                        Name="salary_id" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" /> 
                    <asp:ControlParameter ControlID="txt_PJOB_CD"
                        Name="pjob_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" /> 
                    <asp:ControlParameter ControlID="txt_PJOB_DESC"
                        Name="pjob_desc" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" /> 
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
                    <%--職務代碼--%>
                    <asp:TemplateField HeaderText="職務代碼" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" SortExpression="PJOB_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_PJOB_CD" runat="server" Text='<%#Bind("PJOB_CD")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--職務名稱--%>
                    <asp:TemplateField HeaderText="職務名稱" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="PJOB_DESC">
                        <ItemTemplate>
                            <asp:Label ID="lb_PJOB_DESC" runat="server" Text='<%#Bind("PJOB_DESC")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--類別--%>
                    <asp:TemplateField HeaderText="類別" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="HIRE_TYPE">
                        <ItemTemplate>
                            <asp:Label ID="lb_HIRE_TYPE_DESC" runat="server" Text='<%#Bind("HIRE_TYPE_DESC")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>    
                    <%--薪資項目--%>
                    <asp:TemplateField HeaderText="薪資項目" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" SortExpression="SALARY_ID">
                        <ItemTemplate>
                            <asp:Label ID="lb_SALARY_ID" runat="server" Text='<%#Bind("SALARY_ID")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>    
                    <%--薪資名稱--%>
                    <asp:TemplateField HeaderText="薪資名稱" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="SALARY_NAME">
                        <ItemTemplate>
                            <asp:Label ID="lb_SALARY_NAME" runat="server" Text='<%#Bind("SALARY_NAME")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>    
                    <%--項目金額--%>
                    <asp:TemplateField HeaderText="項目金額" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" SortExpression="PAY">
                        <ItemTemplate>
                            <asp:Label ID="lb_PAY" runat="server" Text='<%#Bind("PAY","{0:##,#}")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>    
                    <%--生效起日--%>
                    <asp:TemplateField HeaderText="生效起日" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="START_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_START_DT" runat="server" Text='<%#Bind("START_DT","{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>    
                    <%--生效迄日--%>
                    <asp:TemplateField HeaderText="生效迄日" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="END_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_END_DT" runat="server" Text='<%#Bind("END_DT","{0:yyyy/MM/dd}")%>' Width="120px"></asp:Label>
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
