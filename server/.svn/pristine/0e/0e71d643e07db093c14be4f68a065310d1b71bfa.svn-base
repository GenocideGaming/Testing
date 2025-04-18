using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Server.Scripts.Custom.WebService
{
    public class FeedbackXmlGenerator
    {
        public static string GenerateXmlString(Mobile from, object target, string feedbackText)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration dec = xmlDoc.CreateXmlDeclaration("1.0", null, null);
            XmlElement feedbackPost = xmlDoc.CreateElement("feedback_post");

            xmlDoc.AppendChild(dec);
            xmlDoc.AppendChild(feedbackPost);

            XmlElement accountName = xmlDoc.CreateElement("account_name");
            XmlElement playerName = xmlDoc.CreateElement("player_name");
            XmlElement feedback = xmlDoc.CreateElement("feedback");
            XmlElement targetType = xmlDoc.CreateElement("target_type");
            XmlElement itemID = xmlDoc.CreateElement("item_id");
            XmlElement locationX = xmlDoc.CreateElement("location_x");
            XmlElement locationY = xmlDoc.CreateElement("location_y");
            XmlElement locationZ = xmlDoc.CreateElement("location_z");

            accountName.InnerText = from.Account.Username;
            playerName.InnerText = from.Name;
            feedback.InnerText = feedbackText;
            targetType.InnerText = target != null ? target.GetType().ToString() : "no target";

            Item item = target != null ? target as Item : null;
            Mobile mob = target != null ? target as Mobile : null;
            

            if (item != null)
            {
                itemID.InnerText = item.ItemID.ToString();
                locationX.InnerText = item.Location.X.ToString();
                locationY.InnerText = item.Location.Y.ToString();
                locationZ.InnerText = item.Location.Z.ToString();
            }
            else if (mob != null)
            {
                itemID.InnerText = "0";
                locationX.InnerText = mob.Location.X.ToString();
                locationY.InnerText = mob.Location.Y.ToString();
                locationZ.InnerText = mob.Location.Z.ToString();
            }
            else
            {
                itemID.InnerText = "0";
                locationX.InnerText = from.Location.X.ToString();
                locationY.InnerText = from.Location.Y.ToString();
                locationZ.InnerText = from.Location.Z.ToString();
            }
        

            feedbackPost.AppendChild(accountName);
            feedbackPost.AppendChild(playerName);
            feedbackPost.AppendChild(feedback);
            feedbackPost.AppendChild(targetType);
            feedbackPost.AppendChild(itemID);
            feedbackPost.AppendChild(locationX);
            feedbackPost.AppendChild(locationY);
            feedbackPost.AppendChild(locationZ);

            return xmlDoc.OuterXml;
        }
    }
}
