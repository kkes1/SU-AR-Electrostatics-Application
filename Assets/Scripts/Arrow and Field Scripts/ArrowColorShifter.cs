using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EM.Core;

/* 
 * NOTE: This script is left unused in the project. This script and its intended functionality were determined to be unnecessary during development. As such, the script is incomplete, lacking proper commentation and contains various bugs.
 * The following script is attached to the VectorArrow prefab.
 * Its contents are not called by any speech command.
 */

public class ArrowColorShifter : MonoBehaviour
{
    public Color baseColor, positiveColor, negativeColor;
    private Transform target;
    Color currentColor;
    MeshRenderer renderer;
    float x, y, z; // used to store transform's position


    private void Start()
    {
        renderer = GetComponent<MeshRenderer>(); // get the renderer at startup
        renderer.material.color = baseColor; // set the current color to baseColor
        currentColor = baseColor; // set the current color to baseColor
    }

    private void Update()
    {

        Vector3 FieldValue = EMController.Instance.GetFieldValue(this.transform.position, EM.Core.FieldType.Electric, null);
        if (FieldValue == Vector3.zero) // if there is no field charge detected
        {
            renderer.material.color = baseColor;
        }
        else
        {
            if(FieldValue.x > 0 && FieldValue.y > 0 && FieldValue.z > 0)
            {
                renderer.material.color = Color.Lerp(renderer.material.color, positiveColor, 0.1f);//(this.transform.position / FieldValue));
            }
            if(FieldValue.x < 0 && FieldValue.y < 0 && FieldValue.z < 0)
            {
                renderer.material.color = Color.Lerp(renderer.material.color, negativeColor, 0.1f);//(this.transform.position / FieldValue));
            }
        }
    }
}