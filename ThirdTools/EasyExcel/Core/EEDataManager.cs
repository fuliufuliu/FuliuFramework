using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using fuliu;
using UnityEngine;

namespace EasyExcel
{
	using RowDataDictInt = Dictionary<int, EERowData>;
	using RowDataDictStr = Dictionary<string, EERowData>;
	using RowDataTowKeyDict = Dictionary<Keys<object, object>, EERowData>;
	using RowDataThreeKeyDict = Dictionary<Keys<object, object, object>, EERowData>;

	public class EEDataManager
	{
		private readonly IEEDataLoader dataLoader;
		private readonly Dictionary<Type, RowDataDictInt> dataCollectionDictInt = new Dictionary<Type, RowDataDictInt>();
		private readonly Dictionary<Type, RowDataDictStr> dataCollectionDictStr = new Dictionary<Type, RowDataDictStr>();
		private readonly Dictionary<Type, RowDataTowKeyDict> dataCollectionDictTowKeys 
			= new Dictionary<Type, RowDataTowKeyDict>();
		private readonly Dictionary<Type, RowDataThreeKeyDict> dataCollectionDictThreeKeys 
			= new Dictionary<Type, RowDataThreeKeyDict>();
		
		public EEDataManager(IEEDataLoader _dataLoader = null)
		{
			dataLoader = _dataLoader ?? new EEDataLoaderResources();
		}
		
		#region Find with key
		
		public T Get<T>(int key) where T : EERowData
		{
			return Get(key, typeof(T)) as T;
		}
		
		public T Get<T>(string key) where T : EERowData
		{
			return Get(key, typeof(T)) as T;
		}
		
		public T Get<T>(object key1, object key2) where T : EERowData
		{
			return Get(key1, key2, typeof(T)) as T;
		}
		
		public T Get<T>(object key1, object key2, object key3) where T : EERowData
		{
			return Get(key1, key2, key3, typeof(T)) as T;
		}

		public EERowData Get(object key1, object key2, Type type)
		{
			var keys = new Keys<object, object>(key1, key2);
			RowDataTowKeyDict soDic;
			dataCollectionDictTowKeys.TryGetValue(type, out soDic);
			if (soDic == null) return null;
			EERowData data;
			soDic.TryGetValue(keys, out data);
			return data;
		}
		
		public EERowData Get(object key1, object key2, object key3, Type type)
		{
			var keys = new Keys<object, object, object>(key1, key2, key3);
			RowDataThreeKeyDict soDic;
			dataCollectionDictThreeKeys.TryGetValue(type, out soDic);
			if (soDic == null) return null;
			EERowData data;
			soDic.TryGetValue(keys, out data);
			return data;
		}

		public EERowData Get(int key, Type type)
		{
			RowDataDictInt soDic;
			dataCollectionDictInt.TryGetValue(type, out soDic);
			if (soDic == null) return null;
			EERowData data;
			soDic.TryGetValue(key, out data);
			return data;
		}
		
		public EERowData Get(string key, Type type)
		{
			RowDataDictStr soDic;
			dataCollectionDictStr.TryGetValue(type, out soDic);
			if (soDic == null) return null;
			EERowData data;
			soDic.TryGetValue(key, out data);
			return data;
		}

		public List<T> GetList<T>() where T : EERowData
		{
			RowDataDictInt dictInt;
			dataCollectionDictInt.TryGetValue(typeof(T), out dictInt);
			if (dictInt != null)
			{
				var list = new List<T>();
				foreach (var data in dictInt)
					list.Add((T) data.Value);
				return list;
			}
			RowDataDictStr dictStr;
			dataCollectionDictStr.TryGetValue(typeof(T), out dictStr);
			if (dictStr != null)
			{
				var list = new List<T>();
				foreach (var data in dictStr)
					list.Add((T) data.Value);
				return list;
			}
			return null;
		}

		public List<EERowData> GetList(Type type)
		{
			RowDataDictInt dictInt;
			dataCollectionDictInt.TryGetValue(type, out dictInt);
			if (dictInt != null)
				return dictInt.Values.ToList();
			RowDataDictStr dictStr;
			dataCollectionDictStr.TryGetValue(type, out dictStr);
			if (dictStr != null)
				return dictStr.Values.ToList();
			return null;
		}
		
		#endregion

		#region Load Assets
		
