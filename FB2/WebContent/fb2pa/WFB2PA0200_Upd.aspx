<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2pa/WFB2PA0200_Upd.aspx.cs" Inherits="WebContent_WFB2PA0200_Upd" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<%@ Register Src="~/UserControl/UCDateTextBoxRange.ascx.ascx" TagPrefix="uc1" TagName="UCDateTextBoxRangeascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
      
    </style>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });

        function iniForm() {
           
            $(".date").datepicker({ dateFormat: 'yy/mm' });
            $(".date").mask('9999/99');
            $(".year").mask('9999');
            $(".number").mask('999999');
            $(".number").css("text-align", "right");
            $(".txtDisabled").css("background-color", "white").css("color", "black").css("border-width", "0");
            $.unblockUI();
            
            //核定分數取得的ajax
            $("#txt_BONUS_SCR_FINAL").change(function () {
               
                if ($("#txt_BONUS_SCR_FINAL").val().length <= 3 && $("#txt_BONUS_SCR_FINAL").val() != "") {
                  
                    $.ajax({
                        url: "WFB2PA0200_GetEVASetByScore.ashx",
                        data: {
                            SCORE: $('#txt_BONUS_SCR_FINAL').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        cache: false,
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_BONUS_SCR_FINAL').val($('#hid_O_BONUS_SCR_FINAL').val());
                                alert(JData.errMsg);
                            }
                            else {
                                if ($('#hid_IS_YN').val() == "N" && JData.TRANS_KEEP_YN == "Y") {
                                    $('#txt_BONUS_SCR_FINAL').val($('#hid_O_BONUS_SCR_FINAL').val().replace(".0",""));
                                    $('#txt_SALARY_YM').val($('#hid_O_SALARY_YM').val());
                                    $('#hid_IS_YN').val($('#hid_O_IS_YN').val());
                                    $('#txt_IS_YN_DESC').val($('#hid_O_IS_YN_DESC').val());
                                    $('#txt_GRADE_CD').val($('#hid_O_GRADE_CD').val());
                                    $('#txt_GRADE_NAME').val($('#hid_O_GRADE_NAME').val());
                                    $('#txt_GROUP_INTEGRAL').val($('#hid_O_GROUP_INTEGRAL').val());
                                    $('#txt_PRO_BONUS').val($('#hid_O_PRO_BONUS').val());

                                    alert("原始分數為「不保留」,輸入之核定分數為「保留」,不允輸入此分數。");
                                    return;
                                } else {
                                    console.log(JData);
                                    $('#hid_IS_YN').val(JData.TRANS_KEEP_YN);
                                    $('#txt_IS_YN_DESC').val(JData.TRANS_KEEP_YN_DESC);
                                    $('#txt_GRADE_CD').val(JData.GRADE_CD);
                                    $('#txt_GRADE_NAME').val(JData.GRADE_NAME);
                                    $('#txt_GROUP_INTEGRAL').val(JData.GROUP_POINT);
                                    $('#txt_PRO_BONUS').val(JData.BONUS_AMT);
                                    if ($('#hid_IS_YN').val() == "Y") {
                                        $('#txt_SALARY_YM').val("");
                                    }

                                }
                              
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    //$('#txt_EMP_NAME').val("");
                }
            });
           
           
        }
        //儲存前檢查
        function saveCheck() {
            var processed = true;
            if ($("#txt_SALARY_YM").val() != "") { 
                if ($("#txt_SALARY_YM").val().replace("/","")<= $("#hid_LAST_YM").val().replace("/","")) {
                    alert("核發年月,不允小於:" + $("#hid_LAST_YM").val());
                    return false;
                }
            }
            if (confirm("確定要儲存 ?")) {
            
                return true;
            
            } else {
                return false;
            }
            if (processed)
                BlockUI();
            if (!processed)
                $.unblockUI();
            return processed;
        }
       

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--頁面table--%>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="10%" />
                                <col width="18%" />
                                <col width="15%" />
                                <col width="18%" />
                                <col width="10%" />
                                <col width="10%" />
                                <col width="10%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                      <%-- 提案條碼 --%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BARCODE_NO" runat="server" Text="提案條碼"></asp:Label>:</th>
                                    <td align="left" class="Body_label" >
                                        <asp:TextBox ID="txt_BARCODE_NO" runat="server"  CssClass="txtDisabled" Enabled="false" BorderWidth="0" ></asp:TextBox>
                                                                         
                                    </td>
                                    <%-- 員工編號 --%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="員工編號"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                         <asp:TextBox ID="txt_EMP_ID" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0" Width="40px"></asp:TextBox>
                                         <asp:TextBox ID="txt_EMP_NAME" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0" Width="90px"></asp:TextBox>    
                                                                                        
                                    </td>
                                      <%-- 提案年月 --%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label1" runat="server" Text="提案年月"></asp:Label>:</th>
                                    <td align="left" class="Body_label" >
                                        <asp:TextBox ID="txt_YM" runat="server"  CssClass="txtDisabled date" Enabled="false" BorderWidth="0" ></asp:TextBox>
                                </tr>
                                <tr>
                                      <%-- 原始分數 --%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BONUS_SCR_FIRST" runat="server" Text="原始分數"></asp:Label>:</th>
                                    <td align="left" class="Body_label" >
                                        <asp:TextBox ID="txt_BONUS_SCR_FIRST" runat="server"  CssClass="txtDisabled number" Enabled="false" BorderWidth="0" Width="90px"></asp:TextBox>
                                    </td>
                                    <%-- 核定分數 --%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_BONUS_SCR_FINAL" runat="server" Text="核定分數"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                         <asp:HiddenField ID="hid_O_BONUS_SCR_FINAL" runat="server" ClientIDMode="Static" />        
                                         <asp:TextBox ID="txt_BONUS_SCR_FINAL" runat="server" CssClass="MandatoryField number" ClientIDMode="Static" maxlength="3" Width="90px"></asp:TextBox>                                                                                           
                                    </td>
                                    <%-- 核發年月 --%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label2" runat="server" Text="核發年月"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                         <asp:TextBox ID="txt_SALARY_YM" runat="server" CssClass="MandatoryField date"  ClientIDMode="Static" maxlength="7" Width="90px"></asp:TextBox>  
                                            <asp:HiddenField ID="hid_O_SALARY_YM" runat="server" ClientIDMode="Static" />                                                                                                 
                                    </td>
                                    
                                </tr>
                                 <tr>
                                     <td colspan="6"></td>
                                 </tr>
                                 <tr>
                                    <%--獎金保留 --%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_IS_YN" runat="server" Text="獎金保留"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" >
                                        <asp:TextBox ID="txt_IS_YN_DESC" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0" ClientIDMode="Static" Width="90px"></asp:TextBox>
                                         <asp:HiddenField ID="hid_IS_YN" runat="server" ClientIDMode="Static" />  
                                         <asp:HiddenField ID="hid_O_IS_YN" runat="server" ClientIDMode="Static" />   
                                         <asp:HiddenField ID="hid_O_IS_YN_DESC" runat="server" ClientIDMode="Static" />      
                                         <asp:HiddenField ID="hid_LAST_YM" runat="server" ClientIDMode="Static" />               
                                    </td>
                                    <%--等級 --%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label3" runat="server" Text="等級"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" >
                                        <asp:TextBox ID="txt_GRADE_CD" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0" ClientIDMode="Static" Width="40px"></asp:TextBox>
                                        <asp:TextBox ID="txt_GRADE_NAME" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0" ClientIDMode="Static" Width="90px"></asp:TextBox>
                                         <asp:HiddenField ID="hid_O_GRADE_CD" runat="server" ClientIDMode="Static" />
                                         <asp:HiddenField ID="hid_O_GRADE_NAME" runat="server" ClientIDMode="Static" />
                                    </td>
                                </tr>
                                <tr>
                                    <%--核定獎金 --%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_PRO_BONUS" runat="server" Text="核定獎金"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" >
                                        <asp:TextBox ID="txt_PRO_BONUS" runat="server" CssClass="txtDisabled number" Enabled="false" BorderWidth="0" ClientIDMode="Static" Width="90px"></asp:TextBox>
                                         <asp:HiddenField ID="hid_O_PRO_BONUS" runat="server" ClientIDMode="Static" />
                                    </td>
                                    <%--團體計點 --%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_GROUP_INTEGRAL" runat="server" Text="團體計點"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" >
                                        <asp:TextBox ID="txt_GROUP_INTEGRAL" runat="server" CssClass="txtDisabled number" Enabled="false" BorderWidth="0" ClientIDMode="Static" Width="90px"></asp:TextBox>
                                         <asp:HiddenField ID="hid_O_GROUP_INTEGRAL" runat="server" ClientIDMode="Static" />
                                    </td>
                                </tr>
                               <tr>
                                    <th></th>
                                    <td align="right" class="Body_label" colspan="3">
                                        <div id="init">
                                            <aces:Btn ID="WFB2PA0200Save" runat="server" Text="儲存"  OnClientClick="return saveCheck();" OnClick="WFB2PA0200Save_Click"  />
                                            <asp:Button runat="server" ID="btn_cancel" Text="取消" OnClick="btn_Cancel_Click" OnClientClick="return confirm('是否確定取消?');"/>
                                        </div>
                                    </td>
                                </tr>
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
                        </div>
                    </td>
                </tr>
            </table>
             <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
