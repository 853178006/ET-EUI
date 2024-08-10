using ET.Server;

namespace ET
{
    [MessageSessionHandler(SceneType.Realm)]
    public class C2R_CreateRoleHandler: MessageSessionHandler<C2R_CreateRole,R2C_CreateRole>
    {
        protected override async ETTask Run(Session session, C2R_CreateRole request, R2C_CreateRole response)
        {
            if (session.GetComponent<SessionLockingComponent>()!=null)
            {
                response.Error = ErrorCode.ERR_RequestRepeatedly;
                session.DisConnect().Coroutine();
                return;
            }

            string token = session.Root().GetComponent<TokenComponent>().Get(request.Account);

            if (token == null || token != request.Token)
            {
                response.Error = ErrorCode.ERR_TokenError;
                session.DisConnect().Coroutine();
                return;
            }
        }
    }
}