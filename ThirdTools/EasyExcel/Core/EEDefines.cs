using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyExcel
{
	/// <summary>
	///     One row in a excel sheet.
	/// </summary>
	[Serializable]
	public abstract class EERowData
	{
		// public object GetKeyFieldValue()
		// {
		// 	var keyField = EEUtility.GetRowDataKeyField(GetType());
		// 	return keyField == null ? null : keyField.GetValue(this);
		// }
		
		public List<object> GetKeyFieldValues()
		{
			var keyFields = EEUtility.GetRowDataKeyFields(GetType());
			List<object> res = new List<object>(keyFields.Count);
			for (int i = 0; i < keyFields.Count; i++)
			{
				var keyField = keyFields[i];
				res.Add(keyField == null ? null : keyField.GetValue(this));
			}

			return res;
		}
	}

	/// <summary>
	///     All RowData in an excel sheet
	/// </summary>
	public abstract class EERowDataCollection : ScriptableObject
	{
		public string ExcelFileName;
		public string ExcelSheetName;
		public string KeyFieldName;
		public abstract void AddData(EERowData data);
		public abstract int GetDataCount();
		public abstract EERowData GetData(int index);

		public virtual void OnLoaded()
		{
		}

		private void OnEnable()
		{
			OnLoaded();
		}
	}
	
	public static class EEConstant
	{
		public const string Version = "3.4";
	}

	/// <summary>
	/// 	Mark which field of class is key
	/// </summary>
	public class EEKeyFieldAttribute : Attribute
	{
		
	}
	
	[AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
	public class EnumLabelAttribute : Attribute
	{
		private int[] order = new int[0];
		public readonly string label;

		public EnumLabelAttribute(string label)
		{
			this.label = label;
		}

		public EnumLabelAttribute(string label, params int[] order)
		{
			this.label = label;
			this.order = order;
		}
	}
	
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class EECommentAttribute : Attribute
	{
		public string content
		{
			get
			{
				return EESettings.Current.Lang == EELang.CN ? contentCN : contentEN;		
			}
		}

		private readonly string contentEN;
		private readonly string contentCN;
		
		public EECommentAttribute(string textEN, string textCN)
		{
			contentEN = textEN;
			contentCN = textCN;
		}
	}

	public enum EELang
	{
		[EnumLabel("English")]
		EN,
		[EnumLabel("中文")]
		CN
	}
	
}