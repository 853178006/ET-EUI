namespace ET
{
    [EntitySystemOf(typeof(AccountSessionsComponent))]
    [FriendOfAttribute(typeof(ET.AccountSessionsComponent))]
    public static partial class AccountSessionsComponentSystem
    {
        [EntitySystem]
        private static void Awake(this AccountSessionsComponent self)
        {

        }
        [EntitySystem]
        private static void Destroy(this ET.AccountSessionsComponent self)
        {
            self.Clear();
        }

        public static Session Get(this AccountSessionsComponent self, string accountName)
        {
            if (!self.AccountSessionDict.TryGetValue(accountName, out EntityRef<Session> session))
            {
                return null;
            }

            return session;
        }

        public static void Add(this AccountSessionsComponent self, string accountName, EntityRef<Session> session)
        {
            if (self.AccountSessionDict.TryAdd(accountName, session))
            {
                return;
            }

            self.AccountSessionDict[accountName] = session;
        }

        public static void Remove(this AccountSessionsComponent self, string accountName)
        {
            if (!self.AccountSessionDict.ContainsKey(accountName))
            {
                return;
            }

            self.AccountSessionDict.Remove(accountName);
        }

        public static void Clear(this AccountSessionsComponent self)
        {
            self.AccountSessionDict.Clear();
        }
    }
}