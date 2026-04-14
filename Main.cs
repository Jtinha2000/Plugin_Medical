// Decompiled with JetBrains decompiler
// Type: UMedical.Main
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

using HarmonyLib;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UMedical.Events;
using UMedical.Models;
using UMedical.Models.Configuration;
using UMedical.Models.Hooking;
using UMedical.Models.Samu.Revive;
using UMedical.Utils;
using UnityEngine;

namespace UMedical
{
    public sealed class Main : RocketPlugin<UMedical.Configuration>
    {
        public static event Main.OnPlayerRemoved OnRemoved;

        public void InvokeOnRemoved(DownedPlayer Player, bool ShouldForceUnhook)
        {
            Main.OnPlayerRemoved onRemoved = Main.OnRemoved;
            if (onRemoved == null)
                return;
            onRemoved(Player, ShouldForceUnhook);
        }

        public static event Main.OnPlayerAdded OnAdded;

        public void InvokeOnAdded(DownedPlayer Player)
        {
            Main.OnPlayerAdded onAdded = Main.OnAdded;
            if (onAdded == null)
                return;
            onAdded(Player);
        }

        public static event Main.OnPlayerDirectDead OnDead;

        public void InvokeOnDirectDead(SDG.Unturned.Player Player)
        {
            Main.OnPlayerDirectDead onDead = Main.OnDead;
            if (onDead == null)
                return;
            onDead(Player);
        }

        public Harmony Harmony { get; set; }

        public static Main Instance { get; set; }

        public List<AfterEffects> PlayersEffects { get; set; }

        public List<DownedPlayer> DownedPlayers { get; set; }

        public List<HookingSession> HookingSessions { get; set; }

