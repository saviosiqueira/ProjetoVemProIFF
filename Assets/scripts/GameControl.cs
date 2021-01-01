using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameControl : MonoBehaviour {

	private List<Pergunta> perguntas = new List<Pergunta>();

	private Pergunta pergunta = new Pergunta();

	private Conexao conexao;

	public GameObject player1;
	public GameObject player2;
	public GameObject player3;
	public GameObject player4;
	public GameObject player5;

	public GameObject botaoCartasPos;
	public GameObject botaoCartasNeg;

    //tela de sorteio
    public GameObject telaSorteio;
    public GameObject botaoRoleta;
    public GameObject txtVezJogador;

    public List<GameObject> imgJogTelaSorteio;
    public List<GameObject> iconeJogadores;
    public List<Sprite> jogador_img_on;
    public List<Sprite> jogador_img_off;

    public List<GameObject> cartasPosGO;
	public List<GameObject> cartasNegGO;

	public Transform t;

	private List<Carta> cartasPos = new List<Carta>();
	private List<Carta> cartasNeg = new List<Carta>();

    //private static List<Equipe> equipesOrdenadas = new List<Equipe>();
    public static List<Equipe> equipes = new List<Equipe>();
	[HideInInspector]
	private Equipe equipeAtual;
	public GameObject equipeAtualGO;

    //ovini
	public GameObject oviniGO;
	public SpriteRenderer oviniLaser1;
	public SpriteRenderer oviniLaser2;

	public static int numeroEquipes = 5;
	public static long salaSelecionada;

    public int quemJoga = 1; //quem joga é o jogador do numero nesse caso player 1

    public int numSorteado = 0;
    public bool podeJogar = false;
    public bool efeitoBonus;

    //Não tá sendo usado pra nada(?)
    private bool acertou;

    private bool iniciarContagem = false;
    private float tempoRestante;

    // Interface
    [Header("Pergunta e respostas")]
    public Text txtPergunta;
    public Text txtAlternativa1;
    public Text txtAlternativa2;
    public Text txtAlternativa3;
    public Text txtAlternativa4;
    public Image imgAlternativa1;
    public Image imgAlternativa2;
    public Image imgAlternativa3;
    public Image imgAlternativa4;
    [Header("Objetos da tela")]
    public GameObject telaAlternativaCorreta;
    public GameObject telaAlternativaIncorreta;
    public GameObject telaTempoLimiteAtingido;
    public Image telaPergunta;
    public Text txtVezDoJogador;
    [Header("Fundos")]
    public GameObject fundoEscuroTela;
    public GameObject fundoEscuroPergunta;
    [Header("Objeto da roleta")]
    public GameObject telaRoleta;
    [Header("Tempo para pergunta")]
    public Text txtTempo;
    [Header("Sprites das Telas de Pergunta")]
    public Sprite bgPerguntaPadrao;
    public Sprite bgPerguntaCoringa;
    public Sprite bgPerguntaBomba;
    [Header("Tela de Vitória")]
    public GameObject telaVitoria;
    public Text txtEquipeVencedora;
    public List<Sprite> totens;
    public Image totem1lugar;
    public Image totem2lugar;
    public Image totem3lugar;
    public GameObject trofeu3lugar;
    [Header("Pergunta Expandida")]
	public GameObject fundoPerguntaExpandida;
	public Image telaPerguntaExpandida;
	public Text txtPerguntaExpandida;
	public Text txtTempoPerguntaExpandida;
	[Header("Sprites das Telas de Pergunta Expandida")]
    public Sprite bgPerguntaExpandidaPadrao;
    public Sprite bgPerguntaExpandidaCoringa;
    public Sprite bgPerguntaExpandidaBomba;
    [Header("Tela de loading")]
    public GameObject telaLoading;

    private GameObject cartaAtual;
    private bool cartaAtualPositiva;

   void Start ()
    {

        SortearOrdem();

        AudioManager.instance.Stop("mus_menu");
        AudioManager.instance.Play("mus_jogo");

        StartCoroutine(TelaSorteio(5));

        //Cartas positivas
        //char tipo, int modQntCasas, int modQntTempo, bool usarAgora
        cartasPos.Add(new Carta(1, 'P', 0, 10, false, false, "na proxima pergunta sua equipe tera +10s"));//na proxima pergunta sua equipe tera +10s
        cartasPos.Add(new Carta(2, 'P', 0, 20, false, false, "na proxima pergunta sua equipe tera +20s"));//na proxima pergunta sua equipe tera +20s
        cartasPos.Add(new Carta(3, 'P', -1, 0, false, false, "se a sua equipe errar a próxima pergunta, tera que voltar apenas 1 casa")); //se a sua equipe errar a próxima pergunta, tera que voltar apenas 1 casa
        cartasPos.Add(new Carta(4, 'P', 2, 0, false, true, "Avance 2 casas"));//Avance 2 casas
        cartasPos.Add(new Carta(5, 'P', 3, 0, false, true, "Avance 3 casas"));//Avance 3 casas


        //Cartas Negativas
        //char tipo, int modQntCasas, int modQntTempo, bool usarAgora
        cartasNeg.Add(new Carta(1, 'N', 0, -5, false, false, "na proxima pergunta sua equipe tera -5s"));//na proxima pergunta sua equipe tera -5s
        cartasNeg.Add(new Carta(2, 'N', 0, -10, false, false, "na proxima pergunta sua equipe tera -10s"));//na proxima pergunta sua equipe tera -10s
        cartasNeg.Add(new Carta(3, 'N', -1, 0, false, true, "volte para casa que estava no inicio do seu turno +1"));//volte para casa que estava no inicio do seu turno +1
        cartasNeg.Add(new Carta(4, 'N', -2, 0, false, true, "volte para casa que estava no inicio do seu turno +2"));//volte para casa que estava no inicio do seu turno +2
        cartasNeg.Add(new Carta(5, 'N', 2, 0, true, true, "volte o dobro da quantidade de casas que andou"));//volte o dobro da quantidade de casas que andou


        //para testar o método de listar as perguntas de uma determinada sala
        //passar como parametro o id da sala no método conexao.ListarPerguntasPeloIdSala(1);
        //conforme exemplo abaixo


        /*
        List<Pergunta> perguntas_teste;
        conexao = new Conexao();

        perguntas_teste = conexao.ListarPerguntasPeloIdSala(salaSelecionada);

        Debug.Log("Pergunta: " + perguntas_teste[0].descricao);
        Debug.Log("Resposta 1: " + perguntas_teste[0].alternativas[0].descricao);
        Debug.Log("Resposta 2: " + perguntas_teste[0].alternativas[1].descricao);
        Debug.Log("Resposta 3: " + perguntas_teste[0].alternativas[2].descricao);
        Debug.Log("Resposta 4: " + perguntas_teste[0].alternativas[3].descricao);
        
        */

        //Para testar sem ir ao menu
        /*Quebrou meus teste seus cusão
        Debug.Log(equipes.Count);
        if (equipes.Count == 0)
        {
            for (int i = 1; i <= 3; i++)
            {
                Equipe j = new Equipe(i, "Equipe " + i, "jogador" + Random.Range(1, 6));
                equipes.Add(j);
            }
            numeroEquipes = 3;
        }
        */
        DesabilitaInativos();

        podeJogar = false;
        efeitoBonus = false;

        equipeAtual = equipes[quemJoga-1];
        equipeAtualGO = GameObject.Find(equipeAtual.nomeGO);

        txtVezDoJogador.text = "Vez da Equipe " + equipeAtual.nome + "!";
    }

    private void DesabilitaInativos()
    {
    	foreach (MouseGrab mg in FindObjectsOfType<MouseGrab>())
    	{
    		foreach (Equipe j in equipes)
    		{
    			if (j.nomeGO.Equals(mg.gameObject.name)) {
    				mg.gameObject.SetActive(true);
    				break;
    			}
    			mg.gameObject.SetActive(false);
    		}
    	}
    }

    void Update () {

    	if ((podeJogar) && (numSorteado > 0)) {
    		switch (equipeAtual.nomeGO)
    		{
    			case "jogador1":
    			    player1.GetComponent<MouseGrab>().enabled = true;
    			    player1.GetComponentInChildren<CapsuleCollider2D>().enabled = true;
    			    break;
    			case "jogador2":
    			    player2.GetComponent<MouseGrab>().enabled = true;
    			    player2.GetComponentInChildren<CapsuleCollider2D>().enabled = true;
    			    break;
    			case "jogador3":
    			    player3.GetComponent<MouseGrab>().enabled = true;
    			    player3.GetComponentInChildren<CapsuleCollider2D>().enabled = true;
    			    break;
    			case "jogador4":
    			    player4.GetComponent<MouseGrab>().enabled = true;
    			    player4.GetComponentInChildren<CapsuleCollider2D>().enabled = true;
    			    break;
    			case "jogador5":
    			    player5.GetComponent<MouseGrab>().enabled = true;
    			    player5.GetComponentInChildren<CapsuleCollider2D>().enabled = true;
    			    break;
    			default:
    			    break;
    		}

            //pontoParadaPlayer1 += numSorteado;
    		if (equipeAtual.pontuacao + numSorteado > 18){
    			equipeAtual.pontuacao = 18;
    		}else{
    			equipeAtual.pontuacao += numSorteado;
    		}


    		Debug.Log(equipeAtual.nome + " anda até a casa: " + equipeAtual.pontuacao);

    		Mover(equipeAtual.pontuacao);

    		podeJogar = false;
    	}

    	if (iniciarContagem)
	    {
	        tempoRestante -= Time.deltaTime;
	        if (tempoRestante > 0)
	        {
	            txtTempo.text = tempoRestante.ToString("#");
	            txtTempoPerguntaExpandida.text = tempoRestante.ToString("#");

	            if (!AudioManager.instance.EstaTocando("snd_clock") && !AudioManager.instance.EstaTocando("snd_clock_10s"))
	            {
	                AudioManager.instance.Play("snd_clock");
	            }
	            if (!AudioManager.instance.EstaTocando("snd_clock_10s") && tempoRestante <= 10.5f)
	            {
	                AudioManager.instance.Stop("snd_clock");
	                AudioManager.instance.Play("snd_clock_10s");
	            }
	        }
	        else
	        {
	            txtTempo.text = "0";
	            txtTempoPerguntaExpandida.text = "0";

	            TempoLimiteAtingido();
	            AudioManager.instance.Stop("snd_clock");
                AudioManager.instance.Stop("snd_clock_10s");
	        }
	    }
    }

    public void HabilitarIcones() {
        for (int i = 0; i < equipes.Count; i++) {
            iconeJogadores[i].SetActive(true);
        }
    }

    public void DesabilitarIcones() {
        for (int i = 0; i < equipes.Count; i++) {
            iconeJogadores[i].SetActive(false);
        }
    }

    public void ProximoAJogar() {

        switch (equipes[quemJoga - 1].nomeGO) {
            case "jogador1":
                iconeJogadores[quemJoga - 1].GetComponent<Image>().sprite = jogador_img_off[0];
                break;
            case "jogador2":
                iconeJogadores[quemJoga - 1].GetComponent<Image>().sprite = jogador_img_off[1];
                break;
            case "jogador3":
                iconeJogadores[quemJoga - 1].GetComponent<Image>().sprite = jogador_img_off[2];
                break;
            case "jogador4":
                iconeJogadores[quemJoga - 1].GetComponent<Image>().sprite = jogador_img_off[3];
                break;
            case "jogador5":
                iconeJogadores[quemJoga - 1].GetComponent<Image>().sprite = jogador_img_off[4];
                break;
            default:
                break;
        }

        // Verifica se o jogador ganhou:
        if (equipeAtual.pontuacao >= 18) {
    		ExibirVencedores();
    		return;
    	}

    	if (quemJoga < numeroEquipes) {
    		quemJoga++;
    	} else {
    		quemJoga = 1;
    	}

    	equipeAtual = equipes[quemJoga - 1];

        switch (equipes[quemJoga-1].nomeGO) {
            case "jogador1":
                iconeJogadores[quemJoga-1].GetComponent<Image>().sprite = jogador_img_on[0];
                break;
            case "jogador2":
                iconeJogadores[quemJoga-1].GetComponent<Image>().sprite = jogador_img_on[1];
                break;
            case "jogador3":
                iconeJogadores[quemJoga-1].GetComponent<Image>().sprite = jogador_img_on[2];
                break;
            case "jogador4":
                iconeJogadores[quemJoga-1].GetComponent<Image>().sprite = jogador_img_on[3];
                break;
            case "jogador5":
                iconeJogadores[quemJoga-1].GetComponent<Image>().sprite = jogador_img_on[4];
                break;
            default:
                break;
        }

        equipeAtualGO = GameObject.Find(equipeAtual.nomeGO);
    	txtVezDoJogador.text = "Vez de " + equipeAtual.nome + "!";
    	GameObject.Find("BotaoExibirRoleta").GetComponent<Button>().interactable = true;
    	efeitoBonus = false;
    }

    // É chamado quando o jogador pressiona o botão de pergunta
    public void GerarPergunta(){

        // Resetar as cores das alternativas
        imgAlternativa1.color = Color.white;
        imgAlternativa2.color = Color.white;
        imgAlternativa3.color = Color.white;
        imgAlternativa4.color = Color.white;

        if (equipeAtual.pontuacao == 6 || equipeAtual.pontuacao == 8 || equipeAtual.pontuacao == 14){
    		telaPergunta.sprite = bgPerguntaCoringa;
			telaPerguntaExpandida.sprite = bgPerguntaExpandidaCoringa;
    		txtTempo.color = new Color(1f, .56f, 0f);
			txtTempoPerguntaExpandida.color = new Color(1f, .56f, 0f);
    	}else if (equipeAtual.pontuacao == 4 || equipeAtual.pontuacao == 10 || equipeAtual.pontuacao == 16) {
    		telaPergunta.sprite = bgPerguntaBomba;
			telaPerguntaExpandida.sprite = bgPerguntaExpandidaBomba;
    		txtTempo.color = new Color(.92f, .28f, .30f);
			txtTempoPerguntaExpandida.color = new Color(.92f, .28f, .30f);
    	} else {
    		telaPergunta.sprite = bgPerguntaPadrao;
			telaPerguntaExpandida.sprite = bgPerguntaExpandidaPadrao;
    		txtTempo.color = new Color(.42f, .70f, .19f);
			txtTempoPerguntaExpandida.color = new Color(.42f, .70f, .19f);
    	}

        DesabilitarIcones();

    	fundoEscuroTela.SetActive(true);
        StartCoroutine(FadeInOutMusicaPergunta(true));
        //AudioManager.instance.Play("clock");

        // Procura uma pergunta aleatória e coloca na tela
    	if (perguntas.Count > 0) {

    		if (equipeAtual.pontuacao == 4 || equipeAtual.pontuacao == 10 || equipeAtual.pontuacao == 16) {


    			List<Pergunta> perguntasBomba = new List<Pergunta>();

    			foreach (Pergunta p in perguntas){

    				if (p.dificuldade == "DIFICIL"){
    					perguntasBomba.Add(p);
    				}
    			}

    			if(perguntasBomba.Count>0)
    			{
    				pergunta = perguntasBomba[Random.Range(0, perguntasBomba.Count)];
    			}
    			else
    			{
    				conexao = new Conexao();
    				if (conexao != null) perguntas = conexao.ListarPerguntasPeloIdSala(salaSelecionada);
    				GerarPergunta();
    			}

    		} else {
    			pergunta = perguntas[Random.Range(0, perguntas.Count)];
    		}

            // Se o jogador possuir uma carta de alterar tempo
    		if (equipeAtual.carta != null && equipeAtual.carta.modQntTempo != 0 && equipeAtual.carta.usarAgora == false){
    			tempoRestante = pergunta.tempo + equipeAtual.carta.modQntTempo;
    			equipeAtual.carta = null;
    		}
    		else
    		{
    			tempoRestante = pergunta.tempo;
    		}


			// Se a pergunta for muito grande mostrar somente uma parte dela
			if (pergunta.descricao.Length > 350) {
				txtPergunta.text = pergunta.descricao.Substring(0, 350) + "... [Clique para ler o restante]";
			}
			else {
				txtPergunta.text = pergunta.descricao;
			}
    		
    		txtAlternativa1.text = " A - " + pergunta.alternativas[0].descricao;
    		txtAlternativa2.text = " B - " + pergunta.alternativas[1].descricao;
    		txtAlternativa3.text = " C - " + pergunta.alternativas[2].descricao;
    		txtAlternativa4.text = " D - " + pergunta.alternativas[3].descricao;

    		IniciarContagem();
    	}
        else // Se não houver perguntas na lista, faz uma nova conexão e busca mais perguntas
        {
        	conexao = new Conexao();
        	if (conexao != null) perguntas = conexao.ListarPerguntasPeloIdSala(salaSelecionada);
        	GerarPergunta();
        }
    }

    public void ValidarResposta(int alternativaSelecionada)
    {
    	if(pergunta.alternativas[alternativaSelecionada].correto) {

    		fundoEscuroPergunta.SetActive(true);
    		telaAlternativaCorreta.SetActive(true);
    		PararContagem();
	        AudioManager.instance.Play("snd_right");
            acertou = true;

            if ((equipeAtual.pontuacao == 6 || equipeAtual.pontuacao == 8 || equipeAtual.pontuacao == 14) && (acertou)) {
            //para testar sem ter que ficar acertando a casa correta do tabuleiro descomente a linha de baixo e comente a de cima
    		//if ((equipeAtual.pontuacao == 1 || equipeAtual.pontuacao == 2 || equipeAtual.pontuacao == 3 || equipeAtual.pontuacao == 4 || equipeAtual.pontuacao == 5 || equipeAtual.pontuacao == 6) && (acertou)) {
    			botaoCartasPos.SetActive(true);
    			botaoCartasPos.GetComponent<Button>().interactable = true;

                // Remove a pergunta da lista para não repetir
    			perguntas.Remove(pergunta);

    			StartCoroutine(nameof(FecharTelaEmSegundos), 1);

                //fazer efeito da carta positiva

    		} else {
                // Remove a pergunta da lista para não repetir
    			perguntas.Remove(pergunta);

    			StartCoroutine(nameof(FecharTelaEmSegundos), 1);

    			ProximoAJogar();
    		}
    	}
        else
        {
    		fundoEscuroPergunta.SetActive(true);
    		telaAlternativaIncorreta.SetActive(true);
    		PararContagem();
            AudioManager.instance.Play("snd_wrong");
    		acertou = false;

            if ((equipeAtual.pontuacao == 6 || equipeAtual.pontuacao == 8 || equipeAtual.pontuacao == 14) && (!acertou)) {
                //para testar sem ter que ficar acertando a casa correta do tabuleiro descomente a linha de baixo e comente a de cima
                //if ((equipeAtual.pontuacao == 1 || equipeAtual.pontuacao == 2 || equipeAtual.pontuacao == 3 || equipeAtual.pontuacao == 4 || equipeAtual.pontuacao == 5 || equipeAtual.pontuacao == 6) && (!acertou)) {

                botaoCartasNeg.SetActive(true);
                botaoCartasNeg.GetComponent<Button>().interactable = true;

                // Remove a pergunta da lista para não repetir
                perguntas.Remove(pergunta);

                StartCoroutine(nameof(FecharTelaEmSegundos), 1);

            } else {
                // Remove a pergunta da lista para não repetir
                perguntas.Remove(pergunta);

                StartCoroutine(nameof(FecharTelaEmSegundos), 1);

                if ((equipeAtual.carta != null) && (equipeAtual.carta.id_carta == 3)) {
                    equipeAtual.pontuacao -= 1;
                    RetornarEfeitoNeg();
                } else if (equipeAtual.pontuacao == 4 || equipeAtual.pontuacao == 10 || equipeAtual.pontuacao == 16) {

                    /*
                    if (equipeAtual.pontuacao - numSorteado != 0) {
                        equipeAtual.pontuacao -= numSorteado * 2;
                        if (equipeAtual.pontuacao <= 0)
                            equipeAtual.pontuacao = 1;
                    } else {
                        */
                        equipeAtual.pontuacao -= numSorteado * 2;
                        if (equipeAtual.pontuacao <= 0)
                            equipeAtual.pontuacao = 0;

                    RetornarEfeitoNeg();
                } else {
    				Retornar();
    			}
    		}
        }
        // Destaca a resposta correta
        DestacarRespostaCorreta();
    }

    public void DestacarRespostaCorreta()
    {
        if (pergunta.alternativas[0].correto == true)
        {
            imgAlternativa1.color = Color.green;
        }
        else
        {
            imgAlternativa1.color = Color.red;
        }

        if (pergunta.alternativas[1].correto == true)
        {
            imgAlternativa2.color = Color.green;
        }
        else
        {
            imgAlternativa2.color = Color.red;
        }

        if (pergunta.alternativas[2].correto == true)
        {
            imgAlternativa3.color = Color.green;
        }
        else
        {
            imgAlternativa3.color = Color.red;
        }

        if (pergunta.alternativas[3].correto == true)
        {
            imgAlternativa4.color = Color.green;
        }
        else
        {
            imgAlternativa4.color = Color.red;
        }
    }

    // Botao comprar carta positiva
    public void BotaoCartasPos() {

    	botaoCartasPos.SetActive(false);

    	efeitoBonus = true;

    	equipeAtual.carta = cartasPos[Random.Range(0, cartasPos.Count)];

    	StartCoroutine(nameof(ExibirCartaPosTela), 4);

    	Debug.Log(equipeAtual.carta.descricao);

    }

    private IEnumerator ExibirCartaPosTela(int segundos) {
    	cartaAtual = Instantiate(cartasPosGO[equipeAtual.carta.id_carta - 1], t);
        cartaAtualPositiva = true;
        yield return new WaitForSeconds(segundos / 2);
    	//Destroy(instancia);
    	//CartaPositiva();
    }

    private IEnumerator ExibirCartaNegTela(int segundos)
    {
        cartaAtual = Instantiate(cartasNegGO[equipeAtual.carta.id_carta - 1], t);
        cartaAtualPositiva = false;
        yield return new WaitForSeconds(segundos / 2);
        //CartaNegativa();
        //yield return new WaitForSeconds(segundos/2);
        //Destroy(instancia);
    }

    public void DestruirCartaAtual()
    {
        if (cartaAtualPositiva)
        {
            CartaPositiva();
            Destroy(cartaAtual);
        }
        else
        {
            CartaNegativa();
            Destroy(cartaAtual);
        }
    }

    void CartaPositiva() {
        //vai fazer aquilo que é para ser feito nessa rodada que seria avançar as casas
        //caso não seja algo para ser realizado nessa rodada ele passa a vez
        //e na proxima vez do jogador será aplicado o efeito da carta
    	if ((equipeAtual.carta.id_carta == 4)) {
    		numSorteado = 2;
    		podeJogar = true;
    	} else if ((equipeAtual.carta.id_carta == 5)) {
    		numSorteado = 3;
    		podeJogar = true;
    	} else {
    		ProximoAJogar();
    	}
    }

    // Botao comprar carta negativa
    public void BotaoCartasNeg() {
    	botaoCartasNeg.SetActive(false);

    	equipeAtual.carta = cartasNeg[Random.Range(0, cartasNeg.Count)];

    	StartCoroutine(nameof(ExibirCartaNegTela), 4);

        // Exibir a carta na tela e adicionar um delay aqui. Depois prossegue
    	Debug.Log(equipeAtual.carta.descricao);

    }

    void CartaNegativa() {
    	if (equipeAtual.carta.id_carta == 3) {
            //volte para casa que estava no inicio do seu turno +1
    		equipeAtual.pontuacao -= (numSorteado + 1);
    		if (equipeAtual.pontuacao <= 0)
    		equipeAtual.pontuacao = 0;
    		RetornarEfeitoNeg();
    	} else if (equipeAtual.carta.id_carta == 4) {
            //volte para casa que estava no inicio do seu turno +2
    		equipeAtual.pontuacao -= (numSorteado + 2);
    		if (equipeAtual.pontuacao <= 0)
    		equipeAtual.pontuacao = 0;
    		RetornarEfeitoNeg();
    	} else if (equipeAtual.carta.id_carta == 5) {
            //volte o dobro da quantidade de casas que andou
    		equipeAtual.pontuacao -= numSorteado * 2;
    		if (equipeAtual.pontuacao <= 0)
    		equipeAtual.pontuacao = 0;
    		RetornarEfeitoNeg();
    	} else {
    		Retornar();
    	}
    }

    void RetornarEfeitoNeg() {

        // Fazendo o jogador voltar casas
    	equipeAtualGO = GameObject.Find(equipeAtual.nomeGO);

    	StartCoroutine(nameof(PodeAndar), 2);

        // Remove a pergunta da lista para não repetir
        //ja esta sendo removida na tela de validacao vai remover de novo?
        //perguntas.Remove(pergunta);
    }

    void Retornar() {
        // Fazendo o jogador voltar casas
    	equipeAtualGO = GameObject.Find(equipeAtual.nomeGO);

    	equipeAtual.pontuacao -= numSorteado;

    	StartCoroutine(nameof(PodeAndar), 2);

        // Remove a pergunta da lista para não repetir
        //ja esta sendo removida na tela de validacao vai remover de novo?
        //perguntas.Remove(pergunta);
    }

    void TempoLimiteAtingido() {
    	PararContagem();
        AudioManager.instance.Play("snd_wrong");
    	fundoEscuroPergunta.SetActive(true);
    	telaTempoLimiteAtingido.SetActive(true);

    	StartCoroutine(nameof(FecharTelaEmSegundos), 1);

    	Retornar();
    }

    private IEnumerator TelaSorteio(float tempo) {

        telaSorteio.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        int aux = 0;

        while (aux < equipes.Count) {

            imgJogTelaSorteio[aux].SetActive(true);
            Debug.Log("aux: " + aux);

            GameObject descricao = GameObject.Find("jogador" + (aux + 1) + "-text");
            descricao.GetComponent<Text>().text = equipes[aux].nome;

            switch (equipes[aux].nomeGO) {
                case "jogador1":
                    imgJogTelaSorteio[aux].GetComponent<Image>().sprite = jogador_img_on[0];

                    if (aux == 0) {
                        iconeJogadores[0].GetComponent<Image>().sprite = jogador_img_on[0];
                    } else {
                        iconeJogadores[aux].GetComponent<Image>().sprite = jogador_img_off[0];
                    }
                    break;
                case "jogador2":
                    imgJogTelaSorteio[aux].GetComponent<Image>().sprite = jogador_img_on[1];

                    if (aux == 0) {
                        iconeJogadores[0].GetComponent<Image>().sprite = jogador_img_on[1];
                    } else {
                        iconeJogadores[aux].GetComponent<Image>().sprite = jogador_img_off[1];
                    }
                    break;
                case "jogador3":
                    imgJogTelaSorteio[aux].GetComponent<Image>().sprite = jogador_img_on[2];

                    if (aux == 0) {
                        iconeJogadores[0].GetComponent<Image>().sprite = jogador_img_on[2];
                    } else {
                        iconeJogadores[aux].GetComponent<Image>().sprite = jogador_img_off[2];
                    }
                    break;
                case "jogador4":
                    imgJogTelaSorteio[aux].GetComponent<Image>().sprite = jogador_img_on[3];

                    if (aux == 0) {
                        iconeJogadores[0].GetComponent<Image>().sprite = jogador_img_on[3];
                    } else {
                        iconeJogadores[aux].GetComponent<Image>().sprite = jogador_img_off[3];
                    }
                    break;
                case "jogador5":
                    imgJogTelaSorteio[aux].GetComponent<Image>().sprite = jogador_img_on[4];

                    if (aux == 0) {
                        iconeJogadores[0].GetComponent<Image>().sprite = jogador_img_on[4];
                    } else {
                        iconeJogadores[aux].GetComponent<Image>().sprite = jogador_img_off[4];
                    }
                    break;
                default:
                    break;
            }
            AudioManager.instance.Play("snd_button");
            yield return new WaitForSeconds(0.8f);
            aux++;
        }
        AudioManager.instance.Play("snd_transition");
        yield return new WaitForSeconds(tempo);

        telaSorteio.SetActive(false);
        botaoRoleta.SetActive(true);
        txtVezJogador.SetActive(true);

        HabilitarIcones();
    }

    private IEnumerator PodeAndar(int segundos) {
    	yield return new WaitForSeconds(segundos);
    	StartCoroutine(nameof(AbduzirEquipe), 0.05);
    }

    private IEnumerator AbduzirEquipe(float tempoEsperar) {

    	int qntRetornar;

    	if (equipeAtual.pontuacao > 0) {
    		qntRetornar = equipeAtual.pontuacao;
    	} else {
    		qntRetornar = 0;
    	}

    	GameObject playerGo = GameObject.Find(equipeAtual.nomeGO);
    	GameObject breakpoint = GameObject.Find("BreakPoint" + (qntRetornar));

        //indo para posicao acima do player
    	Vector2 v2 = new Vector2(playerGo.transform.position.x, playerGo.transform.position.y+2);

        AudioManager.instance.Play("snd_alienVoa");

        while ((oviniGO.transform.position.x != v2.x) && (oviniGO.transform.position.y != v2.y)) {

    		oviniGO.transform.position = Vector2.MoveTowards(oviniGO.transform.position,
    			v2, 30f * Time.deltaTime);

    		yield return new WaitForSeconds(tempoEsperar);
    	}

        AudioManager.instance.Play("snd_alienAbduz");
    	oviniLaser1.enabled = true;
    	yield return new WaitForSeconds(0.5f);
    	oviniLaser1.enabled = false;
    	oviniLaser2.enabled = true;
    	yield return new WaitForSeconds(0.5f);
    	playerGo.GetComponentInChildren<SpriteRenderer>().enabled = false;
    	oviniLaser1.enabled = false;
    	oviniLaser2.enabled = false;


    	yield return new WaitForSeconds(1);

        //teleportando o player para a casa correta
    	v2 = new Vector2(breakpoint.transform.position.x, breakpoint.transform.position.y + 2);

    	while ((oviniGO.transform.position.x != v2.x) && (oviniGO.transform.position.y != v2.y)) {

    		oviniGO.transform.position = Vector2.MoveTowards(oviniGO.transform.position,
    			v2, 30f * Time.deltaTime);
    		playerGo.transform.position = Vector2.MoveTowards(playerGo.transform.position,
    			breakpoint.transform.position, 30f * Time.deltaTime);

    		yield return new WaitForSeconds(tempoEsperar);
    	}

        AudioManager.instance.Play("snd_alienAbduz");
        oviniLaser1.enabled = true;
    	yield return new WaitForSeconds(0.5f);
    	oviniLaser1.enabled = false;
    	oviniLaser2.enabled = true;
    	yield return new WaitForSeconds(0.2f);
    	playerGo.GetComponentInChildren<SpriteRenderer>().enabled = true;
    	yield return new WaitForSeconds(0.3f);
    	oviniLaser1.enabled = false;
    	oviniLaser2.enabled = false;

    	yield return new WaitForSeconds(1);

        //fazendo o ovini ir para uma das 4 posicoes da tela randomicamente
    	GameObject breakpointovini = GameObject.Find("Breakpointovini" + Random.Range(1, 5));

    	while ((oviniGO.transform.position != breakpointovini.transform.position)) {

    		oviniGO.transform.position = Vector2.MoveTowards(oviniGO.transform.position,
    			breakpointovini.transform.position, 30f * Time.deltaTime);

    		yield return new WaitForSeconds(tempoEsperar);
    	}

    	ProximoAJogar();
    }

    private IEnumerator FecharTelaEmSegundos(int segundos)
    {
        AudioManager.instance.Stop("snd_clock");
        AudioManager.instance.Stop("snd_clock_10s");
        GameObject respostaCorreta = GameObject.Find("RespostaCorreta");
    	GameObject respostaIncorreta = GameObject.Find("RespostaIncorreta");
    	yield return new WaitForSeconds(segundos);
    	fundoEscuroTela.SetActive(false);
    	fundoEscuroPergunta.SetActive(false);
    	telaTempoLimiteAtingido.SetActive(false);
    	if (respostaCorreta != null) respostaCorreta.SetActive(false);
    	if (respostaIncorreta != null) respostaIncorreta.SetActive(false);
        StartCoroutine(FadeInOutMusicaPergunta(false));
        fundoEscuroTela.SetActive(false);

        HabilitarIcones();
    }

    public void IniciarContagem(){
    	iniciarContagem = true;
    }

    void PararContagem(){
    	iniciarContagem = false;
    }

    public void ExibirRoleta()
    {
    	telaRoleta.SetActive(!telaRoleta.activeInHierarchy);
    }

    private void Mover(int pontoParadaFinal) {

    	GameObject.Find("BreakPoint" + pontoParadaFinal + "").GetComponent<BoxCollider2D>().enabled = true;
    	GameObject.Find("BreakPoint" + pontoParadaFinal + "").GetComponent<SpriteRenderer>().enabled = true;
    }

    private void ExibirVencedores(){
        List<Equipe> vencedores = equipes.OrderBy(eq => eq.pontuacao).ToList();
        vencedores.Reverse();
        telaVitoria.SetActive(true);
        AudioManager.instance.Stop("mus_Jogo");
        AudioManager.instance.Play("mus_Vitoria");
        txtEquipeVencedora.text = vencedores[0].nome;
        totem1lugar.gameObject.SetActive(true);
        totem2lugar.gameObject.SetActive(true);
        totem3lugar.gameObject.SetActive(true);
       
        switch (vencedores[0].nomeGO)
        {
            case "jogador1":
                totem1lugar.sprite = totens[0];
                break;
            case "jogador2":
                totem1lugar.sprite = totens[1];
                break;
            case "jogador3":
                totem1lugar.sprite = totens[2];
                break;
            case "jogador4":
                totem1lugar.sprite = totens[3];
                break;
            case "jogador5":
                totem1lugar.sprite = totens[4];
                break;
            default:
                totem1lugar.gameObject.SetActive(false);
                break;
        }

        switch (vencedores[1].nomeGO)
        {
            case "jogador1":
                totem2lugar.sprite = totens[0];
                break;
            case "jogador2":
                totem2lugar.sprite = totens[1];
                break;
            case "jogador3":
                totem2lugar.sprite = totens[2];
                break;
            case "jogador4":
                totem2lugar.sprite = totens[3];
                break;
            case "jogador5":
                totem2lugar.sprite = totens[4];
                break;
            default:
                totem2lugar.gameObject.SetActive(false);
                break;
        }

        if (vencedores.Count <= 2)
        {
            trofeu3lugar.SetActive(false);
            return;
        }

        switch (vencedores[2].nomeGO)
        {
            case "jogador1":
                trofeu3lugar.SetActive(true);
                totem3lugar.sprite = totens[0];
                break;
            case "jogador2":
                trofeu3lugar.SetActive(true);
                totem3lugar.sprite = totens[1];
                break;
            case "jogador3":
                trofeu3lugar.SetActive(true);
                totem3lugar.sprite = totens[2];
                break;
            case "jogador4":
                trofeu3lugar.SetActive(true);
                totem3lugar.sprite = totens[3];
                break;
            case "jogador5":
                trofeu3lugar.SetActive(true);
                totem3lugar.sprite = totens[4];
                break;
            default:
                trofeu3lugar.SetActive(false);
                totem3lugar.gameObject.SetActive(false);
                break;
        }

    }

    private IEnumerator FadeInOutMusicaPergunta(bool fadeIn)
    {
        Sound musicaJogo = AudioManager.instance.CurrentSound("mus_jogo");
        Sound musicaPergunta = AudioManager.instance.CurrentSound("mus_pergunta");

        if (fadeIn)
        {
            musicaPergunta.source.volume = 0f;
            musicaPergunta.source.Play();
            while (musicaJogo.source.volume > 0)
            {
                musicaJogo.source.volume = (musicaJogo.source.volume - Time.deltaTime);
                yield return new WaitForSeconds(0.05f);
                if (musicaPergunta.source.volume <= musicaPergunta.volume)
                    musicaPergunta.source.volume = (musicaPergunta.source.volume + Time.deltaTime);
            }

        }
        else
        {
            musicaJogo.source.volume = 0f;
            musicaJogo.source.Play();
            while (musicaPergunta.source.volume > 0)
            {
                musicaPergunta.source.volume = (musicaPergunta.source.volume - Time.deltaTime);
                yield return new WaitForSeconds(0.05f);
                if (musicaJogo.source.volume <= musicaJogo.volume)
                    musicaJogo.source.volume = (musicaJogo.source.volume + Time.deltaTime);
            }
        }
        yield return null;
    }

	public void ExpandirOuOcultarPergunta()
	{
		if (fundoPerguntaExpandida.activeSelf == false)
		{
			fundoEscuroPergunta.SetActive(true);
			fundoPerguntaExpandida.SetActive(true);
			txtPerguntaExpandida.text = pergunta.descricao;
		}
		else
		{
			fundoEscuroPergunta.SetActive(false);
			fundoPerguntaExpandida.SetActive(false);
		}
	}

    public void SortearOrdem() {

        equipes = equipes.OrderBy(x => Random.value).ToList();

    }

    public void RetornarMenu()
    {
        StartCoroutine(nameof(LoadingJogo));
    }

    IEnumerator LoadingJogo()
    {
        telaLoading.SetActive(true);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Menu");
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
                Destroy(gameObject);
            }
            yield return new WaitForSeconds(1);
        }

        yield return null;
    }
}
