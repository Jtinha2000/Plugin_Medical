// Decompiled with JetBrains decompiler
// Type: UMedical.Models.Configuration.MiscConfiguration
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

using System.Collections.Generic;
using UMedical.Models.Configuration.Sub_Configuration;

namespace UMedical.Models.Configuration
{
  public sealed class MiscConfiguration
  {
    public string AvatarURL { get; set; }

    public bool CensureReviveWindow { get; set; }

    public bool DownedBlackWindow { get; set; }

        public bool DownedBlurWindow { get; set; }

        public bool ParadoxUIMode { get; set; }

    public bool DebugMode { get; set; }

    public string MedicalPermission { get; set; }

    public string AnalyzePermission { get; set; }

    public List<ushort> IgnoreDamageWeapons { get; set; }

    public MultiplierConfiguration DamageMutliplier { get; set; }
    public ScreamConfiguration ScreamConfig { get; set; }

        public MiscConfiguration()
    {
    }

    public MiscConfiguration(bool __)
    {
            this.DownedBlurWindow = false;
      this.ParadoxUIMode = true;
      this.DownedBlackWindow = false;
      this.IgnoreDamageWeapons = new List<ushort>()
      {
        (ushort) 1198
      };
      this.AvatarURL = "";
      this.CensureReviveWindow = true;
      this.DebugMode = true;
      this.AnalyzePermission = "analisador";
      this.MedicalPermission = "medico";
      this.DamageMutliplier = new MultiplierConfiguration(true);
            this.ScreamConfig = new ScreamConfiguration(true);
    }
  }
}
