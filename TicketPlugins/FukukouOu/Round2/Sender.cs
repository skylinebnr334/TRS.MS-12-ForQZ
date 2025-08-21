using FukukouOuDtConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRS.TMS12.Interfaces;
using static TRS.TMS12.Static.App;

namespace TRS.TMS12.TicketPlugins.FukukouOu.Round2
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
            string t1 = m.TextBoxes[(int)InputControlTextBox.CLS1];
            string t2 = m.TextBoxes[(int)InputControlTextBox.CLS2];
            string t3 = m.TextBoxes[(int)InputControlTextBox.CLS3];
            string t4 = m.TextBoxes[(int)InputControlTextBox.CLS4];
            return Connector.Send_R2_Result(Connector.Round2_JSONData_DtSet(index,t1,t2,t3,t4));
        }
        public int Get_NextRound()
        {
            return Connector.Get_NextRound_R2();
        }
    }
}
