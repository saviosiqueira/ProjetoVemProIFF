using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartaTela : MonoBehaviour
{
    public void Destruir()
    {
        GameControl gc = FindObjectOfType<GameControl>();
        gc.DestruirCartaAtual();
    }
}
