// Decompiled with JetBrains decompiler
// Type: UMedical.Utils.HookinViewerUtil
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using UMedical.Models;
using UMedical.Models.Hooking;
using UnityEngine;

namespace UMedical.Utils
{
  public class HookinViewerUtil : UnturnedPlayerComponent
  {
    public bool HasUI { get; set; } = false;

    public void Update()
    {
      SDG.Unturned.Player Target = HookinViewerUtil.GetPlayer(this.Player.Player);
      if ((UnityEngine.Object) Target == (UnityEngine.Object) null || this.Player.Player.life.isDead || Main.Instance.DownedPlayers.Any<DownedPlayer>((Func<DownedPlayer, bool>) (X => (long) X.CSteamID == (long) this.Player.CSteamID.m_SteamID)) || Target.animator.gesture != EPlayerGesture.ARREST_START && !Main.Instance.DownedPlayers.Any<DownedPlayer>((Func<DownedPlayer, bool>) (X => (long) X.CSteamID == (long) Target.channel.owner.playerID.steamID.m_SteamID)) || (UnityEngine.Object) this.Player.Player.movement.getVehicle() != (UnityEngine.Object) null && ((IEnumerable<Passenger>) this.Player.Player.movement.getVehicle().passengers).Count<Passenger>((Func<Passenger, bool>) (X => X.player == null)) < 2 || Main.Instance.HookingSessions.Any<HookingSession>((Func<HookingSession, bool>) (X => (UnityEngine.Object) X.Victim == (UnityEngine.Object) this.Player.Player || (UnityEngine.Object) X.Victim == (UnityEngine.Object) Target || (UnityEngine.Object) X.Hooker == (UnityEngine.Object) this.Player.Player || (UnityEngine.Object) X.Hooker == (UnityEngine.Object) Target)))
      {
        if (!this.HasUI)
          return;
        EffectManager.askEffectClearByID((ushort) 3703, this.Player.Player.channel.owner.transportConnection);
        this.HasUI = false;
      }
      else
      {
        if (!this.HasUI)
        {
          EffectManager.sendUIEffect((ushort) 3703, (short) 3703, this.Player.Player.channel.owner.transportConnection, true);
          this.HasUI = true;
        }
        EffectManager.sendUIEffectText((short) 3703, this.Player.Player.channel.owner.transportConnection, true, "TITLE", Main.Instance.Translate("Hooking", (object) Target.channel.owner.playerID.characterName));
      }
    }

    public static SDG.Unturned.Player GetPlayer(SDG.Unturned.Player player)
    {
      RaycastHit[] source1 = Physics.SphereCastAll(new Ray(player.look.aim.position, player.look.aim.forward), 0.1f, 4f, RayMasks.PLAYER_INTERACT | 1024);
      List<RaycastHit> source2 = source1 != null ? ((IEnumerable<RaycastHit>) source1).ToList<RaycastHit>().FindAll((Predicate<RaycastHit>) (x =>
      {
        SDG.Unturned.Player Player = DamageTool.getPlayer(x.transform);
        return (UnityEngine.Object) Player != (UnityEngine.Object) null && (UnityEngine.Object) Player != (UnityEngine.Object) player && (Main.Instance.DownedPlayers.Any<DownedPlayer>((Func<DownedPlayer, bool>) (X => (long) X.CSteamID == (long) Player.channel.owner.playerID.steamID.m_SteamID)) || Player.animator.gesture == EPlayerGesture.ARREST_START);
      })) : (List<RaycastHit>) null;
      return source2.Count == 0 ? (SDG.Unturned.Player) null : DamageTool.getPlayer(source2.FirstOrDefault<RaycastHit>().transform);
    }
  }
}
