// Decompiled with JetBrains decompiler
// Type: UMedical.Models.Samu.Revive.AfterEffects
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

using SDG.Unturned;
using System.Collections;
using UMedical.Events;
using UMedical.Models.Configuration;
using UMedical.Utils;
using UnityEngine;

namespace UMedical.Models.Samu.Revive
{
  public class AfterEffects
  {
    public Player Target { get; set; }

    public int Timer { get; set; }

    public ReviveConfiguration Revive { get; set; }

    public Coroutine Coro { get; set; }

    public AfterEffects()
    {
    }

    public AfterEffects(Player target, ReviveConfiguration revive)
    {
      this.Target = target;
      this.Revive = revive;
      this.Timer = this.Revive.AfterEffectsConfig.Duration;
      this.Coro = Main.Instance.StartCoroutine(this.RemoveCountdown());
    }

    public IEnumerator RemoveCountdown()
    {
      LoggerUtil.SendDebugLog("AfterEffects #01 > RemoveCountdown! Speed: {0}", this.Target.movement.pluginSpeedMultiplier.ToString());
      this.Target.life.onLifeUpdated += (LifeUpdated) (x => AfterEffectsEvents.OnLifeUpdated(this.Target));
      this.Target.movement.sendPluginSpeedMultiplier(this.Target.movement.pluginSpeedMultiplier * this.Revive.AfterEffectsConfig.SpeedMultiplierReduction);
      this.Target.movement.sendPluginJumpMultiplier(this.Target.movement.pluginJumpMultiplier * this.Revive.AfterEffectsConfig.JumpMultiplierReduction);
      LoggerUtil.SendDebugLog("AfterEffects #02 > RemoveCountdown! Speed: {0}", this.Target.movement.pluginSpeedMultiplier.ToString());
      for (; this.Timer > 0; --this.Timer)
        yield return (object) new WaitForSeconds(1f);
      LoggerUtil.SendDebugLog("AfterEffects #03 > RemoveCountdown!");
      this.Dispose();
    }

    public void Dispose()
    {
      if (this.Coro != null)
        Main.Instance.StopCoroutine(this.Coro);
      LoggerUtil.SendDebugLog("AfterEffects #01 > Dispose! Speed: {0}", this.Target.movement.pluginSpeedMultiplier.ToString());
      this.Target.life.onLifeUpdated -= (LifeUpdated) (x => AfterEffectsEvents.OnLifeUpdated(this.Target));
      this.Target.movement.sendPluginSpeedMultiplier(this.Target.movement.pluginSpeedMultiplier / this.Revive.AfterEffectsConfig.SpeedMultiplierReduction);
      this.Target.movement.sendPluginJumpMultiplier(this.Target.movement.pluginJumpMultiplier / this.Revive.AfterEffectsConfig.JumpMultiplierReduction);
      LoggerUtil.SendDebugLog("AfterEffects #02 > Dispose! Speed: {0}", this.Target.movement.pluginSpeedMultiplier.ToString());
      Main.Instance.PlayersEffects.Remove(this);
    }
  }
}
