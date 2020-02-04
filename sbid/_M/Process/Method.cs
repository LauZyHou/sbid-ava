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

    // 进程模板中的方法
    public class Method : ReactiveObject
    {
        private Type returnType;
        private string name;
        private ObservableCollection<Attribute> parameters; // 这里不创建,在构造时传入
        private Crypto cryptoSuffix = Crypto.None;

        // 返回值
        public Type ReturnType { get => returnType; set => this.RaiseAndSetIfChanged(ref returnType, value); }
        // 方法名
        public string Name { get => name; set => this.RaiseAndSetIfChanged(ref name, value); }
        // 参数列表
        public ObservableCollection<Attribute> Parameters { get => parameters; set => parameters = value; }
        // 加解密方式(可选)
        public Crypto CryptoSuffix { get => cryptoSuffix; set => this.RaiseAndSetIfChanged(ref cryptoSuffix, value); }
    }
}
