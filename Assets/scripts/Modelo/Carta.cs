using System.Collections;
using System.Collections.Generic;


public class Carta {

    public int id_carta { get; set; }
    public char tipo { get; set; }
    public string descricao { get; set; }
    public int modQntCasas { get; set; }
    public int modQntTempo { get; set; }
    public bool multiplica { get; set; }
    public bool usarAgora { get; set; }

    public Carta() {

    }

    public Carta(int id_carta, char tipo, int modQntCasas, int modQntTempo, bool multiplica, bool usarAgora, string descricao) {
        this.id_carta = id_carta;
        this.tipo = tipo;
        this.modQntCasas = modQntCasas;
        this.modQntTempo = modQntTempo;
        this.multiplica = multiplica;
        this.usarAgora = usarAgora;
        this.descricao = descricao;
    }
}
