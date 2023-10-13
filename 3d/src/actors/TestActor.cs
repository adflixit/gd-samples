using Godot;

namespace Iso
{
    public class TestActor : Actor
    {
        private AimComponent Aim { get; set; }
        private MeshInstance Marker { get; set; }

        public override void _Ready()
        {
            base._Ready();

            Components.GetNode<AimComponent>("Aim");
            Aim = Components.GetNode<AimComponent>("Aim");
            Marker = GetParent().GetNode<MeshInstance>("Marker");

            MaxAccel = 2.0f;

            Aim.Callback = ProcessRay;
            Aim.RayOffset = 1.0f;
        }

        public void ProcessRay(Object collider, int shape, Vector3 normal, Vector3 point)
        {
            Util.SetOrigin(Marker, point);
        }

        protected override void Attack(int id, bool active)
        {
            switch (id)
            {
                case 0:
                    Aim.Ray.Enabled = active;
                    if (!active)
                    {
                        Util.SetOrigin(Marker, Vector3.Zero);
                    }
                    break;
            }
        }
    }
}
