// Decompiled with JetBrains decompiler
// Type: UMedical.Models.Hooking.Values.MovementInfo
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

namespace UMedical.Models.Hooking.Values
{
  public sealed class MovementInfo
  {
    public float SpeedMultiplier { get; set; }

    public float GravityMultiplier { get; set; }

    public float JumpMultiplier { get; set; }

    public MovementInfo()
    {
    }

    public MovementInfo(float speedMultiplier, float gravityMultiplier, float jumpMultiplier)
    {
      this.SpeedMultiplier = speedMultiplier;
      this.GravityMultiplier = gravityMultiplier;
      this.JumpMultiplier = jumpMultiplier;
    }
  }
}
