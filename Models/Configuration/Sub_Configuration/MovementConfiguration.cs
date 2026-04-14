// Decompiled with JetBrains decompiler
// Type: UMedical.Models.Configuration.Sub_Configuration.MovementConfiguration
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

using SDG.Unturned;

namespace UMedical.Models.Configuration.Sub_Configuration
{
  public sealed class MovementConfiguration
  {
    public float SpeedMultiplier { get; set; }

    public float JumpMultiplier { get; set; }

    public float GravityMultiplier { get; set; }

    public bool CanEnterOnVehicle { get; set; }

    public EPlayerStance DefaultStance { get; set; }

    public MovementConfiguration()
    {
    }

    public MovementConfiguration(bool __)
    {
      this.SpeedMultiplier = 0.01f;
      this.JumpMultiplier = 0.01f;
      this.GravityMultiplier = 1f;
      this.CanEnterOnVehicle = false;
      this.DefaultStance = EPlayerStance.PRONE;
    }
  }
}
