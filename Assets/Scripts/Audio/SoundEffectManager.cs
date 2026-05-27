using UnityEngine;

[RequireComponent(typeof(SoundEffectLibrary))]
public class SoundEffectManager : MonoBehaviour {
	private static SoundEffectManager Instance;
	private SoundEffectLibrary soundEffectLibrary;
	private AudioSource globalSource;

	[Range(0, 1)]
	public float globalVolume = 0.5f;

	private void Awake() {
		if (Instance == null) {
			Instance = this;
			soundEffectLibrary = GetComponent<SoundEffectLibrary>();

			globalSource = gameObject.AddComponent<AudioSource>();
			globalSource.playOnAwake = false;
		} else {
			Destroy(gameObject);
		}
	}

	public static void Play(string soundName) {
		if (Instance == null || Instance.soundEffectLibrary == null) return;

		AudioClip clip = Instance.soundEffectLibrary.GetRandomClip(soundName);
		if (clip != null) {
			Instance.globalSource.PlayOneShot(clip, Instance.globalVolume);
		}
	}

	public static void Play(string soundName, Vector3 position) {
		if (Instance == null || Instance.soundEffectLibrary == null) return;

		AudioClip clip = Instance.soundEffectLibrary.GetRandomClip(soundName);
		if (clip != null) {
			AudioSource.PlayClipAtPoint(clip, position, Instance.globalVolume);
		}
	}
}