using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using crmweb.Common.Auxiliary;
using crmweb.Models.OfferModels;
using crmweb.Services;

namespace crmweb.Controllers
{
    [Route("api/offer")]
    [ApiController]
    public class OfferController : Controller
    {
        private readonly OfferService OfferService;

        public OfferController(OfferService offerService)
        {
            this.OfferService = offerService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateOffer([FromBody] OfferRequestInfo offerInfo)
        {
            if (!ModelState.IsValid)
                return Json(Result.PrepareFailure(""));
            return Ok(await OfferService.CreateOffer(offerInfo));
        }
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateOffer([FromBody] OfferInfo offerInfo)
        {
            return Ok(await OfferService.UpdateOffer(offerInfo));
        }
        [HttpDelete("delete/{Id}")]
        public async Task<IActionResult> DeleteOffer(int Id)
        {
            return Ok(await OfferService.DeleteOffer(Id));
        }
        [HttpGet]
        [Route("list/{SearchString?}")]
        public async Task<Result<List<OfferItem>>> OfferList(string? SearchString)
        {
            return await OfferService.OfferList(SearchString);
        }
        [HttpGet]
        [Route("InfobyId/{id}")]
        public async Task<Result<OfferInfo>> InfobyId(int id)
        {
            return await OfferService.InfoOfferbyId(id);
        }

    }
}