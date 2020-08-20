using sbid._M;
using sbid._VM;
using System.Collections;

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
    }
}
