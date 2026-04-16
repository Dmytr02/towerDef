using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using TouchPhase = UnityEngine.TouchPhase;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

[RequireComponent(typeof(RectTransform))]
public class MultiTouchEventTrigger : MonoBehaviour, IComparable<MultiTouchEventTrigger>
{
    [Serializable] public class PointerEvent : UnityEngine.Events.UnityEvent<PointerEventData> { }

    public PointerEvent OnPointerDownEvent;
    public PointerEvent OnPointerUpEvent;
    public PointerEvent OnPointerEnterEvent;
    public PointerEvent OnPointerExitEvent;
    public PointerEvent OnDragEvent;
    public PointerEvent OnClickEvent;

    public int order = 0;

    private RectTransform rectTransform;
    private float clickThreshold = 40;
    private float clickTimeThreshold = 0.5f;

    private HashSet<int> activePointers = new HashSet<int>();
    private Dictionary<int, PointerEventData> pointerEventDatas = new Dictionary<int, PointerEventData>();
    private Dictionary<int, (Vector2, float)> pressPositions = new Dictionary<int, (Vector2, float)>();

    public static Dictionary<int, MultiTouchEventTrigger> fingerToObject = new Dictionary<int, MultiTouchEventTrigger>();

    public void DelateMultyTouch(MultiTouchEventTrigger eventTrigger)
    {
        int i = fingerToObject.FirstOrDefault(kvp => kvp.Value == eventTrigger).Key;
        fingerToObject.Remove(i);
        pressPositions.Remove(i);
        pointerEventDatas.Remove(i);
        activePointers.Remove(i);
    }
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        MultiTouchEventManager.AddEvent(this);
    }

    private void OnDestroy()
    {
        MultiTouchEventManager.RemoveEvent(this);
    }

    public void OrderedUpdate()
    {
        if (!gameObject.activeInHierarchy && enabled) return;

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            int fingerId = touch.fingerId;
            Vector2 touchPos = touch.position;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, touchPos, null) && !fingerToObject.ContainsKey(fingerId))
                    {
                        fingerToObject[fingerId] = this;

                        activePointers.Add(fingerId);

                        PointerEventData ped = CreatePointerEvent(fingerId, touchPos);
                        pointerEventDatas[fingerId] = ped;
                        pressPositions[fingerId] = (touchPos, Time.time);

                        OnPointerDownEvent?.Invoke(ped);
                    }
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    if (fingerToObject.ContainsKey(fingerId) && fingerToObject[fingerId] == this)
                    {
                        PointerEventData ped = pointerEventDatas[fingerId];
                        ped.delta = touchPos - ped.position;
                        ped.position = touchPos;
                        OnDragEvent?.Invoke(ped);
                    }
                    break;

                case TouchPhase.Canceled:
                case TouchPhase.Ended:
                    if (fingerToObject.ContainsKey(fingerId) && fingerToObject[fingerId] == this)
                    {
                        PointerEventData ped = pointerEventDatas[fingerId];
                        ped.delta = touchPos - ped.position;
                        ped.position = touchPos;

                        OnPointerUpEvent?.Invoke(ped);

                        if (Vector2.Distance(pressPositions[fingerId].Item1, touchPos) <= clickThreshold && Time.time-pressPositions[fingerId].Item2 < clickTimeThreshold)
                            OnClickEvent?.Invoke(ped);
                        
                        fingerToObject.Remove(fingerId);

                        activePointers.Remove(fingerId); 
                        pointerEventDatas.Remove(fingerId);
                        pressPositions.Remove(fingerId); 
                    }
                    break;
            }
        }

#if UNITY_EDITOR
        if (Input.mousePresent)
        {
            Vector2 mousePos = Input.mousePosition;
            int mouseId = -1; 
            if (Input.GetMouseButtonDown(0))
            {
                if (!fingerToObject.ContainsKey(mouseId) && RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePos, null))
                {
                    fingerToObject[mouseId] = this;

                    activePointers.Add(mouseId);
                    PointerEventData ped = CreatePointerEvent(mouseId, mousePos);
                    pointerEventDatas[mouseId] = ped;
                    pressPositions[mouseId] = (mousePos, Time.time);

                    OnPointerDownEvent?.Invoke(ped);
                }
            }
            else if (Input.GetMouseButton(0) && fingerToObject.ContainsKey(mouseId) && fingerToObject[mouseId] == this)
            {
                PointerEventData ped = pointerEventDatas[mouseId];
                ped.delta = (Vector2)mousePos - ped.position;
                ped.position = mousePos;
                OnDragEvent?.Invoke(ped);
            }
            else if (Input.GetMouseButtonUp(0) && fingerToObject.ContainsKey(mouseId) && fingerToObject[mouseId] == this)
            {
                PointerEventData ped = pointerEventDatas[mouseId];
                ped.delta = (Vector2)mousePos - ped.position;
                ped.position = mousePos;

                OnPointerUpEvent?.Invoke(ped);

                //Debug.Log(Vector2.Distance(pressPositions[mouseId].Item1, mousePos) + " | " + (Time.time-pressPositions[mouseId].Item2));
                if (Vector2.Distance(pressPositions[mouseId].Item1, mousePos) <= clickThreshold && Time.time-pressPositions[mouseId].Item2 < clickTimeThreshold)
                    OnClickEvent?.Invoke(ped);
                

                fingerToObject.Remove(mouseId);

                activePointers.Remove(mouseId);
                pointerEventDatas.Remove(mouseId);
                pressPositions.Remove(mouseId);
            }
        }
#endif
    }

    private PointerEventData CreatePointerEvent(int pointerId, Vector2 pos)
    {
        return new PointerEventData(EventSystem.current)
        {
            pointerId = pointerId,
            position = pos,
            pressPosition = pos,
            delta = Vector2.zero
        };
    }


    public int CompareTo(MultiTouchEventTrigger other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;
        var orderComparison = order.CompareTo(other.order);
        if (orderComparison != 0) return orderComparison;
        var clickThresholdComparison = clickThreshold.CompareTo(other.clickThreshold);
        if (clickThresholdComparison != 0) return clickThresholdComparison;
        return clickTimeThreshold.CompareTo(other.clickTimeThreshold);
    }
}
