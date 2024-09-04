namespace ET.Server
{
    [Invoke(TimerInvokeType.PlayerOfflineOutTime)]
    public class PlayerOfflineOutTime : ATimer<PlayerOfflineOutTimeComponent>
    {
        protected override void Run(PlayerOfflineOutTimeComponent t)
        {
            t?.KickPlayer();
        }
    }

    [EntitySystemOf(typeof(PlayerOfflineOutTimeComponent))]
    [FriendOfAttribute(typeof(ET.PlayerOfflineOutTimeComponent))]
    public static partial class PlayerOfflineOutTimeComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.PlayerOfflineOutTimeComponent self)
        {
            self.Timer = self.Root().GetComponent<TimerComponent>().NewOnceTimer(TimeInfo.Instance.ServerNow() + 10000, TimerInvokeType.PlayerOfflineOutTime, self);
        }
        [EntitySystem]
        private static void Destroy(this ET.PlayerOfflineOutTimeComponent self)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
        }

        public static void KickPlayer(this PlayerOfflineOutTimeComponent self)
        {
            DisConnectHelper.KickPlayer(self.GetParent<Player>()).Coroutine();
        }
    }
}