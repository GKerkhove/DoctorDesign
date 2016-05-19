using UnityEngine;
using System;

namespace hg.ApiWebKit.apis.google
{
	public interface IGoogleOperationInterceptor
	{
		bool PendingOAuthOperation { get; }
		bool IsExecutingOperations { get; }
	}
}
