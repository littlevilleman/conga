using UnityEngine;

namespace Client
{
    public class InputManager : MonoBehaviour
    {
        public Vector2Int GetDirectionInput()
        {
            if (Input.GetKey(KeyCode.W))
                return Vector2Int.up;

            if (Input.GetKey(KeyCode.D))
                return Vector2Int.right;

            if (Input.GetKey(KeyCode.A))
                return Vector2Int.left;

            if (Input.GetKey(KeyCode.S))
                return Vector2Int.down;

            return Vector2Int.zero;
        }
    }
}