<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2ib/WFB2IB0700_Qry.aspx.cs" Inherits="WebContent_fb2ib_WFB2IB0700_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            $('.ym').mask('9999/99');
            iniForm();
        });
        function iniForm() {
            $("#txt_PAYMENT_DATE_YM").mask("9999");
            $("#txt_EMP_ID").mask("99999");


            gridviewScroll();
            $.unblockUI();

            $(".money").blur(function () {
                $(this).parseNumber({ format: "#,###", locale: "tw" });
                $(this).formatNumber({ format: "#,###", locale: "tw" });
            });


            //查詢
            $('#txt_EMP_NAME').attr("readonly", true);

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

        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"
            });

            $('#<%=GridView1.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"
            });
        }

        //判斷是否為數字
        function IsIntText() {
            var charkeycode = window.event.keyCode;
            if (charkeycode > 47 && charkeycode < 58) {  //0的keycode為47 ,9的keycode為58
                return true;
            }
            return false;

        }

        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }

        function searchCheck() {
            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                return false;
        }

        //清空畫面
        function ClearAll() {
            //$('#ddl_SYS_CD').val(-1);
            $("#txt_PAYMENT_DATE_YM").val("");
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
        }


    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">

                <colgroup>
                    <col width="15%" />
                    <col width="35%" />
                    <col width="10%" />
                    <col width="40%" />
                </colgroup>
                <tbody>
                    <!-- START: 1st Line in Search Criteria area -->
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_PAYMENT_DATE_YM" runat="server" Text="<%$Resources:Resource,wfb2ib_lb_PAYMENT_DATE_YM%>"></asp:Label>：	
                        </th>
                        <td align="left">
                            <asp:TextBox ID="txt_PAYMENT_DATE_YM" CssClass="MandatoryField" runat="server" MaxLength="4" Width="81px" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ib_required_PAYMENT_DATE_YM%>"
                                ControlToValidate="txt_PAYMENT_DATE_YM" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="調整年度輸入錯誤"
                                ControlToValidate="txt_PAYMENT_DATE_YM" ForeColor="Red" ValidationGroup="GroupA"
                                ValidationExpression="\d\d\d\d" Display="None"></asp:RegularExpressionValidator>
                        </td>
                        <th></th>
                        <td align="right"></td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2ib_lb_EMP_ID%>"></asp:Label>：	
                        </th>
                        <td align="left">
                            <asp:TextBox ID="txt_EMP_ID" CssClass="MandatoryField" runat="server" MaxLength="5" Width="81px" ClientIDMode="Static"></asp:TextBox>
                            <input id="bt_EMP_ID" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N', '');" />
                            <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ib_required_EMP_ID%>"
                                ControlToValidate="txt_EMP_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </td>
                        <th></th>
                        <td align="right">
                            <aces:Btn ID="WFB2IB0700Adjust" runat="server" Text="<%$Resources:Resource,wfb2ib_WFB2IB0700Adjust%>" OnClick="WFB2IB0700Adjust_Click" Visible="false" />
                            <aces:Btn ID="WFB2IB0700Search" runat="server" Text="<%$Resources:Resource,wfb2ib_WFB2IB0700Search%>" OnClick="WFB2IB0700Search_Click" OnClientClick="return searchCheck()" />

                            <%-- <asp:Button ID="WFB2IB0700Adjust" runat="server" Text="<%$Resources:Resource,wfb2ib_WFB2IB0700Adjust%>" OnClick="WFB2IB0700Adjust_Click" Visible="false"/>					
                             <asp:Button ID="WFB2IB0700Search" runat="server" Text="<%$Resources:Resource,wfb2ib_WFB2IB0700Search%>" OnClick="WFB2IB0700Search_Click" OnClientClick="return searchCheck()"/>			
                            --%>
                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2dd_btn_clear%>" onclick="ClearAll();" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" height="1" colspan="4">
                            <hr>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" height="1">
                            <asp:Label ID="lb_before_adjust" runat="server" Text="<%$Resources:Resource,wfb2ib_lb_before_adjust%>" Visible="false"></asp:Label>
                        </td>
                    </tr>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2IB0700DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_PAYMENT_DATE_YM"
                        Name="PAYMENT_DATE" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="EMP_ID" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" meta:resourcekey="gv_resultResource1">
                <Columns>
                    <asp:BoundField DataField="PAYMENT_DATE" HeaderText="<%$Resources:Resource,wfb2ib_lb_PAYMENT_DATE_GRID%>" DataFormatString="{0:yyyy/MM/dd}" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2ib_lb_EMP_ID%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="left" />
                    <asp:BoundField DataField="INS_MONTH_AMOUNT" HeaderText="<%$Resources:Resource,wfb2ib_INS_MONTH_AMOUNT%>" HeaderStyle-Width="80px" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="right" />
                    <asp:BoundField DataField="FOUR_AMOUNT" HeaderText="<%$Resources:Resource,wfb2ib_INS_FOUR_AMOUNT%>" HeaderStyle-Width="80px" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="right" />
                    <asp:BoundField DataField="ONE_TIME_AMOUNT" HeaderText="<%$Resources:Resource,wfb2ib_INS_ONE_TIME_AMOUNT%>" HeaderStyle-Width="80px" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="right" />
                    <asp:BoundField DataField="ACCU_AMOUNT" HeaderText="<%$Resources:Resource,wfb2ib_INS_ACCU_AMOUNT%>" HeaderStyle-Width="80px" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="right" />
                    <asp:BoundField DataField="ACCU_OVER_AMOUNT" HeaderText="<%$Resources:Resource,wfb2ib_INS_ACCU_OVER__AMOUNT%>" HeaderStyle-Width="120px" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="right" />
                    <asp:BoundField DataField="INS_COST_BASE" HeaderText="<%$Resources:Resource,wfb2ib_INS_INS_COST_BASE%>" HeaderStyle-Width="100px" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="right" />
                    <asp:BoundField DataField="INS_COST" HeaderText="<%$Resources:Resource,wfb2ib_INS_INS_COST%>" HeaderStyle-Width="80px" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="right" />
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ib_lb_adjust%>" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:TextBox ID="txt_adjust" CssClass="MandatoryField" Text='<%#Bind("ONE_TIME_AMOUNT")%>' runat="server" MaxLength="7" Width="81px" ClientIDMode="Static"></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                        <FooterStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />

            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <caption>
                    <br />
                    <tr>
                        <td align="left" height="1">
                            <asp:Label ID="lb_after_adjust" runat="server" Text="<%$Resources:Resource,wfb2ib_lb_after_adjust%>" Visible="false"></asp:Label>
                        </td>
                    </tr>
                </caption>
            </table>

            <asp:ObjectDataSource ID="ods2" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2IB0700DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_PAYMENT_DATE_YM"
                        Name="PAYMENT_DATE" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="EMP_ID" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting1"
                OnRowDataBound="gv_result_RowDataBound1" OnRowCreated="gv_result_RowCreated1" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" meta:resourcekey="gv_resultResource1">
                <Columns>
                    <asp:BoundField DataField="PAYMENT_DATE" HeaderText="<%$Resources:Resource,wfb2ib_lb_PAYMENT_DATE_GRID%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="left" />
                    <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2ib_lb_EMP_ID%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="left" />
                    <asp:BoundField DataField="INS_MONTH_AMOUNT" HeaderText="<%$Resources:Resource,wfb2ib_INS_MONTH_AMOUNT%>" HeaderStyle-Width="80px" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="right" />
                    <asp:BoundField DataField="FOUR_AMOUNT" HeaderText="<%$Resources:Resource,wfb2ib_INS_FOUR_AMOUNT%>" HeaderStyle-Width="80px" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="right" />
                    <asp:BoundField DataField="ONE_TIME_AMOUNT" HeaderText="<%$Resources:Resource,wfb2ib_INS_ONE_TIME_AMOUNT%>" HeaderStyle-Width="80px" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="right" />
                    <asp:BoundField DataField="ACCU_AMOUNT" HeaderText="<%$Resources:Resource,wfb2ib_INS_ACCU_AMOUNT%>" HeaderStyle-Width="80px" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="right" />
                    <asp:BoundField DataField="ACCU_OVER_AMOUNT" HeaderText="<%$Resources:Resource,wfb2ib_INS_ACCU_OVER__AMOUNT%>" HeaderStyle-Width="120px" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="right" />
                    <asp:BoundField DataField="INS_COST_BASE" HeaderText="<%$Resources:Resource,wfb2ib_INS_INS_COST_BASE%>" HeaderStyle-Width="100px" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="right" />
                    <asp:BoundField DataField="INS_COST" HeaderText="<%$Resources:Resource,wfb2ib_INS_INS_COST%>" HeaderStyle-Width="80px" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="right" />
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ib_before_INS_COST%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_old_adjust" runat="server" Text='<%#Bind("INS_COST","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <asp:HiddenField ID="HID_PageRow1" runat="server" ClientIDMode="Static" />

            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <tr>
                    <th align="right" height="1">
                        <asp:Label ID="lb_explain" runat="server" Text="<%$Resources:Resource,wfb2ib_lb_explain%>" Visible="false"></asp:Label>
                    </th>
                    <td align="right" height="1">
                        <asp:Label ID="lb_Minus_Value" runat="server" Text="" Visible="false"></asp:Label>
                    </td>
                    <td align="right" height="1">
                        <aces:Btn ID="WFB2IB0700Save" runat="server" Text="<%$Resources:Resource,wfb2ib_WFB2IB0700Save%>" Visible="false" OnClick="WFB2IB0700Save_Click" />

                        <%--<asp:Button ID="WFB2IB0700Save" runat="server" Text="<%$Resources:Resource,wfb2ib_WFB2IB0700Save%>" Visible="false" OnClick="WFB2IB0700Save_Click"/>--%>
                    </td>
                </tr>
            </table>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