        protected override void Load()
        {
            Main.OnRemoved += new Main.OnPlayerRemoved(HookingEvents.SyncMedicalRem);
            Main.OnAdded += new Main.OnPlayerAdded(HookingEvents.SyncMedicalAdd);
            Main.OnDead += new Main.OnPlayerDirectDead(HookingEvents.SyncDirectDead);
            Main.OnDead += new Main.OnPlayerDirectDead(AfterEffectsEvents.SyncDirectDead);
            Main.OnAdded += new Main.OnPlayerAdded(AfterEffectsEvents.SyncMedicalAdd);
            this.PlayersEffects = new List<AfterEffects>();
            this.HookingSessions = new List<HookingSession>();
            this.DownedPlayers = new List<DownedPlayer>();
            Main.Instance = this;
            this.Harmony = new Harmony("com.MainCommunist.patch");
            EffectManager.onEffectButtonClicked += new EffectManager.EffectButtonClickedHandler(UIUtil.OnButtonClicked);
            EffectManager.onEffectTextCommitted += new EffectManager.EffectTextCommittedHandler(UIUtil.OnTextComitted);
            UseableConsumeable.onPerformingAid += new UseableConsumeable.PerformingAidHandler(this.UseableConsumeable_onPerformingAid);
            UnturnedPlayerEvents.OnPlayerDeath += new UnturnedPlayerEvents.PlayerDeath(this.UnturnedPlayerEvents_OnPlayerDeath);
            ChatManager.onChatted += (Chatted)((SteamPlayer player, EChatMode mode, ref Color chatted, ref bool isRich, string text, ref bool isVisible) => DownedEvents.ChatRequest(player.player, ref chatted, ref isVisible));
            VehicleManager.onEnterVehicleRequested += (VehicleManager.EnterVehicleRequestHandler)((SDG.Unturned.Player player, InteractableVehicle vehicle, ref bool shouldAllow) => DownedEvents.EnterVehicleRequest(player, vehicle, ref shouldAllow));
            PlayerVoice.onRelayVoice += (PlayerVoice.RelayVoiceHandler)((PlayerVoice speaker, bool wantsToUseWalkieTalkie, ref bool shouldAllow, ref bool shouldBroadcastOverRadio, ref PlayerVoice.RelayVoiceCullingHandler cullingHandler) => DownedEvents.TalkRequest(speaker.player, ref shouldAllow, ref cullingHandler));
            ItemManager.onTakeItemRequested += (TakeItemRequestHandler)((SDG.Unturned.Player player, byte x, byte y, uint instanceID, byte to_x, byte to_y, byte to_rot, byte to_page, ItemData itemData, ref bool shouldAllow) => DownedEvents.TakeItemRequest(player, itemData, ref shouldAllow));
            PlayerCrafting.onCraftBlueprintRequested = new PlayerCraftingRequestHandler(DownedEvents.PlayerCraftingRequestHandler);
            UnturnedPlayerEvents.OnPlayerUpdateGesture += new UnturnedPlayerEvents.PlayerUpdateGesture(HookingEvents.UnturnedPlayerEvents_OnPlayerUpdateGesture);
            Rocket.Unturned.U.Events.OnPlayerDisconnected += new UnturnedEvents.PlayerDisconnected(HookingEvents.OnDisconnected);
            Rocket.Unturned.U.Events.OnPlayerConnected += new UnturnedEvents.PlayerConnected(this.Events_OnPlayerConnected);
            VehicleManager.onEnterVehicleRequested += (VehicleManager.EnterVehicleRequestHandler)((SDG.Unturned.Player player, InteractableVehicle vehicle, ref bool shouldAllow) => HookingEvents.EnterVehicleRequest(player, vehicle, ref shouldAllow));
            VehicleManager.onExitVehicleRequested += (VehicleManager.ExitVehicleRequestHandler)((SDG.Unturned.Player player, InteractableVehicle vehicle, ref bool shouldAllow, ref Vector3 pendingLocation, ref float pendingYaw) => HookingEvents.ExitVehicleRequest(player, vehicle, ref shouldAllow, ref pendingLocation, ref pendingYaw));
            VehicleManager.onExitVehicleRequested += (VehicleManager.ExitVehicleRequestHandler)((SDG.Unturned.Player player, InteractableVehicle vehicle, ref bool shouldAllow, ref Vector3 pendingLocation, ref float pendingYaw) => DownedEvents.ExitVehicleRequest(player, vehicle, ref shouldAllow, ref pendingLocation, ref pendingYaw));
            this.StartCoroutine(UIUtil.GlobalHintCooldown());
            this.Harmony.PatchAll();

            Configuration.Instance.MiscConfig.DamageMutliplier.GetMultiplierCreate();
            Configuration.Instance.DownedConfig.DamageMutliplierAfterDead.GetMultiplierCreate();
        }

