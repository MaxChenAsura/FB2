<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2pa/WFB2PA0300_Proc.aspx.cs" Inherits="WebContent_WFB2PA0300_Proc" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
     <style type="text/css">
        
    </style>
    <script type="text/javascript">

        $(function () {

            iniForm();
        });
        function iniForm() {
            //日期格式心須
            $(".date").datepicker({ dateFormat: 'yy/mm' });
            $('.date').mask('9999/99');
            $('.date2').mask('9999/99');
            $(".numFormat").mask('999');
            $(".decimal").css("text-align", "right").css("ime-mode", "disabled");

            //GridView必須
            gridviewScroll();
            $.unblockUI();
 
        }

       




        //查詢前檢核
        function CheckSearch() {
            var processed = true;
            if ($("#txt_YM").val() == "") {

                alert("本次發放年月，不可為空白");
                return false;
            }
            if ($("#txt_YM").val() <= $("#txt_PRE_YM").val()) {
                alert("本次發年月須大於薪資已月結年月:" + $("#txt_PRE_YM").val());
                return false;
            }
            if (confirm("確定要執行結轉薪資 ?")) {

                return true;

            } else {
                return false;
            }
            BlockUI();
            return processed;
        }

        //清空畫面
        function ClearAll() {
            $("#txt_YM").val("");
        }


    </script>
    <style type="text/css">
        .auto-style1 {
            height: 23px;
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="10%" />
                    <col width="15%" />
                    <col width="10%" />
                    <col width="15%" />
                    <col width="20%" />
                    <col width="30%" />
                </colgroup>
                <tbody>
                     <tr>                        
                         <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_PRE_YM" runat="server" Text="上次發放年月"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                             <asp:TextBox ID="txt_PRE_YM" runat="server" MaxLength="7" Width="81px" ClientIDMode="Static" CssClass="date2" Enabled="false" BorderWidth="0" ></asp:TextBox>
                        </td>
                         <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_YM" runat="server" Text="本次發放年月"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                             <asp:TextBox ID="txt_YM" runat="server" MaxLength="7" Width="81px" ClientIDMode="Static" CssClass="MandatoryField date"  BorderWidth="0" ></asp:TextBox>
                        </td>
                   </tr>
                                
                    <tr>
                        <th></th>
                        <th></th>
                        <th></th>
                        <td align="right" class="Body_label">
                            <div id="init">
                                <aces:Btn ID="WFB2PA0300Proc" runat="server" Text="結轉薪資" OnClick="WFB2PA0300Process_Click" OnClientClick="return CheckSearch();" />
                           
                                <%--<asp:Button ID="WFB2IA0100Search" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0100Search%>" OnClick="WFB2IA0100Search_Click" OnClientClick="BlockUI();" />--%>
                                            
                                <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2ia_btn_clear%>" OnClientClick="ClearAll();" />
                            </div>
                        </td>
                    </tr>
                  
                </tbody>
            </table>

           
            <asp:HiddenField ID="hid_today" runat="server" ClientIDMode="Static" />

            
               <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />

        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
