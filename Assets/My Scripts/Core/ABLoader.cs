using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Brogue.Zombie;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Brogue.Core
{
    public class ABLoader : MonoBehaviour
    {
        public string zombieMeshesBundleName = "zombiemeshes";
        public string zombieMeshesName = "Zombie Meshes";
        private ZombieMeshes zombieMeshesObject;
        
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

        public Mesh GetRandomZombieMesh(string zombieType)
        {
            switch (zombieType)
            {
                case "Zombie":
                {
                    return zombieMeshesObject.NromalZombieMeshes[
                        Random.Range(0, zombieMeshesObject.NromalZombieMeshes.Length - 1)];
                }
                case "Jumbie":
                {
                    return zombieMeshesObject.JumperZombieMeshes[
                        Random.Range(0, zombieMeshesObject.JumperZombieMeshes.Length - 1)];
                }
                case "Spitbie":
                {
                    return zombieMeshesObject.SpitterZombieMeshes[
                        Random.Range(0, zombieMeshesObject.SpitterZombieMeshes.Length - 1)];
                }
            }
            
            // default mesh
            return zombieMeshesObject.NromalZombieMeshes[0];
        }
    }
}

