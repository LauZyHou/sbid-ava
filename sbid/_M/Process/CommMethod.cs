using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._M
{
    // 通信方法的输入输出枚举
    public enum InOut
    {
        In, Out
    }

    // 进程模板中的通信方法
    public class CommMethod : ReactiveObject
    {
        private string name;
        private ObservableCollection<Attribute> parameters; // 这里不创建,在构造时传入
        private InOut inOutSuffix;

        public CommMethod(string name, ObservableCollection<Attribute> parameters, InOut inOutSuffix)
        {
            this.name = name;
            this.parameters = parameters;
            this.inOutSuffix = inOutSuffix;
        }

        // 函数名
        public string Name { get => name; set => this.RaiseAndSetIfChanged(ref name, value); }
        // 参数列表
        public ObservableCollection<Attribute> Parameters
        {
            get => parameters;
            set
            {
                parameters = value;
                this.RaisePropertyChanged("ParamString");
            }
        }
        // 输入输出(必选)
        public InOut InOutSuffix { get => inOutSuffix; set => this.RaiseAndSetIfChanged(ref inOutSuffix, value); }

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
            string paramString = "";
            foreach (Attribute attribute in parameters)
            {
                paramString += attribute + ", ";
            }
            return name + "(" + paramString.TrimEnd(", ".ToCharArray()) + ");[" + InOutSuffix + "]";
        }

        #endregion
    }
}
