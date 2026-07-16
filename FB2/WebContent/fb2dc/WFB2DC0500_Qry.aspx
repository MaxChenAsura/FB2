<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dc/action/WFB2DC0500_Qry.aspx.cs" Inherits="WebContent_WFB2DC0500_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');
            $(".number").mask('999');

            $("#txt_CARD_NAME").attr("readonly", true);
            $("#txt_PERSON_NAME").attr("readonly", true);

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
                ,freezesize:5
            });
        }

        //清空畫面
        function ClearAll() {
            $("#txt_CARD_NO").val("");
            $("#txt_CARD_NAME").val("");
            $("#txt_PERSON_ID").val("");
            $("#txt_PERSON_NAME").val("");
            $("#ddl_BORROW_STATUS").val("-1");

            $("#txt_START_DT_S").val("");
            $("#txt_START_DT_E").val("");
            $("#ddl_IS_RE_MAKE").val("-1");
            $("#txt_END_DT_S").val("");
            $("#txt_END_DT_E").val("");
            $('#rbl_BORROW_TYPE').find("input[value='1']").prop("checked", "checked");
        }

        function OpenPERSON_ID() {
            var ddl = $("#rbl_BORROW_TYPE input:radio:checked").val();
            if (ddl == '1') {
                var is_super = $("#hid_is_super").val();
                var json = OpenEmpSearch('txt_PERSON_ID', 'txt_PERSON_NAME', is_super);
                if (json != undefined) {
                    $("#txt_PERSON_ID").val(json.EMP_ID);
                    $("#txt_PERSON_NAME").val(json.EMP_NAME);
                }
            } else {
                var json = OpenSearch('Vendor_d_Search.aspx', 'txt_PERSON_ID', 'txt_PERSON_NAME', '');
                if (json != undefined) {
                    $("#txt_PERSON_ID").val(json.CD);
                    $("#txt_PERSON_NAME").val(json.DESC);
                }
            }
        }

        function getEmpName(EMP_id) {
            var txt = document.getElementById(EMP_id);
            if (txt.value != "") {
                document.getElementById('hid_getEmpName').click();
                return false;
            } else {
                $("#txt_PERSON_NAME").val("");
            }
        }

        function LookUpCheckboxs() {
            var ItemCheckBoxs = $(":checkbox[id$=cb_check]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }

        function CheckDelAction() {
            if (LookUpCheckboxs() > 0)
                return confirm("確定要刪除?");
            else {
                alert("請選取資料!");
                return false;
            }
        }

        function getCARD_NAME(CARD_id) {
            var txt = document.getElementById(CARD_id);
            if (txt.value != "") {
                document.getElementById('hid_getCARD_NAME').click();
                return false;
            }
            else {
                $("#txt_CARD_NAME").val("");
            }
        }

        function openCARD_NO() {
            OpenSearch('Card_Search.aspx', 'txt_CARD_NO', 'txt_CARD_NAME', 'CARD_TYPE=Qry', 'Y');
            //var json = OpenSearch('Card_Search.aspx', 'txt_CARD_NO', 'txt_CARD_NAME', 'CARD_TYPE=Qry','Y');
            //if (json != undefined) {
            //    if (json.DESC != "&nbsp;") {
            //        $("#txt_CARD_NAME").val(json.DESC);
            //    }
            //    else {
            //        $("#txt_CARD_NAME").val("");
            //    }
            //}

        }

        function Card_Search(eid, ename, value) {
            var returnValue = value;
            if (value == undefined) {
                returnValue = 'undefined';
            }
            //alert(returnValue);
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                $('#' + eid).val(obj.CD);
                if (obj.DESC != "&nbsp;") {
                    $("#txt_CARD_NAME").val(obj.DESC);
                }
                else {
                    $("#txt_CARD_NAME").val("");
                }

            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="10%" />
                                <col width="30%" />
                                <col width="10%" />
                                <col width="30%" />
                                <col width="20%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <%--借用卡號  --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_CARD_NO" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CARD_NO2%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_CARD_NO" runat="server" MaxLength="8" Width="64px" ClientIDMode="Static" onblur="javascript:getCARD_NAME(this.id)"></asp:TextBox>
                                        <input id="btn_CARD_NO" type="button" value="..." onclick="openCARD_NO();" />
                                        <asp:TextBox ID="txt_CARD_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                     <%--工號/廠商人員編號 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PERSON_ID" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_PERSON_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:RadioButtonList ID="rbl_BORROW_TYPE" runat="server" RepeatDirection="Horizontal" ClientIDMode="Static">
                                            <asp:ListItem Text="<%$Resources:Resource,wfb2dc_lb_PERSON%>" Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="<%$Resources:Resource,wfb2dc_lb_COMPANY%>" Value="2"></asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:TextBox ID="txt_PERSON_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static"
                                            onblur="javascript:getEmpName(this.id)"></asp:TextBox>
                                        <input id="bt_PERSON_ID" type="button" value="..." onclick="OpenPERSON_ID();" />
                                        <asp:TextBox ID="txt_PERSON_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <%--卡片狀態 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BORROW_STATUS" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_BORROW_STATUS%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_BORROW_STATUS" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <%--借用期間 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_START_DT" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_START_DT_RANGE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_START_DT_S" runat="server" ClientIDMode="Static" CssClass="date" Width="81px"></asp:TextBox>~
                                        <asp:TextBox ID="txt_START_DT_E" runat="server" ClientIDMode="Static" CssClass="date" Width="81px"></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidatorSTART_DT_S" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_START_DATE_S%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_START_DT_S" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidatorSTART_DT_E" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_START_DATE_E%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_START_DT_E" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>

                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_START_DT_S"
                                            ControlToValidate="txt_START_DT_E" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_START_DT_RANGE%>" Type="Date" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <%--是否重新製卡 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_IS_RE_MAKE" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_IS_RE_MARK%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_IS_RE_MAKE" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <%-- 歸還期間 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_END_DT" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_END_DT_RANGE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_END_DT_S" runat="server" ClientIDMode="Static" CssClass="date" Width="81px"></asp:TextBox>~
                                        <asp:TextBox ID="txt_END_DT_E" runat="server" ClientIDMode="Static" CssClass="date" Width="81px"></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidatorEND_DT_S" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_END_DATE_S%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_END_DT_S" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidatorEND_DT_E" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_END_DATE_E%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_END_DT_E" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>

                                        <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToCompare="txt_END_DT_S"
                                            ControlToValidate="txt_END_DT_E" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_END_DT_RANGE%>" Type="Date" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2DC0500Search" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0500Search%>" ValidationGroup="GroupA" OnClientClick="CheckValid();" OnClick="WFB2DC0500Search_Click" />
                                            
                                            <%--<asp:Button ID="WFB2DC0500Search" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0500Search%>" ValidationGroup="GroupA" OnClientClick="CheckValid();" OnClick="WFB2DC0500Search_Click" />--%>
                                            
                                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2dc_btn_clear%>" onclick="ClearAll();" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td align="right" class="Body_label">
                        <hr>
                        <div id="init_grid">
                        <aces:Btn ID="WFB2DC0500Borrow" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0500Borrow%>" OnClick="WFB2DC0500Borrow_Click" />
                            <aces:Btn ID="WFB2DC0500Return" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0500Return%>" Visible="false" OnClick="WFB2DC0500Return_Click" />
                            <aces:Btn ID="WFB2DC0500Delete" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0500Delete%>" OnClientClick="return CheckDelAction();" Visible="false" OnClick="WFB2DC0500Delete_Click" />
                            <aces:Btn ID="WFB2DC0500Update" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0500Update%>" Visible="false" OnClick="WFB2DC0500Update_Click" />

<%--                            <asp:Button ID="WFB2DC0500Borrow" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0500Borrow%>" OnClick="WFB2DC0500Borrow_Click" />
                            <asp:Button ID="WFB2DC0500Return" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0500Return%>" Visible="false" OnClick="WFB2DC0500Return_Click" />
                            <asp:Button ID="WFB2DC0500Delete" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0500Delete%>" OnClientClick="return CheckDelAction();" Visible="false" OnClick="WFB2DC0500Delete_Click" />
                            <asp:Button ID="WFB2DC0500Update" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0500Update%>" Visible="false" OnClick="WFB2DC0500Update_Click" />--%>
                        </div>
                    </td>
                </tr>

            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DC0500DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_CARD_NO"
                        Name="card_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="rbl_BORROW_TYPE" DefaultValue=""
                        Name="borrow_type" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="txt_PERSON_ID"
                        Name="person_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_BORROW_STATUS" DefaultValue=""
                        Name="borrow_status" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="txt_START_DT_S"
                        Name="start_dt_s" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_START_DT_E"
                        Name="start_dt_e" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_IS_RE_MAKE" DefaultValue=""
                        Name="is_re_make" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="txt_END_DT_S"
                        Name="end_dt_s" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_END_DT_E"
                        Name="end_dt_e" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" Width="1020px"
                OnRowCreated="gv_result_RowCreated" OnDataBound="gv_result_DataBound"
                OnPageIndexChanging="gv_result_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" Width="20px"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                   
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_RowNumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <%--借卡人員--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_BORROW_TYPE%>" SortExpression="BORROW_TYPE" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_BORROW_TYPE" runat="server" Text='<%#Bind("BORROW_TYPE_NAME")%>' Width="100px"></asp:Label>
                            <asp:HiddenField ID="hid_BORROW_TYPE" runat="server" Value='<%#Bind("BORROW_TYPE")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                     <%--工號/廠商人員編號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_PERSON_ID%>" SortExpression="PERSON_ID" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_PERSON_ID" runat="server" Text='<%#Bind("PERSON_ID")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <%--姓名--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_PERSON_NAME%>" SortExpression="PERSON_NAME" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_PERSON_NAME" runat="server" Text='<%#Bind("PERSON_NAME")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--部門/廠商別--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_PERSON_DC%>" SortExpression="PERSON_DC" HeaderStyle-Width="360px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_PERSON_DC" runat="server" Text='<%#Bind("PERSON_DC")%>' Width="360px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--借用卡號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_CARD_NO2%>" SortExpression="CARD_NO" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_CARD_NO" runat="server" Text='<%#Bind("CARD_NO")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <%--借用起日  --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_START_DT_S%>" SortExpression="START_DT" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_START_DT" runat="server" Text='<%#Bind("START_DT", "{0:yyyy/MM/dd HH:mm}")%>' Width="140px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <%--借用迄日  --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_START_DT_E%>" SortExpression="END_DT" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_END_DT" runat="server" Text='<%#Bind("END_DT", "{0:yyyy/MM/dd HH:mm}")%>' Width="140px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--實際還卡時間  --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_RETURN_DT%>" SortExpression="RETURN_DT" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_RETURN_DT" runat="server" Text='<%#Bind("RETURN_DT", "{0:yyyy/MM/dd HH:mm}")%>' Width="140px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <%--卡片狀態  --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_BORROW_STATUS%>" SortExpression="BORROW_STATUS" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_BORROW_STATUS" runat="server" Text='<%#Bind("BORROW_STATUS")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <%--借用原因  --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_BORROW_REASON_CD%>" SortExpression="BORROW_REASON_CD" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_BORROW_REASON_CD" runat="server" Text='<%#Bind("BORROW_REASON_CD")%>' Width="90px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <%--是否重新製卡  --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_IS_RE_MARK%>" SortExpression="IS_RE_MAKE" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label ID="lb_IS_RE_MAKE" runat="server" ClientIDMode="Static" Width="60px"></asp:Label>
                            <asp:HiddenField ID="hid_IS_RE_MAKE" runat="server" Value='<%#Bind("IS_RE_MAKE")%>' />
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
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:HiddenField ID="hid_is_super" runat="server" ClientIDMode="Static" />
            <asp:Button ID="hid_getEmpName" runat="server" Style="display: none" ClientIDMode="Static" OnClick="hid_getEmpName_Click" />
            <asp:Button ID="hid_getCARD_NAME" runat="server" Style="display: none" ClientIDMode="Static" OnClick="hid_getCARD_NAME_Click" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>

