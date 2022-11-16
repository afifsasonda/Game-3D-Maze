using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] Rigidbody ribdy;
    
    public Vector3 Position => ribdy.position;
    public bool IsMoving => ribdy.velocity != Vector3.zero;
    public bool IsTeleporting => isTeleporting;

    [SerializeField] Vector3 lastPosition;
    bool isTeleporting;

    private void Awake()
    {
        if(ribdy == null)
        {
            ribdy = GetComponent<Rigidbody>();
        }
        lastPosition = this.transform.position;
    }

    internal void AddForce(Vector3 force)
    {
        ribdy.isKinematic = false;
        lastPosition = this.transform.position;
        ribdy.AddForce(force,ForceMode.Impulse);
    }

    private void FixedUpdate() {
        if(ribdy.velocity != Vector3.zero && ribdy.velocity.magnitude < 0.5f)
        {
            ribdy.velocity=Vector3.zero;
            ribdy.isKinematic = true;
            
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Out")
        {
            //teleport
            StopAllCoroutines();
            StartCoroutine(DelayedTeleport());
        }
    }

    IEnumerator DelayedTeleport()
    {
        isTeleporting = true;
        yield return new WaitForSeconds(2);
        ribdy.isKinematic = true;
        this.transform.position = lastPosition;
        isTeleporting = false;
    }

}
