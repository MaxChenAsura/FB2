<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dj/WFB2DJ0100_Qry.aspx.cs" Inherits="WebContent_WFB2DJ0100_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">

        $(function () {

            iniForm();
        });
        function iniForm() {
            //MandatoryField date
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.date').mask('9999/99/99');
            $(".envValue").mask('9999');
            $(".envUnit").mask('9.9');
            $(".decimal").css("text-align", "right").css("ime-mode", "disabled");
            
           // reComma("ALLOWANCE_VALUE",0);
            gridviewScroll();
            $.unblockUI();
        }


        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());

        }

        //凍結視窗用
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "400",
                barcolor: "#7F7F7F"
            });
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
                    if (choietr == null)
                        choietr = i;
                }
            }
            return HaveCheck;
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
            $("#ddl_ENV_ALLOWANCE_TYPE").val("-1");
            $('#<%=rbl_USE_STATUS.ClientID %>').find("input[value=Y]").prop("checked", "checked");
        }
        //檢查英數字
        function CheckSYS_CD(source, arguments) {
            var re = /^[\d|a-zA-Z]+$/;
            if (!re.test($("#txt_NEW_ALLOWANCE_TYPE").val()))
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
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
                    <col width="12%" />
                    <col width="88%" />
                </colgroup>
                <tbody>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--環境津貼等級--%>
                            <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_env_allowance_type%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" colspan="5">
                            <asp:DropDownList ID="ddl_ENV_ALLOWANCE_TYPE" runat="server" ClientIDMode="Static"  ></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--使用狀態--%>
                            <asp:Label ID="lb_STATUS" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_status%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:RadioButtonList ID="rbl_USE_STATUS" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" ClientIDMode="Static">
                                            <asp:ListItem Text="<%$Resources:Resource,wfb2dj_lb_isvalid_true%>" Value="Y" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="<%$Resources:Resource,wfb2dj_lb_isvalid_false%>" Value="N"></asp:ListItem>
                                            <asp:ListItem Text="<%$Resources:Resource,wfb2dj_lb_isvalid_all%>" Value="ALL"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="3">
                             <aces:Btn ID="WFB2DJ0100Search" runat="server" Text="<%$Resources:Resource,wfb2dj_btn_search%>" OnClick="WFB2DJ0100Search_Click" OnClientClick=" BlockUI();"  />

                             <%--<asp:Button ID="WFB2DJ0100Search" runat="server" Text="<%$Resources:Resource,wfb2dj_btn_search%>" OnClick="WFB2DJ0100Search_Click" OnClientClick=" BlockUI();"  />--%>
                              
                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2dj_btn_clear%>" onclick="ClearAll();"   />
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
                        <aces:Btn ID="WFB2DJ0100Add" runat="server" Text="<%$Resources:Resource,wfb2dj_btn_add%>"  Visible="true" OnClick="WFB2DJ0100Add_Click" />
                            <aces:Btn ID="WFB2DJ0100Delete" runat="server" Text="<%$Resources:Resource,wfb2dj_btn_delete%>"   Visible="false" OnClick="WFB2DJ0100Delete_Click" OnClientClick="return doDelete();" />
                            <aces:Btn ID="WFB2DJ0100Edit" runat="server" Text="<%$Resources:Resource,wfb2dj_btn_edit%>"  Visible="false" OnClick="WFB2DJ0100Edit_Click" OnClientClick="BlockUI();" />
                            <aces:Btn ID="WFB2DJ0100Save" runat="server" Text="<%$Resources:Resource,wfb2dj_btn_save%>" Visible="false"  ValidationGroup="GroupA" OnClick="WFB2DJ0100Save_Click"   />

                           <%-- <asp:Button ID="WFB2DJ0100Add" runat="server" Text="<%$Resources:Resource,wfb2dj_btn_add%>"  Visible="true" OnClick="WFB2DJ0100Add_Click" />
                            <asp:Button ID="WFB2DJ0100Delete" runat="server" Text="<%$Resources:Resource,wfb2dj_btn_delete%>"   Visible="false" OnClick="WFB2DJ0100Delete_Click" OnClientClick="return doDelete();" />
                            <asp:Button ID="WFB2DJ0100Edit" runat="server" Text="<%$Resources:Resource,wfb2dj_btn_edit%>"  Visible="false" OnClick="WFB2DJ0100Edit_Click" OnClientClick="BlockUI();" />
                            <asp:Button ID="WFB2DJ0100Save" runat="server" Text="<%$Resources:Resource,wfb2dj_btn_save%>" Visible="false"  ValidationGroup="GroupA" OnClick="WFB2DJ0100Save_Click"   />--%>
                            
                            <asp:Button ID="btn_cancel" runat="server" Text="<%$Resources:Resource,wfb2dj_btn_cancel%>" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="btn_cancel_Click" />
                        </div>
                    </td>
                </tr>
                </tbody>
            </table>


              <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DJ0100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                   <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="hid_qry_ENV_ALLOWANCE_TYPE"
                        Name="env_type" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_qry_USE_STATUS" DefaultValue=""
                        Name="is_valid" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
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
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID"  Width="20px"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--序號--%>
                     <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_rownumber%>" HeaderStyle-Width="40px"  >
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'  Width="40px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                             <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'  Width="40px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                               
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--環境津貼等級--%>
                     <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_env_allowance_type%>" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left" SortExpression="ENV_ALLOWANCE_TYPE" >
                        <ItemTemplate>
                            <asp:Label ID="lb_ENV_ALLOWANCE_TYPE" runat="server" Text='<%#Bind("ENV_ALLOWANCE_TYPE")%>'   Width="120px"></asp:Label>
                        </ItemTemplate>  
                        <EditItemTemplate>
                            <asp:Label ID="lb_ENV_ALLOWANCE_TYPE" runat="server" Text='<%#Bind("ENV_ALLOWANCE_TYPE")%>'    Width="120px" ></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                             <asp:TextBox ID="txt_NEW_ALLOWANCE_TYPE" runat="server"   ClientIDMode="Static" MaxLength="1"  Width="120px"  CssClass="MandatoryField"  ></asp:TextBox>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dj_required_env_allowance_type%>"
                                ControlToValidate="txt_NEW_ALLOWANCE_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                             <asp:RegularExpressionValidator ID="onlyEngNum" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2dj_format_allowance_type%>" ControlToValidate="txt_NEW_ALLOWANCE_TYPE" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--環境津貼等級說明--%>
                     <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_env_allowance_desc%>" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Left" >
                        <ItemTemplate>
                            <asp:Label ID="lb_ENV_ALLOWANCE_DESC" runat="server" Text='<%#Bind("ENV_ALLOWANCE_DESC")%>' Width="160px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_ENV_ALLOWANCE_DESC" runat="server" Text='<%#Bind("ENV_ALLOWANCE_DESC")%>'  ClientIDMode="Static" MaxLength="30" Width="160px"></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_ENV_ALLOWANCE_DESC" runat="server"   ClientIDMode="Static" Width="160px" MaxLength="30" ></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--環境津貼(元/日)--%>
                     <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_env_allowance_value%>" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="right"   SortExpression="ENV_ALLOWANCE_VALUE">
                        <ItemTemplate>
                            <asp:Label ID="lb_ENV_ALLOWANCE_VALUE" runat="server" Text='<%#Bind("ENV_ALLOWANCE_VALUE","{0:n0}")%>'  Width="140px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_ENV_ALLOWANCE_VALUE" runat="server" Text='<%#Bind("ENV_ALLOWANCE_VALUE")%>'  ClientIDMode="Static"  Width="140px"  CssClass=" MandatoryField envValue decimal"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dj_required_env_allowance_value%>"
                                ControlToValidate="txt_EDIT_ENV_ALLOWANCE_VALUE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                             <asp:TextBox ID="txt_NEW_ENV_ALLOWANCE_VALUE" runat="server"  ClientIDMode="Static"  Width="140px"  CssClass=" MandatoryField envValue decimal"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dj_required_env_allowance_value%>"
                                ControlToValidate="txt_NEW_ENV_ALLOWANCE_VALUE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--單位(日)--%>
                     <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_env_min_unit%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="right">
                        <ItemTemplate>
                            <asp:Label ID="lb_ENV_MIN_UNIT" runat="server" Text='<%#Bind("ENV_MIN_UNIT")%>'  Width="80px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                             <asp:DropDownList ID="ddl_EDIT_ENV_MIN_UNIT" runat="server" CssClass="MandatoryField"></asp:DropDownList>
                             <asp:HiddenField ID="hid_EDIT_ENV_MIN_UNIT" runat="server" Value='<%#Bind("ENV_MIN_UNIT")%> ' ClientIDMode="Static" />
                             <%--
                            <asp:TextBox ID="txt_EDIT_ENV_MIN_UNIT" runat="server" Text='<%#Bind("ENV_MIN_UNIT")%>'  ClientIDMode="Static"  Width="80px"  CssClass="MandatoryField envUnit decimal"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dj_required_env_min_unit%>"
                                ControlToValidate="txt_EDIT_ENV_MIN_UNIT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                  --%>
                        </EditItemTemplate>
                        <FooterTemplate>
                              <asp:DropDownList ID="ddl_NEW_ENV_MIN_UNIT" runat="server" CssClass="MandatoryField" Width="80px"></asp:DropDownList>
                             <%--
                            <asp:TextBox ID="txt_NEW_ENV_MIN_UNIT" runat="server"   ClientIDMode="Static"  Width="80px"  CssClass="MandatoryField envUnit decimal"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dj_required_env_min_unit%>"
                                ControlToValidate="txt_NEW_ENV_MIN_UNIT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                 --%>
                        </FooterTemplate>
                    </asp:TemplateField>
                  <%-- 生效日期--%>
                     <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_start_dt%>" HeaderStyle-Width="100px" SortExpression="START_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_START_DT" runat="server" Text='<%#Bind("START_DT", "{0:yyyy/MM/dd}")%>'  Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                             <asp:Label ID="lb_START_DT" runat="server" Text='<%#Bind("START_DT", "{0:yyyy/MM/dd}")%>'  Width="100px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_NEW_START_DT" runat="server"  MaxLength="10" Width="100px" ClientIDMode="Static"    CssClass="MandatoryField date"></asp:TextBox>
                                <!--驗證不可空白 -->
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dj_required_start_dt%>"
                                    ControlToValidate="txt_NEW_START_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <!--驗證日期格式-->
                                <asp:CustomValidator ID="RegularExpressionValidator8" runat="server" ValidateEmptyText="true"
		                        ErrorMessage="<%$Resources:Resource,wfb2dj_format_start_dt%>" ClientValidationFunction="CheckDate" ForeColor="Red"
	                        	ControlToValidate="txt_NEW_START_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--結束日期--%>
                     <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_end_dt%>" HeaderStyle-Width="100px" SortExpression="END_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_END_DT" runat="server" Text='<%#Bind("END_DT", "{0:yyyy/MM/dd}")%>'  Width="100px"></asp:Label>
                            <asp:HiddenField ID="hid_END_DT" runat="server" ClientIDMode="Static" Value='<%#Bind("END_DT", "{0:yyyy/MM/dd}")%>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_EDIT_END_DT" runat="server"  Text='<%#Bind("END_DT", "{0:yyyy/MM/dd}")%>'  MaxLength="10" Width="100px" ClientIDMode="Static"  CssClass="date"></asp:TextBox>
                                <!--驗證日期格式-->
                                 <asp:CustomValidator ID="RegularExpressionValidator4" runat="server" ValidateEmptyText="true"
		                        ErrorMessage="<%$Resources:Resource,wfb2dj_format_end_dt%>" ClientValidationFunction="CheckDate" ForeColor="Red"
	                        	ControlToValidate="txt_EDIT_END_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_NEW_END_DT" runat="server"    MaxLength="10" Width="100px" ClientIDMode="Static"  CssClass="date"></asp:TextBox>
                                <!--驗證日期格式-->
                                 <asp:CustomValidator ID="RegularExpressionValidator9" runat="server" ValidateEmptyText="true"
		                        ErrorMessage="<%$Resources:Resource,wfb2dj_format_end_dt%>" ClientValidationFunction="CheckDate" ForeColor="Red"
	                        	ControlToValidate="txt_NEW_END_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                <!--起日不可大於迄日 -->	
								<asp:CompareValidator ID="foot1" runat="server" ControlToCompare="txt_NEW_START_DT"
                                ControlToValidate="txt_NEW_END_DT" ErrorMessage="<%$Resources:Resource,wfb2dj_error_start_dt_s_e%>" Type="Date" Operator="GreaterThanEqual"
                                Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--備註--%>
                     <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_remark%>" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="left">
                        <ItemTemplate>
                            <asp:Label ID="lb_REMARK" runat="server" Text='<%#Bind("REMARK")%>'  Width="200px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                             <asp:TextBox ID="txt_EDIT_REMARK" runat="server"   Text='<%#Bind("REMARK")%>'  ClientIDMode="Static" MaxLength="210"  Width="200px"></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                              <asp:TextBox ID="txt_NEW_REMARK" runat="server"   ClientIDMode="Static" MaxLength="210"  Width="200px"></asp:TextBox>
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
                                <%--序號--%>
                                 <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_rownumber%>" Width="40px"></asp:Label>
                            </td>
                            <td>  
                                <%--環境津貼等級--%>
                                  <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_env_allowance_type%>" Width="120px"></asp:Label>
                            </td>
                            <td> 
                               <%-- 環境津貼等級說明--%>
                                  <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_env_allowance_desc%>" Width="160px"></asp:Label>
                            </td>
                            <td> 
                                <%--環境津貼(元/日)--%>
                                  <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_env_allowance_value%>" Width="140px"></asp:Label>
                            </td>
                            <td>  
                                <%--單位(日)--%>
                                  <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_env_min_unit%>" Width="80px"></asp:Label>
                            </td>
                            <td> 
                                <%--生效日期--%>
                                  <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_start_dt%>" Width="100px"></asp:Label>
                            </td>
                            <td> 
                                <%--結束日期--%>
                                  <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_end_dt%>" Width="100px"></asp:Label>
                            </td>
                            <td>  
                               <%-- 備註--%>
                                  <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_remark%>" Width="200px"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td>  </td>
                            <td>  </td>
                            <td>   <asp:TextBox ID="txt_NEW_ALLOWANCE_TYPE" runat="server"   ClientIDMode="Static" MaxLength="1"  Width="120px"  CssClass="MandatoryField"></asp:TextBox>
                                  <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dj_required_env_allowance_type%>"
                                ControlToValidate="txt_NEW_ALLOWANCE_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                 <asp:RegularExpressionValidator ID="onlyEngNum" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2dj_format_allowance_type%>" ControlToValidate="txt_NEW_ALLOWANCE_TYPE" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                            </td>
                            <td>  
                                <asp:TextBox ID="txt_NEW_ENV_ALLOWANCE_DESC" runat="server"   ClientIDMode="Static" Width="160px" MaxLength="30" ></asp:TextBox>
                            </td>
                            <td>  
                                 <asp:TextBox ID="txt_NEW_ENV_ALLOWANCE_VALUE" runat="server"  ClientIDMode="Static"  Width="140px"  CssClass=" MandatoryField envValue decimal"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dj_required_env_allowance_type%>"
                                ControlToValidate="txt_NEW_ENV_ALLOWANCE_VALUE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <td>  
                                 <asp:DropDownList ID="ddl_NEW_ENV_MIN_UNIT" runat="server" CssClass="MandatoryField" Width="80px"></asp:DropDownList>
                                <%--
                                <asp:TextBox ID="txt_NEW_ENV_MIN_UNIT" runat="server" ClientIDMode="Static"  Width="80px"  CssClass="MandatoryField envUnit decimal"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dj_required_env_min_unit%>"
                                ControlToValidate="txt_NEW_ENV_MIN_UNIT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                     --%>
                            </td>
                            <td>  
                                <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_NEW_START_DT" runat="server"  MaxLength="10" Width="100px" ClientIDMode="Static"    CssClass="MandatoryField date"></asp:TextBox>
                                <!--驗證不可空白 -->
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dj_required_start_dt%>"
                                    ControlToValidate="txt_NEW_START_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <!--驗證日期格式-->
                                <asp:CustomValidator ID="RegularExpressionValidator8" runat="server" ValidateEmptyText="true"
		                                ErrorMessage="<%$Resources:Resource,wfb2dj_format_start_dt%>" ClientValidationFunction="CheckDate" ForeColor="Red"
		                                ControlToValidate="txt_NEW_START_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>	

                            </div>
                            </td>
                            <td>  
                                <asp:TextBox ID="txt_NEW_END_DT" runat="server"    MaxLength="10" Width="100px" ClientIDMode="Static"  CssClass="date"></asp:TextBox>
                                <!--驗證日期格式-->
                                 <asp:CustomValidator ID="RegularExpressionValidator9" runat="server" ValidateEmptyText="true"
		                        ErrorMessage="<%$Resources:Resource,wfb2dj_format_end_dt%>" ClientValidationFunction="CheckDate" ForeColor="Red"
		                        ControlToValidate="txt_NEW_END_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>	
                                 <!--起日不可大於迄日 -->	
								<asp:CompareValidator ID="foot1" runat="server" ControlToCompare="txt_NEW_START_DT"
                                ControlToValidate="txt_NEW_END_DT" ErrorMessage="<%$Resources:Resource,wfb2dj_error_start_dt_s_e%>" Type="Date" Operator="GreaterThanEqual"
                                Display="None" ValidationGroup="GroupA"></asp:CompareValidator>

                            </td>
                            <td>  
                                <asp:TextBox ID="txt_NEW_REMARK" runat="server"   ClientIDMode="Static" MaxLength="210"  Width="200px"></asp:TextBox>
                            </td>
                        </tr>

                        </table>
                  </EmptyDataTemplate>
                <HeaderStyle CssClass="GridviewScrollHeader" />
                <PagerStyle CssClass="GridviewScrollPager" />
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

             <!-- 查詢條件 -->
            <asp:HiddenField ID="hid_qry_ENV_ALLOWANCE_TYPE" runat="server" ClientIDMode="Static" /> 
            <asp:HiddenField ID="hid_qry_USE_STATUS" runat="server" ClientIDMode="Static" /> 

             <!-- 每頁的筆數 -->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" /> 
            <!-- 是否要凍結視窗 -->
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="N" />
            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>