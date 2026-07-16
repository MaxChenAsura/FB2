<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sj/WFB2SJ0150_Qry.aspx.cs" Inherits="WebContent_WFB2SJ0150_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".number").mask('999');
            $(".number2").mask('99');
            $(".year").mask('9999');
            $(".numberr").css("text-align", "right");
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

        function CheckTARGET_TYPE(TARGET_TYPE_id) {
            var txt = document.getElementById(TARGET_TYPE_id);
            if (txt.value == "-1") {
                //alert("團保對象別不可空白");
                document.getElementById(TARGET_TYPE_id).value = "1";
            }

            $("#txt_NEW_PERSON_QTY_S").removeClass("ro").removeAttr("disabled");
            $("#txt_NEW_PERSON_QTY_E").removeClass("ro").removeAttr("disabled");
            $("#cb_NEW_HOUSE_YN").removeClass("ro").removeAttr("disabled");

            if (txt.value != 3) {
                $("#txt_NEW_PERSON_QTY_S").val("1");
                $("#txt_NEW_PERSON_QTY_S").attr("disabled", true);
                $("#cb_NEW_HOUSE_YN").attr("checked", false);
                $("#cb_NEW_HOUSE_YN").attr("disabled", true);
            }

            if (txt.value == 1 || txt.value == 2) {
                $("#txt_NEW_PERSON_QTY_E").val("1");
                $("#txt_NEW_PERSON_QTY_E").attr("disabled", true);
            }
            else if (txt.value == 4) {
                $("#txt_NEW_PERSON_QTY_E").val("2");
                $("#txt_NEW_PERSON_QTY_E").attr("disabled", true);
            }
        }

        function IsDelete() {
            var answer = confirm("確定要刪除?");
            if (answer)
                return true;
            else {
                document.getElementById('HID_cancel').click();
                return false;
            }
        }

        //清空畫面
        function ClearAll() {
            $("#txt_ASSESS_YEAR").val("");
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
        //查詢前檢核
        //查詢前檢核
        function CheckSearch() {
            var processed = true;
            BlockUI();
            return processed;
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
                                        <asp:Label ID="lb_ASSESS_YEAR" runat="server" Text="年度"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_ASSESS_YEAR" runat="server" Width="60px" ClientIDMode="Static" CssClass="MandatoryField year"></asp:TextBox>
                             <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                ErrorMessage="年度輸入格式錯誤" ControlToValidate="txt_ASSESS_YEAR" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression=".{4,4}" Display="None"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"    ErrorMessage="考核年度必輸入" InitialValue=""
                        ControlToValidate="txt_ASSESS_YEAR" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_ASSESS_TYPE" runat="server" Text="考核類別"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                         <asp:DropDownList ID="ddl_ASSESS_TYPE" runat="server" Width="100px" ClientIDMode="Static" CssClass="MandatoryField"> </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="Req_ASSESS_TYPE" runat="server" 
                                            ErrorMessage="考核類別必輸入" InitialValue="-1"
                                            ControlToValidate="ddl_ASSESS_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SJ0150Search" runat="server" Text="查詢" OnClick="WFB2SJ0150Search_Click" OnClientClick="return CheckSearch();" />

                                            <%--<asp:Button ID="WFB2IA0500Search" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0500Search%>" OnClick="WFB2IA0500Search_Click" OnClientClick="BlockUI();" />--%>

                                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2ia_btn_clear%>" onclick="ClearAll();" />
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
                            <aces:Btn ID="WFB2SJ0150Add" runat="server" Text="新增" OnClick="WFB2SJ0150Add_Click" />
                            <aces:Btn ID="WFB2SJ0150Delete" runat="server" Text="刪除" OnClientClick="return CheckDelAction();" OnClick="WFB2SJ0150Delete_Click" Visible="false" />
                            <aces:Btn ID="WFB2SJ0150Edit" runat="server" Text="修改" OnClick="WFB2SJ0150Edit_Click" Visible="false" />
                            <aces:Btn ID="WFB2SJ0150Dtl" runat="server" Text="明細資料" Visible="false" OnClick="WFB2SJ0150Dtl_Click" ValidationGroup="GroupA" />
                            <aces:Btn ID="WFB2SJ0150Save" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0500Save%>" Visible="false" OnClick="WFB2SJ0150Save_Click" ValidationGroup="GroupA" />

                            <%--<asp:Button ID="WFB2IA0500Add" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0500Add%>" OnClick="WFB2IA0500Add_Click" />
                            <asp:Button ID="WFB2IA0500Delete" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0500Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2IA0500Delete_Click" Visible="false" />
                            <asp:Button ID="WFB2IA0500Edit" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0500Edit%>" OnClick="WFB2IA0500Edit_Click" Visible="false" />
                            <asp:Button ID="WFB2IA0500Save" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0500Save%>" Visible="false" OnClick="WFB2IA0500Save_Click" ValidationGroup="GroupA" />
                            --%>
                            <asp:Button ID="WFB2SJ0150Cancel" runat="server" Text="取消" Visible="false" OnClick="WFB2SJ0150Cancel_Click" OnClientClick="return confirm('是否確定取消?');" />
                        </div>
                    </td>
                </tr>
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SJ0150DAO" EnablePaging="True"
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
                    <%--序號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_RowNumber%>" HeaderStyle-Width="40px">
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
                    <%--年度--%>
                    <asp:TemplateField HeaderText="年度" SortExpression="ASSESS_YEAR" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_ASSESS_YEAR" runat="server" Text='<%#Bind("ASSESS_YEAR")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_ASSESS_YEAR" runat="server" Text='<%#Bind("ASSESS_YEAR")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_NEW_ASSESS_YEAR" runat="server" ClientIDMode="Static" Width="40px" MaxLength="4" CssClass="MandatoryField year"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="req_NEW_ASSESS_YEAR" runat="server" ErrorMessage="年度不可為空白"
                                ControlToValidate="txt_NEW_ASSESS_YEAR" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="req_NEW_ASSESS_YEAR_ONLYEngNum" runat="server"
                                ErrorMessage="年度輸入格式錯誤" ControlToValidate="txt_NEW_ASSESS_YEAR" ForeColor="Red" ValidationGroup="GroupA"
                                 ValidationExpression=".{4,4}" Display="None"></asp:RegularExpressionValidator>
                        </FooterTemplate>
                    </asp:TemplateField>   
                                    
                    <%--考核類別--%>
                    <asp:TemplateField HeaderText="考核類別" SortExpression="ASSESS_TYPE" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_ASSESS_TYPE" runat="server" Text='<%#Bind("ASSESS_TYPE_DESC")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_ASSESS_TYPE" runat="server" Text='<%#Bind("ASSESS_TYPE_DESC")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                 <asp:DropDownList ID="ddl_NEW_ASSESS_TYPE" runat="server" ClientIDMode="Static" Width="60px"
                                     CssClass="MandatoryField">
                                </asp:DropDownList>
                            </div>
                            <asp:RequiredFieldValidator ID="Req_NEW_ASSESS_TYPE" runat="server" ErrorMessage="考核類別不可為空白"
                                ControlToValidate="ddl_NEW_ASSESS_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            
                        </FooterTemplate>
                    </asp:TemplateField>                   
                    <%--職種--%>
                    <asp:TemplateField HeaderText="職種" SortExpression="WS_CD" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_WS_CD" runat="server" Text='<%#Bind("WS_CD")%>' ></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>                           
                            <div style="text-align: left; width: 100%">
                                <asp:DropDownList ID="ddl_WS_CD" runat="server" ClientIDMode="Static" Width="100px"
                                     CssClass="MandatoryField">
                                </asp:DropDownList>
                            </div>
                            <asp:RequiredFieldValidator ID="Req_ddl_WS_CD" runat="server" ErrorMessage="職種不可為空白"
                                ControlToValidate="ddl_WS_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:DropDownList ID="ddl_NEW_WS_CD" runat="server" ClientIDMode="Static" Width="80px"
                                     CssClass="MandatoryField">
                                </asp:DropDownList>
                            </div>
                            <asp:RequiredFieldValidator ID="Req_NEW_ddl_WS_CD" runat="server" ErrorMessage="職種不可為空白"
                                ControlToValidate="ddl_NEW_WS_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>                 
                    <%--考核群組代號--%>
                    <asp:TemplateField HeaderText="考核群組代號" SortExpression="GRP_CD" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_GRP_CD" runat="server" Text='<%#Bind("GRP_CD")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_GRP_CD" runat="server" Text='<%#Bind("GRP_CD")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_NEW_GRP_CD" runat="server" ClientIDMode="Static" Width="100px" MaxLength="5" CssClass="MandatoryField"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="Req_NEW_GRP_CD" runat="server" 
                                ErrorMessage="考核群組代號必輸入"
                                ControlToValidate="txt_NEW_GRP_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>                
                    <%--考核群組名稱--%>
                    <asp:TemplateField HeaderText="考核群組名稱" SortExpression="GRP_NAME" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_GRP_NAME" runat="server" Text='<%#Bind("GRP_NAME")%>'></asp:Label>
                        </ItemTemplate>                       
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%">
                                 <asp:TextBox ID="txt_GRP_NAME" runat="server" Text='<%#Bind("GRP_NAME")%>' ClientIDMode="Static" CssClass="MandatoryField" Width="100px"></asp:TextBox>
                             </div>
                           <asp:RequiredFieldValidator ID="Req_GRP_NAME" runat="server" 
                                ErrorMessage="考核群組名稱必輸入"
                                ControlToValidate="txt_GRP_NAME" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_NEW_GRP_NAME" runat="server" ClientIDMode="Static" Width="100px" MaxLength="15" CssClass="MandatoryField"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="Req_NEW_GRP_NAME" runat="server" 
                                ErrorMessage="考核群組名稱必輸入"
                                ControlToValidate="txt_NEW_GRP_NAME" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>                 
                    <%--多部門重新配置--%> 
                    <asp:TemplateField HeaderText="多部門重新配置" SortExpression="REDEPLOY_YN" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_REDEPLOY_YN" runat="server" Text='<%#Bind("REDEPLOY_YN")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:DropDownList ID="ddl_REDEPLOY_YN" runat="server" ClientIDMode="Static" Width="100px" CssClass="MandatoryField">
                                </asp:DropDownList>
                            </div>
                            <asp:RequiredFieldValidator ID="Req_REDEPLOY_YN" runat="server" ErrorMessage="多部門重新配置必輸入"
                                ControlToValidate="ddl_REDEPLOY_YN" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:DropDownList ID="ddl_NEW_REDEPLOY_YN" runat="server" ClientIDMode="Static" Width="100px" CssClass="MandatoryField">
                                </asp:DropDownList>
                            </div>
                            <asp:RequiredFieldValidator ID="Req_NEW_REDEPLOY_YN" runat="server" ErrorMessage="多部門重新配置必輸入"
                                ControlToValidate="ddl_NEW_REDEPLOY_YN" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                                    
                    <%--報表區分--%> 
                    <asp:TemplateField HeaderText="報表區分" SortExpression="REPORT_TYPE" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_REPORT_TYPE" runat="server" Text='<%#Bind("REPORT_TYPE_DESC")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:DropDownList ID="ddl_REPORT_TYPE" runat="server" ClientIDMode="Static" Width="100px" CssClass="MandatoryField">
                                </asp:DropDownList>
                            </div>
                            <asp:RequiredFieldValidator ID="Req_REPORT_TYPE" runat="server" ErrorMessage="報表區分必輸入"
                                ControlToValidate="ddl_REPORT_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:DropDownList ID="ddl_NEW_REPORT_TYPE" runat="server" ClientIDMode="Static" Width="100px" CssClass="MandatoryField">
                                </asp:DropDownList>
                            </div>
                            <asp:RequiredFieldValidator ID="Req_NEW_REPORT_TYPE" runat="server" ErrorMessage="報表區分必輸入"
                                ControlToValidate="ddl_NEW_REPORT_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--報表區分--%> 
                    <asp:TemplateField HeaderText="是否控管人數" SortExpression="IS_CTL" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_IS_CTL" runat="server" Text='<%#Bind("IS_CTL_DESC")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:DropDownList ID="ddl_IS_CTL" runat="server" ClientIDMode="Static" Width="100px" CssClass="MandatoryField">
                                </asp:DropDownList>
                            </div>
                            <asp:RequiredFieldValidator ID="Req_IS_CTL" runat="server" ErrorMessage="是否控管人數必輸入"
                                ControlToValidate="ddl_IS_CTL" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:DropDownList ID="ddl_NEW_IS_CTL" runat="server" ClientIDMode="Static" Width="100px" CssClass="MandatoryField">
                                </asp:DropDownList>
                            </div>
                            <asp:RequiredFieldValidator ID="Req_NEW_IS_CTL" runat="server" ErrorMessage="是否控管人數必輸入"
                                ControlToValidate="ddl_NEW_IS_CTL" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
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
                                <asp:Label ID="Label1" runat="server" Text="序號" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="年度" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="考核類別" Width="60px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="職種" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="考核群組代號" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="考核群組名稱" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="多部門重新配置" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="報表區分" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="是否控管人數" Width="100px"></asp:Label>
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
                                <div style="text-align: left; width: 100%">
                                    <asp:TextBox ID="txt_NEW_ASSESS_YEAR" runat="server" ClientIDMode="Static" Width="50px" MaxLength="4" CssClass="MandatoryField year"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="年度不可為空白"
                                    ControlToValidate="txt_NEW_ASSESS_YEAR" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                    ErrorMessage="年度輸入格式錯誤" ControlToValidate="txt_NEW_ASSESS_YEAR" ForeColor="Red" ValidationGroup="GroupA"
                                     ValidationExpression=".{4,4}" Display="None"></asp:RegularExpressionValidator>
                            </td>
                            <td> 
                               <div style="text-align: left; width: 100%">
                                 <asp:DropDownList ID="ddl_NEW_ASSESS_TYPE" runat="server" ClientIDMode="Static" Width="50px"
                                     CssClass="MandatoryField">
                                    </asp:DropDownList>
                                </div>
                            <asp:RequiredFieldValidator ID="Req_NEW_ASSESS_TYPE" runat="server" ErrorMessage="考核類別不可為空白"
                                ControlToValidate="ddl_NEW_ASSESS_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <td> 
                                <div style="text-align: left; width: 100%">
                                    <asp:DropDownList ID="ddl_NEW_WS_CD" runat="server" ClientIDMode="Static" Width="100px"
                                         CssClass="MandatoryField">
                                    </asp:DropDownList>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="職種不可為空白"
                                    ControlToValidate="ddl_NEW_WS_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_NEW_GRP_CD" runat="server" ClientIDMode="Static" Width="100px" MaxLength="5" CssClass="MandatoryField"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="Req_NEW_GRP_CD" runat="server" 
                                ErrorMessage="考核群組代號必輸入"
                                ControlToValidate="txt_NEW_GRP_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>

                            </td>
                            <td>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_NEW_GRP_NAME" runat="server" ClientIDMode="Static" Width="100px" MaxLength="15" CssClass="MandatoryField"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="Req_NEW_GRP_NAME" runat="server" 
                                ErrorMessage="考核群組名稱必輸入"
                                ControlToValidate="txt_NEW_GRP_NAME" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>

                            </td>
                            <td>
                            <div style="text-align: left; width: 100%">
                                <asp:DropDownList ID="ddl_NEW_REDEPLOY_YN" runat="server" ClientIDMode="Static" Width="60px" CssClass="MandatoryField">
                                </asp:DropDownList>
                            </div>
                            <asp:RequiredFieldValidator ID="Req_NEW_REDEPLOY_YN" runat="server" ErrorMessage="多部門重新配置必輸入"
                                ControlToValidate="ddl_NEW_REDEPLOY_YN" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>

                            </td>
                            <td>
                            <div style="text-align: left; width: 100%">
                                <asp:DropDownList ID="ddl_NEW_REPORT_TYPE" runat="server" ClientIDMode="Static" Width="60px" CssClass="MandatoryField">
                                </asp:DropDownList>
                            </div>
                            <asp:RequiredFieldValidator ID="Req_NEW_REPORT_TYPE" runat="server" ErrorMessage="報表區分必輸入"
                                ControlToValidate="ddl_NEW_REPORT_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>

                            </td>
                            <td>
                            <div style="text-align: left; width: 100%">
                                <asp:DropDownList ID="ddl_NEW_IS_CTL" runat="server" ClientIDMode="Static" Width="60px" CssClass="MandatoryField">
                                </asp:DropDownList>
                            </div>
                            <asp:RequiredFieldValidator ID="Req_NEW_IS_CTL" runat="server" ErrorMessage="是否控管人數必輸入"
                                ControlToValidate="ddl_NEW_IS_CTL" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>

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
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_50_Rows%>" Value="70"></asp:ListItem>
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
            <asp:Button ID="HID_cancel" runat="server" Style="display: none" ClientIDMode="Static" OnClick="HID_cancel_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

