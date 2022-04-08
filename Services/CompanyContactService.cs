using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using crmweb.Common.Auxiliary;
using crmweb.Data;
using crmweb.Data.Entities;
using crmweb.Models.CompanyContactModels;
using crmweb.Models.UserModels;

namespace crmweb.Services
{
    public class CompanyContactService
    {
        //Member Variables/////////////////////////////////////////////////////
        private readonly MainDb _context;
        private readonly IMapper _mapper;

        //Constructor//////////////////////////////////////////////////////////
        public CompanyContactService(MainDb context , IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //Public Functions/////////////////////////////////////////////////////


            //Creating Contact/////////////////////////////////////////////////////
        public async Task<Result<CompanyContactInfo>> CreateContact(CompanyContactRequestInfo contact)
        {
            if (!CompanyExists(contact.CompanyId))
            {
                return Result<CompanyContactInfo>.PrepareFailure("Company not found");
            }

            CompanyContact vCompanyContact = _mapper.Map<CompanyContact>(contact);

            _context.CompanyContacts.Add(vCompanyContact);

            try
            {
                await _context.SaveChangesAsync();
                CompanyContactInfo info = _mapper.Map<CompanyContactInfo>(vCompanyContact);
                return Result<CompanyContactInfo>.PrepareSuccess(info);

            }
            catch (Exception e)
            {
                return Result<CompanyContactInfo>.PrepareFailure(e.ToString());
            }
        }


        public async Task<Result<CompanyContactInfo>> GetContactbyId(int Id)
        {

            var data = await _context.CompanyContacts.
                Where(u => u.Id == Id)
                .Select(u => _mapper.Map<CompanyContactInfo>(u))
                .FirstOrDefaultAsync();

            try
            {
                return Result<CompanyContactInfo>.PrepareSuccess(data);

            }
            catch (Exception e)
            {
                return Result<CompanyContactInfo>.PrepareFailure(e.ToString());
            }
        }

        //Get Contacts List by Id ////////////////////////////////////////////////////
        public async Task<Result<List<CompanyContactInfo>>> GetContactList(int companyId)
        {
            if (!CompanyExists(companyId))
            {
                return Result<List< CompanyContactInfo >>.PrepareFailure("Company not found");
            }

            try
            {
                var vData = await _context.CompanyContacts.
                    Where(c => c.CompanyId == companyId)
                    .Select(u => _mapper.Map<CompanyContactInfo>(u))
                    .ToListAsync();

                return Result<List<CompanyContactInfo>>.PrepareSuccess(vData);

            }
            catch (Exception e)
            {
                return Result<List<CompanyContactInfo>>.PrepareFailure(e.ToString());
            }
        }


        //Updating Contact/////////////////////////////////////////////////////
        public async Task<Result<CompanyContactInfo>> PutContact(CompanyContactInfo contact)
        {
            if (!CompanyExists(contact.CompanyId))
                return Result<CompanyContactInfo>.PrepareFailure("Company not found");
            if (!ContactExists(contact.Id))
                return Result<CompanyContactInfo>.PrepareFailure("Contact not found");

            CompanyContact vCompanyContact =await 
                _context.CompanyContacts.Where(c => c.Id == contact.Id).FirstOrDefaultAsync();

            _context.CompanyContacts.Attach(vCompanyContact);

            vCompanyContact = _mapper.Map<CompanyContactInfo, CompanyContact>(contact, vCompanyContact);

            try
            {
                await _context.SaveChangesAsync();
                return Result<CompanyContactInfo>.PrepareSuccess(contact);
            }
            catch (Exception e)
            {
                return Result<CompanyContactInfo>.PrepareFailure(e.ToString());
            }
        }


            //Deleting Contact/////////////////////////////////////////////////////
        public async Task<Result> DeleteContact(int id)
        {
            var contact = await _context.CompanyContacts.FindAsync(id);
            if (contact == null)
            {
                return Result.PrepareFailure("Contact not found");
            }

            _context.CompanyContacts.Remove(contact);
            await _context.SaveChangesAsync();

            return Result.PrepareSuccess("Contact Deleted");
        }


        //Private Functions/////////////////////////////////////////////////////
        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }

        private bool ContactExists(int id)
        {
            return _context.CompanyContacts.Any(e => e.Id == id);
        }

    }
}
