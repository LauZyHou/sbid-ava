using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class ErrorBox_SW_VM : ViewModelBase
    {
        private string content;

        public string Content { get => content; set => this.RaiseAndSetIfChanged(ref content, value); }

        public ErrorBox_SW_VM()
        {
        }

        public ErrorBox_SW_VM(string content)
        {
            this.content = content;
        }
    }
}
