using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._M
{
    // 公理
    public class Axiom : ReactiveObject
    {
        private string name;
        private ObservableCollection<Method> methods = new ObservableCollection<Method>();
        private ObservableCollection<Formula> formulas = new ObservableCollection<Formula>();

        public Axiom()
        {
            test_data();
        }

        public Axiom(string name)
        {
            this.name = name;
            test_data();
        }

        public string Name { get => name; set => this.RaiseAndSetIfChanged(ref name, value); }
        public ObservableCollection<Method> Methods { get => methods; set => methods = value; }
        public ObservableCollection<Formula> Formulas { get => formulas; set => formulas = value; }

        private void test_data()
        {
            ObservableCollection<Attribute> parameters1 = new ObservableCollection<Attribute>();
            parameters1.Add(new Attribute(Type.TYPE_INT, "msg"));
            parameters1.Add(new Attribute(Type.TYPE_BOOL, "key"));
            methods.Add(new Method(Type.TYPE_INT, "enc1", parameters1));

            ObservableCollection<Attribute> parameters2 = new ObservableCollection<Attribute>();
            parameters2.Add(new Attribute(Type.TYPE_INT, "msg"));
            methods.Add(new Method(Type.TYPE_BOOL, "send1", parameters2));

            formulas.Add(new Formula("dec1(enc1(m,k),k)=m"));
        }
    }
}
