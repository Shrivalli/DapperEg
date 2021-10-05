using Dapper;
using DapperEg.Models;
using DapperEg.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DapperEg.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IDapper _dapper;
        public HomeController(IDapper dapper)
        {
            _dapper = dapper;
        }
        [HttpPost(nameof(Create))]
        public async Task<int> Create(Parameters data)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("Id", data.Id, DbType.Int32);
            dbparams.Add("Name", data.Name, DbType.String);
            dbparams.Add("Age", data.Age, DbType.Int32);
            var result = await Task.FromResult(_dapper.Insert<int>("[dbo].[SP_Add_Article]"
                , dbparams,
                commandType: CommandType.StoredProcedure));
            return result;
        }
        [HttpGet(nameof(GetById))]
        public async Task<Parameters> GetById(int Id)
        {
            var result = await Task.FromResult(_dapper.Get<Parameters>($"Select * from MyArticle where Id = {Id}", null, commandType: CommandType.Text));
            return result;
        }
        [HttpDelete(nameof(Delete))]
        public async Task<int> Delete(int Id)
        {
            var result = await Task.FromResult(_dapper.Execute($"Delete MyArticle Where Id = {Id}", null, commandType: CommandType.Text));
            return result;
        }
        [HttpGet(nameof(Count))]
        public async Task<int> Count(int num)
        {
            int totalcount = await Task.FromResult(_dapper.Get<int>($"select COUNT(*) from MyArticle WHERE Age like '%{num}%'", null,
                    commandType: CommandType.Text));
            return totalcount;
        }
        [HttpPatch(nameof(Update))]
        public async Task<int> Update(Parameters data)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Id", data.Id);
            dbPara.Add("Name", data.Name, DbType.String);
            dbPara.Add("Age", data.Age, DbType.Int32);
            int updateArticle = await Task.FromResult(_dapper.Update<int>("[dbo].[SP_Update_Article]",
                            dbPara,
                            commandType: CommandType.StoredProcedure));
            return updateArticle;
        }
    }
}

