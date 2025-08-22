using FukukouOuDtConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRS.TMS12.Interfaces;
using static TRS.TMS12.Static.App;

namespace TRS.TMS12.TicketPlugins.FukukouOu.Round1_2025
{
    public class Sender : ISender
    {

        private PluginInfo m;

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

        public Sender(PluginInfo m)
        {
            this.m = m;
        }
        public SendResult Send()
        {

            int index = WideStringToInt(m.TextBoxes[(int)InputControlTextBox.Stage]);
            int next_Q = WideStringToInt(m.TextBoxes[(int)InputControlTextBox.NextQuestion])-1;
            string t1 = m.TextBoxes[(int)InputControlTextBox.CLS1];
            string t2 = m.TextBoxes[(int)InputControlTextBox.CLS2];
            string t3 = m.TextBoxes[(int)InputControlTextBox.CLS3];
            string t4 = m.TextBoxes[(int)InputControlTextBox.CLS4];
            string t5 = m.TextBoxes[(int)InputControlTextBox.CLS5];
            string t6 = m.TextBoxes[(int)InputControlTextBox.CLS6];
            int current_Q = Connector.Get_NextRound_R1_2025().current_question;
            bool isTwo = false;
            switch (current_Q)
            {

                case 31:
                case 17:
                case 33:
                case 34:
                case 20:
                case 13:
                case 6:
                    isTwo = true;
                    break;
            }
            return Connector.Send_R1_2025_Result(Connector.Round1_2025_JSONData_DtSet(index,t1,t2,t3,t4,t5,t6), next_Q,isTwo);
        }
        public SendResult SendPlay()
        {
            return Connector.Send_Play2025R1();
        }
        public SendResult SendStop()
        {
            return Connector.Send_Stop2025R1();
        }
        public Round1_2025_NextJsonData Get_NextRound()
        {
            return Connector.Get_NextRound_R1_2025();
        }
    }
}
