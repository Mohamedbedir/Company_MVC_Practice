using Company.DAL.Models;
using Company.PLL.Interfaces;
using Company.PLL.Repositios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    [Authorize(Roles ="Admin")]
    public class DepartmentController : Controller
    {
        //private IDepatmentRepostrios depatmentRepostrios;
        private readonly IUnitOfWork unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork) //Ask CLR creation A object From Department Repostrios
        {
            //this.depatmentRepostrios = depatmentRepostrios;
            this.unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var departments =await unitOfWork.DepartmentRepostrios.GetAll();
            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid)
            {
                await unitOfWork.DepartmentRepostrios.Add(department);
                int res = await unitOfWork.Complete();
                if (res > 0)
                    TempData["Message"] = "the Department Was Added";
                //Temp data is a dictionary object
                //Used to transfer data from Action to action 
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
                return BadRequest();
            var department =await unitOfWork.DepartmentRepostrios.Get(id.Value);
            if (department is null)
                return NotFound();
            return View(department);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
                return BadRequest();
            var department =await unitOfWork.DepartmentRepostrios.Get(id.Value);
            if (department is null)
                return NotFound();
            return View(department);

        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id, Department department)
        {
            if (id != department.Id)
                return BadRequest(id);
            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.DepartmentRepostrios.Update(department);
                    int res =await unitOfWork.Complete();
                    if (res > 0)
                        TempData["Message"] = "the Department Was Updated";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(department);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
                return BadRequest();
            var department =await unitOfWork.DepartmentRepostrios.Get(id.Value);
            if (department is null)
                return NotFound();
            return View(department);

        }
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeleteConfirmed([FromRoute]int id)
        //{
        //    var department = depatmentRepostrios.Get(id);
        //    if (department is null)
        //        return NotFound();

        //    depatmentRepostrios.Delete(department); // Perform the delete operation
        //    return RedirectToAction(nameof(Index));
        //}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id ,Department department)
        {
            if (id != department.Id)
                return BadRequest();
            try
            {
                unitOfWork.DepartmentRepostrios.Delete(department); // Perform the delete operation
                int res =await unitOfWork.Complete();
                if (res > 0)
                    TempData["Message"] = "the Department Was Deleted";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(department);
            }  
        }
    }
}
