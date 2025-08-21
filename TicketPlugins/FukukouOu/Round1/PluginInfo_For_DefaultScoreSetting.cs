using FukukouOuDtConnector;
using Microsoft.VisualBasic;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TRS.TMS12.Interfaces;
using TRS.TMS12.Resources;
using TRS.TMS12.Static;
using static TRS.TMS12.Static.App;

namespace TRS.TMS12.TicketPlugins.FukukouOu.Round1
{

    public enum InputControlTextBox_For_DefaultScoreSetting
    {
        CORRECT=0,
        MISS,
        ASK_THROW
    }
    public class PluginInfo_For_DefaultScoreSetting : BindableBase, ITicketPluginExtension
    {
        public readonly string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public string TicketName { get; } = "Round 1 デフォルト得点設定";

        private IPluginHost _PluginHost;

        public IPluginHost PluginHost
        {
            get => _PluginHost;
            set
            {
                SetProperty(ref _PluginHost, value);

                List<KeyTab> tabs = new List<KeyTab>()
                {
                    PluginHost.CreateKeyTabFromFile(Path.Combine(assemblyPath, @"KeyLayoutDefinitions\Number_For_DefaultScoreSetting.xml")),
                    new KeyTab("", null),
                    new KeyTab("", null),
                    new KeyTab("", null),
                };
                KeyBaseModel = new KeyControlModel(tabs, new DelegateCommand<string>(ButtonClicked));
            }
        }

        public ISender Sender { get; private set; }

        public UserControl InputControl { get; private set; }
        public UserControl KeyControl { get; private set; }

        public ObservableCollection<bool> FunctionKeysIsEnabled { get; private set; } = new ObservableCollection<bool>()
        {
            true,
            true, true, true, true, false, false, true, false, true, true, true, true, true, true, true,
            true, true, true, true,
            true, true, false,
        };

        private CommandParser commandParser;
        private IEnumerable<InputControlTextBox_For_DefaultScoreSetting> textBoxKeys = (IEnumerable<InputControlTextBox_For_DefaultScoreSetting>)Enum.GetValues(typeof(InputControlTextBox_For_DefaultScoreSetting));

        private KeyControlModel _KeyBaseModel;
        public ObservableCollection<string> TextBoxes { get; set; }

        private int _CurrentTextBox = 0;
        public int CurrentTextBox
        {
            get => _CurrentTextBox;
            set => SetProperty(ref _CurrentTextBox, value);
        }

        public KeyControlModel KeyBaseModel
        {
            get => _KeyBaseModel;
            set => SetProperty(ref _KeyBaseModel, value);
        }
        public PluginInfo_For_DefaultScoreSetting()
        {
            TextBoxes = new ObservableCollection<string>();
            int length = Enum.GetValues(typeof(InputControlTextBox_For_DefaultScoreSetting)).Length;
            for (int i = 0; i < length; i++)
            {
                TextBoxes.Add("");
            }

            commandParser = new CommandParser(this, typeof(InputControlTextBox_For_DefaultScoreSetting));

            InputControl_For_DefaultScoreSetting_ViewModel inputControlViewModel = new InputControl_For_DefaultScoreSetting_ViewModel(this);
            KeyControl_For_DefaultScoreSettingViewModel keyControlViewModel = new KeyControl_For_DefaultScoreSettingViewModel(this);

            InputControl = new InputControl_For_DefaultScoreSetting(inputControlViewModel);
            KeyControl = new KeyControl_For_DefaultScoreSetting(keyControlViewModel);
            Sender = new Sender_For_DefaultScoreSetting(this);

            TextBoxes.CollectionChanged += (sender, e) =>
            {
                bool canSend = true;
                if (TextBoxes[(int)InputControlTextBox_For_DefaultScoreSetting.CORRECT] == "" ||
                TextBoxes[(int)InputControlTextBox_For_DefaultScoreSetting.MISS] == "" ||
                TextBoxes[(int)InputControlTextBox_For_DefaultScoreSetting.ASK_THROW] == "")
                    canSend = false;
                FunctionKeysIsEnabled[(int)FunctionKeys.Send] = canSend;

            };
        }


        public void AddOption(string value)
        {
            throw new NotSupportedException();
        }

        public void SetDiscount(string value)
        {
            throw new NotSupportedException();
        }

        public void SetDefault(string option = "")
        {
            foreach (InputControlTextBox_For_DefaultScoreSetting key in textBoxKeys) TextBoxes[(int)key] = "";

            KeyBaseModel.CurrentTab = 0;
            CurrentTextBox = (int)InputControlTextBox_For_DefaultScoreSetting.CORRECT;

            if (option == "")
            {
                TextBoxes[(int)InputControlTextBox_For_DefaultScoreSetting.CORRECT] = "１";
                TextBoxes[(int)InputControlTextBox_For_DefaultScoreSetting.MISS] = "４";
                TextBoxes[(int)InputControlTextBox_For_DefaultScoreSetting.ASK_THROW] = "０";
                Round1_I_ScoreConfig_JSONData defaultDt = ((Sender_For_DefaultScoreSetting)Sender).get_DefaultScoreConfig();

                TextBoxes[(int)InputControlTextBox_For_DefaultScoreSetting.CORRECT] = IntToWideString(defaultDt.correct);
                TextBoxes[(int)InputControlTextBox_For_DefaultScoreSetting.MISS] = IntToWideString(defaultDt.miss*-1);
                TextBoxes[(int)InputControlTextBox_For_DefaultScoreSetting.ASK_THROW] = IntToWideString(defaultDt.ask_throw*-1);


            }
            else
            {
                commandParser.Parse(option);
            }
        }
        public void ClearFocusedAndAfter()
        {
            InputControlTextBox_For_DefaultScoreSetting[] textBoxes = (InputControlTextBox_For_DefaultScoreSetting[])Enum.GetValues(typeof(InputControlTextBox_For_DefaultScoreSetting));
            foreach (InputControlTextBox_For_DefaultScoreSetting textBox in textBoxes)
            {
                if (textBox >= (InputControlTextBox_For_DefaultScoreSetting)CurrentTextBox) TextBoxes[(int)textBox] = "";
            }
        }
        public void ButtonClicked(string param)
        {
            try
            {
                commandParser.Parse(param);
            }
            catch (Exception ex)
            {
                PluginHost.Dialog.ShowErrorDialog($"コマンドの実行に失敗しました。\n\n\n要求コマンド：\n\n{param}\n\n\n詳細：\n\n{ex}");
            }
        }
    }
}
