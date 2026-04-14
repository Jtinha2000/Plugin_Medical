// Decompiled with JetBrains decompiler
// Type: UMedical.Models.Medical.Values.Revive
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

using SDG.Unturned;
using Steamworks;
using System;
using System.Collections;
using System.Linq;
using UMedical.Models.Configuration;
using UMedical.Models.Samu.Revive;
using UMedical.Utils;
using UnityEngine;

namespace UMedical.Models.Medical.Values
{
  public class Revive
  {
    public Player Medic { get; set; }

    public ushort ReviveItem { get; set; }

    public int Timer { get; set; }

    public Coroutine Countdown { get; set; }

    public Revive()
    {
    }

    public Revive(Player medic, ReviveConfiguration Config)
    {
      this.Medic = medic;
      this.ReviveItem = Config.ItemID;
      this.Timer = Config.TimeToHeal;
      this.Countdown = Main.Instance.StartCoroutine(this.CountdownCoroutine(Config));
      LoggerUtil.SendDebugLog("Revive > Revive Instanciated! {0}", Config.ItemID.ToString());
    }

    public IEnumerator CountdownCoroutine(ReviveConfiguration Config)
    {
      LoggerUtil.SendDebugLog("Revive > Countdown Started! {0}", this.Timer.ToString());
      for (; this.Timer > 0; --this.Timer)
        yield return (object) new WaitForSeconds(1f);
      DownedPlayer Target = Main.Instance.DownedPlayers.FirstOrDefault<DownedPlayer>((Func<DownedPlayer, bool>) (X => X.Revives.Contains(this)));
      Target.Revives.Remove(this);
      UMedical.Models.Medical.Values.Revive.ApplyRevive(Target, Config);
      LoggerUtil.SendDebugLog("Revive > Countdown Ended! {0}", this.Medic.channel.owner.playerID.characterName);
    }

    public void StopRevive()
    {
      LoggerUtil.SendDebugLog("Revive > ReviveStoped! {0}", this.Medic.channel.owner.playerID.characterName);
      Main.Instance.StopCoroutine(this.Countdown);
    }

    public static void ApplyRevive(DownedPlayer Downed, ReviveConfiguration Config)
    {
      Player player = PlayerTool.getPlayer(new CSteamID(Downed.CSteamID));
      LoggerUtil.SendDebugLog("ApplyRevive #01 > Started {0} / ItemID: {1}", player.channel.owner.playerID.characterName, Config.ItemID.ToString());
      if (new System.Random().Next(0, 100) <= Config.FailChance || (Config.BlacklistedCauses.Contains(Downed.Info.Cause) || !Config.WhitelistedLimbs.Contains(Downed.Info.Limb)) && Config.InappropriatedUseCauseFail)
      {
        LoggerUtil.SendDebugLog("ApplyRevive #02 > Failed {0} / Cause: {1} / Limb: {2} / HasCause: {3} / HasLimb: {4} ", player.channel.owner.playerID.characterName, Downed.Info.Cause.ToString(), Downed.Info.Limb.ToString(), Config.BlacklistedCauses.Contains(Downed.Info.Cause).ToString(), Config.WhitelistedLimbs.Contains(Downed.Info.Limb).ToString());
        Downed.RestLifeTime -= Config.TimeReductionOnFail;
        if (Downed.RestLifeTime > 0)
          player.life.ReceiveHealth((byte) ((uint) player.life.health - (uint) Config.HealthReductionOnFail));
        EffectManager.sendEffect(Config.FailEffectID, 30f, player.transform.position);
      }
      else
      {
                
        LoggerUtil.SendDebugLog("ApplyRevive #02 > Pre-Sucess {0} / Cause: {1} / Limb: {2} / HasCause: {3} / HasLimb: {4} ", player.channel.owner.playerID.characterName,Downed.Info.Cause.ToString(),
          Downed.Info.Limb.ToString(), Config.BlacklistedCauses.Contains(Downed.Info.Cause).ToString(), Config.WhitelistedLimbs.Contains(Downed.Info.Limb).ToString());

        if (Config.BlacklistedCauses.Contains(Downed.Info.Cause) || !Config.WhitelistedLimbs.Contains(Downed.Info.Limb))
          return;

        LoggerUtil.SendDebugLog("ApplyRevive #01 > Sucess {0} / ItemID: {1}", player.channel.owner.playerID.characterName, Config.ItemID.ToString());
        player.life.ReceiveHealth((byte) new System.Random().Next((int) Config.MinReviveHealth, (int) Config.MaxReviveHealth));
        player.life.ReceiveVirus((byte) new System.Random().Next((int) Config.MinReviveVirus, (int) Config.MaxReviveVirus));
        EffectManager.sendEffect(Config.SucessEffectID, 30f, player.transform.position);
        Main.Instance.DownedPlayers.Remove(Downed);
        Downed.Dispose(false);
        Main.Instance.PlayersEffects.Add(new AfterEffects(player, Config));
      }
    }
  }
}
