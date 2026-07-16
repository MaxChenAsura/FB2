<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2se/WFB2SE1400_Dtl.aspx.cs" Inherits="WebContent_fb2se_WFB2SE1400_Dtl" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            //$("#tabs").tabs({ active: 0 });
            iniForm();

        });
        //gridviewScroll();
        //$("#tabs").tabs();
        //$('#ul li').click(function () {
        //    li_id = $(this).attr('id');

        //});
        //ChangeTab(li_id);
        var li_id;
        function iniForm() {
            $('#ddlPerPageRow').css("font-family", "Times New Roman, Times, serif").css("font-size", "14px");
            $('#ddlPerPageRow2').css("font-family", "Times New Roman, Times, serif").css("font-size", "14px");
            $('#ddlPerPageRow3').css("font-family", "Times New Roman, Times, serif").css("font-size", "14px");
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $.unblockUI();
            gridviewScroll();
            //$("#tabs").tabs();
            $('#ul li').click(function () {
                li_id = $(this).attr('id');
            });
            if ($("#hid_tab_id").val() != "")
                li_id = $("#hid_tab_id").val();
            ChangeTab(li_id);
            //$("#ui-id-2").click(function () {
            //    var iframe = document.getElementById('iframe2');
            //    iframe.src = iframe.src;
            //});
            //$("#ui-id-3").click(function () {
            //    var iframe = document.getElementById('iframe3');
            //    iframe.src = iframe.src;
            //});
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
        }
        function ChangeTab(tab) {
            $("#tabs").tabs({ active: tab });
            $("#hid_tab_id").val("");
        }

        function ShowRecord(obj) {
            $("#hid_tab_id").val(li_id);
            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            $("#HID_PageRow2").val($("#ddlPerPageRow2").val());
            $("#HID_PageRow3").val($("#ddlPerPageRow3").val());
        }
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F",
                freezesize: 6
            });

        }

        function checkboxsSelected1() {
            var ItemCheckBoxs = $("#gv_result [type=checkbox]");
            var HaveCheck = 0;
            $(":checkbox:checked").each(function () {
                HaveCheck++;
            });
            return HaveCheck;
        };
        function checkboxsSelected2() {
            var ItemCheckBoxs = $("#gv_result2 [type=checkbox]");
            var HaveCheck = 0;
            $(":checkbox:checked").each(function () {
                HaveCheck++;
            });
            return HaveCheck;
        };
        function checkboxsSelected3() {
            var ItemCheckBoxs = $("#gv_result3 [type=checkbox]");
            var HaveCheck = 0;
            $(":checkbox:checked").each(function () {
                HaveCheck++;
            });
            return HaveCheck;
        };

        function checkApprove() {
            var processed = true;
            BlockUI();
            if (checkboxsSelected1() > 0) {
                alert("[個人薪調]頁籤異常註記,有勾選資料,不允執行核可。");
                processed = false;
            } else if (checkboxsSelected2() > 0) {
                alert("[3A以下調薪設定值]頁籤異常註記,有勾選資料,不允執行核可。");
                processed = false;
            } else if (checkboxsSelected3() > 0) {
                alert("[2B以上調薪設定值]頁籤異常註記,有勾選資料,不允執行核可。");
                processed = false;
            } else {
                processed = confirm("核可生效後無法重新核可,確認是否執行核可?");
            }

            if (!processed) {
                $.unblockUI();
            }
            return processed;

        };

        //駁回,檢查是否有勾選
        function checkReject() {
            var processed = true;
            BlockUI();
            var remark = $.trim($("textarea[id $= 'txt_REMARK']").val());

            if (remark == "") {
                alert("執行駁回,備註說明不允空白。");
                processed = false;
            } else {
                processed = confirm("確定要駁回?");
            }

            if (!processed) {
                $.unblockUI();
            }
            return processed;

        };
        //function ReturnValue() {

        //    var send_select = "";
        //    $("#lb_select").children("option").each(function () {
        //        send_select += this.text + ",";

        //    });

        //    if (window.opener != undefined) {
        //        //for chrome
        //        window.opener.returnValue = send_select;
        //    }
        //    else {
        //        window.returnValue = send_select;
        //    }

        //    window.close();
        //}
        function checkMark() {
            var processed = true;
            BlockUI();
            if (checkboxsSelected1() > 0 || checkboxsSelected2() > 0 || checkboxsSelected3() > 0) {
                //processed = confirm("確定要進行一括註記?");
                processed = true;
            } else {
                //alert("請選取異常註記 ");
                //processed = false;
            }

            if (!processed) {
                $.unblockUI();
            }
            return processed;

        }
        //清空畫面
        function ClearAll() {
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table border="0" cellpadding="1" cellspacing="1" class="Body_Label" width="1020">
                <colgroup>
                    <col width="13%" />
                    <col width="40%" />
                    <col width="10%" />
                    <col width="37%" />
                </colgroup>
                <tbody>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EFFECT_YM" runat="server" Text="<%$Resources:Resource,wfb2se_lb_EFFECT_YM%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="lbl_EFFECT_YM" runat="server"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_RELEASE_NAME" runat="server" Text="<%$Resources:Resource,wfb2se_lb_RELEASE_NAME%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label" colspan="2">
                            <asp:Label ID="lbl_RELEASE_NAME" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_RELEASE_DT" runat="server" Text="<%$Resources:Resource,wfb2se_lb_RELEASE_DT%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="lbl_RELEASE_DT" runat="server"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_SUB_DESC" runat="server" Text="<%$Resources:Resource,wfb2se_lb_SUB_DESC%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label" colspan="2">
                            <asp:Label ID="lbl_SUB_DESC" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPROVE_NAME" runat="server" Text="<%$Resources:Resource,wfb2se_lb_APPROVE_NAME%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="lbl_APPROVE_NAME" runat="server"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPROVE_DT" runat="server" Text="<%$Resources:Resource,wfb2se_lb_APPROVE_DT%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label" colspan="2">
                            <asp:Label ID="lbl_APPROVE_DT" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2se_lb_REMARK%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label" colspan="3">
                            <asp:TextBox ID="txt_REMARK" runat="server" Rows="4" TextMode="MultiLine" Columns="45"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="width: 900px; text-align: right;">
                            <asp:Button ID="btn_cancel" runat="server" Text="<%$Resources:Resource,wfb2sc_Back%>" ClientIDMode="Static" OnClick="btn_cancel_Click" /></td>
                        <%--OnClientClick="$(location).attr('href', 'WFB2SE1400_Qry.aspx'); return false;"--%>
                    </tr>
                    <tr>
                        <td align="center" colspan="4" height="1">
                            <hr></hr>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--工號--%>
                            <asp:Label ID="Label13" runat="server" Text="<%$Resources:Resource,wfb2sj_lb_emp_id%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_ID" runat="server" Width="50px" ClientIDMode="Static" MaxLength="5"> </asp:TextBox>
                            <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N', '');" />
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <%--姓名--%>
                            <asp:Label ID="Label15" runat="server" Text="<%$Resources:Resource,wfb2sj_lb_emp_name%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_NAME" runat="server" Width="80px" ClientIDMode="Static"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th></th>
                        <td align="right" colspan="4">
                            <aces:Btn ID="WFB2SE1400Search" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_search%>" OnClick="WFB2SE1400Search_Click" OnClientClick="BlockUI();" />
                            <%--<asp:Button ID="WFB2SE1400Search" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_search%>" OnClick="WFB2SE1400Search_Click" OnClientClick="BlockUI();" />--%>
                            <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_clear%>" OnClientClick="ClearAll();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <aces:Btn ID="WFB2SE1400Mark" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_mark%>" OnClick="WFB2SE1400Mark_Click" OnClientClick="return checkMark();" Enabled="false"/>
                            <%--<asp:Button ID="WFB2SE1400Mark" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_mark%>" OnClick="WFB2SE1400Mark_Click" OnClientClick="return checkMark();" Enabled="false"/>--%>
                        </td>
                        <td colspan="2" style="width: 900px; text-align: right;">
                            <aces:Btn ID="WFB2SE1400APPROVE" runat="server" Text="<%$Resources:Resource,btn_approve%>" ClientIDMode="Static" OnClientClick="BlockUI();" OnClick="btn_approve_Click" Enabled="false" />
                            <aces:Btn ID="WFB2SE1400REJECT" runat="server" Text="<%$Resources:Resource,btn_reject%>" ClientIDMode="Static" Enabled="false" OnClick="btn_reject_Click" OnClientClick="return checkReject();" />

                            <%-- <asp:Button ID="WFB2SE1400APPROVE" runat="server" Text="<%$Resources:Resource,btn_approve%>" ClientIDMode="Static" OnClientClick="BlockUI();" OnClick="btn_approve_Click" Enabled="false" />
                            <asp:Button ID="WFB2SE1400REJECT" runat="server" Text="<%$Resources:Resource,btn_reject%>" ClientIDMode="Static" Enabled="false" OnClick="btn_reject_Click" OnClientClick="return checkReject();" />--%>
                        </td>
                    </tr>
                </tbody>
            </table>
            <tr>
            </tr>
            <tr>
                <td class="Body_label">

                    <div id="tabs" style="width: 1020px">
                        <ul id="ul">
                            <li id="0"><a href="#tabs-1">【個人薪調】</a></li>
                            <li id="1"><a href="#tabs-2">【3A以下調薪設定值】</a></li>
                            <li id="2"><a href="#tabs-3">【2B以上調薪設定值】</a></li>
                        </ul>
                        <div id="tabs-1">

                            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData1"
                                SelectCountMethod="getCount1" TypeName="CFB2SE1400DAO" EnablePaging="True"
                                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                                StartRowIndexParameterName="startRowIndex"
                                OnSelected="ods1_Selected">
                                <SelectParameters>
                                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                                    <asp:Parameter Name="maximumRows" Type="Int32" />
                                    <asp:QueryStringParameter QueryStringField="qdatakey" ConvertEmptyStringToNull="false" Name="qdatakey" Type="String" DefaultValue="" />
                                    <asp:ControlParameter ControlID="txt_EMP_ID"
                                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                                    <asp:ControlParameter ControlID="txt_EMP_NAME"
                                        Name="emp_name" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True"
                                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting"
                                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1475px"
                                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">

                                <Columns>
                                    <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-Width="40px" HeaderText="<%$Resources:Resource,wfb2se_APPROVE_MARK%>" SortExpression="APPROVE_MARK">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="IS_APPROVE_MARK" runat="server" ClientIDMode="AutoID" />
                                            <asp:HiddenField ID="hid_APPROVE_MARK" runat="server" ClientIDMode="Static" Value='<%#Bind("APPROVE_MARK")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px" />
                                    <asp:BoundField DataField="CHG_STATUS" HeaderText="<%$Resources:Resource,wfb2se_CHG_STATUS%>" SortExpression="CHG_STATUS" HeaderStyle-Width="70px" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="left" />
                                    <asp:BoundField DataField="EFFECT_YM" HeaderText="<%$Resources:Resource,wfb2se_EFFECT_YM%>" SortExpression="EFFECT_YM" HeaderStyle-Width="65px" ItemStyle-Width="65px" ItemStyle-HorizontalAlign="left" />
                                    <asp:TemplateField HeaderStyle-Width="60px" HeaderText="<%$Resources:Resource,wfb2se_EMP_ID%>" SortExpression="EMP_ID" ItemStyle-Width="60px">
                                        <ItemTemplate>
                                            <div style="text-align: left; width: 100%">
                                                <asp:Label ID="lbl_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2se_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="70px" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Left" />
                                    <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-Width="30px" HeaderText="<%$Resources:Resource,wfb2se_LEVEL_CD%>" SortExpression="LEVEL_CD">
                                        <ItemTemplate>
                                            <div style="text-align: left; width: 100%">
                                                <asp:Label ID="lbl_LEVEL_CD" runat="server" Text='<%#Bind("LEVEL_CD")%>'></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="GRADE_CD" HeaderText="<%$Resources:Resource,wfb2se_GRADE_CD%>" SortExpression="GRADE_CD" HeaderStyle-Width="30px" ItemStyle-Width="30px" />
                                    <asp:BoundField DataField="DEPT_NAME_20" HeaderText="<%$Resources:Resource,wfb2se_DEPT_NAME_20%>" SortExpression="DEPT_NAME_20" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="DEPT_NAME_30" HeaderText="<%$Resources:Resource,wfb2se_DEPT_NAME_30%>" SortExpression="DEPT_NAME_30" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="DEPT_NAME_40" HeaderText="<%$Resources:Resource,wfb2se_DEPT_NAME_40%>" SortExpression="DEPT_NAME_40" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="THIS_YEAR_GRADE" HeaderText="<%$Resources:Resource,wfb2se_THIS_YEAR_GRADE%>" SortExpression="THIS_YEAR_GRADE" HeaderStyle-Width="30px" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="LEVEL_PAY_OLD" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_LEVEL_PAY_OLD%>" SortExpression="LEVEL_PAY_OLD" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="EXAMINE_ADJ" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_ADJ%>" SortExpression="EXAMINE_ADJ" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="LEVEL_ADJ" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_LEVEL_ADJ%>" SortExpression="LEVEL_ADJ" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="LEVEL_PAY_NEW" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_LEVEL_PAY_NEW%>" SortExpression="LEVEL_PAY_NEW" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="ABILITY_PAY_OLD" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_ABILITY_PAY_OLD%>" SortExpression="ABILITY_PAY_OLD" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                                     <asp:BoundField DataField="ABILITY_ADJ" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_ABILITY_ADJ%>" SortExpression="ABILITY_ADJ" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="ABILITY_PAY_NEW" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_ABILITY_PAY_NEW%>" SortExpression="ABILITY_PAY_NEW" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="LEVEL_PAY_DIFF" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_LEVEL_PAY_DIFF%>" SortExpression="LEVEL_PAY_DIFF" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                                    <asp:CheckBoxField DataField="IS_NOPAYDIFF_YN" HeaderText="<%$Resources:Resource,wfb2se_NOPAYDIFF_YN%>" SortExpression="IS_NOPAYDIFF_YN" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                                </Columns>
                                <EmptyDataTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lit_noda" runat="server" Text="<%$Resources:Resource,wfd2se_nodata%>"></asp:Literal>

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
                                        <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static" AutoPostBack="true" Font-Size="14px">
                                            <asp:ListItem Text="每頁10筆" Value="10"></asp:ListItem>
                                            <asp:ListItem Text="每頁20筆" Value="20"></asp:ListItem>
                                            <asp:ListItem Text="每頁30筆" Value="30"></asp:ListItem>
                                            <asp:ListItem Text="每頁40筆" Value="40"></asp:ListItem>
                                            <asp:ListItem Text="每頁50筆" Value="50"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 5px"></td>
                                    <td style="width: 100%; font-size: 14px;">
                                        <asp:Label ID="lb_TotalCount" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>

                        </div>
                        <div id="tabs-2">
                            <asp:ObjectDataSource ID="ods2" runat="server" SelectMethod="getData2"
                                SelectCountMethod="getCount2" TypeName="CFB2SE1400DAO" EnablePaging="True"
                                SortParameterName="sortExpression" OnSelecting="obs1_Selecting2"
                                StartRowIndexParameterName="startRowIndex"
                                OnSelected="ods1_Selected2">

                                <SelectParameters>
                                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                                    <asp:Parameter Name="maximumRows" Type="Int32" />
                                    <asp:QueryStringParameter QueryStringField="qdatakey" ConvertEmptyStringToNull="false" Name="qdatakey" Type="String" DefaultValue="" />

                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:GridView ID="gv_result2" runat="server" AllowPaging="True" AllowSorting="True"
                                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting2"
                                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated2" Width="1010px"
                                OnPageIndexChanging="gv_result_PageIndexChanging2" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound2">

                                <Columns>
                                    <asp:TemplateField HeaderStyle-Width="40px" HeaderText="<%$Resources:Resource,wfb2se_APPROVE_MARK%>" SortExpression="APPROVE_MARK" ItemStyle-Width="40px">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="IS_APPROVE_MARK" runat="server" ClientIDMode="AutoID" />
                                            <asp:HiddenField ID="hid_APPROVE_MARK" runat="server" ClientIDMode="Static" Value='<%#Bind("APPROVE_MARK")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px" />
                                    <asp:TemplateField HeaderStyle-Width="40px" HeaderText="<%$Resources:Resource,wfb2se_LEVEL_CD%>" SortExpression="LEVEL_CD">
                                        <ItemTemplate>
                                            <div style="text-align: left; width: 100%">
                                                <asp:Label ID="lbl_LEVEL_CD" runat="server" Text='<%#Bind("LEVEL_CD")%>'></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="40px" HeaderText="<%$Resources:Resource,wfb2se_GRADE_CD%>" SortExpression="GRADE_CD" ItemStyle-Width="40px">
                                        <ItemTemplate>
                                            <div style="text-align: center; width: 100%">
                                                <asp:Label ID="lbl_GRADE_CD" runat="server" Text='<%#Bind("GRADE_CD")%>'></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="EXAMINE_A" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_A%>" SortExpression="EXAMINE_A" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="EXAMINE_B" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_B%>" SortExpression="EXAMINE_B" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="EXAMINE_C" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_C%>" SortExpression="EXAMINE_C" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="EXAMINE_D" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_D%>" SortExpression="EXAMINE_D" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="EXAMINE_E" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_E%>" SortExpression="EXAMINE_E" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="ABILITY_ADJ" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_ABILITY_ADJ%>" SortExpression="ABILITY_ADJ" HeaderStyle-Width="90px" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="LEVEL_ADJ" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_LEVEL_ADJ%>" SortExpression="LEVEL_ADJ" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="LEVEL_PAY_LOW" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_LEVEL_PAY_LOW%>" SortExpression="LEVEL_PAY_LOW" HeaderStyle-Width="90px" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="LEVEL_PAY_AVG" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_LEVEL_PAY_AVG%>" SortExpression="LEVEL_PAY_AVG" HeaderStyle-Width="90px" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="LEVEL_PAY_UP" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_LEVEL_PAY_UP%>" SortExpression="LEVEL_PAY_UP" HeaderStyle-Width="90px" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Right" />

                                </Columns>
                                <EmptyDataTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lit_noda" runat="server" Text="<%$Resources:Resource,wfd2se_nodata%>"></asp:Literal>

                                            </td>
                                        </tr>
                                    </table>
                                </EmptyDataTemplate>
                                <PagerStyle CssClass="GridviewScrollPager" />
                                <FooterStyle CssClass="GridviewScrollPager" />
                            </asp:GridView>
                            <table id="OnePage2" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                                <tr height="100%" valign="top">
                                    <td class="GridviewScrollPager TD">
                                        <asp:DropDownList ID="ddlPerPageRow2" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static" AutoPostBack="true" Font-Size="14px">
                                            <asp:ListItem Text="每頁10筆" Value="10"></asp:ListItem>
                                            <asp:ListItem Text="每頁20筆" Value="20"></asp:ListItem>
                                            <asp:ListItem Text="每頁30筆" Value="30"></asp:ListItem>
                                            <asp:ListItem Text="每頁40筆" Value="40"></asp:ListItem>
                                            <asp:ListItem Text="每頁50筆" Value="50"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 5px"></td>
                                    <td style="width: 100%; font-size: 14px;">
                                        <asp:Label ID="lb_TotalCount2" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="tabs-3">
                            <asp:ObjectDataSource ID="ods3" runat="server" SelectMethod="getData3"
                                SelectCountMethod="getCount3" TypeName="CFB2SE1400DAO" EnablePaging="True"
                                SortParameterName="sortExpression" OnSelecting="obs1_Selecting3"
                                StartRowIndexParameterName="startRowIndex"
                                OnSelected="ods1_Selected3">
                                <SelectParameters>
                                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                                    <asp:Parameter Name="maximumRows" Type="Int32" />
                                    <asp:QueryStringParameter QueryStringField="qdatakey" ConvertEmptyStringToNull="false" Name="qdatakey" Type="String" DefaultValue="" />

                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:GridView ID="gv_result3" runat="server" AllowPaging="True" AllowSorting="True"
                                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting3"
                                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated3" Width="1000px"
                                OnPageIndexChanging="gv_result_PageIndexChanging3" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound3">
                                <Columns>

                                    <asp:TemplateField HeaderStyle-Width="70px" HeaderText="<%$Resources:Resource,wfb2se_APPROVE_MARK%>" SortExpression="APPROVE_MARK">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="IS_APPROVE_MARK" runat="server" ClientIDMode="AutoID" />
                                            <asp:HiddenField ID="hid_APPROVE_MARK" runat="server" ClientIDMode="Static" Value='<%#Bind("APPROVE_MARK")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px" />
                                    <asp:TemplateField HeaderStyle-Width="40px" HeaderText="<%$Resources:Resource,wfb2se_LEVEL_CD%>" SortExpression="LEVEL_CD" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="40px">
                                        <ItemTemplate>
                                            <div style="text-align: left; width: 100%">
                                                <asp:Label ID="lbl_LEVEL_CD" runat="server" Text='<%#Bind("LEVEL_CD")%>'></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                                   <asp:BoundField DataField="PJOB_NAME" HeaderText="<%$Resources:Resource,wfb2se_PJOB_TYPE%>" SortExpression="PJOB_TYPE" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"/>
                <asp:BoundField DataField="EXAMINE_S" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_S%>" SortExpression="EXAMINE_S" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="EXAMINE_A" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_A%>" SortExpression="EXAMINE_A" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="EXAMINE_B" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_B%>" SortExpression="EXAMINE_B" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="EXAMINE_C" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_C%>" SortExpression="EXAMINE_C" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="EXAMINE_D" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_D%>" SortExpression="EXAMINE_D" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="EXAMINE_E" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_E%>" SortExpression="EXAMINE_E" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="EXAMINE_F" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_F%>" SortExpression="EXAMINE_F" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="EXAMINE_G" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_G%>" SortExpression="EXAMINE_G" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="EXAMINE_H" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_H%>" SortExpression="EXAMINE_H" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="EXAMINE_I" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_I%>" SortExpression="EXAMINE_I" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="EXAMINE_J" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_J%>" SortExpression="EXAMINE_J" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right"/>
                                </Columns>
                                <EmptyDataTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lit_noda" runat="server" Text="<%$Resources:Resource,wfd2se_nodata%>"></asp:Literal>
                                            </td>
                                        </tr>
                                    </table>
                                </EmptyDataTemplate>
                                <PagerStyle CssClass="GridviewScrollPager" />
                                <FooterStyle CssClass="GridviewScrollPager" />
                            </asp:GridView>
                            <table id="OnePage3" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                                <tr height="100%" valign="top">
                                    <td class="GridviewScrollPager TD">
                                        <asp:DropDownList ID="ddlPerPageRow3" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static" AutoPostBack="true" Font-Size="14px">
                                            <asp:ListItem Text="每頁10筆" Value="10"></asp:ListItem>
                                            <asp:ListItem Text="每頁20筆" Value="20"></asp:ListItem>
                                            <asp:ListItem Text="每頁30筆" Value="30"></asp:ListItem>
                                            <asp:ListItem Text="每頁40筆" Value="40"></asp:ListItem>
                                            <asp:ListItem Text="每頁50筆" Value="50"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 5px"></td>
                                    <td style="width: 100%; font-size: 14px;">
                                        <asp:Label ID="lb_TotalCount3" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>

                    <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="HID_PageRow2" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="HID_PageRow3" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hid_tab_id" runat="server" ClientIDMode="Static" />

                </td>
            </tr>
            <tr>
                <td class="Body_label"></td>
            </tr>

            </table>

        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>

