using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Utility
{
    public class Cooldown
    {
        private float length;
        private float currentTime;
        private bool onCooldown;

        public Cooldown(float Length = 1, bool StartWithCoolDown = false)
        {
            currentTime = 0;
            length = Length;
            onCooldown = StartWithCoolDown;
        }

        public void CoolDownUpdate()
        {
            if (onCooldown)
            {
                currentTime += Time.deltaTime;

                if(currentTime >= length)
                {
                    currentTime = 0;
                    onCooldown = false;
                }
            }

        }


        public bool IsOnCoolDown()
        {
            return onCooldown;
        }

        public void StartCoolDown()
        {
            onCooldown = true;
            currentTime = 0;
        }

    }
}

