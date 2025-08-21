using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TRS.TMS12.TicketPlugins.FukukouOu.Round2
{
    public class KeyControl_For_DefaultScoreSettingViewModel : BindableBase
    {
        private PluginInfo_For_DefaultScoreSetting m;

        public Resources.KeyControlViewModel KeyBaseViewModel { get; } = new Resources.KeyControlViewModel();

        private Visibility _Visibility;
        public Visibility Visibility
        {
            get { return _Visibility; }
            set { SetProperty(ref _Visibility, value); }
        }

        public KeyControl_For_DefaultScoreSettingViewModel(PluginInfo_For_DefaultScoreSetting m)
        {
            this.m = m;
            this.m.PropertyChanged += (sender2, e2) =>
            {
                switch (e2.PropertyName)
                {
                    case nameof(m.PluginHost):
                        ((BindableBase)this.m.PluginHost).PropertyChanged += (sender, e) =>
                        {
                            switch (e.PropertyName)
                            {
                                case nameof(m.PluginHost.CurrentTicket):
                                    Visibility = m.PluginHost.CurrentTicket == this.m ? Visibility.Visible : Visibility.Hidden;
                                    break;
                            }
                        };
                        break;

                    case nameof(m.KeyBaseModel):
                        KeyBaseViewModel.M = m.KeyBaseModel;
                        break;
                }
            };
        }
    }

    /// <summary>
    /// KeyControl_For_DefaultScoreSetting.xaml の相互作用ロジック
    /// </summary>
    public partial class KeyControl_For_DefaultScoreSetting
    {
        public KeyControl_For_DefaultScoreSettingViewModel vm;

        public KeyControl_For_DefaultScoreSetting(KeyControl_For_DefaultScoreSettingViewModel vm)
        {
            InitializeComponent();
            this.vm = vm;
            DataContext = this.vm;
            Base.DataContext = this.vm.KeyBaseViewModel;
        }
    }
}
