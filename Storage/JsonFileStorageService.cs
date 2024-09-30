using Newtonsoft.Json;
using System;
using System.IO;

namespace StorageService
{
    public class JsonFileStorageService : IStorageService
    {
        public void Load<T>(string key, Action<T> callback)
        {
            string path = BuildPath(key);

            if (File.Exists(path))
            {
                using (var fileStream = new StreamReader(path))
                {
                    var json = fileStream.ReadToEnd();
                    var data = JsonConvert.DeserializeObject<T>(json);

                    callback.Invoke(data);
                }
            }
            else 
                callback.Invoke(default);
        }

        public void Save(string key, object data, Action<bool> callback = null)
        {
            var path = BuildPath(key);
            string json = JsonConvert.SerializeObject(data);

            using(var fileStream = new StreamWriter(path))
            {
                fileStream.Write(json);
            }

            callback?.Invoke(true);
        }

        private string BuildPath(string key)
        {
            return Path.Combine(UnityEngine.Application.persistentDataPath, key);
        }
    }
}
