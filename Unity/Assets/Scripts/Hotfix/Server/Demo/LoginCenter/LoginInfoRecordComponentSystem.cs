using System.Collections.Generic;

namespace ET.Server
{
    [EntitySystemOf(typeof(LoginInfoRecordComponent))]
    [FriendOfAttribute(typeof(ET.Server.LoginInfoRecordComponent))]
    public static partial class LoginInfoRecordComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Server.LoginInfoRecordComponent self)
        {

        }
        [EntitySystem]
        private static void Destroy(this ET.Server.LoginInfoRecordComponent self)
        {
            self.Clear();
        }

        public static void Add(this LoginInfoRecordComponent self, long key, int value)
        {
            if (self.AccountLoginInfoDict.TryAdd(key, value))
            {
                return;
            }

            self.AccountLoginInfoDict[key] = value;
        }

        public static void Remove(this LoginInfoRecordComponent self, long key)
        {
            if (!self.AccountLoginInfoDict.ContainsKey(key))
            {
                return;
            }

            self.AccountLoginInfoDict.Remove(key);
        }

        public static int Get(this LoginInfoRecordComponent self, long key)
        {
            return self.AccountLoginInfoDict.GetValueOrDefault(key, -1);
        }

        public static void Clear(this LoginInfoRecordComponent self)
        {
            self.AccountLoginInfoDict.Clear();
        }

        public static bool IsExist(this LoginInfoRecordComponent self, long key)
        {
            return self.AccountLoginInfoDict.ContainsKey(key);
        }
    }
}