using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PacManager.IO.Srp
{
	public static class BinaryReaderExtensions
	{
		public static SrpFile ReadSrpFileFromPac(this BinaryReader reader)
		{
			// First four bytes of SRP file are how many commands there are in the file 
			var lineCount = reader.ReadInt32();

			// The rest of the data is commands 
			var commands = new SrpFileCommand[lineCount];
			for (var counter = 0; counter < lineCount; counter++)
			{
				// First two bytes of command are the length (in bytes) 
				var length = reader.ReadInt16();

				// The next length bytes are the command 
				// These bytes need to be formatted (see FlipBytes()) 
				var bytes = reader.ReadBytes(length);
				BinaryHelpers.FlipBytes(bytes);

				commands[counter] = new SrpFileCommand()
				{
					Bytes = bytes
				};
			}

			return new SrpFile()
			{
				Commands = commands
			};
		}

		public static SrpFile ReadSrpFileFromSrp(this BinaryReader reader)
		{
			var commandList = new List<SrpFileCommand>();
			while (reader.BaseStream.Position < reader.BaseStream.Length)
			{
				var stream = new MemoryStream();
				reader.ReadHexadecimalLineToStream(stream);
				reader.ReadTextLinesToStream(stream);

				commandList.Add(new SrpFileCommand()
				{
					Bytes = stream.ToArray()
				});
			}

			return new SrpFile()
			{
				Commands = commandList.ToArray()
			};
		}

		private static void ReadHexadecimalLineToStream(this BinaryReader reader, MemoryStream stream)
		{
			// Read chars from reader until it reaches a newline character 
			var builder = new StringBuilder();
			var currentChar = reader.ReadChar();
			while (currentChar != '\n')
			{
				builder.Append(currentChar);

				if (reader.BaseStream.Position >= reader.BaseStream.Length)
				{
					break;
				}

				currentChar = reader.ReadChar();
			}

			// Convert hexadecimal string to byte array and write to stream 
			var hexadecimal = builder.ToString();
			for (var counter = 0; counter < hexadecimal.Length; counter += 2)
			{
				stream.WriteByte(Convert.ToByte(hexadecimal.Substring(counter, 2), 16));
			}
		}

		private static void ReadTextLinesToStream(this BinaryReader reader, MemoryStream stream)
		{
			byte currentByte;
			byte nextByte;

			if (!reader.TryReadByte(out currentByte))
			{
				return;
			}
			if (currentByte == 0x0A)
			{
				return;
			}
			if (!reader.TryReadByte(out nextByte))
			{
				return;
			}

			while (currentByte != 0x0A || nextByte != 0x0A)
			{
				switch (currentByte)
				{
					case 0x0A: // Newline 
						stream.WriteByte(0x2C);
						break;
					case 0x2C: // Comma 
						stream.WriteByte(0x81);
						break;
					default:
						stream.WriteByte(currentByte);
						break;
				}

				currentByte = nextByte;
				if (!reader.TryReadByte(out nextByte))
				{
					nextByte = 0x0A;
				}
			}
		}

		private static bool TryReadByte(this BinaryReader reader, out byte readByte)
		{
			if (reader.BaseStream.Position >= reader.BaseStream.Length)
			{
				readByte = 0;

				return false;
			}

			readByte = reader.ReadByte();

			return true;
		}
	}
}
