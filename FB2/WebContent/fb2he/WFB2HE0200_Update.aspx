<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2he/WFB2HE0200_Update.aspx.cs" Inherits="WebContent_WFB2HE_WFB2HE0200_Update" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });
        function iniForm() {
            $.unblockUI();
            $(".textWidth").css("font-family", "Times New Roman, Times, serif").css("font-size", "14px");
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $("#tabs").tabs();

            $("#ddl_JPN_CD").change(function () {
                checkJPN_CD();
            });
            checkJPN_CD();
            //部門代號取得部門名稱的ajax
            $('#txt_DEPT_NAME').attr("readonly", true);
            $("#txt_DEPT_NO").change(function () {
                if ($("#txt_DEPT_NO").val().length == 7) {
                    $.ajax({
                        url: "../commgeo/WFB2GetDeptData.ashx",
                        data: {
                            DEPT_NO: $('#txt_DEPT_NO').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        cache: false,
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_DEPT_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_DEPT_NAME').val(JData.DEPT_NAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_DEPT_NAME').val("");
                }
            });



        }
        function ChangeTab(tab) {
            $("#tabs").tabs({ active: tab });
        }

        function ddl_LEVEL_CD_Changed() {
            ChangeTab(3);
        }



        //家庭儲存前檢查
        function saveFamCheck() {
            var processed = true;

            if (Page_ClientValidate("GroupFam")) {
                if ($("#txt_FAMILY_LICENSE_ID").val() != undefined) {
                    if (checkLicenseID($("#txt_FAMILY_LICENSE_ID").val()))
                        BlockUI();
                    else {
                        alert("眷屬身份證號檢查錯誤");
                        processed = false;
                    }
                }
                else
                    BlockUI();

            }
            else
                processed = false;
            if (!processed)
                $.unblockUI();

            return processed;
        }
        //教育儲存前檢查
        function saveEduCheck() {
            var processed = true;

            if (Page_ClientValidate("GroupEdu")) {
                BlockUI();
            }
            else
                processed = false;
            if (!processed)
                $.unblockUI();

            return processed;
        }

        //教育儲存前檢查
        function saveExpCheck() {
            var processed = true;

            if (Page_ClientValidate("GroupExp")) {
                BlockUI();
            }
            else
                processed = false;
            if (!processed)
                $.unblockUI();

            return processed;
        }
        //總儲存檢查
        function saveCheck() {
            var processed = true;


            if (Page_ClientValidate("GroupA")) {
                if ($("#txt_LICENSE_ID").val() != undefined) {
                    if (checkLicenseID($("#txt_LICENSE_ID").val()))
                        BlockUI();
                    else {
                        alert("身份證號檢查錯誤");
                        processed = false;
                    }
                }
                else
                    BlockUI();
            }
            else
                processed = false;
            if (!processed)
                $.unblockUI();

            return processed;
        }

        function checkJPN_CD() {
            if ($("#ddl_JPN_CD").val() == "-1") {
                $("#txt_START_DT").attr("disabled", "disabled");
                $("#txt_END_DT").attr("disabled", "disabled");
                $("#ddl_RENT_SUBSIDY").attr("disabled", "disabled");
            }
            else {
                $("#txt_START_DT").removeAttr("disabled");
                $("#txt_END_DT").removeAttr("disabled");
                $("#ddl_RENT_SUBSIDY").removeAttr("disabled");
            }

        }

        //兼任的資料
        function openHRItem() {
            window.showModalDialog("WFB2HB0100_OtherPjob.aspx?emp_id=" + $("#txt_EMP_ID").val(), self, 'dialogWidth=600px;dialogHeight=400px;scroll=no');
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
                                <col width="16%" />
                                <col width="20%" />
                                <col width="12%" />
                                <col width="21%" />
                                <col width="9%" />
                                <col width="22%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_EMP_NAME%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                                        
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LICENSE_ID" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_LICENSE_ID2%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_LICENSE_ID" runat="server" MaxLength="30" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                                        
                                    </td>
                                    <%-- 照片 --%>
                                    <th class="Body_TH3">
                                        <asp:Label ID="lb_EMP_PHOTO" runat="server" Text="<%$Resources:Resource,wfb2he_lb_EMP_PHOTO%>"></asp:Label>:
                                    </th>
                                </tr>

                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_NATION_JPN_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_NATION_JPN_CD%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_NATION_CD" runat="server" Font-Bold="false"></asp:Label>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SEX_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_SEX_CD%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SEX_CD" runat="server" BorderWidth="0" ReadOnly="true"></asp:TextBox>
                                    </td>   
                                    <%-- 照片位置 --%>
                                    <td rowspan="4" style="border-right: #cccc99 1px solid; border-top: #cccc99 1px solid; border-left: #cccc99 1px solid; border-bottom: #cccc99 1px solid;" align="center">
                                        <asp:Image ID="EmpPhoto" runat="server" Width="120px" Height="120px" ImageUrl="" />
                                    </td>                              
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BIRTH_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_BIRTH_DT%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_BIRTH_DT" runat="server" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                                        <%--<asp:TextBox ID="txt_BLOOD_TYPE" runat="server" BorderWidth="0" ReadOnly="true"></asp:TextBox>--%>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BIRTHPLACE" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_BIRTHPLACE%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_BIRTHPLACE" runat="server" MaxLength="30" ClientIDMode="Static" BorderWidth="0" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>                                    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_HEIGHT" runat="server" Text="<%$Resources:Resource,wfb2he_lb_HEIGHT%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_HEIGHT" runat="server" MaxLength="3" Width="35px" ClientIDMode="Static" BorderWidth="0" ReadOnly="true"></asp:TextBox>                                        
                                    </td>
                                     <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_WEIGHT" runat="server" Text="<%$Resources:Resource,wfb2he_lb_WEIGHT%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">                                        
                                        <asp:TextBox ID="txt_WEIGHT" runat="server" MaxLength="3" Width="35px" ClientIDMode="Static" BorderWidth="0" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>                          
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BLOOD_TYPE" runat="server" Text="<%$Resources:Resource,wfb2he_lb_BLOOD_TYPE%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">                                        
                                        <asp:TextBox ID="txt_BLOOD_TYPE" runat="server" BorderWidth="0" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_ARMY_CD" runat="server" Text="<%$Resources:Resource,wfb2he_lb_ARMY_CD%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">                                        
                                        <asp:TextBox ID="txt_ARMY_CD" runat="server" BorderWidth="0" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="right" colspan="10">
                        <aces:Btn ID="WFB2HE0200Save" runat="server" Text="<%$Resources:Resource,wfb2he_btn_save%>" OnClick="WFB2HE0200Save_Click" ValidationGroup="GroupA"/> 
                        <asp:Button ID="WFB2HE0200Cancel" runat="server" Text="<%$Resources:Resource,wfb2he_btn_back%>" Visible="true" OnClick="WFB2HE0200Cancel_Click" />

                         <%--
                        <asp:Button ID="WFB2HE0200Save" runat="server" Text="<%$Resources:Resource,wfb2he_btn_save%>" OnClick="WFB2HE0200Save_Click" ValidationGroup="GroupA"/> 
                                    
                                            
                         --%>
                    </td>
                </tr>
            </table>


            <div id="tabs" style="width: 1020px">
                <ul>
                    <li><a href="#tabs-1">【聯絡資料】</a></li>
                    <li><a href="#tabs-2">【學經歷資料】</a></li> 
                    <li><a href="#tabs-3">【求職資料】</a></li>
                    <li><a href="#tabs-4">【應徵資料】</a></li>
                    <li><a href="#tabs-5">【面試結果】</a></li>                                  
                </ul>
                <div id="tabs-1">
                    <br />
                    <table cellspacing="1" cellpadding="1" width="100%" border="0">
                        <colgroup>
                            <col width="15%" />
                            <col width="20%" />
                            <col width="15%" />
                            <col width="20%" />
                            <col width="15%" />
                            <col width="15%" />
                        </colgroup>
                        <tbody>
                            <tr>
                                <td align="left" class="Body_label" colspan="6">【戶籍】
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2he_lb_ZIP%>"></asp:Label>:</th>
                                <td align="left" class="Body_TD3" >
                                    <asp:TextBox ID="txt_REGISTER_ZIP_CD" runat="server" MaxLength="5" Width="50px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                    <asp:TextBox ID="txt_REGISTER_COUNTY" runat="server" MaxLength="10" Width="70px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                    <asp:TextBox ID="txt_REGISTER_REGION" runat="server" MaxLength="10" Width="70px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2he_lb_ADDR%>"></asp:Label>:</th>
                                <td class="Body_TD3" >
                                    <asp:TextBox ID="txt_REGISTER_ADDR" runat="server" MaxLength="255" Width="500px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_REGISTER_TEL%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_REGISTER_TEL" runat="server" MaxLength="10" Width="140px" BorderWidth="0" ReadOnly="true" ClientIDMode="Static" CssClass="textWidth"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="Body_label" colspan="6">【現居】
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="Label11" runat="server" Text="<%$Resources:Resource,wfb2he_lb_ZIP%>"></asp:Label>:</th>
                                <td align="left" class="Body_TD3" >
                                    <asp:TextBox ID="txt_CONTACT_ZIP_CD" runat="server" MaxLength="5" Width="50px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                    <asp:TextBox ID="txt_CONTACT_COUNTY" runat="server" MaxLength="10" Width="70px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                    <asp:TextBox ID="txt_CONTACT_REGION" runat="server" MaxLength="10" Width="70px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>

                                </td>
                            </tr>
                             <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="Label12" runat="server" Text="<%$Resources:Resource,wfb2he_lb_CONTACT_ADDR%>"></asp:Label>:</th>
                                <td class="Body_TD3" >
                                    <asp:TextBox ID="txt_CONTACT_ADDR" runat="server" MaxLength="255" Width="500px" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="Label13" runat="server" Text="<%$Resources:Resource,wfb2he_lb_CONTACT_TEL%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_CONTACT_TEL" runat="server" MaxLength="10" Width="140px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="Label14" runat="server" Text="<%$Resources:Resource,wfb2he_lb_PERSONAL_EMAIL%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_PERSONAL_EMAIL" runat="server" MaxLength="50" Width="280px" BorderWidth="0" ReadOnly="true" ClientIDMode="Static" CssClass="textWidth"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="Label15" runat="server" Text="<%$Resources:Resource,wfb2he_lb_MOBILE_TEL_1%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_MOBILE_TEL_1" runat="server" MaxLength="10" Width="140px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>                               
                            </tr>
                           <tr>
                                <td align="left" class="Body_label" colspan="6">【緊急聯絡人】
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_URG_CONTACT_NAME" runat="server" Text="<%$Resources:Resource,wfb2he_lb_URGENT_CONTACT_NAME%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_URG_CONTACT_NAME" runat="server" MaxLength="30" Width="100px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>                        
                            </tr>
                            <tr>
                                 <th class="Body_TH3">
                                    <asp:Label ID="lb_URG_CONTACT_TEL" runat="server" Text="<%$Resources:Resource,wfb2he_lb_URGENT_CONTACT_TEL%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_URG_CONTACT_TEL" runat="server" MaxLength="12" Width="100px" BorderWidth="0" ReadOnly="true" CssClass="textWidth" ClientIDMode="Static"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                  <th class="Body_TH3">
                                    <asp:Label ID="lb_URG_CONTACT_RELATION" runat="server" Text="<%$Resources:Resource,wfb2he_lb_URGENT_CONTACT_RELATION%>"></asp:Label>:</th>
                                <td class="Body_TD3">                                    
                                    <asp:TextBox ID="txt_URG_CONTACT_RELATION" runat="server" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>  
                            </tr>                                          
                        </tbody>
                    </table>
                </div>
                
                <div id="tabs-2">
                    <br />
                    <table cellspacing="1" cellpadding="1" width="100%" border="0">
                        <colgroup>
                            <col width="15%" />
                            <col width="15%" />
                            <col width="15%" />
                            <col width="15%" />
                            <col width="20%" />
                            <col width="20%" />
                        </colgroup>
                        <tbody>
                            <tr>
                                <td align="left" class="Body_label" colspan="6">【最高學歷】
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_EDUCATION_CD" runat="server" Text="<%$Resources:Resource,wfb2he_lb_EDUCATION_CD%>"></asp:Label>:</th>
                                <td align="left" class="Body_TD3">
                                    <asp:TextBox ID="txt_EDUCATION_CD" runat="server" MaxLength="10" Width="150px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_SCHOOL_NATION_CD" runat="server" Text="<%$Resources:Resource,wfb2he_lb_SCHOOL_NATION_CD%>"></asp:Label>:</th>
                                <td align="left" class="Body_TD3" >
                                    <asp:TextBox ID="txt_SCHOOL_NATION_CD" runat="server" MaxLength="20" Width="150px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>                                  
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_GRADUATION_YEAR" runat="server" Text="<%$Resources:Resource,wfb2he_lb_GRADUATION_YEAR%>"></asp:Label>:</th>
                                <td align="left" class="Body_TD3" >
                                    <asp:TextBox ID="txt_GRADUATION_YEAR" runat="server" MaxLength="10" Width="150px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>                                  
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_SCHOOL_NAME" runat="server" Text="<%$Resources:Resource,wfb2he_lb_SCHOOL_NAME%>"></asp:Label>:</th>
                                <td align="left" class="Body_TD3">
                                    <asp:TextBox ID="txt_SCHOOL_NAME" runat="server" MaxLength="20" Width="150px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                                <th></th>
                                <td></td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_DEPARTMENT_NAME" runat="server" Text="<%$Resources:Resource,wfb2he_lb_DEPARTMENT_NAME%>"></asp:Label>:</th>
                                <td align="left" class="Body_TD3">
                                    <asp:TextBox ID="txt_DEPARTMENT_NAME" runat="server" MaxLength="20" Width="150px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>                                  
                                </td>
                            </tr>                            
                            <tr>
                                <td align="left" class="Body_label" colspan="6">【最近工作經歷】
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_EXP_COMPANY_NAME" runat="server" Text="<%$Resources:Resource,wfb2he_lb_EXP_COMPANY_NAME%>"></asp:Label>:</th>
                                <td align="left" class="Body_TD3" >
                                    <asp:TextBox ID="txt_EXP_COMPANY_NAME" runat="server" MaxLength="60" Width="150px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>                                   
                                </td>
                                <th></th>
                                <td></td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_EXP_TITLE_DESC" runat="server" Text="<%$Resources:Resource,wfb2he_lb_EXP_TITLE_DESC%>"></asp:Label>:</th>
                                <td align="left" class="Body_TD3" >
                                    <asp:TextBox ID="txt_EXP_TITLE_DESC" runat="server" MaxLength="60" Width="150px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>                                   
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_START_YEAR" runat="server" Text="<%$Resources:Resource,wfb2he_lb_START_YEAR%>"></asp:Label>:</th>
                                <td align="left" class="Body_TD3" >
                                    <asp:TextBox ID="txt_START_YEAR" runat="server" MaxLength="6" Width="150px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>                                   
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_END_YEAR" runat="server" Text="<%$Resources:Resource,wfb2he_lb_END_YEAR%>"></asp:Label>:</th>
                                <td align="left" class="Body_TD3" >
                                    <asp:TextBox ID="txt_END_YEAR" runat="server" MaxLength="6" Width="150px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>                                   
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_APPROVE_WORK_YEARS" runat="server" Text="<%$Resources:Resource,wfb2he_lb_APPROVE_WORK_YEARS%>"></asp:Label>:</th>
                                <td align="left" class="Body_TD3" >
                                    <asp:TextBox ID="txt_APPROVE_WORK_YEARS" runat="server" MaxLength="3" Width="150px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>                                   
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="Body_label" colspan="6">【語言能力】
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_LANGUAGE_TOEIC" runat="server" Text="<%$Resources:Resource,wfb2he_lb_LANGUAGE_TOEIC%>"></asp:Label>:</th>
                                <td align="left" class="Body_TD3" >
                                    <asp:TextBox ID="txt_LANGUAGE_TOEIC" runat="server" MaxLength="3" Width="150px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>                                   
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_LANGUAGE_JAPANESE" runat="server" Text="<%$Resources:Resource,wfb2he_lb_LANGUAGE_JAPANESE%>"></asp:Label>:</th>
                                <td align="left" class="Body_TD3" >
                                    <asp:TextBox ID="txt_LANGUAGE_JAPANESE" runat="server" MaxLength="20" Width="150px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>                                   
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_LANGUAGE_OTHER" runat="server" Text="<%$Resources:Resource,wfb2he_lb_LANGUAGE_OTHER%>"></asp:Label>:</th>
                                <td align="left" class="Body_TD3" >
                                    <asp:TextBox ID="txt_LANGUAGE_OTHER" runat="server" MaxLength="50" Width="150px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>                                   
                                </td>
                            </tr>                                                
                        </tbody>
                    </table>
                </div>

                <div id="tabs-3">
                    <br />
                    <table cellspacing="1" cellpadding="1" width="100%" border="0">
                        <colgroup>
                            <col width="15%" />
                            <col width="35%" />
                            <col width="15%" />
                            <col width="35%" />                          
                        </colgroup>
                        <tbody>                            
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_APPLY_CHANNEL" runat="server" Text="<%$Resources:Resource,wfb2he_lb_APPLY_CHANNEL%>"></asp:Label>:</th>
                                <td align="left" class="Body_TD3">
                                    <asp:TextBox ID="txt_APPLY_CHANNEL" runat="server" MaxLength="50" Width="150px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_KZ_EXP" runat="server" Text="<%$Resources:Resource,wfb2he_lb_KZ_EXP%>"></asp:Label>:</th>
                                <td align="left" class="Body_TD3" >
                                    <asp:TextBox ID="txt_KZ_EXP" runat="server" MaxLength="50" Width="150px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>                                  
                                </td>                               
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_TRANSPORT_CD" runat="server" Text="<%$Resources:Resource,wfb2he_lb_TRANSPORT_CD%>"></asp:Label>:</th>
                                <td align="left" class="Body_TD3">
                                    <asp:TextBox ID="txt_TRANSPORT_CD" runat="server" MaxLength="10" Width="150px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>                               
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_TRANSPORT_LICENSE_CD" runat="server" Text="<%$Resources:Resource,wfb2he_lb_TRANSPORT_LICENSE_CD%>"></asp:Label>:</th>
                                <td align="left" class="Body_TD3" >
                                    <asp:TextBox ID="txt_TRANSPORT_LICENSE_CD" runat="server" MaxLength="10" Width="150px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>                                  
                                </td>
                            </tr>     
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_ACCOM_NEED" runat="server" Text="<%$Resources:Resource,wfb2he_lb_ACCOM_NEED%>"></asp:Label>:</th>
                                <td align="left" class="Body_TD3">
                                    <asp:TextBox ID="txt_ACCOM_NEED" runat="server" MaxLength="10" Width="150px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>                               
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_INTRODUCER" runat="server" Text="<%$Resources:Resource,wfb2he_lb_INTRODUCER%>"></asp:Label>:</th>
                                <td align="left" class="Body_TD3" >
                                    <asp:TextBox ID="txt_INTRODUCER" runat="server" MaxLength="30" Width="150px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>                                  
                                </td>
                            </tr>                                           
                        </tbody>
                    </table>
                </div>
                <%-- 應徵資料 --%>
                <div id="tabs-4">
                    <br />
                    <table cellspacing="1" cellpadding="1" width="100%" border="0">
                        <colgroup>
                            <col width="15%" />
                            <col width="20%" />
                            <col width="15%" />
                            <col width="20%" />
                            <col width="15%" />
                            <col width="15%" />                                 
                        </colgroup>
                        <tbody>                            
                            <tr>
                                <%-- 應徵職務 --%>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_PJOB_CD" runat="server" Text="<%$Resources:Resource,wfb2he_lb_PJOB_CD%>"></asp:Label>:</th>
                                <td align="left" class="Body_TD3">
                                    <asp:TextBox ID="txt_PJOB_CD" runat="server" MaxLength="20" Width="150px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                                <th></th>
                                <td></td>  
                                <th></th>
                                <td></td>                                                               
                            </tr>
                            <tr>
                                <%-- 員工區分 --%>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_EMP_CD" runat="server" Text="<%$Resources:Resource,wfb2he_lb_EMP_CD%>"></asp:Label>:</th>
                                <td align="left" class="Body_TD3">
                                    <asp:DropDownList ID="ddl_EMP_CD" runat="server"  ClientIDMode="Static" CssClass="MandatoryField textWidth" ></asp:DropDownList> 
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator113" runat="server" ErrorMessage="<%$Resources:Resource,wfb2he_required_EMP_CD%>"
                                            ControlToValidate="ddl_EMP_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                </td>                               
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_WS_CD" runat="server" Text="<%$Resources:Resource,wfb2he_lb_WS_CD%>"></asp:Label>:</th>
                                <td align="left" class="Body_TD3" >
                                    <asp:TextBox ID="txt_WS_CD" runat="server" MaxLength="10" Width="150px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>                                                                 
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_COMPANY_CD" runat="server" Text="<%$Resources:Resource,wfb2he_lb_COMPANY_CD%>"></asp:Label>:</th>
                                <td align="left" class="Body_TD3" >
                                    <asp:TextBox ID="txt_COMPANY_CD" runat="server" MaxLength="10" Width="150px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>                                  
                                </td>
                            </tr>     
                            <tr>
                                <%-- 工廠區分 --%>
                                <th align="left" class="Body_TH3">
                                     <asp:Label ID="lb_PLANT_CD" runat="server" Text="<%$Resources:Resource,wfb2he_lb_PLANT_CD%>"></asp:Label>:
                                </th>                                    
                                <td align="left" class="Body_TD3">
                                     <asp:DropDownList ID="ddl_PLANT_CD" runat="server"  ClientIDMode="Static" CssClass="MandatoryField textWidth"></asp:DropDownList> 
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2he_required_PLANT_CD%>"
                                            ControlToValidate="ddl_PLANT_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                </td>
                                <%-- 資格級數 --%>
                                <th align="left" class="Body_TH3">
                                     <asp:Label ID="lb_LEVEL_GRADE" runat="server" Text="<%$Resources:Resource,wfb2he_lb_LEVEL_GRADE%>"></asp:Label>:
                                </th>                                    
                                <td align="left" class="Body_TD3">
                                     <asp:DropDownList ID="ddl_LEVEL_CD" runat="server"  ClientIDMode="Static" CssClass="MandatoryField textWidth" OnSelectedIndexChanged="ddl_LEVEL_CD_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList> 
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2he_required_LEVEL_CD%>"
                                            ControlToValidate="ddl_LEVEL_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    <asp:DropDownList ID="ddl_GRADE_CD" runat="server"  ClientIDMode="Static" CssClass="textWidth"></asp:DropDownList> 
                                    <%-- 
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2he_required_GRADE_CD%>"
                                            ControlToValidate="ddl_GRADE_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    --%>
                                </td> 
                                <th align="left" class="Body_TH3">
                                     <asp:Label ID="lb_WORK_CD" runat="server" Text="<%$Resources:Resource,wfb2he_lb_WORK_CD%>"></asp:Label>:
                                </th>                                    
                                <td align="left" class="Body_TD3">
                                    <asp:DropDownList ID="ddl_WORK_CD" runat="server"  ClientIDMode="Static" CssClass="MandatoryField textWidth"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2he_required_WORK_CD%>"
                                            ControlToValidate="ddl_WORK_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>                                            
                                </td>
                            </tr>   
                            <tr>
                                 <%-- 部門代號 --%>
                                <th align="left" class="Body_TH3">
                                    <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2he_lb_DEPT_NO%>"></asp:Label>:
                                </th>
                                <td align="left" class="Body_TD3" colspan ="5">
                                    <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" Width="120px" ClientIDMode="Static"  CssClass="MandatoryField textWidth"></asp:TextBox>
                                    <input id="bt_DEPT_SEARCH" type="button" value="..." onclick="OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', 'N');" />
                                    <asp:TextBox ID="txt_DEPT_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2he_lb_required_DEPT_NO%>"
                                            ControlToValidate="txt_DEPT_NO" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2he_lb_required_DEPT_NAME%>"
                                            ControlToValidate="txt_DEPT_NAME" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                </td>                                        
                            </tr> 
                            <tr>
                                <%-- 預計入社日期 --%>
                                 <th align="left" class="Body_TH3">
                                     <asp:Label ID="lb_JOIN_DT" runat="server" Text="<%$Resources:Resource,wfb2he_lb_JOIN_DT%>"></asp:Label>：	
                                 </th>
                                 <td align="left" class="Body_TD3">
                                    <asp:TextBox ID="txt_JOIN_DT" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date MandatoryField textWidth"></asp:TextBox>                                             
                                     <asp:CustomValidator ID="RegularExpressionValidator10" runat="server" ValidateEmptyText="true"
								    ErrorMessage="<%$Resources:Resource,wfb2he_JOIN_DT_ERR%>" ClientValidationFunction="CheckDate" ForeColor="Red"
								    ControlToValidate="txt_JOIN_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2he_required_JOIN_DT%>"
                                         ControlToValidate="txt_JOIN_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                 </td> 
                                 <th align="left" class="Body_TH3">
                                    <asp:Label ID="lb_EXAM_EXPIRE_DT" runat="server" Text="<%$Resources:Resource,wfb2he_lb_EXAM_EXPIRE_DT%>"></asp:Label>：	
                                 </th>
                                 <td align="left" class="Body_TD3">
                                    <asp:TextBox ID="txt_EXAM_EXPIRE_DT" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date textWidth"></asp:TextBox>                                                                         
                                     <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
								    ErrorMessage="<%$Resources:Resource,wfb2he_EXAM_EXPIRE_DT_ERR%>" ClientValidationFunction="CheckDate" ForeColor="Red"
								    ControlToValidate="txt_EXAM_EXPIRE_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                 </td>  
                                 <th align="left" class="Body_TH3">
                                      <asp:Label ID="lb_PLAN_DESPATCH_DT" runat="server" Text="<%$Resources:Resource,wfb2he_lb_PLAN_DESPATCH_DT%>"></asp:Label>：	
                                 </th>
                                 <td align="left" class="Body_TD3">
                                      <asp:TextBox ID="txt_PLAN_DESPATCH_DT" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date textWidth"></asp:TextBox>                                                                                              
                                         <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
								        ErrorMessage="<%$Resources:Resource,wfb2he_PLAN_DESPATCH_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
								        ControlToValidate="txt_PLAN_DESPATCH_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                 </td> 
                             </tr>                                                                   
                        </tbody>
                    </table>
                </div>

                <div id="tabs-5">
                    <br />
                    <table cellspacing="1" cellpadding="1" width="100%" border="0">
                        <colgroup>
                            <col width="15%" />
                            <col width="20%" />
                            <col width="15%" />
                            <col width="20%" />
                            <col width="15%" />
                            <col width="15%" />                                 
                        </colgroup>
                        <tbody>                            
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_INTERVIEW_RESULT" runat="server" Text="<%$Resources:Resource,wfb2he_lb_INTERVIEW_RESULT%>"></asp:Label>:</th>
                                <td align="left" class="Body_TD3">
                                    <asp:TextBox ID="txt_INTERVIEW_RESULT" runat="server" MaxLength="20" Width="150px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_INTERVIEW_BY" runat="server" Text="<%$Resources:Resource,wfb2he_lb_INTERVIEW_BY%>"></asp:Label>:</th>
                                <td align="left" class="Body_TD3">
                                    <asp:TextBox ID="txt_INTERVIEW_BY" runat="server" MaxLength="20" Width="150px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>  
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_INTERVIEW_DT" runat="server" Text="<%$Resources:Resource,wfb2he_lb_INTERVIEW_DT%>"></asp:Label>:</th>
                                <td align="left" class="Body_TD3">
                                    <asp:TextBox ID="txt_INTERVIEW_DT" runat="server" MaxLength="20" Width="150px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>                                                               
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_ADOPT_RESULT" runat="server" Text="<%$Resources:Resource,wfb2he_lb_ADOPT_RESULT%>"></asp:Label>:</th>
                                <td align="left" class="Body_TD3">
                                    <asp:TextBox ID="txt_ADOPT_RESULT" runat="server" MaxLength="20" Width="150px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>                                                               
                                </td>                               
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_ADOPT_BY" runat="server" Text="<%$Resources:Resource,wfb2he_lb_ADOPT_BY%>"></asp:Label>:</th>
                                <td align="left" class="Body_TD3" >
                                    <asp:TextBox ID="txt_ADOPT_BY" runat="server" MaxLength="10" Width="150px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>                                  
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_ADOPT_DT" runat="server" Text="<%$Resources:Resource,wfb2he_lb_ADOPT_DT%>"></asp:Label>:</th>
                                <td align="left" class="Body_TD3" >
                                    <asp:TextBox ID="txt_ADOPT_DT" runat="server" MaxLength="10" Width="150px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>                                  
                                </td>
                            </tr>     
                            <tr>
                                <th align="left" class="Body_TH3">
                                     <asp:Label ID="lb_APPROVE_STATUS" runat="server" Text="<%$Resources:Resource,wfb2he_lb_APPROVE_STATUS%>"></asp:Label>:
                                </th>                                    
                                <td align="left" class="Body_TD3">
                                     <asp:TextBox ID="txt_APPROVE_STATUS" runat="server" MaxLength="10" Width="150px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>                                  
                                </td>
                                <th align="left" class="Body_TH3">
                                     <asp:Label ID="lb_APPROVE_BY" runat="server" Text="<%$Resources:Resource,wfb2he_lb_APPROVE_BY%>"></asp:Label>:
                                </th>                                    
                                <td align="left" class="Body_TD3">
                                    <asp:TextBox ID="txt_APPROVE_BY" runat="server" MaxLength="10" Width="150px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>   
                                </td>
                                <th align="left" class="Body_TH3">
                                     <asp:Label ID="lb_APPROVE_DT" runat="server" Text="<%$Resources:Resource,wfb2he_lb_APPROVE_DT%>"></asp:Label>:
                                </th>                                    
                                <td align="left" class="Body_TD3">
                                    <asp:TextBox ID="txt_APPROVE_DT" runat="server" MaxLength="10" Width="150px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>   
                                </td>                                 
                            </tr>   
                            <tr>
                                <th align="left" class="Body_TH3">
                                     <asp:Label ID="lb_APPROVE_REMARK" runat="server" Text="<%$Resources:Resource,wfb2he_lb_APPROVE_REMARK%>"></asp:Label>:
                                </th>                                    
                                <td align="left" class="Body_TD3">
                                     <asp:TextBox ID="txt_APPROVE_REMARK" runat="server" MaxLength="10" Width="150px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>                                  
                                </td>                                     
                            </tr>                                                                                               
                        </tbody>
                    </table>
                </div>

            </div>
            <asp:ValidationSummary ID="ValidationSummary4" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
