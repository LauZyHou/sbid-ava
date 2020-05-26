using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    // 原子命题
    public class AtomProposition : ReactiveObject
    {
        public static int _id = 0;
        private Formula refName;
        private int id;

        public AtomProposition()
        {
            _id++;
            this.id = _id;
            this.refName = new Formula("AP" + this.id);
        }

        public Formula RefName { get => refName; }

        public int Id
        {
            get => id;
            set
            {
                id = value;
                if (value > _id)
                    _id = value;
            }
        }
    }
}
