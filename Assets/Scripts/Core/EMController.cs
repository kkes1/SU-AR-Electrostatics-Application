using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EM.Core
{


    public enum FieldType
    {
        Electric,
        Magnetic
    };

    public class EMController : MonoBehaviour
    {

        private static EMController _instance;

        public static EMController Instance
        {
            get
            {
                // Create instance if one doesn't already exist and we aren't closing the application
                if (!_instance && !applicationQuitting)
                {
                    GameObject obj = new GameObject("EMController_Autogen");
                    _instance = obj.AddComponent<EMController>();
                }
                return _instance;
            }
        }

        private static bool applicationQuitting = false;

        private List<FieldGenerator> FieldGenerators = new List<FieldGenerator>();


        // Are we sure we want to have EMController control the forces on the objects?
        // [Jared] No--let's specify how each FieldReactor works in its script.
        private List<FieldReactor> FieldReactors = new List<FieldReactor>();


        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }


        //at the moment the developer has to be smart enough to not add a fieldreactor to a dipole.
        private void FixedUpdate()
        {
            foreach (FieldReactor FR in FieldReactors)
            {
//                Vector3 Force = new Vector3(0, 0, 0);
//                if (FR.fieldType == FieldType.Electric)
//                    Force = FR.eMStrength.Strength *
//                            GetFieldValue(FR.gameObject.transform.position, FieldType.Electric);
//                //do the cross product "backwards" beacuse unity uses left handed cross product
//                else if (FR.fieldType == FieldType.Magnetic)
//                    Force = FR.eMStrength.Strength *
//                            Vector3.Cross(GetFieldValue(FR.gameObject.transform.position, FieldType.Magnetic),
//                                FR.RigidBody.velocity);
//
//                FR.RigidBody.AddForce(Force);
                FR.ApplyForces();
                FR.ApplyTorques();
            }
        }

        public void RegisterFieldGenerator(FieldGenerator NewFieldGenerator)
        {
            // Make sure new field generator isn't already registered
            if (FieldGenerators.Contains(NewFieldGenerator))
            {
                return;
            }
            // Add new field generator
            FieldGenerators.Add(NewFieldGenerator);
        }

        public void UnRegisterFieldGenerator(FieldGenerator RemoveFieldGenerator)
        {
            FieldGenerators.Remove(RemoveFieldGenerator);
        }

        public void RegisterFieldReactor(FieldReactor NewFieldReactor)
        {
            // Make sure new field reactor isn't already registered
            if (FieldReactors.Contains(NewFieldReactor))
            {
                return;
            }
            // Add new field reactor
            FieldReactors.Add(NewFieldReactor);
        }

        public void UnRegisterFieldReactor(FieldReactor RemoveFieldReactor)
        {
            FieldReactors.Remove(RemoveFieldReactor);
        }
        
        /// <summary>
        /// Sets a flag indicating that the application is quitting, such that new instances of EMController aren't
        /// created through UnRegister calls when the application is closing.
        /// </summary>
        private void OnApplicationQuit()
        {
            applicationQuitting = true;
        }
    
        
        /// <summary>
        /// Calculate the value of the <paramref name="field"/> field at world-space <paramref name="position"/>.
        /// If <paramref name="generatorToExclude"/> is specified, it is left out of the calculation, e.g. for
        /// calculating the electric field at a point charge due to other charges.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="field"></param>
        /// <param name="generatorToExclude"></param>
        /// <returns>Direction and magnitude of the field.</returns>
        public Vector3 GetFieldValue(Vector3 position, FieldType field, FieldGenerator generatorToExclude = null)
        {
            Vector3 fieldValue = new Vector3(0, 0, 0);

            foreach (FieldGenerator fieldGenerator in FieldGenerators)
            {
                if (generatorToExclude)
                {
                    if (fieldGenerator == generatorToExclude)
                    {
                        continue;
                    }
                }
                if (fieldGenerator.fieldType == field)
                {
                    fieldValue = fieldValue + fieldGenerator.FieldValue(position);
                }
            }

            return fieldValue;
        }

        
    }
}