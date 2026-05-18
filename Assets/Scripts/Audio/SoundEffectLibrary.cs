using System.Collections.Generic;
using UnityEngine;

public class SoundEffectLibrary : MonoBehaviour {
	[SerializeField] private SoundEffectGroup[] soundEffectGroups;
	Dictionary<string, List<AudioClip>> soundDictionary;

	void InitializeDictionary() {
		if (soundDictionary != null) return;

		soundDictionary = new Dictionary<string, List<AudioClip>>();
		foreach (SoundEffectGroup group in soundEffectGroups) {
			if (string.IsNullOrEmpty(group.name)) continue;
			soundDictionary[group.name] = group.audioClips;
		}
	}

	public AudioClip GetRandomClip(string soundName) {
		InitializeDictionary();

		if (soundDictionary.TryGetValue(soundName, out List<AudioClip> clips)) {
			if (clips != null && clips.Count > 0) {
				return clips[Random.Range(0, clips.Count)];
			}
		}
		Debug.LogWarning($"SoundEffectLibrary: No one for '{soundName}'");

		return null;
	}
}

[System.Serializable]
public struct SoundEffectGroup {
	public string name;
	public List<AudioClip> audioClips;
}