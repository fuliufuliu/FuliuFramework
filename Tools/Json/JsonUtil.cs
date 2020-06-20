//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

//using LitJson;
//using Kooboo.Json;

public static class JsonUtil  {

//	static JsonUtil()
//	{
//		JsonMapper.RegisterExporter<float>((obj, writer) => writer.Write(Convert.ToDouble(obj)));
//		JsonMapper.RegisterImporter<double, float>(input => Convert.ToSingle(input));
//	}
//	
//	public static void RegisterExporter<T>(ExporterFunc<T> exporter)
//	{
//		JsonMapper.RegisterExporter<T>(exporter);
//	}
//	
//	public static void RegisterImporter<TJson, TValue> (ImporterFunc<TJson, TValue> importer)
//	{
//		JsonMapper.RegisterImporter(importer);
//	}
	
	/// <summary>
	/// 将对象序列化为 JSON 字符串。
	/// </summary>
	/// <param name="obj">要序列化的对象。</param>
	/// <returns>序列化后的 JSON 字符串。</returns>
	public static string ToJson(object obj)
	{
//		return JsonSerializer.ToJson(obj);
//		return JsonMapper.ToJson(obj);
//		return JsonFormatter.SerializeObject(obj);
//		return JsonConvert.SerializeObject(obj);
		return JsonUtility.ToJson(obj);
	}

	/// <summary>
	/// 将 JSON 字符串反序列化为对象。
	/// </summary>
	/// <typeparam name="T">对象类型。</typeparam>
	/// <param name="json">要反序列化的 JSON 字符串。</param>
	/// <returns>反序列化后的对象。</returns>
	public static T ToObject<T>(string json)
	{
//		return JsonSerializer.ToObject<T>(json);
//		return JsonMapper.ToObject<T>(json);
//		return (T) JsonFormatter.DeserializeObject(json, typeof(T));
//		return (T) JsonConvert.DeserializeObject<T>(json);
		return JsonUtility.FromJson<T>(json);
	}

	/// <summary>
	/// 将 JSON 字符串反序列化为对象。
	/// </summary>
	/// <param name="objectType">对象类型。</param>
	/// <param name="json">要反序列化的 JSON 字符串。</param>
	/// <returns>反序列化后的对象。</returns>
	public static object ToObject(Type objectType, string json)
	{
//		return JsonSerializer.ToObject(json, objectType);
//		return JsonFormatter.DeserializeObject(json, objectType);
//		return JsonConvert.DeserializeObject(json, objectType);
//		return JsonMapper.ToObject(json, objectType);
		return JsonUtility.FromJson(json, objectType);
	}

//	public static object CallGenericMethod(Type typeOfClass, string methodName, Type[] genericMethodTypes, object obj, params object[] parameters)
//	{
//		MethodInfo mi1 = typeof(JsonUtil).GetMethod("ToObject");
//		MethodInfo mi2 = mi1.MakeGenericMethod(genericMethodTypes);
//		return mi2.Invoke(null, parameters);
//	}
	public static JsonData ToJsonObject(string sJson)
	{
		return JsonMapper.ToObject(sJson);
	}
	
	/// <summary>
	/// 
	/// </summary>
	/// <param name="data"></param>
	/// <typeparam name="T"> T 限制为字典类型 </typeparam>
	/// <returns></returns>
	public static Dictionary<string, JsonData> ToDic(this JsonData data)
	{
		var res = new Dictionary<string, JsonData>();
		foreach (var key in data.Keys)
		{
			res[key] = data[key];
		}

		return res;
	}
}