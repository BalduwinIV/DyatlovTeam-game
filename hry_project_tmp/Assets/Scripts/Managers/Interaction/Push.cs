using System;
using UnityEngine;

public class PushObject : MonoBehaviour
{
    [SerializeField] private float pushForce = 1.0f;
    [SerializeField] private float attachmentDistance = 2.1f;
    private float distance;
    private float originalSpeed = 1;
    private float speed = 1;

    private GameObject objectToPush;
    private Vector3 attachmentPoint;

    private bool isAttached = false;
    private bool isPushing = false;
    private bool canPush = false;


    void FixedUpdate(){
        FindeObject();
        if (objectToPush == null) FindeObject();
        distance = Vector3.Distance(this.transform.position, objectToPush.transform.position);
        if (distance <= attachmentDistance){
            if (canPush && PlayerManager.instance.actionButtonIsPressed()){
                TogglePush();
            }
            if (isAttached){

                AttachToObject();
                float inputHorizontal = PlayerManager.instance.getRawMovementInputX();
                
                if (isPushing){
                    PlayerManager.instance.setPushingState(isPushing);
                    float pushSpeed = pushForce * inputHorizontal;
                    objectToPush.transform.Translate(pushSpeed * Time.deltaTime * Vector3.right);
                }
                else{
                    PlayerManager.instance.setPushingState(isPushing);
                }
            }
        }
        else{
            PlayerManager.instance.setSpeedReduction(originalSpeed);
            speed = originalSpeed;
            isAttached = false;
            isPushing = false;
        }
    }

    public float GetSpeedReduction()
    {
        // Get the rigidbody of the object being pushed
        Rigidbody objectRigidbody = objectToPush.GetComponent<Rigidbody>();
        if (objectRigidbody != null)
        {
            if (isPushing){
            // Adjust speed reduction based on the mass of the object
                float newSpeed = originalSpeed * objectRigidbody.mass / 5;
                PlayerManager.instance.setSpeedReduction(newSpeed);
                return newSpeed;
            }
        }

        // Default speed reduction if no rigidbody is found
        return originalSpeed;
    }

    void TogglePush(){
        isPushing = !isPushing;
        PlayerManager.instance.setPushingState(isPushing);

        if (isPushing){
            AttachToObject();
        }
        else {
            DetachFromObject();
        }
    }

    void AttachToObject(){
        float attachmnetX = transform.position.x;

        attachmentPoint = new Vector3(attachmnetX, transform.position.y, transform.position.z);
        transform.position = attachmentPoint;
        isAttached = true;
    }

    void DetachFromObject(){
        isAttached = false;
        isPushing = false;
    }

    GameObject FindNearestObjectWithTag(string tag){
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);
        if (objectsWithTag.Length == 0) return null;
        GameObject nearestObject = null;
        float nearestDistance = float.MaxValue;
        foreach(GameObject obj in objectsWithTag){
            if (obj != null) {
                float newDistance = Vector3.Distance(transform.position, obj.transform.position);
                if (newDistance < nearestDistance){
                    nearestObject = obj;
                    nearestDistance = newDistance;
                }
            }
        }
        return nearestObject;
    }

    void FindeObject(){
        objectToPush = FindNearestObjectWithTag("ObjectToPush");
        if (objectToPush != null){
            distance = Vector3.Distance(this.transform.position, objectToPush.transform.position);
            if (distance <= attachmentDistance){
                    canPush = true;
            }
            else{
                canPush = false;
            }
            speed = GetSpeedReduction();
        }
    }
}