        protected override void Unload()
        {
            Main.OnRemoved -= new Main.OnPlayerRemoved(HookingEvents.SyncMedicalRem);
            Main.OnAdded -= new Main.OnPlayerAdded(HookingEvents.SyncMedicalAdd);
            Main.OnDead -= new Main.OnPlayerDirectDead(HookingEvents.SyncDirectDead);
            Main.OnDead -= new Main.OnPlayerDirectDead(AfterEffectsEvents.SyncDirectDead);
            Main.OnAdded -= new Main.OnPlayerAdded(AfterEffectsEvents.SyncMedicalAdd);
            this.PlayersEffects.ForEach((Action<AfterEffects>)(X => X.Dispose()));
            this.HookingSessions.ForEach((Action<HookingSession>)(X => X.RemHooking()));
            EffectManager.onEffectButtonClicked -= new EffectManager.EffectButtonClickedHandler(UIUtil.OnButtonClicked);
            EffectManager.onEffectTextCommitted -= new EffectManager.EffectTextCommittedHandler(UIUtil.OnTextComitted);
            UseableConsumeable.onPerformingAid -= new UseableConsumeable.PerformingAidHandler(this.UseableConsumeable_onPerformingAid);
            UnturnedPlayerEvents.OnPlayerDeath -= new UnturnedPlayerEvents.PlayerDeath(this.UnturnedPlayerEvents_OnPlayerDeath);
            ChatManager.onChatted -= (Chatted)((SteamPlayer player, EChatMode mode, ref Color chatted, ref bool isRich, string text, ref bool isVisible) => DownedEvents.ChatRequest(player.player, ref chatted, ref isVisible));
            VehicleManager.onEnterVehicleRequested -= (VehicleManager.EnterVehicleRequestHandler)((SDG.Unturned.Player player, InteractableVehicle vehicle, ref bool shouldAllow) => DownedEvents.EnterVehicleRequest(player, vehicle, ref shouldAllow));
            PlayerVoice.onRelayVoice -= (PlayerVoice.RelayVoiceHandler)((PlayerVoice speaker, bool wantsToUseWalkieTalkie, ref bool shouldAllow, ref bool shouldBroadcastOverRadio, ref PlayerVoice.RelayVoiceCullingHandler cullingHandler) => DownedEvents.TalkRequest(speaker.player, ref shouldAllow, ref cullingHandler));
            ItemManager.onTakeItemRequested -= (TakeItemRequestHandler)((SDG.Unturned.Player player, byte x, byte y, uint instanceID, byte to_x, byte to_y, byte to_rot, byte to_page, ItemData itemData, ref bool shouldAllow) => DownedEvents.TakeItemRequest(player, itemData, ref shouldAllow));
            PlayerCrafting.onCraftBlueprintRequested = (PlayerCraftingRequestHandler)null;
            UnturnedPlayerEvents.OnPlayerUpdateGesture -= new UnturnedPlayerEvents.PlayerUpdateGesture(HookingEvents.UnturnedPlayerEvents_OnPlayerUpdateGesture);
            Rocket.Unturned.U.Events.OnPlayerDisconnected -= new UnturnedEvents.PlayerDisconnected(HookingEvents.OnDisconnected);
            Rocket.Unturned.U.Events.OnPlayerConnected -= new UnturnedEvents.PlayerConnected(this.Events_OnPlayerConnected);
            VehicleManager.onEnterVehicleRequested -= (VehicleManager.EnterVehicleRequestHandler)((SDG.Unturned.Player player, InteractableVehicle vehicle, ref bool shouldAllow) => HookingEvents.EnterVehicleRequest(player, vehicle, ref shouldAllow));
            VehicleManager.onExitVehicleRequested -= (VehicleManager.ExitVehicleRequestHandler)((SDG.Unturned.Player player, InteractableVehicle vehicle, ref bool shouldAllow, ref Vector3 pendingLocation, ref float pendingYaw) => HookingEvents.ExitVehicleRequest(player, vehicle, ref shouldAllow, ref pendingLocation, ref pendingYaw));
            VehicleManager.onExitVehicleRequested -= (VehicleManager.ExitVehicleRequestHandler)((SDG.Unturned.Player player, InteractableVehicle vehicle, ref bool shouldAllow, ref Vector3 pendingLocation, ref float pendingYaw) => DownedEvents.ExitVehicleRequest(player, vehicle, ref shouldAllow, ref pendingLocation, ref pendingYaw));
            this.Harmony.UnpatchAll("com.MainCommunist.patch");
        }

        public override TranslationList DefaultTranslations => new TranslationList()
    {
      {
        "AnalyzeCommandNoPermission",
        "<color=red>[AnalyzeCommand]</color> Para analisador o jogador alvo você precisa ser <color=red>{0}</color>!"
      },
      {
        "AnalyzeCommandNullTarget",
        "<color=red>[AnalyzeCommand]</color> O Alvo não foi encontrado!"
      },
      {
        "BeingRevived",
        "<color=red>[ReviveSystem]</color> Estão tentando te reviver!"
      },
      {
        "AnalyzeCommandSucess",
        "<color=red>[AnalyzeCommand]</color> {0} Foi está sendo analizado!"
      },
      {
        "EndCommandNullTarget",
        "<color=red>[EndCommand]</color> O Alvo não foi encontrado!"
      },
      {
        "EndCommandSucess",
        "<color=red>[EndCommand]</color> {0} Foi finalizado com sucesso!"
      },
      {
        "ReviveCommandNullTarget",
        "<color=red>[ReviveCommand]</color> O Alvo não foi encontrado!"
      },
      {
        "ReviveCommandSucess",
        "<color=red>[ReviveCommand]</color> {0} Foi revivido com sucesso!"
      },
      {
        "ReviveRequestSended",
        "<color=red>[SAMU]</color> {0} Médicos estão disponiveis para seu pedido!"
      },
      {
        "ReviveRequest",
        "<color=red>[SAMU]</color> O(a) {0} precisa de ajuda urgente! Segue a localização em seu gps!"
      },
      {
        "Hooking",
        "Voce pode carregar <color=red>{0}</color>!"
      }
    };

