using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class OptionsView : View
    {
        [SerializeField] private Toggle allowMusicToggle;
        [SerializeField] private Toggle allowSoundToggle;
        [SerializeField] private Slider dificultySlider;
        [SerializeField] private MenuButton confirmButton;

        private void OnEnable()
        {
            allowMusicToggle.onValueChanged.AddListener(OnMuteMusicChanged);
            allowSoundToggle.onValueChanged.AddListener(OnMuteSoundChanged);
            dificultySlider.onValueChanged.AddListener(OnDifficultyChanged);
            confirmButton.Button.onClick.AddListener(OnConfirmOptions);

            confirmButton.Display(.5f);
        }

        private void OnConfirmOptions()
        {
            EventBus.Send(new EventBackToMenu());
        }

        private void OnDifficultyChanged(float dificulty)
        {
            EventBus.Send(new EventChangeDifficulty { difficulty = (int) dificulty });
        }

        private void OnMuteMusicChanged(bool allowMusic)
        {
            EventBus.Send(new EventMuteMusic { mute = !allowMusic });
        }

        private void OnMuteSoundChanged(bool allowSound)
        {
            EventBus.Send(new EventMuteSound { mute = !allowSound});
        }

        private void OnDisable()
        {
            allowMusicToggle.onValueChanged.RemoveListener(OnMuteMusicChanged);
            allowSoundToggle.onValueChanged.RemoveListener(OnMuteSoundChanged);
            dificultySlider.onValueChanged.RemoveListener(OnDifficultyChanged);
            confirmButton.Button.onClick.RemoveListener(OnConfirmOptions);

            confirmButton.Hide();
        }
    }
}
