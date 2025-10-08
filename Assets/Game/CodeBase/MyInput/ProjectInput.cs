using UnityEngine;
using UnityEngine.EventSystems;

namespace MyInput
{
    public class ProjectInput
    {
        public Vector3? GetPointerPosition()
        {
            if (!GameState.InputEnabled)
                return null;

            if (Input.touches.Length > 0)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                    return null;

                return Camera.main.ScreenToWorldPoint(Input.touches[0].position);
            }

            if (Input.GetMouseButton(0))
            {
                if (EventSystem.current.IsPointerOverGameObject()) 
                    return null;

                return Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            return null;
        }
    }
    
    public static class GameState
    {
        public static bool InputEnabled = true;
    }

}