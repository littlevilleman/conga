using Core;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "ParticipantConfig", menuName = "Conga/ParticipantConfig", order = 1)]
    public class ParticipantConfig : ScriptableObject, IParticipantFactory
    {
        [SerializeField] private string name;
        [SerializeField] private Sprite[] sprites;

        public string Name => name;
        public Sprite[] Sprites => sprites;

        public IParticipant Build(Vector2Int location)
        {
            IParticipant participant = new Participant(this, location);
            return participant;
        }
    }
}