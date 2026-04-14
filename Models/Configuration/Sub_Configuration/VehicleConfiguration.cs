// Decompiled with JetBrains decompiler
// Type: UMedical.Models.Configuration.Sub_Configuration.VehicleConfiguration
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

using SDG.Unturned;
using System.Collections.Generic;

namespace UMedical.Models.Configuration.Sub_Configuration
{
  public sealed class VehicleConfiguration
  {
    public bool BlockExitOnMinKPH { get; set; }

    public List<EEngine> BlacklistToDamage { get; set; }

    public float DamageByKPH { get; set; }

    public int MinKPHToDamage { get; set; }

    public EDeathCause CauseToKPHDeath { get; set; }

    public VehicleConfiguration()
    {
    }

    public VehicleConfiguration(
      bool blockExitOnMinKPH,
      List<EEngine> blacklistToDamage,
      float damageByKPH,
      int minKPHToDamage,
      EDeathCause causeToKPHDeath)
    {
      this.BlockExitOnMinKPH = blockExitOnMinKPH;
      this.BlacklistToDamage = blacklistToDamage;
      this.DamageByKPH = damageByKPH;
      this.MinKPHToDamage = minKPHToDamage;
      this.CauseToKPHDeath = causeToKPHDeath;
    }
  }
}
