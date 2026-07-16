<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sa/WFB2SA3100_Dtl.aspx.cs" Inherits="WebContent_WFB2SA3100_Dtl" Culture="auto" UICulture="auto" %>

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


        function mailCheck(msg) {
            var processed = true;
            if (Page_ClientValidate("GroupA")) {                
                //按鈕確認
                processed = doButtonConfirm(msg);
                BlockUI();
            }
            else
                processed = false;
          
            if (!processed)
                $.unblockUI();

            return processed;
        }

 
        //清空畫面
        function ClearAll() {
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
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
                    <col width="10%" />
                    <col width="40%" />
                    <col width="10%" />
                    <col width="40%" />
                </colgroup>
                <tbody>
                     <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_YM" runat="server" Text="年月"></asp:Label></th>
                        <td align="left" class="Body_label"   >
                            <asp:TextBox ID="txt_YM" runat="server"  CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:TextBox>                                
                            <asp:HiddenField ID="hid_EMP_ID" runat="server"   ClientIDMode="Static" />
                        </td>
                    </tr>    
                    <tr>

                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_TITLE" runat="server" Text="<%$Resources:Resource,wfb2se_lb_TITLE%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label" colspan="2" >
                            <asp:TextBox ID="txt_MAIL_TITLE" runat="server" MaxLength="150" Width="100%" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2se_lbl_TITLE_Required%>"
                                ControlToValidate="txt_MAIL_TITLE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </td>
                        <th/>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_MAIL_DESC" runat="server" Text="<%$Resources:Resource,wfb2se_MAIL_DESC%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label" colspan="2">
                            <asp:TextBox ID="txt_MAIL_DESC" runat="server" MaxLength="250" Width="100%" Rows="10" TextMode="MultiLine" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2se_lbl_MAIL_DESC_Required%>"
                                ControlToValidate="txt_MAIL_DESC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </td>
                        <th/>
                    </tr>                             
                    <tr>
                        <td align="right" colspan="6">
                            <aces:Btn ID="WFB2SA3100MAIL_PER" runat="server" Text="寄出(擔當)"   OnClick="WFB2SA3100MAILPER_Click" OnClientClick="return mailCheck(this.value);"    />
                            <aces:Btn ID="WFB2SA3100MAIL_2S"  runat="server" Text="寄出(2S)"  OnClick="WFB2SA3100MAIL2S_Click" OnClientClick="return mailCheck(this.value);"   />
                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2sh_btn_clear%>" onclick="ClearAll();" />
                            <asp:Button runat="server" ID="btn_back" Text="返回" OnClick="btn_back_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>                                                                         
                </tbody>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getDtlData"
                SelectCountMethod="getDtlCount" TypeName="CFB2SA3100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                     <asp:ControlParameter ControlID="txt_YM"
                        Name="ym" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" /> 
                     <asp:ControlParameter ControlID="hid_EMP_ID"
                        Name="emp_id" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" /> 
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1300px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>                    
                    <%--序號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_rownumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--工號--%>
                    <asp:TemplateField HeaderText="工號" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left" SortExpression="EMP_ID">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <%--姓名--%>
                    <asp:TemplateField HeaderText="姓名" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" SortExpression="EMP_NAME">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                           <%--異動日期--%>
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="異動日期" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="START_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_START_DT" runat="server" Text='<%#Bind("STARTDT")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <%--寄件否--%>
                    <asp:TemplateField HeaderText="寄件否" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="IS_MAIL">
                        <ItemTemplate>
                            <asp:Label ID="lb_IS_MAIL" runat="server" Text='<%#Bind("IS_MAIL_DESC")%>' Width="100px"></asp:Label>
                            <asp:HiddenField ID="hid_IS_MAIL" runat="server"  Value='<%#Bind("IS_MAIL")%>' ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                      <%--異動代碼--%>
                    <asp:TemplateField HeaderText="異動代碼" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left" SortExpression="HR_CHG_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_HR_CHG_CD" runat="server" Text='<%#Bind("HR_CHG_CD_DESC")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <%--職務(原)--%>
                    <asp:TemplateField HeaderText="職務(原)" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left"  >
                        <ItemTemplate>
                            <asp:Label ID="lb_PJOB_CD_OLD" runat="server" Text='<%#Bind("PJOB_CD_OLD_DESC")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                      <%--職務--%>
                    <asp:TemplateField HeaderText="職務" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left"  >
                        <ItemTemplate>
                            <asp:Label ID="lb_PJOB_CD" runat="server" Text='<%#Bind("PJOB_CD_DESC")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--職能俸(原)--%>
                    <asp:TemplateField HeaderText="本薪(原)" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right"  >
                        <ItemTemplate>
                            <asp:Label ID="lb_PJOB_PAY_OLD" runat="server" Text='<%#Bind("PJOB_PAY_OLD","{0:N0}")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>   
                      <%--職務俸(原)--%>
                    <asp:TemplateField HeaderText="職務俸(原)" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right"  >
                        <ItemTemplate>
                            <asp:Label ID="lb_ABILITY_PAY_OLD" runat="server" Text='<%#Bind("ABILITY_PAY_OLD","{0:N0}")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>                     
                    <%--職能俸--%>
                    <asp:TemplateField HeaderText="本薪" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right"  >
                        <ItemTemplate>
                            <asp:Label ID="lb_PJOB_PAY" runat="server" Text='<%#Bind("PJOB_PAY","{0:N0}")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>   
                      <%--職務俸--%>
                    <asp:TemplateField HeaderText="職務俸" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right"  >
                        <ItemTemplate>
                            <asp:Label ID="lb_ABILITY_PAY" runat="server" Text='<%#Bind("ABILITY_PAY","{0:N0}")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>   
                     <%--伙食津貼--%>
                    <asp:TemplateField HeaderText="伙食津貼" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right"  >
                        <ItemTemplate>
                            <asp:Label ID="lb_FOOD_PAY" runat="server" Text='<%#Bind("FOOD_PAY","{0:N0}")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>  
                       <%--異動單號--%>
                    <asp:TemplateField HeaderText="異動單號" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left"  >
                        <ItemTemplate>
                            <asp:Label ID="lb_HR_CHG_NO" runat="server" Text='<%#Bind("HR_CHG_NO")%>' Width="120px"></asp:Label>
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
