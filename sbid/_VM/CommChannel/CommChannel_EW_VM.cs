using ReactiveUI;
using sbid._M;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    public class CommChannel_EW_VM : ViewModelBase
    {
        private CommChannel commChannel;
        private ObservableCollection<Process> processes = new ObservableCollection<Process>();

        private ObservableCollection<CommMethodPair> commMethodPairs = new ObservableCollection<CommMethodPair>();

        // 要编辑CommChannel
        public CommChannel CommChannel { get => commChannel; set => commChannel = value; }
        // 集成协议下的所有Process,以用于CommDomain下CommMethodPair的构建和修改
        public ObservableCollection<Process> Processes { get => processes; set => processes = value; }

        // CommMethodPair列表绑定此处
        public ObservableCollection<CommMethodPair> CommMethodPairs { get => commMethodPairs; set => this.RaiseAndSetIfChanged(ref commMethodPairs, value); }
    }
}
