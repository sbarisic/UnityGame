using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;

static class FileIO {

	public static void WriteBytes(string FileName, byte[] Data) {
		File.WriteAllBytes(FileName, Data);
	}

	public static byte[] ReadBytes(string FileName) {
		return File.ReadAllBytes(FileName);
	}
}
