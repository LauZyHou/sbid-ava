﻿using sbid._M;
using sbid._VM;
using System.Collections;
using System.Collections.ObjectModel;

namespace sbid
{
    public class Checker
    {
        #region 通用

        // 检查参数表中是否包含给定的名字
        public static bool ParamList_Contain_Name(IEnumerable paramlist, string name)
        {
            foreach (Attribute attribute in paramlist)
            {
                if (attribute.Identifier == name)
                    return true;
            }
            return false;
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

        #region UserType
        
        // 检查UserType中是否包含这个名字的内容
        public static bool UserType_Contain_PropName(UserType userType, string name)
        {
            foreach (Attribute attribute in userType.Attributes)
            {
                if (attribute.Identifier == name)
                    return true;
            }
            foreach (Method method in userType.Methods)
            {
                if (method.Name == name)
                    return true;
            }
            return false;
        }

        #endregion

        #region Process

        // 检查Process中是否包含这个名字的内容
        public static bool Process_Contain_PropName(Process process, string name)
        {
            foreach (Attribute attribute in process.Attributes)
            {
                if (attribute.Identifier == name)
                    return true;
            }
            foreach (Method method in process.Methods)
            {
                if (method.Name == name)
                    return true;
            }
            foreach (CommMethod commMethod in process.CommMethods)
            {
                if (commMethod.Name == name)
                    return true;
            }
            return false;
        }

        #endregion
    }
}
