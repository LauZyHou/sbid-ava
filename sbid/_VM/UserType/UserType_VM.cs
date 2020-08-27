using sbid._M;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;
using sbid._V;
using Avalonia.Controls;
using System.Collections.ObjectModel;

namespace sbid._VM
{
    public class UserType_VM : NetworkItem_VM
    {
        private Type type;

        // 无参构造时,构造的总是UserType
        public UserType_VM()
        {
            this.type = new UserType();
        }

        // 传入Type参数的构造,当为内置的Type创建UserType_VM时使用此构造
        public UserType_VM(Type type)
        {
            this.type = type;
        }

        // 因为UserType_VM也可能维护底层的int和bool,所以这里用Type而不是UserType
        public Type Type { get => type; set => type = value; }

        #region 右键菜单命令

        // 尝试删除当前UserType_VM
        public void DeleteUserTypeVM()
        {
            if (!(type is UserType) || type == Type.TYPE_TIMER || type == Type.TYPE_BYTE_VEC)
            {
                ResourceManager.mainWindowVM.Tips = "无效的操作，禁止删除内置类型：" + type.Name;
                return;
            }
            // 遍历这个类图面板的建模元素表
            ObservableCollection<ViewModelBase> userControlVMs = ResourceManager.mainWindowVM.SelectedItem.PanelVMs[0].SidePanelVMs[0].UserControlVMs;
            foreach (ViewModelBase vmb in userControlVMs)
            {
                if (vmb is UserType_VM)
                {
                    UserType_VM userType_VM = vmb as UserType_VM;
                    if (!(userType_VM.Type is UserType))
                    {
                        continue;
                    }
                    UserType userType = userType_VM.Type as UserType;
                    // 类型继承此类型时，清除继承
                    if (userType.Parent == this.type)
                    {
                        userType.Parent = null;
                    }
                    // 处理属性列表中的不一致
                    cleanAttributes(userType.Attributes);
                    // 处理方法列表中的不一致
                    cleanMethods(userType.Methods);
                }
                else if (vmb is Process_VM)
                {
                    Process_VM process_VM = vmb as Process_VM;
                    Process process = process_VM.Process;
                    // 处理属性列表中的不一致
                    cleanAttributes(process.Attributes);
                    // 处理方法列表中的不一致
                    cleanMethods(process.Methods);
                    // 处理通信方法列表中的不一致
                    cleanCommMethod(process.CommMethods);
                }
                else if (vmb is InitialKnowledge_VM)
                {
                    InitialKnowledge_VM initialKnowledge_VM = vmb as InitialKnowledge_VM;
                    InitialKnowledge initialKnowledge = initialKnowledge_VM.InitialKnowledge;
                    // 处理单知识声明中的不一致
                    cleanKnowledges(initialKnowledge.Knowledges);
                    // 处理公私钥对声明中的不一致
                    cleanKeyPairs(initialKnowledge.KeyPairs);
                }
                else if (vmb is SecurityProperty_VM)
                {
                    SecurityProperty_VM securityProperty_VM = vmb as SecurityProperty_VM;
                    SecurityProperty securityProperty = securityProperty_VM.SecurityProperty;
                    // 处理保密性中的不一致
                    cleanConfidentials(securityProperty.Confidentials);
                    // 处理认证性中的不一致
                    cleanAuthenticities(securityProperty.Authenticities);
                    // 处理完整性中的不一致
                    cleanIntegrities(securityProperty.Integrities);
                }
            }
            userControlVMs.Remove(this);
            ResourceManager.mainWindowVM.Tips = "删除了自定义类型：" + type.Name;
        }

        // 尝试打开当前UserType_VM的编辑窗体
        public void EditUserTypeVM()
        {
            if (!(type is UserType) || type == Type.TYPE_TIMER || type == Type.TYPE_BYTE_VEC)
            {
                ResourceManager.mainWindowVM.Tips = "无效的操作，禁止编辑内置类型：" + type.Name;
                return;
            }

            // 从主窗体打开编辑窗体,并在其DataContext中集成当前UserType_VM里集成的UserType对象,以能对其作修改
            UserType_EW_V userTypeEWV = new UserType_EW_V()
            {
                DataContext = new UserType_EW_VM()
                {
                    UserType = (UserType)type
                }
            };
            // 将所有的Type也传入,作为可添加的Attribute的可选类型
            foreach (NetworkItem_VM item in ResourceManager.mainWindowVM.SelectedItem.SelectedItem.SelectedItem.UserControlVMs)
            {
                if (item is UserType_VM && item != this)
                {
                    ((UserType_EW_VM)userTypeEWV.DataContext).Types.Add(((UserType_VM)item).Type);
                    // 如果是UserType的话要加到相应列表里
                    if (((UserType_VM)item).Type is UserType)
                    {
                        UserType userType = (UserType)((UserType_VM)item).Type;
                        ((UserType_EW_VM)userTypeEWV.DataContext).UserTypes.Add(userType);
                    }
                }
            }

            // [bugfix]因为在xaml里绑定UserType打开编辑窗口显示出不来，只好在这里手动设置一下
            ComboBox userType_ComboBox = ControlExtensions.FindControl<ComboBox>(userTypeEWV, "userType_ComboBox");
            userType_ComboBox.SelectedItem = ((UserType_EW_VM)userTypeEWV.DataContext).UserType.Parent;

            userTypeEWV.ShowDialog(ResourceManager.mainWindowV);
            ResourceManager.mainWindowVM.Tips = "打开了自定义类型：" + type.Name + "的编辑窗体";
        }

