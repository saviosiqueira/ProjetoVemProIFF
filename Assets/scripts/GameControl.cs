using System;
using System.Collections;
using System.Collections.Generic;
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

	public List<GameObject> cartasPosGO;
	public List<GameObject> cartasNegGO;

	public Transform t;

	private List<Carta> cartasPos = new List<Carta>();
	private List<Carta> cartasNeg = new List<Carta>();

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
    public List<GameObject> totens;

    void Start ()
    {
        AudioManager.instance.Stop("mus_menu");
        AudioManager.instance.Play("mus_jogo");

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
    			return;
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

    	if (iniciarContagem){
    		tempoRestante -= Time.deltaTime;
    		if ( tempoRestante <= 0 )
    		{
    			txtTempo.text = "0";
    			TempoLimiteAtingido();
    		}
    		else
    		{
    			txtTempo.text = tempoRestante.ToString("#");
    		}
    	}
    }

    public void ProximoAJogar() {
    	// Verifica se o jogador ganhou:
    	if(equipeAtual.pontuacao >= 18) {
    		ExibirVencedor(equipeAtual);
    		return;
    	}

    	if (quemJoga < numeroEquipes) {
    		quemJoga++;
    	} else {
    		quemJoga = 1;
    	}

    	equipeAtual = equipes[quemJoga - 1];
    	equipeAtualGO = GameObject.Find(equipeAtual.nomeGO);
    	txtVezDoJogador.text = "Vez de " + equipeAtual.nome + "!";
    	GameObject.Find("BotaoExibirRoleta").GetComponent<Button>().interactable = true;
    	efeitoBonus = false;
    }

    // É chamado quando o jogador pressiona o botão de pergunta
    public void GerarPergunta(){

    	if(equipeAtual.pontuacao == 2 || equipeAtual.pontuacao == 8 || equipeAtual.pontuacao == 14){
    		telaPergunta.sprite = bgPerguntaCoringa;
    		txtTempo.color = new Color(1f, .56f, 0f);
    	}else if (equipeAtual.pontuacao == 4 || equipeAtual.pontuacao == 10 || equipeAtual.pontuacao == 16) {
    		telaPergunta.sprite = bgPerguntaBomba;
    		txtTempo.color = new Color(.92f, .28f, .30f);
    	} else {
    		telaPergunta.sprite = bgPerguntaPadrao;
    		txtTempo.color = new Color(.42f, .70f, .19f);
    	}
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

    		txtPergunta.text = pergunta.descricao;
    		txtAlternativa1.text = pergunta.alternativas[0].descricao;
    		txtAlternativa2.text = pergunta.alternativas[1].descricao;
    		txtAlternativa3.text = pergunta.alternativas[2].descricao;
    		txtAlternativa4.text = pergunta.alternativas[3].descricao;

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
    		acertou = true;

            if ((equipeAtual.pontuacao == 2 || equipeAtual.pontuacao == 8 || equipeAtual.pontuacao == 14) && (acertou)) {
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
    		acertou = false;

            if ((equipeAtual.pontuacao == 2 || equipeAtual.pontuacao == 8 || equipeAtual.pontuacao == 14) && (!acertou)) {
            //para testar sem ter que ficar acertando a casa correta do tabuleiro descomente a linha de baixo e comente a de cima
    		//if ((equipeAtual.pontuacao == 1 || equipeAtual.pontuacao == 2 || equipeAtual.pontuacao == 3 || equipeAtual.pontuacao == 4 || equipeAtual.pontuacao == 5 || equipeAtual.pontuacao == 6) && (!acertou)) {

    			botaoCartasNeg.SetActive(true);
    			botaoCartasNeg.GetComponent<Button>().interactable = true;

                // Remove a pergunta da lista para não repetir
    			perguntas.Remove(pergunta);

    			StartCoroutine(nameof(FecharTelaEmSegundos), 1);

                //fazer efeito da carta negativa

    		} else {
                // Remove a pergunta da lista para não repetir
    			perguntas.Remove(pergunta);

    			StartCoroutine(nameof(FecharTelaEmSegundos), 1);

    			if ((equipeAtual.carta != null) && (equipeAtual.carta.id_carta == 3)) { 
    				equipeAtual.pontuacao -= 1;
    				RetornarEfeitoNeg();
    			}else {
    				Retornar();
    			}
    		}
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
    	GameObject instancia = Instantiate(cartasPosGO[equipeAtual.carta.id_carta - 1], t);
    	yield return new WaitForSeconds(segundos);
    	Destroy(instancia);
    	CartaPositiva();
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

    private IEnumerator ExibirCartaNegTela(int segundos) {
    	GameObject instancia = Instantiate(cartasNegGO[equipeAtual.carta.id_carta - 1], t);
    	yield return new WaitForSeconds(segundos/2);
    	CartaNegativa();
    	yield return new WaitForSeconds(segundos/2);
    	Destroy(instancia);
    }

    void CartaNegativa() {
    	if (equipeAtual.carta.id_carta == 3) {
            //volte para casa que estava no inicio do seu turno +1
    		equipeAtual.pontuacao -= (numSorteado + 1);
    		if (equipeAtual.pontuacao <= 0)
    		equipeAtual.pontuacao = 1;
    		RetornarEfeitoNeg();
    	} else if (equipeAtual.carta.id_carta == 4) {
            //volte para casa que estava no inicio do seu turno +2
    		equipeAtual.pontuacao -= (numSorteado + 2);
    		if (equipeAtual.pontuacao <= 0)
    		equipeAtual.pontuacao = 1;
    		RetornarEfeitoNeg();
    	} else if (equipeAtual.carta.id_carta == 5) {
            //volte o dobro da quantidade de casas que andou
    		equipeAtual.pontuacao -= numSorteado * 2;
    		if (equipeAtual.pontuacao <= 0)
    		equipeAtual.pontuacao = 1;
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
    	fundoEscuroPergunta.SetActive(true);
    	telaTempoLimiteAtingido.SetActive(true);

    	StartCoroutine(nameof(FecharTelaEmSegundos), 1);

    	Retornar();
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
    		qntRetornar = 1;
    	}

    	GameObject playerGo = GameObject.Find(equipeAtual.nomeGO);
    	GameObject breakpoint = GameObject.Find("BreakPoint" + (qntRetornar));

        //indo para posicao acima do player
    	Vector2 v2 = new Vector2(playerGo.transform.position.x, playerGo.transform.position.y+2);       

    	while ((oviniGO.transform.position.x != v2.x) && (oviniGO.transform.position.y != v2.y)) {

    		oviniGO.transform.position = Vector2.MoveTowards(oviniGO.transform.position,
    			v2, 30f * Time.deltaTime);

    		yield return new WaitForSeconds(tempoEsperar);
    	}

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

    	if ((pontoParadaFinal != 8) && (pontoParadaFinal != 9) && (pontoParadaFinal != 16) && (pontoParadaFinal != 17) && (pontoParadaFinal != 24) && (pontoParadaFinal != 25)) {
    		GameObject.Find("BreakPoint" + pontoParadaFinal + "").GetComponent<BoxCollider2D>().enabled = true;
    	} else {
    		GameObject.Find("BreakPoint" + pontoParadaFinal + "").GetComponent<PolygonCollider2D>().enabled = true;
    	}
    	GameObject.Find("BreakPoint" + pontoParadaFinal + "").GetComponent<SpriteRenderer>().enabled = true;
    }

    private void ExibirVencedor(Equipe e){
    	telaVitoria.SetActive(true);
    	txtEquipeVencedora.text = e.nome;
    	int totemDaEquipe;
    	switch (e.nomeGO){
    		case "jogador1":
    		totemDaEquipe = 0;
    		break;
    		case "jogador2":
    		totemDaEquipe = 1;
    		break;
    		case "jogador3":
    		totemDaEquipe = 2;
    		break;
    		case "jogador4":
    		totemDaEquipe = 3;
    		break;
    		case "jogador5":
    		totemDaEquipe = 4;
    		break;
    		default:
    		totemDaEquipe = 0;
    		break;
    	}

    	for (int i = 0; i < totens.Count; i++){
    		if (i == totemDaEquipe){
    			totens[i].SetActive(true);
    		}
    		else
    		{
    			totens[i].SetActive(false);
    		}
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
}
