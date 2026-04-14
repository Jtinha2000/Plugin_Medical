// Decompiled with JetBrains decompiler
// Type: UMedical.Models.Hooking.HookingSession
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using UMedical.Events;
using UMedical.Models.Hooking.Values;
using UMedical.Utils;

namespace UMedical.Models.Hooking
{
  public sealed class HookingSession
  {
    public Player Hooker { get; set; }

    public Player Victim { get; set; }

    private MovementInfo _info { get; set; }

    public bool IsUArrested { get; set; }

    public HookingSession()
    {
    }

    public HookingSession(Player hooker, Player victim, MovementInfo Info, bool isUArrested)
    {
      this._info = Info;
      this.Hooker = hooker;
      this.Victim = victim;
      this.IsUArrested = isUArrested;
    }

    public static void AddHooking(Player Hooker, Player Hooking)
    {
      if ((UnityEngine.Object) Hooker.movement.getVehicle() != (UnityEngine.Object) null && ((IEnumerable<Passenger>) Hooker.movement.getVehicle().passengers).Count<Passenger>((Func<Passenger, bool>) (X => X.player == null)) < 2 || Main.Instance.HookingSessions.Exists((Predicate<HookingSession>) (X => (UnityEngine.Object) X.Victim == (UnityEngine.Object) Hooker || (UnityEngine.Object) X.Victim == (UnityEngine.Object) Hooking || (UnityEngine.Object) X.Hooker == (UnityEngine.Object) Hooker || (UnityEngine.Object) X.Hooker == (UnityEngine.Object) Hooking)))
        return;
      LoggerUtil.SendDebugLog("AddHooking > Added!");
      Main.Instance.HookingSessions.Add(new HookingSession(Hooker, Hooking, new MovementInfo(Hooking.movement.pluginSpeedMultiplier, Hooking.movement.pluginGravityMultiplier, Hooking.movement.pluginJumpMultiplier), Hooking.animator.gesture == EPlayerGesture.ARREST_START));
      if ((UnityEngine.Object) Hooking.movement.getVehicle() != (UnityEngine.Object) null)
        Hooking.movement.forceRemoveFromVehicle();
      if ((UnityEngine.Object) Hooker.movement.getVehicle() != (UnityEngine.Object) null)
      {
        VehicleManager.ServerForcePassengerIntoVehicle(Hooking, Hooker.movement.getVehicle());
      }
      else
      {
        Hooking.stance.checkStance(EPlayerStance.CROUCH);
        Hooking.teleportToPlayer(Hooker);
      }
      Hooking.movement.sendPluginSpeedMultiplier(Hooking.movement.pluginSpeedMultiplier * 0.01f);
      Hooking.movement.sendPluginJumpMultiplier(Hooking.movement.pluginJumpMultiplier * 0.01f);
      Hooking.movement.sendPluginGravityMultiplier(Hooking.movement.pluginGravityMultiplier * 0.01f);
      Hooker.movement.sendPluginSpeedMultiplier(Hooker.movement.pluginSpeedMultiplier * Main.Instance.Configuration.Instance.HookConfig.HookerSpeedMultiplier);
      Hooker.movement.sendPluginJumpMultiplier(Hooker.movement.pluginJumpMultiplier * Main.Instance.Configuration.Instance.HookConfig.HookerJumpMultiplier);
      Hooker.movement.sendPluginGravityMultiplier(Hooker.movement.pluginGravityMultiplier * Main.Instance.Configuration.Instance.HookConfig.HookerGravityMultiplier);
      Hooking.animator.captorID = Hooker.channel.owner.playerID.steamID;
      Hooking.animator.captorItem = (ushort) 1195;
      Hooking.animator.captorStrength = (ushort) 10;
      Hooking.animator.sendGesture(EPlayerGesture.ARREST_START, true);
      Hooking.stance.onStanceUpdated = (StanceUpdated) (() => HookingEvents.OnStanceUpdated(Hooking));
      if (Main.Instance.DownedPlayers.Any<DownedPlayer>((Func<DownedPlayer, bool>) (X => (long) X.CSteamID == (long) Hooking.channel.owner.playerID.steamID.m_SteamID)))
        return;
      HookingSession.ReplicateStateAllBarricades(Hooking, Hooker);
    }

