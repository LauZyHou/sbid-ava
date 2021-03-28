using sbid._VM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace sbid._T
{
    public class UPPAALTranslator
    {
        private static readonly string UPPAALProductsPath = Path.Combine(".", "Products", "UPPAAL");

        /// <summary>
        /// 将当前模型翻译到UPPAAL
        /// </summary>
        public static void Translate()
        {
            // 当前的协议模型
            Protocol_VM currentProtocol_VM = ResourceManager.mainWindowVM.SelectedItem;
            // 要生成到的文件
            string filePath = Path.Combine(UPPAALProductsPath, currentProtocol_VM.Protocol.Name + ".xml");
            // 创建生成文件
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }
            // TODO 写入XML
            XmlTextWriter xmlWriter = new XmlTextWriter(filePath, null);
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.WriteStartElement("nta");
            xmlWriter.WriteEndElement();
            xmlWriter.Flush();
            xmlWriter.Close();
            // 将UPPAAL的开头部分插入进去，这里先读出文件里的所有内容
            // 再重新添加两行XML头，然后再把这块内容加入进去
            string xmlContent = File.ReadAllText(filePath);
            IOTools.WriteToFile(filePath, @"<?xml version=""1.0"" encoding=""utf-8""?>", false, true, true);
            IOTools.WriteToFile(filePath, @"<!DOCTYPE nta PUBLIC '-//Uppaal Team//DTD Flat System 1.1//EN' 'http://www.it.uu.se/research/group/darts/uppaal/flat-1_2.dtd'>", true, true, false);
            IOTools.WriteToFile(filePath, xmlContent, true, true, false);
        }
    }
}
