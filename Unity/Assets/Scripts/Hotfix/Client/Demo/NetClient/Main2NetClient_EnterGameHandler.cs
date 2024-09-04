namespace ET.Client
{
    [MessageHandler(SceneType.NetClient)]
    public class Main2NetClient_EnterGameHandler: MessageHandler<Scene, Main2NetClient_EnterGame,NetClient2Main_EnterGame>
    {
        protected override async ETTask Run(Scene scene, Main2NetClient_EnterGame request, NetClient2Main_EnterGame response)
        {
            string account = request.Account;
            //创建一个gate session，保存在SessionComponent
            NetComponent netComponent=scene.GetComponent<NetComponent>();
            Session gateSession = await netComponent.CreateRouterSession(NetworkHelper.ToIPEndPoint(request.GateAddress), account, account);
            gateSession.AddComponent<ClientSessionErrorComponent>();
            scene.GetComponent<SessionComponent>().Session = gateSession;

            C2G_LoginGameGate c2GLoginGameGate=C2G_LoginGameGate.Create();
            c2GLoginGameGate.Key = request.RealmKey;
            c2GLoginGameGate.Account=request.Account;
            c2GLoginGameGate.RoleId=request.RoleId;
            G2C_LoginGameGate g2CLoginGameGate=await gateSession.Call(c2GLoginGameGate) as G2C_LoginGameGate;

            if (g2CLoginGameGate.Error!=ErrorCode.ERR_Success)
            {
                response.Error=g2CLoginGameGate.Error;
                Log.Error($"登录Gate失败！！！{g2CLoginGameGate}");
                return;
            }

            Log.Debug($"登录gate成功！！！");

            G2C_EnterGame g2CEnterGame=await gateSession.Call(C2G_EnterGame.Create()) as G2C_EnterGame;
            if (g2CEnterGame.Error!=ErrorCode.ERR_Success)
            {
                response.Error = g2CEnterGame.Error;
                Log.Error($"登录map失败！！！{g2CEnterGame.Error}");
                return;
            }

            Log.Debug($"登录map成功！！！");

            response.PlayerId = g2CEnterGame.MyUnitId;
        }
    }
}