using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TRS.TMS12.Interfaces;

namespace FukukouOuDtConnector
{
    public partial class Connector:IPlugin
    {
        
        public int Get_NextRound_R2_2025()
        {
            string point = this.s_url2 + "/next_round";

            HttpRequestMessage rq = new HttpRequestMessage(HttpMethod.Get, point);
            string rbodyS;
            HttpStatusCode hscode = HttpStatusCode.NotFound;
            Task<HttpResponseMessage> resp;
            try
            {
                resp = httpClient.SendAsync(rq);
                rbodyS = resp.Result.Content.ReadAsStringAsync().Result;
                hscode = resp.Result.StatusCode;
                return System.Text.Json.JsonSerializer.Deserialize<Round2_2025_NextJsonData>(rbodyS).current_num;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public SendResult Send_R2_2025_Result(Round2_2025_JSONData_REQ sendData,int index)
        {
            if (PluginHost.SendType == SendTypes.Inquire)
            {
                string point = this.s_url2 + "/status";
                HttpRequestMessage rq = new HttpRequestMessage(HttpMethod.Get, point);
                string rbodyS;
                HttpStatusCode hscode = HttpStatusCode.NotFound;
                Task<HttpResponseMessage> resp;
                try
                {
                    resp = httpClient.SendAsync(rq);
                    rbodyS = resp.Result.Content.ReadAsStringAsync().Result;
                    hscode = resp.Result.StatusCode;
                    return SendResult.No(rbodyS);
                }
                catch (HttpRequestException e)
                {
                    return SendResult.No("Error " + e.Message);
                }
                catch (Exception e2)
                {
                    return SendResult.Error(e2);
                }
                return SendResult.No("エラー(Inquire)");
            }
            if (PluginHost.SendType == SendTypes.Sell)
            {
                {
                    string point = this.s_url2 + "/round_datas_plus";
                    HttpRequestMessage rq = new HttpRequestMessage(HttpMethod.Post, point);
                    string rbodyS;
                    HttpStatusCode hscode = HttpStatusCode.NotFound;
                    var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(sendData), Encoding.UTF8, "application/json");
                    Task<HttpResponseMessage> resp;
                    try
                    {
                        rq.Content = content;
                        resp = httpClient.SendAsync(rq);
                        rbodyS = resp.Result.Content.ReadAsStringAsync().Result;
                        hscode = resp.Result.StatusCode;
                        if (hscode == HttpStatusCode.OK)
                        {
                            //return SendResult.Yes("Success!", "Yeah!", false);
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
                {
                    string point = this.s_url2 + "/next_round";
                    HttpRequestMessage rq = new HttpRequestMessage(HttpMethod.Post, point);
                    string rbodyS;
                    HttpStatusCode hscode = HttpStatusCode.NotFound;
                    var nextRData = new Round2_2025_NextJsonData();
                    nextRData.current_num = index + 1;
                    var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(nextRData), Encoding.UTF8, "application/json");
                    Task<HttpResponseMessage> resp;
                    try
                    {
                        rq.Content = content;
                        resp = httpClient.SendAsync(rq);
                        rbodyS = resp.Result.Content.ReadAsStringAsync().Result;
                        hscode = resp.Result.StatusCode;
                        if (hscode == HttpStatusCode.OK)
                        {
                            //PluginHost.CurrentTicket.SetDefault();
                            return RefreshableSendResult.Yes("Success!", "Yeah!", false, true);
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
                string point = this.s_url2 + "/set_round_data";
                HttpRequestMessage rq = new HttpRequestMessage(HttpMethod.Post, point);
                string rbodyS;
                HttpStatusCode hscode = HttpStatusCode.NotFound;
                var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(sendData), Encoding.UTF8, "application/json");
                Task<HttpResponseMessage> resp;
                try
                {
                    rq.Content = content;
                    resp = httpClient.SendAsync(rq);
                    rbodyS = resp.Result.Content.ReadAsStringAsync().Result;
                    hscode = resp.Result.StatusCode;
                    if (hscode == HttpStatusCode.OK)
                    {
                        return SendResult.Yes("Success!", "訂正処理完了", false);
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
            return SendResult.No("エラ－");
        }
    }
}
