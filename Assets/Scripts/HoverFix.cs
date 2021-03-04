using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverFix : MonoBehaviour
{

    public void OnHoverEnter(Transform t)
    {
        ExecuteEvents.Execute(t.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerEnterHandler);
    }

    public void OnHoverExit(Transform t)
    {
        ExecuteEvents.Execute(t.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerExitHandler);
    }

}
