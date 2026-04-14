// Decompiled with JetBrains decompiler
// Type: UMedical.Events.AfterEffectsEvents
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

using SDG.Unturned;
using System;
using System.Linq;
using UMedical.Models;
using UMedical.Models.Samu.Revive;
using UMedical.Utils;

namespace UMedical.Events
{
  public static class AfterEffectsEvents
  {
    public static void SyncDirectDead(Player Player)
    {
      LoggerUtil.SendDebugLog("SyncDirectDead > AfterEffects Analzying!", Player.ToString());
      if (!Main.Instance.PlayersEffects.Any<AfterEffects>((Func<AfterEffects, bool>) (X => (UnityEngine.Object) X.Target == (UnityEngine.Object) Player)))
        return;
      LoggerUtil.SendDebugLog("SyncDirectDead > AfterEffects Removed!", Player.ToString());
      Main.Instance.PlayersEffects.FirstOrDefault<AfterEffects>((Func<AfterEffects, bool>) (X => (UnityEngine.Object) X.Target == (UnityEngine.Object) Player))?.Dispose();
    }

    public static void SyncMedicalAdd(DownedPlayer Player)
    {
      LoggerUtil.SendDebugLog("SyncMedicalAdd > AfterEffects Analyzing!", Player.ToString());
      if (!Main.Instance.PlayersEffects.Any<AfterEffects>((Func<AfterEffects, bool>) (X => (long) X.Target.channel.owner.playerID.steamID.m_SteamID == (long) Player.CSteamID)))
        return;
      LoggerUtil.SendDebugLog("SyncMedicalAdd > AfterEffects Removed!", Player.CSteamID.ToString());
      Main.Instance.PlayersEffects.FirstOrDefault<AfterEffects>((Func<AfterEffects, bool>) (X => (long) X.Target.channel.owner.playerID.steamID.m_SteamID == (long) Player.CSteamID))?.Dispose();
    }

    public static void OnLifeUpdated(Player Player)
    {
      AfterEffects afterEffects = Main.Instance.PlayersEffects.FirstOrDefault<AfterEffects>((Func<AfterEffects, bool>) (X => (UnityEngine.Object) X.Target == (UnityEngine.Object) Player));
      LoggerUtil.SendDebugLog("OnLifeUpdated #01 > OnLifeUpdated! Player: {0} / Life: {1}", Player.name, Player.life.health.ToString());
      if (afterEffects == null || (int) Player.life.health <= (int) afterEffects.Revive.AfterEffectsConfig.MaxLife)
        return;
      Player.life.ReceiveHealth(afterEffects.Revive.AfterEffectsConfig.MaxLife);
    }
  }
}
