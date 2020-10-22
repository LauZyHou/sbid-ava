using sbid._M;
using sbid._VM;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace sbid
{
    public class Utils
    {
        /// <summary>
        /// 获取锚点(Connector_VM)所在连线(Connection_VM)的另一端的建模元素(NetworkItem_VM)
        /// </summary>
        /// <param name="connector_VM">锚点</param>
        /// <returns>另一端的建模元素</returns>
        public static NetworkItem_VM getAnotherEndNetWorkItemVM(Connector_VM connector_VM)
        {
            // 先获取另一端的锚点
            Connector_VM anotherEndConnectorVM = getAnotherEndConnectorVM(connector_VM);
            if (anotherEndConnectorVM is null)
            {
                return null;
            }
            // 再取出其所在的建模元素
            return anotherEndConnectorVM.NetworkItemVM;
        }

        /// <summary>
        /// 获取锚点(Connector_VM)所在连线(Connection_VM)的另一端的锚点(Connector_VM)
        /// </summary>
        /// <param name="connector_VM">锚点</param>
        /// <returns>另一端的锚点</returns>
        public static Connector_VM getAnotherEndConnectorVM(Connector_VM connector_VM)
        {
            Connection_VM connection_VM = connector_VM.ConnectionVM;
            if (connection_VM is null)
            {
                return null;
            }
            Connector_VM anotherEndConnectorVM;
            if (connection_VM.Source == connector_VM)
            {
                anotherEndConnectorVM = connection_VM.Dest;
            }
            else
            {
                anotherEndConnectorVM = connection_VM.Source;
            }
            return anotherEndConnectorVM;
        }


        /// <summary>
        /// 删除指定面板上指定的建模元素
        /// </summary>
        /// <param name="networkItem_VM">要删除的建模元素</param>
        /// <param name="sidePanel_VM">指定的面板</param>
        public static void deleteAndClearNetworkItemVM(NetworkItem_VM networkItem_VM, SidePanel_VM sidePanel_VM)
        {
            // 清理其锚点上的所有连线
            foreach (Connector_VM connector_VM in networkItem_VM.ConnectorVMs)
            {
                if (connector_VM is null)
                {
                    continue;
                }
                Connection_VM connection_VM = connector_VM.ConnectionVM;
                if (connection_VM is null)
                {
                    continue;
                }
                if (connection_VM.Source == connector_VM)
                {
                    connection_VM.Dest.ConnectionVM = null;
                }
                else
                {
                    connection_VM.Source.ConnectionVM = null;
                }
                sidePanel_VM.UserControlVMs.Remove(connection_VM);
            }
            // 从面板中删除该建模元素
            sidePanel_VM.UserControlVMs.Remove(networkItem_VM);
        }

        /// <summary>
        /// 执行一条命令，不重定向输出
        /// </summary>
        /// <param name="command_file">命令的实体文件</param>
        /// <param name="command_param">命令的参数</param>
        public static void runCommand(string command_file, string command_param = "")
        {
            // 检查一下要执行的命令实体是不是空
            if (string.IsNullOrEmpty(command_file))
            {
                ResourceManager.mainWindowVM.Tips = "验证命令为空，无法验证";
                return;
            }
            // 要执行的验证命令
            ProcessStartInfo processStartInfo = new ProcessStartInfo
                (
                    command_file,
                    command_param
                )
            {
                RedirectStandardOutput = false
            };
            // 执行这条命令，执行过程中可能抛掷异常，直接显示异常信息
            System.Diagnostics.Process process = null;
            try
            {
                process = System.Diagnostics.Process.Start(processStartInfo);
            }
            catch (System.Exception ex)
            {
                ResourceManager.mainWindowVM.Tips = ex.ToString();
            }
            if (process is null)
            {
                return;
            }
        }
    }
}
