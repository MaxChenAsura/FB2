<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2hf/WFB2HF0200_Dtl.aspx.cs" Inherits="WebContent_fb2hb_WFB2HF0200_Dtl" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
            //gridviewScroll();
        });
        function iniForm() {
            $("#tabs").tabs();
            $(".decimal").css("text-align", "right").css("ime-mode", "disabled");
            $(".num").mask('999');
            $(".year").mask('99');
            $(".jpnText").css("color", "blue").css("font-family", "MS Gothic");;
            $(".txtReadOnly").attr("ReadOnly", true).keydown(function () { return false });
            $(".ddlReadOnly").attr("ReadOnly", true).keydown(function () { return false });
            //$(".txtDisabled").css("background-color", "white").css("color", "black");
            $.unblockUI();
        }
        //凍結視窗用
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "600",
                height: "400",
                barcolor: "#7F7F7F"

            });
        }

        function move_item(from, to) {
            if (from == "lb_unselect") {
                var optionSize = $("[id*=lb_select] option").length;
                if (optionSize >= 3) {
                    alert("最多選3項");
                    return;
                }
            }
            $("[id*=" + from + "]").find(":selected").each(function () {
                $("[id*=" + to + "]").append($("<option></option>").attr("value", this.value).text(this.text));
            });
            //  移除選擇的項目
            $("[id*=" + from + "]").find(":selected").remove();
        }

        function returnDEPTValueToPage(dept_no, dept_name, value) {
            var returnValue = value;
            if (value == undefined) {
                returnValue = 'undefined';
            }
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                $("#" + dept_no).val(obj.DEPT_NO);
                $("#" + dept_name).val(obj.DEPT_NAME_20 + " " + obj.DEPT_NAME_30 + " " + obj.DEPT_NAME_40);
            }
        }

        //儲存前檢查
        function tempCheck() {
            var processed = true;
            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                processed = false;
            if (!processed) {
                $.unblockUI();
                return;
            }

            return processed;
        }

        //預計退休年齡需<=65
        function CheckLessThan(source, arguments) {
            var value = $.trim(arguments.Value);
            if (value > 65) {
                arguments.IsValid = false;
                return;
            }
            else {
                arguments.IsValid = true;
                return;
            }
        }

        function CheckArea(source, arguments) {
            var value = $.trim(arguments.Value);
            var select_text = "";
            var select_value = "";
            $("[id*=lb_select]").children("option").each(function () {
                select_text += this.text + ",";
                select_value += this.value + ",";
            });
            $("#hid_select_text").val(select_text);
            $("#hid_select_value").val(select_value);

            //檢核 欲發展之職能領域是否有選
            var optionSize = $("[id*=lb_select] option").length;
            if (optionSize == 0) {
                arguments.IsValid = false;
                return;
            } else {
                arguments.IsValid = true;
                return;
            }


        }
        //確定要取消
        function checkCancel(msg) {
            var processed = true;
            BlockUI();
            processed = confirm("確定要" + msg+"?");
            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="10%" />
                    <col width="20%" />
                    <col width="10%" />
                    <col width="20%" />
                    <col width="10%" />
                    <col width="20%" />
                    <col width="10%" />
                </colgroup>
                <tbody>
                    <tr>
                        <td colspan="6">I.基本資料 <asp:Label ID="lb_1" runat="server" Text="【個人基本情報】" CssClass="jpnText"/></td>
                    </tr>
                    <tr>
                        <%-- 年度 --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_DECLARA_YEAR" runat="server" Text="年度"></asp:Label></th>
                        <td>
                            <asp:TextBox ID="txt_DECLARA_YEAR" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" CssClass="txtReadOnly"></asp:TextBox>
                        </td>
                        <%-- 核可狀態 --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPROVE_STATUS" runat="server" Text="核可狀態"></asp:Label></th>
                        <td>
                            <asp:TextBox ID="txt_APPROVE_STATUS" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" CssClass="txtReadOnly"></asp:TextBox>
                        </td>
                        <th></th>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <%--工號 --%>
                        <th class="Body_TH1" style="width: 120px;">
                            <asp:Label ID="lb_EMP_ID" runat="server" Text="工號"></asp:Label></th>
                        <td class="Body_TR1" style="width: 100px;">
                            <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" CssClass="txtReadOnly"></asp:TextBox>
                        </td>
                        <%--姓名 --%>
                        <th class="Body_TH1" style="width: 120px;">
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text="姓名"></asp:Label></th>
                        <td class="Body_TR1" style="width: 100px;">
                            <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" CssClass="txtReadOnly"></asp:TextBox>
                        </td>
                        <%--資格 --%>
                        <th class="Body_TH1" style="width: 120px;">
                            <asp:Label ID="lb_LEVEL_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_LEVEL_CD%>"></asp:Label></th>
                        <td class="Body_TR1" style="width: 100px;">
                            <asp:TextBox ID="txt_LEVEL_CD" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" CssClass="txtReadOnly"></asp:TextBox>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <%--廠區 --%>
                        <th class="Body_TH1" style="width: 120px;">
                            <asp:Label ID="lb_PLANT_CD" runat="server" Text="工廠區分"></asp:Label></th>
                        <td class="Body_TR1" style="width: 100px;">
                            <asp:TextBox ID="txt_PLANT_CD" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" CssClass="txtReadOnly"></asp:TextBox>
                        </td>
                        <%--職種 --%>
                        <th class="Body_TH1" style="width: 120px;">
                            <asp:Label ID="lb_WS_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_WS_CD%>"></asp:Label></th>
                        <td class="Body_TR1" style="width: 100px;">
                            <asp:TextBox ID="txt_WS_CD" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" CssClass="txtReadOnly"></asp:TextBox>
                        </td>
                        <%--職務名稱 --%>
                        <th class="Body_TH1" style="width: 120px;">
                            <asp:Label ID="lb_PJOB_CD" runat="server" Text="職務名稱"></asp:Label></th>
                        <td class="Body_TR1" style="width: 100px;">
                            <asp:TextBox ID="txt_PJOB_CD" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" CssClass="txtReadOnly"></asp:TextBox>
                        </td>
                        <td></td>
                    </tr>
                    <tr>

                        <%--部門名稱 --%>
                        <th class="Body_TH1" style="width: 120px;">
                            <asp:Label ID="lb_DEPT_NAME" runat="server" Text="部門名稱"></asp:Label></th>
                        <td class="Body_TR1" style="width: 100px;" colspan="5">
                            <asp:TextBox ID="txt_DEPT_NAME" runat="server" MaxLength="10" Width="300px" BorderWidth="0" ClientIDMode="Static" CssClass="txtReadOnly"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <%--年齡 --%>
                        <th class="Body_TH1" style="width: 150px;">
                            <asp:Label ID="lb_AGE" runat="server" Text="年齡"></asp:Label></th>
                        <td class="Body_TR1" style="width: 100px;">
                            <asp:TextBox ID="txt_AGE" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" CssClass="txtReadOnly"></asp:TextBox>
                        </td>
                        <%--在職年資 --%>
                        <th class="Body_TH1" style="width: 120px;">
                            <asp:Label ID="lb_WORK_YEARS" runat="server" Text="在職年資"></asp:Label></th>
                        <td class="Body_TR1" style="width: 100px;">
                            <asp:TextBox ID="txt_WORK_YEARS" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" CssClass="txtReadOnly"></asp:TextBox>
                        </td>
                        <%-- 資格年資--%>
                        <th class="Body_TH1" style="width: 120px;">
                            <asp:Label ID="lb_RECENT_LEVEL_WORK_YEARS" runat="server" Text="資格年資"></asp:Label></th>
                        <td class="Body_TR1" style="width: 100px;">
                            <asp:TextBox ID="txt_RECENT_LEVEL_WORK_YEARS" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" CssClass="txtReadOnly"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <%-- 日文--%>
                        <th class="Body_TH1" style="width: 120px;">
                            <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_total_score_jpn%>"></asp:Label></th>
                        <td class="Body_TR1" style="width: 100px;">
                            <asp:TextBox ID="txt_JLPT" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" CssClass="txtReadOnly"></asp:TextBox>
                        </td>
                        <%-- 英文TOEIC--%>
                        <th class="Body_TH1" style="width: 120px;">
                            <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_total_score_toeic%>"></asp:Label></th>
                        <td class="Body_TR1" style="width: 100px;">
                            <asp:TextBox ID="txt_TOEIC" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" CssClass="txtReadOnly"></asp:TextBox>
                        </td>
                        <th></th>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td align="right" colspan="7">
                            <asp:Button ID="WFB2HF0100Cancel" runat="server" Text="返回" Visible="true" OnClick="WFB2HF0100Cancel_Click" OnClientClick="" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">II.現職工作內容 <asp:Label ID="Label6" runat="server" Text="【担当仕事】" CssClass="jpnText"/></td>
                    </tr>
                    <tr>
                        <td class="Body_TR1" colspan="7">1.主要擔當業務(百分比) <asp:Label ID="Label7" runat="server" Text="【仕事属性区分】" CssClass="jpnText"/> </td>
                    </tr>
                    <tr>
                        <td class="Body_TR1" colspan="7">
                            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData_EDIT1"
                                SelectCountMethod="getCount_EDIT1" TypeName="CFB2HF0200DAO" EnablePaging="True"
                                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                                OnSelected="ods1_Selected">
                                <SelectParameters>
                                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                                    <asp:Parameter Name="maximumRows" Type="Int32" />
                                    <asp:ControlParameter ControlID="txt_DECLARA_YEAR"
                                    Name="declara_year" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                                    <asp:ControlParameter ControlID="txt_EMP_ID"
                                    Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="480px">
                                <Columns>
                                    <%--序號--%>
                                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_rownumber%>" HeaderStyle-Width="40px">
                                        <ItemTemplate>
                                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--年度--%>
                                    <asp:TemplateField HeaderText="項目" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lb_BIZ_ITEM" runat="server" Text='<%#Bind("BIZ_ITEM")%>' Width="200px"></asp:Label>
                                            <asp:HiddenField ID="hid_BIZ_TYPE" runat="server" Value='<%#Bind("BIZ_TYPE")%> ' ClientIDMode="Static" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--百分比(本人)--%>
                                    <asp:TemplateField HeaderText="評價(本人)" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="txt_NEW_BIZ_PERCENT" runat="server" Text='<%#Bind("BIZ_PERCENT")%>' ClientIDMode="Static" ></asp:Label>
                                             <asp:Label ID="lb_percent1" runat="server" Text='%' Width="15px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                      <%--百分比(主管)--%>
                                    <asp:TemplateField HeaderText="評價(主管)" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="txt_NEW_BIZ_PERCENT_P" runat="server" Text='<%#Bind("BIZ_PERCENT_P")%>' ClientIDMode="Static"></asp:Label>
                                             <asp:Label ID="lb_percent2" runat="server" Text='%' Width="15px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="GridviewScrollHeader" />
                                <PagerStyle CssClass="GridviewScrollPager" />

                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td class="Body_TR1" colspan="7">2.業務內容簡述(請以條列式陳述具體內容-至少必填1項,至多3項,最大長度為50) <asp:Label ID="Label8_J" runat="server" Text="【主な業務内容】" CssClass="jpnText"/> </td>
                    </tr>
                    <tr>
                        <th class="Body_TH1" style="width: 120px;">
                            <asp:Label ID="lb_BIZ_C1" runat="server" Text="第1項"></asp:Label></th>
                        <td  class="Body_TR1" colspan="6">
                            <asp:TextBox ID="txt_BIZ_C1" runat="server" MaxLength="50" Width="100%" ClientIDMode="Static"  CssClass="txtReadOnly" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th class="Body_TH1" style="width: 120px;">
                            <asp:Label ID="lb_BIZ_C2" runat="server" Text="第2項"></asp:Label></th>
                        <td class="Body_TR1" colspan="6">
                            <asp:TextBox ID="txt_BIZ_C2" runat="server" MaxLength="50" Width="100%" ClientIDMode="Static"  CssClass="txtReadOnly"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th class="Body_TH1" style="width: 120px;">
                            <asp:Label ID="lb_BIZ_C3" runat="server" Text="第3項"></asp:Label></th>
                        <td class="Body_TR1" colspan="6">
                            <asp:TextBox ID="txt_BIZ_C3" runat="server" MaxLength="50" Width="100%" ClientIDMode="Static" CssClass="txtReadOnly"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="Body_TR1" colspan="7">3.工作評價(5極高、4高、3普通、2低、1極低) <asp:Label ID="Label14" runat="server" Text="【仕事評価】" CssClass="jpnText"/> </td>
                    </tr>
                    <tr>
                        <td class="Body_TR1" colspan="7">
                            <asp:ObjectDataSource ID="ods2" runat="server" SelectMethod="getData_EDIT2"
                                SelectCountMethod="getCount_EDIT2" TypeName="CFB2HF0200DAO" EnablePaging="True"
                                SortParameterName="sortExpression2" OnSelecting="obs1_Selecting2"
                                OnSelected="ods1_Selected2">
                                <SelectParameters>
                                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                                    <asp:Parameter Name="maximumRows" Type="Int32" />
                                    <asp:ControlParameter ControlID="txt_DECLARA_YEAR"
                                    Name="declara_year" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                                    <asp:ControlParameter ControlID="txt_EMP_ID"
                                    Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:GridView ID="gv_result2" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result2_Sorting"
                                OnRowDataBound="gv_result2_RowDataBound" OnRowCreated="gv_result2_RowCreated" Width="480px">
                                <Columns>
                                    <%--序號--%>
                                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_rownumber%>" HeaderStyle-Width="40px">
                                        <ItemTemplate>
                                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--項目--%>
                                    <asp:TemplateField HeaderText="項目" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lb_WORK_ITEM" runat="server" Text='<%#Bind("WORK_ITEM")%>' Width="200px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--評價--%>
                                    <asp:TemplateField HeaderText="評價(本人)" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lb_WORK_GRADES" runat="server" Text='<%#Bind("WORK_GRADES")%> ' ClientIDMode="Static" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <%--評價--%>
                                    <asp:TemplateField HeaderText="評價(主管)" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lb_WORK_GRADES_P" runat="server" Text='<%#Bind("WORK_GRADES_P")%> ' ClientIDMode="Static" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="GridviewScrollHeader" />
                                <PagerStyle CssClass="GridviewScrollPager" />

                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">III.職涯規劃 <asp:Label ID="Label15" runat="server" Text="【キャリアプラン】" CssClass="jpnText"/></td>
                    </tr>
                    <tr>
                        <td class="Body_TR1" colspan="7">1.職涯規劃 <asp:Label ID="Label16" runat="server" Text="【キャリアプラン】" CssClass="jpnText"/> </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <table cellspacing="1" cellpadding="1" border="0" class="Body_Label">
                                <tr>
                                    <th style="width: 40px;"></th>
                                    <th class="Body_TH1" style="width: 200px;">
                                        <asp:Label ID="Label3" runat="server" Text="①欲朝何職種/職位發展"></asp:Label></th>
                                    <td class="Body_TR1">
                                        <asp:DropDownList ID="ddl_PJOB_DEVE_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="欲朝何職種/職位發展不可空白"
                                            ControlToValidate="ddl_PJOB_DEVE_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <table cellspacing="1" cellpadding="1" border="0" class="Body_Label">
                                <tr>
                                    <th style="width: 40px;"></th>
                                    <th class="Body_TH1" style="width: 200px;">
                                        <asp:Label ID="Label5" runat="server" Text="②預計退休年齡(上限65)"></asp:Label></th>
                                    <td class="Body_TR1">
                                        <asp:TextBox ID="txt_RETIRE_AGE" runat="server" MaxLength="2" Width="60px" ClientIDMode="Static"  CssClass="txtReadOnly"></asp:TextBox>
                                        <asp:HiddenField ID="hid_RETIRE_AGE_MAX" runat="server" Value='65' ClientIDMode="Static" />
                                        <asp:RequiredFieldValidator ID="NEW_RETIRE_AGE" runat="server" ErrorMessage="預計退休年齡不可空白"
                                            ControlToValidate="txt_RETIRE_AGE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="NEW_RETIRE_AGE2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="預計退休年齡需為數字" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_RETIRE_AGE" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="NEW_RETIRE_AGE3" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="預計退休年齡需<=65" ClientValidationFunction="CheckLessThan" ForeColor="Red"
                                            ControlToValidate="txt_RETIRE_AGE" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <table cellspacing="1" cellpadding="1" border="0" width="800px" class="Body_Label">
                                <tr>
                                    <th style="width: 40px;"></th>
                                    <th class="Body_TH1" style="width: 300px;">
                                        <asp:Label ID="Label4" runat="server" Text="③欲發展之職能領域(複選，最多三項)"></asp:Label></th>
                                    <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="欲發展之職能領域至少選1項" ClientValidationFunction="CheckArea" ForeColor="Red"
                                        ControlToValidate="txt_DECLARA_YEAR" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    <td colspan="4"> 
                                         <asp:TextBox ID="txt_COMPET_AREA" runat="server" Width="300px" BorderWidth="0" ClientIDMode="Static" CssClass="txtReadOnly"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="Body_TR1" colspan="7">2.能力養成計畫(為了達到個人職涯規劃、目標，所需培養之能力及自我發展計畫。例如語學、專門技能、證照、企劃能力等) <asp:Label ID="lb_17" runat="server" Text="【能力養成計画】" CssClass="jpnText"/></td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <table cellspacing="1" cellpadding="1" border="1" width="1020px" class="Body_Label">
                                <thead>
                                    <th class="Body_TH1" style="width: 40px; text-align: center;">序號</th>
                                    <th class="Body_TH1" style="width: 200px; text-align: center;">欲培養能力</th>
                                    <th class="Body_TH1" style="width: 500px; text-align: center;">養成計畫</th>
                                    <th class="Body_TH1" style="width: 100px; text-align: center;">預計完成(年)</th>
                                    <th class="Body_TH1" style="width: 100px; text-align: center;">預計完成(月)</th>
                                </thead>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lb_Row1" runat="server" Text='1' Width="40px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_DEV_ABILITY1" runat="server" MaxLength="50" Width="98%" ClientIDMode="Static" CssClass="txtReadOnly"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_DEV_PLAN1" runat="server" MaxLength="50" Width="98%" ClientIDMode="Static" CssClass="txtReadOnly"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_PREDICT_YEAR1" runat="server" Width="98%" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_PREDICT_MONTH1" runat="server" Width="98%" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lb_Row2" runat="server" Text='2' Width="40px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_DEV_ABILITY2" runat="server" MaxLength="50" Width="98%" ClientIDMode="Static" CssClass="txtReadOnly"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_DEV_PLAN2" runat="server" MaxLength="50" Width="98%" ClientIDMode="Static" CssClass="txtReadOnly"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_PREDICT_YEAR2" runat="server" Width="98%" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_PREDICT_MONTH2" runat="server" Width="98%" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lb_Row3" runat="server" Text='3' Width="40px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_DEV_ABILITY3" runat="server" MaxLength="50" Width="98%" ClientIDMode="Static" CssClass="txtReadOnly"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_DEV_PLAN3" runat="server" MaxLength="50" Width="98%" ClientIDMode="Static" CssClass="txtReadOnly"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_PREDICT_YEAR3" runat="server" Width="98%" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_PREDICT_MONTH3" runat="server" Width="98%" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    </tr>
                    <tr>
                        <td class="Body_TR1" colspan="7">3.基於職涯規劃與能力發展，個人未來希望之異動/業務調整 <asp:Label ID="Label18_J" runat="server" Text="【異動・業務調整要望】" CssClass="jpnText"/></td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <table cellspacing="1" cellpadding="1" border="0" width="100%" class="Body_Label">
                                <tr>
                                    <th class="Body_TH1" style="width: 80px;">
                                        <asp:Label ID="Label9" runat="server" Text="第1順位"></asp:Label></th>
                                    <td style="width: 160px;">
                                        <asp:DropDownList ID="ddl_BIZ_CHG_TYPE1" runat="server" ClientIDMode="Static" Width="120px"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_BIZ_CHG_TYPE1_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lb_CHG_DEP1" runat="server" Text="異動單位"></asp:Label>
                                        <asp:TextBox ID="txt_CHG_DEPT_NO1" runat="server" Width="70px" ClientIDMode="Static" MaxLength="7" CssClass="txtReadOnly"
                                            AutoPostBack="true" OnTextChanged="txt_CHG_DEPT_NO1_TextChanged"></asp:TextBox>
                                        <input id="bt_CHG_DEPT_NO1" type="button" value="..." onclick="OpenDeptSearch('txt_CHG_DEPT_NO1', 'txt_CHG_DEPT_NAME1', 'N', 'Y');" disabled="disabled"/>
                                        <asp:TextBox ID="txt_CHG_DEPT_NAME1" runat="server" Width="260px" ClientIDMode="Static" BorderWidth="0" CssClass="txtReadOnly"></asp:TextBox>
                                    </td>
                                    <td>ICT受入公司:<asp:DropDownList ID="ddl_ICT_COMPANY_CD1" runat="server" Width="120px" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                   <td class="Body_TR1" colspan="4">
                                        欲擔當工作內容
                                        <asp:TextBox ID="txt_WORK_C1" runat="server" MaxLength="50" Width="100%" ClientIDMode="Static" CssClass="txtReadOnly"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th class="Body_TH1" style="width: 80px;">
                                        <asp:Label ID="Label10" runat="server" Text="第2順位"></asp:Label></th>
                                    <td>
                                        <asp:DropDownList ID="ddl_BIZ_CHG_TYPE2" runat="server" Width="120px" ClientIDMode="Static" 
                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_BIZ_CHG_TYPE2_SelectedIndexChanged"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label12" runat="server" Text="異動單位"></asp:Label>
                                        <asp:TextBox ID="txt_CHG_DEPT_NO2" runat="server" Width="70px" ClientIDMode="Static" MaxLength="7"  CssClass="txtReadOnly"
                                            AutoPostBack="true" OnTextChanged="txt_CHG_DEPT_NO2_TextChanged"></asp:TextBox>
                                        <input id="bt_CHG_DEPT_NO2" type="button" value="..." onclick="OpenDeptSearch('txt_CHG_DEPT_NO2', 'txt_CHG_DEPT_NAME2', 'N', 'Y');" disabled="disabled"/>
                                        <asp:TextBox ID="txt_CHG_DEPT_NAME2" runat="server" Width="260px" ClientIDMode="Static" BorderWidth="0" CssClass="txtReadOnly"></asp:TextBox>
                                    </td>
                                    <td>ICT受入公司:<asp:DropDownList ID="ddl_ICT_COMPANY_CD2" runat="server" Width="120px" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                 <tr>
                                   <td class="Body_TR1" colspan="4">
                                        欲擔當工作內容
                                        <asp:TextBox ID="txt_WORK_C2" runat="server" MaxLength="50" Width="100%" ClientIDMode="Static" CssClass="txtReadOnly"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th class="Body_TH1" style="width: 80px;">
                                        <asp:Label ID="Label11" runat="server" Text="第3順位"></asp:Label></th>
                                    <td>
                                        <asp:DropDownList ID="ddl_BIZ_CHG_TYPE3" runat="server" Width="120px" ClientIDMode="Static"
                                             AutoPostBack="true" OnSelectedIndexChanged="ddl_BIZ_CHG_TYPE3_SelectedIndexChanged"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label13" runat="server" Text="異動單位"></asp:Label>
                                        <asp:TextBox ID="txt_CHG_DEPT_NO3" runat="server" Width="70px" ClientIDMode="Static" MaxLength="7" CssClass="txtReadOnly"
                                            AutoPostBack="true" OnTextChanged="txt_CHG_DEPT_NO3_TextChanged"></asp:TextBox>
                                        <input id="bt_CHG_DEPT_NO3" type="button" value="..." onclick="OpenDeptSearch('txt_CHG_DEPT_NO3', 'txt_CHG_DEPT_NAME3', 'N', 'Y');" disabled="disabled"/>
                                        <asp:TextBox ID="txt_CHG_DEPT_NAME3" runat="server" Width="260px" ClientIDMode="Static" BorderWidth="0" CssClass="txtReadOnly"></asp:TextBox>
                                    </td>
                                    <td>ICT受入公司:<asp:DropDownList ID="ddl_ICT_COMPANY_CD3" runat="server" Width="120px" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Body_TR1" colspan="4">
                                        欲擔當工作內容
                                        <asp:TextBox ID="txt_WORK_C3" runat="server" MaxLength="50" Width="100%" ClientIDMode="Static" CssClass="txtReadOnly"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="Body_TR1" colspan="7">4.希望異動/調整時間點 <asp:Label ID="Label28" runat="server" Text="【異動タイミング】" CssClass="jpnText"/><asp:DropDownList ID="ddl_ADJUST_TIME" runat="server" Width="60px" ClientIDMode="Static"></asp:DropDownList>
                            年
                        </td>
                    </tr>
                    <tr>
                        <td class="Body_TR1" colspan="7">5.上述異動/業務調整之理由(無異動計畫者亦須填寫理由) <asp:Label ID="Label29" runat="server" Text="【理由説明】" CssClass="jpnText"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="Body_TR1" colspan="7">
                            <asp:TextBox TextMode="MultiLine" Rows="3" ID="txt_ADJUST_REASON" runat="server" Width="100%" BorderWidth="1" Style="overflow: auto" CssClass="txtReadOnly"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="業務調整之理由不可空白"
                                ControlToValidate="txt_ADJUST_REASON" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">IV.其他自我申告事項(健康、家庭或其他事項等) <asp:Label ID="Label30" runat="server" Text="【その他申告事項】" CssClass="jpnText"/>
                        </td>
                    </tr>
                   <tr>
                        <td class="Body_TR1"colspan="7">
                            健康狀況
                            <asp:RadioButtonList ID="rbl_HEALTH_STATUS" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" ClientIDMode="Static" >
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="Body_TR1"colspan="7">
                            申告事項記入
                        </td>
                    </tr>
                    <tr>
                        <td class="Body_TR1" colspan="7">
                            <asp:TextBox TextMode="MultiLine" Rows="3" ID="txt_REMARK" runat="server" Width="100%" BorderWidth="1" Style="overflow: auto" CssClass="txtReadOnly"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">V.主管意見 <asp:Label ID="Label32" runat="server" Text="【上司コメント】" CssClass="jpnText"/></td>
                    </tr>
                        <td class="Body_TR1" colspan="7">1.能力養成計畫(為了達到個人職涯規劃、目標，所需培養之能力及自我發展計畫。例如語學、專門技能、證照、企劃能力等) <asp:Label ID="Label8" runat="server" Text="【能力養成計画】" CssClass="jpnText"/></td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <table cellspacing="1" cellpadding="1" border="1" width="1020px" class="Body_Label">
                                <thead>
                                    <th class="Body_TH1" style="width: 40px; text-align: center;">序號</th>
                                    <th class="Body_TH1" style="width: 200px; text-align: center;">須培養能力</th>
                                    <th class="Body_TH1" style="width: 500px; text-align: center;">養成計畫</th>
                                    <th class="Body_TH1" style="width: 100px; text-align: center;">預計完成(年)</th>
                                    <th class="Body_TH1" style="width: 100px; text-align: center;">預計完成(月)</th>
                                </thead>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="Label17" runat="server" Text='1' Width="40px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_P_DEV_ABILITY1" runat="server" MaxLength="50" Width="98%" ClientIDMode="Static"  CssClass="txtReadOnly"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_P_DEV_PLAN1" runat="server" MaxLength="50" Width="98%" ClientIDMode="Static"  CssClass="txtReadOnly"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_P_PREDICT_YEAR1" runat="server" Width="98%" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_P_PREDICT_MONTH1" runat="server" Width="98%" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="Label18" runat="server" Text='2' Width="40px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_P_DEV_ABILITY2" runat="server" MaxLength="50" Width="98%" ClientIDMode="Static" CssClass="txtReadOnly" ></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_P_DEV_PLAN2" runat="server" MaxLength="50" Width="98%" ClientIDMode="Static" CssClass="txtReadOnly"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_P_PREDICT_YEAR2" runat="server" Width="98%" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_P_PREDICT_MONTH2" runat="server" Width="98%" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="Label19" runat="server" Text='3' Width="40px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_P_DEV_ABILITY3" runat="server" MaxLength="50" Width="98%" ClientIDMode="Static" CssClass="txtReadOnly"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_P_DEV_PLAN3" runat="server" MaxLength="50" Width="98%" ClientIDMode="Static" CssClass="txtReadOnly"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_P_PREDICT_YEAR3" runat="server" Width="98%" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_P_PREDICT_MONTH3" runat="server" Width="98%" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    </tr>
                    <tr>
                        <td class="Body_TR1" colspan="7">2.基於職涯規劃與能力發展，個人未來希望之異動/業務調整 <asp:Label ID="Label26" runat="server" Text="【異動・業務調整要望】" CssClass="jpnText"/></td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <table cellspacing="1" cellpadding="1" border="0" width="100%" class="Body_Label">
                                <tr>
                                    <th class="Body_TH1" style="width: 80px;">
                                        <asp:Label ID="Label20" runat="server" Text="第1順位"></asp:Label></th>
                                    <td style="width: 160px;">
                                        <asp:DropDownList ID="ddl_P_BIZ_CHG_TYPE1" runat="server" ClientIDMode="Static" Width="120px"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_P_BIZ_CHG_TYPE1_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label21" runat="server" Text="異動單位"></asp:Label>
                                        <asp:TextBox ID="txt_P_CHG_DEPT_NO1" runat="server" Width="70px" ClientIDMode="Static" MaxLength="7" CssClass="txtReadOnly"
                                            AutoPostBack="true" OnTextChanged="txt_P_CHG_DEPT_NO1_TextChanged"></asp:TextBox>
                                         <asp:Button id="btn_P_CHG_DEPT_NO1" runat="server" Text="..." OnClientClick="OpenDeptSearch('txt_P_CHG_DEPT_NO1', 'txt_P_CHG_DEPT_NAME1', 'Y', 'Y');" CssClass="txtReadOnly" />
                                        <asp:TextBox ID="txt_P_CHG_DEPT_NAME1" runat="server" Width="260px" ClientIDMode="Static" BorderWidth="0" CssClass="txtReadOnly"></asp:TextBox>
                                    </td>
                                    <td>ICT受入公司:<asp:DropDownList ID="ddl_P_ICT_COMPANY_CD1" runat="server" Width="120px" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                   <td class="Body_TR1" colspan="4">
                                        建議擔當工作內容
                                        <asp:TextBox ID="txt_P_WORK_C1" runat="server" MaxLength="50" Width="100%" ClientIDMode="Static" CssClass="txtReadOnly"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th class="Body_TH1" style="width: 80px;">
                                        <asp:Label ID="Label22" runat="server" Text="第2順位"></asp:Label></th>
                                    <td>
                                        <asp:DropDownList ID="ddl_P_BIZ_CHG_TYPE2" runat="server" Width="120px" ClientIDMode="Static" 
                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_P_BIZ_CHG_TYPE2_SelectedIndexChanged"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label23" runat="server" Text="異動單位"></asp:Label>
                                        <asp:TextBox ID="txt_P_CHG_DEPT_NO2" runat="server" Width="70px" ClientIDMode="Static" MaxLength="7"  CssClass="txtReadOnly"
                                            AutoPostBack="true" OnTextChanged="txt_P_CHG_DEPT_NO2_TextChanged"></asp:TextBox>
                                        <asp:Button id="btn_P_CHG_DEPT_NO2" runat="server" Text="..." OnClientClick="OpenDeptSearch('txt_P_CHG_DEPT_NO2', 'txt_P_CHG_DEPT_NAME2', 'Y', 'Y');" CssClass="txtReadOnly" />
                                        <asp:TextBox ID="txt_P_CHG_DEPT_NAME2" runat="server" Width="260px" ClientIDMode="Static" BorderWidth="0" CssClass="txtReadOnly"></asp:TextBox>
                                    </td>
                                    <td>ICT受入公司:<asp:DropDownList ID="ddl_P_ICT_COMPANY_CD2" runat="server" Width="120px" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                 <tr>
                                   <td class="Body_TR1" colspan="4">
                                        建議擔當工作內容
                                        <asp:TextBox ID="txt_P_WORK_C2" runat="server" MaxLength="50" Width="100%" ClientIDMode="Static" CssClass="txtReadOnly"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th class="Body_TH1" style="width: 80px;">
                                        <asp:Label ID="Label24" runat="server" Text="第3順位"></asp:Label></th>
                                    <td>
                                        <asp:DropDownList ID="ddl_P_BIZ_CHG_TYPE3" runat="server" Width="120px" ClientIDMode="Static"
                                             AutoPostBack="true" OnSelectedIndexChanged="ddl_P_BIZ_CHG_TYPE3_SelectedIndexChanged"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label25" runat="server" Text="異動單位"></asp:Label>
                                        <asp:TextBox ID="txt_P_CHG_DEPT_NO3" runat="server" Width="70px" ClientIDMode="Static" MaxLength="7" CssClass="txtReadOnly"
                                            AutoPostBack="true" OnTextChanged="txt_P_CHG_DEPT_NO3_TextChanged"></asp:TextBox>
                                        <asp:Button id="btn_P_CHG_DEPT_NO3" runat="server" Text="..." OnClientClick="OpenDeptSearch('txt_P_CHG_DEPT_NO3', 'txt_P_CHG_DEPT_NAME3', 'Y', 'Y');" CssClass="txtReadOnly" />
                                        <asp:TextBox ID="txt_P_CHG_DEPT_NAME3" runat="server" Width="260px" ClientIDMode="Static" BorderWidth="0" CssClass="txtReadOnly"></asp:TextBox>
                                    </td>
                                    <td>ICT受入公司:<asp:DropDownList ID="ddl_P_ICT_COMPANY_CD3" runat="server" Width="120px" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                   <td class="Body_TR1" colspan="4">
                                        建議擔當工作內容
                                        <asp:TextBox ID="txt_P_WORK_C3" runat="server" MaxLength="50" Width="100%" ClientIDMode="Static" CssClass="txtReadOnly"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="Body_TR1" colspan="7">3.建議異動/調整時間點 <asp:Label ID="Label27" runat="server" Text="【異動タイミング】" CssClass="jpnText"/>
                            <asp:DropDownList ID="ddl_P_ADJUST_TIME" runat="server" Width="60px" ClientIDMode="Static"></asp:DropDownList>
                            年
                        </td>
                    </tr>
                    <tr>
                        <td class="Body_TR1" colspan="7">4.上述異動/業務調整之理由(無異動計畫者亦須填寫理由) <asp:Label ID="Label31" runat="server" Text="【理由説明】" CssClass="jpnText"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="Body_TR1" colspan="7">
                            <asp:TextBox TextMode="MultiLine" Rows="3" ID="txt_P_ADJUST_REASON" runat="server" Width="100%" BorderWidth="1" Style="overflow: auto"  CssClass="txtReadOnly"></asp:TextBox>

                        </td>
                    </tr>
                    <tr>
                        <td class="Body_TR1" colspan="7">5.總評 <asp:Label ID="Label34" runat="server" Text="【総合コメント】" CssClass="jpnText"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="Body_TR1" colspan="7">
                            <asp:TextBox TextMode="MultiLine" Rows="5" ID="txt_G_COMMENT" runat="server" Width="100%" BorderWidth="1" Style="overflow: auto" CssClass="txtReadOnly"></asp:TextBox>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                </tbody>
            </table>

            <!-- 每頁的筆數 -->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />

            <asp:HiddenField ID="hid_MAX_SEQ" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_select_text" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_select_value" runat="server" ClientIDMode="Static" />
            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
