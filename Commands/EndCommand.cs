// Decompiled with JetBrains decompiler
// Type: UMedical.Commands.EndCommand
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

using Rocket.API;
using Rocket.Unturned.Commands;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using UMedical.Models;
using UnityEngine;

namespace UMedical.Commands
{
  public sealed class EndCommand : IRocketCommand
  {
    public AllowedCaller AllowedCaller => AllowedCaller.Both;

    public string Name => "End";

    public string Help => "Can be used to finalize downed players!";

    public string Syntax => "/End <Player>";

    public List<string> Aliases => new List<string>()
    {
      "Finalizar"
    };

    public List<string> Permissions => new List<string>()
    {
      "MedicalEnd"
    };

    public void Execute(IRocketPlayer caller, string[] command)
    {
      Player Target = command.Length != 0 ? ((UnityEngine.Object) command.GetUnturnedPlayerParameter(0)?.Player != (UnityEngine.Object) null ? command.GetUnturnedPlayerParameter(0).Player : PlayerTool.getPlayer(command[0])) : (Player) null;
      Player player = PlayerTool.getPlayer(caller.DisplayName);
      if ((UnityEngine.Object) Target == (UnityEngine.Object) null || !Main.Instance.DownedPlayers.Any<DownedPlayer>((Func<DownedPlayer, bool>) (X => (long) X.CSteamID == (long) Target.channel.owner.playerID.steamID.m_SteamID)))
      {
        if ((UnityEngine.Object) player != (UnityEngine.Object) null)
          ChatManager.serverSendMessage(Main.Instance.Translate("EndCommandNullTarget", (object) Target.channel.owner.playerID.characterName), Color.white, toPlayer: player.channel.owner, iconURL: Main.Instance.Configuration.Instance.MiscConfig.AvatarURL, useRichTextFormatting: true);
        else
          Rocket.Core.Logging.Logger.Log(Main.Instance.Translate("EndCommandNullTarget", (object) Target.channel.owner.playerID.characterName));
      }
      else
      {
        Main.Instance.DownedPlayers.FirstOrDefault<DownedPlayer>((Func<DownedPlayer, bool>) (X => (long) X.CSteamID == (long) Target.channel.owner.playerID.steamID.m_SteamID)).RestLifeTime = 0;
        if ((UnityEngine.Object) player != (UnityEngine.Object) null)
          ChatManager.serverSendMessage(Main.Instance.Translate("EndCommandSucess", (object) Target.channel.owner.playerID.characterName), Color.white, toPlayer: player.channel.owner, iconURL: Main.Instance.Configuration.Instance.MiscConfig.AvatarURL, useRichTextFormatting: true);
        else
          Rocket.Core.Logging.Logger.Log(Main.Instance.Translate("EndCommandSucess", (object) Target.channel.owner.playerID.characterName));
      }
    }
  }
}
