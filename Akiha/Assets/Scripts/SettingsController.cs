using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class SettingsController : MonoBehaviour {
  int _controlMode = 0;
  float _sensivity = 1f;
  float _volume = 1f;

  [SerializeField] Dropdown controlModeSelector;
  [SerializeField] Slider sensivitySlider;
  [SerializeField] Slider volumeSlider;
  [SerializeField] Button saveButton;

  void Start() {
    _controlMode = PlayerPrefs.GetInt("ControlMode", 0);
    _sensivity = PlayerPrefs.GetFloat("Sensivity", 1f);
    _volume = PlayerPrefs.GetFloat("Volume", 1f);
  }

  IEnumerator UnloadThisScene() {
    yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    yield break;
  }

  IEnumerator SaveButtonAnimate() {
    yield return null;
  }

  public void ApplyValue() {
    _controlMode = controlModeSelector.value;
    _sensivity = sensivitySlider.normalizedValue;
    _volume = volumeSlider.normalizedValue;
  }

  public void Save() {
    PlayerPrefs.SetInt("ControlMode", _controlMode);
    PlayerPrefs.SetFloat("Sensivity", _sensivity);
    PlayerPrefs.SetFloat("Volume", _volume);

    StartCoroutine(SaveButtonAnimate());
  }

  public void Open() {
    GetComponent<Canvas>().enabled = true;
  }

  public void Close() {
    GetComponent<Canvas>().enabled = false;
  }
}