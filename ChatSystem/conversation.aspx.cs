using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace ChatSystem
{
    public partial class conversation : System.Web.UI.Page
    {
        static string _userName;
        static string _roomId;
        static string _xmlpath;
        protected void Page_Load(object sender, EventArgs e)
        {
            string userName = Session["username"] != null ? Session["username"].ToString(): "";
            string roomId = Request.QueryString["roomId"];// to do check null value
            if (roomId == null)
            {
                roomId = Session["roomId"].ToString();
            }
            else
            {
                Session["roomId"] = roomId;
            }
            _userName = userName;
            _roomId = roomId;
            string xmlpath = Request.PhysicalApplicationPath + "App_Data/conversation.xml";
            _xmlpath = xmlpath;
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlpath);
            string message = getMessagesForUserFromXml(doc, roomId, userName);
            chatbox.InnerHtml = message;
        }

        static string getMessagesForUserFromXml(XmlDocument doc, string roomId, string userName )
        {
            string message = "";

            XmlNode messages = doc.SelectSingleNode(string.Format("/conversation/chat[@roomId='{0}']/user[@name='{1}']/messages", roomId, userName));
            if (messages != null)
            {
                foreach (XmlNode nodei in messages)
                {

                    foreach (XmlNode nodex in nodei.ChildNodes)
                    {
                        if (nodex.Name.Equals("text"))
                        {
                            message += nodex.InnerText + "<br/>";
                        }
                    }
                }
            }
            return message;
        }

        [System.Web.Services.WebMethod]
        public static string btn_submit_Click(string _message)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(_xmlpath);
            XmlNode messages = doc.SelectSingleNode(string.Format("/conversation/chat[@roomId='{0}']/user[@name='{1}']/messages", _roomId, _userName));
            if (messages != null)
            {
                XmlNode message = GetMessage(_message, doc);
                messages.AppendChild(message);
            }
            else
            {
                XmlNode roomNode = doc.SelectSingleNode(string.Format("/conversation/chat[@roomId='{0}']", _roomId));
                XmlNode userNode = doc.CreateElement("user");
                XmlAttribute attribute = doc.CreateAttribute("name");
                attribute.InnerText = _userName;
                userNode.Attributes.Append(attribute);

                XmlNode messagesNode = doc.CreateElement("messages");

                XmlNode message = GetMessage(_message, doc);
                messagesNode.AppendChild(message);
                userNode.AppendChild(messagesNode);
                roomNode.AppendChild(userNode);
                
            }
            doc.Save(_xmlpath);
            string messageList = getMessagesForUserFromXml(doc, _roomId, _userName);
            return messageList;
        }

        private static XmlNode GetMessage(string _message, XmlDocument doc)
        {
            return CreateMessageNode(_message, doc);
        }

        static XmlNode CreateMessageNode(String _message, XmlDocument doc)
        {
            XmlNode message = doc.CreateElement("message");
            XmlNode text = doc.CreateElement("text");
            text.InnerText = _message;
            // 2019-02-06T09:02:00
            string dateTime = DateTime.Now.ToString();
            XmlAttribute attribute = doc.CreateAttribute("date");
            attribute.InnerText = dateTime;
            message.Attributes.Append(attribute);
            message.AppendChild(text);
            return message;
        }
    }

}