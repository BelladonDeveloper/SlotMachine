using System.Collections.Generic;
using UnityEngine;

namespace Base
{
    public static class Register
    {
        private static readonly Dictionary<string, object> SInstanceByKey = new ();

        public static void Add<T>(T instance) where T : class
        {
            string key = GetKey<T>();
            Add(key, instance);
        }
        
        private static string GetKey<T>() where T : class
        {
            return typeof(T).ToString();
        }
        
        private static void Add(string key, object instance)
        {
            if (!SInstanceByKey.TryAdd(key, instance))
                Debug.LogError("This is already registered!");
        }

        public static T Get<T>() where T : class
        {
            string key = GetKey<T>();
            return Get<T>(key);
        }

        private static T Get<T>(string key) where T : class
        {
            if (SInstanceByKey.TryGetValue(key, out object instance))
                return instance as T;
            
            Debug.LogError($"{key} not registered!");
            return null;
        }

        public static void Remove<T>(T instance) where T : class
        {
            string key = GetKey<T>();
            
            if (SInstanceByKey.ContainsKey(key))
                SInstanceByKey.Remove(key);
            else
                Debug.Log($"{instance} not registered or removed already!");
        }
    }
}