using UnityEngine;

/*
 * The following script is attached to the PointChargePrefab prefab.
 * The function createTheCharge is called by the Main Camera's Speech Input Handler via the speech command "Create Charge".
 * The function createFreeCharge is called by the Main Camera's Speech Input Handler via the speech command "Create Free".
 * The function deleteCharge is called by the PointChargePrefab's Speech Input Handler via the speech command "Delete".
 */

public class CreateDeleteCharges : MonoBehaviour
{
    // Reference to the Prefab. Drag a Prefab into this field in the Inspector.
    public GameObject EMCharge;
    private GameObject newCharge; // used to create a clone of the above public GameObject later.
    public float distance = 2.0f; // How far in front of the camera the object will be instantiated (in meters).

    // This function is called by the "Create Charge" speech command from the Main Camera.
    // This function creates a new Charge that has all movement constraints active. It will remain stationary unless acted upon by the user.
    public void createTheCharge()
    {
        newCharge = Instantiate(EMCharge, new Vector3(0, 0, 0), Quaternion.identity) as GameObject; // create new PointChargePrefab clone and briefly instantiate at the origin.
        newCharge.transform.position = Camera.main.transform.position + Camera.main.transform.forward * distance; // set the clone's position immediately to a set distance in front of the camera.
        newCharge.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ; // set it so that it does not move unless touched by user
    }

    // This function is called by the "Create Free" speech command from the Main Camera.
    // This function creates a new Charge that is not constrained to its original position. It will be freely affected by the forces.
    public void createFreeCharge()
    {
        newCharge = Instantiate(EMCharge, new Vector3(0, 0, 0), Quaternion.identity) as GameObject; // create new PointChargePrefab clone and briefly instantiate at the origin.
        newCharge.transform.position = Camera.main.transform.position + Camera.main.transform.forward * distance; // set the clone's position immediately to a set distance in front of the camera.
        newCharge.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ; // set rotation constraints on the Charge's RigidBody
    }

    // This function is called by the Speech Input Handler of the PointChargePrefab when the user uses the speech command "Delete."
    public void deleteCharge()
    {
        Destroy(this.gameObject); // Call the Destroy() function on the Charge that the user is currently looking at.
    }
}
