using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

//Shortcut command C-k,C-d

namespace Project.Networking {
    //Mark the namespace "SocketIOComponent" and press F12 to jump to the definition
    //and modify some properties
    public class NetworkClient : SocketIOComponent {

        public override void Start() {
            base.Start();
            SetUpEvents();
        }

        public override void Update() {
            base.Update();
        }


        private void SetUpEvents() {
            On("open", (e) => {
                Debug.Log("Connection Made with Server");
            });
        }

    }
}

