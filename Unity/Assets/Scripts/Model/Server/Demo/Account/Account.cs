namespace ET
{
    public enum AccountType
    {
        /// <summary>
        /// 正常
        /// </summary>
        General,

        /// <summary>
        /// 封禁
        /// </summary>
        BlackList,

        /// <summary>
        /// 内部账号，禁止登录的情况下，内部账号允许登录
        /// </summary>
        Insider,

        /// <summary>
        /// GM账号，拥有管理权限
        /// </summary>
        GM
    }

    [ChildOf(typeof(Session))]
    public class Account: Entity,IAwake
    {
        /// <summary>
        /// 账号名称
        /// </summary>
        public string AccountName;

        /// <summary>
        ///
        /// </summary>
        public string Password;

        public AccountType AccountType;

        public long CreateTime;
    }
}