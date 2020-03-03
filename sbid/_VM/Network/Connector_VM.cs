using Avalonia;
using Avalonia.Media;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    // 锚点VM
    public class Connector_VM : ViewModelBase
    {
        public static int _id = 0;
        private Point pos;
        private Point oldPos;
        private Connection_VM connectionVM = null;
        private bool isActive = false;
        private int id;

        // 无参构造用于xaml里Design
        public Connector_VM()
        {
            _id++;
            this.id = _id;
        }

        // 带位置的构造
        public Connector_VM(double x, double y)
        {
            _id++;
            this.id = _id;
            Pos = new Point(x, y);
        }

        // 锚点位置
        public Point Pos
        {
            get => pos;
            set
            {
                this.RaiseAndSetIfChanged(ref pos, value);
                // 对于状态机而言,锚点位置变化时,还应通知计算中心位置
                if (connectionVM != null && connectionVM is Transition_VM)
                    connectionVM.RaisePropertyChanged("MidPos");
                // 顺序图也是一样
                else if (connectionVM is Message_VM)
                    connectionVM.RaisePropertyChanged("MidPos");
            }
        }

        // 锚点旧位置,用于在拖拽图形按下时记录,以保证连线跟着变化
        public Point OldPos { get => oldPos; set => oldPos = value; }

        // 锚点颜色,反映 被占用/活动/空闲
        public ISolidColorBrush Color
        {
            get
            {
                if (connectionVM != null) // 被占用:红
                    return Brushes.Red;
                else if (isActive) // 活动:绿
                    return Brushes.LightGreen;
                return Brushes.White; // 空闲:白
            }
        }

        // 反引所在的Connection_VM,没有连线时就是null
        public Connection_VM ConnectionVM
        {
            get => connectionVM;
            set
            {
                this.RaiseAndSetIfChanged(ref connectionVM, value);
                this.RaisePropertyChanged("Color"); // 通知颜色要重新计算了
            }
        }

        // 是否是活动锚点
        public bool IsActive
        {
            get => isActive;
            set
            {
                this.RaiseAndSetIfChanged(ref isActive, value);
                this.RaisePropertyChanged("Color"); // 通知颜色要重新计算了
            }
        }

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
    }
}
