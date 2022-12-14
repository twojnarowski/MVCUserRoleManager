using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCUserRoleManager.Areas.Identity.Data;
using MVCUserRoleManager.Areas.Identity.DataDto;
using MVCUserRoleManager.Extensions;

namespace MVCUserRoleManager.Controllers
{
    public class UserController : Controller
    {
        /// <summary>
        /// User manager for downloading users' info.
        /// </summary>
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// Role manager for downloading users' roles.
        /// </summary>
        private readonly RoleManager<Role> _roleManager;

        public UserController(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        // GET: UsersController
        public async Task<ActionResult> Index()
        {
            List<User> users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                user.RolesForUser = await _userManager.GetRolesAsync(user);
            }

            return View(users);
        }

        // GET: UsersController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return this.NotFound();
            }

            UserDto userDto = AutoMapperConfig.Initialize().Map<User, UserDto>(user);
            ViewBag.Roles = await _roleManager.Roles.ToListAsync();
            ViewBag.UserRoles = await _userManager.GetRolesAsync(user);
            return this.View(userDto);
        }

        // POST: UsersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, UserDto userDto)
        {
            if (id != userDto.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                var existingUser = await this._userManager.FindByIdAsync(id);
                if (existingUser != null)
                {
                    existingUser.FirstName = userDto.FirstName;
                    existingUser.LastName = userDto.LastName;
                    await this._userManager.UpdateAsync(existingUser);
                    return this.RedirectToAction(nameof(this.Index));
                }
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return this.NotFound();
            }
            ViewBag.Roles = await _roleManager.Roles.ToListAsync();
            ViewBag.UserRoles = await _userManager.GetRolesAsync(user);
            return this.View(userDto);
        }

        // GET: UsersController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return this.NotFound();
            }

            return this.View(user);
        }

        /// <summary>
        /// POST: UsersController/Delete/5.
        /// </summary>
        /// <param name="id">Id of the <see cref="User"/> to be deleted.</param>
        /// <returns>Index when successful.</returns>
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await this._userManager.FindByIdAsync(id);
            if (user != null)
            {
                await this._userManager.DeleteAsync(user);
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: UsersController/AddRole/5
        public async Task<ActionResult> AddRole(string roleId, string userId)
        {
            if (roleId == null || userId == null)
            {
                return this.NotFound();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return this.NotFound();
            }

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return this.NotFound();
            }

            await this._userManager.AddToRoleAsync(user, role.Name);

            return this.Ok();
        }

        // GET: UsersController/RemoveRole/5
        public async Task<ActionResult> RemoveRole(string roleId, string userId)
        {
            if (roleId == null || userId == null)
            {
                return this.NotFound();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return this.NotFound();
            }

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return this.NotFound();
            }

            await this._userManager.RemoveFromRoleAsync(user, role.Name);

            return this.Ok();
        }
    }
}