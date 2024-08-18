namespace ET.Server
{
    [MessageHandler(SceneType.LoginCenter)]
    public class R2L_LoginAccountRequestHandler: MessageHandler<Scene, R2L_LoginAccountRequest,L2R_LoginAccountRequest>
    {
        protected override async ETTask Run(Scene scene, R2L_LoginAccountRequest request, L2R_LoginAccountRequest response)
        {
            long accountId = request.AccountName.GetLongHashCode();

            CoroutineLockComponent coroutineLockComponent = scene.GetComponent<CoroutineLockComponent>();
            using (await coroutineLockComponent.Wait(CoroutineLockType.LoginCenterLock,accountId.GetHashCode()))
            {
                if (!scene.GetComponent<LoginInfoRecordComponent>().IsExist(accountId))
                {
                    return;
                }

                int zone = scene.GetComponent<LoginInfoRecordComponent>().Get(accountId);
                StartSceneConfig gateConfig = RealmGateAddressHelper.GetGate(zone, request.AccountName);

                L2R_DisConnectGateUnit l2RDisConnectGateUnit = L2R_DisConnectGateUnit.Create();
                l2RDisConnectGateUnit.AccountName = request.AccountName;
                var g2LDisconnectGateUnit = await scene.GetComponent<MessageSender>().Call(gateConfig.ActorId, l2RDisConnectGateUnit) as R2L_DisConnectGateUnit;

                response.Error = g2LDisconnectGateUnit.Error;
            }
        }
    }
}