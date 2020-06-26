using sbid._M;
using sbid._V;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    public class SafetyProperty_VM : NetworkItem_VM
    {
        private SafetyProperty safetyProperty;

        public SafetyProperty_VM()
        {
            safetyProperty = new SafetyProperty();
        }

        public SafetyProperty SafetyProperty { get => safetyProperty; set => safetyProperty = value; }

        #region 右键菜单命令

        // 尝试打开当前SafetyProperty_VM的编辑窗体
        public void EditSafetyPropertyVM()
        {
            // 从主窗体打开编辑窗体,并在其DataContext中集成当前SafetyProperty_VM里集成的SafetyProperty对象,以能对其作修改
            SafetyProperty_EW_V safetyPropertyEWV = new SafetyProperty_EW_V()
            {
                DataContext = new SafetyProperty_EW_VM()
                {
                    SafetyProperty = safetyProperty
                }
            };
            // 将所有的Process也传入,作为ProcessMethod的可选进程
            foreach (ViewModelBase item in ResourceManager.mainWindowVM.SelectedItem.SelectedItem.SelectedItem.UserControlVMs)
            {
                if (item is Process_VM)
                {
                    Process_VM processVM = (Process_VM)item;

                    // 计算Process对应状态机的所有状态,写入Process附加的States字段中
                    processVM.Process.States = new ObservableCollection<State>();
                    foreach (ViewModelBase vm in processVM.ProcessToSM_P_VM.StateMachinePVMs[0].UserControlVMs)
                    {
                        if (vm is State_VM)
                        {
                            State_VM stateVM = (State_VM)vm;
                            processVM.Process.States.Add(stateVM.State);
                        }
                    }

                    ((SafetyProperty_EW_VM)safetyPropertyEWV.DataContext).Processes.Add(processVM.Process);
                }
            }

            safetyPropertyEWV.ShowDialog(ResourceManager.mainWindowV);
            ResourceManager.mainWindowVM.Tips = "打开了SafetyProperty[" + safetyProperty.Name + "]的编辑窗体";
        }

        #endregion
    }
}
