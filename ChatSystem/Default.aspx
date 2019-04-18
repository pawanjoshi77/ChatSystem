<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ChatSystem.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" method="post">
        <asp:Label ID="lblOutput" runat="server" />
        
        <div>
            <asp:Label ID="lbl_username" runat="server" Text="Enter your username"></asp:Label>
            <asp:TextBox ID="txt_username" runat="server"></asp:TextBox>
        </div>
        <div>
            <asp:Label ID="lbl_password" runat="server" Text="Enter your password"></asp:Label>
            <asp:TextBox ID="txt_password" runat="server"></asp:TextBox>
        </div>
        <div>
            <asp:Button ID="btn_login" runat="server" Text="Submit" OnClick="btn_login_Click" />
        </div>

        <div>
            <asp:Button ID="btn_google_login" runat="server" Text="Sign In with Google" OnClick="Google_Click" />
        </div>
        
    </form>
</body>
</html>
