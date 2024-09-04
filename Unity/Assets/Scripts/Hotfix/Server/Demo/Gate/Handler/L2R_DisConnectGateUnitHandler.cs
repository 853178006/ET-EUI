namespace ET.Server
{
    [MessageHandler(SceneType.Gate)]
    public class L2R_DisConnectGateUnitHandler: MessageHandler<Scene, L2G_DisConnectGateUnit,G2L_DisConnectGateUnit>
    {
        protected override async ETTask Run(Scene scene, L2G_DisConnectGateUnit request, G2L_DisConnectGateUnit response)
        {
            CoroutineLockComponent coroutineLockComponent=scene.GetComponent<CoroutineLockComponent>();
            using (await coroutineLockComponent.Wait(CoroutineLockType.LoginGate,request.AccountName.GetLongHashCode()))
            {
                PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
                Player player=playerComponent.GetByAccount(request.AccountName);

                if (player == null)
                {
                    return;
                }

                scene.GetComponent<GateSessionKeyComponent>().Remove(request.AccountName.GetLongHashCode());

                Session gateSession = player.GetComponent<PlayerSessionComponent>()?.Session;
                if (gateSession!=null && !gateSession.IsDisposed)
                {
                    A2C_DisConnect a2CDisConnect = A2C_DisConnect.Create();
                    a2CDisConnect.Error = ErrorCode.ERR_OtherAccountLogin;
                    gateSession.Send(a2CDisConnect);
                    gateSession?.DisConnect().Coroutine();
                }

                if (player.GetComponent<PlayerSessionComponent>()?.Session!=null)
                {
                    player.GetComponent<PlayerSessionComponent>().Session = null;
                }

                player.AddComponent<PlayerOfflineOutTimeComponent>();
            }
        }
    }
}