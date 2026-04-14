using Rocket.API;
using SDG.Unturned;
using System.Collections.Generic;
using UMedical.Models;
using UMedical.Models.Configuration;
using UMedical.Models.Configuration.Sub_Configuration;

namespace UMedical
{
  public sealed class Configuration : IRocketPluginConfiguration, IDefaultable
  {
    public CuffConfiguration CuffConfig { get; set; }

    public List<ReviveConfiguration> ReviveConfig { get; set; }

    public UIConfiguration UiConfig { get; set; }

    public DownedConfiguration DownedConfig { get; set; }

    public HookingConfiguration HookConfig { get; set; }

    public MiscConfiguration MiscConfig { get; set; }

    public void LoadDefaults()
    {
      this.CuffConfig = new CuffConfiguration(true);
      this.ReviveConfig = new List<ReviveConfiguration>()
      {
        new ReviveConfiguration(new AfterEffectsConfiguration(0.7f, 0.7f, (byte) 20, 200), true, new List<ELimb>()
        {
          ELimb.LEFT_HAND
        }, true, new List<EDeathCause>()
        {
          EDeathCause.SUICIDE
        }, (ushort) 387, "AdrenalinaReviver", 5, 26, (byte) 0, 5, (ushort) 5, (ushort) 109, (byte) 25, (byte) 80, (byte) 20, (byte) 80),
        new ReviveConfiguration(new AfterEffectsConfiguration(0.7f, 0.7f, (byte) 20, 200), false, new List<ELimb>()
        {
          ELimb.RIGHT_LEG
        }, false, new List<EDeathCause>()
        {   
          EDeathCause.GUN,
          EDeathCause.MELEE
        }, (ushort) 15, "KitMedicoReviver", 0, 0, (byte) 30, 60, (ushort) 5, (ushort) 109, (byte) 8, (byte) 15, (byte) 15, (byte) 25)
      };
      this.UiConfig = new UIConfiguration(true);
      this.MiscConfig = new MiscConfiguration(true);
      this.HookConfig = new HookingConfiguration(true);
      this.DownedConfig = new DownedConfiguration(true);
    }
  }
}
