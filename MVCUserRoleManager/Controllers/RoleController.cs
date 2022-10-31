using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCUserRoleManager.Areas.Identity.Data;

namespace MVCUserRoleManager.Controllers
{
    public class RoleController : Controller
    {
        /// <summary>
        /// User manager for downloading users' info.
        /// </summary>
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// Role manager for downloading users' roles.
        /// </summary>
        private readonly RoleManager<Role> _roleManager;

        public RoleController(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: RolesController
        public async Task<ActionResult> Index()
        {
            List<Role> roles = null!;

            roles = await _roleManager.Roles.ToListAsync();

            return View(roles);
        }

        // GET: RolesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RolesController/Create
        public async Task<ActionResult> Create()
        {
            return this.View();
        }

        // POST: RolesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RoleDto roleDto)
        {
            if (this.ModelState.IsValid)
            {
                Role role = IdentityAutoMapperConfig.Initialize().Map<RoleDto, Role>(roleDto);

                var duplicateRole = await this._roleManager.FindByNameAsync(role.Name);
                if (duplicateRole is null)
                {
                    role.Id = Guid.NewGuid().ToString();
                    await this._roleManager.CreateAsync(role);
                    return this.RedirectToAction(nameof(this.Index));
                }
            }

            return this.View(roleDto);
        }

        // GET: RolesController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return this.NotFound();
            }

            RoleDto roleDto = IdentityAutoMapperConfig.Initialize().Map<Role, RoleDto>(role);
            return this.View(roleDto);
        }

        // POST: RolesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, RoleDto roleDto)
        {
            if (id != roleDto.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                var existingRole = await this._roleManager.RoleExistsAsync(roleDto.Name);
                if (!existingRole)
                {
                    var exist = await this._roleManager.FindByIdAsync(roleDto.Id);
                    exist.Name = roleDto.Name;
                    exist.Badge = roleDto.Badge;
                    await this._roleManager.UpdateAsync(exist);
                    return this.RedirectToAction(nameof(this.Index));
                }
            }

            return this.View(roleDto);
        }

        // GET: RolesController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return this.NotFound();
            }

            return this.View(role);
        }

        /// <summary>
        /// POST: RolesController/Delete/5.
        /// </summary>
        /// <param name="id">Id of the <see cref="Role"/> to be deleted.</param>
        /// <returns>Index when successful.</returns>
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                this._roleManager.DeleteAsync(role);
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}