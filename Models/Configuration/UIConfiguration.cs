// Decompiled with JetBrains decompiler
// Type: UMedical.Models.Configuration.UIConfiguration
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

using SDG.Unturned;
using System.Collections.Generic;

namespace UMedical.Models.Configuration
{
  public class UIConfiguration
  {
    public List<string> PossibleHints { get; set; }

    public int IntervalBeetwenHints { get; set; }

    public float MaxProximityMedicalUIDIstance { get; set; }

    public List<Traduction> Translates { get; set; }

    public string LifeTimeSuffix { get; set; }

    public UIConfiguration()
    {
    }

    public UIConfiguration(bool __)
    {
      this.PossibleHints = new List<string>() { "HINT 1" };
      this.IntervalBeetwenHints = 5;
      this.MaxProximityMedicalUIDIstance = 100f;
      this.LifeTimeSuffix = "<color=red>ML'S</color>";
      this.Translates = new List<Traduction>();
      this.Translates.Add(new Traduction(EDeathCause.BLEEDING, "Sangria"));
    }
  }
}
