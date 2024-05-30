using Avalonia.Controls;
using Avalonia.Controls.Embedding;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Web;
using DynamicData;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net;
using Microsoft.VisualBasic;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Reflection;

namespace spotifyAPIgae
{
    public class SpotifyAuth
    {
        private static readonly string clientID = "de1e57a1216644c989a8e8db60349409";
        private static readonly string clientSecret = "9d2914c4a98f41fb95dada86a7ebd12b";
        private static readonly int verifierLength = 128;
        private static readonly string redirectUriSlash = "http://localhost:5173/callback/";
        private static readonly string redirectUri = "http://localhost:5173/callback";
        private static string codeVerifier = null;
        private static readonly string authPagePath = @"C:\Users\marci\Documents\GitHub\spotifyAPIgae\spotifyAPIgae\authPage.html";
        public SpotifyAuth() { }
        public async Task<SpotifyUser> BeginAuthorization()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(redirectUriSlash);
            Task<string> listenerTask = Task.Run(() => HttpListenerLoop(listener));
            listener.Stop();
            //add some synchronization
            redirectToAuthFlow(clientID);
            var clientCode = await listenerTask;
            string accessToken = await GetAccessToken(clientID, clientCode);
            var token = JsonConvert.DeserializeObject<TokenResponse>(accessToken);
            string profile = await FetchProfileAsync(token.AccessToken);
            SpotifyUser UserProfile = JsonConvert.DeserializeObject<SpotifyUser>(profile);
            
            foreach(var item in UserProfile.GetType().GetProperties())
            {
                Debug.WriteLine(item.GetValue(UserProfile));
            }
            Debug.WriteLine(profile);
            return UserProfile;
        }
        private async Task<dynamic> FetchProfileAsync(string token)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.GetAsync("https://api.spotify.com/v1/me");
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent; 
        }
        private async Task<string> HttpListenerLoop(HttpListener listener)
        {
            listener.Start();
            while(true)
            {
                var context = await listener.GetContextAsync();
                var request = context.Request;
                var response = context.Response;
                var authCode = request.QueryString["code"];
                if (!string.IsNullOrEmpty(authCode))
                {
                    //response.Redirect(authPagePath);
                    response.Close();
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = authPagePath,
                        UseShellExecute = true
                    });
                    return authCode;
                }
                else
                    Debug.WriteLine("didnt get code");
            }
            return "";
        }
        private void ServeAuthPage(HttpListenerResponse response)
        {
            response.StatusCode = (int)HttpStatusCode.Redirect;
            var redirectUrl = $"{redirectUri}/{authPagePath}";
            response.Redirect(redirectUrl);
            response.OutputStream.Close();
        }
        private async void redirectToAuthFlow(string clientID)
        {
            codeVerifier = GenerateCodeVerifier(verifierLength);
            var challenge  = await generateCodeChallenge(codeVerifier);
            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            queryParams["client_id"] = clientID;
            queryParams["response_type"] = "code";
            queryParams["redirect_uri"] = "http://localhost:5173/callback";
            queryParams["scope"] = "user-read-private user-read-email";
            queryParams["code_challenge_method"] = "S256";
            queryParams["code_challenge"] = challenge;

            string authorizeUrl = "https://accounts.spotify.com/authorize?" + queryParams.ToString();
           
            Process.Start(new ProcessStartInfo
            {
                FileName = authorizeUrl,
                UseShellExecute = true
            });

        }
        private static string GenerateCodeVerifier(int codeLength)
        {
            string text = "";
            string possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random rand = new Random();
            for (int i = 0; i < codeLength; i++)
            {
                int r = rand.Next(0,possible.Length);

                text += possible[r];
            }
            return text;
        }
        private async Task<string> generateCodeChallenge(string codeVerifier )
        {
            var data = Encoding.UTF8.GetBytes(codeVerifier);
            byte[]? digest;
            using(SHA256 sha256 = SHA256.Create()) 
            {
               digest = await Task.Run(() => sha256.ComputeHash(data));
            }
            string base64Digest = Convert.ToBase64String(digest);
            base64Digest = base64Digest.Replace("+", "-").Replace("/", "_").TrimEnd('=');
            return base64Digest;
        }
        private async Task<string> GetAccessToken(string clientId, string code)
        {
            var parameters = new Dictionary<string, string>
            {
                { "client_id", clientId },
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", redirectUri },
                { "code_verifier", codeVerifier }
            };

            var content = new FormUrlEncodedContent(parameters);

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                var response = await httpClient.PostAsync("https://accounts.spotify.com/api/token", content);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
        }
    }
}
public class TokenResponse
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }

    [JsonProperty("token_type")]
    public string TokenType { get; set; }

    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonProperty("refresh_token")]
    public string RefreshToken { get; set; }
}