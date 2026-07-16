<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sq/WFB2SQ0200_Qry.aspx.cs" Inherits="WebContent_WFB2SQ0200_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">

        $(function () {

            iniForm();
        });
        function iniForm() {
            //日期格式必須
            $(".date").datepicker({ dateFormat: 'yy/mm' });
            $('.date').mask('9999/99');
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
                    freezesize: 4
                    ,headerrowcount: 2
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
        function CheckModeifyAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidwfb2sc_Mod_NotChoiceMessage').val());
                return false;
            }
        }
        function LookUpCheckboxs() {
            var ItemCheckBoxs = $(":checkbox[id$=cb_check]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked && ItemCheckBoxs[i].id.indexOf("cb_all") == -1) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }
        function CheckSTD_END(source, arguments) {
            if ($("#txt_SALARY_YM_SDT").val() != "" & $("#txt_SALARY_YM_EDT").val() != "") {
                if ($("#txt_SALARY_YM_SDT").val().replace("/", "") > $("#txt_SALARY_YM_EDT").val().replace("/", "")) {
                    arguments.IsValid = false;
                }else
                    arguments.IsValid = true;
            } else
                arguments.IsValid = true;
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
            $("#txt_SALARY_YM_SDT").val("");
            $("#txt_SALARY_YM_EDT").val("");
            $("#ddl_IS_CLOSE").val("Y");
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
                            <%--產假年月--%>
                            <asp:Label ID="lb_SALARY_YM" runat="server" Text="<%$Resources:Resource,wfb2sq_lb_SALARY_YM%>"></asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="txt_SALARY_YM_SDT" runat="server" Width="100px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                            ~
                            <asp:TextBox ID="txt_SALARY_YM_EDT" runat="server" Width="100px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                            <!--驗證日期格式-->
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server"
                                       ErrorMessage ="<%$Resources:Resource,wfb2sq_error_txt_SALARY_YM_SDT%>" ControlToValidate="txt_SALARY_YM_SDT" ForeColor="Red" ValidationGroup="GroupA"
                                       ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator> 
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                       ErrorMessage ="<%$Resources:Resource,wfb2sq_error_txt_SALARY_YM_EDT%>" ControlToValidate="txt_SALARY_YM_EDT" ForeColor="Red" ValidationGroup="GroupA"
                                       ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="false"
                                    ErrorMessage="<%$Resources:Resource,wfb2sq_error_SALARY_YM%>" ClientValidationFunction="CheckSTD_END" ForeColor="Red"
                                    ControlToValidate="txt_SALARY_YM_SDT" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>                            
                            
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
                            <asp:DropDownList ID="ddl_IS_CLOSE" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="6">
                            <aces:Btn ID="WFB2SQ0200Search" runat="server" Text="查詢" OnClick="WFB2SQ0200Search_Click" OnClientClick="return CheckSearch();" />
                             <%-- 
                             <asp:Button ID="WFB2SQ0200Search" runat="server" Text="查詢" OnClick="WFB2SQ0200Search_Click" OnClientClick="return CheckSearch();" />
                             --%>
                            <asp:Button ID="btn_clear" runat="server" type="button" Text="<%$Resources:Resource,btn_clear%>" OnClientClick="return ClearAll();" />                            
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
                                <aces:Btn ID="WFB2SQ0200EDIT" runat="server" Text="修改" OnClientClick="return CheckModeifyAction();" OnClick="WFB2SQ0200EDIT_Click" Visible="False" />
                                <aces:Btn ID="WFB2SQ0200ExcelDown" runat="server" Text="報表下載" Visible="false" OnClick="WFB2SQ0200ExcelDown_Click" OnClientClick="return checkDowning(this.value);" />
                                <aces:Btn ID="WFB2SQ0200ExcelDown2" runat="server" Text="明細下載" Visible="false" OnClick="WFB2SQ0200ExcelDown2_Click" OnClientClick="return checkDowning(this.value);" />
                                <aces:Btn ID="WFB2SQ0200Save" runat="server" Text="<%$Resources:Resource,btn_save%>" Visible="false" OnClick="WFB2SQ0200Save_Click" OnClientClick="CheckValid();" ValidationGroup="GroupB" />
                                <%-- 
                                <asp:Button ID="WFB2SQ0200EDIT" runat="server" Text="修改" OnClientClick="return CheckModeifyAction();" OnClick="WFB2SQ0200EDIT_Click" Visible="False" />
                                <asp:Button ID="WFB2SQ0200ExcelDown" runat="server" Text="報表下載" Visible="false" OnClick="WFB2SQ0200ExcelDown_Click" OnClientClick="return checkDowning(this.value);" />
                                <asp:Button ID="WFB2SQ0200ExcelDown2" runat="server" Text="明細下載" Visible="false" OnClick="WFB2SQ0200ExcelDown2_Click" OnClientClick="return checkDowning(this.value);" />
                                <asp:Button ID="WFB2SQ0200Save" runat="server" Text="<%$Resources:Resource,confirm%>" Visible="false" OnClick="WFB2SQ0200Save_Click" OnClientClick="CheckValid();" ValidationGroup="GroupB" />
                                --%>
                                <asp:Button ID="btn_cancel" runat="server" Text="<%$Resources:Resource,btn_cancel%>" Visible="false" OnClick="btn_cancel_Click" OnClientClick="return confirm($('#Cancel_ConfirmMessage').val());" />
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SQ0200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_SALARY_YM_SDT"
                        Name="ym_st" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_SALARY_YM_EDT"
                        Name="ym_ed" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_IS_CLOSE"
                        Name="is_close" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
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
                        <EditItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" Checked="true" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2sc_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Style="text-align: center;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Style="text-align: center;"></asp:Label>
                        </EditItemTemplate>                        
                    </asp:TemplateField>
                    <%--工號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sq_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>' Width="100%" Style="text-align: left;"></asp:Label>                            
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_EMP_ID_Add" ClientIDMode="Static" MaxLength="5" runat="server" Text='<%#Bind("EMP_ID")%>' Width="100%" Style="text-align: left;" />                            
                                                
                        </EditItemTemplate>                        
                    </asp:TemplateField>
                    <%--姓名--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sq_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>' Width="100%" Style="text-align: left;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_EMP_NAME_Add" ClientIDMode="Static" runat="server" Text='<%#Bind("EMP_NAME")%>' Width="100%" Style="text-align: left;" />
                        </EditItemTemplate>                        
                    </asp:TemplateField>
                    <%--事實發生日--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sq_MATERNITY_SDT%>" SortExpression="MATERNITY_SDT" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_MATERNITY_SDT" runat="server" Text='<%#Bind("MATERNITY_SDT")%>' Width="100%" Style="text-align: left;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_MATERNITY_SDT_Add" ClientIDMode="Static" runat="server" Text='<%#Bind("MATERNITY_SDT")%>' Width="100%" Style="text-align: left;"></asp:Label>
                            <asp:HiddenField ID="hid_SALARY_YM" ClientIDMode="Static" runat="server" Value='<%#Bind("SALARY_YM")%>' />        
                        </EditItemTemplate>                        
                    </asp:TemplateField>
                    <%--產假起日--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sq_APPLY_LEAVE_SDT%>" SortExpression="APPLY_LEAVE_SDT" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_APPLY_LEAVE_SDT" runat="server" Text='<%#Bind("APPLY_LEAVE_SDT")%>' Width="100%" Style="text-align: left;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_APPLY_LEAVE_SDT_Add" ClientIDMode="Static" runat="server" Text='<%#Bind("APPLY_LEAVE_SDT")%>' Width="100%" Style="text-align: left;"></asp:Label>                            
                        </EditItemTemplate>                        
                    </asp:TemplateField>
                    <%--產假迄日--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sq_APPLY_LEAVE_EDT%>" SortExpression="APPLY_LEAVE_EDT" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_APPLY_LEAVE_EDT" runat="server" Text='<%#Bind("APPLY_LEAVE_EDT")%>' Width="100%" Style="text-align: left;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_APPLY_LEAVE_EDT_Add" ClientIDMode="Static" runat="server" Text='<%#Bind("APPLY_LEAVE_EDT")%>' Width="100%" Style="text-align: left;"></asp:Label>                            
                        </EditItemTemplate>                        
                    </asp:TemplateField>
                    <%--產假天數--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sq_MATERNITY_SUMDAY%>" SortExpression="MATERNITY_SUMDAY" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_MATERNITY_SUMDAY" runat="server" Text='<%#Bind("MATERNITY_SUMDAY")%>' Width="100%" Style="text-align: left;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_MATERNITY_SUMDAY_Add" ClientIDMode="Static" runat="server" Text='<%#Bind("MATERNITY_SUMDAY")%>' Width="100%" Style="text-align: left;"></asp:Label>                            
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <%--(合併)日薪六個月平均--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sq_SIX_MONTH_DAILY%>" SortExpression="SIX_MONTH_DAILY" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                        <ItemTemplate>
                            <asp:Label ID="lb_SIX_MONTH_DAILY" runat="server" Text='<%#Bind("SIX_MONTH_DAILY","{0:n0}")%>' Width="100%" Style="text-align: right;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_SIX_MONTH_DAILY_Add" ClientIDMode="Static" runat="server" Text='<%#Bind("SIX_MONTH_DAILY","{0:n0}")%>' Width="100%" Style="text-align: right;"></asp:Label>                            
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--(合併)日薪前月工資--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sq_LAST_MONTH_DAILY%>" SortExpression="LAST_MONTH_DAILY" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                        <ItemTemplate>
                            <asp:Label ID="lb_LAST_MONTH_DAILY" runat="server" Text='<%#Bind("LAST_MONTH_DAILY","{0:n0}")%>' Width="100%" Style="text-align: right;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_LAST_MONTH_DAILY_Add" ClientIDMode="Static" runat="server" Text='<%#Bind("LAST_MONTH_DAILY","{0:n0}")%>' Width="100%" Style="text-align: right;"></asp:Label>                            
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--(合併)日薪本月工資--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sq_THIS_MONTH_DAILY%>" SortExpression="THIS_MONTH_DAILY" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                        <ItemTemplate>
                            <asp:Label ID="lb_THIS_MONTH_DAILY" runat="server" Text='<%#Bind("THIS_MONTH_DAILY","{0:n0}")%>' Width="100%" Style="text-align: right;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_THIS_MONTH_DAILY_Add" ClientIDMode="Static" runat="server" Text='<%#Bind("THIS_MONTH_DAILY","{0:n0}")%>' Width="100%" Style="text-align: right;"></asp:Label>                            
                        </EditItemTemplate>
                    </asp:TemplateField>
                    
                    <%--產假補貼--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sq_MATERNITY_AMOUNT%>" SortExpression="MATERNITY_AMOUNT" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                        <ItemTemplate>
                            <asp:Label ID="lb_MATERNITY_AMOUNT" runat="server" Text='<%#Bind("MATERNITY_AMOUNT","{0:n0}")%>' Width="100%" Style="text-align: right;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_MATERNITY_AMOUNT_Add" ClientIDMode="Static" runat="server" Text='<%#Bind("MATERNITY_AMOUNT","{0:n0}")%>' Width="100%" Style="text-align: right;"></asp:Label>                            
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--備註--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sq_REMARK%>" SortExpression="REMARK" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_REMARK" runat="server" Text='<%#Bind("REMARK")%>' Width="100%" Style="text-align: left;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_REMARK_Add" ClientIDMode="Static" CssClass="MandatoryField" MaxLength="100" runat="server" Width="100%" Style="text-align: left;" />
                            <asp:HiddenField ID="hid_REMARK" ClientIDMode="Static" runat="server" Value='<%#Bind("REMARK")%>' />  
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--結案否--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sq_IS_CLOSE%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="IS_CLOSE">
                        <ItemTemplate>
                            <asp:Label ID="lb_IS_CLOSE" runat="server" Text='<%#Bind("IS_CLOSE_DESC")%>' Width="100px"></asp:Label>                                  
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_IS_CLOSE_Add" ClientIDMode="Static" runat="server" Width="100%" Style="text-align: center;" />
                            <asp:HiddenField ID="hid_IS_CLOSE" ClientIDMode="Static" runat="server" Value='<%#Bind("IS_CLOSE")%>' />  
                        </EditItemTemplate>
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
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
            <!-- 進行新增或修改的檢核 -->
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Mod_NotChoiceMessage" Value="<%$Resources:Resource,wfb2sc_Mod_NotChoiceMessage%>" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="Cancel_ConfirmMessage" Value="<%$Resources:Resource,Cancel_ConfirmMessage%>" />
        </ContentTemplate>

        <%--EXCEL下載用 --%>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SQ0200ExcelDown" />
            <asp:PostBackTrigger ControlID="WFB2SQ0200ExcelDown2" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
