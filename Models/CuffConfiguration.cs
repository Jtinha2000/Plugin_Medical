// Decompiled with JetBrains decompiler
// Type: UMedical.Models.CuffConfiguration
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

namespace UMedical.Models
{
  public class CuffConfiguration
  {
    public string RadiusPermission { get; set; }

    public string RaycastPermission { get; set; }

    public string PlayerPermission { get; set; }

    public int MaxRadius { get; set; }

    public int MaxDistance { get; set; }

    public CuffConfiguration()
    {   
    }

    public CuffConfiguration(bool __)
    {
      this.RadiusPermission = "RadiusCuffPermission";
      this.PlayerPermission = "PlayerCuffPermission";
      this.RaycastPermission = "RaycastCuffPermission";
      this.MaxRadius = 5;
      this.MaxDistance = 20;
    }
  }
}
