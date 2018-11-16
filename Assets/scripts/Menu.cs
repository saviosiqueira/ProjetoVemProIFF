using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	private int equipeAtual;
	private string nomePersonagem;

    [SerializeField] private InputField inputNomeEquipe;
    [SerializeField] private Text textNomeEquipe;
    [SerializeField] private List<Sprite> listaPersonagens;
    [SerializeField] private Image spritePersonagemEscolhido;
    [SerializeField] private GameObject telaLoading;
    [SerializeField] private List<Sala> salas;
    [SerializeField] private AudioManager audioManager;

    private Conexao conexao;

    //tela
    public GameObject botao;
    public Transform t;

    public GameObject telaEquipe;

    public int personagemEscolhido;


    void Start(){
        if (audioManager != null)
            audioManager.Play("mus_menu");
        else
            Debug.LogError("AudioManager não está atribuído");

		equipeAtual = 1;

	    personagemEscolhido = 0;
        spritePersonagemEscolhido.sprite = listaPersonagens[personagemEscolhido];

        //ta assim por enquanto como teste só puxando a descrição da 1 sala do banco de dados
        conexao = new Conexao();
        BuscarSalasHabilitadas();
        
        foreach (Sala s in salas) {
            GameObject instancia  = Instantiate(botao, t);
            instancia.name = "botao"+s.id_sala;
            Button botaoInstancia = instancia.GetComponent<Button>();
            botaoInstancia.GetComponentInChildren<Text>().text = s.descricao;
            botaoInstancia.onClick.AddListener(() => telaEquipe.SetActive(true));
            botaoInstancia.onClick.AddListener(() => AudioManager.instance.Play("snd_button"));
            instancia.GetComponentInChildren<BotaoEscolherSala>().sala = s;
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
			Equipe j = new Equipe(equipeAtual, inputNomeEquipe.text, nomePersonagem);
			GameControl.equipes.Add(j);
			equipeAtual++;
			inputNomeEquipe.text = "";
			Debug.Log(j.id_equipe);

		    telaLoading.SetActive(true);

            StartCoroutine(nameof(LoadingJogo));
		}
		else if (equipeAtual < GameControl.numeroEquipes){

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

        if (listaPersonagens[personagemEscolhido] == null)
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

        if (listaPersonagens[personagemEscolhido] == null)
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
        yield return new WaitForSeconds(3);

        SceneManager.LoadSceneAsync("Principal");
    }
}
