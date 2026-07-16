<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2ha/WFB2HA0800_Qry.aspx.cs" Inherits="WebContent_WFB2HA0800_Qry" Culture="auto" UICulture="auto" %>

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
            var today = getTodayDate();
            $('#hid_today').val(today);

            //職務代號取得職務相關的ajax
            $("#txt_NEW_TPJOBN").change(function () {
                if ($("#txt_NEW_TPJOBN").val().length == 4) {
                    $.ajax({
                        url: "../commgeo/WFB2GetPjobData.ashx",
                        data: {
                            PJOB_CD: $('#txt_NEW_TPJOBN').val(),
                            START_DT: $('#hid_today').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        cache: false,
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#lb_NEW_T1LEVL').text("");
                                $('#lb_NEW_T1WSID').text("");
                                $('#lb_NEW_TPJNMN').text("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#lb_NEW_T1LEVL').text(JData.LEVEL_CD);
                                $('#lb_NEW_T1WSID').text(JData.WS_CD);
                                $('#lb_NEW_TPJNMN').text(JData.PJOB_DESC);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#lb_NEW_T1LEVL').text("");
                    $('#lb_NEW_T1WSID').text("");
                    $('#lb_NEW_TPJNMN').text("");
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
            $("#txt_TPJOBO").val("");
            $("#txt_T1LVLO").val("");
            $("#txt_TPJOBN").val("");
            $("#txt_T1LEVL").val("");
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
                            <%--舊職務代號--%>
                            <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_tpjobo%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_TPJOBO" runat="server" Width="70px" ClientIDMode="Static" MaxLength="2"></asp:TextBox>
                        </td>
                       <th align="left" class="Body_TableHeader">
                            <%--舊資格代號--%>
                            <asp:Label ID="Label12" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_t1lvlo%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_T1LVLO" runat="server" Width="70px" ClientIDMode="Static" MaxLength="2"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--新職務代號--%>
                            <asp:Label ID="Label13" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_tpjobn%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_TPJOBN" runat="server" Width="70px" ClientIDMode="Static" MaxLength="4"></asp:TextBox>
                        </td>
                       <th align="left" class="Body_TableHeader">
                            <%--新資格代號--%>
                            <asp:Label ID="Label14" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_t1levl%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_T1LEVL" runat="server" Width="70px" ClientIDMode="Static" MaxLength="3"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th></th>
                        <td align="right" colspan="4">
                            
                            <aces:Btn ID="WFB2HA0800Search" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_search%>" OnClick="WFB2HA0800Search_Click" OnClientClick="CheckSearch();" />
                            <%--
                            <asp:Button ID="WFB2HA0800Search" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_search%>" OnClick="WFB2HA0800Search_Click" OnClientClick="CheckSearch();" />
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

                                <aces:Btn ID="WFB2HA0800Add" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_add%>" Visible="true" OnClick="WFB2HA0800Add_Click" />
                                <aces:Btn ID="WFB2HA0800Delete" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_delete%>" Visible="false" OnClick="WFB2HA0800Delete_Click" OnClientClick="return doDelete();" />
                                <%-- 
                                <aces:Btn ID="WFB2HA0800Edit" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_edit%>" Visible="false" OnClick="WFB2HA0800Edit_Click" OnClientClick="BlockUI();" />
                                --%>
                                <aces:Btn ID="WFB2HA0800OK" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_ok%>" Visible="false" OnClick="WFB2HA0800OK_Click" OnClientClick="return saveCheck()" />
                                <asp:Button ID="btn_cancel" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_cancel%>" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="btn_cancel_Click" />
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2HA0800DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_TPJOBO"
                        Name="tpjobo" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_T1LVLO"
                        Name="t1lvlo" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_TPJOBN"
                        Name="tpjobn" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_T1LEVL"
                        Name="t1levl" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
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
                    <%--舊職務代號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_tpjobo%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="TPJOBO">
                        <ItemTemplate>
                            <asp:Label ID="lb_TPJOBO" runat="server" Text='<%#Bind("TPJOBO")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                             <asp:HiddenField ID="hid_EDIT_TPJOBO" runat="server" Value='<%#Bind("TPJOBO")%> ' ClientIDMode="Static" />
                            <asp:TextBox ID="txt_EDIT_TPJOBO" runat="server" Text='<%#Bind("TPJOBO")%>' Width="100px" MaxLength="2" CssClass="MandatoryField" style="TEXT-TRANSFORM:uppercase" onkeyup="value=value.replace(/[\W]/g,'')"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="EDIT_TPARTO" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_tpjobo%>"
                                ControlToValidate="txt_EDIT_TPJOBO" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:HiddenField ID="hid_EDIT_TPJOBO" runat="server" Value='<%#Bind("TPJOBO")%> ' ClientIDMode="Static" />
                            <asp:TextBox ID="txt_NEW_TPJOBO" runat="server" ClientIDMode="Static" MaxLength="2" CssClass="MandatoryField"  style="TEXT-TRANSFORM:uppercase" Width="80px" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="NEW_TPJOBO" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_tpjobo%>"
                                ControlToValidate="txt_NEW_TPJOBO" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="NEW_TPJOBO2" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2ha_format_tpjobo%>" ControlToValidate="txt_NEW_TPJOBO" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression=".{2,2}" Display="None"></asp:RegularExpressionValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--舊資格代號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_t1lvlo%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="T1LVLO">
                        <ItemTemplate>
                            <asp:Label ID="lb_T1LVLO" runat="server" Text='<%#Bind("T1LVLO")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                             <asp:HiddenField ID="hid_EDIT_T1LVLO" runat="server" Value='<%#Bind("T1LVLO")%> ' ClientIDMode="Static" />
                            <asp:TextBox ID="txt_EDIT_T1LVLO" runat="server" Text='<%#Bind("T1LVLO")%>' Width="100px" MaxLength="2" CssClass="MandatoryField" style="TEXT-TRANSFORM:uppercase" onkeyup="value=value.replace(/[\W]/g,'')"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="EDIT_TPARTO" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_t1lvlo%>"
                                ControlToValidate="txt_EDIT_T1LVLO" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:HiddenField ID="hid_EDIT_T1LVLO" runat="server" Value='<%#Bind("T1LVLO")%> ' ClientIDMode="Static" />
                            <asp:TextBox ID="txt_NEW_T1LVLO" runat="server" ClientIDMode="Static" MaxLength="2" CssClass="MandatoryField"  style="TEXT-TRANSFORM:uppercase" Width="80px" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="NEW_T1LVLO" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_t1lvlo%>"
                                ControlToValidate="txt_NEW_T1LVLO" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2ha_format_t1lvlo%>" ControlToValidate="txt_NEW_T1LVLO" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression=".{2,2}" Display="None"></asp:RegularExpressionValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--新職務代號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_tpjobn%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="TPJOBN">
                        <ItemTemplate>
                            <asp:Label ID="lb_TPJOBN" runat="server" Text='<%#Bind("TPJOBN")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_TPJOBN" runat="server" Text='<%#Bind("TPJOBN")%>' Width="100px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_TPJOBN" runat="server" ClientIDMode="Static" MaxLength="4"  CssClass="MandatoryField" style="TEXT-TRANSFORM:uppercase" Width="80px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="NEW_TPJOBN2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_tpjobn%>"
                                ControlToValidate="txt_NEW_TPJOBN" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                             <asp:RegularExpressionValidator ID="RegularExpressionValidator0" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2ha_format_tpjobn%>" ControlToValidate="txt_NEW_TPJOBN" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression=".{4,4}" Display="None"></asp:RegularExpressionValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--新資格代號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_t1levl%>" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left" SortExpression="T1LEVL">
                        <ItemTemplate>
                            <asp:Label ID="lb_T1LEVL" runat="server" Text='<%#Bind("T1LEVL")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_T1LEVL" runat="server" Text='<%#Bind("T1LEVL")%>' Width="120px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                             <asp:Label ID="lb_NEW_T1LEVL" runat="server"  Width="120px"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--新職種--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_t1wsid%>" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left" SortExpression="T1WSID">
                        <ItemTemplate>
                            <asp:Label ID="lb_T1WSID" runat="server" Text='<%#Bind("T1WSID")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_T1WSID" runat="server" Text='<%#Bind("T1WSID")%>' Width="120px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                             <asp:Label ID="lb_NEW_T1WSID" runat="server"  Width="120px"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                     <%--新職務名稱--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_tpjnmn%>" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left" SortExpression="TPJNMN">
                        <ItemTemplate>
                            <asp:Label ID="lb_TPJNMN" runat="server" Text='<%#Bind("TPJNMN")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_TPJNMN" runat="server" Text='<%#Bind("TPJNMN")%>' Width="120px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                             <asp:Label ID="lb_NEW_TPJNMN" runat="server" Width="120px"></asp:Label>
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
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_tpjobo%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_t1lvlo%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_tpjobn%>" Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_t1levl%>" Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_t1wsid%>" Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_tpjnmn%>" Width="120px"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td></td>
                            <td></td>
                            <td>
                                  <asp:HiddenField ID="hid_EDIT_TPJOBO" runat="server" Value='<%#Bind("TPJOBO")%> ' ClientIDMode="Static" />
                            <asp:TextBox ID="txt_NEW_TPJOBO" runat="server" ClientIDMode="Static" MaxLength="2" CssClass="MandatoryField"  style="TEXT-TRANSFORM:uppercase" Width="80px" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="NEW_TPJOBO" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_tpjobo%>"
                                ControlToValidate="txt_NEW_TPJOBO" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="NEW_TPJOBO2" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2ha_format_tpjobo%>" ControlToValidate="txt_NEW_TPJOBO" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression=".{2,2}" Display="None"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:HiddenField ID="hid_EDIT_T1LVLO" runat="server" Value='<%#Bind("T1LVLO")%> ' ClientIDMode="Static" />
                                <asp:TextBox ID="txt_NEW_T1LVLO" runat="server" ClientIDMode="Static" MaxLength="2" CssClass="MandatoryField"  style="TEXT-TRANSFORM:uppercase" Width="80px" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="NEW_T1LVLO" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_t1lvlo%>"
                                    ControlToValidate="txt_NEW_T1LVLO" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server"
                                        ErrorMessage="<%$Resources:Resource,wfb2ha_format_t1lvlo%>" ControlToValidate="txt_NEW_T1LVLO" ForeColor="Red" ValidationGroup="GroupA"
                                        ValidationExpression=".{2,2}" Display="None"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                               <asp:TextBox ID="txt_NEW_TPJOBN" runat="server" ClientIDMode="Static" MaxLength="4"  CssClass="MandatoryField" style="TEXT-TRANSFORM:uppercase" Width="80px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="NEW_TPJOBN2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_tpjobn%>"
                                ControlToValidate="txt_NEW_TPJOBN" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                             <asp:RegularExpressionValidator ID="RegularExpressionValidator0" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2ha_format_tpjobn%>" ControlToValidate="txt_NEW_TPJOBN" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression=".{4,4}" Display="None"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:Label ID="lb_NEW_T1LEVL" runat="server"  Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lb_NEW_T1WSID" runat="server"  Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lb_NEW_TPJNMN" runat="server" Width="120px"></asp:Label>
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
