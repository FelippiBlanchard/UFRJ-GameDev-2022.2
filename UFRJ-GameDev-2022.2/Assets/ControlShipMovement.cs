using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlShipMovement : MonoBehaviour
{
    //Variáveis
    private List<float> shipSpeeds = new List<float> { 0, 0.01f, 0.04f, 0.08f };
    private int numSpeed = 0;
    private List<float> shipRotateSpeeds = new List<float> { 0.1f, 0.07f, 0.04f, 0.02f };

    private CharacterController controller;
    public bool controlar = false;
    public bool iniciar = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (controlar) {
            Debug.Log("Aperte E");

            if (Input.GetKey("e")) {
                Debug.Log("começou a controlar");
                GameObject.Find("Player").gameObject.GetComponent<PlayerMovement>().canMove = false;
                iniciar = true;
                controlar = false;

                //Fixar player e câmera no navio -> torna player filho do navio
                Camera.main.transform.parent = transform;
                GameObject.FindGameObjectWithTag("Player").transform.parent = transform;

                //Mudar posição da câmera -> a visão normal também é bem legal
                //último parâmetro é o tempo que a transação demora -> ver isso depois
                //Camera.main.transform.position = Vector3.Lerp(transform.position, new Vector3(5, 20, -25), 0.2f * Time.deltaTime); //-> câmera não segue
                Camera.main.transform.position = transform.position + new Vector3(5, 20, -35);
                Camera.main.transform.rotation = Quaternion.Euler(15, 0, 0);

            }
        
        } else if (iniciar) {

            //Determina a velocidade do navio - velocidades 1, 2 e 3
            if (Input.GetKeyDown("w") && numSpeed < 3) {
                ChangeSpeed(true);
            }
            if (Input.GetKeyDown("s") && numSpeed > 0) {
                ChangeSpeed(false);
            }
            Debug.Log(numSpeed);

            //Faz o navio andar (ou parar, se numSpeed = 0)
            Sailing(shipSpeeds[numSpeed]);

            //Rotação do navio        
            //Esquerda
            if (Input.GetKey("a")) {
                transform.Rotate(0, -shipRotateSpeeds[numSpeed], 0);
            }
            //Direita
            if (Input.GetKey("d")) {
                transform.Rotate(0, shipRotateSpeeds[numSpeed], 0);
            }

            // //Parar de controlar o navio
            // if (Input.GetKeyDown("e")) {
            //     iniciar = false;
            //     controlar = true;

            //     //Tira câmera e player como filhos do navio
            //     Camera.main.transform.parent = null;
            //     GameObject.FindGameObjectWithTag("Player").transform.parent = null;

            //     //Volta posição da câmera à original
            //     Camera.main.transform.position = transform.position - new Vector3(5, 20, -35);
            //     Camera.main.transform.rotation = Quaternion.Euler(-15, 0, 0);
            // }

        }
    }

    private void Sailing(float shipSpeed){
        transform.position += transform.forward * shipSpeed;
    }

    private void ChangeSpeed(bool aumentar){
        if (aumentar) {
            numSpeed += 1;
        } else {
            numSpeed -= 1;
        }
    }
}

