using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;
using Project.Utility;
using Project.Player;

//Shortcut command C-k,C-d

namespace Project.Networking {
    //Mark the namespace "SocketIOComponent" and press F12 to jump to the definition
    //and modify some properties
    public class NetworkClient : SocketIOComponent {

        [Header("Network Client")]
        [SerializeField]
        private Transform networkContainer;
        [SerializeField]
        private GameObject playerPrefab;

        public static string ClientID { get; private set; }

        private Dictionary<string, NetworkIdentity> serverObjects;

        public override void Start() {
            base.Start();
            Initialize();
            SetUpEvents();
        }

        public override void Update() {
            base.Update();
        }

        private void Initialize() {
            serverObjects = new Dictionary<string, NetworkIdentity>();
        }

        private void SetUpEvents() {
            //open prints two times, but this is bug with the socket.io asset
            //the connection does not get duplicated so it does not matter much.
            On("open", (e) => {
                Debug.Log("Connection Made with Server");
            });

            On("register", (e) => {
                ClientID = e.data["id"].ToString().RemoveQuotes();//.RemoveQuotes();

                Debug.LogFormat("Our Client's ID is ({0})", ClientID);
            });

            On("spawn", (e) => {
                string id = e.data["id"].ToString().RemoveQuotes();

                GameObject go = Instantiate(playerPrefab, networkContainer);
                go.name = string.Format("Player ({0})", id);
                //Network Identity: NI : ni
                NetworkIdentity ni = go.GetComponent<NetworkIdentity>();
                ni.SetControllerID(id);
                ni.SetSocketReference(this);
                serverObjects.Add(id, ni);
            });

            On("disconnected", (e) => {
                string id = e.data["id"].ToString().RemoveQuotes();

                GameObject go = serverObjects[id].gameObject;
                Destroy(go);
                serverObjects.Remove(id);
            });

            On("updatePosition", (e) => {
                string id = e.data["id"].ToString().RemoveQuotes();
                float x = e.data["position"]["x"].f;
                float y = e.data["position"]["y"].f;

                NetworkIdentity ni = serverObjects[id];
                ni.transform.position = new Vector3(x, y, 0);
            });

            On("updateRotation", (e) => {
                string id = e.data["id"].ToString().RemoveQuotes();

                float tankRotation = e.data["tankRotation"].f;
                float barrelRotation = e.data["barrelRotation"].f;

                NetworkIdentity ni = serverObjects[id];
                ni.transform.localEulerAngles = new Vector3(0, 0, tankRotation);
                ni.GetComponent<PlayerManager>().SetRotation(barrelRotation);
            });

        }

    }

    [Serializable]
    public class Player {
        public string id;
        public Position position;
    }

    [Serializable]
    public class Position {
        public float x;
        public float y;
    }
    
    [Serializable]
    public class PlayerRotation {
        public float tankRotation;
        public float barrelRotation;
    }
}

