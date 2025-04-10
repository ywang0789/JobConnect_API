using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobConnect_API.Data;
using JobConnect_API.Models;
using Microsoft.AspNetCore.Authorization;

namespace JobConnect_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly AppDbContext _context;

        public JobController(AppDbContext context)
        {
            _context = context;
        }

        private bool JobExists(int id)
        {
            return _context.Jobs.Any(e => e.job_id == id);
        }

        /// <summary>
        /// Get all jobs.
        /// </summary>
        /// <returns></returns>
        // GET: api/Job
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Job>>> GetJobs()
        {
            return await _context.Jobs.ToListAsync();
        }

        /// <summary>
        /// Get a job by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Job/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Job>> GetJob(int id)
        {
            var job = await _context.Jobs.FindAsync(id);

            if (job == null)
            {
                return NotFound();
            }

            return job;
        }

        /// <summary>
        /// Update a job by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="job"></param>
        /// <returns></returns>
        // PUT: api/Job/5
        [HttpPut("{id}")]
        [Authorize(Policy = "RecruiterOnly")]
        public async Task<IActionResult> PutJob(int id, Job job)
        {
            if (id != job.job_id)
            {
                return BadRequest();
            }

            _context.Entry(job).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobExists(id))
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
        /// Create a new job.
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        // POST: api/Job
        [HttpPost]
        [Authorize(Policy = "RecruiterOnly")]
        public async Task<ActionResult<Job>> PostJob(Job job)
        {
            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJob", new { id = job.job_id }, job);
        }

        /// <summary>
        /// Delete a job by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Job/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "RecruiterOnly")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
