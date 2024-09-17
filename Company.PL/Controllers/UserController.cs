using AutoMapper;
using Company.DAL.Models;
using Company.PL.Helper;
using Company.PL.ViewModels;
using Company.PLL.Interfaces;
using Company.PLL.Repositios;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
	public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IMapper mapper;

        public UserController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager ,IMapper mapper)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
            this.mapper = mapper;
        }
		public async Task<IActionResult> Index(string email )
		{
			if (string.IsNullOrEmpty(email))
			{
				var users =await userManager.Users.Select(u => new UserViewModel
				{
					Email = u.Email,
					FName = u.FName,
					LName = u.LName,
                    PhoneNumber = u.PhoneNumber,
                    UserName = u.UserName,
                    Id = u.Id,
					Roles = userManager.GetRolesAsync(u).Result
				}).ToListAsync();

				return View(users);
			}
			else
			{
				var user=await userManager.FindByEmailAsync(email);

				if (user is not null)
				{
                    var maping = new UserViewModel()
                    {
                        Email = user.Email,
                        FName = user.FName,
                        LName = user.LName,
                        PhoneNumber = user.PhoneNumber,
                        UserName = user.UserName,
                        Id = user.Id,
                        Roles = userManager.GetRolesAsync(user).Result
                    };
                    return View(new List<UserViewModel>() { maping });
                }
				else
				{
                    return View(new List<UserViewModel>());
                }
				
			}	
		}

        public async Task<IActionResult> Details(string id)
        {
            if (id is null)
                return BadRequest();
            var employee = await userManager.FindByIdAsync(id);
            if (employee is null)
                return NotFound();
            var EmMap = mapper.Map<ApplicationUser,UserViewModel>(employee);
            return View(EmMap);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
  
            if (id is null)
                return BadRequest();
            var employee = await userManager.FindByIdAsync(id);
            if (employee is null)
                return NotFound();
            var EmMap = mapper.Map<ApplicationUser, UserViewModel>(employee);

            return View(EmMap);
        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] string id, UserViewModel userVM)
        {
            if (id != userVM.Id)
                return BadRequest(id);
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await userManager.FindByIdAsync(id);
                    user.FName=userVM.FName;
                    user.LName=userVM.LName;
                    user.PhoneNumber=userVM.PhoneNumber;
                    //user.Email=userVM.Email;
                    //user.SecurityStamp=Guid.NewGuid().ToString();
                    var res = await userManager.UpdateAsync(user);
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
            return View(userVM);

        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (id is null)
                return BadRequest();
            var employee = await userManager.FindByIdAsync(id);
            if (employee is null)
                return NotFound();
            var employeeVM = mapper.Map<ApplicationUser, UserViewModel>(employee);
            return View(employeeVM);
        }
        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] string id, UserViewModel userVM)
        {
            if (id != userVM.Id)
                return BadRequest();
            try
            {
                //var mappedEmp = mapper.Map<EmployeeViewModel, Employee>(userVM);
                var user= await userManager.FindByIdAsync(id);
                await userManager.DeleteAsync(user);
                
                //unitOfWork.EmployeeRepostrios.Delete(mappedEmp);
                //int res = await unitOfWork.Complete();
                //if (res > 0)
                //    TempData["Message"] = "The Employee Is Deleted";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(userVM);
            }
        }
    }
}
