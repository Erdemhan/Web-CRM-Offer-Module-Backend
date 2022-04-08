using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using crmweb.Common.Auxiliary;
using crmweb.Models.OfferModels;
using crmweb.Data.Entities;
using crmweb.Models.CompanyModels;
using crmweb.Data;
using Microsoft.EntityFrameworkCore;
using crmweb.Models.CompanyContactModels;

namespace crmweb.Services
{
    public class CompanyService
    {
        //Member Variables/////////////////////////////////////////////////////
        private MainDb context;
        private readonly IMapper _mapper;

        //Constructor//////////////////////////////////////////////////////////
        public CompanyService(MainDb Context , IMapper mapper)
        {
            context = Context;
            _mapper = mapper;
        }


        //Public Functions/////////////////////////////////////////////////////


            //Creating Company /////////////////////////////////////////////////////
        public async Task<Result<CompanyItem>> CreateCompany(CompanyRequestInfo company)
        {
            if (CompanyNameExist(company.Name))
            {
                return Result<CompanyItem>.PrepareFailure("Company Name already exist");
            }


            Company vCompany = _mapper.Map<Company>(company);

            try
            {
                context.Companies.Add(vCompany);
                await context.SaveChangesAsync();

                return Result<CompanyItem>.PrepareSuccess(_mapper.Map<CompanyItem>(vCompany));
            }
            catch (Exception e)
            {

                return Result<CompanyItem>.PrepareFailure(e.ToString());
            }
        }


            //Updating Company /////////////////////////////////////////////////////
        public async Task<Result<CompanyInfo>> UpdateCompany(CompanyInfo company)
        {
            if(!CompanyExist(company.Id))
                return Result<CompanyInfo>.PrepareFailure("Company not found");

            if (CompanyNameExist(company.Id,company.Name))
            {
                return Result<CompanyInfo>.PrepareFailure("Company name already exist");
            }
            if (CompanyPrefixExist(company.Id, company.Prefix))
            {
                return Result<CompanyInfo>.PrepareFailure("Company prefix already exist");
            }

            Company vCompany = await context.Companies.Where(c => c.Id == company.Id).FirstOrDefaultAsync();

            context.Companies.Attach(vCompany);

            vCompany = _mapper.Map<CompanyInfo, Company>(company, vCompany);

            try
            {
                await context.SaveChangesAsync();
                return Result<CompanyInfo>.PrepareSuccess(company);
            }
            catch (Exception e)
            {

                return Result<CompanyInfo>.PrepareFailure(e.ToString());
            }
        }


        //Deleting Company/////////////////////////////////////////////////////
        public async Task<Result> DeleteCompany(int id)
        {
            var company = await context.Companies.FindAsync(id);
            if (company == null)
            {
                return Result.PrepareFailure("Company not found");
            }

            try
            {
                context.Companies.Remove(company);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Result.PrepareFailure("Company has related offers , delete them first");
            }
            
            return Result.PrepareSuccess();

        }

        //Get Company List/////////////////////////////////////////////////////
        public async Task<Result<List<CompanyItem>>> GetCompanyList(string SearchString)
        {
            var companyList = new List<Company>();
            if (!string.IsNullOrEmpty(SearchString) && SearchString.Length >= 2)
            {
                companyList = await (from Company in context.Companies
                                     where Company.Name.Contains(SearchString) 
                                     select Company
                                     ).ToListAsync();
                var _company = _mapper.Map<List<CompanyItem>>(companyList);
                return Result<List<CompanyItem>>.PrepareSuccess(_company);
            }
            else
            {
                var company = await context.Companies
                .Select(p => _mapper.Map<CompanyItem>(p))
                .ToListAsync();
                return Result<List<CompanyItem>>.PrepareSuccess(company);
            }
            
        }

        //Get Company by Id ////////////////////////////////////////////////////
        public async Task<Result<List<CompanyInfo>>> GetCompanybyId(int id)
        {
            if (!CompanyExist(id))
            {
                return Result<List<CompanyInfo>>.PrepareFailure("Company not found");
            }

            try
            {
                var company = await context.Companies
                    .Where(c => c.Id == id)
                    .Select(p => _mapper.Map<CompanyInfo>(p)).ToListAsync();

                return Result<List<CompanyInfo>>.PrepareSuccess(company);
            }
            catch (Exception e)
            {
                return Result<List<CompanyInfo>>.PrepareFailure(e.ToString());
            }
        }


        //Private Functions /////////////////////////////////////////////////////

        private bool CompanyExist(int id)
        {
            return context.Companies.Any(p=>p.Id==id);
        }


        private bool CompanyNameExist(string CompanyName)
        {
            return context.Companies.Any(p => p.Name == CompanyName);
        }

        private bool CompanyPrefixExist(int id , string Prefix)
        {
            var company = context.Companies.Where(p => p.Id == id).FirstOrDefault();
            if (company.Prefix == Prefix)
            {
                return false;
            }
            else
            {
                return context.Companies.Any(p => p.Prefix == Prefix);
            }
        }


        private bool CompanyNameExist(int id,string CompanyName)
        {
            var company = context.Companies.Where(p => p.Id == id).FirstOrDefault();
            if (company.Name == CompanyName)
            {
                return false;
            }
            else
            {
                return context.Companies.Any(p=>p.Name==CompanyName);
            }
        }
    }
}
