using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyIdeasPool.Core.Models;
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
		Mock<IOptions<GlobalConfiguration>> _config;

		[TestInitialize]
		public void Setup()
		{
			_mapper = TestHelper.CrateMapper();
			_ideasService = new Mock<IIdeasService>();

			_config = new Mock<IOptions<GlobalConfiguration>>();
			_config.Setup(x => x.Value).Returns(new GlobalConfiguration()
			{
				IdeasPageSize = 2
			}
			);

			controller = new IdeasController(_ideasService.Object, _config.Object, _mapper);

			_ideasService.Setup(x => x.List()).Returns(TestHelper.MockAsyncSet(TestHelper.GenerateIdeas(30)).Object);
		}

		[TestMethod]
		public async Task Get_NoPage_ReturnFirst2()
		{
			var response = await controller.Get();

			Assert.IsNotNull(response.Result);
			var result = response.Result as OkObjectResult;

			Assert.AreEqual(200, result.StatusCode);

			var items = result.Value as IEnumerable<IdeaModel>;

			Assert.AreEqual(2, items.Count());
			Assert.IsTrue(items.First().Content.StartsWith("0 - Idea"));
		}

		[TestMethod]
		public async Task Get_WithPage_ReturnSecond2()
		{
			var response = await controller.Get(2);

			Assert.IsNotNull(response.Result);
			var result = response.Result as OkObjectResult;

			Assert.AreEqual(200, result.StatusCode);

			var items = result.Value as IEnumerable<IdeaModel>;

			Assert.AreEqual(2, items.Count());
			Assert.IsTrue(items.First().Content.StartsWith("2 - Idea"));
		}

		[TestMethod]
		public async Task Get_WithTooBigPage_ReturnEmpty()
		{
			var response = await controller.Get(20);

			Assert.IsNotNull(response.Result);
			var result = response.Result as OkObjectResult;

			Assert.AreEqual(200, result.StatusCode);

			var items = result.Value as IEnumerable<IdeaModel>;

			Assert.AreEqual(0, items.Count());
		}

		[TestMethod]
		public async Task Create_NewIdea_Return()
		{
			var idea = new IdeaModel
			{
				Content = "new idea",
				Confidence = 6,
				Ease = 7,
				Impact = 7
			};

			_ideasService.Setup(x => x.Save(It.IsAny<Idea>()))
				.ReturnsAsync((Idea i) =>
				{
					return new Idea
					{
						CreatedAt = DateTime.Now,
						Id = Guid.NewGuid(),
						Confidence = i.Confidence,
						Ease = i.Ease,
						Impact = i.Impact,
						Content = i.Content
					};
				});

			var response = await controller.Create(idea) as ObjectResult;

			Assert.IsNotNull(response);

			Assert.AreEqual(201, response.StatusCode);

			var newIdea = response.Value as IdeaModel;

			Assert.IsTrue(newIdea.CreatedAt != DateTime.MinValue);
			Assert.IsTrue(newIdea.Id != Guid.Empty);
			Assert.AreEqual(idea.Content, newIdea.Content);
			Assert.AreEqual(idea.Confidence, newIdea.Confidence);
			Assert.AreEqual(idea.Ease, newIdea.Ease);
			Assert.AreEqual(idea.Impact, newIdea.Impact);
			Assert.AreEqual((6 + 7 + 7) / 3d, newIdea.AverageScore);

			_ideasService.Verify(x => x.Save(It.IsAny<Idea>()), Times.Once());
		}

		[TestMethod]
		public async Task Create_InvalidModel_ReturnBadRequest()
		{
			controller.ModelState.AddModelError("Ease", "err");

			var response = await controller.Create(null) as BadRequestObjectResult;

			Assert.IsNotNull(response);
			Assert.AreEqual(response.StatusCode, 400);
		}

		[TestMethod]
		public async Task Delete_InvalidModel_ReturnsBadRequest()
		{
			controller.ModelState.AddModelError("Ease", "err");

			var response = await controller.Delete(Guid.NewGuid()) as BadRequestObjectResult;

			Assert.IsNotNull(response);
			Assert.AreEqual(response.StatusCode, 400);
		}

		[TestMethod]
		public async Task Delete_InvalidModel_Returns()
		{
			var id = Guid.NewGuid();
			var response = await controller.Delete(id) as StatusCodeResult;

			Assert.IsNotNull(response);
			Assert.AreEqual(response.StatusCode, 204);

			_ideasService.Verify(x => x.Delete(id), Times.Once());
		}

		[TestMethod]
		public async Task Update_ExistingIdea_Updates()
		{
			var id = Guid.NewGuid();
			var idea = new IdeaModel
			{
				Content = "idea to update",
				Confidence = 6,
				Ease = 7,
				Impact = 9,
			};

			_ideasService.Setup(x => x.Update(It.IsAny<Idea>()))
				.ReturnsAsync((Idea i) =>
				{
					return new Idea
					{
						Id = i.Id,
						Confidence = i.Confidence,
						Ease = i.Ease,
						Impact = i.Impact,
						Content = i.Content
					};
				});

			var response = await controller.Update(id, idea) as OkObjectResult;

			Assert.IsNotNull(response);

			Assert.AreEqual(200, response.StatusCode);

			var updatedIdea = response.Value as IdeaModel;

			Assert.AreEqual(id, updatedIdea.Id);
			Assert.AreEqual(idea.Content, updatedIdea.Content);
			Assert.AreEqual(idea.Confidence, updatedIdea.Confidence);
			Assert.AreEqual(idea.Ease, updatedIdea.Ease);
			Assert.AreEqual(idea.Impact, updatedIdea.Impact);
			Assert.AreEqual((6 + 7 + 9) / 3d, updatedIdea.AverageScore);

			_ideasService.Verify(x => x.Update(It.IsAny<Idea>()), Times.Once());
		}

		[TestMethod]
		public async Task Update_InvalidData_Updates()
		{
			controller.ModelState.AddModelError("err", "err");

			var response = await controller.Update(Guid.NewGuid(), null) as BadRequestObjectResult;

			Assert.IsNotNull(response);

			Assert.AreEqual(400, response.StatusCode);
		}

		[TestMethod]
		public async Task Update_NotFoundIdea_Throws()
		{
			var id = Guid.NewGuid();
			var idea = new IdeaModel
			{
				Content = "idea to update",
				Confidence = 6,
				Ease = 7,
				Impact = 9,
			};

			_ideasService.Setup(x => x.Update(It.IsAny<Idea>()))
				.Throws<InvalidOperationException>();

			var response = await controller.Update(id, idea) as NotFoundResult;

			Assert.IsNotNull(response);

			Assert.AreEqual(404, response.StatusCode);

			_ideasService.Verify(x => x.Update(It.IsAny<Idea>()), Times.Once());
		}

		[TestMethod]
		public async Task Update_UpdateError_Throws()
		{
			var id = Guid.NewGuid();
			var idea = new IdeaModel
			{
				Content = "idea to update",
				Confidence = 6,
				Ease = 7,
				Impact = 9,
			};

			_ideasService.Setup(x => x.Update(It.IsAny<Idea>()))
				.Throws<Exception>();

			var response = await controller.Update(id, idea) as BadRequestObjectResult;

			Assert.IsNotNull(response);

			Assert.AreEqual(400, response.StatusCode);

			_ideasService.Verify(x => x.Update(It.IsAny<Idea>()), Times.Once());
		}
	}
}
