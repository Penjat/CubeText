using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENTRY{
	BOTTOM,
	TOP,
	LEFT,
	RIGHT,
	RANDOM
}
public class Cube : MonoBehaviour {

	public Animator _animator;
	public Rigidbody _rigidbody;
	private bool _isPaused;
	private float _timer;

	void Update(){
		if(_isPaused){
			_timer -= Time.deltaTime;
			if(_timer <=0){
				_isPaused = false;

				_animator.speed = 1.0f;
			}
		}
	}
	public void SetRigidbody(bool isOn){
		_rigidbody.isKinematic = isOn;
	}
	public Rigidbody GetRigidbody(){
		return _rigidbody;
	}
	public void SetEntryType(ENTRY entry){
		switch(entry){

			case ENTRY.TOP:
				_animator.Play("enter_top");
				break;

			case ENTRY.BOTTOM:
				_animator.Play("enter_bottom");
				break;

			case ENTRY.LEFT:
				_animator.Play("enter_left");
					break;

			case ENTRY.RIGHT:
				_animator.Play("enter_right");
				break;
			case ENTRY.RANDOM:
				int r = Random.Range(0,4);
				SetEntryType((ENTRY)r);
				break;

		}
	}
	public void SetDelay(float delay){
		_timer = delay;
		_isPaused = true;

		_animator.speed = 0.0f;
	}
	public void SetOn(bool b){
		gameObject.SetActive(b);
	}
	public void SetAnimationSpeed(float speed){
		_animator.speed = speed;
	}
}
