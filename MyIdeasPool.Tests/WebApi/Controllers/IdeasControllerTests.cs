using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyIdeasPool.Core.Services;
using MyIdeasPool.WebApi.Configuration;
using MyIdeasPool.WebApi.Controllers;
using MyIdeasPool.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIdeasPool.Tests.WebApi.Controllers
{
	[TestClass]
	public class IdeasControllerTests
	{

		IMapper _mapper;
		IdeasController controller;
		Mock<IIdeasService> _ideasService;
		GlobalConfiguration _config;

		[TestInitialize]
		public void Setup()
		{
			_mapper = TestHelper.CrateMapper();
			_ideasService = new Mock<IIdeasService>();
			_config = new GlobalConfiguration()
			{
				IdeasPageSize = 10
			};

			controller = new IdeasController(_ideasService.Object, _config, _mapper);

			_ideasService.Setup(x => x.List()).Returns(TestHelper.MockAsyncSet(TestHelper.GenerateIdeas(30)).Object);
		}

		[TestMethod]
		public async Task Get_NoPage_ReturnFirst10()
		{
			var response = await controller.Get();

			Assert.IsNotNull(response.Result);
			var result = response.Result as OkObjectResult;

			Assert.AreEqual(200, result.StatusCode);

			var items = result.Value as IEnumerable<IdeaModel>;

			Assert.AreEqual(10, items.Count());
			Assert.IsTrue(items.First().Content.StartsWith("0 - Idea"));
		}
	}
}
