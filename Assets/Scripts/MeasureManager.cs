using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeasureManager : MonoBehaviour
{

    private static MeasureManager _Instance = null;
    public static MeasureManager Instance
    {
        get
        {
            if (_Instance == null) _Instance = FindObjectOfType<MeasureManager>();
            return _Instance;
        }
    }

    [SerializeField]
    private Text valueT;

    [SerializeField]
    private GameObject PrefabPoint;
    private List<GameObject> Points;
    private LineRenderer lineRenderer;


    /// <summary>
    /// Est-ce que le placement de point est autorisé?
    /// </summary>
    public bool isUsable;

    public float InternoeudsSize { get; private set; }


    private IMixedRealityInputSystem inputSystem = null;

    //Récupération du InputSystem
    protected IMixedRealityInputSystem InputSystem
    {
        get
        {
            if (inputSystem == null)
            {
                MixedRealityServiceRegistry.TryGetService(out inputSystem);
            }
            return inputSystem;
        }
    }

    void Start()
    {
        Points = new List<GameObject>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.enabled = true;
    }

    public void StartMeasure()
    {
        lineRenderer.positionCount = 0;
        lineRenderer.enabled = true;
        isUsable = true;
    }

    public void StopMeasure()
    {
        lineRenderer.enabled = false;
        isUsable = false;
        for (int j = 0; j < transform.childCount; j++)
        {
            Destroy(transform.GetChild(j).gameObject);
        }
        Points.Clear();
        lineRenderer.positionCount = 0;
    }

    //Function call when is clicking
    public void Click(InputEventData eventData)
    {

        if (isUsable)
        {
            AddPoint();
        }
    }

    private void AddPoint()
    {
        Debug.Log(InputSystem.GazeProvider.GazeTarget);
#if !UNITY_EDITOR
        if (InputSystem.GazeProvider.GazeTarget.name.Contains("SpatialMesh") || InputSystem.GazeProvider.GazeTarget.name.Contains("Surface"))
        {
#endif
        GameObject point = Instantiate(PrefabPoint, this.transform);
        point.transform.position = InputSystem.GazeProvider.HitPosition;
        Points.Add(point);
        if (Points.Count > 1) //Changer l'état du point de "Removable" à "Manipulable"
        {
            Points[Points.Count - 2].GetComponent<PointCalculHandler>().GetManipulable();
        }
        DistancePoints();
#if !UNITY_EDITOR
    }
#endif
    }

    public void RemoveLastPoint()
    {
        if (Points.Count < 1) return;

        GameObject go = Points[Points.Count - 1];
        Points.Remove(go);
        Destroy(go);
        if (Points.Count > 0) //Changer l'état du nouveau dernier point de "Manipulable" à "Removable"
        {
            Points[Points.Count - 1].GetComponent<PointCalculHandler>().GetRemovable();
        }
        DistancePoints();
    }

    public void DistancePoints()
    {
        float value = 0f;
        for (int i = 1; i < Points.Count; i++)
        {
            value += Vector3.Distance(Points[i - 1].transform.position, Points[i].transform.position) * 100f;
        }
        InternoeudsSize = value;
        valueT.text = InternoeudsSize+" m";
        UpdateLine();
    }

    public void UpdateLine()
    {
        if (Points.Count < 1)
        {
            lineRenderer.positionCount = 0;
            return;
        }
        Vector3[] vec = new Vector3[Points.Count];
        for (int i = 0; i < vec.Length; i++)
        {
            vec[i] = Points[i].transform.position;
        }
        lineRenderer.positionCount = Points.Count;
        lineRenderer.SetPositions(vec);
    }
}
