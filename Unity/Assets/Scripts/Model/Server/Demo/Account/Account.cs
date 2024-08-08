namespace ET
{
    public enum AccountType
    {
        General,
        BlackList,
    }

    [ChildOf(typeof(Session))]
    public class Account: Entity,IAwake
    {
        public string AccountName;

        public string Password;

        public AccountType AccountType;

        public long CreateTime;
    }
}