using System.IO;

namespace PacManager.IO.Var
{
	public static class BinaryWriterExtensions
	{
		public static void WriteForPac(this BinaryWriter writer, VarFile file)
		{
			foreach (var fileLine in file.Lines)
			{
				writer.Write(fileLine.Length);
				writer.Write(fileLine.Bytes);
			}
		}

		public static void WriteForVar(this BinaryWriter writer, VarFile file)
		{
			foreach (var fileLine in file.Lines)
			{
				writer.Write(fileLine.Bytes);
				writer.Write('\n');
			}
		}
	}
}
