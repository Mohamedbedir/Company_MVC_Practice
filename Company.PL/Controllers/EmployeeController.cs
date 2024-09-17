using AutoMapper;
using Company.DAL.Models;
using Company.PL.Helper;
using Company.PL.ViewModels;
using Company.PLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        //private readonly IEmployeeRepostrios employeeRepostrios;
        //private readonly IDepatmentRepostrios depatmentRepostrios;
        //  private readonly IMapper mapper;

        public EmployeeController(IUnitOfWork unitOfWork ,IMapper mapper ) //Ask CLR to Creation object from Empoyee Reposatory
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            //this.employeeRepostrios = employeeRepostrios;
            //this.depatmentRepostrios = depatmentRepostrios;
            //this.mapper = mapper;
        }
     
        public async Task<IActionResult> Index(string Searchvalue)
        {
            //1.1 View data is key value pair Dictionary collection 
            //used to pass data from Action to it's view 
            //used to pass data from view to layout
            //it's faster from view bag
            //define type in compile time 
            // strongly typed
            //ViewData["Message"] = "Hello from view data";
            //1.2 View bag is a dynamic property
            //used to pass data from Action to it's view 
            //used to pass data from view to layout
            //define type in run time
            //Weakley typed
            //ViewBag.Message = "Hello from view Bag";
            IEnumerable<Employee> employees;
            if(string.IsNullOrEmpty(Searchvalue))
                employees =await unitOfWork.EmployeeRepostrios.GetAll();
            else
                employees=unitOfWork.EmployeeRepostrios.GetEmployeesByName(Searchvalue);

           // var employee = employeeRepostrios.GetAll();
            var reversmap=mapper.Map<IEnumerable<Employee>,IEnumerable<EmployeeViewModel>>(employees);
            return View(reversmap);
        }
        [HttpGet]
        public async Task<IActionResult> Create() 
        {
            ViewBag.department =await unitOfWork.DepartmentRepostrios.GetAll();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid) //server side validation
            {
                var mapperemp=mapper.Map<EmployeeViewModel,Employee>(employeeVM);
                mapperemp.ImageName = DocumentSettings.UploadImage(employeeVM.Image, "Images");
                 await unitOfWork.EmployeeRepostrios.Add(mapperemp);
                int res =await unitOfWork.Complete();
                if (res > 0)
                    TempData["Message"] = " Is Added";
                //Temp data is a dictionary object
                //Used to transfer data from Action to action 
                return RedirectToAction(nameof(Index));

            }
            return View(employeeVM);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
                return BadRequest();
            var employee=await unitOfWork.EmployeeRepostrios.Get(id.Value);
            if (employee is null)
                return NotFound();
            var EmMap = mapper.Map<Employee, EmployeeViewModel>(employee);
            return View(EmMap);     
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.department =await unitOfWork.DepartmentRepostrios.GetAll();
            if (id is null)
                return BadRequest();
            var employee =await unitOfWork.EmployeeRepostrios.Get(id.Value);
            if (employee is null)
                return NotFound();
            var employeeVM = mapper.Map<Employee, EmployeeViewModel>(employee);

            return View(employeeVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute]int id ,EmployeeViewModel employeeVM)
        {
            if(id!= employeeVM.Id)
                return BadRequest(id);
            if(ModelState.IsValid)
            {
                try
                {
                    var mapperemp = mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                  //  mapperemp.ImageName = DocumentSettings.UploadImage(employeeVM.Image, "Images");
                    //   var mapperemp = mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    unitOfWork.EmployeeRepostrios.Update(mapperemp);
                    int res =await unitOfWork.Complete();
                    if (res > 0)
                        TempData["Message"] = "The Employee Is Updated";
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty,ex.Message);
                }
            }
            return View(employeeVM);

        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
                return BadRequest();
            var employee = await unitOfWork.EmployeeRepostrios.Get(id.Value);
            if (employee is null)
                return NotFound();
            var employeeVM = mapper.Map<Employee, EmployeeViewModel>(employee);
            return View(employeeVM);
        }
        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute]int id,EmployeeViewModel employeeVM)
        {
            if(id!=employeeVM.Id)
                return BadRequest();
            try
            {
                 var mappedEmp=mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                if (mappedEmp.ImageName != null)
                {
                    DocumentSettings.DeleteFile(mappedEmp.ImageName, "Images");
                }
                unitOfWork.EmployeeRepostrios.Delete(mappedEmp);
                int res =await unitOfWork.Complete();
                if (res > 0)
                    TempData["Message"] = "The Employee Is Deleted";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(employeeVM);
            }
        }
        
    }
}
