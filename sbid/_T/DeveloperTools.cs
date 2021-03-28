using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace sbid._T
{
    public class DeveloperTools
    {
        private static readonly string DevLogFilePath = "C:\\Users\\LauZyHou\\Desktop\\MyLog.txt";

        /// <summary>
        /// 输出开发者日志，用于输出调试
        /// </summary>
        /// <param name="content">日志内容</param>
        public static void DevLog(string content)
        {
            if (!File.Exists(DevLogFilePath))
            {
                ResourceManager.mainWindowVM.Tips = "不存在的开发者日志";
                return;
            }
            TextWriter textWriter = new StreamWriter(DevLogFilePath, true);
            textWriter.WriteLine($"-------{DateTime.Now:yyyy-MM-dd HH:mm:ss}-------");
            textWriter.WriteLine(content);
            textWriter.Close();
        }
    }
}
