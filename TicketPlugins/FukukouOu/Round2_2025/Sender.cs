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
            var t1_obj = Connector.Round2_2025_JSONData_REQ_Gen(0, t1, index);
            var t2_obj = Connector.Round2_2025_JSONData_REQ_Gen(1, t2, index);
            var t3_obj = Connector.Round2_2025_JSONData_REQ_Gen(2, t3, index);
            Round2_2025_JSONData_REQ letObj = t1_obj;
            if (t2_obj.current_phase_PLUS != 0)
            {
                letObj = t2_obj;
            }
            if (t3_obj.current_phase_PLUS != 0)
            {
                letObj = t3_obj;
            }
            return Connector.Send_R2_2025_Result(letObj,index);
        }
        public int Get_NextRound()
        {
            return Connector.Get_NextRound_R2_2025();
        }
    }
}
