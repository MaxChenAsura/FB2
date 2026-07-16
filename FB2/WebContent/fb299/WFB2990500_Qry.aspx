<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb299/WFB2990500_Qry.aspx.cs" Inherits="WebContent_WFB2990500_Qry" Culture="auto" UICulture="auto" %>

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

            //新主假別的ajax
            $("#txt_NEW_TMLCD").change(function () {
                if ($("#txt_NEW_TMLCD").val().length == 1) {
                    $.ajax({
                        url: "../commgeo/WFB2GetLEAVECDData.ashx",
                        data: {
                              MAIN_LEAVE_CD: $('#txt_NEW_TMLCD').val()
                            , SUB_LEAVE_CD: ""
                        },
                        type: "GET",
                        dataType: 'json',
                        cache: false,
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#lb_NEW_TMDESC').val("");
                                $('#txt_NEW_TSDESC').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#lb_NEW_TMDESC').val(JData.MAIN_LEAVE_DESC);
                                $('#txt_NEW_TSDESC').val(JData.MAIN_LEAVE_DESC);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#lb_NEW_TMDESC').val("");
                    $('#txt_NEW_TSDESC').val("");
                }
            });


            //新子假別的ajax
            $("#txt_NEW_TSLCD").change(function () {
                if ($("#txt_NEW_TSLCD").val().length == 2) {
                    $.ajax({
                        url: "../commgeo/WFB2GetLEAVECDData.ashx",
                        data: {
                            MAIN_LEAVE_CD: ""
                            , SUB_LEAVE_CD: $('#txt_NEW_TSLCD').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        cache: false,
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#lb_NEW_TSDESC').val("");
                                $('#lb_NEW_T4UNIT').val("");
                                $('#txt_NEW_TMDESC').val("");
                                $('#txt_NEW_T4UNIT').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#lb_NEW_TSDESC').val(JData.SUB_LEAVE_DESC);
                                $('#lb_NEW_T4UNIT').val(JData.LEAVE_TIME_UNIT);
                                $('#txt_NEW_TMDESC').val(JData.SUB_LEAVE_DESC);
                                $('#txt_NEW_T4UNIT').val(JData.LEAVE_TIME_UNIT);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#lb_NEW_TSDESC').val("");
                    $('#lb_NEW_T4UNIT').val("");
                    $('#txt_NEW_TMDESC').val("");
                    $('#txt_NEW_T4UNIT').val("");
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
            $("#txt_TMLCD").val("");
            $("#txt_T4LCC").val("");
            $("#txt_TSLCD").val("");
        }

        //取得主假別說明
        function getMAIN_LEAVE_DESC(MAIN_LEAVE_id) {
            var txt = document.getElementById(MAIN_LEAVE_id);
            if (txt.value != "") {
                document.getElementById('hid_getMAIN_LEAVE_DESC').click();
                return false;
            } else {
                $("#txt_MAIN_LEAVE_DESC2").val("");
            }
        }

        //取得子假別說明
        function getSUB_LEAVE_DESC(MAIN_LEAVE_id) {
            var txt = document.getElementById(MAIN_LEAVE_id);
            if (txt.value != "" && txt.value.length==2) {
                document.getElementById('btn_getSUB_LEAVE_DESC').click();
                return false;
            } else {
                $("#txt_SUB_LEAVE_DESC2").val("");
            }
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
                            <%--新主假別代號--%>
                            <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb299_lb_tmlcd%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_TMLCD" runat="server" Width="70px" ClientIDMode="Static" MaxLength="2" onblur="javascript:getMAIN_LEAVE_DESC(this.id)" ></asp:TextBox>
                            <asp:TextBox ID="txt_MAIN_LEAVE_DESC2" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                        </td>
                       <th align="left" class="Body_TableHeader">
                            <%--新子假別代號--%>
                            <asp:Label ID="Label12" runat="server" Text="<%$Resources:Resource,wfb299_lb_tslcd%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_TSLCD" runat="server" Width="70px" ClientIDMode="Static" MaxLength="2" onblur="javascript:getSUB_LEAVE_DESC(this.id)"></asp:TextBox>
                            <asp:TextBox ID="txt_SUB_LEAVE_DESC2" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--舊主假別代號--%>
                            <asp:Label ID="Label13" runat="server" Text="<%$Resources:Resource,wfb299_lb_t4lcc%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_T4LCC" runat="server" Width="70px" ClientIDMode="Static" MaxLength="1"></asp:TextBox>
                        </td>
                        <th></th>
                        <td align="left" class="Body_label">
                        </td>
                    </tr>
                    <tr>
                        <th></th>
                        <td align="right" colspan="4">
                            
                            <aces:Btn ID="WFB2990500Search" runat="server" Text="查詢" OnClick="WFB2990500Search_Click" OnClientClick="CheckSearch();" />
                            <%--
                            <asp:Button ID="WFB2990500Search" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_search%>" OnClick="WFB2990500Search_Click" OnClientClick="CheckSearch();" />
                             --%>
                            <input id="btn_clear" runat="server" type="button" value="清除" onclick="ClearAll();" />
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

                                <aces:Btn ID="WFB2990500Add" runat="server" Text="新增" Visible="true" OnClick="WFB2990500Add_Click" />
                                <aces:Btn ID="WFB2990500Delete" runat="server" Text="刪除" Visible="false" OnClick="WFB2990500Delete_Click" OnClientClick="return doDelete();" />
                                <aces:Btn ID="WFB2990500Edit" runat="server" Text="修改" Visible="false" OnClick="WFB2990500Edit_Click" OnClientClick="BlockUI();" />
                                <aces:Btn ID="WFB2990500OK" runat="server" Text="確認" Visible="false" OnClick="WFB2990500OK_Click" OnClientClick="return saveCheck()" />
                                <asp:Button ID="btn_cancel" runat="server" Text="取消" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="btn_cancel_Click" />
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2990500DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_TMLCD"
                        Name="tmlcd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_TSLCD"
                        Name="tslcd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_T4LCC"
                        Name="t4lcc" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
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
                    <%--新主假別代號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_lb_tmlcd%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="TMLCD">
                        <ItemTemplate>
                            <asp:Label ID="lb_TMLCD" runat="server" Text='<%#Bind("TMLCD")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_TMLCD" runat="server" Text='<%#Bind("TMLCD")%>' Width="100px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_TMLCD" runat="server" ClientIDMode="Static" MaxLength="2" CssClass="MandatoryField"  style="TEXT-TRANSFORM:uppercase" Width="80px" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="NEW_TMLCD" runat="server" ErrorMessage="<%$Resources:Resource,wfb299_required_tmlcd%>"
                                ControlToValidate="txt_NEW_TMLCD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--新主假別名稱--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_lb_tmdesc%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="TMDESC">
                        <ItemTemplate>
                            <asp:Label ID="lb_TMDESC" runat="server" Text='<%#Bind("TMDESC")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                             <asp:Label ID="lb_TMDESC" runat="server" Text='<%#Bind("TMDESC")%>' Width="100px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox runat="server" ID="lb_NEW_TMDESC" BorderWidth="0" Enabled="false"  ClientIDMode="Static" Style="background-color: white; color: black;" />
                            <%-- 
                            <asp:Label ID="lb_NEW_TMDESC" runat="server" Text='<%#Bind("TMDESC")%>' Width="100px"></asp:Label>
                                --%>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--新子假別代號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_lb_tslcd%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="TSLCD">
                        <ItemTemplate>
                            <asp:Label ID="lb_TSLCD" runat="server" Text='<%#Bind("TSLCD")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_TSLCD" runat="server" Text='<%#Bind("TSLCD")%>' Width="100px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_TSLCD" runat="server" ClientIDMode="Static" MaxLength="4"  CssClass="MandatoryField" style="TEXT-TRANSFORM:uppercase" Width="80px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="NEW_TSLCD2" runat="server" ErrorMessage="<%$Resources:Resource,wfb299_required_tmlcd%>"
                                ControlToValidate="txt_NEW_TSLCD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                             <asp:RegularExpressionValidator ID="RegularExpressionValidator0" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb299_format_tmlcd%>" ControlToValidate="txt_NEW_TSLCD" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression=".{2,2}" Display="None"></asp:RegularExpressionValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--新子假別名稱--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_lb_tsdesc%>" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left" SortExpression="TSDESC">
                        <ItemTemplate>
                            <asp:Label ID="lb_TSDESC" runat="server" Text='<%#Bind("TSDESC")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_TSDESC" runat="server" Text='<%#Bind("TSDESC")%>' Width="120px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <%-- 
                             <asp:Label ID="lb_NEW_TSDESC" runat="server"  Width="120px"></asp:Label>
                                --%>
                             <asp:TextBox runat="server" ID="lb_NEW_TSDESC" BorderWidth="0" Width="120px" Enabled="false"  ClientIDMode="Static" Style="background-color: white; color: black;" />   
                            
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--新假別時間單位--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_lb_t4unit%>" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left" SortExpression="T4UNIT">
                        <ItemTemplate>
                            <asp:Label ID="lb_T4UNIT" runat="server" Text='<%#Bind("T4UNIT")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_T4UNIT" runat="server" Text='<%#Bind("T4UNIT")%>' Width="120px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox runat="server" ID="lb_NEW_T4UNIT" BorderWidth="0" Width="120px" Enabled="false"  ClientIDMode="Static" Style="background-color: white; color: black;" />   
                        </FooterTemplate>
                    </asp:TemplateField>
                     <%--舊主假別代號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_lb_t4lcc%>" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left" SortExpression="T4LCC">
                        <ItemTemplate>
                            <asp:Label ID="lb_T4LCC" runat="server" Text='<%#Bind("T4LCC")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                             <asp:TextBox ID="txt_EDIT_T4LCC" runat="server" Text='<%#Bind("T4LCC")%>' ClientIDMode="Static" MaxLength="1" CssClass="MandatoryField"  style="TEXT-TRANSFORM:uppercase" Width="80px" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="EDIT_T4LCC" runat="server" ErrorMessage="<%$Resources:Resource,wfb299_required_t4lcc%>"
                                ControlToValidate="txt_EDIT_T4LCC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="EDIT_T4LCC2" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb299_format_t4lcc%>" ControlToValidate="txt_EDIT_T4LCC" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression=".{1,1}" Display="None"></asp:RegularExpressionValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_T4LCC" runat="server" ClientIDMode="Static" MaxLength="1" CssClass="MandatoryField"  style="TEXT-TRANSFORM:uppercase" Width="80px" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="NEW_T4LCC" runat="server" ErrorMessage="<%$Resources:Resource,wfb299_required_t4lcc%>"
                                ControlToValidate="txt_NEW_T4LCC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="NEW_T4LCC2" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb299_format_t4lcc%>" ControlToValidate="txt_NEW_T4LCC" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression=".{1,1}" Display="None"></asp:RegularExpressionValidator>
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
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb299_lb_tmlcd%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb299_lb_tmdesc%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb299_lb_tslcd%>" Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb299_lb_tsdesc%>" Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb299_lb_t4unit%>" Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb299_lb_t4lcc%>" Width="120px"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td></td>
                            <td></td>
                            <td>
                            <asp:TextBox ID="txt_NEW_TMLCD" runat="server" ClientIDMode="Static" MaxLength="2" CssClass="MandatoryField"  style="TEXT-TRANSFORM:uppercase" Width="80px" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="NEW_TMLCD" runat="server" ErrorMessage="<%$Resources:Resource,wfb299_required_tmlcd%>"
                                ControlToValidate="txt_NEW_TMLCD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                 <asp:TextBox runat="server" ID="txt_NEW_TSDESC" BorderWidth="0" Width="120px" Enabled="false"  ClientIDMode="Static" Style="background-color: white; color: black;" />   
                            </td>
                            <td>
                               <asp:TextBox ID="txt_NEW_TSLCD" runat="server" ClientIDMode="Static" MaxLength="4"  CssClass="MandatoryField" style="TEXT-TRANSFORM:uppercase" Width="80px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="NEW_TSLCD2" runat="server" ErrorMessage="<%$Resources:Resource,wfb299_required_tslcd%>"
                                ControlToValidate="txt_NEW_TSLCD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                             <asp:RegularExpressionValidator ID="RegularExpressionValidator0" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb299_format_tmlcd%>" ControlToValidate="txt_NEW_TSLCD" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression=".{2,2}" Display="None"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txt_NEW_TMDESC" BorderWidth="0" Enabled="false"  ClientIDMode="Static" Style="background-color: white; color: black;" />
                            </td>
                            <td>
                                 <asp:TextBox runat="server" ID="txt_NEW_T4UNIT" BorderWidth="0" Width="120px" Enabled="false"  ClientIDMode="Static" Style="background-color: white; color: black;" />   
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_T4LCC" runat="server" ClientIDMode="Static" MaxLength="1" CssClass="MandatoryField"  style="TEXT-TRANSFORM:uppercase" Width="80px" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="NEW_T4LCC" runat="server" ErrorMessage="<%$Resources:Resource,wfb299_required_t4lcc%>"
                                    ControlToValidate="txt_NEW_T4LCC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="NEW_T4LCC2" runat="server"
                                        ErrorMessage="<%$Resources:Resource,wfb299_format_t4lcc%>" ControlToValidate="txt_NEW_T4LCC" ForeColor="Red" ValidationGroup="GroupA"
                                        ValidationExpression=".{1,1}" Display="None"></asp:RegularExpressionValidator>
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

            <asp:Button ID="hid_getMAIN_LEAVE_DESC" runat="server" Style="display: none" ClientIDMode="Static" OnClick="hid_getMAIN_LEAVE_DESC_Click" />
            <asp:Button ID="btn_getSUB_LEAVE_DESC" runat="server" Style="display: none" ClientIDMode="Static" OnClick="btn_getSUB_LEAVE_DESC_Click" />

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
