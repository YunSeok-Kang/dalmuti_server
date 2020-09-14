using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nettention.Proud;

namespace DalmutiServer
{
    public class Ville
    {
        public Ville()
        {
            m_nextNewID = 1;
            m_p2pGroupID = HostID.HostID_None;
        }

        // the players who are online. // 스레드 안정성을 제공하는 C#의 ConcurrentDictionary를 이용해서 RemoteClient 리스트 변수를 정의.
        public ConcurrentDictionary<HostID, RemoteClient> m_players = new ConcurrentDictionary<HostID, RemoteClient>();

        // ville name
        public String m_name;

        // increases for every new world object is added.
        // this value is saved to database, too.
        public int m_nextNewID;

        // world objects // world Object 리스트를 정의. 키로 고유한 ID 번호를 갖게 됨.
        //public ConcurrentDictionary<int, >

        // every players in this ville are P2P communicated.
        // this is useful for lesser latency for cloud-hosting servers (e.g. Amazon AWS)
        // P2P 그룹 변수 정의. 같은 마을에 있는 모든 플레이어는 peer-to-peer로 연결됨.
        // 반드시 P2P 통신을 해야 하는 것은 아니지만 마을 안의 플레이어들 간의 통신에서 사용됨.
        public HostID m_p2pGroupID;
    }
}
