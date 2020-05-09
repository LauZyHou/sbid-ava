using Avalonia.Media;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._M
{
    public class TopoNode : ReactiveObject
    {
        public static int _id = 0;
        private string name;
        private Process process;
        private ObservableCollection<Instance> properties = new ObservableCollection<Instance>();
        private int id;
        private IBrush color = Brushes.LightPink;

        public TopoNode()
        {
            _id++;
            this.id = _id;
            this.name = "T" + this.id;
        }

        public string Name { get => name; set => this.RaiseAndSetIfChanged(ref name, value); }
        public Process Process { get => process; set => this.RaiseAndSetIfChanged(ref process, value); }
        // 属性列表，用于例化Process的实例，参见ReferenceInstance
        public ObservableCollection<Instance> Properties { get => properties; }
        // 结点颜色(实际上这应该是TopoNode_VM的内容，但是为了EW里修改方便，就放在这里了)
        // 从语义上来说，也可以理解成这就是结点的内容，毕竟这是用户对结点形式的定制
        public IBrush Color { get => color; set => this.RaiseAndSetIfChanged(ref color, value); }
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

        #region 静态成员和静态构造

        // 结点的颜色列表
        public static readonly List<IBrush> NodeColors = new List<IBrush>();
        // 静态构造
        static TopoNode()
        {
            NodeColors.Add(Brushes.LightPink);
            NodeColors.Add(Brushes.LightBlue);
            NodeColors.Add(Brushes.LightGreen);
            NodeColors.Add(Brushes.LightYellow);
            NodeColors.Add(Brushes.LightCyan);
        }

        #endregion
    }
}
