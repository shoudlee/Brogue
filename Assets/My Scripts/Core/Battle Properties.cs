namespace Brogue.Core
{
    public interface BattleProperties
    {
        public void GetHit(int damage);
        public void Dead();
        public int Defense();
    }
}