using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FukukouOuDtConnector
{
    /// <summary>
    /// デフォルトScoreコンフィグJSON
    /// </summary>
    public class Round1_I_ScoreConfig_JSONData
    {
        public int index { get; set; }
        public int correct { get; set; }
        public int miss { get; set; }
        public int ask_throw { get; set; }
    }
}
