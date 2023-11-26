using Core;
using UnityEngine;

[CreateAssetMenu(fileName = "RythmConfig", menuName = "Conga/RythmConfig", order = 1)]
public class RythmConfig : ScriptableObject
{
    [SerializeField] private AnimationCurve cadenceCurve;

    public AnimationCurve CadenceCurve => cadenceCurve;
    
    public IRythm Build()
    {
        IRythm participant = new Rythm(this);
        return participant;
    }
}
