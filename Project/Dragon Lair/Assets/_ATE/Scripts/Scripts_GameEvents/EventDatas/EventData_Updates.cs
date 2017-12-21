using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Ate
{


	public class EventData_Updates : EventData
	{
		
		public int updateIndex;
		public int universalFrameLength;

		public EventData_Updates (int updateIndex)
		{
			this.updateIndex = updateIndex;
		}

		public EventData_Updates (int updateIndex, int universalFrameLength)
		{
			this.updateIndex = updateIndex;
			this.universalFrameLength = universalFrameLength;
		}

	}//End Class


}//End Namespace

