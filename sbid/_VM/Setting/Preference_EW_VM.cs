using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace sbid._VM
{
    public class Preference_EW_VM : ViewModelBase
    {
        private readonly string platformStr;
        private string projectSavePath;
        private string proVerifPath;
        private string beaglePath;

        public Preference_EW_VM()
        {
            platformStr = RuntimeInformation.OSDescription;
            projectSavePath = ResourceManager.projectSavePath;
            proVerifPath = ResourceManager.proVerifPath;
            beaglePath = ResourceManager.beaglePath;
        }

        // 运行平台
        public string PlatformStr { get => platformStr; }
        // 项目保存路径
        public string ProjectSavePath
        {
            get => projectSavePath;
            set
            {
                this.RaiseAndSetIfChanged(ref projectSavePath, value);
                ResourceManager.projectSavePath = value;
            }
        }
        // ProVerif可执行文件所在位置
        public string ProVerifPath
        {
            get => proVerifPath;
            set
            {
                this.RaiseAndSetIfChanged(ref proVerifPath, value);
                ResourceManager.proVerifPath = value;
            }
        }
        // Beagle可执行文件所在位置
        public string BeaglePath
        {
            get => beaglePath;
            set
            {
                this.RaiseAndSetIfChanged(ref beaglePath, value);
                ResourceManager.beaglePath = value;
            }
        }
        // 开发者模式
        public bool? DevMode
        {
            get
            {
                return ResourceManager.devMode;
            }
            set
            {
                bool val = (bool)value;
                if (val)
                {
                    ResourceManager.mainWindowVM.Tips = "打开开发者模式";
                }
                else
                {
                    ResourceManager.mainWindowVM.Tips = "关闭开发者模式";
                }
                ResourceManager.devMode = val;
            }
        }

        #region 按钮命令

        // 编辑项目保存路径
        private async void EditProjectSavePath()
        {
            string openFileName = await GetOpenFileName_sbid();
            if (string.IsNullOrEmpty(openFileName))
            {
                ResourceManager.mainWindowVM.Tips = "取消设置项目保存路径";
                return;
            }
            ProjectSavePath = openFileName;
            ResourceManager.mainWindowVM.Tips = "设置项目保存路径，至：" + openFileName;
        }

        // 编辑ProVerif可执行文件位置
        private async void EditProVerifPath()
        {
            string openFileName = await GetOpenFileName_All();
            if (string.IsNullOrEmpty(openFileName))
            {
                ResourceManager.mainWindowVM.Tips = "取消设置ProVerif可执行文件位置";
                return;
            }
            ProVerifPath = openFileName;
            ResourceManager.mainWindowVM.Tips = "设置ProVerif可执行文件位置，至：" + openFileName;
        }


        // 编辑Beagle可执行文件位置
        private async void EditBeaglePath()
        {
            string openFileName = await GetOpenFileName_All();
            if (string.IsNullOrEmpty(openFileName))
            {
                ResourceManager.mainWindowVM.Tips = "取消设置Beagle可执行文件位置";
                return;
            }
            BeaglePath = openFileName;
            ResourceManager.mainWindowVM.Tips = "设置Beagle可执行文件位置，至：" + openFileName;
        }

        #endregion

        #region 私有

        // 预打开文件：返回文件路径，用于寻找sbid项目文件
        private async Task<string> GetOpenFileName_sbid()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filters.Add(new FileDialogFilter() { Name = "项目文件", Extensions = { "sbid" } });
            string[] result = await dialog.ShowAsync(ResourceManager.mainWindowV);
            return result == null ? "" : string.Join(" ", result); // Linux bugfix:直接关闭时不能返回null
        }

        // 预打开文件：返回文件路径，用于寻找所有类型的文件，如ProVerif可执行文件
        private async Task<string> GetOpenFileName_All()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            string[] result = await dialog.ShowAsync(ResourceManager.mainWindowV);
            return result == null ? "" : string.Join(" ", result); // Linux bugfix:直接关闭时不能返回null
        }

        #endregion
    }
}
