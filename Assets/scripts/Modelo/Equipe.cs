using System.Collections;
using System.Collections.Generic;

public class Equipe {

	public long id_equipe {get; set;}
	public string nome {get; set;}
	public string nomeGO {get; set;}
	public int pontuacao {get; set;}
    public Carta carta { get; set; }

    public Equipe() {

    }

	public Equipe(long id_equipe, string nome, string nomeGO) {
		this.id_equipe = id_equipe;
		this.nome = nome;
		this.nomeGO = nomeGO;
		this.pontuacao = 0;
    }
}
