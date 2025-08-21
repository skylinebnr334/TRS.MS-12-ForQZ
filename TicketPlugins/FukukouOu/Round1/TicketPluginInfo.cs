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

namespace TRS.TMS12.TicketPlugins.FukukouOu.Round1
{

    public enum InputControlTextBox
    {
        Stage=0,
        CLS1,
        CLS2,
        CLS3,
        CLS4,
        CLS5,
        CLS6
    }
    public class PluginInfo:BindableBase, ITicketPluginExtension
    {
        public readonly string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public string TicketName { get; } = "Round 1";


        private IPluginHost _PluginHost;

        public IPluginHost PluginHost
        {
            get => _PluginHost;
            set
            {
                SetProperty(ref _PluginHost, value);

                List<KeyTab> tabs = new List<KeyTab>()
                {
                    PluginHost.CreateKeyTabFromFile(Path.Combine(assemblyPath, @"KeyLayoutDefinitions\FullKey.xml")),
                    PluginHost.CreateKeyTabFromFile(Path.Combine(assemblyPath, @"KeyLayoutDefinitions\Number.xml")),
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
        private IEnumerable<InputControlTextBox> textBoxKeys = (IEnumerable<InputControlTextBox>)Enum.GetValues(typeof(InputControlTextBox));

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

        public PluginInfo()
        {
            TextBoxes = new ObservableCollection<string>();
            int length = Enum.GetValues(typeof(InputControlTextBox)).Length;
            for (int i = 0; i < length; i++)
            {
                TextBoxes.Add("");
            }

            commandParser = new CommandParser(this, typeof(InputControlTextBox));

            InputControlViewModel inputControlViewModel = new InputControlViewModel(this);
            KeyControlViewModel keyControlViewModel = new KeyControlViewModel(this);

            InputControl = new InputControl(inputControlViewModel);
            KeyControl = new KeyControl(keyControlViewModel);

            Sender = new Sender(this);

            TextBoxes.CollectionChanged += (sender, e) =>
            {
                /*bool canSend =
                    TextBoxes[(int)InputControlTextBox.Month] != "" &&
                    TextBoxes[(int)InputControlTextBox.Day] != "" &&

                    (TextBoxes[(int)InputControlTextBox.Adult] != "" ||
                    TextBoxes[(int)InputControlTextBox.Child] != "");*/
                bool canSend = true;

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
            foreach (InputControlTextBox key in textBoxKeys) TextBoxes[(int)key] = "";

            KeyBaseModel.CurrentTab = 0;
            CurrentTextBox = (int)InputControlTextBox.Stage;

            if (option == "")
            {
                /*DateTime today = DateTime.Today;
                TextBoxes[(int)InputControlTextBox.Adult] = "１";
                TextBoxes[(int)InputControlTextBox.Month] = Strings.StrConv(today.Month.ToString(), VbStrConv.Wide);
                TextBoxes[(int)InputControlTextBox.Day] = Strings.StrConv(today.Day.ToString(), VbStrConv.Wide);*/
                TextBoxes[(int)InputControlTextBox.CLS1] = "ム";
                TextBoxes[(int)InputControlTextBox.CLS2] = "ム";
                TextBoxes[(int)InputControlTextBox.CLS3] = "ム";
                TextBoxes[(int)InputControlTextBox.CLS4] = "ム";
                TextBoxes[(int)InputControlTextBox.CLS5] = "ム";
                TextBoxes[(int)InputControlTextBox.CLS6] = "ム";
                TextBoxes[(int)InputControlTextBox.Stage] = Strings.StrConv(((Sender)Sender).Get_NextRound().ToString(), VbStrConv.Wide);


            }
            else
            {
                commandParser.Parse(option);
            }
            }

        public void ClearFocusedAndAfter()
        {
            InputControlTextBox[] textBoxes = (InputControlTextBox[])Enum.GetValues(typeof(InputControlTextBox));
            foreach (InputControlTextBox textBox in textBoxes)
            {
                if (textBox >= (InputControlTextBox)CurrentTextBox) TextBoxes[(int)textBox] = "";
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
