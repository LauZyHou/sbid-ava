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
        // 不使用
        None, 
        // 对称加密算法
        AES, DES, 
        // 非对称加密算法 
        RSA, ECC, 
        // 哈希签名算法
        MD5, SHA1, SHA256
    }

    // Process或Axiom中使用的方法
    public class Method : ReactiveObject
    {
        public static int _id = 0;
        private Type returnType;
        private string name;
        private ObservableCollection<Attribute> parameters; // 这里不创建,在构造时传入
        private Crypto cryptoSuffix;
        private int id;

        public Method(Type returnType, string name, ObservableCollection<Attribute> parameters, Crypto cryptoSuffix = Crypto.None)
        {
            _id++;
            this.id = _id;
            this.returnType = returnType;
            this.name = name;
            this.parameters = parameters;
            this.cryptoSuffix = cryptoSuffix;
        }

        #region 属性

        // 返回值
        public Type ReturnType { get => returnType; set => this.RaiseAndSetIfChanged(ref returnType, value); }

        // 方法名
        public string Name { get => name; set => this.RaiseAndSetIfChanged(ref name, value); }

        // 参数列表
        public ObservableCollection<Attribute> Parameters
        {
            get => parameters;
            set
            {
                this.RaiseAndSetIfChanged(ref parameters, value);
                this.RaisePropertyChanged("ParamString");
            }
        }

        // 加解密方式(可选)
        public Crypto CryptoSuffix { get => cryptoSuffix; set => this.RaiseAndSetIfChanged(ref cryptoSuffix, value); }

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

        #endregion

        #region 展示串

        // 展示Parameters
        public string ParamString
        {
            get
            {
                string paramString = "";
                foreach (Attribute attribute in parameters)
                {
                    paramString += attribute + ", ";
                }
                return paramString.TrimEnd(", ".ToCharArray());
            }
        }

        public override string ToString()
        {
            string retString = returnType.Name + " " + name + "(" + ParamString + ")";
            if (cryptoSuffix != Crypto.None) // None不显示
                retString += "[" + cryptoSuffix + "]";
            return retString;
        }

        #endregion

        #region 静态成员和静态构造

        // 内置Method
        public static readonly List<Method> InnerMethods = new List<Method>();
        // 静态构造，实际使用时以它们为模板创建新的对象，里面内容深拷贝
        static Method()
        {
            // 添加内置Method
            // 对称加密
            ObservableCollection<Attribute> symEncParams = new ObservableCollection<Attribute>();
            symEncParams.Add(new Attribute(Type.TYPE_BYTE_VEC, "msg"));
            symEncParams.Add(new Attribute(Type.TYPE_INT, "key"));
            Method symEnc = new Method(Type.TYPE_BYTE_VEC, "SymEnc", symEncParams, Crypto.None);
            InnerMethods.Add(symEnc);
            // 对称解密
            ObservableCollection<Attribute> symDecParams = new ObservableCollection<Attribute>();
            symDecParams.Add(new Attribute(Type.TYPE_BYTE_VEC, "msg"));
            symDecParams.Add(new Attribute(Type.TYPE_INT, "key"));
            Method symDec = new Method(Type.TYPE_BYTE_VEC, "SymDec", symDecParams, Crypto.None);
            InnerMethods.Add(symDec);
            // 私钥签名
            ObservableCollection<Attribute> signParams = new ObservableCollection<Attribute>();
            signParams.Add(new Attribute(Type.TYPE_BYTE_VEC, "msg"));
            signParams.Add(new Attribute(Type.TYPE_INT, "skey"));
            Method sign = new Method(Type.TYPE_BYTE_VEC, "Sign", signParams, Crypto.None);
            InnerMethods.Add(sign);
            // 公钥验证
            ObservableCollection<Attribute> verifyParams = new ObservableCollection<Attribute>();
            verifyParams.Add(new Attribute(Type.TYPE_BYTE_VEC, "msg"));
            verifyParams.Add(new Attribute(Type.TYPE_INT, "pkey"));
            Method verify = new Method(Type.TYPE_BOOL, "Verify", verifyParams, Crypto.None);
            InnerMethods.Add(verify);
        }
        // 加密方式枚举的集合，用于判断对称加密/非对称加密/哈希签名
        public static readonly List<Crypto> Sym = new List<Crypto>() { 
            Crypto.AES, Crypto.DES
        };
        public static readonly List<Crypto> ASym = new List<Crypto>() {
            Crypto.RSA, Crypto.ECC
        };
        public static readonly List<Crypto> Hash = new List<Crypto>() {
            Crypto.MD5, Crypto.SHA1, Crypto.SHA256
        };


        #endregion
    }
}
