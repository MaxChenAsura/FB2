<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dh/WFB2DH0900_Qry.aspx.cs" Inherits="WebContent_fb2dh_WFB2DH0900_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');
            $(".number").mask('999');            

            gridviewScroll();
            $.unblockUI();
            //工號取得姓名的ajax
            $("#txt_EMP_ID").change(function () {
                if ($("#txt_EMP_ID").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_EMP_ID').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_EMP_NAME').val("");
                                //$('#txt_DEPT_NO').val("");
                                //$('#txt_DEPT_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_EMP_NAME').val($.trim(JData.EMP_NAME));
                                //$('#txt_DEPT_NO').val(JData.DEPT_NO);
                                // $('#txt_DEPT_NAME').val(JData.DEPT_NAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_EMP_NAME').val("");
                    //$('#txt_DEPT_NO').val("");
                    //$('#txt_DEPT_NAME').val("");
                }
            });

            //Grid部門代號取得部門名稱的ajax
            $("#txt_DEPT_NO").change(function () {
                if ($("#txt_DEPT_NO").val().length == 7) {
                    $.ajax({
                        url: "../commgeo/WFB2GetDeptData.ashx",
                        data: {
                            DEPT_NO: $('#txt_DEPT_NO').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_DEPT_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_DEPT_NAME').val(JData.DEPT_NAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_DEPT_NAME').val("");
                }

            });
        }

        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }

        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"
                
            });
           
        }

        //清空畫面
        function ClearAll() {
            $("#txt_APPLY_LEAVE_SDT").val("");
            $("#txt_APPLY_LEAVE_EDT").val("");
            $("#txt_APPLY_OVERTIME_SDT").val("");
            $("#txt_APPLY_OVERTIME_EDT").val("");
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#txt_tree_DEPT_NO").val("");
            $("#txt_tree_DEPT_NAME").val("");
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                 <col width="10%" />
                                <col width="20%" />
                                <col width="10%" />
                                <col width="29%" />
                                <col width="10%" />
                                <col width="19%" />
                            </colgroup>
                            <tbody>
                                 <tr>
                                   <%-- 加班區間 --%>    
                                   <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_APPLY_OVERTIME_DT" runat="server" Text="<%$Resources:Resource,wfb2di_lb_APPLY_OVERTIME_DT2%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_APPLY_OVERTIME_SDT" runat="server" ClientIDMode="Static" CssClass="MandatoryField date" Width="80px"></asp:TextBox>~
                                        <asp:TextBox ID="txt_APPLY_OVERTIME_EDT" runat="server" ClientIDMode="Static" CssClass="MandatoryField date" Width="80px"></asp:TextBox>
                                         <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_APPLY_OVERTIME_SDT%>"
                                            ControlToValidate="txt_APPLY_OVERTIME_SDT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_APPLY_OVERTIME_EDT%>"
                                            ControlToValidate="txt_APPLY_OVERTIME_EDT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                          <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="加班日期(起)格式錯誤" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_APPLY_OVERTIME_SDT" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator4" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="加班日期(迄)格式錯誤>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_APPLY_OVERTIME_EDT" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_APPLY_OVERTIME_SDT"
                                            ControlToValidate="txt_APPLY_OVERTIME_EDT" ErrorMessage="加班日期起不能大於加班日期迄" Type="Date" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                    </td>

                                       <%--請假區間--%>                                   
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_APPLY_LEAVE" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label" >
                                        <asp:TextBox ID="txt_APPLY_LEAVE_SDT" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>~
                                       <asp:TextBox ID="txt_APPLY_LEAVE_EDT" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                          <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="請假日期(起)格式錯誤" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_APPLY_LEAVE_SDT" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator3" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="請假日期(迄)格式錯誤>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_APPLY_LEAVE_EDT" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                        <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToCompare="txt_APPLY_LEAVE_SDT"
                                            ControlToValidate="txt_APPLY_LEAVE_EDT" ErrorMessage="請假日期起不能大於請假日期迄" Type="Date" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                    </td>
                          

                                </tr>
                                <tr>
                                    <%-- 工號  --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_EMP_ID%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                        <input id="bt_EMP_ID" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N');" />
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" Width="90px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                    </td>

                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DEPT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_DEPT%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_tree_DEPT_NO" runat="server" MaxLength="7" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                        <input id="bt_DEPT_SEARCH" type="button" value="..." onclick="getDept();" />
                                        <asp:TextBox ID="txt_tree_DEPT_NAME" runat="server" Width="140px" MaxLength="10" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>                                    
                                </tr>
                                
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2DH0900SEARCH" runat="server" Text="查詢" ValidationGroup="GroupA" OnClientClick="CheckValid();" OnClick="WFB2DH0900Search_Click" />
                                             
                                            <%--<asp:Button ID="WFB2DH0900Search" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0700Search%>" ValidationGroup="GroupA" OnClientClick="CheckValid();" OnClick="WFB2DH0900Search_Click" />--%>
                                            
                                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2dh_btn_clear%>" onclick="ClearAll();" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td align="right" class="Body_label">
                        <hr>
                    </td>
                </tr>

            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DH0900DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />                   
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />         
                     <asp:ControlParameter ControlID="txt_tree_DEPT_NO"
                        Name="dept_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />    
                       <asp:ControlParameter ControlID="txt_APPLY_OVERTIME_SDT"
                        Name="over_sdt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" /> 
                       <asp:ControlParameter ControlID="txt_APPLY_OVERTIME_EDT"
                        Name="over_edt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" /> 
                       <asp:ControlParameter ControlID="txt_APPLY_LEAVE_SDT"
                        Name="leave_sdt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" /> 
                       <asp:ControlParameter ControlID="txt_APPLY_LEAVE_EDT"
                        Name="leave_edt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />       
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" Width="1019px"
                OnRowCreated="gv_result_RowCreated" OnDataBound="gv_result_DataBound"
                OnPageIndexChanging="gv_result_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_RowNumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 工號 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh090_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <%-- 姓名 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh090_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>                   
                     <%-- 假日加班日 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh090_APPLY_OVERTIME_DT%>" SortExpression="APPLY_OVERTIME_DT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_APPLY_OVERTIME_DT" runat="server" Text='<%#Bind("APPLY_OVERTIME_DT")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                       <%-- 申告時數 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh090_EXCHANGE_HOUR%>" SortExpression="EXCHANGE_HOUR" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_EXCHANGE_HOUR" runat="server" Text='<%#Bind("EXCHANGE_HOUR")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                      <%-- 剩餘時數 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh090_REST_HOUR%>" SortExpression="REST_HOUR" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_REST_HOUR" runat="server" Text='<%#Bind("REST_HOUR")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <%--表單狀態 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_FORM_STATUS%>" SortExpression="FORM_STATUS" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_FORM_STATUS" runat="server" Text='<%#Bind("FORM_STATUS_DESC")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                      <%-- 換休日期 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh090_CALENDAR_DT%>" SortExpression="CALENDAR_DT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_CALENDAR_DT" runat="server" Text='<%#Bind("CALENDAR_DT")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                      <%-- 換休時數 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh090_MAPPING_HOUR%>" SortExpression="MAPPING_HOUR" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_MAPPING_HOUR" runat="server" Text='<%#Bind("MAPPING_HOUR")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                      <%-- 部門代號 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh090_DEPT_NO%>" SortExpression="DEPT_NO" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_DEPT_NO" runat="server" Text='<%#Bind("DEPT_NO")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 部門名稱 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh090_DEPT_NAME%>"  HeaderStyle-Width="300px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="DEPT_FULL_NAME" runat="server" Text='<%#Bind("DEPT_FULL_NAME")%>' Width="300px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                       <%-- IFLOWNO --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh090_IFLOW_NO%>" SortExpression="IFLOW_NO" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_IFLOW_NO" runat="server" Text='<%#Bind("IFLOW_NO")%>' Width="160px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
               
                <PagerStyle CssClass="GridviewScrollPager" />
                

            </asp:GridView>
            <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">

                    <td class="GridviewScrollPager TD">
                        <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static" AutoPostBack="true">
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_10_Rows%>" Value="10"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_20_Rows%>" Value="20"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_30_Rows%>" Value="30"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_40_Rows%>" Value="40"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_50_Rows%>" Value="50"></asp:ListItem>
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

