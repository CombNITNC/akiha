using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WaitViewer : MonoBehaviour {
  Animator anim;
  [SerializeField] GameObject blocker = null;

  public delegate void Callback();

  Callback func;

  void Start() {
    anim = GetComponent<Animator>();
    blocker.SetActive(false);
  }

  public void Done() {
    if (func != null) {
      func();
    }
    Time.timeScale = 1f;
    blocker.SetActive(false);
  }

  public void ViewGetReady(Callback callback = null) {
    func = callback;
    anim.SetTrigger("Play");
    blocker.SetActive(true);
    Time.timeScale = 0f;
  }
}