<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2dg/WFB2DG0300_Qry.aspx.cs" Inherits="WebContent_fb2dg_WFB2DG0300_Qry" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    
   
      
    <script type="text/javascript">

        

        jQuery(document).ready(function () {
            
            iniForm();

        });
       
        
        function iniForm() {
            $("#txt_DEPT_NAME").css("color", "black").css("background-color", "white").attr("disabled", true);
            $("#txt_YEAR_MONTH_Add").datepicker({ dateFormat: 'yymm' });
            $("#txt_START_DT_S").datepicker({ dateFormat: 'yy-mm-dd' });
            $("#txt_START_DT_E").datepicker({ dateFormat: 'yy-mm-dd' });
            $("#txt_EMP_ID").mask('99999');
            
            $(".number").mask('9999/99');
            gridviewScroll();
            $.unblockUI();
/*
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
*/
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
        function doEmpAjax() {
            //ajax 取得員工基本資料
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
                        $('#txt_EMP_CD').val("");
                        $('#txt_DEPT_DESC').val("");
                        $('#txt_LEAVE_DT').val("");
                        alert(JData.errMsg);
                    } else {
                        $('#txt_EMP_NAME').val(JData.EMP_NAME);
                        $('#txt_EMP_CD').val(JData.EMP_CD_DESC);
                        $('#txt_DEPT_DESC').val(JData.DEPT_NO + '-' + JData.DEPT_NAME);
                        $('#txt_LEAVE_DT').val(JData.LEAVE_DT);
                    }
                },

                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
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
        function ClearAll() {
            $('#txt_EMP_ID').val("");
            $('#txt_EMP_NAME').val("");
            $('#txt_DEPT_NO').val("");
            $('#txt_DEPT_NAME').val("");
            $('#ddl_PLANT_CD').val(-1);
            $('#txt_CAR_PARK_NO').val("");
            $('#txt_CAR_NO').val("");
           
            $('#txt_EMP_NAME1').val("");

            
        }
        function CheckDelAction() {
            if (LookUpCheckboxs() > 0) {
                BlockUI();
                if (confirm($('#HidCheckDelMessage').val())) {
                    BlockUI();
                    return;
                }
                else {
                    $.unblockUI();
                    return false;
                }
            }
            else {
                alert("請選取資料!");
                $.unblockUI();
                return false;
            }
        }
        function CheckMODE_ID(source, arguments) {
            var re = /^[\d|a-zA-Z]+$/;
            if (!re.test($("#txt_MODE_ID_Add").val()))
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }

        function CheckModeifyAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert("請選取一筆資料!");
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


            //if (Page_ClientValidate("GroupA")) {
            //    BlockUI();
            //}
            //else
            //    processed = false;
            //if (!processed)
            //    $.unblockUI();

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
                                <col width="35%" />
                                <col width="10%" />
                                <col width="45%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2dg_lb_EMP_ID%>"></asp:Label>:

                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px"  ClientIDMode="Static"></asp:TextBox>
                                        <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', '');" />
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2dg_lbl_EMP_NAME%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="30" Width="64px"  ClientIDMode="Static"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2dg_lbl_DEPT_NO%>"></asp:Label>:

                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" Width="64px"  ClientIDMode="Static"></asp:TextBox>
                                        <input id="Button14" type="button" value="..." onclick="OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', 'N');" />
                                        <asp:TextBox ID="txt_DEPT_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>

                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_PARKING_PLANT_CD" runat="server" Text="<%$Resources:Resource,wfb2dg_lbl_PLANT_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_PLANT_CD" runat="server" ClientIDMode="Static" ></asp:DropDownList>
                                        <%-- 
                                          <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="請選擇工廠區分"
                                            ControlToValidate="ddl_PLANT_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                        --%>
                                    </td>

                                </tr>
                              <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_CAR_PARK_NO" runat="server" Text="<%$Resources:Resource,wfb2dg_lbl_CAR_PARK_NO%>"></asp:Label>:

                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_CAR_PARK_NO" runat="server" MaxLength="10" Width="120px"  ClientIDMode="Static"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2dd_ERR_CAR_PARK_NO%>" ControlToValidate="txt_CAR_PARK_NO" ForeColor="Red"
                                            ValidationExpression="^[0-9a-zA-Z_]+$" Display="None" ValidationGroup="GroupA">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_CAR_NO" runat="server" Text="<%$Resources:Resource,wfb2dg_lbl_CAR_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_CAR_NO" runat="server" MaxLength="10" Width="64px"  ClientIDMode="Static"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2df_ERR_CAR_NO%>" ControlToValidate="txt_CAR_NO" ForeColor="Red"
                                            ValidationExpression="^[0-9a-zA-Z_]+$" Display="None" ValidationGroup="GroupA">
                                        </asp:RegularExpressionValidator>
                                    </td>

                                </tr>
                               


                                <tr>

                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2DG030Search" runat="server" Text="<%$Resources:Resource,btn_Search%>" OnClick="WFB2DG030Search_Click" ValidationGroup="GroupA" />
                                            
                                            <%--<asp:Button ID="WFB2DG030Search" runat="server" Text="<%$Resources:Resource,btn_Search%>" OnClick="WFB2DG030Search_Click" ValidationGroup="GroupA" />--%>
                                            
                                            <input id="WFB2DG030Clear" type="button" value="<%$Resources:Resource,btn_clear%>" runat="server" onclick="ClearAll();"/>
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
                        <aces:Btn ID="WFB2DG030Add" runat="server" Text="<%$Resources:Resource,btn_Add%>" OnClick="WFB2DG030Add_Click" />
                            <aces:Btn ID="WFB2DG030Delete" runat="server" Text="<%$Resources:Resource,btn_Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2DG030Delete_Click" Visible="False" />
                            <aces:Btn ID="WFB2DG030Edit" runat="server" Text="<%$Resources:Resource,btn_Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2DG030Edit_Click" Visible="False" />
                            <aces:Btn ID="WFB2DG030Save" runat="server" Text="<%$Resources:Resource,btn_Save%>" Visible="false" OnClick="WFB2DG030Save_Click" OnClientClick="return saveCheck();" ValidationGroup="GroupA" />

