using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Nettention.Proud;

namespace DalmutiServer
{
    class DalmutiServer
    {
        public bool m_runLoop;

        private NetServer m_netServer = new NetServer();

        private Nettention.Proud.ThreadPool netWorkerThreadPool = new Nettention.Proud.ThreadPool(8);
        private Nettention.Proud.ThreadPool userWorkerThreadPool = new Nettention.Proud.ThreadPool(8);

        // RMI proxy for server-to-client messaging
        internal SocialGameS2C.Proxy m_S2CProxy = new SocialGameS2C.Proxy();
        internal SocialGameC2S.Stub m_C2SStub = new SocialGameC2S.Stub();

        // key = 마을 이름, value = 마을 객체인 마을 리스트
        private ConcurrentDictionary<String, Ville> m_villes = new ConcurrentDictionary<string, Ville>();
        // guides which client is in which ville.
        private ConcurrentDictionary<HostID, Ville> m_remoteClients = new ConcurrentDictionary<HostID, Ville>();

        private CommonS2C.Proxy _commonS2CProxy = new CommonS2C.Proxy();
        private CommonC2S.Stub _commonC2SStub = new CommonC2S.Stub();

        private ConcurrentDictionary<String, GameRoom> _roomsDict = new ConcurrentDictionary<string, GameRoom>();
        private ConcurrentDictionary<HostID, GameRoom> _remoteClients = new ConcurrentDictionary<HostID, GameRoom>();

        //private ConcurrentDictionary<String, >

