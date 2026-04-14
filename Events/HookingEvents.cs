// Decompiled with JetBrains decompiler
// Type: UMedical.Events.HookingEvents
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using UMedical.Models;
using UMedical.Models.Hooking;
using UMedical.Utils;
using UnityEngine;

namespace UMedical.Events
{
  public sealed class HookingEvents : UnturnedPlayerComponent
  {
    public Vector3 LastPos { get; set; }

    public static void SyncMedicalRem(DownedPlayer Player, bool ShouldForceUnhook)
    {
      LoggerUtil.SendDebugLog("SyncMedicalRem > Analzying!", Player.CSteamID.ToString());
      if (!Main.Instance.HookingSessions.Any<HookingSession>((Func<HookingSession, bool>) (X =>
      {
        if ((long) X.Victim.channel.owner.playerID.steamID.m_SteamID != (long) Player.CSteamID)
          return false;
        return ShouldForceUnhook || X.Victim.animator.gesture != EPlayerGesture.ARREST_START;
      })))
        return;
      LoggerUtil.SendDebugLog("SyncMedicalRem > Removed!", Player.CSteamID.ToString());
      Main.Instance.HookingSessions.FirstOrDefault<HookingSession>((Func<HookingSession, bool>) (X => (long) X.Victim.channel.owner.playerID.steamID.m_SteamID == (long) Player.CSteamID))?.RemHooking();
    }

    public static void SyncDirectDead(SDG.Unturned.Player Player)
    {
      LoggerUtil.SendDebugLog("SyncDirectDead > Analzying!", Player.ToString());
      if (!Main.Instance.HookingSessions.Any<HookingSession>((Func<HookingSession, bool>) (X => (UnityEngine.Object) X.Victim == (UnityEngine.Object) Player || (UnityEngine.Object) X.Hooker == (UnityEngine.Object) Player)))
        return;
      LoggerUtil.SendDebugLog("SyncDirectDead > Removed!", Player.ToString());
      Main.Instance.HookingSessions.FirstOrDefault<HookingSession>((Func<HookingSession, bool>) (X => (UnityEngine.Object) X.Victim == (UnityEngine.Object) Player || (UnityEngine.Object) X.Hooker == (UnityEngine.Object) Player))?.RemHooking();
    }

    public static void SyncMedicalAdd(DownedPlayer Player)
    {
      LoggerUtil.SendDebugLog("SyncMedicalAdd > Analzying!", Player.ToString());
      if (!Main.Instance.HookingSessions.Any<HookingSession>((Func<HookingSession, bool>) (X => (long) X.Hooker.channel.owner.playerID.steamID.m_SteamID == (long) Player.CSteamID)))
        return;
      LoggerUtil.SendDebugLog("SyncMedicalAdd > Removed!", Player.CSteamID.ToString());
      Main.Instance.HookingSessions.FirstOrDefault<HookingSession>((Func<HookingSession, bool>) (X => (long) X.Hooker.channel.owner.playerID.steamID.m_SteamID == (long) Player.CSteamID))?.RemHooking();
    }

    public static void OnDisconnected(UnturnedPlayer Player) => Main.Instance.HookingSessions.FindAll((Predicate<HookingSession>) (X => (long) X.Hooker.channel.owner.playerID.steamID.m_SteamID == (long) Player.CSteamID.m_SteamID || (long) X.Victim.channel.owner.playerID.steamID.m_SteamID == (long) Player.CSteamID.m_SteamID)).ForEach((Action<HookingSession>) (X =>
    {
      X.RemHooking();
      LoggerUtil.SendDebugLog("OnDisconnected > Removed hooking beetwen {0} and {1}!", X.Hooker.name, X.Victim.name);
    }));

    public static void OnStanceUpdated(SDG.Unturned.Player Player)
    {
      if (Player.stance.stance == EPlayerStance.CROUCH || !((UnityEngine.Object) Player.movement.getVehicle() == (UnityEngine.Object) null))
        return;
      LoggerUtil.SendDebugLog("OnStanceUpdated > Updated! {0} for {1}", Player.name, Player.stance.stance.ToString());
      Player.stance.checkStance(EPlayerStance.CROUCH);
    }

