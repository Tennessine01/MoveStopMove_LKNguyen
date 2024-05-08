using PixelPlay.OffScreenIndicator;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach the script to the off screen indicator panel.
/// </summary>
[DefaultExecutionOrder(-1)]
public class OffScreenIndicator : MonoBehaviour
{
    [Range(0.5f, 0.9f)]
    [Tooltip("Distance offset of the indicators from the centre of the screen")]
    [SerializeField] private float screenBoundOffset = 0.9f;

    private Camera mainCamera;
    private Vector3 screenCentre;
    private Vector3 screenBounds;

    private List<Target> targets = new List<Target>();

    public static Action<Target, bool> TargetStateChanged;

    void Awake()
    {
        mainCamera = Camera.main;
        screenCentre = new Vector3(Screen.width, Screen.height, 0) / 2;
        screenBounds = screenCentre * screenBoundOffset;
        TargetStateChanged += HandleTargetStateChanged;
    }

    void LateUpdate()
    {
        DrawIndicators();
    }

    void DrawIndicators()
    {
        foreach (Target target in targets)
        {
            Vector3 screenPosition = OffScreenIndicatorCore.GetScreenPosition(mainCamera, target.transform.position);
            bool isTargetVisible = OffScreenIndicatorCore.IsTargetVisible(screenPosition);
            float distanceFromCamera = target.NeedDistanceText ? target.GetDistanceFromCamera(mainCamera.transform.position) : float.MinValue;
            Indicator indicator = null;

            if (target.NeedArrowIndicator)
            {
                float angle = float.MinValue;
                OffScreenIndicatorCore.GetArrowIndicatorPositionAndAngle(ref screenPosition, ref angle, screenCentre, screenBounds);
                indicator = GetIndicator(ref target.indicator); // Always gets the arrow indicator from the pool.
                indicator.transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
            }

            if (indicator)
            {
                indicator.SetImageColor(target.TargetColor);
                indicator.SetDistanceText(distanceFromCamera);
                indicator.transform.position = screenPosition;
                indicator.SetTextRotation(Quaternion.identity);
            }
        }
    }

    private Indicator GetIndicator(ref Indicator indicator)
    {
        if (indicator == null || indicator.Type != IndicatorType.ARROW)
        {
            indicator?.Activate(false);
            indicator = ArrowObjectPool.current.GetPooledObject();
            indicator.Activate(true); // Sets the indicator as active.
        }
        return indicator;
    }

    private void HandleTargetStateChanged(Target target, bool active)
    {
        if (active)
        {
            targets.Add(target);
        }
        else
        {
            target.indicator?.Activate(false);
            target.indicator = null;
            targets.Remove(target);
        }
    }

    private void OnDestroy()
    {
        TargetStateChanged -= HandleTargetStateChanged;
    }
}
