using Dapper;
using Infrastructure.Core.Interfaces;
using Infrastructure.Core.Persistence;
using Microsoft.Extensions.Logging;
using Server.Broker.Infrastructure.Entities;
using Server.Broker.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Broker.Repositories
{
	public class GpsDataRepository : CoreRepository<GpsData>, IGpsDataRepository
	{
		private const string SQLQUERY_SELECT_LAST = "SELECT TOP (1) * FROM [GpsData] WHERE [DeviceName] = @DeviceName ORDER BY CreateAt DESC";
		private const string SQLQUERY_SELECT_ALL = "SELECT * FROM [GpsData] WHERE [DeviceName] = @DeviceName ORDER BY CreateAt DESC";

		public GpsDataRepository(IDatabaseConnectionFactory connectionFactory, ILogger<CoreRepository<GpsData>> logger)
			: base(connectionFactory, logger)
		{
		}

		public override Task<int> DeleteAsync(GpsData entity)
		{
			throw new NotImplementedException();
		}

		public override Task<int> DeletesAsync(GpsData[] entities)
		{
			throw new NotImplementedException();
		}

		public override Task<Guid?> InsertAsync(GpsData entity)
		{
			return CoreInsertAsync(entity);
		}

		public override Task<int> InsertsAsync(GpsData[] entities)
		{
			throw new NotImplementedException();
		}

		public override Task<IEnumerable<GpsData>> QueryMultipleAsync(GpsData query)
		{
			var connection = connectionFactory.GetConnection();
			return connection.QueryAsync<GpsData>(SQLQUERY_SELECT_ALL, query);
		}

		public override Task<IEnumerable<GpsData>> QueryMultipleByIdAsync(Guid[] ids)
		{
			throw new NotImplementedException();
		}

		public override Task<GpsData> QueryOneAsync(GpsData query)
		{
			throw new NotImplementedException();
		}

		public Task<GpsData> QueryLastOneAsync(GpsData query)
		{
			var connection = connectionFactory.GetConnection();
			return connection.QueryFirstOrDefaultAsync<GpsData>(SQLQUERY_SELECT_LAST, query);
		}

		public override Task<GpsData> QueryOneByIdAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public override Task<int> UpdateAsync(GpsData entity)
		{
			throw new NotImplementedException();
		}

		public override Task<int> UpdatesAsync(GpsData[] entities)
		{
			throw new NotImplementedException();
		}
	}
}
