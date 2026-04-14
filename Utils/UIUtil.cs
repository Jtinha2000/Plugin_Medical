// Decompiled with JetBrains decompiler
// Type: UMedical.Utils.UIUtil
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

using Rocket.API;
using Rocket.API.Serialisation;
using Rocket.Core;
using Rocket.Unturned.Player;
using SDG.NetTransport;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UMedical.Models;
using UMedical.Models.Configuration;
using UMedical.Models.Samu.Values;
using UnityEngine;

namespace UMedical.Utils
{
  public static class UIUtil
  {
    public static Dictionary<SDG.Unturned.Player, RequestInfo> Requests { get; set; } = new Dictionary<SDG.Unturned.Player, RequestInfo>();

    public static IEnumerator GlobalHintCooldown()
    {
      yield return (object) new WaitForSeconds((float) Main.Instance.Configuration.Instance.UiConfig.IntervalBeetwenHints);
      Main.Instance.DownedPlayers.FindAll((Predicate<DownedPlayer>) (X => (UnityEngine.Object) PlayerTool.getPlayer(new CSteamID(X.CSteamID)) != (UnityEngine.Object) null)).ForEach((Action<DownedPlayer>) (X =>
      {
        PlayerTool.getPlayer(new CSteamID(X.CSteamID));
        string text1 = Main.Instance.Configuration.Instance.UiConfig.PossibleHints.RandomOrDefault<string>();
        EffectManager.sendUIEffectText((short) 3701, PlayerTool.getPlayer(new CSteamID(X.CSteamID)).channel.owner.transportConnection, true, "HINTTEXT", text1);
        ITransportConnection transportConnection = PlayerTool.getPlayer(new CSteamID(X.CSteamID)).channel.owner.transportConnection;
        int num = Main.Instance.Configuration.Instance.UiConfig.PossibleHints.IndexOf(text1) + 1;
        string str1 = num.ToString();
        num = Main.Instance.Configuration.Instance.UiConfig.PossibleHints.Count + 1;
        string str2 = num.ToString();
        string text2 = str1 + "/" + str2;
        EffectManager.sendUIEffectText((short) 3701, transportConnection, true, "HINTNUMBER", text2);
      }));
      Main.Instance.StartCoroutine(UIUtil.GlobalHintCooldown());
    }

    public static void OnButtonClicked(SDG.Unturned.Player Player, string ButtonName)
    {
      DownedPlayer Downed = Main.Instance.DownedPlayers.FirstOrDefault<DownedPlayer>((Func<DownedPlayer, bool>) (X => (long) X.CSteamID == (long) Player.channel.owner.playerID.steamID.m_SteamID));
      switch (ButtonName)
      {
        case "HINTEXIT":
          EffectManager.sendUIEffectVisibility((short) 3701, Player.channel.owner.transportConnection, true, "Hint", false);
          EffectManager.sendUIEffectVisibility((short) 3701, Player.channel.owner.transportConnection, true, "HINTENTER", true);
          break;
        case "HELPEXIT":
          EffectManager.sendUIEffectVisibility((short) 3701, Player.channel.owner.transportConnection, true, "Help", false);
          EffectManager.sendUIEffectVisibility((short) 3701, Player.channel.owner.transportConnection, true, "HELPENTER", true);
          break;
        case "HINTENTER":
          EffectManager.sendUIEffectVisibility((short) 3701, Player.channel.owner.transportConnection, true, "Hint", true);
          EffectManager.sendUIEffectVisibility((short) 3701, Player.channel.owner.transportConnection, true, "HINTENTER", false);
          break;
        case "HELPENTER":
          EffectManager.sendUIEffectVisibility((short) 3701, Player.channel.owner.transportConnection, true, "Help", true);
          EffectManager.sendUIEffectVisibility((short) 3701, Player.channel.owner.transportConnection, true, "HELPENTER", false);
          EffectManager.sendUIEffectText((short) 3701, Player.channel.owner.transportConnection, true, "MEDICSprox", string.Format("{0} MEDICOS...", (object) Provider.clients.Count<SteamPlayer>((Func<SteamPlayer, bool>) (X => R.Permissions.GetPermissions((IRocketPlayer) UnturnedPlayer.FromPlayer(X.player)).Any<Permission>((Func<Permission, bool>) (Y => Y.Name.ToLower() == Main.Instance.Configuration.Instance.MiscConfig.MedicalPermission.ToLower()))))));
          EffectManager.sendUIEffectText((short) 3701, Player.channel.owner.transportConnection, true, "MEDICSDISPO", string.Format("{0} MEDICOS...", (object) Provider.clients.Count<SteamPlayer>((Func<SteamPlayer, bool>) (X => (double) (X.player.transform.position - Player.transform.position).sqrMagnitude < (double) Main.Instance.Configuration.Instance.UiConfig.MaxProximityMedicalUIDIstance && R.Permissions.GetPermissions((IRocketPlayer) UnturnedPlayer.FromPlayer(X.player)).Any<Permission>((Func<Permission, bool>) (Y => Y.Name.ToLower() == Main.Instance.Configuration.Instance.MiscConfig.MedicalPermission.ToLower()))))));
          EffectManager.sendUIEffectText((short) 3701, Player.channel.owner.transportConnection, true, "DEATHCAUSE", Main.Instance.Configuration.Instance.UiConfig.Translates.FirstOrDefault<Traduction>((Func<Traduction, bool>) (X => X.Cause == Downed.Info.Cause)).Translate + "...");
          EffectManager.sendUIEffectVisibility((short) 3701, Player.channel.owner.transportConnection, true, UIUtil.GetLimbName(Downed.Info.Limb), false);
          break;
        case "callmedics":
          R.Commands.Execute((IRocketPlayer) UnturnedPlayer.FromPlayer(Player), "SamuCommand");
          break;
      }
    }

    public static void OnTextComitted(SDG.Unturned.Player Player, string InputName, string Text)
    {
    }

    public static string GetLimbName(ELimb Limb) => UIUtil.GetLimb(Limb).ToString() != "LEFT_BACK" ? UIUtil.GetLimb(Limb).ToString() : "CHEST";

    public static ELimb GetLimb(ELimb Limb)
    {
      switch (Limb)
      {
        case ELimb.LEFT_BACK:
        case ELimb.RIGHT_BACK:
        case ELimb.LEFT_FRONT:
        case ELimb.RIGHT_FRONT:
          return ELimb.LEFT_BACK;
        case ELimb.SPINE:
          return ELimb.SKULL;
        default:
          return Limb;
      }
    }
  }
}
