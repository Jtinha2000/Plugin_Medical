// Decompiled with JetBrains decompiler
// Type: UMedical.Models.Configuration.Sub_Configuration.MultiplierConfiguration
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

using SDG.Unturned;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace UMedical.Models.Configuration.Sub_Configuration
{
    public sealed class MultiplierConfiguration
    {
        [XmlIgnore]
        public Dictionary<ELimb, float> GetMultiplier { get; set; }

        public float DamageHandsMultiplier { get; set; }

        public float DamageChestMultiplier { get; set; }

        public float DamageLegsMultiplier { get; set; }

        public float DamageHeadMultiplier { get; set; }

        public MultiplierConfiguration()
        {
        }

        public MultiplierConfiguration(bool __)
        {
            this.DamageHandsMultiplier = 1.1f;
            this.DamageChestMultiplier = 1f;
            this.DamageLegsMultiplier = 1.1f;
            this.DamageHeadMultiplier = 2f;
            GetMultiplierCreate();
        }
        public void GetMultiplierCreate()
        {
            this.GetMultiplier = new Dictionary<ELimb, float>()
      {
        {
          ELimb.LEFT_ARM,
          this.DamageHandsMultiplier
        },
        {
          ELimb.RIGHT_ARM,
          this.DamageHandsMultiplier
        },
        {
          ELimb.LEFT_HAND,
          this.DamageHandsMultiplier
        },
        {
          ELimb.RIGHT_HAND,
          this.DamageHandsMultiplier
        },
        {
          ELimb.LEFT_FOOT,
          this.DamageLegsMultiplier
        },
        {
          ELimb.RIGHT_FOOT,
          this.DamageLegsMultiplier
        },
        {
          ELimb.LEFT_LEG,
          this.DamageLegsMultiplier
        },
        {
          ELimb.RIGHT_LEG,
          this.DamageLegsMultiplier
        },
        {
          ELimb.RIGHT_FRONT,
          this.DamageChestMultiplier
        },
        {
          ELimb.LEFT_FRONT,
          this.DamageChestMultiplier
        },
        {
          ELimb.RIGHT_BACK,
          this.DamageChestMultiplier
        },
        {
          ELimb.LEFT_BACK,
          this.DamageChestMultiplier
        },
        {
          ELimb.SKULL,
          this.DamageHeadMultiplier
        },
        {
          ELimb.SPINE,
          this.DamageHeadMultiplier
        }
      };
        }
    }
}
