<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sj/WFB2SJ0260_Add.aspx.cs" Inherits="WebContent_WFB2SJ0260_Add" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<%@ Register Src="~/UserControl/UCDateTextBoxRange.ascx.ascx" TagPrefix="uc1" TagName="UCDateTextBoxRangeascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        #txt_PJOB_CD {
            text-transform: uppercase;
        }
    </style>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');
            $(".year").mask('9999');
            $(".money").mask('9999999');

            //工號取得姓名的ajax
            $("#txt_EMP_ID").change(function () {
                if ($("#txt_EMP_ID").val().length == 5) {
                    getEMPDeptData($("#txt_EMP_ID").val());
                } else {
                    $('#txt_EMP_NAME').val("");
                    $('#txt_DEPT_NO_OLD').val("");
                    $('#txt_DEPT_NAME_OLD').val("");
                    $('#txt_HEAD_EMP_NAME_OLD').val("");
                    $('#hid_HEAD_EMP_ID_OLD').val("");
                }
            });
            //部門編號取得部門的ajax
            $("#txt_DEPT_NO_NEW").change(function () {
                if ($("#txt_DEPT_NO_NEW").val().length == 7) {
                    $.ajax({
                        url: "WFB2SJ0260GetDeptData.ashx",
                        data: {
                            DEPT_NO: $('#txt_DEPT_NO_NEW').val()
                        },
                        type: "GET",
                        cache: false,
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_DEPT_NAME_NEW').val(""); 
                                $('#txt_DEPT_NO_OLD').val("");
                                $('#txt_DEPT_NAME_OLD').val("");
                                $('#txt_HEAD_EMP_NAME_OLD').val("");
                                $('#hid_HEAD_EMP_ID_OLD').val("");
                                alert(JData.errMsg);
                            }
                            else {

                                $('#txt_DEPT_NAME_NEW').val(JData.DEPT_FULL_NAME);
                                $('#txt_HEAD_EMP_NAME_NEW').val(JData.HEAD_EMP_NAME);
                                $('#hid_HEAD_EMP_ID_NEW').val(JData.HEAD_EMP_ID);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_DEPT_NAME_NEW').val("");
                    $("#txt_HEAD_EMP_NAME_NEW").val("");
                    $("#hid_HEAD_EMP_ID_NEW").val("");
                }
            });
            $.unblockUI();

           
        }
        //儲存前檢查
        function saveCheck() {
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
        function doTest() {
            alert("ok2");
        }
        function OpenAssessDeptSearch(dept_no, dept_name, supervisor, flag, calledFunctionObject) {
            var myiFrameId = "iframe";
            var Url = "../comm/Assess_Dept_Search.aspx?mode=dept&super=" + supervisor + "&parentFuncId=" + parentFuncID;
            var dialogID = 'div_iframeID';
            var $dialog = $('<div id = "' + dialogID + '"></div>')
                        .html('<iframe style="border: 0px; " src="' + Url + '" id="' + myiFrameId + '" width="100%" height="100%"></iframe>')
                        .dialog({
                            autoOpen: false,
                            modal: true,
                            draggable: true,
                            resizable: false,
                            height: 500,
                            width: 350,
                            close: function (ev, ui) {
                                $("#" + dialogID).dialog("destroy");
                            }
                        });
            $('#' + dialogID).attr('flag', flag);
            $('#' + dialogID).attr('stid', dept_no);
            $('#' + dialogID).attr('stname', dept_name);
            $('#' + dialogID).attr('calledFunctionObject', calledFunctionObject);
            $('#' + dialogID).attr('popID', 'AssessDeptSearch');

            $dialog.dialog('open');
        }
        function popAssessReturn(value) {
            var returnValue = value;
            if (value == undefined) {
                returnValue = 'undefined';
            }
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                var flag = window.$('#div_iframeID').attr("flag");
                var eid = window.$('#div_iframeID').attr("stid");
                var ename = window.$('#div_iframeID').attr("stname");
                var calledFunctionObject = window.$('#div_iframeID').attr("calledFunctionObject");
                if (eid != "") {
                    if( $("#" + eid)!=undefined)$("#" + eid).val(obj.DEPT_NO);
                }
                if (ename != "") {
                    if ($("#" + ename) != undefined) $("#" + ename).val(obj.DEPT_NAME);
                }
               
                // 結束後呼叫額外函式
                if (doADDeptReturn != undefined) {
                    if (typeof (doADDeptReturn) == "function") {
                        doADDeptReturn(obj);
                    }
                }

                return obj;

            }

        }
        function doADDeptReturn(jsonObj) {
           
            $("#txt_HEAD_EMP_NAME_NEW").val(jsonObj.HEAD_EMP_NAME);
            $("#hid_HEAD_EMP_ID_NEW").val(jsonObj.HEAD_EMP_ID);
        }
        function popAssessEmpReturn(json) {
            var returnValue = json;
            if (json == undefined) {
                returnValue = 'undefined';
            }
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                getEMPDeptData(obj.EMP_ID);
            }
        }
        function getEMPDeptData(empId) {

            $.ajax({
                url: "WFB2SJ0260GetEmpData.ashx",
                data: {
                    ASSESS_YEAR: $('#hid_ASSESS_YEAR').val(),
                    ASSESS_TYPE: $('#hid_ASSESS_TYPE').val(),
                    EMP_ID: $('#txt_EMP_ID').val()
                },
                type: "GET",
                cache: false,
                dataType: 'json',
                success: function (JData) {
                    if (JData.errMsg != "") {
                        $('#txt_EMP_NAME').val("");
                        alert(JData.errMsg);
                    }
                    else {

                        $('#txt_EMP_NAME').val(JData.EMP_NAME);
                        $('#txt_DEPT_NO_OLD').val(JData.DEPT_NO);
                        $('#txt_DEPT_NAME_OLD').val(JData.DEPT_FULL_NAME);
                        $('#txt_HEAD_EMP_NAME_OLD').val(JData.HEAD_EMP_NAME);
                        $('#hid_HEAD_EMP_ID_OLD').val(JData.HEAD_EMP_ID);
                    }
                },

                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--頁面table--%>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="13%" />
                                <col width="37%" />
                                <col width="13%" />
                                <col width="37%" />
                            </colgroup>
                            <tbody>
                                 <tr>  
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_ASSESS_YEAR" runat="server" Text="考核年度"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_ASSESS_YEAR" runat="server" Width="60px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox>                            
                                          <asp:HiddenField ID="hid_ASSESS_YEAR" runat="server" ClientIDMode="Static" />                                  
                           
                                    </td>                      
                                     <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_ASSESS_TYPE" runat="server" Text="考核類型"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                         <asp:TextBox ID="txt_ASSESS_TYPE" runat="server" Width="60px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox>                            
                                          <asp:HiddenField ID="hid_ASSESS_TYPE" runat="server" ClientIDMode="Static" />                             
                                    </td>
                        
                                </tr>                                
                                 <tr>
                                    <%--工號 --%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="工號"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" >
                                         <asp:TextBox ID="txt_EMP_ID" runat="server"  MaxLength="5" Width="64px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                         <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N');" />  
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" Width="90px" ClientIDMode="Static" BorderWidth="0" ></asp:TextBox>                                      
                                        <asp:RequiredFieldValidator ID="Req_IS_OUT" runat="server" 
                                            ErrorMessage="工號必輸入" InitialValue=""
                                            ControlToValidate="txt_EMP_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator> 
                                       
                                    </td>
                                     <%-- 是否確認 --%>             
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SURE_YN_DESC" runat="server" Text="是否確認"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">  
                                        <asp:TextBox ID="txt_SURE_YN_DESC" runat="server" Width="100px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox> 
                                    </td>
                                </tr>
                                 <tr>
                                    <%--(原)部門代號--%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DEPT_NO_OLD" runat="server" Text="(原)部門代號"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label"  >
                                           <asp:TextBox ID="txt_DEPT_NO_OLD" runat="server" Width="64px" ClientIDMode="Static" BorderWidth="0" ></asp:TextBox> 
                                        <asp:TextBox ID="txt_DEPT_NAME_OLD" runat="server" Width="250px"  ClientIDMode="Static" BorderWidth="0" ></asp:TextBox>
                                    </td>
                                     <%-- (新)部門代號 --%>             
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DEPT_NO_NEW" runat="server" Text="(新)部門代號"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label"  >  
                                        <asp:TextBox ID="txt_DEPT_NO_NEW" runat="server"  MaxLength="7" Width="64px" ClientIDMode="Static" CssClass="MandatoryField" OnClientBlur="doTest();"></asp:TextBox>
                                         <input id="btnDEPTSearch" type="button" value="..." onclick="OpenDeptSearch('txt_DEPT_NO_NEW', 'txt_DEPT_NAME_NEW', 'N');" />  
                                        <asp:TextBox ID="txt_DEPT_NAME_NEW" runat="server" Width="250px"  ClientIDMode="Static" BorderWidth="0" ></asp:TextBox>                                     
                                        <asp:RequiredFieldValidator ID="req_txt_DEPT_NO_NEW" runat="server" 
                                            ErrorMessage="(新)部門代號必輸入" InitialValue=""
                                            ControlToValidate="txt_DEPT_NO_NEW" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator> 
                                    </td>
                                </tr>
                                 <tr>
                                    <%--(原)直屬主管--%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_HEAD_EMP_NAME_OLD" runat="server" Text="(原)直屬主管"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                         <asp:TextBox ID="txt_HEAD_EMP_NAME_OLD" runat="server" Width="80px"  ClientIDMode="Static" BorderWidth="0" ></asp:TextBox>
                                         <asp:HiddenField ID="hid_HEAD_EMP_ID_OLD" runat="server" ClientIDMode="Static" />
                                    </td>
                                     <%-- (新)直屬主管 --%>             
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_HEAD_EMP_NAME_NEW" runat="server" Text="(新)直屬主管"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">  
                                        <asp:TextBox ID="txt_HEAD_EMP_NAME_NEW" runat="server" Width="80px"  ClientIDMode="Static" BorderWidth="0" ></asp:TextBox> 
                                         <asp:HiddenField ID="hid_HEAD_EMP_ID_NEW" runat="server" ClientIDMode="Static" />
                                    </td>
                                </tr>
                                 
                               <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label" >
                                        <div id="init">
                                            <aces:Btn ID="WFB2SJ0260Save_A" runat="server" Text="儲存"  OnClientClick="return saveCheck();" OnClick="WFB2SJ0260Save_Click"  />
                                            <asp:Button runat="server" ID="btn_cancel" Text="取消" OnClick="btn_Cancel_Click" OnClientClick="return confirm('是否確定取消?');"/>
                                            <aces:Btn ID="WFB2SJ0260Confirm" runat="server" Text="確認"  OnClientClick="return saveCheck();" OnClick="WFB2SJ0260Confirm_Click"  />
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
                <tr>
                    <td align="right" class="Body_label">
                         <div id="init_grid">
                        </div>
                    </td>
                </tr>
            </table>
             <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
