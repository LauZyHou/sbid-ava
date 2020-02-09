using ReactiveUI;
using sbid._M;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    public class Axiom_EW_VM : ViewModelBase
    {
        private Axiom axiom;
        private ObservableCollection<_M.Type> types = new ObservableCollection<_M.Type>();
        private ObservableCollection<Attribute> @params = new ObservableCollection<Attribute>();

        // 要编辑的公理
        public Axiom Axiom { get => axiom; set => axiom = value; }
        // 所有Type对象(包括int, bool),作为Attribute/Method/CommMethod的可用类型
        public ObservableCollection<sbid._M.Type> Types { get => types; set => types = value; }
        // Method参数列表绑定此处
        public ObservableCollection<Attribute> Params { get => @params; set => this.RaiseAndSetIfChanged(ref @params, value); }
    }
}
