using Godot;

namespace Iso
{
    public interface IInputReceiver
    {
        void OnConnect(Controller controller);
        void OnDisconnect(Controller controller);
        void InputVector(int id, Vector3 vec);
        void InputAction(int id, bool active);
    }
}
