using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalmutiClient // Proxy와 Stub에 정의된 대로 Marshaler의 namespace를 고쳐준다. 이게 marshaler(cs)='이름' 옵션에 들어가는 애인듯. 이렇게 할 경우 DalmutiClient.Marshaler로 호출하는 듯.
    // 서버랑 클라에 완전 같은 코드가 추가된다.
{
    public class Marshaler
    {
        // Nettention.Proud.Marshaler 안에 기본적인 Marshaler 함수가 정의되어 있음.
        // 3D 벡터 타입을 위한 Marshaler 함수를 추가한다.
        public static void Write(Nettention.Proud.Message msg, UnityEngine.Vector3 b)
        {
            msg.Write(b.x);
            msg.Write(b.y);
            msg.Write(b.z);
        }

        public static void Read(Nettention.Proud.Message msg, out UnityEngine.Vector3 b)
        {
            b = new UnityEngine.Vector3();
            msg.Read(out b.x);
            msg.Read(out b.y);
            msg.Read(out b.z);
        }
    }
}
