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

            await EventSystem.Instance.PublishAsync(root, new LoginFinish());
        }
    }
}