using Godot;

namespace Iso
{
    public class PlayerCamera : Spatial
    {
        public const float Friction = 0.25f * 60.0f;

        public Camera Camera { get; private set; }
        public Spatial Target { get; set; }

        public override void _Ready()
        {
            Camera = GetNode<Camera>("Camera");
        }

        public override void _PhysicsProcess(float delta)
        {
            var transform = Transform;
            transform.origin = transform.origin.LinearInterpolate(Target.Transform.origin, Friction * delta);
            Transform = transform;
        }

        public void SnapToTarget()
        {
            Transform = Target.Transform;
        }
    }
}
