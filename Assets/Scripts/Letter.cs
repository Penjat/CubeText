using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Letter {

	//class that uses an int to define which cube will be activated
	int _width;
	long _data;

	

	public Letter(int width, string data){
		_width = width;
		_data = Convert.ToInt64(data,2);
	}


	public long GetData(){
		return _data;
	}
	public int GetWidth(){
		return _width;
	}
}
