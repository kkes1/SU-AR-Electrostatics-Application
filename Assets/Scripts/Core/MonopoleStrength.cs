using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.UIElements;
using UnityEngine;

/* 
 * The following script is attached to the PointChargePrefab prefab. This script was originally included with the EMCore package.
 * The function changeStrength is called by the PointChargePrefab's Speech Input Handler via the speech commands "Increase" and "Decrease".
 *      "Increase" causes the Speech Input Handler to send the float value "1" to the function, increasing the charge's value by 1.
 *      "Decrease" causes the Speech Input Handler to send the float value "-1" to the function, decreasing the charge's value by 1.
 *      The function has been altered from its original functionality to access the Text Mesh of a child GameObject of the PointChargePrefab,
 *          allowing for the Text Mesh to properly display the charge's current value overlayed on the PointChargePrefab.
 */

namespace EM.Core
{
    public class MonopoleStrength : EMStrength
    {
        //public float InspectorStrength;
        [SerializeField]
        private float _strength = 1;
        private Transform parent; //ADDED 12/2. This Transform will refer to the transform of the PointChargePrefab object.
        public float Strength
        {
            get => _strength;
            set => _strength = value;
        }
        // Start is called before the first frame update
        //If a value is not provided in the inspector then set it to 1.  I could see this happening if object does not include EMStrength and it is generated at runtime.
        void Start()
        {
            if (_strength == 0)
            {
                _strength = 1;
            }
            parent = GetComponent<Transform>(); //ADDED 12/2 TO CHANGE TEXT OVERLAY
            //            else
            //            {
            //                _strength = InspectorStrength;
            //            }
        }
        
        // Accessing and changing strength via a property instead
//        //a way to access the strength from other methods
//        public float GetStrength()
//        {
//            return _strength;
//        }


        //allows strength to be changed
        //CHANGE MADE 12/2: This function now also accesses the Text Mesh of the PointChargePrefab's child GameObject, and alters its contents for display.
            // This allows for the charge level of the Charge to be accurately updated and displayed on the Charge.
        public void ChangeStrength(float Change)
        {
            _strength = _strength + Change;
            parent.GetComponentInChildren<TextMesh>().text = "" + _strength; // ADDED 12/2 TO CHANGE TEXT OVERLAY
        }
//
//
//        //allows strength to be changed
//        public void SetStrength(float Set)
//        {
//            _strength = Set;
//        }
    }
}
