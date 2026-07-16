<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2si/WFB2SI0200_Qry.aspx.cs" Inherits="WebContent_fb2si_WFB2SI0200_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            //$.unblockUI();
            iniForm();
        });
        var li_id;
        function iniForm() {
            countYear();
            $('.year').mask('9999');
            $(".day").mask('999.999');
            $.unblockUI();
            $("#tabs").tabs();
            $('#ul li').click(function () {
                li_id = $(this).attr('id');
            });
            if ($('#hid_Exec').val() == "" || $('#hid_Exec').val() == undefined) {
                ChangeTab(li_id);
            }
            else {
                ChangeTab(2);
            }
        }
        function ChangeTab(li_id) {
            $("#tabs").tabs({ active: li_id });
        }
        //紅利發放天數只能輸入數字
        function Checknum(source, arguments) {
            var re = /^[0-9]+$/;
            if (!re.test($("#txt_BONUS_DAYS").val()))
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }

        function DBcheck() {
            var processed = false;
            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            } else {
                return processed;
            }

            $.ajax({
                url: "WFB2SI0200_ExecuteCheck.ashx",
                data: {
                    BONUS_YEAR: $('#txt_BONUS_YEAR').val(),
                    BONUS_DAYS: $('#txt_BONUS_DAYS').val()
                },
                type: "GET",
                async: false,  //注意!!
                dataType: 'json',
                cache: false,
                success: function (JData) {
                    if (JData.errMsg != "") {   //EXCEPTION
                        alert(JData.errMsg);
                        processed = false;
                    } else {
                        if (JData.hid_H_BONUS_YEAR == "" || JData.hid_H_BONUS_YEAR == null) {
                            alert($('#hid_wfb2si_Gen_NotBonusYearMessage').val());
                            processed = false;
                        }
                        else if (JData.hid_D_BONUS_YEAR == "" || JData.hid_D_BONUS_YEAR == null) {
                            alert($('#hid_wfb2si_Gen_NotTargetGenMessage').val());
                            processed = false;
                        }
                        else if (JData.hid_H_FREEZE_FLAG == "Y") {
                            alert($('#hid_wfb2si_Gen_FreezeMessage').val());
                            processed = false;
                        }

                        else if (JData.hid_H_GEN_DT == "" || JData.hid_H_GEN_DT == null) {
                            if (confirm($('#hid_wfb2si_Gen_ConfirmTargetMessage').val()) == true) {
                                processed = true;
                            }
                        }
                        else {
                            if (confirm($('#hid_wfb2si_Gen_ConfirmMessage').val()) == true) {
                                processed = true;
                            }
                        }

                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                    processed = false;
                }
            });

            if (!processed)
                $.unblockUI();

            return processed;
        }
        function doClear() {
            $("#txt_BONUS_YEAR").val("");
            $("#txt_BONUS_DAYS").val("");
            $('#hid_Exec').val("");
            $("#txt_BONUS_DAYS").next().text("0月");
            //$('#cb_BONUS_ITEM_RP').attr("checked",true);
            //$('#cb_BONUS_ITEM_AL').attr("checked",true);
            //$('#cb_BONUS_ITEM_D').attr("checked",true);
            return false;
        }
        function countYear() {
            var value = $("#txt_BONUS_DAYS").val();
            if (isNaN(new Number(value))) {
                $("#txt_BONUS_DAYS").next().text("值必須為數字");
                return;
            }
            var month = formatFloat(value / 30, 2);
            $("#txt_BONUS_DAYS").next().text(month + "月");
        }
        function formatFloat(num, pos) {
            var size = Math.pow(10, pos);
            return Math.round(num * size) / size;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!--頁面table-->
            <table width="1020px" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <!--查詢條件table(HTML)-->
                        <!--TextBox,input button,DropDownList-->
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
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BONUS_YEAR" runat="server" Text="<%$Resources:Resource,wfb2si_BONUS_YEAR%>"></asp:Label>
                                    </th>
                                    <td colspan="3">
                                        <asp:TextBox ID="txt_BONUS_YEAR" runat="server" MaxLength="4" Width="64px" CssClass="MandatoryField year" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2si_required_BONUS_YEAR%>"
                                            ControlToValidate="txt_BONUS_YEAR" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2si__BONUS_YEAR_num%>" ControlToValidate="txt_BONUS_YEAR" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="\d\d\d\d" Display="None"></asp:RegularExpressionValidator>
                                    </td>

                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BONUS_DAYS" runat="server" Text="<%$Resources:Resource,wfb2si_BONUS_DAYS%>"></asp:Label>
                                    </th>
                                    <td colspan="3">
                                        <asp:TextBox ID="txt_BONUS_DAYS" runat="server" MaxLength="6" Width="64px" CssClass="MandatoryField day" ClientIDMode="Static" onblur="countYear();"></asp:TextBox>
                                        <span></span>
                                        <asp:RequiredFieldValidator ID="RequiredFieldVaslidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2si_required_BONUS_DAYS%>"
                                            ControlToValidate="txt_BONUS_DAYS" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <%--<asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2si__BONUS_DAYS_num%>" ClientValidationFunction="Checknum" ForeColor="Red"
                                            ControlToValidate="txt_BONUS_DAYS" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>--%>
                                        <!--需為數字-->
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="紅利發放天數必須為數字" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_BONUS_DAYS" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>

                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BONUS_ITEM" runat="server" Text="<%$Resources:Resource,wfb2si_BONUS_ITEM%>"></asp:Label>
                                    </th>
                                    <td colspan="3">
                                        <asp:CheckBox ID="cb_BONUS_ITEM_RP" runat="server" Checked="true" Text="<%$Resources:Resource,wfb2si_BONUS_ITEM_RP%>" ClientIDMode="Static" /><br />
                                        <asp:CheckBox ID="cb_BONUS_ITEM_AL" runat="server" Checked="true" Text="<%$Resources:Resource,wfb2si_BONUS_ITEM_AL%>" ClientIDMode="Static" /><br />
                                        <asp:CheckBox ID="cb_BONUS_ITEM_D" runat="server" Checked="true" Text="<%$Resources:Resource,wfb2si_BONUS_ITEM_D%>" ClientIDMode="Static" /><br />
                                        <asp:Label ID="lb_mark" runat="server" Text="<%$Resources:Resource,wfb2si_mark%>" Style="color: red;"></asp:Label>
                                    </td>

                                </tr>
                                <tr>
                                    <asp:HiddenField ID="hid_H_BONUS_YEAR" runat="server" ClientIDMode="Static" Value="" />
                                    <asp:HiddenField ID="hid_D_BONUS_YEAR" runat="server" ClientIDMode="Static" Value="" />
                                    <asp:HiddenField ID="hid_H_GEN_DT" runat="server" ClientIDMode="Static" Value="" />
                                    <asp:HiddenField ID="hid_Exec" runat="server" ClientIDMode="Static" Value="" />
                                    <asp:HiddenField ID="hid_H_FREEZE_FLAG" runat="server" ClientIDMode="Static" Value="" />
                                </tr>

                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">

                                            <aces:Btn ID="WFB2SI0200Execute" runat="server" Text="<%$Resources:Resource,wfb2si_do%>" OnClick="WFB2SI0200Execute_Click" OnClientClick="return DBcheck();" />
                                            <%--
                                            <asp:Button ID="WFB2SI0200Execute" runat="server" Text="<%$Resources:Resource,wfb2si_do%>" OnClick="WFB2SI0200Execute_Click" OnClientClick="return DBcheck();" />
                                            --%>
                                            <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2si_clear%>" OnClientClick="return doClear(); " />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>

                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
            </table>


            <!-- 頁籤 開始-------------------  -->
            <br />
            <div id="tabs" style="width: 1020px">
                <ul id="ul">
                    <li id="0"><a href="#bonusTarget">【紅利對象條件】</a></li>
                    <li id="1"><a href="#bonusBase">【紅利金額條件】</a></li>
                    <li id="2"><a href="#bonusResult">【計算結果】</a></li>
                </ul>
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
                                        <td class="Body_label">非自願性離職(死亡等)</td>
                                    </tr>
                                    <tr>
                                        <td class="Body_label">2.</td>
                                        <td class="Body_label">歸任日藉出向(外籍會社=HINO)</td>
                                    </tr>
                                    <tr>
                                        <td class="Body_label">3</td>
                                        <td class="Body_label">退休人員(支付狀態=N)</td>
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
                                        <td class="Body_label">外籍會社(TMC)</td>
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
                            <td align="left" class="Body_label">(發放base/日 * 發放天數 * 在職比例 ) <span style="font-family: WingDings2">&#177;</span>  紀律反應(不反映在職比例)
                            </td>
                        </tr>
                        <tr>
                            <th align="left" class="Body_TableHeader">發放base(7月調整後base):</th>
                            <td align="left" class="Body_label"><span>個人「職能俸 + 資格俸 + 職務津貼 + 專業津貼 + 伙食津貼」</span>
                            </td>
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
                <div id="bonusResult" style="display: none;" align="left">
                    <table>
                        <tr>
                            <th align="left" class="Body_TableHeader">紅利對象總人數:</th>
                            <td align="right" class="Body_label" width="100px">
                                <asp:Label ID="lb_BONUS_TOTAL_DECIMAL" runat="server" Text=""></asp:Label>人</td>
                            <th align="left" class="Body_TableHeader">紅利總金額：</th>
                            <td align="right" class="Body_label">
                                <asp:Label ID="lb_BONUS_TOTAL_AMOUNT" runat="server" Text=""></asp:Label>元</td>
                        </tr>

                    </table>
                </div>
            </div>
            <!-- 頁籤 結束-------------------  -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <!--resource)-->
            <asp:HiddenField ID="hid_wfb2si_Gen_NotBonusYearMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2si_Gen_NotBonusYearMessage%>" />
            <asp:HiddenField ID="hid_wfb2si_Gen_NotTargetGenMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2si_Gen_NotTargetGenMessage%>" />
            <asp:HiddenField ID="hid_wfb2si_Gen_ConfirmTargetMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2si_Gen_ConfirmTargetMessage%>" />
            <asp:HiddenField ID="hid_wfb2si_Gen_ConfirmMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2si_Gen_ConfirmMessage%>" />
            <asp:HiddenField ID="hid_wfb2si_Gen_FreezeMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2si_Gen_FreezeMessage%>" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
