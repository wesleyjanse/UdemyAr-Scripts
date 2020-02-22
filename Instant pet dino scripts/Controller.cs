using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Wikitude;
using UnityEngine.EventSystems;

public class Controller : MonoBehaviour
{
    public InstantTracker Tracker;
    public Button StateButton;
    public Text StateButtonText;
    public Text MessageBox;
    public GameObject DinoPrefab;
    GameObject dino;
    public GameObject PizzaPrefab;

    GridRenderer grid;
    //InstantTrackable trackable;

    bool isSpawned = false;

    InstantTrackingState trackerState = InstantTrackingState.Initializing;
    bool isChanging = false;

    bool isTracking = false;

    void Awake()
    {
        grid = GetComponent<GridRenderer>();
        grid.enabled = true;

        //trackable = Tracker.GetComponentInChildren<InstantTrackable>();
    }


    public void OnInitializationStarted(InstantTarget target)
    {
        SetSceneEnabled(true);
    }

    public void OnInitializationStopped(InstantTarget target)
    {
        SetSceneEnabled(false);
    }


    public void OnSceneRecognized(InstantTarget target)
    {
        SetSceneEnabled(true);
        isTracking = true;
        MessageBox.text = "Scene Found";
    }

    public void OnSceneLost(InstantTarget target)
    {
        SetSceneEnabled(false);
        isTracking = false;
        MessageBox.text = "Scene Lost";
    }

    public void SetSceneEnabled(bool enabled)
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("pizza");
        foreach(GameObject g in gos)
        {
            Renderer[] rends = g.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in rends)
                r.enabled = enabled;
        }

        Renderer[] dinorends = dino.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in dinorends)
            r.enabled = enabled;
    }

    public void StateButtonPressed()
    {
        if (!isSpawned)
        {
            SpawnDino();
            isSpawned = true;
        }

        if (!isChanging)
        {
            if (trackerState == InstantTrackingState.Initializing)
            {
                if (Tracker.CanStartTracking())
                {
                    StateButtonText.text = "Switching State...";
                    isChanging = true;
                    Tracker.SetState(InstantTrackingState.Tracking);
                }
            }
            else
            {
                StateButtonText.text = "Switching State ...";
                isChanging = true;
                Tracker.SetState(InstantTrackingState.Initializing);
            }
        }
    }

    public void OnStateChange(InstantTrackingState newState)
    {
        trackerState = newState;

        if (trackerState == InstantTrackingState.Initializing)
        {
            StateButtonText.text = "Start Tracking";
            MessageBox.text = "Not Tracking";
        }
        else
        {
            StateButtonText.text = "Stop Tracking";
            MessageBox.text = "Tracking";
        }

        isChanging = false;
    }

    public void OnHeigtValueChanged(float newHeightValue)
    {
        Tracker.DeviceHeightAboveGround = newHeightValue;
    }

    // Start is called before the first frame update
    void Start()
    {
        MessageBox.text = "Starting the SDK";
    }

    void SpawnDino()
    {
        Vector3 zPos = Camera.main.transform.forward * 5;
        zPos.y = 0;
        zPos = Quaternion.AngleAxis(Random.Range(-45, 45), Vector3.up) * zPos;
        dino = Instantiate(DinoPrefab, zPos, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isTracking && !EventSystem.current.IsPointerOverGameObject())
        {
            var cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            UnityEngine.Plane groundPlane = new UnityEngine.Plane(Vector3.up, Vector3.zero);
            float touchPos;
            if (groundPlane.Raycast(cameraRay, out touchPos))
            {
                Vector3 position = cameraRay.GetPoint(touchPos);
                GameObject pizza = Instantiate(PizzaPrefab, position, Quaternion.identity);
            }
        }

        if (trackerState == InstantTrackingState.Initializing)
        {
            if (Tracker.CanStartTracking())
            {
                grid.TargetColor = Color.green;
            } else
            {
                grid.TargetColor = GridRenderer.DefaultTargetColor;
            }
        } else
        {
            grid.TargetColor = GridRenderer.DefaultTargetColor;
        }
    }
}
