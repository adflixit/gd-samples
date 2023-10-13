using Godot;

namespace Iso
{
    public abstract class Controller : Node
    {
        protected IInputReceiver Receiver { get; private set; }

        public void Connect(IInputReceiver receiver)
        {
            Receiver = receiver;
            OnConnect();
            Receiver.OnConnect(this);
        }

        public void Disconnect()
        {
            OnDisconnect();
            Receiver.OnDisconnect(this);
            Receiver = null;
        }

        protected virtual void OnConnect() { }
        protected virtual void OnDisconnect() { }

        protected void InputVector(int id, Vector3 vec) => Receiver.InputVector(id, vec);
        protected void InputAction(int id, bool active) => Receiver.InputAction(id, active);
    }
}
