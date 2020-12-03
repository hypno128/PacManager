using System;

namespace PacManager.IO.Srp
{
	/// <summary>
	/// Class for an individual command in an Srp file 
	/// </summary>
	public class SrpFileCommand
	{
		/// <summary>
		/// The data for the command 
		/// All commands are at least 4 bytes long 
		/// First two bytes are an int "Type" of command (0 is text, 1 is special, 2 is art-related, 3 is music-related, etc)
		/// Second two bytes are a bit flag "Flags" for the command (what these do seems to depend on the "Type" of command)
		/// The remainder of the bytes are the "Data" for the command, size varies (for example: the text being said by a character) 
		/// </summary>
		public byte[] Bytes { get; set; }

		public short Length
		{
			get
			{
				return (short)Bytes.Length;
			}
		}

		public short Type
		{
			get
			{
				return BitConverter.ToInt16(Bytes);
			}
		}
	}
}
