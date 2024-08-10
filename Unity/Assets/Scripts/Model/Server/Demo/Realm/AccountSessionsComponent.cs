using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class AccountSessionsComponent: Entity,IAwake,IDestroy
    {
        public Dictionary<string, EntityRef<Session>> AccountSessionDict = new Dictionary<string, EntityRef<Session>>();
    }
}