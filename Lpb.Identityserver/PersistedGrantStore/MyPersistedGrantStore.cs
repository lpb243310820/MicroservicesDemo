using IdentityServer4.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using System.Collections.Concurrent;

namespace Lpb.Identityserver.PersistedGrantStore
{
    public class MyPersistedGrantStore : IPersistedGrantStore
    {
        private readonly ConcurrentDictionary<string, PersistedGrant> _repository;

        readonly string _filePath = System.IO.Directory.GetCurrentDirectory() + "\\grantstore.txt";
        public MyPersistedGrantStore()
        {
            ConcurrentDictionary<string, PersistedGrant> dict = null;
            try
            {
                var vals = System.IO.File.ReadAllText(_filePath);
                if (!string.IsNullOrEmpty(vals))
                {
                    dict = Newtonsoft.Json.JsonConvert.DeserializeObject<ConcurrentDictionary<string, PersistedGrant>>(vals);
                }
            }
            catch
            {
            }
            _repository = dict ?? new ConcurrentDictionary<string, PersistedGrant>();
        }

        void UpdateStore()
        {
            //这里做测试写入到文件中，如果是实际使用应该写入到数据库/NoSql(Redis)中
            System.IO.File.WriteAllText(_filePath, Newtonsoft.Json.JsonConvert.SerializeObject(_repository));
        }

        public Task StoreAsync(PersistedGrant grant)
        {
            _repository[grant.Key] = grant;
            UpdateStore();
            return Task.FromResult(0);
        }

        public Task<PersistedGrant> GetAsync(string key)
        {
            PersistedGrant token;
            if (_repository.TryGetValue(key, out token))
            {
                return Task.FromResult(token);
            }

            return Task.FromResult<PersistedGrant>(null);
        }

        public Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            var query =
                from item in _repository
                where item.Value.SubjectId == subjectId
                select item.Value;

            var items = query.ToArray().AsEnumerable();
            return Task.FromResult(items);
        }

        public Task RemoveAsync(string key)
        {
            PersistedGrant val;
            _repository.TryRemove(key, out val);
            UpdateStore();
            return Task.FromResult(0);
        }

        public Task RemoveAllAsync(string subjectId, string clientId)
        {
            var query =
                from item in _repository
                where item.Value.ClientId == clientId &&
                    item.Value.SubjectId == subjectId
                select item.Key;

            var keys = query.ToArray();
            foreach (var key in keys)
            {
                PersistedGrant grant;
                _repository.TryRemove(key, out grant);
            }
            UpdateStore();
            return Task.FromResult(0);
        }

        public Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            var query =
                from item in _repository
                where item.Value.SubjectId == subjectId &&
                    item.Value.ClientId == clientId &&
                    item.Value.Type == type
                select item.Key;

            var keys = query.ToArray();
            foreach (var key in keys)
            {
                PersistedGrant grant;
                _repository.TryRemove(key, out grant);
            }
            UpdateStore();
            return Task.FromResult(0);
        }
    }
}
