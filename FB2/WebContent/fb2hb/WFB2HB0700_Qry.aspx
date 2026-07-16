<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2hb/WFB2HB0700_Qry.aspx.cs" Inherits="WebContent_fb2hb_WFB2HB0700_Qry"  Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $("#txt_JOIN_DT").mask("9999/99/99");
            gridviewScroll();
            $.unblockUI();


            $('#txt_DEPT_NAME').attr("readonly", true);
            //部門代號取得部門名稱的ajax
            $("#txt_DEPT_NO").change(function () {
                if ($("#txt_DEPT_NO").val().length == 7) {
                    $.ajax({
                        url: "../commgeo/WFB2GetDeptData.ashx",
                        data: {
                            DEPT_NO: $('#txt_DEPT_NO').val()
                        },
                        type: "GET",
                        cache: false,
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

            
        }
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                 width: "1020",
                 height: "500",
                 barcolor: "#7F7F7F",
                 freezesize: 5
            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
       }      

        function OpenLogin(login_dt, supervisor) {
            //是否能進行啟動登錄作業
            var isSearch = document.getElementById("hid_serrch").value;
            var join_dt = document.getElementById("txt_JOIN_DT").value;
            var emp_name = document.getElementById("txt_EMP_NAME").value;
            var dept_no = document.getElementById("txt_DEPT_NO").value;
            var company_cd = document.getElementById("ddl_COMPANY_CD").value;
            var plant_cd = document.getElementById("ddl_PLANT_CD").value;
            var emp_cd = document.getElementById("ddl_EMP_CD").value;
            var login_cd = document.getElementById("ddl_LOGIN_CD").value;
            var ws_cd = document.getElementById("ddl_WS_CD").value;

            //alert("isSearch=" + isSearch);
            //if (isSearch == 'N') {
            //    alert("請先查詢再按啟動登錄作業");
            //    return;
            //}

            var isCheck = document.getElementById("hid_JPN").value;
                       
            //var returnValue = window.showModalDialog("../fb2hb/Login_Start.aspx?isCheck=" + isCheck + "&super=" + supervisor + "&join_dt=" + join_dt + "&emp_name=" + emp_name + "&dept_no=" + dept_no + "&company_cd=" + company_cd + "&plant_cd=" + plant_cd + "&emp_cd=" + emp_cd + "&login_cd=" + login_cd + "&ws_cd=" + ws_cd + "&parentFuncId=" + parentFuncID, self, 'dialogWidth=600px;dialogHeight=400px;scroll=no');
            
            var myiFrameId = "iframe";
            var Url = "../fb2hb/Login_Start.aspx?isCheck=" + isCheck + "&super=" + supervisor + "&join_dt=" + join_dt + "&emp_name=" + emp_name + "&dept_no=" + dept_no + "&company_cd=" + company_cd + "&plant_cd=" + plant_cd + "&emp_cd=" + emp_cd + "&login_cd=" + login_cd + "&ws_cd=" + ws_cd + "&parentFuncId=" + parentFuncID;
            var dialogID = 'div_iframeID';
            var $dialog = $('<div id = "' + dialogID + '"></div>')
                        .html('<iframe style="border: 0px; " src="' + Url + '" id="' + myiFrameId + '" width="100%" height="100%"></iframe>')
                        .dialog({
                            autoOpen: false,
                            modal: true,
                            draggable: true,
                            resizable: false,
                            height: 500,
                            width: 600,
                            close: function (ev, ui) {
                                $("#" + dialogID).dialog("destroy");
                            }
                        });           

            $dialog.dialog('open');        


        }

        function returnHB070(value) {
            var returnValue = value;
            if (returnValue == undefined) {
                returnValue = window.returnValue;
            }
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                var hid_JOIN_DT_2 = document.getElementById("hid_JOIN_DT_2").value;
                $("#hid_JOIN_DT_2").val(obj.CD);
                hid_JOIN_DT_2 = obj.CD;
                __doPostBack('exec', 'true');
                BlockUI();
                return false;

            }
        }

        //清空畫面
        function ClearAll() {
            $('#ddl_COMPANY_CD').val(-1);
            $('#ddl_PLANT_CD').val(-1);
            $('#ddl_EMP_CD').val(-1);
            $('#ddl_LOGIN_CD').val(-1);
            $('#ddl_WS_CD').val(-1);
            $("#txt_JOIN_DT").val("");
            $("#txt_EMP_NAME").val("");
            $("#txt_DEPT_NO").val("");
            $("#txt_DEPT_NAME").val("");

            
        }

        function CheckDelAction() {
            
            if (LookUpCheckboxs() > 0)
                return confirm($('#hidwfb299_Del_ConfirmMessage').val());
            else {
                alert($('#hidwfb299_Del_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckUpdAction() {

            if (LookUpCheckboxs() > 0)
                return confirm('確定要修改登錄區分？?');
            else {
                alert($('#hidwfb299_Del_NotChoiceMessage').val());
                return false;
            }
        }


        function LookUpCheckboxs() {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                }
            }
           
            return HaveCheck;
        }


    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
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
								<col width="20%" />
								<col width="10%" />
								<col width="30%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_JOIN_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_JOIN_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">                                        
                                        <asp:TextBox ID="txt_JOIN_DT" runat="server" MaxLength="10" Width="80px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                        <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2df_ERR_JOIN_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_JOIN_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>                        
                                                                   
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2hb_EMP_NAME%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">                                        
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="30" Width="64px" ClientIDMode="Static"></asp:TextBox>                                                                
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2hb_DEPT_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">                                        
                                        <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" Width="64px" ClientIDMode="Static"></asp:TextBox>                                                                
                                        <input id="bt_DEPT_SEARCH" type="button" value="..." onclick="OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', 'N');" />
                                        <asp:TextBox ID="txt_DEPT_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                     <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_COMPANY_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_COMPANY_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label"> 
                                        <asp:DropDownList ID="ddl_COMPANY_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>                           
                                    </td>                            
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PLANT_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_PLANT_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">  
                                        <asp:DropDownList ID="ddl_PLANT_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>                                      
                                    </td>
                                     <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_EMP_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_EMP_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>                                       
                                    </td>
                                </tr>
                                <tr>                                  
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LOGIN_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_LOGIN_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_LOGIN_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_WS_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_WS_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_WS_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                        <asp:HiddenField id="hid_userid" runat="server" ClientIDMode="Static" value="" />
                                        <asp:HiddenField id="hid_JPN" runat="server" ClientIDMode="Static" value="" />
                                        <asp:HiddenField id="hid_serrch" runat="server" ClientIDMode="Static" value="N" />
                                        <asp:HiddenField id="hid_JOIN_DT_2" runat="server" ClientIDMode="Static" value="" />
                                    </td>
                                </tr>                               
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                     <th></th>
                                    <td align="right" class="Body_label">
                                        <aces:Btn ID="WFB2HB0700Search" runat="server" Text="<%$Resources:Resource,wfb2df_WFB2HB0700Search%>" OnClientClick="CheckValid();" ValidationGroup="GroupA" OnClick="WFB2HB0700Search_Click" />
                                        <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2hb_btn_clear%>" onclick="ClearAll();" />
                                        <aces:Btn ID="WFB2HB0700UPLOAD" runat="server" Text="<%$Resources:Resource,wfb2hb_btn_upload%>" OnClick="btn_upload_Click" />                                        
                                         <%--
                                        <asp:Button ID="WFB2HB0700Search" runat="server" Text="<%$Resources:Resource,wfb2df_WFB2HB0700Search%>" OnClientClick="CheckValid();" ValidationGroup="GroupA" OnClick="WFB2HB0700Search_Click" />
                                         <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2hb_btn_clear%>" onclick="ClearAll();" />
                                        <asp:Button ID="WFB2HB0700UPLOAD" runat="server" Text="<%$Resources:Resource,wfb2hb_btn_upload%>" OnClick="btn_upload_Click" />
                                          --%>                                          
                                    </td>                                   
                                </tr>
                                 <tr>
                                    <td align="center" height="1" colspan="10">
                                        <hr>
                                    </td>
                                </tr>
                                <tr>
                                    <div id="hid_buttons" runat="server" visible="false">
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2hb_REGISTER_UPD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">                                        
                                        <asp:DropDownList ID="ddl_LOGIN_CD_2" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                        <aces:Btn ID="WFB2HB0700Update" runat="server" Text="<%$Resources:Resource,wfb2hb_REGISTER_ALL%>" OnClientClick="return CheckUpdAction();" OnClick="WFB2HB0700Update_Click" />
                                            
                                        <%--
                                        <asp:Button ID="WFB2HB0700Update" runat="server" Text="<%$Resources:Resource,wfb2hb_REGISTER_ALL%>" OnClientClick="return CheckUpdAction();" OnClick="WFB2HB0700Update_Click" />
                                            --%>
                                    </td>
                                    <td>
                                        
                                    </td>
                                    <td align="right" colspan="10"> 
                                        
                                        <aces:Btn ID="WFB2HB0700Admit" runat="server" Text="<%$Resources:Resource,wfb2hb_REGISTER_START%>" OnClientClick="return OpenLogin('txt_LOGIN_DT','N');" />                                      
                                        <aces:Btn ID="WFB2HB0700Delete" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0400Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2HB0700Delete_Click" />
                                        
                                        <%--
                                        <asp:Button ID="WFB2HB0700Admit" runat="server" Text="<%$Resources:Resource,wfb2hb_REGISTER_START%>" OnClientClick="return OpenLogin('txt_LOGIN_DT','N');" />                                      
                                        <asp:Button ID="WFB2HB0700Delete" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0400Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2HB0700Delete_Click" />
                                        --%>
                                    </td>
                                        </div>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>               
            </table>
             <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2HB0700DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_JOIN_DT" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="join_dt" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="txt_EMP_NAME" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="emp_name" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="txt_DEPT_NO" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="dept_no" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="ddl_COMPANY_CD" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="company_cd" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="ddl_PLANT_CD" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="plant_cd" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="ddl_EMP_CD" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="emp_cd" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="ddl_LOGIN_CD" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="login_cd" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="ddl_WS_CD" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="ws_cd" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="hid_userid" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="userid" PropertyName="Value" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" 
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1090px" 
                OnPageIndexChanging="gv_result_PageIndexChanging" meta:resourcekey="gv_resultResource1">
               <Columns>
                    <asp:TemplateField HeaderStyle-Width="30px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" meta:resourcekey="cb_checkallResource1" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" meta:resourcekey="cb_checkResource1" ClientIDMode="AutoID" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:CheckBox ID="cb_check" runat="server" Checked="true" />
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2df_RowNumber%>" SortExpression="RowNumber" HeaderStyle-Width="40px" ItemStyle-Width="40px"/>
                    <asp:BoundField DataField="LOGIN_CD" HeaderText="<%$Resources:Resource,wfb2hb_lb_LOGIN_CD%>" SortExpression="LOGIN_CD" HeaderStyle-Width="80px" ItemStyle-Width="80px" />
                    <asp:BoundField DataField="DEPT_NO" HeaderText="<%$Resources:Resource,wfb2hb_lb_DEPT_NO%>" SortExpression="DEPT_NO" HeaderStyle-Width="110px" ItemStyle-Width="110px" />
                    <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2hb_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="100px" ItemStyle-Width="100px"/>
                    <asp:BoundField DataField="JOIN_DT" HeaderText="<%$Resources:Resource,wfb2hb_lb_JOIN_DT%>" SortExpression="JOIN_DT" HeaderStyle-Width="120px" ItemStyle-Width="120px"/>
                    <asp:BoundField DataField="COMPANY_CD_DESC" HeaderText="<%$Resources:Resource,wfb2hb_lb_COMPANY_CD%>" SortExpression="COMPANY_CD" HeaderStyle-Width="80px" ItemStyle-Width="80px"/>
                    <asp:BoundField DataField="PLANT_CD_DESC" HeaderText="<%$Resources:Resource,wfb2hb_lb_PLANT_CD%>" SortExpression="PLANT_CD" HeaderStyle-Width="100px" ItemStyle-Width="100px"/>
                    <asp:BoundField DataField="EMP_CD_DESC" HeaderText="<%$Resources:Resource,wfb2hb_lb_EMP_CD%>" SortExpression="EMP_CD" HeaderStyle-Width="100px" ItemStyle-Width="100px"/>
                    <asp:BoundField DataField="WS_CD_DESC" HeaderText="<%$Resources:Resource,wfb2hb_lb_WS_CD%>" SortExpression="WS_CD" HeaderStyle-Width="100px" ItemStyle-Width="120px"/>
                    <asp:BoundField DataField="LEVEL_CD" HeaderText="<%$Resources:Resource,wfb2hb_lb_LEVEL_CD%>" SortExpression="LEVEL_CD" HeaderStyle-Width="80px" ItemStyle-Width="80px"/>
                    <asp:BoundField DataField="GRADE_CD" HeaderText="<%$Resources:Resource,wfb2hb_lb_GRADE_CD%>" SortExpression="GRADE_CD" HeaderStyle-Width="80px" ItemStyle-Width="80px"/>
                    <asp:BoundField DataField="PJOB_CD" HeaderText="<%$Resources:Resource,wfb2hb_lb_PJOB_CD%>" SortExpression="PJOB_CD" HeaderStyle-Width="80px" ItemStyle-Width="80px"/>                   
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
            
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Del_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Del_ConfirmMessage%>" />    
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />      
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />           
        </ContentTemplate>

       
    </asp:UpdatePanel>

</asp:Content>



