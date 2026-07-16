<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2ss/WFB2SS0200_Dtl1.aspx.cs" Inherits="WebContent_WFB2SS0200_Dtl1
    " Culture="auto" UICulture="auto" %>

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
            $(".ym").datepicker({ dateFormat: 'yy/mm' });
            $('.ym').mask('9999/99');
            $("#txt_YM").mask("9999/99");
            //GridView必須
            gridviewScroll();
            $.unblockUI();
            //工號取得姓名的ajax
            //寫在這，按查詢才不會消失
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

        function doUnblock() {
            $.unblockUI();
        }
        function doBlock() {
            BlockUI();
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


        //查詢前檢核
        function CheckSearch() {
            var processed = true;

            if (Page_ClientValidate("GroupA")) {
                processed = true;
                BlockUI();
            }
            else {
                processed = false;
            }

            if (!processed)
                $.unblockUI();
            return processed;
        }
        //資料生成前檢核
        function CheckExecute() {
            var processed = true;
            var year = $("#txt_ASSESS_YEAR").val();
            var maxYear = $("#HID_MAX_ASSESS_YEAR").val();
            if (isNaN(year) == false) {
                if (year > maxYear) {
                    alert("尚未有" + year + "年度的考核資料");
                    return false;
                }
            } else {
                return false;
            }

            if (Page_ClientValidate("GroupA")) {
                if (confirm('確定要進行資料生成?')) {
                    processed = true;
                    BlockUI();
                } else {
                    processed = false;
                }
            }
            else {
                processed = false;
            }

            if (!processed)
                $.unblockUI();
            return processed;
        }

        //資料下載
        function checkDowning(msg) {
            var processed = true;
            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else {
                processed = false;
                return false;
            }

            if (processed) {
                processed = confirm("確定要進行" + msg + "?");
                BlockUI();
            }

            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }

        

        //清空畫面
        function ClearAll() {
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#txt_FIRED_SDT").val("");
            $("#txt_FIRED_EDT").val("");
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
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="10%" />
                    <col width="25%" />
                    <col width="10%" />
                    <col width="25%" />
                    <col width="15%" />
                    <col width="15%" />

                </colgroup>
                <tbody>
                     <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_SALARY_DT" runat="server" Text="發薪日期"></asp:Label></th>
                        <td align="left" class="Body_label"  >
                            <asp:TextBox ID="txt_SALARY_DT" runat="server"  CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:TextBox>                                
                        </td>
                         <th align="left" class="Body_TableHeader">
                            <%--資遣類型--%>
                            <asp:Label ID="lb_FIRED_TYPE" runat="server" Text="資遣類型"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_FIRED_DESC" runat="server"  CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:TextBox>                                
                            <asp:HiddenField ID="hid_FIRED_TYPE" runat="server" ClientIDMode="Static" />
                        </td>
                           <%-- 轉薪資 --%>    
                        <th align="left" class="Body_TableHeader">
                        <asp:Label ID="lb_PRE_STATUS" runat="server" Text="轉薪資"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label"> 
                             <asp:TextBox ID="txt_PRE_STATUS" runat="server"  CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:TextBox>                                
                        </td>      
                    </tr>
                    <tr>                        
                        <th align="left" class="Body_TableHeader">
                            <%--資遣日--%>
                            <asp:Label ID="lb_FIRED_DT" runat="server" Text="<%$Resources:Resource,wfb2ss_lb_fire_dt%>"></asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="txt_FIRED_SDT" runat="server" Width="80px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                            ~
                            <asp:TextBox ID="txt_FIRED_EDT" runat="server" Width="80px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                            <!--驗證日期格式-->
                              <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
								ErrorMessage="<%$Resources:Resource,wfb2ss_error_txt_fire_sdt%>" ClientValidationFunction="CheckDate" ForeColor="Red"
								ControlToValidate="txt_FIRED_SDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>	
                             <asp:CustomValidator ID="qry4" runat="server" ValidateEmptyText="true"
								ErrorMessage="<%$Resources:Resource,wfb2ss_error_txt_fire_edt%>" ClientValidationFunction="CheckDate" ForeColor="Red"
								ControlToValidate="txt_FIRED_EDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>	
                            <asp:CompareValidator ID="qry5" runat="server" ControlToCompare="txt_FIRED_SDT"
                                ControlToValidate="txt_FIRED_EDT" ErrorMessage="<%$Resources:Resource,wfb2ss_error_txt_fire_se_dt%>" Type="Date" Operator="GreaterThanEqual"
                                Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <%--工號--%>
                            <asp:Label ID="Label10" runat="server" Text="<%$Resources:Resource,wfb2sp_lb_emp_id%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_ID" runat="server" Width="100px" ClientIDMode="Static" MaxLength="5" > </asp:TextBox>
                            <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" Width="100px"></asp:TextBox>
                        </td>                
                    </tr>
                    <tr>
                       
                       
                    </tr>
                    <tr>
                        <td align="right" colspan="6">
                            <aces:Btn ID="WFB2SS0200Search" runat="server" Text="查詢" OnClick="WFB2SS0200Search_Click" OnClientClick="return CheckSearch();" />
                             <%-- 
                             <asp:Button ID="WFB2SS0200Search" runat="server" Text="查詢" OnClick="WFB2SS0200Search_Click" OnClientClick="return CheckSearch();" />
                             --%>
                            <input id="btn_clear" runat="server" type="button" value="清除" onclick="ClearAll();" />
                             <asp:Button runat="server" ID="btn_back" Text="返回" OnClick="btn_back_Click" />
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
                                <aces:Btn ID="WFB2SS0200Deatil" runat="server" Text="查詢明細" Visible="false" OnClick="WFB2SS0200Deatil_Click"  />
                                <aces:Btn ID="WFB2SS0200ExcelDown" runat="server" Text="退休金匯出" Visible="false" OnClick="WFB2SS0200ExcelDown_Click" OnClientClick="return checkDowning(this.value);" />
                                <%-- 
                                <asp:Button ID="WFB2SP0500Deatil" runat="server" Text="查詢明細" Visible="false" OnClick="WFB2SP0500Deatil_Click" OnClientClick="" />
                                <asp:Button ID="WFB2SP0500ExcelDown" runat="server" Text="退休金匯出" Visible="false" OnClick="WFB2SP0500ExcelDown_Click" OnClientClick="return checkDowning(this.value);" />
                                --%>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SS0200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />                    
                     <asp:ControlParameter ControlID="txt_FIRED_SDT"
                        Name="fire_SDT" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_FIRED_EDT"
                        Name="fire_EDT" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_SALARY_DT"
                        Name="salary_dt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />                    
                    <asp:ControlParameter ControlID="hid_FIRED_TYPE"
                        Name="type" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" /> 
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1019px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">
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
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_rownumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--工號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_emp_id%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="EMP_ID">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--姓名--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_emp_name%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="EMP_NAME">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--資遺日--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ss_lb_fire_dt%>" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Center" SortExpression="FIRED_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_FIRE_DT" runat="server" Text='<%#Bind("FIRED_DT", "{0:yyyy/MM/dd}")%>' Width="160px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--資遣費--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ss_lb_fired_pay%>" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="right">
                        <ItemTemplate>
                            <asp:Label ID="lb_FIRED_PAY" runat="server" Text='<%#Bind("FIRED_PAY","{0:n0}")%>' Width="200px"></asp:Label>
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
            <!-- 每頁的筆數 -->
            <asp:HiddenField ID="HID_MAX_ASSESS_YEAR" runat="server" ClientIDMode="Static" />
            <!-- 每頁的筆數 -->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <!-- 是否要凍結視窗 -->
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="N" />

            <asp:HiddenField runat="server" ClientIDMode="Static" ID="Del_NotChoiceMessage" Value="<%$Resources:Resource,Add_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="Del_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Del_ConfirmMessage%>" />

            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

        <%--EXCEL下載用 --%>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SS0200ExcelDown" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
