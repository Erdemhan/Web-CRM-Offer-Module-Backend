using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using crmweb.Common.Auxiliary;
using crmweb.Data;
using crmweb.Data.Entities;
using crmweb.Models.UserModels;


namespace crmweb.Services
{
    public class UserService
    {
        //Member Variables/////////////////////////////////////////////////////
        private readonly MainDb _context;
        private readonly IMapper _mapper;

        //Constructor//////////////////////////////////////////////////////////
        public UserService(MainDb context , IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //Public Functions/////////////////////////////////////////////////////


            //Creating User/////////////////////////////////////////////////////
        public async Task<Result<UserInfo>> CreateUser (UserRequestInfo user)
        {
            if (UserNameExists(user.UserName))
            {
                return Result<UserInfo>.PrepareFailure("Username Already Exist");
            }

            User vUser = _mapper.Map<User>(user);

            _context.Users.Add(vUser);

            try
            {
                await _context.SaveChangesAsync();

                UserInfo info = _mapper.Map<UserInfo>(vUser);
                return Result<UserInfo>.PrepareSuccess(info);

            }
            catch (Exception e)
            {
                return Result<UserInfo>.PrepareFailure(e.ToString());
            }
        }

            //Get user by Id/////////////////////////////////////////////////////
        public async Task<Result<List<UserInfo>>> GetUserbyId(int Id)
        {
            if (!UserExists(Id))
                return Result<List<UserInfo>>.PrepareFailure("User not found");

            var data = await _context.Users.
                Where(u => u.Id == Id)
                .Select(u => _mapper.Map<UserInfo>(u))
                .ToListAsync();

            try
            {
                return Result<List<UserInfo>>.PrepareSuccess(data);

            }
            catch (Exception e)
            {
                return Result<List<UserInfo>>.PrepareFailure(e.ToString());
            }
        }


        //Listing User/////////////////////////////////////////////////////
        public async Task<Result<List<UserInfo>>> GetUserList()
        {

            var data = await _context.Users
                .Select(u => _mapper.Map<UserInfo>(u))
                .ToListAsync();

            try
            {
                return Result<List<UserInfo>>.PrepareSuccess(data);

            }
            catch (Exception e)
            {
                return Result<List<UserInfo>>.PrepareFailure(e.ToString());
            }
        }

            //Updating User/////////////////////////////////////////////////////

        public async Task<Result<UserInfo>> PutUser( UserUpdateInfo user)
        {
            if (!UserExists(user.Id))
            {
                return Result<UserInfo>.PrepareFailure("User not found");
            }
            if (UserNameExists(user.Id,user.UserName))
            {
                return Result<UserInfo>.PrepareFailure("Username already exist");
            }



            User currentUser = await _context.Users
                .Where(u => u.Id == user.Id)
                .FirstOrDefaultAsync();

            _context.Users.Attach(currentUser);

            _mapper.Map<UserUpdateInfo,User>(user,currentUser);

            try
            {
                await _context.SaveChangesAsync();
                UserInfo info = _mapper.Map<UserInfo>(currentUser);

                return Result<UserInfo>.PrepareSuccess(info);
            }
            catch (DbUpdateConcurrencyException e)
            {
 
                    return Result<UserInfo>.PrepareFailure(e.ToString());

            }

        }

            //Deleting User/////////////////////////////////////////////////////
        public async Task<Result> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return Result.PrepareFailure("User not found");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Result.PrepareSuccess("User Deleted");
        }


        public async Task<Result<UserInfo>> ActivateUser(int UId)
        {
            if (!UserExists(UId))
            {
                return Result<UserInfo>.PrepareFailure("User not found");
            }


            User currentUser = await _context.Users
                .Where(u => u.Id == UId)
                .FirstOrDefaultAsync();

            _context.Users.Attach(currentUser);


            if (currentUser.IsActive==false)
                currentUser.IsActive = true;
            else
                currentUser.IsActive = false;


            try
            {
                await _context.SaveChangesAsync();
                UserInfo info = _mapper.Map<UserInfo>(currentUser);

                return Result<UserInfo>.PrepareSuccess(info);
            }
            catch (DbUpdateConcurrencyException e)
            {

                return Result<UserInfo>.PrepareFailure(e.ToString());

            }

        }


        public async Task<Result<UserInfo>> ChangeUserRole(int UId)
        {
            if (!UserExists(UId))
            {
                return Result<UserInfo>.PrepareFailure("User not found");
            }


            User currentUser = await _context.Users
                .Where(u => u.Id == UId)
                .FirstOrDefaultAsync();

            _context.Users.Attach(currentUser);


            if (currentUser.Role == 1)
                currentUser.Role = 2;
            else
                currentUser.Role = 1;


            try
            {
                await _context.SaveChangesAsync();
                UserInfo info = _mapper.Map<UserInfo>(currentUser);

                return Result<UserInfo>.PrepareSuccess(info);
            }
            catch (DbUpdateConcurrencyException e)
            {

                return Result<UserInfo>.PrepareFailure(e.ToString());

            }

        }



        //Private Functions////////////////////////////////////////////////////


        //MD5 Hash Function for password /////////////////////////////////////////////////////
        private static string CreateMD5(string password)
        {
            // Use input string to calculate MD5 hash
            var md5 = MD5.Create();

            byte[] inputBytes = Encoding.ASCII.GetBytes(password);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }

            return sb.ToString();
                       
        }

            //User Exist Check /////////////////////////////////////////////////////
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
        private bool UserNameExists(string UserName)
        {
            return _context.Users.Any(e=> e.UserName == UserName);
        }

        private bool UserNameExists(int Id , string UserName)
        {
            var user = _context.Users.Where(u => u.Id == Id).FirstOrDefault();
            if (user.UserName == UserName)
            {
                return false;
            }
            else
            {
                return _context.Users.Any(e => e.UserName == UserName);

            }
        }

    }
}
