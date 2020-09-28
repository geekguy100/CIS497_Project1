/*****************************************************************************
// File Name :         PickUpObject.cs
// Author :            Kyle Grenier
// Creation Date :     9/16/2020
//
// NOTE:               Code taken from class textbook.
// Brief Description : Enables the player to pick up objects in range.
*****************************************************************************/
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    //The range from this object at which an object can be picked up.
    [SerializeField] private float grabbingRange = 3f;

    //The range from this object at which an object can be pulled towards us.
    [SerializeField] private float pullingRange = 20f;

    //The location at which picked up objects are held.
    [SerializeField] private Transform holdPoint = null;

    //The key to press to pick up or drop an object.
    [SerializeField] private KeyCode grabKey = KeyCode.E;

    //The key to press to throw an object.
    [SerializeField] private KeyCode throwKey = KeyCode.Mouse0;

    //The key to press to throw an object.
    //TODO: Code from book states that KeyCode.E should work, and given the conditions that are required to
    //drop a toy, it should work, but the toy appears to be quickly grabbed again upon dropping it. Maybe add a timer before picking it back up?
    //For now, I added in another key to drop the toy.
    [SerializeField] private KeyCode dropKey = KeyCode.Q;

    //The amount of force to apply on a thrown object.
    [SerializeField] private float throwForce = 100f;

    //The amount of force to apply on objects that we're pulling toward us.
    //Don't forget to take friction and drag into account.
    [SerializeField] private float pullForce = 50f;

    //If the grab join encounters this much force, break it.
    [SerializeField] private float grabBreakingForce = 100f;

    //If the grab joint encounters this much torque, break it.
    [SerializeField] private float grabBreakingTorque = 100f;

    //The joint that holds our grabber object. Null if we're not holding anything.
    private FixedJoint grabJoint;

    //The rigidbody that we're holding. Null if we're not holding anything.
    private Rigidbody grabbedRigidbody;

    //LayerMask of interactables.
    [SerializeField] private LayerMask whatIsToys;


    private void Awake()
    {
        //Perform quick validity check when we start up.
        if (holdPoint == null)
            Debug.LogError("Grab hold point must not be null!");

        if (holdPoint.IsChildOf(transform) == false)
            Debug.LogError("Grab hold point must be a child of this object.");

        //ORIGINAL CODE BELOW. For some reason, it changed the MainCamera's LayerMask, not the parent's.
        //Collider playerCollider = GetComponentInParent<Collider>();
        Collider playerCollider = transform.parent.GetComponent<Collider>();
        playerCollider.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void Update()
    {
        //Is the user holding the grab key, and we're not holding something?
        if (Input.GetKey(grabKey) && grabJoint == null)
        {
            //Attempt to perform a pull or a grab.
            AttemptPull();
        }
        //Did the user just press the grab key, and we're holding something?
        else if (Input.GetKeyDown(dropKey) && grabJoint != null)
        {
            Drop();
        }
        //Does the user want to throw the held object, and we're holding something?
        else if (Input.GetKeyDown(throwKey) && grabJoint != null)
        {
            //Apply the throw force.
            Throw();
        }
    }

    //Throw the held object.
    private void Throw()
    {
        //Cannot throw if we're not holding anything.
        if (grabbedRigidbody == null)
            return;

        //Keep a reference to the body we were holding, because Drop will reset it.
        Rigidbody thrownBody = grabbedRigidbody;

        //Calculate the force to apply to the object.
        Vector3 force = transform.forward * throwForce;

        //We need to drop the body we're holding before we can throw it.
        Drop();

        //Apply the force to the object.
        thrownBody.AddForce(force, ForceMode.Impulse);
    }

    //Attempts to pull or pick up the object directly ahead of this object.
    //When this script is attached to a camera, it will try to get the object directly
    //in the middle of the camera's view.
    private void AttemptPull()
    {
        //Perform a raycast. If we hit something that has a Rigidbody that is not kinematic, pick it up.
        var ray = new Ray(transform.position, transform.forward);

        //Create a var to store the results of what we hit.
        RaycastHit hit;

        //Create a layer mask that represents every layer except the player's.
        //var everyLayerButPlayers = ~(1 << LayerMask.NameToLayer("Player"));

        //Combine this layer mask with the one that raycasts usually use; this has the effect of
        //removing the Player layer from the list of layers to raycast against.
        //var layerMask = Physics.DefaultRaycastLayers & everyLayerButPlayers;

        //Perform a raycast that uses this layer mask to ignore the player's.
        //We use our pulling range because it's the longest;
        //if the object is actually within our (shorter) grabbing range, we'll grab it instead
        //of pulling it.
        //var hitSomething = Physics.Raycast(ray, out hit, pullingRange, layerMask);
        var hitSomething = Physics.Raycast(ray, out hit, pullingRange, whatIsToys);

        if (!hitSomething)
        {
            //Our raycast did not hit anything within the pulling range.
            return;
        }

        //We hit something! Is it something we can pick up?
        grabbedRigidbody = hit.rigidbody;

        if (grabbedRigidbody == null || grabbedRigidbody.isKinematic || !grabbedRigidbody.GetComponent<Toy>().grabbable)
        {
            //We cannot pick this up; it either has no rigidbody or is kinematic or the toy has recently been thrown.
            return;
        }

        //We now have an object that's within our pulling range.
        //Is the object within the grabbing range, too?
        if (hit.distance < grabbingRange)
        {
            //We can pick it up.
            //Move the rigidbody to the grab position.
            grabbedRigidbody.transform.position = holdPoint.position;

            //Create a joint that will hold this in place, and configure it.
            grabJoint = gameObject.AddComponent<FixedJoint>();
            grabJoint.connectedBody = grabbedRigidbody;
            grabJoint.breakForce = grabBreakingForce;
            grabJoint.breakTorque = grabBreakingTorque;

            //Ensure that the grabbed object doesn't collide with this collider,
            //or any collider in our parent, which could cause problems.
            foreach (var myCollider in GetComponentsInParent<Collider>())
                Physics.IgnoreCollision(myCollider, hit.collider, true);

            //Make sure this toy cannot be grabbed until after it's been thrown.
            grabbedRigidbody.GetComponent<Toy>().grabbable = false;
        }
        else
        {
            //It's not in the grabbing range, but it is in the pulling range.
            //Pull the rigidbody towards us, until it's in the grabbing range.
            Vector3 pull = -transform.forward * pullForce;
            grabbedRigidbody.AddForce(pull);
        }
    }

    //Drops the object.
    private void Drop()
    {
        if (grabJoint != null)
        {
            Destroy(grabJoint);
        }


        //Bail out if the object we are holding isn't there anymore.
        if (grabbedRigidbody == null)
        {
            return;
        }

        //Re-enable collisions between this object and our collider(s).
        foreach (var myCollider in GetComponentsInParent<Collider>())
        {
            Physics.IgnoreCollision(myCollider, grabbedRigidbody.GetComponent<Collider>(), false);
        }

        grabbedRigidbody.GetComponent<Toy>().OnToyDrop();
        grabbedRigidbody = null;
    }

    //Draw the location of the hold point.
    private void OnDrawGizmos()
    {
        if (holdPoint == null)
            return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(holdPoint.position, 0.2f);
    }

    //Called when a joint that's attached to the game object this component is on has broken.
    private void OnJointBreak(float breakForce)
    {
        //When a joint breaks, call Drop to make sure we clean up after ourselves.
        Drop();
    }
}