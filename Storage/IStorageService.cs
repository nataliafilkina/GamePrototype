using System;
using Zenject;

namespace StorageService
{
    public interface IStorageService //: IInitializable
    {
        void Save(string key, object data, Action<bool> callback = null);
        void Load<T>(string key, Action<T> callback);
    }
}
