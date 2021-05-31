using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace sbid._T
{
    public class IOTools
    {
        /// <summary>
        /// 将内容写入指定的文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="content">写入内容</param>
        /// <param name="append">追加模式</param>
        /// <param name="newLine">新行模式</param>
        /// <param name="createIfNotExist">如果文件不存在就创建</param>
        /// <returns>是否写入成功</returns>
        public static bool WriteToFile(string filePath, string content, bool append, bool newLine, bool createIfNotExist)
        {
            // 如果要写入的文件不存在
            if (!File.Exists(filePath))
            {
                // 如果指定了不存在时创建，那么就将文件创建出来
                if (createIfNotExist)
                {
                    File.Create(filePath);
                }
                // 否则，无法继续向下处理
                else
                {
                    return false;
                }
            }
            // 至此，要写入的文件一定存在，创建一个文本写入器，并将是否追加模式在这里植入
            TextWriter textWriter = new StreamWriter(filePath, append);
            // 创建新行并写入
            if (newLine)
            {
                textWriter.WriteLine(content);
            }
            // 不创建新行，直接写入
            else
            {
                textWriter.Write(content);
            }
            // 写完了，刷新缓冲区并关闭文件
            textWriter.Flush();
            textWriter.Close();
            return true;
        }
    }
}
