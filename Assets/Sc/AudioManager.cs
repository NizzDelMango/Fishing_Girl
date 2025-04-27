using UnityEngine;
using UnityEngine.Audio;

public enum EAudioMixerType { Master, BGM, SFX }
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioMixer audioMixer;

    private bool[] isMute = new bool[3];
    private float[] audioVolumes = new float[3];
    private void Awake()
    {
        Instance = this;
    }

    public void SetAudioVolume(EAudioMixerType audioMixerType, float volume)
    {
        // ����� �ͼ��� ���� -80 ~ 0�����̱� ������ 0.0001 ~ 1�� Log10 * 20�� �Ѵ�.
        audioMixer.SetFloat(audioMixerType.ToString(), Mathf.Log10(volume) * 20);
    }

    public void SetAudioMute(EAudioMixerType audioMixerType)
    {
        int type = (int)audioMixerType;
        if (!isMute[type]) // ��Ʈ 
        {
            isMute[type] = true;
            audioMixer.GetFloat(audioMixerType.ToString(), out float curVolume);
            audioVolumes[type] = curVolume;
            SetAudioVolume(audioMixerType, 0.001f);
        }
        else
        {
            isMute[type] = false;
            SetAudioVolume(audioMixerType, audioVolumes[type]);
        }
    }
}


