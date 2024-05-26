using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

public class ARMultipleObjectController : MonoBehaviour
{
    [SerializeField]
    ARRaycastManager arRaycastManager;

    [SerializeField]
    Camera arCamera;

    GameObject selectedPrefab;
    ARObject selectedObject;

    private static List<ARRaycastHit> arHits = new List<ARRaycastHit>();
    private static RaycastHit physicsHit;

    private float scale = 1.0f;
    private float angle = 0.0f;

    public void SetSelectedPrefab(GameObject selectedPrefab)
    {
        this.selectedPrefab = selectedPrefab;
    }

    public void UpdateScale(float sliderValue)
    {
        scale = sliderValue;
        if (selectedObject)
        {
            selectedObject.transform.localScale = Vector3.one * scale;
        }
    }

    public void UpdateRotation(float sliderValue)
    {
        angle = sliderValue;
        if (selectedObject)
        {
            selectedObject.transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        }
    }

    private void Awake()
    {
        selectedPrefab = arRaycastManager.raycastPrefab;
    }

    // 프레임마다 업데이트
    void Update()
    {
        if (Input.touchCount == 0)
        {
            return;
        }

        Touch touch = Input.GetTouch(0);
        Vector2 touchPosition = touch.position;

        if (IsPointOverUIObject(touchPosition))
        {
            return ;
        }

        // 객체가 선택되면 Selected
        if (touch.phase == TouchPhase.Began)    // 터치가 처음 발생한 순간만 적용
        {
            SelectARObject(touchPosition);
        }
        else if (touch.phase == TouchPhase.Ended) // 터치에서 손이 떨어졌을 때
        {
            if (selectedObject)
            {
                selectedObject.Selected = false;
            }
        }
        
        SelectARPlane(touchPosition);   // ARObject가 선택되지 않았다면(=빈 평면이 선택되면) arRaycast 사용(새 객체 생성)
    }

    private bool SelectARObject(Vector2 touchPosition)
    {
        Ray ray = arCamera.ScreenPointToRay(touchPosition);
        if (Physics.Raycast(ray, out physicsHit))   // 물리적인 오브젝트와 부딪혔을 때
        {
            selectedObject = physicsHit.transform.GetComponent<ARObject>();
            if (selectedObject)
            {
                selectedObject.Selected = true;
                return true;
            }
        }
        return false;
    }

    private void SelectARPlane(Vector2 touchPosition)
    {
        if (arRaycastManager.Raycast(touchPosition, arHits, TrackableType.PlaneWithinPolygon))   // TouchPhase.Began : 처음 터치가 된 순간에만 Raycast를 쏘고 그 위치에 객체를 생성
        {
            Pose hitPose = arHits[0].pose;

            if (!selectedObject)
            {
                var newARObj = Instantiate(selectedPrefab, hitPose.position, hitPose.rotation);
                selectedObject = newARObj.AddComponent<ARObject>();
                selectedObject.transform.localScale = Vector3.one * scale;    // 객체 생성시 미리 지정된 scale로 생성
                selectedObject.transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));   // 객체 생성시 미리 지정된 rotation으로 생성
            }
            else if (selectedObject.Selected)
            {
                selectedObject.transform.position = hitPose.position;
                selectedObject.transform.rotation = hitPose.rotation;
            }
        }
    }

    // UI 클릭시 오브젝트 간섭 X
    // UI가 arRaycast보다 우선순위를 높게 가짐
    bool IsPointOverUIObject(Vector2 pos)
    {
        PointerEventData eventDataCurPosition = new PointerEventData(EventSystem.current);
        eventDataCurPosition.position = pos;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurPosition, results);
        return results.Count > 0;
    }
}
