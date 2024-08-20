using System.Collections.Generic;
using System.Xml;

namespace Ex10
{
    internal static class Repository
    {
        public static XmlDocument CreateFile(string path)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode rootNode = doc.CreateElement("root");
            doc.AppendChild(rootNode);
            doc.Save(path);
            return doc;
        }

        public static XmlDocument LoadFile(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            return doc;
        }

        public static void AddNote(Client client, string path)
        {
            XmlDocument doc = LoadFile(path);
            if (doc.DocumentElement == null) { doc = CreateFile(path); }

            var clientNode = doc.ImportNode(XmlHandler.Serilize(doc, client), true);
            var rootNode = doc.DocumentElement;
            rootNode.AppendChild(clientNode);
            doc.Save(path);
        }

        public static void UpdateNote(Client client, string path)
        {
            XmlDocument doc = LoadFile(path);

            XmlElement root = doc.DocumentElement;
            root.SelectSingleNode($"Client[@ID='{client.ID}']").InnerXml = XmlHandler.Serilize(doc, client).InnerXml;
            doc.Save(path);
        }

        public static List<Client> GetClientsList(string path)
        {
            List<Client> clientsList = new List<Client>();
            XmlNodeList clients = LoadFile(path).DocumentElement.GetElementsByTagName("Client");
            foreach (XmlNode clientNode in clients)
            {
                clientsList.Add(XmlHandler.Deserilize(clientNode));
            }
            return clientsList;
        }
    }
}