        public DalmutiServer()
        {
            m_runLoop = true;

            m_netServer.AttachStub(m_C2SStub);
            m_netServer.AttachProxy(m_S2CProxy);

            m_netServer.AttachProxy(_commonS2CProxy);
            m_netServer.AttachStub(_commonC2SStub);

            // 클라이언트의 접속 요청에 대한 콜백 함수를 지정하는 delegate.
            m_netServer.ConnectionRequestHandler = (AddrPort clientAddr, ByteArray userDataFromClient, ByteArray reply) =>
            {
                reply = new ByteArray();
                reply.Clear();
                return true;
            };

            m_netServer.ClientHackSuspectedHandler = (HostID clientID, HackType hackType) =>
            {

            };

            // 클라이언트가 서버에 접속했을 때 발생하는 콜백 함수를 지정하는 delegate.
            m_netServer.ClientJoinHandler = (NetClientInfo clientInfo) =>
            {
                Console.WriteLine("OnClientJoin: {0}", clientInfo.hostID);
            };

            // 서버가 종료되었을 때 발생하는 콜백 함수를 지정하는 delegate. (클라이언트가 서버로부터 접속 종료 되었을 때 아닌지?)
            m_netServer.ClientLeaveHandler = (NetClientInfo clientInfo, ErrorInfo errorInfo, ByteArray comment) =>
            {
                Console.WriteLine("OnClientLeave: {0}", clientInfo.hostID);

                Monitor.Enter(this);

                //Ville ville;

                //// remove the client and play info, and then remove the ville if it is empty.
                //if (m_remoteClients.TryGetValue(clientInfo.hostID, out ville))
                //{
                //    RemoteClient clientValue;
                //    Ville villeValue;

                //    ville.m_players.TryRemove(clientInfo.hostID, out clientValue);
                //    m_remoteClients.TryRemove(clientInfo.hostID, out villeValue);

                //    if (ville.m_players.Count == 0)
                //    {
                //        UnloadVille(ville);
                //    }
                //}

                GameRoom gameRoom;

                if (_remoteClients.TryGetValue(clientInfo.hostID, out gameRoom))
                {
                    RemoteClient clientValue;
                    GameRoom gameRoomValue;

                    gameRoom.playersDict.TryRemove(clientInfo.hostID, out clientValue);
                    _remoteClients.TryRemove(clientInfo.hostID, out gameRoomValue);

                    Console.WriteLine("{0}: Left Players: {1}", gameRoom.roomKey, gameRoom.playersDict.Count);
                }

                Console.WriteLine("{0}: Left Players: {1}", gameRoom.roomKey, gameRoom.playersDict.Count);

                Monitor.Exit(this);
            };

            m_netServer.ErrorHandler = (ErrorInfo errorInfo) =>
            {
                Console.WriteLine("ONError! {0}", errorInfo.ToString());
            };

            m_netServer.WarningHandler = (ErrorInfo errorInfo) =>
            {
                Console.WriteLine("OnWarning! {0}", errorInfo.ToString());
            };

            m_netServer.ExceptionHandler = (Exception e) =>
            {
                Console.WriteLine("OnWarning! {0}", e.ToString());
            };

            m_netServer.InformationHandler = (ErrorInfo errorInfo) =>
            {
                Console.WriteLine("OnInformation! {0}", errorInfo.ToString());
            };

            m_netServer.NoRmiProcessedHandler = (RmiID rmiID) =>
            {
                Console.WriteLine("OnNoRmiProcessed! {0}", rmiID);
            };

            m_netServer.P2PGroupJoinMemberAckCompleteHandler = (HostID groupHostID, HostID memberHostID, ErrorType result) =>
            {

            };

            m_netServer.TickHandler = (object context) =>
            {

            };

            m_netServer.UserWorkerThreadBeginHandler = () =>
            {

            };

            m_netServer.UserWorkerThreadEndHandler = () =>
            {

            };

            //// 서버에서 로그인 요청 RMI를 받으면 이에 대해 응답하는 RMI를 클라이언트에 보냄
            //m_C2SStub.RequestLogon = (Nettention.Proud.HostID remote,Nettention.Proud.RmiContext rmiContext, String villeName, bool isNewVille) =>
            //{
            //    Monitor.Enter(this);

            //    Ville ville;
            //    HostID[] list = m_netServer.GetClientHostIDs();

            //    // 플레이어가 새로운 마을을 만들거나 기존의 마을 이름을 입력할 수 있음.
            //    /// 새로운 마을 이름이면 새 그룹 ID를 할당. 이게 방 개념인듯.
            //    if (!m_villes.TryGetValue(villeName, out ville))
            //    {
            //        // create new one
            //        ville = new Ville();
            //        ville.m_p2pGroupID = m_netServer.CreateP2PGroup(list, new ByteArray()); // empty P2P groups. players will join it.

            //        Console.WriteLine("m_p2pGroupID: {0}", ville.m_p2pGroupID);

            //        NetClientInfo info = m_netServer.GetClientInfo(list.Last());
            //        Console.WriteLine("Client HostID : {0}, IP:Port : {1}:{2}", info.hostID, info.tcpAddrFromServer.IPToString(), info.tcpAddrFromServer.port);

            //        // load ville info
            //        m_villes.TryAdd(villeName, ville);
            //        ville.m_name = villeName;

            //    }

            //    m_S2CProxy.ReplyLogon(remote, RmiContext.ReliableSend, (int)ville.m_p2pGroupID, 0, "");
            //    //m_S2CProxy.ReplyLogon(remote, RmiContext.ReliableSend, 0, 0, "Login Success");
            //    MoveRemoteClientToLoadedVille(remote, ville);

            //    Monitor.Exit(this);

            //    return true; // RMI 메시지 콜백 함수는 반드시 true를 리턴해야 함.
            //};

            _commonC2SStub.RequestLogon = (Nettention.Proud.HostID remote, Nettention.Proud.RmiContext rmiContext, String nickName, String roomName) =>
            {
                Monitor.Enter(this);

                GameRoom gameRoom;
                if (!_roomsDict.TryGetValue(roomName, out gameRoom)) // roomName에 일치하는 게임 룸이 없으면 생성.
                {
                    gameRoom = new GameRoom();

                    HostID[] list = m_netServer.GetClientHostIDs();
                    NetClientInfo info = m_netServer.GetClientInfo(list.Last());
                    Console.WriteLine("Client HostID : {0}, IP:Port : {1}:{2}", info.hostID, info.tcpAddrFromServer.IPToString(), info.tcpAddrFromServer.port);

                    _roomsDict.TryAdd(roomName, gameRoom);
                    gameRoom.roomKey = roomName;
                }

                _commonS2CProxy.ReplyLogon(remote, RmiContext.ReliableSend, 0, "Connection Success.");
                Console.WriteLine("Client({0}) connected a GameRoom:{1}", nickName, roomName);
                AddRemoteClientToGameRoom(remote, gameRoom);

                Monitor.Exit(this);

                return true;
            };
        }

