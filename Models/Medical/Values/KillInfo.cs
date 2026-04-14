// Decompiled with JetBrains decompiler
// Type: UMedical.Models.Medical.Values.KillInfo
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

using SDG.Unturned;
using Steamworks;
using System;

namespace UMedical.Models.Medical.Values
{
  public sealed class KillInfo
  {
    public ERagdollEffect RagdollEffect { get; set; }

    public SVector3 Ragdoll { get; set; }

    public DateTime Time { get; set; }

    public ulong Killer { get; set; }

    public ushort ItemID { get; set; }

    public EDeathCause Cause { get; set; }

    public ELimb Limb { get; set; }

    public KillInfo()
    {
    }

    public KillInfo(EDeathCause cause, ELimb limb)
    {
      this.Time = DateTime.Now;
      this.Cause = cause;
      this.Limb = limb;
    }

    public KillInfo(
      ERagdollEffect ragdollEffect,
      SVector3 ragdoll,
      ulong killer,
      ushort itemID,
      EDeathCause cause,
      ELimb limb)
    {
      this.Time = DateTime.Now;
      this.RagdollEffect = ragdollEffect;
      this.Ragdoll = ragdoll;
      this.Killer = killer;
      this.ItemID = itemID;
      this.Cause = cause;
      this.Limb = limb;
    }

    public override string ToString() => string.Format("Limb: {0} \nCause:{1} \nWeaponary: {2} \nKillerID: {3} \nKillerIsOnline: {4} \nKillTime: {5} \nRagdoll: {6} \nKillEffect: {7} ", (object) this.Limb, (object) this.Cause, (object) this.ItemID, (object) this.Killer, (object) ((UnityEngine.Object) PlayerTool.getPlayer(new CSteamID(this.Killer)) != (UnityEngine.Object) null), (object) this.Time, (object) this.Ragdoll.ToVector3(), (object) this.RagdollEffect);
  }
}
