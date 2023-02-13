using UnityEngine;
using UnityEngine.InputSystem;

namespace WarriorAnims
{
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/index.html")]

	public class WarriorInputSystemController : MonoBehaviour
    {
		// WarriorInputActions generated file.
		public @WarriorInputs warriorInputs;

		// Inputs.
		[HideInInspector] public bool inputAiming;
		[HideInInspector] public float inputAimVertical = 0;
		[HideInInspector] public float inputAimHorizontal = 0;
		[HideInInspector] public bool inputAttack;
		[HideInInspector] public bool inputAttackMove;
		[HideInInspector] public bool inputAttackRanged;
		[HideInInspector] public bool inputAttackSpecial;
		[HideInInspector] public bool inputBlock;
		[HideInInspector] public bool inputDeath;
		[HideInInspector] public bool inputJump;
		[HideInInspector] public bool inputLightHit;
		[HideInInspector] public bool inputRoll;
		[HideInInspector] public bool inputTarget;
		[HideInInspector] public bool inputTargetKey;
		[HideInInspector] public float inputHorizontal = 0;
		[HideInInspector] public float inputVertical = 0;

		public Vector3 moveInput { get { return CameraRelativeInput(inputHorizontal, inputVertical); } }
		public Vector2 aimInput { get { return CameraRelativeInput(inputAimHorizontal, inputAimVertical); } }

		private float inputPauseTimeout = 0;
		private bool inputPaused = false;

		private void Awake()
        {
			warriorInputs = new @WarriorInputs();
        }

		private void OnEnable()
		{
			warriorInputs.Enable();
		}

		private void OnDisable()
		{
			warriorInputs.Disable();
		}

		private void Update()
		{
			Inputs();
			Toggles();
		}

		/// <summary>
		/// Input abstraction for easier asset updates using outside control schemes.
		/// </summary>
		private void Inputs()
        {
            try {
				inputAttack = warriorInputs.Warrior.Attack.WasPressedThisFrame();
				inputJump = warriorInputs.Warrior.Jump.WasPressedThisFrame();
				inputHorizontal = warriorInputs.Warrior.Move.ReadValue<Vector2>().x;
				inputVertical = warriorInputs.Warrior.Move.ReadValue<Vector2>().y;
            }
			catch (System.Exception)
			{
				Debug.LogError("Inputs not found!  Character must have Player Input component.");
			}
        }

		private void Toggles()
		{
			// Slow time toggle.
			if (Keyboard.current.tKey.wasPressedThisFrame) {
				if (Time.timeScale != 1) { Time.timeScale = 1; }
				else { Time.timeScale = 0.125f; }
			}
			// Pause toggle.
			if (Keyboard.current.pKey.wasPressedThisFrame) {
				if (Time.timeScale != 1) { Time.timeScale = 1; }
				else { Time.timeScale = 0f; }
			}
		}

		/// <summary>
		/// Movement based off camera facing.
		/// </summary>
		private Vector3 CameraRelativeInput(float inputX, float inputZ)
		{
			// Forward vector relative to the camera along the x-z plane.
			Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
			forward.y = 0;
			forward = forward.normalized;

			// Right vector relative to the camera always orthogonal to the forward vector.
			Vector3 right = new Vector3(forward.z, 0, -forward.x);
			Vector3 relativeVelocity = inputHorizontal * right + inputVertical * forward;

			// Reduce input for diagonal movement.
			if (relativeVelocity.magnitude > 1) { relativeVelocity.Normalize(); }

			return relativeVelocity;
		}
	}

	/// <summary>
	/// Extension Method to allow checking InputSystem without Action Callbacks.
	/// </summary>
	public static class InputActionExtensions
	{
		public static bool IsPressed(this InputAction inputAction)
		{
			return inputAction.ReadValue<float>() > 0f;
		}

		public static bool WasPressedThisFrame(this InputAction inputAction)
		{
			return inputAction.triggered && inputAction.ReadValue<float>() > 0f;
		}

		public static bool WasReleasedThisFrame(this InputAction inputAction)
		{
			return inputAction.triggered && inputAction.ReadValue<float>() == 0f;
		}
	}
}