// Decompiled with JetBrains decompiler
// Type: UMedical.Models.Configuration.Traduction
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

using SDG.Unturned;

namespace UMedical.Models.Configuration
{
  public struct Traduction
  {
    public EDeathCause Cause { get; set; }

    public string Translate { get; set; }

    public Traduction(EDeathCause cause, string translate)
    {
      this.Cause = cause;
      this.Translate = translate;
    }
  }
}
