using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;

public class Conexao {

	private string source;
	private MySqlConnection conexao;
    private bool conexaoRealizada = false;

	public void ConectarBanco() {

        source = "Server=127.0.0.1;" +
	             "User=root;" +
	             "Database=bancosgp;" +
	             "Pooling=false;";
        

        conexao = new MySqlConnection (source);
		conexao.Open();
        conexaoRealizada = true;

        Debug.Log("Conexão realizada com sucesso!");
	}

    public void DesconectarBanco() {
        if (conexaoRealizada) {
            conexao.Close();
            conexaoRealizada = false;
        }
    }

    public List<Pergunta> ListarPerguntasPeloIdSala(long id_sala) {

        List<Pergunta> perguntas = new List<Pergunta>();

        ConectarBanco();

        MySqlCommand comando = conexao.CreateCommand();
        comando.CommandText = "select p.ID, p.DESCRICAO, p.DIFICULDADE, p.TEMPO from pergunta p join sala_pergunta sp on (sp.pergunta_ID = p.ID) where sp.Sala_ID = " + id_sala + " and p.HABILITAR = true";


        MySqlDataReader dados = comando.ExecuteReader();

        while (dados.Read()) {
            Pergunta p = new Pergunta();

            p.id_pergunta = (long)dados["id"];

            p.descricao = Convertertexto((byte[]) dados["descricao"]);
            p.dificuldade = (string)dados["dificuldade"];
            p.tempo = (int)dados["tempo"];

            perguntas.Add(p);
        }

        dados.Close();

        foreach (Pergunta p in perguntas) {
            comando.CommandText = "select a.id, a.descricao, a.correto from alternativa a join pergunta_alternativa pa on (a.ID = pa.alternativas_ID) where pa.Pergunta_ID =" + p.id_pergunta;
            dados = comando.ExecuteReader();

            p.alternativas = new List<Alternativa>();

            while (dados.Read()) {

                Alternativa a = new Alternativa();

                a.id_pergunta = (long)dados["id"];
                a.descricao = (string)dados["descricao"];
                a.correto = (bool)dados["correto"];

                p.alternativas.Add(a);
            }

            dados.Close();
        }

        DesconectarBanco();
        
        return perguntas;
    }

    public List<Sala> ListarSalasHabilitadas() {

        List<Sala> salas = new List<Sala>();

        ConectarBanco();

        MySqlCommand comando = conexao.CreateCommand();
        comando.CommandText = "select * from sala s where s.HABILITAR = true";
        MySqlDataReader dados = comando.ExecuteReader();

        while (dados.Read()) {
            Sala s = new Sala();

            s.id_sala = (long)dados["id"];
            s.descricao = (string)dados["descricao"];

            salas.Add(s);

        }

        dados.Close();
        DesconectarBanco();

        return salas;
    }

    public string Convertertexto(byte[] data) {
        return System.Text.Encoding.UTF8.GetString(data);
    }
}
