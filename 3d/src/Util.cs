using Godot;

namespace Iso
{
    public static class Util
    {
        public static void SetOrigin(Spatial node, Vector3 value)
        {
            var transform = node.Transform;
            transform.origin = value;
            node.Transform = transform;
        }
    }
}
