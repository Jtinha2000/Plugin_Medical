// Decompiled with JetBrains decompiler
// Type: UMedical.Models.Configuration.HookingConfiguration
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

namespace UMedical.Models.Configuration
{
  public sealed class HookingConfiguration
  {
    public bool AcessToArrestedDoors { get; set; }

    public float HookerSpeedMultiplier { get; set; }

    public float HookerJumpMultiplier { get; set; }

    public float HookerGravityMultiplier { get; set; }

    public HookingConfiguration()
    {
    }

    public HookingConfiguration(bool __)
    {
      this.AcessToArrestedDoors = true;
      this.HookerGravityMultiplier = 1.5f;
      this.HookerJumpMultiplier = 0.5f;
      this.HookerSpeedMultiplier = 0.5f;
    }
  }
}
