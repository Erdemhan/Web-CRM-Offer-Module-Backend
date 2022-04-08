using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using crmweb.Common.Auxiliary;
using crmweb.Common.Extensions;
using crmweb.Models.CompanyContactModels;
using crmweb.Models.UserModels;
using crmweb.Services;

namespace crmweb.Controllers
{
    [Route("api/companycontact")]
    [ApiController]
    public class CompanyContactController : Controller
    {
        private readonly CompanyContactService CompanyContactService;

        public CompanyContactController(AuthService AuthService, CompanyContactService CompanyContactService)
        {
            this.CompanyContactService = CompanyContactService;
        }


        [HttpGet]
        [Route("list/{Id}")]
        public async Task<Result<List<CompanyContactInfo>>> GetContactListbyId(int Id)
        {
            return await CompanyContactService.GetContactList(Id);
        }

        [HttpGet]
        [Route("getbyid/{Id}")]
        public async Task<Result<CompanyContactInfo>> GetContactbyId(int Id)
        {
            return await CompanyContactService.GetContactbyId(Id);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateContact([FromBody] CompanyContactRequestInfo contact)
        {
            if (!ModelState.IsValid)
                return Json(Result.PrepareFailure(""));

            return Ok(await CompanyContactService.CreateContact(contact));
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> PutUser([FromBody] CompanyContactInfo contact)
        {
            return Ok(await CompanyContactService.PutContact(contact));
        }


        [HttpDelete("delete/{Id}")]
        public async Task<IActionResult> DeleteContact(int Id)
        {
            return Ok(await CompanyContactService.DeleteContact(Id));
        }

    }
}