        #endregion

        #region 私有工具

        // 处理Attribute列表中因本类型删除导致不一致的地方
        private void cleanAttributes(ObservableCollection<Attribute> attribues)
        {
            List<Attribute> removeAttrs = new List<Attribute>();
            foreach (Attribute attr in attribues)
            {
                if (attr.Type == this.type)
                {
                    removeAttrs.Add(attr);
                }
            }
            foreach (Attribute attr in removeAttrs)
            {
                attribues.Remove(attr);
            }
        }

        // 处理Method列表中因本类型删除导致不一致的地方
        private void cleanMethods(ObservableCollection<Method> methods)
        {
            foreach (Method method in methods)
            {
                // 用作返回值，则返回值改为ByteVec
                if (method.ReturnType == this.type)
                {
                    method.ReturnType = Type.TYPE_BYTE_VEC;
                }
                // 用作方法参数，则参数删除
                List<Attribute> removeParams = new List<Attribute>();
                foreach (Attribute param in method.Parameters)
                {
                    if (param.Type == this.type)
                    {
                        removeParams.Add(param);
                        break;
                    }
                }
                foreach (Attribute param in removeParams)
                {
                    method.Parameters.Remove(param);
                }
                method.RaisePropertyChanged("ParamString");
            }
        }

        // 处理CommMethod列表中因本类型删除导致不一致的地方
        private void cleanCommMethod(ObservableCollection<CommMethod> commMethods)
        {
            foreach (CommMethod commMethod in commMethods)
            {
                // 用作方法参数，则参数删除
                List<Attribute> removeParams = new List<Attribute>();
                foreach (Attribute param in commMethod.Parameters)
                {
                    if (param.Type == this.type)
                    {
                        removeParams.Add(param);
                        break;
                    }
                }
                foreach (Attribute param in removeParams)
                {
                    commMethod.Parameters.Remove(param);
                }
                commMethod.RaisePropertyChanged("ParamString");
            }
        }

        // 处理Knowledge列表中因本类型删除导致不一致的地方
        private void cleanKnowledges(ObservableCollection<Knowledge> knowledges)
        {
            List<Knowledge> removeKnowledges = new List<Knowledge>();
            foreach (Knowledge knowledge in knowledges)
            {
                if (knowledge.Attribute.Type == this.type)
                {
                    removeKnowledges.Add(knowledge);
                }
            }
            foreach (Knowledge knowledge in removeKnowledges)
            {
                knowledges.Remove(knowledge);
            }
        }

        // 处理KeyPair列表中因本类型删除导致不一致的地方
        private void cleanKeyPairs(ObservableCollection<KeyPair> keyPairs)
        {
            List<KeyPair> removeKeyPairs = new List<KeyPair>();
            foreach (KeyPair keyPair in keyPairs)
            {
                if (keyPair.PubKey.Type == this.type || keyPair.SecKey.Type == this.type)
                {
                    removeKeyPairs.Add(keyPair);
                }
            }
            foreach (KeyPair keyPair in removeKeyPairs)
            {
                keyPairs.Remove(keyPair);
            }
        }

        // 处理Confidential列表中因本类型删除导致不一致的地方
        private void cleanConfidentials(ObservableCollection<Confidential> confidentials)
        {
            List<Confidential> removeConfs = new List<Confidential>();
            foreach (Confidential conf in confidentials)
            {
                if (conf.Attribute.Type == this.type)
                {
                    removeConfs.Add(conf);
                }
            }
            foreach (Confidential conf in removeConfs)
            {
                confidentials.Remove(conf);
            }
        }

        // 处理Authenticity列表中因本类型删除导致不一致的地方
        private void cleanAuthenticities(ObservableCollection<Authenticity> authenticities)
        {
            List<Authenticity> removeAuths = new List<Authenticity>();
            foreach (Authenticity auth in authenticities)
            {
                if (auth.AttributeA.Type == this.type || auth.AttributeB.Type == this.type ||
                    auth.AttributeA_Attr.Type == this.type || auth.AttributeB_Attr.Type == this.type)
                {
                    removeAuths.Add(auth);
                }
            }
            foreach (Authenticity auth in removeAuths)
            {
                authenticities.Remove(auth);
            }
        }

        // 处理Integrity列表中因本类型删除导致不一致的地方
        private void cleanIntegrities(ObservableCollection<Integrity> integrities)
        {
            List<Integrity> removeIntes = new List<Integrity>();
            foreach (Integrity inte in integrities)
            {
                if (inte.AttributeA.Type == this.type || inte.AttributeB.Type == this.type)
                {
                    removeIntes.Add(inte);
                }
            }
            foreach (Integrity inte in removeIntes)
            {
                integrities.Remove(inte);
            }
        }

        #endregion
    }
}
