using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Andar : MonoBehaviour
{

    public Transform[] pontosParada;

    public int posicaoPlayer = 10; //posicao no vetor (casa do tabuleiro) que o player está
    public int qntAndar = 0;       //quantidade de casas que o player vai andar
    public int qntAndou = 0;       //quantidade de casas que o player já andou

    public GameObject pontosParadas;

    public Vector2 posicaoInicial;
    public Vector2 posicaoFinal;

    public float velMovimentacao = 1f;

    private void Start() {

        //só pra não ter que ficar colocando 62 pontos manualmente, sendo que a cada vez que o script é removido tem que
        //colocar tudo de novo
        for (int i = 0; i < 32; i++) {
            pontosParadas = GameObject.Find("BreakPoint"+ (i+1));
            pontosParada[i] = pontosParadas.transform;
        }
    }

    private void Update()
    {
        Move();
    }

    void Move() { 

        if (qntAndou < qntAndar) {

            posicaoInicial = pontosParada[posicaoPlayer].transform.position;
            posicaoFinal = pontosParada[posicaoPlayer - 1].transform.position;

            StartCoroutine(nameof(MoverJogador), 0.5);

            posicaoPlayer--;
            qntAndou++;
        } else {
            qntAndou = 0;
            qntAndar = 0;
        }

    }

    private IEnumerator MoverJogador(int segundos) {
        yield return new WaitForSeconds(segundos);

        Debug.Log("Entrou");
        transform.position = Vector2.MoveTowards(posicaoInicial, posicaoFinal, velMovimentacao * Time.deltaTime);
    }
}