    public void RemHooking()
    {
      LoggerUtil.SendDebugLog("RemHooking > Removed!");
      this.Victim.movement.sendPluginSpeedMultiplier(this.Victim.movement.pluginSpeedMultiplier / 0.01f);
      this.Victim.movement.sendPluginJumpMultiplier(this.Victim.movement.pluginJumpMultiplier / 0.01f);
      this.Victim.movement.sendPluginGravityMultiplier(this.Victim.movement.pluginGravityMultiplier / 0.01f);
      Main.Instance.HookingSessions.Remove(this);
      EffectManager.askEffectClearByID((ushort) 3709, this.Hooker.channel.owner.transportConnection);
      if (Main.Instance.DownedPlayers.Any<DownedPlayer>((Func<DownedPlayer, bool>) (X => (long) X.CSteamID == (long) this.Victim.channel.owner.playerID.steamID.m_SteamID)))
      {
        this.Victim.stance.checkStance(Main.Instance.Configuration.Instance.DownedConfig.EffectsConfiguration.MovementConfiguration.DefaultStance);
        this.Victim.stance.onStanceUpdated = (StanceUpdated) (() => DownedEvents.StanceUpdated(this.Victim, this.Victim.stance.stance));
      }
      else
        this.Victim.stance.onStanceUpdated = (StanceUpdated) null;
      if (!this.IsUArrested)
      {
        this.Victim.animator.captorID = CSteamID.Nil;
        this.Victim.animator.captorItem = (ushort) 0;
        this.Victim.animator.captorStrength = (ushort) 0;
        this.Victim.animator.sendGesture(EPlayerGesture.ARREST_STOP, true);
      }
      this.Hooker.movement.sendPluginSpeedMultiplier(this.Hooker.movement.pluginSpeedMultiplier / Main.Instance.Configuration.Instance.HookConfig.HookerSpeedMultiplier);
      this.Hooker.movement.sendPluginJumpMultiplier(this.Hooker.movement.pluginJumpMultiplier / Main.Instance.Configuration.Instance.HookConfig.HookerJumpMultiplier);
      this.Hooker.movement.sendPluginGravityMultiplier(this.Hooker.movement.pluginGravityMultiplier / Main.Instance.Configuration.Instance.HookConfig.HookerGravityMultiplier);
      HookingSession.ReplicateStateAllBarricades(this.Victim, this.Victim);
    }

    public static void ReplicateStateAllBarricades(Player Player, Player NewOwner)
    {
      if (!Main.Instance.Configuration.Instance.HookConfig.AcessToArrestedDoors)
        return;
      BarricadeRegion[,] barricadeRegions = BarricadeManager.BarricadeRegions;
      int upperBound1 = barricadeRegions.GetUpperBound(0);
      int upperBound2 = barricadeRegions.GetUpperBound(1);
      for (int lowerBound1 = barricadeRegions.GetLowerBound(0); lowerBound1 <= upperBound1; ++lowerBound1)
      {
        for (int lowerBound2 = barricadeRegions.GetLowerBound(1); lowerBound2 <= upperBound2; ++lowerBound2)
        {
          foreach (BarricadeDrop barricadeDrop in barricadeRegions[lowerBound1, lowerBound2].drops.FindAll((Predicate<BarricadeDrop>) (X =>
          {
            if ((long) X.GetServersideData().owner != (long) Player.channel.owner.playerID.steamID.m_SteamID)
              return false;
            return new List<EBuild>()
            {
              EBuild.DOOR,
              EBuild.GATE,
              EBuild.SHUTTER,
              EBuild.HATCH
            }.Any<EBuild>((Func<EBuild, bool>) (XZ => XZ == X.asset.build));
          })))
          {
            byte[] state = barricadeDrop.asset.getState();
            Array.ConstrainedCopy((Array) BitConverter.GetBytes(NewOwner.channel.owner.playerID.steamID.m_SteamID), 0, (Array) state, 0, 8);
            BarricadeManager.updateReplicatedState(barricadeDrop.model, state, state.Length);
          }
        }
      }
    }
  }
}
