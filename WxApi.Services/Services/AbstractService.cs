using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WxApi.Configuration.Settings;

namespace WxApi.Services
{
    public abstract class AbstractService<T>
    {
        protected readonly HttpClient _httpClient;
        protected readonly IUserService _userService;
        protected readonly AppSettings _appSettings;
        
        public abstract string RequestUri { get; protected set; }

        public AbstractService(HttpClient httpClient, IUserService userService, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _userService = userService;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_appSettings.WxServiceBaseUrl);
        }

        public async Task<List<T>> GetAll()
        {
            var user = _userService.Get();
            var response = await _httpClient.GetAsync($"{RequestUri}?token={user.Token}");

            if (response.IsSuccessStatusCode)
            {
                var items = JsonConvert.DeserializeObject<List<T>>(await response.Content.ReadAsStringAsync());
                return items;
            }

            throw new Exception($"An error occured in {RequestUri} service: {response.ReasonPhrase}");
        }

        public async Task<string> Post(T item)
        {
            var user = _userService.Get();
            var httpContent = new StringContent(JsonConvert.SerializeObject(item), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{RequestUri}?token={user.Token}", httpContent);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return jsonResponse;
            }

            throw new Exception($"An error occured in {RequestUri} service: {response.ReasonPhrase}");
        }
    }
}
