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
            <input id="btn_submit" type="button" value="Submit"
            onclick = "SubmitMessage()" />
        </div>
    </form>
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js" type="text/javascript"></script>
    <script type = "text/javascript">
        function SubmitMessage() {
            $.ajax({
                type: "POST",
                url: "conversation.aspx/btn_submit_Click",
                data: '{_message: "' + $("#txt_message").val() + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: function (response) {
                    alert(response.d);                }
            });
        }
        function OnSuccess(response) {
                 $("#chatbox").html(response.d);
        }
</script>

</body>
</html>
