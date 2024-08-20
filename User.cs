using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Ex10
{
    /// <summary>
    /// Base class for all users.
    /// </summary>
    abstract class User
    {
        public abstract string Name { get; }

        public abstract Dictionary<string, Visibility> ButtonsVisibility { get; }

        /// <summary>
        /// Перечень доступных для изменения полей.
        /// </summary>
        public abstract string[] AccessList { get; }
        /// <summary>
        /// Перечень скрытых для просмотра полей.
        /// </summary>
        public abstract string[] HiddenInfoMask { get; }

        public string UserType { get; private set; }

        public User()
        {
            this.UserType = this.GetType().ToString();
        }

        public Dictionary<string,string> ShowData(Client client)
        {
            var data = new Dictionary<string, string>();
            foreach (var item in client.Properties)
            {
                if (HiddenInfoMask.Contains(item.Name))
                    data.Add(item.Name, "**********");
                else
                    data.Add(item.Name, item.Content);
            }
            return data;
        }

        /// <summary>
        /// Принимает экземпляр клиента и присваивает ему полученные новые данные.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="newData"></param>
        /// <returns></returns>
        public Client EditData(Client currentClient, Client renewClient)
        {
            currentClient.UpdateData(renewClient, this.UserType);
            return currentClient;
        }
    }
}