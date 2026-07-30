<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sj/WFB2SJ3500_Qry.aspx.cs" Inherits="WebContent_WFB2SJ3500_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Src="~/UserControl/UCDateTimeRange.ascx" TagPrefix="uc1" TagName="UCDateTimeRange" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">

        $(function () {

            iniForm();
        });
        function iniForm() {
            //日期格式心須
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.date').mask('9999/99/99');
            $(".year").mask('9999');
            $(".numFormat").mask('9.999');
            $(".decimal").css("text-align", "right").css("ime-mode", "disabled");

            //GridView必須
            gridviewScroll();

            //工號取得姓名的ajax
            $("#txt_EMP_ID").change(function () {
                if ($("#txt_EMP_ID").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_EMP_ID').val()
                        },
                        type: "GET",
                        cache: false,
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_EMP_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_EMP_NAME').val(JData.EMP_NAME);
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
            $.unblockUI();

        }

        function ShowRecord(obj) {
            $("#HID_PageRow").val($("#ddlPerPageRow").val());
        }

        //凍結視窗用
        function gridviewScroll() {
            if ($("#HID_Freeze").val() == "Y") {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "400",
                    barcolor: "#7F7F7F",
                    freezesize: 0

                });
                CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
            }
            else {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "400",
                    barcolor: "#7F7F7F"

                });
            }
        }




        //查詢前檢核
        function CheckSearch() {
            /**var processed = true;
            BlockUI();
            return processed;**/
            var processed = true;

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else {
                processed = false;
            }

            if (!processed)
                $.unblockUI();

            return processed;
        }

        //清空畫面
        function ClearAll() {
            $("#txt_ASSESS_YEAR").val("");
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#ddl_ASSESS_TYPE").val("-1");
            $("#ddl_DATASOURCE").val("-1");
            $("#ddl_IS_OUT").val("-1");
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
                                <col width="5%" />
                                <col width="15%" />
                                <col width="5%" />
                                <col width="15%" />
                                <col width="20%" />
                                <col width="40%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_ASSESS_YEAR" runat="server" Text="考核年度"></asp:Label>:
                                    </th>
                                    <td>
                                       <asp:TextBox ID="txt_ASSESS_YEAR" runat="server" Width="60px" ClientIDMode="Static" CssClass="MandatoryField year"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                            ErrorMessage="年度輸入格式錯誤" ControlToValidate="txt_ASSESS_YEAR" ForeColor="Red" ValidationGroup="GroupA"
                                             ValidationExpression=".{4,4}" Display="None"></asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"    ErrorMessage="考核年度必輸入" InitialValue=""
                                 ControlToValidate="txt_ASSESS_YEAR" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                       <asp:Label ID="lb_ASSESS_TYPE" runat="server" Text="考核類型"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                         <asp:DropDownList ID="ddl_ASSESS_TYPE" runat="server" Width="100px" ClientIDMode="Static" CssClass="MandatoryField"> </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="Req_ASSESS_TYPE" runat="server" 
                                            ErrorMessage="考核類型必輸入" InitialValue="-1"
                                            ControlToValidate="ddl_ASSESS_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>                        
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_EMP_ID" runat="server" Text="工號"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                          <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                          <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N');" />
                                          <asp:TextBox ID="txt_EMP_NAME" runat="server" Width="90px" ClientIDMode="Static" BorderWidth="0" ></asp:TextBox> 
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_DATASOURCE" runat="server" Text="資料來源"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_DATASOURCE" runat="server" Width="100px" ClientIDMode="Static" CssClass=""> </asp:DropDownList>
                                    </td>
                                </tr>     
                      
                                <tr>   
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_IS_OUT" runat="server" Text="外數區分"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_IS_OUT" runat="server" Width="100px" ClientIDMode="Static" CssClass=""> </asp:DropDownList>
                                    </td>                     
                                    <th align="left" class="">
                                    </th>
                                    <td align="left" class="Body_label">
                                    </td>
                                </tr>       
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                             <aces:Btn ID="WFB2SJ3500Search" runat="server" Text="查詢" OnClick="WFB2SJ3500Search_Click" OnClientClick="return CheckSearch();" />
                                            <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2ia_btn_clear%>" OnClientClick="ClearAll();" />
                                            <%--<asp:Button ID="WFB2SF1200Execute" runat="server" Text="<%$Resources:Resource,wfb2sf_WFB2SF1200Execute%>" OnClick="WFB2SF1200Execute_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />--%>
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
                <tr>
                    <td>
                        <!--grid1-->
                        <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                            SelectCountMethod="getCount" TypeName="CFB2SJ3500DAO" EnablePaging="True"
                            SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                            StartRowIndexParameterName="startRowIndex"
                            OnSelected="ods1_Selected">
                            <SelectParameters>
                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                                <asp:Parameter Name="maximumRows" Type="Int32" /> 
                                <asp:ControlParameter ControlID="txt_ASSESS_YEAR"
                                    Name="assess_year" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                                 <asp:ControlParameter ControlID="ddl_ASSESS_TYPE"
                                    Name="assess_type" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
                                <asp:ControlParameter ControlID="txt_EMP_ID"
                                    Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />  
                                 <asp:ControlParameter ControlID="ddl_DATASOURCE"
                                    Name="datasource" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
                                 <asp:ControlParameter ControlID="ddl_IS_OUT"
                                    Name="is_out" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
                            </SelectParameters>
                        </asp:ObjectDataSource>

                        <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>                  
                    <%--序號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_rownumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--工號--%>
                    <asp:TemplateField HeaderText="工號" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" SortExpression="EMP_ID">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>' Width="50px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--姓名--%>
                    <asp:TemplateField HeaderText="姓名" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Left" SortExpression="EMP_ID">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--資料來源--%>
                    <asp:TemplateField HeaderText="資料來源" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left" SortExpression="DATASOURCE">
                        <ItemTemplate>
                            <asp:Label ID="lb_DATASOURCE" runat="server" Text='<%#Bind("DATASOURCE_DESC")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField> 
                    <%--考核區分--%>
                    <asp:TemplateField HeaderText="考核區分" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Left" SortExpression="DISTING_CD_DESC">
                        <ItemTemplate>
                            <asp:Label ID="lb_DISTING_CD_DESC" runat="server" Text='<%#Bind("DISTING_CD_DESC")%>' Width="110px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>  
                    <%--外數區分--%>
                    <asp:TemplateField HeaderText="外數區分" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" SortExpression="IS_OUT">
                        <ItemTemplate>
                            <asp:Label ID="lb_IS_OUT" runat="server" Text='<%#Bind("IS_OUT")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>     
                    <%--考課備註--%>
                    <asp:TemplateField HeaderText="考課備註" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left" SortExpression="REMARK">
                        <ItemTemplate>
                            <asp:Label ID="lb_REMARK" runat="server" Text='<%#Bind("REMARK")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>      
                    <%--絕對考課--%>
                    <asp:TemplateField HeaderText="絕對考課" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" SortExpression="ABS_SCORE">
                        <ItemTemplate>
                            <asp:Label ID="lb_ABS_SCORE" runat="server" Text='<%#Bind("ABS_SCORE")%>' Width="50px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>  
                    <%--E考課除外--%>
                    <asp:TemplateField HeaderText="E考課除外" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" SortExpression="EXCEPT_E">
                        <ItemTemplate>
                            <asp:Label ID="lb_EXCEPT_E" runat="server" Text='<%#Bind("EXCEPT_E")%>' Width="50px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField> 
                </Columns>
                <HeaderStyle CssClass="GridviewScrollHeader" />
                <PagerStyle CssClass="GridviewScrollPager" />

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
                    </td>
                </tr>
                <tr>
                    <td>
                        
                    </td>
                </tr>
            </table>





            <!--紀錄頁數(跳頁用)-->
           <asp:HiddenField ID="hid_today" runat="server" ClientIDMode="Static" />

            <!-- 每頁的筆數 -->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <!-- 是否要凍結視窗 -->
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="N" />
            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
