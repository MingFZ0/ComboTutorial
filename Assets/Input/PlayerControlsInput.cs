//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Input/PlayerControlsInput.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerControlsInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControlsInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControlsInput"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""727b61e9-fcb8-4145-b2ba-e3465a92369b"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""9f997630-3114-4fd1-990f-5a70a1ac8fff"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""LightAttack"",
                    ""type"": ""Button"",
                    ""id"": ""5b498b52-b1bf-4414-aa7e-a541e6b1e824"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MediumAttack"",
                    ""type"": ""Button"",
                    ""id"": ""ebef02ab-71fa-4617-8702-ad819b27d858"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""HeavyAttack"",
                    ""type"": ""Button"",
                    ""id"": ""ad334743-8113-40a8-8490-e587c8178794"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SpecialAttack"",
                    ""type"": ""Button"",
                    ""id"": ""a0d819a5-b4c3-40e6-915b-18da03dfd218"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Up"",
                    ""type"": ""Button"",
                    ""id"": ""ac849017-350b-40ec-92f0-465a72daf305"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Down"",
                    ""type"": ""Button"",
                    ""id"": ""101558be-95a9-4122-81a0-1315c8ca78cc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Left"",
                    ""type"": ""Button"",
                    ""id"": ""e5da2c16-4cd9-44d8-b56f-17f02f97f202"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Right"",
                    ""type"": ""Button"",
                    ""id"": ""b90e7803-4aca-4a14-a015-5b175e0ea3cc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f6989ca4-7368-4e6a-80d4-f9decf4cd9e7"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LightAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dd708a83-49d9-4b70-b1d4-1f6482283525"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HeavyAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7828c979-eadb-462d-b68f-020bae0919d0"",
                    ""path"": ""<Keyboard>/u"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SpecialAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""583e94b4-f6ce-42ad-95ea-78c88e5342b6"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a717ffe4-2181-462d-bd98-04d8ee61318f"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4d1d0802-3ed4-408e-84d0-b9cfcf520fe9"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4aeca87a-0448-4141-aa8e-f4628f26699d"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""81e1dc90-53cd-4fa2-ace0-97ced57c3c90"",
                    ""path"": ""<Keyboard>/k"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MediumAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD Keys"",
                    ""id"": ""84f303c5-fb04-4c85-aa65-80884f2be884"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""2c62a8b2-6e87-4f43-9e06-5bf360a2aefb"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""bf11f607-cfeb-4e29-a36f-1aaa6dd18fc7"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""42bd0a26-c16f-45da-8453-4c4422693c61"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""485be9e1-cab4-4a71-9cc1-3b72275e3136"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_LightAttack = m_Player.FindAction("LightAttack", throwIfNotFound: true);
        m_Player_MediumAttack = m_Player.FindAction("MediumAttack", throwIfNotFound: true);
        m_Player_HeavyAttack = m_Player.FindAction("HeavyAttack", throwIfNotFound: true);
        m_Player_SpecialAttack = m_Player.FindAction("SpecialAttack", throwIfNotFound: true);
        m_Player_Up = m_Player.FindAction("Up", throwIfNotFound: true);
        m_Player_Down = m_Player.FindAction("Down", throwIfNotFound: true);
        m_Player_Left = m_Player.FindAction("Left", throwIfNotFound: true);
        m_Player_Right = m_Player.FindAction("Right", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_LightAttack;
    private readonly InputAction m_Player_MediumAttack;
    private readonly InputAction m_Player_HeavyAttack;
    private readonly InputAction m_Player_SpecialAttack;
    private readonly InputAction m_Player_Up;
    private readonly InputAction m_Player_Down;
    private readonly InputAction m_Player_Left;
    private readonly InputAction m_Player_Right;
    public struct PlayerActions
    {
        private @PlayerControlsInput m_Wrapper;
        public PlayerActions(@PlayerControlsInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @LightAttack => m_Wrapper.m_Player_LightAttack;
        public InputAction @MediumAttack => m_Wrapper.m_Player_MediumAttack;
        public InputAction @HeavyAttack => m_Wrapper.m_Player_HeavyAttack;
        public InputAction @SpecialAttack => m_Wrapper.m_Player_SpecialAttack;
        public InputAction @Up => m_Wrapper.m_Player_Up;
        public InputAction @Down => m_Wrapper.m_Player_Down;
        public InputAction @Left => m_Wrapper.m_Player_Left;
        public InputAction @Right => m_Wrapper.m_Player_Right;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @LightAttack.started += instance.OnLightAttack;
            @LightAttack.performed += instance.OnLightAttack;
            @LightAttack.canceled += instance.OnLightAttack;
            @MediumAttack.started += instance.OnMediumAttack;
            @MediumAttack.performed += instance.OnMediumAttack;
            @MediumAttack.canceled += instance.OnMediumAttack;
            @HeavyAttack.started += instance.OnHeavyAttack;
            @HeavyAttack.performed += instance.OnHeavyAttack;
            @HeavyAttack.canceled += instance.OnHeavyAttack;
            @SpecialAttack.started += instance.OnSpecialAttack;
            @SpecialAttack.performed += instance.OnSpecialAttack;
            @SpecialAttack.canceled += instance.OnSpecialAttack;
            @Up.started += instance.OnUp;
            @Up.performed += instance.OnUp;
            @Up.canceled += instance.OnUp;
            @Down.started += instance.OnDown;
            @Down.performed += instance.OnDown;
            @Down.canceled += instance.OnDown;
            @Left.started += instance.OnLeft;
            @Left.performed += instance.OnLeft;
            @Left.canceled += instance.OnLeft;
            @Right.started += instance.OnRight;
            @Right.performed += instance.OnRight;
            @Right.canceled += instance.OnRight;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @LightAttack.started -= instance.OnLightAttack;
            @LightAttack.performed -= instance.OnLightAttack;
            @LightAttack.canceled -= instance.OnLightAttack;
            @MediumAttack.started -= instance.OnMediumAttack;
            @MediumAttack.performed -= instance.OnMediumAttack;
            @MediumAttack.canceled -= instance.OnMediumAttack;
            @HeavyAttack.started -= instance.OnHeavyAttack;
            @HeavyAttack.performed -= instance.OnHeavyAttack;
            @HeavyAttack.canceled -= instance.OnHeavyAttack;
            @SpecialAttack.started -= instance.OnSpecialAttack;
            @SpecialAttack.performed -= instance.OnSpecialAttack;
            @SpecialAttack.canceled -= instance.OnSpecialAttack;
            @Up.started -= instance.OnUp;
            @Up.performed -= instance.OnUp;
            @Up.canceled -= instance.OnUp;
            @Down.started -= instance.OnDown;
            @Down.performed -= instance.OnDown;
            @Down.canceled -= instance.OnDown;
            @Left.started -= instance.OnLeft;
            @Left.performed -= instance.OnLeft;
            @Left.canceled -= instance.OnLeft;
            @Right.started -= instance.OnRight;
            @Right.performed -= instance.OnRight;
            @Right.canceled -= instance.OnRight;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnLightAttack(InputAction.CallbackContext context);
        void OnMediumAttack(InputAction.CallbackContext context);
        void OnHeavyAttack(InputAction.CallbackContext context);
        void OnSpecialAttack(InputAction.CallbackContext context);
        void OnUp(InputAction.CallbackContext context);
        void OnDown(InputAction.CallbackContext context);
        void OnLeft(InputAction.CallbackContext context);
        void OnRight(InputAction.CallbackContext context);
    }
}
