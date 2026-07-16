<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2ha/WFB2HA0700_Qry.aspx.cs" Inherits="WebContent_WFB2HA0700_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">

        $(function () {

            iniForm();
        });
        function iniForm() {
            //日期格式心須
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.date').mask('9999/99/99');
            $(".numFormat").mask('9.999');
            $(".decimal").css("text-align", "right").css("ime-mode", "disabled");

            reComma("AWARD_BASE", 3);
            //GridView必須
            gridviewScroll();
            $.unblockUI();

            //部門代號取得部門名稱的ajax
            $("#txt_TPARTN").change(function () {
                if ($("#txt_TPARTN").val().length == 7) {
                    $.ajax({
                        url: "../commgeo/WFB2GetDeptData.ashx",
                        data: {
                            DEPT_NO: $('#txt_TPARTN').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        cache: false,
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_qry_DEPT_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_qry_DEPT_NAME').val(JData.DEPT_NAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_qry_DEPT_NAME').val("");
                }
            });

            $("#txt_NEW_TPARTN").change(function () {
                if ($("#txt_NEW_TPARTN").val().length == 7) {
                    $.ajax({
                        url: "../commgeo/WFB2GetDeptData.ashx",
                        data: {
                            DEPT_NO: $('#txt_NEW_TPARTN').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        cache: false,
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#lb_NEW_TPARTN1').text("");
                                $('#lb_NEW_TPARTN2').text("");
                                $('#lb_NEW_TPARTN3').text("");
                                $('#lb_NEW_TPARTN4').text("");
                                $('#lb_NEW_TPARTN5').text("");
                                $('#lb_NEW_TPARTN6').text("");
                                $('#lb_NEW_THWKNO').text("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#lb_NEW_TPARTN1').text(JData.DEPT_NAME_20);
                                $('#lb_NEW_TPARTN2').text(JData.DEPT_NAME_30);
                                $('#lb_NEW_TPARTN3').text(JData.DEPT_NAME_40);
                                $('#lb_NEW_TPARTN4').text(JData.DEPT_NAME_50);
                                $('#lb_NEW_TPARTN5').text(JData.DEPT_NAME_60);
                                $('#lb_NEW_TPARTN6').text(JData.DEPT_NAME_70);
                                $('#lb_NEW_THWKNO').text(JData.HEAD_EMP_ID);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#lb_NEW_TPARTN1').text("");
                    $('#lb_NEW_TPARTN2').text("");
                    $('#lb_NEW_TPARTN3').text("");
                    $('#lb_NEW_TPARTN4').text("");
                    $('#lb_NEW_TPARTN5').text("");
                    $('#lb_NEW_TPARTN6').text("");
                    $('#lb_NEW_THWKNO').text("");
                }
            });

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

        //刪除,檢查是否有勾選
        function doDelete() {
            var processed = true;
            BlockUI();
            if (checkboxsSelected() > 0) {
                processed = confirm("確定要刪除?");
            } else {
                alert("請選取資料!");
                processed = false;
            }
            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }

        var choietr = null;
        //回傳目前Checkbox被勾選的數量
        function checkboxsSelected() {
            choietr = null;
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                    if (choietr == null) {
                        choietr = i;
                    }
                }
            }
            return HaveCheck;
        }

        //查詢前檢核
        function CheckSearch() {
            BlockUI();
            //其它需要檢核的
            /*
            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            */
        }

        ////將DIV的scollbar跑到最低,移到Basic.js
        //function gridViewScrollBottom(id) {
        //    $("table[id$="+id+"]").parent().scrollTop(99999);
        //}


        //儲存前檢查
        function saveCheck() {
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

        //清空畫面
        function ClearAll() {
            $("#txt_TPARTO").val("");
            $("#txt_TPARTN").val("");
            $("#txt_qry_DEPT_NAME").val("");
        }



    </script>
    <style type="text/css">
        .auto-style1 {
            height: 23px;
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="15%" />
                    <col width="35%" />
                    <col width="15%" />
                    <col width="35%" />

                </colgroup>
                <tbody>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--舊部門代號--%>
                            <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_tparto%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_TPARTO" runat="server" Width="70px" ClientIDMode="Static" MaxLength="6"></asp:TextBox>
                        </td>
                       <th align="left" class="Body_TableHeader">
                            <%--新部門代號--%>
                            <asp:Label ID="Label12" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_tpartn%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_TPARTN" runat="server" Width="70px" ClientIDMode="Static" MaxLength="7"></asp:TextBox>
                            <input id="bt_DEPT_SEARCH" type="button" value="..." onclick="OpenDeptSearch('txt_TPARTN', 'txt_qry_DEPT_NAME', 'N', '');" />
                            <asp:TextBox ID="txt_qry_DEPT_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" Width="140px" />
                        </td>
                    </tr>
                    <tr>
                        <th></th>
                        <td align="right" colspan="4">
                            
                            <aces:Btn ID="WFB2HA0700Search" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_search%>" OnClick="WFB2HA0700Search_Click" OnClientClick="CheckSearch();" />
                            <%--
                            <asp:Button ID="WFB2HA0700Search" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_search%>" OnClick="WFB2HA0700Search_Click" OnClientClick="CheckSearch();" />
                             --%>
                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2sh_btn_clear%>" onclick="ClearAll();" />
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

                                <aces:Btn ID="WFB2HA0700Add" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_add%>" Visible="true" OnClick="WFB2HA0700Add_Click" />
                                <aces:Btn ID="WFB2HA0700Delete" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_delete%>" Visible="false" OnClick="WFB2HA0700Delete_Click" OnClientClick="return doDelete();" />
                                <%-- 
                                <aces:Btn ID="WFB2HA0700Edit" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_edit%>" Visible="false" OnClick="WFB2HA0700Edit_Click" OnClientClick="BlockUI();" />
                                --%>
                                <aces:Btn ID="WFB2HA0700OK" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_ok%>" Visible="false" OnClick="WFB2HA0700OK_Click" OnClientClick="return saveCheck()" />
                                <asp:Button ID="btn_cancel" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_cancel%>" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="btn_cancel_Click" />
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2HA0700DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_TPARTO"
                        Name="tparto" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_TPARTN"
                        Name="tpartn" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
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
                        <EditItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--舊部門代號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_tparto%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="TPARTO">
                        <ItemTemplate>
                            <asp:Label ID="lb_TPARTO" runat="server" Text='<%#Bind("TPARTO")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                             <asp:HiddenField ID="hid_EDIT_TPART" runat="server" Value='<%#Bind("TPARTO")%> ' ClientIDMode="Static" />
                            <asp:TextBox ID="txt_EDIT_TPARTO" runat="server" Text='<%#Bind("TPARTO")%>' Width="100px" MaxLength="6" CssClass="MandatoryField" style="TEXT-TRANSFORM:uppercase" onkeyup="value=value.replace(/[\W]/g,'')"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="EDIT_TPARTO" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_tparto%>"
                                ControlToValidate="txt_EDIT_TPARTO" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:HiddenField ID="hid_EDIT_TPART" runat="server" Value='<%#Bind("TPARTO")%> ' ClientIDMode="Static" />
                            <asp:TextBox ID="txt_NEW_TPARTO" runat="server" ClientIDMode="Static" MaxLength="6" CssClass="MandatoryField"  style="TEXT-TRANSFORM:uppercase" Width="80px" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="NEW_TPARTO" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_tparto%>"
                                ControlToValidate="txt_NEW_TPARTO" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2ha_format_tparto%>" ControlToValidate="txt_NEW_TPARTO" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression=".{6,6}" Display="None"></asp:RegularExpressionValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--新部門代號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_tpartn%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="TPARTN">
                        <ItemTemplate>
                            <asp:Label ID="lb_TPARTN" runat="server" Text='<%#Bind("TPARTN")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_TPARTN" runat="server" Text='<%#Bind("TPARTN")%>' Width="100px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_TPARTN" runat="server" ClientIDMode="Static" MaxLength="7"  CssClass="MandatoryField" style="TEXT-TRANSFORM:uppercase" Width="80px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="NEW_TPARTN" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_tpartn1%>"
                                ControlToValidate="txt_NEW_TPARTN" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                             <asp:RegularExpressionValidator ID="RegularExpressionValidator0" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2ha_format_tpartn1%>" ControlToValidate="txt_NEW_TPARTN" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression=".{7,7}" Display="None"></asp:RegularExpressionValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--新部級單位名稱--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_tpartn1%>" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left" SortExpression="TPARTN1">
                        <ItemTemplate>
                            <asp:Label ID="lb_TPARTN1" runat="server" Text='<%#Bind("TPARTN1")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_TPARTN1" runat="server" Text='<%#Bind("TPARTN1")%>' Width="120px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                             <asp:Label ID="lb_NEW_TPARTN1" runat="server" Text='<%#Bind("TPARTN1")%>' Width="120px"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--新室級單位名稱--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_tpartn2%>" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left" SortExpression="TPARTN2">
                        <ItemTemplate>
                            <asp:Label ID="lb_TPARTN2" runat="server" Text='<%#Bind("TPARTN2")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_TPARTN2" runat="server" Text='<%#Bind("TPARTN2")%>' Width="120px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                             <asp:Label ID="lb_NEW_TPARTN2" runat="server" Text='<%#Bind("TPARTN2")%>' Width="120px"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                     <%--新課級單位名稱--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_tpartn3%>" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left" SortExpression="TPARTN3">
                        <ItemTemplate>
                            <asp:Label ID="lb_TPARTN3" runat="server" Text='<%#Bind("TPARTN3")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_TPARTN3" runat="server" Text='<%#Bind("TPARTN3")%>' Width="120px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                             <asp:Label ID="lb_NEW_TPARTN3" runat="server" Text='<%#Bind("TPARTN3")%>' Width="120px"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                     <%--新工級單位名稱--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_tpartn4%>" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left" SortExpression="TPARTN4">
                        <ItemTemplate>
                            <asp:Label ID="lb_TPARTN4" runat="server" Text='<%#Bind("TPARTN4")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_TPARTN4" runat="server" Text='<%#Bind("TPARTN4")%>' Width="120px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                             <asp:Label ID="lb_NEW_TPARTN4" runat="server" Text='<%#Bind("TPARTN4")%>' Width="120px"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                     <%--新組級單位名稱--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_tpartn5%>" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left" SortExpression="TPARTN5">
                        <ItemTemplate>
                            <asp:Label ID="lb_TPARTN5" runat="server" Text='<%#Bind("TPARTN5")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_TPARTN5" runat="server" Text='<%#Bind("TPARTN5")%>' Width="120px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                             <asp:Label ID="lb_NEW_TPARTN5" runat="server" Text='<%#Bind("TPARTN5")%>' Width="120px"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                     <%--新班級單位名稱--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_tpartn6%>" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left" SortExpression="TPARTN6">
                        <ItemTemplate>
                            <asp:Label ID="lb_TPARTN6" runat="server" Text='<%#Bind("TPARTN6")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_TPARTN6" runat="server" Text='<%#Bind("TPARTN6")%>' Width="120px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                             <asp:Label ID="lb_NEW_TPARTN6" runat="server" Text='<%#Bind("TPARTN6")%>' Width="120px"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                     <%--部門主管--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_thwkno%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="THWKNO">
                        <ItemTemplate>
                            <asp:Label ID="lb_THWKNO" runat="server" Text='<%#Bind("THWKNO")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_THWKNO" runat="server" Text='<%#Bind("THWKNO")%>' Width="100px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                             <asp:Label ID="lb_NEW_THWKNO" runat="server" Text='<%#Bind("THWKNO")%>' Width="100px"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
                <%--當DB無資料時，就會使用此table --%>
                <EmptyDataTemplate>
                    <table class="grid-view" width="1020px">
                        <tr class="header">
                            <td>
                                <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" Width="20px" />
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_rownumber%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_tparto%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_tpartn%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_tpartn1%>" Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_tpartn2%>" Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_tpartn3%>" Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_tpartn4%>" Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_tpartn5%>" Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label10" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_tpartn6%>" Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_thwkno%>" Width="100px"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td></td>
                            <td></td>
                            <td>
                                 <asp:HiddenField ID="hid_EDIT_TPART" runat="server" Value='<%#Bind("TPARTO")%> ' ClientIDMode="Static" />
                                <asp:TextBox ID="txt_NEW_TPARTO" runat="server" ClientIDMode="Static" MaxLength="6" CssClass="MandatoryField"  style="TEXT-TRANSFORM:uppercase" Width="80px" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="NEW_TPARTO" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_tparto%>"
                                    ControlToValidate="txt_NEW_TPARTO" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server"
                                        ErrorMessage="<%$Resources:Resource,wfb2ha_format_tparto%>" ControlToValidate="txt_NEW_TPARTO" ForeColor="Red" ValidationGroup="GroupA"
                                        ValidationExpression=".{6,6}" Display="None"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_TPARTN" runat="server" ClientIDMode="Static" MaxLength="7"  CssClass="MandatoryField" style="TEXT-TRANSFORM:uppercase" Width="80px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="NEW_TPARTN" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_tpartn1%>"
                                    ControlToValidate="txt_NEW_TPARTN" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                 <asp:RegularExpressionValidator ID="RegularExpressionValidator0" runat="server"
                                        ErrorMessage="<%$Resources:Resource,wfb2ha_format_tpartn1%>" ControlToValidate="txt_NEW_TPARTN" ForeColor="Red" ValidationGroup="GroupA"
                                        ValidationExpression=".{7,7}" Display="None"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:Label ID="lb_NEW_TPARTN1" runat="server" Text='<%#Bind("TPARTN1")%>' Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lb_NEW_TPARTN2" runat="server" Text='<%#Bind("TPARTN2")%>' Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lb_NEW_TPARTN3" runat="server" Text='<%#Bind("TPARTN3")%>' Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lb_NEW_TPARTN4" runat="server" Text='<%#Bind("TPARTN3")%>' Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lb_NEW_TPARTN5" runat="server" Text='<%#Bind("TPARTN3")%>' Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lb_NEW_TPARTN6" runat="server" Text='<%#Bind("TPARTN3")%>' Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lb_NEW_THWKNO" runat="server" Text='<%#Bind("TPARTN3")%>' Width="100px"></asp:Label>
                            </td>
                    </table>
                </EmptyDataTemplate>
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
            <!-- 每頁的筆數 -->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <!-- 是否要凍結視窗 -->
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="N" />
            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
