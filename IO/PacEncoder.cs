using PacManager.IO.Srp;
using PacManager.IO.Var;
using System.IO;

namespace PacManager.IO
{
	public class PacEncoder
	{
		public PacEncoder(string directory)
		{
			if (!Directory.Exists(directory))
			{
				throw new DirectoryNotFoundException();
			}

			_directory = directory;
		}

		public void Encode(string path)
		{
			if (File.Exists(path))
			{
				File.Delete(path);
			}

			var filenames = Directory.GetFiles(_directory);
			var header = new PacHeader(filenames);

			using (var writer = new BinaryWriter(File.Create(path)))
			{
				// Write pac data 
				writer.BaseStream.Position = header.DataOffset;
				for (var counter = 0; counter < filenames.Length; counter++)
				{
					var startingPosition = writer.BaseStream.Position;

					// Copy data from file 
					using (var reader = new BinaryReader(File.OpenRead(filenames[counter])))
					{
						var extension = Path.GetExtension(filenames[counter]);
						switch (extension)
						{
							case SrpFile.Extension:
								var srpFile = reader.ReadSrpFileFromSrp();
								writer.WriteForPac(srpFile);
								break;
							case VarFile.Extension:
								var varFile = reader.ReadVarFileFromVar();
								writer.WriteForPac(varFile);
								break;
							default:
								var bytes = reader.ReadBytes((int)reader.BaseStream.Length);
								writer.Write(bytes);
								break;
						}
					}

					// Save header offset and length 
					header.Entries[counter].Offset = startingPosition - header.DataOffset;
					header.Entries[counter].Length = (int)(writer.BaseStream.Position - startingPosition);
				}

				// Write pac header 
				writer.BaseStream.Position = 0;
				writer.Write(header);
			}
		}

		private readonly string _directory;
	}
}
