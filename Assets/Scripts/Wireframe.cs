using UnityEngine;
using System.Collections;

public class Wireframe : MonoBehaviour {

	void OnPreRender() {
		GL.wireframe = true;
	}
	void OnPostRender() {
		GL.wireframe = false;
	}
}
