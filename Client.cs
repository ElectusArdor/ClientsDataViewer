using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Ex10
{
    /// <summary>
    /// Клиент банка.
    /// </summary>
    internal class Client
    {
        public static string[] ClientDataModel {  get; private set; } = { "FirstName", "LastName", "Patronymic", "PhoneNum", "Passport" };
        public uint ID { get; private set; }
        public Property FirstName { get; private set; }
        public Property LastName { get; private set; }
        public Property Patronymic { get; private set; }
        public Property PhoneNum { get; private set; }
        public Property Passport { get; private set; }
        public Property[] Properties { get; private set; }

        public Client(string firstName, string lastName, string patronymic, string phoneNum, string passport, string changer, uint id)
        {
            FirstName = new Property("FirstName", firstName, changer);
            LastName = new Property("LastName", lastName, changer);
            Patronymic = new Property("Patronymic", patronymic, changer);
            PhoneNum = new Property("PhoneNum", phoneNum, changer);
            Passport = new Property("Passport", passport, changer);
            ID = id;
            Properties = new[] { FirstName, LastName, Patronymic, PhoneNum, Passport };
        }

        public Client(Dictionary<string, Property> properties, uint id)
        {
            FirstName = properties["FirstName"];
            LastName = properties["LastName"];
            Patronymic = properties["Patronymic"];
            PhoneNum = properties["PhoneNum"];
            Passport = properties["Passport"];
            ID = id;
            Properties = new[] { FirstName, LastName, Patronymic, PhoneNum, Passport };
        }

        public void UpdateData(Client client, string changer)
        {
            foreach (var newProperty in client.Properties)
            {
                Property currentProperty = Properties.FirstOrDefault(a => a.Name == newProperty.Name);
                if (currentProperty.Content != newProperty.Content & newProperty.Content != "")
                    currentProperty.Renew(newProperty.Content, "изменено", changer);
            }
        }

        public bool Check()
        {
            foreach (var property in Properties)
            {
                if (property.Content == "")
                {
                    MessageBox.Show("Заполните все поля");
                    return false;
                }
            }
            return true;
        }

        public override string ToString()
        {
            return $"{ID, 4} {LastName.Content} {FirstName.Content} {Patronymic.Content}";
        }
    }
}