		public void Load()
		{
#if UNITY_EDITOR
			if (!EESettings.Current.GeneratedAssetPath.Contains("/Resources/"))
			{
				UnityEditor.EditorUtility.DisplayDialog("EasyExcel",
					string.Format(
						"AssetPath of EasyExcel Settings MUST be in Resources folder.\nCurrent is {0}.",
						EESettings.Current.GeneratedAssetPath), "OK");
				return;
			}
#endif
			dataCollectionDictInt.Clear();
			dataCollectionDictStr.Clear();

			var baseDataCollectionType = typeof(EERowDataCollection);
			foreach (var dataCollectionType in baseDataCollectionType.Assembly.GetTypes().Where(t => t.IsSubclassOf(baseDataCollectionType)))
				ParseOneDataCollection(dataCollectionType);

			EELog.Log(string.Format("{0} tables loaded.", dataCollectionDictInt.Count + dataCollectionDictStr.Count));
		}

		private void ParseOneDataCollection(Type dataCollectionType)
		{
			try
			{
				var sheetClassName = dataCollectionType.Name;//GetSheetName(dataCollectionType);
				var collection = dataLoader.Load(sheetClassName);
				if (collection == null)
				{
					EELog.LogError("EEDataManager: Load asset error, sheet name " + sheetClassName);
					return;
				}

				var rowDataType = GetRowDataClassType(collection.ExcelFileName, dataCollectionType);
				var keyFields = EEUtility.GetRowDataKeyFields(rowDataType);
				if (keyFields.Count == 0)
				{
					EELog.LogError("EEDataManager: Cannot find Key field in sheet " + sheetClassName);
					return;
				}

				if (keyFields.Count == 1)
				{
					var keyField = keyFields[0];
					var keyType = keyField.FieldType;
					if (keyType == typeof(int))
					{
						var dataDict = new RowDataDictInt();
						for (var i = 0; i < collection.GetDataCount(); ++i)
						{
							var data = collection.GetData(i);
							int key = (int) keyField.GetValue(data);
							dataDict.Add(key, data);
						}
					
						dataCollectionDictInt.Add(rowDataType, dataDict);
					}
					else if (keyType == typeof(string))
					{
						var dataDict = new RowDataDictStr();
						for (var i = 0; i < collection.GetDataCount(); ++i)
						{
							var data = collection.GetData(i);
							string key = (string) keyField.GetValue(data);
							dataDict.Add(key, data);
						}

						dataCollectionDictStr.Add(rowDataType, dataDict);
					}
					else
					{
						EELog.LogError(string.Format("Load {0} failed. There is no valid Key field in ", dataCollectionType.Name));
					}
				}
				else if(keyFields.Count == 2)
				{
					var dataDict = new RowDataTowKeyDict();
					for (var i = 0; i < collection.GetDataCount(); ++i)
					{
						var data = collection.GetData(i);
						var key1 = keyFields[0].GetValue(data);
						var key2 = keyFields[1].GetValue(data);
						dataDict.Add(new Keys<object, object>(key1, key2), data);
					}
					
					dataCollectionDictTowKeys.Add(rowDataType, dataDict);
				}
				else if(keyFields.Count == 3)
				{
					var dataDict = new RowDataThreeKeyDict();
					for (var i = 0; i < collection.GetDataCount(); ++i)
					{
						var data = collection.GetData(i);
						var key1 = keyFields[0].GetValue(data);
						var key2 = keyFields[1].GetValue(data);
						var key3 = keyFields[2].GetValue(data);
						dataDict.Add(new Keys<object, object, object>(key1, key2, key3), data);
					}
					
					dataCollectionDictThreeKeys.Add(rowDataType, dataDict);
				}else if (keyFields.Count > 3)
				{
					Debug.LogErrorFormat("EasyExcel 导入功能暂不支持 key超过3个的表！");
				}
			}
			catch (Exception e)
			{
				EELog.LogError(e.ToString());
			}
		}

		private static Type GetRowDataClassType(string excelFileName, Type sheetClassType)
		{
			var excelName = Path.GetFileNameWithoutExtension(excelFileName);
			var sheetName = GetSheetName(sheetClassType);
			var type = Type.GetType(EESettings.Current.GetRowDataClassName(excelName, sheetName, true));
			return type;
		}

		private static string GetSheetName(Type sheetClassType)
		{
			return EESettings.Current.GetSheetName(sheetClassType);
		}

		#endregion
	}
}