<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dj/WFB2DJ0200_Qry.aspx.cs" Inherits="WebContent_WFB2DJ0200_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">

        $(function () {

            iniForm();
        });
        function iniForm() {
            //MandatoryField date
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.date').mask('9999/99/99');
            $(".maxHour").mask('999');
            $(".decimal").css("text-align", "right").css("ime-mode", "disabled");
            $(".txtDisabled").css("background-color", "white").css("color", "black").css("border-width", "0");
            $('.date').css("ime-mode", "disabled");
            gridviewScroll();
            $.unblockUI();

            //寫在這，按查詢才不會消失
            $('#txt_DEPT_NAME_SHOW').attr("readonly", true);
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
                        cache: false,
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_DEPT_NAME_SHOW').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_DEPT_NAME_SHOW').val(JData.DEPT_NAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_DEPT_NAME_SHOW').val("");
                }
            });

            $("#txt_NEW_DEPT_NO").change(function () {
                if ($("#txt_NEW_DEPT_NO").val().length == 7) {
                    $.ajax({
                        url: "../commgeo/WFB2GetDeptData.ashx",
                        data: {
                            DEPT_NO: $('#txt_NEW_DEPT_NO').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        cache: false,
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_NEW_DEPT_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_NEW_DEPT_NAME').val(JData.DEPT_NAME_20 + " " + JData.DEPT_NAME_40);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_NEW_DEPT_NAME').val("");
                }
            });

        }

        function OpenDeptSearchDJ020(dept_no, dept_name) {
            //var supervisor = "Y";
            OpenDeptSearch(dept_no, dept_name, 'Y', 'Y');
            //var returnValue = window.showModalDialog("../comm/Dept_Search.aspx?mode=dept&super=" + supervisor + "&parentFuncId=" + parentFuncID, self, 'dialogWidth=200px;dialogHeight=400px;scroll=no;addressbar:No;');
            //if (returnValue == undefined) {
            //    returnValue = window.returnValue;
            //}

            //if (!(typeof returnValue === 'undefined')) {
            //    var obj = jQuery.parseJSON(returnValue);
            //    $("#" + dept_no).val(obj.DEPT_NO);
            //    $("#" + dept_name).val(obj.DEPT_NAME_20 + "" + obj.DEPT_NAME_40);
            //    //return obj;
            //}
        }
        function returnDEPTValueToPage(dept_no, dept_name, value) {
            var returnValue = value;
            if (value == undefined) {
                returnValue = 'undefined';
            }
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                $("#" + dept_no).val(obj.DEPT_NO);
                $("#" + dept_name).val(obj.DEPT_NAME_20 + "" + obj.DEPT_NAME_40);
                //return obj;
            }
        }
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());

        }

        //凍結視窗用
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "400",
                barcolor: "#7F7F7F"
            });
        }

        //刪除,檢查是否有勾選
        function doDelete() {
            var processed = true;
            BlockUI();

            if (checkboxsSelected() > 0) {
                processed = confirm("確定要刪除?");
            } else {
                alert("請選取資料!");
                processed = false;
            }

            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }



        var choietr = null;
        //回傳目前Checkbox被勾選的數量
        function checkboxsSelected() {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                    if (choietr == null)
                        choietr = i;
                }
            }
            return HaveCheck;
        }

        //儲存前檢查
        function saveCheck() {
            var processed = true;

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else {
                processed = false;
            }
            if (!processed) {
                $.unblockUI();
                return;
            }

            return processed;
        }

        //清空畫面
        function ClearAll() {
            $("#txt_DEPT_NAME").val("");
            $("#txt_LAYOUT_NO").val("");
            $("#txt_DEPT_NO").val("");
            $("#txt_DEPT_NAME_SHOW").val("");
            $("#ddl_ENV_ALLOWANCE_TYPE").val("-1");
            $('#<%=rbl_USE_STATUS.ClientID %>').find("input[value=Y]").prop("checked", "checked");
        }

    </script>
    <style type="text/css">
        .auto-style1 {
            height: 23px;
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="15%" />
                    <col width="35%" />
                    <col width="15%" />
                    <col width="35%" />

                </colgroup>
                <tbody>
                    <tr>

                        <%--部門代號 --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="Label11" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_dept_no%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_DEPT_NO" runat="server" Width="70px" ClientIDMode="Static" MaxLength="7"></asp:TextBox>
                            <input id="bt_DEPT_SEARCH" type="button" value="..." onclick="OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME_SHOW', 'N', '');" />
                            <asp:TextBox ID="txt_DEPT_NAME_SHOW" runat="server" Width="160px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                        </td>

                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="Label10" runat="server" Text="Layout NO."></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_LAYOUT_NO" runat="server" Width="100px" ClientIDMode="Static"></asp:TextBox>

                        </td>
                    </tr>
                    <tr>
                        <%--部門名稱 --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_dept_name%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_DEPT_NAME" runat="server" Width="200px" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <%--環境津貼等級: --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_env_allowance_type%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" colspan="4">
                            <asp:DropDownList ID="ddl_ENV_ALLOWANCE_TYPE" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_STATUS" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_status%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:RadioButtonList ID="rbl_USE_STATUS" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" ClientIDMode="Static">
                                <asp:ListItem Text="<%$Resources:Resource,wfb2dj_lb_isvalid_true%>" Value="Y" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:Resource,wfb2dj_lb_isvalid_false%>" Value="N"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:Resource,wfb2dj_lb_isvalid_all%>" Value="ALL"></asp:ListItem>
                            </asp:RadioButtonList>
                            <%--直別: --%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="Label14" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_work_shift_name%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_WORK_SHIFT_NAME" runat="server" Width="100px" ClientIDMode="Static"></asp:TextBox>

                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="4">
                            <aces:Btn ID="WFB2DJ0200Search" runat="server" Text="<%$Resources:Resource,wfb2dj_btn_search%>" OnClick="WFB2DJ0200Search_Click" OnClientClick=" BlockUI();" />

                            <%--<asp:Button ID="WFB2DJ0200Search" runat="server" Text="<%$Resources:Resource,wfb2dj_btn_search%>" OnClick="WFB2DJ0200Search_Click" OnClientClick=" BlockUI();" />--%>

                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2dj_btn_clear%>" onclick="ClearAll();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="Body_label" colspan="4">
                            <div id="init_grid">

                                <aces:Btn ID="WFB2DJ0200Add" runat="server" Text="<%$Resources:Resource,wfb2dj_btn_add%>" Visible="true" OnClick="WFB2DJ0200Add_Click" />
                                <aces:Btn ID="WFB2DJ0200Delete" runat="server" Text="<%$Resources:Resource,wfb2dj_btn_delete%>" Visible="false" OnClick="WFB2DJ0200Delete_Click" OnClientClick="return doDelete();" />
                                <aces:Btn ID="WFB2DJ0200Edit" runat="server" Text="<%$Resources:Resource,wfb2dj_btn_edit%>" Visible="false" OnClick="WFB2DJ0200Edit_Click" OnClientClick="BlockUI();" />
                                <aces:Btn ID="WFB2DJ0200Save" runat="server" Text="<%$Resources:Resource,wfb2dj_btn_save%>" Visible="false" ValidationGroup="GroupA" OnClick="WFB2DJ0200Save_Click" />
                                <%-- <asp:Button ID="WFB2DJ0200Add" runat="server" Text="<%$Resources:Resource,wfb2dj_btn_add%>" Visible="true" OnClick="WFB2DJ0200Add_Click" />
                                <asp:Button ID="WFB2DJ0200Delete" runat="server" Text="<%$Resources:Resource,wfb2dj_btn_delete%>" Visible="false" OnClick="WFB2DJ0200Delete_Click" OnClientClick="return doDelete();" />
                                <asp:Button ID="WFB2DJ0200Edit" runat="server" Text="<%$Resources:Resource,wfb2dj_btn_edit%>" Visible="false" OnClick="WFB2DJ0200Edit_Click" OnClientClick="BlockUI();" />
                                <asp:Button ID="WFB2DJ0200Save" runat="server" Text="<%$Resources:Resource,wfb2dj_btn_save%>" Visible="false" ValidationGroup="GroupA" OnClick="WFB2DJ0200Save_Click" />--%>

                                <asp:Button ID="btn_cancel" runat="server" Text="<%$Resources:Resource,wfb2dj_btn_cancel%>" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="btn_cancel_Click" />
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DJ0200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="hid_qry_DEPT_NAME"
                        Name="dept_name" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_qry_LAYOUT_NO"
                        Name="layout_no" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_DEPT_NO"
                        Name="dept_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_qry_ENV_ALLOWANCE_TYPE"
                        Name="env_type" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_qry_USE_STATUS" DefaultValue=""
                        Name="is_valid" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                     <asp:ControlParameter ControlID="txt_WORK_SHIFT_NAME"
                        Name="work_shift_name" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" Width="20px" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" Width="20px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_rownumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--部門名稱--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_dept_name%>" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" SortExpression="DEPT_NAME">
                        <ItemTemplate>
                            <asp:Label ID="lb_DEPT_NAME" runat="server" Text='<%#Bind("DEPT_NAME")%>' Width="200px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_DEPT_NAME" runat="server" Text='<%#Bind("DEPT_NAME")%>' Width="200px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_DEPT_NAME" runat="server" ClientIDMode="Static" Width="200px" CssClass="txtDisabled" Enabled="false"></asp:TextBox>

                            <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dj_required_dept_name%>"
                                ControlToValidate="txt_NEW_DEPT_NAME" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--部門代號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_dept_no%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="DEPT_NO">
                        <ItemTemplate>
                            <asp:Label ID="lb_DEPT_NO" runat="server" Text='<%#Bind("DEPT_NO")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_DEPT_NO" runat="server" Text='<%#Bind("DEPT_NO")%>' Width="100px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_DEPT_NO" runat="server" MaxLength="7" Width="100px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                            <input id="Button15" type="button" value="..." onclick="OpenDeptSearchDJ020('txt_NEW_DEPT_NO', 'txt_NEW_DEPT_NAME');" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dj_required_dept_no%>"
                                ControlToValidate="txt_NEW_DEPT_NO" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="empty10" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2dj_format_dept_no%>" ControlToValidate="txt_NEW_DEPT_NO" ForeColor="Red" ValidationGroup="GroupA"
                                ValidationExpression=".{7,7}" Display="None"></asp:RegularExpressionValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--直 別--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_work_shift_name%>" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" SortExpression="WORK_SHIFT_NAME">
                        <ItemTemplate>
                            <asp:Label ID="lb_WORK_SHIFT_NAME" runat="server" Text='<%#Bind("WORK_SHIFT_NAME")%>' Width="50px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_WORK_SHIFT_NAME" runat="server" MaxLength="20" Text='<%#Bind("WORK_SHIFT_NAME")%>' ClientIDMode="Static" Width="50px" CssClass=" MandatoryField"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dj_required_work_shift_name%>"
                                ControlToValidate="txt_EDIT_WORK_SHIFT_NAME" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_WORK_SHIFT_NAME" runat="server" MaxLength="20" ClientIDMode="Static" Width="50px" CssClass=" MandatoryField"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dj_required_work_shift_name%>"
                                ControlToValidate="txt_NEW_WORK_SHIFT_NAME" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--Layout NO. --%>
                    <asp:TemplateField HeaderText="Layout NO." HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" SortExpression="LAYOUT_NO">
                        <ItemTemplate>
                            <asp:Label ID="lb_LAYOUT_NO" runat="server" Text='<%#Bind("LAYOUT_NO")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_LAYOUT_NO" runat="server" Text='<%#Bind("LAYOUT_NO")%>' Width="80px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_LAYOUT_NO" runat="server" MaxLength="3" ClientIDMode="Static" Width="80px" CssClass="MandatoryField maxHour decimal"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dj_required_layout_no%>"
                                ControlToValidate="txt_NEW_LAYOUT_NO" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>

                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--環境津貼等級 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_env_allowance_type%>" HeaderStyle-Width="60px" SortExpression="ENV_ALLOWANCE_TYPE" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_ENV_ALLOWANCE_TYPE" runat="server" Text='<%#Bind("ENV_ALLOWANCE_TYPE")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_ENV_ALLOWANCE_TYPE" runat="server" Text='<%#Bind("ENV_ALLOWANCE_TYPE")%>' Width="60px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_NEW_ENV_ALLOWANCE_TYPE" runat="server" CssClass="MandatoryField"></asp:DropDownList>
                            <%--<asp:HiddenField ID="hid_NEW_ENV_ALLOWANCE_TYPE" runat="server" Value='<%#Bind("ENV_ALLOWANCE_TYPE")%>' />--%>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="津貼等級不可空白>"
                                ControlToValidate="ddl_NEW_ENV_ALLOWANCE_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>--%>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--申請人日上限 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_env_max_hour%>" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Right" SortExpression="ENV_MAX_HOUR">
                        <ItemTemplate>
                            <asp:Label ID="lb_ENV_MAX_HOUR" runat="server" Text='<%#Bind("ENV_MAX_HOUR")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_ENV_MAX_HOUR" runat="server" Text='<%#Bind("ENV_MAX_HOUR")%>' ClientIDMode="Static" Width="60px" CssClass="MandatoryField maxHour decimal"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dj_required_env_max_hour%>"
                                ControlToValidate="txt_EDIT_ENV_MAX_HOUR" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_ENV_MAX_HOUR" runat="server" ClientIDMode="Static" Width="60px" CssClass="MandatoryField maxHour decimal"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dj_required_env_max_hour%>"
                                ControlToValidate="txt_NEW_ENV_MAX_HOUR" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--生效日期 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_start_dt%>" HeaderStyle-Width="100px" SortExpression="START_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_START_DT" runat="server" Text='<%#Bind("START_DT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_START_DT" runat="server" Text='<%#Bind("START_DT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_NEW_START_DT" runat="server" MaxLength="10" Width="100px" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>
                                <!--驗證不可空白 -->
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dj_required_start_dt%>"
                                    ControlToValidate="txt_NEW_START_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <!--驗證日期格式-->
                                <asp:CustomValidator ID="RegularExpressionValidator12" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2dj_format_start_dt%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_NEW_START_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--結束日期 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_end_dt%>" HeaderStyle-Width="100px" SortExpression="END_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_END_DT" runat="server" Text='<%#Bind("END_DT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                            <asp:HiddenField ID="hid_END_DT" runat="server" ClientIDMode="Static" Value='<%#Bind("END_DT", "{0:yyyy/MM/dd}")%>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_EDIT_END_DT" runat="server" Text='<%#Bind("END_DT", "{0:yyyy/MM/dd}")%>' MaxLength="10" Width="100px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                <!--驗證日期格式-->
                                <asp:CustomValidator ID="RegularExpressionValidator13" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2dj_format_end_dt%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_EDIT_END_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_NEW_END_DT" runat="server" MaxLength="10" Width="100px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                <!--驗證不可空白 -->
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2si_required_BONUS_DT%>"
                                    ControlToValidate="txt_NEW_BONUS_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>--%>
                                <!--驗證日期格式-->
                                <asp:CustomValidator ID="RegularExpressionValidator14" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2dj_format_end_dt%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_NEW_END_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                <!--起日不可大於迄日 -->
                                <asp:CompareValidator ID="qry5" runat="server" ControlToCompare="txt_NEW_START_DT"
                                    ControlToValidate="txt_NEW_END_DT" ErrorMessage="<%$Resources:Resource,wfb2dj_error_start_dt_s_e%>" Type="Date" Operator="GreaterThanEqual"
                                    Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--備註 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_remark%>" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="left">
                        <ItemTemplate>
                            <asp:Label ID="lb_REMARK" runat="server" Text='<%#Bind("REMARK")%>' Width="200px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_REMARK" runat="server" Text='<%#Bind("REMARK")%>' ClientIDMode="Static" MaxLength="210" Width="200px"></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_REMARK" runat="server" ClientIDMode="Static" MaxLength="210" Width="200px"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
                <%--當DB無資料時，就會使用此table --%>
                <EmptyDataTemplate>
                    <table class="grid-view" width="1020px">
                        <tr class="header">
                            <td>
                                <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" Width="20px" />
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_rownumber%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_dept_name%>" Width="200px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label13" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_dept_no%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_work_shift_name%>" Width="50px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="Layout NO." Width="80px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_env_allowance_type%>" Width="60px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_env_max_hour%>" Width="60px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_start_dt%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_end_dt%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label12" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_remark%>" Width="200px"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td></td>
                            <td></td>
                            <td>
                                <asp:TextBox ID="txt_NEW_DEPT_NAME" runat="server" ClientIDMode="Static" Width="140px" CssClass="" Enabled="false"></asp:TextBox>

                                <asp:RequiredFieldValidator ID="empty1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dj_required_dept_name%>"
                                    ControlToValidate="txt_NEW_DEPT_NAME" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_DEPT_NO" runat="server" MaxLength="7" Width="100px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                <input id="Button15" type="button" value="..." onclick="OpenDeptSearch('txt_NEW_DEPT_NO', 'txt_NEW_DEPT_NAME', 'N', '');" />
                                <asp:RequiredFieldValidator ID="empty2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dj_required_dept_no%>"
                                    ControlToValidate="txt_NEW_DEPT_NO" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="empty10" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2dj_format_dept_no%>" ControlToValidate="txt_NEW_DEPT_NO" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression=".{7,7}" Display="None"></asp:RegularExpressionValidator>

                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_WORK_SHIFT_NAME" runat="server" MaxLength="20" ClientIDMode="Static" Width="50px" CssClass=" MandatoryField"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="empty3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dj_required_work_shift_name%>"
                                    ControlToValidate="txt_NEW_WORK_SHIFT_NAME" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_LAYOUT_NO" runat="server" MaxLength="3" ClientIDMode="Static" Width="80px" CssClass="MandatoryField"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="empty4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dj_required_layout_no%>"
                                    ControlToValidate="txt_NEW_LAYOUT_NO" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_NEW_ENV_ALLOWANCE_TYPE" runat="server" CssClass="MandatoryField"></asp:DropDownList>
                                <%--                                <asp:RequiredFieldValidator ID="empty5" runat="server" ErrorMessage="津貼等級不可空白>"
                                    ControlToValidate="ddl_NEW_ENV_ALLOWANCE_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>--%>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_ENV_MAX_HOUR" runat="server" ClientIDMode="Static" Width="80px" CssClass="MandatoryField maxHour decimal"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="empty6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dj_required_env_max_hour%>"
                                    ControlToValidate="txt_NEW_ENV_MAX_HOUR" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <div style="text-align: left; width: 100%">
                                    <asp:TextBox ID="txt_NEW_START_DT" runat="server" MaxLength="10" Width="100px" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>
                                    <!--驗證不可空白 -->
                                    <asp:RequiredFieldValidator ID="empty7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dj_required_start_dt%>"
                                        ControlToValidate="txt_NEW_START_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    <!--驗證日期格式-->
                                    <asp:CustomValidator ID="empty8" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="<%$Resources:Resource,wfb2dj_required_start_dt%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                        ControlToValidate="txt_NEW_START_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                </div>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_END_DT" runat="server" MaxLength="10" Width="100px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                <!--驗證日期格式-->
                                <asp:CustomValidator ID="empty9" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2dj_format_end_dt%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_NEW_END_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                <!--起日不可大於迄日 -->
                                <asp:CompareValidator ID="qry5" runat="server" ControlToCompare="txt_NEW_START_DT"
                                    ControlToValidate="txt_NEW_END_DT" ErrorMessage="<%$Resources:Resource,wfb2dj_error_start_dt_s_e%>" Type="Date" Operator="GreaterThanEqual"
                                    Display="None" ValidationGroup="GroupA"></asp:CompareValidator>

                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_REMARK" runat="server" ClientIDMode="Static" MaxLength="210" Width="200px"></asp:TextBox>
                            </td>
                        </tr>

                    </table>
                </EmptyDataTemplate>
                <HeaderStyle CssClass="GridviewScrollHeader" />
                <PagerStyle CssClass="GridviewScrollPager" />

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

            <!-- 查詢條件 -->
            <asp:HiddenField ID="hid_qry_DEPT_NAME" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_lb_DEPT_NAME" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_qry_LAYOUT_NO" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_qry_DEPT_NO" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_qry_ENV_ALLOWANCE_TYPE" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_qry_USE_STATUS" runat="server" ClientIDMode="Static" />
            <!-- 每頁的筆數 -->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <!-- 是否要凍結視窗 -->
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="N" />
            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
