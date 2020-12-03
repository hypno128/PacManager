using PacManager.IO;
using System;
using System.IO;

namespace PacManager
{
	public class Program
	{
		public static void Main(string[] args)
		{
			if (args.Length < 2)
			{
				Console.WriteLine("Missing parameters");
				return;
			}

			// Prepare values 
			var command = args[0];
			var source = args[1];
			string destination;
			if (args.Length < 3)
			{
				destination = source;
				Console.WriteLine("Using default destination");
			}
			else
			{
				destination = args[2];
			}

			// Run operation 
			if (command == "-p")
			{
				destination = Path.ChangeExtension(destination, ".pac");

				var encoder = new PacEncoder(source);
				encoder.Encode(destination);
			}
			else if (command == "-u")
			{
				destination = Path.ChangeExtension(destination, null);

				using (var decoder = new PacDecoder(source))
				{
					decoder.Decode(destination);
				}
			}
			else
			{
				Console.WriteLine("Invalid command");
			}
		}
	}
}
