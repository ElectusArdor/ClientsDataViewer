using System.Collections.Generic;
using System.Windows;

namespace Ex10
{
    class Consultant : User
    {
        public override string Name { get { return "Консультант"; } }

        private string[] accessList = { "PhoneNum" };
        private string[] showInfoMask = { "Passport" };
        private Dictionary<string, Visibility> buttonsVisibility =
            new Dictionary<string, Visibility> { { "ChangeBtn", Visibility.Visible }, { "NewClientBtn", Visibility.Hidden } };

        public override Dictionary<string, Visibility> ButtonsVisibility => buttonsVisibility;
        public override string[] AccessList => accessList;
        public override string[] HiddenInfoMask => showInfoMask;
    }
}
