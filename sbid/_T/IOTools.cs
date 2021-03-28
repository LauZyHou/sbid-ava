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
            if (!File.Exists(filePath))
            {
                if (createIfNotExist)
                {
                    File.Create(filePath);
                }
                else
                {
                    return false;
                }
            }
            TextWriter textWriter = new StreamWriter(filePath, append);
            if (newLine)
            {
                textWriter.WriteLine(content);
            }
            else
            {
                textWriter.Write(content);
            }
            textWriter.Flush();
            textWriter.Close();
            return true;
        }
    }
}
