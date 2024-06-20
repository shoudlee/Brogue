using System;
using System.Collections;
using System.Collections.Generic;
using Brogue.Bullet;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class BulletPool : MonoBehaviour
{
   public static BulletPool Instance;

   [SerializeField] private NormalBullet normalBulletPrefab;
   private ObjectPool<NormalBullet> normalBulletPool;

   private void Awake()
   {
      if (Instance == null)
      {
         Instance = this;
         DontDestroyOnLoad(gameObject);
      }
      else
      {
         Destroy(gameObject);
         return;
      }

      normalBulletPool = new ObjectPool<NormalBullet>(CreateNormalBullet, OnGetNormalBullet, actionOnRelease:OnReleaseNormalBullet,
         OnDestroyNormalBullet, defaultCapacity:200,maxSize:800);
   }

   private NormalBullet CreateNormalBullet()
   {
      var _bullet = Instantiate(normalBulletPrefab,new Vector3(0,-10,0), Quaternion.identity);
      _bullet.gameObject.SetActive(false);
      return _bullet;
   }

   private void OnGetNormalBullet(NormalBullet bullet)
   {
      bullet.gameObject.SetActive(true);
   }

   private void OnReleaseNormalBullet(NormalBullet bullet)
   {
      bullet.transform.position = new Vector3(0, -10, 0);
      bullet.gameObject.SetActive(false);
   }

   private void OnDestroyNormalBullet(NormalBullet bullet)
   {
      Destroy(bullet.gameObject);
   }

   public NormalBullet GetNormalBullet()
   {
      return normalBulletPool.Get();
   }

   public void ReturnNormalBullet(NormalBullet bullet)
   {
      if (bullet.isActiveAndEnabled)
      {
         normalBulletPool.Release(bullet);
      }
      // normalBulletPool.Release(bullet);
   }
}