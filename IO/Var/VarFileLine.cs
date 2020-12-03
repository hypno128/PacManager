namespace PacManager.IO.Var
{
	public class VarFileLine
	{
		public byte[] Bytes { get; set; }

		public short Length
		{
			get
			{
				return (short)Bytes.Length;
			}
		}
	}
}
