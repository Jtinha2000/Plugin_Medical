// Decompiled with JetBrains decompiler
// Type: UMedical.Patches.PDamage
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

using HarmonyLib;
using Rocket.API;
using Rocket.API.Serialisation;
using Rocket.Core;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using UMedical.Models;
using UMedical.Models.Configuration.Sub_Configuration;
using UMedical.Models.Medical.Values;
using UMedical.Utils;
using UnityEngine;

namespace UMedical.Patches
{
    [HarmonyPatch(typeof(PlayerLife), "doDamage")]
    public static class PDamage
    {
        public static void Prefix(ref byte amount, Vector3 newRagdoll, EDeathCause newCause, ELimb newLimb, CSteamID newKiller, ref EPlayerKill kill, bool trackKill, ERagdollEffect newRagdollEffect, ref bool canCauseBleeding, PlayerLife __instance)
        {
            LoggerUtil.SendDebugLog("HarmonyPatch > DamagePatch > IsNullMurderer {0} | IsNullVictim {1}", (PlayerTool.getPlayer(newKiller) is null).ToString(), (__instance.player is null).ToString());
            if (PlayerTool.getPlayer(newKiller) != null && Main.Instance.Configuration.Instance.MiscConfig.IgnoreDamageWeapons.Contains(PlayerTool.getPlayer(newKiller).equipment.itemID))
                amount = 0;
            LoggerUtil.SendDebugLog("HarmonyPatch > DamagePatch > Amount {0}", amount.ToString());
            amount = (byte)(amount * (Main.Instance.DownedPlayers.Any(X => X.CSteamID == __instance.player.channel.owner.playerID.steamID.m_SteamID) ? Main.Instance.Configuration.Instance.DownedConfig.DamageMutliplierAfterDead.GetMultiplier[newLimb] : Main.Instance.Configuration.Instance.MiscConfig.DamageMutliplier.GetMultiplier[newLimb]));
            LoggerUtil.SendDebugLog("HarmonyPatch > DamagePatch > Amount {0} / Cause {1} / TgtTime {2}", amount.ToString(), newCause.ToString(), Main.Instance.Configuration.Instance.DownedConfig.LifeTime.First(X => X.Cause == newCause).Time.ToString());
            if (!__instance.isDead && !Main.Instance.DownedPlayers.Exists((Predicate<DownedPlayer>)(X => (long)X.CSteamID == (long)__instance.player.channel.owner.playerID.steamID.m_SteamID)) && (int)__instance.player.life.health - (int)amount <= (int)Main.Instance.Configuration.Instance.DownedConfig.MinimalLife && (Main.Instance.Configuration.Instance.DownedConfig.LifeTime.FirstOrDefault(X => X.Cause == newCause).Time * (Provider.clients.Count(X => X != __instance.player.channel.owner && R.Permissions.GetPermissions((IRocketPlayer)UnturnedPlayer.FromPlayer(__instance.player)).Any<Permission>((Func<Permission, bool>)(YX => YX.Name == Main.Instance.Configuration.Instance.MiscConfig.MedicalPermission))) != 0 ? 1 : Main.Instance.Configuration.Instance.DownedConfig.LifeTime.FirstOrDefault<LifeTime>((Func<LifeTime, bool>)(X => X.Cause == newCause)).MultiplierIfNoMedical) > 5 && !Main.Instance.Configuration.Instance.DownedConfig.DirectDeathWeapons.Contains((UnityEngine.Object)PlayerTool.getPlayer(newKiller) != (UnityEngine.Object)null ? PlayerTool.getPlayer(newKiller).equipment.itemID : (ushort)71)) && !(Main.Instance.Configuration.Instance.DownedConfig.HeadshotDirectDeath && newCause == EDeathCause.GUN && newLimb == ELimb.SKULL))
            {
                LoggerUtil.SendDebugLog("HarmonyPatch > DamagePatch > Added {0}", __instance.player.name);
                if (__instance.player.equipment.itemID != (ushort)0 && (Main.Instance.Configuration.Instance.DownedConfig.DropWeaponOnDowned || Main.Instance.Configuration.Instance.DownedConfig.DropItemsOnDowned) && (UnityEngine.Object)__instance.player.movement.getVehicle() == (UnityEngine.Object)null)
                {
                    byte index = __instance.player.inventory.items[(int)__instance.player.equipment.equippedPage].getIndex(__instance.player.equipment.equipped_x, __instance.player.equipment.equipped_y);
                    ItemManager.dropItem(__instance.player.inventory.items[(int)__instance.player.equipment.equippedPage].items[(int)index].item, new Vector3(__instance.player.transform.position.x, __instance.player.transform.position.y + 1f, __instance.player.transform.position.z), false, true, false);
                    __instance.player.inventory.items[(int)__instance.player.equipment.equippedPage].removeItem(index);
                }
                Main.Instance.DownedPlayers.Add(new DownedPlayer(__instance.player.channel.owner.playerID.steamID.m_SteamID, new KillInfo(newRagdollEffect, new SVector3(newRagdoll), (UnityEngine.Object)PlayerTool.getPlayer(newKiller) != (UnityEngine.Object)null ? newKiller.m_SteamID : 0UL, (UnityEngine.Object)PlayerTool.getPlayer(newKiller) != (UnityEngine.Object)null ? PlayerTool.getPlayer(newKiller).equipment.itemID : (ushort)0, newCause, UIUtil.GetLimb(newLimb)), (int)((double)((IEnumerable<LifeTime>)Main.Instance.Configuration.Instance.DownedConfig.LifeTime).FirstOrDefault<LifeTime>((Func<LifeTime, bool>)(X => X.Cause == newCause)).Time * (Provider.clients.Count<SteamPlayer>((Func<SteamPlayer, bool>)(X => X != __instance.player.channel.owner && R.Permissions.GetPermissions((IRocketPlayer)UnturnedPlayer.FromPlayer(__instance.player)).Any<Permission>((Func<Permission, bool>)(YX => YX.Name == Main.Instance.Configuration.Instance.MiscConfig.MedicalPermission)))) != 0 ? 1.0 : (double)((IEnumerable<LifeTime>)Main.Instance.Configuration.Instance.DownedConfig.LifeTime).FirstOrDefault<LifeTime>((Func<LifeTime, bool>)(X => X.Cause == newCause)).MultiplierIfNoMedical))));
                amount = (byte)0;
            }
            else
            {
                int num;
                if (!__instance.isDead)
                {
                    DownedPlayer downedPlayer = Main.Instance.DownedPlayers.Find((Predicate<DownedPlayer>)(X => (long)X.CSteamID == (long)__instance.player.channel.owner.playerID.steamID.m_SteamID));
                    num = downedPlayer != null ? (downedPlayer.RestLifeTime > 0 ? 1 : 0) : 0;
                }
                else
                    num = 0;
                if (num == 0)
                    return;
                amount = (DateTime.Now - Main.Instance.DownedPlayers.Find((Predicate<DownedPlayer>)(X => (long)X.CSteamID == (long)__instance.player.channel.owner.playerID.steamID.m_SteamID)).Info.Time).TotalSeconds >= (double)Main.Instance.Configuration.Instance.DownedConfig.ImmunityAfterDeadSeconds && Main.Instance.Configuration.Instance.DownedConfig.CanBeFinalized || (int)__instance.player.life.health - (int)(byte)((double)amount * (double)Main.Instance.Configuration.Instance.DownedConfig.DamageMutliplierAfterDead.GetMultiplier[newLimb]) > (int)Main.Instance.Configuration.Instance.DownedConfig.MinimalLife ? (byte)((double)amount * (double)Main.Instance.Configuration.Instance.DownedConfig.DamageMutliplierAfterDead.GetMultiplier[newLimb]) : (byte)((uint)__instance.player.life.health - (uint)Main.Instance.Configuration.Instance.DownedConfig.MinimalLife);
                LoggerUtil.SendDebugLog("HarmonyPatch > DamagePatch > RestLifeTimeBiggerThan0 {0} > Amount {1}", __instance.player.name, amount.ToString());
            }
        }
    }
}
