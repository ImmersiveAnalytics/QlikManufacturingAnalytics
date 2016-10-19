using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using SimpleJSON;
using System.Text.RegularExpressions;

namespace ManufacturingAnalytics
{
	public class SenseConnector : MonoBehaviour
	{
		public GameObject[] Tracks;
		public GameObject plantCanvas;
		public GameObject conveyor1, conveyor2, conveyor3, conveyor4;

		private GameObject widget;
		private bool newWidget = false;
		private Text[] objectValues;
		private Text[] objectStatus;

		private WebSocket _ws;
		private bool updating = true;
		private bool takingAction = false;

		IEnumerator Start () {
			WebSocket w = new WebSocket(new Uri("ws://localhost:44444/socket.io/?EIO=3&transport=websocket"));
			yield return StartCoroutine(w.Connect());

			objectStatus = plantCanvas.GetComponentsInChildren<Text> ();
			_ws = w;

			while (true)
			{

//				Debug.Log(Input.GetMouseButtonDown(0));
				//Debug.Log(Input.GetButton("Fire2"));
				if (Input.GetButton ("Fire1") && Input.GetButton ("Fire2")) {
					if (!takingAction) {
						takingAction = true;
						Debug.Log ("Action");
					}
				} else {
					takingAction = false;
				}

				string msg = w.RecvString();

				if ((msg != null) && (updating))
				{
					// Debug.Log ("Received: "+msg);
					string pattern = "^\\d+";
                    Regex rgx = new Regex(pattern);
                    string j = rgx.Replace(msg,"");
					//Debug.Log ("json msg: "+j);

					if(j.Length > 0){
						var jData = JSON.Parse (j);

						Debug.Log("readvalue: " + jData[1]["readvalue"].Value + " statuscode: " + jData[1]["statuscode"].Value + " sensorposition: " + jData[1]["sensorposition"].AsFloat + " conveyorsection: " + jData[1]["conveyorsection"].AsInt);

						// Delete existing widget
						if (newWidget && jData [1] ["sensorposition"].AsInt == 1) {
							Destroy (widget);
							Array.Clear (objectValues, 0, objectValues.Length);
							newWidget = false;
						}
						// Create new widget
						if (!newWidget && jData [1] ["sensorposition"].AsFloat > 0) {
							widget = Instantiate (Resources.Load ("Widget")) as GameObject;
							float pos = (jData [1] ["sensorposition"].AsFloat - 1f) % 4f * 5f;
							widget.transform.position = new Vector3 (0, 0, pos);
							int track = (int)Math.Floor ((decimal)((jData [1] ["sensorposition"].AsFloat - 1f) / 4f));
							widget.transform.SetParent (Tracks [track].transform, false);

							objectValues = widget.GetComponentsInChildren<Text> ();
							newWidget = true;
						}
						if (newWidget) {
							foreach (Text objText in objectValues) {
								objText.text = jData [1] ["readvalue"].Value;
							}
						}

						// Set Status Code
						foreach (Text objText in objectStatus) {
							objText.text = jData[1]["statuscode"].Value;
						}
						Color col;
						switch(jData[1]["statuscode"].Value){
							case "OK":
								col = Color.blue;
								break;
							case "Med":
								col = Color.yellow;
								break;
							case "High":
								col = new Color(255, 128, 0, 255);
								break;
							case "Fail":
								col = Color.grey;
								break;
							case "Critical":
								col = Color.red;
								break;
							default:
								col = Color.white;
								break;
						}
						foreach (Text objText in objectStatus) {
							objText.material.color = col;
						}

						// required to keep ws cnx alive
						w.SendString("unity");
					}
				}
				if (w.error != null)
				{
					Debug.LogError ("Error: "+w.error);
					break;
				}
				yield return 0;
			}
			Debug.Log ("closing ws");
			w.Close();
		}

		public void Test(){
			Debug.Log ("Clicked");
			updating = !updating;
			conveyor1.GetComponent<ConveyorScript>().conveyorOn = !conveyor1.GetComponent<ConveyorScript>().conveyorOn;
			conveyor2.GetComponent<ConveyorScript>().conveyorOn = !conveyor2.GetComponent<ConveyorScript>().conveyorOn;
			conveyor3.GetComponent<ConveyorScript>().conveyorOn = !conveyor3.GetComponent<ConveyorScript>().conveyorOn;
			conveyor4.GetComponent<ConveyorScript>().conveyorOn = !conveyor4.GetComponent<ConveyorScript>().conveyorOn;
			_ws.SendString ("togglePower");
		}

	}
}