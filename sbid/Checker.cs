using sbid._M;
using System.Collections.ObjectModel;

namespace sbid
{
    public class Checker
    {
        #region 通用

        /// <summary>
        /// 检查所有Attribute的Identifier和给定的不重复
        /// </summary>
        /// <param name="attributes">要检查的Attribute列表</param>
        /// <param name="ignoreAttr">检查时忽略的Attribute</param>
        /// <param name="identifier">要检查的Identifier</param>
        /// <returns>检查是否通过</returns>
        public static bool checkAttributeIdentifier(ObservableCollection<Attribute> attributes, Attribute ignoreAttr, string identifier)
        {
            foreach (Attribute attr in attributes)
            {
                if (attr == ignoreAttr)
                    continue;
                if (attr.Identifier == identifier)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 检查所有Method的Name和给定的不重复
        /// </summary>
        /// <param name="methods">要检查的Method列表</param>
        /// <param name="ignoreMethod">检查时忽略的Method</param>
        /// <param name="name">要检查的Name</param>
        /// <returns>检查是否通过</returns>
        public static bool checkMethodName(ObservableCollection<Method> methods, Method ignoreMethod, string name)
        {
            foreach (Method method in methods)
            {
                if (method == ignoreMethod)
                    continue;
                if (method.Name == name)
                    return false;
            }
            return true;
        }

        #endregion


        #region Proces

        /// <summary>
        /// 检查所有CommMethod的Name和给定的不重复
        /// </summary>
        /// <param name="commMethods">要检查的CommMethod列表</param>
        /// <param name="ignoreCommMethod">检查时忽略的CommMethod</param>
        /// <param name="name">要检查的Name</param>
        /// <returns>检查是否通过</returns>
        public static bool checkCommMethodName(ObservableCollection<CommMethod> commMethods, CommMethod ignoreCommMethod, string name)
        {
            foreach (CommMethod commMethod in commMethods)
            {
                if (commMethod == ignoreCommMethod)
                    continue;
                if (commMethod.Name == name)
                    return false;
            }
            return true;
        }

        #endregion

    }
}
