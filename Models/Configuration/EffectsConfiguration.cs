// Decompiled with JetBrains decompiler
// Type: UMedical.Models.Configuration.EffectsConfiguration
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

using UMedical.Models.Configuration.Sub_Configuration;

namespace UMedical.Models.Configuration
{
  public sealed class EffectsConfiguration
  {
    public MovementConfiguration MovementConfiguration { get; set; }

    public ItemConfiguration ItemConfiguration { get; set; }

    public CommunicationConfiguration CommunicationConfiguration { get; set; }

    public EffectsConfiguration()
    {
    }

    public EffectsConfiguration(bool __)
    {
      this.MovementConfiguration = new MovementConfiguration(true);
      this.ItemConfiguration = new ItemConfiguration(true);
      this.CommunicationConfiguration = new CommunicationConfiguration(true);
    }
  }
}
