using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakPoint : MonoBehaviour {

    [SerializeField]
    private GameControl gamecontrol;

    [SerializeField]
    private GameObject player;

    public bool colidiu;

    public float moveSpeed;

    // Use this for initialization
    void Start () {
        colidiu = false;
    }
	
	// Update is called once per frame
	void Update () {
        StartCoroutine(nameof(Colidiu), 1);
    }

    IEnumerator Colidiu(int tempoEsperar) {
        if ((colidiu) && (player.GetComponent<MouseGrab>().soltou) && (!gamecontrol.efeitoBonus)) {
            player.transform.position = transform.position;

            GetComponent<Collider2D>().enabled = false;
            player.GetComponentInChildren<CapsuleCollider2D>().enabled = false;
            player.GetComponent<MouseGrab>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;

            yield return new WaitForSeconds(tempoEsperar);

            gamecontrol.GerarPergunta();
        } else if ((colidiu) && (player.GetComponent<MouseGrab>().soltou) && (gamecontrol.efeitoBonus)) {
                player.transform.position = transform.position;

                GetComponent<Collider2D>().enabled = false;
                player.GetComponentInChildren<CapsuleCollider2D>().enabled = false;
                player.GetComponent<MouseGrab>().enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;

                gamecontrol.ProximoAJogar();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.name.StartsWith("Trigger")) return;
        player = collision.transform.parent.parent.gameObject;
        colidiu = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name.Equals("Trigger"))
            colidiu = false;
    }
}
