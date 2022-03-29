using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    Rigidbody rigidbody;
    public float moveSpeed = 20;
    GameObject gun;
    // Start is called before the first frame update
    void Start()
    {
        gun = GameObject.FindGameObjectWithTag("Gun");

        Destroy(gameObject, 2);
        Vector3 bulletDirection = (transform.position - gun.transform.position).normalized;
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = bulletDirection * moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
