<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sj/WFB2SJ0420_Upd.aspx.cs" Inherits="WebContent_WFB2SJ0420_Upd" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<%@ Register Src="~/UserControl/UCDateTextBoxRange.ascx.ascx" TagPrefix="uc1" TagName="UCDateTextBoxRangeascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        #txt_PJOB_CD {
            text-transform: uppercase;
        }
    </style>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');
            $(".year").mask('9999');
            $(".money").mask('9999999');

            if (document.getElementById("hid_HAS_MA_B").value == "Y") {
                document.getElementById("TR_MA_B_EMP").style.display = "";
            }
            $.unblockUI();

           
        }
        //儲存前檢查
        function saveCheck() {
            var processed = true;
            if (!Page_ClientValidate("GroupA")) {
                processed = false;
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
      <iframe id="dwnframe" name="dwnframe" scrolling="no"  runat="server" frameborder="0" height="0" width="0" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--頁面table--%>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="10%" />
                                <col width="10%" />
                                <col width="10%" />
                                <col width="10%" />
                                <col width="10%" />
                                <col width="10%" />
                                <col width="10%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <%-- 年度 --%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_ASSESS_YEAR" runat="server" Text="年度"></asp:Label>:</th>
                                    <td align="left" class="Body_label" >
                                         <asp:TextBox ID="txt_ASSESS_YEAR" runat="server" Width="60px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox>                            
                                         <asp:HiddenField ID="hid_ASSESS_YEAR" runat="server" ClientIDMode="Static" />
                                    </td>
                                    <%-- 類型 --%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label1" runat="server" Text="類型"></asp:Label>:</th>
                                    <td align="left" class="Body_label" >
                                        <asp:TextBox ID="txt_ASSESS_TYPE" runat="server" Width="60px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox>                            
                                        <asp:HiddenField ID="hid_ASSESS_TYPE" runat="server" ClientIDMode="Static" /> 
                                        <asp:HiddenField ID="hid_DEPT20_EMP_ID" runat="server" ClientIDMode="Static" /> 
                                        <asp:HiddenField ID="hid_MA_A_EMP_ID" runat="server" ClientIDMode="Static" /> 
                                        <asp:HiddenField ID="hid_MA_B_EMP_ID" runat="server" ClientIDMode="Static" /> 
                                        <asp:HiddenField ID="hid_MA_EMP_ID" runat="server" ClientIDMode="Static" /> 
                                        <asp:HiddenField ID="hid_MA_TYPE" runat="server" ClientIDMode="Static" /> 
                                        <asp:HiddenField ID="hid_HAS_MA_B" Value="N" runat="server" ClientIDMode="Static" /> 
                                           
                                    </td> 
                                     <%--要望考核--%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SUGGEST_SCORE" runat="server" Text="要望考核"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                          <asp:TextBox ID="txt_SUGGEST_SCORE" runat="server" Width="100px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox>
                                    </td>
                                     <th></th>
                                </tr>
                                 <tr>
                                    <%--部門名稱--%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DEPT_NAME" runat="server" Text="部門名稱"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="3">
                                           <asp:TextBox ID="txt_DEPT_NAME" runat="server" Width="250px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox>
                                    </td>
                                     
                                     <%--部門提出--%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label7" runat="server" Text="部門提出"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                          <asp:TextBox ID="txt_SCORE_DEPT" runat="server" Width="100px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox>
                                    </td>
                                     <th></th>
                                </tr>
                                 <tr>
                                    <%--工號 --%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="工號"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                         <asp:TextBox ID="txt_EMP_ID" runat="server" Width="70px" CssClass="txtDisabled"  Enabled="false" BorderWidth="0"></asp:TextBox>
                                         
                                    </td>
                                     <%-- 姓名 --%>             
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_NAME" runat="server" Text="姓名"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">  
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" Width="100px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0"  ></asp:TextBox> 
                                    </td>
                                     <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_WS_CD" runat="server" Text="職種"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">  
                                        <asp:TextBox ID="txt_WS_CD" runat="server" Width="70px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox> 
                                    </td>
                                   
                                     <th></th>
                                </tr>
                                 <tr>
                                     <%-- 資格 --%>             
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LEVEL_CD" runat="server" Text="資格"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">  
                                        <asp:TextBox ID="txt_LEVEL_CD" runat="server" Width="70px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox> 
                                    </td>
                                    
                                     <%-- 職務名稱 --%>             
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PJOB_DESC" runat="server" Text="職務名稱"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">  
                                        <asp:TextBox ID="txt_PJOB_DESC" runat="server" Width="70px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox> 
                                    </td>
                                     <th></th>
                                </tr>
                                 <tr>
                                   
                                     
                                     <th></th>
                                     <th></th>
                                     <th></th>
                                </tr>
                                 <tr>
                                    <%--年齡--%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_AGE" runat="server" Text="年齡"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                           <asp:TextBox ID="txt_AGE" runat="server" Width="30px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox>
                                    </td>
                                     <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_WORK_YEARS" runat="server" Text="入社年資"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">  
                                        <asp:TextBox ID="txt_WORK_YEARS" runat="server" Width="70px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox> 
                                    </td>
                                     <%-- 資格年資 --%>             
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_RECENT_LEVEL_WORK_YEARS" runat="server" Text="資格年資"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">  
                                        <asp:TextBox ID="txt_RECENT_LEVEL_WORK_YEARS" runat="server" Width="70px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox> 
                                    </td>
                                     <th></th>
                                </tr>
                                 <tr>
                                    <%--備註內容--%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DISTING_REMARK" runat="server" Text="備註內容"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="5">
                                           <asp:TextBox ID="txt_DISTING_REMARK" runat="server" Width="250px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox>
                                    </td>
                                     <th></th>
                                </tr>
                                <tr>
                                    <%--考核履歷--%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label2" runat="server" Text="考核履歷"  ></asp:Label>:
                                    </th>
                                    <td align="center" class="Body_TableHeader">
                                          <asp:Label ID="Label5" runat="server" Text="能力"  ></asp:Label>
                                    </td>
                                     <th align="center" class="Body_TableHeader">
                                        <asp:Label ID="Label3" runat="server" Text="業績"></asp:Label>
                                    </th>
                                    <th></th>
                                     <%-- 勤怠記錄 --%>             
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label4" runat="server" Text="勤怠記錄"></asp:Label>:
                                    </th>
                                     <th></th>
                                     <th></th>
                                </tr>
                                 <tr>
                                    <%--能力前1回--%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SCORE_1H_1" runat="server" Text="前1年"  ></asp:Label>:
                                    </th>
                                    <td align="center" class="Body_label">
                                           <asp:TextBox ID="txt_SCORE_1H_1" runat="server" Width="30px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox>
                                    </td>
                                     <th align="center" class="Body_label">
                                        <asp:TextBox ID="txt_SCORE_2H_1" runat="server" Width="30px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox> 
                                    </th>
                                    <th></th>            
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LEAVE_OP" runat="server" Text="遲/早(次)"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">  
                                        <asp:TextBox ID="txt_LEAVE_OP" runat="server" Width="70px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox> 
                                    </td>
                                     <th></th>
                                </tr>
                                 <tr>
                                    <%--能力前2回--%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SCORE_1H_2" runat="server" Text="前2年"  ></asp:Label>:
                                    </th>
                                    <td align="center" class="Body_label">
                                           <asp:TextBox ID="txt_SCORE_1H_2" runat="server" Width="30px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox>
                                    </td>
                                    <td align="center" class="Body_label">  
                                        <asp:TextBox ID="txt_SCORE_2H_2" runat="server" Width="30px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox> 
                                    </td>
                                     <th></th>      
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label6" runat="server" Text="曠職(天)"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">  
                                        <asp:TextBox ID="txt_LEAVE_Q" runat="server" Width="70px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox> 
                                    </td>
                                   
                                     <th></th>
                                </tr>
                                 <tr>
                                    <%--能力前3回--%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SCORE_1H_3" runat="server" Text="前3年"  ></asp:Label>:
                                    </th>
                                    <td align="center" class="Body_label">
                                           <asp:TextBox ID="txt_SCORE_1H_3" runat="server" Width="30px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox>
                                    </td>
                                    <td align="center" class="Body_label">  
                                        <asp:TextBox ID="txt_SCORE_2H_3" runat="server" Width="30px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox> 
                                    </td>
                                     <th></th>
                                     <%-- 職務名稱 --%>             
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LEAVE_AB" runat="server" Text="事/病假(天)"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">  
                                        <asp:TextBox ID="txt_LEAVE_AB" runat="server" Width="70px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox> 
                                    </td>
                                     <th></th>
                                </tr>
                                 <tr>
                                   
                                     <%-- 填表人 --%>             
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_CREATED_BY" runat="server" Text="填表人"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="2">  
                                        <asp:TextBox ID="txt_CREATED_BY" runat="server" Width="64px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox> 
                                        <asp:TextBox ID="txt_CREATED_NAME" runat="server" Width="100px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0"  ></asp:TextBox> 
                                    </td>
                                      <th></th> 
                                     <th></th>
                                     <th></th>
                                     <th></th>
                                </tr>
                                
                                 <tr>
                                    <%--部長簽核結果 --%>    
                                    <th align="left" class="Body_TableHeader" >
                                        <asp:Label ID="lb_AUDRESULT1_YN" runat="server" Text="經理審核"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                         <asp:TextBox ID="txt_AUDRESULT1_YN_DESC" runat="server" Width="100px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox> 
                                        <asp:DropDownList ID="ddl_AUDRESULT1_YN" runat="server" Width="100px" ClientIDMode="Static" CssClass="MandatoryField"> </asp:DropDownList>
                                    </td>
                                      <th></th>
                                     <th></th>
                                     <th></th>
                                     <th></th>
                                     <th></th>
                                </tr>
                                <tr id="TR_MA_B_EMP" style="display:none">
                                    <%--二階理事簽核結果 --%>    
                                    <th align="left" class="Body_TableHeader" >
                                        <asp:Label ID="lb_AUDRESULT2_YN" runat="server" Text="理事審核"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                         <asp:TextBox ID="txt_AUDRESULT2_YN_DESC" runat="server" Width="100px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox> 
                                        <asp:DropDownList ID="ddl_AUDRESULT2_YN" runat="server" Width="100px" ClientIDMode="Static"  CssClass="MandatoryField"> </asp:DropDownList>
                                    </td>
                                      <th></th>
                                     <th></th>
                                     <th></th>
                                     <th></th>
                                     <th></th>
                                </tr><tr>
                                    <%--協理審核結果 --%>    
                                    <th align="left" class="Body_TableHeader" >
                                        <asp:Label ID="lb_AUDRESULT3_YN" runat="server" Text="協理/理事審核"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                         <asp:TextBox ID="txt_AUDRESULT3_YN_DESC" runat="server" Width="100px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox> 
                                        <asp:DropDownList ID="ddl_AUDRESULT3_YN" runat="server" Width="100px" ClientIDMode="Static"  CssClass="MandatoryField"> </asp:DropDownList>
                                       
                                    </td>
                                      <th></th>
                                     <th></th>
                                     <th></th>
                                     <th></th>
                                     <th></th>
                                </tr>
                                 <tr>
                                    <!--備註說明-->
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_REMARK" runat="server" Text="【具體事由】"></asp:Label>:
                                    </th> 
                                     <th  colspan="6"></th>
                                </tr> 
                                <tr>
                                    
                                    <td colspan="6">
                                        <asp:TextBox TextMode="MultiLine" Rows="5" ID="txt_SUGGEST_REMARK" runat="server" Width="100%" BorderWidth="1" Style="overflow: auto" Enabled="false" ></asp:TextBox>
                                    </td>
                                    <th></th>
                                </tr>
                                 <tr>
                                    <!--附件上傳-->
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_ATTACH" runat="server" Text="附件檔案"></asp:Label>:
                                    </th> 
                                     <td  colspan="6">
                                          <asp:HiddenField ID="hid_SUGGEST_FILE_NAME" runat="server" ClientIDMode="Static" />
                                          <asp:LinkButton ID="WFB2SJ0420FileDown" runat="server" Text='' OnClick="WFB2SJ0420FileDown_Click"  Width="300px" Visible="false"></asp:LinkButton>
                                     </td>
                                </tr> 
                               <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label" colspan="3">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SJ0420Save" runat="server" Text="提出簽核"   OnClick="WFB2SJ0420Save_Click"  />
                                            <asp:Button runat="server" ID="btn_cancel" Text="返回" OnClick="btn_Cancel_Click" OnClientClick="return confirm('是否確定取消?');"/>
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
