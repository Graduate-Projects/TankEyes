using Supplier.Mobile.Models.Notification;
using Supplier.Mobile.Interface;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Supplier.Mobile.Services
{
    public class NotificationRegistrationService : INotificationRegistrationService
    {
        string RequestUrl = $"api/notifications/installations";

        string _baseApiUrl;
        HttpClient _client;
        IDeviceInstallationService _deviceInstallationService;

        public NotificationRegistrationService(string baseApiUri)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Accept", "application/json");

            _baseApiUrl = baseApiUri;
        }
        public NotificationRegistrationService(string baseApiUri, string apiKey)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.DefaultRequestHeaders.Add("apikey", apiKey);

            _baseApiUrl = baseApiUri;
        }

        IDeviceInstallationService DeviceInstallationService
            => _deviceInstallationService ??= ServiceContainer.Resolve<IDeviceInstallationService>();

        public async Task UnRegisterDeviceAsync()
        {
            var cachedToken = await SecureStorage.GetAsync(Utils.LocalStorage.CachedDeviceTokenKey)
                .ConfigureAwait(false);

            if (cachedToken == null)
                return;

            var deviceId = DeviceInstallationService?.GetDeviceId();

            if (string.IsNullOrWhiteSpace(deviceId))
                throw new Exception("Unable to resolve an ID for the device.");

            await SendAsync(HttpMethod.Delete, $"{RequestUrl}/{deviceId}")
                .ConfigureAwait(false);

            SecureStorage.Remove(Utils.LocalStorage.CachedDeviceTokenKey);
            SecureStorage.Remove(Utils.LocalStorage.CachedTagsKey);
        }

        public async Task RegisterDeviceAsync(params string[] tags)
        {
            var deviceInstallation = DeviceInstallationService?.GetDeviceInstallation(tags);
            try
            {
                //Utils.Diagnostic.LogEvent($"Start Register Device $InstallationId {{{deviceInstallation}}}", new Dictionary<string, string> { { "tags", string.Join(", ", tags) } });

                await SendAsync(HttpMethod.Put, RequestUrl, deviceInstallation).ConfigureAwait(false);

                await SecureStorage.SetAsync(Utils.LocalStorage.CachedDeviceTokenKey, deviceInstallation.PushChannel).ConfigureAwait(false);

                await SecureStorage.SetAsync(Utils.LocalStorage.CachedTagsKey, Newtonsoft.Json.JsonConvert.SerializeObject(tags));

                //Utils.Diagnostic.LogEvent($"Finish Register Device $InstallationId {{{deviceInstallation}}}", new Dictionary<string, string> { { "tags", string.Join(", ", tags) } });
            }
            catch (Exception ex)
            {
                Utils.Diagnostic.Log(ex, $"Try Register Device  $InstallationId {{{deviceInstallation}}} on tags: {string.Join(", ",tags)}");
            }
        }

        public async Task RefreshRegistrationAsync()
        {
            try
            {
                var cachedToken = await SecureStorage.GetAsync(Utils.LocalStorage.CachedDeviceTokenKey).ConfigureAwait(false);

                var serializedTags = await SecureStorage.GetAsync(Utils.LocalStorage.CachedTagsKey).ConfigureAwait(false);

                if (string.IsNullOrWhiteSpace(cachedToken) ||
                    string.IsNullOrWhiteSpace(serializedTags) ||
                    string.IsNullOrWhiteSpace(DeviceInstallationService.Token) ||
                    cachedToken == DeviceInstallationService.Token)
                    return;

                var tags = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(serializedTags);

                await RegisterDeviceAsync(tags);
            }
            catch (Exception ex)
            {
                Utils.Diagnostic.Log(ex, $"Try Refresh Register Device Notification");
            }
        }

        async Task SendAsync<T>(HttpMethod requestType, string requestUri, T obj)
        {
            string serializedContent = null;

            await Task.Run(() => serializedContent = Newtonsoft.Json.JsonConvert.SerializeObject(obj))
                .ConfigureAwait(false);

            await SendAsync(requestType, requestUri, serializedContent);
        }

        async Task SendAsync(HttpMethod requestType,string requestUri,string jsonRequest = null)
        {
            var URL = $"{_baseApiUrl}{requestUri}";
            var ResponseMessage = string.Empty;
            try
            {
                var request = new HttpRequestMessage(requestType, new Uri(URL));

                if (jsonRequest != null)
                    request.Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                var response = await _client.SendAsync(request).ConfigureAwait(false);
                ResponseMessage = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Utils.Diagnostic.Log(ex, $"Url: {URL}, response: {ResponseMessage}");
            }
        }
    }
}
