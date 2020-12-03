namespace PacManager.IO.Srp
{
	public static class BinaryHelpers
	{
		/// <summary>
		/// This method "flips" the first and second half of the bytes back to the usual state.
		/// 
		/// Some data in these files appears to be "flipped", with the first and second half of the bytes swapped.
		/// Instead of "12345678" they are written "56781234".
		/// Not certain if this is some form of encoding, or some setting for Japanese computers.
		/// </summary>
		/// <param name="bytes">Array of bytes to "flip"</param>
		public static void FlipBytes(byte[] bytes)
		{
			for (var counter = 0; counter < bytes.Length; counter++)
			{
				bytes[counter] = (byte)(bytes[counter] >> 4 | bytes[counter] << 4);
			}
		}
	}
}
