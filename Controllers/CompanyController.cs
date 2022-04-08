using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using crmweb.Common.Auxiliary;
using crmweb.Models.CompanyModels;
using crmweb.Services;

namespace crmweb.Controllers
{
    [Route("api/company")]
    [ApiController]
    public class CompanyController : Controller
    {
        private readonly CompanyService CompanyService;

        public CompanyController(CompanyService companyService)
        {
            this.CompanyService = companyService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyRequestInfo companyInfo)
        {
            if (!ModelState.IsValid)
                return Json(Result.PrepareFailure(""));
            return Ok(await CompanyService.CreateCompany(companyInfo));
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateCompany([FromBody] CompanyInfo companyInfo)
        {
            return Ok(await CompanyService.UpdateCompany(companyInfo));
        }

        [HttpDelete]
        [Route("delete/{Id}")]
        public async Task<IActionResult> DeleteCompany(int Id)
        {
            return Ok(await CompanyService.DeleteCompany(Id));
        }

        [HttpGet]
        [Route("list/{SearchString?}")]
        public async Task<Result<List<CompanyItem>>> GetCompanyList(string? SearchString)
        {
            return await CompanyService.GetCompanyList(SearchString);
        }

        [HttpGet]
        [Route("getbyId/{Id}")]
        public async Task<Result<List<CompanyInfo>>> GetCompanybyId(int Id)
        {
            return await CompanyService.GetCompanybyId(Id);
        }

    }
}
