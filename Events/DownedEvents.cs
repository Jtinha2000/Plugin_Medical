// Decompiled with JetBrains decompiler
// Type: UMedical.Events.DownedEvents
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

using SDG.Unturned;
using System;
using System.Linq;
using UMedical.Models;
using UMedical.Utils;
using UnityEngine;

namespace UMedical.Events
{
  public static class DownedEvents
  {
    public static void DropRequest(Player Player, Item Item, ref bool ShouldAllow) => ShouldAllow = !Main.Instance.Configuration.Instance.DownedConfig.EffectsConfiguration.ItemConfiguration.BlockDropItems || Main.Instance.Configuration.Instance.DownedConfig.EffectsConfiguration.ItemConfiguration.IgnoreItems.Contains(Item.id) & ShouldAllow;

    public static void StanceUpdated(Player Player, EPlayerStance FinalStance)
    {
      if (FinalStance == Main.Instance.Configuration.Instance.DownedConfig.EffectsConfiguration.MovementConfiguration.DefaultStance || !((UnityEngine.Object) Player.movement.getVehicle() == (UnityEngine.Object) null))
        return;
      LoggerUtil.SendDebugLog("OnStanceUpdated #01 > Updated! {0} for {1}", Player.name, Player.stance.stance.ToString());
      Player.stance.checkStance(Main.Instance.Configuration.Instance.DownedConfig.EffectsConfiguration.MovementConfiguration.DefaultStance);
    }

    public static void EquipRequest(Player Player, Item Item, ref bool ShouldAllow) => ShouldAllow = !Main.Instance.Configuration.Instance.DownedConfig.EffectsConfiguration.ItemConfiguration.BlockEquipItems || Main.Instance.Configuration.Instance.DownedConfig.EffectsConfiguration.ItemConfiguration.IgnoreItems.Contains(Item.id) & ShouldAllow;

    public static void ChatRequest(Player Player, ref Color color, ref bool ShouldAllow)
    {
      if (!Main.Instance.DownedPlayers.Any<DownedPlayer>((Func<DownedPlayer, bool>) (X => (long) X.CSteamID == (long) Player.channel.owner.playerID.steamID.m_SteamID)))
        return;
      ShouldAllow = Main.Instance.Configuration.Instance.DownedConfig.EffectsConfiguration.CommunicationConfiguration.PermitSendChat & ShouldAllow;
      color = Main.Instance.Configuration.Instance.DownedConfig.EffectsConfiguration.CommunicationConfiguration.ChangeSendedChatsColor ? Main.Instance.Configuration.Instance.DownedConfig.EffectsConfiguration.CommunicationConfiguration.Color : color;
    }

    public static void PlayerCraftingRequestHandler(
      PlayerCrafting crafting,
      ref ushort itemID,
      ref byte blueprintIndex,
      ref bool shouldAllow)
    {
      if (!Main.Instance.DownedPlayers.Any<DownedPlayer>((Func<DownedPlayer, bool>) (X => (long) X.CSteamID == (long) crafting.player.channel.owner.playerID.steamID.m_SteamID)))
        return;
      shouldAllow = !Main.Instance.Configuration.Instance.DownedConfig.EffectsConfiguration.ItemConfiguration.BlockCrafting & shouldAllow;
    }

    public static void TalkRequest(
      Player Player,
      ref bool ShouldAllow,
      ref PlayerVoice.RelayVoiceCullingHandler Delegate)
    {
      Delegate += new PlayerVoice.RelayVoiceCullingHandler(DownedEvents.CullingHandler);
      if (!Main.Instance.DownedPlayers.Any<DownedPlayer>((Func<DownedPlayer, bool>) (X => (long) X.CSteamID == (long) Player.channel.owner.playerID.steamID.m_SteamID)))
        return;
      ShouldAllow = Main.Instance.Configuration.Instance.DownedConfig.EffectsConfiguration.CommunicationConfiguration.PermitTalk & ShouldAllow;
    }

    public static bool CullingHandler(PlayerVoice speaker, PlayerVoice listener)
    {
      if (!Main.Instance.DownedPlayers.Any<DownedPlayer>((Func<DownedPlayer, bool>) (X => (long) X.CSteamID == (long) listener.channel.owner.playerID.steamID.m_SteamID)))
        return true;
      return Main.Instance.Configuration.Instance.DownedConfig.EffectsConfiguration.CommunicationConfiguration.PermitHear && Main.Instance.DownedPlayers.Any<DownedPlayer>((Func<DownedPlayer, bool>) (X => (long) X.CSteamID == (long) listener.channel.owner.playerID.steamID.m_SteamID));
    }

    public static void EnterVehicleRequest(
      Player Player,
      InteractableVehicle Vehicle,
      ref bool ShouldAllow)
    {
      if (!Main.Instance.DownedPlayers.Any<DownedPlayer>((Func<DownedPlayer, bool>) (X => (long) X.CSteamID == (long) Player.channel.owner.playerID.steamID.m_SteamID)))
        return;
      ShouldAllow = Main.Instance.Configuration.Instance.DownedConfig.EffectsConfiguration.MovementConfiguration.CanEnterOnVehicle & ShouldAllow;
    }

    public static void ExitVehicleRequest(
      Player Player,
      InteractableVehicle Vehicle,
      ref bool ShouldAllow,
      ref Vector3 DeployPos,
      ref float _)
    {
      float num = Math.Abs(MeasurementTool.speedToKPH(Vehicle.speed));
      if ((double) num < (double) Main.Instance.Configuration.Instance.DownedConfig.VehicleConfig.MinKPHToDamage || !ShouldAllow || Main.Instance.Configuration.Instance.DownedConfig.VehicleConfig.BlacklistToDamage.Any<EEngine>((Func<EEngine, bool>) (x => x == Vehicle.asset.engine)))
        return;
      if (Main.Instance.Configuration.Instance.DownedConfig.VehicleConfig.BlockExitOnMinKPH)
        ShouldAllow = false;
      else
        DamageTool.damage(Player, Main.Instance.Configuration.Instance.DownedConfig.VehicleConfig.CauseToKPHDeath, (ELimb) new System.Random().Next(0, 13), Player.channel.owner.playerID.steamID, Vector3.back, num * Main.Instance.Configuration.Instance.DownedConfig.VehicleConfig.DamageByKPH, 1f, out EPlayerKill _, false);
    }

    public static void TakeItemRequest(Player Player, ItemData ItemData, ref bool ShouldAllow)
    {
      if (!Main.Instance.DownedPlayers.Any<DownedPlayer>((Func<DownedPlayer, bool>) (X => (long) X.CSteamID == (long) Player.channel.owner.playerID.steamID.m_SteamID)))
        return;
      ShouldAllow = !Main.Instance.Configuration.Instance.DownedConfig.EffectsConfiguration.ItemConfiguration.BlockPickItems || Main.Instance.Configuration.Instance.DownedConfig.EffectsConfiguration.ItemConfiguration.IgnoreItems.Contains(ItemData.item.id) & ShouldAllow;
    }
  }
}
