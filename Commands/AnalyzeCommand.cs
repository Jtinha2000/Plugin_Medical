// Decompiled with JetBrains decompiler
// Type: UMedical.Commands.AnalyzeCommand
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

using Rocket.API;
using Rocket.Core;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using UMedical.Models;
using UMedical.Models.Configuration;
using UMedical.Models.Configuration.Sub_Configuration;
using UMedical.Models.Hooking;
using UMedical.Utils;
using UnityEngine;

namespace UMedical.Commands
{
  public sealed class AnalyzeCommand : IRocketCommand
  {
    public AllowedCaller AllowedCaller => AllowedCaller.Player;

    public string Name => "Analyze";

    public string Help => "Can be used to analyze downed players!";

    public string Syntax => "/Analyze";

    public List<string> Aliases => new List<string>()
    {
      "Analisar"
    };

    public List<string> Permissions => new List<string>()
    {
      "MedicalAnalyze"
    };

    public void Execute(IRocketPlayer caller, string[] command)
    {
      SDG.Unturned.Player player = PlayerTool.getPlayer(caller.DisplayName);
      if (!R.Permissions.HasPermission((IRocketPlayer) UnturnedPlayer.FromPlayer(player), Main.Instance.Configuration.Instance.MiscConfig.AnalyzePermission))
      {
        ChatManager.serverSendMessage(Main.Instance.Translate("AnalyzeCommandNoPermission", (object) Main.Instance.Configuration.Instance.MiscConfig.AnalyzePermission), Color.white, toPlayer: player.channel.owner, iconURL: Main.Instance.Configuration.Instance.MiscConfig.AvatarURL, useRichTextFormatting: true);
      }
      else
      {
        SDG.Unturned.Player Target = Main.Instance.HookingSessions.FirstOrDefault<HookingSession>((Func<HookingSession, bool>) (X => (UnityEngine.Object) X.Hooker == (UnityEngine.Object) player))?.Victim;
        DownedPlayer Downed = Main.Instance.DownedPlayers.FirstOrDefault<DownedPlayer>((Func<DownedPlayer, bool>) (X => (long) X.CSteamID == ((UnityEngine.Object) Target == (UnityEngine.Object) null ? 0L : (long) Target.channel.owner.playerID.steamID.m_SteamID)));
        if ((UnityEngine.Object) Target == (UnityEngine.Object) null || Downed == null)
        {
          ChatManager.serverSendMessage(Main.Instance.Translate("AnalyzeCommandNullTarget", (object) Target.channel.owner.playerID.characterName), Color.white, toPlayer: player.channel.owner, iconURL: Main.Instance.Configuration.Instance.MiscConfig.AvatarURL, useRichTextFormatting: true);
        }
        else
        {
          EffectManager.sendUIEffect((ushort) 3709, (short) 3709, player.channel.owner.transportConnection, true);
          EffectManager.sendUIEffectText((short) 3709, player.channel.owner.transportConnection, true, "HELPTOPIC", "ANALISE");
          EffectManager.sendUIEffectText((short) 3709, player.channel.owner.transportConnection, true, "DEATHCAUSE", Main.Instance.Configuration.Instance.UiConfig.Translates.FirstOrDefault<Traduction>((Func<Traduction, bool>) (X => X.Cause == Downed.Info.Cause)).Translate + "...");
          EffectManager.sendUIEffectVisibility((short) 3709, player.channel.owner.transportConnection, true, UIUtil.GetLimbName(Downed.Info.Limb), false);
          for (int index = 100; index > 0; --index)
            EffectManager.sendUIEffectVisibility((short) 3709, player.channel.owner.transportConnection, true, string.Format("HP({0})", (object) index), index <= Downed.RestLifeTime / (int) ((IEnumerable<LifeTime>) Main.Instance.Configuration.Instance.DownedConfig.LifeTime).FirstOrDefault<LifeTime>((Func<LifeTime, bool>) (X => X.Cause == Downed.Info.Cause)).Time * 100);
          ChatManager.serverSendMessage(Main.Instance.Translate("AnalyzeCommandSucess", (object) Target.channel.owner.playerID.characterName), Color.white, toPlayer: player.channel.owner, iconURL: Main.Instance.Configuration.Instance.MiscConfig.AvatarURL, useRichTextFormatting: true);
        }
      }
    }
  }
}
