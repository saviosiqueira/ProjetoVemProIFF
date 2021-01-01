using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    private bool gerouBotaoSala = false;
    private bool podeGerarBotaoSala = true;
    [SerializeField]
    private Transform botoes;

    private int equipeAtual;
	private string nomePersonagem;

    [SerializeField]
	private InputField inputNomeEquipe;
    [SerializeField]
    private Text textNomeEquipe;
    [SerializeField]
    private GameObject txtMsgAlerta;
    [SerializeField]
    private List<Sprite> listaPersonagens;
    [SerializeField]
    private Image spritePersonagemEscolhido;
    [SerializeField]
    private GameObject telaLoading;
    [SerializeField]
    private List<Sala> salas;
    [SerializeField] private GameObject mensagemErro;
    [SerializeField] private Text txtMensagemErro;
    [SerializeField] private AudioManager audioManager;

    private Conexao conexao;

    //tela
    public GameObject botao;
    public Transform t;

    public GameObject telaEquipe;

    public int personagemEscolhido;


    void Start()
    {
        //Reseta a lista de equipes ao retornar ao menu
        GameControl.equipes = new List<Equipe>();
        //Reatribui o audio manager
        audioManager = FindObjectOfType<AudioManager>();

        if (audioManager != null)
        {
            audioManager.Play("mus_menu");
            audioManager.Stop("mus_jogo");
        }
        else
            Debug.LogError("AudioManager não está atribuído");

        equipeAtual = 1;

	    personagemEscolhido = 0;
        spritePersonagemEscolhido.sprite = listaPersonagens[personagemEscolhido];

        conexao = new Conexao();
    }

    public void IniciarJogo() {
        BuscarSalasHabilitadas();

        if (salas.Count > 0) {

            if (podeGerarBotaoSala) {

                gerouBotaoSala = true;
                podeGerarBotaoSala = false;
                txtMsgAlerta.SetActive(false);

                foreach (Sala s in salas) {
                    GameObject instancia = Instantiate(botao, t);
                    instancia.name = "botao" + s.id_sala;
                    Button botaoInstancia = instancia.GetComponent<Button>();
                    botaoInstancia.GetComponentInChildren<Text>().text = s.descricao;
                    botaoInstancia.onClick.AddListener(() => telaEquipe.SetActive(true));
                    botaoInstancia.onClick.AddListener(() => AudioManager.instance.Play("snd_button"));
                    instancia.GetComponentInChildren<BotaoEscolherSala>().sala = s;
                }
            }
        } else {

            foreach (Transform child in botoes) {
                Destroy(child.gameObject);
            }
            txtMsgAlerta.SetActive(true);
            podeGerarBotaoSala = true;
        }
    }

    public void NumeroDeJogadores (int x) {
		GameControl.numeroEquipes = x;
	}

    public void BuscarSalasHabilitadas() {
        salas = conexao.ListarSalasHabilitadas();
    }

	public void CriarJogador(){
	    nomePersonagem = "jogador" + (personagemEscolhido + 1);
	    //Debug.Log(nomePersonagem);

        if (equipeAtual == GameControl.numeroEquipes)
		{
            // Se a equipe não preencheu o nome, volta
            if (inputNomeEquipe.text.Length == 0) {
                mensagemErro.SetActive(true);
                txtMensagemErro.text = "Nome da equipe em branco";
                return;
            }
            if (inputNomeEquipe.text.Length > 30) {
                mensagemErro.SetActive(true);
                txtMensagemErro.text = "Nome da equipe não pode exceder 30 caracteres";
                return;
            }
			Equipe j = new Equipe(equipeAtual, inputNomeEquipe.text, nomePersonagem);
			GameControl.equipes.Add(j);
			equipeAtual++;
			inputNomeEquipe.text = "";
			Debug.Log(j.id_equipe);

		    telaLoading.SetActive(true);

            StartCoroutine(nameof(LoadingJogo));
		}
		else if (equipeAtual < GameControl.numeroEquipes){
            // Se a equipe não preencheu o nome, volta
            if (inputNomeEquipe.text.Length == 0) {
                mensagemErro.SetActive(true);
                txtMensagemErro.text = "Nome da equipe em branco";
                return;
            }
            if (inputNomeEquipe.text.Length > 30) {
                mensagemErro.SetActive(true);
                txtMensagemErro.text = "Nome da equipe não pode exceder 30 caracteres";
                return;
            }
            Equipe j = new Equipe(equipeAtual, inputNomeEquipe.text, nomePersonagem);
			GameControl.equipes.Add(j);
			equipeAtual++;
			inputNomeEquipe.text = "";
		    textNomeEquipe.text = "Equipe " + equipeAtual + "\n Digite o nome da sua equipe";
			Debug.Log(j.id_equipe);
		}

	    listaPersonagens[personagemEscolhido] = null;
        Incrementa();
	}

	public void NomePersonagemEscolhido(string nome){
		nomePersonagem = nome;
	}

    public void QuitGame() {
        Application.Quit();
    }

    public void Incrementa()
    {
        if (personagemEscolhido >= listaPersonagens.Count-1)
            personagemEscolhido = 0;
        else
            personagemEscolhido++;

        while (listaPersonagens[personagemEscolhido] == null)
        {
            if (personagemEscolhido+1 > listaPersonagens.Count)
                personagemEscolhido = 0;
            else
                personagemEscolhido++;
        }

        spritePersonagemEscolhido.sprite = listaPersonagens[personagemEscolhido];
    }

    public void Decrementa()
    {
        if (personagemEscolhido <= 0)
            personagemEscolhido = listaPersonagens.Count-1;
        else
            personagemEscolhido--;

        while (listaPersonagens[personagemEscolhido] == null)
        {
            if (personagemEscolhido - 1 < 0)
                personagemEscolhido = listaPersonagens.Count-1;
            else
                personagemEscolhido--;
        }

        spritePersonagemEscolhido.sprite = listaPersonagens[personagemEscolhido];
    }

    IEnumerator LoadingJogo()
    {
        //yield return new WaitForSeconds(3);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Principal");
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
            }
            yield return new WaitForSeconds(1);
        }

        yield return null;
    }
}