using System.Collections.Generic;
using VSBaseAngular.Models.Search;
using VSBaseAngular.Models.Keys;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Options;
using VSBaseAngular.Helpers.Options;
using System.Linq;
using AutoMapper;
using System.Net.Http;
using System.IO;
using VSBaseAngular.Models;

namespace VSBaseAngular.Business
{
    public class UserReader : IReader<User>
    {
        private readonly IOptions<ApiConfig> _apiConfigs;
        private readonly IMapper _mapper;
        private readonly HttpClient _client;

        public UserReader(IOptions<ApiConfig> apiConfigs,
            IMapper mapper,
            HttpClient client)
        {
            _apiConfigs = apiConfigs;
            _mapper = mapper;
            _client = client;
        }


        public async Task<User> GetAsync(KeySet<User> key)
        {
            var uKey = key as UserKey;
            
            var section = _apiConfigs.Value?.Configs?.FirstOrDefault(a => a.ApplicationName == "UserAccounts");
            if (section == null) throw new Exception("No configuration found for UserAccounts Api");

            var url = Path.Combine(section.Url, uKey.Domain, uKey.UserId);

            var response = await _client.GetAsync(url);

            return response.ContentAsType<User>();
        }

        public Task<IEnumerable<User>> SearchAsync(SearchBaseModel<User> model)
        {
            throw new NotImplementedException();
        }

    }
}