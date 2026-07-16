<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dd/WFB2DD0100_Qry.aspx.cs" Inherits="WebContent_fb2dd_WFB2DD0100_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $("#txt_EMP_ID").mask("99999");
            $("#txt_NEW_EMP_ID").mask("99999");
            gridviewScroll();
            $.unblockUI();
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


            //新增.修改
            $('#txt_NEW_EMP_ID').change(function () {

                //ajax 取得員工基本資料
                $.ajax({
                    url: "WFB2DD0100_GetEmpData.ashx",
                    data: {
                        EMP_ID: $('#txt_NEW_EMP_ID').val()
                    },
                    type: "GET",
                    dataType: 'json',
                    success: function (JData) {
                        if (JData.errMsg != "")
                            alert(JData.errMsg);
                        else {
                            $('#lb_NEW_EMP_ID').text(JData.EMP_ID);
                            $('#lb_NEW_EMP_NAME').text(JData.EMP_NAME);
                            $('#lb_NEW_DEPT_NAME').text(JData.DEPT_NAME);
                            $('#lb_NEW_PLANT_CD').text(JData.PLANT_NAME);
                        }
                    },

                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });

            });
        }

        function gridviewScroll() {

            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "400",
                barcolor: "#7F7F7F"
                //freezesize: 3
            });

        }

        //清空畫面
        function ClearAll() {
            //$('#ddl_SYS_CD').val(-1);
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $('#<%=rb_IS_CANCEL.ClientID %>').find("input[value=Y]").prop("checked", "");
            $('#<%=rb_IS_CANCEL.ClientID %>').find("input[value=N]").prop("checked", "");
            $('#<%=rb_IS_CALCULATE.ClientID %>').find("input[value=1]").prop("checked", "");
            $('#<%=rb_IS_CALCULATE.ClientID %>').find("input[value=0]").prop("checked", "");
            $("#rb_IS_CALCULATE").attr('checked', false);
            $("#txt_DEPT_NO").val("");
            $("#txt_DEPT_NAME").val("");
            $("#ddl_PLANT_CD").val("-1");
            $("#rb_IS_CANCEL").attr('checked', false);
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

            if ($("#hid_Valid_Flag").val() =="Y") {
                if (Page_ClientValidate("GroupA")) {
                    BlockUI();
                }
                else {
                    return;
                }
                if (!processed)
                    $.unblockUI();
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
            //var supervisor = $('#HID_IS_SUPERVISOR').val("");
            //var returnValue = window.showModalDialog("../comm/Dept_Search.aspx?mode=all&super=" + supervisor + "&parentFuncId=" + parentFuncID, self, 'dialogWidth=1000px;dialogHeight=400px;scroll=no;addressbar:No;');
            //if (returnValue == undefined) {
            //    returnValue = window.returnValue;
            //}
            //if (!(typeof returnValue === 'undefined')) {

            //    var obj = jQuery.parseJSON(returnValue);
            //    $("#" + emp_id).val(obj.EMP_ID);
            //    //$("#" + emp_name).val(obj.EMP_NAME);
            //    doEmpAjax();
            //    return obj;

            //}
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
            if ($("#txt_NEW_EMP_ID").val().length == 5) {
                $.ajax({
                    url: "WFB2DD0100_GetEmpData.ashx",
                    data: {
                        EMP_ID: $('#txt_NEW_EMP_ID').val()
                    },
                    type: "GET",
                    dataType: 'json',
                    success: function (JData) {
                        if (JData.errMsg != "") {
                            $('#txt_NEW_EMP_ID').val("");
                            $('#lb_NEW_DEPT_NAME').text("");
                            $('#lb_NEW_EMP_NAME').text("");
                            $('#lb_NEW_PLANT_CD').text("");                          

                            alert(JData.errMsg);
                        }
                        else {
                            $('#txt_NEW_EMP_ID').val(JData.EMP_ID);
                            $('#lb_NEW_DEPT_NAME').text(JData.DEPT_NAME);
                            $('#lb_NEW_EMP_NAME').text(JData.EMP_NAME);
                            $('#lb_NEW_PLANT_CD').text(JData.PLANT_NAME);
                        }
                    },

                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
            } else {
                $('#txt_NEW_EMP_ID').val("");
                $('#lb_NEW_DEPT_NAME').text("");
                $('#lb_NEW_EMP_NAME').text("");
                $('#lb_NEW_PLANT_CD').text("");

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
				    <col width="30%" />
					<col width="10%" />
					<col width="15%" />
					<col width="15%" />
					<col width="20%" />
                </colgroup>
                <tbody>
                    <!-- START: 1st Line in Search Criteria area -->
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_EMP_ID%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" >
                            <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="42px" ClientIDMode="Static"></asp:TextBox>
                            <input id="bt_EMP_ID" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N');" />                            
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_EMP_NAME%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" ></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="ib_IS_CALCULATE" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_IS_CALCULATE%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:RadioButtonList ID="rb_IS_CALCULATE" runat="server" ClientIDMode="Static" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1">是</asp:ListItem>
                                <asp:ListItem Value="0">否</asp:ListItem>                        
                            </asp:RadioButtonList>                           
                        </td>
                    </tr>
                    <tr>
                         <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_DEPT_NO%>"></asp:Label>:
                         </th>
                         <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" Width="60px" ClientIDMode="Static"></asp:TextBox>
                                <input id="bt_DEPT_SEARCH" type="button" value="..." onclick="OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', 'N');" />
                            <asp:TextBox ID="txt_DEPT_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_PLANT_CD" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_PLANT_CD%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_PLANT_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_IS_CANCEL%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:RadioButtonList ID="rb_IS_CANCEL" runat="server" ClientIDMode="Static" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                <asp:ListItem Value="Y">是</asp:ListItem>
                                <asp:ListItem Value="N">否</asp:ListItem>                        
                            </asp:RadioButtonList>                           
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="Body_label" colspan="10">
                            <div id="init">
                                <aces:Btn ID="WFB2DD0100Search" runat="server" Text="<%$Resources:Resource,wfb2dd_WFB2DD0100Search%>" OnClick="WFB2DD0100Search_Click"/>

                                <%--<asp:Button ID="WFB2DD0100Search" runat="server" Text="<%$Resources:Resource,wfb2dd_WFB2DD0100Search%>" OnClick="WFB2DD0100Search_Click"/>--%>
                                
                                <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2dd_btn_clear%>" onclick="ClearAll();"/>                               
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" height="1" colspan="10">
                            <hr>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="10">
                        <aces:Btn ID="WFB2DD0100Add" runat="server" Text="<%$Resources:Resource,wfb2dd_WFB2DD0100Add%>" OnClick="WFB2DD0100Add_Click"  />
                            <aces:Btn ID="WFB2DD0100Edit" runat="server" Text="<%$Resources:Resource,wfb2dd_WFB2DD0100Edit%>" Visible="false" OnClick="WFB2DD0100Edit_Click" />
                            <aces:Btn ID="WFB2DD0100Detail" runat="server" Text="<%$Resources:Resource,wfb2dd_WFB2DD0100Detail%>"  Visible="false" OnClick="WFB2DD0100Detail_Click"/>
                            <aces:Btn ID="WFB2DD0100Save" runat="server" Text="<%$Resources:Resource,wfb2dd_WFB2DD0100Save%>" OnClientClick="return saveCheck();" Visible="false" OnClick="WFB2DD0100Save_Click"/>

                            <%--<asp:Button ID="WFB2DD0100Add" runat="server" Text="<%$Resources:Resource,wfb2dd_WFB2DD0100Add%>" OnClick="WFB2DD0100Add_Click"  />
                            <asp:Button ID="WFB2DD0100Edit" runat="server" Text="<%$Resources:Resource,wfb2dd_WFB2DD0100Edit%>" Visible="false" OnClick="WFB2DD0100Edit_Click" />
                            <asp:Button ID="WFB2DD0100Detail" runat="server" Text="<%$Resources:Resource,wfb2dd_WFB2DD0100Detail%>"  Visible="false" OnClick="WFB2DD0100Detail_Click"/>
                            <asp:Button ID="WFB2DD0100Save" runat="server" Text="<%$Resources:Resource,wfb2dd_WFB2DD0100Save%>" OnClientClick="return saveCheck();" Visible="false" OnClick="WFB2DD0100Save_Click"/>
                            --%>
                            <asp:Button ID="WFB2DD0100Cancel" runat="server" Text="<%$Resources:Resource,wfb2dd_WFB2DD0100Cancel%>" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="WFB2DD0100Cancel_Click" CausesValidation="false"/>
                            <asp:HiddenField  id="hid_Valid_Flag" runat="server" ClientIDMode="Static" value="" />
                        </td>
                    </tr>
                    <!-- END: Create a line -->
                </tbody>
            </table>
              <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DD0100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_EMP_ID" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="emp_id" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="txt_EMP_NAME" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="emp_name" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="rb_IS_CALCULATE" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="is_calculate" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="txt_DEPT_NO" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="dept_no" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="ddl_PLANT_CD" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="plant_cd" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="rb_IS_CANCEL" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="is_cancel" PropertyName="SelectedValue" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_RowNumber%>" HeaderStyle-Width="20px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dd_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_EMP_ID" runat="server" MaxLength="5" Width="40px" CssClass="MandatoryField AjaxEMPID" ClientIDMode="Static"></asp:TextBox>
                            <%--<input id="bt_EMP_ID" type="button" value="..." onclick="OpenEmpSearchDD010('txt_NEW_EMP_ID');" />--%>
                            <input id="bt_EMP_ID" type="button" value="..." onclick="OpenEmpSearchDD010('txt_NEW_EMP_ID','','','Y');" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dd_Required_EMP_ID%>"
                                ControlToValidate="txt_NEW_EMP_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2dd_EMP_ID_isError%>" ControlToValidate="txt_NEW_EMP_ID" ForeColor="Red"
                                ValidationExpression="^[0-9]+$" Display="None" ValidationGroup="GroupA">
                            </asp:RegularExpressionValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dd_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_NEW_EMP_NAME" runat="server" ClientIDMode="Static"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dd_lb_DEPT_NO%>" SortExpression="DEPT_NO" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_DEPT_NAME" runat="server" Text='<%#Bind("DEPT_NAME")%>' Style="word-wrap: break-word;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_DEPT_NAME" runat="server" Text='<%#Bind("DEPT_NAME")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_NEW_DEPT_NAME" runat="server" ClientIDMode="Static"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dd_lb_PLANT_CD%>" SortExpression="PLANT_CD" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_PLANT_CD" runat="server" Text='<%#Bind("PLANT_CD")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_PLANT_CD" runat="server" MaxLength="10" Width="81px" BorderWidth="0" ReadOnly="true" Text='<%#Bind("PLANT_CD")%>'></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_NEW_PLANT_CD" runat="server" Text='<%#Bind("PLANT_CD")%>'></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dd_lb_IS_CANCEL%>" SortExpression="IS_CANCEL" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_IS_CANCEL" runat="server" Text='<%#Bind("IS_CANCEL")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>                            
                            <div style="text-align: center; width: 100%">
                                <asp:DropDownList ID="ddl_EDIT_IS_CANCEL" runat="server" MaxLength="1" ClientIDMode="Static">
                                    <asp:ListItem Value="Y" Text="<%$Resources:Resource,wfb2dd_IS_CANCEL_Y%>" ></asp:ListItem>
                                    <asp:ListItem Value="N" Text="<%$Resources:Resource,wfb2dd_IS_CANCEL_N%>" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:HiddenField  id="hid_IS_CANCEL" runat="server" ClientIDMode="Static" value='<%#Bind("IS_CANCEL")%>' />
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>                           
                            <div style="text-align: center; width: 100%">
                                <asp:DropDownList ID="ddl_NEW_IS_CANCEL" runat="server" MaxLength="1" ClientIDMode="Static">
                                    <asp:ListItem Value="Y" Text="<%$Resources:Resource,wfb2dd_IS_CANCEL_Y%>" ></asp:ListItem>
                                    <asp:ListItem Value="N" Text="<%$Resources:Resource,wfb2dd_IS_CANCEL_N%>" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dd_lb_IS_CALCULATE%>" SortExpression="IS_CALCULATE" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_IS_CALCULATE" runat="server" Text='<%#Bind("IS_CALCULATE")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>                            
                            <div style="text-align: center; width: 100%">
                                <asp:DropDownList ID="ddl_EDIT_IS_CALCULATE" runat="server" MaxLength="1" ClientIDMode="Static">
                                    <asp:ListItem Value="1" Text="<%$Resources:Resource,wfb2dd_IS_CALCULATE_Y%>" ></asp:ListItem>
                                    <asp:ListItem Value="0" Text="<%$Resources:Resource,wfb2dd_IS_CALCULATE_N%>" ></asp:ListItem>
                                </asp:DropDownList>
                                <asp:HiddenField  id="hid_IS_CALCULATE" runat="server" ClientIDMode="Static" value='<%#Bind("IS_CALCULATE")%>' />
                            </div>                          
                        </EditItemTemplate>
                        <FooterTemplate>                            
                            <div style="text-align: center; width: 100%">
                                <asp:DropDownList ID="ddl_NEW_IS_CALCULATE" runat="server" MaxLength="1" ClientIDMode="Static">
                                    <asp:ListItem Value="1" Text="<%$Resources:Resource,wfb2dd_IS_CALCULATE_Y%>" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="<%$Resources:Resource,wfb2dd_IS_CALCULATE_N%>" ></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>                   
                </Columns>
                <EmptyDataTemplate>
                    <table class="grid-view" width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                        <tr class="header">
                            <td></td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2dd_RowNumber%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_EMP_ID%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_EMP_NAME%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_DEPT_NO%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_PLANT_CD%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_IS_CANCEL%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_IS_CALCULATE%>"></asp:Label>
                            </td>                           
                        </tr>
                        <tr class="normal">
                            <td></td>
                            <td></td>
                            <td>
                                <asp:TextBox ID="txt_NEW_EMP_ID" runat="server" MaxLength="5" Width="40px" CssClass="MandatoryField AjaxEMPID" ClientIDMode="Static"></asp:TextBox>
                            <input id="bt_EMP_ID" type="button" value="..." onclick="OpenEmpSearchDD010('txt_NEW_EMP_ID');" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dd_Required_EMP_ID%>"
                                ControlToValidate="txt_NEW_EMP_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2dd_EMP_ID_isError%>" ControlToValidate="txt_NEW_EMP_ID" ForeColor="Red"
                                ValidationExpression="^[0-9]+$" Display="None" ValidationGroup="GroupA">
                            </asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:Label ID="lb_NEW_EMP_NAME" runat="server" ClientIDMode="Static"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lb_NEW_DEPT_NAME" runat="server" ClientIDMode="Static"></asp:Label>
                            </td>
                            <td>
                                 <asp:Label ID="lb_NEW_PLANT_CD" runat="server" Text='<%#Bind("PLANT_CD")%>'></asp:Label>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                <asp:DropDownList ID="ddl_NEW_IS_CANCEL" runat="server" MaxLength="1" ClientIDMode="Static">
                                    <asp:ListItem Value="Y" Text="<%$Resources:Resource,wfb2dd_IS_CANCEL_Y%>" ></asp:ListItem>
                                    <asp:ListItem Value="N" Text="<%$Resources:Resource,wfb2dd_IS_CANCEL_N%>" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </div>                              
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                <asp:DropDownList ID="ddl_NEW_IS_CALCULATE" runat="server" MaxLength="1" ClientIDMode="Static">
                                    <asp:ListItem Value="1" Text="<%$Resources:Resource,wfb2dd_IS_CALCULATE_Y%>" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="<%$Resources:Resource,wfb2dd_IS_CALCULATE_N%>" ></asp:ListItem>
                                </asp:DropDownList>
                            </div>                             
                            </td>
                        </tr>
                    </table>

                </EmptyDataTemplate>
                <HeaderStyle CssClass="GridviewScrollHeader" />
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle HorizontalAlign="Center" />
                <EditRowStyle HorizontalAlign="Center" />
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
            <asp:HiddenField ID="HID_ISADD" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>