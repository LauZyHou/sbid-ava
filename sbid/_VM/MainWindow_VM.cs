using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ReactiveUI;
using System.Reactive;
using sbid._M;
using Avalonia.Controls;
using System.Threading.Tasks;
using System.Xml;
using Avalonia.Media;

namespace sbid._VM
{
    public class MainWindow_VM : ViewModelBase
    {
        #region 字段

        private string tips = "在此处获取操作提示";
        private ObservableCollection<Protocol_VM> protocolVMs = new ObservableCollection<Protocol_VM>();
        private Protocol_VM selectedItem;
        private bool connectorVisible = true;

        #endregion

        #region 构造

        public MainWindow_VM()
        {
            // 把自己挂到全局资源上
            ResourceManager.mainWindowVM = this;
        }

        #endregion

        #region 命令控制

        // 添加新协议
        public void AddProtocol()
        {
            Protocol_VM protocol_VM = new Protocol_VM();
            protocolVMs.Add(protocol_VM);
            SelectedItem = protocol_VM;

            // 为协议的类图面板初始化int和bool
            ClassDiagram_P_VM classDiagram_P_VM = (ClassDiagram_P_VM)protocol_VM.PanelVMs[0].SidePanelVMs[0];
            classDiagram_P_VM.init_data();
        }

        // 添加"进程模板-状态机"大面板,要将所在Process引用传入以反向查询,将大面板返回以给Process_VM集成
        // 此方法在用户创建Process时调用
        public ProcessToSM_P_VM AddProcessToSM(Process process)
        {
            ProcessToSM_P_VM pvm = new ProcessToSM_P_VM(process);
            pvm.init_data(); // 这里实际是初始化第一个孩子StateMachine(顶层)

            // 添加到当前协议的状态机下
            selectedItem.PanelVMs[1].SidePanelVMs.Add(pvm);
            selectedItem.PanelVMs[1].SelectedItem = pvm;

            return pvm;
        }

        // 添加新拓扑图
        public void AddTopoGraph()
        {
            if (selectedItem == null)
                return;

            selectedItem.SelectedItem = selectedItem.PanelVMs[2];

            TopoGraph_P_VM pvm = new TopoGraph_P_VM();
            selectedItem.PanelVMs[2].SidePanelVMs.Add(pvm);
            selectedItem.PanelVMs[2].SelectedItem = pvm;
        }

        // 添加新攻击树
        public void AddAttackTree()
        {
            if (selectedItem == null)
                return;

            selectedItem.SelectedItem = selectedItem.PanelVMs[3];

            AttackTree_P_VM pvm = new AttackTree_P_VM();
            selectedItem.PanelVMs[3].SidePanelVMs.Add(pvm);
            selectedItem.PanelVMs[3].SelectedItem = pvm;
        }

        // 添加新CTL语法树
        public void AddCTLTree()
        {
            if (selectedItem == null)
                return;

            selectedItem.SelectedItem = selectedItem.PanelVMs[4];

            CTLTree_P_VM pvm = new CTLTree_P_VM();
            selectedItem.PanelVMs[4].SidePanelVMs.Add(pvm);
            selectedItem.PanelVMs[4].SelectedItem = pvm;
        }

        // 添加新顺序图
        public void AddSequenceDiagram()
        {
            if (selectedItem == null)
                return;

            selectedItem.SelectedItem = selectedItem.PanelVMs[5];

            SequenceDiagram_P_VM pvm = new SequenceDiagram_P_VM();
            selectedItem.PanelVMs[5].SidePanelVMs.Add(pvm);
            selectedItem.PanelVMs[5].SelectedItem = pvm;
        }

        // 按下【保存】按钮
        public async void SaveAllVM()
        {
            string saveFileName = await GetSaveFileName();
            if (string.IsNullOrEmpty(saveFileName))
            {
                Tips = "取消保存项目";
                return;
            }
            // 执行保存逻辑
            bool succ = DoSave(saveFileName);
            if (succ)
                Tips = "项目保存，至：" + saveFileName;
            else
                Tips = "[ERROR]项目保存失败！";
        }

        // 按下【载入】按钮
        public async void ReloadAllVM()
        {
            string openFileName = await GetOpenFileName();
            if (string.IsNullOrEmpty(openFileName))
            {
                Tips = "取消载入项目";
                return;
            }
            // 执行载入逻辑
            bool succ = DoReload(openFileName);
            if (succ)
                Tips = "载入项目，从：" + openFileName;
            //else
            //    Tips = "项目载入失败！请检查项目文件是否被手动修改";
            // 这里改成在返回false时提示具体的错误
        }

        // 按下【生成XML】按钮
        public async void GenerateXml()
        {
            string saveFileName = await GetSaveFileName();
            if (string.IsNullOrEmpty(saveFileName))
            {
                Tips = "取消生成XML";
                return;
            }
            // 执行生成XML逻辑，这一套是生成验证用的XML，不能用来返回模型文件
            bool succ = DoSave2(saveFileName);
            if (succ)
                Tips = "生成XML，至：" + saveFileName;
            else
                Tips = "[ERROR]XML生成失败！";
        }
        #endregion

        #region 私有

        // 预打开文件：返回文件路径
        private async Task<string> GetOpenFileName()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filters.Add(new FileDialogFilter() { Name = "项目文件", Extensions = { "sbid" } });
            string[] result = await dialog.ShowAsync(ResourceManager.mainWindowV);
            return result == null ? "" : string.Join(" ", result); // Linux bugfix:直接关闭时不能返回null
        }