<%--                            <asp:Button ID="WFB2DG030Add" runat="server" Text="<%$Resources:Resource,btn_Add%>" OnClick="WFB2DG030Add_Click" />
                            <asp:Button ID="WFB2DG030Delete" runat="server" Text="<%$Resources:Resource,btn_Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2DG030Delete_Click" Visible="False" />
                            <asp:Button ID="WFB2DG030Edit" runat="server" Text="<%$Resources:Resource,btn_Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2DG030Edit_Click" Visible="False" />
                            <asp:Button ID="WFB2DG030Save" runat="server" Text="<%$Resources:Resource,btn_Save%>" Visible="false" OnClick="WFB2DG030Save_Click" OnClientClick="return saveCheck();" ValidationGroup="GroupA" />--%>
                            
                            <asp:Button ID="WFB2DG030Cancel" runat="server" Text="<%$Resources:Resource,btn_Cancel%>" Visible="false" OnClick="WFB2DG030Cancel_Click" OnClientClick="return confirm('是否確定取消?');" />
                        </div>

                    </td>
                </tr>
            </table>
<!--2990400_QRY結束-->
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="Cfb2DG030DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="txt_EMP_ID" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                   <asp:ControlParameter ControlID="txt_EMP_NAME"
                        Name="txt_EMP_NAME" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                     <asp:ControlParameter ControlID="txt_DEPT_NO"
                        Name="txt_DEPT_NO" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_PLANT_CD"
                        Name="ddl_PLANT_CD" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_CAR_PARK_NO"
                        Name="txt_CAR_PARK_NO" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                     <asp:ControlParameter ControlID="txt_CAR_NO"
                        Name="txt_CAR_NO" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                     



                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField>
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
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2ib_RowNumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                     
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dg_lb_EMP_ID%>" SortExpression="EMP_ID" ItemStyle-Width="50" >
                        <ItemTemplate>
                            <div style="text-align: center">
                                <asp:Label ID="lbl_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>' ></asp:Label>
                                <asp:HiddenField ID="HidLEVEL_CD" Value='<%#Bind("LEVEL_CD")%>' runat="server" />
                                <asp:HiddenField ID="HidPJOB_CD" Value='<%#Bind("PJOB_CD")%>' runat="server" />
                                <asp:HiddenField ID="HidPJOB_DESC" Value='<%#Bind("PJOB_DESC")%>' runat="server" />
                                <asp:HiddenField ID="HidWORK_SHIFT_DESC" Value='<%#Bind("WORK_SHIFT_CD")%>' runat="server" />
                                <asp:HiddenField ID="HidPARKING_PLANT_CD" Value='<%#Bind("PARKING_PLANT_CD")%>' runat="server" />
                                <asp:HiddenField ID="HidCAR_BRAND" Value='<%#Bind("CAR_BRAND")%>' runat="server" />
                                <asp:HiddenField ID="HidCAR_TYPE" Value='<%#Bind("CAR_TYPE")%>' runat="server" />
                                <asp:HiddenField ID="HidPARKING_TOOL" Value='<%#Bind("PARKING_TOOL")%>' runat="server" />
                                <asp:HiddenField ID="HidCAR_PARK_NO" Value='<%#Bind("CAR_PARK_NO")%>' runat="server" />
                                <asp:HiddenField ID="HidIFLOW_NO" Value='<%#Bind("IFLOW_NO")%>' runat="server" />

                            </div>
                        </ItemTemplate>                        
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dg_lb_EMP_NAME%>" SortExpression="EMP_NAME" ItemStyle-Width="50" >
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                           
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dg_lb_DEPT_NO%>" SortExpression="DEPT_NO" ItemStyle-Width="70">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="lbl_DEPT_NO" runat="server" Text='<%# Convert.ToString(Eval("DEPT_NO"))%>'></asp:Label>
                            </div>
                        </ItemTemplate>                        
                    </asp:TemplateField>
                  <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dg_lb_DEPT_NAME%>" SortExpression="DEPT_NAME">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_DEPT_NAME" runat="server" Text='<%#Bind("DEPT_NAME")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dg_lb_CAR_PARK_NO%>" SortExpression="CAR_PARK_NO">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_CAR_PARK_NO" runat="server" Text='<%#Bind("CAR_PARK2")%>'></asp:Label>
                            </div>
                        </ItemTemplate>                       
                    </asp:TemplateField>
                    
                    
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dg_lb_CAR_NO%>" SortExpression="CAR_NO">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_CAR_NO" runat="server"  Text='<%#Bind("CAR_NO")%>'></asp:Label>

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
                        <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static" >
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

            <asp:HiddenField runat="server" ClientIDMode="Static" ID="checkDateFailMessage" Value="<%$Resources:Resource,wfb2ib_check_Error%>"/>
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="DateFailMessage" Value="<%$Resources:Resource,wfb2ib_checkdata_Error%>"/>
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="HidCheckDelMessage" Value="<%$Resources:Resource,wfb2ib_CheckDel_Required%>"/>
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="HidCheckCanMessage" Value="<%$Resources:Resource,wfb2ib_CheckCan_Required%>"/>
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="HidCheckEditMessage" Value="<%$Resources:Resource,wfb2ib_CheckEdit_Required%>"/>
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="HidCheckDeleteMessage" Value="<%$Resources:Resource,wfb2ib_CheckDelete_Required%>"/>

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