        private void UseableConsumeable_onPerformingAid(
          SDG.Unturned.Player instigator,
          SDG.Unturned.Player target,
          ItemConsumeableAsset asset,
          ref bool shouldAllow)
        {
            ReviveConfiguration Config = this.Configuration.Instance.ReviveConfig.FirstOrDefault<ReviveConfiguration>((Func<ReviveConfiguration, bool>)(X => (int)X.ItemID == (int)asset.id));
            DownedPlayer Downed = this.DownedPlayers.FirstOrDefault<DownedPlayer>((Func<DownedPlayer, bool>)(X => (long)X.CSteamID == (long)target.channel.owner.playerID.steamID.m_SteamID));
            if (Downed == null || Config == null || !R.Permissions.HasPermission((IRocketPlayer)UnturnedPlayer.FromPlayer(instigator), Config.NeededPermission))
                return;
            if (Config.TimeToHeal > 0)
            {
                ChatManager.serverSendMessage(this.Translate("BeingRevived"), Color.white, toPlayer: target.channel.owner, useRichTextFormatting: true);
                Downed.Revives.Add(new UMedical.Models.Medical.Values.Revive(instigator, Config));
            }
            else
                UMedical.Models.Medical.Values.Revive.ApplyRevive(Downed, Config);
        }

        private void Events_OnPlayerConnected(UnturnedPlayer player)
        {
            LoggerUtil.SendDebugLog("OnPlayerConnected > Started! {0} - {1}", player.DisplayName, player.CSteamID.m_SteamID.ToString());
            if (!this.DownedPlayers.Any<DownedPlayer>((Func<DownedPlayer, bool>)(X => (long)X.CSteamID == (long)player.CSteamID.m_SteamID)))
                return;
            LoggerUtil.SendDebugLog("OnPlayerConnected > Passed! {0}", player.DisplayName);
            DownedPlayer DownedPlayer = this.DownedPlayers.First<DownedPlayer>((Func<DownedPlayer, bool>)(X => (long)X.CSteamID == (long)player.CSteamID.m_SteamID));
            LoggerUtil.SendDebugLog("OnPlayerConnected > TDownedPlayer! {0}", (DownedPlayer == null).ToString());
            LoggerUtil.SendDebugLog("OnPlayerConnected > PlayerAdd! {0}", "Starting");
            DownedPlayer.AddPlayer(player.Player);
            if (Main.Instance.Configuration.Instance.DownedConfig.CanSeedpUpDeath)
            {
                DownedPlayer.Listener = new KeyListener(player.Player);
                DownedPlayer.Listener.SpeedUpRequested += DownedPlayer.Listener_SpeedUpRequested;
            }
            LoggerUtil.SendDebugLog("OnPlayerConnected > PlayerAdd! {0}", "Sucessfuly");
            DownedPlayer.RestLifeTime -= Main.Instance.Configuration.Instance.DownedConfig.ExitConfiguration.LifeTimeReduction;
            DownedPlayer.DeathCoroutine = Main.Instance.StartCoroutine(Main.DeathCoroutine(DownedPlayer, DownedPlayer.RestLifeTime));
            if (this.Configuration.Instance.DownedConfig.ExitConfiguration.KillOnReconnect)
            {
                LoggerUtil.SendDebugLog("OnPlayerConnected > KillOnReconnect! {0}", player.DisplayName);
                DownedPlayer.RestLifeTime = 0;
            }
            LoggerUtil.SendDebugLog("OnPlayerConnected > Finished! {0}", player.DisplayName);
            AfterEffects afterEffects = this.PlayersEffects.FirstOrDefault<AfterEffects>((Func<AfterEffects, bool>)(X => (UnityEngine.Object)X.Target == (UnityEngine.Object)player.Player));
            if (afterEffects == null)
                return;
            afterEffects.Coro = this.StartCoroutine(afterEffects.RemoveCountdown());
        }

