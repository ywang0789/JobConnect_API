using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobConnect_API.Data;
using JobConnect_API.Models;

namespace JobConnect_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ApplicationController(AppDbContext context)
        {
            _context = context;
        }

        private bool ApplicationExists(int id)
        {
            return _context.Applications.Any(e => e.application_id == id);
        }

        /// <summary>
        /// Get all applications.
        /// </summary>
        /// <returns></returns>
        // GET: api/Application
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Application>>> GetApplications()
        {
            return await _context.Applications.ToListAsync();
        }

        /// <summary>
        /// Get a specific application by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Application/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Application>> GetApplication(int id)
        {
            var application = await _context.Applications.FindAsync(id);

            if (application == null)
            {
                return NotFound();
            }

            return application;
        }

        /// <summary>
        /// Update a specific application by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="application"></param>
        /// <returns></returns>
        // PUT: api/Application/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplication(int id, Application application)
        {
            if (id != application.application_id)
            {
                return BadRequest();
            }

            _context.Entry(application).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Create a new application.
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        // POST: api/Application
        [HttpPost]
        public async Task<ActionResult<Application>> PostApplication(Application application)
        {
            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetApplication", new { id = application.application_id }, application);
        }

        /// <summary>
        /// delete a specific application by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Application/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplication(int id)
        {
            var application = await _context.Applications.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        
    }
}
