using sbid._M;
using sbid._VM;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Xml;

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
        /// 执行一条命令，不关心返回结果
        /// </summary>
        /// <param name="command_file">命令实体文件</param>
        /// <param name="command_param">命令参数</param>
        /// <returns>返回true表示命令执行成功，false表示命令执行失败</returns>
        public static bool runCommand(string command_file, string command_param)
        {
            // 检查一下要执行的命令实体是不是空
            if (string.IsNullOrEmpty(command_file))
            {
                ResourceManager.mainWindowVM.Tips = "待执行命令为空，无法执行";
                return false;
            }
            // 自动识别，根据操作系统的不同，调用不同的后缀名脚本
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                command_file += ".bat";
            }
            else
            {
                if (string.IsNullOrEmpty(command_param))
                {
                    command_param = command_file + ".sh";
                    command_file = "bash";
                }
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
            // 执行这条命令，执行过程中可能捕获异常
            System.Diagnostics.Process process = null;
            try
            {
                process = System.Diagnostics.Process.Start(processStartInfo);
            }
            catch (System.Exception ex)
            {
                // 直接显示异常信息
                ResourceManager.mainWindowVM.Tips = ex.ToString();
            }
            if (process is null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 执行一条命令，返回命令行返回的字符串
        /// </summary>
        /// <param name="command_file">命令实体文件</param>
        /// <param name="command_param">命令参数</param>
        /// <returns></returns>
        public static string runCommandWithRes(string command_file, string command_param)
        {
            // 自动识别，根据操作系统的不同，调用不同的后缀名脚本
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                command_file += ".bat";
            }
            else if (string.IsNullOrEmpty(command_param))
            {
                command_param = command_file + ".sh";
                command_file = "bash";
            }
            // 要执行的语法检查命令
            ProcessStartInfo processStartInfo = new ProcessStartInfo
                (
                    command_file,
                    command_param
                )
            {
                RedirectStandardOutput = true
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
                return "[ERROR]process is null，见函数Utils.runCommandWithRes";
            }
            // 读取并写入结果
            string res = "";
            using (System.IO.StreamReader streamReader = process.StandardOutput)
            {
                if (!streamReader.EndOfStream)
                {
                    res = streamReader.ReadToEnd();
                }
                if (!process.HasExited)
                {
                    process.Kill();
                }
            }
            return res;
        }

        /// <summary>
        /// 对于当前协议，生成后端验证和代码生成用的XML文件
        /// </summary>
        public static void ExportBackXML()
        {
            string fpath = ResourceManager.back_xml;
            // 获取当前选中的协议
            Protocol_VM protocolVM = ResourceManager.mainWindowVM.SelectedItem;
            if (protocolVM is null) { return; }
            // 创建XmlTextWriter
            XmlTextWriter xmlWriter = null;
            try
            {
                xmlWriter = new XmlTextWriter(fpath, null);
            }
            catch (System.Exception e)
            {
                ResourceManager.mainWindowVM.Tips = e.Message;
                return;
            }
            // 生成后端XML文件
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.WriteStartElement("Protocol");
            xmlWriter.WriteAttributeString("name", protocolVM.Protocol.Name);

            #region 概览>类图面板

            // 这个比较特殊，因为就这一个，而且一定有这一个，用户既不能创建也不能销毁
            xmlWriter.WriteStartElement("ClassDiagram");
            ClassDiagram_P_VM classDiagram_P_VM = (ClassDiagram_P_VM)protocolVM.PanelVMs[0].SidePanelVMs[0];
            foreach (ViewModelBase item in classDiagram_P_VM.UserControlVMs)
            {
                // 按照每个类图的类型做不同的保存方法
                if (item is UserType_VM)
                {
                    UserType_VM vm = (UserType_VM)item;
                    xmlWriter.WriteStartElement("UserType");
                    xmlWriter.WriteAttributeString("name", vm.Type.Name);
                    // xmlWriter.WriteAttributeString("id", vm.Type.Id.ToString());
                    if (!(vm.Type is UserType)) // 基本类型
                    {
                        xmlWriter.WriteAttributeString("basic", "true");
                        // 注意，基本类型在创建类图时就创建了，所以要
                    }
                    else if (vm.Type == Type.TYPE_BYTE_VEC || vm.Type == Type.TYPE_TIMER) // 内置的复合类型
                    {
                        xmlWriter.WriteAttributeString("basic", "middle");
                    }
                    else // 用户自定义复合类型
                    {
                        UserType userType = (UserType)vm.Type;
                        xmlWriter.WriteAttributeString("basic", "false");
                        if (userType.Parent != null) // 有继承关系
                            xmlWriter.WriteAttributeString("parent", userType.Parent.Name);
                        else // 无继承关系
                            xmlWriter.WriteAttributeString("parent", "");
                        foreach (Attribute attr in userType.Attributes)
                        {
                            xmlWriter.WriteStartElement("Attribute");
                            ResourceManager.writeAttribute2(xmlWriter, attr);
                            xmlWriter.WriteEndElement();
                        }
                        foreach (Method method in userType.Methods)
                        {
                            xmlWriter.WriteStartElement("Method");
                            xmlWriter.WriteAttributeString("returnType", method.ReturnType.Name);
                            xmlWriter.WriteAttributeString("name", method.Name);
                            xmlWriter.WriteAttributeString("cryptoSuffix", method.CryptoSuffix.ToString());
                            xmlWriter.WriteAttributeString("achieve", method.Achieve);
                            // xmlWriter.WriteAttributeString("id", method.Id.ToString());
                            foreach (Attribute attr in method.Parameters)
                            {
                                xmlWriter.WriteStartElement("Parameter"); // 注意这里叫Parameter了
                                ResourceManager.writeAttribute2(xmlWriter, attr);
                                xmlWriter.WriteEndElement();
                            }
                            xmlWriter.WriteEndElement();
                        }
                    }
                    xmlWriter.WriteEndElement();
                }
                else if (item is Process_VM)
                {
                    Process_VM vm = (Process_VM)item;
                    xmlWriter.WriteStartElement("Process");
                    xmlWriter.WriteAttributeString("name", vm.Process.RefName.Content);
                    // xmlWriter.WriteAttributeString("id", vm.Process.Id.ToString());
                    foreach (Attribute attr in vm.Process.Attributes)
                    {
                        xmlWriter.WriteStartElement("Attribute");
                        ResourceManager.writeAttribute2(xmlWriter, attr);
                        xmlWriter.WriteEndElement();
                    }
                    foreach (Method method in vm.Process.Methods)
                    {
                        xmlWriter.WriteStartElement("Method");
                        xmlWriter.WriteAttributeString("returnType", method.ReturnType.Name);
                        xmlWriter.WriteAttributeString("name", method.Name);
                        xmlWriter.WriteAttributeString("cryptoSuffix", method.CryptoSuffix.ToString());
                        xmlWriter.WriteAttributeString("achieve", method.Achieve);
                        // xmlWriter.WriteAttributeString("id", method.Id.ToString());
                        foreach (Attribute attr in method.Parameters)
                        {
                            xmlWriter.WriteStartElement("Parameter");
                            ResourceManager.writeAttribute2(xmlWriter, attr);
                            xmlWriter.WriteEndElement();
                        }
                        xmlWriter.WriteEndElement();
                    }
                    foreach (CommMethod commMethod in vm.Process.CommMethods)
                    {
                        xmlWriter.WriteStartElement("CommMethod");
                        xmlWriter.WriteAttributeString("name", commMethod.Name);
                        xmlWriter.WriteAttributeString("inOutSuffix", commMethod.InOutSuffix.ToString());
                        xmlWriter.WriteAttributeString("commWay", commMethod.CommWay.ToString());
                        // xmlWriter.WriteAttributeString("id", commMethod.Id.ToString());
                        foreach (Attribute attr in commMethod.Parameters)
                        {
                            xmlWriter.WriteStartElement("Parameter");
                            ResourceManager.writeAttribute2(xmlWriter, attr);
                            xmlWriter.WriteEndElement();
                        }
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();
                }
                else if (item is Axiom_VM)
                {
                    Axiom_VM vm = (Axiom_VM)item;
                    xmlWriter.WriteStartElement("Axiom");
                    xmlWriter.WriteAttributeString("name", vm.Axiom.Name);
                    // xmlWriter.WriteAttributeString("id", vm.Axiom.Id.ToString());
                    foreach (ProcessMethod processMethod in vm.Axiom.ProcessMethods)
                    {
                        xmlWriter.WriteStartElement("ProcessMethod");
                        xmlWriter.WriteAttributeString("process", processMethod.Process.RefName.Content);
                        xmlWriter.WriteAttributeString("method", processMethod.Method.Name);
                        xmlWriter.WriteEndElement();
                    }
                    foreach (Formula formula in vm.Axiom.Formulas)
                    {
                        xmlWriter.WriteStartElement("Formula");
                        xmlWriter.WriteAttributeString("content", formula.Content);
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();
                }
                else if (item is InitialKnowledge_VM)
                {
                    InitialKnowledge_VM vm = (InitialKnowledge_VM)item;
                    xmlWriter.WriteStartElement("InitialKnowledge");
                    if (vm.InitialKnowledge.Process == null) // 全局，不关联Process
                    {
                        xmlWriter.WriteAttributeString("process", "");
                    }
                    else
                    {
                        xmlWriter.WriteAttributeString("process", vm.InitialKnowledge.Process.RefName.Content);
                    }
                    // xmlWriter.WriteAttributeString("id", vm.InitialKnowledge.Id.ToString());
                    foreach (Knowledge knowledge in vm.InitialKnowledge.Knowledges)
                    {
                        xmlWriter.WriteStartElement("Knowledge");
                        xmlWriter.WriteAttributeString("process", knowledge.Process.RefName.Content);
                        // 这里不再用Attribute的Id，而是直接用Identifier
                        xmlWriter.WriteAttributeString("attribute", knowledge.Attribute.Identifier);
                        xmlWriter.WriteEndElement();
                    }
                    foreach (KeyPair keyPair in vm.InitialKnowledge.KeyPairs)
                    {
                        xmlWriter.WriteStartElement("KeyPair");
                        xmlWriter.WriteAttributeString("pubProcess", keyPair.PubProcess.RefName.Content);
                        xmlWriter.WriteAttributeString("pubKey", keyPair.PubKey.Identifier);
                        xmlWriter.WriteAttributeString("secProcess", keyPair.SecProcess.RefName.Content);
                        xmlWriter.WriteAttributeString("secKey", keyPair.SecKey.Identifier);
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();
                }
                else if (item is SafetyProperty_VM)
                {
                    SafetyProperty_VM vm = (SafetyProperty_VM)item;
                    xmlWriter.WriteStartElement("SafetyProperty");
                    xmlWriter.WriteAttributeString("name", vm.SafetyProperty.Name);
                    // xmlWriter.WriteAttributeString("id", vm.SafetyProperty.Id.ToString());
                    foreach (Formula formula in vm.SafetyProperty.CTLs)
                    {
                        xmlWriter.WriteStartElement("CTL");
                        xmlWriter.WriteAttributeString("formula", formula.Content);
                        xmlWriter.WriteEndElement();
                    }
                    foreach (Formula formula in vm.SafetyProperty.Invariants)
                    {
                        xmlWriter.WriteStartElement("Invariant");
                        xmlWriter.WriteAttributeString("content", formula.Content);
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();
                }
                else if (item is SecurityProperty_VM)
                {
                    SecurityProperty_VM vm = (SecurityProperty_VM)item;
                    xmlWriter.WriteStartElement("SecurityProperty");
                    xmlWriter.WriteAttributeString("name", vm.SecurityProperty.Name);
                    // xmlWriter.WriteAttributeString("id", vm.SecurityProperty.Id.ToString());
                    foreach (Confidential confidential in vm.SecurityProperty.Confidentials)
                    {
                        xmlWriter.WriteStartElement("Confidential");
                        xmlWriter.WriteAttributeString("process", confidential.Process.RefName.Content);
                        xmlWriter.WriteAttributeString("attribute", confidential.Attribute.Identifier);
                        xmlWriter.WriteEndElement();
                    }
                    foreach (Authenticity authenticity in vm.SecurityProperty.Authenticities)
                    {
                        xmlWriter.WriteStartElement("Authenticity");
                        xmlWriter.WriteAttributeString("processA", authenticity.ProcessA.RefName.Content);
                        xmlWriter.WriteAttributeString("stateA", authenticity.StateA.Name);
                        xmlWriter.WriteAttributeString("attributeA", authenticity.AttributeA.Identifier);
                        xmlWriter.WriteAttributeString("attributeA_Attr", authenticity.AttributeA_Attr.Identifier);
                        xmlWriter.WriteAttributeString("processB", authenticity.ProcessB.RefName.Content);
                        xmlWriter.WriteAttributeString("stateB", authenticity.StateB.Name);
                        xmlWriter.WriteAttributeString("attributeB", authenticity.AttributeB.Identifier);
                        xmlWriter.WriteAttributeString("attributeB_Attr", authenticity.AttributeB_Attr.Identifier);
                        xmlWriter.WriteEndElement();
                    }
                    foreach (Integrity integrity in vm.SecurityProperty.Integrities)
                    {
                        xmlWriter.WriteStartElement("Integrity");
                        xmlWriter.WriteAttributeString("processA", integrity.ProcessA.RefName.Content);
                        xmlWriter.WriteAttributeString("stateA", integrity.StateA.Name);
                        xmlWriter.WriteAttributeString("attributeA", integrity.AttributeA.Identifier);
                        xmlWriter.WriteAttributeString("processB", integrity.ProcessB.RefName.Content);
                        xmlWriter.WriteAttributeString("stateB", integrity.StateB.Name);
                        xmlWriter.WriteAttributeString("attributeB", integrity.AttributeB.Identifier);
                        xmlWriter.WriteEndElement();
                    }
                    foreach (Availability availability in vm.SecurityProperty.Availabilities)
                    {
                        xmlWriter.WriteStartElement("Availability");
                        xmlWriter.WriteAttributeString("process", availability.Process.RefName.Content);
                        xmlWriter.WriteAttributeString("state", availability.State.Name);
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();
                }
                else if (item is CommChannel_VM)
                {
                    CommChannel_VM vm = (CommChannel_VM)item;
                    xmlWriter.WriteStartElement("CommChannel");
                    xmlWriter.WriteAttributeString("name", vm.CommChannel.Name);
                    // xmlWriter.WriteAttributeString("id", vm.CommChannel.Id.ToString());
                    foreach (CommMethodPair commMethodPair in vm.CommChannel.CommMethodPairs)
                    {
                        xmlWriter.WriteStartElement("CommMethodPair");
                        xmlWriter.WriteAttributeString("id", commMethodPair.Id.ToString());
                        xmlWriter.WriteAttributeString("processA", commMethodPair.ProcessA.RefName.Content);
                        xmlWriter.WriteAttributeString("commMethodA", commMethodPair.CommMethodA.Name);
                        xmlWriter.WriteAttributeString("processB", commMethodPair.ProcessB.RefName.Content);
                        xmlWriter.WriteAttributeString("commMethodB", commMethodPair.CommMethodB.Name);
                        xmlWriter.WriteAttributeString("privacy", commMethodPair.Privacy.ToString());
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();
                }
            }
            xmlWriter.WriteEndElement();

            #endregion

            #region "进程模板-状态机"面板

            xmlWriter.WriteStartElement("ProcessToSMs");
            ObservableCollection<SidePanel_VM> processToSM_P_VMs = protocolVM.PanelVMs[1].SidePanelVMs; // 所有的"进程模板-状态机"面板
            foreach (SidePanel_VM sidePanel_VM in processToSM_P_VMs)
            {
                ProcessToSM_P_VM processToSM_P_VM = (ProcessToSM_P_VM)sidePanel_VM;
                xmlWriter.WriteStartElement("ProcessToSM");
                xmlWriter.WriteAttributeString("process", processToSM_P_VM.Process.RefName.Content);
                // 对于里面的每个状态机面板，写入XML
                foreach (StateMachine_P_VM stateMachine_P_VM in processToSM_P_VM.StateMachinePVMs)
                {
                    xmlWriter.WriteStartElement("StateMachine");
                    // 注意顶层状态这里改用空串""
                    xmlWriter.WriteAttributeString("refine_state", stateMachine_P_VM.State == State.TopState ? "" : stateMachine_P_VM.State.Name);
                    foreach (ViewModelBase vm in stateMachine_P_VM.UserControlVMs) // 写入状态机的结点和连线等
                    {
                        if (vm is State_VM)
                        {
                            State_VM state_VM = (State_VM)vm;
                            xmlWriter.WriteStartElement("State");
                            xmlWriter.WriteAttributeString("name", state_VM.State.Name);
                            // xmlWriter.WriteAttributeString("id", state_VM.State.Id.ToString());
                            xmlWriter.WriteEndElement();
                        }
                        /*
                        else if (vm is InitState_VM)
                        {
                            //InitState_VM initState_VM = (InitState_VM)vm;
                            //xmlWriter.WriteStartElement("InitState");
                            xmlWriter.WriteStartElement("State");
                            xmlWriter.WriteAttributeString("name", "_init");
                            xmlWriter.WriteEndElement();
                        }
                        else if (vm is FinalState_VM)
                        {
                            //FinalState_VM finalState_VM = (FinalState_VM)vm;
                            //xmlWriter.WriteStartElement("FinalState");
                            xmlWriter.WriteStartElement("State");
                            xmlWriter.WriteAttributeString("name", "_final");
                            xmlWriter.WriteEndElement();
                        }
                        */
                        // 对于普通状态和初始状态，允许出边，遍历锚点计算目标状态和转移，以生成转移关系
                        if (vm is State_VM || vm is InitState_VM)
                        {
                            // 当前状态名
                            string sourceState = vm is InitState_VM ? "_init" : ((State_VM)vm).State.Name;
                            NetworkItem_VM networkItem_VM = (NetworkItem_VM)vm;
                            // 遍历当前状态所有锚点
                            foreach (Connector_VM connector_VM in networkItem_VM.ConnectorVMs)
                            {
                                // 当前状态存在出边
                                if (connector_VM.ConnectionVM != null && connector_VM.ConnectionVM.Source == connector_VM)
                                {
                                    xmlWriter.WriteStartElement("Transition");
                                    StateTrans_VM stateTrans_VM = null; // 记录途经的（最后一个）StateTrans，也可没有
                                    Connector_VM nowConnector_VM = connector_VM.ConnectionVM.Dest;
                                    // 跳过若干控制点和StateTrans结点
                                    while (true)
                                    {
                                        // 控制点，跳过向Dest走
                                        if (nowConnector_VM.NetworkItemVM is ControlPoint_VM)
                                        {
                                            ControlPoint_VM controlPoint_VM = (ControlPoint_VM)nowConnector_VM.NetworkItemVM;
                                            nowConnector_VM = controlPoint_VM.ConnectorVMs[1].ConnectionVM.Dest;
                                        }
                                        // StateTrans结点，记录一下然后向下走
                                        else if (nowConnector_VM.NetworkItemVM is StateTrans_VM)
                                        {
                                            stateTrans_VM = (StateTrans_VM)nowConnector_VM.NetworkItemVM;
                                            // 遍历StateTrans的锚点找向下走的点
                                            nowConnector_VM = null;
                                            foreach (Connector_VM stc in stateTrans_VM.ConnectorVMs)
                                            {
                                                if (stc.ConnectionVM != null && stc.ConnectionVM.Source == stc)
                                                {
                                                    nowConnector_VM = stc.ConnectionVM.Dest;
                                                    break;
                                                }
                                            }
                                            // 如果没找到向下走的点，说明用户StateTrans没往下连，这里就直接break用默认的_final就行了
                                            if (nowConnector_VM == null)
                                            {
                                                break;
                                            }
                                        }
                                        else // 遇到三类状态了，结束
                                        {
                                            break;
                                        }
                                    }

                                    // 目标状态名，如果没找到（StatTrans没往下连）那就认为是连到FinalState了
                                    string destState = "_final";
                                    // 根据三类状态不同计算destState名称
                                    if (nowConnector_VM == null)
                                    {
                                        destState = "_final";
                                    }
                                    else if (nowConnector_VM.NetworkItemVM is State_VM)
                                    {
                                        State_VM state_VM = (State_VM)nowConnector_VM.NetworkItemVM;
                                        destState = state_VM.State.Name;
                                    }
                                    else if (nowConnector_VM.NetworkItemVM is FinalState_VM)
                                    {
                                        destState = "_final";
                                    }
                                    else if (nowConnector_VM.NetworkItemVM is InitState_VM) // 不符合规约
                                    {
                                        destState = "_init";
                                    }
                                    // 源和目标状态
                                    xmlWriter.WriteAttributeString("source", sourceState);
                                    xmlWriter.WriteAttributeString("dest", destState);
                                    // 转移上的Guard和Action
                                    if (stateTrans_VM != null)
                                    {
                                        xmlWriter.WriteAttributeString("guard", stateTrans_VM.StateTrans.Guard.Content);
                                        //foreach (Formula formula in stateTrans_VM.StateTrans.Guards) // Guard条件列表
                                        //{
                                        //    xmlWriter.WriteStartElement("Guard");
                                        //    xmlWriter.WriteAttributeString("content", formula.Content);
                                        //    xmlWriter.WriteEndElement();
                                        //}
                                        foreach (Formula formula in stateTrans_VM.StateTrans.Actions) // Action动作列表
                                        {
                                            xmlWriter.WriteStartElement("Action");
                                            xmlWriter.WriteAttributeString("content", formula.Content);
                                            xmlWriter.WriteEndElement();
                                        }
                                    }
                                    xmlWriter.WriteEndElement();
                                }
                            }
                        }
                    }
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement();

            #endregion

            // 协议尾
            xmlWriter.WriteEndElement();
            xmlWriter.Flush();
            xmlWriter.Close();

            ResourceManager.mainWindowVM.Tips = "生成后端XML，至：" + fpath;
        }
    }
}
