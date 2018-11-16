using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BotaoEscolherSala : MonoBehaviour {

    public Sala sala {get; set;}

    public BotaoEscolherSala() {

    }

    public BotaoEscolherSala(Sala sala) {
        this.sala = sala;
    }

    public void SelecionarSalaEscolhida () {
        GameControl.salaSelecionada = sala.id_sala;
        Debug.Log("valor de id da sala enviado:" + sala.id_sala);
    }
}
