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
    [SerializeField] private UnityEvent onFindPage;

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
            evento.Invoke();
        }
        //Find capitain diary pages
        if (obj.tag == "Diary Page 1") {
            findPage1.Invoke();
            onFindPage.Invoke();
            Debug.Log("page1");
        }
        else if (obj.tag == "Diary Page 2") {
            findPage2.Invoke();
            onFindPage.Invoke();
        }
        else if (obj.tag == "Diary Page 3") {
            findPage3.Invoke();
            onFindPage.Invoke();
        }
        else if (obj.tag == "Diary Page 4") {
            findPage4.Invoke();
            onFindPage.Invoke();
        }
        else if (obj.tag == "Diary Page 5") {
            findPage5.Invoke();
            onFindPage.Invoke();
        }
        else if (obj.tag == "Diary Page 6") {
            findPage6.Invoke();
            onFindPage.Invoke();
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