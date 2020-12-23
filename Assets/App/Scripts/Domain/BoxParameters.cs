namespace DynamicBox.Domain
{
	public class BoxParameters
	{
		public float width;
		public float height;
		public float depth;
		public double instanceID;

		
		public BoxParameters ()
		{
			
		}
		
		public BoxParameters (float width, float height, float depth, double instanceID)
		{
			this.width = width;
			this.height = height;
			this.depth = depth;
			this.instanceID = instanceID;
		}
	}
}