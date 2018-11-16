using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseGrab : MonoBehaviour {

    //public Transform initialPosition;

    public bool soltou;

    public float moveSpeed;
    public float offset = 0.05f;

    private bool following;

    public Vector2 posAnterior;

    // Use this for initialization
    void Start() {
        soltou = true;
        following = false;
        offset += 10;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0) && ((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).magnitude <= offset)) {
                soltou = false;
                following = true;
        }
        if (following) {
            transform.position = Vector2.Lerp(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), moveSpeed);
        }
        if (Input.GetMouseButtonUp(0) && ((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).magnitude <= offset)) {
                soltou = true;
                following = false;
        }
    }
}
