using Project.Player;
using Project.Utility;
using Project.Utility.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Networking {
    [RequireComponent(typeof(NetworkIdentity))]
    public class NetworkRotation : MonoBehaviour {

        //C-. to get the intellisense to get the namespace
        [Header("Referenced Values")]
        [SerializeField]
        [GreyOut]
        private float oldTankRotation;

        [SerializeField]
        [GreyOut]
        private float oldBarrelRotation;

        [Header("Class References")]
        [SerializeField]
        private PlayerManager playerManager;

        private NetworkIdentity networkIdentity;
        private PlayerRotation player;

        private float stillCounter = 0;

        // Use this for initialization
        public void Start() {
            networkIdentity = GetComponent<NetworkIdentity>();

            player = new PlayerRotation();
            player.tankRotation = 0;
            player.barrelRotation = 0;

            if (!networkIdentity.IsControlling()) {
                enabled = false;
            }
        }

        // Update is called once per frame
        public void Update() {
            if (networkIdentity.IsControlling()) {
                if (oldTankRotation == transform.localEulerAngles.z && oldBarrelRotation == playerManager.GetLastRotation()) {
                    stillCounter += Time.deltaTime;

                    if (stillCounter >= 1) {
                        stillCounter = 0;
                        SendData();
                    }
                } else {
                    oldTankRotation = transform.localEulerAngles.z;
                    oldBarrelRotation = playerManager.GetLastRotation();

                    stillCounter = 0;

                    SendData();
                }
            }
        }

        private void SendData() {
            //Update Player Information
            player.tankRotation = transform.localEulerAngles.z.TwoDecimals();
            player.barrelRotation = playerManager.GetLastRotation().TwoDecimals();

            networkIdentity.GetSocket().Emit("updateRotation", new JSONObject(JsonUtility.ToJson(player)));

        }
    }
}
