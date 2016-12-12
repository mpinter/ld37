using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using flyyoufools;

public class InputScript : MonoBehaviour {
	public IObservable<Vector2> Movement { get; private set; }
	public IObservable<bool> Spacebar { get; private set; }
	public IObservable<bool> SpecialSpacebar { get; private set; }
	public IObservable<bool> Escape { get; private set; }
	public IObservable<bool> CtrlDown { get; private set; }
	public IObservable<bool> CtrlUp { get; private set; }
	private Animator playerAnimator;
	private MasterScript masterScript;
	private void Awake() {
		Movement = this.UpdateAsObservable()
		//.Throttle(System.TimeSpan.FromSeconds(1))
		.Where(_ => {
			return !playerAnimator.GetBool("Run") && !masterScript.blockInput;
		})
		.Select(_ => {
			var up = Input.GetKeyDown(KeyCode.UpArrow);
			var down = Input.GetKeyDown(KeyCode.DownArrow);
			var left = Input.GetKeyDown(KeyCode.LeftArrow); 
			var right = Input.GetKeyDown(KeyCode.RightArrow);

			float x = left ? -1 : (right ? 1 : 0);
			float y = up ? 1 : (down ? -1 : 0);
			
			return new Vector2(x, -y).normalized;
		})
		.Where(v => v != Vector2.zero)
		.Do(_ => {
			playerAnimator.SetBool("Run", true);

			var up = Input.GetKeyDown(KeyCode.UpArrow);
			var down = Input.GetKeyDown(KeyCode.DownArrow);
			var left = Input.GetKeyDown(KeyCode.LeftArrow); 
			var right = Input.GetKeyDown(KeyCode.RightArrow);

			float x = left ? -1 : (right ? 1 : 0);
			float y = up ? 1 : (down ? -1 : 0);

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
		.Where(_ => {
			return masterScript.startup || (!playerAnimator.GetBool("Run") && !masterScript.blockInput);
		})
		.Select(_ => {
			return Input.GetKeyDown("space");
		});

		SpecialSpacebar = this.UpdateAsObservable()
		.Where(_ => {
			return masterScript.startup || (!playerAnimator.GetBool("Run") && masterScript.blockInput);
		})
		.Select(_ => {
			return Input.GetKeyDown("space");
		});
		
		Escape = this.UpdateAsObservable()
		.Select(_ => {
			//Debug.Log(Input.GetKeyDown("escape"));
			return Input.GetKeyDown("escape");
		});

		CtrlDown = this.UpdateAsObservable()
		.Where(_ => {
			return !masterScript.blockInput;
		})
		.Select(_ => {
			//Debug.Log(Input.GetKeyDown("escape"));
			return Input.GetKeyDown(KeyCode.LeftControl);
		});
		CtrlUp = this.UpdateAsObservable()
		.Select(_ => {
			//Debug.Log(Input.GetKeyDown("escape"));
			return Input.GetKeyUp(KeyCode.LeftControl);
		});

	}

	void Start () {
		playerAnimator = GameObject.FindWithTag("Player").GetComponent<Animator>();
		masterScript = GameObject.FindWithTag("Master").GetComponent<MasterScript>();
	}
}
