namespace ET
{
    public enum RoleInfoState
    {
        Normal,
        Freeze,
    }

    [ChildOf]
    public class RoleInfo: Entity,IAwake,IDestroy
    {
        public string RoleName;
        public RoleInfoState State;
        public string AccountName;
        public long LastLoginTime;
        public long CreateTime;
        public int ServerId;
    }
}