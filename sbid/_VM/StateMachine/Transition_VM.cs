using Avalonia;
using ReactiveUI;
using sbid._M;
using sbid._V;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class Transition_VM : Arrow_VM
    {
        private Transition transition = new Transition();

        public Transition Transition { get => transition; set => this.RaiseAndSetIfChanged(ref transition, value); }

        // 状态机Gurad条件和Actions的位置点,位于两锚点中心附近
        public Point MidPos
        {
            get
            {
                double x = (Source.Pos.X + Dest.Pos.X) / 2;
                double y = (Source.Pos.Y + Dest.Pos.Y) / 2;
                return new Point(x - 40, y - 30);
            }
        }

        #region 右键菜单命令

        // 尝试打开编辑转移关系的窗口
        public void EditTransitionVM()
        {
            // 从主窗体打开编辑窗体,并在其DataContext中集成当前Transition_VM里集成的Transition对象,以能对其作修改
            Transition_EW_V transitionEWV = new Transition_EW_V()
            {
                DataContext = new Transition_EW_VM()
                {
                    Transition = transition
                }
            };
            transitionEWV.ShowDialog(ResourceManager.mainWindowV);
            ResourceManager.mainWindowVM.Tips = "打开了转移关系的编辑窗体";
        }

        #endregion
    }
}
