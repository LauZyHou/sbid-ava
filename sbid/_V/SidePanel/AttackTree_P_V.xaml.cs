using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using sbid._M;
using sbid._VM;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;

namespace sbid._V
{
    public class AttackTree_P_V : UserControl
    {
        public AttackTree_P_V()
        {
            this.InitializeComponent();
            // 初始化.cs文件中的事件处理
            init_event();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #region 监听鼠标位置用

        private Point mousePos;

        // 无法直接获取到鼠标位置，必须在鼠标相关事件回调方法里取得
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            // 特别注意，要取得的不是相对这个ClassDiagram_P_V的位置，而是相对于里面的内容控件
            ItemsControl panel = ControlExtensions.FindControl<ItemsControl>(this, "panel");
            // 右键在这个面板上按下时
            if (e.GetCurrentPoint(panel).Properties.PointerUpdateKind == PointerUpdateKind.RightButtonPressed)
            {
                // 更新位置
                mousePos = e.GetPosition(panel);
            }
        }

        #endregion

        #region 右键菜单命令

        // 创建攻击结点
        public void CreateAttackVM()
        {
            Attack_VM attackVM = new Attack_VM(mousePos.X, mousePos.Y);
            AttackTreePVM.UserControlVMs.Add(attackVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的攻击结点：" + attackVM.Attack.Content;
        }

        // 创建[与]关系
        public void CreateRelationVM_AND()
        {
            Relation_VM relationVM = new Relation_VM(mousePos.X, mousePos.Y) { Relation = _M.Relation.AND };
            AttackTreePVM.UserControlVMs.Add(relationVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的[与]关系结点(and)";
        }

        // 创建[或]关系
        public void CreateRelationVM_OR()
        {
            Relation_VM relationVM = new Relation_VM(mousePos.X, mousePos.Y) { Relation = _M.Relation.OR };
            AttackTreePVM.UserControlVMs.Add(relationVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的[或]关系结点(or)";
        }

        // 创建[非]关系
        public void CreateRelationVM_NEG()
        {
            Relation_VM relationVM = new Relation_VM(mousePos.X, mousePos.Y) { Relation = _M.Relation.NEG };
            AttackTreePVM.UserControlVMs.Add(relationVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的[非]关系结点(negation)";
        }

        // 创建[顺序与]关系
        public void CreateRelationVM_SAND()
        {
            Relation_VM relationVM = new Relation_VM(mousePos.X, mousePos.Y) { Relation = _M.Relation.SAND };
            AttackTreePVM.UserControlVMs.Add(relationVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的[顺序与]关系结点(sequence and)";
        }

        #endregion

        #region 按钮命令

        // 应用叶子分析的选中项(实际上全部都是翻转规则)
        public void ReverseLeafAttackVM()
        {
            ListBox leafAttackVM_ListBox = ControlExtensions.FindControl<ListBox>(this, "leafAttackVM_ListBox");
            if (leafAttackVM_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要应用的叶子处理规则！";
                return;
            }

            // 翻转
            Attack_VM attack_VM = (Attack_VM)leafAttackVM_ListBox.SelectedItem;
            attack_VM.BeAttacked = !attack_VM.BeAttacked;

            // 重新计算
            AttackTreePVM.HandleAttackVM.CalculateBeAttacked();
        }

        #endregion

        // 对应的VM
        public AttackTree_P_VM AttackTreePVM { get => (AttackTree_P_VM)DataContext; }

        #region 事件

        // "叶子攻击分析"ListBox的选中项变化的处理
        private void leafAttackVM_ListBox_Changed(object sender, SelectionChangedEventArgs e)
        {
            // 先清空[安全策略数据库]的绑定列表
            AttackTreePVM.SecurityPolicies.Clear();

            // 读取策略数据库文件，解析为策略List
            string jsonStr = File.ReadAllText("Assets/SecurityPolicy.json");
            List<SecurityPolicy> securityPolicies = JsonSerializer.Deserialize<List<SecurityPolicy>>(jsonStr);

            // 找出选中项(是一个Attack_VM)，并获得其中Attack的文字
            ListBox leafAttackVM_ListBox = ControlExtensions.FindControl<ListBox>(this, "leafAttackVM_ListBox");
            if (leafAttackVM_ListBox.SelectedItem == null)
            {
                return;
            }
            Attack_VM attack_VM = (Attack_VM)leafAttackVM_ListBox.SelectedItem;
            string attackContent = attack_VM.Attack.Content;

            // 这个集合用于快速判断一个标签是否能和选中项的Attack的文字匹配
            // 这里记录那些无法匹配的标签
            HashSet<string> failLabelHashSet = new HashSet<string>();

            // 遍历策略数据库，检查是否匹配
            foreach (SecurityPolicy securityPolicy in securityPolicies)
            {
                foreach (string label in securityPolicy.Labels) // 该策略的每个标签
                {
                    if (failLabelHashSet.Contains(label)) // 先检查是否已经检查过为"不匹配"
                    {
                        continue;
                    }
                    if (!attackContent.Contains(label)) // 再进行字符串匹配的检查
                    {
                        failLabelHashSet.Add(label);
                        continue;
                    }
                    // 至此，匹配成功，将其添加到[安全策略数据库]的绑定列表中
                    AttackTreePVM.SecurityPolicies.Add(securityPolicy);
                }
            }
        }

        #endregion

        #region 初始化

        // 初始化.cs文件中的事件处理方法,一些无法在xaml中绑定的部分在这里绑定
        private void init_event()
        {
            // 绑定"叶子攻击分析"ListBox的选中项变化的处理
            ListBox leafAttackVM_ListBox = ControlExtensions.FindControl<ListBox>(this, "leafAttackVM_ListBox");
            leafAttackVM_ListBox.SelectionChanged += leafAttackVM_ListBox_Changed;
        }

        #endregion

        #region 测试

        // Json序列化 + 写入文件
        private void test_json_write()
        {
            // 形成Json字符串的设置
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                // 这个选项解决输出编码问题
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All)
            };

            // 构建要序列化的对象
            List<SecurityPolicy> list = new List<SecurityPolicy>();
            SecurityPolicy securityPolicy = new SecurityPolicy();
            securityPolicy.Content = "测试内容a";
            securityPolicy.Labels = new List<string> { "标签1", "标签2" };
            list.Add(securityPolicy);
            securityPolicy = new SecurityPolicy();
            securityPolicy.Content = "测试内容b";
            securityPolicy.Labels = new List<string> { "标签3", "标签4" };
            list.Add(securityPolicy);

            // 形成Json字符串
            string jsonStr = JsonSerializer.Serialize(list, options);

            // 写入文件(覆盖)
            File.WriteAllText("Assets/SecurityPolicy.json", jsonStr, Encoding.UTF8);
        }

        // 读文件 + Json反序列化
        private void test_json_read()
        {
            //test_json_write(); // 因为资源每次从源文件拷贝到编译后的目录，这里调用一次写将其盖掉
            string jsonStr = File.ReadAllText("Assets/SecurityPolicy.json");
            // 此处断点调试测试
            List<SecurityPolicy> list = JsonSerializer.Deserialize<List<SecurityPolicy>>(jsonStr);
        }

        #endregion
    }
}
