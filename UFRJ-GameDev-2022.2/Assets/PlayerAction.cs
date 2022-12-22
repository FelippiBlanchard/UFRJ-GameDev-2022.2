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
    [SerializeField] private UnityEvent findPage1, findPage2, findPage3, findPage4, findPage5, findPage6;
    

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
        //Find capitain diary pages
        if (obj.tag == "Diary Page 1") {
            //play chest animation (open)
            findPage1.Invoke();
            Debug.Log("page1");
        }
        else if (obj.tag == "Diary Page 2") {
            //play chest animation (open)
            findPage2.Invoke();
        }
        else if (obj.tag == "Diary Page 3") {
            //play chest animation (open)
            findPage3.Invoke();
        }
        else if (obj.tag == "Diary Page 4") {
            //play chest animation (open)
            findPage4.Invoke();
        }
        else if (obj.tag == "Diary Page 5") {
            //play chest animation (open)
            findPage5.Invoke();
        }
        else if (obj.tag == "Diary Page 6") {
            //play chest animation (open)
            findPage6.Invoke();
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