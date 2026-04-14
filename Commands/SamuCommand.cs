// Decompiled with JetBrains decompiler
// Type: UMedical.Commands.SamuCommand
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

using Rocket.API;
using Rocket.API.Serialisation;
using Rocket.Core;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using UMedical.Models;
using UnityEngine;

namespace UMedical.Commands
{
  public class SamuCommand : IRocketCommand
  {
    public AllowedCaller AllowedCaller => AllowedCaller.Player;

    public string Name => nameof (SamuCommand);

    public string Help => "Can be used to request fast help!";

    public string Syntax => "/Samu";

    public List<string> Aliases => new List<string>()
    {
      "Help",
      "Samu"
    };

    public List<string> Permissions => new List<string>()
    {
      "MedicalSamu"
    };

    public void Execute(IRocketPlayer caller, string[] command)
    {
      SDG.Unturned.Player Caller = PlayerTool.getPlayer(caller.DisplayName);
      if (!Main.Instance.DownedPlayers.Any<DownedPlayer>((Func<DownedPlayer, bool>) (X => (long) X.CSteamID == (long) Caller.channel.owner.playerID.steamID.m_SteamID)))
        return;
      foreach (SteamPlayer toPlayer in Provider.clients.Where<SteamPlayer>((Func<SteamPlayer, bool>) (X => (UnityEngine.Object) X.player != (UnityEngine.Object) Caller && !X.player.life.isDead && R.Permissions.GetPermissions((IRocketPlayer) UnturnedPlayer.FromCSteamID(X.playerID.steamID)).Any<Permission>((Func<Permission, bool>) (Y => Y.Name == Main.Instance.Configuration.Instance.MiscConfig.MedicalPermission)))))
      {
        ChatManager.serverSendMessage(Main.Instance.Translate("ReviveRequest", (object) caller.DisplayName), Color.white, toPlayer: toPlayer, iconURL: Main.Instance.Configuration.Instance.MiscConfig.AvatarURL, useRichTextFormatting: true);
        toPlayer.player.quests.replicateSetMarker(true, Caller.transform.position, caller.DisplayName);
      }
      ChatManager.serverSendMessage(Main.Instance.Translate("ReviveRequestSended", (object) Provider.clients.Where<SteamPlayer>((Func<SteamPlayer, bool>) (X => (UnityEngine.Object) X.player != (UnityEngine.Object) Caller && !X.player.life.isDead && R.Permissions.GetPermissions((IRocketPlayer) UnturnedPlayer.FromCSteamID(X.playerID.steamID)).Any<Permission>((Func<Permission, bool>) (Y => Y.Name == Main.Instance.Configuration.Instance.MiscConfig.MedicalPermission)))).Count<SteamPlayer>()), Color.white, toPlayer: Caller.channel.owner, iconURL: Main.Instance.Configuration.Instance.MiscConfig.AvatarURL, useRichTextFormatting: true);
    }
  }
}
