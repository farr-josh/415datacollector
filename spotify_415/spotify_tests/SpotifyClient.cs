using spotify_tests.API_ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace spotify_tests
{
    public class SpotifyClient
    {
        HttpClient httpClient;
        private string AccessToken;
        public SpotifyClient()
        {
            GetNewToken();
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            httpClient.Timeout = TimeSpan.FromMinutes(3);
        }

        public string GetArtistByIDS(string ids)
        {
            var builder = new UriBuilder("https://api.spotify.com/v1/artists");
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["ids"] = ids;
            builder.Query = query.ToString();
            var httpResponse = httpClient.GetAsync(builder.ToString());
            var response = httpResponse.Result;
            if (response.IsSuccessStatusCode)
            {
                string responseBody = response.Content.ReadAsStringAsync().Result;
                return responseBody;
            }
            else
            {
                Console.WriteLine(response);
                Console.WriteLine("Response error");
                return string.Empty;
            }
        }

        public string GetAlbumsByArtistID(string id, long offset, long limit)
        {
            var builder = new UriBuilder("https://api.spotify.com/v1/artists/" + id + "/albums");
            var query = HttpUtility.ParseQueryString(builder.Query);
            //query["type"] = "album";
            query["include_groups"] = "album,single";
            query["limit"] = limit.ToString();
            query["offset"] = offset.ToString();
            //query["q"] = name;
            builder.Query = query.ToString();

            var httpResponse = httpClient.GetAsync(builder.ToString());
            var response = httpResponse.Result;
            if (response.IsSuccessStatusCode)
            {
                string responseBody = response.Content.ReadAsStringAsync().Result;
                return responseBody;
            }
            else
            {
                Console.WriteLine(response);
                Console.WriteLine("Response error");
                return string.Empty;
            }
        }

        public string GetTracksByAlbumID(string id, long offset, long limit)
        {
            var builder = new UriBuilder("https://api.spotify.com/v1/albums/" + id + "/tracks");
            var query = HttpUtility.ParseQueryString(builder.Query);
            //query["type"] = "album";
            query["limit"] = limit.ToString();
            query["offset"] = offset.ToString();
            //query["q"] = name;
            builder.Query = query.ToString();

            var httpResponse = httpClient.GetAsync(builder.ToString());
            var response = httpResponse.Result;
            if (response.IsSuccessStatusCode)
            {
                string responseBody = response.Content.ReadAsStringAsync().Result;
                return responseBody;
            }
            else
            {
                Console.WriteLine(response);
                Console.WriteLine("Response error");
                return string.Empty;
            }
        }

        public string GetTracksAudioFeatures(string ids)
        {
            var builder = new UriBuilder("https://api.spotify.com/v1/audio-features");
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["ids"] = ids;
            builder.Query = query.ToString();

            var httpResponse = httpClient.GetAsync(builder.ToString());
            var response = httpResponse.Result;
            if ((int)response.StatusCode == 421)
            {
                GetNewToken();
                GetTracksAudioFeatures(ids);
            }
            if (response.IsSuccessStatusCode)
            {
                string responseBody = response.Content.ReadAsStringAsync().Result;
                return responseBody;
            }
            else
            {
                Console.WriteLine(response);
                Console.WriteLine("Response error");
                return string.Empty;
            }
        }



        /// <summary>
        /// ADD YOUR CLIENTID AND SECRET ID HERE
        /// https://developer.spotify.com/dashboard/
        /// login
        /// create project
        /// copy and paste the client and secret id from browser
        /// </summary>
        public void GetNewToken()
        {
            string clientID = "";
            string SecretID = "";

            HttpClient client = new HttpClient();
            Uri baseUri = new Uri("https://accounts.spotify.com");
            client.BaseAddress = baseUri;
            client.DefaultRequestHeaders.Clear();
            //client.DefaultRequestHeaders.ConnectionClose = true;

            //Post body content
            var values = new List<KeyValuePair<string, string>>();
            values.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            var content = new FormUrlEncodedContent(values);
            var authenticationString = $"{clientID}:{SecretID}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/token");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
            requestMessage.Content = content;

            //make the request
            var task = client.SendAsync(requestMessage);
            var response = task.Result;
            response.EnsureSuccessStatusCode();
            string responseBody = response.Content.ReadAsStringAsync().Result;

            AccessTokenResponse token = AccessTokenResponse.FromJson(responseBody);
            AccessToken = token.AccessToken;
            Console.WriteLine("Created new access token: " + AccessToken);
        }





    }
}
