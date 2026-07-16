<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2ib/WFB2IB0300_Qry.aspx.cs" Inherits="WebContent_fb2ib_WFB2IB0300_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>    
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            
            iniForm();
        });
        function iniForm() {

            $.unblockUI();
            $('.date').datepicker({ dateFormat: 'yy/mm' });
            $('.ym').mask('9999/99');
            gridviewScroll();

        }




        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "400",
                barcolor: "#7F7F7F"
                //, freezesize: 5
            });
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
/*
            //檢查是否做過月度結算
            $.ajax({
                url: "WFB2IB0300_CheckData.ashx",
                data: {
                    txt_YM: $('#txt_YM').val()

                },
                type: "GET",
                dataType: 'text',
                async: false,
                success: function (result) {
                    if (result == "Y")
                        if (!confirm("該期間已經有資料，執行後將刪除相同年月資料，是否要繼續執行？")) {
                            processed = false;
                        } 
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });
*/
            if (processed) {
                if (Page_ClientValidate("GroupA")) {
                    BlockUI();
                } else
                    processed = false;
            }


            if (!processed) {
                $.unblockUI();
                return;
            }
            return processed;

        }
        
        function uploadCheck() {
            //檢查是否有選擇檔案            
            //if ($('#fileUpload').val() == '') {
            //    alert("請選擇要上傳的檔案");
            //    return false;
            if (!Page_ClientValidate("GroupB")) {
                return false;
            } else {
                if (confirm('您確定要以上傳的Excel檔，取代相同年月的資料嗎?')) {
                    return true;
                }
                else {
                    return false;
                }
            }

        }

        function checkDowning(msg) {
            var processed = true;

            processed = confirm("確定要進行" + msg + "?");
            //if (processed) {
            //    BlockUI();
            //}
            return processed;
        }

        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#ddlPerPageRow").val());
        }
        //清空畫面
        function ClearAll() {
            $('#txt_YM_search').val("");
            $('#txt_EMP_ID_search').val("");
            $("#txt_LICENSE_ID").val("");
            $("#txt_TAX_FORMAT").val("");          

        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">    
    <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
        <tr>
            <td colspan="5">
                <fieldset style="padding: 5px">
                    <legend class="Body_label">
                        <asp:Label ID="lb_DATA_UPLOAD" runat="server" Text="<%$Resources:Resource,wfb2ib_lb_DATA_UPLOAD%>"></asp:Label>
                    </legend>
                    <table align="left">
                        <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_UploadExcel" runat="server" Text="<%$Resources:Resource,wfb2ib_lb_UploadExcel%>"></asp:Label>：
                            </th>
                            <td align="left" class="Body_label">
                                <asp:FileUpload ID="fileUpload" runat="server" class="MandatoryField" Width="600px" ClientIDMode="Static" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="fileUpload" ForeColor="Red" ValidationGroup="GroupB"
                                    ErrorMessage="<%$Resources:Resource,wfb2_format_excel%>" ValidationExpression="^([a-zA-Z].*|[1-9].*)\.(xls|XLS|xlsx|XLSX)$" Display="None"></asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="fileUpload" ForeColor="Red" ValidationGroup="GroupB"
                                    ErrorMessage="<%$Resources:Resource,wfb2_required_excel%>" Display="None"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                    <table align="right">
                        <tr>
                            <td>
                                <asp:Button ID="WFB2IB0300Upload" runat="server" Text="<%$Resources:Resource,wfb2ib_WFB2IB0300Upload%>" OnClick="WFB2IB0300Upload_Click" OnClientClick="return uploadCheck();" />
                                <aces:Btn ID="WFB2IB0300ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2ib_WFB2IB0300ExcelDown%>" OnClick="WFB2IB0300ExcelDown_Click" OnClientClick="return checkDowning(this.value);" />

                                <%--<aces:Btn ID="WFB2IB0300Upload" runat="server" Text="<%$Resources:Resource,wfb2ib_WFB2IB0300Upload%>" OnClick="WFB2IB0300Upload_Click" OnClientClick="return uploadCheck();" /> 
                                    <asp:Button ID="WFB2IB0300Upload" runat="server" Text="<%$Resources:Resource,wfb2ib_WFB2IB0300Upload%>" OnClick="WFB2IB0300Upload_Click" OnClientClick="return uploadCheck();" />
                                            <asp:Button ID="WFB2IB0300ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2ib_WFB2IB0300ExcelDown%>" OnClick="WFB2IB0300ExcelDown_Click" OnClientClick="return checkDowning(this.value);"/>   				--%>
                            </td>
                        </tr>
                    </table>

                </fieldset>
            </td>
        </tr>
        <tr style="height:50px"></tr>
    </table>
    <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="15%" />
                                <col width="35%" />
                                <col width="15%" />
                                <col width="35%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <%--查詢年月--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_YM_search" runat="server" Text="<%$Resources:Resource,wfb2ib_lb_YM_search%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_YM_search" runat="server" MaxLength="6" CssClass="ym date MandatoryField" ClientIDMode="Static" Width="100px"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ib_YM_search_Format_Error%>"
                                            ControlToValidate="txt_YM_search" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                    </td>

                                    <%--工號/廠商代號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID_search" runat="server" Text="<%$Resources:Resource,wfb2ib_lb_EMP_ID_search%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID_search" runat="server" MaxLength="6" CssClass="ym" ClientIDMode="Static" Width="100px"></asp:TextBox>                                       
                                    </td>
                                </tr>                                
                                <tr>
                                    <%--統一編號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LICENSE_ID" runat="server" Text="<%$Resources:Resource,wfb2ib_lb_LICENSE_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_LICENSE_ID" runat="server" ClientIDMode="Static" MaxLength="20"></asp:TextBox>
                                    </td>
                                    <%--格式代號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_TAX_FORMAT" runat="server" Text="<%$Resources:Resource,wfb2ib_lb_TAX_FORMAT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_TAX_FORMAT" runat="server" ClientIDMode="Static" MaxLength="3"></asp:TextBox>
                                    </td>
                                </tr>                                
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <asp:Button ID="WFB2IB0300Search" runat="server" Text="<%$Resources:Resource,wfb2ib_WFB2IB0300Search%>" OnClick="WFB2IB0300Search_Click" ValidationGroup="GroupA" />
<%--                                            <asp:Button ID="WFB2IB0300Search" runat="server" Text="<%$Resources:Resource,wfb2ib_WFB2IB0300Search%>" OnClick="WFB2IB0300Search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
                                                <aces:Btn ID="WFB2IB0300Search" runat="server" Text="<%$Resources:Resource,wfb2ib_WFB2IB0300Search%>" OnClick="WFB2IB0300Search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
    --%>
                                            <asp:Button ID="btn_clear" runat="server" type="button" Text="<%$Resources:Resource,wfb2ib_btn_clear%>" OnClientClick="ClearAll();" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>                
                
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2IB0300DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_YM_search" DefaultValue=""
                        Name="ym" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_EMP_ID_search" DefaultValue=""
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_LICENSE_ID" DefaultValue=""
                        Name="license_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_TAX_FORMAT" DefaultValue=""
                        Name="tax_format" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />                   
                </SelectParameters>
            </asp:ObjectDataSource>

            <%--meta:resourcekey="gv_resultResource1"--%>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1200px"
                OnPageIndexChanging="gv_result_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2dd_RowNumber%>" HeaderStyle-Width="40px"/>
                    <asp:BoundField DataField="YM" HeaderText="<%$Resources:Resource,wfb2ib_YM%>" SortExpression="YM" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="left"/>
                    <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2ib_lb_EMP2_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="left"/>
                    <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2ib_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="left"/>
                    <asp:BoundField DataField="LICENSE_ID" HeaderText="<%$Resources:Resource,wfb2ib_lb_LICENSE_ID%>" SortExpression="LICENSE_ID" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="left"/>
                    <asp:BoundField DataField="PAYMENT_DATE" HeaderText="<%$Resources:Resource,wfb2ib_lb_PAYMENT_DATE%>" SortExpression="PAYMENT_DATE" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="left"/>
                    <asp:BoundField DataField="BILL_NO" HeaderText="<%$Resources:Resource,wfb2ib_lb_BILL_NO%>" SortExpression="BILL_NO" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="left"/>
                    <asp:BoundField DataField="ITEM_SEQ" HeaderText="<%$Resources:Resource,wfb2ib_lb_ITEM_SEQ%>" SortExpression="ITEM_SEQ" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="left"/>
                    <asp:BoundField DataField="VCHID" HeaderText="<%$Resources:Resource,wfb2ib_lb_VCHID%>" SortExpression="VCHID" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="left"/>
                    <asp:BoundField DataField="VCH_NAME" HeaderText="<%$Resources:Resource,wfb2ib_lb_VCH_NAME%>" SortExpression="VCH_NAME" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="left"/>
                    <asp:BoundField DataField="TAX_FORMAT" HeaderText="<%$Resources:Resource,wfb2ib_lb_TAX_FORMAT%>" SortExpression="TAX_FORMAT" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="left"/>
                    <asp:BoundField DataField="CODE_CD" HeaderText="<%$Resources:Resource,wfb2ib_lb_CODE_CD%>" SortExpression="CODE_CD" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="left"/>
                    <asp:BoundField DataField="AMOUNT" HeaderText="<%$Resources:Resource,wfb2ib_lb_AMOUNT%>" SortExpression="AMOUNT" HeaderStyle-Width="140px"  DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="right"/>
                    <asp:BoundField DataField="INS_COST" HeaderText="<%$Resources:Resource,wfb2ib_lb_INS_COST%>" SortExpression="INS_COST" HeaderStyle-Width="130px" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="right"/>
                     
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
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
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
           
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>