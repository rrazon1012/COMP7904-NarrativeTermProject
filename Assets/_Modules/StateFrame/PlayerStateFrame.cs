using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateFrame : StateFrame {
    // Player Components
    [HideInInspector] public InputBuffer inputBuffer;
    [HideInInspector] public PlayerMotor motor;
    [HideInInspector] public Animator animator;
    [HideInInspector] public VoidController voidController;
    [HideInInspector] public InspectController inspectController;
    [HideInInspector] public InteractionManager interactionManager;
    [HideInInspector] public LockController lockController;

    
    void Awake() {
        inputBuffer = this.GetComponent<InputBuffer>();
        motor = this.GetComponent<PlayerMotor>();
        animator = this.GetComponent<Animator>();
        voidController = this.GetComponent<VoidController>();
        inspectController = this.GetComponent<InspectController>();
        interactionManager = this.GetComponent<InteractionManager>();
        lockController = this.GetComponent<LockController>();
    }
}
