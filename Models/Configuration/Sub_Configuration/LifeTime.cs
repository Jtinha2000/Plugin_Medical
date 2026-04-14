// Decompiled with JetBrains decompiler
// Type: UMedical.Models.Configuration.Sub_Configuration.LifeTime
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

using SDG.Unturned;

namespace UMedical.Models.Configuration.Sub_Configuration
{
  public sealed class LifeTime
  {
    public EDeathCause Cause { get; set; }

    public ushort Time { get; set; }

    public float MultiplierIfNoMedical { get; set; }

    public LifeTime()
    {
    }

    public LifeTime(EDeathCause cause, ushort time, float multiplierIfNoMedical)
    {
      this.Cause = cause;
      this.Time = time;
      this.MultiplierIfNoMedical = multiplierIfNoMedical;
    }
  }
}
