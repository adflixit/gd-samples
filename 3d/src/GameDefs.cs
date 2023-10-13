using Godot;

namespace Iso
{
    public static class GameDefs
    {
        public const float ViewHeight = 1.0f;
        public static readonly Plane ViewPlane;

        static GameDefs()
        {
            ViewPlane = new Plane(Vector3.Up, ViewHeight);
        }
    }

    public enum DamageType
    {
        Generic,
        Physics,
        Ray,
        Blast,
        Dissolution,
    }

    public struct DamageInfo
    {
        public readonly Node Inflictor;
        public readonly Node Attacker;
        public readonly Vector3 Force;
        public readonly Vector3 Position;
        public readonly Vector3 Direction;
        public readonly int Damage;
        public readonly DamageType DamageType;

        public DamageInfo(Node inflictor, Actor attacker, Vector3 force, Vector3 position, Vector3 direction, int damage, DamageType damageType)
        {
            Inflictor = inflictor;
            Attacker = attacker;
            Force = force;
            Position = position;
            Direction = direction;
            Damage = damage;
            DamageType = damageType;
        }
    }
}
