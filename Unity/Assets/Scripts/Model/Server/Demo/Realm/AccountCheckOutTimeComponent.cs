namespace ET
{
    [ComponentOf(typeof(Session))]
    public class AccountCheckOutTimeComponent: Entity,IAwake<string>,IDestroy
    {
        public string AccountName;

        public long Timer;
    }
}