using System.Collections.Generic;

namespace Domain.Interfaces
{
	public class DataPage<T>
	{
		public IList<T> Items;
		public int Total;
	}
}
