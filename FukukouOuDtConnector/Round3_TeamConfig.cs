using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FukukouOuDtConnector
{
    public partial class Connector
    {
        public Round3_TeamConfig_JSONData[] get_R3TeamData()
        {

            string point = this.s_url3 + "/get_teamnames";
            HttpRequestMessage rq = new HttpRequestMessage(HttpMethod.Get, point);
            string rbodyS;
            HttpStatusCode hscode = HttpStatusCode.NotFound;
            Task<HttpResponseMessage> resp;
            try
            {
                resp = httpClient.SendAsync(rq);
                rbodyS = resp.Result.Content.ReadAsStringAsync().Result;
                //hscode = resp.Result.StatusCode;

                var getDt = JsonSerializer.Deserialize<Round3_TeamConfig_JSONData[]>(rbodyS);
                return getDt;
            }
            catch (Exception e)
            {
                //none
            }
            return new Round3_TeamConfig_JSONData[0];
        }
    }
}
