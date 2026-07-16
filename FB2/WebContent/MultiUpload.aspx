<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MultiUpload.aspx.cs" Inherits="WebContent_MultiUpload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type = "text/javascript">
        var counter = 0;
        function AddFileUpload() {
            var div = document.createElement('DIV');
            div.innerHTML = '<input id="file' + counter + '" name = "file' + counter +
                            '" type="file" />' +
                            '<input id="Button' + counter + '" type="button" ' +
                            'value="Remove" onclick = "RemoveFileUpload(this)" />';
            document.getElementById("FileUploadContainer").appendChild(div);
            counter++;
        }
        function RemoveFileUpload(div) {
            document.getElementById("FileUploadContainer").removeChild(div.parentNode);
        }
</script>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data" method = "post">
        <input id="Button1" type="button" value="新增上傳" onclick="AddFileUpload()" />
        <br />
        <br />
        <div id="FileUploadContainer">
            <!--FileUpload Controls will be added here -->
        </div>
        <br />
        <asp:Button ID="btnUpload" runat="server"
            Text="上傳" OnClick="btnUpload_Click" />
    </form>
</body>
</html>
