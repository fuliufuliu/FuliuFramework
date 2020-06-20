﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace EasyExcel
{
	public static class EELog
	{
		public static void Log(string message)
		{
			Debug.Log("[EasyExcel] " + message);
		}
		
		public static void LogWarning(string message)
		{
			Debug.LogWarning("[EasyExcel] " + message);
		}

		public static void LogError(string message)
		{
			Debug.LogError("[EasyExcel] " + message);
		}
	}
	
	public static class EEUtility
	{
		public static bool IsExcelFileSupported(string filePath)
		{
			if (string.IsNullOrEmpty(filePath))
				return false;
			var fileName = Path.GetFileName(filePath);
			if (fileName.Contains("~$"))// avoid temporary files
				return false;
			var lower = Path.GetExtension(filePath).ToLower();
			return lower == ".xlsx" || lower == ".xls" || lower == ".xlsm";
		}
		
		public static string GetFieldComment(Type classType, string fieldName)
		{
			try
			{
				var fld = classType.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				var comment = fld.GetCustomAttributes(typeof(EECommentAttribute), true)[0] as EECommentAttribute;
				return comment != null ? comment.content : null;
			}
			catch
			{
				// ignored
			}

			return null;
		}

		// public static FieldInfo GetRowDataKeyField(Type rowDataType)
		// {
		// 	var fields = rowDataType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		// 	var keyField = (from fieldInfo in fields let attrs = fieldInfo.GetCustomAttributes(typeof(EEKeyFieldAttribute), false) 
		// 		where attrs.Length > 0 select fieldInfo).FirstOrDefault();
		// 	return keyField;
		// }
		
		public static List<FieldInfo> GetRowDataKeyFields(Type rowDataType)
		{
			var fields = rowDataType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			var keyField = from fieldInfo in fields let attrs = fieldInfo.GetCustomAttributes(typeof(EEKeyFieldAttribute), false) 
				where attrs.Length > 0 select fieldInfo;
			return keyField?.ToList();
		}
	}
}