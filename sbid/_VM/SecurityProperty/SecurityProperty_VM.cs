using sbid._M;
using sbid._V;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    public class SecurityProperty_VM : NetworkItem_VM
    {
        private SecurityProperty securityProperty;

        public SecurityProperty_VM()
        {
            securityProperty = new SecurityProperty();
        }

        public SecurityProperty SecurityProperty { get => securityProperty; set => securityProperty = value; }

        #region 右键菜单命令

        // 尝试打开当前SecurityProperty_VM的编辑窗体
        public void EditSecurityPropertyVM()
        {
            // 从主窗体打开编辑窗体,并在其DataContext中集成当前SecurityProperty_VM里集成的SecurityProperty对象,以能对其作修改
            SecurityProperty_EW_V securityPropertyEWV = new SecurityProperty_EW_V()
            {
                DataContext = new SecurityProperty_EW_VM()
                {
                    SecurityProperty = securityProperty
                }
            };
            // 将所有的Process传入,作为Confidential和Authenticity去选用的参数
            foreach (ViewModelBase item in ResourceManager.mainWindowVM.SelectedItem.SelectedItem.SelectedItem.UserControlVMs)
            {
                if (item is Process_VM)
                {
                    Process_VM processVM = (Process_VM)item;

                    // 计算Process对应状态机的所有状态,写入Process附加的States字段中
                    /*
                    processVM.Process.States = new ObservableCollection<State>();
                    foreach (ViewModelBase vm in processVM.ProcessToSM_P_VM.StateMachinePVMs[0].UserControlVMs)
                    {
                        if (vm is State_VM)
                        {
                            State_VM stateVM = (State_VM)vm;
                            processVM.Process.States.Add(stateVM.State);
                        }
                    }
                    */

                    ((SecurityProperty_EW_VM)securityPropertyEWV.DataContext).Processes.Add(processVM.Process);
                }
            }

            securityPropertyEWV.ShowDialog(ResourceManager.mainWindowV);
            ResourceManager.mainWindowVM.Tips = "打开了SecurityProperty[" + securityProperty.Name + "]的编辑窗体";
        }

        // 删除当前SecurityProperty_VM
        private void DeleteSecurityPropertyVM()
        {
            ObservableCollection<ViewModelBase> userControlVMs = ResourceManager.mainWindowVM.SelectedItem.PanelVMs[0].SidePanelVMs[0].UserControlVMs;
            userControlVMs.Remove(this);
            ResourceManager.mainWindowVM.Tips = "删除了信息安全性质：" + securityProperty.Name;
        }

        #endregion
    }
}