    public static void ExitVehicleRequest(
      SDG.Unturned.Player Player,
      InteractableVehicle Vehicle,
      ref bool ShouldAllow,
      ref Vector3 DeployPos,
      ref float _)
    {
      ShouldAllow = !Main.Instance.HookingSessions.Any<HookingSession>((Func<HookingSession, bool>) (X => (UnityEngine.Object) X.Victim == (UnityEngine.Object) Player)) & ShouldAllow;
      LoggerUtil.SendDebugLog("ExitVehicleRequest > ShouldAllow Modified {0}!", ShouldAllow.ToString());
      if (!Main.Instance.HookingSessions.Any<HookingSession>((Func<HookingSession, bool>) (X => (UnityEngine.Object) X.Hooker == (UnityEngine.Object) Player)))
        return;
      LoggerUtil.SendDebugLog("ExitVehicleRequest > Passed!");
      SDG.Unturned.Player victim = Main.Instance.HookingSessions.FirstOrDefault<HookingSession>((Func<HookingSession, bool>) (X => (UnityEngine.Object) X.Hooker == (UnityEngine.Object) Player)).Victim;
      Vehicle.findPlayerSeat(victim, out byte _);
      Main.Instance.HookingSessions.FirstOrDefault<HookingSession>((Func<HookingSession, bool>) (X => (UnityEngine.Object) X.Hooker == (UnityEngine.Object) Player)).RemHooking();
      VehicleManager.removePlayerTeleportUnsafe(Vehicle, victim, DeployPos, _);
    }

    public static void EnterVehicleRequest(
      SDG.Unturned.Player Player,
      InteractableVehicle Vehicle,
      ref bool ShouldAllow)
    {
      LoggerUtil.SendDebugLog("EnterVehicleRequest > ShouldAllow Modified {0}!", ShouldAllow.ToString());
      ShouldAllow = !Main.Instance.HookingSessions.Any<HookingSession>((Func<HookingSession, bool>) (X => (UnityEngine.Object) X.Victim == (UnityEngine.Object) Player)) & ShouldAllow;
      LoggerUtil.SendDebugLog("EnterVehicleRequest > ShouldAllow Modified {0}!", ShouldAllow.ToString());
      if (!Main.Instance.HookingSessions.Any<HookingSession>((Func<HookingSession, bool>) (X => (UnityEngine.Object) X.Hooker == (UnityEngine.Object) Player)))
        return;
      if (((IEnumerable<Passenger>) Vehicle.passengers).Count<Passenger>((Func<Passenger, bool>) (X => X.player == null)) < 2 || !Vehicle.tryAddPlayer(out byte _, Player))
      {
        LoggerUtil.SendDebugLog("EnterVehicleRequest > Passed 1!");
        Main.Instance.HookingSessions.FirstOrDefault<HookingSession>((Func<HookingSession, bool>) (X => (UnityEngine.Object) X.Hooker == (UnityEngine.Object) Player)).RemHooking();
      }
      else
      {
        VehicleManager.ServerForcePassengerIntoVehicle(Main.Instance.HookingSessions.FirstOrDefault<HookingSession>((Func<HookingSession, bool>) (X => (UnityEngine.Object) X.Hooker == (UnityEngine.Object) Player)).Victim, Vehicle);
        LoggerUtil.SendDebugLog("EnterVehicleRequest > Passed 2!");
      }
    }

