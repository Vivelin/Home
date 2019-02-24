using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication.OAuth;

using Newtonsoft.Json.Linq;

namespace Vivelin.Home
{
    /// <summary>
    /// Represents a client used to refresh Twitch access tokens.
    /// </summary>
    public class TwitchTokenClient
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private readonly string clientId;
        private readonly string clientSecret;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwitchTokenClient"/>
        /// class with the specified Twitch client ID and secret.
        /// </summary>
        /// <param name="clientId">The Twitch client ID.</param>
        /// <param name="clientSecret">The Twitch client secret.</param>
        public TwitchTokenClient(string clientId, string clientSecret)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
        }

        /// <summary>
        /// Gets or sets the Twitch OAuth token endpoint address.
        /// </summary>
        public Uri TokenEndpoint { get; set; }
            = new Uri(AspNet.Security.OAuth.Twitch.TwitchAuthenticationDefaults.TokenEndpoint);

        /// <summary>
        /// Requests a new access token based on a given OAuth refresh token.
        /// </summary>
        /// <param name="refreshToken">
        /// The refresh token issued to the client.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> that returns the OAuth token endpoint response.
        /// </returns>
        public async Task<OAuthTokenResponse> RefreshAccessTokenAsync(string refreshToken)
        {
            var tokenRequestParameters = new Dictionary<string, string>
            {
                {"grant_type", "refresh_token" },
                {"refresh_token", refreshToken },
                {"client_id", clientId },
                {"client_secret", clientSecret },
                { "scope", "user:read:email" }
            };

            var requestContent = new FormUrlEncodedContent(tokenRequestParameters);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, TokenEndpoint);
            requestMessage.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            requestMessage.Content = requestContent;

            var response = await httpClient.SendAsync(requestMessage);
            if (response.IsSuccessStatusCode)
            {
                var payload = JObject.Parse(await response.Content.ReadAsStringAsync());
                return OAuthTokenResponse.Success(payload);
            }
            else
            {
                var error = await GetExceptionAsync(response);
                return OAuthTokenResponse.Failed(error);
            }
        }

        private static async Task<Exception> GetExceptionAsync(HttpResponseMessage response)
        {
            var output = new StringBuilder();
            output.Append("Status: ").Append(response.StatusCode).Append(";");
            output.Append("Headers: ").Append(response.Headers.ToString()).Append(";");
            output.Append("Body: ").Append(await response.Content.ReadAsStringAsync()).Append(";");
            return new Exception("OAuth token endpoint failure: " + output.ToString());
        }
    }
}