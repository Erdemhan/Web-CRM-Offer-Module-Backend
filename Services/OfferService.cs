using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using crmweb.Common.Auxiliary;
using crmweb.Data;
using crmweb.Data.Entities;
using crmweb.Models.OfferModels;

namespace crmweb.Services
{
    public class OfferService
    {
        private MainDb context;
        private readonly IMapper _mapper;

        public OfferService(MainDb Context, IMapper mapper)
        {
            this.context = Context;
            this._mapper = mapper;

        }


        //// CreateOffer ////////////////////////////////////////////////////////////

        public async Task<Result<OfferInfo>> CreateOffer(OfferRequestInfo offerRequestInfo)
        {

            try
            {
                var _offerRequestInfo = _mapper.Map<OfferHeader>(offerRequestInfo);

                _offerRequestInfo.CreateTime = DateTime.Now;

                _offerRequestInfo.State = 1;
                _offerRequestInfo.OfferNo = OfferCount(_offerRequestInfo.CompanyId) + 1;
                _offerRequestInfo.RevisionNo = 0;

                _offerRequestInfo.ReleaseDate = null;
                _offerRequestInfo.ValidationDate = null;


                context.OfferHeaders.Add(_offerRequestInfo);
                await context.SaveChangesAsync();
                var offerInfo = _mapper.Map<OfferInfo>(_offerRequestInfo);
                return Result<OfferInfo>.PrepareSuccess(offerInfo);
            }
            catch (Exception e)
            {
                return Result<OfferInfo>.PrepareFailure(e.ToString());
            }
        }


        ////// UpdateOffer /////////////////////////////////////////////


        public async Task<Result<OfferInfo>> UpdateOffer(OfferInfo offerInfo)
        {
            if (offerInfo.State == 2)
            {
                return await Revision(offerInfo);
            }
            else
            {
                return await OfferUpdateFonksiyon(offerInfo);
            }
        }


        /////// DeleteOffer //////////////////////////////////////////////////////////////


        public async Task<Result> DeleteOffer(int id)
        {
            var _offer = await context.OfferHeaders.FindAsync(id);
            if (_offer == null)
            {
                return Result.PrepareFailure("Offer not found");
            }
            context.OfferHeaders.Remove(_offer);
            await context.SaveChangesAsync();
            return Result.PrepareSuccess();
        }


        ///// InfoOfferbyIdOffer //////////////////////////////////////////////////////////////


        public async Task<Result<OfferInfo>> InfoOfferbyId(int id)
        {
            try
            {
                if (!OfferExist(id))
                {
                    return Result<OfferInfo>.PrepareFailure("Offer header not found");
                }

                var offerHeader = await context.OfferHeaders
                    .Where(p => p.Id == id)
                    .Include(p => p.OfferDetail)
                    .Select(p => _mapper.Map<OfferInfo>(p))
                    .FirstOrDefaultAsync();

                offerHeader.OfferCode = CreateOfferNo(offerHeader);
              
                return Result<OfferInfo>.PrepareSuccess(offerHeader);
            }
            catch (Exception e)
            {
                return Result<OfferInfo>.PrepareFailure(e.ToString());
            }
        }


        ////// Offer List /////////////////////////////////////////////////////////


        public async Task<Result<List<OfferItem>>> OfferList(string SearchString)
        {
            if (!string.IsNullOrEmpty(SearchString) && SearchString.Length >= 2)
            {
                var offerList = await context.OfferHeaders.Include(o=>o.OfferCompany).Include(o=>o.OfferCompanyContact).Where(o=>o.Header.Contains(SearchString)).ToListAsync();
                var _offerList = _mapper.Map<List<OfferItem>>(offerList);
                return Result<List<OfferItem>>.PrepareSuccess(_offerList);
            }
            else
            {
                var offerHeader = await context.OfferHeaders.Include(o=>o.OfferCompany).Include(o=>o.OfferCompanyContact).ToListAsync();
                var vOfferItem = _mapper.Map<List<OfferItemTransfer>>(offerHeader);
                var offerInfo = _mapper.Map<List<OfferItem>>(vOfferItem
                    .Select(o => {
                        o.OfferNo = CreateOfferNo(o);
                        return o;
                    }));
                return Result<List<OfferItem>>.PrepareSuccess(offerInfo);
            }
        }
        //Update Offer Fonksiyon////////////////////////////////////////

        public async Task<Result<OfferInfo>> OfferUpdateFonksiyon(OfferInfo offerInfo)
        {
            try
            {
                if (!OfferExist(offerInfo.Id))
                {
                    return Result<OfferInfo>.PrepareFailure("Offer header not found");
                }

                OfferHeader offerHeader = await context.OfferHeaders.Include(p => p.OfferDetail)
                    .Where(p => p.Id == offerInfo.Id).FirstOrDefaultAsync();
                context.OfferHeaders.Attach(offerHeader);
                offerHeader = _mapper.Map<OfferInfo, OfferHeader>(offerInfo, offerHeader);
                offerHeader.ReleaseDate = null;
                offerHeader.ValidationDate = null;
                await context.SaveChangesAsync();
                return Result<OfferInfo>.PrepareSuccess(offerInfo);
            }
            catch (Exception e)
            {
                return Result<OfferInfo>.PrepareFailure(e.ToString());
            }
        }

        //Offer Revision ////////////////////////////////////////////////
        public async Task<Result<OfferInfo>> Revision(OfferInfo offerInfo)
        {
            try
            {
                offerInfo.ReleaseDate = null;
                offerInfo.ValidationDate = null;
                offerInfo.Id = 0;
                offerInfo.OfferDetail.Select(d => d.Id = null).ToList();

                var offerHeader = _mapper.Map<OfferHeader>(offerInfo);

                offerHeader.CreateTime = DateTime.Now;
                offerHeader.RevisionNo++;

                context.OfferHeaders.Add(offerHeader);
                await context.SaveChangesAsync();
                var _offerHeader = _mapper.Map<OfferInfo>(offerHeader);
                return Result<OfferInfo>.PrepareSuccess(_offerHeader);
            }
            catch (Exception e)
            {

                return Result<OfferInfo>.PrepareFailure(e.ToString());
            }
        }

        /////////////////////////////////////////////////////////////////
        private bool OfferExist(int id)
        {
            return context.OfferHeaders.Any(p => p.Id == id);
        }

        private string CreateOfferNo(OfferInfo info)
        {
            var prefix = context.Companies
                .Where(c => c.Id == info.CompanyId)
                .Select(c => c.Prefix)
                .FirstOrDefault();

            return (prefix + "." + info.OfferNo + "." + info.RevisionNo);
        }

        private string CreateOfferNo(OfferItemTransfer info)
        {
            var prefix = context.Companies
                .Where(c => c.Id == info.CompanyId)
                .Select(c => c.Prefix)
                .FirstOrDefault();

            return (prefix + "." + info.OfferNo + "." + info.RevisionNo);
        }

        private int OfferCount(int companyId)
        {
            return context.OfferHeaders
                .Where(o => o.CompanyId == companyId)
                .Count();
        }
    }
}