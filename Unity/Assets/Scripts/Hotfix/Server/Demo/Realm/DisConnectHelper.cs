namespace ET.Server
{
    public static class DisConnectHelper
    {
        public static async ETTask DisConnect(this Session self)
        {
            if (self == null || self.IsDisposed)
            {
                return;
            }

            long instanceId = self.InstanceId;

            TimerComponent timerComponent = self.Root().GetComponent<TimerComponent>();
            await timerComponent.WaitAsync(1000);

            if (self.InstanceId != instanceId)
            {
                return;
            }

            self.Dispose();
        }

        public static async ETTask KickPlayerNoLock(Player player)
        {
            await ETTask.CompletedTask;
        }

        public static async ETTask KickPlayer(Player player)
        {
            if (player == null || player.IsDisposed)
            {
                return;
            }

            long instanceId = player.InstanceId;

            CoroutineLockComponent coroutineLockComponent = player.Root().GetComponent<CoroutineLockComponent>();
            using (await coroutineLockComponent.Wait(CoroutineLockType.LoginGate,player.Account.GetLongHashCode()))
            {
                if (player.IsDisposed || instanceId != player.InstanceId)
                {
                    return;
                }

                await KickPlayerNoLock(player);
            }
        }
    }
}