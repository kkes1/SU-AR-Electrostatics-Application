using Microsoft.Azure.SpatialAnchors;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.WSA;
using UnityEngine.XR.WSA.Input;

public class SphereAnchor : MonoBehaviour
{
    public GameObject spherePrefab;
    protected string SpatialAnchorsAccountId = "c1eb8a1c-40f5-4ed1-8312-d595a305f409";
    protected string SpatialAnchorsAccountKey = "pW0GcJc9mqIqjnLXVNi0FJDw2rp9upMlQ13aprIN3TA=";
    private GestureRecognizer recognizer;
    protected CloudSpatialAnchorSession cloudSpatialAnchorSession;
    protected CloudSpatialAnchor currentCloudAnchor;
    protected bool tapExecuted = false;
    protected string cloudSpatialAnchorId = "";
    protected float recommendedForCreate = 0;
    protected GameObject sphere;
    public List<GameObject> anchors = new List<GameObject>();
    public List<Material> anchorMats = new List<Material>();

    protected Material sphereMaterial;
    // Start is called before the first frame update
    void Start()
    {
        recognizer = new GestureRecognizer();
        recognizer.StartCapturingGestures();
        recognizer.SetRecognizableGestures(GestureSettings.Tap);
        recognizer.Tapped += HandleTap;
        InitializeSession();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void InitializeSession()
    {
        if(string.IsNullOrEmpty(SpatialAnchorsAccountId))
        {
            Debug.LogError("No account id set.");
            return;
        }
        if(string.IsNullOrEmpty(SpatialAnchorsAccountKey))
        {
            Debug.LogError("No account key set.");
            return;
        }
        cloudSpatialAnchorSession = new CloudSpatialAnchorSession();
        cloudSpatialAnchorSession.Configuration.AccountId = SpatialAnchorsAccountId.Trim();
        cloudSpatialAnchorSession.Configuration.AccountKey = SpatialAnchorsAccountKey.Trim();
        cloudSpatialAnchorSession.LogLevel = SessionLogLevel.All;
        cloudSpatialAnchorSession.Error += CloudSpatialAnchorSession_Error;
        cloudSpatialAnchorSession.OnLogDebug += CloudSpatialAnchorSession_OnLogDebug;
        cloudSpatialAnchorSession.SessionUpdated += CloudSpatialAnchorSession_SessionUpdated;
        cloudSpatialAnchorSession.Start();
    }
    public void HandleTap(TappedEventArgs tapEvent)
    {
        /*if(tapExecuted)
        {
            return;
        }*/
        tapExecuted = true;
        Ray GazeRay = new Ray(tapEvent.headPose.position, tapEvent.headPose.forward);

        RaycastHit hitInfo;
        Physics.Raycast(GazeRay, out hitInfo, float.MaxValue);

        this.CreateAndSaveSphere(hitInfo.point);

    }
    protected virtual void CreateAndSaveSphere(Vector3 hitPoint)
    {
        //make sphere object
        sphere = GameObject.Instantiate(spherePrefab, hitPoint, Quaternion.identity) as GameObject;
        //add local anchor to it
        sphere.AddComponent<WorldAnchor>();
        sphereMaterial = spherePrefab.GetComponent<MeshRenderer>().material;
        anchors.Add(sphere);
        anchorMats.Add(sphereMaterial);
        sphereMaterial.color = Color.white;
        //Local Anchor created

        //create spatial anchor
        currentCloudAnchor = new CloudSpatialAnchor();

        WorldAnchor worldAnchor = sphere.GetComponent<WorldAnchor>();
        //create ptr for spatial anchor
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
                success = currentCloudAnchor != null;

                if (success)
                {
                    tapExecuted = false;      
                    cloudSpatialAnchorId = currentCloudAnchor.Identifier;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("ASA Error: " + ex.Message);
            }
        });
        }
    private void CloudSpatialAnchorSession_Error(object sender, SessionErrorEventArgs args)
    {
        Debug.LogError("ASA Error: " + args.ErrorMessage);
    }

    private void CloudSpatialAnchorSession_OnLogDebug(object sender, OnLogDebugEventArgs args)
    {
        Debug.Log("ASA Log: " + args.Message);
        System.Diagnostics.Debug.WriteLine("ASA Log: " + args.Message);
    }

    private void CloudSpatialAnchorSession_SessionUpdated(object sender, SessionUpdatedEventArgs args)
    {
        Debug.Log("ASA Log: recommendedForCreate: " + args.Status.RecommendedForCreateProgress);
        recommendedForCreate = args.Status.RecommendedForCreateProgress;
    }

}
