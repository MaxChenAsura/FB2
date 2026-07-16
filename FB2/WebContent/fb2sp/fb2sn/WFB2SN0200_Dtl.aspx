<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sn/WFB2SN0200_Dtl.aspx.cs" Inherits="WebContent_WFB2SN0200_Dtl" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        $(function () {

            iniForm();
        });
        
        function iniForm() {
          
            $.unblockUI();
            gridviewScroll();
        };

        
        function ShowRecord(obj) {
           
            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            
        };

        //凍結視窗用
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "400",
                barcolor: "#7F7F7F"              

            });
        }

        var choietr = null;
        //回傳目前Checkbox被勾選的數量
        function checkboxsSelected() {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            $(":checkbox:checked").each(function () {
                HaveCheck++;
            });
            /*
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                    if (choietr == null) {
                        choietr = i;
                    }
                }
            }
            */
            return HaveCheck;
        };


        //核可,檢查是否有勾選
        function checkApprove() {
            var processed = true;
            BlockUI();
            //var remark = $.trim($("textarea[id $= 'txt_REMARK']").val());
            //if (checkboxsSelected() > 0) {
            //    alert("請取消異常註記!");
            //    processed = false;
            //} else {
            processed = confirm("確定要核可?");
            //}

            if (!processed) {
                $.unblockUI();
            }
            return processed;

        };

        //駁回,檢查是否有勾選
        function checkReject() {
            var processed = true;
            BlockUI();
            var remark = $.trim($("textarea[id $= 'txt_REMARK']").val());
            if (remark == "" /* && checkboxsSelected() == "0"*/) {
                alert("請輸入備註說明!");
                processed = false;
            } else {
                processed = confirm("確定要駁回?");
            }

            if (!processed) {
                $.unblockUI();
            }
            return processed;

        };
        //年獎相關資料下載
        function checkDowning(msg) {
            var processed = true;
            BlockUI();
            processed = confirm("確定要進行" + msg);
            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }
        //清空
        function doClear() {
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            return false;
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
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="15%" />
                    <col width="20%" />
                    <col width="15%" />
                    <col width="20%" />
                    <col width="10%" />
                    <col width="15%" />
                    <col width="5%" />
                </colgroup>
                <tbody>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <!--阿法值對象-->
                            <asp:Label ID="Label" runat="server" Text="<%$Resources:Resource,wfb2sn_lbl_AFA_FOR%>"></asp:Label>:
                        </th>
                        <td>
                            <asp:TextBox ID="txt_AFA_FOR" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:TextBox>
                        </td>                       
                    </tr>
                    <tr>                       
                        <th align="left" class="Body_TableHeader">
                            <!--阿法值發放日期-->
                            <asp:Label ID="lb_AFA_AWARD_DT" runat="server" Text="<%$Resources:Resource,wfb2sn_lb_afa_award_dt%>"></asp:Label>:
                        </th>
                        <td>
                            <asp:TextBox ID="txt_AFA_AWARD_DT" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <!--阿法值總金額-->
                            <asp:Label ID="lb_AFA_TOTAL_MONEY" runat="server" Text="<%$Resources:Resource,wfb2sn_lb_afa_total_money%>"></asp:Label>:
                        </th>
                        <td>
                            <asp:TextBox ID="txt_AFA_TOTAL_MONEY" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <!--阿法值對象總人數-->
                            <asp:Label ID="lb_AFA_TOTAL_PEOPLE" runat="server" Text=" <%$Resources:Resource,wfb2sn_lb_afa_total_people%>"></asp:Label>:
                        </th>
                        <td>
                            <asp:TextBox ID="txt_AFA_TOTAL_PEOPLE" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <!--備註說明-->
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2sn_lb_remark%>"></asp:Label>:
                        </th>
                        <td colspan="5">
                            <asp:TextBox TextMode="MultiLine" Rows="5" ID="txt_REMARK" runat="server" Width="100%" BorderWidth="1" Style="overflow: auto"></asp:TextBox>
                        </td>
                    </tr>                    
                    <tr>
                        <th></th>
                        <td align="right" colspan="5">
                            <aces:Btn ID="WFB2SN0200Back" runat="server" Text="<%$Resources:Resource,wfb2sn_btn_back%>" OnClientClick="BlockUI();" OnClick="WFB2SN0200Back_Click"/>
                            
                            <%-- 
                            <aces:Btn ID="WFB2SN0200Back" runat="server" Text="<%$Resources:Resource,wfb2sn_btn_back%>" OnClientClick="BlockUI();" OnClick="WFB2SN0200Back_Click"/>
                            <asp:Button ID="WFB2SN0200Back" runat="server" Text="<%$Resources:Resource,wfb2sn_btn_back%>" OnClientClick="BlockUI();" OnClick="WFB2SN0200Back_Click"/>
                                     --%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>                  
                   
                    <tr>
                        <td align="left" colspan="3">                           
                        </td>
                        <td align="right" colspan="3">
                            
                            <aces:Btn ID="WFB2SN0200Approve" runat="server" Text="<%$Resources:Resource,wfb2sn_btn_approve%>" OnClientClick="return checkApprove();" OnClick="WFB2SN0200Approve_Click"/>
                            <aces:Btn ID="WFB2SN0200Reject" runat="server" Text="<%$Resources:Resource,wfb2sn_btn_reject%>" OnClientClick="return checkReject();" OnClick="WFB2SN0200Reject_Click"/>
                            <%--
                            <aces:Btn ID="WFB2SN0200Approve" runat="server" Text="<%$Resources:Resource,wfb2sn_btn_approve%>" OnClientClick="return checkApprove();" OnClick="WFB2SN0200Approve_Click"/>
                            <aces:Btn ID="WFB2SN0200Reject" runat="server" Text="<%$Resources:Resource,wfb2sn_btn_reject%>" OnClientClick="return checkReject();" OnClick="WFB2SN0200Reject_Click"/>
                            <asp:Button ID="WFB2SN0200Approve" runat="server" Text="<%$Resources:Resource,wfb2sn_btn_approve%>" OnClientClick="return checkApprove();" OnClick="WFB2SN0200Approve_Click"/>
                            <asp:Button ID="WFB2SN0200Reject" runat="server" Text="<%$Resources:Resource,wfb2sn_btn_reject%>" OnClientClick="return checkReject();" OnClick="WFB2SN0200Reject_Click"/>
                            --%>
                        </td>
                    </tr>

                </tbody>
            </table>
            
            <asp:ObjectDataSource ID="ods2" runat="server" SelectMethod="getDataDtl"
                SelectCountMethod="getCountDtl" TypeName="CFB2SN0200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="hid_AFA_FOR" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="AFA_FOR" PropertyName="Value" Type="String" />                                               
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1019px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sn_lb_AFA_APPROVE_MARK%>" HeaderStyle-Width="80px">                       
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2df_RowNumber%>" HeaderStyle-Width="60px" />
                    <asp:BoundField DataField="AFA_APPROVE_STATUS" HeaderText="<%$Resources:Resource,wfb2sn_lb_AFA_APPROVE_STATUS%>" SortExpression="AFA_APPROVE_STATUS"
                         HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"/>
                    <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2sn_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2sn_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left"/>
                    <asp:BoundField DataField="PJOB_CD" HeaderText="<%$Resources:Resource,wfb2sn_lb_PJOB_CD%>" SortExpression="PJOB_CD" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left"/>
                    <asp:BoundField DataField="LEVEL_CD" HeaderText="<%$Resources:Resource,wfb2sn_lb_LEVEL_CD%>" SortExpression="LEVEL_CD" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left"/>
                    <asp:BoundField DataField="AFA_AMT" HeaderText="<%$Resources:Resource,wfb2sn_lb_AFA_AMT%>" SortExpression="AFA_AMT" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:n0}"/>                   
                    <asp:BoundField DataField="AFA_APPROVE_MARK" />              
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
             <td style="font-size: 14px;">
                <asp:Label ID="lb_TotalCount2" runat="server" Text="" Visible="false" Font-Size="10"></asp:Label>
            </td>
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
                     
            <asp:HiddenField ID="hid_AFA_FOR" runat="server" ClientIDMode="Static" />
            <!-- SN020_Qry的查詢條件 -->
            <asp:HiddenField ID="HID_AWARD_ROUND" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_FREEZE_FLAG" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_APPROVE_STATUS" runat="server" ClientIDMode="Static" />

            <!-- -->


            <!-- 是否為supervisor  -->
            <asp:HiddenField ID="HID_IS_SUPERVISOR" runat="server" ClientIDMode="Static" />
            <!-- 每頁的筆數 -->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_PageRow2" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_tab_id" runat="server" ClientIDMode="Static" />
            <!-- 是否要凍結視窗 -->
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
        
       
    </asp:UpdatePanel>
</asp:Content>
