using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._M
{
    // 进程模板
    public class Process : ReactiveObject
    {
        public static int _id = 0;
        private Formula refName;
        private ObservableCollection<Attribute> attributes = new ObservableCollection<Attribute>();
        private ObservableCollection<Method> methods = new ObservableCollection<Method>();
        private ObservableCollection<CommMethod> commMethods = new ObservableCollection<CommMethod>();

        private ObservableCollection<State> states;
        private int id;


        public Process()
        {
            _id++;
            this.id = _id;
            this.refName = new Formula("P" + this.id);
            //test_data();
        }

        // 进程模板名，使用引用型Name，因为需要状态机SidePanel面板名自动同步
        public Formula RefName { get => refName; }
        // 成员参数列表
        public ObservableCollection<Attribute> Attributes { get => attributes; set => attributes = value; }
        // 方法列表
        public ObservableCollection<Method> Methods { get => methods; set => methods = value; }
        // 通信方法列表
        public ObservableCollection<CommMethod> CommMethods { get => commMethods; set => commMethods = value; }

        // *对应的状态机上的所有状态(这是给两类Property中编辑时,选择Process里的状态用)
        // 这个字段不是实时计算的,仅在编辑两类Property窗口打开前重新计算,并重新写入
        public ObservableCollection<State> States { get => states; set => states = value; }
        public int Id
        {
            get => id;
            set
            {
                id = value;
                if (value > _id)
                    _id = value;
            }
        }
        

        // Process的所有UserType型Attribute的列表，给认证性质使用
        // 因为只有UserType型Attribute才有二级属性可言
        public List<Attribute> UserTypeAttributes
        {
            get
            {
                List<Attribute> list = new List<Attribute>();
                foreach (Attribute attribute in attributes)
                {
                    if (attribute.Type is UserType)
                    {
                        list.Add(attribute);
                    }
                }
                return list;
            }
        }

        private void test_data()
        {
            attributes.Add(new Attribute(Type.TYPE_INT, "a"));
            attributes.Add(new Attribute(Type.TYPE_BOOL, "b"));

            ObservableCollection<Attribute> parameters1 = new ObservableCollection<Attribute>();
            parameters1.Add(new Attribute(Type.TYPE_INT, "msg"));
            parameters1.Add(new Attribute(Type.TYPE_BOOL, "key"));
            methods.Add(new Method(Type.TYPE_INT, "enc", parameters1, Crypto.SHA256));

            ObservableCollection<Attribute> parameters2 = new ObservableCollection<Attribute>();
            parameters2.Add(new Attribute(Type.TYPE_INT, "msg"));
            commMethods.Add(new CommMethod("send", parameters2, InOut.Out, CommWay.UDP));
        }
    }
}
