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
        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Session["username"] != null ? Session["username"].ToString(): "";
            string roomId = Request.QueryString["roomId"];// to do check null value
            if (roomId == null)
            {
                roomId = Session["roomId"].ToString();
            }
            else
            {
                Session["roomId"] = roomId;
            }
            string xmlpath = Request.PhysicalApplicationPath + "App_Data/conversation.xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlpath);
            string message = "";

            XmlNode messages = doc.SelectSingleNode(string.Format("/conversation/chat[@roomId='{0}']/user[@name='{1}']/messages", roomId, username));
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

                chatbox.InnerHtml = message;
            }
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            string username = Session["username"].ToString();
            string roomId = Session["roomId"].ToString();

            string path = Request.PhysicalApplicationPath + "App_Data/conversation.xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNode messages = doc.SelectSingleNode(string.Format("/conversation/chat[@roomId='{0}']/user[@name='{1}']/messages", roomId, username));
            if (messages != null)
            {               
                XmlNode message = CreateMessageNode(doc);
                messages.AppendChild(message);
                lblOutput.Text = "Message added";
            }
            else
            {
                XmlNode roomNode = doc.SelectSingleNode(string.Format("/conversation/chat[@roomId='{0}']", roomId));
                XmlNode userNode = doc.CreateElement("user");
                XmlAttribute attribute = doc.CreateAttribute("name");
                attribute.InnerText = username;
                userNode.Attributes.Append(attribute);

                XmlNode messagesNode = doc.CreateElement("messages");

                XmlNode message = CreateMessageNode(doc);
                messagesNode.AppendChild(message);
                userNode.AppendChild(messagesNode);
                roomNode.AppendChild(userNode);
                lblOutput.Text = "Message added";
            }
            doc.Save(path);
            Response.Redirect("conversation.aspx");

        }

        XmlNode CreateMessageNode(XmlDocument doc)
        {
            XmlNode message = doc.CreateElement("message");
            XmlNode text = doc.CreateElement("text");
            text.InnerText = txt_message.Text;
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