using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WaitViewer : MonoBehaviour {
  Animator anim;
  [SerializeField] GameObject blocker;

  void Start() {
    anim = GetComponent<Animator>();
    blocker.SetActive(false);
  }

  public void Done() {
    Time.timeScale = 1f;
    blocker.SetActive(false);
  }

  public void ViewGetReady() {
    anim.SetTrigger("Play");
    blocker.SetActive(true);
    Time.timeScale = 0f;
  }
}