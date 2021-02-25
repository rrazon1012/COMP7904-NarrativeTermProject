// GENERATED AUTOMATICALLY FROM 'Assets/_Modules/InputManagers/PlayerInputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""PlayerControls"",
            ""id"": ""41396791-c358-44d2-b0e4-959dc28258b3"",
            ""actions"": [
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""PassThrough"",
                    ""id"": ""a6c58238-cd29-47c5-9319-d89b94f1d183"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseAxisX"",
                    ""type"": ""Value"",
                    ""id"": ""f69abc32-67b8-4d26-9a21-dff7b6a88881"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseAxisY"",
                    ""type"": ""Value"",
                    ""id"": ""0348c25d-4c79-464f-a99c-62d2942214be"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""d47cef5b-b5f3-4191-811f-563c1b0f5452"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Rotation"",
                    ""type"": ""PassThrough"",
                    ""id"": ""9c91cf5e-3581-4e62-9bb8-bfbde1207edc"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Action1"",
                    ""type"": ""Button"",
                    ""id"": ""fd058c5a-66c5-4efd-961d-6f8f7a1e3734"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Action2"",
                    ""type"": ""Button"",
                    ""id"": ""bd5f3187-7740-47ca-8879-dbfcbac93689"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Action3"",
                    ""type"": ""Button"",
                    ""id"": ""8f94d2c9-9d48-4a88-a87d-782aa7856f55"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Action4"",
                    ""type"": ""Button"",
                    ""id"": ""54b1ac3f-0f48-452f-9e6b-ac9eea92ba9d"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""6d883fa1-b22e-4472-b31f-ef6f11676cf2"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keys"",
                    ""id"": ""bf8149fd-c3da-4de3-8226-94aeed26491b"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""91b0c86a-5acf-461d-9016-da6a8f06a0f1"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""7e005b3d-31a1-42fc-837c-b4257c75645f"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""73d185f2-b4d1-43e5-b3da-8d7f14c22955"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""0e0619d2-a4c3-45d3-b86f-4e0fece38004"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""e546b37a-60a9-4fba-b4f4-17cb5b436f82"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""46820aaa-2055-4211-9f69-7ef71f6d4ddc"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Action2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8ef28007-3751-4884-8854-6773338e76ce"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Action2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e8008c5a-0e72-44f6-8703-6ff70cd4fdcf"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Action1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aba3f661-52d3-421c-8f02-94300c654828"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Action1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""05ce583c-79fa-41f2-bf56-57fe3c2a7bc1"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""28cc76e6-d17e-4383-92f3-b6acc6c5a904"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone(min=0.2)"",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Rotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""613c5256-9fa1-4c97-bc52-5f436b29b4f3"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Action4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e30a1a9a-b35f-44cf-bb76-6748c47bfb1b"",
                    ""path"": ""<Mouse>/forwardButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Action3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""166a35cc-3d2e-441e-9522-0a10bca6a4fe"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Action3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""66f68573-a076-4d6a-a84c-9e5f7af83457"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": ""Clamp(min=0.9,max=1)"",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Action3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""41b9175f-4f24-4175-a72c-b1ac545b098c"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Action4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9766c586-82b1-403c-9490-aa8f61172cf8"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""96024636-5f80-4963-ac5e-1626af381a41"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""02607723-1ab7-4b98-bb3f-93ab61f7261a"",
                    ""path"": ""<Mouse>/delta/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""MouseAxisX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dcd9cd8e-2fa2-4e70-9202-f1bf60941e55"",
                    ""path"": ""<Mouse>/delta/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""MouseAxisY"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard&Mouse"",
            ""bindingGroup"": ""Keyboard&Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // PlayerControls
        m_PlayerControls = asset.FindActionMap("PlayerControls", throwIfNotFound: true);
        m_PlayerControls_MousePosition = m_PlayerControls.FindAction("MousePosition", throwIfNotFound: true);
        m_PlayerControls_MouseAxisX = m_PlayerControls.FindAction("MouseAxisX", throwIfNotFound: true);
        m_PlayerControls_MouseAxisY = m_PlayerControls.FindAction("MouseAxisY", throwIfNotFound: true);
        m_PlayerControls_Movement = m_PlayerControls.FindAction("Movement", throwIfNotFound: true);
        m_PlayerControls_Rotation = m_PlayerControls.FindAction("Rotation", throwIfNotFound: true);
        m_PlayerControls_Action1 = m_PlayerControls.FindAction("Action1", throwIfNotFound: true);
        m_PlayerControls_Action2 = m_PlayerControls.FindAction("Action2", throwIfNotFound: true);
        m_PlayerControls_Action3 = m_PlayerControls.FindAction("Action3", throwIfNotFound: true);
        m_PlayerControls_Action4 = m_PlayerControls.FindAction("Action4", throwIfNotFound: true);
        m_PlayerControls_Interact = m_PlayerControls.FindAction("Interact", throwIfNotFound: true);
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

    // PlayerControls
    private readonly InputActionMap m_PlayerControls;
    private IPlayerControlsActions m_PlayerControlsActionsCallbackInterface;
    private readonly InputAction m_PlayerControls_MousePosition;
    private readonly InputAction m_PlayerControls_MouseAxisX;
    private readonly InputAction m_PlayerControls_MouseAxisY;
    private readonly InputAction m_PlayerControls_Movement;
    private readonly InputAction m_PlayerControls_Rotation;
    private readonly InputAction m_PlayerControls_Action1;
    private readonly InputAction m_PlayerControls_Action2;
    private readonly InputAction m_PlayerControls_Action3;
    private readonly InputAction m_PlayerControls_Action4;
    private readonly InputAction m_PlayerControls_Interact;
    public struct PlayerControlsActions
    {
        private @PlayerInputActions m_Wrapper;
        public PlayerControlsActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @MousePosition => m_Wrapper.m_PlayerControls_MousePosition;
        public InputAction @MouseAxisX => m_Wrapper.m_PlayerControls_MouseAxisX;
        public InputAction @MouseAxisY => m_Wrapper.m_PlayerControls_MouseAxisY;
        public InputAction @Movement => m_Wrapper.m_PlayerControls_Movement;
        public InputAction @Rotation => m_Wrapper.m_PlayerControls_Rotation;
        public InputAction @Action1 => m_Wrapper.m_PlayerControls_Action1;
        public InputAction @Action2 => m_Wrapper.m_PlayerControls_Action2;
        public InputAction @Action3 => m_Wrapper.m_PlayerControls_Action3;
        public InputAction @Action4 => m_Wrapper.m_PlayerControls_Action4;
        public InputAction @Interact => m_Wrapper.m_PlayerControls_Interact;
        public InputActionMap Get() { return m_Wrapper.m_PlayerControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerControlsActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerControlsActions instance)
        {
            if (m_Wrapper.m_PlayerControlsActionsCallbackInterface != null)
            {
                @MousePosition.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMousePosition;
                @MouseAxisX.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMouseAxisX;
                @MouseAxisX.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMouseAxisX;
                @MouseAxisX.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMouseAxisX;
                @MouseAxisY.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMouseAxisY;
                @MouseAxisY.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMouseAxisY;
                @MouseAxisY.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMouseAxisY;
                @Movement.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMovement;
                @Rotation.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnRotation;
                @Rotation.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnRotation;
                @Rotation.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnRotation;
                @Action1.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAction1;
                @Action1.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAction1;
                @Action1.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAction1;
                @Action2.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAction2;
                @Action2.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAction2;
                @Action2.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAction2;
                @Action3.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAction3;
                @Action3.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAction3;
                @Action3.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAction3;
                @Action4.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAction4;
                @Action4.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAction4;
                @Action4.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAction4;
                @Interact.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnInteract;
            }
            m_Wrapper.m_PlayerControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
                @MouseAxisX.started += instance.OnMouseAxisX;
                @MouseAxisX.performed += instance.OnMouseAxisX;
                @MouseAxisX.canceled += instance.OnMouseAxisX;
                @MouseAxisY.started += instance.OnMouseAxisY;
                @MouseAxisY.performed += instance.OnMouseAxisY;
                @MouseAxisY.canceled += instance.OnMouseAxisY;
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Rotation.started += instance.OnRotation;
                @Rotation.performed += instance.OnRotation;
                @Rotation.canceled += instance.OnRotation;
                @Action1.started += instance.OnAction1;
                @Action1.performed += instance.OnAction1;
                @Action1.canceled += instance.OnAction1;
                @Action2.started += instance.OnAction2;
                @Action2.performed += instance.OnAction2;
                @Action2.canceled += instance.OnAction2;
                @Action3.started += instance.OnAction3;
                @Action3.performed += instance.OnAction3;
                @Action3.canceled += instance.OnAction3;
                @Action4.started += instance.OnAction4;
                @Action4.performed += instance.OnAction4;
                @Action4.canceled += instance.OnAction4;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
            }
        }
    }
    public PlayerControlsActions @PlayerControls => new PlayerControlsActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard&Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IPlayerControlsActions
    {
        void OnMousePosition(InputAction.CallbackContext context);
        void OnMouseAxisX(InputAction.CallbackContext context);
        void OnMouseAxisY(InputAction.CallbackContext context);
        void OnMovement(InputAction.CallbackContext context);
        void OnRotation(InputAction.CallbackContext context);
        void OnAction1(InputAction.CallbackContext context);
        void OnAction2(InputAction.CallbackContext context);
        void OnAction3(InputAction.CallbackContext context);
        void OnAction4(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
    }
}