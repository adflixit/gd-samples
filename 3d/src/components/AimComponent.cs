using Godot;

namespace Iso
{
    public delegate void RayCastCallback(Object collider, int shape, Vector3 normal, Vector3 point);

    public class AimComponent : Spatial, IInputReceiver
    {
        public RayCast Ray { get; private set; }
        public RayCastCallback Callback { get; set; }

        public float RayOffset { get; set; }

        private float _friction = 0.5f * 60.0f;
        public float Friction
        {
            get => _friction;
            set => _friction = value * 60.0f;
        }
        public Vector3 Direction { get; private set; }
        public Vector3 Position { get; private set; }
        public float Angle { get; private set; }
        public float Smooth { get; private set; }

        public override void _Ready()
        {
            Ray = GetNode<RayCast>("Ray");
            Ray.Translation = new Vector3(RayOffset, 0.0f, 0.0f);
        }

        public override void _PhysicsProcess(float delta)
        {
            Angle = -Mathf.Atan2(Direction.z, Direction.x);
            Smooth = Mathf.LerpAngle(Smooth, Angle, Friction * delta);
            Rotation = new Vector3(0.0f, Smooth, 0.0f);

            if (Ray.IsColliding())
            {
                Callback?.Invoke(Ray.GetCollider(), Ray.GetColliderShape(),
                                 Ray.GetCollisionNormal(), Ray.GetCollisionPoint());
            }
        }

        // IControllerInputReceiver

        public void OnConnect(Controller controller) { }

        public void OnDisconnect(Controller controller) { }
        
        public void InputVector(int id, Vector3 vec)
        {
            switch (id)
            {
                case (int)ActorInput.AimDir:
                    Direction = vec;
                    break;

                case (int)ActorInput.AimPos:
                    Position = vec;
                    break;
            }
        }

        public void InputAction(int id, bool active) { }
    }
}
