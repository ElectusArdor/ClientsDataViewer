using System.Collections.Generic;
using System.Windows;
using System.Xml;

namespace Ex10
{
    internal static class XmlHandler
    {
        public static XmlNode Serilize(XmlDocument doc, Client client)
        {
            var clientNode = doc.CreateElement("Client");

            var id = doc.CreateAttribute("ID");
            id.Value = client.ID.ToString();
            clientNode.Attributes.Append(id);

            foreach (Property p in client.Properties)
            {
                var node = doc.CreateElement(p.Name);
                node.InnerText = p.Content;

                var noteDate = doc.CreateAttribute("NoteDate");
                noteDate.Value = p.NoteDate;
                node.Attributes.Append(noteDate);

                var changeType = doc.CreateAttribute("ChangeType");
                changeType.Value = p.ChangeType;
                node.Attributes.Append(changeType);

                var whoChanged = doc.CreateAttribute("WhoChanged");
                whoChanged.Value = p.WhoChanged;
                node.Attributes.Append(whoChanged);

                clientNode.AppendChild(node);
            }

            return clientNode;
        }

        public static Client Deserilize(XmlNode el)
        {
            uint id;
            if (uint.TryParse(el.Attributes["ID"].Value, out id))
            {
                string[] propNames = Client.ClientDataModel;
                Dictionary<string, Property> properties = new Dictionary<string, Property>();
                foreach (string propName in propNames)
                {
                    XmlNode node = el.SelectSingleNode(propName);
                    Property prop = new Property(node.Name, node.InnerText,
                        node.Attributes["WhoChanged"].Value, node.Attributes["NoteDate"].Value, node.Attributes["ChangeType"].Value);
                    properties.Add(propName, prop);
                }

                Client client = new Client(properties, id);
                return client;
            }
            else { MessageBox.Show($"Ошибка парсинга ID, ID = {el.Attributes["ID"].Value}"); }
            return null;
        }
    }
}
