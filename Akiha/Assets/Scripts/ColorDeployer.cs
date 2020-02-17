using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ColorDeployer : MonoBehaviour, IHasColor, ICollideWithColor {
	[SerializeField] CMYK delpoyingColor = CMYK.Red;
	AudioSource source;
	[SerializeField] AudioClip depolySound = null;

	void Start() {
		source = GetComponent<AudioSource>();
		source.clip = depolySound;
		source.playOnAwake = false;
	}

	public CMYK GetColor() {
		return delpoyingColor;
	}

	public void CollideWith(CMYK color, PlayerController player) {
		source.Play();
	}
}