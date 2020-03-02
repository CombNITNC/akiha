using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {
  public void NewGame() {
    GameObject.FindWithTag("God").GetComponent<GameGod>().ExecuteAllStory();
  }

  public void StageSelect() {
    SceneManager.LoadScene("StageSelect");
  }
}