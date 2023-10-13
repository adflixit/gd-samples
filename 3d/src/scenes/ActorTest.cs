using Godot;

namespace Iso
{
	public abstract class ActorTest : Spatial
	{
		private PlayerController _controller;

		public override void _Ready()
		{
			_controller = GetNode<PlayerController>("PlayerController");
			_controller.CameraRig = GetNode<PlayerCamera>("PlayerCamera");
			_controller.Connect(GetNode<TestActor>("TestActor"));
		}
	}
}
