<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sm/WFB2SM1400_Qry.aspx.cs" Inherits="WebContent_fb2sm_WFB2SM1400_Qry"  Culture="auto" UICulture="auto"%>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask("9999/99/99");
            $("#txt_DATA_SEQ").mask("99");
            gridviewScroll();
            $.unblockUI();
        }
        function gridviewScroll() {

            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "400",
                barcolor: "#7F7F7F"
                //freezesize: 3
            });

        }
        function saveCheck() {
            var processed = true;
            if (Page_ClientValidate("GroupA")) {
                //檢查相關規則
                $.ajax({
                    url: "WFB2SM1400_Check.ashx",
                    data: {
                        CHECK_ITEM: "1",
                        DATA_YEAR: $('#txt_DATA_YEAR').val(),
                        EMP_ID: $('#txt_EMP_ID').val(),
                        DATA_SEQ: $('#txt_DATA_SEQ').val()
                    },
                    type: "GET",
                    dataType: 'text',
                    async: false,
                    success: function (result) {
                        if (result != "") {
                            alert(result);
                            processed = false;
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
                if (processed) {
                    $.ajax({
                        url: "WFB2SM1400_Check.ashx",
                        data: {
                            CHECK_ITEM: "2",
                            DATA_YEAR: $('#txt_DATA_YEAR').val(),
                            EMP_ID: $('#txt_EMP_ID').val()                            
                        },
                        type: "GET",
                        dataType: 'text',
                        async: false,
                        success: function (result) {
                            if (result != "") {
                                alert(result);
                                processed = false;
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                }
                if (processed) {
                    $.ajax({
                        url: "WFB2SM1400_Check.ashx",
                        data: {
                            CHECK_ITEM: "3",
                            DATA_YEAR: $('#txt_DATA_YEAR').val(),
                            EMP_ID: $('#txt_EMP_ID').val()                           
                        },
                        type: "GET",
                        dataType: 'text',
                        async: false,
                        success: function (result) {
                            processed = confirm(result);
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                }
            }
            else
                processed = false;
            if (!processed)
                $.unblockUI();
            else
                BlockUI();

            return processed;
        }

        //清空畫面
        function ClearAll() {

            $("#txt_DATA_YM").val("");
            $("#txt_SEND_DT").val("");
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#txt_MAIL_TITLE").val("");
            $("#txt_MAIL_DESC").val("");
            $("#txt_DATA_YEAR").val("");
            $("#txt_DATA_SEQ").val("");
            $("#txt_CC_EMAIL").val("");
        }
        //發送EMAIL日期 不可小於等於系統日!
        function CheckYMD(source, arguments) {
            var now = new Date();
            var Current = now.getFullYear() + "/" + (now.getMonth() + 1) + "/" + now.getDate();
            var email_date = $("#txt_SEND_DT").val();
            if ((Date.parse(email_date)).valueOf() <= (Date.parse(Current)).valueOf()) {
                arguments.IsValid = false;
            }
            else {
                arguments.IsValid = true;
            }
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
            <table cellspacing="1" cellpadding="1" width="1020px" border="0" class="Body_Label">
                <colgroup>
                    <col width="15%" />
                    <col width="40%" />
                    <col width="15%" />
                    <col width="30%" />

                </colgroup>
                <tbody>
                    <!-- START: 1st Line in Search Criteria area -->
                    <tr>                        
                        <!-- 寄送日期 -->
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_SEND_DT" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_SEND_DT%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_SEND_DT" runat="server" ClientIDMode="Static" MaxLength="10" Width="84px" CssClass="MandatoryField date"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2sm_ERR_SEND_DT%>" ControlToValidate="txt_SEND_DT" ForeColor="Red"
                                ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])[/ /.](0[1-9]|[12][0-9]|3[01])" Display="None" ValidationGroup="GroupA">
                            </asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sm_Required_SEND_DT%>"
                                ControlToValidate="txt_SEND_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfd2sc_GreaterThan_EMAIL_DT%>" ClientValidationFunction="CheckYMD" ForeColor="Red"
                                            ControlToValidate="txt_SEND_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </td>
                        <th></th>
                        <th></th>
                        <th></th>
                        <th></th>
                    </tr>
                    <tr>
                        <!-- 晉升年度 -->
                         <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_DATA_YEAR" runat="server" Text="<%$Resources:Resource,wfb2sm_DATA_YEAR%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_DATA_YEAR" runat="server" MaxLength="4" Width="32px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sm_Required_M_DATA_YEAR%>"
                                ControlToValidate="txt_DATA_YEAR" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sm_Required_M_DATA_YEAR%>"
                                ControlToValidate="txt_DATA_YEAR" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_DATA_SEQ" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_DATA_SEQ%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_DATA_SEQ" runat="server" ClientIDMode="Static" MaxLength="2" Width="84px" CssClass="MandatoryField"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sm_Required_DATA_SEQ%>"
                                ControlToValidate="txt_DATA_SEQ" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </td>
                        <th></th>
                        <th></th>
                    </tr>
                    <tr>
                        <!-- 工號 -->
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_EMP_ID%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="10" Width="64px" ClientIDMode="Static"></asp:TextBox>
                            <input id="bt_EMP_ID" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N');" />
                            <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="10" BorderWidth="0" ClientIDMode="Static" Width="80px"></asp:TextBox>
                           
                        </td>
                         <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_CC_EMAIL" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_CC_EMAIL%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_CC_EMAIL" runat="server" MaxLength="10" Width="64px" ClientIDMode="Static"></asp:TextBox>
                            <input id="bt_CC_EMAIL" type="button" value="..." onclick="OpenEmpSearch('txt_CC_EMAIL', 'txt_CC_EMAIL_NAME', 'N');" />
                            <asp:TextBox ID="txt_CC_EMAIL_NAME" runat="server" MaxLength="10" BorderWidth="0" ClientIDMode="Static" Width="80px"></asp:TextBox>
                        </td>
                        <th></th>
                        <th></th>
                    </tr>
                    <tr>
                        <!-- 主旨 -->
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_MAIL_TITLE" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_MAIL_TITLE%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_MAIL_TITLE" runat="server" ClientIDMode="Static" MaxLength="150" Width="200px" CssClass="MandatoryField"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sm_Required_MAIL_TITLE%>"
                                ControlToValidate="txt_MAIL_TITLE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </td>

                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_MAIL_DESC" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_MAIL_DESC%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_MAIL_DESC" runat="server" ClientIDMode="Static" MaxLength="300" Width="200px" CssClass="MandatoryField"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sm_Required_MAIL_DESC%>"
                                ControlToValidate="txt_MAIL_DESC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </td>
                        <th></th>
                        <th></th>
                    </tr>
                    <tr>
                        <!-- START: Create MODULE ID -->
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_CONTACT_TO" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_CONTACT_TO%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_CONTACT_TO" runat="server" ClientIDMode="Static" MaxLength="100" Width="400px" CssClass="MandatoryField"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sm_Required_CONTACT_TO%>"
                                ControlToValidate="txt_CONTACT_TO" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </td>

                        <th></th>
                        <th></th>
                        <th></th>
                        <th></th>
                    </tr>
                    <tr>
                        <td align="right" class="Body_label" colspan="10">
                            <div id="init">
                                <%--                                     
                                    <aces:Btn ID="WFB2SM140Execute" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SM140Execute%>" OnClientClick="return saveCheck();" ValidationGroup="GroupA" OnClick="WFB2SM140Execute_Click" />
                                    <aces:Btn ID="WFB2SM140Search" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SM140Search%>" ValidationGroup="GroupB" OnClick="WFB2SM140Search_Click" />
                                    <asp:Button ID="WFB2SM140Execute" runat="server" Text="<%$Resources:Resource,wfb2sm_WFB2SM140Execute%>" OnClientClick="return saveCheck();" ValidationGroup="GroupA" OnClick="WFB2SM140Execute_Click" />
                                    <asp:Button ID="WFB2SM140Search" runat="server" Text="<%$Resources:Resource,wfb2sm_WFB2SM140Search%>" ValidationGroup="GroupB" OnClick="WFB2SM140Search_Click" />
                                    
                                    --%>
                                    <aces:Btn ID="WFB2SM140Execute" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SM140Execute%>" OnClientClick="return saveCheck();" ValidationGroup="GroupA" OnClick="WFB2SM140Execute_Click" />
                                    <aces:Btn ID="WFB2SM140Search" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SM140Search%>" ValidationGroup="GroupB" OnClick="WFB2SM140Search_Click" />
                                
                                
                                <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2sa_btn_clear%>" onclick="ClearAll();" />
                            </div>
                        </td>
                    </tr>


                    <!-- end: Create MODULE ID -->
                    <!-- START: Create a line to separate Search field with body field -->
                    <tr>
                        <td align="center" height="1" colspan="10">
                            <hr>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="10">
                            <%-- 
                                  <aces:Btn ID="WFB2SM1400Print" runat="server" Text="<%$Resources:Resource,wfb2sm_WFB2SM1400Print%>" Visible="false" OnClick="WFB2SM1400Print_Click" style="height: 21px" />
                                <asp:Button ID="WFB2SM1400Print" runat="server" Text="<%$Resources:Resource,wfb2sm_WFB2SM1400Print%>" Visible="false" OnClick="WFB2SM1400Print_Click" style="height: 21px"/>
                                --%>
                            <aces:Btn ID="WFB2SM1400Print" runat="server" Text="<%$Resources:Resource,wfb2sm_WFB2SM1400Print%>" Visible="false" OnClick="WFB2SM1400Print_Click" style="height: 21px" />
                        </td>
                    </tr>
                    
                    <!-- END: Create a line -->
                </tbody>
            </table>
             <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SM1400DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />                   
                    <asp:ControlParameter ControlID="txt_DATA_YEAR"
                        Name="DATA_YEAR" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />  
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="EMP_ID" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_SEND_DT"
                        Name="SEND_DT" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />                                                           
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>                            
                            <asp:Label ID="cb_all" runat="server"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2sa_RowNumber%>" HeaderStyle-Width="60px" />
                    <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2sm_lb_EMP_ID%>" SortExpression="EMP_ID"
                         HeaderStyle-Width="320px" ItemStyle-HorizontalAlign="Left"/>
                    <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2sm_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="320px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="SEND_DT" HeaderText="<%$Resources:Resource,wfb2sm_lb_SEND_DT%>" SortExpression="SEND_DT" HeaderStyle-Width="320px" ItemStyle-HorizontalAlign="Left"/>          
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
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SM1400Print" />          
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
