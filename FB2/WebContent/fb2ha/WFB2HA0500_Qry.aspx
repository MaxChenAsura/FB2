<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2ha/WFB2HA0500_Qry.aspx.cs" Inherits="WebContent_fb2ha_WFB2HA0500_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".ymd").mask("9999/99/99");
            $(".number6").mask("999999");
            $(".number6").css("text-align", "right").css("ime-mode", "disabled");
            $(".number2").mask("999");
            $(".number2").css("text-align", "right").css("ime-mode", "disabled");
            $(".EnNum4").mask("AAAA");
            $(".EnNum4").css("ime-mode", "disabled");
            gridviewScroll();
            $.unblockUI();

            $("#lb_PJOB_DESC").attr("readonly", true);
            $('#txt_PJOB_CD').change(function () {

                if ($('#txt_PJOB_CD').val().length == 4) {

                    //ajax 取得員工基本資料
                    $.ajax({
                        url: "WFB2HA0500_PJOB.ashx",
                        data: {
                            PJOB_CD: $('#txt_PJOB_CD').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        cache: false,
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $("#txt_PJOB_CD").val("");
                                $("#lb_PJOB_DESC").val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $("#lb_PJOB_DESC").val(JData.PJOB_DESC);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $("#lb_PJOB_DESC").val("");
                }
            });


        }
        function getPjob() {
            var now = new Date();
            var date = now.format("yyyy/MM/dd");
            var PJOB_CD = $("#txt_PJOB_CD").val();

            OpenSearch('Pjob_Search.aspx', 'txt_PJOB_CD', 'hid_PJOB_DESC', 'PJOB_CD=' + PJOB_CD + '&START_DT=' + date, 'Y');
            //var obj = OpenSearch('Pjob_Search.aspx', 'txt_PJOB_CD', 'hid_PJOB_DESC', 'PJOB_CD=' + PJOB_CD + '&START_DT=' + date,'Y');
            //if (obj != undefined)
            //    $("#lb_PJOB_DESC").val(obj.DESC);

        }
        function Pjob_Search(obj_cd, obj_desc, value) {
            var returnValue = value;
            if (returnValue == undefined) {
                returnValue = window.returnValue;
            }
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                $('#' + obj_cd).val(obj.CD);
                $('#' + obj_desc).val(obj.DESC);
                $("#lb_PJOB_DESC").val(obj.DESC);
            }
        }

        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());

        }
        function gridviewScroll() {
            if ($("#HID_Freeze").val() == "Y") {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "400",
                    barcolor: "#7F7F7F",
                    freezesize: 6

                });
                CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
            }
            else {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "400",
                    barcolor: "#7F7F7F"

                });
            }
        }

        //清空畫面
        function ClearAll() {
            $("#txt_PJOB_CD").val("");
            $("#lb_PJOB_DESC").val("");
            $("#ddl_LEVEL_CD").val("-1");
            $("#ddl_WS_CD").val("-1");
            $("#ddl_PJOB_AGE_LIMIT").val("-1");
            $("#ddl_PJOB_LEVEL").val("-1");
            $("#ddl_BUSINESS_TRIP_GRP").val("-1");
            $("#txt_START_DT_S").val("");
            $("#txt_START_DT_E").val("");
            $("#txt_END_DT_S").val("");
            $("#txt_END_DT_E").val("");
            $('#rbl_IS_VALID').find("input[value='Y']").prop("checked", "checked");
        }

        function CheckSearch() {

            //if ($("#txt_START_DT_S").val() == "" && $("#txt_START_DT_E").val() == "") {
            //    if ($("#rbl_IS_VALID input:radio:checked").val() != "ALL") {
            //        alert("未輸入生效期間，不可以點選「有效」、「無效」");
            //        return false;
            //    }
            //}

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
        }
        //儲存前檢查
        function saveCheck() {
            var processed = true;
            var msg = "";
            if (Page_ClientValidate("GroupA")) {
                if ($("#HID_PJOB_CD").val() == "M") {
                    if ($("#txt_NEW_PROFESSION_ALLOWANCE").val() != "" && $("#txt_NEW_PROFESSION_ALLOWANCE").val() != 0) {
                        msg = "職務代號第一碼若為'M'，不可輸入專業津貼";
                        processed = false;
                    }
                }
                if ($("#HID_PJOB_CD").val() == "P") {
                    if ($("#txt_NEW_MANAGEMENT_ALLOWANCE").val() != "" && $("#txt_NEW_MANAGEMENT_ALLOWANCE").val() != 0) {
                        msg = "職務代號第一碼若為'P'，不可輸入職務津貼";
                        processed = false;
                    }
                }

                if (!processed) {
                    $.unblockUI();
                    alert(msg);
                    return false;
                }
                else {
                    BlockUI();
                    return true;
                }
            }
        }

        function setHidPjobCD(obj) {
            var value = $("#" + obj.id).val();
            $("#HID_PJOB_CD").val(value.substring(0, 1));
        }

        function setSearchColumn() {
        }

        //刪除,檢查是否有勾選
        function doDelete() {
            if (checkboxsSelected() > 0) {
                return confirm($('#hidwfb299_Del_ConfirmMessage').val());
            } else {
                alert($('#hidwfb299_Del_NotChoiceMessage').val());
                return false;
            }
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
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="10%" />
                    <col width="18%" />
                    <col width="15%" />
                    <col width="18%" />
                    <col width="10%" />
                    <col width="10%" />
                </colgroup>
                <tbody>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_PJOB_CD" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_PJOB_CD%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" >
                            <asp:TextBox ID="txt_PJOB_CD" runat="server" MaxLength="4" Width="60px" ClientIDMode="Static"></asp:TextBox>
                            <input id="btn_PJOB_CD" type="button" value="..." onclick="getPjob();" />
                            <asp:TextBox ID="lb_PJOB_DESC" runat="server" size="13" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_LEVEL_CD" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_LEVEL_CD%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" >
                            <asp:DropDownList ID="ddl_LEVEL_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_WS_CD" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_WS_CD%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_WS_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>

                    </tr>

                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_PJOB_LEVEL" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_PJOB_LEVEL%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" >
                            <asp:DropDownList ID="ddl_PJOB_LEVEL" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>

                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_BUSINESS_TRIP_GRP" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_BUSINESS_TRIP_GRP%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" >
                            <asp:DropDownList ID="ddl_BUSINESS_TRIP_GRP" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_PJOB_AGE_LIMIT" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_PJOB_AGE_LIMIT%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_PJOB_AGE_LIMIT" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>

                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_START_DT" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_start_dt%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" >
                            <asp:TextBox ID="txt_START_DT_S" runat="server" MaxLength="10" Width="70px" ClientIDMode="Static" CssClass="ymd date"></asp:TextBox>
                            ~
                            <asp:TextBox ID="txt_START_DT_E" runat="server" MaxLength="10" Width="70px" ClientIDMode="Static" CssClass="ymd date"></asp:TextBox>

                            <asp:CustomValidator ID="cv_START_DT_S" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_START_SDT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_START_DT_S" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="cv_START_DT_E" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_START_EDT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_START_DT_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_START_DT_S"
                                ControlToValidate="txt_START_DT_E" ErrorMessage="<%$Resources:Resource,wfb2ha_CP_START_DT%>" Type="Date" Operator="GreaterThanEqual"
                                Display="None" ValidationGroup="GroupA"></asp:CompareValidator>

                        </td>
                        <%--結束日期--%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_end_dt%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label"  >
                            <asp:TextBox ID="txt_END_DT_S" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="ymd date"></asp:TextBox>
                            ~
                                <asp:TextBox ID="txt_END_DT_E" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="ymd date"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_END_SDT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_END_DT_S" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_END_EDT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_END_DT_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToCompare="txt_END_DT_S"
                                ControlToValidate="txt_END_DT_E" ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_EFFECT_END_DT_RANGE3%>" Type="Date" Operator="GreaterThanEqual"
                                Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                        </td>
                        <td align="left" class="Body_label" colspan="2" >
                            <asp:RadioButtonList ID="rbl_IS_VALID" runat="server" RepeatDirection="Horizontal" ClientIDMode="Static">
                                <asp:ListItem Text="<%$Resources:Resource,wfb2sa_rbl_isvalid_y%>" Value="Y" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:Resource,wfb2sa_rbl_isvalid_n%>" Value="N"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:Resource,wfb2sa_rbl_isvalid_a%>" Value="ALL"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="Body_label" colspan="6">
                            <div id="init">
                                <aces:Btn ID="WFB2HA0500Search" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0500Search%>" OnClick="WFB2HA0500Search_Click" OnClientClick="return CheckSearch();" />
                                <%--<asp:Button ID="WFB2HA0500Search" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0500Search%>" OnClick="WFB2HA0500Search_Click" OnClientClick="return CheckSearch();" />--%>

                                <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2ha_btn_clear%>" onclick="ClearAll();" />
                            </div>
                        </td>
                    </tr>

                    <tr>
                        <td align="center" height="1" colspan="6">
                            <hr>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="6">

                            <aces:Btn ID="WFB2HA0500Add" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0500Add%>" OnClick="WFB2HA0500Add_Click" />
                            <aces:Btn ID="WFB2HA0500Delete" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0500Delete%>" OnClientClick="return doDelete();" OnClick="WFB2HA0500Delete_Click" Visible="false" />
                            <aces:Btn ID="WFB2HA0500Edit" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0500Edit%>" OnClick="WFB2HA0500Edit_Click" Visible="false" />
                            <aces:Btn ID="WFB2HA0500Save" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0500Save%>" Visible="false" OnClick="WFB2HA0500Save_Click" OnClientClick="return saveCheck();" />

                            <%-- <asp:Button ID="WFB2HA0500Add" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0500Add%>" OnClick="WFB2HA0500Add_Click" />
                            <asp:Button ID="WFB2HA0500Delete" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0500Delete%>" OnClientClick="return doDelete();" OnClick="WFB2HA0500Delete_Click" Visible="false" />
                            <asp:Button ID="WFB2HA0500Edit" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0500Edit%>" OnClick="WFB2HA0500Edit_Click" Visible="false" />
                            <asp:Button ID="WFB2HA0500Save" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0500Save%>" Visible="false" OnClick="WFB2HA0500Save_Click" OnClientClick="return saveCheck();" />--%>
                            <asp:Button ID="WFB2HA0500Cancel" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0500Cancel%>" Visible="false" OnClick="WFB2HA0500Cancel_Click" OnClientClick="return confirm($('#hidwfb299_Cancel_ConfirmMessage').val())" />
                        </td>
                    </tr>
                </tbody>
            </table>

            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="10%" />
                    <col width="30%" />
                    <col width="39%" />
                    <col width="21%" />
                </colgroup>
                <tbody>
                </tbody>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2HA0500DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="HID_search_PJOB_CD" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="pjob_cd" PropertyName="Value" Type="String" />
                    <asp:ControlParameter ControlID="HID_LEVEL_CD" DefaultValue="" ConvertEmptyStringToNull="False"
                        Name="level_cd" PropertyName="Value" Type="String" />
                    <asp:ControlParameter ControlID="HID_WS_CD" DefaultValue="" ConvertEmptyStringToNull="False"
                        Name="ws_cd" PropertyName="Value" Type="String" />
                    <asp:ControlParameter ControlID="HID_PJOB_AGE_LIMIT" DefaultValue="" ConvertEmptyStringToNull="False"
                        Name="pjob_age_limit" PropertyName="Value" Type="String" />
                    <asp:ControlParameter ControlID="HID_PJOB_LEVEL" DefaultValue="" ConvertEmptyStringToNull="False"
                        Name="pjob_level" PropertyName="Value" Type="String" />
                    <asp:ControlParameter ControlID="HID_BUSINESS_TRIP_GRP" DefaultValue="" ConvertEmptyStringToNull="False"
                        Name="business_trip_grp" PropertyName="Value" Type="String" />
                    <asp:ControlParameter ControlID="HID_START_DT_S" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="start_dt_s" PropertyName="Value" Type="String" />
                    <asp:ControlParameter ControlID="HID_START_DT_E" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="start_dt_e" PropertyName="Value" Type="String" />
                    <asp:ControlParameter ControlID="txt_END_DT_S"
                        Name="end_dt_s" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_END_DT_E"
                        Name="end_dt_e" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="HID_IS_VALID" DefaultValue="" ConvertEmptyStringToNull="False"
                        Name="is_valid" PropertyName="Value" Type="String" />
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
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_RowNumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_PJOB_CD%>" SortExpression="PJOB_CD" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_PJOB_CD" runat="server" Text='<%#Bind("PJOB_CD")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_PJOB_CD" runat="server" Width="80px" CssClass="MandatoryField EnNum4" Text='<%#Bind("PJOB_CD")%>' onchange="setHidPjobCD(this);"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_PJOB_CD%>"
                                ControlToValidate="txt_EDIT_PJOB_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="Regular_NEW_PJOB_CD" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2ha_error_pjob_cd%>" ControlToValidate="txt_EDIT_PJOB_CD" ForeColor="Red" ValidationGroup="GroupA"
                                ValidationExpression=".{4,4}" Display="None"></asp:RegularExpressionValidator>
                            <asp:RegularExpressionValidator ID="onlyEngNum" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2ha_format_pjob_cd%>" ControlToValidate="txt_EDIT_PJOB_CD" ForeColor="Red" ValidationGroup="GroupA"
                                ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_PJOB_CD" runat="server" Width="80px" CssClass="MandatoryField EnNum4" onchange="setHidPjobCD(this);"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_PJOB_CD%>"
                                ControlToValidate="txt_NEW_PJOB_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="Regular_NEW_PJOB_CD" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2ha_error_pjob_cd%>" ControlToValidate="txt_NEW_PJOB_CD" ForeColor="Red" ValidationGroup="GroupA"
                                ValidationExpression=".{4,4}" Display="None"></asp:RegularExpressionValidator>
                            <asp:RegularExpressionValidator ID="onlyEngNum" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2ha_format_pjob_cd%>" ControlToValidate="txt_NEW_PJOB_CD" ForeColor="Red" ValidationGroup="GroupA"
                                ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_PJOB_DESC%>" SortExpression="PJOB_DESC" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_PJOB_DESC" runat="server" Text='<%#Bind("PJOB_DESC")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_PJOB_DESC" runat="server" MaxLength="30" Width="100px" CssClass="MandatoryField" Text='<%#Bind("PJOB_DESC")%>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_PJOB_DESC%>"
                                ControlToValidate="txt_EDIT_PJOB_DESC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_PJOB_DESC" runat="server" MaxLength="30" Width="100px" CssClass="MandatoryField"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_PJOB_DESC%>"
                                ControlToValidate="txt_NEW_PJOB_DESC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_START_DT%>" SortExpression="START_DT" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_START_DT" runat="server" Text='<%#Bind("START_DT")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_START_DT" runat="server" MaxLength="10" Width="100px" CssClass="MandatoryField date ymd" Text='<%#Bind("START_DT")%>' OnTextChanged="txt_EDIT_START_DT_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_Required_START_DT%>"
                                ControlToValidate="txt_EDIT_START_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cv_EDIT_START_DT" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_START_DT_S%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_EDIT_START_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_START_DT" runat="server" MaxLength="10" Width="100px" CssClass="MandatoryField date ymd" OnTextChanged="txt_NEW_START_DT_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_Required_START_DT%>"
                                ControlToValidate="txt_NEW_START_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cv_NEW_START_DT" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_START_DT_S%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_NEW_START_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_END_DT%>" SortExpression="END_DT" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_END_DT" runat="server" Text='<%#Bind("END_DT")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_END_DT" runat="server" MaxLength="10" Width="100px" CssClass="MandatoryField date ymd" Text='<%#Bind("END_DT")%>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_Required_START_DT%>"
                                ControlToValidate="txt_EDIT_END_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cv_EDIT_END_DT" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_END_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_EDIT_END_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToCompare="txt_EDIT_START_DT"
                                ControlToValidate="txt_EDIT_END_DT" ErrorMessage="<%$Resources:Resource,wfb2ha_Start_End_Date%>" Type="Date" Operator="GreaterThanEqual"
                                Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_END_DT" runat="server" MaxLength="10" Width="100px" CssClass="MandatoryField date ymd"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_Required_END_DT%>"
                                ControlToValidate="txt_NEW_END_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cv_NEW_END_DT" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_END_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_NEW_END_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_NEW_START_DT"
                                ControlToValidate="txt_NEW_END_DT" ErrorMessage="<%$Resources:Resource,wfb2ha_CP_START_DT%>" Type="Date" Operator="GreaterThanEqual"
                                Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_LEVEL_CD%>" SortExpression="LEVEL_CD" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEVEL_CD" runat="server" Text='<%#Bind("LEVEL_CD")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_EDIT_LEVEL_CD" runat="server" CssClass="MandatoryField"></asp:DropDownList>
                            <asp:HiddenField ID="hid_EDIT_LEVEL_CD" runat="server" Value='<%#Bind("LEVEL_CD")%>' />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_Required_LEVEL_CD%>"
                                ControlToValidate="ddl_EDIT_LEVEL_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_NEW_LEVEL_CD" runat="server" CssClass="MandatoryField"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_Required_LEVEL_CD%>"
                                ControlToValidate="ddl_NEW_LEVEL_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_WS_CD%>" SortExpression="WS_CD" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_WS_CD" runat="server" Text='<%#Bind("WS_CD")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_EDIT_WS_CD" runat="server" CssClass="MandatoryField"></asp:DropDownList>
                            <asp:HiddenField ID="hid_EDIT_WS_CD" runat="server" Value='<%#Bind("WS_CD")%>' />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_Required_WS_CD%>"
                                ControlToValidate="ddl_EDIT_WS_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_NEW_WS_CD" runat="server" CssClass="MandatoryField"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_Required_WS_CD%>"
                                ControlToValidate="ddl_NEW_WS_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_MANAGEMENT_ALLOWANCE%>" SortExpression="MANAGEMENT_ALLOWANCE" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_MANAGEMENT_ALLOWANCE" runat="server" Text='<%#Bind("MANAGEMENT_ALLOWANCE","{0:n0}")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_NEW_MANAGEMENT_ALLOWANCE" runat="server" Width="70px" CssClass="number6" Text='<%#Bind("MANAGEMENT_ALLOWANCE")%>'></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorMANAGEMENT_ALLOWANCE" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ha_MANAGEMENT_ALLOWANCE_isError%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_MANAGEMENT_ALLOWANCE" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_MANAGEMENT_ALLOWANCE" runat="server" Width="70px" CssClass="number6"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorMANAGEMENT_ALLOWANCE" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ha_MANAGEMENT_ALLOWANCE_isError%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_MANAGEMENT_ALLOWANCE" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_PROFESSION_ALLOWANCE%>" SortExpression="PROFESSION_ALLOWANCE" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_PROFESSION_ALLOWANCE" runat="server" Text='<%#Bind("PROFESSION_ALLOWANCE","{0:n0}")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_NEW_PROFESSION_ALLOWANCE" runat="server" Width="70px" CssClass="number6" Text='<%#Bind("PROFESSION_ALLOWANCE")%>'></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorPROFESSION_ALLOWANCE" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ha_PROFESSION_ALLOWANCE_isError%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_PROFESSION_ALLOWANCE" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>

                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_PROFESSION_ALLOWANCE" runat="server" Width="70px" CssClass="number6"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorPROFESSION_ALLOWANCE" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ha_PROFESSION_ALLOWANCE_isError%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_PROFESSION_ALLOWANCE" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>

                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_PJOB_AGE_LIMIT%>" SortExpression="PJOB_AGE_LIMIT" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_PJOB_AGE_LIMIT" runat="server" Text='<%#Bind("PJOB_AGE_LIMIT")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_EDIT_PJOB_AGE_LIMIT" runat="server" CssClass="MandatoryField"></asp:DropDownList>
                            <asp:HiddenField ID="hid_EDIT_PJOB_AGE_LIMIT" runat="server" Value='<%#Bind("PJOB_AGE_LIMIT")%>' />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_Required_PJOB_AGE_LIMIT%>"
                                ControlToValidate="ddl_EDIT_PJOB_AGE_LIMIT" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_NEW_PJOB_AGE_LIMIT" runat="server" CssClass="MandatoryField"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_Required_PJOB_AGE_LIMIT%>"
                                ControlToValidate="ddl_NEW_PJOB_AGE_LIMIT" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--職務層級 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_PJOB_LEVEL%>" SortExpression="PJOB_LEVEL" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_PJOB_LEVEL" runat="server" Text='<%#Bind("PJOB_LEVEL_DESC")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_EDIT_PJOB_LEVEL" runat="server" CssClass="MandatoryField"></asp:DropDownList>
                            <asp:HiddenField ID="hid_EDIT_PJOB_LEVEL" runat="server" Value='<%#Bind("PJOB_LEVEL")%>' />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_Required_PJOB_LEVEL%>"
                                ControlToValidate="ddl_EDIT_PJOB_LEVEL" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_NEW_PJOB_LEVEL" runat="server" CssClass="MandatoryField"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_Required_PJOB_LEVEL%>"
                                ControlToValidate="ddl_NEW_PJOB_LEVEL" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_PJOB_FLOW_LEVEL%>" SortExpression="PJOB_FLOW_LEVEL" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_PJOB_FLOW_LEVEL" runat="server" Text='<%#Bind("PJOB_FLOW_LEVEL")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_PJOB_FLOW_LEVEL" runat="server" MaxLength="4" Width="30px" CssClass="MandatoryField number2" Text='<%#Bind("PJOB_FLOW_LEVEL")%>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_Required_PJOB_FLOW_LEVEL%>"
                                ControlToValidate="txt_EDIT_PJOB_FLOW_LEVEL" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_PJOB_FLOW_LEVEL" runat="server" MaxLength="4" Width="30px" CssClass="MandatoryField number2"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_Required_PJOB_FLOW_LEVEL%>"
                                ControlToValidate="txt_NEW_PJOB_FLOW_LEVEL" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_BUSINESS_TRIP_GRP%>" SortExpression="BUSINESS_TRIP_GRP" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_BUSINESS_TRIP_GRP" runat="server" Text='<%#Bind("BUSINESS_TRIP_GRP_DESC")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_EDIT_BUSINESS_TRIP_GRP" runat="server" Width="100px" CssClass="MandatoryField"></asp:DropDownList>
                            <asp:HiddenField ID="hid_EDIT_BUSINESS_TRIP_GRP" runat="server" Value='<%#Bind("BUSINESS_TRIP_GRP")%>' />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_Required_BUSINESS_TRIP_GRP%>"
                                ControlToValidate="ddl_EDIT_BUSINESS_TRIP_GRP" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_NEW_BUSINESS_TRIP_GRP" runat="server" Width="100px" CssClass="MandatoryField"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_Required_BUSINESS_TRIP_GRP%>"
                                ControlToValidate="ddl_NEW_BUSINESS_TRIP_GRP" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_REMARK%>" SortExpression="REMARK" HeaderStyle-Width="250px" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_REMARK" runat="server" Text='<%#Bind("REMARK")%>' Width="250px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_REMARK" runat="server" MaxLength="210" Width="300px" Text='<%#Bind("REMARK")%>'></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_REMARK" runat="server" MaxLength="210" Width="300px"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <div style="width: 1020px; overflow: auto;">
                        <table class="grid-view" width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                            <tr class="header">
                                <td></td>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2ha_RowNumber%>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_PJOB_CD%>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_PJOB_DESC%>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2ha_START_DT%>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label17" runat="server" Text="<%$Resources:Resource,wfb2ha_END_DT%>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_LEVEL_CD%>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_WS_CD%>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_MANAGEMENT_ALLOWANCE%>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_PROFESSION_ALLOWANCE%>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_PJOB_AGE_LIMIT%>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label10" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_PJOB_LEVEL%>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label11" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_PJOB_FLOW_LEVEL%>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label12" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_BUSINESS_TRIP_GRP%>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label13" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_REMARK%>"></asp:Label>
                                </td>
                            </tr>
                            <tr class="normal">
                                <td></td>
                                <td></td>
                                <td>
                                    <asp:TextBox ID="txt_NEW_PJOB_CD" runat="server" MaxLength="4" Width="30px" CssClass="MandatoryField" onchange="setHidPjobCD(this);"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_PJOB_CD%>"
                                        ControlToValidate="txt_NEW_PJOB_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_NEW_PJOB_DESC" runat="server" MaxLength="30" Width="210px" CssClass="MandatoryField"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_PJOB_DESC%>"
                                        ControlToValidate="txt_NEW_PJOB_DESC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_NEW_START_DT" runat="server" MaxLength="10" Width="100px" CssClass="MandatoryField date ymd" OnTextChanged="txt_NEW_START_DT_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_Required_START_DT%>"
                                        ControlToValidate="txt_NEW_START_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cv_NEW_START_DT" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_START_DT_S%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                        ControlToValidate="txt_NEW_START_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_NEW_END_DT" runat="server" MaxLength="10" Width="100px" CssClass="MandatoryField date ymd"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_Required_END_DT%>"
                                        ControlToValidate="txt_NEW_END_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cv_NEW_END_DT" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_END_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                        ControlToValidate="txt_NEW_END_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToCompare="txt_NEW_START_DT"
                                        ControlToValidate="txt_NEW_END_DT" ErrorMessage="<%$Resources:Resource,wfb2ha_Start_End_Date%>" Type="Date" Operator="GreaterThanEqual"
                                        Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_NEW_LEVEL_CD" runat="server"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_Required_LEVEL_CD%>"
                                        ControlToValidate="ddl_NEW_LEVEL_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_NEW_WS_CD" runat="server"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_Required_WS_CD%>"
                                        ControlToValidate="ddl_NEW_WS_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_NEW_MANAGEMENT_ALLOWANCE" runat="server" Width="70px" CssClass="number6"></asp:TextBox>
                                    <asp:CustomValidator ID="CustomValidatorMANAGEMENT_ALLOWANCE" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="<%$Resources:Resource,wfb2ha_MANAGEMENT_ALLOWANCE_isError%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                        ControlToValidate="txt_NEW_MANAGEMENT_ALLOWANCE" ValidationGroup="GroupA" Display="None">
                                    </asp:CustomValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_NEW_PROFESSION_ALLOWANCE" runat="server" Width="70px" CssClass="number6"></asp:TextBox>
                                    <asp:CustomValidator ID="CustomValidatorPROFESSION_ALLOWANCE" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="<%$Resources:Resource,wfb2ha_PROFESSION_ALLOWANCE_isError%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                        ControlToValidate="txt_NEW_PROFESSION_ALLOWANCE" ValidationGroup="GroupA" Display="None">
                                    </asp:CustomValidator>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_NEW_PJOB_AGE_LIMIT" runat="server"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_Required_PJOB_AGE_LIMIT%>"
                                        ControlToValidate="ddl_NEW_PJOB_AGE_LIMIT" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_NEW_PJOB_LEVEL" runat="server"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_Required_PJOB_LEVEL%>"
                                        ControlToValidate="ddl_NEW_PJOB_LEVEL" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_NEW_PJOB_FLOW_LEVEL" runat="server" MaxLength="4" Width="30px" CssClass="MandatoryField number2"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_Required_PJOB_FLOW_LEVEL%>"
                                        ControlToValidate="txt_NEW_PJOB_FLOW_LEVEL" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_NEW_BUSINESS_TRIP_GRP" runat="server"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_Required_BUSINESS_TRIP_GRP%>"
                                        ControlToValidate="ddl_NEW_BUSINESS_TRIP_GRP" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_NEW_REMARK" runat="server" MaxLength="210" Width="300px"></asp:TextBox>
                                </td>
                                <td></td>

                            </tr>
                        </table>
                    </div>
                </EmptyDataTemplate>
                <HeaderStyle CssClass="GridviewScrollHeader" />
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle HorizontalAlign="Center" />
                <EditRowStyle HorizontalAlign="Center" />
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
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
            <asp:HiddenField ID="HID_PJOB_CD" runat="server" ClientIDMode="Static" Value="" />
            <asp:HiddenField ID="HID_MANAGEMENT_ALLOWANCE" runat="server" ClientIDMode="Static" Value="" />
            <asp:HiddenField ID="HID_PROFESSION_ALLOWANCE" runat="server" ClientIDMode="Static" Value="" />

            <asp:HiddenField ID="HID_search_PJOB_CD" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_LEVEL_CD" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_WS_CD" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_PJOB_AGE_LIMIT" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_PJOB_LEVEL" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_BUSINESS_TRIP_GRP" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_START_DT_S" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_START_DT_E" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_IS_VALID" runat="server" ClientIDMode="Static" />

            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Del_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Del_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Cancel_ConfirmMessage%>" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
