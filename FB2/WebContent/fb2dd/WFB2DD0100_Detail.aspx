<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dd/WFB2DD0100_Detail.aspx.cs" Inherits="WebContent_fb2dd_WFB2DD0100_Detail" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $("#txt_START_DT").mask("9999/99/99");
            
            
            gridviewScroll();
            $.unblockUI();

         
        }

        //function checkConfirm() {

        //    window.location.href = "WFB2DD0100_Qry.aspx";
        //    return false;
        //    // history.back(-2);
        //}

        function gridviewScroll() {

            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "400",
                barcolor: "#7F7F7F",
                freezesize: 3
            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
        }        

        function CheckDelAction() {
            if (LookUpCheckboxs() > 0)
                return confirm($('#hidwfb299_Del_ConfirmMessage').val());
            else {
                alert($('#hidwfb299_Del_NotChoiceMessage').val());
                return false;
            }
        }

        function LookUpCheckboxs() {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }

        function CheckDelAction1() {
            
            var dt = document.all.<%= gv_result.ClientID %>;
            
            if (LookUpCheckboxs(dt.rows.length) < 0) {
                alert($('#hidwfb2dd_Del_Err_Select').val());
                return false;
            }

            if (LookUpCheckboxs(dt.rows.length) > 0)
                return confirm($('#hidwfb299_Del_ConfirmMessage').val());
            else {
                alert($('#hidwfb299_Del_NotChoiceMessage').val());
                return false;
            }
        }

        function LookUpCheckboxs1(s) {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            var perpageRow =  $("#ddlPerPageRow").val();//每頁筆數
            var pageRow = new Number(perpageRow);       
            if(s > pageRow)
            {
                s = pageRow;
            }
            var chk = 4+s;

            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    var hd = i;
                    if (i != 4 && i != chk) {
                        return -1;
                    }

                    HaveCheck++;
                }
            }
           
            return HaveCheck;
        }
        //清空畫面
        function ClearAll() {
            //$('#ddl_SYS_CD').val(-1);
        }

        function CheckSearch() {

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                return false;
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

        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }

        function IsIntText() {
            var charkeycode = window.event.keyCode;
            if (charkeycode > 47 && charkeycode < 58) {  //0的keycode為47 ,9的keycode為58
                return true;
            }
            return false;
        }

        function calculateMoney() {
            var st = '';
            if ($('#cb_SINGLE_TRIP').prop("checked")) {
                st = 'Y';
            } else {
                st = 'N';
            }        

            
            $.ajax({
                url: "WFB2DD0100_calculateMoney.ashx",
                data: {
                    FACTORY_CD: $('#ddl_FACTORY_CD').val(),
                    TRANSPORT_CD: $('#ddl_TRANSPORT_CD').val(),
                    KILOMETER_AMOUNT: $('#txt_KILOMETER_AMOUNT').val(),
                    FARE_PRICE: $('#txt_FARE_PRICE').val(),
                    SINGLE_TRIP: st
                },
                type: "GET",
                dataType: 'json',
                success: function (JData) {
                    if (JData.errMsg != "")
                        alert(JData.errMsg);
                    else {
                        //$('#lb_DAILY_PAY').text(JData.DAILY_PAY);
                        $('#txt_DAILY_PAY').val(JData.DAILY_PAY);
                        
                        
                    }
                },

                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });
            
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="15%" />
                    <col width="18%" />
                    <col width="12%" />
                    <col width="14%" />
                    <col width="15%" />
                    <col width="26%" />
                </colgroup>
                <tbody>
                    <!-- START: 1st Line in Search Criteria area -->
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_EMP_ID%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_EMP_NAME%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_NAME" runat="server" Width="180px" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="ib_WS_CD" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_WS_CD%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_WS_CD" runat="server" Width="180px" MaxLength="20" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_DEPT_NO%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" colspan="5">
                            <asp:TextBox ID="txt_DEPT_NO" runat="server" Width="500px" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_CHG_CD" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_EMP_CHG_CD%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_CHG_CD" runat="server" Width="180px" MaxLength="10" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                        </td>

                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_CD" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_EMP_CD%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_CD" runat="server" MaxLength="10" Width="160px" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_JOIN_DT" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_JOIN_DT%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_JOIN_DT" runat="server" MaxLength="10" Width="180px" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_WORK_SHIFT_CD" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_WORK_SHIFT_CD%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_WORK_SHIFT_CD" runat="server" MaxLength="10" BorderWidth="0" ReadOnly="true" Width="200px" ClientIDMode="Static"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_LEVEL_CD" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_LEVEL_CD%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_LEVEL_CD" runat="server" MaxLength="10" Width="160px" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_PJOB_CD" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_PJOB_CD%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_PJOB_CD" runat="server" MaxLength="10" Width="180px" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_AGE" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_AGE%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_AGE" runat="server" MaxLength="3" Width="200px" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_CONTACT_TEL" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_CONTACT_TEL%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_CONTACT_TEL" runat="server" MaxLength="10" BorderWidth="0" ReadOnly="true" Width="160px" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_MOBILE_TEL_1" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_MOBILE_TEL_1%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_MOBILE_TEL_1" runat="server" MaxLength="10" Width="180px" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_PLANT_CD" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_PLANT_CD%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_PLANT_CD" runat="server" Width="200px" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                            <asp:HiddenField ID="hid_PLANT_CD" runat="server" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_CONTACT_ADDR" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_CONTACT_ADDR%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" colspan="5">
                            <asp:TextBox ID="txt_CONTACT_ADDR" runat="server" MaxLength="10" Width="500px" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_REGISTER_ADDR" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_REGISTER_ADDR%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" colspan="5">
                            <asp:TextBox ID="txt_REGISTER_ADDR" runat="server" MaxLength="10" Width="500px" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_CAR_NO" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_CAR_NO%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_CAR_NO" runat="server" MaxLength="10" Width="64px" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_PARKING_TOOL" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_PARKING_TOOL%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_PARKING_TOOL" runat="server" MaxLength="10" Width="64px" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <%-- <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPLICATION_NO" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_APPLICATION_NO%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">                           
                        </td>    --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_START_DT" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_START_DT%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_START_DT" runat="server" MaxLength="10" Width="80px" class="MandatoryField" ClientIDMode="Static" CssClass="date"></asp:TextBox>

                            <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dd_ERR_START_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_START_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>

                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_FACTORY_CD" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_FACTORY_CD%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_FACTORY_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_CHG_REASON" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_CHG_REASON%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_CHG_REASON" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <%--<th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_IFLOW_NO" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_IFLOW_NO%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_IFLOW_NO" runat="server" BorderWidth="0" ClientIDMode="Static" MaxLength="10" ReadOnly="true" Width="64px"></asp:TextBox>
                        </td>  --%>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_AREA_CD" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_AREA_CD%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_AREA_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_TRANSPORT_CD" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_TRANSPORT_CD%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_TRANSPORT_CD" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddl_TRANSPORT_CD_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            <asp:HiddenField ID="hid_TRANSPORT_MONEY" runat="server" ClientIDMode="Static" />
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_LINE_CD" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_LINE_CD%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_LINE_CD" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddl_LINE_CD_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            <asp:DropDownList ID="ddl_STATION_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_KILOMETER_AMOUNT" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_KILOMETER_AMOUNT%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_KILOMETER_AMOUNT" runat="server" MaxLength="3" Width="64px" ClientIDMode="Static" onkeypress="return IsIntText();" onpaste="return false"></asp:TextBox>
                            <asp:HiddenField ID="hid_CAR_KM" runat="server" ClientIDMode="Static" />
                            <asp:HiddenField ID="hid_MOTO_KM" runat="server" ClientIDMode="Static" />
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_FARE_PRICE" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_FARE_PRICE%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_FARE_PRICE" runat="server" MaxLength="3" Width="64px" ClientIDMode="Static" onkeypress="return IsIntText();" onpaste="return false"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_SINGLE_TRIP" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_SPECIAL%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:CheckBox ID="cb_SINGLE_TRIP" runat="server" ClientIDMode="Static" Text="<%$Resources:Resource,wfb2dd_lb_SINGLE_TRIP%>" />
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_label">
                            <%--                             <asp:Button ID="dailyPay" runat="server" Text="<%$Resources:Resource,wfb2dd_WFB2DD0101Cal_DailyPay%>" Visible="false" OnClientClick="calculateMoney();" />
                            --%>

                            <input id="dailyPay" type="button" runat="server" onclick="calculateMoney();" value="<%$Resources:Resource,wfb2dd_WFB2DD0101Cal_DailyPay%>" />
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_DAILY_PAY" runat="server" MaxLength="10" Width="64px" ReadOnly="true" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                        </td>

                    </tr>
                    <tr>
                        <%--<th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_DAILY_PAY" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_DAILY_PAY%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">                           
                        </td>    --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_REMARK%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" colspan="3">
                            <asp:TextBox ID="txt_REMARK" runat="server" MaxLength="150" Width="500px" ClientIDMode="Static"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_ADDRESS" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_ADDRESS%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" colspan="5">
                            <asp:TextBox ID="txt_ADDRESS" runat="server" MaxLength="150" Width="500px" ClientIDMode="Static"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_IS_CALCULATE" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_IS_CALCULATE_t%>" Width="160px"></asp:Label></th>
                        <td align="left" class="Body_label">
                            <asp:RadioButtonList ID="rb_IS_CALCULATE" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1">是</asp:ListItem>
                                <asp:ListItem Value="0">否</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_IS_CANCEL" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_IS_CANCEL_t%>" Width="120px"></asp:Label></th>
                        <td align="left" class="Body_label">
                            <asp:RadioButtonList ID="rb_IS_CANCEL" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal">
                                <asp:ListItem Value="Y">是</asp:ListItem>
                                <asp:ListItem Value="N">否</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="Body_label" colspan="10">
                            <div id="init">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" height="1" colspan="10">
                            <hr>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="10">
                        <aces:Btn ID="WFB2DD0101Add" runat="server" Text="<%$Resources:Resource,wfb2dd_WFB2DD0101Add%>" Visible="true" OnClick="WFB2DD0101Add_Click" />
                            <aces:Btn ID="WFB2DD0101EditInsert" runat="server" Text="<%$Resources:Resource,wfb2dd_WFB2DD0101EditInsert%>" Visible="true" OnClick="WFB2DD0101EditInsert_Click" />
                            <aces:Btn ID="WFB2DD0101Delete" runat="server" Text="<%$Resources:Resource,wfb2dd_WFB2DD0101Delete%>" Visible="true" OnClick="WFB2DD0101Delete_Click" OnClientClick="return CheckDelAction();" />
                            <aces:Btn ID="WFB2DD0101Save" runat="server" Text="<%$Resources:Resource,wfb2dd_WFB2DD0101Save%>" OnClientClick="return saveCheck();" Visible="false" OnClick="WFB2DD0101Save_Click" Style="height: 21px" />

<%--                            <asp:Button ID="WFB2DD0101Add" runat="server" Text="<%$Resources:Resource,wfb2dd_WFB2DD0101Add%>" Visible="true" OnClick="WFB2DD0101Add_Click" />
                            <asp:Button ID="WFB2DD0101EditInsert" runat="server" Text="<%$Resources:Resource,wfb2dd_WFB2DD0101EditInsert%>" Visible="true" OnClick="WFB2DD0101EditInsert_Click" />
                            <asp:Button ID="WFB2DD0101Delete" runat="server" Text="<%$Resources:Resource,wfb2dd_WFB2DD0101Delete%>" Visible="true" OnClick="WFB2DD0101Delete_Click" OnClientClick="return CheckDelAction();" />
                            <asp:Button ID="WFB2DD0101Save" runat="server" Text="<%$Resources:Resource,wfb2dd_WFB2DD0101Save%>" OnClientClick="return saveCheck();" Visible="false" OnClick="WFB2DD0101Save_Click" Style="height: 21px" />--%>
                            
                            <asp:Button ID="WFB2DD0101Cancel" runat="server" Text="<%$Resources:Resource,wfb2dd_WFB2DD0101Cancel%>" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="WFB2DD0101Cancel_Click" />
                            <asp:Button ID="btn_back1" runat="server" Text="<%$Resources:Resource,wfb2dd_btn_back%>" OnClick="btn_back1_Click" />
                        </td>
                    </tr>
                    <!-- END: Create a line -->
                </tbody>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData1"
                SelectCountMethod="getCount1" TypeName="CFB2DD0100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />                    
                    <asp:ControlParameter ControlID="hid_EMP_ID" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="emp_id" PropertyName="Value" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1200px" 
                OnPageIndexChanging="gv_result_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" meta:resourcekey="cb_checkallResource1" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" meta:resourcekey="cb_checkResource1" ClientIDMode="AutoID" />
                        </ItemTemplate>
                        <EditItemTemplate>

                            <asp:CheckBox ID="cb_check" runat="server" Checked="true" />

                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2dd_RowNumber%>" SortExpression="RowNumber" HeaderStyle-Width="40px" ItemStyle-Width="40px" />
                    <asp:BoundField DataField="APPLICATION_NO" HeaderText="<%$Resources:Resource,wfb2dd_lb_APPLICATION_NO%>" SortExpression="APPLICATION_NO" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="left" />
                    <asp:BoundField DataField="START_DT" HeaderText="<%$Resources:Resource,wfb2dd_lb_START_DT%>" SortExpression="START_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="left" />
                    <asp:BoundField DataField="END_DT" HeaderText="<%$Resources:Resource,wfb2dd_lb_END_DT%>" SortExpression="END_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="left" />
                    <asp:BoundField DataField="IFLOW_NO" HeaderText="<%$Resources:Resource,wfb2dd_lb_IFLOW_NO%>" SortExpression="IFLOW_NO" HeaderStyle-Width="150px" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="left" />
                    <asp:BoundField DataField="CHG_REASON_DESC" HeaderText="<%$Resources:Resource,wfb2dd_lb_CHG_REASON%>" SortExpression="CHG_REASON" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="left" />
                    <asp:BoundField DataField="IS_CANCEL" HeaderText="<%$Resources:Resource,wfb2dd_lb_IS_CANCEL%>" SortExpression="IS_CANCEL" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="left" />
                    <asp:BoundField DataField="IS_CALCULATE" HeaderText="<%$Resources:Resource,wfb2dd_lb_IS_CALCULATE%>" SortExpression="IS_CALCULATE" HeaderStyle-Width="150px" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="left" />
                    <asp:BoundField DataField="FACTORY_DESC" HeaderText="<%$Resources:Resource,wfb2dd_lb_FACTORY_CD%>" SortExpression="FACTORY_CD" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="left" />
                    <asp:BoundField DataField="AREA_DESC" HeaderText="<%$Resources:Resource,wfb2dd_lb_AREA_CD%>" SortExpression="AREA_CD" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="left" />
                    <asp:BoundField DataField="TRANSPORT_DESC" HeaderText="<%$Resources:Resource,wfb2dd_lb_TRANSPORT_CD%>" SortExpression="TRANSPORT_CD" HeaderStyle-Width="150px" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="left" />
                    <asp:BoundField DataField="LINE_DESC" HeaderText="<%$Resources:Resource,wfb2dd_lb_LINE_CD%>" SortExpression="LINE_CD" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="left" />
                    <asp:BoundField DataField="KILOMETER_AMOUNT" HeaderText="<%$Resources:Resource,wfb2dd_lb_KILOMETER_AMOUNT%>" SortExpression="KILOMETER_AMOUNT" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:n0}" />
                    <asp:BoundField DataField="FARE_PRICE" HeaderText="<%$Resources:Resource,wfb2dd_lb_FARE_PRICE%>" SortExpression="FARE_PRICE" HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:n0}" />
                    <asp:BoundField DataField="SINGLE_TRIP" HeaderText="<%$Resources:Resource,wfb2dd_lb_SINGLE_TRIP%>" SortExpression="SINGLE_TRIP" HeaderStyle-Width="80px" ItemStyle-Width="80px" />
                    <asp:BoundField DataField="DAILY_PAY" HeaderText="<%$Resources:Resource,wfb2dd_lb_DAILY_PAY%>" SortExpression="DAILY_PAY" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:n0}" />
                    <asp:BoundField DataField="ADDRESS" HeaderText="<%$Resources:Resource,wfb2dd_lb_ADDRESS%>" SortExpression="ADDRESS" HeaderStyle-Width="250px" ItemStyle-Width="250px" ItemStyle-HorizontalAlign="left" />
                    <asp:BoundField DataField="REMARK" HeaderText="<%$Resources:Resource,wfb2dd_lb_REMARK%>" SortExpression="REMARK" HeaderStyle-Width="250px" ItemStyle-Width="250px" ItemStyle-HorizontalAlign="left" />
                    <asp:BoundField DataField="STATION_CD" />
                    <asp:BoundField DataField="CHG_REASON" />
                    <asp:BoundField DataField="FACTORY_CD" />
                    <asp:BoundField DataField="AREA_CD" />
                    <asp:BoundField DataField="TRANSPORT_CD" />
                    <asp:BoundField DataField="LINE_CD" />
                </Columns>                
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle HorizontalAlign="Center" />                
            </asp:GridView>
            
           
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_EMP_ID" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Del_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Del_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2dd_Del_Err_Select" Value="<%$Resources:Resource,wfb2dd_Del_Err_SelectMessage%>" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
