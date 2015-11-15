using System.Collections;
using System.Collections.Generic;

namespace BrainTools
{
    internal class BiDictionary<T1, T2> : IDictionary<T1, T2>
    {
        Dictionary<T1, T2> dict1 = new Dictionary<T1, T2>();
        Dictionary<T2, T1> dict2 = new Dictionary<T2, T1>();

        public BiDictionary()
        { }

        public BiDictionary(IDictionary<T1, T2> dictionary)
        {
            foreach(KeyValuePair<T1,T2> kvp in dictionary)
                Add(kvp.Key, kvp.Value);
        }

        public void Add(T1 first, T2 second)
        {
            dict1.Add(first, second);
            dict2.Add(second, first);
        }

        public T2 this[T1 first]
        {
            get { return GetFromFirst(first); }
        }

        public T1 this[T2 second]
        {
            get { return GetFromSecond(second); }
        }

        public T2 GetFromFirst(T1 key)
        {
            return dict1[key];
        }

        public T1 GetFromSecond(T2 key)
        {
            return dict2[key];
        }


        #region IDictionary<Key, Value> Implementation

        public bool ContainsKey(T1 key)
        {
            return dict1.ContainsKey(key);
        }

        public bool ContainsKey(T2 key)
        {
            return dict2.ContainsKey(key);
        }

        public ICollection<T1> Keys
        {
            get { return dict1.Keys; }
        }

        public bool Remove(T1 key)
        {
            return dict1.Remove(key);
        }

        public bool Remove(T2 key)
        {
            return dict2.Remove(key);
        }

        public bool TryGetValue(T1 key, out T2 value)
        {
            return dict1.TryGetValue(key, out value);
        }

        public bool TryGetValue(T2 key, out T1 value)
        {
            return dict2.TryGetValue(key, out value);
        }

        public ICollection<T2> Values
        {
            get { return dict2.Keys; }
        }

        T2 IDictionary<T1, T2>.this[T1 key]
        {
            get { return dict1[key]; }
            set { dict1[key] = value; }
        }

        public void Add(KeyValuePair<T1, T2> item)
        {
            throw new System.NotImplementedException();
        }

        public void Add(KeyValuePair<T2, T1> item)
        {
            throw new System.NotImplementedException();
        }

        public void Clear()
        {
            dict1.Clear();
            dict2.Clear();
        }

        public bool Contains(KeyValuePair<T1, T2> item)
        {
            throw new System.NotImplementedException();
        }

        public void CopyTo(KeyValuePair<T1, T2>[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public int Count
        {
            get { return dict1.Count; }
        }

        public bool IsReadOnly
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool Remove(KeyValuePair<T1, T2> item)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}