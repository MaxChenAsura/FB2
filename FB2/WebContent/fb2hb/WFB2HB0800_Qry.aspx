<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2hb/WFB2HB0800_Qry.aspx.cs" Inherits="WebContent_WFB2HB0800_Qry" Culture="auto" UICulture="auto" %>
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
            $('#txt_EMP_ID').change(function () {
                if ($('#txt_EMP_ID').val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_EMP_ID').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg == "1")
                                alert(JData.errMsg);
                            else {
                                $('#txt_EMP_NAME').val(JData.EMP_NAME);
                                $('#txt_EMP_NAME').keydown(false);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                }
            });
            $('#txt_EMP_ID_Add').change(function () {
                if ($('#txt_EMP_ID_Add').val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_EMP_ID_Add').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg == "1")
                                alert(JData.errMsg);
                            else {
                                $('#txt_EMP_NAME_Add').val(JData.EMP_NAME);
                                $('#txt_EMP_NAME_Add').keydown(false);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                }
            });
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
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
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
        function IsIntText() {
            var charkeycode = window.event.keyCode;
            if (charkeycode > 47 && charkeycode < 58) {
                return true;
            }
            return false;
        }
        function SelectAllCheckboxesForHB0800(spanChk) {
            elm = document.forms[0];
            for (i = 0; i <= elm.length - 1; i++) {

                if (elm[i].type == "checkbox" && elm[i].id != spanChk.id && elm[i].id.substr(elm[i].id.length - 8, 18) == 'cb_check') {
                    ///if (elm.elements[i].disabled !=true) {
                        if (elm.elements[i].checked != spanChk.checked)
                            $('#' + elm[i].id).prop('checked', spanChk.checked);
                   /// }
                }
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
                                        <asp:Label ID="lbl_EMP_ID" runat="server" Text="員工"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                                      <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N');" />
                                                      <asp:TextBox ID="txt_EMP_NAME" runat="server" Width="90px" ClientIDMode="Static" BorderWidth="0" ></asp:TextBox> 
                                    </td>       
                                   <td></td>
                                   <td></td>
                                   <td></td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2HB0800Search" runat="server" Text="查詢" OnClick="WFB2HB0800Search_Click" OnClientClick="return CheckSearch();" />

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
                            <aces:Btn ID="WFB2HB0800Add" runat="server" Text="新增" OnClick="WFB2HB0800Add_Click" />
                            <aces:Btn ID="WFB2HB0800Delete" runat="server" Text="刪除" OnClientClick="return CheckDelAction();" OnClick="WFB2HB0800Delete_Click" Visible="false" />
                            <aces:Btn ID="WFB2HB0800Edit" runat="server" Text="修改" OnClick="WFB2HB0800Edit_Click" Visible="false" />
                           <aces:Btn ID="WFB2HB0800Save" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0500Save%>" Visible="false" OnClick="WFB2HB0800Save_Click" ValidationGroup="GroupA" />

                            <%--<asp:Button ID="WFB2IA0500Add" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0500Add%>" OnClick="WFB2IA0500Add_Click" />
                            <asp:Button ID="WFB2IA0500Delete" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0500Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2IA0500Delete_Click" Visible="false" />
                            <asp:Button ID="WFB2IA0500Edit" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0500Edit%>" OnClick="WFB2IA0500Edit_Click" Visible="false" />
                            <asp:Button ID="WFB2IA0500Save" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0500Save%>" Visible="false" OnClick="WFB2IA0500Save_Click" ValidationGroup="GroupA" />
                            --%>
                            <asp:Button ID="WFB2HB0800Cancel" runat="server" Text="取消" Visible="false" OnClick="WFB2HB0800Cancel_Click" OnClientClick="return confirm('是否確定取消?');" />
                        </div>
                    </td>
                </tr>
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2HB0800DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                     <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                 
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
                             <asp:HiddenField ID="hid_EMP_STATUS" Value='<%#Bind("EMP_STATUS")%>'  runat="server" ClientIDMode="Static" /> 
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:CheckBox ID="cb_check" runat="server" Checked="true" />
                                <asp:HiddenField ID="hid_EMP_STATUS" Value='<%#Bind("EMP_STATUS")%>'  runat="server" ClientIDMode="Static" /> 
                            </div>
                        </EditItemTemplate>
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
                    <%--員工--%>
                    <asp:TemplateField HeaderText="員工編號" SortExpression="EMP_ID" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                             <div style="text-align: center; width: 120px">
                                <asp:TextBox ID="txt_EMP_ID_Add" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" CssClass="MandatoryField" onkeypress="return IsIntText();"></asp:TextBox>
                                <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID_Add', 'txt_EMP_NAME_Add', 'N');" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ib_EMP_ID_Required%>"
                                    ControlToValidate="txt_EMP_ID_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="">
                                </asp:RequiredFieldValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>   
                                    
                    <%--員工姓名--%>
                    <asp:TemplateField HeaderText="員工姓名" SortExpression="EMP_NAME" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                             <asp:TextBox ID="txt_EMP_NAME_Add" MaxLength="150" ClientIDMode="Static" runat="server" Text='<%#Bind("EMP_NAME")%>'  BorderWidth="0" ></asp:TextBox>
                            
                        </FooterTemplate>
                    </asp:TemplateField>                   
                              
                    <%--日文分數--%>
                    <asp:TemplateField HeaderText="日文分數" SortExpression="LANGUAGE_JAPANESE" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_LANGUAGE_JAPANESE" runat="server" Text='<%#Bind("LANGUAGE_JAPANESE")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_LANGUAGE_JAPANESE_Add" runat="server" ClientIDMode="Static" Width="100px" MaxLength="30"  Text='<%#Bind("LANGUAGE_JAPANESE")%>'></asp:TextBox>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_LANGUAGE_JAPANESE_Add" runat="server" ClientIDMode="Static" Width="100px" MaxLength="30"  Text='<%#Bind("LANGUAGE_JAPANESE")%>'></asp:TextBox>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>  
                    <%--英文分數--%>
                    <asp:TemplateField HeaderText="英文分數" SortExpression="LANGUAGE_TOEIC" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_LANGUAGE_TOEIC" runat="server" Text='<%#Bind("LANGUAGE_TOEIC")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox TextMode="Number" ID="txt_LANGUAGE_TOEIC_Add" runat="server" ClientIDMode="Static" Width="100px" MaxLength="3"  CssClass=" number"  Text='<%#Bind("LANGUAGE_TOEIC")%>' onkeypress="return IsIntText();"></asp:TextBox>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox TextMode="Number" ID="txt_LANGUAGE_TOEIC_Add" runat="server" ClientIDMode="Static" Width="100px" MaxLength="3"  CssClass="number" Text='<%#Bind("LANGUAGE_TOEIC")%>'  onkeypress="return IsIntText();" ></asp:TextBox>
                            </div>
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
                                <asp:Label ID="Label2" runat="server" Text="員工編號" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="員工名稱" Width="60px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="日文分數" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="英文分數" Width="100px"></asp:Label>
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
                                    <asp:TextBox ID="txt_EMP_ID_Add" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" CssClass="MandatoryField" onkeypress="return IsIntText();"></asp:TextBox>
                                    <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID_Add', 'txt_EMP_NAME_Add', 'N');" />
                                  <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="員工編號不可為空白"
                                    ControlToValidate="txt_EMP_ID_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                </div>
                             
                            </td>                            
                            <td>
                                <div style="text-align: center; width: 100%">
                                      <asp:TextBox ID="txt_EMP_NAME_Add" MaxLength="150" ClientIDMode="Static" runat="server" Text='<%#Bind("EMP_NAME")%>'  BorderWidth="0" ></asp:TextBox>
                                </div>
                            </td>
                            <td> 
                                <div style="text-align: left; width: 100%">
                                 <asp:TextBox ID="txt_LANGUAGE_JAPANESE_Add" runat="server" ClientIDMode="Static" Width="100px" MaxLength="30" ></asp:TextBox>
                                </div>
                           
                            </td>
                            <td> 
                                
                                <div style="text-align: left; width: 100%">
                                 <asp:TextBox ID="txt_LANGUAGE_TOEIC_Add" TextMode="Number" runat="server" ClientIDMode="Static" Width="100px" MaxLength="3" CssClass="number" onkeypress="return IsIntText();"></asp:TextBox>
                                </div>
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

