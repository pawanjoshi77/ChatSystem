<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="conversation.aspx.cs" Inherits="ChatSystem.conversation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Chat Conversation</h1>
        <asp:Label ID="lblOutput" runat="server" />
        <asp:XmlDataSource ID="xmlSource" runat="server" DataFile="App_Data/conversation.xml" />
          
       <div id="chatbox" runat="server" style="border:2px solid red; margin-top:10px; background-color:white; width:500px; height:300px;"></div>
        <div>
            <asp:Label ID="lbl_message" runat="server" Text="Enter your message"></asp:Label>
            <asp:TextBox ID="txt_message" runat="server"></asp:TextBox>
        </div>
        <div>
            <asp:Button ID="btn_submit" runat="server" Text="Submit" OnClick="btn_submit_Click" />
        </div>
    </form>
</body>
</html>
