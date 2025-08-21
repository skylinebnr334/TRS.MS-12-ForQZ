using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using TRS.TMS12.Interfaces;

namespace FukukouOuDtConnector
{
    public partial class Connector
    {
        public Round2_I_ScoreConfig_JSONData get_DefaultScoreConfigR2()
        {
            string point = this.s_url2 + "/get_score_setting";

            HttpRequestMessage rq = new HttpRequestMessage(HttpMethod.Get, point);
            string rbodyS;
            HttpStatusCode hscode = HttpStatusCode.NotFound;
            Task<HttpResponseMessage> resp;
            try
            {
                resp = httpClient.SendAsync(rq);
                rbodyS = resp.Result.Content.ReadAsStringAsync().Result;
                //hscode = resp.Result.StatusCode;

                var getDt = JsonSerializer.Deserialize<Round2_I_ScoreConfig_JSONData[]>(rbodyS);
                foreach(var dt in getDt.Where(z=>z.index==-810))
                {
                    return dt;
                }
                return new Round2_I_ScoreConfig_JSONData()
                {
                    index = -810,
                    ask_throw = 0,
                    correct = 1,
                    miss = -1
                };
                //return int.Parse(rbodyS);
            }
            catch (Exception e)
            {
                return new Round2_I_ScoreConfig_JSONData()
                {
                    index=-810,
                    ask_throw=0,
                    correct=1,
                    miss=1
                };
            }
        }
        public SendResult SendScoreConfigR2(Round2_I_ScoreConfig_JSONData data)
        {
            if (PluginHost.SendType == SendTypes.Sell)
            {
                {
                    string point = this.s_url2 + "/set_score_setting";
                    HttpRequestMessage rq = new HttpRequestMessage(HttpMethod.Post, point);
                    string rbodyS;
                    HttpStatusCode hscode = HttpStatusCode.NotFound;
                    var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
                    Task<HttpResponseMessage> resp;
                    try
                    {
                        rq.Content = content;
                        resp = httpClient.SendAsync(rq);
                        rbodyS = resp.Result.Content.ReadAsStringAsync().Result;
                        hscode = resp.Result.StatusCode;
                        if (hscode == HttpStatusCode.OK)
                        {
                            return SendResult.Yes("Success!", "Yeah!", false);
                        }
                        else
                        {
                            return SendResult.No(rbodyS);
                        }
                    }
                    catch (HttpRequestException e)
                    {
                        return SendResult.No("Error " + e.Message);
                    }
                    catch (Exception e2)
                    {
                        return SendResult.Error(e2);
                    }
                }

            }
            else
            {
                return SendResult.No("エラー");
            }
        }
    }
}
