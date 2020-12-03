using System.Collections.Generic;
using System.IO;

namespace PacManager.IO.Var
{
	public static class BinaryReaderExtensions
	{
		public static VarFile ReadVarFileFromPac(this BinaryReader reader, int fileLength)
		{
			var lineList = new List<VarFileLine>();

			var endingPosition = reader.BaseStream.Position + fileLength;
			while (reader.BaseStream.Position < endingPosition)
			{
				var lineLength = reader.ReadInt16();

				lineList.Add(new VarFileLine()
				{
					Bytes = reader.ReadBytes(lineLength)
				});
			}

			return new VarFile()
			{
				Lines = lineList.ToArray()
			};
		}

		public static VarFile ReadVarFileFromVar(this BinaryReader reader)
		{
			var lineList = new List<VarFileLine>();
			while (reader.BaseStream.Length > reader.BaseStream.Position)
			{
				var bytes = reader.ReadLine();
				if (bytes.Length == 0)
				{
					continue;
				}

				lineList.Add(new VarFileLine()
				{
					Bytes = bytes
				});
			}

			return new VarFile()
			{
				Lines = lineList.ToArray()
			};
		}

		private static byte[] ReadLine(this BinaryReader reader)
		{
			var stream = new MemoryStream();

			var currentByte = reader.ReadByte();
			while (currentByte != 0x0A)
			{
				stream.WriteByte(currentByte);

				currentByte = reader.ReadByte();
			}

			return stream.ToArray();
		}
	}
}
