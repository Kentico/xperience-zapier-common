﻿using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using CMS;
using CMS.Core;
using CMS.DataEngine;

using Newtonsoft.Json;

using Xperience.Zapier.Common;

[assembly: RegisterImplementation(typeof(IZapierClient), typeof(ZapierClient), Priority = RegistrationPriority.SystemDefault)]
namespace Xperience.Zapier.Common
{
    /// <summary>
    /// Default implementation of <see cref="IZapierClient"/>.
    /// </summary>
    internal class ZapierClient : IZapierClient
    {
        private readonly HttpClient httpClient;


        /// <summary>
        /// Constructor.
        /// </summary>
        public ZapierClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }


        /// <inheritdoc/>
        public Task TriggerWebhook(string webhookUrl, params BaseInfo[] data)
        {
            if (String.IsNullOrEmpty(webhookUrl))
            {
                throw new ArgumentNullException(nameof(webhookUrl));
            }

            if (data == null || !data.Any())
            {
                throw new InvalidOperationException("No data provided.");
            }

            var content = data.Where(info => info != null).Select(info => info.ToZapierObject());
            var json = JsonConvert.SerializeObject(content);

            return DoPost(webhookUrl, json);
        }


        private async Task DoPost(string webhookUrl, string content)
        {
            var buffer = Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(webhookUrl),
                Content = byteContent
            };

            var response = await httpClient.SendAsync(httpRequestMessage);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"POST to {webhookUrl} failed with the following message:<br/>{message}");
            }
        }
    }
}
