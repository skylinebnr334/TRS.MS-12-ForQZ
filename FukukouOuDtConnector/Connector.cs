using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TRS.TMS12.Interfaces;

namespace FukukouOuDtConnector
{
    public partial class Connector : IPlugin
    {
        private string s_url = "";
        private string s_url2 = "";
        private string s_url3 = "";
        private readonly HttpClient httpClient;
        public void Initialize(string ServerURL)
        {
            s_url = ServerURL;
            s_url2 = s_url.Replace("Server1", "Server2");
        }
        public void Initialize(string ServerURL, string ServerURL2)
        {
            s_url = ServerURL;
            s_url2 = ServerURL2;
        }
        public void Initialize(string ServerURL, string ServerURL2,string ServerURL3)
        {
            s_url = ServerURL;
            s_url2 = ServerURL2;
            s_url3 = ServerURL3;
        }
        public Connector()
        {

            this.httpClient = new HttpClient();
        }

        public static Round1_JSONData Round1_JSONData_DtSet(int index, int t1, int t2, int t3, int t4, int t5,int t6)
        {
            var rObj = new Round1_JSONData
            {
                index = index,
                team1 = t1,
                team2 = t2,
                team3 = t3,
                team4 = t4,
                team5 = t5,
                team6 = t6
            };
            return rObj;
        }
        public static Round1_2025_JSONData Round1_2025_JSONData_DtSet(int index, int t1, int t2, int t3, int t4, int t5, int t6)
        {
            var rObj = new Round1_2025_JSONData
            {
                id = index,
                team1 = t1,
                team2 = t2,
                team3 = t3,
                team4 = t4,
                team5 = t5,
                team6 = t6
            };
            return rObj;
        }
        public static Round1_2025_JSONData Round1_2025_JSONData_DtSet(int index, string t1, string t2, string t3, string t4, string t5, string t6)
        {
            return Round1_2025_JSONData_DtSet(index, str_conv_int_r1_2025(t1), str_conv_int_r1_2025(t2), str_conv_int_r1_2025(t3), str_conv_int_r1_2025(t4), str_conv_int_r1_2025(t5), str_conv_int_r1_2025(t6));

        }
        public static Round1_JSONData Round1_JSONData_DtSet(int index, string t1, string t2, string t3, string t4, string t5,string t6)
        {
            return Round1_JSONData_DtSet(index, str_conv_int_r1(t1), str_conv_int_r1(t2), str_conv_int_r1(t3), str_conv_int_r1(t4), str_conv_int_r1(t5), str_conv_int_r1(t6));
            
        }


        public static Round2_JSONData Round2_JSONData_DtSet(int index, int t1, int t2, int t3, int t4)
        {
            var rObj = new Round2_JSONData
            {
                index = index,
                team1 = t1,
                team2 = t2,
                team3 = t3,
                team4 = t4
            };
            return rObj;
        }
        public static Round3_JSONData Round3_JSONData_DtSet(int index, int t1, int t2)
        {
            var rObj = new Round3_JSONData
            {
                index = index,
                team1 = t1,
                team2 = t2
            };
            return rObj;
        }
        public static Round2_JSONData Round2_JSONData_DtSet(int index, string t1, string t2, string t3, string t4)
        {
            return Round2_JSONData_DtSet(index, str_conv_int_r1(t1), str_conv_int_r1(t2), str_conv_int_r1(t3), str_conv_int_r1(t4));

        }
        public static Round3_JSONData Round3_JSONData_DtSet(int index, string t1, string t2)
        {
            return Round3_JSONData_DtSet(index, str_conv_int_r1(t1), str_conv_int_r1(t2));

        }
        public static Round2_2025_JSONData_REQ Round2_2025_JSONData_REQ_Gen(int team_id, string phase_value,
            int index)
        {
            return Round2_2025_JSONData_REQ_Gen(team_id, str_conv_int_r2_2025(phase_value),
                index);
        }
        public static Round2_2025_JSONData_REQ Round2_2025_JSONData_REQ_Gen(int team_id, int phase_value,
            int index)
        {
            var miss_timing_X = -1;
            var latest_down_num_X = -1;
            if (phase_value == -2)
            {
                miss_timing_X = index;
                phase_value = -1;
            }
            if (phase_value < 0)
            {
                latest_down_num_X=index;
            }
                var rObj = new Round2_2025_JSONData_REQ
                {
                    team_id = team_id,
                    current_phase_PLUS = phase_value,
                    miss_timing = miss_timing_X,
                    latest_down_num = latest_down_num_X
                };
            return rObj;
        }
        private static int str_conv_int_r1(string param1)
        {
            if (param1.Equals("〇"))
            {
                return 1;
            }
            else if(param1.Equals("✕")){
                return 2;
            }
            return 3;
        }
        private static int str_conv_int_r1_2025(string param1)
        {
            if (param1.Equals("〇"))
            {
                return 1;
            }
            else if (param1.Equals("✕"))
            {
                return 0;
            }
            return 0;
        }

        private static int str_conv_int_r2_2025(string param1)
        {
            if (param1.Equals("↑"))
            {
                return 1;
            }
            else if (param1.Equals("↓"))
            {
                return -1;
            }else if (param1.Equals("✕"))
            {
                return -2;
            }
                return 0;
        }
    }
}
