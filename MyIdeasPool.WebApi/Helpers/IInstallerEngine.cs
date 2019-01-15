using MyIdeasPool.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyIdeasPool.WebApi.Helpers
{
	public interface IInstallerEngine
	{
		Task Install();
	}

	public class InstallerEngine : IInstallerEngine
	{

		private readonly IEnumerable<IInstaller> _installers;
		public InstallerEngine(IEnumerable<IInstaller> installers)
		{
			_installers = installers;
		}

		public async Task Install()
		{
			foreach (var installer in _installers)
			{
				await installer.Install();
			}
		}
	}
}
