﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace BizHawk.Emulation.Common
{
	/// <summary>
	/// This is a generic implementation of IInputCallbackSystem that can be used
	/// by any core
	/// </summary>
	/// <seealso cref="IInputCallbackSystem" />
	public class InputCallbackSystem : List<Action<int>>, IInputCallbackSystem
	{
		public void Call(int gamepadIndex)
		{
			foreach (var action in this)
			{
				action(gamepadIndex);
			}
		}

		// TODO: these just happen to be all the add/remove methods the client uses, to be thorough the others should be overriden as well
		public void RemoveAll(IEnumerable<Action<int>> actions)
		{
			var hadAny = this.Any();

			foreach (var action in actions)
			{
				Remove(action);
			}

			var hasAny = this.Any();

			Changes(hadAny, hasAny);
		}

		public new void Add(Action<int> item)
		{
			var hadAny = this.Any();
			base.Add(item);
			var hasAny = this.Any();

			Changes(hadAny, hasAny);
		}

		public new bool Remove(Action<int> item)
		{
			var hadAny = this.Any();
			var result = base.Remove(item);
			var hasAny = this.Any();

			Changes(hadAny, hasAny);

			return result;
		}

		public delegate void ActiveChangedEventHandler();
		public event ActiveChangedEventHandler? ActiveChanged;

		private void Changes(bool hadAny, bool hasAny)
		{
			if ((hadAny && !hasAny) || (!hadAny && hasAny))
			{
				ActiveChanged?.Invoke();
			}
		}
	}
}
