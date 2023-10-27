using System.IO;
using UnityEngine;

namespace HyperCasualPack.ScriptableObjects
{
	public class SaveManager : MonoBehaviour
	{
		[SerializeField] SaveablesList _itemListToSave;

		SaveData _saveData = new SaveData();
		static string savePath;

        void Awake()
        {
			savePath = Path.Combine(Application.persistentDataPath, "gamedata.json");
        }

        void Start()
		{
			RestoreAll();
		}

		void OnApplicationPause(bool pauseStatus)
		{
			if (pauseStatus)
			{
				SaveAll();	
			}
		}

		void OnApplicationQuit()
		{
			SaveAll();
		}

		void RestoreAll()
		{
			if (File.Exists(savePath))
			{
				SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(savePath));
				
				foreach (SaveableSO saveableSO in _itemListToSave.Saveables)
				{
					if (saveData.saves.ContainsKey(saveableSO.GetGuid))
					{
                        saveableSO.RestoreState(saveData.saves[saveableSO.GetGuid]);
                    }
                }
			}
			else
			{
				foreach (SaveableSO saveableSO in _itemListToSave.Saveables)
				{
					saveableSO.RestoreState(saveableSO.GetDefaultValue);
				}
			}
		}

		void SaveAll()
		{
			foreach (SaveableSO saveableSO in _itemListToSave.Saveables)
			{
				if (_saveData.saves.ContainsKey(saveableSO.GetGuid))
				{
					_saveData.saves[saveableSO.GetGuid] = (int)saveableSO.CaptureState();
				}
				else
				{
					_saveData.saves.Add(saveableSO.GetGuid, (int)saveableSO.CaptureState());
				}
			}
			string convertedSaveData = JsonUtility.ToJson(_saveData);
			File.WriteAllText(savePath, convertedSaveData);
		}
	}

}
