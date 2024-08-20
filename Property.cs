using System;

namespace Ex10
{
    /// <summary>
    /// Контейнер для хранения данных о клиенте.
    /// </summary>
    internal class Property
    {
        public string Name { get; private set; }
        public string Content { get; private set; }
        public string NoteDate { get; private set; }
        public string ChangeType { get; private set; }
        public string WhoChanged { get; private set; }

        public Property(string name, string content, string whoChanged)
        {
            this.Name = name;
            Renew(content, "новый", whoChanged);
        }

        public Property(string name, string content, string whoChanged, string noteDate, string changeType)
        {
            this.Name = name;
            this.Content = content;
            this.WhoChanged = whoChanged;
            this.ChangeType = changeType;
            this.NoteDate = noteDate;
        }

        public void Renew(string content, string changeType, string whoChanged)
        {
            Content = content;
            NoteDate = DateTime.Now.ToString();
            ChangeType = changeType;
            WhoChanged = whoChanged;
        }
    }
}
