using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ExpandSupporter : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {
	[SerializeField] float holdTime = 1f;
	float count = 0;
	bool held = false;

	[SerializeField] UnityEvent onHold = new UnityEvent();

	void Update() {
		if (held) {
			count += Time.unscaledDeltaTime;
		} else {
			count = 0f;
		}
	}

	public void OnPointerDown(PointerEventData eventData) {
		held = true;
	}

	public void OnPointerUp(PointerEventData eventData) {
		held = false;
		if (count > holdTime) {
			onHold.Invoke();
		}
	}

	public void OnPointerExit(PointerEventData eventData) {
		held = false;
	}

	public void BringFront() {
		transform.SetAsLastSibling();
	}
}