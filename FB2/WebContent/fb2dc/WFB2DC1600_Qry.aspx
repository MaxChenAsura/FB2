<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dc/action/WFB2DC1600_Qry.aspx.cs" Inherits="WebContent_WFB2DC1600_Qry" Culture="auto" UICulture="auto" %>

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

            $("#txt_PERSON_ID").change(function () {
                if ($("#txt_PERSON_ID").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_PERSON_ID').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        cache: false,
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_PERSON_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_PERSON_NAME').val($.trim(JData.EMP_NAME));
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_PERSON_NAME').val("");
                }
            });

            //部門代號取得部門名稱的ajax
            $("#txt_EDIT_PERSON_ID").change(function () {
                if ($("#txt_EDIT_PERSON_ID").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_EDIT_PERSON_ID').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        cache: false,
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#lb_EDIT_PERSON_NAME').text("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#lb_EDIT_PERSON_NAME').text($.trim(JData.EMP_NAME));
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#lb_EDIT_PERSON_NAME').text("");
                }
            });
        }

        //凍結視窗用
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "400",
                barcolor: "#7F7F7F"
            });
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
            $("#txt_CLOCK_DT_S").val("");
            $("#txt_CLOCK_DT_E").val("");
            $("#ddl_CARD_CHECK_STATUS").val("-1");
            $("#txt_PERSON_ID").val("");
            $("#txt_PERSON_NAME").val("");
            $("#txt_CLOCK_NO").val("");
            $("#txt_CLOCK_DESC2").val("");
            setDefaultData();
        }
        //給予查詢條件預設值
        function setDefaultData() {
            $("#txt_CLOCK_DT_S").val($("#hid_defalut_DT_S").val());
            $("#txt_CLOCK_DT_E").val($("#hid_defalut_DT_E").val());
        }

        //取得卡鐘名稱
        function getCLOCK_DESC(CLOCK_id) {
            var txt = document.getElementById(CLOCK_id);
            if (txt.value != "") {
                document.getElementById('hid_getCLOCK_DESC').click();
                return false;
            } else {
                $("#txt_CLOCK_DESC2").val("");
            }
        }

        //查詢前檢核
        function CheckSearch() {
            var processed = true;

            if (Page_ClientValidate("GroupA")) {
                processed = true;
                BlockUI();
            }
            else {
                processed = false;
            }

            if (!processed)
                $.unblockUI();
            return processed;
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
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_CLOCK_DT" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CLOCK_DT%>"></asp:Label>:
                        </th>
                        <%-- 查詢期間 --%>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_CLOCK_DT_S" runat="server" ClientIDMode="Static" CssClass="MandatoryField date" Width="100px"></asp:TextBox>~
                                        <asp:TextBox ID="txt_CLOCK_DT_E" runat="server" ClientIDMode="Static" CssClass="MandatoryField date" Width="100px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CLOCK_DT_S%>"
                                ControlToValidate="txt_CLOCK_DT_S" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CLOCK_DT_E%>"
                                ControlToValidate="txt_CLOCK_DT_E" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidatorCLOCK_DT_S" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CLOCK_DATE_S%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_CLOCK_DT_S" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidatorCLOCK_DT_E" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CLOCK_DATE_E%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_CLOCK_DT_E" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_CLOCK_DT_S"
                                ControlToValidate="txt_CLOCK_DT_E" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CLOCK_DATE_RANGE%>" Type="Date" Operator="GreaterThanEqual"
                                Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                        </td>
                        <%-- 處理狀態 --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_CARD_CHECK_STATUS" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CARD_CHECK_STATUS%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_CARD_CHECK_STATUS" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <%-- 工號 --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_PERSON_ID" runat="server" Text="工號"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_PERSON_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" ></asp:TextBox>
                             <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_PERSON_ID', 'txt_PERSON_NAME', 'N', '');" />
                            <asp:TextBox ID="txt_PERSON_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <%-- 卡鐘編號 --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_CLOCK_NO" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CLOCK_NO%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_CLOCK_NO" runat="server" MaxLength="3" Width="64px" ClientIDMode="Static" onblur="javascript:getCLOCK_DESC(this.id)"></asp:TextBox>
                            <input id="btn_CLOCK_NO" type="button" value="..." onclick="OpenSearch('Clock_Search.aspx', 'txt_CLOCK_NO', 'txt_CLOCK_DESC2', '');" />
                            <asp:TextBox ID="txt_CLOCK_DESC2" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                        </td>
                    </tr>
                                       <tr>
                        <td align="right" colspan="4">
                            <aces:Btn ID="WFB2DC1600Search" runat="server" Text="<%$Resources:Resource,wfb2dj_btn_search%>" OnClick="WFB2DC1600Search_Click" OnClientClick="return CheckSearch();"  />

                            <%--<asp:Button ID="WFB2DC1600Search" runat="server" Text="<%$Resources:Resource,wfb2dj_btn_search%>" OnClick="WFB2DC1600Search_Click" OnClientClick=" BlockUI();" />--%>

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
                         <aces:Btn ID="WFB2DC1600Edit" runat="server" Text="修改" Visible="false" OnClick="WFB2DC1600Edit_Click" OnClientClick="BlockUI();" />
                        <aces:Btn ID="WFB2DC1600Save" runat="server" Text="確認" Visible="false" OnClick="WFB2DC1600Save_Click"  OnClientClick="BlockUI();" />
                        <asp:Button ID="btn_cancel" runat="server" Text="<%$Resources:Resource,wfb2dj_btn_cancel%>" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="btn_cancel_Click" />
                        </td>
                    </tr>
                </tbody>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DC1600DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_CLOCK_DT_S"
                        Name="clock_dt_s" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_CLOCK_DT_E"
                        Name="clock_dt_e" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_CARD_CHECK_STATUS"
                        Name="card_check_status" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_PERSON_ID"
                        Name="person_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_CLOCK_NO" DefaultValue=""
                        Name="clock_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 序號 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_rownumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--卡鐘編號 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_CLOCK_NO%>" SortExpression="CLOCK_NO" HeaderStyle-Width="200px"  ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_CLOCK_NO" runat="server" Text='<%#Bind("CLOCK_NO_DESC")%>' Width="200px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_CLOCK_NO" runat="server" Text='<%#Bind("CLOCK_NO_DESC")%>' Width="200px"></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--卡號 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_CARD_NO%>" SortExpression="CARD_NO" HeaderStyle-Width="80px"  ItemStyle-Width="80px" ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <asp:Label ID="lb_CARD_NO" runat="server" Text='<%#Bind("CARD_NO")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_CARD_NO" runat="server" Text='<%#Bind("CARD_NO")%>' Width="80px"></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--刷卡時間 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_CLOCK_DT2%>" SortExpression="CLOCK_DT" HeaderStyle-Width="160px" ItemStyle-Width="160px" ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <asp:Label ID="lb_CLOCK_DT" runat="server" Text='<%#Bind("CLOCK_DT", "{0:yyyy/MM/dd HH:mm:ss}")%>' Width="160px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_CLOCK_DT" runat="server" Text='<%#Bind("CLOCK_DT", "{0:yyyy/MM/dd HH:mm:ss}")%>' Width="160px"></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                     <%--部門名稱 --%>
                    <asp:TemplateField HeaderText="部門名稱" HeaderStyle-Width="120px"  ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Left" >
                        <ItemTemplate>
                            <asp:Label ID="lb_PERSON_DC" runat="server" Text='<%#Bind("PERSON_DC")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_PERSON_DC" runat="server" Text='<%#Bind("PERSON_DC")%>' Width="120px"></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                     <%--工號 --%>
                    <asp:TemplateField HeaderText="工號" SortExpression="PERSON_ID" HeaderStyle-Width="70px"  ItemStyle-Width="70px" >
                        <ItemTemplate>
                            <asp:Label ID="lb_PERSON_ID" runat="server" Text='<%#Bind("PERSON_ID")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_PERSON_ID" runat="server" MaxLength="5" Text='<%#Bind("PERSON_ID")%>' ClientIDMode="Static"  Width="70px" CssClass=""></asp:TextBox>
                            <asp:HiddenField ID="hid_ORI_PERSON_ID" runat="server" Value='<%#Bind("PERSON_ID")%>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--人員姓名 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_PERSON_NAME2%>" SortExpression="PERSON_NAME" HeaderStyle-Width="80px"  ItemStyle-Width="80px" >
                        <ItemTemplate>
                            <asp:Label ID="lb_PERSON_NAME" runat="server" Text='<%#Bind("PERSON_NAME")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_EDIT_PERSON_NAME" runat="server" Text='<%#Bind("PERSON_NAME")%>' Width="80px"></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--處理狀態 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_CARD_CHECK_STATUS%>" SortExpression="CARD_CHECK_STATUS" HeaderStyle-Width="120px"  ItemStyle-Width="120px" >
                        <ItemTemplate>
                            <asp:Label ID="lb_CARD_CHECK_STATUS" runat="server" Text='<%#Bind("CARD_CHECK_STATUS")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_CARD_CHECK_STATUS" runat="server" Text='<%#Bind("CARD_CHECK_STATUS")%>' Width="120px"></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
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
            <%-- 隱藏按鈕 --%>
            <asp:Button ID="hid_getCLOCK_DESC" runat="server" Style="display: none" ClientIDMode="Static" OnClick="hid_getCLOCK_DESC_Click" />

            <asp:HiddenField ID="hid_defalut_DT_S" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_defalut_DT_E" runat="server" ClientIDMode="Static" />
            <!-- 每頁的筆數 -->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <!-- 是否要凍結視窗 -->
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="N" />
            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
