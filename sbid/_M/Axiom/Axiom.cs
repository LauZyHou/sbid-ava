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
        public static int _id = 0;
        private string name;
        private ObservableCollection<ProcessMethod> processMethods = new ObservableCollection<ProcessMethod>();
        private ObservableCollection<Formula> formulas = new ObservableCollection<Formula>();
        private int id;
        

        public Axiom()
        {
            _id++;
            this.id = _id;
            this.name = "Ax" + this.id;
            //test_data();
        }

        public string Name { get => name; set => this.RaiseAndSetIfChanged(ref name, value); }
        public ObservableCollection<ProcessMethod> ProcessMethods { get => processMethods; set => processMethods = value; }
        public ObservableCollection<Formula> Formulas { get => formulas; set => formulas = value; }
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

        private void test_data()
        {
            formulas.Add(new Formula("dec1(enc1(m,k),k)=m"));
        }
    }
}
