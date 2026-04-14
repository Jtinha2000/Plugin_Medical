// Decompiled with JetBrains decompiler
// Type: UMedical.Patches.POnServerDisconnected
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

using HarmonyLib;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;

namespace UMedical.Patches
{
  [HarmonyPatch(typeof (SaveManager), "onServerDisconnected")]
  public static class POnServerDisconnected
  {
    public static void Prefix(CSteamID steamID) => Main.Instance.Events_OnPlayerDisconnected(UnturnedPlayer.FromCSteamID(steamID));
  }
}
