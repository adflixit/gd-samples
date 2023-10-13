using Godot;

namespace Iso
{
    public class InputReceiverContainer : Spatial, IInputReceiver
    {
        public void OnConnect(Controller controller)
        {
            foreach (var child in GetChildren())
            {
                if (child is IInputReceiver receiver)
                {
                    receiver.OnConnect(controller);
                }
            }
        }

        public void OnDisconnect(Controller controller)
        {
            foreach (var child in GetChildren())
            {
                if (child is IInputReceiver receiver)
                {
                    receiver.OnDisconnect(controller);
                }
            }
        }

        public void InputVector(int id, Vector3 vec)
        {
            foreach (var child in GetChildren())
            {
                if (child is IInputReceiver receiver)
                {
                    receiver.InputVector(id, vec);
                }
            }
        }

        public void InputAction(int id, bool active)
        {
            foreach (var child in GetChildren())
            {
                if (child is IInputReceiver receiver)
                {
                    receiver.InputAction(id, active);
                }
            }
        }
    }
}
