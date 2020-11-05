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

        #region 静态成员和静态构造

        // 内置公理
        public static readonly List<Formula> InnerFormulas = new List<Formula>();
        // 静态构造
        static Axiom()
        {
            // 对称加密->对称解密
            InnerFormulas.Add(new Formula("SymDec(SymEnc(m,k),k)=m"));
            // 公钥签名->私钥验证
            InnerFormulas.Add(new Formula("Verify(Sign(m,sk),pk)=True"));
            // 非对称加密->非对称解密
            InnerFormulas.Add(new Formula("AsymDec(AsymEnc(m,pk),sk)=m"));
        }

        #endregion
    }
}
