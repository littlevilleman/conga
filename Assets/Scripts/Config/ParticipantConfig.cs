using Core.Conga;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "ParticipantConfig", menuName = "Conga/ParticipantConfig", order = 1)]
    public class ParticipantConfig : ScriptableObject, IParticipantFactory
    {
        [SerializeField] private string name;
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private Color[] colors = new Color[] {};

        public string Name => name;
        public Sprite[] Sprites => sprites;
        public Color[] Colors => colors;

        public IParticipant Build(Vector2Int location)
        {
            return new Participant(this, location);
        }
    }
}