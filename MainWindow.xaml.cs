using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;

namespace Ex10
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private User user;

        private delegate void SaveData();
        private SaveData saveData;

        private uint nextID;

        private string filePath = "data.txt";
        private Dictionary<string, User> users = new Dictionary<string, User>() {
            { "Консультант", new Consultant() },
            { "Менеджер", new Manager() } };

        private List<Client> clients = new List<Client>();

        private BrushConverter bc = new BrushConverter();
        private Client currentClient;
        private string[] allAccesses = { "FirstName", "LastName", "Patronymic", "PhoneNum", "Passport" };

        public MainWindow()
        {
            InitializeComponent();

            if (!File.Exists(filePath))
            {
                Repository.CreateFile(filePath);
                nextID = 0;
            }
            else
            {
                clients = Repository.GetClientsList(filePath);
                currentClient = clients[0];
                ShowClientsList(clients);
                nextID = clients[clients.Count - 1].ID + 1;
            }

            ClientsList.SelectionChanged += SelectClient;
        }

        /// <summary>
        /// Изменение уровня доступа сотрудника.
        /// </summary>
        private void SetAccessLvl(object sender, SelectionChangedEventArgs e)
        {
            string accessLvl = (AccessSelector.SelectedValue as TextBlock).Text;
            user = users[accessLvl];

            if (currentClient != null) ShowInfo(currentClient);
            else ClearForm();
            ChangeData(allAccesses, false);

            ButtonsVis(Visibility.Hidden, user.ButtonsVisibility["ChangeBtn"], user.ButtonsVisibility["NewClientBtn"]);
            MessageBox.Show($"Выбран уровень доступа \"{accessLvl}\"");
        }

        /// <summary>
        /// Вывод данных клыента в форму.
        /// </summary>
        /// <param name="client"></param>
        private void ShowInfo(Client client)
        {
            Dictionary<string, string> data = user.ShowData(client);
            FirstName.Text = data["FirstName"];
            NameInfo.Text = $"{currentClient.FirstName.ChangeType} от {currentClient.FirstName.NoteDate} {currentClient.FirstName.WhoChanged}";
            LastName.Text = data["LastName"];
            LastNameInfo.Text = $"{currentClient.LastName.ChangeType} от {currentClient.LastName.NoteDate} {currentClient.LastName.WhoChanged}";
            Patronymic.Text = data["Patronymic"];
            PatronymicInfo.Text = $"{currentClient.Patronymic.ChangeType} от {currentClient.Patronymic.NoteDate} {currentClient.Patronymic.WhoChanged}";
            PhoneNum.Text = data["PhoneNum"];
            PhoneNumInfo.Text = $"{currentClient.PhoneNum.ChangeType} от {currentClient.PhoneNum.NoteDate} {currentClient.PhoneNum.WhoChanged}";
            Passport.Text = data["Passport"];
            PassportInfo.Text = $"{currentClient.Passport.ChangeType} от {currentClient.Passport.NoteDate} {currentClient.Passport.WhoChanged}";
            ID.Text = client.ID.ToString();
        }

        /// <summary>
        /// Выводит список клиентов.
        /// </summary>
        /// <param name="clients"></param>
        private void ShowClientsList(List<Client> clients)
        {
            List<string> clientsNameList = new List<string>();
            foreach (Client client in clients)
            {
                clientsNameList.Add(client.ToString());
            }
            ClientsList.ItemsSource = clientsNameList;
        }

        /// <summary>
        /// Переход к вводу данных нового клиента.
        /// </summary>
        private void CreateClient(object sender, RoutedEventArgs e)
        {
            ButtonsVis(Visibility.Visible, Visibility.Hidden, Visibility.Hidden);
            ClearForm();

            string[] accessList = user.AccessList;
            ChangeData(user.AccessList, true);
            saveData = SaveNewClient;
        }

        /// <summary>
        /// Переход в режим редактирования.
        /// </summary>
        private void EditData(object sender, RoutedEventArgs e)
        {
            ButtonsVis(Visibility.Visible, Visibility.Hidden, Visibility.Hidden);
            ChangeData(user.AccessList, true);
            saveData = SaveChanges;
        }

        /// <summary>
        /// Включение/отключение редактирования данных в TextBox.
        /// </summary>
        /// <param name="accessList">Список полей, к которым есть доступ на изменение</param>
        /// <param name="editable">Флаг включения/отключения режима редактирования TextBox</param>
        private void ChangeData(string[] accessList, bool editable)
        {
            foreach (var x in DataGrid.Children)
            {
                if (x is TextBox)
                {
                    if (accessList.Contains((x as TextBox).Name))
                    {
                        (x as TextBox).IsReadOnly = !editable;
                        (x as TextBox).Background = editable ? (Brush)bc.ConvertFrom("#FFCCFFCC"): (Brush)bc.ConvertFrom("#FFFFFFFF");
                    }
                }
            }
        }

        /// <summary>
        /// Изменение видимости кнопок интерфейса.
        /// </summary>
        /// <param name="editBtnsVis">Флаг видимости кнопок "Сохранить" и "Отмена"</param>
        /// <param name="changeBtnVis">Флаг видимости кнопки "Изменить"</param>
        /// <param name="newClientBtnVis">Флаг видимости кнопки "Новый клиент"</param>
        private void ButtonsVis(Visibility editBtnsVis, Visibility changeBtnVis, Visibility newClientBtnVis)
        {
            SaveBtn.Visibility = editBtnsVis;
            CancelBtn.Visibility = editBtnsVis;
            ChangeBtn.Visibility = changeBtnVis;
            NewClientBtn.Visibility = newClientBtnVis;
        }

        /// <summary>
        /// Отмена изменений данных клиента.
        /// </summary>
        private void Cancellation(object sender, RoutedEventArgs e)
        {
            ChangeData(allAccesses, false);
            ShowInfo(currentClient);

            ButtonsVis(Visibility.Hidden, user.ButtonsVisibility["ChangeBtn"], user.ButtonsVisibility["NewClientBtn"]);
        }

        /// <summary>
        /// Сохранение изменений данных клиента.
        /// </summary>
        private void SaveChanges()
        {
            Client renewClient = new Client(FirstName.Text, LastName.Text, Patronymic.Text,
                PhoneNum.Text, Passport.Text, user.Name, currentClient.ID);

            if(!renewClient.Check()) { return; }

            foreach (var property in renewClient.Properties)
            {
                if (!user.AccessList.Contains(property.Name))
                    property.Renew(currentClient.Properties.FirstOrDefault(a => a.Name == property.Name).Content, "", user.Name);
            }
            currentClient = user.EditData(currentClient, renewClient);
            Repository.UpdateNote(currentClient, filePath);
        }

        /// <summary>
        /// Сохранение нового клиента.
        /// </summary>
        private void SaveNewClient()
        {
            Client newClient = new Client(FirstName.Text, LastName.Text, Patronymic.Text,
                PhoneNum.Text, Passport.Text, user.Name, nextID++);
            if (newClient.Check())
            {
                (user as Manager).NewClient(newClient);
                currentClient = newClient;
                clients.Add(currentClient);
            }
        }

        /// <summary>
        /// запись данных в файл.
        /// </summary>
        private void Save(object sender, RoutedEventArgs e)
        {
            saveData.Invoke();
            ChangeData(allAccesses, false);
            ShowInfo(currentClient);
            ShowClientsList(clients);
            ButtonsVis(Visibility.Hidden, user.ButtonsVisibility["ChangeBtn"], user.ButtonsVisibility["NewClientBtn"]);
        }

        /// <summary>
        /// Очистка формы.
        /// </summary>
        private void ClearForm()
        {
            ID.Text = "";
            FirstName.Text = "";
            LastName.Text = "";
            Patronymic.Text = "";
            PhoneNum.Text = "";
            Passport.Text = "";
        }

        private void SelectClient(object sender, SelectionChangedEventArgs e)
        {
            Cancellation(null, null);
            currentClient = clients[ClientsList.SelectedIndex];
            ShowInfo(currentClient);
        }
    }
}
