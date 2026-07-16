<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2ha/WFB2HA0600_Dtl.aspx.cs" Inherits="WebContent_fb2ha_WFB2HA0600_Dtl" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    
   
      
    <script type="text/javascript">


        jQuery(document).ready(function () {

            iniForm();
            $(".PERSON").blur(function () {
                alert("This input field has lost its focus.");

            });

        });


        function iniForm() {
            $("#txt_YEAR_MONTH_Add").datepicker({ dateFormat: 'yymm' });
            $("#txt_START_DT_S").datepicker({ dateFormat: 'yy-mm-dd' });
            $("#txt_START_DT_E").datepicker({ dateFormat: 'yy-mm-dd' });
            $(".number").mask('9999/99');
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
        function ClearAll() {
            $('#ddl_SYS_ID').val(-1);
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
                alert($('#hidWFB2IB_Mod_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckDtlAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidWFB2IB_Dtl_NotChoiceMessage').val());
                return false;
            }
        }
        //儲存前檢查
        function saveCheck() {
            var processed = true;


            //if (Page_ClientValidate("GroupA")) {
            //    BlockUI();
            //}
            //else
            //    processed = false;
            //if (!processed)
            //    $.unblockUI();

            return processed;
        }

        function CheckSaveAction() {
            if ($('#txtCALENDAR_CD_Edit').val() != undefined) {
                if ($('#txtCALENDAR_CD_Edit').val().trim() == "") {
                    alert($('#hidWFB2IB_txtCALENDAR_CD_NotNull').val());
                    return false;
                }
                else
                    return confirm($('#hidWFB2IB_Save_ConfirmMessage').val());
            }
            else
                return confirm($('#hidWFB2IB_Save_ConfirmMessage').val());
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
    </script>

    <style type="text/css">
        .MandatoryField {}
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
 <!--2990400_QRY開始-->
           <table>
                <tr>
                    <th>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                               <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_HR_CHG_CD" runat="server" Text="">人事異動代碼</asp:Label>:

                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Literal ID="lit_HR_CHG_CD" runat="server" ClientIDMode="Static" ></asp:Literal>

                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_HR_CHG_DESC" runat="server" Text="">人事異動代碼說明</asp:Label>:

                                    </th>
                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:TextBox ID="txt_HR_CHG_DESC" runat="server"  Width="150px"  ClientIDMode="Static"  Enabled="false"></asp:TextBox>

                                    </td>
                                    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_IS_VALID" runat="server" Text="">使用中</asp:Label>:

                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_IS_VALID" runat="server" ClientIDMode="Static" Enabled="false">
                                            <asp:ListItem>Y</asp:ListItem>
                                            <asp:ListItem>N</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>

                                    </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_IS_FOR_BATCH" runat="server" Text="">一括異動適用</asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_IS_FOR_BATCH" runat="server" ClientIDMode="Static" Enabled="false">
                                            <asp:ListItem Value="Y">Y-適用</asp:ListItem>
                                            <asp:ListItem Value="N">N-不適用</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_IS_FOR_TRANSFER_IN" runat="server" Text="">借調人員適用</asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_IS_FOR_TRANSFER_IN" runat="server" ClientIDMode="Static" Enabled="false">
                                            <asp:ListItem Value="Y">Y-適用</asp:ListItem>
                                            <asp:ListItem Value="N">N-不適用</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_IS_SHOW" runat="server" Text="">人事履歷是否顯示</asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_IS_SHOW" runat="server" ClientIDMode="Static" Enabled="false">
                                            <asp:ListItem>Y</asp:ListItem>
                                            <asp:ListItem>N</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_IS_PROFESSION_PJOB" runat="server" Text="">是否專業職務</asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_IS_PROFESSION_PJOB" runat="server" ClientIDMode="Static" Enabled="false">
                                            <asp:ListItem>Y</asp:ListItem>
                                            <asp:ListItem>N</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>

                                </tr>
                                        <tr>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lbl_UPD_RIGHT_CD" runat="server" Text="權限區分"></asp:Label>
                                                : </th>
                                            <td align="left" class="Body_label" colspan="5">
                                        <asp:DropDownList ID="ddl_UPD_RIGHT_CD" runat="server" ClientIDMode="Static" Enabled="false">
                                            <asp:ListItem>A</asp:ListItem>
                                            <asp:ListItem>D</asp:ListItem>
                                        </asp:DropDownList>
                                         【A】只有管理部可輸入的異動別；【D】各單位可輸入的異動別。
                                            </td>
                                        </tr>
                                        <tr>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lbl_IS_INS_EARLIER" runat="server" Text="保險提前生效"></asp:Label>: 
                                           </th>
                                            <td align="left" class="Body_label" colspan="5">
                                                <asp:DropDownList ID="ddl_IS_INS_EARLIER" runat="server" ClientIDMode="Static" Enabled="false">
                                                    <asp:ListItem>Y</asp:ListItem>
                                                    <asp:ListItem>N</asp:ListItem>
                                                </asp:DropDownList>
                                                    【Y】保險預計處理日預設為空白；【N】保險預計處理日預設為異動生效日。
                                            </td>
                                        </tr>
                                        <tr>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lbl_IS_TEMP" runat="server" Text="是否暫時狀態"></asp:Label>: 
                                           </th>
                                            <td align="left" class="Body_label" colspan="5">
                                                <asp:DropDownList ID="ddl_IS_TEMP" runat="server" ClientIDMode="Static" Enabled="false">
                                                    <asp:ListItem>Y</asp:ListItem>
                                                    <asp:ListItem>E</asp:ListItem>
                                                    <asp:ListItem>N</asp:ListItem>
                                                </asp:DropDownList>
                                                    【Y】有起迄期間狀態之異動別，且為狀態開始；【E】暫時狀態結束；【N】無關暫時狀態。
                                            </td>
                                        </tr>
                                        <tr>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lbl_IS_LEAVE" runat="server" Text="是否離社"></asp:Label>: 
                                           </th>
                                            <td align="left" class="Body_label" colspan="5">
                                                <asp:DropDownList ID="ddl_IS_LEAVE" runat="server" ClientIDMode="Static" Enabled="false">
                                                    <asp:ListItem>Y</asp:ListItem>
                                                    <asp:ListItem>N</asp:ListItem>
                                                    <asp:ListItem>X</asp:ListItem>
                                                </asp:DropDownList>
                                                    【Y】自願離社；【N】非自願離社；【X】無關離社。
                                            </td>
                                        </tr>
                                        <tr>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lbl_IS_UPD_HR" runat="server" Text="是否人事更新"></asp:Label>: 
                                           </th>
                                            <td align="left" class="Body_label" colspan="5">
                                                <asp:DropDownList ID="ddl_IS_UPD_HR" runat="server" ClientIDMode="Static" Enabled="false">
                                                    <asp:ListItem>Y</asp:ListItem>
                                                    <asp:ListItem>N</asp:ListItem>                                                    
                                                </asp:DropDownList>
                                                    異動生效後，【Y】更新員工人事主檔；【N】不更新。
                                            </td>
                                        </tr>
                                        <tr>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lbl_IS_UPD_DEPT_HEAD" runat="server" Text="是否部門主管更新"></asp:Label>: 
                                           </th>
                                            <td align="left" class="Body_label" colspan="5">
                                                <asp:DropDownList ID="ddl_IS_UPD_DEPT_HEAD" runat="server" ClientIDMode="Static" Enabled="false">
                                                    <asp:ListItem>Y</asp:ListItem>
                                                    <asp:ListItem>N</asp:ListItem>                                                    
                                                </asp:DropDownList>
                                                    異動生效後，【Y】更新部門主管及員工主管；【N】不更新。
                                            </td>
                                        </tr>
                                        <tr>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lbl_SALARY_PROC_CD" runat="server" Text="薪資處理區分"></asp:Label>: 
                                           </th>
                                            <td align="left" class="Body_label" colspan="5">
                                                <asp:DropDownList ID="ddl_SALARY_PROC_CD" runat="server" ClientIDMode="Static" Enabled="false">
                                                    <asp:ListItem>I</asp:ListItem>
                                                    <asp:ListItem>O</asp:ListItem>
                                                    <asp:ListItem>U</asp:ListItem>
                                                    <asp:ListItem>N</asp:ListItem>                                                      
                                                </asp:DropDownList>
                                                    異動生效後，通知薪資系統【I】新增；【O】結束；【U】變更；【N】無關； 敘薪資料。
                                            </td>
                                        </tr>
                                        <tr>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lbl_INSURANCE_PROC_CD" runat="server" Text="保險處理區分"></asp:Label>: 
                                           </th>
                                            <td align="left" class="Body_label" colspan="5">
                                                <asp:DropDownList ID="ddl_INSURANCE_PROC_CD" runat="server" ClientIDMode="Static" Enabled="false">
                                                    <asp:ListItem>I</asp:ListItem>
                                                    <asp:ListItem>O</asp:ListItem>
                                                    <asp:ListItem>U</asp:ListItem>
                                                    <asp:ListItem>N</asp:ListItem>                                                      
                                                </asp:DropDownList>
                                                    異動生效後，通知保險系統【I】加保；【O】退保；【U】調整保險資料；【N】無關保險。
                                            </td>
                                        </tr>
                                        <tr>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lbl_DUTY_PROC_CD" runat="server" Text="勤務處理區分"></asp:Label>: 
                                           </th>
                                            <td align="left" class="Body_label" colspan="5">
                                                <asp:DropDownList ID="ddl_DUTY_PROC_CD" runat="server" ClientIDMode="Static" Enabled="false">
                                                    <asp:ListItem>I</asp:ListItem>
                                                    <asp:ListItem>O</asp:ListItem>
                                                    <asp:ListItem>U</asp:ListItem>
                                                    <asp:ListItem>N</asp:ListItem>                                                      
                                                </asp:DropDownList>
                                                    異動生效後，通知勤務系統【I】產生；【O】刪除；【U】更新；【N】無關； 日勤務班表資料
                                            </td>
                                        </tr>
                                        <tr>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lbl_CONTRACT_PROC_CD" runat="server" Text="期間工處理區分"></asp:Label>: 
                                           </th>
                                            <td align="left" class="Body_label" colspan="5">
                                                <asp:DropDownList ID="ddl_CONTRACT_PROC_CD" runat="server" ClientIDMode="Static" Enabled="false">
                                                    <asp:ListItem>I</asp:ListItem>
                                                    <asp:ListItem>O</asp:ListItem>
                                                    <asp:ListItem>U</asp:ListItem>
                                                    <asp:ListItem>N</asp:ListItem>                                                      
                                                </asp:DropDownList>
                                                    異動生效後，【I】產生；【O】刪除；【U】更新；【N】無關； 獎金發放計劃。
                                            </td>
                                        </tr>
                                        <tr>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lbl_EMP_CHG_STATUS" runat="server" Text="人事異動身份狀態"></asp:Label>: 
                                           </th>
                                            <td align="left" class="Body_label">
                                                <asp:DropDownList ID="ddl_EMP_CHG_STATUS" runat="server" ClientIDMode="Static" Enabled="false">                                                                                                      
                                                </asp:DropDownList>
                                                    
                                            </td>
                                        </tr>
                                        <tr>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lbl_REMARK" runat="server" Text="備註"></asp:Label>: 
                                           </th>
                                            <td align="left" class="Body_label" colspan="4">
                                        <asp:TextBox ID="txt_REMARK" runat="server"  Width="100%"  ClientIDMode="Static"  Enabled="false"></asp:TextBox>
                                            </td>
                                        </tr>









                                        <tr>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th><th></th>
                                            <td align="right" class="Body_label">                                               
                                                    <asp:Button ID="WFB2HA0600back" runat="server" OnClick="WFB2HA0600back_Click" OnClientClick="BlockUI();" Text="<%$Resources:Resource,btn_back%>" />
                                            </td>
                                        </tr>
                                    </caption>
                                </tr>
                                <tr>
                                    <td align="right" class="Body_label">
                                        <div id="init_grid">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    
                                </tr>
                                <!--2990400_QRY結束-->
                        <asp:ObjectDataSource ID="ods1" runat="server" EnablePaging="True" OnSelected="ods1_Selected" OnSelecting="obs1_Selecting" SelectCountMethod="getCount" SelectMethod="getData" SortParameterName="sortExpression" StartRowIndexParameterName="startRowIndex" TypeName="Cfb2sb2300DAO">
                            <SelectParameters>
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="cb_all" runat="server" ClientIDMode="Static" meta:resourcekey="cb_checkallResource1" onclick="javascript:SelectAllCheckboxes(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" meta:resourcekey="cb_checkResource1" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div style="text-align: center; width: 100%">
                                            <asp:CheckBox ID="cb_check" runat="server" Checked="true" />
                                        </div>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="40px" HeaderText="<%$Resources:Resource,wfb2ib_RowNumber%>" meta:resourcekey="RowNumber">
                                    <ItemTemplate>
                                        <div style="text-align: center; width: 100%">
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
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_EMP_ID%>" SortExpression="EMP_ID">
                                    <ItemTemplate>
                                        <div style="text-align: center; width: 100%">
                                            <asp:Label ID="lbl_EMP_ID" runat="server" CssClass="number" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_EMP_NAME%>" SortExpression="EMP_NAME">
                                    <ItemTemplate>
                                        <div style="text-align: center; width: 100%">
                                            <asp:Label ID="lbl_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_EMP_CD%>" SortExpression="DESC1">
                                    <ItemTemplate>
                                        <div style="text-align: center; width: 100%;">
                                            <asp:Label ID="lbl_DESC1" runat="server" Text='<%# Convert.ToString(Eval("DESC1"))%>'></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_SALARY_ID%>" SortExpression="SALARY_NAME">
                                    <ItemTemplate>
                                        <div style="text-align: center; width: 100%;">
                                            <asp:Label ID="INS_MAX_MONTH" runat="server" Text='<%#Bind("SALARY_NAME")%>'></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_CHG_AMT_A%>" SortExpression="CHG_AMT_A">
                                    <ItemTemplate>
                                        <div style="text-align: center; width: 100%;">
                                            <asp:Label ID="lbl_CHG_AMT_A" runat="server" Text='<%#Bind("CHG_AMT_A")%>'></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_START_DT%>" SortExpression="START_DT_A">
                                    <ItemTemplate>
                                        <div style="text-align: center; width: 100%;">
                                            <asp:Label ID="lbl_START_DT_A" runat="server" CssClass="number" Text='<%#Bind("START_DT_B")%>'></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_END_DATE_A%>" SortExpression="END_DATE_A">
                                    <ItemTemplate>
                                        <div style="text-align: center; width: 100%;">
                                            <asp:Label ID="lbl_END_DATE_A" runat="server" CssClass="number" Text='<%#Bind("END_DATE_B")%>'></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_PROCESS_STATUS%>" SortExpression="PROCESS_STATUS">
                                    <ItemTemplate>
                                        <div style="text-align: center; width: 100%;">
                                            <asp:Label ID="lbl_PROCESS_STATUS" runat="server" Text='<%#Bind("PROCESS_STATUS")%>'></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_CHG_STATUS%>" SortExpression="CHG_STATUS">
                                    <ItemTemplate>
                                        <div style="text-align: center; width: 100%;">
                                            <asp:Label ID="lbl_CHG_STATUS" runat="server" Text='<%#Bind("CHG_STATUS")%>'></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_CHG_AMT_B%>" SortExpression="CHG_AMT_B">
                                    <ItemTemplate>
                                        <div style="text-align: center; width: 100%;">
                                            <asp:Label ID="lbl_CHG_AMT_B" runat="server" Text='<%#Bind("CHG_AMT_B")%>'></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_END_DT_A%>" SortExpression="END_DATE_A">
                                    <ItemTemplate>
                                        <div style="text-align: center; width: 100%;">
                                            <asp:Label ID="lbl_END_DT_B" runat="server" Text='<%#Bind("END_DATE_A")%>'></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_APPROVE_BY%>" SortExpression="APPROVE_BY">
                                    <ItemTemplate>
                                        <div style="text-align: center; width: 100%;">
                                            <asp:Label ID="lbl_APPROVE_BY" runat="server" Text='<%#Bind("APPROVE_BY")%>'></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_APPROVE_DT%>" SortExpression="APPROVE_DT">
                                    <ItemTemplate>
                                        <div style="text-align: center; width: 100%;">
                                            <asp:Label ID="lbl_APPROVE_DT" runat="server" Text='<%#Bind("APPROVE_DT")%>'></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_REMARK%>" SortExpression="REMARK">
                                    <ItemTemplate>
                                        <div style="text-align: center; width: 100%;">
                                            <asp:Label ID="lbl_REMARK" runat="server" Text='<%#Bind("REMARK")%>'></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_APP_REMARK%>" SortExpression="APP_REMARK">
                                    <ItemTemplate>
                                        <div style="text-align: center; width: 100%;">
                                            <asp:Label ID="lbl_APP_REMARK" runat="server" Text='<%#Bind("APP_REMARK")%>'></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle CssClass="GridviewScrollPager" />
                            <FooterStyle CssClass="GridviewScrollPager" />
                            <EmptyDataTemplate>
                                <table bgcolor="#FFFFFF" border="0" cellpadding="0" cellspacing="0" class="grid-view" height="100%" width="1020">
                                    <tr class="header">
                                        <td width="20px"></td>
                                        <td width="40px">
                                            <asp:Label ID="lblHeaderRowNumber" runat="server" Text="<%$Resources:Resource,wfb299_RowNumber%>"></asp:Label>
                                        </td>
                                        <td width="60px">
                                            <asp:Label ID="lblHeaderYEAR_MONTH" runat="server" Text="<%$Resources:Resource,WFB2IB_YEAR_MONTH%>"></asp:Label>
                                        </td>
                                        <td width="60px">
                                            <asp:Label ID="lblHeaderINS_RATE_PERSON" runat="server" Text="<%$Resources:Resource,WFB2IB_INS_RATE_PERSON%>"></asp:Label>
                                        </td>
                                        <td width="60px">
                                            <asp:Label ID="lblHeaderINS_RATE_COMP" runat="server" Text="<%$Resources:Resource,WFB2IB_INS_RATE_COMP%>"></asp:Label>
                                        </td>
                                        <td width="60px">
                                            <asp:Label ID="lblHeaderINS_MAX_MONTH" runat="server" Text="<%$Resources:Resource,WFB2IB_INS_MAX_MONTH%>"></asp:Label>
                                        </td>
                                        <td width="60px">
                                            <asp:Label ID="lblHeaderBUDGET_INS_MIN_AMOUNT" runat="server" Text="<%$Resources:Resource,WFB2IB_INS_MIN_AMOUNT%>"></asp:Label>
                                        </td>
                                        <td width="60px">
                                            <asp:Label ID="lblHeaderINS_MAX_AMOUNT" runat="server" Text="<%$Resources:Resource,WFB2IB_INS_MAX_AMOUNT%>"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="normal">
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <div style="text-align: center; width: 100%;">
                                                <asp:TextBox ID="txt_YEAR_MONTH_Add" runat="server" ClientIDMode="Static"  MaxLength="6" Text='<%#Bind("ACC_DEPT_NAME")%>'></asp:TextBox>
                                            </div>
                                        </td>
                                        <td>
                                            <div style="text-align: center; width: 100%">
                                                <asp:TextBox ID="txt_INS_RATE_PERSON_Add" runat="server"  MaxLength="6"></asp:TextBox>
                                            </div>
                                        </td>
                                        <td>
                                            <div style="text-align: center; width: 100%">
                                                <asp:TextBox ID="txt_INS_RATE_COMP_Add" runat="server"  MaxLength="2"></asp:TextBox>
                                            </div>
                                        </td>
                                        <td>
                                            <div style="text-align: center; width: 100%">
                                                <asp:TextBox ID="txt_INS_MAX_MONTH_Add" runat="server"  MaxLength="150"></asp:TextBox>
                                            </div>
                                        </td>
                                        <td>
                                            <div style="text-align: center; width: 100%">
                                                <asp:TextBox ID="txt_INS_MIN_AMOUNT_Add" runat="server"  MaxLength="150"></asp:TextBox>
                                            </div>
                                        </td>
                                        <td>
                                            <div style="text-align: center; width: 100%">
                                                <asp:TextBox ID="txt_INS_MAX_AMOUNT_Add" runat="server"  MaxLength="150"></asp:TextBox>
                                            </div>
                                            </div>
                                        </td>
                                        </div>
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                        </asp:GridView>
                        <table id="OnePage" runat="server" bgcolor="#FFFFFF" border="0" cellpadding="0" cellspacing="0" height="100%" style="padding-top: 5px; padding-left: 5px" visible="false">
                            <tr height="100%" valign="top">
                                <td class="GridviewScrollPager TD">
                                    <asp:DropDownList ID="ddlPerPageRow" runat="server" ClientIDMode="Static" onchange="javascript:ShowRecord('')">
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
                        <asp:HiddenField ID="hidwfb299_Del_NotChoiceMessage" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hidwfb299_Mod_NotChoiceMessage" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hidwfb299_Save_ConfirmMessage" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hidwfb299_Del_ConfirmMessage" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hidwfb299_Dtl_NotChoiceMessage" runat="server" ClientIDMode="Static" />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ShowSummary="false" ValidationGroup="GroupA" />
                                    </tr>

                        </table>

        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


