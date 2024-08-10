namespace ET.Client
{
    public static class LoginHelper
    {
        public static async ETTask Login(Scene root, string account, string password)
        {
            //游戏客户端发送网络消息的组件，移除之后重新挂载是为了保证和之前的连接没有任何关系
            root.RemoveComponent<ClientSenderComponent>();
            ClientSenderComponent clientSenderComponent = root.AddComponent<ClientSenderComponent>();

            NetClient2Main_Login response = await clientSenderComponent.LoginAsync(account, password);
            if (response.Error!=ErrorCode.ERR_Success)
            {
                Log.Error($"请求登录失败，返回错误:{response.Error}");
                return;
            }

            Log.Debug($"请求登录成功！！！");
            string token = response.Token;

            // root.GetComponent<PlayerComponent>().MyId = response.PlayerId;

            //获取服务器列表
            C2R_GetServerInfos c2RGetServerInfos = C2R_GetServerInfos.Create();
            c2RGetServerInfos.Account = account;
            c2RGetServerInfos.Token = response.Token;
            R2C_GetServerInfos r2CGetServerInfos=await clientSenderComponent.Call(c2RGetServerInfos) as R2C_GetServerInfos;
            if (r2CGetServerInfos.Error!=ErrorCode.ERR_Success)
            {
                Log.Error($"请求服务器列表失败");
                return;
            }

            //临时逻辑，默认取第一个区服
            ServerInfoProto serverInfoProto = r2CGetServerInfos.ServerInfoProtoList[0];
            Log.Debug($"请求服务器列表成功，区服名称：{serverInfoProto.ServerName} 区服ID：{serverInfoProto.Id}");

            //获取区服角色列表
            C2R_GetRoles c2RGetRoles = C2R_GetRoles.Create();
            c2RGetRoles.Token = token;
            c2RGetRoles.Account = account;
            c2RGetRoles.ServerId = serverInfoProto.Id;

            R2C_GetRoles r2CGetRoles=await clientSenderComponent.Call(c2RGetRoles) as R2C_GetRoles;
            if (r2CGetRoles.Error!=ErrorCode.ERR_Success)
            {
                Log.Error($"请求区服列表失败");
                return;
            }

            RoleInfoProto roleInfoProto = default;
            if (r2CGetRoles.RoleInfoProtoList.Count<=0)
            {
                //无角色信息，创建角色信息

            }

            await EventSystem.Instance.PublishAsync(root, new LoginFinish());
        }
    }
}