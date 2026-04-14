using Rocket.Unturned.Chat;
using SDG.Unturned;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UMedical.Models
{
    public class KeyListener
    {
        public delegate void OnSpeedupRequested_Handler();

        public Player Owner { get; set; }
        public bool LastState { get; set; }
        public Coroutine Repeater { get; set; }
        public event OnSpeedupRequested_Handler SpeedUpRequested;
        public KeyListener(Player owner)
        {
            Owner = owner;
            LastState = Owner.input.keys[7];
            Repeater = Main.Instance.StartCoroutine(DelayedUpdate());
        }
        public void Kill()
        {
            Main.Instance.StopCoroutine(Repeater);
        }
        public IEnumerator DelayedUpdate()
        {
            while (true)
            {
                yield return new WaitForSeconds(Main.Instance.Configuration.Instance.DownedConfig.SpeedUpKeyInterval);

                if (LastState != Owner.input.keys[7])
                {
                    if (LastState == false && SpeedUpRequested != null)
                        SpeedUpRequested.Invoke();
                    LastState = Owner.input.keys[7];
                }
            }
        }
    }
}
