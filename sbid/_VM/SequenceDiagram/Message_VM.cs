using Avalonia;
using Avalonia.Controls;
using sbid._M;
using sbid._V;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    // 顺序图三类消息传递的基类VM
    public abstract class Message_VM : Arrow_VM
    {
        private CommMessage commMessage = new CommMessage(null);

        // 通信消息
        public CommMessage CommMessage { get => commMessage; }

        // 消息所在的位置点,位于两锚点中心附近
        public Point MidPos
        {
            get
            {
                double x = (Source.Pos.X + Dest.Pos.X) / 2;
                double y = (Source.Pos.Y + Dest.Pos.Y) / 2;
                return new Point(x - 40, y - 10);
            }
        }

        #region 右键菜单命令

        // 尝试打开编辑消息内容的窗口
        public void EditMessage()
        {
            // 从主窗体打开编辑窗体,并在其DataContext中集成当前Message_VM里集成的CommMessage对象,以能对其作修改
            Message_EW_V messageEWV = new Message_EW_V()
            {
                DataContext = new Message_EW_VM()
                {
                    CommMessage = commMessage
                }
            };
            // 将Source方ObjLifeLine的进程模板的所有[OUT]型CommMethod存入
            ObjLifeLine_VM objLifeLine_VM = (ObjLifeLine_VM)Source.NetworkItemVM;
            if (objLifeLine_VM.SeqObject.Process != null)
            {
                foreach (CommMethod commMethod in objLifeLine_VM.SeqObject.Process.CommMethods)
                {
                    if (commMethod.InOutSuffix == InOut.Out)
                    {
                        ((Message_EW_VM)messageEWV.DataContext).CommMethods.Add(commMethod);
                    }
                }
            }

            // [bugfix]因为在xaml里绑定Process打开编辑窗口显示出不来，只好在这里手动设置一下
            ComboBox commMethod_ComboBox = ControlExtensions.FindControl<ComboBox>(messageEWV, "commMethod_ComboBox");
            commMethod_ComboBox.SelectedItem = ((Message_EW_VM)messageEWV.DataContext).CommMessage.CommMethod;

            messageEWV.ShowDialog(ResourceManager.mainWindowV);
            ResourceManager.mainWindowVM.Tips = "打开了消息编辑窗体";
        }

        #endregion
    }
}
