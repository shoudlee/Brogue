using UnityEngine;

namespace Brogue.Core
{
    public interface IBattleProperties
    {
        public void GetHit(int damage);
        public void Dead();
        public int Defense();
        public Transform Transform();
    }
}