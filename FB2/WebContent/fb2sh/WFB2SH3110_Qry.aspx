<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sh/WFB2SH3110_Qry.aspx.cs" Inherits="WebContent_WFB2SH3110_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">

        $(function () {

            iniForm();
        });
        function iniForm() {
            //日期格式心須
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.date').mask('9999/99/99');
            $(".numFormat").mask('99.99');
            $(".numFormat2").mask('99999');
            $(".decimal").css("text-align", "right").css("ime-mode", "disabled");


            // 設定職務名稱為唯讀
            $("input[id$='txt_NEW_PJOB_DESC']").attr("readonly", true);
            $("input[id$='txt_NEW_PJOB_DESC']").css("background-color", "transparent");

            //reComma("AWARD_BASE", 3);
            //GridView必須
            gridviewScroll();
            $.unblockUI();
        }

        // ★ 自訂函式：處理 Pjob_Search 回傳值
        function Pjob_Search(obj_cd, obj_desc, value) {
            console.log("=== Pjob_Search callback START ===");
            console.log("obj_cd:", obj_cd);
            console.log("obj_desc:", obj_desc);
            console.log("value:", value);
            
            var returnValue = value;
            if (returnValue == undefined || returnValue === 'undefined') {
                console.log("returnValue is undefined, exit");
                return;
            }

            try {
                var obj = jQuery.parseJSON(returnValue);
                console.log("Parsed JSON:", obj);
                console.log("CD:", obj.CD);
                console.log("DESC:", obj.DESC);

                // ★ 使用屬性選擇器找到 UpdatePanel 內的控制項
                var $cdControl = $('[id$="' + obj_cd + '"]');
                var $descControl = $('[id$="' + obj_desc + '"]');

                console.log("Found CD control count:", $cdControl.length);
                console.log("Found DESC control count:", $descControl.length);

                if ($cdControl.length > 0) {
                    console.log("CD control ID:", $cdControl.attr('id'));
                    console.log("CD control NAME:", $cdControl.attr('name'));
                    $cdControl.val(obj.CD);
                    console.log("Set PJOB_CD to:", obj.CD);
                    console.log("Current value:", $cdControl.val());

                    // ★ 觸發 AutoPostBack（必須用實際的 name 屬性）
                    var controlName = $cdControl.attr('name');
                    console.log("Triggering __doPostBack with:", controlName);
                    
                    if (controlName && typeof __doPostBack !== 'undefined') {
                        setTimeout(function() {
                            __doPostBack(controlName, '');
                        }, 100);
                    } else {
                        console.log("ERROR: Cannot trigger postback - controlName:", controlName, ", __doPostBack exists:", typeof __doPostBack !== 'undefined');
                    }
                }

                if ($descControl.length > 0) {
                    console.log("DESC control ID:", $descControl.attr('id'));
                    $descControl.val(obj.DESC);
                    console.log("Set PJOB_DESC to:", obj.DESC);
                }
                
                console.log("=== Pjob_Search callback END ===");
            } catch (ex) {
                console.error("Error in Pjob_Search:", ex);
                alert("發生錯誤: " + ex.message);
            }
        }

        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());

        }

        //凍結視窗用
        function gridviewScroll() {
            if ($("#HID_Freeze").val() == "Y") {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "400",
                    barcolor: "#7F7F7F",
                    freezesize: 0

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

        //刪除,檢查是否有勾選
        function doDelete() {
            var processed = true;
            BlockUI();
            if (checkboxsSelected() > 0) {
                processed = confirm("確定要刪除?");
            } else {
                alert("請選取資料!");
                processed = false;
            }
            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }

        var choietr = null;
        //回傳目前Checkbox被勾選的數量
        function checkboxsSelected() {
            choietr = null;
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                    if (choietr == null) {
                        choietr = i;
                    }
                }
            }
            return HaveCheck;
        }

        //查詢前檢核
        function CheckSearch() {
            BlockUI();
            //其它需要檢核的
            /*
            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            */
        }

        ////將DIV的scollbar跑到最低,移到Basic.js
        //function gridViewScrollBottom(id) {
        //    $("table[id$="+id+"]").parent().scrollTop(99999);
        //}


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
            $("#txt_PJOB_CD").val("");
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
                    <col width="10%" />
                    <col width="20%" />
                    <col width="10%" />
                    <col width="20%" />
                    <col width="10%" />
                    <col width="20%" />
                    <col width="10%" />

                </colgroup>
                <tbody>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--考績--%>
                            <asp:Label ID="Label10" runat="server" Text="職務代碼"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_PJOB_CD" runat="server" Width="100px" ClientIDMode="Static" > </asp:TextBox>
                        </td>
                        <th align="left" class="Body_label">
                           </th>
                        <td align="left" class="Body_label">
                            
                        </td>
                        <th align="left" class="Body_label">
                            </th>
                        <td align="left" class="Body_label">
                          
                        </td>
                    </tr>
                    <tr>
                        <th></th>
                        <td align="right" colspan="5">
                            
                            <aces:Btn ID="WFB2SH3110Search" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_search%>" OnClick="WFB2SH3110Search_Click" OnClientClick="CheckSearch();" />
                            <%--
                            <asp:Button ID="WFB2SH3110Search" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_search%>" OnClick="WFB2SH3110Search_Click" OnClientClick="CheckSearch();" />
                             --%>
                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2sh_btn_clear%>" onclick="ClearAll();" />
                        </td>
                    </tr>
                    <tr>
                        <td  class="Body_label" colspan="6">
                             <hr size="3">
                               【<asp:Label ID="lb_LABOR" runat="server" Text="考績格差設定" align="left"></asp:Label>】
                               <div id="init_grid" align="right">
                                <aces:Btn ID="WFB2SH3110Add" runat="server" Text="新增" Visible="true" OnClick="WFB2SH3110Add_Click" />
                                <aces:Btn ID="WFB2SH3110Delete" runat="server" Text="刪除" Visible="false" OnClick="WFB2SH3110Delete_Click" OnClientClick="return doDelete();" />
                                <aces:Btn ID="WFB2SH3110EDIT" runat="server" Text="修改" Visible="false" OnClick="WFB2SH3110EDIT_Click" OnClientClick="BlockUI();" />
                                <aces:Btn ID="WFB2SH3110Save" runat="server" Text="確認" Visible="false" OnClick="WFB2SH3110SAVE_Click" OnClientClick="return saveCheck()" />
                                <%--
                                <asp:Button ID="WFB2SH3110Add" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_add%>" Visible="true" OnClick="WFB2SH3110Add_Click"  />
                                <asp:Button ID="WFB2SH3110Delete" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_delete%>" Visible="false" OnClick="WFB2SH3110Delete_Click" OnClientClick="return doDelete();" />
                                <asp:Button ID="WFB2SH3110Edit" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_edit%>" Visible="false" OnClick="WFB2SH3110Edit_Click" OnClientClick="BlockUI();" />
                                <asp:Button ID="WFB2SH3110OK" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_ok%>" Visible="false"  OnClick="WFB2SH3110OK_Click" OnClientClick="return saveCheck()"/>
                                --%>

                                <asp:Button ID="btn_cancel" runat="server" Text="清除" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="btn_cancel_Click" />
                                </div>
                     
                                <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getAward_Data"
                                    SelectCountMethod="getAwardCount" TypeName="CFB2SH3110DAO" EnablePaging="True"
                                    SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                                    OnSelected="ods1_Selected">
                                    <SelectParameters>
                                        <asp:Parameter Name="startRowIndex" Type="Int32" />
                                        <asp:Parameter Name="maximumRows" Type="Int32" />
                                        <asp:ControlParameter ControlID="txt_PJOB_CD"
                                            Name="pjob_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
       
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                                <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                                    AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                                    OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                                    OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound"
                                    ShowHeaderWhenEmpty="true">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="20px">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" Width="20px" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" Width="20px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--序號--%>
                                        <asp:TemplateField HeaderText="序號" HeaderStyle-Width="40px">
                                            <ItemTemplate>
                                                <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <%--職務代碼--%>
                                        <asp:TemplateField HeaderText="職務代碼" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left" SortExpression="PJOB_CD">
                                            <ItemTemplate>
                                                <asp:Label ID="lb_PJOB_CD" runat="server" Text='<%#Bind("PJOB_CD")%>' Width="80px"></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lb_PJOB_CD" runat="server" Text='<%#Bind("PJOB_CD")%>' Width="80px"></asp:Label>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                 <div style="text-align: left; width: 100%">
                                                  <asp:TextBox ID="txt_NEW_PJOB_CD" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="50px" MaxLength="3" OnTextChanged="txt_NEW_PJOB_CD_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                     <input id="btn_PJOB_CD" type="button" value="..." onclick="OpenSearch('Pjob_Search.aspx','txt_NEW_PJOB_CD', 'txt_NEW_PJOB_DESC','','Y');" />
                                                  </div>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <%--職務名稱--%>
                                        <asp:TemplateField HeaderText="職務名稱" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="WS_CD">
                                            <ItemTemplate>
                                                <asp:Label ID="lb_PJOB_DESC" runat="server" Text='<%#Bind("PJOB_DESC")%>' Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lb_PJOB_DESC" runat="server" Text='<%#Bind("PJOB_DESC")%>' Width="100px"></asp:Label>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txt_NEW_PJOB_DESC" runat="server" ClientIDMode="Static" BorderWidth="0" Width="80px"></asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <%--年資起--%>
                                        <asp:TemplateField HeaderText="年資起" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" SortExpression="YEAR_S">
                                            <ItemTemplate>
                                                <asp:Label ID="lb_YEAR_S" runat="server" Text='<%#Bind("YEAR_S")%>' Width="80px"></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lb_YEAR_S" runat="server" Text='<%#Bind("YEAR_S")%>' Width="80px"></asp:Label>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                               <asp:TextBox ID="txt_NEW_YEAR_S" runat="server" ClientIDMode="Static" Width="100px"></asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <%--年資訖--%>
                                        <asp:TemplateField HeaderText="年資訖" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" SortExpression="YEAR_E">
                                            <ItemTemplate>
                                                <asp:Label ID="lb_YEAR_E" runat="server" Text='<%#Bind("YEAR_E")%>' Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txt_EDIT_YEAR_E" runat="server" Text='<%#Bind("YEAR_E")%>' ClientIDMode="Static" Width="100px" CssClass="MandatoryField numFormat  decimal"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="EDIT_YEAR_E" runat="server" ErrorMessage="年資訖不可為空白"
                                                    ControlToValidate="txt_EDIT_YEAR_E" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txt_NEW_YEAR_E" runat="server" ClientIDMode="Static" Width="100px" CssClass="MandatoryField numFormat  decimal"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="NEW_AYEAR_E" runat="server" ErrorMessage="年資訖不可為空白"
                                                    ControlToValidate="txt_NEW_YEAR_E" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <%--基準獎金--%>
                                        <asp:TemplateField HeaderText="基準獎金" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right" SortExpression="BONUS_BASE">
                                            <ItemTemplate>
                                                <asp:Label ID="lb_BONUS_BASE" runat="server" Text='<%#Bind("BONUS_BASE")%>' Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txt_EDIT_BONUS_BASE" runat="server" Text='<%#Bind("BONUS_BASE")%>' MaxLength="50" ClientIDMode="Static" Width="100px"></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txt_NEW_BONUS_BASE" runat="server" MaxLength="50" ClientIDMode="Static" Width="100px"> </asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="GridviewScrollHeader" />
                                    <PagerStyle CssClass="GridviewScrollPager" />

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
                        </td>

                    </tr>
                   
                </tbody>
            </table>


            

            
            <!-- 查詢條件 -->
            <asp:HiddenField ID="hid_qry_PJOB_CD" runat="server" ClientIDMode="Static" />

            <!-- 每頁的筆數 -->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <!-- 是否要凍結視窗 -->
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="N" />
            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
