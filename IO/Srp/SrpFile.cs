namespace PacManager.IO.Srp
{
	/// <summary>
	/// Class for SRP file data 
	/// </summary>
	public class SrpFile
	{
		public SrpFileCommand[] Commands { get; set; }

		public int Length
		{
			get
			{
				return Commands.Length;
			}
		}

		public const string Extension = ".srp";
	}
}
