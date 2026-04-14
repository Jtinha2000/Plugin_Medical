// Decompiled with JetBrains decompiler
// Type: UMedical.Models.Samu.Values.RequestInfo
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

namespace UMedical.Models.Samu.Values
{
  public class RequestInfo
  {
    public string Name { get; set; }

    public string Local { get; set; }

    public string Cause { get; set; }

    public bool Sended { get; set; }

    public RequestInfo()
    {
    }

    public RequestInfo(string name, string local, string cause, bool sended)
    {
      this.Name = name;
      this.Local = local;
      this.Cause = cause;
      this.Sended = sended;
    }
  }
}
