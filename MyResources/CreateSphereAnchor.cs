using Microsoft.Azure.SpatialAnchors;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.WSA;
using UnityEngine.XR.WSA.Input;
public class CreateSphereAnchor : MonoBehaviour
{
        public GameObject spherePrefab;
        public List<GameObject> createdSpheres = new List<GameObject>();
        public List<Material> createdSphereMats = new List<Material>();
        protected string SpatialAnchorsAccountId = "c1eb8a1c-40f5-4ed1-8312-d595a305f409";
        protected string SpatialAnchorsAccountKey = "pW0GcJc9mqIqjnLXVNi0FJDw2rp9upMlQ13aprIN3TA=";
        private GestureRecognizer recognizer;
        protected bool tapped = false;
        protected string cloudSpatialAnchorId = "";
        protected GameObject Currsphere;
        protected Material CurrsphereMaterial;
        protected float recommendedForCreate = 0;
        protected CloudSpatialAnchorSession cloudSpatialAnchorSession;
        protected CloudSpatialAnchor currentCloudAnchor;
    // Start is called before the first frame update
    void Start()
        {
            recognizer = new GestureRecognizer();
            recognizer.StartCapturingGestures();
            recognizer.SetRecognizableGestures(GestureSettings.Tap);
            recognizer.Tapped += TapAction;
            InitializeSpatialAnchorsSession();
        }   
        public void TapAction(TappedEventArgs tapEvent)
        {
            if (tapped)
            {
                return;
            }
            tapped = true;
            Ray GazeRay = new Ray(tapEvent.headPose.position, tapEvent.headPose.forward);
            RaycastHit hitInfo;
            Physics.Raycast(GazeRay, out hitInfo, float.MaxValue);

            this.CreateUploadSphere(hitInfo.point);
        }
    protected virtual void CreateUploadSphere(Vector3 hitPoint)
        {
            Currsphere = GameObject.Instantiate(spherePrefab, hitPoint, Quaternion.identity) as GameObject;
            createdSpheres.Add(Currsphere);
            Currsphere.AddComponent<WorldAnchor>();
            CurrsphereMaterial = Currsphere.GetComponent<MeshRenderer>().material;
            createdSphereMats.Add(CurrsphereMaterial);
            currentCloudAnchor = new CloudSpatialAnchor();
            WorldAnchor worldAnchor = Currsphere.GetComponent<WorldAnchor>();
            if (worldAnchor == null)
            {
                throw new Exception("Couldn't find the local anchor");
            }
            currentCloudAnchor.LocalAnchor = worldAnchor.GetNativeSpatialAnchorPtr();
                Task.Run(async () =>
                {
                    while (recommendedForCreate < 1.0F)
                    {
                        await Task.Delay(330);
                    }

                    bool success = false;
                    try
                    {
                        await cloudSpatialAnchorSession.CreateAnchorAsync(currentCloudAnchor);
                        if (currentCloudAnchor != null)
                        {
                            success = true;
                        }

                        if (success)
                        {
                            tapped = false;
                            cloudSpatialAnchorId = currentCloudAnchor.Identifier;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("ASA Error: " + ex.Message);
                    }
                });
        }
        void InitializeSpatialAnchorsSession()
        {
            if (string.IsNullOrEmpty(SpatialAnchorsAccountId))
            {
                Debug.LogError("No account id set.");
                return;
            }

            if (string.IsNullOrEmpty(SpatialAnchorsAccountKey))
            {
                Debug.LogError("No account key set.");
                return;
            }

            cloudSpatialAnchorSession = new CloudSpatialAnchorSession();
            cloudSpatialAnchorSession.LogLevel = SessionLogLevel.All;   
            cloudSpatialAnchorSession.Configuration.AccountId = SpatialAnchorsAccountId.Trim();
            cloudSpatialAnchorSession.Configuration.AccountKey = SpatialAnchorsAccountKey.Trim();
            cloudSpatialAnchorSession.SessionUpdated += CloudSpatialAnchorSession_SessionUpdated;
            cloudSpatialAnchorSession.Start();
        }
        private void CloudSpatialAnchorSession_SessionUpdated(object sender, SessionUpdatedEventArgs args)
        {
            recommendedForCreate = args.Status.RecommendedForCreateProgress;
        }

}