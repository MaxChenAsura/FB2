<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dc/action/WFB2DC0100_Qry.aspx.cs" Inherits="WebContent_WFB2DC0100_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {

            $("#txt_CLOCK_DESC2").attr("readonly", true);

            gridviewScroll();
            $.unblockUI();
        }

        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }

        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"
            });
        }

        //清空畫面
        function ClearAll() {
            $("#ddl_CLOCK_TYPE2").val("-1");
            $("#ddl_PLANT_CD2").val("-1");
            $("#txt_CLOCK_NO").val("");
            $("#txt_CLOCK_DESC2").val("");
        }

        function getCLOCK() {
            var clock_no = $("#txt_CLOCK_NO").val();
            var json = OpenSearch('Clock_Search.aspx', 'txt_CLOCK_NO', 'txt_CLOCK_DESC2', 'CLOCK_NO=' + clock_no);
            //if (json != undefined)
            //    alert("CLOCK_NO=" + json.CD + "&CLOCK_DESC=" + json.DESC);
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

        function getCLOCK_DESC(CLOCK_id) {
            var txt = document.getElementById(CLOCK_id);
            if (txt.value != "") {
                document.getElementById('hid_getCLOCK_DESC').click();
                return false;
            }
            else {
                $("#txt_CLOCK_DESC2").val("");
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
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
                                        <asp:Label ID="lb_CLOCK_TYPE" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CLOCK_TYPE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_CLOCK_TYPE2" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PLANT_CD" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_PLANT_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_PLANT_CD2" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_CLOCK_NO" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CLOCK_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_CLOCK_NO" runat="server" MaxLength="3" Width="64px" ClientIDMode="Static" onblur="javascript:getCLOCK_DESC(this.id)"></asp:TextBox>
                                        <input id="btn_CLOCK_NO" type="button" value="..." onclick="getCLOCK();" />
                                        <asp:TextBox ID="txt_CLOCK_DESC2" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2DC0100Search" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0100Search%>" OnClientClick="BlockUI();" OnClick="WFB2DC0100Search_Click" />
                                            
                                            <%--<asp:Button ID="WFB2DC0100Search" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0100Search%>" OnClientClick="BlockUI();" OnClick="WFB2DC0100Search_Click" />--%>
                                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2dc_btn_clear%>" onclick="ClearAll();" />
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
                        <aces:Btn ID="WFB2DC0100Add" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0200Add%>" OnClick="WFB2DC0100Add_Click" />
                            <aces:Btn ID="WFB2DC0100Delete" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0200Delete%>" OnClientClick="return CheckDelAction();" Visible="false" OnClick="WFB2DC0100Delete_Click" />
                            <aces:Btn ID="WFB2DC0100Edit" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0200Edit%>" Visible="false" OnClick="WFB2DC0100Edit_Click" />
                            <aces:Btn ID="WFB2DC0100Save" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0200Save%>" Visible="false" ValidationGroup="GroupA" OnClick="WFB2DC0100Save_Click" />

<%--                            <asp:Button ID="WFB2DC0100Add" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0200Add%>" OnClick="WFB2DC0100Add_Click" />
                            <asp:Button ID="WFB2DC0100Delete" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0200Delete%>" OnClientClick="return CheckDelAction();" Visible="false" OnClick="WFB2DC0100Delete_Click" />
                            <asp:Button ID="WFB2DC0100Edit" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0200Edit%>" Visible="false" OnClick="WFB2DC0100Edit_Click" />
                            <asp:Button ID="WFB2DC0100Save" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0200Save%>" Visible="false" ValidationGroup="GroupA" OnClick="WFB2DC0100Save_Click" />--%>
                            
                            <asp:Button ID="WFB2DC0100Cancel" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0200Cancel%>" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="WFB2DC0100Cancel_Click" />
                        </div>
                    </td>
                </tr>
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DC0100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="ddl_CLOCK_TYPE2" DefaultValue=""
                        Name="clock_type" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="ddl_PLANT_CD2" DefaultValue=""
                        Name="plant_cd" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="txt_CLOCK_NO"
                        Name="clock_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" Width="1020px"
                OnSorting="gv_result_Sorting" OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated"
                OnDataBound="gv_result_DataBound"
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
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_RowNumber%>" HeaderStyle-Width="40px">
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
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_CLOCK_NO%>" SortExpression="CLOCK_NO" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_CLOCK_NO" runat="server" Text='<%#Bind("CLOCK_NO")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lb_CLOCK_NO" runat="server" Text='<%#Bind("CLOCK_NO")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:TextBox ID="txt_NEW_CLOCK_NO" runat="server" ClientIDMode="Static" Width="80px" CssClass="MandatoryField" MaxLength="3"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CLOCK_NO%>"
                                ControlToValidate="txt_NEW_CLOCK_NO" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CLOCK_NO_Chinese%>" ControlToValidate="txt_NEW_CLOCK_NO" ForeColor="Red"
                                ValidationExpression="^[0-9a-zA-Z_]+$" Display="None" ValidationGroup="GroupA">
                            </asp:RegularExpressionValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_CLOCK_DESC%>" SortExpression="CLOCK_DESC" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_CLOCK_DESC" runat="server" Text='<%#Bind("CLOCK_DESC")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_CLOCK_DESC" runat="server" Text='<%#Bind("CLOCK_DESC")%>' ClientIDMode="Static" Width="100%" CssClass="MandatoryField" MaxLength="60"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CLOCK_DESC%>"
                                ControlToValidate="txt_CLOCK_DESC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_NEW_CLOCK_DESC" runat="server" ClientIDMode="Static" Width="100%" CssClass="MandatoryField" MaxLength="60"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CLOCK_DESC%>"
                                ControlToValidate="txt_NEW_CLOCK_DESC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_CLOCK_TYPE%>" SortExpression="CLOCK_TYPE" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_CLOCK_TYPE" runat="server" Text='<%#Bind("CLOCK_TYPE")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_CLOCK_TYPE" runat="server" ClientIDMode="Static" Width="80px" CssClass="MandatoryField" OnSelectedIndexChanged="ddl_CLOCK_TYPE_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            <asp:HiddenField ID="hid_CLOCK_TYPE" runat="server" Value='<%#Bind("CLOCK_TYPE")%>' />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CLOCK_TYPE%>"
                                ControlToValidate="ddl_CLOCK_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:DropDownList ID="ddl_NEW_CLOCK_TYPE" runat="server" ClientIDMode="Static" Width="80px" CssClass="MandatoryField" OnSelectedIndexChanged="ddl_NEW_CLOCK_TYPE_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CLOCK_TYPE%>"
                                ControlToValidate="ddl_NEW_CLOCK_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_IS_VALID%>" SortExpression="IS_VALID" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_IS_VALID" runat="server" Text='<%#Bind("IS_VALID")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_IS_VALID" runat="server" ClientIDMode="Static" Width="60px" CssClass="MandatoryField"></asp:DropDownList>
                            <asp:HiddenField ID="hid_IS_VALID" runat="server" Value='<%#Bind("IS_VALID")%>' />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_IS_VALID%>"
                                ControlToValidate="ddl_IS_VALID" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:DropDownList ID="ddl_NEW_IS_VALID" runat="server" ClientIDMode="Static" Width="60px" CssClass="MandatoryField"></asp:DropDownList>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_IS_VALID%>"
                                ControlToValidate="ddl_NEW_IS_VALID" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%-- 用途區分 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_CLOCK_USED_CD%>" SortExpression="CLOCK_USED_CD" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_CLOCK_USED_CD" runat="server" Text='<%#Bind("CLOCK_USED_CD")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_CLOCK_USED_CD" runat="server" ClientIDMode="Static" Width="120px"></asp:DropDownList>
                            <asp:HiddenField ID="hid_CLOCK_USED_CD" runat="server" Value='<%#Bind("CLOCK_USED_CD")%>' />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:DropDownList ID="ddl_NEW_CLOCK_USED_CD" runat="server" ClientIDMode="Static" Width="120px"></asp:DropDownList>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_PLANT_CD%>" SortExpression="PLANT_CD" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_PLANT_CD" runat="server" Text='<%#Bind("PLANT_CD")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_PLANT_CD" runat="server" ClientIDMode="Static" Width="80px" CssClass="MandatoryField"></asp:DropDownList>
                            <asp:HiddenField ID="hid_PLANT_CD" runat="server" Value='<%#Bind("PLANT_CD")%>' />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_PLANT_CD%>"
                                ControlToValidate="ddl_PLANT_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:DropDownList ID="ddl_NEW_PLANT_CD" runat="server" ClientIDMode="Static" Width="80px" CssClass="MandatoryField"></asp:DropDownList>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_PLANT_CD%>"
                                ControlToValidate="ddl_NEW_PLANT_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_PARK_DEFAULT%>" SortExpression="PARK_DEFAULT" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_PARK_DEFAULT" runat="server" Text='<%#Bind("PARK_DEFAULT")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:DropDownList ID="ddl_PARK_DEFAULT" runat="server" ClientIDMode="Static" Width="100px"></asp:DropDownList>
                            </div>
                            <asp:HiddenField ID="hid_PARK_DEFAULT" runat="server" Value='<%#Bind("PARK_DEFAULT")%>' />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:DropDownList ID="ddl_NEW_PARK_DEFAULT" runat="server" ClientIDMode="Static" Width="100px"></asp:DropDownList>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_CLOCK_IP%>" SortExpression="CLOCK_IP" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_CLOCK_IP" runat="server" Text='<%#Bind("CLOCK_IP")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_CLOCK_IP" runat="server" Text='<%#Bind("CLOCK_IP")%>' ClientIDMode="Static" Width="100%" CssClass="MandatoryField" MaxLength="30"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CLOCK_IP%>"
                                ControlToValidate="txt_CLOCK_IP" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_NEW_CLOCK_IP" runat="server" ClientIDMode="Static" Width="100%" CssClass="MandatoryField" MaxLength="30"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CLOCK_IP%>"
                                ControlToValidate="txt_NEW_CLOCK_IP" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
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
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2dc_RowNumber%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CLOCK_NO%>" Width="80px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CLOCK_DESC%>" Width="150px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CLOCK_TYPE%>" Width="80px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_IS_VALID%>" Width="60px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CLOCK_USED_CD%>" Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_PLANT_CD%>" Width="80px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_PARK_DEFAULT%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CLOCK_IP%>" Width="100px"></asp:Label>
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
                                <%-- 卡鐘編號 --%>
                                <div style="text-align: center; width: 100%">
                                    <asp:TextBox ID="txt_NEW_CLOCK_NO" runat="server" ClientIDMode="Static" Width="80px" CssClass="MandatoryField" MaxLength="3"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CLOCK_NO%>"
                                    ControlToValidate="txt_NEW_CLOCK_NO" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CLOCK_NO_Chinese%>" ControlToValidate="txt_NEW_CLOCK_NO" ForeColor="Red"
                                ValidationExpression="^[0-9a-zA-Z_]+$" Display="None" ValidationGroup="GroupA">
                            </asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <%-- 卡鐘位置 --%>
                                <div style="text-align: left; width: 100%">
                                    <asp:TextBox ID="txt_NEW_CLOCK_DESC" runat="server" ClientIDMode="Static" Width="100%" CssClass="MandatoryField" MaxLength="60"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CLOCK_DESC%>"
                                    ControlToValidate="txt_NEW_CLOCK_DESC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                 <%-- 卡鐘類別 --%>
                                <div style="text-align: left; width: 100%">
                                    <asp:DropDownList ID="ddl_NEW_CLOCK_TYPE" runat="server" ClientIDMode="Static" Width="80px" CssClass="MandatoryField" OnSelectedIndexChanged="ddl_NEW_CLOCK_TYPE_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CLOCK_TYPE%>"
                                    ControlToValidate="ddl_NEW_CLOCK_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                 <%-- 使用中 --%>
                                <div style="text-align: left; width: 100%">
                                    <asp:DropDownList ID="ddl_NEW_IS_VALID" runat="server" ClientIDMode="Static" Width="60px" CssClass="MandatoryField"></asp:DropDownList>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_IS_VALID%>"
                                    ControlToValidate="ddl_NEW_IS_VALID" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                  <%-- 用途區分 --%>
                                <div style="text-align: left; width: 100%">
                                    <asp:DropDownList ID="ddl_NEW_CLOCK_USED_CD" runat="server" ClientIDMode="Static" Width="120px"></asp:DropDownList>
                                </div>
                            </td>
                            <td>
                                 <%-- 工廠區分 --%>
                                <div style="text-align: left; width: 100%">
                                    <asp:DropDownList ID="ddl_NEW_PLANT_CD" runat="server" ClientIDMode="Static" Width="80px" CssClass="MandatoryField"></asp:DropDownList>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_PLANT_CD%>"
                                    ControlToValidate="ddl_NEW_PLANT_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <%-- 全員使用 --%>
                                <div style="text-align: center; width: 100%">
                                    <asp:DropDownList ID="ddl_NEW_PARK_DEFAULT" runat="server" ClientIDMode="Static" Width="100px"></asp:DropDownList>
                                </div>
                            </td>
                            <td>
                                  <%-- 卡鐘IP --%>
                                <div style="text-align: left; width: 100%">
                                    <asp:TextBox ID="txt_NEW_CLOCK_IP" runat="server" ClientIDMode="Static" Width="100%" CssClass="MandatoryField" MaxLength="30"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CLOCK_IP%>"
                                    ControlToValidate="txt_NEW_CLOCK_IP" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
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
        <asp:Button ID="hid_getCLOCK_DESC" runat="server" Style="display: none" ClientIDMode="Static" OnClick="hid_getCLOCK_DESC_Click"/>
        </ContentTemplate>

    </asp:UpdatePanel>

</asp:Content>