        public void Events_OnPlayerDisconnected(UnturnedPlayer player)
        {
            LoggerUtil.SendDebugLog("OnPlayerDisconnected > Started! {0} - {1}", player.DisplayName, player.CSteamID.m_SteamID.ToString());
            if (!this.DownedPlayers.Any<DownedPlayer>((Func<DownedPlayer, bool>)(X => (long)X.CSteamID == (long)player.CSteamID.m_SteamID)))
                return;
            LoggerUtil.SendDebugLog("OnPlayerDisconnected > Passed! {0}", "Starting Dispose");
            this.DownedPlayers.First<DownedPlayer>((Func<DownedPlayer, bool>)(X => (long)X.CSteamID == (long)player.CSteamID.m_SteamID)).Dispose(true);
            LoggerUtil.SendDebugLog("OnPlayerDisconnected > Passed! {0}", "Disposed");
            if (Main.Instance.Configuration.Instance.DownedConfig.ExitConfiguration.DropInventoryOnExit)
            {
                for (int index = 0; index <= 6; ++index)
                {
                    if (player.Inventory.items[index] != null)
                    {
                        LoggerUtil.SendDebugLog("OnPlayerDisconnected > InventoryLog! {0}", index.ToString());
                        while (player.Inventory.items[index].items.Count<ItemJar>() > 0)
                        {
                            ItemManager.dropItem(player.Inventory.items[index].items[0].item, player.Player.transform.position, false, true, true);
                            player.Inventory.items[index].removeItem((byte)0);
                        }
                    }
                }
            }
            LoggerUtil.SendDebugLog("OnPlayerDisconnected > Finished! {0}", player.DisplayName);
            AfterEffects afterEffects = this.PlayersEffects.FirstOrDefault<AfterEffects>((Func<AfterEffects, bool>)(X => (UnityEngine.Object)X.Target == (UnityEngine.Object)player.Player));
            if (afterEffects == null)
                return;
            this.StopCoroutine(afterEffects.Coro);
            afterEffects.Coro = (Coroutine)null;
        }

        private void UnturnedPlayerEvents_OnPlayerDeath(
          UnturnedPlayer player,
          EDeathCause cause,
          ELimb limb,
          CSteamID murderer)
        {
            LoggerUtil.SendDebugLog("OnPlayerDeath > Started! {0}", player.DisplayName);
            if (!this.DownedPlayers.Any<DownedPlayer>((Func<DownedPlayer, bool>)(X => (long)X.CSteamID == (long)player.CSteamID.m_SteamID)))
            {
                Main.OnDead(player.Player);
            }
            else
            {
                LoggerUtil.SendDebugLog("OnPlayerDeath > Passed! {0}", player.DisplayName);
                DownedPlayer downedPlayer = this.DownedPlayers.First<DownedPlayer>((Func<DownedPlayer, bool>)(X => (long)X.CSteamID == (long)player.CSteamID.m_SteamID));
                LoggerUtil.SendDebugLog("OnPlayerDeath > TDownedPlayer! {0}", (downedPlayer == null).ToString());
                LoggerUtil.SendDebugLog("OnPlayerDeath > Starting Disposing! {0}", player.DisplayName);
                this.DownedPlayers.Remove(downedPlayer);
                downedPlayer.Dispose(true);
                LoggerUtil.SendDebugLog("OnPlayerDeath > Finished! / IsDead? {0}", player.Player.life.isDead.ToString());
                if (!Main.Instance.Configuration.Instance.MiscConfig.CensureReviveWindow)
                    return;

                player.Player.life.ServerRespawn(false);
            }
        }

