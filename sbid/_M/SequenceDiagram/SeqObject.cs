using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    // 序列图"对象"
    public class SeqObject : ReactiveObject
    {
        private string objName;
        private string className;

        public SeqObject(string objName, string className)
        {
            this.objName = objName;
            this.className = className;
        }

        public string ObjName { get => objName; set => this.RaiseAndSetIfChanged(ref objName, value); }
        public string ClassName { get => className; set => this.RaiseAndSetIfChanged(ref className, value); }
    }
}
