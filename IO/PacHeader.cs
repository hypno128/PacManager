using System;
using System.IO;

namespace PacManager.IO
{
	/// <summary>
	/// Class for the header data at the start of a pac file 
	/// Head data includes information about what files are stored in the pac 
	/// </summary>
	public class PacHeader
	{
		public PacHeader()
		{

		}
		public PacHeader(string[] filenames) : this()
		{
			var nameLength = 0;
			var entries = new PacHeaderEntry[filenames.Length];
			for (var counter = 0; counter < filenames.Length; counter++)
			{
				entries[counter] = new PacHeaderEntry()
				{
					Name = Path.GetFileNameWithoutExtension(filenames[counter])
				};

				nameLength = Math.Max(nameLength, entries[counter].Name.Length);
			}

			EntryCount = (short)filenames.Length;
			NameLength = (short)nameLength;
			DataOffset = 7 + filenames.Length * (nameLength + 12);
			Entries = entries;
		}

		public short EntryCount { get; set; }

		public short NameLength { get; set; }

		public int DataOffset { get; set; }

		public PacHeaderEntry[] Entries { get; set; }
	}
}