        public static IEnumerator DeathCoroutine(DownedPlayer DownedPlayer, float Total)
        {
            float restLifeTime = DownedPlayer.RestLifeTime;
            DateTime LastScream = DateTime.MinValue;
            System.Random Randomizer = new System.Random();

            LoggerUtil.SendDebugLog("DeathCoroutine > Started! {0}", restLifeTime.ToString());
            SDG.Unturned.Player PlayerDowned = PlayerTool.getPlayer(new CSteamID(DownedPlayer.CSteamID));
            while (DownedPlayer.RestLifeTime > 0)
            {
                if ((DateTime.Now - LastScream).TotalSeconds > Main.Instance.Configuration.Instance.MiscConfig.ScreamConfig.MinCooldown && Randomizer.Next(0, 100) <= Main.Instance.Configuration.Instance.MiscConfig.ScreamConfig.ScreamChance)
                {
                    TriggerEffectParameters Params = new TriggerEffectParameters(Assets.find(EAssetType.EFFECT, Main.Instance.Configuration.Instance.MiscConfig.ScreamConfig.ScreamIDs.RandomOrDefault()) as EffectAsset);
                    Params.wasInstigatedByPlayer = true;
                    Params.shouldReplicate = true;
                    Params.relevantDistance = Main.Instance.Configuration.Instance.MiscConfig.ScreamConfig.RelevantDistance;
                    Params.position = UnturnedPlayer.FromCSteamID(new CSteamID(DownedPlayer.CSteamID)).Player.transform.position;
                    EffectManager.triggerEffect(Params);
                    LastScream = DateTime.Now;
                }
                yield return (object)new WaitForSeconds(0.98f);
                if (!DownedPlayer.Revives.Any<UMedical.Models.Medical.Values.Revive>((Func<UMedical.Models.Medical.Values.Revive, bool>)(X => Main.Instance.Configuration.Instance.ReviveConfig.FirstOrDefault<ReviveConfiguration>((Func<ReviveConfiguration, bool>)(Y => (int)Y.ItemID == (int)X.ReviveItem)).PauseDownedTimer)))
                    --DownedPlayer.RestLifeTime;
                EffectManager.sendUIEffectText((short)3701, PlayerDowned.channel.owner.transportConnection, true, "PORCENTAGE", (DownedPlayer.RestLifeTime * 10).ToString() + " " + Main.Instance.Configuration.Instance.UiConfig.LifeTimeSuffix);
                float porcentage = (DownedPlayer.RestLifeTime/Total) * 100;
                for(int uiindex = 1; uiindex <= 100; uiindex++)
                    EffectManager.sendUIEffectVisibility((short)3701, PlayerDowned.channel.owner.transportConnection, true, $"PORCENTAGE [1] ({uiindex})", uiindex <= porcentage);
            }
            restLifeTime = DownedPlayer.RestLifeTime;
            LoggerUtil.SendDebugLog("DeathCoroutine > Killing! {0}", DownedPlayer.RestLifeTime.ToString());
            DamageTool.damage(PlayerDowned, DownedPlayer.Info.Cause, DownedPlayer.Info.Limb, new CSteamID(DownedPlayer.Info.Killer), DownedPlayer.Info.Ragdoll.ToVector3(), (float)byte.MaxValue, 1f, out EPlayerKill _, false, ragdollEffect: DownedPlayer.Info.RagdollEffect);
            restLifeTime = DownedPlayer.RestLifeTime;
            LoggerUtil.SendDebugLog("DeathCoroutine > Finished! {0}", DownedPlayer.RestLifeTime.ToString());
        }

        public delegate void OnPlayerAdded(DownedPlayer Info);

        public delegate void OnPlayerDirectDead(SDG.Unturned.Player Info);

        public delegate void OnPlayerRemoved(DownedPlayer Info, bool ShouldForceUnhook);
    }
}
