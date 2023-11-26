using Config;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class ConfigManager : MonoBehaviour
    {
        [SerializeField] private RythmConfig rythmConfig;
        [SerializeField] private List<ParticipantConfig> participantsConfig;
        public List<ParticipantConfig> Participants => participantsConfig;
        public RythmConfig Rythm => rythmConfig;
    }
}