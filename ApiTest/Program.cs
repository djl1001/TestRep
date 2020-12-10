using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ApiTest
{
    public class Program
    {
        public static string Key = "8552ec0080406aa050ac863fd4658e3ea3f62cc78702d3df839efc9a637dd13e";
        public static string strData = "8552ec0080406aa050ac863fd4658e3ea3f62cc78702d3df839efc9a637dd13ehttps://open.taobao.com/";
        public static int Partner_ID = 842924;
        static void Main(string[] args)
        {



            int Shopid = 117937217;
            DateTime dt = DateTime.Now;
            long startTime = (dt.AddDays(-50).ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            long endTime = (dt.AddDays(-40).ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            long timestamp = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            string status = "COMPLETED";
            Console.WriteLine(timestamp);
            string url = "https://partner.shopeemobile.com/api/v1/orders/get";
            //string da = "{" + "\"partner_id\":" + Partner_ID + "," +
            //            "\"shopid\":" + Shopid + "," +
            //            "\"timestamp\":" + timestamp +","+
            //            "\"order_status\":" + "\"COMPLETED\""+
            //            "}";
            //string[] orderlist = { "200703D0QC5031" };
            var jsonobject = new JObject();
            dynamic authorInfo = jsonobject;
            //authorInfo.create_time_from = startTime;
            //authorInfo.create_time_to = endTime;
            authorInfo.partner_id = 842924;
            authorInfo.order_status = "COMPLETED";
            authorInfo.shopid = 117937217;
            authorInfo.timestamp = timestamp;
            var j = authorInfo.ToString();
            string sign = url + "|" + j + "";
            string Authorization = HmacSHA256(sign, Key);
            Console.WriteLine(timestamp);
            Console.WriteLine(Authorization);
            string data = PostJson(url, j, Authorization);
            Console.WriteLine(data);

        }

        public static string PostJson(string url, string jsonstring, string Authorization)
        {
            // 创建一个HTTP请求
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            // Post请求方式
            request.Method = "POST";
            request.ContentType = "application/json";
            request.AllowAutoRedirect = false;
            request.KeepAlive = true;
            //头部请求方式 
            request.Headers.Add("Authorization", Authorization);
            MethodInfo priMethod = request.Headers.GetType().GetMethod("AddWithoutValidate", BindingFlags.Instance | BindingFlags.NonPublic);
            // 设置参数，并进行URL编码
            string paraUrlCoded = jsonstring;
            byte[] payload;
            payload = Encoding.UTF8.GetBytes(paraUrlCoded);
            request.ContentLength = payload.Length;
            Stream writer;
            try
            {
                //获取用于写入请求数据的Stream对象
                writer = request.GetRequestStream();
            }
            catch (Exception)
            {
                writer = null;
                Console.Write("连接服务器失败!");
            }
            // 将请求参数写入流
            writer.Write(payload, 0, payload.Length);
            writer.Close();//关闭请求流
            HttpWebResponse response;
            try
            {
                // 获得响应流
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.StackTrace.ToString());
                response = ex.Response as HttpWebResponse;
            }
            Stream s = response.GetResponseStream();
            StreamReader sRead = new StreamReader(s);
            // 获取返回json字符串
            string postContent = sRead.ReadToEnd();
            string jsonStr = (JValue.Parse(postContent)).ToString();
            sRead.Close();
            return jsonStr;
        }
        private static string HmacSHA256(string secret, string signKey)
        {
            string signRet = string.Empty;
            using (HMACSHA256 mac = new HMACSHA256(Encoding.UTF8.GetBytes(signKey)))
            {
                byte[] hash = mac.ComputeHash(Encoding.UTF8.GetBytes(secret));
                signRet = ToHexString(hash);
            }
            return signRet;
        }


        public static string ToHexString(byte[] bytes)
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();
                foreach (byte b in bytes)
                {
                    strB.AppendFormat("{0:x2}", b);
                }
                hexString = strB.ToString();
            }
            return hexString;
        }

        public static string GetSHA256HashFromString(string strData)
        {
            byte[] bytValue = System.Text.Encoding.UTF8.GetBytes(strData);
            SHA256 sha256 = new SHA256CryptoServiceProvider();

            byte[] retVal = sha256.ComputeHash(bytValue);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
