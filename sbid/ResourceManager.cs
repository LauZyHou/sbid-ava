using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Media.Imaging;
using Avalonia.VisualTree;
using sbid._M;
using sbid._V;
using sbid._VM;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Xml;

namespace sbid
{
    public class ResourceManager
    {
        // 保存主窗体ViewModel,用于在全局任何位置都能直接获取
        public static MainWindow_VM mainWindowVM;
        // 保存主窗体View,用于在全局任何位置都能直接获取
        public static MainWindow_V mainWindowV;
        // 保存当前运行的操作系统信息（在MainWindow_VM构造时写入）
        public static Platform Platform;
        // 保存当前程序所在的运行目录路径（在MainWindow_VM构造时写入）
        public static string RunPath;
        // 维护当前项目保存位置（项目打开时写入，也可在首选项中编辑）
        public static string projectSavePath = null;
        // 维护ProVerif可执行文件位置（在首选项中编辑）
        public static string proVerifPath = null;
        // 维护Beagle可执行文件位置（在首选项中编辑）
        public static string beaglePath = null;
        // 开发者模式（在首选项中编辑）
        public static bool devMode = false;

        // 性质验证命令所在脚本
        public static string justVerifyCommmad_file = "./resource/verify";
        public static string justVerifyCommmad_param = null;

        // 【11月10日作废】模拟执行代码生成命令所在脚本
        public static string justSimuGenCommand_file = "./resource/simu_gen";
        public static string justSimuGenCommand_param = null;
        // 【11月10日作废】模拟执行代码编译命令所在脚本
        public static string justCompileCommand_file = "./resource/simu_compile";
        public static string justCompileCommand_param = null;
        // 【11月10日作废】模拟执行命令所在脚本
        public static string justRunCommand_file = "./resource/simu_run";
        public static string justRunCommand_param = null;

        // 【11月10日作废】可执行代码生成命令所在脚本
        public static string justExecGenCommand_file = "./resource/exec_gen";
        public static string justExecGenCommand_param = null;

        // 语法检查：检查
        public static string Check_check = "./resource/Check/check";
        // 性质验证：验证功能安全性质
        public static string Verify_safety = "./resource/Verify/safety";
        // 性质验证：验证信息安全性质
        public static string Verify_security = "./resource/Verify/security";
        // 模拟执行：执行
        public static string Simu_simu = "./resource/Simu/simu";
        // 代码框架生成：生成
        public static string FrameGen_gen = "./resource/FrameGen/gen";
        // 程序精化：精化
        public static string CodeRefine_refine = "./resource/CodeRefine/refine";
        // 可执行代码生成：生成
        public static string CodeGen_gen = "./resource/CodeGen/gen";
        // 可执行代码生成：编译
        public static string CodeGen_comp = "./resource/CodeGen/comp";
        // 可执行代码生成：运行
        public static string CodeGen_run = "./resource/CodeGen/run";

        /*
        【可变动资源】需要在编译后手动复制到输出目录，不论是Debug模式和Release模式
        所有的可变动资源不能设置成被VS管理的资源（即在解决方案资源管理器中不能存在）
        否则将在“生成单个文件”时被尝试加进去，最终却会导致不可用
        */
        // 安全策略数据库文件。用于攻击树叶子攻击分析时得到安全策略提示
        public static string security_policy_json = "./resource/SecurityPolicy.json";
        // 生成后端XML文件时的索引文件。用于性质验证、模拟执行、代码生成
        public static string back_xml_sbid = "./backxml/back.sbid"; // 用于DoSave2【11月11作废】
        public static string back_xml = "./backxml/current.xml"; // 用于ExportBackXML【11月11启用】
        // （临时）测试provrif文件
        public static string test_proverif_pv = "/resource/proverif.pv";

        #region 导出图片相关接口

        // 预保存文件：传入文件后缀(如sbid/xml/png)，返回文件路径
        public static async Task<string> GetSaveFileName(string suffix)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filters.Add(new FileDialogFilter() { Name = suffix + "文件", Extensions = { suffix } });
            string result = await dialog.ShowAsync(mainWindowV);
            // Linux bugfix:某些平台输入文件名不会自动补全.sbid后缀名,这里判断一下手动补上
            if (string.IsNullOrEmpty(result) || result.EndsWith("." + suffix))
                return result;
            return result + "." + suffix;
        }

