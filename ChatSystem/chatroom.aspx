<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="chatroom.aspx.cs" Inherits="ChatSystem.chatroom" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    
    <form id="form1" runat="server">
        <h1>List of chatrooms</h1>
        <asp:Label ID="lblOutput" runat="server" />
        <asp:XmlDataSource ID="xmlSource" runat="server" DataFile="App_Data/chatroom.xml" XPath="/chatroomlist/chatroom" />
          
        <asp:GridView ID="gridOutput" runat="server" CssClass="table table-hover table-striped"
            DataSourceID="xmlSource" AutoGenerateColumns="false" >
              <Columns>
                  <asp:TemplateField HeaderText="RoomID">
                      <ItemTemplate>
                        <a href="conversation.aspx?roomId=<%#XPath("@roomId") %>" /><%#XPath("@roomId") %>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Room Name">
                      <ItemTemplate>
                          <%#XPath("roomName/text()") %>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Number of Users">
                      <ItemTemplate>
                          <%#XPath("numberofUsers/text()") %>
                      </ItemTemplate>
                  </asp:TemplateField>
              </Columns>
             <RowStyle CssClass="cursor-pointer" />
          </asp:GridView>
    </form>
    <script src="js/jquery-3.3.1.min.js"></script>
    <script src="js/popper.min.js"></script>
    <script src="js/bootstrap.min.js"></script>
</body>
</html>
