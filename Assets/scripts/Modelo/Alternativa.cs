using System.Collections;
using System.Collections.Generic;

public class Alternativa {

    public long id_pergunta { get; set; }
    public string descricao { get; set; }
    public bool correto { get; set; }

    public Alternativa() {

    }

    public Alternativa(long id_pergunta, string descricao, bool correto) {
        this.id_pergunta = id_pergunta;
        this.descricao = descricao;
        this.correto = correto;
    }

}
