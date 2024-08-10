namespace ET
{
    [EntitySystemOf(typeof(ServerInfo))]
    [FriendOfAttribute(typeof(ET.ServerInfo))]
    public static partial class ServerInfoSystem
    {
        [EntitySystem]
        private static void Awake(this ET.ServerInfo self)
        {

        }

        public static void FromMessage(this ServerInfo self, ServerInfoProto infoProto)
        {
            self.Status = infoProto.Status;
            self.ServerName = infoProto.ServerName;
        }

        public static ServerInfoProto ToMessage(this ServerInfo self)
        {
            ServerInfoProto message = ServerInfoProto.Create();
            message.Id = (int)self.Id;
            message.Status = self.Status;
            message.ServerName = self.ServerName;
            return message;
        }
    }
}