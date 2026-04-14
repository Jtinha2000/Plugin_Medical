// Decompiled with JetBrains decompiler
// Type: UMedical.Models.Configuration.Sub_Configuration.CommunicationConfiguration
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

using UnityEngine;

namespace UMedical.Models.Configuration.Sub_Configuration
{
  public sealed class CommunicationConfiguration
  {
    public bool PermitSendChat { get; set; }

    public bool ChangeSendedChatsColor { get; set; }

    public Color Color { get; set; }

    public bool PermitTalk { get; set; }

    public bool PermitHear { get; set; }

    public CommunicationConfiguration()
    {
    }

    public CommunicationConfiguration(bool __)
    {
      this.PermitHear = true;
      this.PermitSendChat = false;
      this.ChangeSendedChatsColor = true;
      this.Color = Color.red;
      this.PermitTalk = false;
    }
  }
}
