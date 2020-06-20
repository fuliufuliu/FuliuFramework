using System.Collections.Generic;

namespace fuliu
{
    public struct Keys<TKey0, TKey1>
    {
        public TKey0 key0;
        public TKey1 key1;
        
        public Keys(TKey0 t0, TKey1 t1)
        {
            key0 = t0;
            key1 = t1;
        }
    }
    
    public struct Keys<TKey0, TKey1, TKey2>
    {
        public TKey0 key0;
        public TKey1 key1;
        public TKey2 key2;
        
        public Keys(TKey0 t0, TKey1 t1, TKey2 t2)
        {
            key0 = t0;
            key1 = t1;
            key2 = t2;
        }
    }
    
    
    public class MultiKeyDictionary<TKey0, TKey1, TValue>: Dictionary<Keys<TKey0, TKey1>, TValue>
    {
        void Test()
        {
            var a = new MultiKeyDictionary<int, int, string>();
            a[1, 1] = "";
        }

        private TValue this[TKey0 key0, TKey1 key1]
        {
            get
            {
                var newKey = new Keys<TKey0, TKey1>(key0, key1);
                if (ContainsKey(newKey))
                {
                    return this[newKey];
                }

                return default;
            }
            set
            {
                var newKey = new Keys<TKey0, TKey1>(key0, key1);
                this[newKey] = value;
            }
        }

        public bool Containskey(TKey0 key0, TKey1 key1)
        {
            return ContainsKey(new Keys<TKey0, TKey1>(key0, key1));
        }
    }
    
    public class MultiKeyDictionary<TKey0, TKey1, TKey2, TValue>: Dictionary<Keys<TKey0, TKey1, TKey2>, TValue>
    {
        private TValue this[TKey0 key0, TKey1 key1, TKey2 key2]
        {
            get
            {
                var newKey = new Keys<TKey0, TKey1, TKey2>(key0, key1, key2);
                if (ContainsKey(newKey))
                {
                    return this[newKey];
                }

                return default;
            }
            set
            {
                var newKey = new Keys<TKey0, TKey1, TKey2>(key0, key1, key2);
                this[newKey] = value;
            }
        }

        public bool Containskey(TKey0 key0, TKey1 key1, TKey2 key2)
        {
            return ContainsKey(new Keys<TKey0, TKey1, TKey2>(key0, key1, key2));
        }
    }
}