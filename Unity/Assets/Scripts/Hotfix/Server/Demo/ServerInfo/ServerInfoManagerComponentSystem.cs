namespace ET.Server
{
    [EntitySystemOf(typeof(ServerInfoManagerComponent))]
    [FriendOfAttribute(typeof(ET.Server.ServerInfoManagerComponent))]
    [FriendOfAttribute(typeof(ET.ServerInfo))]
    public static partial class ServerInfoManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Server.ServerInfoManagerComponent self)
        {
            self.Load();
        }
        [EntitySystem]
        private static void Destroy(this ET.Server.ServerInfoManagerComponent self)
        {
            self.Clear();
        }

        public static void Load(this ServerInfoManagerComponent self)
        {
            foreach (var serverInfoRef in self.ServerInfos)
            {
                ServerInfo serverInfo = serverInfoRef;
                serverInfo?.Dispose();
            }
            self.ServerInfos.Clear();

            var serverInfoConfigs = StartZoneConfigCategory.Instance.GetAll();

            foreach (StartZoneConfig config in serverInfoConfigs.Values)
            {
                if (config.ZoneType != 1)
                {
                    continue;
                }

                ServerInfo newServerInfo = self.AddChildWithId<ServerInfo>(config.Id);
                newServerInfo.ServerName = config.DBName;
                newServerInfo.Status = (int)ServerStatus.Normal;
                self.ServerInfos.Add(newServerInfo);
            }
        }

        public static void Clear(this ServerInfoManagerComponent self)
        {
            foreach (var serverInfoRef in self.ServerInfos)
            {
                ServerInfo serverInfo = serverInfoRef;
                serverInfo?.Dispose();
            }
            self.ServerInfos.Clear();
        }
    }
}