using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Project.Utility.Attributes;

namespace Project.Networking {
    public class NetworkIdentity : MonoBehaviour {

        [Header("Helpful Values")]
        [SerializeField]
        [GreyOut]
        private string id;
        [SerializeField]
        [GreyOut]
        private bool isControlling;

        // Use this for initialization
        void Start () {
		
	    }
	
	    // Update is called once per frame
	    void Update () {
		
	    }
    }
}