        // 渲染并保存图片：传入文件全路径、要渲染的ItemsControl
        // todo 做成全局菜单栏里保存的方式
        public static void RenderImage(string path, ItemsControl itemsControl)
        {
            // 直接关闭导出窗口时path为空
            if (path == null)
                return;
            // 构造渲染图片的容器
            PixelSize pixelSize = new PixelSize((int)itemsControl.Width * 2, (int)itemsControl.Height * 2);
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(pixelSize, new Vector(192, 192));

            // 在可视树上找到这个ItemsControl的的孩子ItemsPresenter
            IEnumerator<IVisual> panelChildren = itemsControl.GetVisualChildren().GetEnumerator();
            panelChildren.MoveNext();
            // ListBox > Border > ScrollViewer > Grid > ScrollContentPresenter > ItemsPresenter
            if (itemsControl is ListBox)
            {
                Border border = (Border)panelChildren.Current;
                panelChildren = border.GetVisualChildren().GetEnumerator();
                panelChildren.MoveNext();
                ScrollViewer scrollViewer = (ScrollViewer)panelChildren.Current;
                panelChildren = scrollViewer.GetVisualChildren().GetEnumerator();
                panelChildren.MoveNext();
                Grid grid = (Grid)panelChildren.Current;
                panelChildren = grid.GetVisualChildren().GetEnumerator();
                panelChildren.MoveNext();
                ScrollContentPresenter scrollContentPresenter = (ScrollContentPresenter)panelChildren.Current;
                panelChildren = scrollContentPresenter.GetVisualChildren().GetEnumerator();
                panelChildren.MoveNext();
            }
            // ItemsControl > Border > ItemsPresenter
            else
            {
                Border border = (Border)panelChildren.Current;
                panelChildren = border.GetVisualChildren().GetEnumerator();
                panelChildren.MoveNext();
            }
            ItemsPresenter itemsPresenter = (ItemsPresenter)panelChildren.Current;

            // 渲染并保存
            renderTargetBitmap.Render(itemsPresenter);
            renderTargetBitmap.Save(path);

            mainWindowVM.Tips = "导出图片至：" + path;
        }

        #endregion

        #region 写XML的一些接口(项目保存)

        public static void writeAttribute(XmlTextWriter xmlWriter, Attribute attr)
        {
            xmlWriter.WriteAttributeString("type_ref", attr.Type.Id.ToString());
            xmlWriter.WriteAttributeString("identifier", attr.Identifier);
            xmlWriter.WriteAttributeString("isArray", attr.IsArray.ToString());
            xmlWriter.WriteAttributeString("len", attr.Len);
            xmlWriter.WriteAttributeString("id", attr.Id.ToString());
        }

        public static void writeInstance(XmlTextWriter xmlWriter, Instance instance)
        {
            xmlWriter.WriteStartElement("Instance");
            xmlWriter.WriteAttributeString("type_ref", instance.Type.Id.ToString());
            xmlWriter.WriteAttributeString("identifier", instance.Identifier);
            xmlWriter.WriteAttributeString("isArray", instance.IsArray.ToString());
            if (instance is ValueInstance)
            {
                ValueInstance valueInstance = (ValueInstance)instance;
                xmlWriter.WriteAttributeString("value", valueInstance.Value);
            }
            else if (instance is ArrayInstance)
            {
                ArrayInstance arrayInstance = (ArrayInstance)instance;
                foreach (Instance inst in arrayInstance.ArrayItems)
                {
                    writeInstance(xmlWriter, inst);
                }
            }
            else if (instance is ReferenceInstance)
            {
                ReferenceInstance referenceInstance = (ReferenceInstance)instance;
                foreach (Instance inst in referenceInstance.Properties)
                {
                    writeInstance(xmlWriter, inst);
                }
            }
            xmlWriter.WriteEndElement();
        }

        #endregion

        #region 读XML的一些接口(项目读取)

        public static Instance readInstance(XmlNode node, Dictionary<int, Type> typeDict)
        {
            XmlElement element = (XmlElement)node;
            int typeRef = int.Parse(element.GetAttribute("type_ref"));
            Debug.Assert(typeDict.ContainsKey(typeRef));
            string identifier = element.GetAttribute("identifier");
            bool isArray = bool.Parse(element.GetAttribute("isArray"));
            // 使用临时Attribute来构造Instance
            Attribute attribute = new Attribute(
                typeDict[typeRef],
                identifier,
                isArray,
                true
            );
            if (isArray) // 数组类型，继续解析数组项
            {
                ArrayInstance arrayInstance = new ArrayInstance(attribute);
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    Instance itemInstance = readInstance(childNode, typeDict);
                    arrayInstance.ArrayItems.Add(itemInstance);
                }
                return arrayInstance;
            }
            else if (typeDict[typeRef] is UserType) // 引用类型，继续解析成员
            {
                ReferenceInstance referenceInstance = new ReferenceInstance(attribute);
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    Instance propInstance = readInstance(childNode, typeDict);
                    referenceInstance.Properties.Add(propInstance);
                }
                return referenceInstance;
            }
            else // 值类型，要把值读出来
            {
                ValueInstance valueInstance = new ValueInstance(attribute);
                valueInstance.Value = element.GetAttribute("value");
                return valueInstance;
            }
        }

        #endregion

        #region 写XML的一些接口（保存为验证用的XML）

        public static void writeAttribute2(XmlTextWriter xmlWriter, Attribute attr)
        {
            xmlWriter.WriteAttributeString("type", attr.Type.Name);
            xmlWriter.WriteAttributeString("identifier", attr.Identifier);
            xmlWriter.WriteAttributeString("isArray", attr.IsArray.ToString());
            xmlWriter.WriteAttributeString("len", attr.Len);
            // xmlWriter.WriteAttributeString("id", attr.Id.ToString());
        }

        #endregion
    }
}
