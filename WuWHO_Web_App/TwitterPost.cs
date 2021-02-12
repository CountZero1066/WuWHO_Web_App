using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WuWHO_Web_App
{
    public class TwitterPost
    {
        const string TwitterApiBaseUrl = "https://api.twitter.com/1.1/";
        readonly string consumerKey, consumerKeySecret, accessToken, accessTokenSecret;
        readonly HMACSHA1 sigHasher;
        readonly DateTime epochUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        public TwitterPost(string consumerKey, string consumerKeySecret, string accessToken, string accessTokenSecret)
        {
            this.consumerKey = "n6vhsJ4BKULXjECUpkZ7n9lvq";
            this.consumerKeySecret = "ORA3Jfg6E1rg9tuIaIOcAUrbIZPFEnkO7bnyKuo6j7XwBwJilH";
            this.accessToken = "1355915412198871042-Kh7f1QqfgSirlva7SQmy07vrluS32e";
            this.accessTokenSecret = "bqcAuILA2U3KJSqSSqsWUke075X0pqzEDL7g0drJuTWhM";

            sigHasher = new HMACSHA1(new ASCIIEncoding().GetBytes(string.Format("{0}&{1}", consumerKeySecret, accessTokenSecret)));
        }
        
        public Task<string> Tweet(string text)
        {
            var data = new Dictionary<string, string> {
            { "status", text },
            { "trim_user", "1" }
        };

            return SendRequest("statuses/update.json", data);
        }

        Task<string> SendRequest(string url, Dictionary<string, string> data)
        {
            var fullUrl = TwitterApiBaseUrl + url;

            var timestamp = (int)((DateTime.UtcNow - epochUtc).TotalSeconds);

            data.Add("oauth_consumer_key", consumerKey);
            data.Add("oauth_signature_method", "HMAC-SHA1");
            data.Add("oauth_timestamp", timestamp.ToString());
            data.Add("oauth_nonce", "a"); 
            data.Add("oauth_token", accessToken);
            data.Add("oauth_version", "1.0");
            
            data.Add("oauth_signature", GenerateSignature(fullUrl, data));
            
            string oAuthHeader = GenerateOAuthHeader(data);

            var formData = new FormUrlEncodedContent(data.Where(kvp => !kvp.Key.StartsWith("oauth_")));

            return SendRequest(fullUrl, oAuthHeader, formData);
        }
        
        string GenerateSignature(string url, Dictionary<string, string> data)
        {
            var sigString = string.Join(
                "&",
                data
                    .Union(data)
                    .Select(kvp => string.Format("{0}={1}", Uri.EscapeDataString(kvp.Key), Uri.EscapeDataString(kvp.Value)))
                    .OrderBy(s => s)
            );

            var fullSigData = string.Format(
                "{0}&{1}&{2}",
                "POST",
                Uri.EscapeDataString(url),
                Uri.EscapeDataString(sigString.ToString())
            );

            return Convert.ToBase64String(sigHasher.ComputeHash(new ASCIIEncoding().GetBytes(fullSigData.ToString())));
        }

        string GenerateOAuthHeader(Dictionary<string, string> data)
        {
            return "OAuth " + string.Join(
                ", ",
                data
                    .Where(kvp => kvp.Key.StartsWith("oauth_"))
                    .Select(kvp => string.Format("{0}=\"{1}\"", Uri.EscapeDataString(kvp.Key), Uri.EscapeDataString(kvp.Value)))
                    .OrderBy(s => s)
            );
        }
        
        async Task<string> SendRequest(string fullUrl, string oAuthHeader, FormUrlEncodedContent formData)
        {
            using (var http = new HttpClient())
            {
                http.DefaultRequestHeaders.Add("Authorization", oAuthHeader);

                var httpResp = await http.PostAsync(fullUrl, formData);
                var respBody = await httpResp.Content.ReadAsStringAsync();

                return respBody;
            }
        }
        public static void Sendtweet(string tweetbody)
        {

            var postTweet = new TwitterPost("n6vhsJ4BKULXjECUpkZ7n9lvq", "ORA3Jfg6E1rg9tuIaIOcAUrbIZPFEnkO7bnyKuo6j7XwBwJilH", "1355915412198871042-Kh7f1QqfgSirlva7SQmy07vrluS32e", "bqcAuILA2U3KJSqSSqsWUke075X0pqzEDL7g0drJuTWhM");
            var response = postTweet.Tweet(tweetbody);
            Console.WriteLine(response);

        }

    }

}