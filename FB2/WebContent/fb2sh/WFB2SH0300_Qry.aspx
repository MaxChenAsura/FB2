<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sh/WFB2SH0300_Qry.aspx.cs" Inherits="WebContent_WFB2SH0300_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            //$.unblockUI();
            iniForm();
        });
        var li_id;
        function iniForm() {
            $.unblockUI();
            $("#tabs").tabs();
            $(".day").mask('999.999');
            $(".day").css("text-align", "right").css("ime-mode", "disabled");
            $(".year").mask('9999');
            countYear();

            //執行後顯示 計算結果的tab
            $('#ul li').click(function () {
                li_id = $(this).attr('id');
            });
            if ($('#hid_Exec').val() == "" || $('#hid_Exec').val() == undefined) {
                ChangeTab(li_id);
            }
            else {
                ChangeTab(2);
            }

            $("#ddl_AWARD_ROUND").change(function () {
                var value =$(this).val();
                if (value == 1) {
                    $("#cb_AWARD_ITEM_A").prop("checked", false);
                    $("#cb_AWARD_ITEM_RP").prop("checked", false);
                    $("#cb_AWARD_ITEM_AL").prop("checked", false);
                    $("#cb_AWARD_ITEM_D").prop("checked", false);
                    
                }
                if (value == 2) {
                    $("#cb_AWARD_ITEM_A").prop("checked", true);
                    $("#cb_AWARD_ITEM_RP").prop("checked", true);
                    $("#cb_AWARD_ITEM_AL").prop("checked", true);
                    $("#cb_AWARD_ITEM_D").prop("checked", true);
                }
                if (value == 3) {
                    $("#cb_AWARD_ITEM_A").prop("checked", true);
                    $("#cb_AWARD_ITEM_RP").prop("checked", false);
                    $("#cb_AWARD_ITEM_AL").prop("checked", true);
                    $("#cb_AWARD_ITEM_D").prop("checked", false);
                }
            });


        }
        function ChangeTab(li_id) {
            $("#tabs").tabs({ active: li_id });
        }


        //執行檢核
        function executeCheck() {
            var processed = false;
            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            } else {
                return processed;
            }

            $.ajax({
                url: "WFB2SH0300_ExecuteCheck.ashx",
                data: {
                    AWARD_YEAR: $('#txt_AWARD_YEAR').val(),
                    AWARD_ROUND: $('#ddl_AWARD_ROUND').val()
                },
                type: "GET",
                async: false,  //注意!!
                dataType: 'json',
                cache: false,
                success: function (JData) {
                    if (JData.errMsg != "") {
                        alert(JData.errMsg);
                        processed = false;
                    }else {
                        if (JData.GEN_DT == "") {
                            if (confirm("是否已確認年獎發放對象資料？")) {
                                processed = true;
                            }
                        } else {
                            if (confirm("確定要進行年獎計算！ [ !!!注意：己維護資料會被覆蓋]")) {
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
            $("#txt_AWARD_YEAR").val("");
            $("#txt_AWARD_DAYS").val("");
            $('#hid_Exec').val("");
            $('[id$=lb_AWARD_TOTAL_DECIMAL]').text("");
            $('[id$=lb_AWARD_TOTAL_AMOUNT]').text("");
            $("#ddl_AWARD_ROUND").val("1");
            $("#cb_AWARD_ITEM_A").prop("checked", false);
            $("#cb_AWARD_ITEM_RP").prop("checked", false);
            $("#cb_AWARD_ITEM_AL").prop("checked", false);
            $("#cb_AWARD_ITEM_D").prop("checked", false);
            return false;
        }

        function countYear() {
            var value = $("#txt_AWARD_DAYS").val();
            var month = formatFloat(value / 30, 2);
            $("#txt_AWARD_DAYS").next().text(month + "月");
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
                                        <!-- 年度 -->
                                        <asp:Label ID="lb_AWARD_YEAR" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_award_year%>"></asp:Label>
                                    </th>
                                    <td colspan="3">
                                        <asp:TextBox ID="txt_AWARD_YEAR" runat="server" MaxLength="4" Width="64px" CssClass="MandatoryField year" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sh_required_award_year%>"
                                            ControlToValidate="txt_AWARD_YEAR" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <!--驗證長度需為4 -->	
							        	<asp:RegularExpressionValidator ID="RegularExpressionValidator_year_s" runat="server"
                                          ErrorMessage="<%$Resources:Resource,wfb2sh_format_award_year%>" ControlToValidate="txt_AWARD_YEAR" ForeColor="Red" ValidationGroup="GroupA"
                                          ValidationExpression=".{4,4}" Display="None"></asp:RegularExpressionValidator>
                                      
                                    </td>

                                </tr>
                                <tr>

                                    <th align="left" class="Body_TableHeader">
                                        <%--年獎回數--%>
                                        <asp:Label ID="Label11" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_award_round%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <%-- 
                                        <asp:DropDownList ID="ddl_AWARD_ROUND" runat="server" Width="60px" ClientIDMode="Static" CssClass="MandatoryField" OnSelectedIndexChanged="ddl_AWARD_ROUND_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            --%>
                                         <asp:DropDownList ID="ddl_AWARD_ROUND" runat="server" ClientIDMode="Static" CssClass="MandatoryField" ></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>

                                    <th align="left" class="Body_TableHeader">
                                        <%--年獎發放天數--%>
                                        <asp:Label ID="lb_AWARD_DAYS" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_award_days%>"></asp:Label>
                                    </th>
                                    <td colspan="3">
                                        <asp:TextBox ID="txt_AWARD_DAYS" runat="server" Width="64px" CssClass="MandatoryField day" ClientIDMode="Static" onblur="countYear();"></asp:TextBox>
                                        <span></span>
                                        <asp:RequiredFieldValidator ID="RequiredFieldVaslidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sh_required_award_days%>"
                                            ControlToValidate="txt_AWARD_DAYS" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                          <!--需為數字-->
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="<%$Resources:Resource,wfb2sh_format_award_days%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                        ControlToValidate="txt_AWARD_DAYS" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <%--反映項目--%>
                                        <asp:Label ID="lb_AWARD_ITEM" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_award_items%>"></asp:Label>
                                    </th>
                                    <td colspan="3">
                                        <asp:CheckBox ID="cb_AWARD_ITEM_A" runat="server" Checked="false" Text="<%$Resources:Resource,wfb2sh_lb_award_item_a%>" ClientIDMode="Static" /><br />
                                        <asp:CheckBox ID="cb_AWARD_ITEM_RP" runat="server" Checked="false" Text="<%$Resources:Resource,wfb2sh_lb_award_item_rp%>" ClientIDMode="Static" /><br />
                                        <asp:CheckBox ID="cb_AWARD_ITEM_AL" runat="server" Checked="false" Text="<%$Resources:Resource,wfb2sh_lb_award_item_al%>" ClientIDMode="Static" /><br />
                                        <asp:CheckBox ID="cb_AWARD_ITEM_D" runat="server" Checked="false" Text="<%$Resources:Resource,wfb2sh_lb_award_item_d%>" ClientIDMode="Static" /><br />
                                        <asp:Label ID="lb_mark" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_award_item%>" Style="color: red;"></asp:Label>
                                    </td>

                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                                
                                            <aces:Btn ID="WFB2SH0300Execute" runat="server" Text="執行" OnClick="WFB2SH0300Execute_Click" OnClientClick="return executeCheck();" />
                                           <%-- 
                                            <asp:Button ID="WFB2SH0300Execute" runat="server" Text="執行" OnClick="WFB2SH0300Execute_Click" OnClientClick="return executeCheck();" />
                                            --%>
                                            <asp:Button ID="btn_clear" runat="server" Text="清除" OnClientClick="return doClear();" />
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
                    <li id="0"><a href="#AWARDTarget">【年獎對象條件】</a></li>
                    <li id="1"><a href="#AWARDBase">【年獎金額條件】</a></li>
                    <li id="2"><a href="#AWARDResult">【計算結果】</a></li>
                </ul>
                <div id="AWARDTarget" align="left">
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
                                        <td class="Body_label">3.</td>
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
                                        <td class="Body_label">12月份(發放年度)入社新人[僅一回(先行分)]</td>
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

                <div id="AWARDBase" align="left">
                    <table border="1">
                        <tr>
                            <th align="left" class="Body_TableHeader">發放公式(一回):</th>
                            <td align="left" class="Body_label">發放base/日 * 在職比例  * <font color="red">實際 </font>發放天數  
                            </td>
                        </tr>
                        <tr>
                            <th align="left" class="Body_TableHeader">發放公式(二回):</th>
                            <td align="left" class="Body_label">發放base/日 * 在職比例  * <font color="red">實際 </font>發放天數 * 考績反映<span style="font-family: WingDings2">&#177;</span>  紀律反應 - 第一回發放金額
                            </td>
                        </tr>
                        <tr>
                            <th align="left" class="Body_TableHeader">發放公式(三回):</th>
                            <td align="left" class="Body_label">發放base/日 * 在職比例  * <font color="red">補足 </font>發放天數 * 考績反映
                            </td>
                        </tr>
                        <tr>
                            <th align="left" class="Body_TableHeader">發放base:</th>
                            <td align="left" class="Body_label">個人「職能俸 + 資格俸 + 職務津貼 + 專業津貼 + 伙食津貼」
                            </td>
                        </tr>
                        <tr>
                            <th align="left" class="Body_TableHeader">在職比例:</th>
                            <td align="left" class="Body_label">
                                <table>
                                    <tr>
                                        <td class="Body_label">比例:</td>
                                        <td class="Body_label">(yyyy/01/01 ~ yyyy/12/31期間之在職天數- 勤怠反映) / 365 天</td>
                                    </tr>
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
                                        <td class="Body_label">=&gt <font color="#FF0000">
                                            <asp:Label ID="lb_Y_LEAVE_UC" runat="server" Text=""></asp:Label>
                                        </font>日在職天數
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Body_label">有薪病假1天[30(含)天以內]
                                        </td>
                                        <td class="Body_label">=&gt  <font color="#FF0000">
                                            <asp:Label ID="lb_Y_LEAVE_B" runat="server" Text=""></asp:Label></font>日在職天數
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Body_label">有薪病假1天[30天以上]
                                        </td>
                                        <td class="Body_label">=&gt  <font color="#FF0000">
                                            <asp:Label ID="lb_Y_LEAVE_B_over30" runat="server" Text=""></asp:Label></font>日在職天數
                                        </td>
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
                                        <td class="Body_label">1日 =&gt  <font color="#FF0000">
                                            <asp:Label ID="lb_Y_LEAVE_Q" runat="server" Text=""></asp:Label></font>日年獎
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Body_label">2.遲到早退:
                                        </td>
                                        <td class="Body_label">19回(含)以上者，自第19回起1回  =&gt  <font color="#FF0000">
                                            <asp:Label ID="lb_Y_LEAVE_OP" runat="server" Text=""></asp:Label></font>日年獎
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Body_label">3.獎勵:
                                        </td>
                                        <td class="Body_label">
                                            <table>
                                                <tr>
                                                    <td class="Body_label">嘉獎乙次 =&gt <font color="#FF0000">+
                                                        <asp:Label ID="lb_Y_THIRD_CNT_P" runat="server" Text=""></asp:Label></font>日年獎</td>
                                                </tr>
                                                <tr>
                                                    <td class="Body_label">記功乙次 =&gt <font color="#FF0000">+
                                                        <asp:Label ID="lb_Y_SECOND_CNT_P" runat="server" Text=""></asp:Label></font>日年獎</td>
                                                </tr>
                                                <tr>
                                                    <td class="Body_label">大功乙次 =&gt <font color="#FF0000">+
                                                        <asp:Label ID="lb_Y_FIRST_CNT_P" runat="server" Text=""></asp:Label></font>日年獎</td>
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
                                                    <td class="Body_label">申誡乙次 =&gt <font color="#FF0000">
                                                        <asp:Label ID="lb_Y_THIRD_CNT_M" runat="server" Text=""></asp:Label></font>日年獎</td>
                                                </tr>
                                                <tr>
                                                    <td class="Body_label">記過乙次 =&gt  <font color="#FF0000">
                                                        <asp:Label ID="lb_Y_SECOND_CNT_M" runat="server" Text=""></asp:Label></font>日年獎</td>
                                                </tr>
                                                <tr>
                                                    <td class="Body_label">大過乙次 =&gt  <font color="#FF0000">
                                                        <asp:Label ID="lb_Y_FIRST_CNT_M" runat="server" Text=""></asp:Label></font>日年獎</td>
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
                                                    <td class="Body_label">3支嘉獎(申誡) =&gt <span>1支小功(小過)</span></td>
                                                </tr>
                                                <tr>
                                                    <td class="Body_label">3支小功(小過) =&gt <span>1支大功(大過)</span></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="AWARDResult" style="display: none;" align="left">
                    <table>
                        <tr>
                            <th align="left" class="Body_TableHeader">年獎對象總人數:</th>
                            <td align="right" class="Body_label" width="100px">
                                <asp:Label ID="lb_AWARD_TOTAL_DECIMAL" runat="server" Text=""  ClientIDMode="Static" ></asp:Label>人</td>
                            <th align="left" class="Body_TableHeader">年獎總金額：</th>
                            <td align="right" class="Body_label">
                                <asp:Label ID="lb_AWARD_TOTAL_AMOUNT" runat="server" Text="" ClientIDMode="Static"></asp:Label>元</td>
                        </tr>

                    </table>
                </div>
            </div>
            <!-- 頁籤 結束-------------------  -->
            <asp:HiddenField ID="hid_H_AWARD_YEAR" runat="server" ClientIDMode="Static" Value="" />
            <asp:HiddenField ID="hid_TARGET_GEN_DT" runat="server" ClientIDMode="Static" Value="" />
            <asp:HiddenField ID="hid_H_GEN_DT" runat="server" ClientIDMode="Static" Value="" />
            <asp:HiddenField ID="hid_Exec" runat="server" ClientIDMode="Static" Value="" />

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <%--<asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />--%>
            <!--resource)-->
            <asp:HiddenField ID="hid_wfb2si_Gen_NotAWARDYearMessage" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2si_Gen_NotTargetGenMessage" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2si_Gen_ConfirmTargetMessage" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2si_Gen_ConfirmMessage" runat="server" ClientIDMode="Static" />
            <%--<asp:HiddenField ID="hid_wfb2sk_Mod_NotChoiceMessage" runat="server" ClientIDMode="Static" />--%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
