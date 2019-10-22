using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointCalculHandler : MonoBehaviour
{
    private  bool isActuallyManipulable;
    private bool isRemovable;
    private ManipulationHandler manip;
    private Image Cross;
    private Button CrossBtn;


    // Start is called before the first frame update
    void Start()
    {
        isRemovable = true;
        isActuallyManipulable = false;
        manip = GetComponentInChildren<ManipulationHandler>();
        manip.enabled = false;
        Cross = GetComponentsInChildren<Image>()[1];
        CrossBtn = GetComponentInChildren<Button>();
        CrossBtn.onClick.AddListener(MeasureManager.Instance.RemoveLastPoint);
        Cross.enabled = CrossBtn.enabled = true;
    }

    public void GetManipulable()
    {
        if (isRemovable)
        {
            Cross.enabled = CrossBtn.enabled = false;
            isRemovable = false;
            manip.enabled = true;
        }
    }

    public void GetRemovable()
    {
        if (!isRemovable)
        {
            Cross.enabled = CrossBtn.enabled  = true;
            isRemovable = true;
            manip.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (isActuallyManipulable)
        {
            MeasureManager.Instance.DistancePoints();
        }
    }

    public void SetIsActuallyManipulable(bool value)
    {
        isActuallyManipulable = value;
    }
}
