using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Move : MonoBehaviour {

	[ClientRpc]
	public void RpcMove(Vector3 _position) {
		Debug.Log ("Rpc Move");
		transform.position = _position;
	}
}
