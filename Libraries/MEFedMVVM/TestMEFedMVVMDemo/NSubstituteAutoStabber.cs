using NSubstitute;
using System;

namespace MEFedMVVM.Testability.Moq
{
	public class NSubstituteAutoStabber : AutoStabberBase
	{
		protected override object CreateStub(Type type)
		{
			return Substitute.For([type], null);
		}
	}
}