// Decompiled with JetBrains decompiler
// Type: UMedical.Models.DownedPlayer
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

using Newtonsoft.Json;
using Rocket.API;
using Rocket.Core;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using UMedical.Events;
using UMedical.Models.Hooking;
using UMedical.Models.Medical.Values;
using UMedical.Utils;
using UnityEngine;

namespace UMedical.Models
{
    public sealed class DownedPlayer
    {
        public ulong CSteamID { get; set; }

        public KillInfo Info { get; set; }

        public float RestLifeTime { get; set; }

        public List<Revive> Revives { get; set; }

        public Coroutine DeathCoroutine { get; set; }

        [JsonIgnore]
        public KeyListener Listener { get; set; }

        public DownedPlayer()
        {
        }

        private DownedPlayer(SDG.Unturned.Player _Player)
        {
            LoggerUtil.SendDebugLog("Builder > InitialFaseStarted! Identifier: {0}", "Player _Player");
            this.Revives = new List<Revive>();
            _Player.life.ReceiveHealth((byte)100);
            DownedPlayer.AddPlayer(_Player);
            if (Main.Instance.Configuration.Instance.DownedConfig.CanSeedpUpDeath)
            {
                Listener = new KeyListener(_Player);
                Listener.SpeedUpRequested += Listener_SpeedUpRequested;
            }
            LoggerUtil.SendDebugLog("Builder > InitialFaseFinished! Identifier: {0}", "Player _Player");
        }
        public DownedPlayer(ulong cSteamID, KillInfo info, int restLifeTime)
          : this(PlayerTool.getPlayer(new Steamworks.CSteamID(cSteamID)))
        {
            LoggerUtil.SendDebugLog("Builder > DefaultBuilderStarted! Identifier: {0}", "ulong cSteamID, KillInfo info, int restLifeTime");
            this.Revives = new List<Revive>();
            this.CSteamID = cSteamID;
            this.Info = info;
            this.RestLifeTime = restLifeTime;
            this.DeathCoroutine = Main.Instance.StartCoroutine(Main.DeathCoroutine(this, this.RestLifeTime));
            LoggerUtil.SendDebugLog("Builder > DefaultBuilderFinished! Identifier: {0}", "ulong cSteamID, KillInfo info, int restLifeTime");
            Main.Instance.InvokeOnAdded(this);
        }
        public void Listener_SpeedUpRequested()
        {
            RestLifeTime -= Main.Instance.Configuration.Instance.DownedConfig.SpeedUpReduction;
        }

