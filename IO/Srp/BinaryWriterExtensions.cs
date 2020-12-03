using System;
using System.IO;

namespace PacManager.IO.Srp
{
	public static class BinaryWriterExtensions
	{
		public static void WriteForPac(this BinaryWriter writer, SrpFile file)
		{
			writer.Write(file.Length);
			foreach (var fileCommand in file.Commands)
			{
				writer.Write(fileCommand.Length);

				BinaryHelpers.FlipBytes(fileCommand.Bytes);
				writer.Write(fileCommand.Bytes);
			}
		}

		public static void WriteForSrp(this BinaryWriter writer, SrpFile file)
		{
			foreach (var fileCommand in file.Commands)
			{
				if (fileCommand.Type == 0)
				{
					writer.WriteHexadecimal(fileCommand.Bytes, 0, 4);
					writer.Write('\n');
					writer.WriteText(fileCommand.Bytes, 4, fileCommand.Length - 4);
				}
				else
				{
					writer.WriteHexadecimal(fileCommand.Bytes, 0, fileCommand.Length);
				}

				writer.Write('\n');
				writer.Write('\n');
			}
		}

		private static void WriteHexadecimal(this BinaryWriter writer, byte[] bytes, int index, int length)
		{
			writer.Write(BitConverter.ToString(bytes, index, length).Replace("-", "").ToCharArray());
		}

		private static void WriteText(this BinaryWriter writer, byte[] bytes, int index, int length)
		{
			for (var counter = index; counter < index + length; counter++)
			{
				switch (bytes[counter])
				{
					case 0x2C: // Comma 
						writer.Write((byte)0x0A); // Newline 
						break;
					default:
						writer.Write(bytes[counter]);
						break;
				}
			}
		}
	}
}
