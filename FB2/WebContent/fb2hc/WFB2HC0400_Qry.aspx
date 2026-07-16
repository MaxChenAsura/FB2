<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2hc/WFB2HC0400_Qry.aspx.cs" Inherits="WebContent_fb2hc_WFB2HC0400_Qry" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
            if ($("#hid_PAY_YM_search").val() == '')
                setTabs_display('none');

        });
        var li_id;
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date2").datepicker({ dateFormat: 'yy/mm' });
            $(".date2").mask('9999/99');


            $("#WFB2HC0400StlAmt").val($("#hid_wfb2hc_WFB2HC0400StlAmt").val());
            $("#WFB2HC0400StlLock").val($("#hid_wfb2hc_WFB2HC0400StlLock").val());
            $("#WFB2HC0400StlUnLock").val($("#hid_wfb2hc_WFB2HC0400StlUnLock").val());
            gridviewScroll();
            gridviewScroll2();
            //$("#tabs").tabs();
            $('#ul li').click(function () {
                li_id = $(this).attr('id');
            });
            if ($("#hid_tab_id").val() != "")
                li_id = $("#hid_tab_id").val();
            ChangeTab(li_id);



            $('#WFB2HC0400StlAmt').click(function () {
                if (!proc_data()) return;
                var ym = getThisMonth().replace('/', '');
                var pay_ym = $("#txt_PAY_YM_search").val().replace('/', '');
                if (parseInt(pay_ym, 10) > parseInt(ym, 10)) {
                    alert('發放年月需小於等於系統年月');
                    $.unblockUI();
                    return;
                }
                BlockUI();
                //ajax 結算
                $.ajax({
                    url: "WFB2HC0400_DataProc.ashx",
                    data: {
                        DATA_TYPE: "WFB2HC0400StlAmt_Click",
                        PAY_YM: $('#hid_PAY_YM_search').val()
                    },
                    type: "GET",
                    dataType: 'json',
                    cache: false,
                    async: false,
                    success: function (JData) {
                        if (JData.procMsg != "") {
                            if ($("#hid_IS_QRY").val() == "Y") {
                                if ($("#" + '<%=WFB2HC0400Search.ClientID%>') != null) {
                                    $("#" + '<%=WFB2HC0400Search.ClientID%>').click();
                                }
                            }
                            setTimeout("alert('" + JData.procMsg + "');$.unblockUI();", 2000);
                        }
                    },

                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                        $.unblockUI();
                    }
                });
            });

            $('#WFB2HC0400StlLock').click(function () {
                if (!proc_data()) return;
                BlockUI();
                //ajax 結算鎖定 
                $.ajax({
                    url: "WFB2HC0400_DataProc.ashx",
                    data: {
                        DATA_TYPE: "WFB2HC0400StlLock_Click_step1",
                        PAY_YM: $('#hid_PAY_YM_search').val()
                    },
                    type: "GET",
                    dataType: 'json',
                    cache: false,
                    success: function (JData) {
                        if (confirm(JData.procMsg)) {
                            WFB2HC0400StlLock_Click_step2();
                        }
                    },

                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
                $.unblockUI();
            });

            $('#WFB2HC0400StlUnLock').click(function () {
                if (!proc_data()) return;
                BlockUI();
                //ajax 結算
                $.ajax({
                    url: "WFB2HC0400_DataProc.ashx",
                    data: {
                        DATA_TYPE: "WFB2HC0400StlUnLock_Click",
                        PAY_YM: $('#hid_PAY_YM_search').val()
                    },
                    type: "GET",
                    dataType: 'json',
                    cache: false,
                    success: function (JData) {
                        alert(JData.procMsg);
                    },

                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
                $.unblockUI();
            });

            $.unblockUI();
        }

        function WFB2HC0400StlLock_Click_step2() {
            if (!proc_data()) return;
            BlockUI();
            //ajax 結算
            $.ajax({
                url: "WFB2HC0400_DataProc.ashx",
                data: {
                    DATA_TYPE: "WFB2HC0400StlLock_Click_step2",
                    PAY_YM: $('#hid_PAY_YM_search').val()
                },
                type: "GET",
                dataType: 'json',
                cache: false,
                success: function (JData) {
                    alert(JData.procMsg);
                },

                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });
            $.unblockUI();
        };

        function setTabs_display(displaymod) {
            $('#tabs').css('display', displaymod);
        }
        function ChangeTab(tab) {
            $("#tabs").tabs({ active: tab });
            $("#hid_tab_id").val("");
        }

        function ShowRecord(obj) {
            $("#hid_tab_id").val(obj);
            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        function ShowRecord2(obj) {
            $("#hid_tab_id").val(obj);
            $("#HID_PageRow2").val($("#ddlPerPageRow2").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"
            });
        }
        function gridviewScroll2() {
            $('#<%=gv_result2.ClientID%>').gridviewScroll({
                width: "1020",
                height: "200",
                barcolor: "#7F7F7F"
            });
        }
        function ClearAll() {
            $('#txt_PAY_YM_search').val(getThisMonth());
            $('#txt_SALARY_DT').val("");
            return false;
        }
        function proc_data() {
            BlockUI();
            var txt_PAY_YM_search = $("#txt_PAY_YM_search").val();
            if (txt_PAY_YM_search == "") {
                alert($("#hid_wfb2hc_Required_PAY_YM").val());
                $.unblockUI();
                return false;
            }
            $("#tmp_PAY_YM_search").val("");
            if (txt_PAY_YM_search != "")
                $("#tmp_PAY_YM_search").val(txt_PAY_YM_search + '/01');
            if (!isDate($("#tmp_PAY_YM_search").val())) {
                alert($("#hid_wfb2hc_PAY_YM_Format_Error").val());
                $.unblockUI();
                return false;
            }
            $("#hid_PAY_YM_search").val(txt_PAY_YM_search.replace("/", ""));
            return true;
        }
        function redirToDtl(url, pay_ym, salary_dt, company_cd, company_cd_desc, bonus_type, bonus_type_desc, member_cnt, amt_cnt) {
            BlockUI();
            window.location.href = url + ".aspx?datakey=" + pay_ym + "," + salary_dt + "," + company_cd + "," + company_cd_desc + "," + bonus_type + "," + bonus_type_desc + "," + member_cnt + "," + amt_cnt;
        }
        function isDate(ExpiryDate) {
            var objDate,  // date object initialized from the ExpiryDate string 
                mSeconds, // ExpiryDate in milliseconds 
                day,      // day 
                month,    // month 
                year;     // year 
            // date length should be 10 characters (no more no less) 
            if (ExpiryDate.length !== 10) {
                return false;
            }
            // third and sixth character should be '/' 
            if (ExpiryDate.substring(4, 5) !== '/' || ExpiryDate.substring(7, 8) !== '/') {
                return false;
            }
            // extract month, day and year from the ExpiryDate (expected format is mm/dd/yyyy) 
            // subtraction will cast variables to integer implicitly (needed 
            // for !== comparing) 
            month = ExpiryDate.substring(5, 7) - 1; // because months in JS start from 0 
            day = ExpiryDate.substring(8, 10) - 0;
            year = ExpiryDate.substring(0, 4) - 0;

            // convert ExpiryDate to milliseconds 
            mSeconds = (new Date(year, month, day)).getTime();
            // initialize Date() object from calculated milliseconds 
            objDate = new Date();
            objDate.setTime(mSeconds);
            // compare input date and parts from Date() object 
            // if difference exists then date isn't valid 
            if (objDate.getFullYear() !== year ||
                objDate.getMonth() !== month ||
                objDate.getDate() !== day) {
                return false;
            }
            // otherwise return true 
            return true;
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:TextBox ID="txt_pre_Master_Key" runat="server" Style="display: none;" ClientIDMode="Static" />
    <asp:TextBox ID="txt_Master_Key" runat="server" Style="display: none;" ClientIDMode="Static" />
    <asp:TextBox ID="txt_Detail_search" runat="server" Style="display: none;" ClientIDMode="Static" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="12%" />
                                <col width="18%" />
                                <col width="12%" />
                                <col width="58%" />

                            </colgroup>
                            <tbody>
                                <tr>
                                    <td height="10px"></td>
                                </tr>
                                <tr>
                                    <%--發放年月--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PAY_YM_search" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_PAY_YM%>"></asp:Label>：
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_PAY_YM_search" runat="server" MaxLength="6" size="5" CssClass="MandatoryField date2" ClientIDMode="Static"></asp:TextBox>
                                        <asp:TextBox ID="tmp_PAY_YM_search" runat="server" size="5" Style="display: none;" ClientIDMode="Static"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="rfv_PAY_YM_search" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_Required_PAY_YM%>"
                                            ControlToValidate="txt_PAY_YM_search" ForeColor="Red" Display="None" ValidationGroup="GroupA"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cv_PAY_YM_search" runat="server" 
                	                        ControlToValidate="tmp_PAY_YM_search" ErrorMessage="<%$Resources:Resource,wfb2hc_PAY_YM_Format_Error%>" Type="Date" Operator="DataTypeCheck"
                                            Display="None" ForeColor="Red" ValidationGroup="GroupA"></asp:CompareValidator>--%>
                                    </td>
                                    <th></th>
                                    <td></td>
                                    <%--發薪日期
	                                <th align="left"  class="Body_TableHeader">
		                                <asp:Label ID="lb_SALARY_DT" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_SALARY_DT%>"></asp:Label>:
	                                </th>
                                    <td align="left" class="Body_label" colspan="3">											
		                                <asp:TextBox ID="txt_SALARY_DT" runat="server" MaxLength="10" size="10" style="border:none" readonly="true" ClientIDMode="Static"></asp:TextBox>		                                
	                                </td>
                                    --%>
                                </tr>
                                <tr>
                                    <td align="right" class="Body_label" colspan="10">
                                        <div id="init">
                                            <aces:Btn ID="WFB2HC0400Search" runat="server" Text="<%$Resources:Resource,btn_search%>" OnClick="WFB2HC0400Search_Click" OnClientClick="return proc_data();" />

                                            <%--<asp:Button ID="WFB2HC0400Search" runat="server" Text="<%$Resources:Resource,btn_search%>" OnClick="WFB2HC0400Search_Click" OnClientClick="return proc_data();" />--%>

                                            <asp:Button ID="WFB2HC0400Clear" runat="server" type="button" Text="<%$Resources:Resource,btn_clear%>" OnClientClick="return ClearAll();" />

                                            <%--                                        <aces:Btn type="button" id="WFB2HC0400StlAmt" value="" />
                                            <aces:Btn type="button" id="WFB2HC0400StlLock" value="" />
                                            <aces:Btn type="button" id="WFB2HC0400StlUnLock" value="" />--%>

                                            <input type="button" id="WFB2HC0400StlAmt" value="" />
                                            <input type="button" id="WFB2HC0400StlLock" value="" />
                                            <input type="button" id="WFB2HC0400StlUnLock" value="" />

                                            <%--<asp:Button ID="WFB2HC0400StlAmt" runat="server" Text="<%$Resources:Resource,wfb2hc_WFB2HC0400StlAmt%>" OnClientClick="proc_data();CheckValid();" ValidationGroup="GroupA" ClientIDMode="Static" />--%>
                                            <%--<asp:Button ID="WFB2HC0400StlLock" runat="server" Text="<%$Resources:Resource,wfb2hc_WFB2HC0400StlLock%>" OnClientClick="proc_data();CheckValid();" ValidationGroup="GroupA" ClientIDMode="Static" />
                                            <asp:Button ID="WFB2HC0400StlUnLock" runat="server" Text="<%$Resources:Resource,wfb2hc_WFB2HC0400StlUnLock%>" OnClientClick="proc_data();CheckValid();" ValidationGroup="GroupA" ClientIDMode="Static" />--%>
                                            <asp:HiddenField ID="hid_IS_QRY" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_PAY_YM_search" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_SALARY_DT_search" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_WFB2HC0400StlAmt_check_Message" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_WFB2HC0400StlLock_check_Message" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_WFB2HC0400StlLock_confirm_Message" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_WFB2HC0400StlUnLock_check_Message1" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_WFB2HC0400StlUnLock_check_Message2" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_wfb2hc_WFB2HC0400StlAmt" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_wfb2hc_WFB2HC0400StlLock" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_wfb2hc_WFB2HC0400StlUnLock" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_wfb2hc_Required_PAY_YM" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_wfb2hc_PAY_YM_Format_Error" ClientIDMode="Static" runat="server" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
            </table>
            <div id="tabs" style="width: 1020px;">
                <ul>
                    <li id="0"><a href="#tabs-1">【發放人數】</a></li>
                    <li id="1"><a href="#tabs-2">【發放金額】</a></li>
                </ul>
                <div id="tabs-1">
                    <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                        <colgroup>
                            <col width="12%" />
                            <col width="88%" />
                        </colgroup>
                        <tbody>
                            <tr>
                                <td height="10px"></td>
                            </tr>
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_TOTAL_MEMBER" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_TOTAL_MEMBER%>"></asp:Label>：
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:TextBox ID="txt_TOTAL_MEMBER" runat="server" MaxLength="10" size="10" Style="border: none" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_TOTAL_REAL" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_TOTAL_REAL%>"></asp:Label>：
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:TextBox ID="txt_TOTAL_REAL" runat="server" MaxLength="10" size="10" Style="border: none" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData1"
                        SelectCountMethod="getCount1" TypeName="CFB2HC0400DAO" EnablePaging="True"
                        SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                        StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                        <SelectParameters>
                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                            <asp:Parameter Name="maximumRows" Type="Int32" />
                            <asp:ControlParameter ControlID="hid_PAY_YM_search"
                                Name="pay_ym" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                        </SelectParameters>
                    </asp:ObjectDataSource>

                    <%--meta:resourcekey="gv_resultResource1"--%>
                    <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                        AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                        OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                        OnPageIndexChanging="gv_result_PageIndexChanging" meta:resourcekey="gv_resultResource1">
                        <Columns>
                            <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2hc_hd_RowNumber%>" />
                            <asp:BoundField DataField="COMPANY_CD_DESC" HeaderText="<%$Resources:Resource,wfb2hc_hd_COMPANY_CD_DESC%>" ItemStyle-Width="190px" SortExpression="COMPANY_CD_DESC" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="START_DT" HeaderText="<%$Resources:Resource,wfb2hc_hd_START_DT%>" ItemStyle-Width="190px" SortExpression="START_DT" DataFormatString="{0:yyyy/MM/dd}" />
                            <asp:BoundField DataField="END_DT" HeaderText="<%$Resources:Resource,wfb2hc_hd_END_DT1%>" ItemStyle-Width="190px" SortExpression="END_DT" DataFormatString="{0:yyyy/MM/dd}" />
                            <asp:BoundField DataField="MEMBER_CNT" HeaderText="<%$Resources:Resource,wfb2hc_hd_MEMBER_CNT%>" ItemStyle-Width="190px" SortExpression="MEMBER_CNT" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:##,#}" />
                            <asp:BoundField DataField="REAL_CNT" HeaderText="<%$Resources:Resource,wfb2hc_hd_REAL_CNT%>" ItemStyle-Width="190px" SortExpression="REAL_CNT" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:##,#}" />
                        </Columns>
                        <PagerStyle CssClass="GridviewScrollPager" />
                        <FooterStyle CssClass="GridviewScrollPager" />
                    </asp:GridView>
                    <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />

                    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Detail1_Choice_Not_Equal_1_Message" />
                    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Detail2_Choice_Not_Equal_1_Message" />
                </div>
                <div id="tabs-2">
                    <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                        <colgroup>
                            <col width="12%" />
                            <col width="18%" />
                            <col width="8%" />
                            <col width="15%" />
                            <col width="8%" />
                            <col width="15%" />
                            <col width="24%" />
                        </colgroup>
                        <tbody>
                            <tr>
                                <td height="10px"></td>
                            </tr>
                            <tr>
                                <%--發放人數合計--%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_TOTAL_REAL1" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_TOTAL_REAL%>"></asp:Label>:
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:TextBox ID="txt_TOTAL_REAL1" runat="server" MaxLength="10" size="10" Style="border: none" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <%--KZ--%>
                                <th align="left" class="Body_TableHeader">KZ:
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:TextBox ID="txt_TOTAL_KZ" runat="server" MaxLength="10" size="10" Style="border: none" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <%--派遣--%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_TOTAL_DISPATCH" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_TOTAL_DISPATCH%>"></asp:Label>:
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:TextBox ID="txt_TOTAL_DISPATCH" runat="server" MaxLength="10" size="10" Style="border: none" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <td align="left" class="Body_label"></td>
                            </tr>
                            <tr>
                                <%--發放金額合計--%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_TOTAL_AMT_REAL" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_TOTAL_AMT_REAL%>"></asp:Label>:
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:TextBox ID="txt_TOTAL_AMT_REAL" runat="server" MaxLength="20" size="20" Style="border: none" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <%--KZ--%>
                                <th align="left" class="Body_TableHeader">KZ:</th>
                                <td align="left" class="Body_label">
                                    <asp:TextBox ID="txt_TOTAL_AMT_KZ" runat="server" MaxLength="20" size="20" Style="border: none" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <%--派遣--%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_TOTAL_AMT_DISPATCH" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_TOTAL_DISPATCH%>"></asp:Label>:
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:TextBox ID="txt_TOTAL_AMT_DISPATCH" runat="server" MaxLength="20" size="20" Style="border: none" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <td align="left" class="Body_label"></td>
                            </tr>
                        </tbody>
                    </table>
                    <asp:ObjectDataSource ID="ods2" runat="server" SelectMethod="getData2"
                        SelectCountMethod="getCount2" TypeName="CFB2HC0400DAO" EnablePaging="True"
                        SortParameterName="sortExpression" OnSelecting="ods2_Selecting"
                        StartRowIndexParameterName="startRowIndex" OnSelected="ods2_Selected">
                        <SelectParameters>
                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                            <asp:Parameter Name="maximumRows" Type="Int32" />
                            <asp:ControlParameter ControlID="hid_PAY_YM_search"
                                Name="pay_ym" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <%--meta:resourcekey="gv_resultResource2"--%>
                    <asp:GridView ID="gv_result2" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                        AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting2"
                        OnRowDataBound="gv_result_RowDataBound2" OnRowCreated="gv_result_RowCreated2" Width="1020px"
                        OnPageIndexChanging="gv_result_PageIndexChanging2" meta:resourcekey="gv_resultResource2">
                        <Columns>
                            <%--<asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2hc_hd_RowNumber%>" ItemStyle-Width="40px" />--%>
                            <asp:BoundField DataField="COMPANY_CD_DESC" HeaderText="<%$Resources:Resource,wfb2hc_hd_COMPANY_CD_DESC%>" ItemStyle-Width="200px" SortExpression="COMPANY_CD_DESC" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="BONUS_TYPE_DESC" HeaderText="<%$Resources:Resource,wfb2hc_hd_BONUS_TYPE_DESC%>" ItemStyle-Width="200px" SortExpression="BONUS_TYPE_DESC" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="CNT" HeaderText="<%$Resources:Resource,wfb2hc_hd_REAL_CNT%>" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Right" SortExpression="BONUS_AMT" DataFormatString="{0:##,#}" />
                            <asp:BoundField DataField="BONUS_AMT" HeaderText="<%$Resources:Resource,wfb2hc_hd_BONUS_AMT1%>" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Right" SortExpression="BONUS_AMT" DataFormatString="{0:##,#}" />
                            <asp:TemplateField ItemStyle-Width="200px">
                                <HeaderTemplate>
                                    <asp:Label ID="lb_function" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_function%>"></asp:Label></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Button ID="WFB2HC0400Detail" runat="server" Text="<%$Resources:Resource,wfb2hc_btn_search_detail%>" />
                                    <asp:HiddenField ID="hid_COMPANY_CD" Value='<%#Bind("COMPANY_CD")%>' ClientIDMode="Static" runat="server" />
                                    <asp:HiddenField ID="hid_BONUS_TYPE" Value='<%#Bind("BONUS_TYPE")%>' ClientIDMode="Static" runat="server" />
                                    <asp:HiddenField ID="hid_COMPANY_CD_DESC" Value='<%#Bind("COMPANY_CD_DESC")%>' ClientIDMode="Static" runat="server" />
                                    <asp:HiddenField ID="hid_BONUS_TYPE_DESC" Value='<%#Bind("BONUS_TYPE_DESC")%>' ClientIDMode="Static" runat="server" />
                                    <asp:HiddenField ID="hid_CNT" Value='<%#Bind("CNT")%>' ClientIDMode="Static" runat="server" />
                                    <asp:HiddenField ID="hid_BONUS_AMT" Value='<%#Bind("BONUS_AMT")%>' ClientIDMode="Static" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="GridviewScrollPager" />
                        <FooterStyle CssClass="GridviewScrollPager" />
                    </asp:GridView>
                    <asp:HiddenField ID="HID_PageRow2" runat="server" ClientIDMode="Static" />
                     <asp:HiddenField ID="hid_tab_id" runat="server" ClientIDMode="Static" />
                </div>
            </div>
            
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2HC0400Clear" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="vs1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
</asp:Content>


