using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAction : MonoBehaviour
{
//Variables
    //Animations
    private CharacterController controller;
    private Animator anim;
    private Rigidbody rb;
    private CapsuleCollider cc;

    [SerializeField] private UnityEvent evento;

    [SerializeField] private UnityEvent fimDeJogo;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider obj) {
        //Find treasure chest
        if (obj.tag == "Legendary Treasure") {
            //play chest animation (open)
            evento.Invoke();
        }
    }

    private void OnTriggerExit(Collider obj) {
        //Find treasure chest
        if (obj.tag == "Legendary Treasure") {
            //play chest animation (open)
            evento.Invoke();
        }
    }

    private void OnTriggerStay(Collider obj){
        if (obj.tag == "Legendary Treasure") {
            //if player presses "E" game ends
            if (Input.GetKey("e")) {
                Debug.Log("fim de jogo");
                fimDeJogo.Invoke();
            }
        }
    }

}