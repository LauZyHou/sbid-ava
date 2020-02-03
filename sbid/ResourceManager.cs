using sbid._V;
using sbid._VM;

namespace sbid
{
    public class ResourceManager
    {
        // 保存主窗体ViewModel,用于在全局任何位置都能直接获取
        public static MainWindow_VM mainWindowVM;
        // 保存主窗体View,用于在全局任何位置都能直接获取
        public static MainWindow_V mainWindowV;
    }
}
