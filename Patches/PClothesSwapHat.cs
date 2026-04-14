// Decompiled with JetBrains decompiler
// Type: UMedical.Patches.PClothesSwapHat
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

using HarmonyLib;
using SDG.Unturned;
using System;
using System.Linq;
using UMedical.Models;

namespace UMedical.Patches
{
  [HarmonyPatch(typeof (PlayerClothing), "ReceiveSwapHatRequest")]
  public static class PClothesSwapHat
  {
    public static bool Prefix(PlayerClothing __instance) => !Main.Instance.DownedPlayers.Any<DownedPlayer>((Func<DownedPlayer, bool>) (X => (long) X.CSteamID == (long) __instance.player.channel.owner.playerID.steamID.m_SteamID));
  }
}
