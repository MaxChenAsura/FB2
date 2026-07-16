<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2ha/WFB2HA0600_PRIV2.aspx.cs" Inherits="WebContent_fb2ha_WFB2HA0600_PRIV2" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <style type="text/css">
        #txt_HR_CHG_CD {
            text-transform: uppercase;
        }

        #txt_HR_CHG_CD_Add {
            text-transform: uppercase;
        }
    </style>


    <script type="text/javascript">


        jQuery(document).ready(function () {

            iniForm();

        });


        function iniForm() {
            $("#txt_HR_CHG_DESC_Add,#txt_HR_CHG_ITEM_DESC_Add,#txt_HR_CHG_DESC").css("color", "black").css("background-color", "white").attr("disabled", true);
            $(".number").mask('9999/99');
            gridviewScroll();
            $.unblockUI();
            $('#txt_HR_CHG_CD').change(function () {
                if ($('#txt_HR_CHG_CD').val().length == 3) {
                    $.ajax({
                        url: "../commgeo/WFB2ChangeCode_Search.ashx",
                        data: {
                            HR_CHG_CD: $('#txt_HR_CHG_CD').val()
                        },
                        type: "GET",
                        cache: false,
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_HR_CHG_DESC').val("");
                            }
                            else {
                                $('#txt_HR_CHG_CD').val(JData.HR_CHG_CD_A);
                                $('#txt_HR_CHG_DESC').val(JData.HR_CHG_DESC);

                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_HR_CHG_DESC').val("");
                }
            });
            $('#txt_HR_CHG_CD_Add').change(function () {
                if ($('#txt_HR_CHG_CD_Add').val().length == 3) {
                    var regex = new RegExp("^[a-zA-Z0-9]+$");
                    var str = $('#txt_HR_CHG_CD_Add').val();
                    if (regex.test(str)) {
                        //ajax 取得薪資
                        $.ajax({
                            url: "../commgeo/WFB2ChangeCode_Search.ashx",
                            data: {
                                HR_CHG_CD: $('#txt_HR_CHG_CD_Add').val()
                            },
                            type: "GET",
                            cache: false,
                            dataType: 'json',
                            success: function (JData) {
                                if (JData.errMsg != "") {
                                    alert(JData.errMsg);
                                    $('#txt_HR_CHG_CD_Add').val("");
                                    $('#txt_HR_CHG_DESC_Add').val("");
                                }
                                else {
                                    $('#txt_HR_CHG_CD_Add').val(JData.HR_CHG_CD_A);
                                    $('#txt_HR_CHG_DESC_Add').val(JData.HR_CHG_DESC);
                                }
                            },
                            error: function (xhr, ajaxOptions, thrownError) {
                                alert(xhr.status);
                                alert(thrownError);
                            }
                        });
                    } else {
                        alert($("#hidwfb2_alphanumeric_onlyMessage").val());
                        $('#txt_HR_CHG_CD_Add').val("");
                        $('#txt_HR_CHG_DESC_Add').val("");
                        return false;

                    }
                } else {
                    $('#txt_HR_CHG_DESC_Add').val("");
                }
            });
            $('#ddl_HR_CHG_ITEM_Add').change(function () {
                $.ajax({
                    url: "../commgeo/WFB2GetCommData.ashx",
                    data: {
                        SUB_CD: $("#ddl_HR_CHG_ITEM_Add option:selected").val()
                        , SYS_CD: 'HC'
                        , MAIN_CD: 'HR_CHG_ITEM'
                    },
                    type: "GET",
                    cache: false,
                    dataType: 'json',
                    success: function (JData) {
                        if (JData.errMsg != "") {
                            alert(JData.errMsg);
                            $('#txt_HR_CHG_ITEM_DESC_Add').val("");
                        }
                        else {
                            $('#txt_HR_CHG_ITEM_DESC_Add').val(JData.SUB_DESC);
                        }
                    },

                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
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
        function ClearAll() {
            $('#txt_HR_CHG_CD').val("");
            $('#txt_HR_CHG_DESC').val("");
            $('#ddl_HR_CHG_ITEM').val(-1);
        }

        function CheckMODE_ID(source, arguments) {
            var re = /^[\d|a-zA-Z]+$/;
            if (!re.test($("#txt_MODE_ID_Add").val()))
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }

        function CheckModeifyAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidWfb2ha_Mod_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckDtlAction() {
            var processed = true;
            if (LookUpCheckboxs() > 0) {
                processed = true;
            } else {
                alert($('#hidwfb2_Dtl_NotChoiceMessage').val());
                processed = false;
                return processed;
            }
            if (confirm($('#hidwfb2_Del_ConfirmMessage').val())) {
                processed = true;
                $.unblockUI();
            } else {
                processed = false;
            }
            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }
        //儲存前檢查
        function saveCheck() {
            var processed = true;


            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                processed = false;
            if (!processed)
                $.unblockUI();

            return processed;
        }


    </script>

    <style type="text/css">
        .auto-style1 {
            width: 15%;
        }

        #init {
            width: 617px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!--2990400_QRY開始-->
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="25%" />
                                <col width="35%" />
                                <col width="25%" />
                                <col width="30%" />
                            </colgroup>
                            <tbody>
                                <caption>

                                    <tr>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lbl_HR_CHG_CD" runat="server" Text="">人事異動代碼</asp:Label>:
                                        </th>
                                        <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_HR_CHG_CD" runat="server" ClientIDMode="Static" MaxLength="3" Width="46px"></asp:TextBox>
                                            <input id="btn_HR_CHG_CD" type="button" value="..." onclick="OpenSearch('HrChangeCode_Search.aspx', 'txt_HR_CHG_CD', 'txt_HR_CHG_DESC', '');" />
                                            <asp:TextBox ID="txt_HR_CHG_DESC" runat="server" BorderWidth="0" ClientIDMode="Static" Width="100px"></asp:TextBox>
                                        </td>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lbl_HR_CHG_ITEM" runat="server" Text="">人事異動項目:</asp:Label>
                                        </th>
                                        <td align="left" class="Body_label">
                                            <asp:DropDownList ID="ddl_HR_CHG_ITEM" runat="server" ClientIDMode="Static">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th></th>
                                        <th></th>
                                        <th class="auto-style1"></th>
                                        <td align="right" class="Body_label">
                                            <div id="init">
                                                <aces:Btn ID="WFB2HA0600Search" runat="server" OnClick="WFB2HA0600Search_Click" OnClientClick="BlockUI();" Text="<%$Resources:Resource,btn_Search%>" />

                                                <%--<asp:Button ID="WFB2HA0600Search" runat="server" OnClick="WFB2HA0600Search_Click" OnClientClick="BlockUI();" Text="<%$Resources:Resource,btn_Search%>" />--%>
                                                <input id="WFB2HA0600Clear" type="button" value="<%$Resources:Resource,btn_clear%>" runat="server" onclick="ClearAll();" />
                                                <asp:Button ID="WFB2HA0600_BACK" runat="server" OnClick="WFB2HA0600_BACK_Click" Text="<%$Resources:Resource,btn_back%>" />
                                            </div>
                                        </td>
                                    </tr>

                                </caption>
                            </tbody>
                        </table>

                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>

                    <td align="right" class="Body_label">
                        <div id="init_grid">
                            <aces:Btn ID="WFB2HA0600Add" runat="server" Text="<%$Resources:Resource,btn_Add%>" OnClick="WFB2HA0600Add_Click" />
                            <aces:Btn ID="WFB2HA0600Delete" runat="server" Text="<%$Resources:Resource,btn_Delete%>" OnClientClick="return CheckDtlAction();" OnClick="WFB2HA0600Delete_Click" Visible="False" />
                            <aces:Btn ID="WFB2HA0600Save" runat="server" Text="<%$Resources:Resource,btn_Save%>" Visible="false" OnClick="WFB2HA0600Save_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />

                            <%--                            
                            <asp:Button ID="WFB2HA0600Add" runat="server" Text="<%$Resources:Resource,btn_Add%>" OnClick="WFB2HA0600Add_Click" />
                            <asp:Button ID="WFB2HA0600Delete" runat="server" Text="<%$Resources:Resource,btn_Delete%>" OnClientClick="return CheckDtlAction();" OnClick="WFB2HA0600Delete_Click" Visible="False" />
                            <asp:Button ID="WFB2HA0600Save" runat="server" Text="<%$Resources:Resource,btn_Save%>" Visible="false" OnClick="WFB2HA0600Save_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />--%>

                            <asp:Button ID="WFB2HA0600Cancel" runat="server" Text="<%$Resources:Resource,btn_Cancel%>" Visible="false" OnClick="WFB2HA0600Cancel_Click" OnClientClick="return confirm('是否確定取消?');" />


                        </div>

                    </td>
                </tr>
            </table>
            <!--2990400_QRY結束-->
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="Cfb2ha0600_PRIV2DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_HR_CHG_CD"
                        Name="HR_CHG_CD" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_HR_CHG_ITEM"
                        Name="HR_CHG_ITEM" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />

                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1024px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" meta:resourcekey="cb_checkallResource1" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" meta:resourcekey="cb_checkResource1" ClientIDMode="AutoID" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div>
                                <asp:CheckBox ID="cb_check" runat="server" Checked="true" />
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2ib_RowNumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <div>
                                <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,Wfb2ha_HR_CHG_CD%>" SortExpression="HR_CHG_CD">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_HR_CHG_CD" runat="server" Text='<%#Bind("HR_CHG_CD")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:TextBox ID="txt_HR_CHG_CD_Add" runat="server" Text='<%#Bind("HR_CHG_CD")%>'></asp:TextBox>
                            </div>
                        </EditItemTemplate>

                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:TextBox ID="txt_HR_CHG_CD_Add" runat="server" MaxLength="3" Width="64px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                <input id="btn_HR_CHG_CD" type="button" value="..." onclick="OpenSearch('HrChangeCode_Search.aspx', 'txt_HR_CHG_CD_Add', 'txt_HR_CHG_DESC_Add', '');" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,Wfb2ha_HR_CHG_CD_Required%>"
                                    ControlToValidate="txt_HR_CHG_CD_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,Wfb2ha_HR_CHG_DESC%>" SortExpression="HR_CHG_DESC">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:Label ID="lbl_HR_CHG_DESC" runat="server" Text='<%#Bind("HR_CHG_B")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:TextBox ID="txt_HR_CHG_DESC_Add" runat="server" Text='<%#Bind("HR_CHG_B")%>' BorderWidth="0"></asp:TextBox>
                            </div>
                        </EditItemTemplate>

                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_HR_CHG_DESC_Add" runat="server" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,Wfb2ha_HR_CHG_ITEM%>" SortExpression="HR_CHG_ITEM">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="lbl_HR_CHG_ITEM" runat="server" Text='<%#Bind("HR_CHG_ITEM")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:DropDownList ID="ddl_HR_CHG_ITEM_Add" runat="server" ClientIDMode="Static">
                                </asp:DropDownList>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:DropDownList ID="ddl_HR_CHG_ITEM_Add" runat="server" ClientIDMode="Static" CssClass="MandatoryField">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,Wfb2ha_HR_CHG_ITEM_Required%>"
                                    ControlToValidate="ddl_HR_CHG_ITEM_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                </asp:RequiredFieldValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,Wfb2ha_HR_CHG_ITEM_DESC%>" SortExpression="HR_CHG_ITEM_DESC">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_HR_CHG_ITEM_DESC" runat="server" Text='<%#Bind("HR_CHG_ITEM_DESC")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:TextBox ID="txt_HR_CHG_ITEM_DESC_Add" runat="server" Text='<%#Bind("HR_CHG_ITEM_DESC")%>'></asp:TextBox>
                            </div>
                        </EditItemTemplate>

                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_HR_CHG_ITEM_DESC_Add" runat="server" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
                <EmptyDataTemplate>
                    <table class="grid-view" width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                        <tr class="header">

                            <td width="250px">
                                <asp:Label ID="lblHeaderHR_CHG_CD" runat="server" Text="<%$Resources:Resource,Wfb2ha_HR_CHG_CD%>"></asp:Label>
                            </td>
                            <td width="250px">
                                <asp:Label ID="lblHeaderHR_CHG_DESC" runat="server" Text="<%$Resources:Resource,Wfb2ha_HR_CHG_DESC%>"></asp:Label>
                            </td>
                            <td width="250px">
                                <asp:Label ID="lblHeaderHR_CHG_ITEM" runat="server" Text="<%$Resources:Resource,Wfb2ha_HR_CHG_ITEM%>"></asp:Label>
                            </td>
                            <td width="250px">
                                <asp:Label ID="lblHeaderHR_CHG_ITEM_DESC" runat="server" Text="<%$Resources:Resource,Wfb2ha_HR_CHG_ITEM_DESC%>"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">

                            <td>
                                <div style="text-align: center; width: 100%;">
                                    <asp:TextBox ID="txt_HR_CHG_CD_Add" ClientIDMode="Static" runat="server" MaxLength="3" CssClass="MandatoryField" Text='<%#Bind("ACC_DEPT_NAME")%>'></asp:TextBox>
                                    <input id="btn_HR_CHG_CD" type="button" value="..." onclick="OpenSearch('HrChangeCode_Search.aspx', 'txt_HR_CHG_CD', 'txt_HR_CHG_DESC', 'HR_CHG_CD=B&UPD_RIGHT_CD=D&IS_FOR_BATCH=Y&IS_FOR_TRANSFER_IN=3&IS_VALID=Y&EMP_ID=10003');" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,Wfb2ha_HR_CHG_CD_Required%>"
                                        ControlToValidate="txt_HR_CHG_CD_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                    </asp:RequiredFieldValidator>

                                </div>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:TextBox ID="txt_HR_CHG_DESC_Add" runat="server" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:DropDownList ID="ddl_HR_CHG_ITEM_Add" MaxLength="2" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,Wfb2ha_HR_CHG_ITEM_Required%>"
                                        ControlToValidate="ddl_HR_CHG_ITEM_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:TextBox ID="txt_HR_CHG_ITEM_DESC_Add" runat="server" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                </div>
                            </td>

                            </div>
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
            </asp:GridView>
            <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">
                    <td class="GridviewScrollPager TD">
                        <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static">
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

            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2_alphanumeric_onlyMessage" Value="<%$Resources:Resource,wfb2_alphanumeric_onlyMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb2_Dtl_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2_Dtl_NotChoiceMessage" Value="<%$Resources:Resource,wfb2_Dtl_NotChoiceMessage%>" />

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


