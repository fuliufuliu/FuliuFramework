using System;
using System.Linq;
using EasyExcel;
using UnityEngine;

public class ExampleLoadData : MonoBehaviour
{
	private readonly EEDataManager _eeDataManager = new EEDataManager();

	private void Awake()
	{
		// Load all data sheets like this when initializing your game
		_eeDataManager.Load();
		
		FindDataExample();
	}

	private void FindDataExample()
	{
		// ------------Uncomment below after importing example files------------
		/* 
		 
		// Get EasyExcelGenerated.KeyColumn with string-type key
		// You can specify a column in a sheet as key with Your_Column_Name:Key.
		var keyColumn = _eeDataManager.Get<EasyExcelGenerated.KeyColumn>("Brigand");
		Debug.Log(keyColumn.Description);
		
		// Get EasyExcelGenerated.MultiSheet01 with int-type key
		var multiSheet01 = _eeDataManager.Get<EasyExcelGenerated.MultiSheet01>(1001);
		Debug.Log(multiSheet01.Description);

		// Get EasyExcelGenerated.KeyColumn list
		List<EasyExcelGenerated.KeyColumn> list = _eeDataManager.GetList<EasyExcelGenerated.KeyColumn>();
		foreach (var data in list)
		{
			Debug.Log(data.Icon);
		}
		
		*/
	}

	#region Just for showing the data. You do not have to know these

	private void OnGUI()
	{
		gui(_eeDataManager);
	}

	// Just for test show, you do not have to know the details below.
	public static void gui(EEDataManager eeDataManager)
	{
		var index = 0;
		var labelBottom = 0;
		index++;
		GUI.Label(new Rect(10, labelBottom + index * 40, 800, 40), "API examples:");
		index++;
		GUI.Label(new Rect(30, labelBottom + index * 40, 800, 40), "1.Load all data:\n    <color=#569CD6>EEDataManager.Load();</color>");
		index++;
		GUI.Label(new Rect(30, labelBottom + index * 40, 800, 40),
			"2.Find a XXXData by id (int or string):\n    <color=#569CD6>EEDataManager.Get<XXXData>(id);</color>");
		index++;
		GUI.Label(new Rect(30, labelBottom + index * 40, 800, 40),
			"3.Find XXXData list:\n    <color=#569CD6>EEDataManager.GetList<XXXData>();</color>");

		index++;
		var baseType = typeof(EERowDataCollection);
		var assembly = baseType.Assembly;
		var types = assembly.GetTypes().Where(t => t.IsSubclassOf(baseType));
		var sheetClassTypes = types as Type[] ?? types.ToArray();
		if (!sheetClassTypes.Any())
		{
			labelBottom = index * 40;
			GUI.Label(new Rect(10, labelBottom, 800, 100),
				"<color=#FF5555>No data loaded, you need to import excel files first by Tools->EasyExcel->Import.</color>");
		}
		else
		{
			labelBottom = index * 40 + 20;
			GUI.Label(new Rect(10, labelBottom, 800, 30), string.Format("Loaded <color=#569CD6>{0}</color> sheets:", sheetClassTypes.Count()));
			labelBottom += 30;
			var typesIndex = 0;
			foreach (var sheetClassType in sheetClassTypes)
			{
				var collectClassName = sheetClassType.FullName;
				/*var headNameRaw =
					collectClassName.Substring(0, collectClassName.IndexOf(EESettings.Current.SheetDataPostfix, StringComparison.Ordinal));
				var headParts = headNameRaw.Split('.');*/
				var headParts = collectClassName.Split('.');
				var fileName = headParts.Length == 1 ? null : headParts[0].Substring(EESettings.Current.NameSpacePrefix.Length);
				var sheetClassName = headParts.Length == 1 ? headParts[0] : headParts[1];
				var sheetName = EESettings.Current.GetSheetName(sheetClassType);
				var rowDataClassName = EESettings.Current.GetRowDataClassName(fileName, sheetName, true);
				var rowType = Type.GetType(rowDataClassName);
				var dic = eeDataManager.GetList(rowType);
				var rowDataClassNameShort = EESettings.Current.GetRowDataClassName(fileName, sheetName, false);
				GUI.Label(new Rect(30, labelBottom + typesIndex * 20, 380, 20), string.Format("Sheet Class: <color=#569CD6>{0}</color>", sheetClassName));
				GUI.Label(new Rect(410, labelBottom + typesIndex * 20, 250, 20), string.Format("RowData Class: <color=#569CD6>{0}</color>", rowDataClassNameShort));
				GUI.Label(new Rect(660, labelBottom + typesIndex * 20, 200, 20), string.Format("Rows: <color=#569CD6>{0}</color>", dic != null ? dic.Count.ToString() : "empty"));
				typesIndex++;
			}
		}
	}

	#endregion
	
}