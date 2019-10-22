using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirTapHandler : MonoBehaviour, IMixedRealityPointerHandler
{

    private void Start()
    {
        CoreServices.InputSystem.RegisterHandler<IMixedRealityPointerHandler>(this);
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        if (CoreServices.InputSystem.GazeProvider.GazePointer.Result.CurrentPointerTarget != null)
        {
            if (CoreServices.InputSystem.GazeProvider.GazePointer.Result.CurrentPointerTarget.name.Contains("Point"))
            {
                return;
            }
        }
        MeasureManager.Instance.Click(eventData);
    }

    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
    }

    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {
    }

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
    }
}
