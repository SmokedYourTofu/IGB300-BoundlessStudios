using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour
{
    [SerializeField] private GameObject Microgame;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Canvas Controlcanvas;
    private Vector2 currentPos;
    private Vector2 StartPos;

    private void Awake()
    {
        canvas = this.transform.parent.GetComponent<Canvas>();
        Controlcanvas = GameObject.FindGameObjectWithTag("Controller").GetComponent<Canvas>();
        StartPos = transform.localPosition;
        Controlcanvas.enabled = false;
    }

    public void DragHandler(BaseEventData data)
    {
        PointerEventData pointerData = (PointerEventData)data;

        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, pointerData.position, canvas.worldCamera, out position);

        transform.position = canvas.transform.TransformPoint(position);
    }

    public void OnDrop()
    {
        if (Vector2.Distance(transform.localPosition, StartPos) > 30f)
        {
            //do score/points or tally completed part of task

            Destroy(Microgame);
            Controlcanvas.enabled = true;
        }
    }
}
