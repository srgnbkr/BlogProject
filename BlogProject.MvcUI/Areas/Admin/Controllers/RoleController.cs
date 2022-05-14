using AutoMapper;
using BlogProject.Entities.Concrete;
using BlogProject.Entities.DTOs.RoleDto;
using BlogProject.Entities.DTOs.UserDto;
using BlogProject.MvcUI.Areas.Admin.Models.UsersModels;
using BlogProject.MvcUI.Helpers.Abstract;
using BlogProject.Services.Constants;
using BlogProject.Shared.Utilities.Extensions;
using BlogProject.Shared.Utilities.Results.ComplexTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlogProject.MvcUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class RoleController : BaseController
    {
        #region Variables
        private readonly RoleManager<Role> _roleManager;
        #endregion

        #region Constructor
        public RoleController(
            RoleManager<Role> roleManager,
            UserManager<User> userManager, IMapper mapper,
            IImageHelper imageHelper) : base(userManager, mapper, imageHelper)
        {
            _roleManager = roleManager;
        }

        #endregion


        #region Index
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(new RoleListDto { Roles = roles });
        }
        #endregion

        #region GetAllRoles
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var roleListDto = JsonSerializer.Serialize(new RoleListDto { Roles = roles });
            return Json(roleListDto);


        }
        #endregion

        #region Assign
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> Assign(int userId)
        {
            //kullanıcıları aldık
            var user = await UserManager.Users.SingleOrDefaultAsync(x => x.Id == userId);
            //rolleri aldık
            var roles = await _roleManager.Roles.ToListAsync();
            //kullanıcının sahip olduğu rolleri aldık
            var userRoles = await UserManager.GetRolesAsync(user);

            UserRoleAssignDto userRoleAssignDto = new UserRoleAssignDto
            {
                UserId = user.Id,
                UserName = user.UserName,
            };
            foreach (var role in roles)
            {
                RoleAssignDto roleAssignDto = new RoleAssignDto
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    HasRole = userRoles.Contains(role.Name)
                };
                userRoleAssignDto.RoleAssignDtos.Add(roleAssignDto);
            }
            return PartialView("_RoleAssignPartial", userRoleAssignDto);

            
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult>  Assign(UserRoleAssignDto userRoleAssignDto)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.Users.SingleOrDefaultAsync(x => x.Id == userRoleAssignDto.UserId);
                foreach (var roleAssignDto in userRoleAssignDto.RoleAssignDtos)
                {
                    if (roleAssignDto.HasRole)
                        await UserManager.AddToRoleAsync(user, roleAssignDto.RoleName);
                    else
                        await UserManager.RemoveFromRoleAsync(user, roleAssignDto.RoleName);
                }

                await UserManager.UpdateSecurityStampAsync(user);

                var userRoleAssignAjaxViewModel = JsonSerializer.Serialize(new UserRoleAssignAjaxViewModel
                {
                    UserDto = new UserDto
                    {
                        User = user,
                        Message =Messages.Role.RoleAssign,
                        ResultStatus = ResultStatus.Success
                    },
                    RoleAssignPartial = await this.RenderViewToStringAsync("_RoleAssignPartial", userRoleAssignDto)
                });
                return Json(userRoleAssignAjaxViewModel);
            }
            else
            {
                var userRoleAssignAjaxErrorModel = JsonSerializer.Serialize(new UserRoleAssignAjaxViewModel
                {
                    RoleAssignPartial = await this.RenderViewToStringAsync("_RoleAssignPartial", userRoleAssignDto),
                    UserRoleAssignDto = userRoleAssignDto
                });
                return Json(userRoleAssignAjaxErrorModel);
            }

                
            
            
            
        }



        #endregion
    }
}