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
        // 生成UPPAAL文件的目录
        private static readonly string UPPAALProductsPath = Path.Combine(".", "Products", "UPPAAL");

        /// <summary>
        /// 将当前模型翻译到UPPAAL
        /// </summary>
        public static void Translate()
        {
            // 当前的协议模型
            Protocol_VM currentProtocol_VM = ResourceManager.mainWindowVM.SelectedItem;
            // 如果在工具中没有建立协议模型，那么就给出提示并不做转换
            if (currentProtocol_VM is null)
            {
                ResourceManager.mainWindowVM.Tips = "无可用的协议模型！";
                return;
            }
            // 要生成到的文件
            string filePath = Path.Combine(UPPAALProductsPath, currentProtocol_VM.Protocol.Name + ".xml");
            // 创建生成文件
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }
            // 从UPPAAL顶层结构生成XML
            WriteRoot(filePath);
            // 将UPPAAL的开头部分插入进去，这里先读出文件里的所有内容
            // 再重新添加两行XML头，然后再把这块内容加入进去
            string xmlContent = File.ReadAllText(filePath);
            IOTools.WriteToFile(filePath, @"<?xml version=""1.0"" encoding=""utf-8""?>", false, true, true);
            IOTools.WriteToFile(filePath, @"<!DOCTYPE nta PUBLIC '-//Uppaal Team//DTD Flat System 1.1//EN' 'http://www.it.uu.se/research/group/darts/uppaal/flat-1_2.dtd'>", true, true, false);
            IOTools.WriteToFile(filePath, xmlContent, true, true, false);
            // 转换结束后，给出提示 todo: 转换为绝对路径
            ResourceManager.mainWindowVM.Tips = $"转换为UPPAAL模型：{filePath}";
        }

        #region 私有

        /// <summary>
        /// 从UPPAAL顶层结构<nta>生成XML
        /// </summary>
        /// <param name="filePath">要写入的XML文件路径</param>
        private static void WriteRoot(string filePath)
        {
            XmlTextWriter xmlWriter = new XmlTextWriter(filePath, null);
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.WriteStartElement("nta");
            WriteTopDeclaration(xmlWriter); // 顶层<declaration>
            WriteAllTemplates(xmlWriter); // 顶层若干<template>
            WriteSystem(xmlWriter); // 顶层<system>
            WriteQueries(xmlWriter); // 顶层<queries>
            xmlWriter.WriteEndElement();
            xmlWriter.Flush();
            xmlWriter.Close();
        }

        /// <summary>
        /// 生成UPPAAL顶层<declaration>结构
        /// </summary>
        /// <param name="xmlWriter">XML写入器</param>
        private static void WriteTopDeclaration(XmlWriter xmlWriter)
        {
            xmlWriter.WriteStartElement("declaration");

            xmlWriter.WriteEndElement();
        }

        /// <summary>
        /// 生成UPPAAL顶层的若干<template>结构
        /// </summary>
        /// <param name="xmlWriter">XML写入器</param>
        private static void WriteAllTemplates(XmlWriter xmlWriter)
        {

        }

        /// <summary>
        /// 生成UPPAAL顶层的某个<template>结构
        /// </summary>
        /// <param name="xmlWriter">XML写入器</param>
        private static void WriteTemplate(XmlWriter xmlWriter)
        {
            xmlWriter.WriteStartElement("template");

            xmlWriter.WriteEndElement();
        }

        /// <summary>
        /// 生成UPPAAL顶层的<system>结构
        /// </summary>
        /// <param name="xmlWriter">XML写入器</param>
        private static void WriteSystem(XmlWriter xmlWriter)
        {
            xmlWriter.WriteStartElement("system");

            xmlWriter.WriteEndElement();
        }

        /// <summary>
        /// 生成UPPAAL顶层的<queries>结构
        /// </summary>
        /// <param name="xmlWriter">XML写入器</param>
        private static void WriteQueries(XmlWriter xmlWriter)
        {
            xmlWriter.WriteStartElement("queries");

            xmlWriter.WriteEndElement();
        }

        #endregion
    }
}
