using sbid._M;
using sbid._VM;
using System.Collections;
using System.Collections.ObjectModel;

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
            return anotherEndConnectorVM.NetworkItemVM;
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
    }
}
