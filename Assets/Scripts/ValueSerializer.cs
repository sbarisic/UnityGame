using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public interface IValueSerializable {
	void Serialize(BinaryWriter BW);
	void Deserialize(BinaryReader BR);
}

class ValueField {
	public string Name;

	Type FieldType;
	object Value;

	public ValueField(string Name, Type FieldType) {
		this.Name = Name;
		this.FieldType = FieldType;
	}

	public T GetValue<T>() {
		return (T)Value;
	}

	public void SetValue<T>(T Val) {
		if (typeof(T) != FieldType)
			throw new Exception("Invalid value type");

		Value = Val;
	}

	public ValueField(BinaryReader BR) {
		Name = BR.ReadString();
		FieldType = Type.GetType(BR.ReadString());

		if (FieldType == typeof(string))
			Value = BR.ReadString();
		else if (FieldType == typeof(int))
			Value = BR.ReadInt32();
		else if (FieldType == typeof(bool))
			Value = BR.ReadBoolean();
		else if (FieldType == typeof(float))
			Value = BR.ReadSingle();
		else {
			if (FieldType.GetInterface(nameof(IValueSerializable)) != null) {
				bool IsNull = BR.ReadBoolean();

				if (IsNull)
					Value = null;
				else {
					IValueSerializable ValInstance = (IValueSerializable)Activator.CreateInstance(FieldType);
					ValInstance.Deserialize(BR);
					Value = ValInstance;
				}

				return;
			}

			throw new Exception("Unknown type");
		}
	}

	public void Serialize(BinaryWriter BW) {
		BW.Write(Name);
		BW.Write(FieldType.AssemblyQualifiedName);

		if (FieldType == typeof(string))
			BW.Write((string)Value);
		else if (FieldType == typeof(int))
			BW.Write((int)Value);
		else if (FieldType == typeof(bool))
			BW.Write((bool)Value);
		else if (FieldType == typeof(float))
			BW.Write((float)Value);
		else {
			if (FieldType.GetInterface(nameof(IValueSerializable)) != null) {
				IValueSerializable ValInstance = (IValueSerializable)Value;

				if (ValInstance == null)
					BW.Write(true);
				else {
					BW.Write(false);
					ValInstance.Serialize(BW);
				}

				return;
			}

			throw new Exception("Unknown type");
		}
	}
}

static class ValueSerializer {
	const string SettingsFile = "settings.dat";
	static List<ValueField> Fields = new List<ValueField>();

	static ValueSerializer() {
		Deserialize();
	}

	static void Serialize() {
		using (FileStream OutStream = File.Open(SettingsFile, FileMode.Create))
		using (BinaryWriter BW = new BinaryWriter(OutStream)) {
			BW.Write(Fields.Count);

			foreach (var F in Fields)
				F.Serialize(BW);
		}
	}

	static void Deserialize() {
		if (!File.Exists(SettingsFile))
			return;

		using (FileStream InStream = File.Open(SettingsFile, FileMode.Open))
		using (BinaryReader BR = new BinaryReader(InStream)) {
			Fields.Clear();
			int Count = BR.ReadInt32();

			for (int i = 0; i < Count; i++)
				Fields.Add(new ValueField(BR));
		}
	}

	public static T GetValue<T>(string Name, T DefaultValue) {
		if (Fields.Count == 0)
			Deserialize();

		foreach (var F in Fields)
			if (F.Name == Name)
				return F.GetValue<T>();

		SetValue(Name, DefaultValue);
		return DefaultValue;
	}

	public static void SetValue<T>(string Name, T Val) {
		foreach (var F in Fields)
			if (F.Name == Name) {
				/*if (F.GetValue<T>().Equals(Val))
					return;*/

				F.SetValue(Val);
				Serialize();
				return;
			}

		ValueField VF = new ValueField(Name, typeof(T));
		VF.SetValue(Val);
		Fields.Add(VF);
		Serialize();
	}
}