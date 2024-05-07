using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

namespace Brogue.Zombie
{
    public class ZombieMeshes : MonoBehaviour
    {
        static ZombieMeshes Instance;

        public Mesh[] NromalZombieMeshes;
        public Mesh[] JumperZombieMeshes;
        public Mesh[] SpitterZombieMeshes;
    
        private void Awake()
        {
            if (Instance is null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }


    }
}

