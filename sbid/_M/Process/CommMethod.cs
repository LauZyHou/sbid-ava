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
        public static int _id = 0;
        private string name;
        private ObservableCollection<Attribute> parameters; // 这里不创建,在构造时传入
        private InOut inOutSuffix;
        private int id;

        public CommMethod(string name, ObservableCollection<Attribute> parameters, InOut inOutSuffix)
        {
            _id++;
            this.id = _id;
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

        // 注意，这个部分只在顺序图连线里用到了，所以这里的格式随便改
        // 如果其它地方也用到了，那就要保证不影响到需求了
        // 更好的方式是把各个xxxMessage_V文件里的样式用一种更好的方式实现
        // 所以这里标个fixme
        public override string ToString()
        {
            string paramString = "";
            foreach (Attribute attribute in parameters)
            {
                paramString += attribute + ", ";
            }
            //return name + "(" + paramString.TrimEnd(", ".ToCharArray()) + ");[" + InOutSuffix + "]";
            return name + "(" + paramString.TrimEnd(", ".ToCharArray()) + ")";
        }

        #endregion
    }
}
