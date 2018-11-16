using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinNumber : MonoBehaviour {

    public GameObject gameObjectGC;

    public int numero;
    private int cont;

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Number1")) {
            numero = 1;
        } else if (collision.CompareTag("Number2")) {
            numero = 2;
        } else if (collision.CompareTag("Number3")) {
            numero = 3;
        } else if (collision.CompareTag("Number4")) {
            numero = 4;
        } else if (collision.CompareTag("Number5")) {
            numero = 5;
        } else if (collision.CompareTag("Number6")) {
            numero = 6;
        }

        if (cont++ >= 1)
        {
            AudioManager.instance.Play("click");
            cont=1;
        }

    }

    private void OnDisable() {
        if (gameObjectGC != null)
        {
            gameObjectGC.GetComponent<GameControl>().numSorteado = numero;
            gameObjectGC.GetComponent<GameControl>().podeJogar = true;
        }
        cont = 0;
    }
}