    public static void UnturnedPlayerEvents_OnPlayerUpdateGesture(
      UnturnedPlayer player,
      UnturnedPlayerEvents.PlayerGesture gesture)
    {
      LoggerUtil.SendDebugLog("OnPlayerUpdateGesture > Gesture Updated {0}! / {1} / {2} / {3} / ", gesture.ToString(), Main.Instance.HookingSessions.Any<HookingSession>((Func<HookingSession, bool>) (x => (UnityEngine.Object) x.Victim == (UnityEngine.Object) player.Player)).ToString(), (gesture == UnturnedPlayerEvents.PlayerGesture.Arrest_Stop).ToString(), Main.Instance.DownedPlayers.Any<DownedPlayer>((Func<DownedPlayer, bool>) (X => (long) X.CSteamID == (long) player.CSteamID.m_SteamID)).ToString());
      SDG.Unturned.Player Player = HookinViewerUtil.GetPlayer(player.Player);
      if (gesture != UnturnedPlayerEvents.PlayerGesture.InventoryOpen && gesture != UnturnedPlayerEvents.PlayerGesture.InventoryClose && gesture != UnturnedPlayerEvents.PlayerGesture.SurrenderStart && Main.Instance.HookingSessions.Any<HookingSession>((Func<HookingSession, bool>) (x => (UnityEngine.Object) x.Hooker == (UnityEngine.Object) player.Player)) && (UnityEngine.Object) player.Player.movement.getVehicle() == (UnityEngine.Object) null || Main.Instance.HookingSessions.Any<HookingSession>((Func<HookingSession, bool>) (x => (UnityEngine.Object) x.Victim == (UnityEngine.Object) player.Player)) && gesture == UnturnedPlayerEvents.PlayerGesture.Arrest_Stop && !Main.Instance.DownedPlayers.Any<DownedPlayer>((Func<DownedPlayer, bool>) (X => (long) X.CSteamID == (long) player.CSteamID.m_SteamID)))
      {
        Main.Instance.HookingSessions.FirstOrDefault<HookingSession>((Func<HookingSession, bool>) (x => (UnityEngine.Object) x.Hooker == (UnityEngine.Object) player.Player || (UnityEngine.Object) x.Victim == (UnityEngine.Object) player.Player)).RemHooking();
      }
      else
      {
        if (gesture != UnturnedPlayerEvents.PlayerGesture.SurrenderStart || !((UnityEngine.Object) Player != (UnityEngine.Object) null) || Player.animator.gesture != EPlayerGesture.ARREST_START && !Main.Instance.DownedPlayers.Any<DownedPlayer>((Func<DownedPlayer, bool>) (X => (long) X.CSteamID == (long) Player.channel.owner.playerID.steamID.m_SteamID)))
          return;
        HookingSession.AddHooking(player.Player, Player);
      }
    }

    public void Update()
    {
      if (!(this.LastPos != this.Player.Player.transform.position) || !Main.Instance.HookingSessions.Any<HookingSession>((Func<HookingSession, bool>) (X =>
      {
        if ((long) X.Hooker.channel.owner.playerID.steamID.m_SteamID == (long) this.Player.Player.channel.owner.playerID.steamID.m_SteamID)
          return true;
        return (long) X.Victim.channel.owner.playerID.steamID.m_SteamID == (long) this.Player.Player.channel.owner.playerID.steamID.m_SteamID && (UnityEngine.Object) X.Victim.movement.getVehicle() == (UnityEngine.Object) null && (UnityEngine.Object) X.Hooker.movement.getVehicle() == (UnityEngine.Object) null;
      })))
        return;
      this.LastPos = this.Player.Player.transform.position;
      Main.Instance.HookingSessions.FirstOrDefault<HookingSession>((Func<HookingSession, bool>) (X => (long) X.Hooker.channel.owner.playerID.steamID.m_SteamID == (long) this.Player.Player.channel.owner.playerID.steamID.m_SteamID || (long) X.Victim.channel.owner.playerID.steamID.m_SteamID == (long) this.Player.Player.channel.owner.playerID.steamID.m_SteamID)).Victim.teleportToPlayer(Main.Instance.HookingSessions.FirstOrDefault<HookingSession>((Func<HookingSession, bool>) (X => (long) X.Hooker.channel.owner.playerID.steamID.m_SteamID == (long) this.Player.Player.channel.owner.playerID.steamID.m_SteamID || (long) X.Victim.channel.owner.playerID.steamID.m_SteamID == (long) this.Player.Player.channel.owner.playerID.steamID.m_SteamID)).Hooker);
    }
  }
}
