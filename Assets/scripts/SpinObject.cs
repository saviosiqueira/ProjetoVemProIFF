using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpinObject : MonoBehaviour {
 
    [SerializeField]
    private float deltaLimit = 0.2f;
    [SerializeField]
    private float deltaReduce = 0.5f;
    public bool CanRotate {get; set;}
    [SerializeField]
    [Header("Valor que indica velocidade de rotação da roleta")]
    private float deltaRotation;

    private List<MouseGrab> scriptsJogadores;
    private float previousRotation;
	private float currentRotation;
	private float quarterRotation;
    private bool stopDrag;
    private bool girou;

	private void Start()
	{
	    scriptsJogadores = FindObjectsOfType<MouseGrab>().ToList();
		CanRotate = true;
	    DesativarPlayers();
    }

	// Update is called once per frame
	private void Update () {
		RotateThis();
	}

	private void RotateThis()
	{

        if (Input.GetMouseButtonDown(0) && CanRotate) {

            // Get initial rotation of this game object
            deltaRotation = 0f;

            Debug.Log("Down posicao da roleta: " + transform.position);
            Debug.Log("Down posicao do mouse na tela ao clicar: " + Camera.main.ScreenToWorldPoint(Input.mousePosition));

            previousRotation = AngleBetweenPoints(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Debug.Log("Down previous rotation:" + previousRotation);

            stopDrag = true;

        }else if (Input.GetMouseButton (0) && CanRotate) {

            Debug.Log("Entrou aqui teste");

			// Rotate along the mouse under Delta Rotation Limit
			currentRotation = AngleBetweenPoints (transform.position, Camera.main.ScreenToWorldPoint (Input.mousePosition));
			deltaRotation = Mathf.DeltaAngle (currentRotation, previousRotation);

            Debug.Log("Current rotation" + currentRotation + " Delta rotation " + deltaRotation);

			previousRotation = currentRotation;
			transform.Rotate (Vector3.back * Time.deltaTime, deltaRotation);
            
            if (deltaRotation != 0)
                stopDrag = true;
            
        }
        if (Mathf.Abs(deltaRotation) > 15f || girou) {
            Debug.Log("Comecou rodar");
            if (deltaRotation != 0) girou = true;

		    // Inertia
			transform.Rotate (Vector3.back * Time.deltaTime, deltaRotation);
			deltaRotation = Mathf.Lerp (deltaRotation, 0, deltaReduce * Time.deltaTime);

            if (Mathf.Abs(deltaRotation) <= deltaLimit)
		    {
		        deltaRotation = 0;
                if (girou)
		            StartCoroutine(nameof(PararPorSegundos), 3);
		    }
		    else
		    {
		        CanRotate = false;
		    }
		}

	}

	private float AngleBetweenPoints (Vector2 v2Position1, Vector2 v2Position2)
	{
		Vector2 v2FromLine = v2Position2 - v2Position1;
		Vector2 v2ToLine = new Vector2 (1, 0);
		
		float fltAngle = Vector2.Angle (v2FromLine, v2ToLine);

		// If rotation is more than 180
		Vector3 v3Cross = Vector3.Cross (v2FromLine, v2ToLine);
		if (v3Cross.z > 0) {
			fltAngle = 360f - fltAngle;
		}
		
		return fltAngle;
	}

    private void OnDisable()
    {
        CanRotate = true;
        girou = false;
    }

    private IEnumerator PararPorSegundos(int tempoEsperar)
    {
        yield return new WaitForSeconds(tempoEsperar);
        transform.parent.gameObject.SetActive(false);
        
    }

    private void DesativarPlayers() {
        foreach (MouseGrab jogador in scriptsJogadores)
        {
            jogador.enabled = false;
        }
    }
}
