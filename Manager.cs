using System.Collections.Generic;
using System.Windows;

namespace Ex10
{
    class Manager : Consultant, IAdmin
    {
        public override string Name { get { return "Менеджер"; } }

        private string[] accessList = { "FirstName", "LastName", "Patronymic", "PhoneNum", "Passport" };
        private string[] showInfoMask = { };
        private Dictionary<string, Visibility> buttonsVisibility =
            new Dictionary<string, Visibility> { { "ChangeBtn", Visibility.Visible }, { "NewClientBtn", Visibility.Visible } };

        public override Dictionary<string, Visibility> ButtonsVisibility => buttonsVisibility;
        public override string[] AccessList => accessList;
        public override string[] HiddenInfoMask => showInfoMask;

        public void NewClient(Client client)
        {
            Repository.AddNote(client, "data.txt");
        }
    }
}
