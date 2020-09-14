using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nettention.Proud;

namespace DalmutiServer
{
    /// <summary>
    /// 게임 룸 정보. 룸에서 룸 자체의 설정과 플레이 할 게임과 그것의 설정을 담당.
    /// </summary>
    public class GameRoom
    {
        public String roomKey;

        // the players who are online. // 스레드 안정성을 제공하는 C#의 ConcurrentDictionary를 이용해서 RemoteClient 리스트 변수를 정의.
        public ConcurrentDictionary<HostID, RemoteClient> playersDict = new ConcurrentDictionary<HostID, RemoteClient>();
    }
}
