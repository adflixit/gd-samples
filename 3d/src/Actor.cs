using Godot;

namespace Iso
{
    public abstract class Actor : KinematicBody, IInputReceiver
    {
        public Area Hitbox { get; private set; }
        public Spatial Model { get; private set; }
        public InputReceiverContainer Components { get; private set; }

        public BitField Flags { get; protected set; }
        public ActorState State { get; protected set; }
        public ActorMoveType MoveType { get; protected set; }

        private float _friction = 0.9f * 60.0f;
        public float Friction
        {
            get => _friction;
            set => _friction = value * 60.0f;
        }
        public Vector3 Direction { get; protected set; }
        public Vector3 Velocity { get; protected set; }
        public float MaxAccel { get; protected set; } = 1.0f;
        public float MaxAccelMult { get; protected set; } = 1.0f;

        public override void _Ready()
        {
            Hitbox = GetNode<Area>("Hitbox");
            Model = GetNode<Spatial>("Model");
            Components = GetNode<InputReceiverContainer>("Components");
        }

        public override void _PhysicsProcess(float delta)
        {
            ProcessMovement(delta);
        }

        protected virtual void ProcessMovement(float delta)
        {
            Velocity += Direction * MaxAccel * MaxAccelMult;
            Velocity *= Friction * delta;
            Velocity = MoveAndSlide(Velocity, Vector3.Up, true);
        }

        protected virtual void Attack(int id, bool active) { }

        // IControllerInputReceiver

        public virtual void OnConnect(Controller controller)
        {
            Components?.OnConnect(controller);
        }

        public virtual void OnDisconnect(Controller controller)
        {
            Direction = Vector3.Zero;

            Components?.OnDisconnect(controller);
        }

        public virtual void InputVector(int id, Vector3 vec)
        {
            switch (id)
            {
                case (int)ActorInput.Dir:
                    Direction = vec;
                    break;
            }

            Components?.InputVector(id, vec);
        }

        public virtual void InputAction(int id, bool active)
        {
            switch (id)
            {
                case (int)ActorInput.Attack:
                case (int)ActorInput.Attack2:
                case (int)ActorInput.Attack3:
                case (int)ActorInput.Attack4:
                    Attack(id - (int)ActorInput.Attack, active);
                    break;
            }

            Components?.InputAction(id, active);
        }
    }
}
