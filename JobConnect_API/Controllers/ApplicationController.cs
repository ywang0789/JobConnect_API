using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobConnect_API.Data;
using JobConnect_API.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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

        /// <summary>
        /// Helper method to check if an application exists by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool ApplicationExists(int id)
        {
            return _context.Applications.Any(e => e.application_id == id);
        }

        /// <summary>
        /// helper method to get the current user's ID.
        /// </summary>
        /// <returns></returns>
        private string? GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        /// <summary>
        /// Helper method to check if the current user is a recruiter.
        /// </summary>
        /// <returns></returns>
        private bool IsRecruiter()
        {
            return User.IsInRole("recruiter");
        }

        /// <summary>
        /// Get all applications belonging to the current user if applicant,
        /// get all applications if recruiter.
        /// </summary>
        /// <returns></returns>
        // GET: api/Application
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Application>>> GetApplications()
        {
            if (IsRecruiter())
            {
                return await _context.Applications
                    .Include(a => a.Job)
                    .Include(a => a.Account)
                    .ToListAsync();
            }

            var userId = GetCurrentUserId();
            return await _context.Applications
                .Where(a => a.account_id == userId)
                .Include(a => a.Job)
                .ToListAsync();
        }

        /// <summary>
        /// Get a specific application by ID.
        /// recruiters can see get any application,
        /// applicants can only see their own applications.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Application/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Application>> GetApplication(int id)
        {
            var application = await _context.Applications
                .Include(a => a.Job)
                .Include(a => a.Account)
                .FirstOrDefaultAsync(a => a.application_id == id);

            if (application == null)
                return NotFound();

            if (!IsRecruiter() && application.account_id != GetCurrentUserId())
                return Forbid();

            return application;
        }

        /// <summary>
        /// Get all applications for a specific job by job ID.
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        // GET: api/Application/job/{jobId}
        [HttpGet("job/{jobId}")]
        [Authorize(Policy = "RecruiterOnly")]
        public async Task<ActionResult<IEnumerable<Application>>> GetApplicationsByJobId(int jobId)
        {
            var applications = await _context.Applications
                .Where(a => a.job_id == jobId)
                .ToListAsync();
            if (applications == null || applications.Count == 0)
            {
                return NotFound();
            }
            return applications;
        }

        /// <summary>
        /// Update a specific application by ID.
        /// only recruiters can update applications.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="application"></param>
        /// <returns></returns>
        // PUT: api/Application/5
        [HttpPut("{id}")]
        [Authorize(Policy = "RecruiterOnly")]
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
        /// Create a new application for a job
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        // POST: api/Application/job/5
        [HttpPost("job/{jobId}")]
        [Authorize(Policy = "ApplicantOnly")]
        public async Task<ActionResult<Application>> PostApplication(int jobId, Application application)
        {
            var userId = GetCurrentUserId();
            application.account_id = userId;
            application.job_id = jobId;
            _context.Applications.Add(application);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetApplication", new { id = application.application_id }, application);
        }

        /// <summary>
        /// delete a specific application by ID.
        /// only the applicant can delete their own application.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Application/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "ApplicantOnly")]
        public async Task<IActionResult> DeleteApplication(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var application = await _context.Applications.FindAsync(id);

            if (application == null)
            {
                return NotFound();
            }

            if (application.account_id != userId)
            {
                return Forbid();
            }

            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        
    }
}
