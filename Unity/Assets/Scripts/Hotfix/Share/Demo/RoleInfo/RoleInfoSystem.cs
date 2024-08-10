namespace ET
{
    [EntitySystemOf(typeof(RoleInfo))]
    [FriendOfAttribute(typeof(ET.RoleInfo))]
    public static class RoleInfoSystem
    {
        [EntitySystem]
        private static void Awake(this ET.RoleInfo self)
        {

        }
        [EntitySystem]
        private static void Destroy(this ET.RoleInfo self)
        {
            self.Clear();
        }

        public static void FromMessage(this RoleInfo self, RoleInfoProto infoProto)
        {
            self.RoleName = infoProto.Name;
            self.State = (RoleInfoState)infoProto.State;
            self.AccountName = infoProto.AccountName;
            self.CreateTime = infoProto.CreateTime;
            self.ServerId = infoProto.ServerId;
            self.LastLoginTime = infoProto.LastLoginTime;
        }

        public static RoleInfoProto ToMessage(this RoleInfo self)
        {
            RoleInfoProto infoProto = RoleInfoProto.Create();
            infoProto.Id = self.Id;
            infoProto.Name = self.RoleName;
            infoProto.State = (int)self.State;
            infoProto.AccountName = self.AccountName;
            infoProto.CreateTime = self.CreateTime;
            infoProto.ServerId = self.ServerId;
            infoProto.LastLoginTime = self.LastLoginTime;
            return infoProto;
        }

        public static void Clear(this RoleInfo self)
        {
            self.RoleName = default;
            self.State = default;
            self.AccountName = default;
            self.CreateTime = default;
            self.ServerId = default;
            self.LastLoginTime = default;
        }
    }
}