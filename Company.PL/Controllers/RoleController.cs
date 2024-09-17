using AutoMapper;
using Company.DAL.Models;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;

        public RoleController(RoleManager<IdentityRole> roleManager ,IMapper mapper)
        {
            this.roleManager = roleManager;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index(string name)
        {
            if(string.IsNullOrEmpty(name))
            {
                var role = await roleManager.Roles.Select(u => new RoleViewModel
                {
                    Id = u.Id,
                    RoleName = u.Name
                }).ToListAsync();

                return View(role);
            }
            else
            {
                var role = await roleManager.FindByNameAsync(name);
                if(role is not null)
                {
                    var mapedrole = new RoleViewModel()
                    {
                        Id = role.Id,
                        RoleName = role.Name
                    };
                    return View(new List<RoleViewModel>() { mapedrole });
                }
                else {
                    return View(new List<RoleViewModel>());
                }
            }

          
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel roleVM)
        {
            if(ModelState.IsValid)
            {
                var maperRole=mapper.Map<RoleViewModel,IdentityRole>(roleVM);
                await roleManager.CreateAsync(maperRole);

                return RedirectToAction(nameof(Index));


            }
            return View(roleVM);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id is null)
                return BadRequest();
            var employee = await roleManager.FindByIdAsync(id);
            if (employee is null)
                return NotFound();
            var roleMap = mapper.Map<IdentityRole, RoleViewModel>(employee);
            return View(roleMap);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {

            if (id is null)
                return BadRequest();
            var employee = await roleManager.FindByIdAsync(id);
            if (employee is null)
                return NotFound();
            var roleMap = mapper.Map<IdentityRole, RoleViewModel>(employee);

            return View(roleMap);
        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleViewModel roleVM)
        {
            if (id != roleVM.Id)
                return BadRequest(id);
            if (ModelState.IsValid)
            {
                try
                {
                    var role = await roleManager.FindByIdAsync(id);
                    role.Name = roleVM.RoleName;
                  
                    //user.Email=userVM.Email;
                    //user.SecurityStamp=Guid.NewGuid().ToString();
                    var res = await roleManager.UpdateAsync(role);
                    //var mapperemp = mapper.Map<EmployeeViewModel, Employee>(userVM); unTracked
                    //  mapperemp.ImageName = DocumentSettings.UploadImage(employeeVM.Image, "Images");
                    //   var mapperemp = mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                    return RedirectToAction(nameof(Index));

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(roleVM);

        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (id is null)
                return BadRequest();
            var employee = await roleManager.FindByIdAsync(id);
            if (employee is null)
                return NotFound();
            var roleVM = mapper.Map<IdentityRole, RoleViewModel>(employee);
            return View(roleVM);
        }
        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] string id, RoleViewModel roleVM)
        {
            if (id != roleVM.Id)
                return BadRequest();
            try
            {
                //var mappedEmp = mapper.Map<EmployeeViewModel, Employee>(userVM);
                var user = await roleManager.FindByIdAsync(id);
                await roleManager.DeleteAsync(user);

                //unitOfWork.EmployeeRepostrios.Delete(mappedEmp);
                //int res = await unitOfWork.Complete();
                //if (res > 0)
                //    TempData["Message"] = "The Employee Is Deleted";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(roleVM);
            }
        }

    }
}
