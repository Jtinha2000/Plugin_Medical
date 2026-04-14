// Decompiled with JetBrains decompiler
// Type: UMedical.Models.Configuration.Sub_Configuration.ItemConfiguration
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

using System.Collections.Generic;

namespace UMedical.Models.Configuration.Sub_Configuration
{
  public sealed class ItemConfiguration
  {
    public bool BlockCrafting { get; set; }

    public bool BlockDropItems { get; set; }

    public bool BlockEquipItems { get; set; }

    public bool BlockPickItems { get; set; }

    public List<ushort> IgnoreItems { get; set; }

    public ItemConfiguration()
    {
    }

    public ItemConfiguration(bool __)
    {
      this.BlockCrafting = true;
      this.BlockDropItems = true;
      this.BlockEquipItems = true;
      this.BlockPickItems = true;
      this.IgnoreItems = new List<ushort>() { (ushort) 0 };
    }
  }
}
