<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sm/WFB2SM1200_Dtl.aspx.cs" Inherits="WebContent_fb2sm_WFB2SM1200_Dtl" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });

        function iniForm() {
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
        function CheckORDER_SEQ(source, arguments) {
            var re = /(?!0{3})\d{3}/;
            if ($("#txt_ORDER_SEQ_Add").val() != "") {
                if (!re.test($("#txt_ORDER_SEQ_Add").val()))
                    arguments.IsValid = false;
                else
                    arguments.IsValid = true;
            }
            else {
                arguments.IsValid = true;
            }
        }
        function CheckConfirmAction() {
            if (LookUpCheckboxs() == 0) {
                if (confirm($('#hidwfb2sm_CheckConfirmAction').val())) {
                    BlockUI();
                    return true;
                }
                else
                    return false;
            }
            else {
                alert($('#hidwfb2sm_Confirm_ChoiceMessage').val());
                return false;
            }
        }
        function CheckRejectAction() {
            var errormessage = "";
            if ($('#txt_REMARK_DESC').val().trim() == "" || $('#txt_REMARK_DESC').val() == null || $('#txt_REMARK_DESC').val().trim() == "　") //沒有填寫註記
                errormessage += $('#hidwfb2sm_RejectRemark_NotChoiceMessage').val() + "\r\n";
            //if (LookUpCheckboxs() == 0)  //沒有勾選
            //    errormessage += $('#hidwfb2sm_Reject_NotChoiceMessage').val();

            if (errormessage.trim().length > 0) {
                alert(errormessage);
                return false;
            }
            else {
                if (confirm($('#hidwfb2sm_CheckRejectAction').val())) {
                    BlockUI();
                    return true;
                }
                else
                    return false;
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
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%-- <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
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
                                    <%--年度--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DATA_YEAR" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_DATA_YEAR%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_DATA_YEAR_TXT" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                    <%--晉昇回數--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DATA_SEQ" runat="server" Text="<%$Resources:Resource,wfb2sm__lbDATA_SEQ%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_DATA_SEQ_TXT" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <%--提出核可日期--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_NOTICE_DT" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_NOTICE_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_NOTICE_DT_TXT" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                    <%--提出核可人員--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_NOTICE_BY" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_NOTICE_BY%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_NOTICE_BY_TXT" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <%--核可日期--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_APPROVE_DT" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_APPROVE_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_APPROVE_DT_TXT" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                    <%--核可人員--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_APPROVE_BY" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_APPROVE_BY%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_APPROVE_BY_TXT" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <%--備註說明--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_REMARK_DESC" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_REMARK_DESC%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="4">
                                        <asp:TextBox ID="txt_REMARK_DESC" Width="100%" Height="50px" runat="server" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <%--生效日期--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EXECUTIVE_DT" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_EXECUTIVE_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_EXECUTIVE_DT_TXT" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label"></td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>

                    <td align="right" class="Body_label">
                        <div id="init_grid">
                            <aces:Btn ID="WFB2SM1200Confirm" runat="server" Text="<%$Resources:Resource,wfb2sm_WFB2SM1200Confirm%>" OnClientClick="return CheckConfirmAction();" OnClick="WFB2SM120Confirm_Click" />
                            <aces:Btn ID="WFB2SM1200Reject" runat="server" Text="<%$Resources:Resource,wfb2sm_WFB2SM1200Reject%>" OnClientClick="return CheckRejectAction();" OnClick="WFB2SM120Reject_Click" />
                            <%--<asp:Button ID="WFB2SM1200Confirm" runat="server" Text="<%$Resources:Resource,wfb2sm_WFB2SM1200Confirm%>" OnClientClick="return CheckConfirmAction();" OnClick="WFB2SM120Confirm_Click" />
                            <asp:Button ID="WFB2SM1200Reject" runat="server" Text="<%$Resources:Resource,wfb2sm_WFB2SM1200Reject%>" OnClientClick="return CheckRejectAction();" OnClick="WFB2SM120Reject_Click" />--%>
                            <asp:Button ID="btn_back" OnClick="btn_back_Click" runat="server" ClientIDMode="Static" Text="<%$Resources:Resource,btn_back%>" />
                        </div>

                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getDtlData"
                SelectCountMethod="getDtlCount" TypeName="CFB2SM1200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="hid_QDATAKEY"
                        Name="hid_qdatakey" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="false" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" Width="1600px">
                <Columns>
                    <%--異常註記--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_error_check%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:CheckBox ID="cb_check" runat="server" meta:resourcekey="cb_checkResource1" ClientIDMode="AutoID" />
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:CheckBox ID="cb_check" runat="server" Checked="true" />
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--序號--%>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2sm_RowNumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--異動狀態--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_CHG_STATUS%>" SortExpression="P.CHG_STATUS" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lbl_CHG_STATUS" runat="server" Text='<%#Bind("CHG_STATUS")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--核可--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_PROCESS_STATUS%>" SortExpression="P.PROCESS_STATUS" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lbl_PROCESS_STATUS" runat="server" Text='<%#Bind("PROCESS_STATUS")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--部門--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_DEPT_NO%>" SortExpression="DEPT_NO" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lbl_DEPT_NO" runat="server" Text='<%#Bind("DEPT")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--工號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_EMP_ID%>" SortExpression="P.EMP_ID" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lbl_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--姓名--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_EMP_NAME%>" SortExpression="P.EMP_NAME" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lbl_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--職種--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_WS_CD%>" SortExpression="P.WS_CD" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lbl_WS_CD" runat="server" Text='<%#Bind("WS_CD")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--年資--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_WORK_YEARS%>" SortExpression="P.WORK_YEARS" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lbl_WORK_YEARS" runat="server" Text='<%#Bind("WORK_YEARS")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--原資格--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_LEVEL_CD%>" SortExpression="P.LEVEL_CD" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lbl_LEVEL_CD" runat="server" Text='<%#Bind("LEVEL_CD")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--原級數--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_GRADE_CD%>" SortExpression="P.GRADE_CD" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lbl_GRADE_CD" runat="server" Text='<%#Bind("GRADE_CD")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--原職稱--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_PJOB_CD%>" SortExpression="PJOB_CD" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lbl_PJOB_CD" runat="server" Text='<%#Bind("PJOB")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--資格年資--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_LEVEL_WORK_YEARS%>" SortExpression="P.LEVEL_WORK_YEARS" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lbl_LEVEL_WORK_YEARS" runat="server" Text='<%#Bind("LEVEL_WORK_YEARS")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--晉昇資格--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_LEVEL_CD_NEW%>" SortExpression="P.LEVEL_CD_NEW" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lbl_LEVEL_CD_NEW" runat="server" Text='<%#Bind("LEVEL_CD_NEW")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--晉昇級數--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_GRADE_CD_NEW%>" SortExpression="P.GRADE_CD_NEW" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lbl_GRADE_CD_NEW" runat="server" Text='<%#Bind("GRADE_CD_NEW")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--晉升職務--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_PJOB_CD_NEW%>" SortExpression="PJOB_CD_NEW" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lbl_PJOB_CD_NEW" runat="server" Text='<%#Bind("PJOB_NEW")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--在職區分--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_EMP_CHG_CD%>" SortExpression="EMP_CHG_CD" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lbl_EMP_CHG_CD" runat="server" Text='<%#Bind("EMP_CHG_CD_SUB")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--前一能力考核--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_ASSESS_SCORE_1%>" SortExpression="P.ASSESS_SCORE_1" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lbl_ASSESS_SCORE_1" runat="server" Text='<%#Bind("ASSESS_SCORE_1")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_ASSESS_SCORE_2%>" SortExpression="P.ASSESS_SCORE_2" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lbl_ASSESS_SCORE_2" runat="server" Text='<%#Bind("ASSESS_SCORE_2")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_ASSESS_SCORE_3%>" SortExpression="P.ASSESS_SCORE_3" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lbl_ASSESS_SCORE_3" runat="server" Text='<%#Bind("ASSESS_SCORE_3")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_ASSESS_SCORE_4%>" SortExpression="P.ASSESS_SCORE_4" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lbl_ASSESS_SCORE_4" runat="server" Text='<%#Bind("ASSESS_SCORE_4")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_ASSESS_SCORE_5%>" SortExpression="P.ASSESS_SCORE_5" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lbl_ASSESS_SCORE_5" runat="server" Text='<%#Bind("ASSESS_SCORE_5")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>

            <table id="total" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="true" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">
                    <td style="width: 5px"></td>
                    <td style="font-size: 14px;">
                        <asp:DropDownList ID="ddlPerPageRow" runat="server">
                            <asp:ListItem Text="每頁10000筆" Value="" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="lb_TotalCount" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hid_QDATAKEY" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_PPROCESS_STATUS" runat="server" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sm_Confirm_ChoiceMessage" Value="<%$Resources:Resource,wfb2sm_Confirm_ChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sm_Reject_NotChoiceMessage" Value="<%$Resources:Resource,wfb2sm_Reject_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sm_RejectRemark_NotChoiceMessage" Value="<%$Resources:Resource,wfb2sm_RejectRemark_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sm_CheckConfirmAction" Value="<%$Resources:Resource,wfb2sm_CheckConfirmAction%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sm_CheckRejectAction" Value="<%$Resources:Resource,wfb2sm_CheckRejectAction%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sm_Confirm_Clicksuccess" Value="<%$Resources:Resource,wfb2sm_Confirm_Clicksuccess%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sm_Reject_Clicksuccess" Value="<%$Resources:Resource,wfb2sm_Reject_Clicksuccess%>" />

            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


