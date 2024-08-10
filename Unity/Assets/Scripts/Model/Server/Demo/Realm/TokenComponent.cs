using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class TokenComponent: Entity,IAwake
    {
        public readonly Dictionary<string, string> TokenDict = new Dictionary<string, string>();
    }
}