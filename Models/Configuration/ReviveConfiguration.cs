// Decompiled with JetBrains decompiler
// Type: UMedical.Models.Configuration.ReviveConfiguration
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

using SDG.Unturned;
using System.Collections.Generic;
using UMedical.Models.Configuration.Sub_Configuration;

namespace UMedical.Models.Configuration
{
  public class ReviveConfiguration
  {
    public ushort ItemID { get; set; }

    public string NeededPermission { get; set; }

    public List<EDeathCause> BlacklistedCauses { get; set; }

    public List<ELimb> WhitelistedLimbs { get; set; }

    public bool InappropriatedUseCauseFail { get; set; }

    public AfterEffectsConfiguration AfterEffectsConfig { get; set; }

    public int TimeToHeal { get; set; }

    public bool PauseDownedTimer { get; set; }

    public int FailChance { get; set; }

    public byte HealthReductionOnFail { get; set; }

    public int TimeReductionOnFail { get; set; }

    public ushort FailEffectID { get; set; }

    public ushort SucessEffectID { get; set; }

    public byte MinReviveVirus { get; set; }

    public byte MaxReviveVirus { get; set; }

    public byte MinReviveHealth { get; set; }

    public byte MaxReviveHealth { get; set; }

    public ReviveConfiguration()
    {
    }

    public ReviveConfiguration(
      AfterEffectsConfiguration afterEffectsConfig,
      bool WrongUsePunish,
      List<ELimb> Limbs,
      bool Pause,
      List<EDeathCause> blacklistedCauses,
      ushort itemID,
      string neededPermission,
      int timeToHeal,
      int failChance,
      byte healthReductionOnFail,
      int timeReductionOnFail,
      ushort failEffectID,
      ushort sucessEffectID,
      byte minReviveVirus,
      byte maxReviveVirus,
      byte minReviveHealth,
      byte maxReviveHealth)
    {
      this.AfterEffectsConfig = afterEffectsConfig;
      this.InappropriatedUseCauseFail = WrongUsePunish;
      this.WhitelistedLimbs = Limbs;
      this.PauseDownedTimer = Pause;
      this.ItemID = itemID;
      this.NeededPermission = neededPermission;
      this.BlacklistedCauses = blacklistedCauses;
      this.TimeToHeal = timeToHeal;
      this.FailChance = failChance;
      this.HealthReductionOnFail = healthReductionOnFail;
      this.TimeReductionOnFail = timeReductionOnFail;
      this.FailEffectID = failEffectID;
      this.SucessEffectID = sucessEffectID;
      this.MinReviveVirus = minReviveVirus;
      this.MaxReviveVirus = maxReviveVirus;
      this.MinReviveHealth = minReviveHealth;
      this.MaxReviveHealth = maxReviveHealth;
    }
  }
}
