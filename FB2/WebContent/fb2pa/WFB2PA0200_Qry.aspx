<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2pa/WFB2PA0200_Qry.aspx.cs" Inherits="WebContent_WFB2PA0200_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
     <style type="text/css">
        
    </style>
    <script type="text/javascript">

        $(function () {

            iniForm();
        });
        function iniForm() {
            //日期格式心須
            $(".date").datepicker({ dateFormat: 'yy/mm' });
            $('.date').mask('9999/99');
            $(".numFormat").mask('99999');
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
            var processed = true;
            BlockUI();
            return processed;
        }

        //清空畫面
        function ClearAll() {
            $("#txt_YM").val("");
            $("#txt_BARCODE_NO").val("");
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#ddl_IS_YN").val("-1");
        }


    </script>
    <style type="text/css">
        .auto-style1 {
            height: 23px;
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="15%" />
                    <col width="20%" />
                    <col width="15%" />
                    <col width="20%" />
                    <col width="20%" />
                    <col width="20%" />
                </colgroup>
                <tbody>
                     <tr>                        
                         <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_YM" runat="server" Text="提案年月"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                             <asp:TextBox ID="txt_YM" runat="server" MaxLength="7" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                        </td>
                         <th align="left" class="Body_TableHeader">
                            <asp:Label ID="Label1" runat="server" Text="提案條碼"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                             <asp:TextBox ID="txt_BARCODE_NO" runat="server" MaxLength="7" Width="81px" ClientIDMode="Static" ></asp:TextBox>    
                             <asp:HiddenField ID="hid_LAST_YM" runat="server" ClientIDMode="Static" />             
                        </td>
                   </tr>
                   <tr>
                         <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lbl_IS_YN" runat="server" Text="提案保留"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_IS_YN" runat="server" Width="100px" ClientIDMode="Static"> </asp:DropDownList>
                        </td>
                          <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lbl_EMP_ID" runat="server" Text="員工編號"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static"></asp:TextBox>
                            <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N');" />
                            <asp:TextBox ID="txt_EMP_NAME" runat="server" Width="90px" ClientIDMode="Static" BorderWidth="0" ></asp:TextBox> 
                        </td>       
                    </tr>                   
                    <tr>
                        <th></th>
                        <th></th>
                        <th></th>
                        <td align="right" class="Body_label">
                            <div id="init">
                                <aces:Btn ID="WFB2PA0200Search" runat="server" Text="查詢" OnClick="WFB2PA0200Search_Click" OnClientClick="return CheckSearch();" />
                           
                                <%--<asp:Button ID="WFB2IA0100Search" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0100Search%>" OnClick="WFB2IA0100Search_Click" OnClientClick="BlockUI();" />--%>
                                            
                                <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2ia_btn_clear%>" OnClientClick="ClearAll();" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="Body_label" colspan="4">
                            <div id="init_grid">
                                <aces:Btn ID="WFB2PA0200Upd" runat="server" Text="修改" Visible="false" OnClick="WFB2PA0200Upd_Click" OnClientClick="BlockUI();" />
                                <aces:Btn ID="WFB2PA0200Del" runat="server" Text="刪除" Visible="false" OnClick="WFB2PA0200Del_Click" OnClientClick="return doButtonConfirm(this.value);" />
                               
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2PA0200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" /> 
                    <asp:ControlParameter ControlID="txt_YM"
                        Name="ym" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_BARCODE_NO"
                        Name="barcodeNo" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                     <asp:ControlParameter ControlID="ddl_IS_YN"
                        Name="isYn" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" /> 
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="empId" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" Width="20px" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" Width="20px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--序號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_rownumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--提案年月--%>
                    <asp:TemplateField HeaderText="提案年月" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="YM">
                        <ItemTemplate>
                            <asp:Label ID="lb_YM" runat="server" Text='<%#Bind("YM")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--提案條碼--%>
                    <asp:TemplateField HeaderText="提案條碼" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="BARCODE_NO">
                        <ItemTemplate>
                            <asp:Label ID="lb_BARCODE_NO" runat="server" Text='<%#Bind("BARCODE_NO")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--員工編號--%>
                    <asp:TemplateField HeaderText="員工編號" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left" SortExpression="EMP_ID">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>  
                    <%--員工姓名--%>
                    <asp:TemplateField HeaderText="員工姓名" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="EMP_NAME">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>  
                    <%--核定分數--%>
                    <asp:TemplateField HeaderText="核定分數" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Right" SortExpression="BONUS_SCR_FINAL">
                        <ItemTemplate>
                            <asp:Label ID="lb_BONUS_SCR_FINAL" runat="server" Text='<%#Bind("BONUS_SCR_FINAL")%>' Width="60px" CssClass="numFormat"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>  
                    <%--獎金--%>
                    <asp:TemplateField HeaderText="獎金" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" SortExpression="PRO_BONUS">
                        <ItemTemplate>
                            <asp:Label ID="lb_PRO_BONUS" runat="server" Text='<%#Bind("PRO_BONUS")%>' Width="80px" CssClass="numFormat"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>    
                    <%--等級--%>
                    <asp:TemplateField HeaderText="等級" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left" SortExpression="GRADE_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_GRADE_CD" runat="server" Text='<%#Bind("GRADE_CD")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>     
                    <%--等級名稱--%>
                    <asp:TemplateField HeaderText="等級名稱" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="GRADE_NAME">
                        <ItemTemplate>
                            <asp:Label ID="lb_GRADE_NAME" runat="server" Text='<%#Bind("GRADE_NAME")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>      
                    <%--獎金保留--%>
                    <asp:TemplateField HeaderText="獎金保留" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left" SortExpression="IS_YN">
                        <ItemTemplate>
                            <asp:Label ID="lb_IS_YN_DESC" runat="server" Text='<%#Bind("IS_YN_DESC")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>       
                    <%--核發年月--%>
                    <asp:TemplateField HeaderText="核發年月" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left" SortExpression="SALARY_YM">
                        <ItemTemplate>
                            <asp:Label ID="lb_SALARY_YM" runat="server" Text='<%#Bind("SALARY_YM")%>' Width="60px" CssClass="date"></asp:Label>     
                                         <asp:HiddenField ID="hid_SALARY_YM" runat="server" ClientIDMode="Static" Value='<%#Bind("SALARY_YM")%>'/>            
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
            <asp:HiddenField ID="hid_today" runat="server" ClientIDMode="Static" />

            <!-- 每頁的筆數 -->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <!-- 是否要凍結視窗 -->
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="N" />
            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />

        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
