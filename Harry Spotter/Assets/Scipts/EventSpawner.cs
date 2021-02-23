using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.Utilities;
using System.Collections.Generic;
using static GameNetwork;
using Mapbox.Examples;
using System.Collections;

public class EventSpawner : MonoBehaviour
{
	[SerializeField]
	AbstractMap _map;

	[SerializeField]
	[Geocode]
	string[] _locationStrings;

	[SerializeField]
	Vector2d[] _locations;

	[SerializeField]
	float _spawnScale = 100f;

	[SerializeField]
	GameObject _markerPrefab;

	List<GameObject> _spawnedObjects;



	[SerializeField] private Material[] _markerMaterials;
	[SerializeField] private GameNetwork _gameNetwork;
	private bool _areThereEvents = false;
	[SerializeField] private LocationStatus _locationStatus;
	[SerializeField] private bool _pcDebugMod;
	[SerializeField] private LocalUser _localUser;


	[SerializeField] private int _respawnEventRange = 200;
	private string _originalEventSpawnPoseLat;
	private string _originalEventSpawnPoseLon;

	void Start()
	{

		_areThereEvents = false;
		//Make the call to backend to get event
		if (_pcDebugMod == true)
		{
			GameNetworkEvent _event = new GameNetworkEvent("ff81b218-c1d3-11ea-81e3-f3d73c97a2c0", "-0.0206224", "51.4978606");
			StartCoroutine(_gameNetwork.GetEvent("https://harryspotter.eu.ngrok.io/getEvents", _event.Serialize().ToString(), GetEventResults));
		} else
		{
			StartCoroutine(InitializeEvent());
		}

		//SpawnEvent();
	}

	private IEnumerator InitializeEvent()
	{
		yield return new WaitUntil(() => _locationStatus.GetLocationLat() != "0" && _locationStatus.GetLocationLon() != "0");
		GameNetworkEvent _event = new GameNetworkEvent(_localUser.userID, _locationStatus.GetLocationLat(), _locationStatus.GetLocationLon());
		StartCoroutine(_gameNetwork.GetEvent("https://harryspotter.eu.ngrok.io/getEvents", _event.Serialize().ToString(), GetEventResults));
		_originalEventSpawnPoseLat = _locationStatus.GetLocationLat();
		_originalEventSpawnPoseLon = _locationStatus.GetLocationLon();

	}

	//For Internal Testing only
	public void SpawnEvent()
	{
		//Spawn Event
		_locations = new Vector2d[_locationStrings.Length];
		_spawnedObjects = new List<GameObject>();
		for (int i = 0; i < _locationStrings.Length; i++)
		{
			var locationString = _locationStrings[i];
			//print(_locationStrings[i]);
			_locations[i] = Conversions.StringToLatLon(locationString);
			print(_locations[i]);
			var instance = Instantiate(_markerPrefab);
			instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
			instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
			_spawnedObjects.Add(instance);
		}
		_areThereEvents = true;
	}

	//Get Event CallBack method
	public void GetEventResults(EventCallBack eventCall)
	{

		foreach (var i in eventCall.events)
		{
			print("Event Id: " + i.event_id + " Pos Lat: " + i.poi_lat + " Pos Lon: " + i.poi_lon + " Pos ype: " + i.poi_type + " Defender ID: " + i.defenderId + " House Number: " + i.mayor_house + " Prob_poi: " + i.prob_poi);
			//locationString.Add(i.poi_lat.ToString() + ", " + i.poi_lon.ToString());
			//_locations = new Vector2d[i.poi_lat, i.poi_lon];
		}

		//Spawn Events
		_locations = new Vector2d[eventCall.events.Count];
		_spawnedObjects = new List<GameObject>();
		for (int i = 0; i < eventCall.events.Count; i++)
		{
			var locationString = eventCall.events[i].poi_lat + ", " + eventCall.events[i].poi_lon;
			//print("Location: " + locationString);
			_locations[i] = Conversions.StringToLatLon(locationString);
			//print("After: " + _locations[i]);
			var instance = Instantiate(_markerPrefab);
			instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
			instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
			//setting the name of event gameobjet to the event id string
			instance.transform.name = eventCall.events[i].event_id;
			//Setting the colour of marker to the corresponding house
			// 0:No Mayor, 1: Hufflepuff, 2: Ravenclaw, 3: Griffindor , 4: Slytherin
			//eventCall.events[i].mayor_house
			instance.GetComponent<MeshRenderer>().material = _markerMaterials[eventCall.events[i].mayor_house];
			_spawnedObjects.Add(instance);
		}
		_areThereEvents = true;


	}

	//public void 
	private void Update()
	{
		if (_areThereEvents == true)
		{
			int count = _spawnedObjects.Count;
			for (int i = 0; i < count; i++)
			{
				var spawnedObject = _spawnedObjects[i];
				var location = _locations[i];
				spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
				spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
			}
			DistanceChecker();
		}


	}


	

	//Event Respawn
	public void DistanceChecker()
	{
		var userLocation = new GeoCoordinatePortable.GeoCoordinate(double.Parse(_locationStatus.GetLocationLon()), double.Parse(_locationStatus.GetLocationLat()));
		var eventLocation = new GeoCoordinatePortable.GeoCoordinate(double.Parse(_originalEventSpawnPoseLon), double.Parse(_originalEventSpawnPoseLat));
		//Debug.Log(userLocation);
		//Debug.Log(eventLocation);
		var distance = userLocation.GetDistanceTo(eventLocation);

		//Debug.Log(distance);

		if (Mathf.RoundToInt((float)distance) > _respawnEventRange)
        {
			DeleteSpawnObject();
			SpawnEvents();
        }
	}

	private void SpawnEvents()
    {
		GameNetworkEvent _event = new GameNetworkEvent(_localUser.userID, _locationStatus.GetLocationLat(), _locationStatus.GetLocationLon());
		StartCoroutine(_gameNetwork.GetEvent("https://harryspotter.eu.ngrok.io/getEvents", _event.Serialize().ToString(), GetEventResults));
		_originalEventSpawnPoseLat = _locationStatus.GetLocationLat();
		_originalEventSpawnPoseLon = _locationStatus.GetLocationLon();
	}

	//Delete Spawn Object
	public void DeleteSpawnObject()
    {
		foreach(var i in _spawnedObjects)
        {
			i.Destroy();
        }
    }

	

}
