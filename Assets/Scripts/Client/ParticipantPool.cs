using UnityEngine;

namespace Client
{
    public class ParticipantPool : Pool<ParticipantBehaviour>
    {
        public override ParticipantBehaviour CreateElement()
        {
            ParticipantBehaviour element = null;

            if (elements.Count < limitCount)
            {
                element = Instantiate(prefab, transform);
                element.gameObject.SetActive(false);
                elements.Add(element);
            }
            else
                Debug.LogError("Participants reached limit");

            return element;
        }
    }
}