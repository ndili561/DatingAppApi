using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DAL;
using DAL.Model;
using BLL;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using API.DTO;
using Microsoft.AspNetCore.Mvc.Infrastructure;



// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //[Authorize]
    public class AuthController : BaseController<DTO.BaseEntity>
    {
        private readonly IActionDescriptorCollectionProvider _provider;
        private readonly IAuthRepository _auth;
       

        public AuthController(IAuthRepository aut, IActionDescriptorCollectionProvider pro)
        {
            _provider = pro;
            _auth = aut;
            
        }
      
        public IActionResult GetRoutes()
        {
            var routes = _provider.ActionDescriptors.Items.Select(x => new {
                Action = x.RouteValues["Action"],
                Controller = x.RouteValues["Controller"],
                Name = x.AttributeRouteInfo.Name,
                Template = x.AttributeRouteInfo.Template
            }).ToList();
            return Ok(routes);
        }


        [AllowAnonymous]
        [Route("api/auth/register")]
        public async Task<IActionResult> Register(UserDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("add user or password");
            }
            if (await _auth.UserExists(user.UserName))
            {
                return BadRequest("user Exist");
            }
            //var NewUser = Mapper.Map<User>(user);
            //_auth.Register(NewUser,user.PassWord);

            //await _context.Users.AddAsync(NewUser);
            //await _context.Save();
            var addeduser = await _auth.Register(Mapper.Map<User>(user));
            if (addeduser != null)
            {
                return StatusCode(201);
            }
            return BadRequest("error adding user");
            

        }
       
        [Route("api/auth/login/")]
        [HttpOptions, HttpPost]
        [AllowAnonymous]       
        public async Task<IActionResult> Login(UserDTO userdto)
        {
           var token= await GetSingleToken(async () => await _auth.LoginAsync(userdto.UserName,userdto.PassWord));
            if (token != null)
            {
                return Ok(token);
            }
            return Unauthorized();
        }
    }
}
