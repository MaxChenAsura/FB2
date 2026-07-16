<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dh/WFB2DH0200_Qry.aspx.cs" Inherits="WebContent_fb2dh_WFB2DH0200_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".number").mask('999.99');
            $(".number").css("text-align", "right");

            gridviewScroll();
            $.unblockUI();
        }

        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }

        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1040",
                height: "500",
                barcolor: "#7F7F7F",
                headerrowcount: 2
            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
        }

        //清空畫面
        function ClearAll() {
            $("#ddl_UNION_PJOB_CD").val("-1");
        }

        function getUNION_PJOB() {
            var union_pjob_cd = $("#txt_UNION_PJOB_CD").val();
            var json = OpenSearch('UNION_PJOB_Search.aspx', 'txt_UNION_PJOB_CD', 'txt_UNION_PJOB_DESC2', 'UNION_PJOB_CD=' + union_pjob_cd);
            //if (json != undefined)
            //    alert("工會職務:" + json.CD + "\n職務說明:" + json.DESC);
        }

        function LookUpCheckboxs() {
            var ItemCheckBoxs = $(":checkbox[id$=cb_check]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }

        function CheckDelAction() {
            if (LookUpCheckboxs() > 0)
                return confirm("確定要刪除?");
            else {
                alert("請選取資料!");
                return false;
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="10%" />
                                <col width="30%" />
                                <col width="10%" />
                                <col width="30%" />
                                <col width="20%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_UNION_PJOB_CD" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_UNION_PJOB_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_UNION_PJOB_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2DH0200Search" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0200Search%>" OnClientClick="BlockUI();" OnClick="WFB2DH0200Search_Click" />

                                            <%--<asp:Button ID="WFB2DH0200Search" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0200Search%>" OnClientClick="BlockUI();" OnClick="WFB2DH0200Search_Click" />--%>

                                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2dh_btn_clear%>" onclick="ClearAll();" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td align="right" class="Body_label">
                        <hr>
                        <div id="init_grid">
                            <aces:Btn ID="WFB2DH0200Add" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0200Add%>" OnClick="WFB2DH0200Add_Click" />
                            <aces:Btn ID="WFB2DH0200Delete" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0200Delete%>" OnClientClick="return CheckDelAction();" Visible="false" OnClick="WFB2DH0200Delete_Click" />
                            <aces:Btn ID="WFB2DH0200Edit" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0200Edit%>" Visible="false" OnClick="WFB2DH0200Edit_Click" />
                            <aces:Btn ID="WFB2DH0200Save" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0200Save%>" Visible="false" ValidationGroup="GroupA" OnClick="WFB2DH0200Save_Click" />

                            <%--<asp:Button ID="WFB2DH0200Add" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0200Add%>" OnClick="WFB2DH0200Add_Click" />
                            <asp:Button ID="WFB2DH0200Delete" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0200Delete%>" OnClientClick="return CheckDelAction();" Visible="false" OnClick="WFB2DH0200Delete_Click" />
                            <asp:Button ID="WFB2DH0200Edit" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0200Edit%>" Visible="false" OnClick="WFB2DH0200Edit_Click" />
                            <asp:Button ID="WFB2DH0200Save" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0200Save%>" Visible="false" ValidationGroup="GroupA" OnClick="WFB2DH0200Save_Click" />--%>

                            <asp:Button ID="btn_Cancel" runat="server" Text="<%$Resources:Resource,wfb2dh_btn_Cancel%>" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="btn_Cancel_Click" />
                        </div>
                    </td>
                </tr>

            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DH0200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="ddl_UNION_PJOB_CD" DefaultValue=""
                        Name="union_pjob_cd" PropertyName="SelectedValue" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" Width="1020px"
                OnRowCreated="gv_result_RowCreated" OnDataBound="gv_result_DataBound"
                OnPageIndexChanging="gv_result_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_RowNumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_UNION_PJOB_CD%>" SortExpression="UNION_PJOB_CD" HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <asp:Label ID="lb_UNION_PJOB_CD" runat="server" Text='<%#Bind("UNION_PJOB_CD")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lb_UNION_PJOB_CD" runat="server" Text='<%#Bind("UNION_PJOB_CD")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:TextBox ID="txt_NEW_UNION_PJOB_CD" runat="server" ClientIDMode="Static" MaxLength="1" CssClass="MandatoryField" Width="80px"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_UNION_PJOB_CD%>"
                                ControlToValidate="txt_NEW_UNION_PJOB_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_UNION_PJOB_CD_Chinese%>" ControlToValidate="txt_NEW_UNION_PJOB_CD" ForeColor="Red"
                                ValidationExpression="^[0-9a-zA-Z_]+$" Display="None" ValidationGroup="GroupA">
                            </asp:RegularExpressionValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_UNION_PJOB_DESC%>" SortExpression="UNION_PJOB_DESC" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_UNION_PJOB_DESC" runat="server" Text='<%#Bind("UNION_PJOB_DESC")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_UNION_PJOB_DESC" runat="server" Text='<%#Bind("UNION_PJOB_DESC")%>' MaxLength="30" ClientIDMode="Static" CssClass="MandatoryField" Width="80px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_UNION_PJOB_DESC%>"
                                ControlToValidate="txt_UNION_PJOB_DESC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_NEW_UNION_PJOB_DESC" runat="server" ClientIDMode="Static" MaxLength="30" CssClass="MandatoryField" Width="80px"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_UNION_PJOB_DESC%>"
                                ControlToValidate="txt_NEW_UNION_PJOB_DESC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_LEAVE_MAX_HOUR_01%>" SortExpression="LEAVE_MAX_HOUR_01" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEAVE_MAX_HOUR_01" runat="server" Text='<%#Bind("LEAVE_MAX_HOUR_01")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_LEAVE_MAX_HOUR_01" runat="server" Text='<%#Bind("LEAVE_MAX_HOUR_01")%>' ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_01%>"
                                ControlToValidate="txt_LEAVE_MAX_HOUR_01" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidatorMAX_HOUR_01" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_01_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_LEAVE_MAX_HOUR_01" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_NEW_LEAVE_MAX_HOUR_01" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_01%>"
                                ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_01" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidatorMAX_HOUR_01" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_01_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_01" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_LEAVE_MAX_HOUR_02%>" SortExpression="LEAVE_MAX_HOUR_02" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEAVE_MAX_HOUR_02" runat="server" Text='<%#Bind("LEAVE_MAX_HOUR_02")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_LEAVE_MAX_HOUR_02" runat="server" Text='<%#Bind("LEAVE_MAX_HOUR_02")%>' ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_02%>"
                                ControlToValidate="txt_LEAVE_MAX_HOUR_02" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidatorMAX_HOUR_02" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_02_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_LEAVE_MAX_HOUR_02" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_NEW_LEAVE_MAX_HOUR_02" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_02%>"
                                ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_02" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidatorMAX_HOUR_02" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_02_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_02" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_LEAVE_MAX_HOUR_03%>" SortExpression="LEAVE_MAX_HOUR_03" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEAVE_MAX_HOUR_03" runat="server" Text='<%#Bind("LEAVE_MAX_HOUR_03")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_LEAVE_MAX_HOUR_03" runat="server" Text='<%#Bind("LEAVE_MAX_HOUR_03")%>' ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_03%>"
                                ControlToValidate="txt_LEAVE_MAX_HOUR_03" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidatorMAX_HOUR_03" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_03_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_LEAVE_MAX_HOUR_03" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_NEW_LEAVE_MAX_HOUR_03" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_03%>"
                                ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_03" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidatorMAX_HOUR_03" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_03_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_03" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_LEAVE_MAX_HOUR_04%>" SortExpression="LEAVE_MAX_HOUR_04" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEAVE_MAX_HOUR_04" runat="server" Text='<%#Bind("LEAVE_MAX_HOUR_04")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_LEAVE_MAX_HOUR_04" runat="server" Text='<%#Bind("LEAVE_MAX_HOUR_04")%>' ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_04%>"
                                ControlToValidate="txt_LEAVE_MAX_HOUR_04" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidatorMAX_HOUR_04" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_04_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_LEAVE_MAX_HOUR_04" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_NEW_LEAVE_MAX_HOUR_04" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_04%>"
                                ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_04" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidatorMAX_HOUR_04" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_04_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_04" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_LEAVE_MAX_HOUR_05%>" SortExpression="LEAVE_MAX_HOUR_05" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEAVE_MAX_HOUR_05" runat="server" Text='<%#Bind("LEAVE_MAX_HOUR_05")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_LEAVE_MAX_HOUR_05" runat="server" Text='<%#Bind("LEAVE_MAX_HOUR_05")%>' ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_05%>"
                                ControlToValidate="txt_LEAVE_MAX_HOUR_05" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidatorMAX_HOUR_05" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_05_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_LEAVE_MAX_HOUR_05" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_NEW_LEAVE_MAX_HOUR_05" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_05%>"
                                ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_05" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidatorMAX_HOUR_05" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_05_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_05" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_LEAVE_MAX_HOUR_06%>" SortExpression="LEAVE_MAX_HOUR_06" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEAVE_MAX_HOUR_06" runat="server" Text='<%#Bind("LEAVE_MAX_HOUR_06")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_LEAVE_MAX_HOUR_06" runat="server" Text='<%#Bind("LEAVE_MAX_HOUR_06")%>' ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_06%>"
                                ControlToValidate="txt_LEAVE_MAX_HOUR_06" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidatorMAX_HOUR_06" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_06_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_LEAVE_MAX_HOUR_06" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_NEW_LEAVE_MAX_HOUR_06" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_06%>"
                                ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_06" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidatorMAX_HOUR_06" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_06_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_06" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_LEAVE_MAX_HOUR_07%>" SortExpression="LEAVE_MAX_HOUR_07" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEAVE_MAX_HOUR_07" runat="server" Text='<%#Bind("LEAVE_MAX_HOUR_07")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_LEAVE_MAX_HOUR_07" runat="server" Text='<%#Bind("LEAVE_MAX_HOUR_07")%>' ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_07%>"
                                ControlToValidate="txt_LEAVE_MAX_HOUR_07" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidatorMAX_HOUR_07" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_07_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_LEAVE_MAX_HOUR_07" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_NEW_LEAVE_MAX_HOUR_07" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_07%>"
                                ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_07" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidatorMAX_HOUR_07" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_07_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_07" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_LEAVE_MAX_HOUR_08%>" SortExpression="LEAVE_MAX_HOUR_08" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEAVE_MAX_HOUR_08" runat="server" Text='<%#Bind("LEAVE_MAX_HOUR_08")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_LEAVE_MAX_HOUR_08" runat="server" Text='<%#Bind("LEAVE_MAX_HOUR_08")%>' ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_08%>"
                                ControlToValidate="txt_LEAVE_MAX_HOUR_08" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidatorMAX_HOUR_08" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_08_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_LEAVE_MAX_HOUR_08" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_NEW_LEAVE_MAX_HOUR_08" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_08%>"
                                ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_08" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidatorMAX_HOUR_08" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_08_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_08" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_LEAVE_MAX_HOUR_09%>" SortExpression="LEAVE_MAX_HOUR_09" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEAVE_MAX_HOUR_09" runat="server" Text='<%#Bind("LEAVE_MAX_HOUR_09")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_LEAVE_MAX_HOUR_09" runat="server" Text='<%#Bind("LEAVE_MAX_HOUR_09")%>' ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_09%>"
                                ControlToValidate="txt_LEAVE_MAX_HOUR_09" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidatorMAX_HOUR_09" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_09_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_LEAVE_MAX_HOUR_09" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_NEW_LEAVE_MAX_HOUR_09" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_09%>"
                                ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_09" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidatorMAX_HOUR_09" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_09_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_09" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_LEAVE_MAX_HOUR_10%>" SortExpression="LEAVE_MAX_HOUR_10" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEAVE_MAX_HOUR_10" runat="server" Text='<%#Bind("LEAVE_MAX_HOUR_10")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_LEAVE_MAX_HOUR_10" runat="server" Text='<%#Bind("LEAVE_MAX_HOUR_10")%>' ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_10%>"
                                ControlToValidate="txt_LEAVE_MAX_HOUR_10" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidatorMAX_HOUR_10" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_10_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_LEAVE_MAX_HOUR_10" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_NEW_LEAVE_MAX_HOUR_10" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_10%>"
                                ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_10" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidatorMAX_HOUR_10" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_10_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_10" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_LEAVE_MAX_HOUR_11%>" SortExpression="LEAVE_MAX_HOUR_11" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEAVE_MAX_HOUR_11" runat="server" Text='<%#Bind("LEAVE_MAX_HOUR_11")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_LEAVE_MAX_HOUR_11" runat="server" Text='<%#Bind("LEAVE_MAX_HOUR_11")%>' ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_11%>"
                                ControlToValidate="txt_LEAVE_MAX_HOUR_11" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidatorMAX_HOUR_11" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_11_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_LEAVE_MAX_HOUR_11" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_NEW_LEAVE_MAX_HOUR_11" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_11%>"
                                ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_11" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidatorMAX_HOUR_11" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_11_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_11" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_LEAVE_MAX_HOUR_12%>" SortExpression="LEAVE_MAX_HOUR_12" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEAVE_MAX_HOUR_12" runat="server" Text='<%#Bind("LEAVE_MAX_HOUR_12")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_LEAVE_MAX_HOUR_12" runat="server" Text='<%#Bind("LEAVE_MAX_HOUR_12")%>' ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_12%>"
                                ControlToValidate="txt_LEAVE_MAX_HOUR_12" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidatorMAX_HOUR_12" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_12_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_LEAVE_MAX_HOUR_12" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_NEW_LEAVE_MAX_HOUR_12" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_12%>"
                                ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_12" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidatorMAX_HOUR_12" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_12_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_12" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <table class="grid-view" width="1020px">
                        <tr class="header">
                            <td>
                                <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" Width="20px" />
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2dh_RowNumber%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_UNION_PJOB_CD%>" Width="80px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_UNION_PJOB_DESC%>" Width="80px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_LEAVE_MAX_HOUR_01%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_LEAVE_MAX_HOUR_02%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_LEAVE_MAX_HOUR_03%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_LEAVE_MAX_HOUR_04%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_LEAVE_MAX_HOUR_05%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_LEAVE_MAX_HOUR_06%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label10" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_LEAVE_MAX_HOUR_07%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_LEAVE_MAX_HOUR_08%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label12" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_LEAVE_MAX_HOUR_09%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label13" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_LEAVE_MAX_HOUR_10%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label14" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_LEAVE_MAX_HOUR_11%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label15" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_LEAVE_MAX_HOUR_12%>" Width="40px"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td>
                                <asp:CheckBox ID="cb_check" runat="server" Width="20px" />
                            </td>
                            <td>
                                <asp:Label ID="lb_NewRowNumber" runat="server" Text="1" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:TextBox ID="txt_NEW_UNION_PJOB_CD" runat="server" ClientIDMode="Static" MaxLength="1" CssClass="MandatoryField" Width="80px"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_UNION_PJOB_CD%>"
                                    ControlToValidate="txt_NEW_UNION_PJOB_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_UNION_PJOB_CD_Chinese%>" ControlToValidate="txt_NEW_UNION_PJOB_CD" ForeColor="Red"
                                    ValidationExpression="^[0-9a-zA-Z_]+$" Display="None" ValidationGroup="GroupA">
                                </asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <div style="text-align: left; width: 100%">
                                    <asp:TextBox ID="txt_NEW_UNION_PJOB_DESC" runat="server" ClientIDMode="Static" MaxLength="30" CssClass="MandatoryField" Width="80px"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_UNION_PJOB_DESC%>"
                                    ControlToValidate="txt_NEW_UNION_PJOB_DESC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <div style="text-align: right; width: 100%">
                                    <asp:TextBox ID="txt_NEW_LEAVE_MAX_HOUR_01" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_01%>"
                                    ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_01" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidatorMAX_HOUR_01" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_01_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_01" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
                            </td>
                            <td>
                                <div style="text-align: right; width: 100%">
                                    <asp:TextBox ID="txt_NEW_LEAVE_MAX_HOUR_02" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_02%>"
                                    ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_02" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidatorMAX_HOUR_02" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_02_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_02" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
                            </td>
                            <td>
                                <div style="text-align: right; width: 100%">
                                    <asp:TextBox ID="txt_NEW_LEAVE_MAX_HOUR_03" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_03%>"
                                    ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_03" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidatorMAX_HOUR_03" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_03_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_03" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
                            </td>
                            <td>
                                <div style="text-align: right; width: 100%">
                                    <asp:TextBox ID="txt_NEW_LEAVE_MAX_HOUR_04" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_04%>"
                                    ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_04" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidatorMAX_HOUR_04" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_04_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_04" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
                            </td>
                            <td>
                                <div style="text-align: right; width: 100%">
                                    <asp:TextBox ID="txt_NEW_LEAVE_MAX_HOUR_05" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_05%>"
                                    ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_05" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidatorMAX_HOUR_05" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_05_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_05" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
                            </td>
                            <td>
                                <div style="text-align: right; width: 100%">
                                    <asp:TextBox ID="txt_NEW_LEAVE_MAX_HOUR_06" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_06%>"
                                    ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_06" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidatorMAX_HOUR_06" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_06_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_06" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
                            </td>
                            <td>
                                <div style="text-align: right; width: 100%">
                                    <asp:TextBox ID="txt_NEW_LEAVE_MAX_HOUR_07" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_07%>"
                                    ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_07" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidatorMAX_HOUR_07" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_07_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_07" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
                            </td>
                            <td>
                                <div style="text-align: right; width: 100%">
                                    <asp:TextBox ID="txt_NEW_LEAVE_MAX_HOUR_08" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_08%>"
                                    ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_08" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidatorMAX_HOUR_08" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_08_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_08" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
                            </td>
                            <td>
                                <div style="text-align: right; width: 100%">
                                    <asp:TextBox ID="txt_NEW_LEAVE_MAX_HOUR_09" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_09%>"
                                    ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_09" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidatorMAX_HOUR_09" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_09_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_09" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
                            </td>
                            <td>
                                <div style="text-align: right; width: 100%">
                                    <asp:TextBox ID="txt_NEW_LEAVE_MAX_HOUR_10" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_10%>"
                                    ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_10" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidatorMAX_HOUR_10" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_10_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_10" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
                            </td>
                            <td>
                                <div style="text-align: right; width: 100%">
                                    <asp:TextBox ID="txt_NEW_LEAVE_MAX_HOUR_11" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_11%>"
                                    ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_11" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidatorMAX_HOUR_11" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_11_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_11" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
                            </td>
                            <td>
                                <div style="text-align: right; width: 100%">
                                    <asp:TextBox ID="txt_NEW_LEAVE_MAX_HOUR_12" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Width="40px"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_12%>"
                                    ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_12" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidatorMAX_HOUR_12" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_HOUR_12_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_LEAVE_MAX_HOUR_12" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
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
                        <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static" AutoPostBack="true">
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_10_Rows%>" Value="10"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_20_Rows%>" Value="20"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_30_Rows%>" Value="30"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_40_Rows%>" Value="40"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_50_Rows%>" Value="50"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 5px"></td>
                    <td style="font-size: 14px;">
                        <asp:Label ID="lb_TotalCount" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>

