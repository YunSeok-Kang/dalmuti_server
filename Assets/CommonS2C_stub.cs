﻿




// Generated by PIDL compiler.
// Do not modify this file, but modify the source .pidl file.

using System;
using System.Net;	     

namespace CommonS2C
{
	public class Stub:Nettention.Proud.RmiStub
	{
public AfterRmiInvocationDelegate AfterRmiInvocation = delegate(Nettention.Proud.AfterRmiSummary summary) {};
public BeforeRmiInvocationDelegate BeforeRmiInvocation = delegate(Nettention.Proud.BeforeRmiSummary summary) {};

		public delegate bool ReplyLogonDelegate(Nettention.Proud.HostID remote,Nettention.Proud.RmiContext rmiContext, int result, String comment);  
		public ReplyLogonDelegate ReplyLogon = delegate(Nettention.Proud.HostID remote,Nettention.Proud.RmiContext rmiContext, int result, String comment)
		{ 
			return false;
		};
	public override bool ProcessReceivedMessage(Nettention.Proud.ReceivedMessage pa, Object hostTag) 
	{
		Nettention.Proud.HostID remote=pa.RemoteHostID;
		if(remote==Nettention.Proud.HostID.HostID_None)
		{
			ShowUnknownHostIDWarning(remote);
		}

		Nettention.Proud.Message __msg=pa.ReadOnlyMessage;
		int orgReadOffset = __msg.ReadOffset;
        Nettention.Proud.RmiID __rmiID = Nettention.Proud.RmiID.RmiID_None;
        if (!__msg.Read( out __rmiID))
            goto __fail;
					
		switch(__rmiID)
		{
        case Common.ReplyLogon:
            ProcessReceivedMessage_ReplyLogon(__msg, pa, hostTag, remote);
            break;
		default:
			 goto __fail;
		}
		return true;
__fail:
	  {
			__msg.ReadOffset = orgReadOffset;
			return false;
	  }
	}
    void ProcessReceivedMessage_ReplyLogon(Nettention.Proud.Message __msg, Nettention.Proud.ReceivedMessage pa, Object hostTag, Nettention.Proud.HostID remote)
    {
        Nettention.Proud.RmiContext ctx = new Nettention.Proud.RmiContext();
        ctx.sentFrom=pa.RemoteHostID;
        ctx.relayed=pa.IsRelayed;
        ctx.hostTag=hostTag;
        ctx.encryptMode = pa.EncryptMode;
        ctx.compressMode = pa.CompressMode;

        int result; Nettention.Proud.Marshaler.Read(__msg,out result);	
String comment; Nettention.Proud.Marshaler.Read(__msg,out comment);	
core.PostCheckReadMessage(__msg, RmiName_ReplyLogon);
        if(enableNotifyCallFromStub==true)
        {
        string parameterString = "";
        parameterString+=result.ToString()+",";
parameterString+=comment.ToString()+",";
        NotifyCallFromStub(Common.ReplyLogon, RmiName_ReplyLogon,parameterString);
        }

        if(enableStubProfiling)
        {
        Nettention.Proud.BeforeRmiSummary summary = new Nettention.Proud.BeforeRmiSummary();
        summary.rmiID = Common.ReplyLogon;
        summary.rmiName = RmiName_ReplyLogon;
        summary.hostID = remote;
        summary.hostTag = hostTag;
        BeforeRmiInvocation(summary);
        }

        long t0 = Nettention.Proud.PreciseCurrentTime.GetTimeMs();

        // Call this method.
        bool __ret =ReplyLogon (remote,ctx , result, comment );

        if(__ret==false)
        {
        // Error: RMI function that a user did not create has been called. 
        core.ShowNotImplementedRmiWarning(RmiName_ReplyLogon);
        }

        if(enableStubProfiling)
        {
        Nettention.Proud.AfterRmiSummary summary = new Nettention.Proud.AfterRmiSummary();
        summary.rmiID = Common.ReplyLogon;
        summary.rmiName = RmiName_ReplyLogon;
        summary.hostID = remote;
        summary.hostTag = hostTag;
        summary.elapsedTime = Nettention.Proud.PreciseCurrentTime.GetTimeMs()-t0;
        AfterRmiInvocation(summary);
        }
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
		public override Nettention.Proud.RmiID[] GetRmiIDList { get{return Common.RmiIDList;} }
		
	}
}

