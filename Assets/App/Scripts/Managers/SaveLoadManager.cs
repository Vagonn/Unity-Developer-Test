using DynamicBox.Domain;
using DynamicBox.Enums;
using DynamicBox.SaveManagement;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
	private SaveManager saveManager;
	private BoxParameters boxParameters = new BoxParameters (1, 2, 3, 4);

	void Start ()
	{
		saveManager = new SaveManager (StorageMethod.JSON);

		// Save (SaveType.UserPrefs, boxParameters);
		// Load (SaveType.UserPrefs);

		// Save (SaveType.Local, boxParameters);
		// Load (SaveType.Local);
	}

	public void Save (SaveType _saveType, BoxParameters _boxParameters)
	{
		switch (_saveType)
		{
			case SaveType.Local:
				saveManager.SaveToFile (boxParameters, "BoxParameters");

				Debug.Log ("Local: Saved UserPrefs data = " + boxParameters.depth);
				break;
			case SaveType.UserPrefs:
				string boxData = JsonUtility.ToJson (_boxParameters);
				PlayerPrefs.SetString ("boxData", boxData);

				Debug.Log ("UserPrefs: Saved UserPrefs data = " + boxData);
				break;
		}
	}

	public void Load (SaveType _saveType)
	{
		switch (_saveType)
		{
			case SaveType.Local:
				boxParameters = saveManager.LoadFromFile ("BoxParameters", new BoxParameters ());

				Debug.Log ("Local: Loaded UserPrefs data = " + boxParameters.depth);
				break;
			case SaveType.UserPrefs:
				string boxData = PlayerPrefs.GetString ("boxData");
				boxParameters = JsonUtility.FromJson<BoxParameters> (boxData);
				
				Debug.Log ("UserPrefs: Loaded UserPrefsData = " + boxParameters.height);
				break;
		}
	}
}