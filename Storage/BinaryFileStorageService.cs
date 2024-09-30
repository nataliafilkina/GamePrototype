using Storage.Surrogate;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

namespace StorageService
{
    public class BinaryFileStorageService : IStorageService
    {
        private BinaryFormatter formatter;

        public BinaryFileStorageService()
        {
            InitBinaryFormatter();
        }

        private void InitBinaryFormatter()
        {
            formatter = new BinaryFormatter();
            var selector = new SurrogateSelector();
            var vector3Surrogate = new Vector3SerializationSurrogate();
            var quaternionSurrogate = new QuaternionSerializationSurrogate();

            selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3Surrogate);
            selector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), quaternionSurrogate);

            formatter.SurrogateSelector = selector;
        }

        public void Load<T>(string key,  Action<T> callback)
        {
            string path = BuildPath(key);
            T saveData = default;

            if (File.Exists(path))
            {
                using (var fileStream = new FileStream(path, FileMode.Open))
                {
                    saveData = (T)formatter.Deserialize(fileStream);
                }
            }
            callback.Invoke(saveData);
        }

        public void Save(string key, object data, Action<bool> callback = null)
        {
            FileStream fileStream = null;
            var path = BuildPath(key);

            try
            {
                fileStream = new FileStream(path, FileMode.Create);

                formatter.Serialize(fileStream, data);
                callback.Invoke(true);
            }
            catch
            {
                callback?.Invoke(false);
            }
            finally 
            {
                fileStream?.Close(); 
            }
        }

        private string BuildPath(string key)
        {
            return Path.Combine(UnityEngine.Application.persistentDataPath, key);
        }
    }
}
