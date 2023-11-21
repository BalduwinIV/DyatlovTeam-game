using System;
using UnityEngine;

public class PushObject : MonoBehaviour
{
    [SerializeField] private float pushForce = 1.0f;
    [SerializeField] private float attachmentDistance = 1.5f;
    private float distance;

    private GameObject objectToPush;
    private Vector3 attachmentPoint;

    private bool isAttached = false;
    private bool isPushing = false;
    private bool canPush = false;

    void Start(){
        objectToPush = GameObject.FindGameObjectsWithTag("ObjectToPush");
        distance = Vektor3.Distance(this.transform.position, objectToPush.transform.position);
        if (distance <= attachmentDistance) canPush = true;
    }

    void Update(){
        if (canPush && Input.GetKeyDown(KeyCode.E)){
            TogglePush();
        }
        if (isPushing){
            float inputHorizontal = Input.GetAxis("Horizontal");
            float pushDirection = Mathf.Sign(inputHorizontal);

            if (isAttached){
                float pushSpeed = pushForce * inputHorizontal;
                objectToPush.transform.Translate(Vector3.right * pushSpeed * Time.deltaTime);

            }
        }
    }

    void TogglePush(){
        isPushing = !isPushing;

        if (isPushing){
            AttachToObject();
        }
        else{
            DetachFromObject();
        }
    }

    void AttachToObject(){
        float playerWidth = GetComponent<Collider>().bounds.size.x;
        float objectWidth = objectToPush.GetComponent<Collider>().bounds.size.x;

        float offset = (playerWidth + objectWidth) / 2;
        attachmentPoint = new Vector3(objectToPush.transform.position.x + offset * Mathf.Sign(transform.localScale.x), transform.position.y, transfrom.position.z);

        transrom.position = attachmentPoint;
        isAttached = true;
    }

    void DetachFromObject(){
        isAttached = false;
    }
}
