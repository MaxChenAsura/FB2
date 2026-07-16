<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2si/WFB2SI0300_Dtl.aspx.cs" Inherits="WebContent_fb2si_WFB2SI0300_Dtl" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        var li_id;
        function iniForm() {
            $('#ddlPerPageRow').css("font-family", "Times New Roman, Times, serif").css("font-size", "14px");
            $(".ymd").mask('9999/99/99');
            //$(".money").formatNumber({ format: "#,###", locale: "tw" });
            gridviewScroll();
            $.unblockUI();
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
                        cache: false,
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
            $("#tabs").tabs();
            $('#ul li').click(function () {
                li_id = $(this).attr('id');
            });
            ChangeTab(li_id);

            $('#tab0').one('click', function () {
                gridviewScroll();
            });

            $('#tab0').trigger('click');


        }
        function ChangeTab(li_id) {
            $("#tabs").tabs({ active: li_id });
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
        function CheckApprove() {
            //if (LookUpCheckboxs() > 0) {
            //    alert($('#hid_wfb2si_cancel_approve_mark').val());
            //    return false;
            //}
            //else {
                return confirm($('#hid_wfb2si_approve_ConfirmMessage').val());
            //}
        }
        function CheckReject() {
            if ($('#txt_REMARK').val().trim() == "") {
                alert($('#hid_wfb2si_required_Remark').val());
                return false;
            }
            else {
                return confirm($('#hid_wfb2si_reject_ConfirmMessage').val());
            }
        }

        function LookUpCheckboxs() {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked && ItemCheckBoxs[i].id.indexOf("cb_all") == -1) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }

        //下載
        function checkDowning(msg) {
            BlockUI();
            if (confirm("確定要進行" + msg)) {
                return true;
            }
            else {
                $.unblockUI();
                return false;
            }
        }
        //清空
        function doClear() {
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            return false;
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
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="20%" />
                                <col width="25%" />
                                <col width="20%" />
                                <col width="25%" />
                                <col width="20%" />

                            </colgroup>
                            <tbody>

                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BONUS_YEAR" runat="server" Text="<%$Resources:Resource,wfb2si_BONUS_YEAR%>"></asp:Label>
                                    </th>
                                    <td colspan="3">
                                        <asp:TextBox ID="txt_BONUS_YEAR" runat="server" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BONUS_DAYS" runat="server" Text="<%$Resources:Resource,wfb2si_BONUS_DAYS%>"></asp:Label>
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txt_BONUS_DAYS" runat="server" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BONUS_DT" runat="server" Text="<%$Resources:Resource,wfb2si_lb_BONUS_DT%>"></asp:Label>
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txt_BONUS_DT" runat="server" ReadOnly="true" BorderWidth="0" CssClass="ymd"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BONUS_TOTAL_AMOUNT" runat="server" Text="<%$Resources:Resource,wfb2si_BONUS_TOTAL_AMOUNT%>"></asp:Label>
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txt_BONUS_TOTAL_AMOUNT" runat="server" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BONUS_TOTAL_DECIMAL" runat="server" Text="<%$Resources:Resource,wfb2si_BONUS_TOTAL_DECIMAL%>"></asp:Label>
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txt_BONUS_TOTAL_DECIMAL" runat="server" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2si_REMARK%>"></asp:Label>
                                    </th>
                                    <td colspan="5">
                                        <asp:TextBox TextMode="MultiLine" Rows="5" ID="txt_REMARK" runat="server" Width="100%" BorderWidth="1" Style="overflow: auto" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_Excel_Download" runat="server" Text="<%$Resources:Resource,wfb2si_Excel_Download%>"></asp:Label>
                                    </th>
                                    <td colspan="3">
                                        <aces:Btn ID="WFB2SI0300ExcelDown1" runat="server" Text="<%$Resources:Resource,wfb2si_download_thisApprove%>" OnClick="WFB2SI0300ExcelDown1_Click" OnClientClick="return checkDowning(this.value);" />
                                        <aces:Btn ID="WFB2SI0300ExcelDown2" runat="server" Text="<%$Resources:Resource,wfb2si_download_previousCompare%>" OnClick="WFB2SI0300ExcelDown2_Click" OnClientClick="return checkDowning(this.value);" />
                                        <aces:Btn ID="WFB2SI0300ExcelDown3" runat="server" Text="<%$Resources:Resource,wfb2si_download_originalCompare%>" OnClick="WFB2SI0300ExcelDown3_Click" OnClientClick="return checkDowning(this.value);" />

                                       <%-- <asp:Button ID="WFB2SI0300ExcelDown1" runat="server" Text="<%$Resources:Resource,wfb2si_download_thisApprove%>" OnClick="WFB2SI0300ExcelDown1_Click" OnClientClick="return checkDowning(this.value);" />
                                        <asp:Button ID="WFB2SI0300ExcelDown2" runat="server" Text="<%$Resources:Resource,wfb2si_download_previousCompare%>" OnClick="WFB2SI0300ExcelDown2_Click" OnClientClick="return checkDowning(this.value);" />
                                        <asp:Button ID="WFB2SI0300ExcelDown3" runat="server" Text="<%$Resources:Resource,wfb2si_download_originalCompare%>" OnClick="WFB2SI0300ExcelDown3_Click" OnClientClick="return checkDowning(this.value);" />--%>
                                    </td>

                                </tr>
                            </tbody>

                        </table>

                        <tr>
                            <td>
                                <table cellspacing="1" cellpadding="1" width="1020px" border="0" class="Body_Label">
                                    <colgroup>
                                        <col width="16%" />
                                        <col width="20%" />
                                        <col width="16%" />
                                        <col width="29%" />
                                        <col width="29%" />

                                    </colgroup>
                                    <tbody>
                                        <tr>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <td align="right" class="Body_label">
                                                <div id="init">
                                                    <asp:Button ID="btn_backpage" runat="server" Text="<%$Resources:Resource,wfb2si_backpage%>" OnClick="btn_backpage_Click" OnClientClick="BlockUI();" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5">
                                                <hr />
                                            </td>
                                        </tr>
                                        <tr>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_EMP_ID%>"></asp:Label>:
                                            </th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" CssClass=" empid"></asp:TextBox>
                                                <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N', '');" />
                                            </td>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_EMP_NAME%>"></asp:Label>:
                                            </th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="10" Width="70px" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <%--<th></th>--%>
                                            <td align="right" class="Body_label" colspan="2">
                                                <div id="init_grid">
                                                    <aces:Btn ID="WFB2SI0301Search" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0100Search%>" OnClick="WFB2SI0301Search_Click" OnClientClick="BlockUI();" />

                                                    <%--<asp:Button ID="WFB2SI0301Search" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0100Search%>" OnClick="WFB2SI0301Search_Click" OnClientClick="BlockUI();" />--%>
                                                    <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2ia_btn_clear%>" OnClientClick="return doClear();" />
                                                    
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5">
                                                <hr />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="2">
                                                    <aces:Btn ID="WFB2SI0300Mark" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_mark%>" OnClick="WFB2SI0300Mark_Click" OnClientClick="BlockUI();" />

                                                    <%--<asp:Button ID="WFB2SI0300Mark" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_mark%>" OnClick="WFB2SI0300Mark_Click" OnClientClick="BlockUI();" />--%>
                                            </td>
                                            <td align="right" colspan="3">
                                                    <aces:Btn ID="WFB2SI0300Approve" runat="server" Text="<%$Resources:Resource,wfb2si_APPROVE_FLAG%>" OnClientClick="return CheckApprove();BlockUI();" OnClick="WFB2SI0300Approve_Click" />
                                                    <aces:Btn ID="WFB2SI0300Reject" runat="server" Text="<%$Resources:Resource,wfb2si_Reject%>" OnClientClick="return CheckReject();BlockUI();" OnClick="WFB2SI0300Reject_Click" />

                                                    <%--<asp:Button ID="WFB2SI0300Approve" runat="server" Text="<%$Resources:Resource,wfb2si_APPROVE_FLAG%>" OnClientClick="return CheckApprove();BlockUI();" OnClick="WFB2SI0300Approve_Click" />
                                                    <asp:Button ID="WFB2SI0300Reject" runat="server" Text="<%$Resources:Resource,wfb2si_Reject%>" OnClientClick="return CheckReject();BlockUI();" OnClick="WFB2SI0300Reject_Click" />--%>
                                            </td>
                                        </tr>
                                    </tbody>

                                </table>
                            </td>
                        </tr>
                    </td>
                </tr>



            </table>

            <!-- 頁籤 開始-------------------  -->
            <div id="tabs" style="width: 1020px">
                <ul id="ul">
                    <li id="tab0"><a href="#gridList1">【紅利發放對象】</a></li>
                    <li id="1"><a href="#bonusTarget">【紅利對象條件】</a></li>
                    <li id="2"><a href="#bonusBase">【紅利金額條件】</a></li>
                </ul>
                <div id="gridList1" align="left">
                    <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="GetDtlData2"
                        SelectCountMethod="GetDtlCount" TypeName="CFB2SI0300DAO" EnablePaging="True"
                        SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                        StartRowIndexParameterName="startRowIndex"
                        OnSelected="ods1_Selected">

                        <SelectParameters>
                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                            <asp:Parameter Name="maximumRows" Type="Int32" />
                            <asp:ControlParameter ControlID="txt_BONUS_YEAR" DefaultValue=""
                                Name="bonus_year" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                            <asp:ControlParameter ControlID="txt_EMP_ID" DefaultValue=""
                                Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                            <asp:ControlParameter ControlID="txt_EMP_NAME" DefaultValue=""
                                Name="emp_name" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                        </SelectParameters>
                    </asp:ObjectDataSource>


                    <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                        AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                        OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1100px"
                        OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">

                        <Columns>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_APPROVE_MARK%>" SortExpression="APPROVE_MARK" HeaderStyle-Width="60px">
                                <ItemTemplate>
                                    <asp:CheckBox Width="60px" Style="text-align: center;" ID="cb_check" runat="server" ClientIDMode="AutoID" />
                                    <asp:Label Width="60px" Style="text-align: center;" ID="lb_APPROVE_MARK" runat="server" Text='<%#Bind("APPROVE_MARK")%>' Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2si_RowNum%>" HeaderStyle-Width="60px">
                                <ItemTemplate>
                                    <asp:Label Width="60px" Style="text-align: center;" ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_CHG_STATUS%>" SortExpression="CHG_STATUS" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:Label Width="80px" Style="text-align: left;" ID="lb_CHG_STATUS" runat="server" Text='<%#Bind("CHG_STATUS")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_APPROVE_FLAG%>" SortExpression="APPROVE_FLAG" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:Label Width="80px" Style="text-align: left;" ID="lb_APPROVE_FLAG" runat="server" Text='<%#Bind("APPROVE_FLAG")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_WS_CD%>" SortExpression="WS_CD" HeaderStyle-Width="120px">
                                <ItemTemplate>
                                    <asp:Label Width="120px" Style="text-align: left;" ID="lb_WS_CD" runat="server" Text='<%#Bind("WS_CD")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_lb_LEVEL_CD%>" SortExpression="LEVEL_CD" HeaderStyle-Width="40px">
                                <ItemTemplate>
                                    <asp:Label Width="40px" Style="text-align: left;" ID="lb_LEVEL_CD" runat="server" Text='<%#Bind("LEVEL_CD")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_EMP_ID1%>" SortExpression="EMP_ID" HeaderStyle-Width="60px">
                                <ItemTemplate>
                                    <asp:Label Width="60px" Style="text-align: left;" ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_EMP_NAME1%>" SortExpression="EMP_NAME" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:Label Width="80px" Style="text-align: left;" ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_WORK_DAYS%>" SortExpression="WORK_DAYS" HeaderStyle-Width="60px">
                                <ItemTemplate>
                                    <asp:Label Width="60px" Style="text-align: right;" ID="lb_WORK_DAYS" runat="server" Text='<%#Bind("WORK_DAYS")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_ATTEND_DAYS%>" SortExpression="ATTEND_DAYS" HeaderStyle-Width="60px">
                                <ItemTemplate>
                                    <asp:Label Width="60px" Style="text-align: right;" ID="lb_ATTEND_DAYS" runat="server" Text='<%#Bind("ATTEND_DAYS")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_BONUS_WORK_DAYS%>" SortExpression="BONUS_WORK_DAYS" HeaderStyle-Width="60px">
                                <ItemTemplate>
                                    <asp:Label Width="60px" Style="text-align: right;" ID="lb_BONUS_WORK_DAYS" runat="server" Text='<%#Bind("BONUS_WORK_DAYS")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_REWARD_DAYS%>" SortExpression="REWARD_DAYS" HeaderStyle-Width="60px">
                                <ItemTemplate>
                                    <asp:Label Width="60px" Style="text-align: right;" ID="lb_REWARD_DAYS" runat="server" Text='<%#Bind("REWARD_DAYS")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_DISCIPLINE_DAYS%>" SortExpression="DISCIPLINE_DAYS" HeaderStyle-Width="60px">
                                <ItemTemplate>
                                    <asp:Label Width="60px" Style="text-align: right;" ID="lb_DISCIPLINE_DAYS" runat="server" Text='<%#Bind("DISCIPLINE_DAYS")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_BONUS_AMT%>" SortExpression="BONUS_AMT" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:Label Width="80px" Style="text-align: right;" ID="lb_BONUS_AMT" runat="server" Text='<%#Bind("BONUS_AMT", "{0:N0}")%>' ClientIDMode="Static"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_PAY_STATUS1%>" SortExpression="PAY_TYPE" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:Label Width="80px" Style="text-align: left;" ID="lb_PAY_STATUS" runat="server" Text='<%#Bind("PAY_TYPE")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_EMP_CHG_CD1%>" SortExpression="EMP_CHG_CD" HeaderStyle-Width="140px">
                                <ItemTemplate>
                                    <asp:Label Width="140px" Style="text-align: left;" ID="lb_EMP_CHG_CD" runat="server" Text='<%#Bind("EMP_CHG_CD")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>

                        <EmptyDataTemplate>

                            <table class="grid-view" width="1020px">
                                <tr class="header">
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2si_APPROVE_MARK%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label" runat="server" Text="<%$Resources:Resource,wfb2si_RowNum%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Labe3" runat="server" Text="<%$Resources:Resource,wfb2si_CHG_STATUS%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2si_APPROVE_FLAG%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Labe5" runat="server" Text="<%$Resources:Resource,wfb2si_WS_CD%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Labe6" runat="server" Text="<%$Resources:Resource,wfb2si_lb_LEVEL_CD%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Labe7" runat="server" Text="<%$Resources:Resource,wfb2si_EMP_ID1%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Labe8" runat="server" Text="<%$Resources:Resource,wfb2si_EMP_NAME1%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Labe9" runat="server" Text="<%$Resources:Resource,wfb2si_WORK_DAYS%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Labe10" runat="server" Text="<%$Resources:Resource,wfb2si_ATTEND_DAYS%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label11" runat="server" Text="<%$Resources:Resource,wfb2si_BONUS_WORK_DAYS%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2si_REWARD_DAYS%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2si_DISCIPLINE_DAYS%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label14" runat="server" Text="<%$Resources:Resource,wfb2si_BONUS_AMT%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label15" runat="server" Text="<%$Resources:Resource,wfb2si_PAY_STATUS1%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label16" runat="server" Text="<%$Resources:Resource,wfb2si_EMP_CHG_CD1%>"></asp:Label>
                                    </td>

                                </tr>

                            </table>

                        </EmptyDataTemplate>
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
                </div>

                <div id="bonusTarget" align="left">
                    <table border="1">
                        <tr>
                            <th align="left" class="Body_TableHeader">發放日在籍者:</th>
                            <td align="left" class="Body_label">一般員工(含留職停薪)
                            </td>
                        </tr>
                        <tr>
                            <th align="left" class="Body_TableHeader">特殊對象(不在籍者):</th>
                            <td align="left" class="Body_label" colspan="3">

                                <table>
                                    <tr>
                                        <td class="Body_label">1.</td>
                                        <td class="Body_label">一般員工死亡或退休者</td>
                                    </tr>
                                    <tr>
                                        <td class="Body_label">2.</td>
                                        <td class="Body_label">歸任日藉出向</td>
                                    </tr>
                                    <tr>
                                        <td class="Body_label">3</td>
                                        <td class="Body_label">因兵役或提前解約離職者</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <th align="left" class="Body_TableHeader">除外對象:</th>
                            <td align="left" class="Body_label">
                                <table>
                                    <tr>
                                        <td class="Body_label">1.</td>
                                        <td class="Body_label">董事長(MA10)</td>
                                    </tr>
                                    <tr>
                                        <td class="Body_label">2.</td>
                                        <td class="Body_label">期間工(PJ10)</td>
                                    </tr>
                                    <tr>
                                        <td class="Body_label">3.</td>
                                        <td class="Body_label">派遺人員(PJ20,PJ30,PJ40)</td>
                                    </tr>
                                    <tr>
                                        <td class="Body_label">4.</td>
                                        <td class="Body_label">研修生(PJ60)</td>
                                    </tr>
                                </table>

                            </td>
                        </tr>
                    </table>
                </div>

                <div id="bonusBase" align="left">
                    <table border="1">
                        <tr>
                            <th align="left" class="Body_TableHeader">發放公式:</th>
                            <td align="left" class="Body_label">(發放base/日 * 發放天數 * 在職比例 ) <span style="font-family: WingDings2">&#177;</span>  紀律反應
                            </td>
                        </tr>
                        <tr>
                            <th align="left" class="Body_TableHeader">發放base(7月調整後base):</th>
                            <td align="left" class="Body_label"><span>個人「職能俸 + 資格俸 + 職務津貼 + 專業津貼 + 伙食津貼」
                            </span></td>
                        </tr>
                        <tr>
                            <th align="left" class="Body_TableHeader">在職比例:</th>
                            <td align="left" class="Body_label">
                                <table>
                                    <tr>
                                        <td class="Body_label">比例:</td>
                                        <td class="Body_label"><span>(yyyy/04/01 ~ yyyy+1/03/31期間之在職天數- 勤怠反映) / 365 天</span></td>
                                    </tr>
                                    <!-- 
							<tr>
								<td class="Body_label">勤怠反映:</td>
								<td class="Body_label">事假(家庭照顧假、無薪病假)1天 =&gt <font color="#FF0000">-1 </font>日在職天數</td>
							</tr>
							<tr>
								<td class="Body_label"></td>
								<td class="Body_label">有薪病假1天 =&gt  <font color="#FF0000">-1 </font>日在職天數</td>
							</tr>
							 -->
                                </table>

                            </td>
                        </tr>
                        <tr>
                            <th align="left" class="Body_TableHeader">勤怠反映:</th>
                            <td align="left" class="Body_label">
                                <table>
                                    <tr>
                                        <td class="Body_label">事假(家庭照顧假、無薪病假)1天 
                                        </td>
                                        <td class="Body_label"><span>=&gt <font color="#FF0000">
                                            <asp:Label ID="lb_B_LEAVE_UC" runat="server" Text=""></asp:Label>
                                        </font>日在職天數
                                        </span></td>
                                    </tr>
                                    <tr>
                                        <td class="Body_label">有薪病假1天[30(含)天以內]
                                        </td>
                                        <td class="Body_label"><span>=&gt  <font color="#FF0000">
                                            <asp:Label ID="lb_B_LEAVE_B" runat="server" Text=""></asp:Label></font>日在職天數
                                        </span></td>
                                    </tr>
                                    <tr>
                                        <td class="Body_label">有薪病假1天[30天以上]
                                        </td>
                                        <td class="Body_label"><span>=&gt  <font color="#FF0000">
                                            <asp:Label ID="lb_B_LEAVE_B_OVER30" runat="server" Text=""></asp:Label></font>日在職天數
                                        </span></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                        <tr>
                            <th align="left" class="Body_TableHeader">紀律反映:</th>
                            <td align="left" class="Body_label">
                                <table>
                                    <tr>
                                        <td class="Body_label">1.曠工:
                                        </td>
                                        <td class="Body_label"><span>1日 =&gt  <font color="#FF0000">
                                            <asp:Label ID="lb_B_LEAVE_Q" runat="server" Text=""></asp:Label></font>日紅利
                                        </span></td>
                                    </tr>
                                    <tr>
                                        <td class="Body_label">2.遲到早退:
                                        </td>
                                        <td class="Body_label"><span>19回(含)以上者，自第19回起1回  =&gt  <font color="#FF0000">
                                            <asp:Label ID="lb_B_LEAVE_OP" runat="server" Text=""></asp:Label></font>日紅利
                                        </span></td>
                                    </tr>
                                    <tr>
                                        <td class="Body_label">3.獎勵:
                                        </td>
                                        <td class="Body_label">
                                            <table>
                                                <tr>
                                                    <td class="Body_label"><span>嘉獎乙次 =&gt <font color="#FF0000">+
                                                    <asp:Label ID="lb_B_THIRD_CNT_P" runat="server" Text=""></asp:Label></font>日紅利</span></td>
                                                </tr>
                                                <tr>
                                                    <td class="Body_label"><span>記功乙次 =&gt <font color="#FF0000">+
                                                    <asp:Label ID="lb_B_SECOND_CNT_P" runat="server" Text=""></asp:Label></font>日紅利</span></td>
                                                </tr>
                                                <tr>
                                                    <td class="Body_label"><span>大功乙次 =&gt <font color="#FF0000">+
                                                    <asp:Label ID="lb_B_FIRST_CNT_P" runat="server" Text=""></asp:Label></font>日紅利</span></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Body_label">4.懲罰:
                                        </td>
                                        <td class="Body_label">
                                            <table>
                                                <tr>
                                                    <td class="Body_label"><span>申誡乙次 =&gt <font color="#FF0000">
                                                        <asp:Label ID="lb_B_THIRD_CNT_M" runat="server" Text=""></asp:Label></font>日紅利</span></td>
                                                </tr>
                                                <tr>
                                                    <td class="Body_label"><span>記過乙次 =&gt  <font color="#FF0000">
                                                        <asp:Label ID="lb_B_SECOND_CNT_M" runat="server" Text=""></asp:Label></font>日紅利</span></td>
                                                </tr>
                                                <tr>
                                                    <td class="Body_label"><span>大過乙次 =&gt  <font color="#FF0000">
                                                        <asp:Label ID="lb_B_FIRST_CNT_M" runat="server" Text=""></asp:Label></font>日紅利</span></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Body_label">5.備註:
                                        </td>
                                        <td class="Body_label">
                                            <table>
                                                <tr>
                                                    <td class="Body_label"><span>3支嘉獎(申誡) =&gt 1支小功(小過)</span></td>
                                                </tr>
                                                <tr>
                                                    <td class="Body_label"><span>3支小功(小過) =&gt 1支大功(大過)</span></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <!-- 頁籤 結束-------------------  -->
            <!--紀錄頁數(跳頁用)-->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
            <asp:HiddenField ID="hid_wfb2si_cancel_approve_mark" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2si_cancel_approve_mark%>" />
            <asp:HiddenField ID="hid_wfb2si_approve_ConfirmMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2si_approve_ConfirmMessage%>" />
            <asp:HiddenField ID="hid_wfb2si_required_Remark" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2si_required_Remark%>" />
            <asp:HiddenField ID="hid_wfb2si_reject_ConfirmMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2si_reject_ConfirmMessage%>" />
            <%--<asp:HiddenField ID="hid_wfb2si_Upd_NotChoiceMessage" runat="server" ClientIDMode="Static" />--%>
        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SI0300ExcelDown1" />
            <asp:PostBackTrigger ControlID="WFB2SI0300ExcelDown2" />
            <asp:PostBackTrigger ControlID="WFB2SI0300ExcelDown3" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

