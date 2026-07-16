<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2ha/WFB2HA0600_Update.aspx.cs" Inherits="WebContent_fb2ha_WFB2HA0600_Update" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });


        function iniForm() {
            $.unblockUI();
        }
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }


        //儲存前檢查
        function saveCheck() {
            var processed = true;
            var ipc = $("#ddl_INSURANCE_PROC_CD").val();//保險處理區分
            var ecs = $("#ddl_EMP_CHG_STATUS").val();//人事異動身份狀態
            if (ipc == "O" && ecs.length != 1) {
                alert("保險處理區分為O時，人事異動身份狀態僅能1碼");
                return false;
            }

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else {
                processed = false;
                return processed;
            }
            if (!processed) {
                $.unblockUI();
                return;
            }
            return processed;
        }

        function openQry() {
            window.location.href("WFB2HA0600_Qry.aspx");
            return false;
        }

    </script>

    <style type="text/css">
        .MandatoryField {
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <th>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_HR_CHG_CD" runat="server" Text="">人事異動代碼</asp:Label>:

                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Literal ID="lit_HR_CHG_CD" runat="server" ClientIDMode="Static"></asp:Literal>

                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_HR_CHG_DESC" runat="server" Text="">人事異動代碼說明</asp:Label>:

                                    </th>
                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:TextBox ID="txt_HR_CHG_DESC" runat="server" Width="150px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="請輸入人事異動代碼說明"
                                            ControlToValidate="txt_HR_CHG_DESC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>

                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_IS_VALID" runat="server" Text="">使用中</asp:Label>:

                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_IS_VALID" runat="server" ClientIDMode="Static" CssClass="MandatoryField">
                                            <asp:ListItem Value="Y">Y</asp:ListItem>
                                            <asp:ListItem Value="N">N</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>

                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_IS_FOR_BATCH" runat="server" Text="">一括異動適用</asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_IS_FOR_BATCH" runat="server" ClientIDMode="Static" CssClass="MandatoryField">
                                            <asp:ListItem Value="Y">Y-適用</asp:ListItem>
                                            <asp:ListItem Value="N">N-不適用</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_IS_FOR_TRANSFER_IN" runat="server" Text="">借調人員適用</asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_IS_FOR_TRANSFER_IN" runat="server" ClientIDMode="Static" CssClass="MandatoryField">
                                            <asp:ListItem Value="Y">Y-適用</asp:ListItem>
                                            <asp:ListItem Value="N">N-不適用</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_IS_SHOW" runat="server" Text="">人事履歷是否顯示</asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_IS_SHOW" runat="server" ClientIDMode="Static" CssClass="MandatoryField">
                                            <asp:ListItem Value="Y">Y</asp:ListItem>
                                            <asp:ListItem Value="N">N</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_IS_PROFESSION_PJOB" runat="server" Text="">是否專業職務</asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_IS_PROFESSION_PJOB" runat="server" ClientIDMode="Static" CssClass="MandatoryField">
                                            <asp:ListItem Value="Y">Y</asp:ListItem>
                                            <asp:ListItem Value="N">N</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>

                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_UPD_RIGHT_CD" runat="server" Text="權限區分"></asp:Label>
                                        : </th>
                                    <td align="left" class="Body_label" colspan="5">
                                        <asp:DropDownList ID="ddl_UPD_RIGHT_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField">
                                            <asp:ListItem Value="A">A</asp:ListItem>
                                            <asp:ListItem Value="D">D</asp:ListItem>
                                        </asp:DropDownList>
                                        【A】只有管理部可輸入的異動別；【D】各單位可輸入的異動別。
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_IS_INS_EARLIER" runat="server" Text="保險提前生效"></asp:Label>: 
                                    </th>
                                    <td align="left" class="Body_label" colspan="5">
                                        <asp:DropDownList ID="ddl_IS_INS_EARLIER" runat="server" ClientIDMode="Static" CssClass="MandatoryField">
                                            <asp:ListItem>Y</asp:ListItem>
                                            <asp:ListItem>N</asp:ListItem>
                                        </asp:DropDownList>
                                        【Y】保險預計處理日預設為空白；【N】保險預計處理日預設為異動生效日。
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_IS_TEMP" runat="server" Text="是否暫時狀態"></asp:Label>: 
                                    </th>
                                    <td align="left" class="Body_label" colspan="5">
                                        <asp:DropDownList ID="ddl_IS_TEMP" runat="server" ClientIDMode="Static" CssClass="MandatoryField">
                                            <asp:ListItem>Y</asp:ListItem>
                                            <asp:ListItem>E</asp:ListItem>
                                            <asp:ListItem>N</asp:ListItem>
                                        </asp:DropDownList>
                                        【Y】有起迄期間狀態之異動別，且為狀態開始；【E】暫時狀態結束；【N】無關暫時狀態。
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_IS_LEAVE" runat="server" Text="是否離社"></asp:Label>: 
                                    </th>
                                    <td align="left" class="Body_label" colspan="5">
                                        <asp:DropDownList ID="ddl_IS_LEAVE" runat="server" ClientIDMode="Static" CssClass="MandatoryField">
                                            <asp:ListItem>Y</asp:ListItem>
                                            <asp:ListItem>N</asp:ListItem>
                                            <asp:ListItem>X</asp:ListItem>
                                        </asp:DropDownList>
                                        【Y】自願離社；【N】非自願離社；【X】無關離社。
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_IS_UPD_HR" runat="server" Text="是否人事更新"></asp:Label>: 
                                    </th>
                                    <td align="left" class="Body_label" colspan="5">
                                        <asp:DropDownList ID="ddl_IS_UPD_HR" runat="server" ClientIDMode="Static" CssClass="MandatoryField">
                                            <asp:ListItem>Y</asp:ListItem>
                                            <asp:ListItem>N</asp:ListItem>
                                        </asp:DropDownList>
                                        異動生效後，【Y】更新員工人事主檔；【N】不更新。
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_IS_UPD_DEPT_HEAD" runat="server" Text="是否部門主管更新"></asp:Label>: 
                                    </th>
                                    <td align="left" class="Body_label" colspan="5">
                                        <asp:DropDownList ID="ddl_IS_UPD_DEPT_HEAD" runat="server" ClientIDMode="Static" CssClass="MandatoryField">
                                            <asp:ListItem>Y</asp:ListItem>
                                            <asp:ListItem>N</asp:ListItem>
                                        </asp:DropDownList>
                                        異動生效後，【Y】更新部門主管及員工主管；【N】不更新。
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_SALARY_PROC_CD" runat="server" Text="薪資處理區分"></asp:Label>: 
                                    </th>
                                    <td align="left" class="Body_label" colspan="5">
                                        <asp:DropDownList ID="ddl_SALARY_PROC_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField">
                                            <asp:ListItem>I</asp:ListItem>
                                            <asp:ListItem>O</asp:ListItem>
                                            <asp:ListItem>U</asp:ListItem>
                                            <asp:ListItem>N</asp:ListItem>
                                        </asp:DropDownList>
                                        異動生效後，通知薪資系統【I】新增；【O】結束；【U】變更；【N】無關； 敘薪資料。
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_INSURANCE_PROC_CD" runat="server" Text="保險處理區分"></asp:Label>: 
                                    </th>
                                    <td align="left" class="Body_label" colspan="5">
                                        <asp:DropDownList ID="ddl_INSURANCE_PROC_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField">
                                            <asp:ListItem>I</asp:ListItem>
                                            <asp:ListItem>O</asp:ListItem>
                                            <asp:ListItem>U</asp:ListItem>
                                            <asp:ListItem>N</asp:ListItem>
                                        </asp:DropDownList>
                                        異動生效後，通知保險系統【I】加保；【O】退保；【U】調整保險資料；【N】無關保險。
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_DUTY_PROC_CD" runat="server" Text="勤務處理區分"></asp:Label>: 
                                    </th>
                                    <td align="left" class="Body_label" colspan="5">
                                        <asp:DropDownList ID="ddl_DUTY_PROC_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField">
                                            <asp:ListItem>I</asp:ListItem>
                                            <asp:ListItem>O</asp:ListItem>
                                            <asp:ListItem>U</asp:ListItem>
                                            <asp:ListItem>N</asp:ListItem>
                                        </asp:DropDownList>
                                        異動生效後，通知勤務系統【I】產生；【O】刪除；【U】更新；【N】無關； 日勤務班表資料
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_CONTRACT_PROC_CD" runat="server" Text="期間工處理區分"></asp:Label>: 
                                    </th>
                                    <td align="left" class="Body_label" colspan="5">
                                        <asp:DropDownList ID="ddl_CONTRACT_PROC_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField">
                                            <asp:ListItem>I</asp:ListItem>
                                            <asp:ListItem>O</asp:ListItem>
                                            <asp:ListItem>U</asp:ListItem>
                                            <asp:ListItem>N</asp:ListItem>
                                        </asp:DropDownList>
                                        異動生效後，【I】產生；【O】刪除；【U】更新；【N】無關； 獎金發放計劃。
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_EMP_CHG_STATUS" runat="server" Text="人事異動身份狀態"></asp:Label>: 
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_EMP_CHG_STATUS" runat="server" ClientIDMode="Static" CssClass="MandatoryField">
                                        </asp:DropDownList>

                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_REMARK" runat="server" Text="備註"></asp:Label>: 
                                    </th>
                                    <td align="left" class="Body_label" colspan="4">
                                        <asp:TextBox ID="txt_REMARK" runat="server" Width="100%" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <aces:Btn ID="WFB2HA0600Save" runat="server" OnClick="WFB2HA0600Save_Click" OnClientClick="BlockUI();" ValidationGroup="GroupA" Text="<%$Resources:Resource,btn_Save%>"   />

                                        <%--<asp:Button ID="WFB2HA0600Save" runat="server" OnClick="WFB2HA0600Save_Click" OnClientClick="BlockUI();" ValidationGroup="GroupA"  Text="<%$Resources:Resource,btn_Save%>" />--%>

                                        <asp:Button ID="WFB2HA0600Clear" runat="server" OnClick="WFB2HA0600Clear_Click" Text="<%$Resources:Resource,btn_Cancel%>"  OnClientClick="return confirm('是否確定取消?');" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </th>
                </tr>
            </table>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ShowSummary="false" ValidationGroup="GroupA" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


