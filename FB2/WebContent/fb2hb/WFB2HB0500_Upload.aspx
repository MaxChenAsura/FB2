<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2hb/WFB2HB0500_Upload.aspx.cs" Inherits="WebContent_fb2hb_WFB2HB0500_Upload" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function checkConfirm() {

            history.back(-2);
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
        <colgroup>
            <col width="15%" />
            <col width="55%" />
            <col width="30%" />
        </colgroup>
        <tbody>
            <tr>
                <td>
                    <br>
                </td>
            </tr>
            <tr>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_UploadExcel" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_UploadExcel%>"></asp:Label>:
                </th>
                <td align="left" class="Body_label">
                    <asp:FileUpload ID="FileUpload1" runat="server" class="MandatoryField" Width="600px" />
                </td>

                <td align="right" class="Body_label">
                    <%--<aces:Btn ID="WFB2HB0501ExcelImport" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0501ExcelImport%>" OnClick="WFB2HB0501ExcelImport_Click" " />--%>
                    <asp:Button ID="WFB2HB0501ExcelImport" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0501ExcelImport%>" OnClick="WFB2HB0501ExcelImport_Click" />
                    <asp:Button ID="btn_back" Text="<%$Resources:Resource,wfb2hb_btn_back%>" OnClick="btn_back_Click"  runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <br>
                </td>
            </tr>
            <tr>
                <td>
                    <br>
                </td>
            </tr>
            <tr>
                <td>
                    <br>
                </td>
            </tr>
            <tr>
                <td align="left" class="Body_label" colspan="5">

                    <fieldset style="padding: 5px">
                        <legend class="Body_label">
                            <asp:Label ID="lb_TemplateDownload" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_TemplateDownload%>"></asp:Label>

                        </legend>
                        <table align="right">
                            <tr>
                                <td>
                                    <p style="color: red;">
                                        <asp:Label ID="lb_type" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_type%>"></asp:Label>:

                                    </p>
                                </td>
                                <td>
                                    <asp:Button ID="WFB2HB0501ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0501ExcelDown%>" OnClick="WFB2HB0501ExcelDown_Click" />
                                </td>
                            </tr>
                        </table>

                    </fieldset>
                </td>
            </tr>
            <tr>
                <td align="left" class="Body_label"></td>
                <td align="left" class="Body_label"></td>
            </tr>
        </tbody>
    </table>
</asp:Content>
