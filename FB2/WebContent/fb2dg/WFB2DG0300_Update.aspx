<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2dg/WFB2DG0300_Update.aspx.cs" Inherits="WebContent_fb2dg_WFB2DG0300_Update" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <style type="text/css">
        #txt_EMP_ID {background-color:white;}
        #txt_EMP_NAME {background-color:white;}
        #txt_DEPT_NO {background-color:white;}
        #txt_LEVEL_CD {background-color:white;}
        #txt_PJOB_DESC {background-color:white;}
        #txt_LINE_CD {background-color:white;}
         #txt_PLANT_CD {background-color:white;}
    </style>
   
      
    <script type="text/javascript">


        jQuery(document).ready(function () {

            iniForm();

        });


        function iniForm() {
            $("#txt_REMAINDER_PARKING_SPOT").css("color", "black").css("background-color", "white").attr("disabled", true);
            
            $("#txt_YEAR_MONTH_Add").datepicker({ dateFormat: 'yymm' });
            $("#txt_START_DT_S").datepicker({ dateFormat: 'yy-mm-dd' });
            $("#txt_START_DT_E").datepicker({ dateFormat: 'yy-mm-dd' });
            $(".number").mask('9999/99');
            gridviewScroll();
            $.unblockUI();
           

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
        function ClearAll() {
            $('#ddl_SYS_ID').val(-1);
        }

        function CheckMODE_ID(source, arguments) {
            var re = /^[\d|a-zA-Z]+$/;
            if (!re.test($("#txt_MODE_ID_Add").val()))
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }
        function CheckSelectCount(source, arguments) {
            //if ($("#lb_select option").length == 0)
            //    arguments.IsValid = false;
            //else
            //    arguments.IsValid = true;

            if ($("#lb_select option").length == 0) {
                var check = $('#HID_NEED_SELECT').val();
                if (check == "N") {
                    arguments.IsValid = true;
                } else if (check == "Y") {
                    arguments.IsValid = false;
                } else {
                    arguments.IsValid = false;
                }

            } else
                arguments.IsValid = true;
        }

        function CheckModeifyAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidWFB2IB_Mod_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckDtlAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidWFB2IB_Dtl_NotChoiceMessage').val());
                return false;
            }
        }
        //儲存前檢查
        function saveCheck() {
           

           
            var processed = true;


            if (Page_ClientValidate("GroupA")) {
                if ($('#txt_REMAINDER_PARKING_SPOT').val() <= 0) {
                    return confirm('剩餘數小於等於0, 是否確定要儲存?');
                } else {
                    return confirm('是否確定儲存?');
                }

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

        function CheckSaveAction() {
            if ($('#txtCALENDAR_CD_Edit').val() != undefined) {
                if ($('#txtCALENDAR_CD_Edit').val().trim() == "") {
                    alert($('#hidWFB2IB_txtCALENDAR_CD_NotNull').val());
                    return false;
                }
                else
                    return confirm($('#hidWFB2IB_Save_ConfirmMessage').val());
            }
            else
                return confirm($('#hidWFB2IB_Save_ConfirmMessage').val());
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
        function OpenEmpSearch1(emp_id, emp_name, DEPT_NO, supervisor) {
            OpenEmpSearch(emp_id, emp_name, supervisor, 'Y');
            //var returnValue = window.showModalDialog("../comm/Dept_Search.aspx?mode=all&super=" + supervisor, self, 'dialogWidth=900px;dialogHeight=400px;scroll=no;addressbar:No;');
            //if (returnValue == undefined) {
            //    returnValue = window.returnValue;
            //}
            //if (!(typeof returnValue === 'undefined')) {

            //    var obj = jQuery.parseJSON(returnValue);
            //    $("#" + emp_id).val(obj.EMP_ID);
            //    $("#" + emp_name).val(obj.EMP_NAME);
            //    $("#" + DEPT_NO).val(obj.DEPT_NO);
            //    return obj;

            //}
        }
        function returnEMPValueToPage(eid, ename, value) {
            var returnValue = value;
            if (value == undefined) {
                returnValue = 'undefined';
            }
            //alert(returnValue);
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                $('#' + eid).val(obj.EMP_ID.trim());
                $("#" + ename).val(obj.EMP_NAME.trim());
                $('#txt_DEPT_NO').val(obj.DEPT_NO + obj.DEPT_NAME);         
                return obj;

            }
        }

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
 <!--2990400_QRY開始-->
           <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

                <tr height="100%" valign="top">
                    <td>






                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="10%" />
                                <col width="10%" />
                                <col width="10%" />
                                <col width="10%" />
                                <col width="10%" />
                                <col width="10%" />

                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2dg_lb_EMP_ID%>"></asp:Label>:

                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="10" Width="123px"  ClientIDMode="Static" Enabled="false" BorderWidth="0" ></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dg_lb_EMP_ID_Required%>"
                                        ControlToValidate="txt_EMP_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2dg_lb_EMP_NAME%>" ></asp:Label>:

                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="10" Width="64px"  ClientIDMode="Static"  Enabled="false" BorderWidth="0"></asp:TextBox>

                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_PLANT_CD" runat="server" Text="<%$Resources:Resource,wfb2dg_lbl_PLANT_CD%>"></asp:Label>:

                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_PLANT_CD" runat="server" MaxLength="10" Width="64px"  ClientIDMode="Static" Enabled="false" BorderWidth="0"></asp:TextBox>

                                    </td>
                                    </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_PARKING_PLANT_CD" runat="server" Text="<%$Resources:Resource,wfb2dg_lbl_PARKING_PLANT_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_PLANT_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField" OnSelectedIndexChanged="ddl_PLANT_CD_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>
                                        <asp:HiddenField ID="HidPLANT_CD" runat="server" ClientIDMode="Static" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dg_lbl_PARKING_PLANT_CD_Required%>"
                                        ControlToValidate="ddl_PLANT_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2dg_lbl_DEPT_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DEPT_NO" runat="server" ClientIDMode="Static" Enabled="false" BorderWidth="0"></asp:TextBox>
                                        <asp:HiddenField ID="HidDEPT_NAME" runat="server" ClientIDMode="Static" />
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_LEVEL_CD" runat="server" Text="<%$Resources:Resource,wfb2dg_lbl_LEVEL_CD%>" ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_LEVEL_CD" runat="server" ClientIDMode="Static" Enabled="false" BorderWidth="0"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_PJOB_DESC" runat="server" ClientIDMode="Static" Text="<%$Resources:Resource,wfb2dg_lbl_PJOB_DESC%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_PJOB_DESC" runat="server" ClientIDMode="Static" Enabled="false" BorderWidth="0"></asp:TextBox>
                                        <asp:HiddenField ID="HidPJOB_CD" runat="server" ClientIDMode="Static" />
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lbl_LINE_CD" runat="server" Text="<%$Resources:Resource,wfb2dg_lbl_LINE_CD%>"></asp:Label>:
                                    </th>
                                    
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_LINE_CD" runat="server" ClientIDMode="Static" Enabled="false" BorderWidth="0" ></asp:TextBox>
                                        <asp:HiddenField ID="Hid_work_shift_cd" runat="server" ClientIDMode="Static" />
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lbl_CAR_NO" runat="server" Text="<%$Resources:Resource,wfb2dg_lbl_CAR_NO%>"></asp:Label>:
                                    </th>
                                    
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_CAR_NO" runat="server" MaxLength="8" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                        <asp:HiddenField ID="HidCAR_NO" runat="server" ClientIDMode="Static" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dg_lbl_CAR_NO_Required%>"
                                        ControlToValidate="txt_CAR_NO" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="">
                                        </asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="onlyEngNum" runat="server"
                                        ErrorMessage="<%$Resources:Resource,wfb2dg_CAR_NO_Error%>" ControlToValidate="txt_CAR_NO" ForeColor="Red" ValidationGroup="GroupA"
                                        ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>

                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lbl_PARKING_CD" runat="server" Text="<%$Resources:Resource,wfb2dg_lbl_PARKING_CD%>"></asp:Label>:
                                    </th>
                                    
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_PARKING_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField" OnSelectedIndexChanged="ddl_PARKING_CD_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        <asp:HiddenField ID="HidPARKING_CD" runat="server" ClientIDMode="Static" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dg_lbl_PARKING_CD_Required%>"
                                        ControlToValidate="ddl_PARKING_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lbl_CAR_PARK_NO" runat="server" Text="<%$Resources:Resource,wfb2dg_lbl_CAR_PARK_NO%>"></asp:Label>:
                                    </th>
                                    
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_CAR_PARK_NO" runat="server" ClientIDMode="Static" CssClass="MandatoryField" OnSelectedIndexChanged="ddl_CAR_PARK_NO_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        <asp:HiddenField ID="HidCAR_PARK_NO" runat="server" ClientIDMode="Static" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dg_lbl_CAR_PARK_NO_Required%>"
                                        ControlToValidate="ddl_CAR_PARK_NO" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    </tr>
                                    <tr>
                                    <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lbl_CAR_BRAND" runat="server" Text="<%$Resources:Resource,wfb2dg_lbl_CAR_BRAND%>"></asp:Label>:
                                    </th>
                                    
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_CAR_BRAND" runat="server" ClientIDMode="Static" CssClass="MandatoryField"  OnSelectedIndexChanged="ddl_BRAND_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        <asp:HiddenField ID="HidCAR_BRAND" runat="server" ClientIDMode="Static" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dg_lbl_CAR_BRAND_Required%>"
                                        ControlToValidate="ddl_CAR_BRAND" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lbl_CAR_TYPE" runat="server" Text="<%$Resources:Resource,wfb2dg_lbl_CAR_TYPE%>"></asp:Label>:
                                    </th>
                                    
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_CAR_TYPE" runat="server" ClientIDMode="Static" CssClass="MandatoryField">
                                           
                                        </asp:DropDownList>
                                         <asp:HiddenField ID="HidCAR_TYPE" runat="server" ClientIDMode="Static" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dg_lbl_CAR_TYPE_Required%>"
                                        ControlToValidate="ddl_CAR_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="">
                                        </asp:RequiredFieldValidator>

                                    </td>
                                    </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lbl_REMAINDER_PARKING_SPOT" runat="server" Text="<%$Resources:Resource,wfb2dg_lbl_REMAINDER_PARKING_SPOT%>"></asp:Label>:
                                    </th>
                                    
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_REMAINDER_PARKING_SPOT" runat="server" ClientIDMode="Static"  BorderWidth="0" Enabled="false" ></asp:TextBox>
                                    </td>
                                   
                                    </tr>
                                   <tr>
                                    <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2dg_lbl_CLOCK%>"></asp:Label>:
                                    </th>

                                <td height="177" align="right">
                                    <asp:ListBox ID="lb_unselect" runat="server" SelectionMode="Multiple" ClientIDMode="Static"
                                        Height="171" Width="200"></asp:ListBox>
                                </td>
                                <td>

                                    <table align="center">
                                        <tr>
                                            <td>
                                                
                                                <asp:Button ID="btn_select" runat="server" Text=">"  style="height: 30px; width: 30px;" OnClick="btn_select_Click"/>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btn_unselect" runat="server" Text="<"  style="height: 30px; width: 30px;" OnClick="btn_unselect_Click"/>
                                        </tr>
                                    </table>
                                                                        <table align="center">
                                        <tr>
                                            <td>
                                                
                                                &nbsp;</tr>
                                        <tr>
                                            <td>
                                                <asp:HiddenField ID="HidIFLOW_NO" runat="server" ClientIDMode="Static" />
                                                &nbsp;</tr>
                                    </table>

                                </td>
                                <td>
                                    <asp:ListBox ID="lb_select" runat="server" SelectionMode="Multiple" Height="171" Width="200" ClientIDMode="Static" CssClass="MandatoryField"></asp:ListBox>
                                                                <asp:CustomValidator ID="CustomValidator3" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dg_lb_select_Required%>" ClientValidationFunction="CheckSelectCount" ForeColor="Red"
                                ControlToValidate="lb_select" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                                </td>
                            </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2DG030Save" runat="server" Text="<%$Resources:Resource,btn_Save%>" OnClick="WFB2DG030Save_Click" OnClientClick="return saveCheck();" ValidationGroup="GroupA" />

                                            <%--<asp:Button ID="WFB2DG030Save" runat="server" Text="<%$Resources:Resource,btn_Save%>" OnClick="WFB2DG030Save_Click" OnClientClick="return saveCheck();" ValidationGroup="GroupA" />--%>
                                            
                                            <asp:Button ID="WFB2IB0100Clear" runat="server" type="button" Text="<%$Resources:Resource,btn_Cancel%>" OnClientClick="return confirm('是否確定取消?');" OnClick="WFB2DG030Clear_Click" />
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
<!--2990400_QRY結束-->
           <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData2"
                SelectCountMethod="getCount2" TypeName="Cfb2DG030DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="ods1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                 <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                   <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="txt_EMP_ID" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                     



                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_UPDATE_DT%>" SortExpression="UPDATE_DT">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:Label ID="lbl_UPDATE_DT" runat="server" Text='<%#Bind("UPDATE_DT")%>' ></asp:Label>
                            </div>
                        </ItemTemplate>                        
                    </asp:TemplateField>
                     
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_PLANT_CD%>" SortExpression="PARKING_PLANT_CD">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_PARKING_PLANT_CD" runat="server" Text='<%#Bind("PARKING_PLANT_CD")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                           
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_DEPT_NO%>" SortExpression="DEPT_NO">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_DEPT_NO" runat="server" Text='<%# Convert.ToString(Eval("DEPT_NO"))%>'></asp:Label>
                            </div>
                        </ItemTemplate>                        
                    </asp:TemplateField>
                  <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_DEPT_NAME%>" SortExpression="DEPT_NAME">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_DEPT_NAME" runat="server" Text='<%#Bind("DEPT_NAME")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_LEVEL_CD%>" SortExpression="LEVEL_CD">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="lbl_LEVEL_CD" runat="server" Text='<%#Bind("LEVEL_CD")%>'></asp:Label>
                            </div>
                        </ItemTemplate>                       
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_PJOB_NAME%>" SortExpression="PJOB_NAME">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_PJOB_NAME" runat="server" Text='<%#Bind("PJOB_NAME")%>'></asp:Label>

                                 </div>
                        </ItemTemplate>
                        
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_PARKING_CD%>" SortExpression="PARKING_TOOL">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="lbl_PARKING_TOOL" runat="server" Text='<%#Bind("PARKING_TOOL")%>'></asp:Label>

                                 </div>
                        </ItemTemplate>
                        
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_CAR_PARK_NO%>" SortExpression="CAR_PARK_NO">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_CAR_PARK_NO" runat="server"  Text='<%#Bind("CAR_PARK_NO")%>'></asp:Label>

                                 </div>
                        </ItemTemplate>
                        
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_CAR_BRAND%>" SortExpression="CAR_BRAND">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="lbl_CAR_BRAND" runat="server" Text='<%#Bind("CAR_BRAND")%>'></asp:Label>

                                 </div>
                        </ItemTemplate>
                        
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_CAR_TYPE%>" SortExpression="CAR_TYPE">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="lbl_CAR_TYPE" runat="server" Text='<%#Bind("CAR_TYPE")%>'></asp:Label>

                                 </div>
                        </ItemTemplate>

                        
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_CAR_NO%>" SortExpression="CAR_NO">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_CAR_NO" runat="server" Text='<%#Bind("CAR_NO")%>'></asp:Label>

                                 </div>
                        </ItemTemplate>

                        
                    </asp:TemplateField>
                   
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
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

            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_NotChoiceMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Mod_NotChoiceMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Save_ConfirmMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_ConfirmMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Dtl_NotChoiceMessage" />
            <asp:HiddenField ID="HID_FUNC_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2_alphanumeric_onlyMessage" Value="<%$Resources:Resource,wfb2_alphanumeric_onlyMessage%>" />
            <asp:HiddenField ID="HID_NEED_SELECT" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


