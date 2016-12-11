using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using flyyoufools;

public class InputScript : MonoBehaviour {
	public IObservable<Vector2> Movement { get; private set; }
	public IObservable<bool> Spacebar { get; private set; }
	private Animator playerAnimator;
	private void Awake() {
		Movement = this.UpdateAsObservable()
		//.Throttle(System.TimeSpan.FromSeconds(1))
		.Where(_ => {
			return !playerAnimator.GetBool("Run");
		})
		.Select(_ => {
			var x = Input.GetAxis("Horizontal");
			var y = Input.GetAxis("Vertical");
			return new Vector2(x, -y).normalized;
		})
		.Where(v => v != Vector2.zero)
		.Do(_ => {
			playerAnimator.SetBool("Run", true);
			var x = Input.GetAxis("Horizontal");
			var y = Input.GetAxis("Vertical");
			if (x > 0) {
				playerAnimator.SetTrigger("RunRight");
			}
			if (x < 0) {
				playerAnimator.SetTrigger("RunLeft");
			}
			if (y > 0) {
				playerAnimator.SetTrigger("RunTop");
			}
			if (y < 0) {
				playerAnimator.SetTrigger("RunDown");
			}
		});
		Spacebar = this.UpdateAsObservable()
		.Select(_ => {
			//Debug.Log(Input.GetKeyDown("space"));
			return Input.GetKeyDown("space");
		});
	}

	void Start () {
		playerAnimator = GameObject.FindWithTag("Player").GetComponent<Animator>();
	}
}
