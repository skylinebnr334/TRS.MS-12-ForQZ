using FukukouOuDtConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRS.TMS12.Interfaces;
using static TRS.TMS12.Static.App;
namespace TRS.TMS12.TicketPlugins.FukukouOu.Round2_2025
{
    public class Sender_For_DefaultScoreSetting : ISender
    {
        private PluginInfo_For_DefaultScoreSetting m;

        private Connector _Connector = null;
        private Connector Connector
        {
            get
            {
                if (_Connector is null)
                {
                    _Connector = (Connector)m.PluginHost.Plugins.Find(p => p is Connector);
                }

                return _Connector;
            }
        }
        public Sender_For_DefaultScoreSetting(PluginInfo_For_DefaultScoreSetting m)
        {
            this.m = m;
        }
        public SendResult Send()
        {
            int correct = WideStringToInt(m.TextBoxes[(int)InputControlTextBox_For_DefaultScoreSetting.CORRECT]);
            int miss = WideStringToInt(m.TextBoxes[(int)InputControlTextBox_For_DefaultScoreSetting.MISS]) * -1;
            int ask_throw = WideStringToInt(m.TextBoxes[(int)InputControlTextBox_For_DefaultScoreSetting.ASK_THROW])*-1;
            var sendObj = new Round2_I_ScoreConfig_JSONData()
            {
                correct = correct,
                miss = miss,
                ask_throw = ask_throw,
                index = -810
            };
            return Connector.SendScoreConfigR2(sendObj);

            //return SendResult.No("Err");
        }
        public Round2_I_ScoreConfig_JSONData get_DefaultScoreConfig()
        {
            return Connector.get_DefaultScoreConfigR2();
        }

    }
}
