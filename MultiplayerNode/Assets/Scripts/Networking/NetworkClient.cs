using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

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

        private Dictionary<string, GameObject> serverObjects;

        public override void Start() {
            base.Start();
            Initialize();
            SetUpEvents();
        }

        public override void Update() {
            base.Update();
        }

        private void Initialize() {
            serverObjects = new Dictionary<string, GameObject>();
        }

        private void SetUpEvents() {
            //open prints two times, but this is bug with the socket.io asset
            //the connection does not get duplicated so it does not matter much.
            On("open", (e) => {
                Debug.Log("Connection Made with Server");
            });

            On("register", (e) => {
                ClientID = e.data["id"].ToString();//.RemoveQuotes();

                Debug.LogFormat("Our Client's ID is ({0})", ClientID);
            });

            On("spawn", (e) => {
                string id = e.data["id"].ToString();

                GameObject go = new GameObject("Server ID: " + id);
                go.transform.SetParent(networkContainer);
                serverObjects.Add(id, go);
            });

            On("disconnected", (e) => {
                string id = e.data["id"].ToString();

                GameObject go = serverObjects[id];
                Destroy(go);
                serverObjects.Remove(id);
            });

        }

    }
}

