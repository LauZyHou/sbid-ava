using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    // 攻击结点伴随的关系类型枚举：或、与、顺序与
    public enum AttackRelation
    {
        OR, AND, SAND
    }

    // 攻击和关系放在同一个结点上时，"攻击-关系"数据结构
    public class AttackWithRelation : ReactiveObject
    {
        private string description;
        private AttackRelation attackRelation = AttackRelation.OR;

        public AttackWithRelation(string description)
        {
            this.description = description;
        }

        // 攻击的文字描述
        public string Description { get => description; set => this.RaiseAndSetIfChanged(ref description, value); }
        // 关系类型
        public AttackRelation AttackRelation { get => attackRelation; set => this.RaiseAndSetIfChanged(ref attackRelation, value); }
    }
}
