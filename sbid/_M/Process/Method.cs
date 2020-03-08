using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._M
{
    // 方法的加解密方式枚举
    public enum Crypto
    {
        None, AES, SHA256
    }

    // Process或Axiom中使用的方法
    public class Method : ReactiveObject
    {
        private Type returnType;
        private string name;
        private ObservableCollection<Attribute> parameters; // 这里不创建,在构造时传入
        private Crypto cryptoSuffix;

        public Method(Type returnType, string name, ObservableCollection<Attribute> parameters, Crypto cryptoSuffix = Crypto.None)
        {
            this.returnType = returnType;
            this.name = name;
            this.parameters = parameters;
            this.cryptoSuffix = cryptoSuffix;
        }

        // 返回值
        public Type ReturnType
        {
            get => returnType;
            set
            {
                this.RaiseAndSetIfChanged(ref returnType, value);
                this.RaisePropertyChanged("ShowString");
            }
        }

        // 方法名
        public string Name
        {
            get => name;
            set
            {
                this.RaiseAndSetIfChanged(ref name, value);
                this.RaisePropertyChanged("ShowString");
            }
        }

        // 参数列表
        public ObservableCollection<Attribute> Parameters
        {
            get => parameters;
            set
            {
                this.RaiseAndSetIfChanged(ref parameters, value);
                this.RaisePropertyChanged("ShowString");
            }
        }

        // 加解密方式(可选)
        public Crypto CryptoSuffix
        {
            get => cryptoSuffix;
            set
            {
                this.RaiseAndSetIfChanged(ref cryptoSuffix, value);
                this.RaisePropertyChanged("ShowString");
            }
        }

        // 展示串(不带Crypto)
        public string ShowString
        {
            get
            {
                string paramString = "";
                if (parameters != null && parameters.Count > 0)
                {
                    paramString = parameters[0].ToString();
                    for (int i = 1; i < parameters.Count; i++)
                    {
                        paramString += ", " + parameters[i].ToString();
                    }
                }
                return returnType.Name + " " + name + "(" + paramString + ")";
            }
        }

        public override string ToString()
        {
            string paramString = "";
            foreach (Attribute attribute in parameters)
            {
                paramString += attribute + ", ";            
            }
            // 去除末尾", "
            paramString = paramString.TrimEnd(", ".ToCharArray());
            string retString = returnType.Name + " " + name + "(" + paramString + ");";
            if (cryptoSuffix != Crypto.None) // None不显示
                retString += "[" + cryptoSuffix + "]";
            return retString;
        }

        #region 静态成员和静态构造

        // 内置Method
        public static readonly List<Method> InnerMethods = new List<Method>();
        // 静态构造
        static Method()
        {
            // 添加内置Method
            // enc
            ObservableCollection<Attribute> encParams = new ObservableCollection<Attribute>();
            encParams.Add(new Attribute(Type.TYPE_INT, "key"));
            encParams.Add(new Attribute(Type.TYPE_INT, "msg"));
            Method enc = new Method(Type.TYPE_INT, "enc", encParams, Crypto.None);
            InnerMethods.Add(enc);
            // dec
            ObservableCollection<Attribute> decParams = new ObservableCollection<Attribute>();
            decParams.Add(new Attribute(Type.TYPE_INT, "key"));
            Method dec = new Method(Type.TYPE_INT, "dec", decParams, Crypto.None);
            InnerMethods.Add(dec);
        }

        #endregion
    }
}
