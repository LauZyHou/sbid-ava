using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ReactiveUI;
using System.Reactive;
using sbid._M;
using Avalonia.Controls;
using System.Threading.Tasks;
using System.Xml;

namespace sbid._VM
{
    public class MainWindow_VM : ViewModelBase
    {
        #region 字段

        private string tips = "123";
        private ObservableCollection<Protocol_VM> protocolVMs = new ObservableCollection<Protocol_VM>();
        private Protocol_VM selectedItem;

        #endregion

        #region 构造

        public MainWindow_VM()
        {
            //// 指示添加类图等命令是否可用的IObservable对象
            //IObservable<bool> addEnabled = this.WhenAnyValue(
            //    x => x.SelectedItem,
            //    x => !Protocol_VM.IsNull(x)
            //    );

            //// 生成命令
            //AddClassDiagram = ReactiveCommand.Create(
            //    () => new ClassDiagram_P_VM(),
            //    addEnabled
            //    );

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

        // 添加新类图
        /*
        public void AddClassDiagram()
        {
            // 判定协议已创建
            if (selectedItem == null)
                return;

            // 切换到当前协议的类图面板下
            selectedItem.SelectedItem = selectedItem.PanelVMs[0];

            // 添加过程
            ClassDiagram_P_VM pvm = new ClassDiagram_P_VM();
            selectedItem.PanelVMs[0].SidePanelVMs.Add(pvm);
            selectedItem.PanelVMs[0].SelectedItem = pvm;
        }
        */

        // 添加状态机,要将所在Process引用传入以反向查询,将状态机面板返回以给Process_VM集成
        // 此方法在用户创建Process时调用
        public StateMachine_P_VM AddStateMachine(Process process)
        {
            StateMachine_P_VM pvm = new StateMachine_P_VM(process);
            pvm.init_data();

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

        // 按下保存按钮
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

        // 按下载入按钮
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

                // 概览>类图面板。这个比较特殊，因为就这一个，而且一定有这一个，用户既不能创建也不能销毁
                xmlWriter.WriteStartElement("ClassDiagram_P_VM");
                ClassDiagram_P_VM classDiagram_P_VM = (ClassDiagram_P_VM)protocolVM.PanelVMs[0].SidePanelVMs[0];
                foreach (NetworkItem_VM item in classDiagram_P_VM.NetworkItemVMs)
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
                        if (!(vm.Type is UserType)) // 基本类型(int,bool)
                        {
                            xmlWriter.WriteAttributeString("basic", "true");
                            // 注意，基本类型在创建类图时就创建了，所以要
                        }
                        else // 用户自定义类型
                        {
                            UserType userType = (UserType)vm.Type;
                            xmlWriter.WriteAttributeString("basic", "false");
                            foreach (Attribute attr in userType.Attributes)
                            {
                                xmlWriter.WriteStartElement("Attribute");
                                xmlWriter.WriteAttributeString("type_ref", attr.Type.Id.ToString());
                                xmlWriter.WriteAttributeString("identifier", attr.Identifier);
                                xmlWriter.WriteAttributeString("id", attr.Id.ToString());
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
                        xmlWriter.WriteAttributeString("name", vm.Process.Name);
                        xmlWriter.WriteAttributeString("id", vm.Process.Id.ToString());
                        foreach (Attribute attr in vm.Process.Attributes)
                        {
                            xmlWriter.WriteStartElement("Attribute");
                            xmlWriter.WriteAttributeString("type_ref", attr.Type.Id.ToString());
                            xmlWriter.WriteAttributeString("identifier", attr.Identifier);
                            xmlWriter.WriteAttributeString("id", attr.Id.ToString());
                            xmlWriter.WriteEndElement();
                        }
                        foreach (Method method in vm.Process.Methods)
                        {
                            xmlWriter.WriteStartElement("Method");
                            xmlWriter.WriteAttributeString("returnType_ref", method.ReturnType.Id.ToString());
                            xmlWriter.WriteAttributeString("name", method.Name);
                            xmlWriter.WriteAttributeString("cryptoSuffix", method.CryptoSuffix.ToString());
                            foreach (Attribute attr in method.Parameters)
                            {
                                xmlWriter.WriteStartElement("Parameter");
                                xmlWriter.WriteAttributeString("type_ref", attr.Type.Id.ToString());
                                xmlWriter.WriteAttributeString("identifier", attr.Identifier);
                                xmlWriter.WriteAttributeString("id", attr.Id.ToString());
                                xmlWriter.WriteEndElement();
                            }
                            xmlWriter.WriteEndElement();
                        }
                        foreach (CommMethod commMethod in vm.Process.CommMethods)
                        {
                            xmlWriter.WriteStartElement("CommMethod");
                            xmlWriter.WriteAttributeString("name", commMethod.Name);
                            xmlWriter.WriteAttributeString("inOutSuffix", commMethod.InOutSuffix.ToString());
                            foreach (Attribute attr in commMethod.Parameters)
                            {
                                xmlWriter.WriteStartElement("Parameter");
                                xmlWriter.WriteAttributeString("type_ref", attr.Type.Id.ToString());
                                xmlWriter.WriteAttributeString("identifier", attr.Identifier);
                                xmlWriter.WriteAttributeString("id", attr.Id.ToString());
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
                        foreach (Method method in vm.Axiom.Methods)
                        {
                            xmlWriter.WriteStartElement("Method");
                            xmlWriter.WriteAttributeString("returnType_ref", method.ReturnType.Id.ToString());
                            xmlWriter.WriteAttributeString("name", method.Name);
                            xmlWriter.WriteAttributeString("cryptoSuffix", method.CryptoSuffix.ToString());
                            foreach (Attribute attr in method.Parameters)
                            {
                                xmlWriter.WriteStartElement("Parameter");
                                xmlWriter.WriteAttributeString("type_ref", attr.Type.Id.ToString());
                                xmlWriter.WriteAttributeString("identifier", attr.Identifier);
                                xmlWriter.WriteAttributeString("id", attr.Id.ToString());
                                xmlWriter.WriteEndElement();
                            }
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
                        xmlWriter.WriteAttributeString("name", vm.InitialKnowledge.Name);
                        xmlWriter.WriteAttributeString("id", vm.InitialKnowledge.Id.ToString());
                        foreach (KnowledgePair knowledgePair in vm.InitialKnowledge.KnowledgePairs)
                        {
                            xmlWriter.WriteStartElement("KnowledgePair");
                            xmlWriter.WriteAttributeString("process_ref", knowledgePair.Process.Id.ToString());
                            // 为了这里需要，特地为Attribute也添加了Id，读取xml的时候才能知道选的是Process的哪个Attribute
                            // 下面类似的地方标注了"注意"字样
                            xmlWriter.WriteAttributeString("attribute_ref", knowledgePair.Attribute.Id.ToString());
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
                        foreach (Formula formula in vm.SafetyProperty.CTLs)
                        {
                            xmlWriter.WriteStartElement("CTL");
                            xmlWriter.WriteAttributeString("content", formula.Content);
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
                            xmlWriter.WriteAttributeString("processB_ref", authenticity.ProcessB.Id.ToString());
                            xmlWriter.WriteAttributeString("stateB_ref", authenticity.StateB.Id.ToString());
                            xmlWriter.WriteAttributeString("attributeB_ref", authenticity.AttributeB.Id.ToString()); // 注意
                            xmlWriter.WriteEndElement();
                        }
                        xmlWriter.WriteEndElement();
                    }
                }
                xmlWriter.WriteEndElement();

                // 状态机面板
                xmlWriter.WriteStartElement("StateMachine_P_VMs");
                ObservableCollection<SidePanel_VM> stateMachine_P_VMs = protocolVM.PanelVMs[1].SidePanelVMs; // 所有的状态机
                foreach (SidePanel_VM sidePanel_VM in stateMachine_P_VMs)
                {
                    StateMachine_P_VM stateMachine_P_VM = (StateMachine_P_VM)sidePanel_VM;
                    xmlWriter.WriteStartElement("StateMachine_P_VM");
                    xmlWriter.WriteAttributeString("process_ref", stateMachine_P_VM.Process.Id.ToString());
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
                    }
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();

                // 攻击树面板todo
                xmlWriter.WriteStartElement("AttackTree_P_VMs");
                xmlWriter.WriteEndElement();

                // 序列图面板todo
                xmlWriter.WriteStartElement("SequenceDiagram_P_VMs");
                xmlWriter.WriteEndElement();

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

                // 概览>类图面板
                /*
                 因为可能出现先创建的类图引用了后创建的类图的情况
                 所以这里扫描两遍类图
                 第一遍扫描把所有的类图VM创建出来(里面的M自动跟着创建)，注意还要把id盖掉
                 第二遍扫描再去管里面的内容(根据xxx_ref所指定的id找引用了谁)
                 */
                // 第一遍扫描
                ClassDiagram_P_VM classDiagram_P_VM = (ClassDiagram_P_VM)protocolVM.PanelVMs[0].SidePanelVMs[0];
                XmlNode xmlNode = doc.SelectSingleNode("Protocol_VM/ClassDiagram_P_VM");
                XmlNodeList nodeList = xmlNode.ChildNodes;
                foreach (XmlNode node in nodeList)
                {
                    XmlElement element = (XmlElement)node;
                    NetworkItem_VM networkItem_VM = null;
                    switch (node.Name)
                    {
                        case "UserType_VM":
                            if (element.GetAttribute("basic") == "true") // 内置类型
                            {
                                if (element.GetAttribute("name") == "int")
                                {
                                    Type.TYPE_INT.Id = int.Parse(element.GetAttribute("id"));
                                    networkItem_VM = new UserType_VM(Type.TYPE_INT);
                                }
                                else if (element.GetAttribute("name") == "bool")
                                {
                                    Type.TYPE_BOOL.Id = int.Parse(element.GetAttribute("id"));
                                    networkItem_VM = new UserType_VM(Type.TYPE_BOOL);
                                }
                            }
                            else // 用户自定义类型
                            {
                                networkItem_VM = new UserType_VM();
                                ((UserType_VM)networkItem_VM).Type.Id = int.Parse(element.GetAttribute("id"));
                                ((UserType_VM)networkItem_VM).Type.Name = element.GetAttribute("name");
                            }
                            break;
                        case "Process_VM":
                            networkItem_VM = new Process_VM();
                            Process_VM process_VM = (Process_VM)networkItem_VM;
                            process_VM.Process.Id = int.Parse(element.GetAttribute("id"));
                            process_VM.Process.Name = element.GetAttribute("name");
                            // 创建对应的状态机面板，加到当前协议的状态机选项卡下面
                            StateMachine_P_VM pvm = new StateMachine_P_VM(process_VM.Process);
                            protocolVM.PanelVMs[1].SidePanelVMs.Add(pvm);
                            protocolVM.PanelVMs[1].SelectedItem = pvm; // fixme 这里改成记录用户保存的选择
                            process_VM.StateMachine_P_VM = pvm; // 从Process的反引
                            break;
                        case "Axiom_VM":
                            networkItem_VM = new Axiom_VM();
                            ((Axiom_VM)networkItem_VM).Axiom.Id = int.Parse(element.GetAttribute("id"));
                            ((Axiom_VM)networkItem_VM).Axiom.Name = element.GetAttribute("name");
                            break;
                        case "InitialKnowledge_VM":
                            networkItem_VM = new InitialKnowledge_VM();
                            ((InitialKnowledge_VM)networkItem_VM).InitialKnowledge.Id = int.Parse(element.GetAttribute("id"));
                            ((InitialKnowledge_VM)networkItem_VM).InitialKnowledge.Name = element.GetAttribute("name");
                            break;
                        case "SafetyProperty_VM":
                            networkItem_VM = new SafetyProperty_VM();
                            ((SafetyProperty_VM)networkItem_VM).SafetyProperty.Id = int.Parse(element.GetAttribute("id"));
                            ((SafetyProperty_VM)networkItem_VM).SafetyProperty.Name = element.GetAttribute("name");
                            break;
                        case "SecurityProperty_VM":
                            networkItem_VM = new SecurityProperty_VM();
                            ((SecurityProperty_VM)networkItem_VM).SecurityProperty.Id = int.Parse(element.GetAttribute("id"));
                            ((SecurityProperty_VM)networkItem_VM).SecurityProperty.Name = element.GetAttribute("name");
                            break;
                    }
                    // 写入位置信息
                    if (networkItem_VM != null)
                    {
                        networkItem_VM.X = double.Parse(element.GetAttribute("x"));
                        networkItem_VM.Y = double.Parse(element.GetAttribute("y"));
                        classDiagram_P_VM.NetworkItemVMs.Add(networkItem_VM);
                    }
                }

                // 状态机面板
                ObservableCollection<SidePanel_VM> stateMachine_P_VMs = protocolVM.PanelVMs[1].SidePanelVMs; // 这里是创建进程模板时创建的
                xmlNode = doc.SelectSingleNode("Protocol_VM/StateMachine_P_VMs");
                nodeList = xmlNode.ChildNodes;
                if (stateMachine_P_VMs.Count != nodeList.Count)
                {
                    Tips = "[解析StateMachine_P_VMs时出错]子结点数和进程模板数不同！";
                    cleanProject();
                    return false;
                }
                for (int j = 0; j < nodeList.Count; j++) // <StateMachine_P_VM process_ref="xxx">
                {
                    XmlNode node = nodeList[j];
                    XmlElement element = (XmlElement)node;
                    StateMachine_P_VM stateMachine_P_VM = (StateMachine_P_VM)stateMachine_P_VMs[j];
                    if (node.Name != "StateMachine_P_VM")
                    {
                        Tips = "[解析StateMachine_P_VMs时出错]其下出现了异常标签！";
                        cleanProject();
                        return false;
                    }
                    if (element.GetAttribute("process_ref") != stateMachine_P_VM.Process.Id.ToString())
                    {
                        Tips = "[解析StateMachine_P_VM时出错]process_ref和进程模板不能按序对应！";
                        cleanProject();
                        return false;
                    }
                    Dictionary<int, Connector_VM> connectorDict = new Dictionary<int, Connector_VM>(); // 记录id->锚点的字典,用于连线
                    XmlNodeList stateAndTransList = node.ChildNodes;
                    foreach (XmlNode satNode in stateAndTransList) // State_VM/Transition_VM/InitState_VM/FinalState_VM
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
                                    connectorDict.Add(id, state_VM.ConnectorVMs[k]); // 记录到字典里
                                }
                                stateMachine_P_VM.UserControlVMs.Add(state_VM);
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
                            case "Transition_VM":
                                Transition_VM transition_VM = new Transition_VM();
                                int sourceRef = int.Parse(satElement.GetAttribute("source_ref"));
                                int destRef = int.Parse(satElement.GetAttribute("dest_ref"));
                                if (!(connectorDict.ContainsKey(sourceRef) && connectorDict.ContainsKey(destRef)))
                                {
                                    Tips = "[解析Transition_VM时出错]无法找到某端的锚点！";
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
                        }
                    }
                }
            }

            /*
            // 读取".sbid"文件，以创建这么多的协议面板
            XmlTextReader xmlReader = new XmlTextReader(fileName);
            xmlReader.WhitespaceHandling = WhitespaceHandling.None;
            protocolVMs.Clear(); // 清除旧的协议
            while (xmlReader.Read())
            {
                if(xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name== "Protocol_VM")
                {
                    this.AddProtocol();
                }
            }
            xmlReader.Close();

            // 读取各个协议的".xml"文件，这里改用XmlDocument
            for (int i = 0; i < protocolVMs.Count; i++)
            {
                // 当前协议
                Protocol_VM protocolVM = protocolVMs[i];
                // 每个协议文件名是"项目名_i.xml"
                xmlReader = new XmlTextReader(cleanName + "_" + i.ToString() + ".xml");

                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        switch (xmlReader.Name)
                        {
                            case "ClassDiagram_P_VM":
                                // todo
                                break;
                            case "StateMachine_P_VMs":
                                break;
                            case "AttackTree_P_VMs":
                                break;
                            case "SequenceDiagram_P_VMs":
                                break;
                        }
                    }
                }

                xmlReader.Close();
            }
            */

            return true;
        }

        // 重置项目中的各项元数据，如id计数器等
        private void cleanProject()
        {
            protocolVMs.Clear();
            Protocol_VM._id = Type._id = Process._id = Axiom._id = InitialKnowledge._id
                = Attribute._id = SafetyProperty._id = State._id = SecurityProperty._id
                = Connector_VM._id = 0;
            selectedItem = null;
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

        // [命令]添加类图
        //public ReactiveCommand<Unit, ClassDiagram_P_VM> AddClassDiagram { get; set; }

        #endregion
    }
}
