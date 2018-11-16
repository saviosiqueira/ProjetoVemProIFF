using System.Collections;
using System.Collections.Generic;

public class Pergunta {

    public long id_pergunta { get; set;}
    public string descricao {get; set;}
    public string dificuldade {get; set;}
    public int tempo {get; set;}
    public List<Alternativa> alternativas {get; set;}

    public Pergunta() {

    }

    public Pergunta(long id_pergunta, string descricao, string nivel, int tempo) {
        this.id_pergunta = id_pergunta;
        this.descricao = descricao;
        this.dificuldade = nivel;
        this.tempo = tempo;
        this.alternativas = new List<Alternativa>();
    }
}