        public void Start()
        {
            // fill server startup parameters
            StartServerParameter sp = new StartServerParameter();
            sp.protocolVersion = new Nettention.Proud.Guid(DalmutiCommon.Vars.g_dalmutiProtocolVersion);
            sp.tcpPorts = new IntArray();
            sp.tcpPorts.Add(DalmutiCommon.Vars.g_serverPort); // must be same to the port number at client
            sp.serverAddrAtClient = "192.168.0.28";
            sp.localNicAddr = "192.168.0.28";
            sp.SetExternalNetWorkerThreadPool(netWorkerThreadPool);
            sp.SetExternalUserWorkerThreadPool(userWorkerThreadPool);

            // let's start!
            m_netServer.Start(sp);
        }

        //private void UnloadVille(Ville ville)
        //{
        //    // ban the players in the ville
        //    foreach (KeyValuePair<HostID, RemoteClient> ipPlayer in ville.m_players)
        //    {
        //        m_netServer.CloseConnection(ipPlayer.Key);
        //    }

        //    Ville villeVaule;

        //    // shutdown the loaded ville
        //    m_villes.TryRemove(ville.m_name, out villeVaule);

        //    // release the cache data tree
        //    m_netServer.DestroyP2PGroup(ville.m_p2pGroupID);
        //}

        public void Dispose()
        {
            // NetServer의 경우 프로그램 종료 또는 NetServer 객체 파괴시 명시적으로 NetServer.Dispose() 를 호출해주어야 함.
            m_netServer.Dispose();
        }

        //public void MoveRemoteClientToLoadedVille(HostID remote, Ville ville)
        //{
        //    RemoteClient remoteClientValue;
        //    Ville villeValue;

        //    if (!ville.m_players.TryGetValue(remote, out remoteClientValue) && !m_remoteClients.TryGetValue(remote, out villeValue))
        //    {
        //        // 새로 마을에 들어온 클라이언트를 리스트에 추가
        //        ville.m_players.TryAdd(remote, new RemoteClient());
        //        m_remoteClients.TryAdd(remote, ville);

        //        // 같은 그룹 ID를 가진 클라이언트끼리 P2P 통신이 가능하도록 해줌.
        //        // now, the player can do P2P communication with other player in the same ville.
        //        m_netServer.JoinP2PGroup(remote, ville.m_p2pGroupID);

        //        // notify current world state to new user
        //        // Social game programming footage 4 Realtime collaborated farming at server plant 8:10
        //    }
        //}

        public void AddRemoteClientToGameRoom(HostID remote, GameRoom gameRoom)
        {
            RemoteClient remoteClientValue;
            GameRoom gameRoomValue;

            if (!gameRoom.playersDict.TryGetValue(remote, out remoteClientValue) && !_remoteClients.TryGetValue(remote, out gameRoomValue))
            {
                gameRoom.playersDict.TryAdd(remote, new RemoteClient());
                _remoteClients.TryAdd(remote, gameRoom);
            }

            Console.WriteLine("{0}: Current Players: {1}", gameRoom.roomKey, gameRoom.playersDict.Count);
        }

        static void Main(string[] args)
        {
            //NetServer netServer = new NetServer();

            //StartServerParameter param = new StartServerParameter();
            //param.tcpPorts.Add(35675);
            //param.udpPorts.Add(35675);

            Console.WriteLine("<ProudNet Realtime Dalmuti Game Server");
            Console.WriteLine("ESC: Quit");

            DalmutiServer server = new DalmutiServer();

            try
            {
                server.Start();
                Console.WriteLine("Server start ok.");

                while (server.m_runLoop)
                {
                    if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
                    {
                        break;
                    }

                    System.Threading.Thread.Sleep(1000);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}", e.Message.ToString());
            }
            finally
            {
                server.Dispose(); // Main Threaed 종료 시 Dispose 함수 호출.
            }
        }
    }
}
