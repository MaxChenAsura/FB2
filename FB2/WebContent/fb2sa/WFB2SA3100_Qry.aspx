<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sa/WFB2SA3100_Qry.aspx.cs" Inherits="WebContent_WFB2SA3100_Qry" Culture="auto" UICulture="auto" %>

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
            $(".ym").datepicker({ dateFormat: 'yy/mm' });
            $('.ym').mask('9999/99');
            $(".numFormat").mask('9.999');
            $(".decimal").css("text-align", "right").css("ime-mode", "disabled");

            //GridView必須
            gridviewScroll();
            $.unblockUI();
            

            $('#txt_EMP_NAME').attr("readonly", true);
            $("#txt_EMP_ID").change(function () {
                if ($("#txt_EMP_ID").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_EMP_ID').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        cache: false,
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_EMP_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_EMP_NAME').val($.trim(JData.EMP_NAME));

                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_EMP_NAME').val("");
                }
            });
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
            if (!Page_ClientValidate("GroupA")) {
                processed = false;
            }
            if (processed)
                BlockUI();
            if (!processed)
                $.unblockUI();
            return processed;
        }


        //清空畫面
        function ClearAll() {
            $("#ddl_IS_MAIL").val("-1");
            $("#txt_YM").val("");
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
                    <col width="30%" />
                    <col width="10%" />
                    <col width="30%" />
                    <col width="10%" />
                    <col width="10%" />
                </colgroup>
                <tbody>
                     <tr>
                         <%--年月--%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_YM" runat="server" Text="異動年月"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_YM" runat="server" MaxLength="7" CssClass="MandatoryField ym" ClientIDMode="Static" Width="64px"></asp:TextBox>
                              <asp:RequiredFieldValidator ID="req_YM" runat="server" ErrorMessage="異動年月必輸入"
                            ControlToValidate="txt_YM" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="異動年月格式錯誤"
                                ControlToValidate="txt_YM" ForeColor="Red" ValidationGroup="GroupA"
                                ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                        </td>                       
                    </tr>
                     <tr>
                        <%-- 工號 --%>    
                        <th align="left" class="Body_TableHeader">
                        <asp:Label ID="lb_EMP_D" runat="server" Text="工號"></asp:Label>:
                        </th>
                         <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_ID" runat="server" Width="80px" ClientIDMode="Static" MaxLength="5" CssClass="" > </asp:TextBox>                           
                            <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" Width="80px"></asp:TextBox>                                        
                        </td>               
                        <th align="left" class="Body_TableHeader">
                        <%--寄件否--%>
                        <asp:Label ID="lb_IS_MAIL" runat="server" Text="寄件否"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                                <asp:DropDownList ID="ddl_IS_MAIL" runat="server" ClientIDMode="Static"  ></asp:DropDownList>       
                        </td>
                    </tr>
                               
                    <tr>
                        <td align="right" colspan="6">
                            <aces:Btn ID="WFB2SA3100Search" runat="server" Text="查詢" OnClick="WFB2SA3100Search_Click" OnClientClick="return CheckSearch();" />
                            <aces:Btn ID="WFB2SA3100GEN" runat="server" Text="對象生成" OnClick="WFB2SA3100GEN_Click" OnClientClick="return CheckSearch();"   />
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
                                <aces:Btn ID="WFB2SA3100DEL_MAIL" runat="server" Text="不寄" Visible="false" OnClick="WFB2SA3100DEL_MAIL_Click" OnClientClick="BlockUI();" />
                                <aces:Btn ID="WFB2SA3100RE_MAIL"  runat="server" Text="重寄" Visible="false" OnClick="WFB2SA3100RE_MAIL_Click" OnClientClick="BlockUI();" />
                                <aces:Btn ID="WFB2SA3100DO_MAIL"  runat="server" Text="已寄" Visible="false" OnClick="WFB2SA3100DO_MAIL_Click" OnClientClick="BlockUI();" />
                                <aces:Btn ID="WFB2SA3100MAIL"     runat="server" Text="寄件通知" Visible="false" OnClick="WFB2SA3100MAIL_Click" OnClientClick="BlockUI();" />
                                <aces:Btn ID="WFB2SA3100EXCELDOWN" runat="server" Text="EXCEL下載" Visible="false" OnClick="WFB2SA3100EXCELDOWN_Click" OnClientClick="BlockUI();" />
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SA3100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                     <asp:ControlParameter ControlID="txt_YM"
                        Name="ym" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" /> 
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" /> 
                    <asp:ControlParameter ControlID="ddl_IS_MAIL"
                        Name="is_mail" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />         
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1300px"
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
                    </asp:TemplateField>
                     <%--異動日期--%>
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
                <%--匯出EXCEL按鈕必寫--%>
            <asp:PostBackTrigger ControlID="WFB2SA3100EXCELDOWN" />  
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
