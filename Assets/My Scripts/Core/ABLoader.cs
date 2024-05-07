using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Brogue.Zombie;
using UnityEngine;


namespace Brogue.Core
{
    public class ABLoader : MonoBehaviour
    {
        public string zombieMeshesBundleName = "zombiemeshes";
        public string zombieMeshesName = "Zombie Meshes";
        public ZombieMeshes zombieMeshesObject;
        
        // private void Awake()
        // {
        //     if (Instance is null)
        //     {
        //         Instance = this;
        //         DontDestroyOnLoad(gameObject);
        //     }
        //     else
        //     {
        //         Destroy(gameObject);
        //     }
        // }

        private void Awake()
        {
            AssetBundle zombieMeshesBundle =
                AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, zombieMeshesBundleName));
            if (zombieMeshesBundle is null)
            {
                Debug.Log("zombie meshes bundle not found.");
            }

            GameObject _zombieMeshesObject = zombieMeshesBundle.LoadAsset<GameObject>(zombieMeshesName);
            GameObject _z = Instantiate(_zombieMeshesObject);
            _z.transform.parent = transform;
            zombieMeshesObject = _z.GetComponent<ZombieMeshes>();
            zombieMeshesBundle.Unload(false);
        }
    }
}

