using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordController : MonoBehaviour
{
    [SerializeField]
    private float speed = 1000f;
    [SerializeField]
    private float timeToDestroy = 1f;
    [SerializeField]
    private GameObject player;

    //set timer to destroy this object
    private void OnEnable()
    {
        Destroy(gameObject, timeToDestroy);
    }

    //move sword around body to resemble swinging motion
    void Update()
    {
        transform.RotateAround(transform.position, -Vector3.up, speed * Time.deltaTime);
        transform.RotateAround(transform.position, Vector3.right, .02f * speed * Time.deltaTime);
        transform.Translate(Vector3.right * 4 * Time.deltaTime);
    }

    //detect contact
    private void OnCollisionEnter(Collision other)
    {

        ContactPoint contact = other.GetContact(0);

    }
}