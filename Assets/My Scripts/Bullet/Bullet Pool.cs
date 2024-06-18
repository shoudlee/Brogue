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
         OnDestroyNormalBullet, defaultCapacity:100,maxSize:500);
   }

   private NormalBullet CreateNormalBullet()
   {
      var _bullet = Instantiate(normalBulletPrefab);
      _bullet.gameObject.SetActive(false);
      return _bullet;
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
      if (bullet.isActiveAndEnabled)
      {
         normalBulletPool.Release(bullet);
      }
   }
}