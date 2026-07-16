<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sg/WFB2SG0100_Qry.aspx.cs" Inherits="WebContent_WFB2SG0100_Qry" Culture="auto" UICulture="auto" %>
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
            $(".amt").mask('9999999');
            $(".year").mask('99999');
            $(".decimal").css("text-align", "right").css("ime-mode", "disabled");


            //onblur時，加入comma
            //reComma("FESTIVAL_AMT", 0);
            //GridView必須
            gridviewScroll();
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
                    freezesize: 2

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
            if (checkboxsSelected() > 0) {
                return confirm("確定要刪除?");
            } else {
                alert("請選取資料!");
                return false;
            }
        }

        var choietr = null;
        //回傳目前Checkbox被勾選的數量
        function checkboxsSelected() {
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
            //其它需要檢核的

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
        }
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
            $("#ddl_FESTIVAL_TYPE").val("-1");
        }

        function OpenMultiSelectWindow(window_name, targetObj, festivalType, festivalPayCond, pridCD) {

            //var returnValue = window.showModalDialog("../fb2sg/" + window_name + "?festivalType=" + festivalType + "&festivalPayCond=" + festivalPayCond + "&pridCD=" + pridCD + "&parentFuncId=" + parentFuncID,
            //                    self, 'dialogWidth=520px;dialogHeight=400px;scroll=no');
            //if (returnValue == undefined) {
            //    returnValue = window.returnValue;
            //}
            //if (!(typeof returnValue === 'undefined')) {

            //    $("#" + targetObj).val(returnValue);

            //}
            var myiFrameId = "iframe";
            var Url = "../fb2sg/" + window_name + "?festivalType=" + festivalType + "&festivalPayCond=" + festivalPayCond + "&pridCD=" + pridCD + "&parentFuncId=" + parentFuncID;
            var dialogID = 'div_iframeID';
            var $dialog = $('<div id = "' + dialogID + '"></div>')
                        .html('<iframe style="border: 0px; " src="' + Url + '" id="' + myiFrameId + '" width="100%" height="100%"></iframe>')
                        .dialog({
                            autoOpen: false,
                            modal: true,
                            draggable: true,
                            resizable: false,
                            height: 400,
                            width: 520,
                            close: function (ev, ui) {
                                $("#" + dialogID).dialog("destroy");
                            }
                        });
            $('#' + dialogID).attr('stid', targetObj);

            $dialog.dialog('open');

        }
        function Multi_Select_SG(obj_cd, value) {
            var returnValue = value;
            if (returnValue == undefined) {
                returnValue = window.returnValue;
            }

            if (!(typeof returnValue === 'undefined')) {
                $("#" + obj_cd).val(returnValue);
            }
        }
        ////下載
        //function checkDowning(msg) {
        //    var processed = true;
        //    BlockUI();
        //    processed = confirm("確定要進行" + msg);
        //    if (!processed) {
        //        $.unblockUI();
        //    }
        //    return processed;
        //}
        function doBlock() {
           // BlockUI();
        }
        function doUnBlock() {
            $.unblockUI();
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
     <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="10%" />
                    <col width="10%" />
                    <col width="80%" />
                </colgroup>
                <tbody>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--節金類別--%>
                            <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_festival_type%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" colspan="2">
                            <asp:DropDownList ID="ddl_FESTIVAL_TYPE" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="Body_label" colspan="2">
                            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click" OnClientClick="return checkDowning('歷年節金條件資料下載');"  >歷年節金條件資料下載</asp:LinkButton>
                        </td>
                        <td align="right" colspan="2">
                            <aces:Btn ID="WFB2SG0100Search" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_search%>" OnClick="WFB2SG0100Search_Click" OnClientClick="BlockUI();" />

                            <%--<asp:Button ID="WFB2SG0100Search" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_search%>" OnClick="WFB2SG0100Search_Click" OnClientClick="BlockUI();" />--%>
                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2sg_btn_clear%>" onclick="ClearAll();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="Body_label" colspan="6">
                            <div id="init_grid">
                                <aces:Btn ID="WFB2SG0100Add" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_add%>" Visible="true" OnClick="WFB2SG0100Add_Click" />
                                <aces:Btn ID="WFB2SG0100Delete" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_delete%>" Visible="false" OnClick="WFB2SG0100Delete_Click" OnClientClick="return doDelete();" />
                                <aces:Btn ID="WFB2SG0100Edit" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_edit%>" Visible="false" OnClick="WFB2SG0100Edit_Click" OnClientClick="BlockUI();" />
                                <aces:Btn ID="WFB2SG0100OK" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_ok%>" Visible="false"  OnClick="WFB2SG0100OK_Click" OnClientClick="saveCheck();" />

                                <%--<asp:Button ID="WFB2SG0100Add" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_add%>" Visible="true" OnClick="WFB2SG0100Add_Click" />
                                <asp:Button ID="WFB2SG0100Delete" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_delete%>" Visible="false" OnClick="WFB2SG0100Delete_Click" OnClientClick="return doDelete();" />
                                <asp:Button ID="WFB2SG0100Edit" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_edit%>" Visible="false" OnClick="WFB2SG0100Edit_Click" OnClientClick="BlockUI();" />
                                <asp:Button ID="WFB2SG0100OK" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_ok%>" Visible="false"  OnClick="WFB2SG0100OK_Click" OnClientClick="saveCheck();" />--%>
                                <asp:Button ID="btn_cancel" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_cancel%>" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="btn_cancel_Click" />
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SG0100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="hid_qry_FESTIVAL_TYPE"
                        Name="festivalType" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
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
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_rownumber%>" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="60px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--節金類別--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_festival_type%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="FESTIVAL_TYPE">
                        <ItemTemplate>
                            <asp:Label ID="lb_FESTIVAL_TYPE" runat="server" Text='<%#Bind("FESTIVAL_TYPE_DESC")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_FESTIVAL_TYPE" runat="server" Text='<%#Bind("FESTIVAL_TYPE_DESC")%>' Width="100px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_NEW_FESTIVAL_TYPE" runat="server" CssClass="MandatoryField"></asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--節金給付條件--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_festival_pay_cond%>" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" SortExpression="FESTIVAL_PAY_COND">
                        <ItemTemplate>
                            <asp:Label ID="lb_FESTIVAL_PAY_COND" runat="server" Text='<%#Bind("FESTIVAL_PAY_COND")%>' Width="200px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_FESTIVAL_PAY_COND" Text='<%#Bind("FESTIVAL_PAY_COND")%>'  runat="server" ClientIDMode="Static" MaxLength="50" Width="200px" ></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_FESTIVAL_PAY_COND" runat="server" ClientIDMode="Static" Width="200px" MaxLength="50" ></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--節金給付金額--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_festival_amt%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="right" SortExpression="FESTIVAL_AMT">
                        <ItemTemplate>
                            <asp:Label ID="lb_FESTIVAL_AMT" runat="server" Text='<%#Bind("FESTIVAL_AMT","{0:n0}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_FESTIVAL_AMT" runat="server" Text='<%#Bind("FESTIVAL_AMT")%>' MaxLength="7" ClientIDMode="Static" Width="100px" CssClass="MandatoryField amt decimal"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="EDIT_FESTIVAL_AMT" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sg_required_festival_amt%>"
                                ControlToValidate="txt_EDIT_FESTIVAL_AMT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                             <!--需為數字-->
                                <asp:CustomValidator ID="Custom_EDIT_FESTIVAL_AMT" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sg_format_festival_amt%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_EDIT_FESTIVAL_AMT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_FESTIVAL_AMT" runat="server" ClientIDMode="Static" Width="100px" MaxLength="7" CssClass="MandatoryField amt decimal" Style="text-align: right;ime-mode:disabled"    onpaste="return false">></asp:TextBox>
                            <asp:RequiredFieldValidator ID="NEW_FESTIVAL_AMT" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sg_required_festival_amt%>"
                                ControlToValidate="txt_NEW_FESTIVAL_AMT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                              <!--需為數字-->
                                <asp:CustomValidator ID="Custom_NEW_FESTIVAL_AMT" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sg_format_festival_amt%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_FESTIVAL_AMT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--在職年資起--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_work_years_sdt%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="right" SortExpression="WORK_YEARS_SDT">
                        <ItemTemplate>
                            <asp:Label ID="lb_WORK_YEARS_SDT" runat="server" Text='<%#Bind("WORK_YEARS_SDT")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_WORK_YEARS_SDT" runat="server" Text='<%#Bind("WORK_YEARS_SDT")%>' Width="100px" MaxLength="5"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_WORK_YEARS_SDT" runat="server" ClientIDMode="Static" MaxLength="5" Width="100px" CssClass=" MandatoryField year decimal"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="DIT_WORK_YEARS_SDT" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sg_required_work_years_sdt%>"
                                ControlToValidate="txt_NEW_WORK_YEARS_SDT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                             <!--需為數字-->
                                <asp:CustomValidator ID="Custom_NEW_WORK_YEARS_SDT" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sg_format_work_years_sdt%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_WORK_YEARS_SDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--在職年資迄--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_work_years_edt%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="right" SortExpression="WORK_YEARS_EDT">
                        <ItemTemplate>
                            <asp:Label ID="lb_WORK_YEARS_EDT" runat="server" Text='<%#Bind("WORK_YEARS_EDT")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_WORK_YEARS_EDT" runat="server" Text='<%#Bind("WORK_YEARS_EDT")%>' MaxLength="5" ClientIDMode="Static" Width="100px" CssClass="year decimal"></asp:TextBox>
                            <!--需為數字-->
                                <asp:CustomValidator ID="Custom_EDIT_WORK_YEARS_EDT" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sg_format_work_years_edt%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_EDIT_WORK_YEARS_EDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_WORK_YEARS_EDT" runat="server" ClientIDMode="Static" Width="100px" MaxLength="5" CssClass="year decimal"></asp:TextBox>
                            <!--需為數字-->
                                <asp:CustomValidator ID="Custom_NEW_WORK_YEARS_EDT" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sg_format_work_years_edt%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_WORK_YEARS_EDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--員工區分--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_prid_cd%>" HeaderStyle-Width="260px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_PRID_CD" runat="server" Text='<%#Bind("PRID_CD")%>' Width="260px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_PRID_CD" runat="server" Text='<%#Bind("PRID_CD")%>' Width="260px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="text_NEW_PRID_CD" runat="server" Text='<%#Bind("PRID_CD")%>' Width="200px" CssClass=" MandatoryField"></asp:TextBox>
                            <input id="Button14" runat="server" type="button" value="..." />
                            <asp:RequiredFieldValidator ID="NEW_PRID_CD" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sg_required_prid_cd%>"
                                ControlToValidate="text_NEW_PRID_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
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
                                <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_rownumber%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_festival_type%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_festival_pay_cond%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_festival_amt%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_work_years_sdt%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_work_years_edt%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_prid_cd%>" Width="260px"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td></td>
                            <td></td>
                            <td>
                                <asp:DropDownList ID="ddl_NEW_FESTIVAL_TYPE" runat="server" CssClass="MandatoryField"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_FESTIVAL_PAY_COND" runat="server" ClientIDMode="Static" Width="100px" MaxLength="50"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_FESTIVAL_AMT" runat="server" ClientIDMode="Static" Width="100px" MaxLength="9" CssClass="MandatoryField amt decimal"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="NEW_FESTIVAL_AMT" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sg_required_festival_amt%>"
                                    ControlToValidate="txt_NEW_FESTIVAL_AMT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                  <!--需為數字-->
                                <asp:CustomValidator ID="Custom_NEW_FESTIVAL_AMT2" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sg_format_festival_amt%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_FESTIVAL_AMT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_WORK_YEARS_SDT" runat="server" ClientIDMode="Static" Width="100px" CssClass=" MandatoryField year decimal"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="DIT_WORK_YEARS_SDT" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sg_required_work_years_sdt%>"
                                    ControlToValidate="txt_NEW_WORK_YEARS_SDT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <!--需為數字-->
                                <asp:CustomValidator ID="Custom_NEW_WORK_YEARS_SDT2" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sg_format_work_years_sdt%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_WORK_YEARS_SDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_WORK_YEARS_EDT" runat="server" ClientIDMode="Static" Width="100px" CssClass="year decimal"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="text_NEW_PRID_CD" runat="server" Text='<%#Bind("PRID_CD")%>' Width="200px" CssClass=" MandatoryField"></asp:TextBox>
                                <input id="Button14" runat="server" type="button" value="..." />
                                <asp:RequiredFieldValidator ID="NEW_PRID_CD" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sg_required_prid_cd%>"
                                    ControlToValidate="text_NEW_PRID_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                 <!--需為數字-->
                                <asp:CustomValidator ID="Custom_NEW_WORK_YEARS_EDT2" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sg_format_work_years_edt%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_WORK_YEARS_EDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
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
            <!-- 查詢條件 -->
            <asp:HiddenField ID="hid_qry_FESTIVAL_TYPE" runat="server" ClientIDMode="Static" /> 


            <asp:HiddenField ID="HiD_PridCD_OLD" runat="server" ClientIDMode="Static" />
            <!-- 每頁的筆數 -->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <!-- 是否要凍結視窗 -->
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="N" />
            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
        <%--EXCEL下載用 --%>
        <Triggers>
            <asp:PostBackTrigger ControlID="LinkButton1" />
        </Triggers>

    </asp:UpdatePanel>
</asp:Content>
