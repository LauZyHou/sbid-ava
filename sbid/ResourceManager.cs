using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Media.Imaging;
using Avalonia.VisualTree;
using sbid._M;
using sbid._V;
using sbid._VM;
using System.Collections.Generic;
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
            // 构造渲染图片的容器
            PixelSize pixelSize = new PixelSize((int)itemsControl.Width * 2, (int)itemsControl.Height * 2);
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(pixelSize, new Vector(192, 192));

            // 在可视树上找到这个ItemsControl的的孩子ItemsPresenter
            IEnumerator<IVisual> panelChildren = itemsControl.GetVisualChildren().GetEnumerator();
            panelChildren.MoveNext();
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
            xmlWriter.WriteAttributeString("id", attr.Id.ToString());
        }

        #endregion

        #region 读XML的一些接口(项目读取)

        #endregion
    }
}
