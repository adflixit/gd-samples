namespace Iso
{
    public enum ActorFlags
    {
        Player,
    }

    public enum ActorState
    {
        None,
        Active,
        Stunned,
        Dead,
    }

    public enum ActorMoveType
    {
        None,
        Step,
        Hover,
    }

    public enum ActorInput
    {
        Dir,
        AimDir,
        AimPos,
        Attack,
        Attack2,
        Attack3,
        Attack4,
    }
}
