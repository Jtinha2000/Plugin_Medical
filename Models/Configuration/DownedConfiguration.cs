// Decompiled with JetBrains decompiler
// Type: UMedical.Models.DownedConfiguration
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

using SDG.Unturned;
using System;
using System.Collections.Generic;
using UMedical.Models.Configuration;
using UMedical.Models.Configuration.Sub_Configuration;

namespace UMedical.Models
{
    public sealed class DownedConfiguration
    {
        public ushort MinimalLife { get; set; }

        public UMedical.Models.Configuration.Sub_Configuration.LifeTime[] LifeTime { get; set; }

        public int ImmunityAfterDeadSeconds { get; set; }

        public bool CanBeFinalized { get; set; }

        public bool CanSeedpUpDeath { get; set; }

        public float SpeedUpKeyInterval { get; set; }

        public int SpeedUpReduction { get; set; }

        public bool DropWeaponOnDowned { get; set; }

        public bool DropItemsOnDowned { get; set; }

        public List<ushort> DirectDeathWeapons { get; set; }
        public bool HeadshotDirectDeath { get; set; }

        public VehicleConfiguration VehicleConfig { get; set; }

        public ExitConfiguration ExitConfiguration { get; set; }

        public MultiplierConfiguration DamageMutliplierAfterDead { get; set; }

        public EffectsConfiguration EffectsConfiguration { get; set; }

        public DownedConfiguration()
        {
        }

        public DownedConfiguration(bool __)
        {
            CanSeedpUpDeath = true;
            SpeedUpKeyInterval = 0.01f;
            SpeedUpReduction = 5;
            this.VehicleConfig = new VehicleConfiguration(false, new List<EEngine>()
      {
        EEngine.PLANE,
        EEngine.HELICOPTER
      }, 0.4f, 50, EDeathCause.SHRED);
            this.HeadshotDirectDeath = true;
            this.ExitConfiguration = new ExitConfiguration(true);
            this.MinimalLife = (ushort)5;
            this.ImmunityAfterDeadSeconds = 5;
            this.DropItemsOnDowned = true;
            this.DropWeaponOnDowned = true;
            this.DamageMutliplierAfterDead = new MultiplierConfiguration(true);
            this.CanBeFinalized = false;
            this.DirectDeathWeapons = new List<ushort>()
      {
        (ushort) 363
      };
            this.EffectsConfiguration = new EffectsConfiguration(true);
            this.LifeTime = new UMedical.Models.Configuration.Sub_Configuration.LifeTime[30];
            for (int cause = 0; cause < 29; ++cause)
                this.LifeTime[cause] = new UMedical.Models.Configuration.Sub_Configuration.LifeTime((EDeathCause)cause, (ushort)new Random().Next(34, 96), 0.5f);
        }
    }
}
