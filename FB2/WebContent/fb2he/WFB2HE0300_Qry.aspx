<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2he/WFB2HE0300_Qry.aspx.cs" Inherits="WebContent_WFB2HE0300_Qry" Culture="auto" UICulture="auto" %>

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
            $('#txt_ADOPT_NAME').attr("readonly", true);
            $("#txt_ADOPT_BY").change(function () {
                if ($("#txt_ADOPT_BY").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_ADOPT_BY').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        cache: false,
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_ADOPT_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_ADOPT_NAME').val($.trim(JData.EMP_NAME));
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_ADOPT_NAME').val("");
                }
            });
            //工號取得姓名的ajax
            $('#txt_APPROVE_NAME').attr("readonly", true);
            $("#txt_APPROVE_BY").change(function () {
                if ($("#txt_APPROVE_BY").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_APPROVE_BY').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        cache: false,
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_APPROVE_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_APPROVE_NAME').val($.trim(JData.EMP_NAME));
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_APPROVE_NAME').val("");
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
                    freezesize: 5
                });
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

        //清空畫面
        function ClearAll() {
            $("#txt_PJOB_CD").val("");
            $("#ddl_INTERVIEW_PROCESS_STATUS").val("-1");
            $("#txt_ADOPT_DT_S").val("");
            $("#txt_ADOPT_DT_E").val("");
            $("#txt_ADOPT_BY").val("");
            $("#ddl_ADOPT_RESULT").val("Y");
            $("#txt_APPROVE_DT_S").val("");
            $("#txt_APPROVE_DT_E").val("");
            $("#txt_APPROVE_BY").val("");
            $("#ddl_APPROVE_STATUS").val("N");
        }

        //檢查是否有勾選
        function doCheck(value) {
            var processed = true;
            BlockUI();
            if (checkboxsSelected() > 0) {
                processed = confirm("確定要"+value+"?");
            } else {
                alert("請選取資料!");
                processed = false;
            }
            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }

        var choietr = null;
        //回傳目前Checkbox被勾選的數量
        function checkboxsSelected() {
            choietr = null;
            var ItemCheckBoxs = $(":checkbox[id$=cb_check]");
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
        }
        //確認
        function doCheck(value) {
            var processed = true;
            BlockUI();
            if (checkboxsSelected() > 0) {
                processed = confirm("確定要" + value + "?");
            } else {
                alert("請選取資料!");
                processed = false;
            }
            if (!processed) {
                $.unblockUI();
            }
            return processed;
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
                    <col width="10%" />
                    <col width="20%" />

                </colgroup>
                <tbody>
                    <tr>
                          <%--應徵職務--%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_PJOB_CD" runat="server" Text="<%$Resources:Resource,wfb2he_lb_pjob_cd%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_PJOB_CD" runat="server" Width="80px" ClientIDMode="Static" MaxLength="4"> </asp:TextBox>
                        </td>
                        <%--面試處理狀態--%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2he_lb_interview_process_status%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                              <asp:DropDownList ID="ddl_INTERVIEW_PROCESS_STATUS" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                        <th></th><td></td>
                    </tr>
                    <tr>
                          <%--採用日期--%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2he_lb_adopt_dt%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                              <asp:TextBox ID="txt_ADOPT_DT_S" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                        ~
                            <asp:TextBox ID="txt_ADOPT_DT_E" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_ADOPT_DT_S"
                                ControlToValidate="txt_ADOPT_DT_E" ErrorMessage="<%$Resources:Resource,wfb2he_error_approve_dt_se%>" Type="Date" Operator="GreaterThanEqual"
                                Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                            <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2he_error_adopt_dt_s%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_ADOPT_DT_S" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2he_error_adopt_dt_e%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_ADOPT_DT_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </td>
                        <%--採用人員--%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_ADOPT_BY" runat="server" Text="<%$Resources:Resource,wfb2he_lb_adopt_by%>"></asp:Label>
                        <td align="left" class="Body_label">
                           <asp:TextBox ID="txt_ADOPT_BY" runat="server" Width="50px" ClientIDMode="Static" MaxLength="5"> </asp:TextBox>
                            <input id="Button1" type="button" value="..." onclick="OpenEmpSearch('txt_ADOPT_BY', 'txt_ADOPT_NAME', 'N', '');" />
                            <asp:TextBox ID="txt_ADOPT_NAME" runat="server" Width="100px" ClientIDMode="Static" BorderWidth="0" ></asp:TextBox>
                        </td>
                         <%--採用結果--%>
                          <th align="left" class="Body_TableHeader">
                             <asp:Label ID="lb_ADOPT_RESULT" runat="server" Text="<%$Resources:Resource,wfb2he_lb_adopt_result%>"></asp:Label>:
                        </th>
                        <td>
                             <asp:DropDownList ID="ddl_ADOPT_RESULT" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                          <%--簽核日期--%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2he_lb_approve_dt%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_APPROVE_DT_S" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                        ~
                            <asp:TextBox ID="txt_APPROVE_DT_E" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                            <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToCompare="txt_APPROVE_DT_S"
                                ControlToValidate="txt_APPROVE_DT_E" ErrorMessage="<%$Resources:Resource,wfb2he_error_adopt_dt_se%>" Type="Date" Operator="GreaterThanEqual"
                                Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                            <asp:CustomValidator ID="CustomValidator3" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2he_error_approve_dt_s%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_APPROVE_DT_S" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator4" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2he_error_approve_dt_e%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_APPROVE_DT_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </td>
                        <%--簽核人員--%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPROVE_BY" runat="server" Text="<%$Resources:Resource,wfb2he_lb_approve_by%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                           <asp:TextBox ID="txt_APPROVE_BY" runat="server" Width="50px" ClientIDMode="Static" MaxLength="5"> </asp:TextBox>
                            <input id="Button2" type="button" value="..." onclick="OpenEmpSearch('txt_APPROVE_BY', 'txt_APPROVE_NAME', 'N', '');" />
                            <asp:TextBox ID="txt_APPROVE_NAME" runat="server" Width="100px" ClientIDMode="Static" BorderWidth="0" ></asp:TextBox>
                        </td>
                        </td>
                         <%-- 簽核狀態--%>
                         <th align="left" class="Body_TableHeader">
                             <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2he_lb_approve_status%>"></asp:Label>:
                        </th>
                        <td>
                             <asp:DropDownList ID="ddl_APPROVE_STATUS" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="6">
                             <asp:Button ID="WFB2HE0300Search" runat="server" Text="查詢" OnClick="WFB2HE0300Search_Click" OnClientClick="return CheckSearch();" />
                            <input id="btn_clear" runat="server" type="button" value="清除" onclick="ClearAll();" />
                            <%-- 
                            
                            --%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="Body_label" colspan="3">
                              <table id="batchRemarkTable">
                                    <tr>
                                        <%-- 簽核備註 --%>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_APPROVE_REMARK" runat="server" Text='<%$Resources:Resource,wfb2he_lb_approve_remark%>' ClientIDMode="Static" />:
                                        </th>
                                        <td align="left" class="Body_label">
                                             <asp:TextBox ID="txt_APPROVE_REMARK" runat="server" Width="300px" ClientIDMode="Static" MaxLength="210"> </asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                        </td>
                        <td align="right" class="Body_label" colspan="3">
                            <div id="init_grid">
                                 <asp:Button ID="WFB2HE0300Approve" runat="server" Text="核可"      OnClick="WFB2HE0300Approve_Click" OnClientClick="return doCheck(this.value);"  />
                                <asp:Button ID="WFB2HE0300Reject" runat="server"   Text="駁回"      OnClick="WFB2HE0300Reject_Click" OnClientClick="return doCheck(this.value);" />
                                <asp:Button ID="WFB2HE0300Detail" runat="server"   Text="查詢明細"  OnClick="WFB2HE0300Detail_Click" OnClientClick="BlockUI();" />
                                <%-- 
                                <asp:Button ID="WFB2HE0300Approve" runat="server" Text="核可" Visible="false"  OnClick="WFB2HE0300Approve_Click" OnClientClick="return doDelete();"  />
                                <asp:Button ID="WFB2HE0300Reject" runat="server" Text="駁回" Visible="false" OnClick="WFB2HE0300Reject_Click" OnClientClick="BlockUI();" />
                                --%>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2HE0300DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                     <asp:ControlParameter ControlID="txt_PJOB_CD"
                        Name="pjob_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_INTERVIEW_PROCESS_STATUS"
                        Name="interview_process_status" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
                     <asp:ControlParameter ControlID="txt_ADOPT_DT_S"
                        Name="adopt_DT_S" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_ADOPT_DT_E"
                        Name="adopt_DT_E" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_ADOPT_BY"
                        Name="adopt_by" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_ADOPT_RESULT"
                        Name="adopt_result" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_APPROVE_DT_S"
                        Name="approve_DT_S" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_APPROVE_DT_E"
                        Name="approve_DT_E" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_APPROVE_BY"
                        Name="approve_by" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                     <asp:ControlParameter ControlID="ddl_APPROVE_STATUS"
                        Name="approve_status" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="SelectNonDisabledCheckboxes(this);" ClientIDMode="Static" Width="20px" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" Width="20px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--序號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2he_lb_rownumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--姓名--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2he_lb_emp_name%>" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Left" SortExpression="EMP_NAME">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--應徵日期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2he_lb_apply_dt%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="APPLY_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_APPLY_DT" runat="server" Text='<%#Bind("APPLY_DT","{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--應徵職務--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2he_lb_pjob_cd%>" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left" SortExpression="PJOB_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_PJOB_CD_DESC" runat="server" Text='<%#Bind("PJOB_CD_DESC")%>' Width="120px"></asp:Label>
                             <%-- 應徵職務代號--%>
                            <asp:HiddenField ID="hid_PJOB_CD" runat="server" Value='<%#Bind("PJOB_CD")%>' ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--員工區分--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2he_lb_emp_cd%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left" SortExpression="EMP_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_CD_DESC" runat="server" Text='<%#Bind("EMP_CD_DESC")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--資格代號 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2he_lb_level_cd%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" SortExpression="LEVEL_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEVEL_CD" runat="server" Text='<%#Bind("LEVEL_CD")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--級數代號  --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2he_lb_grade_cd%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" SortExpression="GRADE_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_GRADE_CD" runat="server" Text='<%#Bind("GRADE_CD")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--採用結果--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2he_lb_adopt_result%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" SortExpression="ADOPT_RESULT">
                        <ItemTemplate>
                            <asp:Label ID="lb_ADOPT_RESULT_DESC" runat="server" Text='<%#Bind("ADOPT_RESULT_DESC")%>' Width="80px"></asp:Label>
                            <%-- 採用結果--%>
                            <asp:HiddenField ID="hid_ADOPT_RESULT" runat="server" Value='<%#Bind("ADOPT_RESULT")%>' ClientIDMode="Static" />
                                
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--採用人員--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2he_lb_adopt_by%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" SortExpression="ADOPT_BY">
                        <ItemTemplate>
                             <asp:Label ID="lb_ADOPT_NAME" runat="server" Text='<%#Bind("ADOPT_NAME")%>'  ClientIDMode="Static"  Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--採用日期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2he_lb_adopt_dt%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="ADOPT_DT">
                        <ItemTemplate>
                             <asp:Label ID="lb_ADOPT_DT" runat="server" Text='<%#Bind("ADOPT_DT","{0:yyyy/MM/dd}")%>'  ClientIDMode="Static" Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--簽核結果--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2he_lb_approve_status%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" SortExpression="APPROVE_STATUS">
                        <ItemTemplate>
                            <asp:Label ID="lb_APPROVE_STATUS_DESC" runat="server" Text='<%#Bind("APPROVE_STATUS_DESC")%>' Width="80px"></asp:Label>
                            <%-- 簽核結果--%>
                            <asp:HiddenField ID="hid_APPROVE_STATUS" runat="server" Value='<%#Bind("APPROVE_STATUS")%>' ClientIDMode="Static" />
                                
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--簽核人員--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2he_lb_approve_by%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" SortExpression="APPROVE_BY">
                        <ItemTemplate>
                             <asp:Label ID="lb_APPROVE_NAME" runat="server" Text='<%#Bind("APPROVE_NAME")%>'  ClientIDMode="Static"  Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--簽核日期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2he_lb_approve_dt%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="APPROVE_DT">
                        <ItemTemplate>
                             <asp:Label ID="lb_APPROVE_DT" runat="server" Text='<%#Bind("APPROVE_DT","{0:yyyy/MM/dd}")%>'  ClientIDMode="Static"  Width="100px"></asp:Label>
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
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>


    </asp:UpdatePanel>
</asp:Content>
