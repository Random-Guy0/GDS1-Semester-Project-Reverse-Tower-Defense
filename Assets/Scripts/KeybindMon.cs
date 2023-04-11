using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeybindMon : MonoBehaviour
{
    public Button Normal;
    public Button Speedy;
    public Button Tanky;
    public Button Path;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("A key was pressed");
            // simulate a click on the button
            ExecuteEvents.Execute(Normal.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("A key was pressed");
            // simulate a click on the button
            ExecuteEvents.Execute(Speedy.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("A key was pressed");
            // simulate a click on the button
            ExecuteEvents.Execute(Tanky.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("A key was pressed");
            // simulate a click on the button
            ExecuteEvents.Execute(Path.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
        }
    }
}