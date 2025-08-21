using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TRS.TMS12.Interfaces;
using static TRS.TMS12.Static.App;

namespace FukukouOuDtConnector
{

    public partial class Connector : IPlugin
    {
        private IPluginHost _PluginHost;
        public IPluginHost PluginHost
        {
            get => _PluginHost;
            set
            {
                _PluginHost = value;
                string server_url = "http://localhost:3000/Server1";
                string server_url2 = "http://localhost:3000/Server2";
                string server_url3 = "http://localhost:3000/Server3";
                try
                {

                    string path = Path.Combine(AppDirectory, "Settings", "FukukouOu.xml");
                    XElement connectorSetting = XDocument.Load(path).Element("FukukouOuSetting");

                    server_url = connectorSetting.Element("Round1").Element("ServerUrl").Attribute("Value").Value;
                    server_url2 = connectorSetting.Element("Round2").Element("ServerUrl").Attribute("Value").Value;
                    server_url3 = connectorSetting.Element("Round3").Element("ServerUrl").Attribute("Value").Value;
                }
                catch(Exception e)
                {
                    Console.Error.WriteLine(e.Message);
                }
                Initialize(server_url,server_url2,server_url3);
                //Initialize("http://localhost:810/Server1");
            }
        }
    }
}
