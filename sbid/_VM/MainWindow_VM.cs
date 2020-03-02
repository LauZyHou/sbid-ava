﻿using System.Collections.Generic;
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
            else
                Tips = "项目载入失败！请检查项目文件是否被手动修改";
        }
        #endregion

        #region 私有

        // 预打开文件：返回文件路径
        private async Task<string> GetOpenFileName()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filters.Add(new FileDialogFilter() { Name = "项目文件", Extensions = { "sbid" } });
            string[] result = await dialog.ShowAsync(ResourceManager.mainWindowV);
            return string.Join(" ", result);
        }

        // 预保存文件：返回文件路径
        private async Task<string> GetSaveFileName()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filters.Add(new FileDialogFilter() { Name = "项目文件", Extensions = { "sbid" } });
            string result = await dialog.ShowAsync(ResourceManager.mainWindowV);
            return result;
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
                                xmlWriter.WriteEndElement();
                            }
                        }
                        xmlWriter.WriteEndElement();
                    }
                    else if (item is Process_VM)
                    {
                        // todo
                    }
                }
                xmlWriter.WriteEndElement();

                // 状态机面板todo
                xmlWriter.WriteStartElement("StateMachine_P_VMs");
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

            // 重置协议数据
            protocolVMs.Clear();
            Protocol_VM._id = Type._id = 0;
            selectedItem = null;

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
                 第一遍扫描把所有的类图VM创建出来(里面的V自动跟着创建)，注意还要把id写好
                 第二遍扫描再去管里面的内容(根据id找引用了谁)
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
