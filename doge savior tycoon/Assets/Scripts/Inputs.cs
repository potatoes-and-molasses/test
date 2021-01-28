// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Inputs.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Inputs : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Inputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Inputs"",
    ""maps"": [
        {
            ""name"": ""Keys and Mouse"",
            ""id"": ""6a8819b8-5956-4e46-b25b-b2daf6c21f0a"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""b332ef46-f800-42d2-9206-ed925062beb0"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""arrows"",
                    ""id"": ""63d81b83-dad5-403b-9abd-0337257b772b"",
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
                    ""id"": ""a00604bc-01c5-434e-ac0c-fc829dba736a"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""bb112993-a432-42cc-b01f-6a7218ed0d17"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""e79160a0-6c9b-4de8-bc41-578b99d7d5fb"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""b844388c-f534-4672-8cdc-08f94ced61e7"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""wasd"",
                    ""id"": ""b0eda3a8-0f52-4c33-92c3-678d53ca2a0b"",
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
                    ""id"": ""7ea663f3-8184-4a08-8382-34cf018b9ef0"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""9558d4e7-cd15-4e2f-81ab-ebcf61eda643"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b765d998-72e0-4aef-8f24-26e7645a72ac"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""ab9e73f5-f975-448e-aed9-1aefc6da3966"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Keys and Mouse
        m_KeysandMouse = asset.FindActionMap("Keys and Mouse", throwIfNotFound: true);
        m_KeysandMouse_Movement = m_KeysandMouse.FindAction("Movement", throwIfNotFound: true);
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

    // Keys and Mouse
    private readonly InputActionMap m_KeysandMouse;
    private IKeysandMouseActions m_KeysandMouseActionsCallbackInterface;
    private readonly InputAction m_KeysandMouse_Movement;
    public struct KeysandMouseActions
    {
        private @Inputs m_Wrapper;
        public KeysandMouseActions(@Inputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_KeysandMouse_Movement;
        public InputActionMap Get() { return m_Wrapper.m_KeysandMouse; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(KeysandMouseActions set) { return set.Get(); }
        public void SetCallbacks(IKeysandMouseActions instance)
        {
            if (m_Wrapper.m_KeysandMouseActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_KeysandMouseActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_KeysandMouseActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_KeysandMouseActionsCallbackInterface.OnMovement;
            }
            m_Wrapper.m_KeysandMouseActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
            }
        }
    }
    public KeysandMouseActions @KeysandMouse => new KeysandMouseActions(this);
    public interface IKeysandMouseActions
    {
        void OnMovement(InputAction.CallbackContext context);
    }
}