        public static void AddPlayer(SDG.Unturned.Player Player)
        {
            LoggerUtil.SendDebugLog("AddPlayer > Started {0}", Player.name);
            Player.life.ReceiveLifeStats((byte)100, Player.life.food, Player.life.water, Player.life.virus, Player.life.oxygen, false, Player.life.isBroken);
            LoggerUtil.SendDebugLog("AddPlayer > BeforeSpeedApplies: {0}", Player.movement.pluginSpeedMultiplier.ToString());
            Player.movement.sendPluginSpeedMultiplier(Player.movement.pluginSpeedMultiplier * Main.Instance.Configuration.Instance.DownedConfig.EffectsConfiguration.MovementConfiguration.SpeedMultiplier);
            Player.movement.sendPluginJumpMultiplier(Player.movement.pluginJumpMultiplier * Main.Instance.Configuration.Instance.DownedConfig.EffectsConfiguration.MovementConfiguration.JumpMultiplier);
            Player.movement.sendPluginGravityMultiplier(Player.movement.pluginGravityMultiplier * Main.Instance.Configuration.Instance.DownedConfig.EffectsConfiguration.MovementConfiguration.GravityMultiplier);
            LoggerUtil.SendDebugLog("AddPlayer > AfterSpeedApplies: {0}", Player.movement.pluginSpeedMultiplier.ToString());
            if (Main.Instance.Configuration.Instance.MiscConfig.DownedBlurWindow)
            {
                Player.enablePluginWidgetFlag(EPluginWidgetFlags.Modal);
                Player.enablePluginWidgetFlag(EPluginWidgetFlags.ForceBlur);
            }
            Player.disablePluginWidgetFlag(EPluginWidgetFlags.ShowOxygen);
            Player.disablePluginWidgetFlag(EPluginWidgetFlags.ShowVirus);
            Player.disablePluginWidgetFlag(EPluginWidgetFlags.ShowFood);
            Player.disablePluginWidgetFlag(EPluginWidgetFlags.ShowWater);
            Player.disablePluginWidgetFlag(EPluginWidgetFlags.ShowStamina);
            Player.disablePluginWidgetFlag(EPluginWidgetFlags.ShowStatusIcons);
            Player.disablePluginWidgetFlag(EPluginWidgetFlags.ShowVehicleStatus);
            Player.disablePluginWidgetFlag(EPluginWidgetFlags.ShowUseableGunStatus);

            if(Main.Instance.Configuration.Instance.DownedConfig.DropItemsOnDowned)
                for (byte page = 0; page < 7; page++)
                {
                    var itemsPage = Player.inventory.items[page];
                    if (itemsPage == null)
                        continue;

                    while (itemsPage.getItemCount() > 0)
                    {
                        ItemJar Item = itemsPage.getItem(0);
                        ItemManager.dropItem(Item.item, Player.transform.position, false, true, false);
                        itemsPage.removeItem(0);
                    }
                }

            if (Main.Instance.Configuration.Instance.MiscConfig.ParadoxUIMode)
                R.Commands.Execute((IRocketPlayer)UnturnedPlayer.FromPlayer(Player), "changehud 0");
            Player.stance.onStanceUpdated = (StanceUpdated)(() => DownedEvents.StanceUpdated(Player, Player.stance.stance));
            Player.equipment.onEquipRequested = (PlayerEquipRequestHandler)((PlayerEquipment equipment, ItemJar jar, ItemAsset asset, ref bool shouldAllow) => DownedEvents.EquipRequest(Player, jar.item, ref shouldAllow));
            Player.inventory.onDropItemRequested = (DropItemRequestHandler)((PlayerInventory inventory, Item item, ref bool shouldAllow) => DownedEvents.DropRequest(Player, item, ref shouldAllow));
            if (!Main.Instance.HookingSessions.Any<HookingSession>((Func<HookingSession, bool>)(X => (UnityEngine.Object)X.Victim == (UnityEngine.Object)Player)))
            {
                if ((UnityEngine.Object)Player.movement.getVehicle() != (UnityEngine.Object)null)
                    Player.movement.forceRemoveFromVehicle();
                Player.stance.checkStance(Main.Instance.Configuration.Instance.DownedConfig.EffectsConfiguration.MovementConfiguration.DefaultStance);
            }
            Player.equipment.dequip();
            EffectManager.sendUIEffect((ushort)3701, (short)3701, Player.channel.owner.transportConnection, true);
            if (!Main.Instance.Configuration.Instance.MiscConfig.DownedBlackWindow)
                EffectManager.sendUIEffectVisibility((short)3701, Player.channel.owner.transportConnection, true, "BackGroundTOp", false);
            EffectManager.sendUIEffectVisibility((short)3701, Player.channel.owner.transportConnection, true, "Image1 " + ((LightingManager.isDaytime) ? "(Day)" : "(Night)"), true);
            string text = Main.Instance.Configuration.Instance.UiConfig.PossibleHints.RandomOrDefault<string>();
            EffectManager.sendUIEffectText((short)3701, Player.channel.owner.transportConnection, true, "HINTTEXT", text);
            EffectManager.sendUIEffectText((short)3701, Player.channel.owner.transportConnection, true, "HINTNUMBER", (Main.Instance.Configuration.Instance.UiConfig.PossibleHints.IndexOf(text) + 1).ToString() + "/" + (Main.Instance.Configuration.Instance.UiConfig.PossibleHints.Count + 1).ToString());
            LoggerUtil.SendDebugLog("AddPlayer > Finished {0}", Player.name);
        }

