using UnityEngine;

namespace Brogue.Zombie
{
    public interface IZombieHitable
    {
        public void GetHit(int damage);
        public Transform GetPosition();
    }
}