<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="OpenWindowTest.aspx.cs" Inherits="WebContent_Example_OpenWindowTest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function getPjob() {
            var json = OpenSearch('Pjob_Search.aspx', 'txt_PJOB_CD', 'txt_PJOB_DESC', 'WS_CD=S&LEVEL_CD=2SA&PJOB_CD=MB10&START_DT=2014/05/01');
            if (json != undefined)
                alert("資格代號:" + json.Val1 + "\n職種:" + json.Val2);
        }

        function getWorkShift() {
            var json = OpenSearch('WorkShift_Search.aspx', 'txt_WORK_SHIFT_CD', 'txt_WORK_SHIFT_DESC', 'WORK_SHIFT_CD=11&WORKER_WORK_SHIFT=W');
            if (json != undefined)
                alert("行事曆代碼:" + json.Val1 + "\n行事曆說明:" + json.Val2);
        }

        function getReduce() {
            var json = OpenSearch('Reduce_Search.aspx', 'txt_REDUCE_CD', 'txt_REDUCE_DESC', 'REDUCE_CD=A');
            if (json != undefined)
                alert("勞保(個人)負擔比例%:" + json.Val1 + "\n" + "健保(個人)負擔比例%:" + json.Val2 + "\n" + "健保政府補助金額上限(定額):" + json.Val3);
        }

        function getLevel() {
            var json = OpenSearch('Level_Search.aspx', 'txt_INS_TYPE', 'txt_INS_AMT', 'INS_TYPE=A&INS=20000');
            if (json != undefined)
                alert("薪資下限:" + json.Val1 + "\n" + "薪資上限:" + json.Val2);
        }

        function getEmpFamily() {
            var json = OpenSearch('EmpFamily_Search.aspx', 'txt_FAMILY_LICENSE_ID', 'txt_FAMILY_NAME', 'EMP_ID=10006&FAMILY_LICENSE_ID=A123456789');
            if (json != undefined)
                alert("關係名稱:" + json.Val1 + "\n" + "國家名稱:" + json.Val2 + "\n" + "國家別:" + json.Val3 + "\n" + "關係代號:" + json.Val4);
        }

        function getEmpAndFamily() {
            var json = OpenSearch('EmpAndFamily_Search.aspx', 'txt_LICENCE_ID', 'txt_EMP_NAME', 'IDENTITY_KIND=B&EMP_ID=10006&LICENCE_ID=A123456789');
            if (json != undefined)
                alert("身份別:" + json.Val1 + "\n" + "關係名稱:" + json.Val2 + "\n" + "出生日期:" + json.Val3 + "\n" + "關係代號:" + json.Val4);
        }

        function getRegion() {
            var json = OpenSearch('Region_Search.aspx', 'txt_ZIP_CD', 'txt_REGION', 'ZIP_CD=100');
            alert("縣市:" + json.Val1);
        }

        function getDept() {
            var json = OpenDeptSearch('txt_tree_DEPT_NO', 'txt_tree_DEPT_NAME', 'N');
            if (json != undefined)
                alert("上層部門代號:" + json.UP_DEPT_NO + "\n" + "上層部門名稱:" + json.UP_DEPT_NAME + "\n" + "部門主管工號:" + json.HEAD_EMP_ID + "\n" + "部門主管名稱:" + json.HEAD_EMP_NAME);
        }

        function getEmp() {
            var json = OpenEmpSearch('txt_HEAD_EMP_ID', 'txt_HEAD_EMP_NAME', 'N');
            if (json != undefined)
                alert("所屬部門代號:" + json.DEPT_NO + "\n" + "所屬部門名稱:" + json.DEPT_NAME + "\n" +
                    "員工區分:" + json.EMP_CD + "\n" + "資格代號:" + json.LEVEL_CD + "\n" +
                    "級數代號:" + json.GRADE_CD + "\n" + "職務代號:" + json.PJOB_CD + "\n" +
                    "員工區分:" + json.JOIN_DT + "\n" + "資格代號:" + json.BE_EMP_DT + "\n" +
                    "職種:" + json.WS_CD + "\n" + "在職狀態:" + json.EMP_STATUS + "\n");
        }
        function getCompany(){
            var json = OpenSearch('Company_Search.aspx', 'txt_COMPANY_CD', 'txt_COMPANY_NAME', 'COMPANY_CD=A');
            if (json != undefined)
                alert("健保單位代號:" + json.Val1 + "\n" + "稅捐機關代號:" + json.Val2 + "\n" +
                    "勞保單位代號:" + json.Val3 + "\n" + "營利事業登記證號:" + json.Val4 + "\n" +
                    "稅籍編號:" + json.Val5);
        }

        function getSalary9999() {
            var json = OpenSearch('Salary9999_Search.aspx','txt_SALARY9999_ID', 'txt_SALARY9999_NAME', 'SALARY_ID=' + $("#txt_SALARY9999_ID").val());
          
        }
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    1.組織樹查詢視窗:<asp:TextBox ID="txt_tree_DEPT_NO" runat="server" MaxLength="10" Width="64px" ClientIDMode="Static"></asp:TextBox>
    <input id="bt_DEPT_SEARCH" type="button" value="..." onclick="getDept();" />
    <asp:TextBox ID="txt_tree_DEPT_NAME" runat="server" MaxLength="10" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
    <br />
    2.組織樹人員查詢視窗:<asp:TextBox ID="txt_HEAD_EMP_ID" runat="server" MaxLength="10" Width="64px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
    <input id="bt_HEAD_EMP_ID" type="button" value="..." onclick="getEmp();" />
    <asp:TextBox ID="txt_HEAD_EMP_NAME" runat="server" MaxLength="10" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
    <br />
    3.人事異動代碼查詢視窗:<asp:TextBox ID="txt_HR_CHG_CD" runat="server" MaxLength="10" Width="64px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
    <input id="btn_HR_CHG_CD" type="button" value="..." onclick="OpenSearch('HrChangeCode_Search.aspx', 'txt_HR_CHG_CD', 'txt_HR_CHG_DESC', 'HR_CHG_CD=B&UPD_RIGHT_CD=D&IS_FOR_BATCH=Y&IS_FOR_TRANSFER_IN=3&IS_VALID=Y&EMP_ID=10003');" />
    <asp:TextBox ID="txt_HR_CHG_DESC" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
    <br />
    4.輪值表查詢視窗:<asp:TextBox ID="txt_WORK_SHIFT_CD" runat="server" MaxLength="10" Width="64px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
    <input id="btn_WORK_SHIFT_CD" type="button" value="..." onclick="getWorkShift();" />
    <asp:TextBox ID="txt_WORK_SHIFT_DESC" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
    <br />
    5.主假別查詢視窗:<asp:TextBox ID="txt_MAIN_LEAVE_CD" runat="server" MaxLength="10" Width="64px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
    <input id="Button1" type="button" value="..." onclick="OpenSearch('LeaveType_Search.aspx', 'txt_MAIN_LEAVE_CD', 'txt_MAIN_LEAVE_DESC', '');" />
    <asp:TextBox ID="txt_MAIN_LEAVE_DESC" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
    <br />
    6.班別查詢視窗:<asp:TextBox ID="txt_SHIFT_CD" runat="server" MaxLength="10" Width="64px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
    <input id="Button2" type="button" value="..." onclick="OpenSearch('Shift_Search.aspx', 'txt_SHIFT_CD', 'txt_SHIFT_DESC', '');" />
    <asp:TextBox ID="txt_SHIFT_DESC" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
    <br />
    7.廠商查詢視窗:<asp:TextBox ID="txt_VENDOR_NO" runat="server" MaxLength="10" Width="64px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
    <input id="Button3" type="button" value="..." onclick="OpenSearch('Vendor_Search.aspx', 'txt_VENDOR_NO', 'txt_VENDOR_NAME', '');" />
    <asp:TextBox ID="txt_VENDOR_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
    <br />
    8.卡鐘查詢視窗:<asp:TextBox ID="txt_CLOCK_NO" runat="server" MaxLength="10" Width="64px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
    <input id="Button4" type="button" value="..." onclick="OpenSearch('Clock_Search.aspx', 'txt_CLOCK_NO', 'txt_CLOCK_DESC', 'CLOCK_NO=A');" />
    <asp:TextBox ID="txt_CLOCK_DESC" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
    <br />
    9.廠商人員查詢視窗:<asp:TextBox ID="txt_VENDOR_MEMBER_NO" runat="server" MaxLength="10" Width="64px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
    <input id="Button5" type="button" value="..." onclick="OpenSearch('Vendor_d_Search.aspx', 'txt_VENDOR_MEMBER_NO', 'txt_VENDOR_MEMBER_NAME', 'VENDOR_NO=A');" />
    <asp:TextBox ID="txt_VENDOR_MEMBER_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
    <br />
    10.卡片查詢視窗:<asp:TextBox ID="txt_CARD_NO" runat="server" MaxLength="10" Width="64px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
    <input id="Button6" type="button" value="..." onclick="OpenSearch('Card_Search.aspx', 'txt_CARD_NO', 'txt_CARD_NAME', '');" />
    <asp:TextBox ID="txt_CARD_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
    <br />
    11.郵遞區號查詢視窗:<asp:TextBox ID="txt_ZIP_CD" runat="server" MaxLength="10" Width="64px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
    <input id="Button7" type="button" value="..." onclick="getRegion();" />
    <asp:TextBox ID="txt_REGION" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
    <br />
    12.薪資項目查詢視窗:<asp:TextBox ID="txt_SALARY_ID" runat="server" MaxLength="10" Width="64px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
    <input id="Button8" type="button" value="..." onclick="OpenSearch('SalaryItem_Search.aspx', 'txt_SALARY_ID', 'txt_SALARY_NAME', 'SALARY_ID=A');" />
    <asp:TextBox ID="txt_SALARY_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
    <br />
    13.薪資部門區分查詢視窗:<asp:TextBox ID="txt_ACC_DEPT_NO" runat="server" MaxLength="10" Width="64px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
    <input id="Button9" type="button" value="..." onclick="OpenSearch('DeptAcc_Search.aspx', 'txt_ACC_DEPT_NO', 'txt_ACC_DEPT_NAME', 'ACC_DEPT_NO=A');" />
    <asp:TextBox ID="txt_ACC_DEPT_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
    <br />
    14.權限內加扣款類薪資項目查詢視窗:<asp:TextBox ID="txt_SALARY_MEM_ID" runat="server" MaxLength="10" Width="64px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
    <input id="Button10" type="button" value="..." onclick="OpenSearch('SalaryItemMem_Search.aspx', 'txt_SALARY_MEM_ID', 'txt_SALARY_MEM_NAME', 'SALARY_ID=A');" />
    <asp:TextBox ID="txt_SALARY_MEM_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
    <br />
    15.公司資料查詢視窗:<asp:TextBox ID="txt_COMPANY_CD" runat="server" MaxLength="10" Width="64px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
    <input id="Button11" type="button" value="..." onclick="getCompany();" />
    <asp:TextBox ID="txt_COMPANY_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
    <br />
    16.保險減免代碼查詢視窗:<asp:TextBox ID="txt_REDUCE_CD" runat="server" MaxLength="10" Width="64px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
    <input id="Button12" type="button" value="..." onclick="getReduce();" />
    <asp:TextBox ID="txt_REDUCE_DESC" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
    <br />
    17.投保級距查詢視窗:<asp:TextBox ID="txt_INS_TYPE" runat="server" MaxLength="10" Width="64px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
    <input id="Button13" type="button" value="..." onclick="getLevel();" />
    <asp:TextBox ID="txt_INS_AMT" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
    <br />
    18.部門查詢視窗-清單:<asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="10" Width="64px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
    <input id="Button14" type="button" value="..." onclick="OpenSearch('DeptGrid_Search.aspx', 'txt_DEPT_NO', 'txt_DEPT_NAME', 'DEPT_NO=KA00000');" />
    <asp:TextBox ID="txt_DEPT_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
    <br />
    19.共用代碼查詢視窗:<asp:TextBox ID="txt_SUB_CD" runat="server" MaxLength="10" Width="64px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
    <input id="Button15" type="button" value="..." onclick="OpenSearch('CommCode_Search.aspx', 'txt_SUB_CD', 'txt_SUB_DESC', 'SYS_CD=DF&MAIN_CD=ACCOM_BUILD_CD&SUB_CD=111');" />
    <asp:TextBox ID="txt_SUB_DESC" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
    <br />
    20.社內外人員查詢視窗:<asp:TextBox ID="txt_ID" runat="server" MaxLength="10" Width="64px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
    <input id="Button16" type="button" value="..." onclick="OpenSearch('People_Search.aspx', 'txt_ID', 'txt_NAME', '');" />
    <asp:TextBox ID="txt_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
    <br />
    21.員工家庭成員查詢視窗:<asp:TextBox ID="txt_FAMILY_LICENSE_ID" runat="server" MaxLength="10" Width="64px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
    <input id="Button17" type="button" value="..." onclick="getEmpFamily();" />
    <asp:TextBox ID="txt_FAMILY_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
    <br />
    22.本人及員工家庭成員查詢視窗:<asp:TextBox ID="txt_LICENCE_ID" runat="server" MaxLength="10" Width="64px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
    <input id="Button18" type="button" value="..." onclick="getEmpAndFamily();" />
    <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
    <br />
    23.職務查詢視窗:<asp:TextBox ID="txt_PJOB_CD" runat="server" MaxLength="10" Width="64px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
    <input id="Button19" type="button" value="..." onclick="getPjob();" />
    <asp:TextBox ID="txt_PJOB_DESC" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
    <br />
    24.薪資媒體檔項目查詢視窗:<asp:TextBox ID="txt_SALARY9999_ID" runat="server" MaxLength="10" Width="64px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
    <input id="Button20" type="button" value="..." onclick="getSalary9999();" />
    <asp:TextBox ID="txt_SALARY9999_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
    <br />
</asp:Content>

