<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2de/WFB2DE0700_Qry.aspx.cs" Inherits="WebContent_fb2de_WFB2DE0700_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm' });
            $(".ymd").datepicker({ dateFormat: 'yy/mm/dd' });
            $("#txt_WORK_DT").mask("9999/99");
            $("#txt_MANAGER_DT_S").mask("9999/99/99");
            $("#txt_MANAGER_DT_E").mask("9999/99/99");
            gridviewScroll();
            $.unblockUI(); 
            $('#txt_EMP_NAME').attr("readonly", true);
            $('#txt_DEPT_NAME').attr("readonly", true);

            //查詢
            //$('#txt_EMP_NAME').attr("readonly", true);
            
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
                                $('#txt_DEPT_NAME').val("");                               
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_EMP_NAME').val($.trim(JData.EMP_NAME));
                                $('#txt_DEPT_NAME').val(JData.DEPT_NAME);                                
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_EMP_NAME').val("");
                    $('#txt_DEPT_NAME').val("");
                }
            });                     
            
        }

        //判斷是否為數字
        function IsIntText() {
            var charkeycode = window.event.keyCode;
            if (charkeycode > 47 && charkeycode < 58) {  //0的keycode為47 ,9的keycode為58
                return true;
            }
            return false;

        }
        function addzero(input) {
            if (input.value.length == 1) input.value = '0' + input.value;
            return input;
        }

        function gridviewScroll() {

            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "400",
                barcolor: "#7F7F7F"                
            });

        }

        //清空畫面
        function ClearAll() {
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#txt_DEPT_NAME").val("");
            $("#txt_WORK_DT").val("");
            $("#txt_MANAGER_DT_S").val("");
            $("#txt_MANAGER_DT_E").val("");
            $('#<%=rb_dt1.ClientID %>').prop("checked", false);
            $('#<%=rb_dt2.ClientID %>').prop("checked", false);
        }

        function CheckSearch() {

            if (Page_ClientValidate("GroupA")) {
                if ($('#<%=rb_dt1.ClientID %>').prop("checked") == true  ) {                    
                    if ($("#txt_WORK_DT").val() =='') {
                        alert("勤務期間的年月不可空白!");
                        return false;
                    }                    
                }
                if ($('#<%=rb_dt2.ClientID %>').prop("checked") == true) {                    
                    if ($("#txt_MANAGER_DT_S").val() == '' || $("#txt_MANAGER_DT_E").val() == '') {
                        alert("勤務期間的起迄日期不可空白!");
                        return false;
                    }
                    //只能輸入間隔60天
                    var a1 = new Date($('#txt_MANAGER_DT_S').val());
                    var a2 = new Date($('#txt_MANAGER_DT_E').val());
                    var gap = a2.getTime() - a1.getTime();
                    if (Math.floor(gap / (1000 * 60 * 60 * 24)) > 60) {
                        alert("勤務起迄日不可超過六十天!!");
                        return false;
                    }
                }

                BlockUI();
            }
            else
                return false;
        }

        //儲存前檢查
        function saveCheck() {
            var processed = true;
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
        
        function CheckEMP_ID(source, arguments) {
            var re = /^[\d|a-zA-Z]+$/;
            if (!re.test($("#txt_NEW_EMP_ID").val()))
                //arguments.IsValid = false;
                return false;
            else
                arguments.IsValid = true;
        }

        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }

        function OpenEmpSearchDD010(emp_id) {
            var supervisor = $('#HID_IS_SUPERVISOR').val("");
            var returnValue;
            var myiFrameId = "iframe";
            var Url = '../comm/Dept_Search.aspx?mode=all&super=' + supervisor + '&parentFuncId=' + parentFuncID;
            var dialogID = 'div_iframeID';
            var $dialog = $('<div id = "' + dialogID + '"></div>')
                        .html('<iframe style="border: 0px; " src="' + Url + '" id="' + myiFrameId + '" width="100%" height="100%"></iframe>')
                        .dialog({
                            autoOpen: false,
                            modal: true,
                            draggable: true,
                            resizable: false,
                            height: 600,
                            width: 1100,
                            close: function (ev, ui) {
                                $("#" + dialogID).dialog("destroy");
                            }
                        });
            $('#' + dialogID).attr('flag', 'Y');
            $('#' + dialogID).attr('stid', emp_id);
            $('#' + dialogID).attr('stname', '');

            $dialog.dialog('open');            
        }
        function returnEMPValueToPage(eid, ename, value){
            var returnValue = value;
            if (value == undefined) {
                returnValue = 'undefined';
            }
            //alert(returnValue);
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                window.parent.$('#' + eid).val(obj.EMP_ID.trim());
                doEmpAjax();
                return obj;               

            }
        }

        function doEmpAjax() {            
            if ($("#txt_EMP_ID").val().length == 5) {
                $.ajax({
                    url: "WFB2DE0700_GetEmpData.ashx",
                    data: {
                        EMP_ID: $('#txt_EMP_ID').val()
                    },
                    type: "GET",
                    dataType: 'json',
                    success: function (JData) {
                        if (JData.errMsg != "") {
                            $('#txt_EMP_ID').val("");
                            $('#txt_DEPT_NAME').val("");
                            $('#txt_EMP_NAME').val("");

                            alert(JData.errMsg);
                        }
                        else {
                            $('#txt_EMP_ID').val(JData.EMP_ID);
                            $('#txt_DEPT_NAME').val(JData.DEPT_NAME);
                            $('#txt_EMP_NAME').val(JData.EMP_NAME);
                        }
                    },

                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
            } else {
                $('#txt_EMP_ID').val("");
                $('#txt_DEPT_NAME').val("");
                $('#txt_EMP_NAME').val("");

            }
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="10%" />
                    <col width="20%" />
                    <col width="10%" />
                    <col width="20%" />
                    <col width="10%" />
                    <col width="30%" />
                </colgroup>
                <tbody>
                    <!-- START: 1st Line in Search Criteria area -->
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_EMP_ID%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" >
                            <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="42px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                            <input id="bt_EMP_ID" type="button" value="..." onclick="OpenEmpSearchDD010('txt_EMP_ID');" />  
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2df_EMP_ID%>"
                                    ControlToValidate="txt_EMP_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>                          
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_EMP_NAME%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0"  Width="81px" ClientIDMode="Static" ></asp:TextBox>
                        </td>   
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_DEPT_NAME" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_DEPT_NO%>"></asp:Label>:
                         </th>
                         <td align="left" class="Body_label">                            
                            <asp:TextBox ID="txt_DEPT_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                        </td>                    
                    </tr> 
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_work_dt" runat="server" Text="<%$Resources:Resource,wfb2de_lb_work_dt%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" colspan="3">
                            <asp:RadioButton ID="rb_dt1" runat="server" ClientIDMode="Static" OnCheckedChanged="rb_dt1CheckedChanged" AutoPostBack ="true"/>年月：
                            <asp:TextBox ID="txt_WORK_DT" CssClass="date" runat="server" MaxLength="7" Width="81px" ClientIDMode="Static"></asp:TextBox>	
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                 ErrorMessage="<%$Resources:Resource,wfb2de_error_txt_WORK_DT%>" ControlToValidate="txt_WORK_DT" ForeColor="Red" ValidationGroup="GroupA"
                                 ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                            <asp:RadioButton ID="rb_dt2" runat="server" ClientIDMode="Static" OnCheckedChanged="rb_dt2CheckedChanged" AutoPostBack ="true"/>起迄日期：
                            <asp:TextBox ID="txt_MANAGER_DT_S" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="ymd"></asp:TextBox>
                             ~  
                            <asp:TextBox ID="txt_MANAGER_DT_E" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="ymd"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                 ErrorMessage="<%$Resources:Resource,wfb2de_ERR_MANAGER_DT_S%>" ControlToValidate="txt_MANAGER_DT_S" ForeColor="Red"
                                 ValidationExpression="^(?:(?:([0-9]{4}(-|/)(?:(?:0?[1,3-9]|1[0-2])(-|/)(?:29|30)|((?:0?[13578]|1[02])(-|/)31)))|([0-9]{4}(-|/)(?:0?[1-9]|1[0-2])(-|/)(?:0?[1-9]|1\d|2[0-8]))|(((?:(\d\d(?:0[48]|[2468][048]|[13579][26]))|(?:0[48]00|[2468][048]00|[13579][26]00))(-|/)0?2(-|/)29))))$" Display="None" ValidationGroup="GroupA">
                            </asp:RegularExpressionValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                 ErrorMessage="<%$Resources:Resource,wfb2de_ERR_MANAGER_DT_E%>" ControlToValidate="txt_MANAGER_DT_E" ForeColor="Red"
                                 ValidationExpression="^(?:(?:([0-9]{4}(-|/)(?:(?:0?[1,3-9]|1[0-2])(-|/)(?:29|30)|((?:0?[13578]|1[02])(-|/)31)))|([0-9]{4}(-|/)(?:0?[1-9]|1[0-2])(-|/)(?:0?[1-9]|1\d|2[0-8]))|(((?:(\d\d(?:0[48]|[2468][048]|[13579][26]))|(?:0[48]00|[2468][048]00|[13579][26]00))(-|/)0?2(-|/)29))))$" Display="None" ValidationGroup="GroupA">
                            </asp:RegularExpressionValidator>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_MANAGER_DT_S"
                                 ControlToValidate="txt_MANAGER_DT_E" ErrorMessage="<%$Resources:Resource,wfb2de_ERR_MANAGER_DT%>" Type="Date" Operator="GreaterThanEqual"
                                 Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                                      
                        </td>
                                          
                    </tr>                   
                    <tr>
                        <td align="right" class="Body_label" colspan="10">
                            <div id="init">
                                <aces:Btn ID="WFB2DE0700Search" runat="server" Text="<%$Resources:Resource,wfb2de_WFB2DE0700Search%>" OnClick="WFB2DE0700Search_Click" OnClientClick="return CheckSearch();"/>

                                <%--
                                    <asp:Button ID="WFB2DE0700Search" runat="server" Text="<%$Resources:Resource,wfb2de_WFB2DE0700Search%>" OnClick="WFB2DE0700Search_Click" OnClientClick="return CheckSearch();"/>
                                    <aces:Btn ID="WFB2DE0700Search" runat="server" Text="<%$Resources:Resource,wfb2de_WFB2DE0700Search%>" OnClick="WFB2DE0700Search_Click" OnClientClick="return CheckSearch();"/>
                                    --%>
                                
                                <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2dd_btn_clear%>" onclick="ClearAll();"/>                               
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" height="1" colspan="10">
                            <hr>
                        </td>
                    </tr>                    
                    <!-- END: Create a line -->
                </tbody>
            </table>
            <table cellspacing="1" cellpadding="1" width="100" border="0" class="Body_Label">
                <tr>
                    <th align="left" height="1">
                        <asp:Label ID="lb_totalMoney_Menu" runat="server" Text="<%$Resources:Resource,wfb2de_lb_totalMoney_Menu%>" Visible="false"></asp:Label>
                    </th>
                    <td align="left" height="1">
                        <asp:Label ID="lb_totalMoney" runat="server" Text="" ClientIDMode="Static" Visible="false"></asp:Label>
                    </td>                    
                </tr>
                <tr>                    
                    <td align="left" height="1">
                        <asp:Label ID="lb_every_money" runat="server" MaxLength="100" Width="200px" ClientIDMode="Static" Text="" Visible="false"></asp:Label>
                    </td>                    
                </tr>
            </table>
              <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DE0700DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="EMP_ID" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" /> 
                    <asp:ControlParameter ControlID="rb_dt1" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="rb_dt1" PropertyName="Checked"  Type="Boolean" />  
                    <asp:ControlParameter ControlID="rb_dt2" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="rb_dt2" PropertyName="Checked" Type="Boolean" />  
                    <asp:ControlParameter ControlID="txt_WORK_DT"
                        Name="WORK_DT" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />     
                    <asp:ControlParameter ControlID="txt_MANAGER_DT_S"
                        Name="MANAGER_DT_S" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />  
                    <asp:ControlParameter ControlID="txt_MANAGER_DT_E"
                        Name="MANAGER_DT_E" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />           
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1018px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>                                      
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2dd_RowNumber%>" HeaderStyle-Width="40px"/>
                    <asp:BoundField DataField="MANAGER_DT" HeaderText="<%$Resources:Resource,wfb2de_MANAGER_DT_SE%>" SortExpression="MANAGER_DT" HeaderStyle-Width="220px" ItemStyle-HorizontalAlign="left"/>
                    <asp:BoundField DataField="CLOCK_DESC" HeaderText="<%$Resources:Resource,wfb2de_CLOCK_DESC%>" SortExpression="CLOCK_DESC" HeaderStyle-Width="520px" ItemStyle-HorizontalAlign="left"/>
                    <asp:BoundField DataField="PRICE" HeaderText="<%$Resources:Resource,wfb2de_lb_MONEY%>" SortExpression="PRICE" HeaderStyle-Width="220px" ItemStyle-HorizontalAlign="Right"/>                    
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