        public static void RemPlayer(DownedPlayer Downed, bool ShouldForceUnhook)
        {
            SDG.Unturned.Player Player = PlayerTool.getPlayer(new Steamworks.CSteamID(Downed.CSteamID));
            LoggerUtil.SendDebugLog("RemPlayer > Started {0}", Player.name);
            Downed.Revives.ForEach((Action<Revive>)(X => X.StopRevive()));
            LoggerUtil.SendDebugLog("RemPlayer > BeforeSpeedApplies: {0}", Player.movement.pluginSpeedMultiplier.ToString());
            Player.movement.sendPluginSpeedMultiplier(Player.movement.pluginSpeedMultiplier / Main.Instance.Configuration.Instance.DownedConfig.EffectsConfiguration.MovementConfiguration.SpeedMultiplier);
            Player.movement.sendPluginJumpMultiplier(Player.movement.pluginJumpMultiplier / Main.Instance.Configuration.Instance.DownedConfig.EffectsConfiguration.MovementConfiguration.JumpMultiplier);
            Player.movement.sendPluginGravityMultiplier(Player.movement.pluginGravityMultiplier / Main.Instance.Configuration.Instance.DownedConfig.EffectsConfiguration.MovementConfiguration.GravityMultiplier);
            LoggerUtil.SendDebugLog("RemPlayer > AfterSpeedApplies: {0}", Player.movement.pluginSpeedMultiplier.ToString());
            if (Downed.Listener != null)
                Downed.Listener.Kill();
            if (Main.Instance.Configuration.Instance.MiscConfig.DownedBlurWindow)
            {
                Player.disablePluginWidgetFlag(EPluginWidgetFlags.ForceBlur);
                Player.disablePluginWidgetFlag(EPluginWidgetFlags.Modal);
            }
            if (!Main.Instance.Configuration.Instance.MiscConfig.ParadoxUIMode)
            {
                Player.enablePluginWidgetFlag(EPluginWidgetFlags.ShowOxygen);
                Player.enablePluginWidgetFlag(EPluginWidgetFlags.ShowVirus);
                Player.enablePluginWidgetFlag(EPluginWidgetFlags.ShowFood);
                Player.enablePluginWidgetFlag(EPluginWidgetFlags.ShowWater);
                Player.enablePluginWidgetFlag(EPluginWidgetFlags.ShowStamina);
                Player.enablePluginWidgetFlag(EPluginWidgetFlags.ShowStatusIcons);
                Player.enablePluginWidgetFlag(EPluginWidgetFlags.ShowVehicleStatus);
                Player.enablePluginWidgetFlag(EPluginWidgetFlags.ShowUseableGunStatus);
            }
            else
                R.Commands.Execute((IRocketPlayer)UnturnedPlayer.FromPlayer(Player), "changehud 1");
            if (Main.Instance.HookingSessions.Any<HookingSession>((Func<HookingSession, bool>)(X => (UnityEngine.Object)X.Victim == (UnityEngine.Object)Player)))
            {
                EffectManager.askEffectClearByID((ushort)3709, Main.Instance.HookingSessions.First<HookingSession>((Func<HookingSession, bool>)(X => (UnityEngine.Object)X.Victim == (UnityEngine.Object)Player)).Hooker.channel.owner.transportConnection);
                Player.stance.checkStance(EPlayerStance.CROUCH);
                Player.stance.onStanceUpdated = (StanceUpdated)(() => HookingEvents.OnStanceUpdated(Player));
            }
            else
                Player.stance.onStanceUpdated = (StanceUpdated)null;
            Player.equipment.onEquipRequested = (PlayerEquipRequestHandler)null;
            Player.inventory.onDropItemRequested = (DropItemRequestHandler)null;
            Player.stance.checkStance(EPlayerStance.STAND);
            EffectManager.askEffectClearByID((ushort)3701, Player.channel.owner.transportConnection);
            LoggerUtil.SendDebugLog("RemPlayer > Finished {0}", Player.name);
            Main.Instance.InvokeOnRemoved(Downed, ShouldForceUnhook);
        }

        public void Dispose(bool ShouldForceUnhook)
        {
            LoggerUtil.SendDebugLog("Dispose > Started! {0}", this.CSteamID.ToString());
            LoggerUtil.SendDebugLog("Dispose > Removing! {0}", this.CSteamID.ToString());
            DownedPlayer.RemPlayer(this, ShouldForceUnhook);
            LoggerUtil.SendDebugLog("Dispose > Removed! {0}", this.CSteamID.ToString());
            if (Listener != null)
                Listener.Kill();
            if (this.DeathCoroutine != null)
            {
                Main.Instance.StopCoroutine(this.DeathCoroutine);
                LoggerUtil.SendDebugLog("Dispose > Coroutine Stopped! {0}", this.CSteamID.ToString());
            }

            LoggerUtil.SendDebugLog("Dispose > Finished! {0}", this.CSteamID.ToString());
        }
    }
}
