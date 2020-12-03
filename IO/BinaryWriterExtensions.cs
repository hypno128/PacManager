using System.IO;

namespace PacManager.IO
{
	public static class BinaryWriterExtensions
	{
		public static void Write(this BinaryWriter writer, PacHeader header)
		{
			writer.Write(header.EntryCount);
			writer.Write((byte)header.NameLength);
			writer.Write(header.DataOffset);
			foreach (var headerEntry in header.Entries)
			{
				var chars = headerEntry.Name.ToCharArray();
				for (var counter = 0; counter < chars.Length; counter++)
				{
					writer.Write((byte)chars[counter]);
				}

				writer.BaseStream.Position += header.NameLength - headerEntry.Name.Length;

				writer.Write(headerEntry.Offset);
				writer.Write(headerEntry.Length);
			}
		}
	}
}
