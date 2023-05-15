using System.Linq;

namespace Business.Util
{
	public class FieldValidationHelper : IFiledValidationHelper
	{
		public bool AreStringPropsNullOrEmpty(object o)
		{
			bool isNullOrEmpty = o.GetType().GetProperties()
				.Where(pi => pi.PropertyType == typeof(string))
				.Select(pi => (string)pi.GetValue(o))
				.Any(value => string.IsNullOrEmpty(value));

			return false;
		}
	}
}
