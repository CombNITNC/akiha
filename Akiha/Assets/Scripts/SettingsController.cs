using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class SettingsController : MonoBehaviour {
  int _controlMode = 0;
  float _sensivity = 1f;
  float _volume = 1f;

  [SerializeField] Dropdown controlModeSelector = null;
  [SerializeField] Slider sensivitySlider = null;
  [SerializeField] Slider volumeSlider = null;

  Canvas canvas;
  GameController gameController;

  void Start() {
    Init();

    canvas = GetComponent<Canvas>();
    var gameControllerObj = GameObject.FindWithTag("GameController");
    if (gameControllerObj != null) {
      gameController = gameControllerObj.GetComponent<GameController>();
    }
  }

  void OnEnable() {
    Init();
    controlModeSelector.value = _controlMode;
    sensivitySlider.normalizedValue = _sensivity;
    volumeSlider.normalizedValue = _volume;
  }

  void Init() {
    _controlMode = PlayerPrefs.GetInt("ControlMode", 0);
    _sensivity = PlayerPrefs.GetFloat("Sensivity", 1f);
    _volume = PlayerPrefs.GetFloat("Volume", 1f);
  }

  IEnumerator UnloadThisScene() {
    yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    yield break;
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

    if (gameController != null) {
      gameController.NotifyChangedSettings();
    }
  }

  public void Open() {
    canvas.enabled = true;
  }

  public void Close() {
    canvas.enabled = false;
  }
}