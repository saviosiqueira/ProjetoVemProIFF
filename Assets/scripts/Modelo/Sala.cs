using System.Collections;
using System.Collections.Generic;

public class Sala {

    public long id_sala { get; set; }
    public string descricao { get; set; }

    public Sala() {
    }

    public Sala(long id_sala, string descricao) {
        this.id_sala = id_sala;
        this.descricao = descricao;
    }
}
