using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMedical.Models.Configuration.Sub_Configuration
{
    public class ScreamConfiguration
    {
        public List<ushort> ScreamIDs { get; set; }
        public int MinCooldown { get; set; }
        public int ScreamChance { get; set; }
        public float RelevantDistance { get; set; }
        public ScreamConfiguration()
        {
            
        }
        public ScreamConfiguration(List<ushort> screamIDs, int minCooldown, int screamChance, float relevanteDistance)
        {
            ScreamIDs = screamIDs;
            MinCooldown = minCooldown;
            ScreamChance = screamChance;
            RelevantDistance = relevanteDistance;
        }
        public ScreamConfiguration(bool Default)
        {
            ScreamIDs = new List<ushort> { 12300 , 12301, 12302, 12303, 12304, 12305, 12306};
            MinCooldown = 5;
            ScreamChance = 100;
            RelevantDistance = 50f;
        }
    }
}
