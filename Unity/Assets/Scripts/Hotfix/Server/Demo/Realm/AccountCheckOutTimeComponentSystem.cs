using ET.Server;

namespace ET
{
    [Invoke(TimerInvokeType.AccountSessionCheckOutTime)]
    public class AccountSessionCheckOutTimer : ATimer<AccountCheckOutTimeComponent>
    {
        protected override void Run(AccountCheckOutTimeComponent t)
        {
            t?.DeleteSession();
        }
    }

    [EntitySystemOf(typeof(AccountCheckOutTimeComponent))]
    [FriendOfAttribute(typeof(ET.AccountCheckOutTimeComponent))]
    public static partial class AccountCheckOutTimeComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.AccountCheckOutTimeComponent self, string accountName)
        {
            self.Init(accountName);
        }
        [EntitySystem]
        private static void Destroy(this ET.AccountCheckOutTimeComponent self)
        {
            self.Clear();
        }

        public static void Init(this AccountCheckOutTimeComponent self, string accountName)
        {
            self.AccountName = accountName;
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
            self.Timer = self.Root().GetComponent<TimerComponent>()
                    .NewOnceTimer(TimeInfo.Instance.ServerNow() + 600000, TimerInvokeType.AccountSessionCheckOutTime, self);
        }

        public static void Clear(this AccountCheckOutTimeComponent self)
        {
            self.AccountName = default;
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
        }

        public static void DeleteSession(this AccountCheckOutTimeComponent self)
        {
            Session session = self.GetParent<Session>();

            Session originSession = session.Root().GetComponent<AccountSessionsComponent>().Get(self.AccountName);
            if (originSession!=null && session.InstanceId==originSession.InstanceId)
            {
                session.Root().GetComponent<AccountSessionsComponent>().Remove(self.AccountName);
            }

            A2C_DisConnect a2CDisConnect = A2C_DisConnect.Create();
            a2CDisConnect.Error = 1;
            session?.Send(a2CDisConnect);
            session?.DisConnect().Coroutine();
        }
    }
}