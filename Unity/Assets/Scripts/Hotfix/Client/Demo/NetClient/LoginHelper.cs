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
                C2R_CreateRole c2RCreateRole = C2R_CreateRole.Create();
                c2RCreateRole.Token = token;
                c2RCreateRole.Account = account;
                c2RCreateRole.ServerId = serverInfoProto.Id;
                c2RCreateRole.Name = account;

                R2C_CreateRole r2CCreateRole=await clientSenderComponent.Call(c2RCreateRole) as R2C_CreateRole;

                if (r2CCreateRole.Error!=ErrorCode.ERR_Success)
                {
                    Log.Error($"创建区服角色失败");
                    return;
                }

                roleInfoProto = r2CCreateRole.RoleInfoProto;
            }
            else
            {
                roleInfoProto = r2CGetRoles.RoleInfoProtoList[0];
            }

            //请求获取RealmKey
            C2R_GetRealmKey c2RGetRealmKey = C2R_GetRealmKey.Create();
            c2RGetRealmKey.Token = token;
            c2RGetRealmKey.Account = account;
            c2RGetRealmKey.ServerId = serverInfoProto.Id;
            R2C_GetRealmKey r2CGetRealmKey=await clientSenderComponent.Call(c2RGetRealmKey) as R2C_GetRealmKey;

            if (r2CGetRealmKey.Error!=ErrorCode.ERR_Success)
            {
                Log.Error("获取RealmKey失败");
                return;
            }

            //请求游戏角色进入地图
            NetClient2Main_EnterGame netClient2Main_EnterGame = NetClient2Main_EnterGame.Create();
            if (netClient2Main_EnterGame.Error!=ErrorCode.ERR_Success)
            {
                Log.Error($"进入游戏失败：{netClient2Main_EnterGame.Error}");
                return;
            }




            await EventSystem.Instance.PublishAsync(root, new LoginFinish());
        }
    }
}