using System;
using EasyExcel;
using UnityEngine;

public class ExampleCustomDataLoader : MonoBehaviour
{
	private EEDataManager _eeDataManager;
	
	private void Start()
	{
		// use ExampleDataLoaderResources as loader
		_eeDataManager = new EEDataManager(new ExampleDataLoaderResources());
		
		// use ExampleDataLoaderAssetBundle as loader
		//_eeDataManager = new EEDataManager(new ExampleDataLoaderAssetBundle());
		
		// Load all data sheets like this
		_eeDataManager.Load();
	}
	
	// Load by Resources.Load.
	private class ExampleDataLoaderResources : IEEDataLoader
	{
		public EERowDataCollection Load(string sheetClassName)
		{
			var headName = sheetClassName;
			var filePath = EESettings.Current.GeneratedAssetPath.
				               Substring(EESettings.Current.GeneratedAssetPath.IndexOf("Resources/", StringComparison.Ordinal) + "Resources/".Length)
			               + headName;
			var collection = Resources.Load(filePath) as EERowDataCollection;
			return collection;
		}
	}
	
	// Load by AssetBundle
	private class ExampleDataLoaderAssetBundle : IEEDataLoader
	{
		public EERowDataCollection Load(string sheetClassName)
		{
			// Your AssetBundle file path
			var bundlePath = Application.persistentDataPath + "/***Your Bundle File Name***";
			// Your AssetBundle file path
			var assetPath = "Assets/ExampleFolder/" + sheetClassName + ".asset";
			var bundle = AssetBundle.LoadFromFile(bundlePath);
			var collection =  bundle.LoadAsset(assetPath) as EERowDataCollection;
			return collection;
		}
	}


	#region Just for showing the data. You do not have to know these
	
	private void OnGUI()
	{
		ExampleLoadData.gui(_eeDataManager);
	}
	
	#endregion
	
}