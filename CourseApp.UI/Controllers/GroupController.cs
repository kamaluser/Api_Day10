using CourseApp.UI.Exceptions;
using CourseApp.UI.Filters;
using CourseApp.UI.Models;
using CourseApp.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CourseApp.UI.Controllers
{
    [ServiceFilter(typeof(AuthFilter))]
    public class GroupController : Controller
    {
        private readonly ICrudService _crudService;

        public GroupController(ICrudService crudService)
        {
            _crudService = crudService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            try
            {
                var groups = await _crudService.GetAllPaginated<GroupListItemDetailedGetResponse>("groups", page);
                return View(groups);
            }
            catch (HttpException e)
            {
                if (e.Status == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("login", "auth");
                }
                else
                {
                    throw;

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GroupCreateRequest dto)
        {
            try
            {
                await _crudService.Create<GroupCreateRequest, GroupListItemDetailedGetResponse>("groups", dto);
                return RedirectToAction("Index");
            }
            catch (HttpException e)
            {
                if (e.Status == System.Net.HttpStatusCode.BadRequest)
                    ModelState.AddModelError("", "Validation error occurred.");
                return View(dto);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var group = await _crudService.Get<GroupCreateRequest>($"groups/{id}");
                return View(group);
            }
            catch (HttpException e)
            {
                if (e.Status == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("login", "auth");
                }
                else if (e.Status == System.Net.HttpStatusCode.NotFound)
                {
                    return RedirectToAction("error", "home", new { message = "Group not found" });
                }
                else
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GroupCreateRequest editRequest, int id)
        {
            try
            {
                await _crudService.Edit<GroupCreateRequest, GroupListItemDetailedGetResponse>($"groups/{id}", editRequest);
                return RedirectToAction("Index");
            }
            catch (HttpException e)
            {
                if (e.Status == System.Net.HttpStatusCode.BadRequest)
                {
                    ModelState.AddModelError("", "Validation error occurred.");
                }
                else
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return View(editRequest);
        }


        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _crudService.Delete($"groups/{id}");
                return Ok();
            }
            catch (HttpException e)
            {
                if (e.Status == System.Net.HttpStatusCode.Unauthorized)
                {
                    return Unauthorized();
                }
                else if (e.Status == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
