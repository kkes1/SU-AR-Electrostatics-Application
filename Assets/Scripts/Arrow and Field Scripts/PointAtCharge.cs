using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EM.Core;

/* 
 * The following script is attached to the VectorArrow prefab.
 * Its contents are not called by any speech command.
 */

public class PointAtCharge : MonoBehaviour
{
    /*
     * This function uses EM.Core to call and store the value of the electric field.
     * If an electric field is detected, the current contextual object is rotated such that it is reacting to the electric field.
     */
    private void Update()
    {

        Vector3 FieldValue = EMController.Instance.GetFieldValue(this.transform.position, EM.Core.FieldType.Electric, null); // retrieve electric field value from EM Core
        if (FieldValue == Vector3.zero) // if there is no field charge detected...
        {
            Quaternion rotation = new Quaternion();
            rotation.Set(0, 0, 0, 0); // this makes it so that LookRotation does not spit out continuous errors when no charges are found.
            this.transform.rotation = rotation; // set rotation of current object's transform to previous instantiated quaternion
        }
        else
        {
            Quaternion rotation = Quaternion.LookRotation(FieldValue); // create new Quaternion that rotates towards FieldValue
            this.transform.rotation = rotation; // set rotation of current arrow reactor to point towards FieldValue
        }
    }
}