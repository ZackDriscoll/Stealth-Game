using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    private Transform tf;
    public float movementSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        pawn = FindObjectOfType<PlayerPawn>();
        tf = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pawn.Attack();
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            tf.position += tf.right * movementSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            tf.position -= tf.right * movementSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            tf.position += tf.up * movementSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            tf.position -= tf.up * movementSpeed * Time.deltaTime;
        }

        Vector3 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 lookAt = mouseScreenPosition;

        float AngleRad = Mathf.Atan2(lookAt.y - this.transform.position.y, lookAt.x - this.transform.position.x);

        float AngleDeg = (180 / Mathf.PI) * AngleRad;

        this.transform.rotation = Quaternion.Euler(0, 0, AngleDeg);
    }
}