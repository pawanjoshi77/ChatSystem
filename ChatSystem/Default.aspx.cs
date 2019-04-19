using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace ChatSystem
{
    public class GooglePlusAccessToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string id_token { get; set; }
        public string refresh_token { get; set; }
    }
    public class GoogleUserOutputData
    {
        public string id { get; set; }
        public string name { get; set; }
        public string given_name { get; set; }
        public string email { get; set; }
        public string picture { get; set; }
    }
    public partial class Default : System.Web.UI.Page
    {
        protected string googleplus_client_id = "538898332741-u032k2pc46v7d2k3nrqlejc8vve5175p.apps.googleusercontent.com";    // Replace this with your Client ID
        protected string googleplus_client_secret = "oJzriGpa5Uc_-J9Az-s89hPl";                                                // Replace this with your Client Secret
        protected string googleplus_redirect_url = "http://localhost:50829/Default.aspx";                                         // Replace this with your Redirect URL; Your Redirect URL from your developer.google application should match this URL.
        protected string Parameters;

        private static readonly byte[] salt = Encoding.ASCII.GetBytes("This is My Salt value");
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["username"] = txt_username.Text;
            if ((Session.Contents.Count > 0) && (Session["loginWith"] != null) && (Session["loginWith"].ToString() == "google"))
            {
                try
                {
                    GoogleUserOutputData serStatus1 = new GoogleUserOutputData();
                    string json_data = string.Empty;
                    var url = Request.Url.Query;
                    if (url != "")
                    {
                        string queryString = url.ToString();
                        char[] delimiterChars = { '=' };
                        string[] words = queryString.Split(delimiterChars);
                        string code = words[1];

                        if (code != null)
                        {
                            //get the access token 
                            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("https://accounts.google.com/o/oauth2/token");
                            webRequest.Method = "POST";
                            Parameters = "code=" + code + "&client_id=" + googleplus_client_id + "&client_secret=" + googleplus_client_secret + "&redirect_uri=" + googleplus_redirect_url + "&grant_type=authorization_code";
                            byte[] byteArray = Encoding.UTF8.GetBytes(Parameters);
                            webRequest.ContentType = "application/x-www-form-urlencoded";
                            webRequest.ContentLength = byteArray.Length;
                            Stream postStream = webRequest.GetRequestStream();
                            // Add the post data to the web request
                            postStream.Write(byteArray, 0, byteArray.Length);
                            postStream.Close();

                            WebResponse response = webRequest.GetResponse();
                            postStream = response.GetResponseStream();
                            StreamReader reader = new StreamReader(postStream);
                            string responseFromServer = reader.ReadToEnd();

                            GooglePlusAccessToken serStatus = JsonConvert.DeserializeObject<GooglePlusAccessToken>(responseFromServer);

                            if (serStatus != null)
                            {
                                string accessToken = string.Empty;
                                accessToken = serStatus.access_token;

                                if (!string.IsNullOrEmpty(accessToken))
                                {
                                    using (var w = new WebClient())
                                    {
                                        json_data = w.DownloadString("https://www.googleapis.com/oauth2/v1/userinfo?access_token=" + accessToken);
                                        serStatus1 = JsonConvert.DeserializeObject<GoogleUserOutputData>(json_data);
                                    }
                                    // This is where you want to add the code if login is successful.
                                    // getgoogleplususerdataSer(accessToken);
                                    Session["username"] = serStatus1.name;
                                    Response.Redirect("chatroom.aspx");

                                }
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    //throw new Exception(ex.Message, ex);
                    Response.Redirect("chatroom.aspx");
                }
            }
         }
        protected void Google_Click(object sender, EventArgs e)
        {
            var Googleurl = "https://accounts.google.com/o/oauth2/auth?response_type=code&redirect_uri=" + googleplus_redirect_url + "&scope=https://www.googleapis.com/auth/userinfo.email%20https://www.googleapis.com/auth/userinfo.profile&client_id=" + googleplus_client_id;
            Session["loginWith"] = "google";
            Response.Redirect(Googleurl);
        }

        protected void btn_login_Click(object sender, EventArgs e)
        {

            string path = Request.PhysicalApplicationPath + "App_Data/users.xml";
            XmlDocument doc = new XmlDocument();

            if (System.IO.File.Exists(path))
            {
                doc.Load(path);
                XmlNode root = doc.SelectSingleNode("userlist");
                XmlNode users = CreateUsersNode(doc);
                root.AppendChild(users);
                //lblOutput.Text = "User added";
            }
            else
            {
                XmlNode decTag = doc.CreateXmlDeclaration("1.0", "utf-8", "");
                doc.AppendChild(decTag);
                XmlNode root = doc.CreateElement("userlist");
                XmlNode users = CreateUsersNode(doc);
                root.AppendChild(users);
                doc.AppendChild(root);
                //lblOutput.Text = "XML file created and user added successfully";
            }
            doc.Save(path);

            Response.Redirect("chatroom.aspx");

        }
        XmlNode CreateUsersNode(XmlDocument doc)
        {
            XmlNode users = doc.CreateElement("users");
            XmlNode username = doc.CreateElement("username");
            username.InnerText = txt_username.Text;
            

            XmlNode password = doc.CreateElement("password");
            string plainTextPasswd = txt_password.Text;
            byte[] hashedPasswd = Hash(plainTextPasswd, salt);
            byte[] hashedEncryptedPasswd = Hash(hashedPasswd, salt);
            string encryptedPasswd = Convert.ToBase64String(hashedEncryptedPasswd);
            password.InnerText = encryptedPasswd;
           users.AppendChild(username);
            users.AppendChild(password);
           bool tstPasswd = ConfirmPassword(plainTextPasswd, encryptedPasswd);
            return users;
        }

        public static byte[] Hash(string value, byte[] salt)
        {
            return Hash(Encoding.UTF8.GetBytes(value), salt);
        }

        public static byte[] Hash(byte[] value, byte[] salt)
        {
            byte[] saltedValue = value.Concat(salt).ToArray();
            // Alternatively use CopyTo.
            //var saltedValue = new byte[value.Length + salt.Length];
            //value.CopyTo(saltedValue, 0);
            //salt.CopyTo(saltedValue, value.Length);

            return new SHA256Managed().ComputeHash(saltedValue);
        }

        public bool ConfirmPassword(string password, string storedPasswd)
        {
            byte[] hashedPasswd = Hash(password, salt);
            byte[] hashedEncryptedPasswd = Hash(hashedPasswd, salt);
            string encryptedPasswd = Convert.ToBase64String(hashedEncryptedPasswd);
            return storedPasswd.Equals(encryptedPasswd);
        }

    }
}