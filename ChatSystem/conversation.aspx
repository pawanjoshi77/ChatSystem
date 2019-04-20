<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="conversation.aspx.cs" Inherits="ChatSystem.conversation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <div class="text-center">
    <form id="form1" runat="server">
        <h1>Chat Conversation</h1>
        <asp:Label ID="lblOutput" runat="server" />
        <asp:XmlDataSource ID="xmlSource" runat="server" DataFile="App_Data/conversation.xml" />
          
       <div id="chatbox" class="center-block text-center" runat="server" style="border:2px solid red; margin-top:10px; background-color:white; height:300px;"></div>
        <div class="center-block text-center" style="margin-top: 20px;">
            <asp:TextBox ID="txt_message" runat="server" placeholder="Enter your message" class="form-control input-normal"></asp:TextBox>
        </div>
        <div class="center-block text-center">
            <input id="btn_submit" class="btn btn-primary" type="button" value="Submit"
            onclick = "SubmitMessage()" style="margin-top: 20px;" />
        </div>
    </form>
        </div>
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
        <script src="js/popper.min.js"></script>
        <script src="js/bootstrap.min.js"></script>
</html>
