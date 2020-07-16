using sbid._M;
using sbid._VM;
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

        #region 进程模板

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

        #region 状态机

        /// <summary>
        /// 检查给出的状态名不会和状态机面板中的状态重名
        /// </summary>
        /// <param name="stateMachine_P_VM">要检查的状态机面板</param>
        /// <param name="ignoreState">检查时忽略的状态</param>
        /// <param name="name">给出的状态名</param>
        /// <returns>检查是否通过</returns>
        public static bool checkStateName(StateMachine_P_VM stateMachine_P_VM, State ignoreState, string name)
        {
            foreach (ViewModelBase vmb in stateMachine_P_VM.UserControlVMs)
            {
                if(vmb is State_VM)
                {
                    State_VM state_VM = (State_VM)vmb;
                    if (state_VM.State == ignoreState)
                        continue;
                    if (state_VM.State.Name == name)
                        return false;
                }
            }
            return true;
        }

        #endregion
    }
}
