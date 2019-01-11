using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Moq;
using MyIdeasPool.Core;
using MyIdeasPool.Core.Models;
using MyIdeasPool.WebApi;

namespace MyIdeasPool.Tests
{
	class TestHelper
	{

		public static IMapper CrateMapper()
		{
			return new MapperConfiguration(m =>
			{
				m.AddProfile<WebApiMappingProfile>();
				m.AddProfile<CoreMappingProfile>();
			}
			).CreateMapper();
		}

		internal static List<Idea> GenerateIdeas(int count)
		{
			var result = new List<Idea>();
			var rand = new Random();

			for (int i = 0; i < count; i++)
			{
				var id = Guid.NewGuid();
				result.Add(new Idea
				{
					Id = id,
					Confidence = rand.Next(0, 10),
					Ease = rand.Next(0, 10),
					Impact = rand.Next(0, 10),
					CreatedAt = DateTime.Now,
					Content = $"{i} - Idea {id}"
				});
			}

			return result;
		}

		public static Mock<DbSet<T>> MockAsyncSet<T>(IList<T> list)
			where T : class
		{
			var mockList = list.AsQueryable();

			var mockSet = new Mock<DbSet<T>>();
			mockSet.As<IAsyncEnumerable<T>>()
				.Setup(m => m.GetEnumerator())
				.Returns(new TestAsyncEnumerator<T>(mockList.GetEnumerator()));

			mockSet.As<IQueryable<T>>()
			   .Setup(m => m.Provider)
			   .Returns(new TestAsyncQueryProvider<T>(mockList.Provider));

			mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(mockList.Expression);
			mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(mockList.ElementType);
			mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(mockList.GetEnumerator());

			return mockSet;
		}
	}

	internal class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
	{
		private readonly IQueryProvider _inner;

		internal TestAsyncQueryProvider(IQueryProvider inner)
		{
			_inner = inner;
		}

		public IQueryable CreateQuery(Expression expression)
		{
			switch (expression)
			{
				case MethodCallExpression m:
					{
						var resultType = m.Method.ReturnType; // it shoud be IQueryable<T>
						var tElement = resultType.GetGenericArguments()[0];
						var queryType = typeof(TestAsyncEnumerable<>).MakeGenericType(tElement);
						return (IQueryable)Activator.CreateInstance(queryType, expression);
					}
			}
			return new TestAsyncEnumerable<TEntity>(expression);
		}

		public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
		{
			return new TestAsyncEnumerable<TElement>(expression);
		}

		public object Execute(Expression expression)
		{
			return _inner.Execute(expression);
		}

		public TResult Execute<TResult>(Expression expression)
		{
			return _inner.Execute<TResult>(expression);
		}

		public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
		{
			return Task.FromResult(Execute(expression));
		}

		public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
		{
			return Task.FromResult(Execute<TResult>(expression));
		}

		public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
		{
			return _inner.Execute<IAsyncEnumerable<TResult>>(expression);
		}
	}

	internal class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
	{
		public TestAsyncEnumerable(IEnumerable<T> enumerable)
			: base(enumerable)
		{ }

		public TestAsyncEnumerable(Expression expression)
			: base(expression)
		{ }

		public IAsyncEnumerator<T> GetAsyncEnumerator()
		{
			return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
		}

		public IAsyncEnumerator<T> GetEnumerator()
		{
			return GetAsyncEnumerator();
		}

		IQueryProvider IQueryable.Provider
		{
			get { return new TestAsyncQueryProvider<T>(this); }
		}
	}

	internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
	{
		private readonly IEnumerator<T> _inner;

		public TestAsyncEnumerator(IEnumerator<T> inner)
		{
			_inner = inner;
		}

		public void Dispose()
		{
			_inner.Dispose();
		}

		public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
		{
			return Task.FromResult(_inner.MoveNext());
		}

		public Task<bool> MoveNext(CancellationToken cancellationToken)
		{
			return Task.FromResult(_inner.MoveNext());
		}

		public T Current
		{
			get { return _inner.Current; }
		}
	}
}
