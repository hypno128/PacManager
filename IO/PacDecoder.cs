using PacManager.IO.Srp;
using PacManager.IO.Var;
using System;
using System.IO;

namespace PacManager.IO
{
	public class PacDecoder : IDisposable
	{
		public PacDecoder(string path)
		{
			if (!File.Exists(path))
			{
				throw new FileNotFoundException();
			}

			_reader = new BinaryReader(File.OpenRead(path));
		}

		public void Decode(string directory)
		{
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			// Get header information from beginning of pac file 
			_reader.BaseStream.Position = 0;
			var header = _reader.ReadPacHeader();
			foreach (var headerEntry in header.Entries)
			{
				// Get extension for output file 
				var extension = ReadExtension(_reader, header.DataOffset + headerEntry.Offset, headerEntry.Length);

				// Get path for output file 
				var path = Path.Combine(directory, headerEntry.Name);
				path = Path.ChangeExtension(path, extension);
				if (File.Exists(path))
				{
					File.Delete(path);
				}

				// Copy data to output file 
				using (var writer = new BinaryWriter(File.Create(path)))
				{
					_reader.BaseStream.Position = header.DataOffset + headerEntry.Offset;
					switch (extension)
					{
						case SrpFile.Extension:
							var srpFile = _reader.ReadSrpFileFromPac();
							writer.WriteForSrp(srpFile);
							break;
						case VarFile.Extension:
							var varFile = _reader.ReadVarFileFromPac(headerEntry.Length);
							writer.WriteForVar(varFile);
							break;
						default:
							var bytes = _reader.ReadBytes(headerEntry.Length);
							writer.Write(bytes);
							break;
					}
				}
			}
		}

		public void Dispose()
		{
			_reader.Dispose();
		}

		private readonly BinaryReader _reader;

		private string ReadExtension(BinaryReader reader, long position, long length)
		{
			// Srp file, determines what happens in a scene 
			reader.BaseStream.Position = position + length - 4; 
			if (reader.ReadInt16() == 0x10 && 
				reader.ReadInt16() == 0x03)
			{
				return SrpFile.Extension;
			}

			// Var file, sets variable values 
			reader.BaseStream.Position = position + length - 5; 
			if (new string(reader.ReadChars(5)) == "[END]") 
			{
				return VarFile.Extension;
			}

			return null;
		}
	}
}
