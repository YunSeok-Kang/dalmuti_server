﻿




// Generated by PIDL compiler.
// Do not modify this file, but modify the source .pidl file.

using System;
using System.Net;

namespace SocialGameC2S
{
	internal class Proxy:Nettention.Proud.RmiProxy
	{
public bool RequestLogon(Nettention.Proud.HostID remote,Nettention.Proud.RmiContext rmiContext, String villeName, bool isNewVille)
{
	Nettention.Proud.Message __msg=new Nettention.Proud.Message();
		__msg.SimplePacketMode = core.IsSimplePacketMode();
		Nettention.Proud.RmiID __msgid= Common.RequestLogon;
		__msg.Write(__msgid);
		Nettention.Proud.Marshaler.Write(__msg, villeName);
		Nettention.Proud.Marshaler.Write(__msg, isNewVille);
		
	Nettention.Proud.HostID[] __list = new Nettention.Proud.HostID[1];
	__list[0] = remote;
		
	return RmiSend(__list,rmiContext,__msg,
		RmiName_RequestLogon, Common.RequestLogon);
}

public bool RequestLogon(Nettention.Proud.HostID[] remotes,Nettention.Proud.RmiContext rmiContext, String villeName, bool isNewVille)
{
	Nettention.Proud.Message __msg=new Nettention.Proud.Message();
__msg.SimplePacketMode = core.IsSimplePacketMode();
Nettention.Proud.RmiID __msgid= Common.RequestLogon;
__msg.Write(__msgid);
Nettention.Proud.Marshaler.Write(__msg, villeName);
Nettention.Proud.Marshaler.Write(__msg, isNewVille);
		
	return RmiSend(remotes,rmiContext,__msg,
		RmiName_RequestLogon, Common.RequestLogon);
}
#if USE_RMI_NAME_STRING
// RMI name declaration.
// It is the unique pointer that indicates RMI name such as RMI profiler.
public const string RmiName_RequestLogon="RequestLogon";
       
public const string RmiName_First = RmiName_RequestLogon;
#else
// RMI name declaration.
// It is the unique pointer that indicates RMI name such as RMI profiler.
public const string RmiName_RequestLogon="";
       
public const string RmiName_First = "";
#endif
		public override Nettention.Proud.RmiID[] GetRmiIDList() { return Common.RmiIDList; } 
	}
}

