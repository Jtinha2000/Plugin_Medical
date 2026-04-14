// Decompiled with JetBrains decompiler
// Type: UMedical.Models.Configuration.ExitConfiguration
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

namespace UMedical.Models.Configuration
{
  public sealed class ExitConfiguration
  {
    public bool DropInventoryOnExit { get; set; }

    public bool KillOnReconnect { get; set; }

    public int LifeTimeReduction { get; set; }

    public ExitConfiguration()
    {
    }

    public ExitConfiguration(bool __)
    {
      this.DropInventoryOnExit = true;
      this.KillOnReconnect = false;
      this.LifeTimeReduction = 0;
    }
  }
}
