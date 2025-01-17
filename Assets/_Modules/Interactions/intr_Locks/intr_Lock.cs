﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class intr_Lock : InteractableObject
{
    [SerializeField] protected string password = "1234";
    [SerializeField] protected bool interact_Lock = false;
    [SerializeField] protected bool is_Locked = true;
    public bool IsLocked { get { return is_Locked; } }
    [SerializeField] protected GameObject lock_Canvas;

    [SerializeField] protected Text text_comb1;
    [SerializeField] protected Text text_comb2;
    [SerializeField] protected Text text_comb3;
    [SerializeField] protected Text text_comb4;

    [SerializeField] protected Button inc_comb1;
    [SerializeField] protected Button inc_comb2;
    [SerializeField] protected Button inc_comb3;
    [SerializeField] protected Button inc_comb4;
    
    [SerializeField] protected Button dec_comb1;
    [SerializeField] protected Button dec_comb2;
    [SerializeField] protected Button dec_comb3;
    [SerializeField] protected Button dec_comb4;

    [SerializeField] int comb1 = 0;
    [SerializeField] int comb2 = 0;
    [SerializeField] int comb3 = 0;
    [SerializeField] int comb4 = 0;

    private void Start()
    {
        inc_comb1.onClick.AddListener(delegate { increment_First(); });
        dec_comb1.onClick.AddListener(delegate { decrement_First(); });

        inc_comb2.onClick.AddListener(delegate { increment_Second(); });
        dec_comb2.onClick.AddListener(delegate { decrement_Second(); });

        inc_comb3.onClick.AddListener(delegate { increment_Third(); });
        dec_comb3.onClick.AddListener(delegate { decrement_Third(); });

        if (inc_comb4 != null) {
            inc_comb4.onClick.AddListener(delegate { increment_Fourth(); });
            dec_comb4.onClick.AddListener(delegate { decrement_Fourth(); });
        }
    }

    void FixedUpdate()
    {
        UpdateInteraction();
        text_comb1.text = comb1.ToString();
        text_comb2.text = comb2.ToString();
        text_comb3.text = comb3.ToString();

        if (text_comb4 != null) {
            text_comb4.text = comb4.ToString();
        }
        
        if (is_Locked == false) {
            GameObject.Destroy(this.gameObject);
        }
    }

    public override void OnInteraction(InteractionManager interactor)
    {

        if (active)
        {
            base.OnInteraction(interactor);
            if (!interact_Lock && is_Locked)
            {
                interactor.currentInteraction = this;
            }
        }

    }

    public void increment_First()
    {
        PlayInteractionAudio();
        if (comb1 >= 9)
        {
            comb1 = 0;
        }
        else
        {
            ++comb1;
        }
    }
    public void increment_Second()
    {
        PlayInteractionAudio();
        if (comb2 >= 9)
        {
            comb2 = 0;
        }
        else
        {
            ++comb2;
        }
    }
    public void increment_Third()
    {
        PlayInteractionAudio();
        if (comb3 >= 9)
        {
            comb3 = 0;
        }
        else
        {
            ++comb3;
        }
    }
    public void increment_Fourth()
    {
        PlayInteractionAudio();
        if (comb4 >= 9)
        {
            comb4 = 0;
        }
        else
        {
            
            ++comb4;
        }
    }
    public void decrement_First()
    {
        PlayInteractionAudio();
        if (comb1 <= 0) { comb1 = 9; } else { --comb1; }
    }
    public void decrement_Second()
    {
        PlayInteractionAudio();
        if (comb2 <= 0) { comb2 = 9; } else { --comb2; }
    }
    public void decrement_Third()
    {
        PlayInteractionAudio();
        if (comb3 <= 0)
        {
            comb3 = 9;
        }
        else
        {
            --comb3;
        }
    }
    public void decrement_Fourth()
    {
        PlayInteractionAudio();
        if (comb4 <= 0)
        {
            comb4 = 9;
        }
        else
        {
            --comb4;
        }
    }

    public void showCanvas() { lock_Canvas.gameObject.SetActive(true);}
    public void hideCanvas() { lock_Canvas.gameObject.SetActive(false);}

    public bool checkCombination() { if (password.Equals(comb1 + "" + comb2 + "" + comb3 + "" + comb4))
        {
            Debug.Log(password.Equals(comb1 + "" + comb2 + "" + comb3 + "" + comb4));
            is_Locked = false;
            EventSystem.current.LockOpen();
            if (interactionAudio != null && interactionAudio.Length > 0)
            {
                AudioDirector.Instance.PlayRandomAudioAtPoint(interactionAudio[1], this.transform.position);
            }
            return true; 
        } 
        else 
        { 
            return false; 
        } 
    }

    protected override void PlayInteractionAudio()
    {
        if (interactionAudio != null && interactionAudio.Length > 0)
        {
            AudioDirector.Instance.PlayRandomAudioAtPoint(interactionAudio[0], this.transform.position);
        }
    }
}
