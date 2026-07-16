<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sp/WFB2SP0200_Qry.aspx.cs" Inherits="WebContent_WFB2SP0200_Qry" Culture="auto" UICulture="auto" %>

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
            $("#ddl_COMPUTER_TYPE").val("A");
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#txt_RETIRE_SDT").val("");
            $("#txt_RETIRE_EDT").val("");
            $("#ddl_CLOSE_YN").val("Y");
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
                            <%--計算類別--%>
                            <asp:Label ID="Label11" runat="server" Text="<%$Resources:Resource,wfb2sp_lb_computer_type%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_COMPUTER_TYPE" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <%--退休日--%>
                            <asp:Label ID="lb_AWARD_ITEM" runat="server" Text="<%$Resources:Resource,wfb2sp_lb_retire_dt%>"></asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="txt_RETIRE_SDT" runat="server" Width="100px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                            ~
                            <asp:TextBox ID="txt_RETIRE_EDT" runat="server" Width="100px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                            <!--驗證日期格式-->
                              <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
								ErrorMessage="<%$Resources:Resource,wfb2sp_error_txt_retire_sdt%>" ClientValidationFunction="CheckDate" ForeColor="Red"
								ControlToValidate="txt_RETIRE_SDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>	
                             <asp:CustomValidator ID="qry4" runat="server" ValidateEmptyText="true"
								ErrorMessage="<%$Resources:Resource,wfb2sp_error_txt_retire_edt%>" ClientValidationFunction="CheckDate" ForeColor="Red"
								ControlToValidate="txt_RETIRE_EDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>	
                            <asp:CompareValidator ID="qry5" runat="server" ControlToCompare="txt_RETIRE_SDT"
                                ControlToValidate="txt_RETIRE_EDT" ErrorMessage="<%$Resources:Resource,wfb2sp_error_txt_retire_se_dt%>" Type="Date" Operator="GreaterThanEqual"
                                Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                        </td>
                        <th></th>
                        <td></td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--工號--%>
                            <asp:Label ID="Label10" runat="server" Text="<%$Resources:Resource,wfb2sp_lb_emp_id%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_ID" runat="server" Width="50px" ClientIDMode="Static" MaxLength="5" > </asp:TextBox>
                            <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" Width="80px"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <%--結案--%>
                            <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2sp_lb_close_yn%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_CLOSE_YN" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="6">
                            <aces:Btn ID="WFB2SP0200Search" runat="server" Text="查詢" OnClick="WFB2SP0200Search_Click" OnClientClick="return CheckSearch();" />
                             <%-- 
                             <asp:Button ID="WFB2SP0200Search" runat="server" Text="查詢" OnClick="WFB2SP0200Search_Click" OnClientClick="return CheckSearch();" />
                             --%>
                            <input id="btn_clear" runat="server" type="button" value="清除" onclick="ClearAll();" />
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
                                <aces:Btn ID="WFB2SP0200Deatil" runat="server" Text="查詢明細" Visible="false" OnClick="WFB2SP0200Deatil_Click" OnClientClick="" />
                                <aces:Btn ID="WFB2SP0200ExcelDown" runat="server" Text="退休金匯出" Visible="false" OnClick="WFB2SP0200ExcelDown_Click" OnClientClick="return checkDowning(this.value);" />
                                <%-- 
                                <asp:Button ID="WFB2SP0200Deatil" runat="server" Text="查詢明細" Visible="false" OnClick="WFB2SP0200Deatil_Click" OnClientClick="" />
                                <asp:Button ID="WFB2SP0200ExcelDown" runat="server" Text="退休金匯出" Visible="false" OnClick="WFB2SP0200ExcelDown_Click" OnClientClick="return checkDowning(this.value);" />
                                --%>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SP0200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="ddl_COMPUTER_TYPE"
                        Name="computer_type" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
                     <asp:ControlParameter ControlID="txt_RETIRE_SDT"
                        Name="retire_SDT" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_RETIRE_EDT"
                        Name="retire_EDT" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_CLOSE_YN"
                        Name="close_YN" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
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
                    <%--退休日--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_retire_dt%>" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Center" SortExpression="RETIRE_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_RETIRE_DT" runat="server" Text='<%#Bind("RETIRE_DT", "{0:yyyy/MM/dd}")%>' Width="160px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--退休金--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_retire_pay_tmp%>" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="right">
                        <ItemTemplate>
                            <asp:Label ID="lb_RETIRE_PAY" runat="server" Text='<%#Bind("RETIRE_PAY","{0:n0}")%>' Width="200px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--結案否--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_close_yn%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="CLOSE_YN">
                        <ItemTemplate>
                            <asp:Label ID="lb_CLOSE_YN" runat="server" Text='<%#Bind("CLOSE_YN")%>' Width="100px"></asp:Label>
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
            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

        <%--EXCEL下載用 --%>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SP0200ExcelDown" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
