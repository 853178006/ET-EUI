using System;

namespace ET
{
    [EntitySystemOf(typeof(TokenComponent))]
    [FriendOfAttribute(typeof(ET.TokenComponent))]
    public static partial class TokenComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.TokenComponent self)
        {

        }

        public static void Add(this TokenComponent self, string key, string token)
        {
            self.TokenDict.Add(key, token);
            self.TimeOutRemoveKey(key, token).Coroutine();
        }

        public static string Get(this TokenComponent self, string key)
        {
            string value = String.Empty;
            self.TokenDict.TryGetValue(key, out key);
            return value;
        }

        public static void Remove(this TokenComponent self, string key)
        {
            if (!self.TokenDict.ContainsKey(key))
            {
                return;
            }

            self.TokenDict.Remove(key);
        }

        public static async ETTask TimeOutRemoveKey(this TokenComponent self, string key, string token)
        {
            await self.Root().GetComponent<TimerComponent>().WaitAsync(60000);
            string onlineToken = self.Get(key);

            if (!string.IsNullOrEmpty(onlineToken) && onlineToken == token)
            {
                self.Remove(key);
            }
        }

    }
}