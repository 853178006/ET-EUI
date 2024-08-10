using ET.Server;

namespace ET
{
    [MessageSessionHandler(SceneType.Realm)]
    [FriendOfAttribute(typeof(ET.RoleInfo))]
    public class C2R_GetRolesHandler : MessageSessionHandler<C2R_GetRoles, R2C_GetRoles>
    {
        protected override async ETTask Run(Session session, C2R_GetRoles request, R2C_GetRoles response)
        {
            if (session.GetComponent<SessionLockingComponent>() != null)
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

            CoroutineLockComponent coroutineLockComponent = session.Root().GetComponent<CoroutineLockComponent>();
            using SessionLockingComponent sessionLockingComponent = session.AddComponent<SessionLockingComponent>();
            using CoroutineLock coroutineLock = await coroutineLockComponent.Wait(CoroutineLockType.CreateRole, request.Account.GetLongHashCode());

            DBComponent dbComponent = session.Root().GetComponent<DBManagerComponent>().GetZoneDB(session.Zone());
            var roleInfos = await dbComponent.Query<RoleInfo>(d =>
                    d.AccountName == request.Account && d.ServerId == request.ServerId && d.State == RoleInfoState.Normal);

            if (roleInfos==null || roleInfos.Count<=0)
            {
                return;
            }

            foreach (RoleInfo roleInfo in roleInfos)
            {
                response.RoleInfoProtoList.Add(roleInfo.ToMessage());
            }
        }
    }
}