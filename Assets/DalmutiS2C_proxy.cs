﻿




// Generated by PIDL compiler.
// Do not modify this file, but modify the source .pidl file.

using System;
using System.Net;

namespace SocialGameS2C
{
	internal class Proxy:Nettention.Proud.RmiProxy
	{
public bool ReplyLogon(Nettention.Proud.HostID remote,Nettention.Proud.RmiContext rmiContext, int groupID, int result, String comment)
{
	Nettention.Proud.Message __msg=new Nettention.Proud.Message();
		__msg.SimplePacketMode = core.IsSimplePacketMode();
		Nettention.Proud.RmiID __msgid= Common.ReplyLogon;
		__msg.Write(__msgid);
		Nettention.Proud.Marshaler.Write(__msg, groupID);
		Nettention.Proud.Marshaler.Write(__msg, result);
		Nettention.Proud.Marshaler.Write(__msg, comment);
		
	Nettention.Proud.HostID[] __list = new Nettention.Proud.HostID[1];
	__list[0] = remote;
		
	return RmiSend(__list,rmiContext,__msg,
		RmiName_ReplyLogon, Common.ReplyLogon);
}

public bool ReplyLogon(Nettention.Proud.HostID[] remotes,Nettention.Proud.RmiContext rmiContext, int groupID, int result, String comment)
{
	Nettention.Proud.Message __msg=new Nettention.Proud.Message();
__msg.SimplePacketMode = core.IsSimplePacketMode();
Nettention.Proud.RmiID __msgid= Common.ReplyLogon;
__msg.Write(__msgid);
Nettention.Proud.Marshaler.Write(__msg, groupID);
Nettention.Proud.Marshaler.Write(__msg, result);
Nettention.Proud.Marshaler.Write(__msg, comment);
		
	return RmiSend(remotes,rmiContext,__msg,
		RmiName_ReplyLogon, Common.ReplyLogon);
}
#if USE_RMI_NAME_STRING
// RMI name declaration.
// It is the unique pointer that indicates RMI name such as RMI profiler.
public const string RmiName_ReplyLogon="ReplyLogon";
       
public const string RmiName_First = RmiName_ReplyLogon;
#else
// RMI name declaration.
// It is the unique pointer that indicates RMI name such as RMI profiler.
public const string RmiName_ReplyLogon="";
       
public const string RmiName_First = "";
#endif
		public override Nettention.Proud.RmiID[] GetRmiIDList() { return Common.RmiIDList; } 
	}
}
