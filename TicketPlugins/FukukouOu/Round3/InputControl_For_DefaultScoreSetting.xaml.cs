using Gayak.Collections;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
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

namespace TRS.TMS12.TicketPlugins.FukukouOu.Round3
{
    public class InputControl_For_DefaultScoreSetting_ViewModel: BindableBase
    {
        private PluginInfo_For_DefaultScoreSetting m;

        private Visibility _Visibility;
        public Visibility Visibility
        {
            get { return _Visibility; }
            set { SetProperty(ref _Visibility, value); }
        }

        private InputControlTextBox_For_DefaultScoreSetting _CurrentTextBox;
        public InputControlTextBox_For_DefaultScoreSetting CurrentTextBox
        {
            get { return _CurrentTextBox; }
            set { SetProperty(ref _CurrentTextBox, value); m.CurrentTextBox = (int)CurrentTextBox; }
        }

        public ObservableDictionary<InputControlTextBox_For_DefaultScoreSetting, string> TextBoxes { get; set; }

        public DelegateCommand TextChanged { get; private set; }

        public DelegateCommand SideMenu { get; private set; }

        public DelegateCommand<string> GotFocus { get; private set; }
        public InputControl_For_DefaultScoreSetting_ViewModel(PluginInfo_For_DefaultScoreSetting m)
        {
            this.m = m;
            this.m.PropertyChanged += new PropertyChangedEventHandler((sender, e) =>
            {
                switch (e.PropertyName)
                {
                    case nameof(m.PluginHost):
                        ((BindableBase)this.m.PluginHost).PropertyChanged += new PropertyChangedEventHandler((hsender, he) =>
                        {
                            switch (he.PropertyName)
                            {
                                case nameof(m.PluginHost.CurrentTicket):
                                    Visibility = m.PluginHost.CurrentTicket == this.m ? Visibility.Visible : Visibility.Hidden;
                                    break;
                            }
                        });
                        break;

                    case nameof(m.CurrentTextBox):
                        CurrentTextBox = (InputControlTextBox_For_DefaultScoreSetting)m.CurrentTextBox;
                        break;
                }
            });

            m.TextBoxes.CollectionChanged += new NotifyCollectionChangedEventHandler((sender, e) =>
            {
                try
                {
                    TextBoxes[(InputControlTextBox_For_DefaultScoreSetting)e.OldStartingIndex] = m.TextBoxes[e.NewStartingIndex];
                }
                catch { }
            });

            TextBoxes = new ObservableDictionary<InputControlTextBox_For_DefaultScoreSetting, string>();
            foreach (InputControlTextBox_For_DefaultScoreSetting i in Enum.GetValues(typeof(InputControlTextBox_For_DefaultScoreSetting)))
            {
                TextBoxes.Add(i, "");
            }

            TextBoxes.CollectionChanged += new NotifyCollectionChangedEventHandler((sender, e) =>
            {
                try
                {
                    KeyValuePair<InputControlTextBox_For_DefaultScoreSetting, string> textBox = TextBoxes.First(t => m.TextBoxes[(int)t.Key] != t.Value);
                    m.TextBoxes[(int)textBox.Key] = textBox.Value;
                }
                catch { }

            });

            SideMenu = new DelegateCommand(() =>
            {
                m.PluginHost.GoToSideMenu();
            });

            GotFocus = new DelegateCommand<string>(param =>
            {
                CurrentTextBox = (InputControlTextBox_For_DefaultScoreSetting)Enum.Parse(typeof(InputControlTextBox_For_DefaultScoreSetting), param);
            });
        }
    }
    /// <summary>
    /// InputControl_For_DefaultScoreSetting.xaml の相互作用ロジック
    /// </summary>
    public partial class InputControl_For_DefaultScoreSetting : UserControl
    {
        public InputControl_For_DefaultScoreSetting_ViewModel vm;
        public InputControl_For_DefaultScoreSetting(InputControl_For_DefaultScoreSetting_ViewModel vm)
        {
            this.vm = vm;
            InitializeComponent();
            DataContext = this.vm;

            this.vm.PropertyChanged += new PropertyChangedEventHandler((sender, e) =>
            {
                switch (e.PropertyName)
                {
                    case nameof(this.vm.CurrentTextBox):
                        TextBox textBox = FindName(this.vm.CurrentTextBox.ToString()) as TextBox;
                        if (textBox != null)
                        {
                            textBox.Focus();
                        }
                        break;
                }
            });
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
