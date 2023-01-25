using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    [SerializeField]
    private GameObject bulletDecal;
    [SerializeField]
    private GameObject DamagePopupText;
    [SerializeField]
    private float speed = 50f;
    [SerializeField]
    private float timeToDestroy = 3f;

    public Vector3 target { get; set; }
    public bool hit { get; set; }

    //set timer to destroy this object
    private void OnEnable()
    {
        Destroy(gameObject, timeToDestroy);
    }

    //move to raycast
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if(!hit && Vector3.Distance(transform.position, target) < .01f)
        {
            Destroy(gameObject);
        }
    }

    //leave bullet hole and/or break bullet on impact and display damage as floating text
    private void OnCollisionEnter(Collision other){

        ContactPoint contact = other.GetContact(0);
              
        GameObject.Instantiate(DamagePopupText, transform.position, Quaternion.identity);        
        GameObject.Instantiate(bulletDecal, contact.point + contact.normal * .0001f, Quaternion.LookRotation(contact.normal));
        GameObject txt = GameObject.Instantiate(DamagePopupText, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
