using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnFallingTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            transform.parent.GetComponent<Animator>().SetTrigger("FallingAction");
        }
    }
}
