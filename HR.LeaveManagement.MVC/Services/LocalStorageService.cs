using System.Collections.Generic;
using Hanssens.Net;
using HR.LeaveManagement.MVC.Contracts;

namespace HR.LeaveManagement.MVC.Services
{
    public class LocalStorageService : ILocalStorageService
    {
        private readonly LocalStorage _localStorage;

        public LocalStorageService()
        {
            var config = new LocalStorageConfiguration()
            {
                AutoLoad = true,
                AutoSave = true,
                Filename = "HR.LEAVEMGMT"
            };

            _localStorage = new LocalStorage(config);
        }

        public void ClearStorage(List<string> keys)
        {
            foreach (var key in keys)
            {
                _localStorage.Remove(key);
            }
        }

        public bool Exist(string key)
        {
            return _localStorage.Exists(key);
        }

        public T GetStorageValue<T>(string key)
        {
            return _localStorage.Get<T>(key);
        }

        public void SetStorageValue<T>(string key, T value)
        {
            _localStorage.Store(key, value);
            _localStorage.Persist();
        }
    }
}
