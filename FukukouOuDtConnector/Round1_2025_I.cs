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
        public Round1_2025_NextJsonData Get_NextRound_R1_2025()
        {
            string point = this.s_url + "/next_round";

            HttpRequestMessage rq = new HttpRequestMessage(HttpMethod.Get, point);
            string rbodyS;
            HttpStatusCode hscode = HttpStatusCode.NotFound;
            Task<HttpResponseMessage> resp;
            try
            {
                resp = httpClient.SendAsync(rq);
                rbodyS = resp.Result.Content.ReadAsStringAsync().Result;
                hscode = resp.Result.StatusCode;

                return System.Text.Json.JsonSerializer.Deserialize<Round1_2025_NextJsonData>(rbodyS);
            }
            catch (Exception e)
            {
                return new Round1_2025_NextJsonData();
            }
        }
        public SendResult Send_Stop2025R1()
        {
            string point = this.s_url + "/stop_video";
            HttpRequestMessage rq = new HttpRequestMessage(HttpMethod.Post, point);
            string rbodyS;
            HttpStatusCode hscode = HttpStatusCode.NotFound;
            Task<HttpResponseMessage> resp;
            try
            {
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
        public SendResult Send_Play2025R1()
        {
            string point = this.s_url + "/play_video";
            HttpRequestMessage rq = new HttpRequestMessage(HttpMethod.Post, point);
            string rbodyS;
            HttpStatusCode hscode = HttpStatusCode.NotFound;
            Task<HttpResponseMessage> resp;
            try
            {
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
        public SendResult Send_R1_2025_Result(Round1_2025_JSONData sendData,int next_question,bool isTwoQuestion=false)
        {
            if (PluginHost.SendType == SendTypes.Inquire)
            {
                string point = this.s_url + "/status";
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
                }catch(HttpRequestException e)
                {
                    return SendResult.No("Error "+e.Message);
                }catch(Exception e2)
                {
                    return SendResult.Error(e2);
                }
                return SendResult.No("エラー(Inquire)");
            }
            if (PluginHost.SendType == SendTypes.Sell)
            {
                {
                    string point = this.s_url + "/round_datas";
                    HttpRequestMessage rq = new HttpRequestMessage(HttpMethod.Post, point);
                    string rbodyS;
                    HttpStatusCode hscode = HttpStatusCode.NotFound;
                    if (isTwoQuestion)
                    {
                        sendData.team1 = sendData.team1 * 2;
                        sendData.team2 = sendData.team2 * 2;
                        sendData.team3 = sendData.team3 * 2;
                        sendData.team4 = sendData.team4 * 2;
                        sendData.team5 = sendData.team5 * 2;
                        sendData.team6 = sendData.team6 * 2;
                    }
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
                    string point = this.s_url + "/used_questions";
                    HttpRequestMessage rq = new HttpRequestMessage(HttpMethod.Post, point);
                    string rbodyS;
                    HttpStatusCode hscode = HttpStatusCode.NotFound;
                    Round1_2025_USEDQJSONData dA=new Round1_2025_USEDQJSONData();
                    dA.id = next_question;
                    var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(dA), Encoding.UTF8, "application/json");
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
                    string point = this.s_url + "/next_round";
                    HttpRequestMessage rq = new HttpRequestMessage(HttpMethod.Post, point);
                    string rbodyS;
                    HttpStatusCode hscode = HttpStatusCode.NotFound;
                    var nextRData = new Round1_2025_NextJsonData();
                    nextRData.current_stage = sendData.id + 1;
                    nextRData.current_question = next_question;
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
                            return RefreshableSendResult.Yes("Success!", "Yeah!", false,true);
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
                string point = this.s_url + "/set_round_data";
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