        // 预保存文件：返回文件路径
        private async Task<string> GetSaveFileName()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filters.Add(new FileDialogFilter() { Name = "项目文件", Extensions = { "sbid" } });
            string result = await dialog.ShowAsync(ResourceManager.mainWindowV);
            // Linux bugfix:某些平台输入文件名不会自动补全.sbid后缀名,这里判断一下手动补上
            if (string.IsNullOrEmpty(result) || result.EndsWith(".sbid"))
                return result;
            return result + ".sbid";
        }

        // 执行保存项目，传入".sbid"文件名，返回是否保存成功
        // 每个协议分成一个文件，保存到和".sbid"文件同一目录下，在".sbid"中记录这些协议文件的前缀名
        private bool DoSave(string fileName)
        {
            // 同级目录
            //string dirPath = System.IO.Path.GetDirectoryName(fileName);
            // 项目文件去除后缀名".sbid"部分
            string cleanName = fileName.Substring(0, fileName.Length - 5);

            // 写入".sbid"文件
            XmlTextWriter xmlWriter = new XmlTextWriter(fileName, null);
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.WriteStartElement("Project");
            string nowTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            xmlWriter.WriteAttributeString("updTime", nowTime); // 最后修改时间
            for (int i = 0; i < protocolVMs.Count; i++)
            {
                xmlWriter.WriteStartElement("Protocol_VM");
                // 因为协议可能重名，这里用从0开始的自增id
                xmlWriter.WriteAttributeString("id", i.ToString());
                // 这里是为了方便用户查看，放个Name进去，实际上对于找对应协议的xml文件没作用
                xmlWriter.WriteAttributeString("name", protocolVMs[i].Protocol.Name);
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement();
            xmlWriter.Flush();
            xmlWriter.Close();

            // 写入各个协议的".xml"文件
            for (int i = 0; i < protocolVMs.Count; i++)
            {
                // 当前协议
                Protocol_VM protocolVM = protocolVMs[i];
                // 每个协议文件名是"项目名_i.xml"
                xmlWriter = new XmlTextWriter(cleanName + "_" + i.ToString() + ".xml", null);
                xmlWriter.Formatting = Formatting.Indented;
                xmlWriter.WriteStartElement("Protocol_VM");
                xmlWriter.WriteAttributeString("name", protocolVM.Protocol.Name);

                #region 概览>类图面板
                // 这个比较特殊，因为就这一个，而且一定有这一个，用户既不能创建也不能销毁
                xmlWriter.WriteStartElement("ClassDiagram_P_VM");
                ClassDiagram_P_VM classDiagram_P_VM = (ClassDiagram_P_VM)protocolVM.PanelVMs[0].SidePanelVMs[0];
                foreach (ViewModelBase item in classDiagram_P_VM.UserControlVMs)
                {
                    // 按照每个类图的类型做不同的保存方法
                    if (item is UserType_VM)
                    {
                        UserType_VM vm = (UserType_VM)item;
                        xmlWriter.WriteStartElement("UserType_VM");
                        xmlWriter.WriteAttributeString("x", vm.X.ToString());
                        xmlWriter.WriteAttributeString("y", vm.Y.ToString());
                        xmlWriter.WriteAttributeString("name", vm.Type.Name);
                        xmlWriter.WriteAttributeString("id", vm.Type.Id.ToString());
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
                                xmlWriter.WriteAttributeString("parent_ref", userType.Parent.Id.ToString());
                            else // 无继承关系
                                xmlWriter.WriteAttributeString("parent_ref", "-1");
                            foreach (Attribute attr in userType.Attributes)
                            {
                                xmlWriter.WriteStartElement("Attribute");
                                ResourceManager.writeAttribute(xmlWriter, attr);
                                xmlWriter.WriteEndElement();
                            }
                            foreach (Method method in userType.Methods)
                            {
                                xmlWriter.WriteStartElement("Method");
                                xmlWriter.WriteAttributeString("returnType_ref", method.ReturnType.Id.ToString());
                                xmlWriter.WriteAttributeString("name", method.Name);
                                xmlWriter.WriteAttributeString("cryptoSuffix", method.CryptoSuffix.ToString());
                                xmlWriter.WriteAttributeString("id", method.Id.ToString());
                                foreach (Attribute attr in method.Parameters)
                                {
                                    xmlWriter.WriteStartElement("Parameter"); // 注意这里叫Parameter了
                                    ResourceManager.writeAttribute(xmlWriter, attr);
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
                        xmlWriter.WriteStartElement("Process_VM");
                        xmlWriter.WriteAttributeString("x", vm.X.ToString());
                        xmlWriter.WriteAttributeString("y", vm.Y.ToString());
                        xmlWriter.WriteAttributeString("name", vm.Process.RefName.Content);
                        xmlWriter.WriteAttributeString("id", vm.Process.Id.ToString());
                        foreach (Attribute attr in vm.Process.Attributes)
                        {
                            xmlWriter.WriteStartElement("Attribute");
                            ResourceManager.writeAttribute(xmlWriter, attr);
                            xmlWriter.WriteEndElement();
                        }
                        foreach (Method method in vm.Process.Methods)
                        {
                            xmlWriter.WriteStartElement("Method");
                            xmlWriter.WriteAttributeString("returnType_ref", method.ReturnType.Id.ToString());
                            xmlWriter.WriteAttributeString("name", method.Name);
                            xmlWriter.WriteAttributeString("cryptoSuffix", method.CryptoSuffix.ToString());
                            xmlWriter.WriteAttributeString("id", method.Id.ToString());
                            foreach (Attribute attr in method.Parameters)
                            {
                                xmlWriter.WriteStartElement("Parameter");
                                ResourceManager.writeAttribute(xmlWriter, attr);
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
                            xmlWriter.WriteAttributeString("id", commMethod.Id.ToString());
                            foreach (Attribute attr in commMethod.Parameters)
                            {
                                xmlWriter.WriteStartElement("Parameter");
                                ResourceManager.writeAttribute(xmlWriter, attr);
                                xmlWriter.WriteEndElement();
                            }
                            xmlWriter.WriteEndElement();
                        }
                        xmlWriter.WriteEndElement();
                    }
                    else if (item is Axiom_VM)
                    {
                        Axiom_VM vm = (Axiom_VM)item;
                        xmlWriter.WriteStartElement("Axiom_VM");
                        xmlWriter.WriteAttributeString("x", vm.X.ToString());
                        xmlWriter.WriteAttributeString("y", vm.Y.ToString());
                        xmlWriter.WriteAttributeString("name", vm.Axiom.Name);
                        xmlWriter.WriteAttributeString("id", vm.Axiom.Id.ToString());
                        foreach (ProcessMethod processMethod in vm.Axiom.ProcessMethods)
                        {
                            xmlWriter.WriteStartElement("ProcessMethod");
                            xmlWriter.WriteAttributeString("process_ref", processMethod.Process.Id.ToString());
                            xmlWriter.WriteAttributeString("method_ref", processMethod.Method.Id.ToString());
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
                        xmlWriter.WriteStartElement("InitialKnowledge_VM");
                        xmlWriter.WriteAttributeString("x", vm.X.ToString());
                        xmlWriter.WriteAttributeString("y", vm.Y.ToString());
                        if (vm.InitialKnowledge.Process == null) // 全局，不关联Process
                        {
                            xmlWriter.WriteAttributeString("process_ref", "-1");
                        }
                        else
                        {
                            xmlWriter.WriteAttributeString("process_ref", vm.InitialKnowledge.Process.Id.ToString());
                        }
                        xmlWriter.WriteAttributeString("id", vm.InitialKnowledge.Id.ToString());
                        foreach (Knowledge knowledge in vm.InitialKnowledge.Knowledges)
                        {
                            xmlWriter.WriteStartElement("Knowledge");
                            xmlWriter.WriteAttributeString("process_ref", knowledge.Process.Id.ToString());
                            // 为了这里需要，特地为Attribute也添加了Id，读取xml的时候才能知道选的是Process的哪个Attribute
                            // 下面类似的地方标注了"注意"字样
                            xmlWriter.WriteAttributeString("attribute_ref", knowledge.Attribute.Id.ToString());
                            xmlWriter.WriteEndElement();
                        }
                        foreach (KeyPair keyPair in vm.InitialKnowledge.KeyPairs)
                        {
                            xmlWriter.WriteStartElement("KeyPair");
                            xmlWriter.WriteAttributeString("pubProcess_ref", keyPair.PubProcess.Id.ToString());
                            xmlWriter.WriteAttributeString("pubKey_ref", keyPair.PubKey.Id.ToString());
                            xmlWriter.WriteAttributeString("secProcess_ref", keyPair.SecProcess.Id.ToString());
                            xmlWriter.WriteAttributeString("secKey_ref", keyPair.SecKey.Id.ToString());
                            xmlWriter.WriteEndElement();
                        }
                        xmlWriter.WriteEndElement();
                    }
                    else if (item is SafetyProperty_VM)
                    {
                        SafetyProperty_VM vm = (SafetyProperty_VM)item;
                        xmlWriter.WriteStartElement("SafetyProperty_VM");
                        xmlWriter.WriteAttributeString("x", vm.X.ToString());
                        xmlWriter.WriteAttributeString("y", vm.Y.ToString());
                        xmlWriter.WriteAttributeString("name", vm.SafetyProperty.Name);
                        xmlWriter.WriteAttributeString("id", vm.SafetyProperty.Id.ToString());
                        foreach (CTL ctl in vm.SafetyProperty.CTLs)
                        {
                            xmlWriter.WriteStartElement("CTL");
                            xmlWriter.WriteAttributeString("process_ref", ctl.Process.Id.ToString());
                            xmlWriter.WriteAttributeString("state_ref", ctl.State.Id.ToString());
                            xmlWriter.WriteAttributeString("formula", ctl.Formula.Content);
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
                        xmlWriter.WriteStartElement("SecurityProperty_VM");
                        xmlWriter.WriteAttributeString("x", vm.X.ToString());
                        xmlWriter.WriteAttributeString("y", vm.Y.ToString());
                        xmlWriter.WriteAttributeString("name", vm.SecurityProperty.Name);
                        xmlWriter.WriteAttributeString("id", vm.SecurityProperty.Id.ToString());
                        foreach (Confidential confidential in vm.SecurityProperty.Confidentials)
                        {
                            xmlWriter.WriteStartElement("Confidential");
                            xmlWriter.WriteAttributeString("process_ref", confidential.Process.Id.ToString());
                            xmlWriter.WriteAttributeString("attribute_ref", confidential.Attribute.Id.ToString()); // 注意
                            xmlWriter.WriteEndElement();
                        }
                        foreach (Authenticity authenticity in vm.SecurityProperty.Authenticities)
                        {
                            xmlWriter.WriteStartElement("Authenticity");
                            xmlWriter.WriteAttributeString("processA_ref", authenticity.ProcessA.Id.ToString());
                            xmlWriter.WriteAttributeString("stateA_ref", authenticity.StateA.Id.ToString());
                            xmlWriter.WriteAttributeString("attributeA_ref", authenticity.AttributeA.Id.ToString()); // 注意
                            xmlWriter.WriteAttributeString("attributeA_Attr_ref", authenticity.AttributeA_Attr.Id.ToString()); // 注意
                            xmlWriter.WriteAttributeString("processB_ref", authenticity.ProcessB.Id.ToString());
                            xmlWriter.WriteAttributeString("stateB_ref", authenticity.StateB.Id.ToString());
                            xmlWriter.WriteAttributeString("attributeB_ref", authenticity.AttributeB.Id.ToString()); // 注意
                            xmlWriter.WriteAttributeString("attributeB_Attr_ref", authenticity.AttributeB_Attr.Id.ToString()); // 注意
                            xmlWriter.WriteEndElement();
                        }
                        foreach (Integrity integrity in vm.SecurityProperty.Integrities)
                        {
                            xmlWriter.WriteStartElement("Integrity");
                            xmlWriter.WriteAttributeString("processA_ref", integrity.ProcessA.Id.ToString());
                            xmlWriter.WriteAttributeString("stateA_ref", integrity.StateA.Id.ToString());
                            xmlWriter.WriteAttributeString("attributeA_ref", integrity.AttributeA.Id.ToString()); // 注意
                            xmlWriter.WriteAttributeString("processB_ref", integrity.ProcessB.Id.ToString());
                            xmlWriter.WriteAttributeString("stateB_ref", integrity.StateB.Id.ToString());
                            xmlWriter.WriteAttributeString("attributeB_ref", integrity.AttributeB.Id.ToString()); // 注意
                            xmlWriter.WriteEndElement();
                        }
                        xmlWriter.WriteEndElement();
                    }
                    else if (item is CommChannel_VM)
                    {
                        CommChannel_VM vm = (CommChannel_VM)item;
                        xmlWriter.WriteStartElement("CommChannel_VM");
                        xmlWriter.WriteAttributeString("x", vm.X.ToString());
                        xmlWriter.WriteAttributeString("y", vm.Y.ToString());
                        xmlWriter.WriteAttributeString("name", vm.CommChannel.Name);
                        xmlWriter.WriteAttributeString("id", vm.CommChannel.Id.ToString());
                        foreach (CommMethodPair commMethodPair in vm.CommChannel.CommMethodPairs)
                        {
                            xmlWriter.WriteStartElement("CommMethodPair");
                            xmlWriter.WriteAttributeString("id", commMethodPair.Id.ToString());
                            xmlWriter.WriteAttributeString("processA_ref", commMethodPair.ProcessA.Id.ToString());
                            xmlWriter.WriteAttributeString("commMethodA_ref", commMethodPair.CommMethodA.Id.ToString()); // 这里用了CommMethod的Id
                            xmlWriter.WriteAttributeString("processB_ref", commMethodPair.ProcessB.Id.ToString());
                            xmlWriter.WriteAttributeString("commMethodB_ref", commMethodPair.CommMethodB.Id.ToString()); // 这里用了CommMethod的Id
                            xmlWriter.WriteAttributeString("privacy", commMethodPair.Privacy.ToString());
                            xmlWriter.WriteEndElement();
                        }
                        xmlWriter.WriteEndElement();
                    }
                }
                xmlWriter.WriteEndElement();
                #endregion

                #region "进程模板-状态机"面板
                xmlWriter.WriteStartElement("ProcessToSM_P_VMs");
                ObservableCollection<SidePanel_VM> processToSM_P_VMs = protocolVM.PanelVMs[1].SidePanelVMs; // 所有的"进程模板-状态机"面板
                foreach (SidePanel_VM sidePanel_VM in processToSM_P_VMs)
                {
                    ProcessToSM_P_VM processToSM_P_VM = (ProcessToSM_P_VM)sidePanel_VM;
                    xmlWriter.WriteStartElement("ProcessToSM_P_VM");
                    xmlWriter.WriteAttributeString("process_ref", processToSM_P_VM.Process.Id.ToString());
                    // 对于里面的每个状态机面板，写入XML
                    foreach (StateMachine_P_VM stateMachine_P_VM in processToSM_P_VM.StateMachinePVMs)
                    {
                        xmlWriter.WriteStartElement("StateMachine_P_VM");
                        xmlWriter.WriteAttributeString("state_ref", stateMachine_P_VM.State.Id.ToString());
                        foreach (ViewModelBase vm in stateMachine_P_VM.UserControlVMs) // 写入状态机的结点和连线等
                        {
                            if (vm is State_VM)
                            {
                                State_VM state_VM = (State_VM)vm;
                                xmlWriter.WriteStartElement("State_VM");
                                xmlWriter.WriteAttributeString("x", state_VM.X.ToString());
                                xmlWriter.WriteAttributeString("y", state_VM.Y.ToString());
                                xmlWriter.WriteAttributeString("name", state_VM.State.Name);
                                xmlWriter.WriteAttributeString("id", state_VM.State.Id.ToString());
                                foreach (Connector_VM connector_VM in state_VM.ConnectorVMs) // 身上所有锚点的id号
                                {
                                    xmlWriter.WriteStartElement("Connector_VM");
                                    xmlWriter.WriteAttributeString("id", connector_VM.Id.ToString());
                                    xmlWriter.WriteEndElement();
                                }
                                xmlWriter.WriteEndElement();
                            }
                            /*
                            else if (vm is Transition_VM)
                            {
                                Transition_VM transition_VM = (Transition_VM)vm;
                                xmlWriter.WriteStartElement("Transition_VM");
                                xmlWriter.WriteAttributeString("source_ref", transition_VM.Source.Id.ToString()); // 源锚点
                                xmlWriter.WriteAttributeString("dest_ref", transition_VM.Dest.Id.ToString()); // 目标锚点
                                xmlWriter.WriteAttributeString("guard", transition_VM.Transition.Guard); // Guard条件
                                foreach (Formula formula in transition_VM.Transition.Actions) // Actions列表
                                {
                                    xmlWriter.WriteStartElement("Action");
                                    xmlWriter.WriteAttributeString("content", formula.Content);
                                    xmlWriter.WriteEndElement();
                                }
                                xmlWriter.WriteEndElement();
                            }
                            */
                            else if (vm is InitState_VM)
                            {
                                InitState_VM initState_VM = (InitState_VM)vm;
                                xmlWriter.WriteStartElement("InitState_VM");
                                xmlWriter.WriteAttributeString("x", initState_VM.X.ToString());
                                xmlWriter.WriteAttributeString("y", initState_VM.Y.ToString());
                                // *需不需要给InitState_VM本身设置id？目前的想法是不需要id
                                foreach (Connector_VM connector_VM in initState_VM.ConnectorVMs) // 身上所有锚点的id号
                                {
                                    xmlWriter.WriteStartElement("Connector_VM");
                                    xmlWriter.WriteAttributeString("id", connector_VM.Id.ToString());
                                    xmlWriter.WriteEndElement();
                                }
                                xmlWriter.WriteEndElement();
                            }
                            else if (vm is FinalState_VM)
                            {
                                FinalState_VM finalState_VM = (FinalState_VM)vm;
                                xmlWriter.WriteStartElement("FinalState_VM");
                                xmlWriter.WriteAttributeString("x", finalState_VM.X.ToString());
                                xmlWriter.WriteAttributeString("y", finalState_VM.Y.ToString());
                                // *需不需要给FinalState_VM本身设置id？目前的想法是不需要id
                                foreach (Connector_VM connector_VM in finalState_VM.ConnectorVMs) // 身上所有锚点的id号
                                {
                                    xmlWriter.WriteStartElement("Connector_VM");
                                    xmlWriter.WriteAttributeString("id", connector_VM.Id.ToString());
                                    xmlWriter.WriteEndElement();
                                }
                                xmlWriter.WriteEndElement();
                            }
                            else if (vm is StateTrans_VM)
                            {
                                StateTrans_VM stateTrans_VM = (StateTrans_VM)vm;
                                xmlWriter.WriteStartElement("StateTrans_VM");
                                xmlWriter.WriteAttributeString("x", stateTrans_VM.X.ToString());
                                xmlWriter.WriteAttributeString("y", stateTrans_VM.Y.ToString());
                                foreach (Formula formula in stateTrans_VM.StateTrans.Guards) // Guard条件列表
                                {
                                    xmlWriter.WriteStartElement("Guard");
                                    xmlWriter.WriteAttributeString("content", formula.Content);
                                    xmlWriter.WriteEndElement();
                                }
                                foreach (Formula formula in stateTrans_VM.StateTrans.Actions) // Action动作列表
                                {
                                    xmlWriter.WriteStartElement("Action");
                                    xmlWriter.WriteAttributeString("content", formula.Content);
                                    xmlWriter.WriteEndElement();
                                }
                                foreach (Connector_VM connector_VM in stateTrans_VM.ConnectorVMs) // 身上所有锚点的id号
                                {
                                    xmlWriter.WriteStartElement("Connector_VM");
                                    xmlWriter.WriteAttributeString("id", connector_VM.Id.ToString());
                                    xmlWriter.WriteEndElement();
                                }
                                xmlWriter.WriteEndElement();
                            }
                            else if (vm is ControlPoint_VM)
                            {
                                ControlPoint_VM controlPoint_VM = (ControlPoint_VM)vm;
                                xmlWriter.WriteStartElement("ControlPoint_VM");
                                xmlWriter.WriteAttributeString("x", controlPoint_VM.X.ToString());
                                xmlWriter.WriteAttributeString("y", controlPoint_VM.Y.ToString());
                                foreach (Connector_VM connector_VM in controlPoint_VM.ConnectorVMs) // 身上所有锚点的id号
                                {
                                    xmlWriter.WriteStartElement("Connector_VM");
                                    xmlWriter.WriteAttributeString("id", connector_VM.Id.ToString());
                                    xmlWriter.WriteEndElement();
                                }
                                xmlWriter.WriteEndElement();
                            }
                            else if (vm is Arrow_VM) // Arrow继承自Connection所以必须放在前面
                            {
                                Arrow_VM arrow_VM = (Arrow_VM)vm;
                                xmlWriter.WriteStartElement("Arrow_VM");
                                xmlWriter.WriteAttributeString("source_ref", arrow_VM.Source.Id.ToString()); // 源锚点
                                xmlWriter.WriteAttributeString("dest_ref", arrow_VM.Dest.Id.ToString()); // 目标锚点
                                xmlWriter.WriteEndElement();
                            }
                            else if (vm is Connection_VM)
                            {
                                Connection_VM connection_VM = (Connection_VM)vm;
                                xmlWriter.WriteStartElement("Connection_VM");
                                xmlWriter.WriteAttributeString("source_ref", connection_VM.Source.Id.ToString()); // 源锚点
                                xmlWriter.WriteAttributeString("dest_ref", connection_VM.Dest.Id.ToString()); // 目标锚点
                                xmlWriter.WriteEndElement();
                            }
                        }
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
                #endregion

                #region 攻击树面板
                xmlWriter.WriteStartElement("AttackTree_P_VMs");
                ObservableCollection<SidePanel_VM> attackTree_P_VMs = protocolVM.PanelVMs[3].SidePanelVMs; // 所有的攻击树
                foreach (SidePanel_VM sidePanel_VM in attackTree_P_VMs)
                {
                    AttackTree_P_VM attackTree_P_VM = (AttackTree_P_VM)sidePanel_VM;
                    xmlWriter.WriteStartElement("AttackTree_P_VM");
                    xmlWriter.WriteAttributeString("name", attackTree_P_VM.RefName.Content);
                    foreach (ViewModelBase vm in attackTree_P_VM.UserControlVMs) // 写入结点和连线等
                    {
                        if (vm is Attack_VM)
                        {
                            Attack_VM attack_VM = (Attack_VM)vm;
                            xmlWriter.WriteStartElement("Attack_VM");
                            xmlWriter.WriteAttributeString("x", attack_VM.X.ToString());
                            xmlWriter.WriteAttributeString("y", attack_VM.Y.ToString());
                            xmlWriter.WriteAttributeString("beAttacked", attack_VM.BeAttacked.ToString());
                            xmlWriter.WriteAttributeString("isLocked", attack_VM.IsLocked.ToString());
                            xmlWriter.WriteAttributeString("content", attack_VM.Attack.Content);
                            foreach (Connector_VM connector_VM in attack_VM.ConnectorVMs) // 身上所有锚点的id号
                            {
                                xmlWriter.WriteStartElement("Connector_VM");
                                xmlWriter.WriteAttributeString("id", connector_VM.Id.ToString());
                                xmlWriter.WriteEndElement();
                            }
                            xmlWriter.WriteEndElement();
                        }
                        else if (vm is Relation_VM)
                        {
                            Relation_VM relation_VM = (Relation_VM)vm;
                            xmlWriter.WriteStartElement("Relation_VM");
                            xmlWriter.WriteAttributeString("x", relation_VM.X.ToString());
                            xmlWriter.WriteAttributeString("y", relation_VM.Y.ToString());
                            xmlWriter.WriteAttributeString("relation", relation_VM.Relation.ToString());
                            foreach (Connector_VM connector_VM in relation_VM.ConnectorVMs) // 身上所有锚点的id号
                            {
                                xmlWriter.WriteStartElement("Connector_VM");
                                xmlWriter.WriteAttributeString("id", connector_VM.Id.ToString());
                                xmlWriter.WriteEndElement();
                            }
                            xmlWriter.WriteEndElement();
                        }
                        else if (vm is Arrow_VM)
                        {
                            Arrow_VM arrow_VM = (Arrow_VM)vm;
                            xmlWriter.WriteStartElement("Arrow_VM");
                            xmlWriter.WriteAttributeString("source_ref", arrow_VM.Source.Id.ToString()); // 源锚点
                            xmlWriter.WriteAttributeString("dest_ref", arrow_VM.Dest.Id.ToString()); // 目标锚点
                            xmlWriter.WriteEndElement();
                        }
                    }
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
                #endregion

                #region 序列图面板
                xmlWriter.WriteStartElement("SequenceDiagram_P_VMs");
                ObservableCollection<SidePanel_VM> sequenceDiagram_P_VMs = protocolVM.PanelVMs[5].SidePanelVMs; // 所有的序列图
                foreach (SidePanel_VM sidePanel_VM in sequenceDiagram_P_VMs)
                {
                    SequenceDiagram_P_VM sequenceDiagram_P_VM = (SequenceDiagram_P_VM)sidePanel_VM;
                    xmlWriter.WriteStartElement("SequenceDiagram_P_VM");
                    xmlWriter.WriteAttributeString("name", sequenceDiagram_P_VM.RefName.Content);
                    foreach (ViewModelBase vm in sequenceDiagram_P_VM.UserControlVMs) // 对象-生命线 或 消息
                    {
                        if (vm is ObjLifeLine_VM)
                        {
                            ObjLifeLine_VM objLifeLine_VM = (ObjLifeLine_VM)vm;
                            xmlWriter.WriteStartElement("ObjLifeLine_VM");
                            xmlWriter.WriteAttributeString("x", objLifeLine_VM.X.ToString());
                            xmlWriter.WriteAttributeString("y", objLifeLine_VM.Y.ToString());
                            if (objLifeLine_VM.SeqObject.Process != null) // 设了Process
                                xmlWriter.WriteAttributeString("process_ref", objLifeLine_VM.SeqObject.Process.Id.ToString());
                            else // 还没设Process
                                xmlWriter.WriteAttributeString("process_ref", "-1");
                            foreach (Connector_VM connector_VM in objLifeLine_VM.ConnectorVMs) // 身上所有锚点的id号
                            {
                                xmlWriter.WriteStartElement("Connector_VM");
                                xmlWriter.WriteAttributeString("id", connector_VM.Id.ToString());
                                xmlWriter.WriteEndElement();
                            }
                            xmlWriter.WriteEndElement();
                        }
                        else if (vm is Message_VM) // 这是消息的基类，具体要创建的标签取决于具体消息类型
                        {
                            Message_VM message_VM = (Message_VM)vm;
                            if (vm is SyncMessage_VM)
                            {
                                xmlWriter.WriteStartElement("SyncMessage_VM");
                            }
                            else if (vm is AsyncMessage_VM)
                            {
                                xmlWriter.WriteStartElement("AsyncMessage_VM");
                            }
                            else if (vm is ReturnMessage_VM)
                            {
                                xmlWriter.WriteStartElement("ReturnMessage_VM");
                            }
                            else if (vm is SyncMessage_Self_VM)
                            {
                                xmlWriter.WriteStartElement("SyncMessage_Self_VM");
                            }
                            else if (vm is AsyncMessage_Self_VM)
                            {
                                xmlWriter.WriteStartElement("AsyncMessage_Self_VM");
                            }
                            else
                            {
                                continue;
                            }
                            if (message_VM.CommMessage.CommMethod != null) // 设了CommMethod
                                xmlWriter.WriteAttributeString("commMethod_ref", message_VM.CommMessage.CommMethod.Id.ToString());
                            else // 还没设CommMethod
                                xmlWriter.WriteAttributeString("commMethod_ref", "-1");
                            xmlWriter.WriteAttributeString("source_ref", message_VM.Source.Id.ToString()); // 源锚点
                            xmlWriter.WriteAttributeString("dest_ref", message_VM.Dest.Id.ToString()); // 目标锚点
                            xmlWriter.WriteEndElement();
                        }
                    }
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
                #endregion

                #region 拓扑图面板
                xmlWriter.WriteStartElement("TopoGraph_P_VMs");
                ObservableCollection<SidePanel_VM> topoGrapgh_P_VMs = protocolVM.PanelVMs[2].SidePanelVMs; // 所有的拓扑图
                foreach (SidePanel_VM sidePanel_VM in topoGrapgh_P_VMs)
                {
                    TopoGraph_P_VM topoGraph_P_VM = (TopoGraph_P_VM)sidePanel_VM;
                    xmlWriter.WriteStartElement("TopoGraph_P_VM");
                    xmlWriter.WriteAttributeString("name", topoGraph_P_VM.RefName.Content);
                    foreach (ViewModelBase vm in topoGraph_P_VM.UserControlVMs) // 拓扑结点 或 拓扑连线
                    {
                        if (vm is TopoNode_VM) // 拓扑结点
                        {
                            TopoNode_VM topoNode_VM = (TopoNode_VM)vm;
                            xmlWriter.WriteStartElement("TopoNode_VM");
                            xmlWriter.WriteAttributeString("x", topoNode_VM.X.ToString());
                            xmlWriter.WriteAttributeString("y", topoNode_VM.Y.ToString());
                            xmlWriter.WriteAttributeString("name", topoNode_VM.TopoNode.Name);
                            if (vm is TopoNode_Circle_VM)
                                xmlWriter.WriteAttributeString("shape", "Circle");
                            else if (vm is TopoNode_Square_VM)
                                xmlWriter.WriteAttributeString("shape", "Square");
                            xmlWriter.WriteAttributeString("color", topoNode_VM.TopoNode.Color.ToString());
                            if (topoNode_VM.TopoNode.Process != null) // 设了Process
                                xmlWriter.WriteAttributeString("process_ref", topoNode_VM.TopoNode.Process.Id.ToString());
                            else // 还没设Process
                                xmlWriter.WriteAttributeString("process_ref", "-1");
                            foreach (Connector_VM connector_VM in topoNode_VM.ConnectorVMs) // 身上所有锚点的id号
                            {
                                xmlWriter.WriteStartElement("Connector_VM");
                                xmlWriter.WriteAttributeString("id", connector_VM.Id.ToString());
                                xmlWriter.WriteEndElement();
                            }
                            // 例化的各个Instance的保存
                            foreach (Instance instance in topoNode_VM.TopoNode.Properties)
                            {
                                ResourceManager.writeInstance(xmlWriter, instance);
                            }
                            xmlWriter.WriteEndElement();
                        }
                        /*
                        else if (vm is TopoLink_VM) // 这是拓扑连线的基类,具体要创建的标签取决于具体连线类型
                        {
                            TopoLink_VM topoLink_VM = (TopoLink_VM)vm;
                            if (topoLink_VM is OneWayTopoLink_VM)
                            {
                                xmlWriter.WriteStartElement("OneWayTopoLink_VM");
                            }
                            else if (topoLink_VM is TwoWayTopoLink_VM)
                            {
                                xmlWriter.WriteStartElement("TwoWayTopoLink_VM");
                            }
                            else
                            {
                                continue;
                            }
                            xmlWriter.WriteAttributeString("content", topoLink_VM.TopoLink.Content); // 线上内容，即TopoLink对象唯一字段
                            xmlWriter.WriteAttributeString("source_ref", topoLink_VM.Source.Id.ToString()); // 源锚点
                            xmlWriter.WriteAttributeString("dest_ref", topoLink_VM.Dest.Id.ToString()); // 目标锚点
                            xmlWriter.WriteEndElement();
                        }
                        */
                        else if (vm is TopoEdge_VM) // 拓扑图上的边
                        {
                            TopoEdge_VM topoEdge_VM = (TopoEdge_VM)vm;
                            xmlWriter.WriteStartElement("TopoEdge_VM");
                            xmlWriter.WriteAttributeString("cost", topoEdge_VM.TopoEdge.Cost); // 通信代价
                            // 通信方法序对
                            if (topoEdge_VM.TopoEdge.CommMethodPair == null)
                            {
                                xmlWriter.WriteAttributeString("commMethodPair_ref", "-1");
                            }
                            else
                            {
                                xmlWriter.WriteAttributeString("commMethodPair_ref", topoEdge_VM.TopoEdge.CommMethodPair.Id.ToString());
                            }
                            xmlWriter.WriteAttributeString("source_ref", topoEdge_VM.Source.Id.ToString()); // 源锚点
                            xmlWriter.WriteAttributeString("dest_ref", topoEdge_VM.Dest.Id.ToString()); // 目标锚点
                            xmlWriter.WriteEndElement();
                        }
                    }
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
                #endregion

                #region CTL面板

                xmlWriter.WriteStartElement("CTLTree_P_VMs");
                ObservableCollection<SidePanel_VM> ctlTree_P_VMs = protocolVM.PanelVMs[4].SidePanelVMs; // 所有的CTL语法树
                foreach (SidePanel_VM sidePanel_VM in ctlTree_P_VMs)
                {
                    CTLTree_P_VM ctlTree_P_VM = (CTLTree_P_VM)sidePanel_VM;
                    xmlWriter.WriteStartElement("CTLTree_P_VM");
                    xmlWriter.WriteAttributeString("name", ctlTree_P_VM.RefName.Content);
                    foreach (ViewModelBase vm in ctlTree_P_VM.UserControlVMs)
                    {
                        if (vm is AtomProposition_VM) // 原子命题结点
                        {
                            AtomProposition_VM atomProposition_VM = (AtomProposition_VM)vm;
                            xmlWriter.WriteStartElement("AtomProposition_VM");
                            xmlWriter.WriteAttributeString("x", atomProposition_VM.X.ToString());
                            xmlWriter.WriteAttributeString("y", atomProposition_VM.Y.ToString());
                            xmlWriter.WriteAttributeString("name", atomProposition_VM.AtomProposition.RefName.Content);
                            foreach (Connector_VM connector_VM in atomProposition_VM.ConnectorVMs) // 身上所有锚点的id号
                            {
                                xmlWriter.WriteStartElement("Connector_VM");
                                xmlWriter.WriteAttributeString("id", connector_VM.Id.ToString());
                                xmlWriter.WriteEndElement();
                            }
                            xmlWriter.WriteEndElement();
                        }
                        else if (vm is CTLRelation_VM) // CTL关系结点
                        {
                            CTLRelation_VM ctlRelation_VM = (CTLRelation_VM)vm;
                            xmlWriter.WriteStartElement("CTLRelation_VM");
                            xmlWriter.WriteAttributeString("x", ctlRelation_VM.X.ToString());
                            xmlWriter.WriteAttributeString("y", ctlRelation_VM.Y.ToString());
                            xmlWriter.WriteAttributeString("ctlRelation", ctlRelation_VM.CTLRelation.ToString());
                            foreach (Connector_VM connector_VM in ctlRelation_VM.ConnectorVMs) // 身上所有锚点的id号
                            {
                                xmlWriter.WriteStartElement("Connector_VM");
                                xmlWriter.WriteAttributeString("id", connector_VM.Id.ToString());
                                xmlWriter.WriteEndElement();
                            }
                            xmlWriter.WriteEndElement();
                        }
                        else if (vm is LogicRelation_VM) // 命题逻辑关系结点
                        {
                            LogicRelation_VM logicRelation_VM = (LogicRelation_VM)vm;
                            xmlWriter.WriteStartElement("LogicRelation_VM");
                            xmlWriter.WriteAttributeString("x", logicRelation_VM.X.ToString());
                            xmlWriter.WriteAttributeString("y", logicRelation_VM.Y.ToString());
                            xmlWriter.WriteAttributeString("logicRelation", logicRelation_VM.LogicRelation.ToString());
                            foreach (Connector_VM connector_VM in logicRelation_VM.ConnectorVMs) // 身上所有锚点的id号
                            {
                                xmlWriter.WriteStartElement("Connector_VM");
                                xmlWriter.WriteAttributeString("id", connector_VM.Id.ToString());
                                xmlWriter.WriteEndElement();
                            }
                            xmlWriter.WriteEndElement();
                        }
                        else if (vm is Connection_VM) // 连接线
                        {
                            Connection_VM connection_VM = (Connection_VM)vm;
                            xmlWriter.WriteStartElement("Connection_VM");
                            xmlWriter.WriteAttributeString("source_ref", connection_VM.Source.Id.ToString()); // 源锚点
                            xmlWriter.WriteAttributeString("dest_ref", connection_VM.Dest.Id.ToString()); // 目标锚点
                            xmlWriter.WriteEndElement();
                        }
                    }
                }

                #endregion

                // 协议尾
                xmlWriter.WriteEndElement();
                xmlWriter.Flush();
                xmlWriter.Close();
            }
            return true;
        }

        // 执行载入项目，传入".sbid"文件名，返回是否载入成功
        private bool DoReload(string fileName)
        {
            // 项目文件去除后缀名".sbid"部分
            string cleanName = fileName.Substring(0, fileName.Length - 5);

            // 重置项目中的各项元数据，如id计数器等
            cleanProject();

            // 读取".sbid"文件，以创建相应的协议面板
            XmlDocument doc = new XmlDocument();
            XmlReader reader = XmlReader.Create(fileName);
            doc.Load(reader);
            XmlNode projectNode = doc.SelectSingleNode("Project");
            XmlNodeList protocolList = projectNode.ChildNodes;
            foreach (XmlNode protocolNode in protocolList)
            {
                Protocol_VM protocol_VM = new Protocol_VM();
                protocol_VM.Protocol.Name = ((XmlElement)protocolNode).GetAttribute("name");
                protocolVMs.Add(protocol_VM);
            }
            reader.Close();

            // 读取各个协议的".xml"文件
            for (int i = 0; i < protocolVMs.Count; i++)
            {
                // 当前协议
                Protocol_VM protocolVM = protocolVMs[i];
                // 每个协议文件名是"项目名_i.xml"
                reader = XmlReader.Create(cleanName + "_" + i.ToString() + ".xml");
                doc.Load(reader);

                #region 概览>类图面板（第一遍扫描）
                /*
                 因为可能出现先创建的类图引用了后创建的类图的情况
                 所以这里扫描两遍类图
                 第一遍扫描把所有的类图VM创建出来(里面的M自动跟着创建)，注意还要把id盖掉
                 第二遍扫描再去管里面的内容(根据xxx_ref所指定的id找引用了谁)
                 */
                Dictionary<int, Type> typeDict = new Dictionary<int, Type>(); // id->类型
                Dictionary<int, Process_VM> processVMDict = new Dictionary<int, Process_VM>(); // id->进程模板VM(VM里才能找到状态机)
                Dictionary<int, CommMethodPair> commMethodPairDict = new Dictionary<int, CommMethodPair>(); // id->通信方法序对
                ClassDiagram_P_VM classDiagram_P_VM = (ClassDiagram_P_VM)protocolVM.PanelVMs[0].SidePanelVMs[0];
                XmlNode xmlNode = doc.SelectSingleNode("Protocol_VM/ClassDiagram_P_VM");
                XmlNodeList nodeList = xmlNode.ChildNodes;
                foreach (XmlNode node in nodeList)
                {
                    XmlElement element = (XmlElement)node;
                    ViewModelBase userControl_VM = null;
                    int id = int.Parse(element.GetAttribute("id"));
                    switch (node.Name)
                    {
                        case "UserType_VM":
                            // true:基本类型, false:用户自定义复合类型, middle:内置的复合类型
                            switch (element.GetAttribute("basic"))
                            {
                                case "true": // 基本类型
                                    switch (element.GetAttribute("name"))
                                    {
                                        case "int":
                                            Type.TYPE_INT.Id = id;
                                            typeDict[id] = Type.TYPE_INT; // 写入字典
                                            userControl_VM = new UserType_VM(Type.TYPE_INT);
                                            break;
                                        case "bool":
                                            Type.TYPE_BOOL.Id = id;
                                            typeDict[id] = Type.TYPE_BOOL; // 写入字典
                                            userControl_VM = new UserType_VM(Type.TYPE_BOOL);
                                            break;
                                        case "number":
                                            Type.TYPE_NUM.Id = id;
                                            typeDict[id] = Type.TYPE_NUM; // 写入字典
                                            userControl_VM = new UserType_VM(Type.TYPE_NUM);
                                            break;
                                        case "byte":
                                            Type.TYPE_BYTE.Id = id;
                                            typeDict[id] = Type.TYPE_BYTE; // 写入字典
                                            userControl_VM = new UserType_VM(Type.TYPE_BYTE);
                                            break;
                                    }
                                    break;
                                case "middle": // 内置的复合类型
                                    switch (element.GetAttribute("name"))
                                    {
                                        case "ByteVec":
                                            userControl_VM = new UserType_VM(Type.TYPE_BYTE_VEC);
                                            ((UserType_VM)userControl_VM).Type.Id = id;
                                            typeDict[id] = ((UserType_VM)userControl_VM).Type; // 写入字典
                                            break;
                                        case "Timer":
                                            userControl_VM = new UserType_VM(Type.TYPE_TIMER);
                                            ((UserType_VM)userControl_VM).Type.Id = id;
                                            typeDict[id] = ((UserType_VM)userControl_VM).Type; // 写入字典
                                            break;
                                    }
                                    break;
                                case "false": // 用户自定义复合类型
                                    userControl_VM = new UserType_VM();
                                    ((UserType_VM)userControl_VM).Type.Id = id;
                                    typeDict[id] = ((UserType_VM)userControl_VM).Type; // 写入字典
                                    ((UserType_VM)userControl_VM).Type.Name = element.GetAttribute("name");
                                    // Parent引用放到第二次扫描中取出，因为在这里引用的UserType未必已经创建好了
                                    break;
                            }
                            break;
                        case "Process_VM":
                            userControl_VM = new Process_VM();
                            Process_VM process_VM = (Process_VM)userControl_VM;
                            process_VM.Process.Id = id;
                            processVMDict[id] = process_VM; // 写入字典
                            process_VM.Process.RefName.Content = element.GetAttribute("name");
                            // 创建对应的"进程-状态机"大面板，加到当前协议的状态机选项卡下面
                            ProcessToSM_P_VM pvm = new ProcessToSM_P_VM(process_VM.Process);
                            protocolVM.PanelVMs[1].SidePanelVMs.Add(pvm);
                            protocolVM.PanelVMs[1].SelectedItem = pvm; // fixme 这里改成记录用户保存的选择
                            process_VM.ProcessToSM_P_VM = pvm; // 从Process的反引
                            break;
                        case "Axiom_VM":
                            userControl_VM = new Axiom_VM();
                            ((Axiom_VM)userControl_VM).Axiom.Id = id;
                            ((Axiom_VM)userControl_VM).Axiom.Name = element.GetAttribute("name");
                            break;
                        case "InitialKnowledge_VM":
                            userControl_VM = new InitialKnowledge_VM();
                            ((InitialKnowledge_VM)userControl_VM).InitialKnowledge.Id = id;
                            break;
                        case "SafetyProperty_VM":
                            userControl_VM = new SafetyProperty_VM();
                            ((SafetyProperty_VM)userControl_VM).SafetyProperty.Id = id;
                            ((SafetyProperty_VM)userControl_VM).SafetyProperty.Name = element.GetAttribute("name");
                            break;
                        case "SecurityProperty_VM":
                            userControl_VM = new SecurityProperty_VM();
                            ((SecurityProperty_VM)userControl_VM).SecurityProperty.Id = id;
                            ((SecurityProperty_VM)userControl_VM).SecurityProperty.Name = element.GetAttribute("name");
                            break;
                        case "CommChannel_VM":
                            userControl_VM = new CommChannel_VM();
                            ((CommChannel_VM)userControl_VM).CommChannel.Id = id;
                            ((CommChannel_VM)userControl_VM).CommChannel.Name = element.GetAttribute("name");
                            break;
                        default:
                            Tips = "[解析ClassDiagram_P_VM时出错]未知的子标签！";
                            cleanProject();
                            return false;
                    }
                    // 创建的图形而不是连线
                    if (userControl_VM is NetworkItem_VM)
                    {
                        ((NetworkItem_VM)userControl_VM).X = double.Parse(element.GetAttribute("x"));
                        ((NetworkItem_VM)userControl_VM).Y = double.Parse(element.GetAttribute("y"));
                        classDiagram_P_VM.UserControlVMs.Add(userControl_VM);
                    }
                    // 创建的是连线
                    else if (userControl_VM != null)
                    {
                        classDiagram_P_VM.UserControlVMs.Add(userControl_VM);
                    }
                }
                #endregion

                #region "进程模板-状态机"面板
                ObservableCollection<SidePanel_VM> processToSM_P_VMs = protocolVM.PanelVMs[1].SidePanelVMs; // 这里是创建进程模板时创建的
                xmlNode = doc.SelectSingleNode("Protocol_VM/ProcessToSM_P_VMs");
                nodeList = xmlNode.ChildNodes;
                if (processToSM_P_VMs.Count != nodeList.Count)
                {
                    Tips = "[解析ProcessToSM_P_VMs时出错]子结点数和进程模板数不同！";
                    cleanProject();
                    return false;
                }
                for (int j = 0; j < nodeList.Count; j++) // <ProcessToSM_P_VM process_ref="xxx">
                {
                    XmlNode node = nodeList[j];
                    XmlElement element = (XmlElement)node;
                    ProcessToSM_P_VM processToSM_P_VM = (ProcessToSM_P_VM)processToSM_P_VMs[j];
                    if (element.GetAttribute("process_ref") != processToSM_P_VM.Process.Id.ToString())
                    {
                        Tips = "[解析StateMachine_P_VM时出错]process_ref和进程模板不能按序对应！";
                        cleanProject();
                        return false;
                    }
                    // 遍历里面的每个状态机面板
                    Dictionary<int, State> stateDict = new Dictionary<int, State>(); // 边解析边记录状态
                    stateDict.Add(1, State.TopState); // 顶层状态唯一实例
                    foreach (XmlNode smNode in node.ChildNodes) // <StateMachine_P_VM state_ref="xxx">
                    {
                        XmlElement smElement = (XmlElement)smNode;
                        int state_ref = int.Parse(smElement.GetAttribute("state_ref"));
                        if (!stateDict.ContainsKey(state_ref))
                        {
                            Tips = "[解析StateMachine_P_VM时出错]无法找到精化的状态！";
                            cleanProject();
                            return false;
                        }
                        StateMachine_P_VM stateMachine_P_VM = new StateMachine_P_VM(stateDict[state_ref]);
                        // 解析具体的状态机面板
                        Dictionary<int, Connector_VM> connectorDict = new Dictionary<int, Connector_VM>(); // 记录id->锚点的字典,用于连线
                        foreach (XmlNode satNode in smNode.ChildNodes) // State_VM/InitState_VM/FinalState_VM/...
                        {
                            XmlElement satElement = (XmlElement)satNode;
                            switch (satNode.Name)
                            {
                                case "State_VM":
                                    double x = double.Parse(satElement.GetAttribute("x"));
                                    double y = double.Parse(satElement.GetAttribute("y"));
                                    State_VM state_VM = new State_VM(x, y);
                                    state_VM.State.Name = satElement.GetAttribute("name");
                                    state_VM.State.Id = int.Parse(satElement.GetAttribute("id"));
                                    XmlNodeList connectorList = satNode.ChildNodes;
                                    if (state_VM.ConnectorVMs.Count != connectorList.Count)
                                    {
                                        Tips = "[解析State_VM时出错]锚点数量和系统要求不一致！";
                                        cleanProject();
                                        return false;
                                    }
                                    for (int k = 0; k < connectorList.Count; k++) // <Connector_VM id="xxx" />
                                    {
                                        XmlNode connectorNode = connectorList[k];
                                        XmlElement connectorElement = (XmlElement)connectorNode;
                                        int id = int.Parse(connectorElement.GetAttribute("id"));
                                        state_VM.ConnectorVMs[k].Id = id;
                                        connectorDict.Add(id, state_VM.ConnectorVMs[k]); // 锚点记录到字典
                                    }
                                    stateMachine_P_VM.UserControlVMs.Add(state_VM);
                                    stateDict.Add(state_VM.State.Id, state_VM.State); // 状态记录到字典
                                    break;
                                case "InitState_VM":
                                    x = double.Parse(satElement.GetAttribute("x"));
                                    y = double.Parse(satElement.GetAttribute("y"));
                                    InitState_VM initState_VM = new InitState_VM(x, y);
                                    connectorList = satNode.ChildNodes;
                                    if (initState_VM.ConnectorVMs.Count != connectorList.Count)
                                    {
                                        Tips = "[解析InitState_VM时出错]锚点数量和系统要求不一致！";
                                        cleanProject();
                                        return false;
                                    }
                                    for (int k = 0; k < connectorList.Count; k++) // <Connector_VM id="xxx" />
                                    {
                                        XmlNode connectorNode = connectorList[k];
                                        XmlElement connectorElement = (XmlElement)connectorNode;
                                        int id = int.Parse(connectorElement.GetAttribute("id"));
                                        initState_VM.ConnectorVMs[k].Id = id;
                                        connectorDict.Add(id, initState_VM.ConnectorVMs[k]); // 记录到字典里
                                    }
                                    stateMachine_P_VM.UserControlVMs.Add(initState_VM);
                                    break;
                                case "FinalState_VM":
                                    x = double.Parse(satElement.GetAttribute("x"));
                                    y = double.Parse(satElement.GetAttribute("y"));
                                    FinalState_VM finalState_VM = new FinalState_VM(x, y);
                                    connectorList = satNode.ChildNodes;
                                    if (finalState_VM.ConnectorVMs.Count != connectorList.Count)
                                    {
                                        Tips = "[解析FinalState_VM时出错]锚点数量和系统要求不一致！";
                                        cleanProject();
                                        return false;
                                    }
                                    for (int k = 0; k < connectorList.Count; k++) // <Connector_VM id="xxx" />
                                    {
                                        XmlNode connectorNode = connectorList[k];
                                        XmlElement connectorElement = (XmlElement)connectorNode;
                                        int id = int.Parse(connectorElement.GetAttribute("id"));
                                        finalState_VM.ConnectorVMs[k].Id = id;
                                        connectorDict.Add(id, finalState_VM.ConnectorVMs[k]); // 记录到字典里
                                    }
                                    stateMachine_P_VM.UserControlVMs.Add(finalState_VM);
                                    break;
                                /*
                                case "Transition_VM":
                                    Transition_VM transition_VM = new Transition_VM();
                                    int sourceRef = int.Parse(satElement.GetAttribute("source_ref"));
                                    int destRef = int.Parse(satElement.GetAttribute("dest_ref"));
                                    if (!(connectorDict.ContainsKey(sourceRef) && connectorDict.ContainsKey(destRef)))
                                    {
                                        Tips = "[解析Transition_VM时出错]无法找到某端的锚点！";
                                        cleanProject();
                                        return false;
                                    }
                                    transition_VM.Source = connectorDict[sourceRef]; // 连线两端引用锚点
                                    transition_VM.Dest = connectorDict[destRef];
                                    connectorDict[sourceRef].ConnectionVM = transition_VM; // 从锚点反引连线
                                    connectorDict[destRef].ConnectionVM = transition_VM;
                                    transition_VM.Transition.Guard = satElement.GetAttribute("guard"); // Gurad条件
                                    XmlNodeList actionList = satNode.ChildNodes;
                                    foreach (XmlNode actionNode in actionList) // <Action content="xxx" />
                                    {
                                        XmlElement actionElement = (XmlElement)actionNode;
                                        transition_VM.Transition.Actions.Add(new Formula(actionElement.GetAttribute("content")));
                                    }
                                    stateMachine_P_VM.UserControlVMs.Add(transition_VM);
                                    break;
                                */
                                case "StateTrans_VM":
                                    x = double.Parse(satElement.GetAttribute("x"));
                                    y = double.Parse(satElement.GetAttribute("y"));
                                    StateTrans_VM stateTrans_VM = new StateTrans_VM(x, y);
                                    int connector_index = 0; // 当前搜索到第几个锚点，从0计数
                                    foreach (XmlNode childNode in satNode.ChildNodes)
                                    {
                                        XmlElement childElement = (XmlElement)childNode;
                                        switch (childNode.Name)
                                        {
                                            case "Guard":
                                                Formula guard = new Formula(childElement.GetAttribute("content"));
                                                stateTrans_VM.StateTrans.Guards.Add(guard);
                                                break;
                                            case "Action":
                                                Formula action = new Formula(childElement.GetAttribute("content"));
                                                stateTrans_VM.StateTrans.Actions.Add(action);
                                                break;
                                            case "Connector_VM":
                                                if (connector_index >= stateTrans_VM.ConnectorVMs.Count)
                                                {
                                                    Tips = "[解析FinalState_VM时出错]锚点数量和系统要求不一致！";
                                                    cleanProject();
                                                    return false;
                                                }
                                                int id = int.Parse(childElement.GetAttribute("id"));
                                                stateTrans_VM.ConnectorVMs[connector_index].Id = id;
                                                connectorDict.Add(id, stateTrans_VM.ConnectorVMs[connector_index]); // 记录到字典里
                                                connector_index++;
                                                break;
                                        }
                                    }
                                    stateMachine_P_VM.UserControlVMs.Add(stateTrans_VM);
                                    break;
                                case "ControlPoint_VM":
                                    x = double.Parse(satElement.GetAttribute("x"));
                                    y = double.Parse(satElement.GetAttribute("y"));
                                    ControlPoint_VM controlPoint_VM = new ControlPoint_VM(x, y);
                                    connectorList = satNode.ChildNodes;
                                    if (controlPoint_VM.ConnectorVMs.Count != connectorList.Count)
                                    {
                                        Tips = "[解析ControlPoint_VM时出错]锚点数量和系统要求不一致！";
                                        cleanProject();
                                        return false;
                                    }
                                    for (int k = 0; k < connectorList.Count; k++) // <Connector_VM id="xxx" />
                                    {
                                        XmlNode connectorNode = connectorList[k];
                                        XmlElement connectorElement = (XmlElement)connectorNode;
                                        int id = int.Parse(connectorElement.GetAttribute("id"));
                                        controlPoint_VM.ConnectorVMs[k].Id = id;
                                        connectorDict.Add(id, controlPoint_VM.ConnectorVMs[k]); // 记录到字典里
                                    }
                                    stateMachine_P_VM.UserControlVMs.Add(controlPoint_VM);
                                    break;
                                case "Connection_VM":
                                    Connection_VM connection_VM = new Connection_VM();
                                    int sourceRef = int.Parse(satElement.GetAttribute("source_ref"));
                                    int destRef = int.Parse(satElement.GetAttribute("dest_ref"));
                                    if (!(connectorDict.ContainsKey(sourceRef) && connectorDict.ContainsKey(destRef)))
                                    {
                                        Tips = "[解析Connection_VM时出错]无法找到某端的锚点！";
                                        cleanProject();
                                        return false;
                                    }
                                    connection_VM.Source = connectorDict[sourceRef]; // 连线两端引用锚点
                                    connection_VM.Dest = connectorDict[destRef];
                                    connectorDict[sourceRef].ConnectionVM = connection_VM; // 从锚点反引连线
                                    connectorDict[destRef].ConnectionVM = connection_VM;
                                    stateMachine_P_VM.UserControlVMs.Add(connection_VM);
                                    break;
                                case "Arrow_VM":
                                    Arrow_VM arrow_VM = new Arrow_VM();
                                    sourceRef = int.Parse(satElement.GetAttribute("source_ref"));
                                    destRef = int.Parse(satElement.GetAttribute("dest_ref"));
                                    if (!(connectorDict.ContainsKey(sourceRef) && connectorDict.ContainsKey(destRef)))
                                    {
                                        Tips = "[解析Arrow_VM时出错]无法找到某端的锚点！";
                                        cleanProject();
                                        return false;
                                    }
                                    arrow_VM.Source = connectorDict[sourceRef]; // 连线两端引用锚点
                                    arrow_VM.Dest = connectorDict[destRef];
                                    connectorDict[sourceRef].ConnectionVM = arrow_VM; // 从锚点反引连线
                                    connectorDict[destRef].ConnectionVM = arrow_VM;
                                    stateMachine_P_VM.UserControlVMs.Add(arrow_VM);
                                    break;
                            }
                        }
                        // 放入ProcessToSM_P_VM大面板里
                        processToSM_P_VM.StateMachinePVMs.Add(stateMachine_P_VM);
                    }
                }

                #endregion

                #region 概览>类图面板（第二遍扫描）
                /*
                 第二遍扫描不包括引用了Process的Attribute或State或CommMethod的类图
                 因为Process_VM可能创建在他们的后面，然后再向他们中间引用后面的Process_VM
                 所以这几个东西交给第三遍扫描去做
                */
                xmlNode = doc.SelectSingleNode("Protocol_VM/ClassDiagram_P_VM");
                nodeList = xmlNode.ChildNodes;
                for (int j = 0; j < nodeList.Count; j++)
                {
                    XmlNode node = nodeList[j];
                    XmlElement element = (XmlElement)node;
                    // 因为第一次扫描和这次顺序一样，所以这里直接取
                    ViewModelBase userControl_VM = classDiagram_P_VM.UserControlVMs[j];
                    switch (node.Name)
                    {
                        case "UserType_VM":
                            if (element.GetAttribute("basic") == "false") // true和middle都不用再处理了
                            {
                                UserType_VM userType_VM = (UserType_VM)userControl_VM;
                                UserType userType = (UserType)userType_VM.Type;
                                int parentRef = int.Parse(element.GetAttribute("parent_ref"));
                                if (parentRef != -1 && (!typeDict.ContainsKey(parentRef) || !(typeDict[parentRef] is UserType)))
                                {
                                    Tips = "[解析UserType_VM时出错]无法认定Parent的类型，必须是存在的UserType！";
                                    cleanProject();
                                    return false;
                                }
                                if (parentRef != -1) // 仅当有继承关系时写入
                                    userType.Parent = (UserType)typeDict[parentRef];
                                // id和name在第一轮就处理过了，这里只要处理Attribute和Method
                                foreach (XmlNode attrNode in node.ChildNodes) // Attribute/Method
                                {
                                    XmlElement attrElement = (XmlElement)attrNode;
                                    switch (attrNode.Name)
                                    {
                                        case "Attribute":
                                            int typeRef = int.Parse(attrElement.GetAttribute("type_ref"));
                                            int id = int.Parse(attrElement.GetAttribute("id"));
                                            string identifier = attrElement.GetAttribute("identifier");
                                            bool isArray = bool.Parse(attrElement.GetAttribute("isArray"));
                                            if (!typeDict.ContainsKey(typeRef))
                                            {
                                                Tips = "[解析UserType_VM时出错]无法找到Attribute的类型！";
                                                cleanProject();
                                                return false;
                                            }
                                            Attribute attribute = new Attribute(typeDict[typeRef], identifier, isArray);
                                            attribute.Id = id;
                                            userType.Attributes.Add(attribute);
                                            break;
                                        case "Method":
                                            id = int.Parse(attrElement.GetAttribute("id"));
                                            int returnTypeRef = int.Parse(attrElement.GetAttribute("returnType_ref"));
                                            if (!typeDict.ContainsKey(returnTypeRef))
                                            {
                                                Tips = "[解析UserType_VM时出错]无法找到Method的返回类型！";
                                                cleanProject();
                                                return false;
                                            }
                                            string name = attrElement.GetAttribute("name");
                                            Crypto cryptoSuffix = (Crypto)System.Enum.Parse(typeof(Crypto), attrElement.GetAttribute("cryptoSuffix"));
                                            ObservableCollection<Attribute> parameters = new ObservableCollection<Attribute>();
                                            foreach (XmlNode paramNode in attrNode.ChildNodes) // <Parameter type_ref="1" identifier="key" isArray="False" id="10" />
                                            {
                                                XmlElement paramElement = (XmlElement)paramNode;
                                                typeRef = int.Parse(paramElement.GetAttribute("type_ref"));
                                                int paramId = int.Parse(paramElement.GetAttribute("id"));
                                                identifier = paramElement.GetAttribute("identifier");
                                                isArray = bool.Parse(paramElement.GetAttribute("isArray"));
                                                if (!typeDict.ContainsKey(typeRef))
                                                {
                                                    Tips = "[解析UserType_VM时出错]无法找到Method的参数类型！";
                                                    cleanProject();
                                                    return false;
                                                }
                                                Attribute param = new Attribute(typeDict[typeRef], identifier, isArray);
                                                param.Id = paramId;
                                                parameters.Add(param);
                                            }
                                            Method method = new Method(typeDict[returnTypeRef], name, parameters, cryptoSuffix);
                                            method.Id = id;
                                            userType.Methods.Add(method);
                                            break;
                                    }
                                }
                            }
                            break;
                        case "Process_VM":
                            Process_VM process_VM = (Process_VM)userControl_VM;
                            Process process = process_VM.Process;
                            foreach (XmlNode processChildNode in node.ChildNodes) // Attribute/Method/CommMethod
                            {
                                XmlElement pcElement = (XmlElement)processChildNode;
                                switch (processChildNode.Name)
                                {
                                    case "Attribute":
                                        int typeRef = int.Parse(pcElement.GetAttribute("type_ref"));
                                        int id = int.Parse(pcElement.GetAttribute("id"));
                                        string identifier = pcElement.GetAttribute("identifier");
                                        bool isArray = bool.Parse(pcElement.GetAttribute("isArray"));
                                        if (!typeDict.ContainsKey(typeRef))
                                        {
                                            Tips = "[解析Process_VM时出错]无法找到Attribute的类型！";
                                            cleanProject();
                                            return false;
                                        }
                                        Attribute attribute = new Attribute(typeDict[typeRef], identifier, isArray);
                                        attribute.Id = id;
                                        process.Attributes.Add(attribute);
                                        break;
                                    case "Method":
                                        id = int.Parse(pcElement.GetAttribute("id"));
                                        int returnTypeRef = int.Parse(pcElement.GetAttribute("returnType_ref"));
                                        if (!typeDict.ContainsKey(returnTypeRef))
                                        {
                                            Tips = "[解析Process_VM时出错]无法找到Method的返回类型！";
                                            cleanProject();
                                            return false;
                                        }
                                        string name = pcElement.GetAttribute("name");
                                        Crypto cryptoSuffix = (Crypto)System.Enum.Parse(typeof(Crypto), pcElement.GetAttribute("cryptoSuffix"));
                                        ObservableCollection<Attribute> parameters = new ObservableCollection<Attribute>();
                                        foreach (XmlNode paramNode in processChildNode.ChildNodes) // <Parameter type_ref="1" identifier="key" id="10" />
                                        {
                                            XmlElement paramElement = (XmlElement)paramNode;
                                            typeRef = int.Parse(paramElement.GetAttribute("type_ref"));
                                            int paramId = int.Parse(paramElement.GetAttribute("id"));
                                            identifier = paramElement.GetAttribute("identifier");
                                            isArray = bool.Parse(paramElement.GetAttribute("isArray"));
                                            if (!typeDict.ContainsKey(typeRef))
                                            {
                                                Tips = "[解析Process_VM时出错]无法找到Method的参数类型！";
                                                cleanProject();
                                                return false;
                                            }
                                            Attribute param = new Attribute(typeDict[typeRef], identifier, isArray);
                                            param.Id = paramId;
                                            parameters.Add(param);
                                        }
                                        Method method = new Method(typeDict[returnTypeRef], name, parameters, cryptoSuffix);
                                        method.Id = id;
                                        process.Methods.Add(method);
                                        break;
                                    case "CommMethod":
                                        name = pcElement.GetAttribute("name");
                                        InOut inOutSuffix = (InOut)System.Enum.Parse(typeof(InOut), pcElement.GetAttribute("inOutSuffix"));
                                        CommWay commWay = (CommWay)System.Enum.Parse(typeof(CommWay), pcElement.GetAttribute("commWay"));
                                        parameters = new ObservableCollection<Attribute>();
                                        foreach (XmlNode paramNode in processChildNode.ChildNodes) // <Parameter type_ref="1" identifier="key" id="10" />
                                        {
                                            XmlElement paramElement = (XmlElement)paramNode;
                                            typeRef = int.Parse(paramElement.GetAttribute("type_ref"));
                                            int paramId = int.Parse(paramElement.GetAttribute("id"));
                                            identifier = paramElement.GetAttribute("identifier");
                                            isArray = bool.Parse(paramElement.GetAttribute("isArray"));
                                            if (!typeDict.ContainsKey(typeRef))
                                            {
                                                Tips = "[解析Process_VM时出错]无法找到CommMethod的参数类型！";
                                                cleanProject();
                                                return false;
                                            }
                                            Attribute param = new Attribute(typeDict[typeRef], identifier, isArray);
                                            param.Id = paramId;
                                            parameters.Add(param);
                                        }
                                        CommMethod commMethod = new CommMethod(name, parameters, inOutSuffix, commWay);
                                        process.CommMethods.Add(commMethod);
                                        break;
                                }
                            }
                            break;
                            /*
                            case "SafetyProperty_VM":
                                SafetyProperty_VM safetyProperty_VM = (SafetyProperty_VM)userControl_VM;
                                SafetyProperty safetyProperty = safetyProperty_VM.SafetyProperty;
                                foreach (XmlNode safetyChildNode in node.ChildNodes) // <CTL content="" /> 或 <Invariant content = "" />
                                {
                                    XmlElement safetyElement = (XmlElement)safetyChildNode;
                                    switch (safetyChildNode.Name)
                                    {
                                        case "CTL":
                                            string ctl = safetyElement.GetAttribute("content");
                                            safetyProperty.CTLs.Add(new Formula(ctl));
                                            break;
                                        case "Invariant":
                                            string invariant = safetyElement.GetAttribute("content");
                                            safetyProperty.Invariants.Add(new Formula(invariant));
                                            break;
                                    }
                                }
                                break;
                            */
                    }
                }
                #endregion

                #region 概览>类图面板（第三遍扫描）
                /*
                 处理InitialKnowledge_VM和SecurityProperty_VM
                */
                for (int j = 0; j < nodeList.Count; j++)
                {
                    XmlNode node = nodeList[j];
                    XmlElement element = (XmlElement)node;
                    // 因为第一次扫描和这次顺序一样，所以这里直接取
                    ViewModelBase userControl_VM = classDiagram_P_VM.UserControlVMs[j];
                    switch (node.Name)
                    {
                        case "InitialKnowledge_VM":
                            int processRef = int.Parse(element.GetAttribute("process_ref"));
                            if (processRef != -1 && !processVMDict.ContainsKey(processRef)) // -1表示全局，不引用
                            {
                                Tips = "[解析InitialKnowledge_VM时出错]无法找到引用的进程模板！";
                                cleanProject();
                                return false;
                            }
                            InitialKnowledge_VM initialKnowledge_VM = (InitialKnowledge_VM)userControl_VM;
                            InitialKnowledge initialKnowledge = initialKnowledge_VM.InitialKnowledge;
                            if (processRef != -1)
                            {
                                initialKnowledge.Process = processVMDict[processRef].Process;
                            }
                            foreach (XmlNode ikpChildNode in node.ChildNodes) // Knowledge 或 KeyPair
                            {
                                XmlElement ikpElement = (XmlElement)ikpChildNode;
                                switch (ikpElement.Name)
                                {
                                    case "Knowledge":
                                        processRef = int.Parse(ikpElement.GetAttribute("process_ref"));
                                        if (!processVMDict.ContainsKey(processRef))
                                        {
                                            Tips = "[解析InitialKnowledge_VM时出错]无法找到Knowledge引用的进程模板！";
                                            cleanProject();
                                            return false;
                                        }
                                        int attributeRef = int.Parse(ikpElement.GetAttribute("attribute_ref"));
                                        Attribute findAttr = null;
                                        foreach (Attribute attribute in processVMDict[processRef].Process.Attributes)
                                        {
                                            if (attribute.Id == attributeRef)
                                            {
                                                findAttr = attribute;
                                                break;
                                            }
                                        }
                                        if (findAttr == null)
                                        {
                                            Tips = "[解析InitialKnowledge_VM时出错]无法找到Knowledge引用的进程模板下的Attribute！";
                                            cleanProject();
                                            return false;
                                        }
                                        Knowledge knowledge = new Knowledge(processVMDict[processRef].Process, findAttr);
                                        initialKnowledge.Knowledges.Add(knowledge);
                                        break;
                                    case "KeyPair":
                                        int pubProcess_ref = int.Parse(ikpElement.GetAttribute("pubProcess_ref"));
                                        int secProcess_ref = int.Parse(ikpElement.GetAttribute("secProcess_ref"));
                                        if (!processVMDict.ContainsKey(pubProcess_ref) || !processVMDict.ContainsKey(secProcess_ref))
                                        {
                                            Tips = "[解析InitialKnowledge_VM时出错]无法找到KeyPair引用的进程模板！";
                                            cleanProject();
                                            return false;
                                        }
                                        int pubKey_ref = int.Parse(ikpElement.GetAttribute("pubKey_ref"));
                                        int secKey_ref = int.Parse(ikpElement.GetAttribute("secKey_ref"));
                                        Attribute pubKey = null, secKey = null;
                                        foreach (Attribute attribute in processVMDict[pubProcess_ref].Process.Attributes)
                                        {
                                            if (attribute.Id == pubKey_ref)
                                            {
                                                pubKey = attribute;
                                                break;
                                            }
                                        }
                                        foreach (Attribute attribute in processVMDict[secProcess_ref].Process.Attributes)
                                        {
                                            if (attribute.Id == secKey_ref)
                                            {
                                                secKey = attribute;
                                                break;
                                            }
                                        }
                                        if (pubKey == null || secKey == null)
                                        {
                                            Tips = "[解析InitialKnowledge_VM时出错]无法找到KeyPair引用的进程模板下的Attribute！";
                                            cleanProject();
                                            return false;
                                        }
                                        KeyPair keyPair = new KeyPair(
                                            processVMDict[pubProcess_ref].Process,
                                            pubKey,
                                            processVMDict[secProcess_ref].Process,
                                            secKey
                                        );
                                        initialKnowledge.KeyPairs.Add(keyPair);
                                        break;
                                }
                            }
                            break;
                        case "SafetyProperty_VM":
                            SafetyProperty_VM safetyProperty_VM = (SafetyProperty_VM)userControl_VM;
                            SafetyProperty safetyProperty = safetyProperty_VM.SafetyProperty;
                            foreach (XmlNode safetyChildNode in node.ChildNodes) // <CTL process_ref="" state_ref="" formula="" /> 或 <Invariant content = "" />
                            {
                                XmlElement safetyElement = (XmlElement)safetyChildNode;
                                switch (safetyChildNode.Name)
                                {
                                    case "CTL":
                                        processRef = int.Parse(safetyElement.GetAttribute("process_ref"));
                                        if (!processVMDict.ContainsKey(processRef))
                                        {
                                            Tips = "[解析SafetyProperty_VM时出错]无法找到CTL引用的进程模板！";
                                            cleanProject();
                                            return false;
                                        }
                                        int stateRef = int.Parse(safetyElement.GetAttribute("state_ref"));
                                        State state = null;
                                        foreach (ViewModelBase vmb in processVMDict[processRef].ProcessToSM_P_VM.StateMachinePVMs[0].UserControlVMs)
                                        {
                                            if (vmb is State_VM)
                                            {
                                                State_VM state_VM = (State_VM)vmb;
                                                if (state_VM.State.Id == stateRef)
                                                {
                                                    state = state_VM.State;
                                                }
                                            }
                                        }
                                        if (state == null)
                                        {
                                            Tips = "[解析SafetyProperty_VM时出错]无法找到CTL引用的状态机下的State！";
                                            cleanProject();
                                            return false;
                                        }
                                        string content = safetyElement.GetAttribute("formula");
                                        safetyProperty.CTLs.Add(new CTL(processVMDict[processRef].Process, state, new Formula(content)));
                                        break;
                                    case "Invariant":
                                        content = safetyElement.GetAttribute("content");
                                        safetyProperty.Invariants.Add(new Formula(content));
                                        break;
                                }
                            }
                            break;
                        case "SecurityProperty_VM":
                            SecurityProperty_VM securityProperty_VM = (SecurityProperty_VM)userControl_VM;
                            SecurityProperty securityProperty = securityProperty_VM.SecurityProperty;
                            foreach (XmlNode securityChildNode in node.ChildNodes) // Confidential(2引) 或 Authenticity(8引) 或 Integrity(6引)
                            {
                                XmlElement securityElement = (XmlElement)securityChildNode;
                                switch (securityChildNode.Name)
                                {
                                    case "Confidential":
                                        processRef = int.Parse(securityElement.GetAttribute("process_ref"));
                                        if (!processVMDict.ContainsKey(processRef))
                                        {
                                            Tips = "[解析SecurityProperty_VM时出错]无法找到Confidential引用的进程模板！";
                                            cleanProject();
                                            return false;
                                        }
                                        int attributeRef = int.Parse(securityElement.GetAttribute("attribute_ref"));
                                        Attribute findAttr = null;
                                        foreach (Attribute attribute in processVMDict[processRef].Process.Attributes)
                                        {
                                            if (attribute.Id == attributeRef)
                                            {
                                                findAttr = attribute;
                                                break;
                                            }
                                        }
                                        if (findAttr == null)
                                        {
                                            Tips = "[解析SecurityProperty_VM时出错]无法找到Confidential引用的进程模板下的Attribute！";
                                            cleanProject();
                                            return false;
                                        }
                                        Confidential confidential = new Confidential(processVMDict[processRef].Process, findAttr);
                                        securityProperty.Confidentials.Add(confidential);
                                        break;
                                    case "Authenticity":
                                        int processA_ref = int.Parse(securityElement.GetAttribute("processA_ref"));
                                        int processB_ref = int.Parse(securityElement.GetAttribute("processB_ref"));
                                        if (!(processVMDict.ContainsKey(processA_ref) && processVMDict.ContainsKey(processB_ref)))
                                        {
                                            Tips = "[解析SecurityProperty_VM时出错]无法找到Authenticity引用的进程模板！";
                                            cleanProject();
                                            return false;
                                        }
                                        int stateA_ref = int.Parse(securityElement.GetAttribute("stateA_ref"));
                                        int stateB_ref = int.Parse(securityElement.GetAttribute("stateB_ref"));
                                        State stateA = null;
                                        State stateB = null;
                                        foreach (ViewModelBase vmb in processVMDict[processA_ref].ProcessToSM_P_VM.StateMachinePVMs[0].UserControlVMs)
                                        {
                                            if (vmb is State_VM)
                                            {
                                                State_VM state_VM = (State_VM)vmb;
                                                if (state_VM.State.Id == stateA_ref)
                                                {
                                                    stateA = state_VM.State;
                                                }
                                            }
                                        }
                                        foreach (ViewModelBase vmb in processVMDict[processB_ref].ProcessToSM_P_VM.StateMachinePVMs[0].UserControlVMs)
                                        {
                                            if (vmb is State_VM)
                                            {
                                                State_VM state_VM = (State_VM)vmb;
                                                if (state_VM.State.Id == stateB_ref)
                                                {
                                                    stateB = state_VM.State;
                                                }
                                            }
                                        }
                                        if (stateA == null || stateB == null)
                                        {
                                            Tips = "[解析SecurityProperty_VM时出错]无法找到Authenticity引用的状态机下的State！";
                                            cleanProject();
                                            return false;
                                        }
                                        int attributeA_ref = int.Parse(securityElement.GetAttribute("attributeA_ref"));
                                        int attributeB_ref = int.Parse(securityElement.GetAttribute("attributeB_ref"));
                                        Attribute attributeA = null;
                                        Attribute attributeB = null;
                                        foreach (Attribute attribute in processVMDict[processA_ref].Process.Attributes)
                                        {
                                            if (attribute.Id == attributeA_ref)
                                            {
                                                attributeA = attribute;
                                                break;
                                            }
                                        }
                                        foreach (Attribute attribute in processVMDict[processB_ref].Process.Attributes)
                                        {
                                            if (attribute.Id == attributeB_ref)
                                            {
                                                attributeB = attribute;
                                                break;
                                            }
                                        }
                                        if (attributeA == null || attributeB == null)
                                        {
                                            Tips = "[解析SecurityProperty_VM时出错]无法找到Authenticity引用的进程模板下的Attribute！";
                                            cleanProject();
                                            return false;
                                        }
                                        if (!(attributeA.Type is UserType) || !(attributeB.Type is UserType))
                                        {
                                            Tips = "[解析SecurityProperty_VM时出错]Authenticity引用的进程模板下的Attribute不是UserType型！";
                                            cleanProject();
                                            return false;
                                        }
                                        int attributeA_Attr_ref = int.Parse(securityElement.GetAttribute("attributeA_Attr_ref"));
                                        int attributeB_Attr_ref = int.Parse(securityElement.GetAttribute("attributeB_Attr_ref"));
                                        Attribute attributeA_Attr = null;
                                        Attribute attributeB_Attr = null;
                                        foreach (Attribute attribute in ((UserType)attributeA.Type).Attributes)
                                        {
                                            if (attribute.Id == attributeA_Attr_ref)
                                            {
                                                attributeA_Attr = attribute;
                                                break;
                                            }
                                        }
                                        foreach (Attribute attribute in ((UserType)attributeB.Type).Attributes)
                                        {
                                            if (attribute.Id == attributeB_Attr_ref)
                                            {
                                                attributeB_Attr = attribute;
                                                break;
                                            }
                                        }
                                        if (attributeA_Attr == null || attributeB_Attr == null)
                                        {
                                            Tips = "[解析SecurityProperty_VM时出错]无法找到Authenticity引用的进程模板下的二级属性！";
                                            cleanProject();
                                            return false;
                                        }
                                        Authenticity authenticity = new Authenticity(
                                            processVMDict[processA_ref].Process,
                                            stateA, attributeA, attributeA_Attr,
                                            processVMDict[processB_ref].Process,
                                            stateB, attributeB, attributeB_Attr);
                                        securityProperty.Authenticities.Add(authenticity);
                                        break;
                                    case "Integrity":
                                        processA_ref = int.Parse(securityElement.GetAttribute("processA_ref"));
                                        processB_ref = int.Parse(securityElement.GetAttribute("processB_ref"));
                                        if (!(processVMDict.ContainsKey(processA_ref) && processVMDict.ContainsKey(processB_ref)))
                                        {
                                            Tips = "[解析SecurityProperty_VM时出错]无法找到Integrity引用的进程模板！";
                                            cleanProject();
                                            return false;
                                        }
                                        stateA_ref = int.Parse(securityElement.GetAttribute("stateA_ref"));
                                        stateB_ref = int.Parse(securityElement.GetAttribute("stateB_ref"));
                                        stateA = null;
                                        stateB = null;
                                        foreach (ViewModelBase vmb in processVMDict[processA_ref].ProcessToSM_P_VM.StateMachinePVMs[0].UserControlVMs)
                                        {
                                            if (vmb is State_VM)
                                            {
                                                State_VM state_VM = (State_VM)vmb;
                                                if (state_VM.State.Id == stateA_ref)
                                                {
                                                    stateA = state_VM.State;
                                                }
                                            }
                                        }
                                        foreach (ViewModelBase vmb in processVMDict[processB_ref].ProcessToSM_P_VM.StateMachinePVMs[0].UserControlVMs)
                                        {
                                            if (vmb is State_VM)
                                            {
                                                State_VM state_VM = (State_VM)vmb;
                                                if (state_VM.State.Id == stateB_ref)
                                                {
                                                    stateB = state_VM.State;
                                                }
                                            }
                                        }
                                        if (stateA == null || stateB == null)
                                        {
                                            Tips = "[解析SecurityProperty_VM时出错]无法找到Integrity引用的状态机下的State！";
                                            cleanProject();
                                            return false;
                                        }
                                        attributeA_ref = int.Parse(securityElement.GetAttribute("attributeA_ref"));
                                        attributeB_ref = int.Parse(securityElement.GetAttribute("attributeB_ref"));
                                        attributeA = null;
                                        attributeB = null;
                                        foreach (Attribute attribute in processVMDict[processA_ref].Process.Attributes)
                                        {
                                            if (attribute.Id == attributeA_ref)
                                            {
                                                attributeA = attribute;
                                                break;
                                            }
                                        }
                                        foreach (Attribute attribute in processVMDict[processB_ref].Process.Attributes)
                                        {
                                            if (attribute.Id == attributeB_ref)
                                            {
                                                attributeB = attribute;
                                                break;
                                            }
                                        }
                                        if (attributeA == null || attributeB == null)
                                        {
                                            Tips = "[解析SecurityProperty_VM时出错]无法找到Integrity引用的进程模板下的Attribute！";
                                            cleanProject();
                                            return false;
                                        }
                                        Integrity integrity = new Integrity(
                                            processVMDict[processA_ref].Process,
                                            stateA, attributeA,
                                            processVMDict[processB_ref].Process,
                                            stateB, attributeB);
                                        securityProperty.Integrities.Add(integrity);
                                        break;
                                }
                            }
                            break;
                        case "CommChannel_VM":
                            CommChannel_VM commChannel_VM = (CommChannel_VM)userControl_VM;
                            CommChannel commChannel = commChannel_VM.CommChannel;
                            foreach (XmlNode cmpChildNode in node.ChildNodes) // <CommMethodPair process_ref="1" commMethod_ref="1" />
                            {
                                XmlElement cmpElement = (XmlElement)cmpChildNode;
                                if (cmpElement.Name != "CommMethodPair")
                                {
                                    Tips = "[解析CommChannel_VM时出错]子结点必须是CommMethodPair！";
                                    cleanProject();
                                    return false;
                                }
                                int processARef = int.Parse(cmpElement.GetAttribute("processA_ref"));
                                int processBRef = int.Parse(cmpElement.GetAttribute("processB_ref"));
                                if (!processVMDict.ContainsKey(processARef) || !processVMDict.ContainsKey(processBRef))
                                {
                                    Tips = "[解析CommChannel_VM时出错]无法找到CommMethodPair引用的进程模板！";
                                    cleanProject();
                                    return false;
                                }
                                int commMethodARef = int.Parse(cmpElement.GetAttribute("commMethodA_ref"));
                                CommMethod findCommMethodA = null;
                                foreach (CommMethod commMethod in processVMDict[processARef].Process.CommMethods)
                                {
                                    if (commMethod.Id == commMethodARef)
                                    {
                                        findCommMethodA = commMethod;
                                        break;
                                    }
                                }
                                int commMethodBRef = int.Parse(cmpElement.GetAttribute("commMethodB_ref"));
                                CommMethod findCommMethodB = null;
                                foreach (CommMethod commMethod in processVMDict[processBRef].Process.CommMethods)
                                {
                                    if (commMethod.Id == commMethodBRef)
                                    {
                                        findCommMethodB = commMethod;
                                        break;
                                    }
                                }
                                if (findCommMethodA == null || findCommMethodB == null)
                                {
                                    Tips = "[解析CommChannel_VM时出错]无法找到CommMethodPair引用的进程模板下的CommMethod！";
                                    cleanProject();
                                    return false;
                                }
                                bool privacy = bool.Parse(cmpElement.GetAttribute("privacy"));
                                CommMethodPair commMethodPair = new CommMethodPair(
                                    processVMDict[processARef].Process,
                                    findCommMethodA,
                                    processVMDict[processBRef].Process,
                                    findCommMethodB,
                                    privacy);
                                int id = int.Parse(element.GetAttribute("id"));
                                commMethodPair.Id = id;
                                commMethodPairDict.Add(id, commMethodPair);
                                commChannel.CommMethodPairs.Add(commMethodPair);
                            }
                            break;
                        case "Axiom_VM":
                            Axiom_VM axiom_VM = (Axiom_VM)userControl_VM;
                            Axiom axiom = axiom_VM.Axiom;
                            foreach (XmlNode axiomChildNode in node.ChildNodes) // ProcessMethod/Formula
                            {
                                XmlElement acElement = (XmlElement)axiomChildNode;
                                switch (axiomChildNode.Name)
                                {
                                    case "ProcessMethod":
                                        processRef = int.Parse(acElement.GetAttribute("process_ref"));
                                        if (!processVMDict.ContainsKey(processRef))
                                        {
                                            Tips = "[解析Axiom_VM时出错]无法找到ProcessMethod引用的进程模板！";
                                            cleanProject();
                                            return false;
                                        }
                                        int methodRef = int.Parse(acElement.GetAttribute("method_ref"));
                                        Method findMethod = null;
                                        foreach (Method method in processVMDict[processRef].Process.Methods)
                                        {
                                            if (method.Id == methodRef)
                                            {
                                                findMethod = method;
                                                break;
                                            }
                                        }
                                        if (findMethod == null)
                                        {
                                            Tips = "[解析Axiom_VM时出错]无法找到ProcessMethod引用的进程模板下的Method！";
                                            cleanProject();
                                            return false;
                                        }
                                        ProcessMethod processMethod = new ProcessMethod(processVMDict[processRef].Process, findMethod);
                                        axiom.ProcessMethods.Add(processMethod);
                                        break;
                                    case "Formula":
                                        Formula formula = new Formula(acElement.GetAttribute("content"));
                                        axiom.Formulas.Add(formula);
                                        break;
                                }
                            }
                            break;
                    }
                }
                #endregion

                #region 攻击树面板
                xmlNode = doc.SelectSingleNode("Protocol_VM/AttackTree_P_VMs");
                nodeList = xmlNode.ChildNodes;
                ObservableCollection<SidePanel_VM> attackTree_P_VMs = protocolVM.PanelVMs[3].SidePanelVMs;
                foreach (XmlNode node in nodeList) // <AttackTree_P_VM name="攻击树1">
                {
                    XmlElement element = (XmlElement)node;
                    AttackTree_P_VM attackTree_P_VM = new AttackTree_P_VM();
                    attackTree_P_VM.RefName.Content = element.GetAttribute("name");
                    Dictionary<int, Connector_VM> connectorDict = new Dictionary<int, Connector_VM>(); // 记录id->锚点的字典,用于连线
                    foreach (XmlNode attackChildNode in node.ChildNodes) // Attack_VM/Relation_VM/Arrow_VM
                    {
                        XmlElement acElement = (XmlElement)attackChildNode;
                        switch (attackChildNode.Name)
                        {
                            case "Attack_VM":
                                double x = double.Parse(acElement.GetAttribute("x"));
                                double y = double.Parse(acElement.GetAttribute("y"));
                                Attack_VM attack_VM = new Attack_VM(x, y);
                                attack_VM.Attack.Content = acElement.GetAttribute("content");
                                attack_VM.BeAttacked = acElement.GetAttribute("beAttacked") == "True";
                                attack_VM.IsLocked = acElement.GetAttribute("isLocked") == "True";
                                if (attack_VM.ConnectorVMs.Count != acElement.ChildNodes.Count)
                                {
                                    Tips = "[解析Attack_VM时出错]锚点数量和系统要求不一致！";
                                    cleanProject();
                                    return false;
                                }
                                for (int j = 0; j < attack_VM.ConnectorVMs.Count; j++)
                                {
                                    XmlNode connectorNode = acElement.ChildNodes[j];
                                    XmlElement connectorElement = (XmlElement)connectorNode;
                                    int id = int.Parse(connectorElement.GetAttribute("id"));
                                    attack_VM.ConnectorVMs[j].Id = id;
                                    connectorDict.Add(id, attack_VM.ConnectorVMs[j]); // 记录到字典里
                                }
                                attackTree_P_VM.UserControlVMs.Add(attack_VM);
                                break;
                            case "Relation_VM":
                                x = double.Parse(acElement.GetAttribute("x"));
                                y = double.Parse(acElement.GetAttribute("y"));
                                Relation_VM relation_VM = new Relation_VM(x, y);
                                relation_VM.Relation = (Relation)System.Enum.Parse(typeof(Relation), acElement.GetAttribute("relation"));
                                if (relation_VM.ConnectorVMs.Count != acElement.ChildNodes.Count)
                                {
                                    Tips = "[解析Relation_VM时出错]锚点数量和系统要求不一致！";
                                    cleanProject();
                                    return false;
                                }
                                for (int j = 0; j < relation_VM.ConnectorVMs.Count; j++)
                                {
                                    XmlNode connectorNode = acElement.ChildNodes[j];
                                    XmlElement connectorElement = (XmlElement)connectorNode;
                                    int id = int.Parse(connectorElement.GetAttribute("id"));
                                    relation_VM.ConnectorVMs[j].Id = id;
                                    connectorDict.Add(id, relation_VM.ConnectorVMs[j]); // 记录到字典里
                                }
                                attackTree_P_VM.UserControlVMs.Add(relation_VM);
                                break;
                            case "Arrow_VM":
                                Arrow_VM arrow_VM = new Arrow_VM();
                                int sourceRef = int.Parse(acElement.GetAttribute("source_ref"));
                                int destRef = int.Parse(acElement.GetAttribute("dest_ref"));
                                if (!(connectorDict.ContainsKey(sourceRef) && connectorDict.ContainsKey(destRef)))
                                {
                                    Tips = "[解析Arrow_VM时出错]无法找到某端的锚点！";
                                    cleanProject();
                                    return false;
                                }
                                arrow_VM.Source = connectorDict[sourceRef]; // 连线两端引用锚点
                                arrow_VM.Dest = connectorDict[destRef];
                                connectorDict[sourceRef].ConnectionVM = arrow_VM; // 从锚点反引连线
                                connectorDict[destRef].ConnectionVM = arrow_VM;
                                attackTree_P_VM.UserControlVMs.Add(arrow_VM);
                                break;
                        }
                    }
                    attackTree_P_VMs.Add(attackTree_P_VM);
                    protocolVM.PanelVMs[3].SelectedItem = attackTree_P_VM; // fixme 改成记录SelectedItem
                }
                #endregion

                #region 序列图面板
                xmlNode = doc.SelectSingleNode("Protocol_VM/SequenceDiagram_P_VMs");
                nodeList = xmlNode.ChildNodes;
                ObservableCollection<SidePanel_VM> sequenceDiagram_P_VMs = protocolVM.PanelVMs[5].SidePanelVMs;
                foreach (XmlNode node in nodeList) // <SequenceDiagram_P_VM name="顺序图1">
                {
                    XmlElement element = (XmlElement)node;
                    SequenceDiagram_P_VM sequenceDiagram_P_VM = new SequenceDiagram_P_VM();
                    sequenceDiagram_P_VM.RefName.Content = element.GetAttribute("name");
                    Dictionary<int, Connector_VM> connectorDict = new Dictionary<int, Connector_VM>(); // 记录id->锚点的字典,用于连线
                    Dictionary<int, CommMethod> outCommMethodDict = new Dictionary<int, CommMethod>(); // 记录id->OUT型CommMethod的字典，用于连线上的CommMethod查找
                    foreach (XmlNode sequenceChildNode in node.ChildNodes) // ObjLifeLine_VM/各类Message_VM
                    {
                        XmlElement scElement = (XmlElement)sequenceChildNode;
                        if (sequenceChildNode.Name == "ObjLifeLine_VM")
                        {
                            double x = double.Parse(scElement.GetAttribute("x"));
                            double y = double.Parse(scElement.GetAttribute("y"));
                            ObjLifeLine_VM objLifeLine_VM = new ObjLifeLine_VM(x, y);
                            // 处理引用的进程模板
                            int processRef = int.Parse(scElement.GetAttribute("process_ref"));
                            if (processRef == -1)
                            {
                                // nothing to do
                            }
                            else if (!processVMDict.ContainsKey(processRef))
                            {
                                Tips = "[解析ObjLifeLine_VM时出错]无法找到引用的进程模板！";
                                cleanProject();
                                return false;
                            }
                            else
                            {
                                objLifeLine_VM.SeqObject.Process = processVMDict[processRef].Process;
                                // 将OUT型CommMethod记录到字典里
                                // 因为ObjLifeLine_VM一定先于身上的Message_VM创建，所以一定能在后面查到
                                foreach (CommMethod commMethod in objLifeLine_VM.SeqObject.Process.CommMethods)
                                {
                                    if (commMethod.InOutSuffix == InOut.Out &&
                                        !outCommMethodDict.ContainsKey(commMethod.Id))
                                    {// 因为可能有两个ObjLifeline组合了一样的Process，导致这里重复，所以判重
                                        outCommMethodDict.Add(commMethod.Id, commMethod);
                                    }
                                }
                            }
                            // 处理锚点
                            if (objLifeLine_VM.ConnectorVMs.Count != scElement.ChildNodes.Count)
                            {
                                Tips = "[解析ObjLifeLine_VM时出错]锚点数量和系统要求不一致！";
                                cleanProject();
                                return false;
                            }
                            // 锚点记录到字典里
                            for (int j = 0; j < objLifeLine_VM.ConnectorVMs.Count; j++)
                            {
                                XmlNode connectorNode = scElement.ChildNodes[j];
                                XmlElement connectorElement = (XmlElement)connectorNode;
                                int id = int.Parse(connectorElement.GetAttribute("id"));
                                objLifeLine_VM.ConnectorVMs[j].Id = id;
                                connectorDict.Add(id, objLifeLine_VM.ConnectorVMs[j]);
                            }
                            sequenceDiagram_P_VM.UserControlVMs.Add(objLifeLine_VM);
                        }
                        else
                        {
                            Message_VM message_VM = null;
                            switch (sequenceChildNode.Name)
                            {
                                case "SyncMessage_VM":
                                    message_VM = new SyncMessage_VM();
                                    break;
                                case "AsyncMessage_VM":
                                    message_VM = new AsyncMessage_VM();
                                    break;
                                case "ReturnMessage_VM":
                                    message_VM = new ReturnMessage_VM();
                                    break;
                                case "SyncMessage_Self_VM":
                                    message_VM = new SyncMessage_Self_VM();
                                    break;
                                case "AsyncMessage_Self_VM":
                                    message_VM = new AsyncMessage_Self_VM();
                                    break;
                            }
                            if (message_VM != null)
                            {
                                // 获取引用的CommMethod
                                int commMethodRef = int.Parse(scElement.GetAttribute("commMethod_ref"));
                                if (commMethodRef == -1)
                                {
                                    //nothing to do
                                }
                                else if (!outCommMethodDict.ContainsKey(commMethodRef))
                                {
                                    Tips = "[解析" + sequenceChildNode.Name + "时出错]无法找到引用的CommMethod！";
                                    cleanProject();
                                    return false;
                                }
                                else
                                {
                                    message_VM.CommMessage.CommMethod = outCommMethodDict[commMethodRef];
                                }
                                int sourceRef = int.Parse(scElement.GetAttribute("source_ref"));
                                int destRef = int.Parse(scElement.GetAttribute("dest_ref"));
                                if (!(connectorDict.ContainsKey(sourceRef) && connectorDict.ContainsKey(destRef)))
                                {
                                    Tips = "[解析Message_VM时出错]无法找到某端的锚点！";
                                    cleanProject();
                                    return false;
                                }
                                message_VM.Source = connectorDict[sourceRef]; // 连线两端引用锚点
                                message_VM.Dest = connectorDict[destRef];
                                connectorDict[sourceRef].ConnectionVM = message_VM; // 从锚点反引连线
                                connectorDict[destRef].ConnectionVM = message_VM;
                                sequenceDiagram_P_VM.UserControlVMs.Add(message_VM);
                            }
                        }
                    }
                    sequenceDiagram_P_VMs.Add(sequenceDiagram_P_VM);
                    protocolVM.PanelVMs[5].SelectedItem = sequenceDiagram_P_VM; // fixme 改成记录SelectedItem
                }
                #endregion

                #region 拓扑图面板
                xmlNode = doc.SelectSingleNode("Protocol_VM/TopoGraph_P_VMs");
                nodeList = xmlNode.ChildNodes;
                ObservableCollection<SidePanel_VM> topoGraph_P_VMs = protocolVM.PanelVMs[2].SidePanelVMs;
                foreach (XmlNode node in nodeList) // <TopoGraph_P_VM name="拓扑图1">
                {
                    XmlElement element = (XmlElement)node;
                    TopoGraph_P_VM topoGraph_P_VM = new TopoGraph_P_VM();
                    topoGraph_P_VM.RefName.Content = element.GetAttribute("name");
                    Dictionary<int, Connector_VM> connectorDict = new Dictionary<int, Connector_VM>(); // 记录id->锚点的字典,用于连线
                    foreach (XmlNode topoChildNode in node.ChildNodes) // TopoNode_VM 和 各类TopoLink_VM
                    {
                        XmlElement tcElement = (XmlElement)topoChildNode;
                        if (topoChildNode.Name == "TopoNode_VM")
                        {
                            double x = double.Parse(tcElement.GetAttribute("x"));
                            double y = double.Parse(tcElement.GetAttribute("y"));
                            TopoNode_VM topoNode_VM = null;
                            switch (tcElement.GetAttribute("shape"))
                            {
                                case "Circle":
                                    topoNode_VM = new TopoNode_Circle_VM(x, y);
                                    break;
                                case "Square":
                                    topoNode_VM = new TopoNode_Square_VM(x, y);
                                    break;
                            }
                            if (topoNode_VM == null)
                            {
                                Tips = "[解析TopoNode_VM时出错]无法识别的结点形状！";
                                cleanProject();
                                return false;
                            }
                            topoNode_VM.TopoNode.Name = tcElement.GetAttribute("name");
                            topoNode_VM.TopoNode.Color = Brush.Parse(tcElement.GetAttribute("color"));
                            // 处理引用的进程模板
                            int processRef = int.Parse(tcElement.GetAttribute("process_ref"));
                            if (processRef == -1)
                            {
                                // nothing to do
                            }
                            else if (!processVMDict.ContainsKey(processRef))
                            {
                                Tips = "[解析TopoNode_VM时出错]无法找到引用的进程模板！";
                                cleanProject();
                                return false;
                            }
                            else
                            {
                                topoNode_VM.TopoNode.Process = processVMDict[processRef].Process;
                            }
                            // 处理锚点 和 例化的Instance
                            int connectorNum = 0; // 用来记录锚点数量
                            foreach (XmlNode nodeChildNode in topoChildNode.ChildNodes)
                            {
                                XmlElement nodeChildElement = (XmlElement)nodeChildNode;
                                switch (nodeChildNode.Name)
                                {
                                    case "Connector_VM": // 锚点
                                        if (connectorNum == topoNode_VM.ConnectorVMs.Count)
                                        {
                                            Tips = "[解析TopoNode_VM时出错]锚点数量和系统要求不一致！";
                                            cleanProject();
                                            return false;
                                        }
                                        int id = int.Parse(nodeChildElement.GetAttribute("id"));
                                        topoNode_VM.ConnectorVMs[connectorNum].Id = id;
                                        connectorDict.Add(id, topoNode_VM.ConnectorVMs[connectorNum]);
                                        connectorNum++;
                                        break;
                                    case "Instance": // 例化对象
                                        topoNode_VM.TopoNode.Properties.Add(ResourceManager.readInstance(nodeChildNode, typeDict));
                                        break;
                                    default:
                                        Tips = "[解析TopoNode_VM时出错]无法识别的子标签！";
                                        cleanProject();
                                        return false;
                                }
                            }
                            topoGraph_P_VM.UserControlVMs.Add(topoNode_VM);
                        }
                        /*
                        else
                        {
                            TopoLink_VM topoLink_VM = null;
                            switch (topoChildNode.Name)
                            {
                                case "OneWayTopoLink_VM":
                                    topoLink_VM = new OneWayTopoLink_VM();
                                    break;
                                case "TwoWayTopoLink_VM":
                                    topoLink_VM = new TwoWayTopoLink_VM();
                                    break;
                            }
                            if (topoLink_VM != null)
                            {
                                topoLink_VM.TopoLink.Content = tcElement.GetAttribute("content");
                                int sourceRef = int.Parse(tcElement.GetAttribute("source_ref"));
                                int destRef = int.Parse(tcElement.GetAttribute("dest_ref"));
                                if (!(connectorDict.ContainsKey(sourceRef) && connectorDict.ContainsKey(destRef)))
                                {
                                    Tips = "[解析TopoLink_VM时出错]无法找到某端的锚点！";
                                    cleanProject();
                                    return false;
                                }
                                topoLink_VM.Source = connectorDict[sourceRef]; // 连线两端引用锚点
                                topoLink_VM.Dest = connectorDict[destRef];
                                connectorDict[sourceRef].ConnectionVM = topoLink_VM; // 从锚点反引连线
                                connectorDict[destRef].ConnectionVM = topoLink_VM;
                                topoGraph_P_VM.UserControlVMs.Add(topoLink_VM);
                            }
                        }
                        */
                        else if (topoChildNode.Name == "TopoEdge_VM")
                        {
                            TopoEdge_VM topoEdge_VM = new TopoEdge_VM();
                            topoEdge_VM.TopoEdge.Cost = tcElement.GetAttribute("cost");
                            int commMethodPair_ref = int.Parse(tcElement.GetAttribute("commMethodPair_ref"));
                            if (commMethodPair_ref >= 0)
                            {
                                if (!commMethodPairDict.ContainsKey(commMethodPair_ref))
                                {
                                    Tips = "[解析TopoEdge_VM时出错]无法找到引用的CommMethodPair！";
                                    cleanProject();
                                    return false;
                                }
                                topoEdge_VM.TopoEdge.CommMethodPair = commMethodPairDict[commMethodPair_ref];
                            }
                            int sourceRef = int.Parse(tcElement.GetAttribute("source_ref"));
                            int destRef = int.Parse(tcElement.GetAttribute("dest_ref"));
                            if (!(connectorDict.ContainsKey(sourceRef) && connectorDict.ContainsKey(destRef)))
                            {
                                Tips = "[解析TopoEdge_VM时出错]无法找到某端的锚点！";
                                cleanProject();
                                return false;
                            }
                            topoEdge_VM.Source = connectorDict[sourceRef]; // 连线两端引用锚点
                            topoEdge_VM.Dest = connectorDict[destRef];
                            connectorDict[sourceRef].ConnectionVM = topoEdge_VM; // 从锚点反引连线
                            connectorDict[destRef].ConnectionVM = topoEdge_VM;
                            topoGraph_P_VM.UserControlVMs.Add(topoEdge_VM);
                        }
                    }
                    topoGraph_P_VMs.Add(topoGraph_P_VM);
                    protocolVM.PanelVMs[2].SelectedItem = topoGraph_P_VM; // fixme 改成记录SelectedItem
                }
                #endregion
            }
            return true;
        }

        // 重置项目中的各项元数据，如id计数器等
        private void cleanProject()
        {
            protocolVMs.Clear();
            Protocol_VM._id = Process._id = Axiom._id = InitialKnowledge._id
                = Attribute._id = SafetyProperty._id = SecurityProperty._id
                = Connector_VM._id = CommMethod._id = CommChannel._id
                = SequenceDiagram_P_VM._id = TopoGraph_P_VM._id = CTLTree_P_VM._id
                = AttackTree_P_VM._id = TopoNode._id = CommMethodPair._id = 0;
            // 特别注意，对于带有静态创建的内置对象的类型，_id要置为内置对象的数目
            Type._id = 6;
            State._id = 1;
            Method._id = 4;
            selectedItem = null;
        }

        // 生成验证用的XML文件，类似于DoSave的行为，但是XML文件规则不同
        private bool DoSave2(string fileName)
        {
            // 项目文件去除后缀名".sbid"部分
            string cleanName = fileName.Substring(0, fileName.Length - 5);

            // 写入".sbid"文件
            XmlTextWriter xmlWriter = new XmlTextWriter(fileName, null);
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.WriteStartElement("Project");
            string nowTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            xmlWriter.WriteAttributeString("updTime", nowTime); // 最后修改时间
            for (int i = 0; i < protocolVMs.Count; i++)
            {
                xmlWriter.WriteStartElement("Protocol");
                // xmlWriter.WriteAttributeString("id", i.ToString());
                xmlWriter.WriteAttributeString("name", protocolVMs[i].Protocol.Name);
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement();
            xmlWriter.Flush();
            xmlWriter.Close();

            // 写入各个协议的".xml"文件
            for (int i = 0; i < protocolVMs.Count; i++)
            {
                // 当前协议
                Protocol_VM protocolVM = protocolVMs[i];
                // 每个协议文件名是"项目名_i.xml"
                xmlWriter = new XmlTextWriter(cleanName + "_" + i.ToString() + ".xml", null);
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
                        foreach (CTL ctl in vm.SafetyProperty.CTLs)
                        {
                            xmlWriter.WriteStartElement("CTL");
                            xmlWriter.WriteAttributeString("process", ctl.Process.RefName.Content);
                            xmlWriter.WriteAttributeString("state", ctl.State.Name);
                            xmlWriter.WriteAttributeString("formula", ctl.Formula.Content);
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
                                            foreach (Formula formula in stateTrans_VM.StateTrans.Guards) // Guard条件列表
                                            {
                                                xmlWriter.WriteStartElement("Guard");
                                                xmlWriter.WriteAttributeString("content", formula.Content);
                                                xmlWriter.WriteEndElement();
                                            }
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
            }
            return true;
        }

        #endregion

        #region 属性

        // 下方提示条内容
        public string Tips
        {
            get => tips;
            set => this.RaiseAndSetIfChanged(ref tips, value);
        }

        // 集成所有的协议
        public ObservableCollection<Protocol_VM> ProtocolVMs { get => protocolVMs; set => protocolVMs = value; }

        // 记录当前选中项,用于在打开新面板时立即切换过去
        public Protocol_VM SelectedItem
        {
            get => selectedItem;
            set => this.RaiseAndSetIfChanged(ref selectedItem, value);
        }

        // 锚点是否可见
        public bool ConnectorVisible { get => connectorVisible; set => this.RaiseAndSetIfChanged(ref connectorVisible, value); }

        #endregion
    }
}
