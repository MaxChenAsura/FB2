<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2he/WFB2HE0200_Qry.aspx.cs" Inherits="WebContent_fb2he_WFB2HE0200_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask("9999/99/99");
            gridviewScroll();
            $.unblockUI();

            //工號取得姓名的ajax
            $("#txt_INTERVIEW_BY").change(function () {
                if ($("#txt_INTERVIEW_BY").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_INTERVIEW_BY').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_INTERVIEW_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_INTERVIEW_NAME').val($.trim(JData.EMP_NAME));
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_INTERVIEW_NAME').val("");
                }
            });

            $("#txt_ADOPT_BY").change(function () {
                if ($("#txt_ADOPT_BY").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_ADOPT_BY').val()
                        },
                        type: "GET",
                        dataType: 'json',
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

            $("#txt_APPROVE_BY").change(function () {
                if ($("#txt_APPROVE_BY").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_APPROVE_BY').val()
                        },
                        type: "GET",
                        dataType: 'json',
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
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "400",
                barcolor: "#7F7F7F"
                , freezesize: 5
            });
        }

        function CheckSearch() {

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                return false;
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
            //alert($("#gv_result_ddlPerPageRow").val());
        }

        function checkvalue() {
            var processed = true;

            if (!Page_ClientValidate("GroupA")) {
                processed = false;
            }
            else {
                BlockUI();
            }
            if (!processed)
                $.unblockUI();


            return processed;

        }
        function searchCheck() {
            var processed = true;

            if (!Page_ClientValidate("GroupB")) {
                processed = false;
            }
            else {
                BlockUI();
            }
            if (!processed)
                $.unblockUI();


            return processed;
        }


        //EXCEL匯出提示
        function checkDown(msg) {
            var processed = true;
            processed = confirm("確定要進行" + msg + "?");
            if (!processed) {
                $.unblockUI();
            } else {
                BlockUI();
            }
            return processed;
        }


        //清空畫面
        function ClearAll() {

            $("#txt_EMP_NAME").val("");
            $("#txt_PJOB_CD").val("");
            $("#ddl_INTERVIEW_PROCESS_STATUS").val("-1");
            $("#txt_INTERVIEW_DT_S").val("");
            $("#txt_INTERVIEW_DT_E").val("");
            $("#txt_INTERVIEW_BY").val("");
            $("#txt_INTERVIEW_NAME").val("");
            $("#ddl_INTERVIEW_RESULT").val("-1");
            $("#txt_ADOPT_DT_S").val("");
            $("#txt_ADOPT_DT_E").val("");
            $("#txt_ADOPT_BY").val("");
            $("#txt_ADOPT_NAME").val("");
            $("#ddl_ADOPT_RESULT").val("-1");
            $("#txt_APPROVE_DT_S").val("");
            $("#txt_APPROVE_DT_E").val("");
            $("#txt_APPROVE_BY").val("");
            $("#txt_APPROVE_NAME").val("");
            $("#ddl_APPROVE_STATUS").val("-1");

        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                            <colgroup>
                                <col width="12%" />
					            <col width="23%" />									
					            <col width="12%" />
					            <col width="21%" />
					            <col width="12%" />
					            <col width="20%" />
                            </colgroup>
                            <tbody>
                               <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2he_lb_EMP_NAME%>"></asp:Label>：	
                                    </th>
                                    <td align="left">                                            
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="30" Width="81px" ClientIDMode="Static" ></asp:TextBox>                           
                                    </td> 
                                   <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PJOB_CD" runat="server" Text="<%$Resources:Resource,wfb2he_lb_PJOB_CD%>"></asp:Label>：	
                                    </th>
                                    <td align="left">                                            
                                        <asp:TextBox ID="txt_PJOB_CD" runat="server" MaxLength="4" Width="81px" ClientIDMode="Static" ></asp:TextBox>                           
                                    </td> 
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_INTERVIEW_PROCESS_STATUS" runat="server" Text="<%$Resources:Resource,wfb2he_lb_INTERVIEW_PROCESS_STATUS%>"></asp:Label>：	
                                    </th>
                                    <td align="left">                                            
                                         <asp:DropDownList ID="ddl_INTERVIEW_PROCESS_STATUS" runat="server" ClientIDMode="Static" ></asp:DropDownList>                             
                                    </td>                      
                                </tr>
                               <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_INTERVIEW_DT" runat="server" Text="<%$Resources:Resource,wfb2he_lb_INTERVIEW_DT%>"></asp:Label>：	
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txt_INTERVIEW_DT_S" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                                    ~  
                                        <asp:TextBox ID="txt_INTERVIEW_DT_E" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                             ErrorMessage="<%$Resources:Resource,wfb2he_ERR_INTERVIEW_DT_S%>" ControlToValidate="txt_INTERVIEW_DT_S" ForeColor="Red"
                                             ValidationExpression="^(?:(?:([0-9]{4}(-|/)(?:(?:0?[1,3-9]|1[0-2])(-|/)(?:29|30)|((?:0?[13578]|1[02])(-|/)31)))|([0-9]{4}(-|/)(?:0?[1-9]|1[0-2])(-|/)(?:0?[1-9]|1\d|2[0-8]))|(((?:(\d\d(?:0[48]|[2468][048]|[13579][26]))|(?:0[48]00|[2468][048]00|[13579][26]00))(-|/)0?2(-|/)29))))$" Display="None" ValidationGroup="GroupA">
                                        </asp:RegularExpressionValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                             ErrorMessage="<%$Resources:Resource,wfb2he_ERR_INTERVIEW_DT_E%>" ControlToValidate="txt_INTERVIEW_DT_E" ForeColor="Red"
                                             ValidationExpression="^(?:(?:([0-9]{4}(-|/)(?:(?:0?[1,3-9]|1[0-2])(-|/)(?:29|30)|((?:0?[13578]|1[02])(-|/)31)))|([0-9]{4}(-|/)(?:0?[1-9]|1[0-2])(-|/)(?:0?[1-9]|1\d|2[0-8]))|(((?:(\d\d(?:0[48]|[2468][048]|[13579][26]))|(?:0[48]00|[2468][048]00|[13579][26]00))(-|/)0?2(-|/)29))))$" Display="None" ValidationGroup="GroupA">
                                        </asp:RegularExpressionValidator>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_INTERVIEW_DT_S"
                                             ControlToValidate="txt_INTERVIEW_DT_E" ErrorMessage="<%$Resources:Resource,wfb2he_ERR_INTERVIEW_DT%>" Type="Date" Operator="GreaterThanEqual"
                                             Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                    </td> 
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_INTERVIEW_BY" runat="server" Text="<%$Resources:Resource,wfb2he_lb_INTERVIEW_BY%>"></asp:Label>：	
                                    </th>
                                    <td align="left">                                            
                                        <asp:TextBox ID="txt_INTERVIEW_BY" runat="server" MaxLength="20" Width="81px" ClientIDMode="Static" ></asp:TextBox>   
                                        <input id="bt_INTERVIEW_BY" type="button" value="..." onclick="OpenEmpSearch('txt_INTERVIEW_BY', 'txt_INTERVIEW_NAME', 'N');" />   
                                        <asp:TextBox ID="txt_INTERVIEW_NAME" runat="server" ClientIDMode="Static" BorderWidth="0" Width="81px"></asp:TextBox>                       
                                    </td> 
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_INTERVIEW_RESULT" runat="server" Text="<%$Resources:Resource,wfb2he_lb_INTERVIEW_RESULT%>"></asp:Label>：	
                                    </th>
                                    <td align="left">                                            
                                         <asp:DropDownList ID="ddl_INTERVIEW_RESULT" runat="server" ClientIDMode="Static" ></asp:DropDownList>                             
                                    </td>                      
                                </tr> 
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_ADOPT_DT" runat="server" Text="<%$Resources:Resource,wfb2he_lb_ADOPT_DT%>"></asp:Label>：	
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txt_ADOPT_DT_S" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                                    ~  
                                        <asp:TextBox ID="txt_ADOPT_DT_E" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                             ErrorMessage="<%$Resources:Resource,wfb2he_ERR_txt_ADOPT_DT_S%>" ControlToValidate="txt_ADOPT_DT_S" ForeColor="Red"
                                             ValidationExpression="^(?:(?:([0-9]{4}(-|/)(?:(?:0?[1,3-9]|1[0-2])(-|/)(?:29|30)|((?:0?[13578]|1[02])(-|/)31)))|([0-9]{4}(-|/)(?:0?[1-9]|1[0-2])(-|/)(?:0?[1-9]|1\d|2[0-8]))|(((?:(\d\d(?:0[48]|[2468][048]|[13579][26]))|(?:0[48]00|[2468][048]00|[13579][26]00))(-|/)0?2(-|/)29))))$" Display="None" ValidationGroup="GroupA">
                                        </asp:RegularExpressionValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server"
                                             ErrorMessage="<%$Resources:Resource,wfb2he_ERR_txt_ADOPT_DT_E%>" ControlToValidate="txt_ADOPT_DT_E" ForeColor="Red"
                                             ValidationExpression="^(?:(?:([0-9]{4}(-|/)(?:(?:0?[1,3-9]|1[0-2])(-|/)(?:29|30)|((?:0?[13578]|1[02])(-|/)31)))|([0-9]{4}(-|/)(?:0?[1-9]|1[0-2])(-|/)(?:0?[1-9]|1\d|2[0-8]))|(((?:(\d\d(?:0[48]|[2468][048]|[13579][26]))|(?:0[48]00|[2468][048]00|[13579][26]00))(-|/)0?2(-|/)29))))$" Display="None" ValidationGroup="GroupA">
                                        </asp:RegularExpressionValidator>
                                        <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToCompare="txt_ADOPT_DT_S"
                                             ControlToValidate="txt_ADOPT_DT_E" ErrorMessage="<%$Resources:Resource,wfb2he_ERR_txt_ADOPT_DT%>" Type="Date" Operator="GreaterThanEqual"
                                             Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                    </td> 
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_ADOPT_BY" runat="server" Text="<%$Resources:Resource,wfb2he_lb_ADOPT_BY%>"></asp:Label>：	
                                    </th>
                                    <td align="left">                                            
                                        <asp:TextBox ID="txt_ADOPT_BY" runat="server" MaxLength="20" Width="81px" ClientIDMode="Static" ></asp:TextBox>   
                                        <input id="bt_ADOPT_BY" type="button" value="..." onclick="OpenEmpSearch('txt_ADOPT_BY', 'txt_ADOPT_NAME', 'N');" />   
                                        <asp:TextBox ID="txt_ADOPT_NAME" runat="server" ClientIDMode="Static" BorderWidth="0" Width="81px"></asp:TextBox>                       
                                    </td> 
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_ADOPT_RESULT" runat="server" Text="<%$Resources:Resource,wfb2he_lb_ADOPT_RESULT%>"></asp:Label>：	
                                    </th>
                                    <td align="left">                                            
                                         <asp:DropDownList ID="ddl_ADOPT_RESULT" runat="server" ClientIDMode="Static" ></asp:DropDownList>                             
                                    </td>                      
                                </tr> 
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_APPROVE_DT" runat="server" Text="<%$Resources:Resource,wfb2he_lb_APPROVE_DT%>"></asp:Label>：	
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txt_APPROVE_DT_S" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                                    ~  
                                        <asp:TextBox ID="txt_APPROVE_DT_E" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server"
                                             ErrorMessage="<%$Resources:Resource,wfb2he_ERR_txt_APPROVE_DT_S%>" ControlToValidate="txt_APPROVE_DT_S" ForeColor="Red"
                                             ValidationExpression="^(?:(?:([0-9]{4}(-|/)(?:(?:0?[1,3-9]|1[0-2])(-|/)(?:29|30)|((?:0?[13578]|1[02])(-|/)31)))|([0-9]{4}(-|/)(?:0?[1-9]|1[0-2])(-|/)(?:0?[1-9]|1\d|2[0-8]))|(((?:(\d\d(?:0[48]|[2468][048]|[13579][26]))|(?:0[48]00|[2468][048]00|[13579][26]00))(-|/)0?2(-|/)29))))$" Display="None" ValidationGroup="GroupA">
                                        </asp:RegularExpressionValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server"
                                             ErrorMessage="<%$Resources:Resource,wfb2he_ERR_txt_APPROVE_DT_E%>" ControlToValidate="txt_APPROVE_DT_E" ForeColor="Red"
                                             ValidationExpression="^(?:(?:([0-9]{4}(-|/)(?:(?:0?[1,3-9]|1[0-2])(-|/)(?:29|30)|((?:0?[13578]|1[02])(-|/)31)))|([0-9]{4}(-|/)(?:0?[1-9]|1[0-2])(-|/)(?:0?[1-9]|1\d|2[0-8]))|(((?:(\d\d(?:0[48]|[2468][048]|[13579][26]))|(?:0[48]00|[2468][048]00|[13579][26]00))(-|/)0?2(-|/)29))))$" Display="None" ValidationGroup="GroupA">
                                        </asp:RegularExpressionValidator>
                                        <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToCompare="txt_APPROVE_DT_S"
                                             ControlToValidate="txt_APPROVE_DT_E" ErrorMessage="<%$Resources:Resource,wfb2he_ERR_txt_APPROVE_DT%>" Type="Date" Operator="GreaterThanEqual"
                                             Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                    </td> 
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2he_lb_APPROVE_BY%>"></asp:Label>：	
                                    </th>
                                    <td align="left">                                            
                                        <asp:TextBox ID="txt_APPROVE_BY" runat="server" MaxLength="20" Width="81px" ClientIDMode="Static" ></asp:TextBox>   
                                        <input id="Button1" type="button" value="..." onclick="OpenEmpSearch('txt_APPROVE_BY', 'txt_APPROVE_NAME', 'N');" />   
                                        <asp:TextBox ID="txt_APPROVE_NAME" runat="server" ClientIDMode="Static" BorderWidth="0" Width="81px"></asp:TextBox>                       
                                    </td> 
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_APPROVE_STATUS" runat="server" Text="<%$Resources:Resource,wfb2he_lb_APPROVE_STATUS%>"></asp:Label>：	
                                    </th>
                                    <td align="left">                                            
                                         <asp:DropDownList ID="ddl_APPROVE_STATUS" runat="server" ClientIDMode="Static" ></asp:DropDownList>                             
                                    </td>                      
                                </tr>
                                <tr>
                                    <th></th>
                                    <td align="right" colspan="5">
                                        <aces:Btn ID="WFB2HE0200Search" runat="server" Text="<%$Resources:Resource,wfb2he_btn_search%>" OnClick="WFB2HE0200Search_Click"/>                         
                                        <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2sh_btn_clear%>" onclick="ClearAll();" />

                                        <aces:Btn ID="WFB2HE0200UPDATE_BATCH" runat="server" Text="<%$Resources:Resource,wfb2he_WFB2HE0200UPDATE_BATCH%>" OnClick="WFB2HE0200UPDATE_BATCH_Click"/>
                                        <aces:Btn ID="WFB2HE0200Mail" runat="server" Text="<%$Resources:Resource,wfb2he_WFB2HE0200Mail%>" OnClick="WFB2HE0200Mail_Click"/>

                                        <%--
                                        <asp:Button ID="WFB2HE0200Search" runat="server" Text="<%$Resources:Resource,wfb2he_btn_search%>" OnClick="WFB2HE0200Search_Click"/>                         
                                        <asp:Button ID="WFB2HE0200UPDATE_BATCH" runat="server" Text="<%$Resources:Resource,wfb2he_WFB2HE0200UPDATE_BATCH%>" OnClick="WFB2HE0200UPDATE_BATCH_Click"/>
                                        <asp:Button ID="WFB2HE0200Mail" runat="server" Text="<%$Resources:Resource,wfb2he_WFB2HE0200Mail%>" OnClick="WFB2HE0200Mail_Click"/>                        

                                            --%>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                            <hr />
                    </td>
                </tr>
                <tr>                    
                    <td align="right" class="Body_label">                            
                        <aces:Btn ID="WFB2HE0200EDIT" runat="server" Text="<%$Resources:Resource,wfb2he_WFB2HE0200EDIT%>" Visible="false" OnClick="WFB2HE0200EDIT_Click"/>
                        <aces:Btn ID="WFB2HE0200DETAIL" runat="server" Text="<%$Resources:Resource,wfb2he_WFB2HE0200DETAIL%>" Visible="false" OnClick="WFB2HE0200DETAIL_Click"/>
                        <aces:Btn ID="WFB2HE0200ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2he_WFB2HE0200ExcelDown%>" Visible="false" OnClick="WFB2HE0200ExcelDown_Click" OnClientClick="return checkDown(this.value);"/>

                        <%--
                        <asp:Button ID="WFB2HE0200EDIT" runat="server" Text="<%$Resources:Resource,wfb2he_WFB2HE0200EDIT%>" Visible="false" OnClick="WFB2HE0200EDIT_Click"/>
                        <asp:Button ID="WFB2HE0200DETAIL" runat="server" Text="<%$Resources:Resource,wfb2he_WFB2HE0200DETAIL%>" Visible="false" OnClick="WFB2HE0200DETAIL_Click"/>
                        <asp:Button ID="WFB2HE0200ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2he_WFB2HE0200ExcelDown%>" Visible="false" OnClick="WFB2HE0200ExcelDown_Click" OnClientClick="return checkDown(this.value);"/>
                            --%>
                    </td>                        
                </tr> 
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2HE0200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_EMP_NAME"
                        Name="EMP_NAME" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                     <asp:ControlParameter ControlID="txt_PJOB_CD"
                        Name="PJOB_CD" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_INTERVIEW_PROCESS_STATUS" DefaultValue=""
                        Name="INTERVIEW_PROCESS_STATUS" PropertyName="SelectedValue" Type="String" />     
                     <asp:ControlParameter ControlID="txt_INTERVIEW_DT_S"
                        Name="INTERVIEW_DT_S" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                     <asp:ControlParameter ControlID="txt_INTERVIEW_DT_E"
                        Name="INTERVIEW_DT_E" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_INTERVIEW_BY"
                        Name="INTERVIEW_BY" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_INTERVIEW_RESULT" DefaultValue=""
                        Name="INTERVIEW_RESULT" PropertyName="SelectedValue" Type="String" />  
                     <asp:ControlParameter ControlID="txt_ADOPT_DT_S"
                        Name="ADOPT_DT_S" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                     <asp:ControlParameter ControlID="txt_ADOPT_DT_E"
                        Name="ADOPT_DT_E" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_ADOPT_BY"
                        Name="ADOPT_BY" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_ADOPT_RESULT" DefaultValue=""
                        Name="ADOPT_RESULT" PropertyName="SelectedValue" Type="String" />   
                     <asp:ControlParameter ControlID="txt_APPROVE_DT_S"
                        Name="APPROVE_DT_S" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                     <asp:ControlParameter ControlID="txt_APPROVE_DT_E"
                        Name="APPROVE_DT_E" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_APPROVE_BY"
                        Name="APPROVE_BY" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_APPROVE_STATUS" DefaultValue=""
                        Name="APPROVE_STATUS" PropertyName="SelectedValue" Type="String" />              
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="40px"  ItemStyle-Width="40px" >
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="SelectNonDisabledCheckboxes(this);" ClientIDMode="Static" Width="40px" />
                        </HeaderTemplate>                     
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" Width="40px"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--序號 --%>
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2df_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px" />
                    <%--應徵職務 --%>
                    <asp:BoundField DataField="PJOB_CD_DESC" HeaderText="<%$Resources:Resource,wfb2he_lb_PJOB_CD%>" SortExpression="PJOB_CD" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Left"/>
                    <%--員工區分 --%>
                    <asp:BoundField DataField="EMP_CD" HeaderText="<%$Resources:Resource,wfb2he_lb_EMP_CD%>" SortExpression="EMP_CD" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                    <%--姓名 --%>
                    <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2he_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center"/>
                    <%--出生日期 --%>
                    <asp:BoundField DataField="BIRTH_DT" HeaderText="<%$Resources:Resource,wfb2he_lb_BIRTH_DT%>" SortExpression="BIRTH_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center"/>
                    <%--身份證字號 --%>
                    <asp:BoundField DataField="LICENSE_ID" HeaderText="<%$Resources:Resource,wfb2he_lb_LICENSE_ID%>" SortExpression="LICENSE_ID" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Center"/>
                    <%--性別     --%>
                    <asp:BoundField DataField="SEX_CD" HeaderText="<%$Resources:Resource,wfb2he_lb_SEX_CD%>" SortExpression="SEX_CD" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />
                    <%--身高--%> 
                    <asp:BoundField DataField="HEIGHT" HeaderText="<%$Resources:Resource,wfb2he_lb_HEIGHT%>" SortExpression="HEIGHT" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Right"/>  
                    <%--體重 --%>
                    <asp:BoundField DataField="WEIGHT" HeaderText="<%$Resources:Resource,wfb2he_lb_WEIGHT%>" SortExpression="WEIGHT" HeaderStyle-Width="50px"  ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Right"/>  
                    <%--學歷 --%>
                    <asp:BoundField DataField="SCHOOL_NAME" HeaderText="<%$Resources:Resource,wfb2he_lb_SCHOOL_NAME%>" SortExpression="SCHOOL_NAME" HeaderStyle-Width="260px"   ItemStyle-Width="260px" ItemStyle-HorizontalAlign="Left"/>  
                    <%--介紹人 --%>
                    <asp:BoundField DataField="INTRODUCER" HeaderText="<%$Resources:Resource,wfb2he_lb_INTRODUCER%>" SortExpression="INTRODUCER" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left"/>  
                    <%--國瑞經驗 --%>
                    <asp:BoundField DataField="KZ_EXP" HeaderText="<%$Resources:Resource,wfb2he_lb_KZ_EXP%>" SortExpression="KZ_EXP" HeaderStyle-Width="80px"  ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center"/>  
                    <%--住宿與否 --%>
                    <asp:BoundField DataField="ACCOM_NEED" HeaderText="<%$Resources:Resource,wfb2he_lb_ACCOM_NEED%>" SortExpression="ACCOM_NEED" HeaderStyle-Width="80px"  ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center"/>  
                    <%--駕照別 --%>
                    <asp:BoundField DataField="TRANSPORT_LICENSE_CD" HeaderText="<%$Resources:Resource,wfb2he_lb_TRANSPORT_LICENSE_CD%>" SortExpression="TRANSPORT_LICENSE_CD" HeaderStyle-Width="80px"  ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left"/>  
                    <%--通勤工具 --%>
                    <asp:BoundField DataField="TRANSPORT_CD" HeaderText="<%$Resources:Resource,wfb2he_lb_TRANSPORT_CD%>" SortExpression="TRANSPORT_CD" HeaderStyle-Width="80px"  ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left"/>  
                    <%--兵役狀態 --%>
                    <asp:BoundField DataField="ARMY_CD" HeaderText="<%$Resources:Resource,wfb2he_lb_ARMY_CD%>" SortExpression="ARMY_CD" HeaderStyle-Width="80px"  ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left"/>  
                    <%--應徵日期  --%>
                    <asp:BoundField DataField="APPLY_DT" HeaderText="<%$Resources:Resource,wfb2he_lb_APPLY_DT%>" SortExpression="APPLY_DT" HeaderStyle-Width="100px"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center"/>  
                     <%--面試處理狀態  --%>
                    <asp:BoundField DataField="INTERVIEW_PROCESS_STATUS_DESC" HeaderText="<%$Resources:Resource,wfb2he_lb_INTERVIEW_PROCESS_STATUS%>" SortExpression="INTERVIEW_PROCESS_STATUS" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Left"/>  
                    <%--面試結果 --%>
                    <asp:BoundField DataField="INTERVIEW_RESULT" HeaderText="<%$Resources:Resource,wfb2he_lb_INTERVIEW_RESULT%>" SortExpression="INTERVIEW_RESULT" HeaderStyle-Width="80px"  ItemStyle-Width="80px"  ItemStyle-HorizontalAlign="Left"/>  
                    <%--面試人員 --%>
                    <asp:BoundField DataField="INTERVIEW_NAME" HeaderText="<%$Resources:Resource,wfb2he_lb_INTERVIEW_BY%>" SortExpression="INTERVIEW_BY" HeaderStyle-Width="80px"  ItemStyle-Width="80px"  ItemStyle-HorizontalAlign="Center"/>  
                    <%--面試日期 --%>
                    <asp:BoundField DataField="INTERVIEW_DT" HeaderText="<%$Resources:Resource,wfb2he_lb_INTERVIEW_DT%>" SortExpression="INTERVIEW_DT" HeaderStyle-Width="100px"  ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Center"/>  
                    <%--採用結果 --%>
                    <asp:BoundField DataField="ADOPT_RESULT" HeaderText="<%$Resources:Resource,wfb2he_lb_ADOPT_RESULT%>" SortExpression="ADOPT_RESULT" HeaderStyle-Width="80px"  ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left"/>  
                    <%--採用人員 --%>
                    <asp:BoundField DataField="ADOPT_NAME" HeaderText="<%$Resources:Resource,wfb2he_lb_ADOPT_BY%>" SortExpression="ADOPT_BY" HeaderStyle-Width="80px"  ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center"/>  
                    <%--採用日期 --%>
                    <asp:BoundField DataField="ADOPT_DT" HeaderText="<%$Resources:Resource,wfb2he_lb_ADOPT_DT%>" SortExpression="ADOPT_DT"  HeaderStyle-Width="100px"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center"/>  
                    <%--簽核狀態 --%>
                    <asp:BoundField DataField="APPROVE_STATUS" HeaderText="<%$Resources:Resource,wfb2he_lb_APPROVE_STATUS%>" SortExpression="APPROVE_STATUS" HeaderStyle-Width="80px"  ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left"/>  
                    <%--簽核人員 --%>
                    <asp:BoundField DataField="APPROVE_NAME" HeaderText="<%$Resources:Resource,wfb2he_lb_APPROVE_BY%>" SortExpression="APPROVE_BY" HeaderStyle-Width="80px"  ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center"/>  
                    <%--簽核日期 --%>
                    <asp:BoundField DataField="APPROVE_DT" HeaderText="<%$Resources:Resource,wfb2he_lb_APPROVE_DT%>" SortExpression="APPROVE_DT" HeaderStyle-Width="100px"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center"/> 
                    <asp:TemplateField HeaderText="" >
                        <ItemTemplate>
                            <asp:HiddenField ID="hid_PJOB_CD" runat="server" Value='<%#Bind("PJOB_CD")%>' ClientIDMode="Static" />
                            <asp:HiddenField ID="hid_INTERVIEW_PROCESS_STATUS" runat="server" Value='<%#Bind("INTERVIEW_PROCESS_STATUS")%>' ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
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

        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2HE0200ExcelDown" />  
        </Triggers>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
    <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
</asp:Content>


