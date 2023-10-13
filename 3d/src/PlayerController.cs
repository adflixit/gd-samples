using Godot;

namespace Iso
{
    public class PlayerController : Controller
    {
        public const float DirDeadzone = 0.2f;

        private string[] _dirActionList = { "move_left", "move_right", "move_forward", "move_back" };
        private string[] _aimActionList = { "aim_left", "aim_right", "aim_forward", "aim_back" };

        public PlayerCamera CameraRig { get; set; }
        public bool MouseAim { get; private set; }

        protected override void OnConnect()
        {
            CameraRig.Target = Receiver as Spatial;
        }

        private Basis CamBasis() => CameraRig.Camera.GlobalTransform.basis;

        private Vector3 CamBasisVecForAxis(string action)
        {
            switch (action)
            {
                case "move_left":
                case "aim_left":
                    return -CamBasis().x;
                case "move_right":
                case "aim_right":
                    return CamBasis().x;
                case "move_forward":
                case "aim_forward":
                    return -CamBasis().z;
                case "move_back":
                case "aim_back":
                    return CamBasis().z;
            }

            return Vector3.Zero;
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event.IsEcho())
            {
                return;
            }

            if (@event is InputEventMouseMotion)
            {
                if (!MouseAim)
                {
                    MouseAim = true;
                }

                var pos = GetViewport().GetMousePosition();
                var from = CameraRig.Camera.ProjectRayOrigin(pos);
                var to = from + CameraRig.Camera.ProjectRayNormal(pos) * 1000.0f;
                var aimPos = GameDefs.ViewPlane.IntersectRay(from, to) / 1.03104085938f ?? Vector3.Zero;
                var aimDir = aimPos - CameraRig.Target.Transform.origin;

                InputVector((int)ActorInput.AimPos, aimPos);
                InputVector((int)ActorInput.AimDir, aimDir.Normalized());
            }
            else if (@event is InputEventKey || @event is InputEventMouseButton ||
                     @event is InputEventJoypadButton || @event is InputEventJoypadMotion)
            {
                if (MouseAim)
                {
                    foreach (string action in _aimActionList)
                    {
                        if (@event.GetActionStrength(action) > 0.0f)
                        {
                            MouseAim = false;
                        }
                    }
                }

                // Direction

                foreach (string action in _dirActionList)
                {
                    if (@event.IsAction(action))
                    {
                        var dir = Vector3.Zero;

                        foreach (string actionNested in _dirActionList)
                        {
                            var vec = CamBasisVecForAxis(actionNested);
                            var strength = Input.GetActionStrength(actionNested);
                            dir += vec * (strength < DirDeadzone ? 0.0f : strength);
                        }

                        InputVector((int)ActorInput.Dir, dir.Normalized());
                        break;
                    }
                }

                // Aim direction

                foreach (string action in _aimActionList)
                {
                    if (@event.IsAction(action))
                    {
                        var dir = Vector3.Zero;
                        var sum = 0.0f;

                        foreach (string actionNested in _aimActionList)
                        {
                            var vec = CamBasisVecForAxis(actionNested);
                            var strength = Input.GetActionStrength(actionNested);
                            sum += strength;
                            dir += vec * strength;
                        }

                        if (sum > 0.0f)
                        {
                            InputVector((int)ActorInput.AimDir, dir.Normalized());
                        }
                        break;
                    }
                }

                // Attack

                if (@event.IsAction("attack"))
                {
                    InputAction((int)ActorInput.Attack, @event.IsPressed());
                }

                if (@event.IsAction("attack2"))
                {
                    InputAction((int)ActorInput.Attack2, @event.IsPressed());
                }
            }
        }
    }
}
