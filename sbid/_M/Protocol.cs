using System;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._M
{
    public class Protocol
    {
        private string name;
        private ObservableCollection<UserType> userTypes = new ObservableCollection<UserType>();

        public Protocol(string name)
        {
            this.name = name;
        }

        public string Name { get => name; set => name = value; }
        public ObservableCollection<UserType> UserTypes { get => userTypes; set => userTypes = value; }
    }
}
