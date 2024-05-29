using System;
using System.Collections;
using System.Collections.Generic;
using Brogue.Bullet;
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
         OnDestroyNormalBullet, defaultCapacity:50,maxSize:100);
   }

   private NormalBullet CreateNormalBullet()
   {
      return Instantiate(normalBulletPrefab);
   }

   private void OnGetNormalBullet(NormalBullet bullet)
   {
      bullet.gameObject.SetActive(true);
   }

   private void OnReleaseNormalBullet(NormalBullet bullet)
   {
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
      normalBulletPool.Release(bullet);
   }
}