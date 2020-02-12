using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using sbid._M;

namespace sbid._V
{
    public class SequenceDiagram_P_V : UserControl
    {
        public SequenceDiagram_P_V()
        {
            this.InitializeComponent();

            init_binding();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #region 辅助构造

        // 初始化.cs文件中的数据绑定,一些不方便在xaml中绑定的部分在这里绑定
        private void init_binding()
        {
            // 绑定SeqMessage的InOut枚举
            ComboBox seqMessage_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "seqMessage_ComboBox");
            seqMessage_ComboBox.Items = System.Enum.GetValues(typeof(SeqMessage));
        }

        #endregion
    }
}
