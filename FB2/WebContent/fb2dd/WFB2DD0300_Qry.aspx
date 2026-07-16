<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dd/WFB2DD0300_Qry.aspx.cs" Inherits="WebContent_fb2dd_WFB2DD0300_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $('.ym').mask('9999/99');
            $(".date").datepicker({ dateFormat: 'yy/mm' });
            $("#txt_MANAGER_YM").mask("9999/99");
            $("#txt_EMP_ID").mask("99999");
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

        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "400",
                barcolor: "#7F7F7F",
                freezesize: 3
            });
          

        }

        //清空畫面
        function ClearAll() {
            $('#ddl_FACTORY_CD').val(-1);
            $('#ddl_ALLOWANCE_CD').val(-1);
            $("#txt_MANAGER_YM").val("");
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
           
        }

        function CheckSearch() {

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                return false;
        }

        //儲存前檢查
        function saveCheck() {
            var processed = true;
            //alert($("#hid_Valid_Flag").val());
            if ($("#txt_MANAGER_YM").val() == '' && $("#ddl_FACTORY_CD").val() =='-1'
                && $("#txt_EMP_ID").val() == '' && $("#txt_EMP_NAME").val() ==''
                && $("#ddl_ALLOWANCE_CD").val() == '-1') {
                alert("請至少填選一項查詢條件!!");
                processed = false;
                return false;
            }            

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
        }


        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <COL width="15%" />
					<COL width="15%" />
					<COL width="15%" />
					<COL width="15%" />
					<COL width="40%" />
                </colgroup>
                <tbody>
                    <!-- START: 1st Line in Search Criteria area -->
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_MANAGER_YM" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_MANAGER_YM%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" >
                            <asp:TextBox ID="txt_MANAGER_YM" runat="server" class="date"   MaxLength="7" Width="81px" ClientIDMode="Static"></asp:TextBox>                            
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2dd_error_MANAGER_YM%>" ControlToValidate="txt_MANAGER_YM" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_FACTORY_CD" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_FACTORY_CD%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_FACTORY_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>                            
                        </td>                      
                    </tr>
                    <tr>
                         <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_EMP_ID%>"></asp:Label>:
                         </th>
                         <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="81px" ClientIDMode="Static"></asp:TextBox>                         
                            <input id="bt_EMP_ID" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N');" />
                         </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_EMP_NAME%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="30" Width="81px" ClientIDMode="Static"></asp:TextBox>                         
                        </td>                      
                    </tr>
                    <tr>
                         <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_ALLOWANCE_CD" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_ALLOWANCE_CD%>"></asp:Label>:
                         </th>
                         <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_ALLOWANCE_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>                            
                        </td>
                                       
                    </tr>
                    <tr>
                        <td align="right" class="Body_label" colspan="4">
                            <div id="init">
                                <aces:Btn ID="WFB2DD0300Search" runat="server" Text="<%$Resources:Resource,wfb2dd_WFB2DD0100Search%>" OnClick="WFB2DD0300Search_Click" OnClientClick="return saveCheck();"  />

                                <%--<asp:Button ID="WFB2DD0300Search" runat="server" Text="<%$Resources:Resource,wfb2dd_WFB2DD0100Search%>" OnClick="WFB2DD0300Search_Click" OnClientClick="return saveCheck();" style="height: 21px" />--%>
                                
                                <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2dd_btn_clear%>" onclick="ClearAll();"/>                               
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" height="1" colspan="4">
                            <hr>
                        </td>
                    </tr>                   
                    <!-- END: Create a line -->
                </tbody>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DD0300DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_MANAGER_YM"
                        Name="MANAGER_YM" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_FACTORY_CD" DefaultValue=""
                        Name="FACTORY_CD" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />   
                     <asp:ControlParameter ControlID="txt_EMP_ID" DefaultValue=""
                        Name="EMP_ID" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                     <asp:ControlParameter ControlID="txt_EMP_NAME" DefaultValue=""
                        Name="EMP_NAME" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                     <asp:ControlParameter ControlID="ddl_ALLOWANCE_CD" DefaultValue=""
                        Name="ALLOWANCE_CD" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />                
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1500px" 
                OnPageIndexChanging="gv_result_PageIndexChanging" meta:resourcekey="gv_resultResource1" >
                <Columns>                   
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2dd_RowNumber%>" HeaderStyle-Width="40px"/>
                    <asp:BoundField DataField="MANAGER_YM" HeaderText="<%$Resources:Resource,wfb2dd_lb_MANAGER_YM%>" SortExpression="MANAGER_YM" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="left"/>
                    <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2dd_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="left"/>
                    <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2dd_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="left"/>
                    <asp:BoundField DataField="LEVEL_CD" HeaderText="<%$Resources:Resource,wfb2dd_lb_LEVEL_CD%>" SortExpression="LEVEL_CD" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="left"/>
                    <asp:BoundField DataField="AGE" HeaderText="<%$Resources:Resource,wfb2dd_lb_AGE%>" SortExpression="AGE" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="left"/>
                    <asp:BoundField DataField="ALLOWANCE_CD" HeaderText="<%$Resources:Resource,wfb2dd_lb_ALLOWANCE_CD%>" SortExpression="ALLOWANCE_CD" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="left"/>
                    <asp:BoundField DataField="TOTAL_PAY" HeaderText="<%$Resources:Resource,wfb2dd_lb_TOTAL_PAY%>" SortExpression="TOTAL_PAY" HeaderStyle-Width="80px" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="right"/>
                    <asp:BoundField DataField="DAILY_PAY" HeaderText="<%$Resources:Resource,wfb2dd_lb_DAILY_PAY%>" SortExpression="DAILY_PAY" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="right"/>
                    <asp:BoundField DataField="WORKING_DT" HeaderText="<%$Resources:Resource,wfb2dd_lb_WORKING_DT%>" SortExpression="WORKING_DT" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="right"/>
                    <asp:BoundField DataField="START_DT" HeaderText="<%$Resources:Resource,wfb2dd_lb_START_DT%>" SortExpression="START_DT" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="left"/>
                    <asp:BoundField DataField="FACTORY_CD" HeaderText="<%$Resources:Resource,wfb2dd_lb_FACTORY_CD%>" SortExpression="FACTORY_CD" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="left"/>
                    <asp:BoundField DataField="AREA_CD" HeaderText="<%$Resources:Resource,wfb2dd_lb_AREA_CD%>" SortExpression="AREA_CD" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="left"/>
                    <asp:BoundField DataField="TRANSPORT_CD" HeaderText="<%$Resources:Resource,wfb2dd_lb_TRANSPORT_CD%>" SortExpression="TRANSPORT_CD" HeaderStyle-Width="130px" ItemStyle-HorizontalAlign="left"/>
                    <asp:BoundField DataField="KILOMETER_AMOUNT" HeaderText="<%$Resources:Resource,wfb2dd_lb_KILOMETER_AMOUNT%>" SortExpression="KILOMETER_AMOUNT" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="left"/>
                    <asp:BoundField DataField="FARE_PRICE" HeaderText="<%$Resources:Resource,wfb2dd_lb_FARE_PRICE%>" SortExpression="FARE_PRICE" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="right"/>
                    <asp:BoundField DataField="SINGLE_TRIP" HeaderText="<%$Resources:Resource,wfb2dd_lb_SINGLE_TRIP%>" SortExpression="SINGLE_TRIP" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="left"/>
                    <asp:BoundField DataField="LINE_CD" HeaderText="<%$Resources:Resource,wfb2dd_lb_LINE_CD%>" SortExpression="LINE_CD" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="left"/>
                    <asp:BoundField DataField="BELONG_TO_DT" HeaderText="<%$Resources:Resource,wfb2dd_lb_BELONG_TO_DT%>" SortExpression="BELONG_TO_DT" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="left"/>
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <td style="font-size: 14px;">
                <asp:Label ID="lb_TotalCount2" runat="server" Text="" Visible="false" Font-Size="10"></asp:Label>
            </td>
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
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
                     
        
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>