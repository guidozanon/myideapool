using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyIdeasPool.Core.Domain;
using MyIdeasPool.Core.Models;
using MyIdeasPool.Core.Services;
using MyIdeasPool.WebApi.Configuration;
using MyIdeasPool.WebApi.Controllers;
using MyIdeasPool.WebApi.Models;
using MyIdeasPool.WebApi.Security;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyIdeasPool.Tests.WebApi.Controllers
{
	[TestClass]
	public class UsersControllerTests
	{

		UsersController controller;
		Mock<IUserService> _userService;
		IMapper _mapper;
		Mock<UserManager<UserEntity>> _userManager;
		Mock<ITokenGenerator> _tokenGenerator;
		Mock<IOptions<GlobalConfiguration>> _config;
		Mock<IUserStore<UserEntity>> _userStore;

		[TestInitialize]
		public void Setup()
		{
			_userService = new Mock<IUserService>();
			_mapper = TestHelper.CrateMapper();
			_userStore = new Mock<IUserStore<UserEntity>>();
			_userManager = new Mock<UserManager<UserEntity>>(_userStore.Object, null, null, null, null, null, null, null, null);
			_tokenGenerator = new Mock<ITokenGenerator>();
			_config = new Mock<IOptions<GlobalConfiguration>>();

			controller = new UsersController(_userService.Object, _config.Object, _mapper, _userManager.Object, _tokenGenerator.Object);

			_tokenGenerator.Setup(x => x.Generate(It.IsAny<User>())).Returns(new TokenModel
			{
				Jwt = "token",
				RefreshToken = "refreshtoken"
			});
		}

		[TestMethod]
		public async Task Signup_CreatesUser_ReturnsToken()
		{
			_userManager.Setup(x => x.CreateAsync(It.IsAny<UserEntity>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
			var model = new SignupModel
			{
				Name = "Test User",
				Email = "testuser@test.com",
				Password = "testPassword"
			};

			var result = await controller.Signup(model) as OkObjectResult;

			Assert.IsNotNull(result);
			var token = result.Value as TokenModel;

			Assert.IsNotNull(token);
			Assert.AreEqual("token", token.Jwt);
			Assert.AreEqual("refreshtoken", token.RefreshToken);

			_tokenGenerator.Verify(x => x.Generate(It.IsAny<User>()), Times.Once());
			_userService.Verify(x => x.AddToken("token", TokenType.Token), Times.Once());
			_userService.Verify(x => x.AddToken("refreshtoken", TokenType.RefreshToken), Times.Once());
			_userManager.Verify(x => x.CreateAsync(It.IsAny<UserEntity>(), It.IsAny<string>()), Times.Once());
		}

		//TODO complete tests.
	}
}
