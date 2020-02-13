using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    public class Attack : ReactiveObject
    {
        private string content;

        public Attack(string content)
        {
            this.content = content;
        }

        public string Content { get => content; set => this.RaiseAndSetIfChanged(ref content, value); }
    }
}
