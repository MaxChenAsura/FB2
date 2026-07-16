<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2hb/WFB2HB0500_Qry.aspx.cs" Inherits="WebContent_fb2hb_WFB2HB0500_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            gridviewScroll();
            $.unblockUI();

            $('#txt_NEW_EMP_ID').change(function () {
                //ajax 取得員工基本資料
                $.ajax({
                    url: "WFB2HB0400_GetEmpData.ashx",
                    data: {
                        EMP_ID: $('#txt_NEW_EMP_ID').val()
                    },
                    type: "GET",
                    cache: false,
                    dataType: 'json',
                    success: function (JData) {
                        if (JData.errMsg != "")
                            alert(JData.errMsg);
                        else {
                            $('#lb_NEW_EMP_NAME').text(JData.EMP_NAME);
                            $('#lb_ORI_NEW_DEPT_NAME').text(JData.DEPT_NAME);

                        }
                    },

                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });

            });
        }

        function gridviewScroll() {

            if ($("#HID_Freeze").val() == "Y") {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "400",
                    barcolor: "#7F7F7F",
                    freezesize: 4

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

        //清空畫面
        function ClearAll() {
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#ddl_SKILL_TYPE").val("-1");
            $("#ddl_SKILL_GRADE").val("-1");
        }

        //儲存前檢查
        function saveCheck() {
            var processed = true;
            var errMessage = "";
            //alert($("#txt_NEW_SKILL_GRADE").val());
            //新增下
            if ($("#ddl_NEW_SKILL_TYPE").val() == "1" || $("#ddl_NEW_SKILL_TYPE").val() == "2") {
                if ($("#txt_NEW_SKILL_GRADE").val() == "") {
                    errMessage += "技能專長類別為外語或證照, 則外語等級/證照等級必須輸入 \n";
                }
                if ($("#txt_NEW_SKILL_ORG").val() == "") {
                    errMessage += "技能專長類別為外語或證照, 則認證機構必須輸入 \n";
                }
            }
            if ($("#ddl_NEW_SKILL_TYPE").val() == "3") {
                if ($("#txt_NEW_AWARD_DT").val() == "")
                    errMessage += "技能專長類別為獲獎, 則獲獎日期必須輸入 \n";
            }

            //更新下
            if ($("#HID_SKILL_TYPE").val() == "1" || $("#HID_SKILL_TYPE").val() == "2") {
                if ($("#txt_EDIT_SKILL_GRADE").val() == "") {
                    errMessage += "技能專長類別為外語或證照, 則外語等級/證照等級必須輸入 \n";
                }
                if ($("#txt_EDIT_SKILL_ORG").val() == "") {
                    errMessage += "技能專長類別為外語或證照, 則認證機構必須輸入 \n";
                }
            }
            if ($("#HID_SKILL_TYPE").val() == "3") {
                if ($("#txt_EDIT_AWARD_DT").val() == "")
                    errMessage += "技能專長類別為獲獎, 則獲獎日期必須輸入 \n";
            }

            if (errMessage != "") {
                processed = false;
                alert(errMessage);

            }

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                processed = false;
            if (!processed)
                $.unblockUI();

            return processed;
        }

        function openUpload() {
            window.location.href("WFB2HB0500_Upload.aspx");
            return false;
        }

        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="12%" />
                    <col width="20%" />
                    <col width="15%" />
                    <col width="20%" />
                    <col width="15%" />
                    <col width="18%" />
                </colgroup>
                <tbody>
                    <!-- START: 1st Line in Search Criteria area -->
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_EMP_ID%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="6" Width="42px" ClientIDMode="Static"></asp:TextBox>
                            <input id="bt_EMP_ID" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N');" />
                            <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="10" Width="50px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_SKILL_TYPE" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_SKILL_TYPE%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_SKILL_TYPE" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_SKILL_GRADE" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_SKILL_GRADE%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_SKILL_GRADE" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="Body_label" colspan="10">
                            <div id="init">
                                <%--<aces:Btn ID="WFB2HB0500Search" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0500Search%>" OnClick="WFB2HA0500Search_Click" OnClientClick="BlockUI();" />--%>
                                <asp:Button ID="WFB2HB0500Search" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0500Search%>" OnClick="WFB2HA0500Search_Click" OnClientClick="BlockUI();" />
                                <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2hb_btn_clear%>" onclick="ClearAll();" />
                                <%--<aces:Btn ID="WFB2HB0500Upload" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0500Upload%>" OnClientClick="return openUpload();"  />--%>
                                <asp:Button ID="WFB2HB0500Upload" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0500Upload%>" OnClientClick="return openUpload();" />
                            </div>
                        </td>
                    </tr>


                    <!-- end: Create MODULE ID -->
                    <!-- START: Create a line to separate Search field with body field -->
                    <tr>
                        <td align="center" height="1" colspan="10">
                            <hr>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="10">
                            <%--<aces:Btn ID="WFB2HB0500Add" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0500Add%>" OnClick="WFB2HB0500Add_Click" />
                            <aces:Btn ID="WFB2HB0500Delete" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0500Delete%>" OnClientClick="return confirm('確定要刪除?');" OnClick="WFB2HB0500Delete_Click" Visible="False" />
                            <aces:Btn ID="WFB2HB0500Edit" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0500Edit%>" OnClick="WFB2HB0500Edit_Click" Visible="False" />
                            <aces:Btn ID="WFB2HB0500Save" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0500Save%>" Visible="false" OnClick="WFB2HB0500Save_Click" OnClientClick="return saveCheck();" />
                            <aces:Btn ID="WFB2HB0500Cancel" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0500Cancel%>" Visible="false" OnClick="WFB2HB0500Cancel_Click" OnClientClick="return confirm('是否確定取消?');" />--%>
                            <asp:Button ID="WFB2HB0500Add" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0500Add%>" OnClick="WFB2HB0500Add_Click" />
                            <asp:Button ID="WFB2HB0500Delete" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0500Delete%>" OnClientClick="return confirm('確定要刪除?');" OnClick="WFB2HB0500Delete_Click" Visible="False" />
                            <asp:Button ID="WFB2HB0500Edit" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0500Edit%>" OnClick="WFB2HB0500Edit_Click" Visible="False" />
                            <asp:Button ID="WFB2HB0500Save" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0500Save%>" Visible="false" OnClick="WFB2HB0500Save_Click" OnClientClick="return saveCheck();" />
                            <asp:Button ID="WFB2HB0500Cancel" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0500Cancel%>" Visible="false" OnClick="WFB2HB0500Cancel_Click" OnClientClick="return confirm('是否確定取消?');" />
                        </td>
                    </tr>
                    <!-- END: Create a line -->
                </tbody>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2HB0500DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_EMP_ID" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="emp_id" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="ddl_SKILL_TYPE" DefaultValue=""
                        Name="skill_type" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="ddl_SKILL_GRADE" DefaultValue=""
                        Name="skill_grade" PropertyName="SelectedValue" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1200px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_RowNumber%>" HeaderStyle-Width="20px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_EMP_ID" runat="server" MaxLength="5" Width="40px" CssClass="MandatoryField AjaxEMPID" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hb_Required_EMP_ID%>"
                                ControlToValidate="txt_NEW_EMP_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_NEW_EMP_NAME" runat="server" ClientIDMode="Static"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_ORI_DEPT_NO%>" SortExpression="ORI_DEPT_NO" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_ORI_DEPT_NAME" runat="server" Text='<%#Bind("ORI_DEPT_FULL_NAME")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_ORI_DEPT_NAME" runat="server" Text='<%#Bind("ORI_DEPT_FULL_NAME")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_ORI_NEW_DEPT_NAME" runat="server" ClientIDMode="Static"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_SKILL_TYPE%>" SortExpression="SKILL_TYPE" HeaderStyle-Width="50px">
                        <ItemTemplate>
                            <asp:Label ID="lb_SKILL_TYPE" runat="server" Text='<%#Bind("SKILL_TYPE_NAME")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_SKILL_TYPE" runat="server" Text='<%#Bind("SKILL_TYPE_NAME")%>'></asp:Label>
                            <asp:HiddenField ID="HID_SKILL_TYPE" runat="server" ClientIDMode="Static" Value='<%#Bind("SKILL_TYPE")%>' />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_NEW_SKILL_TYPE" runat="server" CssClass="MandatoryField" ClientIDMode="Static"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hb_Required_SKILL_TYPE%>"
                                ControlToValidate="ddl_NEW_SKILL_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_SKILL_DESC%>" SortExpression="SKILL_DESC" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_SKILL_DESC" runat="server" Text='<%#Bind("SKILL_DESC")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_SKILL_DESC" runat="server" Text='<%#Bind("SKILL_DESC")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_SKILL_DESC" runat="server" CssClass="MandatoryField"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hb_Required_SKILL_DESC%>"
                                ControlToValidate="txt_NEW_SKILL_DESC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_SKILL_GRADE%>" SortExpression="SKILL_GRADE" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_SKILL_GRADE" runat="server" Text='<%#Bind("SKILL_GRADE")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_SKILL_GRADE" runat="server" Text='<%#Bind("SKILL_GRADE")%>' ClientIDMode="Static"></asp:TextBox>

                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_SKILL_GRADE" runat="server" ClientIDMode="Static"></asp:TextBox>

                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_SKILL_ORG%>" SortExpression="SKILL_ORG" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_SKILL_ORG" runat="server" Text='<%#Bind("SKILL_ORG")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_SKILL_ORG" runat="server" Text='<%#Bind("SKILL_ORG")%>'></asp:TextBox>

                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_SKILL_ORG" runat="server"></asp:TextBox>

                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_AWARD_DT%>" SortExpression="AWARD_DT" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_AWARD_DT" runat="server" Text='<%#Bind("AWARD_DT")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_AWARD_DT" runat="server" MaxLength="10" Width="81px" CssClass="date" ClientIDMode="Static" Text='<%#Bind("AWARD_DT")%>'></asp:TextBox>

                            <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2hb_ERR_AWARD_DT%>" ControlToValidate="txt_EDIT_AWARD_DT" ForeColor="Red"
                                ValidationExpression="(19|20|99)\d\d[/ /.](0[1-9]|1[012])[/ /.](0[1-9]|[12][0-9]|3[01])" Display="None" ValidationGroup="GroupA">
                            </asp:RegularExpressionValidator>


                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_AWARD_DT" runat="server" MaxLength="10" Width="81px" CssClass="date" ClientIDMode="Static"></asp:TextBox>

                            <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2hb_ERR_AWARD_DT%>" ControlToValidate="txt_NEW_AWARD_DT" ForeColor="Red"
                                ValidationExpression="(19|20|99)\d\d[/ /.](0[1-9]|1[012])[/ /.](0[1-9]|[12][0-9]|3[01])" Display="None" ValidationGroup="GroupA">
                            </asp:RegularExpressionValidator>

                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>

                    <table class="grid-view" width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                        <tr class="header">
                            <td></td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2ha_RowNumber%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_EMP_ID%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_EMP_NAME%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2hb_ORI_DEPT_NO%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text="<%$Resources:Resource,wfb2hb_SKILL_TYPE%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2hb_SKILL_DESC%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2hb_SKILL_GRADE%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2hb_SKILL_ORG%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2hb_AWARD_DT%>"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td></td>
                            <td></td>
                            <td>
                                <asp:TextBox ID="txt_NEW_EMP_ID" runat="server" MaxLength="5" Width="40px" CssClass="MandatoryField"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hb_Required_EMP_ID%>"
                                    ControlToValidate="txt_NEW_EMP_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:Label ID="lb_NEW_EMP_NAME" runat="server" ClientIDMode="Static"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lb_ORI_NEW_DEPT_NAME" runat="server" ClientIDMode="Static"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_NEW_SKILL_TYPE" runat="server" CssClass="MandatoryField" ClientIDMode="Static"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hb_Required_SKILL_TYPE%>"
                                    ControlToValidate="ddl_NEW_SKILL_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_SKILL_DESC" runat="server" CssClass="MandatoryField"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hb_Required_SKILL_DESC%>"
                                    ControlToValidate="txt_NEW_SKILL_DESC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_SKILL_GRADE" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_SKILL_ORG" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_AWARD_DT" runat="server" MaxLength="10" Width="81px" CssClass="date" ClientIDMode="Static"></asp:TextBox>

                                <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2hb_ERR_AWARD_DT%>" ControlToValidate="txt_NEW_AWARD_DT" ForeColor="Red"
                                    ValidationExpression="(19|20|99)\d\d[/ /.](0[1-9]|1[012])[/ /.](0[1-9]|[12][0-9]|3[01])" Display="None" ValidationGroup="GroupA">
                                </asp:RegularExpressionValidator>

                            </td>

                        </tr>
                    </table>

                </EmptyDataTemplate>
                <HeaderStyle CssClass="GridviewScrollHeader" />
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle HorizontalAlign="Center" />
                <EditRowStyle HorizontalAlign="Center" />
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
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
