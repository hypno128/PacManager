using System.IO;

namespace PacManager.IO
{
	public static class BinaryReaderExtensions
	{
		public static PacHeader ReadPacHeader(this BinaryReader reader)
		{
			var entryCount = reader.ReadInt16();
			var nameLength = reader.ReadByte();
			var dataOffset = reader.ReadInt32();

			var entries = new PacHeaderEntry[entryCount];
			for (var counter = 0; counter < entryCount; counter++)
			{
				entries[counter] = reader.ReadPacHeaderEntry(nameLength);
			}

			return new PacHeader()
			{
				EntryCount = entryCount,
				NameLength = nameLength,
				DataOffset = dataOffset,
				Entries = entries
			};
		}

		public static PacHeaderEntry ReadPacHeaderEntry(this BinaryReader reader, int nameLength)
		{
			// Running into some encoding issues when using a string (replaces unrecognized characters) 
			// Copying the bytes directly into chars avoids the encoding problem for foreign characters 
			var bytes = reader.ReadBytes(nameLength);
			var chars = new char[bytes.Length];
			for (var counter = 0; counter < bytes.Length; counter++)
			{
				chars[counter] = (char)bytes[counter];
			}

			return new PacHeaderEntry()
			{
				Name = new string(chars).Trim('\0'),
				Offset = reader.ReadInt64(),
				Length = reader.ReadInt32()
			};
		}
	}
}
