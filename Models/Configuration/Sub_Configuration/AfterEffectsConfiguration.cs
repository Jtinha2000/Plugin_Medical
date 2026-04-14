// Decompiled with JetBrains decompiler
// Type: UMedical.Models.Configuration.Sub_Configuration.AfterEffectsConfiguration
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

namespace UMedical.Models.Configuration.Sub_Configuration
{
  public class AfterEffectsConfiguration
  {
    public float SpeedMultiplierReduction { get; set; }

    public float JumpMultiplierReduction { get; set; }

    public byte MaxLife { get; set; }

    public int Duration { get; set; }

    public AfterEffectsConfiguration()
    {
    }

    public AfterEffectsConfiguration(
      float speedMultiplierReduction,
      float jumpMultiplierReduction,
      byte maxLife,
      int duration)
    {
      this.SpeedMultiplierReduction = speedMultiplierReduction;
      this.JumpMultiplierReduction = jumpMultiplierReduction;
      this.MaxLife = maxLife;
      this.Duration = duration;
    }
  }
}
