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

    // 通信方法中的通信方式枚举
    public enum CommWay
    {
        NativeEthernetFrame, UDP
    }

    // 进程模板中的通信方法
    public class CommMethod : ReactiveObject
    {
        public static int _id = 0;
        private string name;
        private ObservableCollection<Attribute> parameters; // 这里不创建,在构造时传入
        private InOut inOutSuffix;
        private CommWay commWay;
        private int id;
        private string typeId = "";

        public CommMethod(string name, ObservableCollection<Attribute> parameters,
                          InOut inOutSuffix, CommWay commWay)
        {
            _id++;
            this.id = _id;
            this.name = name;
            this.parameters = parameters;
            this.inOutSuffix = inOutSuffix;
            this.commWay = commWay;
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
        // 通信方式(必选)
        public CommWay CommWay { get => commWay; set => this.RaiseAndSetIfChanged(ref commWay, value); }

        // 【仅用于原生以太网帧】
        // 类型号
        public string TypeId { get => typeId; set => this.RaiseAndSetIfChanged(ref typeId, value); }

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

        // 完整展示串
        public string ShowString
        {
            get
            {
                return ToString() + "[" + InOutSuffix + "][" + CommWay + "]";
            }
        }

        #endregion
    }
}
